using System;
using Icon.Common;
using Topshelf;
using Topshelf.SimpleInjector;

namespace Services.Extract
{
    class Program
    {
        static void Main(string[] args)
        {

            var container = SimpleInjectorInitializer.Init();

            var rc = HostFactory.Run(x =>
            {
                x.UseSimpleInjector(container);
                x.Service<ExtractService>(s =>                                 
                {
                    s.ConstructUsingSimpleInjector();
                    s.WhenStarted(tc => tc.Start());                      
                    s.WhenStopped(tc => tc.Stop());                       
                });
                x.RunAsLocalSystem();                                     

                x.SetDescription(AppSettingsAccessor.GetStringSetting("TopShelfDescription", "SCAD Extract Service"));                 
                x.SetDisplayName(AppSettingsAccessor.GetStringSetting("TopShelfDisplayName", "ExtractService"));                                
                x.SetServiceName(AppSettingsAccessor.GetStringSetting("TopShelfServiceName", "ExtractService"));                                
            });                                                           

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode()); 
            Environment.ExitCode = exitCode;

        }

    }
}
