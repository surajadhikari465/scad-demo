namespace Vim.Service
{
    using Icon.Common;
    using Icon.Common.Email;
    using Icon.Logging;
    using SimpleInjector;
    using System;
    using System.Linq;
    using System.Reflection;

    public class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            
            // Change the 'BrandClient' AppConfig.AppSetting to point to a different project to load a different implementation
            // of IVimBrandClient.
            string brandClientAssemblyName = AppSettingsAccessor.GetStringSetting(AppSettings.BrandClient, true);
            Assembly clientAssembly = Assembly.Load(brandClientAssemblyName);

            try
            {
                // Get the project name that contains exactly 1 implementation of IVimBrandClient
                Type vimBrandClientType = clientAssembly.GetExportedTypes().Single(
                    t => t.GetInterface(typeof(IVimBrandClient).Name) != null);

                container.Register(typeof(IVimBrandClient), vimBrandClientType);
                container.Register<ILogger<VimService>, NLogLogger<VimService>>();
                container.RegisterSingle(typeof(ILogger), () => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
                container.RegisterSingle<IEmailClient>(() => EmailClient.CreateFromConfig());
                container.Verify();

                return container;
            }
            catch (InvalidOperationException ioe)
            {
                throw new Exception(
                    string.Format(
                        "The project {0} should contain exactly one implementation of the interface IVimBrandClient",
                        brandClientAssemblyName),
                    ioe);
            }
        }
    }
}
