using HCM.Domain.Enums;

namespace HCM.Domain.ViewModels.Manager
{
    public class ManagerViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public ManagerType ManagerType { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
