using Microsoft.AspNetCore.Hosting;

namespace KitBuilder.Web.Helpers
{
    public static class EnvironmentExtensions
    {
        public static bool IsLocal(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Local");
        }
    }
}