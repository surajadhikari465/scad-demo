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
    public class DeleteBrandsCommandHandler
        : DeleteHierarchyClassesGenericCommandHandler<DeleteHierarchyClassesParameter>,
        ICommandHandler<DeleteBrandsParameter>
    {
        private const int brandHierarchyId = Hierarchies.Brands;
        protected override int HierarchyId
        {
            get { return brandHierarchyId; }
        }

        public DeleteBrandsCommandHandler(IDbProvider dbProvider)
            : base(dbProvider) { }

        public void Execute(DeleteBrandsParameter data)
        {
            base.Execute(data);
        }
    }
}
