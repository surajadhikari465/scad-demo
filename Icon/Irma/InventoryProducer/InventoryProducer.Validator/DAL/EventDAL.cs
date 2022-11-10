using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InventoryProducer.Validator.DAL
{
    internal class EventDAL : DALBase
    {
        internal EventDAL(string tableName, Database database) : base(tableName, database)
        {
        }
        internal bool Create(Event newEvent)
        {
            DateTime timeNow = DateTime.Now;
            string insertSQL = $@"
                INSERT INTO {TableName} (
					[EventTypeCode]
					,[MessageType]
					,[KeyID]
					,[SecondaryKeyID]
					,[InsertDate]
					,[MessageTimestampUtc]
					)
				VALUES (
					@EventTypeCode
					,@MessageType
					,@KeyID
					,@SecondaryKeyID
					,@InsertDate
					,@MessageTimestampUtc
					)";
            return this.Database
                .ExecuteSqlCommand(
                insertSQL,
                new SqlParameter("@EventTypeCode", newEvent.EventTypeCode),
                new SqlParameter("@KeyID", newEvent.KeyID),
                new SqlParameter("@SecondaryKeyID", newEvent.SecondaryKeyID.HasValue ? newEvent.SecondaryKeyID.Value : DBNull.Value),
                new SqlParameter("@MessageType", newEvent.MessageType),
                new SqlParameter("@InsertDate", timeNow),
                new SqlParameter("@MessageTimestampUtc", timeNow)
                ) == 1;

        } 

        internal void Truncate()
        {
            string truncateSQL = $"truncate table [ItemCatalog].{TableName}";
            this.Database.ExecuteSqlCommand(truncateSQL, new object[0]);
        }
    }
}
