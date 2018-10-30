using System;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using ServiceStack.Text;
using Topshelf;
using Topshelf.SimpleInjector;


namespace KitBuilder.ESB.Listeners.Item.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = SimpleInjectorInitializer.InitializeContainer();

            container.GetInstance<EsbConnectionSettings>().PrintDump();

            
            
            //Register services
            HostFactory.Run(r =>
            {

                //r.UseSimpleInjector(container);

                r.Service<IListenerApplication>(s =>
                {
                    //s.ConstructUsingSimpleInjector();
                    s.ConstructUsing(c => container.GetInstance<IListenerApplication>());
                    s.WhenStarted(c => c.Run());
                    s.WhenStopped(c => c.Close());
                });

                r.SetDescription("Processes HospitalityItems from Global Product Messages to be used with KitBuilder");
                r.SetDisplayName("KitBuilder Item Listener");
                r.SetServiceName("KitBuilder.Item.Listener");
            });
        }
    }
}