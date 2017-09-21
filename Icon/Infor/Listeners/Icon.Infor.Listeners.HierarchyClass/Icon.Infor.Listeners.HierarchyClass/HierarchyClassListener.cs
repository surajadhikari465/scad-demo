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
using Icon.Infor.Listeners.HierarchyClass.EsbService;
using System.Data.SqlClient;
using Icon.Esb.Services.ConfirmationBod;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassListener : ListenerApplication<HierarchyClassListener, ListenerApplicationSettings>
    {
        private IMessageParser<IEnumerable<InforHierarchyClassModel>> messageParser;
        private IEnumerable<IHierarchyClassService> services;
        private HierarchyClassEsbService esbService;
        private ICollectionValidator<InforHierarchyClassModel> validator;
        private IHierarchyClassListenerNotifier notifier;
        private ICommandHandler<ArchiveHierarchyClassesCommand> archiveHierarchyClassesCommandHandler;
        private ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler;

        public HierarchyClassListener(
            IMessageParser<IEnumerable<InforHierarchyClassModel>> messageParser,
            ICollectionValidator<InforHierarchyClassModel> validator,
            IEnumerable<IHierarchyClassService> services,
            HierarchyClassEsbService esbService,
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
            this.esbService = esbService;
            this.validator = validator;
            this.archiveHierarchyClassesCommandHandler = archiveHierarchyClassesCommandHandler;
            this.archiveMessageCommandHandler = archiveMessageCommandHandler;
            this.notifier = notifier;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            IEnumerable<InforHierarchyClassModel> hierarchyClasses = new List<InforHierarchyClassModel>();
            ConfirmationBodEsbErrorTypes errorType = ConfirmationBodEsbErrorTypes.Data;
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
                errorType = GetErrorTypeFromException(ex);

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
                        notifier.NotifyOfError(args.Message, errorType, hierarchyClasses.Where(hc => hc.ErrorCode != null).ToList());
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
            }
        }

        private ConfirmationBodEsbErrorTypes GetErrorTypeFromException(Exception ex)
        {
            ConfirmationBodEsbErrorTypes errorType;
            if (ex.Message == ApplicationErrors.Codes.UnableToParseHierarchyClass)
            {
                errorType = ConfirmationBodEsbErrorTypes.Schema;
            }
            // 2627 is unique constraint key error and 547 is foreign key violation
            else if (ex.GetBaseException().GetType() == typeof(SqlException) &&
                     ((SqlException)ex).Number == 2627 || ((SqlException)ex).Number == 547)
            {
                errorType = ConfirmationBodEsbErrorTypes.DatabaseConstraint;
            }
            else
            {
                errorType = ConfirmationBodEsbErrorTypes.Data;
            }
            return errorType;
        }
    }
}