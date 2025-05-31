using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Domain.Entities.Identity
{
    public class UserRoleEntity
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntity Role { get; set; }
    }
}
