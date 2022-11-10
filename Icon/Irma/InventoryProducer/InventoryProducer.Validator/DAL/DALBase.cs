using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Validator.DAL
{
    internal class DALBase
    {
        protected readonly string TableName;
        protected readonly Database Database;

        internal DALBase(string tableName, Database database)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}
