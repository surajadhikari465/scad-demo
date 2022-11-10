using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Validator.DAL
{
    internal class MessageDAL : DALBase
    {
        internal MessageDAL(Database database) : base("[amz].[MessageArchive]", database)
        {
        }

        internal Message? GetMessage(long messageNumber, int keyId)
        {
            string sqlQuery = @$"
                SELECT TOP (1) 
                      [InsertDate]
                      ,[MessageNumber]
                      ,[Message] as RawMessage
                  FROM [ItemCatalog].{TableName}
                  where MessageNumber = @MessageNumber
                    and KeyId = @KeyId
                ";
            return this.Database.SqlQuery<Message>(sqlQuery,
                                                     new SqlParameter("@MessageNumber", messageNumber), new SqlParameter("@KeyId", keyId)).FirstOrDefault();
        }
    }
}
