using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Entities.Identity
{
    public class RoleEntity : EntityBase<RoleEntity>
    {
        [Required]
        [MaxLength(RoleConstraints.NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<UserRoleEntity> Users { get; set; }
    }
}
