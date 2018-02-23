using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class DeleteMerchandiseClassCommandHandler
        : DeleteHierarchyClassesGenericCommandHandler<DeleteHierarchyClassesParameter>,
        ICommandHandler<DeleteMerchandiseClassParameter>
    {
        private const int brandHierarchyId = Hierarchies.Merchandise;
        protected override int hierarchyId { get => brandHierarchyId; }

        public DeleteMerchandiseClassCommandHandler(IDbProvider dbProvider)
            : base(dbProvider) { }


        public void Execute(DeleteMerchandiseClassParameter data)
        {
            base.Execute(data);
        }
    }
}
