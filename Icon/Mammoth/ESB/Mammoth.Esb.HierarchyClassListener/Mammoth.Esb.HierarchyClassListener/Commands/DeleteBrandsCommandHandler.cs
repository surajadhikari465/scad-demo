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
    public class DeleteBrandsCommandHandler : ICommandHandler<DeleteBrandsCommand>
    {
        private IDbProvider dbProvider;

        public DeleteBrandsCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(DeleteBrandsCommand data)
        {
            int affectedCount = dbProvider.Connection.Execute(
                @"DELETE FROM dbo.HierarchyClass WHERE HierarchyClassID IN @HierarchyClassIds AND HierarchyID = @BrandHierarchyId",
                new
                {
                    HierarchyClassIds = data.Brands.Select(b => b.HierarchyClassId),
                    BrandHierarchyId = Hierarchies.Brands
                },
                dbProvider.Transaction);
        }
    }
}
