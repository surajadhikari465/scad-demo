using FastMember;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ArchiveHierarchyClassesCommandHandler : ICommandHandler<ArchiveHierarchyClassesCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public ArchiveHierarchyClassesCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(ArchiveHierarchyClassesCommand data)
        {
            using (IconContext context = contextFactory.CreateContext())
            {
                using (var sqlBulkCopy = new SqlBulkCopy(context.Database.Connection as SqlConnection))
                {
                    context.Database.Connection.Open();
                    using (var reader = ObjectReader.Create(data.Models.Select(hc => new InforMessageArchiveHierarchy(hc)),
                        nameof(InforMessageArchiveHierarchy.MessageArchiveId),
                        nameof(InforMessageArchiveHierarchy.HierarchyClassId),
                        nameof(InforMessageArchiveHierarchy.HierarchyClassName),
                        nameof(InforMessageArchiveHierarchy.HierarchyName),
                        nameof(InforMessageArchiveHierarchy.InforMessageId),
                        nameof(InforMessageArchiveHierarchy.Context),
                        nameof(InforMessageArchiveHierarchy.ErrorCode),
                        nameof(InforMessageArchiveHierarchy.ErrorDetails)))
                    {
                        sqlBulkCopy.DestinationTableName = "infor.MessageArchiveHierarchy";
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
