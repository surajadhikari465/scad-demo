using System.Data.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace InventoryProducer.Validator.DAL
{
    internal class MessageArchiveEventDAL : DALBase
    {
        internal MessageArchiveEventDAL(Database database) : base("[amz].[MessageArchiveEvent]", database)
        {
        }
        internal IList<MessageArchiveEvent> GetList(string[] eventTypeCodes, int maxRecords = 1000, string beforeDate = "2022-08-01 00:00:00.0000000")
        {
            var messageArchiveEventsRaw = new List<MessageArchiveEvent>();
            foreach(string eventTypeCode in eventTypeCodes)
            {
                string sqlQuery = @$"
                SELECT TOP ({maxRecords/eventTypeCodes.Length}) [MessageArchiveEventID]
                  ,[KeyID]
                  ,[SecondaryKeyID]
                  ,[MessageType]
                  ,[EventTypeCode]
                  ,[InsertDate]
                  ,[MessageTimestampUtc]
                  ,[MessageID]
                  ,[MessageHeaders] as MessageHeadersJSON
                  ,[Message]
                  ,[ErrorCode]
                  ,[ErrorDetails]
                  ,[ArchiveInsertDateUtc]
                FROM [ItemCatalog].{TableName} tb
                inner join dbo.OrderHeader oh on oh.OrderHeader_ID = tb.KeyID
                where InsertDate < '{beforeDate}'
                and EventTypeCode = '{eventTypeCode}'
                order by InsertDate desc";
                var output = this.Database.SqlQuery<MessageArchiveEvent>(sqlQuery,
                                                        new object[0]).ToList();
                messageArchiveEventsRaw.AddRange(output);
            }
            
            return messageArchiveEventsRaw;
        }
        internal MessageArchiveEvent Get(int keyId, int? secondaryKeyId, string eventTypeCode)
        {
            string secondaryKeyIdExpression = secondaryKeyId.HasValue ? "= @SecondaryKeyID" : "is null";
            string sqlQuery = @$"
                SELECT TOP (1) [MessageArchiveEventID]
                  ,[KeyID]
                  ,[SecondaryKeyID]
                  ,[MessageType]
                  ,[EventTypeCode]
                  ,[InsertDate]
                  ,[MessageTimestampUtc]
                  ,[MessageID]
                  ,[MessageHeaders] as MessageHeadersJSON
                  ,[Message]
                  ,[ErrorCode]
                  ,[ErrorDetails]
                  ,[ArchiveInsertDateUtc]
                FROM [ItemCatalog].{TableName}
                where KeyID = @KeyID and SecondaryKeyID {secondaryKeyIdExpression} and EventTypeCode = @EventTypeCode
                order by InsertDate desc";
            var sqlParams = new List<SqlParameter>() { new SqlParameter("@KeyID", keyId), new SqlParameter("@EventTypeCode", eventTypeCode) };
            if (secondaryKeyId.HasValue)
            {
                sqlParams.Add(new SqlParameter("@SecondaryKeyID", secondaryKeyId.Value));
            }
            var messageArchiveEventsRaw = this.Database.SqlQuery<MessageArchiveEvent>(sqlQuery, sqlParams.ToArray()).First();
            return messageArchiveEventsRaw;
        }
    }
}
