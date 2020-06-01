using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using OOS.Model;
using OOS.Model.Repository;
using OOSCommon;
using OutOfStock.ScanManagement;
using ProductDataBoundedContext;
using SharedKernel;
using StructureMap;
using Topshelf;


namespace OutOfStock.ScanProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                ObjectFactory.Configure(cfg =>
                {
                    cfg.For<IRawScanRepository>().Use<RawScanRepository>();
                    cfg.For<IConfigure>().Use<BasicConfigurator>();
                    cfg.For<IConfigurator>().Use<ScanProcessorConfigurator>();
                    cfg.For<IScanOutOfStockItemService>().Use<ScanOutOfStockItemService>();
                    cfg.For<ILogService>().Singleton().Use<LogService>();
                    cfg.For<IConfigurator>().Use<ScanProcessorConfigurator>();
                    cfg.For<IEventCreatorFactory>().Use<EventCreatorFactory>();
                    cfg.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                    cfg.For<IRegionRepository>().Use<RegionRepository>();
                    cfg.For<IProductRepository>().Use<ProductRepository>();
                    cfg.For<ProductDataService>().Use<ProductDataService>();
                    cfg.For<IStoreRepository>().Use<StoreRepository>();
                    cfg.For<IRetailItemRepository>().Use<RetailItemRepository>();
                    cfg.For<IUserProfile>().Use<ScanProcessorUserProfile>();
                    cfg.For<IProductFactory>().Use<ProductFactory>();
                    cfg.For<IJob>().Use<ScanProcessorJob>();
                });

                

                x.Service<ScanProcessorJob>(s =>
                {
                    
                    s.ConstructUsing(name => ObjectFactory.GetInstance<ScanProcessorJob>());             
                    s.WhenStarted(tc => tc.Start());                       
                    s.WhenStopped(tc => tc.Stop());                        
                });
                x.RunAsLocalSystem();                                      

                x.SetDescription("OutOfStock Raw Scan Processor");                  
                x.SetDisplayName("[gnpt] OOSRawScanProcessor");                                 
                x.SetServiceName("OOSRawScanProcessor");                                 
            });                                                            

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  
            Environment.ExitCode = exitCode;
        }
    }
}
