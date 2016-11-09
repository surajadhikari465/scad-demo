using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Esb;
using Icon.Logging;
using Icon.Common.Email;
using Icon.Esb.MessageParsers;
using Mammoth.Esb.HierarchyClassListener.Models;
using Icon.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Services;

namespace Mammoth.Esb.HierarchyClassListener
{
    public class MammothHierarchyClassListener : ListenerApplication<MammothHierarchyClassListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<HierarchyClassModel>> messageParser;
        private IHierarchyClassService hierarchyClassService;

        public MammothHierarchyClassListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger,
            IMessageParser<List<HierarchyClassModel>> messageParser,
            IHierarchyClassService hierarchyClassService)
            : base(listenerApplicationSettings,
                  esbConnectionSettings,
                  subscriber,
                  emailClient,
                  logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassService = hierarchyClassService;
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
                    hierarchyClassService.AddOrUpdateHierarchyClasses(new AddOrUpdateHierarchyClassesCommand { HierarchyClasses = hierarchyClasses });
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
