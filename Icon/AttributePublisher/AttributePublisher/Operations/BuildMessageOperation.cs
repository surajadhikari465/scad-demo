using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.MessageBuilders;
using AttributePublisher.Models;
using AttributePublisher.Services;
using Esb.Core.MessageBuilders;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributePublisher.Operations
{
    public class BuildMessageOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private IMessageBuilder<List<AttributeModel>> messageBuilder;
        private IMessageHeaderBuilder messageHeaderBuilder;
        private AttributePublisherServiceSettings settings;

        public BuildMessageOperation(
            IOperation<AttributePublisherServiceParameters> next, 
            IMessageBuilder<List<AttributeModel>> messageBuilder, 
            IMessageHeaderBuilder messageHeaderBuilder,
            AttributePublisherServiceSettings settings) : base(next)
        {
            this.messageBuilder = messageBuilder;
            this.messageHeaderBuilder = messageHeaderBuilder;
            this.settings = settings;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            foreach (var attributes in parameters.Attributes.Batch(settings.RecordsPerMessage))
            {
                string messageText = messageBuilder.BuildMessage(attributes.ToList());
                var messageID = Guid.NewGuid().ToString();

                parameters.AttributeMessages.Add(
                    new AttributeMessageModel
                    {
                        Attributes = attributes.ToList(),
                        MessageId = messageID,
                        Message = messageText,
                        MessageHeaders = messageHeaderBuilder.BuildHeader(messageID),
                    });
            }
        }
    }
}
