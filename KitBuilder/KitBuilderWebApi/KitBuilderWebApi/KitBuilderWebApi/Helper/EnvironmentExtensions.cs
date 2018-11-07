using Microsoft.AspNetCore.Hosting;

namespace KitBuilderWebApi.Helper
{

    /*
      This creates a new environment type called Local that has its own appSettings.Local.json config
      and "IIS Local" build profile (Play button)

      Intended to be used when developing on local dev machines. 
     */
    public static class EnvironmentExtensions
    {
        public static bool IsLocal(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Local");
        }
    }
}