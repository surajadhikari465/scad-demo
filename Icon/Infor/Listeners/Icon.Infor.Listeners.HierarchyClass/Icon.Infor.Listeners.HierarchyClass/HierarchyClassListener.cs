using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Esb;
using Icon.Common.Email;
using Icon.Logging;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.MessageParsers;
using Icon.Infor.Listeners.HierarchyClass.Validators;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Common.DataAccess;
using Newtonsoft.Json;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Extensions;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassListener : ListenerApplication<HierarchyClassListener, ListenerApplicationSettings>
    {
        private IMessageParser<IEnumerable<InforHierarchyClassModel>> messageParser;
        private IEnumerable<IHierarchyClassService> services;
        private ICollectionValidator<InforHierarchyClassModel> validator;
        private IHierarchyClassListenerNotifier notifier;
        private IRenewableContext<IconContext> globalContext;
        private ICommandHandler<ArchiveHierarchyClassesCommand> archiveHierarchyClassesCommandHandler;
        private ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler;

        public HierarchyClassListener(
            IMessageParser<IEnumerable<InforHierarchyClassModel>> messageParser,
            ICollectionValidator<InforHierarchyClassModel> validator,
            IEnumerable<IHierarchyClassService> services,
            IRenewableContext<IconContext> globalContext,
            ICommandHandler<ArchiveHierarchyClassesCommand> archiveHierarchyClassesCommandHandler,
            ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            IHierarchyClassListenerNotifier notifier,
            ILogger<HierarchyClassListener> logger) 
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.services = services;
            this.validator = validator;
            this.globalContext = globalContext;
            this.archiveHierarchyClassesCommandHandler = archiveHierarchyClassesCommandHandler;
            this.archiveMessageCommandHandler = archiveMessageCommandHandler;
            this.notifier = notifier;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            this.globalContext.Refresh();
            IEnumerable<InforHierarchyClassModel> hierarchyClasses = new List<InforHierarchyClassModel>();
            try
            {
                hierarchyClasses = messageParser.ParseMessage(args.Message);

                if (hierarchyClasses.Any())
                {
                    validator.ValidateCollection(hierarchyClasses);

                    this.services.ToList().ForEach(
                        s => s.ProcessHierarchyClassMessages(hierarchyClasses));
                }
            }
            catch (Exception ex)
            {
                this.LogAndNotifyErrorWithMessage(ex, args);
                if(hierarchyClasses != null)
                {
                    foreach (var hierarchyClass in hierarchyClasses.Where(hc => hc.ErrorCode == null))
                    {
                        hierarchyClass.ErrorCode = ApplicationErrors.Codes.UnexpectedError;
                        hierarchyClass.ErrorDetails = $"{ApplicationErrors.Descriptions.UnexpectedError} Error Details: {ex}";
                    }
                }
            }
            finally
            {
                try
                {
                    archiveMessageCommandHandler.Execute(new ArchiveMessageCommand { Message = args.Message });
                    if (hierarchyClasses.Any())
                    {
                        archiveHierarchyClassesCommandHandler.Execute(new ArchiveHierarchyClassesCommand { Models = hierarchyClasses });
                        notifier.NotifyOfError(args.Message, hierarchyClasses.Where(hc => hc.ErrorCode != null).ToList());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(
                        new
                        {
                            ErrorCode = ApplicationErrors.Codes.UnableToArchiveMessage,
                            ErrorDescription = ApplicationErrors.Descriptions.UnableToArchiveMessage,
                            Message = args.Message.ToString(),
                            Exception = ex.ToString()
                        }.ToJson());
                }
                this.AcknowledgeMessage(args);
                this.globalContext.Refresh();
            }
        }
    }
}
