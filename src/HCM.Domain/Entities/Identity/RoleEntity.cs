namespace HCM.Domain.Entities.Identity
{
    public class RoleEntity : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<UserRoleEntity> Users { get; set; }
    }
}
