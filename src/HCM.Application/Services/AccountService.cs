using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;

namespace HCM.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;

        public AccountService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Register()
        {
            throw new NotImplementedException();
        }
    }
}
