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
        protected IDbProvider DbProvider { get; set; }
        protected abstract int HierarchyId { get; }

        public DeleteHierarchyClassesGenericCommandHandler(IDbProvider dbProvider)
        {
            this.DbProvider = dbProvider;
        }

        public void Execute(T data)
        {
            const string tempTable = "#tempDeleteHierarchyClass";
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
                DELETE FROM dbo.HierarchyClass
                WHERE HierarchyID = {HierarchyId}
                    AND HierarchyClassID IN (SELECT HierarchyClassID FROM {tempTable});";
            var deleteHierarchyClassResult = DbProvider.Connection.Execute(
                sql: sqlToExecute, param: null, transaction: DbProvider.Transaction);
        }
    }
}
