using HCM.Domain.Constraints.Manager;
using HCM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Manager
{
    public class ManagerModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(ManagerConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(ManagerConstraints.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(ManagerConstraints.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(ManagerConstraints.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(ManagerConstraints.AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public ManagerType ManagerType { get; set; }
    }
}
