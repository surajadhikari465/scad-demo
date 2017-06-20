using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener
{
    public class MammothHierarchyClassListener : ListenerApplication<MammothHierarchyClassListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<HierarchyClassModel>> messageParser;
        private IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService;
        private IHierarchyClassService<DeleteBrandRequest> deleteBrandsService;

        public MammothHierarchyClassListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger,
            IMessageParser<List<HierarchyClassModel>> messageParser,
            IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService,
            IHierarchyClassService<DeleteBrandRequest> deleteBrandsService)
            : base(listenerApplicationSettings,
                  esbConnectionSettings,
                  subscriber,
                  emailClient,
                  logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassService = hierarchyClassService;
            this.deleteBrandsService = deleteBrandsService;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            List<HierarchyClassModel> hierarchyClasses = null;
            try
            {
                hierarchyClasses = messageParser.ParseMessage(args.Message);
            }
            catch (Exception e)
            {
                LogAndNotifyError(e);
            }

            if (hierarchyClasses != null)
            {
                try
                {
                    switch (hierarchyClasses.First().Action)
                    {
                        case Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.AddOrUpdate:
                            hierarchyClassService.ProcessHierarchyClasses(new AddOrUpdateHierarchyClassRequest { HierarchyClasses = hierarchyClasses });
                            break;
                        case Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete:
                            deleteBrandsService.ProcessHierarchyClasses(new DeleteBrandRequest { HierarchyClasses = hierarchyClasses });
                            break;
                        default:
                            throw new ArgumentException($"No handler specified for Action {hierarchyClasses.First().Action}");
                    }

                }
                catch (Exception e)
                {
                    LogAndNotifyError(e);
                }
            }

            AcknowledgeMessage(args);
        }
    }
}
