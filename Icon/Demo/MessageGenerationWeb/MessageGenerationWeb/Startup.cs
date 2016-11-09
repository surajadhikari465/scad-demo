using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessageGenerationWeb.Startup))]
namespace MessageGenerationWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
