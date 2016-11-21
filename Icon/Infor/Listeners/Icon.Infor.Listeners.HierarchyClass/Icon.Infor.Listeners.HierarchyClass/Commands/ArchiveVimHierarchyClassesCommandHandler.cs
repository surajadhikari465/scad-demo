using FastMember;
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
    public class ArchiveVimHierarchyClassesCommandHandler : ICommandHandler<ArchiveVimHierarchyClassesCommand>
    {
        public void Execute(ArchiveVimHierarchyClassesCommand data)
        {
            if (data.HierarchyClasses.Any())
            {
                using (var sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
                {
                    using (var reader = ObjectReader.Create(data.HierarchyClasses.Select(hc => new VimMessageArchiveHierarchy(hc)),
                        nameof(VimMessageArchiveHierarchy.MessageArchiveId),
                        nameof(VimMessageArchiveHierarchy.HierarchyClassId),
                        nameof(VimMessageArchiveHierarchy.HierarchyClassName),
                        nameof(VimMessageArchiveHierarchy.HierarchyName),
                        nameof(VimMessageArchiveHierarchy.EsbMessageId),
                        nameof(VimMessageArchiveHierarchy.Context),
                        nameof(VimMessageArchiveHierarchy.ErrorCode),
                        nameof(VimMessageArchiveHierarchy.ErrorDetails)))
                    {
                        sqlBulkCopy.DestinationTableName = "vim.MessageArchiveHierarchy";
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
