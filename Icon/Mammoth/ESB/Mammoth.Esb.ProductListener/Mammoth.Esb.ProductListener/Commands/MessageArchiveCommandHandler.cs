using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Newtonsoft.Json;
using System.Data;
using System.Xml.Linq;

namespace Mammoth.Esb.ProductListener.Commands
{
    public class MessageArchiveCommandHandler : ICommandHandler<MessageArchiveCommand>
    {
        IDbProvider db;

        //Global Item Type
        private const int MessageTypeId = 8;
        //Failed Status
        private const int MessageStatusId = 3;       
        
        public MessageArchiveCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(MessageArchiveCommand data)
        {
            if (db.Connection.State == ConnectionState.Closed)
                db.Connection.Open();

            if (data.MessageId != null)
            {
                db.Connection.Execute(
                    @"  INSERT INTO esb.MessageArchive(MessageID, MessageTypeId, MessageStatusId, MessageHeadersJson, MessageBody, InsertDateUtc)
                    VALUES(@MessageID, @MessageTypeId, @MessageStatusId, @MessageHeadersJson, @MessageBody, @InsertDateUtc)",
                    new
                    {
                        MessageID = data.MessageId,
                        MessageTypeId = MessageTypeId,
                        MessageStatusId = MessageStatusId,
                        MessageHeadersJson = data.MessageHeadersJson,
                        MessageBody = XDocument.Parse(data.MessageBody),
                        InsertDateUtc = data.InsertDateUtc
                    }, transaction: this.db.Transaction);
            }
        }
    }
}