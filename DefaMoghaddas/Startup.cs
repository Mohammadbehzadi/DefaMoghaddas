using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DefaMoghaddas.Startup))]
namespace DefaMoghaddas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
