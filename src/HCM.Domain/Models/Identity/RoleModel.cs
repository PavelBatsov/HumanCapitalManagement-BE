﻿namespace HCM.Domain.Models.Identity
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get;set; }

        public string Name { get; set; }
    }
}
