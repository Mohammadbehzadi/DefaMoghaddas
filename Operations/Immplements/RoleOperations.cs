using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.Immplements
{
    public class RoleOperations : IRoleOperations<IdentityRole>
    {
        public IQueryable<IdentityRole> Get()
        {
            var context = new AppIdentityDbContext();
            var identityRoles = context.Roles;

            return identityRoles;
        }
        public IdentityRole Add(IdentityRole entity)
        {
            var context = new AppIdentityDbContext();
            var roles = context.Roles.Add(entity);
            context.SaveChanges();
            return roles;
        }
        public IdentityRole Update(IdentityRole entity)
        {
            var context = new AppIdentityDbContext();
            context.Roles.AddOrUpdate(q => q.Id, entity);
            context.SaveChanges();
            return entity;
        }
        public bool Delete(string roleId)
        {
            var context = new AppIdentityDbContext();
            var role = context.Roles.Remove(context.Roles.Single(q => q.Id == roleId));
            context.SaveChanges();

            return true;
        }
    }
}