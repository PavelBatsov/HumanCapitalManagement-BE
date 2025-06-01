using HCM.Domain.Configurations;
using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HCM.Application.Services
{
    public class TokenHandlerService : ITokenHandlerService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserTokenRepository userTokenRepository;
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public TokenHandlerService(
            IUserRepository userRepository,
            IOptions<AuthenticationConfiguration> authenticationConfiguration,
            IUserTokenRepository userTokenRepository)
        {
            this.userRepository = userRepository;
            this.authenticationConfiguration = authenticationConfiguration.Value;
            this.userTokenRepository = userTokenRepository;
        }

        public async Task<TokenModel> GenerateTokenAsync(UserEntity user)
        {
            var token = new TokenModel
            {
                AccessToken = await CreateAccessToken(user),
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiredAt = DateTime.UtcNow.AddSeconds(authenticationConfiguration.RefreshTokenExpiration)
            };

            // Usually we store the token in cache like Redis
            await StoreTokenInDatabaseAsync(user.Id, token.RefreshToken, token.RefreshTokenExpiredAt);

            return token;
        }

        public async Task RevokeRefreshTokenAsync(RefreshTokenModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                throw new Exception(Strings.EmptyRefreshToken);
            }

            var userToken = await userTokenRepository.GetRefreshTokenAsync(model.RefreshToken);

            if (string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                throw new Exception(Strings.EmptyRefreshToken);
            }

            userTokenRepository.Delete(userToken);
            await userTokenRepository.SaveAsync();
        }

        public async Task<TokenModel> RefreshTokenAsync(RefreshTokenModel model)
        {
            await RevokeRefreshTokenAsync(model);

            var user = await userRepository.GetAsync(model.UserId)
                ?? throw new Exception(Strings.UserNotFound);

            return await GenerateTokenAsync(user);
        }

        private async Task<string> CreateAccessToken(UserEntity user)
        {
            var key = Encoding.UTF8.GetBytes(authenticationConfiguration.Secret);
            var singingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var accessTokenExpiredAt = DateTime.UtcNow.AddSeconds(authenticationConfiguration.AccessTokenExpiration);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = await GetClaimsAsync(user),
                Audience = authenticationConfiguration.Audience,
                Issuer = authenticationConfiguration.Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = accessTokenExpiredAt,
                SigningCredentials = singingCredentials
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(securityToken);

            return encryptedToken;
        }

        private static string CreateRefreshToken()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var secret = Guid.NewGuid().ToString();
            var byteArr = Encoding.ASCII.GetBytes(salt + secret);

            return Convert.ToBase64String(byteArr);
        }

        private async Task<ClaimsIdentity> GetClaimsAsync(UserEntity user)
        {
            var userRole = await this.userRepository.GetUserRoleAsync(user.Id);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole.Name)
            });

            return claimsIdentity;
        }

        private async Task StoreTokenInDatabaseAsync(Guid userId, string refreshToken, DateTime expiredAt)
        {
            var userToken = new UserTokenEntity
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = expiredAt
            };

            await userTokenRepository.AddAsync(userToken);
            await userTokenRepository.SaveAsync();
        }
    }
}
