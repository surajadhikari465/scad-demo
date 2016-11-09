using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshBrandsCommandHandler : ICommandHandler<RefreshBrandsCommand>
    {
        private IconContext context;

        public RefreshBrandsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RefreshBrandsCommand data)
        {
            var ids = context.HierarchyClass
                .Where(hc => hc.hierarchyID == Hierarchies.Brands && data.Brands.Contains(hc.hierarchyClassID))
                .Select(hc => hc.hierarchyClassID)
                .ToList()
                .ConvertAll(i => new
                {
                    I = i
                })
                .ToDataTable();

            RefreshBrands(ids);
        }

        private void RefreshBrands(DataTable ids)
        {
            SqlParameter inputType = new SqlParameter("ids", SqlDbType.Structured)
            {
                TypeName = "app.IntList",
                Value = ids
            };

            string sql = "EXEC app.RefreshBrands @ids";
            context.Database.ExecuteSqlCommand(sql, inputType);
        }
    }
}
