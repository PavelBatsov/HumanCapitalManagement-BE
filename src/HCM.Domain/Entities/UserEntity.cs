using HCM.Domain.Constraints.Identity;
using HCM.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Entities
{
    public class UserEntity : TrackableEntity<UserEntity>
    {
        [Required]
        [MaxLength(UserConstraints.UserNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
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

        public virtual ICollection<UserRoleEntity> Roles { get; set; }
    }
}
