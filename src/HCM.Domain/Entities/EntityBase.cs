using HCM.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Entities
{
    public class EntityBase<TEntity> : IEntityBase
        where TEntity : class
    {
        [Key]
        public Guid Id { get; set; }
    }
}
