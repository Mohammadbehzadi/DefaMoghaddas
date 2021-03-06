﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Domain.Entities
{
    public class PermissionsRoles
    {
        [Key, ForeignKey("Role"), Column(Order = 1)]
        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }

        [Key, ForeignKey("Permission"), Column(Order = 2)]
        public Guid? PermissionId { get; set; }
        public virtual Permissions Permission { get; set; }
    }
}