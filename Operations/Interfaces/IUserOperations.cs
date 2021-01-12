using Data.Domain.Entities;
using Data.Domain.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Operations.Interfaces
{
    public interface IUserOperations<T> : IBaseRepository<T> where T : class
    {
        IQueryable<ApplicationUser> GetUsers(Expression<Func<ApplicationUser, bool>> predicate);
        List<string> GetUserRoleIds(string userName);
        IEnumerable<string> GetPermissionsByRole(List<string> roleIds);
        IEnumerable<Permissions> GetPermissions();
        IEnumerable<IdentityRole> GetRoles();
        bool AddUserRole(List<string> roleIds, string userId);
        bool RemoveUserRole(List<string> roleIds, string userId);
        bool GetUserRole(string roleIds, string userId);
        bool UserHavePermission(string userName, PermissionsEnum permission);
        bool AddBranchToUser(string userId, Guid branchId);
        bool AddEmailToUser(Email email);
    }
}
