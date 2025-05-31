using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
