﻿using HCM.Domain.Models.Identity;

namespace HCM.Domain.ViewModels.Identity
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public AddressModel Address { get; set; }
    }
}
