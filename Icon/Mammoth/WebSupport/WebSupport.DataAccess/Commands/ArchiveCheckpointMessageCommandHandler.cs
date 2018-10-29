using Mammoth.Common.DataAccess;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;

namespace WebSupport.DataAccess.Commands
{
    public class ArchiveCheckpointMessageCommandHandler : ICommandHandler<ArchiveCheckpointMessageCommandParameters>
    {
        private IDbConnection dbConnection;

        public ArchiveCheckpointMessageCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(ArchiveCheckpointMessageCommandParameters data)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            if (data.MessageId != null)
            {
                dbConnection.Execute(
                    @"  INSERT INTO esb.MessageArchive(MessageID, MessageTypeId, MessageStatusId, MessageBody, MessageHeadersJson)
                    VALUES(@MessageID, @MessageTypeId, @MessageStatusId, @MessageBody, @MessageHeadersJson)",
                    new
                    {
                        MessageID = data.MessageId,
                        MessageTypeId = MessageTypes.CheckpointRequest,
                        MessageStatusId = data.MessageStatusId,
                        MessageBody = XDocument.Parse(data.Message),
                        MessageHeadersJson = data.MessageHeadersJson,
                    });

            }
        }
    }
}
