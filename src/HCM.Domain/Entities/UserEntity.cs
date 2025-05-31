using HCM.Domain.Constraints.Identity;
using HCM.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Entities
{
    public class UserEntity : EntityBase
    {
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

        public virtual ICollection<UserRoleEntity> Roles { get; set; }
    }
}
