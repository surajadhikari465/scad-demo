using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateHierarchyClassesCommandHandler : ICommandHandler<AddOrUpdateHierarchyClassesCommand>
    {
        private IDbProvider dbProvider;

        public AddOrUpdateHierarchyClassesCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(AddOrUpdateHierarchyClassesCommand data)
        {
            DateTime timestamp = AssignTimestamp(data);
            Guid transactionId = Guid.NewGuid();
            CopyHierarchyClassesToStaging(data, transactionId);
            MergeFromStaging(transactionId);
            DeleteFromStaging(transactionId);
        }

        private static DateTime AssignTimestamp(AddOrUpdateHierarchyClassesCommand data)
        {
            DateTime timestamp = DateTime.Now;
            foreach (var hierarchyClass in data.HierarchyClasses)
            {
                hierarchyClass.Timestamp = timestamp;
            }

            return timestamp;
        }

        private void CopyHierarchyClassesToStaging(AddOrUpdateHierarchyClassesCommand data, Guid transactionId)
        {
            if (dbProvider.Transaction != null)
            {
                using (var sqlBulkCopy = new SqlBulkCopy(dbProvider.Connection as SqlConnection,
                    SqlBulkCopyOptions.Default,
                    dbProvider.Transaction as SqlTransaction))
                {
                    sqlBulkCopy.DestinationTableName = "stage.HierarchyClass";
                    sqlBulkCopy.WriteToServer(data.HierarchyClasses
                        .Select(hc => new
                        {
                            hc.HierarchyClassId,
                            hc.HierarchyId,
                            hc.HierarchyClassName,
                            hc.HierarchyClassParentId,
                            hc.Timestamp,
                            transactionId
                        }).ToDataTable());
                }
            }
            else
            {
                using (var sqlBulkCopy = new SqlBulkCopy(dbProvider.Connection as SqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = "stage.HierarchyClass";
                    sqlBulkCopy.WriteToServer(data.HierarchyClasses
                        .Select(hc => new
                        {
                            hc.HierarchyClassId,
                            hc.HierarchyId,
                            hc.HierarchyClassName,
                            hc.HierarchyClassParentId,
                            hc.Timestamp,
                            transactionId
                        }).ToDataTable());
                }
            }
        }

        private void MergeFromStaging(Guid transactionId)
        {
            dbProvider.Connection.Execute(
                            "dbo.AddOrUpdateHierarchyClass_FromStaging",
                            new { transactionId = transactionId },
                            dbProvider.Transaction,
                            null,
                            CommandType.StoredProcedure);
        }

        private void DeleteFromStaging(Guid transactionId)
        {
            dbProvider.Connection.Execute(
                            "DELETE FROM stage.HierarchyClass WHERE TransactionId = @transactionId",
                            new { transactionId = transactionId },
                            dbProvider.Transaction);
        }
    }
}
