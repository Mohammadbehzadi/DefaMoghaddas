using Data.Domain.Entities;
using Data.Domain.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace DAL.Classes
{
    public class ApplicationUserSignIn : SignInManager<ApplicationUser, string>
    {
        public ApplicationUserSignIn(ApplicationUserManagment applicationUserManager, IAuthenticationManager authenticationManager)
            : base(applicationUserManager, authenticationManager)
        {
            ApplicationUserManager = applicationUserManager;
        }

        public new ApplicationUserManagment ApplicationUserManager { get; set; }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        //{
        //    return user.GenerateUserIdentityAsync((ApplicationUserManagment)applicationUserManager);
        //}

        public static ApplicationUserSignIn Create(IdentityFactoryOptions<ApplicationUserSignIn> options, IOwinContext context)
        {
            return new ApplicationUserSignIn(context.GetUserManager<ApplicationUserManagment>(), context.Authentication);
        }
    }
}
