using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;
using Services.NewItem.Cache;
using Services.NewItem.Commands;
using Services.NewItem.MessageBuilders;
using Services.NewItem.Processor;
using Services.NewItem.Queries;
using Services.NewItem.Services;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;
using Topshelf;

namespace Services.NewItem
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<INewItemApplication>(s =>
                {
                    s.ConstructUsing(c => SimpleInjectorInitializer.InitializeContainer().GetInstance<INewItemApplication>());
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
