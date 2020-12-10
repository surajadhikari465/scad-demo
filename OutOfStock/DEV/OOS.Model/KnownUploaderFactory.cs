using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MassTransit;
using OOSCommon;
using OOSCommon.DataContext;
using OOSCommon.Import;
using OOSCommon.VIM;

namespace OOS.Model
{
    //public class KnownUploaderFactory : ICreateKnownUploader
    //{
    //    private IConfigure configurator;
    //    private IOOSLog logger;
    //    private IServiceBus bus;

    //    private const bool Validate = false;

    //    public KnownUploaderFactory(ILogService logService, IConfigure configurator, IServiceBus bus)
    //    {
    //        logger = logService.GetLogger();
    //        this.configurator = configurator;
    //        this.bus = bus;
    //    }

    //    public IOOSUpdateKnown Make()
    //    {
    //        return new OOSUpdateKnown(Validate, logger, EntityFactory(), VimRepository(), configurator.GetEFConnectionString());
    //    }

    //    private IVIMRepository VimRepository()
    //    {
    //        return new VIMRepository(configurator.GetOOSConnectionString(), configurator.GetVIMServiceName(), logger);
    //    }

    //    private ICreateDisposableEntities EntityFactory()
    //    {
    //        return new EntityFactory(configurator);
    //    }
    //}
}
