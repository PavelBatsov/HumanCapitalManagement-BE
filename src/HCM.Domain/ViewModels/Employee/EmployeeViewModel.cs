﻿using HCM.Domain.Enums;

namespace HCM.Domain.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

        public Guid ManagerId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public string EmployeeName => $"{FirstName} {LastName}";
    }
}
