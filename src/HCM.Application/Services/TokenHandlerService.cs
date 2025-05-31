using HCM.Domain.Configurations;
using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Models.Identity;
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
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public TokenHandlerService(
            IUserRepository userRepository,
            IOptions<AuthenticationConfiguration> authenticationConfiguration)
        {
            this.userRepository = userRepository;
            this.authenticationConfiguration = authenticationConfiguration.Value;
        }

        public async Task<TokenModel> GenerateTokenAsync(UserEntity applicationUser)
        {
            var token = new TokenModel
            {
                AccessToken = await CreateAccessToken(applicationUser),
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiredAt = DateTime.UtcNow.AddSeconds(authenticationConfiguration.RefreshTokenExpiration)
            };

            // TODO Store in Db or Redis cache

            return token;
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
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            var generatedRandomNumber = Convert.ToBase64String(randomNumber);

            var salt = generatedRandomNumber;
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
    }
}
