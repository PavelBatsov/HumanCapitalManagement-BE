using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
