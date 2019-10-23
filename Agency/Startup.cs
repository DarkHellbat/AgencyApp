using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Agency.Startup))]
namespace Agency
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
