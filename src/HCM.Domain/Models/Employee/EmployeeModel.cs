using HCM.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Employee
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; }
    }
}
