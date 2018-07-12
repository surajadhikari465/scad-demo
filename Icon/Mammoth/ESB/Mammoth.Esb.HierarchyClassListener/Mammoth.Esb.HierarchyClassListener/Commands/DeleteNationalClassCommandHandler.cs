using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Models;
using System;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteNationalClassCommandHandler
        : DeleteHierarchyClassesGenericCommandHandler<DeleteHierarchyClassesParameter>,
        ICommandHandler<DeleteNationalClassParameter>
    {
        private const int brandHierarchyId = Hierarchies.National;
        protected override int HierarchyId
        {
            get { return brandHierarchyId; }
        }

        public DeleteNationalClassCommandHandler(IDbProvider dbProvider)
            : base(dbProvider) { }


        public void Execute(DeleteNationalClassParameter data)
        {
            const string tempTable = "#tempDeleteNationalClass";
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
                DELETE HM FROM dbo.Hierarchy_NationalClass HM
                    INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.FamilyHCID;";
            var deleteSegmentResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_NationalClass
                SET CategoryHCID = NULL, SubcategoryHCID = NULL, ClassHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_NationalClass HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.CategoryHCID;";
            var deleteFamilyResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_NationalClass
                SET SubcategoryHCID = NULL, ClassHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_NationalClass HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.SubcategoryHCID;";
            var deleteClassResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            sqlToExecute = $@"
                UPDATE dbo.Hierarchy_NationalClass
                SET ClassHCID = NULL, ModifiedDate = GETDATE()
                FROM dbo.Hierarchy_NationalClass HM
                        INNER JOIN {tempTable} TEMP ON TEMP.HierarchyClassID = HM.ClassHCID;";
            var deleteSubBrickResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);

            // call the base execute method to delete the hierarchy classes themselves
            base.Execute(data);
        }
    }
}