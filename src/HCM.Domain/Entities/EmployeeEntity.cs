using HCM.Domain.Constraints.Employee;
using HCM.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Domain.Entities
{
    public class EmployeeEntity
    {
        [Required]
        [EmailAddress]
        [MaxLength(EmployeeConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(EmployeeConstraints.PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(EmployeeConstraints.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(EmployeeConstraints.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EmployeeConstraints.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(EmployeeConstraints.AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; }

        public Guid ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public ManagerEntity Manager { get; set; }
    }
}
