using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshHierarchiesCommandHandler : ICommandHandler<RefreshHierarchiesCommand>
    {
        private IconContext context;

        public RefreshHierarchiesCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RefreshHierarchiesCommand data)
        {
            var hierarchyClasses = GetListOfHiearchiesFromIds(data.HierarchyClassIds);

            var brands = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Brands);
            var nationals = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.National);
            var merchandises = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Merchandise);
            var manufacturer = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Manufacturer);

            if (brands.Rows.Count > 0) RefreshHierarchyClasses(brands, "EXEC app.RefreshBrands @ids");
            if (nationals.Rows.Count > 0) RefreshHierarchyClasses(nationals, "EXEC app.RefreshNationalHierarchyClasses @ids");
            if (merchandises.Rows.Count > 0) RefreshHierarchyClasses(merchandises, "EXEC app.RefreshMerchandiseHierarchyClasses @ids");
            if (manufacturer.Rows.Count > 0 && data.IsManufacturerHierarchyMessage) RefreshHierarchyClasses(manufacturer, "EXEC app.RefreshManufacturerHierarchyClasses @ids");
        }

        private List<HierarchyClass> GetListOfHiearchiesFromIds(List<int> ids)
        {
            return context.HierarchyClass
                .Where(hc => ids.Contains(hc.hierarchyClassID))
                .ToList();
        }

        private DataTable GetDataTableFromHierachyClassList(List<HierarchyClass> hierarchyClasses, int hierarchyId)
        {
            return hierarchyClasses
                .Where(hc => hc.hierarchyID == hierarchyId)
                .Select(hc => hc.hierarchyClassID)
                .ToList()
                .ConvertAll(i => new
                {
                    I = i
                })
                .ToDataTable();
        }

        private void RefreshHierarchyClasses(DataTable ids, string sql)
        {
            SqlParameter inputType = GetIntListFromIdsDataTable(ids);
            context.Database.ExecuteSqlCommand(sql, inputType);
        }

        private SqlParameter GetIntListFromIdsDataTable(DataTable ids)
        {
            return new SqlParameter("ids", SqlDbType.Structured)
            {
                TypeName = "app.IntList",
                Value = ids
            };
        }
    }
}
