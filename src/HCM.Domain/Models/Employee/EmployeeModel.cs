using HCM.Domain.Constraints.Employee;
using HCM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Employee
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(EmployeeConstraints.EmailMaxLength)]
        public string Email { get; set; }

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

        [Required]
        public Guid ManagerId { get; set; }
    }
}
