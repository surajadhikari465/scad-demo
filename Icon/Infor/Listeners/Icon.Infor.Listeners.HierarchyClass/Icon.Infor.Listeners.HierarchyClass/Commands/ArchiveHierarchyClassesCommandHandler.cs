using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ArchiveHierarchyClassesCommandHandler : ICommandHandler<ArchiveHierarchyClassesCommand>
    {
        public void Execute(ArchiveHierarchyClassesCommand data)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                sqlBulkCopy.DestinationTableName = "infor.MessageArchiveHierarchy";
                sqlBulkCopy.WriteToServer(data.Models
                    .Select(m => new MessageArchiveHierarchyClassModel(m))
                    .ToDataTable());
            }
        }
    }
}
