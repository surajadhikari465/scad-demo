using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteMerchandiseClassCommandHandler
        : DeleteHierarchyClassesGenericCommandHandler<DeleteHierarchyClassesParameter>,
        ICommandHandler<DeleteMerchandiseClassParameter>
    {
        private const int brandHierarchyId = Hierarchies.Merchandise;
        protected override int HierarchyId
        {
            get { return brandHierarchyId; }
        }

        public DeleteMerchandiseClassCommandHandler(IDbProvider dbProvider)
            : base(dbProvider) { }


        public void Execute(DeleteMerchandiseClassParameter data)
        {
            const string tempTable = "#tempDeleteMerchandiseClass";
            string sqlToExecute = String.Empty;

            sqlToExecute = $"CREATE TABLE {tempTable} (HierarchyClassID int);";
            DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            var tempInsertParams = data.HierarchyClasses
                .Select(hc => new HierarchyClassModel { HierarchyClassId = hc.HierarchyClassId });
            sqlToExecute = $"INSERT INTO {tempTable} VALUES (@HierarchyClassId);";
            var insertTempResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: tempInsertParams, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                DELETE HM FROM dbo.Hierarchy_Merchandise HM
                    INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.SegmentHCID;";
            var deleteSegmentResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_Merchandise
                SET FamilyHCID = NULL, ClassHCID = NULL, BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_Merchandise HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.FamilyHCID;";
            var deleteFamilyResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_Merchandise
                SET ClassHCID = NULL, BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_Merchandise HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.ClassHCID;";
            var deleteClassResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_Merchandise
                SET BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_Merchandise HM
                       INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.BrickHCID;";
            var deleteBrickResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_Merchandise
                SET SubBrickHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_Merchandise HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.SubBrickHCID;";
            var deleteSubBrickResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            // call the base execute method to delete the hierarchy classes themselves
            base.Execute(data);
        }
    }
}
