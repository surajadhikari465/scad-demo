using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.Cache;
using Infor.Services.NewItem.Commands;
using Infor.Services.NewItem.MessageBuilders;
using Infor.Services.NewItem.Processor;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Services;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;
using Topshelf;

namespace Infor.Services.NewItem
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = SimpleInjectorInitializer.InitializeContainer().GetInstance<IInforNewItemApplication>();
            HostFactory.Run(r =>
            {
                r.Service<IInforNewItemApplication>(s =>
                {
                    s.ConstructUsing(c => SimpleInjectorInitializer.InitializeContainer().GetInstance<IInforNewItemApplication>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }
    }
}
