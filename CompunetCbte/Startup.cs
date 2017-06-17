using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CompunetCbte.Startup))]
namespace CompunetCbte
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
