using AttributePublisher.DataAccess.Commands;
using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Linq;

namespace AttributePublisher.Operations
{
    public class ArchiveAttributeMessagesOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private const int AttributeMessageTypeId = 14;

        private ICommandHandler<ArchiveMessagesCommand> archiveMessagesCommandHandler;

        public ArchiveAttributeMessagesOperation(IOperation<AttributePublisherServiceParameters> next, ICommandHandler<ArchiveMessagesCommand> archiveMessagesCommandHandler) : base(next)
        {
            this.archiveMessagesCommandHandler = archiveMessagesCommandHandler;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            archiveMessagesCommandHandler.Execute(new ArchiveMessagesCommand
            {
                Messages = parameters.AttributeMessages.Select(m => new MessageArchiveModel
                {
                    Message = XDocument.Parse(m.Message).ToString(),
                    MessageHeaders = JsonConvert.SerializeObject(m.MessageHeaders),
                    MessageId = m.MessageId,
                    MessageTypeId = AttributeMessageTypeId,
                    AttributeModels = m.Attributes.Select(a => new MessageArchiveAttributeModel
                    {
                        MessageId = m.MessageId,
                        AttributeId = a.AttributeId,
                        MessageQueueAttributeJson = JsonConvert.SerializeObject(a)
                    }).ToList()
                }).ToList()
            });
        }
    }
}