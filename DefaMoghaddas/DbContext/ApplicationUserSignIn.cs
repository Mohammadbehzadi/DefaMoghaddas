
using DefaMoghaddas.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace DefaMoghaddas.DbContext
{
    public class ApplicationUserSignIn : SignInManager<ApplicationUser, string>
    {
        public ApplicationUserSignIn(UserContext userContextt, IAuthenticationManager authenticationManager)
            : base(userContextt, authenticationManager)
        {
            UserContextt = userContextt;
        }

        public new UserContext UserContextt { get; set; }


        public static ApplicationUserSignIn Create(IdentityFactoryOptions<ApplicationUserSignIn> options, IOwinContext context)
        {
            return new ApplicationUserSignIn(context.GetUserManager<UserContext>(), context.Authentication);
        }
    }
}