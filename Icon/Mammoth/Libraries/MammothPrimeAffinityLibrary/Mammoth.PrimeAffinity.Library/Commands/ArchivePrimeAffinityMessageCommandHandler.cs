using Dapper;
using FastMember;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Mammoth.PrimeAffinity.Library.Commands
{
    public class ArchivePrimeAffinityMessageCommandHandler : ICommandHandler<ArchivePrimeAffinityMessageCommand>
    {
        private IDbConnection dbConnection;

        public ArchivePrimeAffinityMessageCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
        }

        public void Execute(ArchivePrimeAffinityMessageCommand data)
        {
            if (data.MessageId != null)
            {
                dbConnection.Execute(
                    @"  INSERT INTO esb.MessageArchive(MessageID, MessageTypeId, MessageStatusId, MessageBody, MessageHeadersJson)
                    VALUES(@MessageID, @MessageTypeId, @MessageStatusId, @MessageBody, @MessageHeadersJson)",
                    new
                    {
                        MessageID = data.MessageId,
                        MessageTypeId = MessageTypes.PrimePsg,
                        MessageStatusId = data.MessageStatusId,
                        MessageBody = XDocument.Parse(data.Message),
                        MessageHeadersJson = data.MessageHeadersJson,
                    });
            }
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbConnection as SqlConnection))
            {
                sqlBulkCopy.DestinationTableName = "esb.MessageArchiveDetailPrimePsg";
                sqlBulkCopy.WriteToServer(ObjectReader.Create(
                    data.PrimeAffinityMessageModels.Select(p => new
                    {
                        MessageArchiveDetailPrimePsgID = 0,
                        p.MessageAction,
                        p.Region,
                        p.ItemID,
                        p.BusinessUnitID,
                        MessageID = data.MessageId,
                        JsonObject = JsonConvert.SerializeObject(p),
                        ErrorCode = p.ErrorCode,
                        ErrorDetails = p.ErrorDetails
                    }),
                    "MessageArchiveDetailPrimePsgID",
                    nameof(PrimeAffinityMessageModel.MessageAction),
                    nameof(PrimeAffinityMessageModel.Region),
                    nameof(PrimeAffinityMessageModel.ItemID),
                    nameof(PrimeAffinityMessageModel.BusinessUnitID),
                    "MessageID",
                    "JsonObject",
                    nameof(PrimeAffinityMessageModel.ErrorCode),
                    nameof(PrimeAffinityMessageModel.ErrorDetails)));
            }
        }
    }
}
