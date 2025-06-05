using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get;set; }

        [Required]
        public string Name { get; set; }
    }
}
