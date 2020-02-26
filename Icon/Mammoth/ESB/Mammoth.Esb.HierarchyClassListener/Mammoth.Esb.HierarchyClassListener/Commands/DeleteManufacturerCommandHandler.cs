using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteManufacturerCommandHandler
        : DeleteHierarchyClassesGenericCommandHandler<DeleteHierarchyClassesParameter>,
        ICommandHandler<DeleteManufacturerParameter>
    {
        private const int manufacturerHierarchyId = Hierarchies.Manufacturer;
        protected override int HierarchyId
        {
            get { return manufacturerHierarchyId; }
        }

        public DeleteManufacturerCommandHandler(IDbProvider dbProvider)
            : base(dbProvider) { }

        public void Execute(DeleteManufacturerParameter data)
        {
            base.Execute(data);
        }
    }
}