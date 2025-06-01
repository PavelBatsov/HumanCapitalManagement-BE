namespace HCM.Domain.Entities.Identity
{
    public class RoleEntity : EntityBase<RoleEntity>
    {
        public string Name { get; set; }

        public virtual ICollection<UserRoleEntity> Users { get; set; }
    }
}
