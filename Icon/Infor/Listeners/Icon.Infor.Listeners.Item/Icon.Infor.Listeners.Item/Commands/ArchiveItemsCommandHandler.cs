using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Commands
{
    public class ArchiveItemsCommandHandler : ICommandHandler<ArchiveItemsCommand>
    {
        public void Execute(ArchiveItemsCommand data)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                sqlBulkCopy.DestinationTableName = "infor.MessageArchiveProduct";
                sqlBulkCopy.WriteToServer(data.Models
                    .Select(m => new MessageArchiveProductModel(m))
                    .ToDataTable());
            }
        }
    }
}