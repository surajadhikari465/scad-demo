using System;
using MassTransit;
using MassTransit.NLogIntegration;
using OOSCommon;
using StructureMap;
using Topshelf;

namespace OffshelfUploadBoundedContext
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var container = Bootstrap();
            HostFactory.Run(c =>
            {
                c.SetServiceName("OffshelfUploadBoundedContext");
                c.SetDisplayName("Offshelf Upload");
                c.SetDescription("Masstransit sample for event subscription");
                c.DependsOnMsmq();
                c.RunAsLocalService();
                c.Service<OffshelfUploadService>(s =>
                {
                    s.ConstructUsing(builder => container.GetInstance<OffshelfUploadService>());
                    s.WhenStarted(o => o.Start());
                    s.WhenStopped(o =>
                    {
                        o.Stop();
                        container.Dispose();
                    });
                });
            });

        }

        private static IContainer Bootstrap()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AddAllTypesOf(typeof(IConsumer));
                    scan.AddAllTypesOf(typeof (IProductStatusRepository));
                });
                cfg.For<IConfigure>().Use<BasicConfigurator>();
            });
            
            container.Configure(cfg =>
            {
                cfg.For<IServiceBus>().Use(context => ServiceBusFactory.New(sbc =>
                {
                    sbc.ReceiveFrom(container.GetInstance<IConfigure>().GetOffshelfUploadBoundedContextEndpoint());
                    sbc.UseNLog();
                    sbc.UseMsmq();
                    sbc.VerifyMsmqConfiguration();
                    sbc.UseMulticastSubscriptionClient();
                    sbc.UseControlBus();
                    sbc.SetCreateMissingQueues(true);
                    sbc.Subscribe(subs => { subs.LoadFrom(container); });
                }));
            });
            return container;
        }

    }
}
