using System.Data.Entity;
using Data.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.DataContexts
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext()
            : base("DefaConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Database.CommandTimeout = 3600;
        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<PermissionsRoles> PermissionsRoles { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<UserEmail> UserEmails { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey(q => new { q.LoginProvider, q.ProviderKey, q.UserId });
            modelBuilder.Entity<IdentityUserRole>().HasKey(q => new { q.RoleId, q.UserId });

            modelBuilder.Entity<Person>()
                .HasMany(q => q.SubPerson)
                .WithOptional(q => q.Parent)
                .Map(q => q.MapKey("ParentId"));
        }
    }
}
