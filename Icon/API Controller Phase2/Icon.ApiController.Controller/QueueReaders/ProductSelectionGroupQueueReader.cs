using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Icon.ApiController.Controller.QueueReaders
{
    public class ProductSelectionGroupQueueReader : IQueueReader<MessageQueueProductSelectionGroup, Contracts.SelectionGroupsType>
    {
        private ILogger<ProductSelectionGroupQueueReader> logger;
        private IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>> updateMessageQueueStatusCommandHandler;

        public ProductSelectionGroupQueueReader(
            ILogger<ProductSelectionGroupQueueReader> logger,
            IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>> updateMessageQueueStatusCommandHandler)
        {
            this.logger = logger;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
        }

        public List<MessageQueueProductSelectionGroup> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueueProductSelectionGroup>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters).OrderBy(m => m.MessageQueueId).ToList();
        }

        public List<MessageQueueProductSelectionGroup> GroupMessagesForMiniBulk(List<MessageQueueProductSelectionGroup> messages)
        {
            if (messages == null || !messages.Any())
            {
                throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
            }

            int miniBulkLimit = 0;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitProductSelectionGroup"], out miniBulkLimit))
            {
                miniBulkLimit = 100;
            }

            var groupedMessages = messages
                .Take(miniBulkLimit)
                .GroupBy(m => m.ProductSelectionGroupId)
                .Select(group => group.First());

            return groupedMessages.ToList();
        }

        public Contracts.SelectionGroupsType BuildMiniBulk(List<MessageQueueProductSelectionGroup> messages)
        {
            if (messages == null || !messages.Any())
            {
                throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
            }

            Contracts.SelectionGroupsType selectionGroup = new Contracts.SelectionGroupsType
            {
                // R10 has requested that the ID of the PSG be the name of the PSG and not the ID Icon uses as a unique identifier.
                group = messages.Select(m => new Contracts.GroupTypeType
                    {
                        Action = GetAction(m.MessageActionId, m.MessageQueueId),
                        ActionSpecified = true,
                        id = m.ProductSelectionGroupName,
                        description = m.ProductSelectionGroupName,
                        name = m.ProductSelectionGroupName,
                        type = m.ProductSelectionGroupTypeName
                    })
                .ToArray()
            };

            return selectionGroup;
        }

        private Contracts.ActionEnum GetAction(int actionId, int messageQueueId)
        {
            switch (actionId)
            {
                case MessageActionTypes.AddOrUpdate:
                    {
                        return Contracts.ActionEnum.AddOrUpdate;
                    }

                case MessageActionTypes.Delete:
                    {
                        return Contracts.ActionEnum.Delete;
                    }

                default:
                    {
                        throw new ArgumentException(string.Format("Tried to set Action to {0} for PSG message with MessageQueueId {1}.  {0} is not a valid action for PSG messages.",
                            actionId, messageQueueId));
                    }
            }
        }

        public void MarkQueuedMessagesAsInProcess(int businessUnit)
        {
            throw new NotImplementedException();
        }
    }
}
