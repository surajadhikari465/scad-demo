using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace AttributePublisher.DataAccess.Commands
{
    public class ArchiveMessagesCommandHandler : ICommandHandler<ArchiveMessagesCommand>
    {
        private IDbConnection dbConnection;

        public ArchiveMessagesCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(ArchiveMessagesCommand data)
        {
            foreach (var message in data.Messages)
            {
                dbConnection.Execute(
                    "INSERT INTO esb.MessageArchive(MessageId, MessageTypeId, Message, MessageHeaders) VALUES (@MessageId, @MessageTypeId, @Message, @MessageHeaders)",
                    message);
                foreach (var attribute in message.AttributeModels)
                {
                    dbConnection.Execute(
                        "INSERT INTO esb.MessageQueueAttributeArchive(MessageId, AttributeId, MessageQueueAttributeJson) VALUES (@MessageId, @AttributeId, @MessageQueueAttributeJson)",
                        attribute);
                }
            }
        }
    }
}
