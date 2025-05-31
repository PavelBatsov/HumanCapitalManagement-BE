using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;

namespace HCM.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandlerService tokenHandlerService;

        public AccountService(
            IUserRepository userRepository,
            ITokenHandlerService tokenHandlerService)
        {
            this.userRepository = userRepository;
            this.tokenHandlerService = tokenHandlerService;
        }

        public async Task Login()
        {
            // Check User
            // Check Pass

            var token = tokenHandlerService.GenerateTokenAsync(new UserEntity());

            throw new NotImplementedException();
        }

        public Task Register()
        {
            throw new NotImplementedException();
        }
    }
}
