using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class UserModel
    {
        public Guid RoleId { get; set; }

        [MaxLength(UserConstraints.UserNameMaxLength)]
        public string UserName { get; set; }

        [MaxLength(UserConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [MaxLength(UserConstraints.PasswordMaxLength)]
        public string Password { get; set; }

        [MaxLength(UserConstraints.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(UserConstraints.LastNameMaxLength)]
        public string LastName { get; set; }
    }
}
