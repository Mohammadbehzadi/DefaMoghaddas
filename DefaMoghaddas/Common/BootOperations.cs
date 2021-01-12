using DAL.DataContexts;
using Data.Domain.Entities;
using Data.Domain.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Web.Mvc.Extensions;

namespace DefaMoghaddas.Common
{
    public static class BootOperations
    {
        private const string Username = "DeveloperUser";
        private const string Email = "DeveloperUser@hytech.com";
        private const string Password = "DeveloperPassword";
        private const string Rolename = "DeveloperRole";

        public static void Startup()
        {
            DeleteBaseInfo();

            AddDeveloperUser();
            AddDeveloperRole();
            AssignUserToRole();
            AddPermissions();
            AssignDeveloperToPermission();
            DeleteExtraPermission();
        }

        private static void AddDeveloperUser()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AppIdentityDbContext()));
            manager.Create(new ApplicationUser
            {
                UserName = Username,
                Email = Email,
                EmailConfirmed = true
            }, Password);
        }

        private static void AddDeveloperRole()
        {
            var context = new AppIdentityDbContext();
            context.Roles.Add(new IdentityRole
            {
                Name = Rolename
            });

            context.SaveChanges();
        }

        private static void AssignUserToRole()
        {
            var context = new AppIdentityDbContext();

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AppIdentityDbContext()));
            var applicationUser = context.Users.SingleOrDefault(q => q.UserName == Username);
            if (applicationUser != null)
            {
                var developerId = applicationUser.Id;
                manager.AddToRoleAsync(developerId, Rolename);
            }
        }

        private static void AddPermissions()
        {
            var context = new AppIdentityDbContext();
            var permissionList = Enum.GetValues(typeof(PermissionsEnum));
            if (context.Permissions.Count() == permissionList.AsQueryable().Count()) return;

            if (context.Permissions.Count() < permissionList.AsQueryable().Count())
            {
                var permissionContextName = context.Permissions.Select(q => q.Name);
                foreach (var permission in permissionList)
                {
                    var description = Extensions.ToDescriptionEnum(((PermissionsEnum)permission));
                    if (!permissionContextName.Contains(description))
                    {
                        context.Permissions.Add(new Permissions
                        {
                            Name = permission.ToString(),
                            Description = description,
                        });
                    }
                }
                context.SaveChanges();
            }
        }

        private static void AssignDeveloperToPermission()
        {
            var context = new AppIdentityDbContext();
            var permissionList = Enum.GetValues(typeof(PermissionsEnum));

            foreach (var permission in permissionList)
            {
                context.PermissionsRoles.Add(new PermissionsRoles
                {
                    Permission = context.Permissions.SingleOrDefault(q => q.Name.Equals(permission.ToString())),
                    Role = context.Roles.SingleOrDefault(q => q.Name == Rolename)
                });
            }
            context.SaveChanges();
        }

        private static void DeleteBaseInfo()
        {
            var context = new AppIdentityDbContext();
            var identityRole = context.Roles.SingleOrDefault(q => q.Name == Rolename);

            if (!context.Roles.Any() || !context.Users.Any() || !context.Permissions.Any()) return;

            var users = context.Users.Where(q => q.UserName == Username).ToList();
            if (identityRole != null)
            {
                var roleId = identityRole.Id;
                var applicationUser = users.FirstOrDefault();
                if (applicationUser != null)
                {
                    var userPermission = context.PermissionsRoles.Where(q => q.RoleId == roleId).ToList();
                    if (userPermission.Any())
                    {
                        userPermission.ForEach(q => context.PermissionsRoles.Remove(q));
                        context.SaveChanges();
                    }
                }
            }

            users.ForEach(q => context.Users.Remove(q));

            var roles = context.Roles.Where(q => q.Name == Rolename);
            roles.ForEach(q => context.Roles.Remove(q));

            if (!context.PermissionsRoles.Any())
            {
                var permissionses = context.Permissions;
                permissionses.ForEach(q => context.Permissions.Remove(q));
            }

            context.SaveChanges();
        }

        private static void DeleteExtraPermission()
        {
            var context = new AppIdentityDbContext();
            var identityRole = context.Roles.SingleOrDefault(q => q.Name == Rolename);
            if (identityRole != null)
            {
                var roleId = identityRole.Id;

                var permissionList = Enum.GetValues(typeof(PermissionsEnum));
                if (context.Permissions.Count() > permissionList.AsQueryable().Count())
                {
                    var permissionContextName = context.Permissions.Select(q => q.Name);
                    foreach (var permission in permissionList)
                    {
                        var description = Extensions.ToDescriptionEnum(((PermissionsEnum)permission));
                        if (!permissionContextName.Contains(description))
                        {
                            context.Permissions.Add(new Permissions
                            {
                                Name = permission.ToString(),
                                Description = description,
                                PermissionsRoles = new Collection<PermissionsRoles>
                                {
                                    new PermissionsRoles
                                    {
                                        RoleId = roleId
                                    }
                                }
                            });
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}