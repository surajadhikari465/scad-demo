using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Supporty.Startup))]
namespace Supporty
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
