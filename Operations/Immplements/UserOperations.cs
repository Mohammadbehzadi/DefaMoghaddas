using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.Immplements
{
    public class UserOperations : IUserOperations<ApplicationUser>
    {
        public IQueryable<ApplicationUser> GetUsers(Expression<Func<ApplicationUser, bool>> predicate)
        {
            var context = new AppIdentityDbContext();
            var identityRoles = context.Users.Where(predicate);

            return identityRoles;
        }
        public List<string> GetUserRoleIds(string userName)
        {
            var context = new AppIdentityDbContext();
            var applicationUser = context.Users.SingleOrDefault(q => q.UserName == userName);
            if (applicationUser != null)
            {
                var userId = applicationUser.Id;
                var roleIds = context.IdentityUserRoles.Where(q => q.UserId == userId).Select(q => q.RoleId).ToList();
                return roleIds;
            }

            return null;
        }
        public IEnumerable<string> GetPermissionsByRole(List<string> roleIds)
        {
            var context = new AppIdentityDbContext();
            var permissions = context.PermissionsRoles.Where(q => roleIds.Contains(q.RoleId))
                .Select(q => q.Permission);

            return permissions.Any() ? permissions.Select(q => q.Name).ToList() : null;
        }
        public IEnumerable<Permissions> GetPermissions()
        {
            var context = new AppIdentityDbContext();
            var permissions = context.Permissions.ToList();

            return permissions;
        }
        public IEnumerable<IdentityRole> GetRoles()
        {
            var context = new AppIdentityDbContext();
            var roles = context.Roles.ToList();
            var model = roles.Select(item => new IdentityRole
            {
                Id = item.Id,
                Name = item.Name
            });
            return model;
        }
        public IQueryable<ApplicationUser> Get()
        {
            var context = new AppIdentityDbContext();
            var identityRoles = context.Users;

            return identityRoles;
        }
        public ApplicationUser Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
        public bool Delete(string userId)
        {
            var context = new AppIdentityDbContext();
            var user = context.Users.Remove(context.Users.Single(q => q.Id == userId));
            context.SaveChanges();

            return true;
        }
        public bool AddUserRole(List<string> roleIds, string userId)
        {
            try
            {
                var context = new AppIdentityDbContext();

                foreach (var roleId in roleIds)
                {
                    context.IdentityUserRoles.Add(new IdentityUserRole
                    {
                        RoleId = roleId,
                        UserId = userId
                    });
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveUserRole(List<string> roleIds, string userId)
        {
            try
            {
                var context = new AppIdentityDbContext();
                foreach (var roleId in roleIds)
                {
                    var userRole = context.IdentityUserRoles.SingleOrDefault(q => q.RoleId == roleId && q.UserId == userId);
                    context.IdentityUserRoles.Remove(userRole);
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool GetUserRole(string roleId, string userId)
        {
            try
            {
                var context = new AppIdentityDbContext();
                var existRoleUser = context.IdentityUserRoles.Where(q => q.RoleId == roleId && q.UserId == userId);
                if (!existRoleUser.Any())
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UserHavePermission(string userName, PermissionsEnum permission)
        {
            var roles = GetUserRoleIds(userName);
            var permissions = GetPermissionsByRole(roles);
            return permissions.Contains(permission.ToString());
        }
        public bool AddBranchToUser(string userId, Guid branchId)
        {
            try
            {
                var context = new AppIdentityDbContext();
                var user = context.Users.SingleOrDefault(q => q.Id == userId);
                user.Person = context.Person.SingleOrDefault(q => q.Id == branchId);
                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddEmailToUser(Email email)
        {
            using (var context = new AppIdentityDbContext())
            {
                var entity = new Email
                {
                    Body = email.Body,
                    Subject = email.Subject,
                };
                context.Emails.Add(entity);
                foreach (var userEmail in email.To)
                {
                    context.UserEmails.Add(new UserEmail
                    {
                        Email = entity,
                        ApplicationUser = context.Users.SingleOrDefault(q => q.UserName == userEmail.ApplicationUser.UserName)
                    });
                }
                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException exception)
                {
                    return false;
                }

            }
        }
    }
}