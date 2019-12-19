using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.QueueReaders
{
    public class HierarchyQueueReader : IQueueReader<MessageQueueHierarchy, Contracts.HierarchyType>
    {
        private ILogger<HierarchyQueueReader> logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>> updateMessageQueueStatusCommandHandler;

        public HierarchyQueueReader(
            ILogger<HierarchyQueueReader> logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>> updateMessageQueueStatusCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
        }

        public List<MessageQueueHierarchy> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueueHierarchy>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueueHierarchy> GroupMessagesForMiniBulk(List<MessageQueueHierarchy> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
            }

            // Determine the top-level hierarchy information based on the first message in the queue.  Only messages which match this top-level information
            // can be bundled together.
            int currentMessageIndex = 0;
            MessageQueueHierarchy message = messages[currentMessageIndex++];

            var hierarchyId = message.HierarchyId;
            var hierarchyPrototype = new Contracts.HierarchyPrototypeType
            {
                hierarchyLevelName = message.HierarchyLevelName,
                itemsAttached = message.ItemsAttached ? "1" : "0"
            };

            var groupedMessages = new List<MessageQueueHierarchy> { message };

            int miniBulkLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitHierarchy"], out miniBulkLimit))
            {
                miniBulkLimit = 100;
            }

            while (groupedMessages.Count < miniBulkLimit && currentMessageIndex < messages.Count)
            {
                message = messages[currentMessageIndex];

                if (MessageContainsDifferentHierarchy(hierarchyId, message))
                {
                    currentMessageIndex++;
                    continue;
                }
                else if (MessageContainsDifferentPrototype(hierarchyPrototype, message.HierarchyLevelName, message.ItemsAttached ? "1" : "0"))
                {
                    currentMessageIndex++;
                    continue;
                }
                else
                {
                    groupedMessages.Add(message);
                    currentMessageIndex++;
                }
            }

            logger.Info(string.Format("Grouped {0} queued messages to be included in the mini-bulk.", groupedMessages.Count));

            return groupedMessages;
        }

        public Contracts.HierarchyType BuildMiniBulk(List<MessageQueueHierarchy> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
            }

            var miniBulk = new Contracts.HierarchyType
            {
                @class = new Contracts.HierarchyClassType[messages.Count],

                // Assign the top-level hierarchy information. This will be the same for each message in this mini-bulk, so using the first one will do.
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = messages[0].HierarchyId,
                name = messages[0].HierarchyName,
                prototype = new Contracts.HierarchyPrototypeType
                {
                    hierarchyLevelName = messages[0].HierarchyLevelName,
                    itemsAttached = messages[0].ItemsAttached ? "1" : "0"
                }
            };

            // Populate the hierarchyClass information.
            int currentMiniBulkIndex = 0;
            foreach (var message in messages)
            {
                try
                {
                    var miniBulkEntry = new Contracts.HierarchyClassType
                    {
                        Action = GetMessageAction(message.MessageActionId),
                        ActionSpecified = true,
                        id = message.HierarchyClassId.ToString(),
                        name = message.HierarchyClassName,
                        level = message.HierarchyLevel,
                        parentId = new Contracts.hierarchyParentClassType { Value = message.HierarchyParentClassId ?? 0 }
                    };

                    var traits = BuildTraits(message);
                    miniBulkEntry.traits = traits.Any() ? traits : null;

                    miniBulk.@class[currentMiniBulkIndex++] = miniBulkEntry;
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                        message.MessageQueueId));

                    ExceptionLogger<HierarchyQueueReader> exceptionLogger = new ExceptionLogger<HierarchyQueueReader>(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                    var command = new UpdateMessageQueueStatusCommand<MessageQueueHierarchy>
                    {
                        QueuedMessages = new List<MessageQueueHierarchy> { message },
                        MessageStatusId = MessageStatusTypes.Failed,
                        ResetInProcessBy = true
                    };

                    updateMessageQueueStatusCommandHandler.Execute(command);

                    string errorMessage = string.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
                    string emailSubject = Resource.FailedToAddQueuedMessageToMiniBulkEmailSubject;
                    string emailBody = EmailHelper.BuildMessageBodyForMiniBulkError(errorMessage, message.MessageQueueId, ex.ToString());

                    try
                    {
                        emailClient.Send(emailBody, emailSubject);
                    }
                    catch (Exception mailEx)
                    {
                        string mailErrorMessage = "A failure occurred while attempting to send the alert email.";
                        exceptionLogger.LogException(mailErrorMessage, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                    }
                }
            }

            return miniBulk;
        }

        private Contracts.TraitType[] BuildTraits(MessageQueueHierarchy message)
        {
            List<Contracts.TraitType> traits = new List<Contracts.TraitType>();

            if (message.HierarchyId == Hierarchies.National)
            {
                traits.Add(BuildTrait(TraitCodes.NationalClassCode, String.Empty, message.NationalClassCode));
            }

            if (message.HierarchyId == Hierarchies.Brands)
            {
                traits.AddRange(new List<Contracts.TraitType>()
                    {
                        BuildTrait(TraitCodes.BrandAbbreviation, TraitDescriptions.BrandAbbreviation, message.BrandAbbreviation),
                        BuildTrait(TraitCodes.ZipCode, TraitDescriptions.ZipCode, message.ZipCode),
                        BuildTrait(TraitCodes.Designation, TraitDescriptions.Designation, message.Designation),
                        BuildTrait(TraitCodes.Locality, TraitDescriptions.Locality, message.Locality),
                        BuildTrait(TraitCodes.ParentCompany, TraitDescriptions.ParentCompany, message.ParentCompany)
                    });
            }

            if (message.HierarchyId == Hierarchies.Manufacturer)
            {
                traits.AddRange(new List<Contracts.TraitType>()
                {
                    BuildTrait(TraitCodes.ZipCode, TraitDescriptions.ZipCode, message.ZipCode),
                    BuildTrait(TraitCodes.ArCustomerId, TraitDescriptions.ArCustomerId, message.ArCustomerId)
                });
            }

            return traits.Where(b => b.type.value[0].value != string.Empty).ToArray();
        }

        private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value)
        {
            var trait = new Contracts.TraitType
            {
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = traitDescription,
                    value = new Contracts.TraitValueType[]
                            {
                                                new Contracts.TraitValueType
                                                {
                                                        value = value ?? string.Empty,
                                                }
                            }
                }
            };

            return trait;
        }

        private Contracts.ActionEnum GetMessageAction(int messageActionId)
        {
            switch (messageActionId)
            {
                case MessageActionTypes.AddOrUpdate:
                    return Contracts.ActionEnum.AddOrUpdate;

                case MessageActionTypes.Delete:
                    return Contracts.ActionEnum.Delete;

                default:
                    throw new ArgumentException(string.Format("Invalid message action {0}:  Provided messageActionId does not match any available actions in the schema.", messageActionId));
            }
        }

        private bool MessageContainsDifferentPrototype(Contracts.HierarchyPrototypeType hierarchyPrototype, string levelName, string itemsAttached)
        {
            return hierarchyPrototype.hierarchyLevelName != levelName || hierarchyPrototype.itemsAttached != itemsAttached;
        }

        private bool MessageContainsDifferentHierarchy(int hierarchyId, MessageQueueHierarchy message)
        {
            return hierarchyId != message.HierarchyId;
        }

        public void MarkQueuedMessagesAsInProcess(int businessUnit)
        {
            throw new NotImplementedException();
        }
    }
}
