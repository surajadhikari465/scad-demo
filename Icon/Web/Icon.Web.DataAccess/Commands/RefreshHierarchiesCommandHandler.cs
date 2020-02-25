using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
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
        private AppSettings settings;

        public RefreshHierarchiesCommandHandler(IconContext context, AppSettings settings)
        {
            this.context = context;
            this.settings = settings;
        }

        public void Execute(RefreshHierarchiesCommand data)
        {
            var hierarchyClasses = GetListOfHiearchiesFromIds(data.HierarchyClassIds);

            var brands = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Brands);
            var nationals = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.National);
            var merchandises = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Merchandise);
            var manufacturer = GetDataTableFromHierachyClassList(hierarchyClasses, Hierarchies.Manufacturer);

            var regions = settings.HierarchyClassRefreshEventConfiguredRegions.ToList();

            if (brands.Rows.Count > 0) RefreshHierarchyClasses(brands, regions, "EXEC app.RefreshBrands @ids, @regions");
            if (nationals.Rows.Count > 0) RefreshHierarchyClasses(nationals, regions, "EXEC app.RefreshNationalHierarchyClasses @ids, @regions");
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
            SqlParameter hierarchyClassIds = GetIntListFromIdsDataTable(ids);
            context.Database.ExecuteSqlCommand(sql, hierarchyClassIds);
        }

        private void RefreshHierarchyClasses(DataTable ids, List<string> regions, string sql)
        {
            SqlParameter hierarchyClassIds = GetIntListFromIdsDataTable(ids);
            SqlParameter regionsParameter = new SqlParameter("regions", SqlDbType.Structured)
            {
                TypeName = "app.RegionAbbrType",
                Value = regions
                    .ConvertAll(r => new { RegionAbbr = r })
                    .ToDataTable()
            };
            context.Database.ExecuteSqlCommand(sql, hierarchyClassIds, regionsParameter);
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
