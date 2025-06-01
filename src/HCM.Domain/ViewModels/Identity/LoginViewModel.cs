using HCM.Domain.Models.Identity;

namespace HCM.Domain.ViewModels.Identity
{
    public class LoginViewModel
    {
        public TokenModel Token { get; set; }

        public UserViewModel User { get; set; }
    }
}
