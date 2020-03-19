using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using System.Linq;
using Icon.Common;

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
            DbProvider.Connection.Execute(
                sql: "dbo.DeleteHierarchyMerchandise",
                param: new
                {
                    @hcID = data.HierarchyClasses.Select(x => new
                    {
                        Value = x.HierarchyClassId,
                    })
                    .ToDataTable()
                    .AsTableValuedParameter("dbo.IntListType")
                },
            transaction: DbProvider.Transaction,
            commandType: System.Data.CommandType.StoredProcedure);

            base.Execute(data);
        }
    }
}
