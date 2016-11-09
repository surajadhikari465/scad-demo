using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshMerchandiseHierarchyClassesCommandHandler : ICommandHandler<RefreshMerchandiseHierarchyClassesCommand>
    {
        private IconContext context;

        public RefreshMerchandiseHierarchyClassesCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RefreshMerchandiseHierarchyClassesCommand data)
        {
            var validMerchandiseIds = context.HierarchyClass
                .Where(hc => hc.hierarchyID == Hierarchies.Merchandise && data.MerchandiseHierarchyClassIds.Contains(hc.hierarchyClassID))
                .Select(sc => sc.hierarchyClassID)
                .ToList()
                .ConvertAll(id => new
                {
                    I = id
                })
                .ToDataTable();

            RefreshMerchandiseHierarchyClasses(validMerchandiseIds);
        }

        private void RefreshMerchandiseHierarchyClasses(DataTable validMerchandiseIds)
        {
            SqlParameter inputType = new SqlParameter("ids", SqlDbType.Structured)
            {
                TypeName = "app.IntList",
                Value = validMerchandiseIds
            };

            string sql = "EXEC app.RefreshMerchandiseHierarchyClasses @ids";
            context.Database.ExecuteSqlCommand(sql, inputType);
        }
    }
}