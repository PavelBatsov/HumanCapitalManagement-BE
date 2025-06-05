using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [MaxLength(UserConstraints.UserNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(UserConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(UserConstraints.PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(UserConstraints.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserConstraints.LastNameMaxLength)]
        public string LastName { get; set; }

        public AddressModel Address { get; set; }
    }
}
