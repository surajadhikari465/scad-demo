using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammoth.Esb.HierarchyClassListener.Models;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public abstract class DeleteHierarchyClassesGenericCommandHandler<T>
        : ICommandHandler<T> where T : IHierarchyClassesParameter
    {
        protected IDbProvider dbProvider;
        protected abstract int hierarchyId { get; }

        public DeleteHierarchyClassesGenericCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(T data)
        {
            var dbParameters = new
            {
                HierarchyClassIds = data.HierarchyClasses.Select(b => b.HierarchyClassId),
                HierarchyId = hierarchyId
            };
            var lineageDeleteSql = "DELETE FROM dbo.Hierarchy_Merchandise" +
                $" WHERE SegmentHCID IN @{nameof(dbParameters.HierarchyClassIds)}" +
                $" OR FamilyHCID IN @{nameof(dbParameters.HierarchyClassIds)}" +
                $" OR ClassHCID IN @{nameof(dbParameters.HierarchyClassIds)}" +
                $" OR BrickHCID IN @{nameof(dbParameters.HierarchyClassIds)}" +
                $" OR SubBrickHCID IN @{nameof(dbParameters.HierarchyClassIds)}";
            var hierarchyClassDeleteSql = "DELETE FROM dbo.HierarchyClass " +
                     $"WHERE HierarchyClassID IN @{nameof(dbParameters.HierarchyClassIds)} " +
                     $"AND HierarchyID = @{nameof(dbParameters.HierarchyId)}";

            int affectedCountForLineage= dbProvider.Connection
                .Execute(lineageDeleteSql, dbParameters, dbProvider.Transaction);
            int affectedCount = dbProvider.Connection
                .Execute(hierarchyClassDeleteSql, dbParameters, dbProvider.Transaction);
        }
    }
}
