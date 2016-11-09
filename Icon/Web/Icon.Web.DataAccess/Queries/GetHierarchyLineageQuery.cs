using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyLineageQuery : IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>
    {
        private readonly IconContext context;

        public GetHierarchyLineageQuery(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClassListModel Search(GetHierarchyLineageParameters parameters)
        {
            string sql = @"app.GetAllHierarchyLineage @includeBrowsingHierarchy, @includeNationalHierarchy, @includeFinancialHierarchy";

            var command = this.context.Database.Connection.CreateCommand();
            command.CommandText = sql;

            SqlParameter includeBrowsingHierarchy = new SqlParameter("includeBrowsingHierarchy", SqlDbType.Bit);
            includeBrowsingHierarchy.Value = true;
            command.Parameters.Add(includeBrowsingHierarchy);

            SqlParameter includeNationalClassHierarchy = new SqlParameter("includeNationalHierarchy", SqlDbType.Bit);
            includeNationalClassHierarchy.Value = true;
            command.Parameters.Add(includeNationalClassHierarchy);

            SqlParameter includeFinancialHierarchy = new SqlParameter("includeFinancialHierarchy", SqlDbType.Bit);
            includeFinancialHierarchy.Value = true;
            command.Parameters.Add(includeFinancialHierarchy);

            HierarchyClassListModel hierarchyListModel = new Models.HierarchyClassListModel();

            try
            {
                this.context.Database.Connection.Open();
                var reader = command.ExecuteReader();

                var objectContext = ((IObjectContextAdapter)this.context).ObjectContext;

                hierarchyListModel.BrandHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();

                reader.NextResult();
                hierarchyListModel.TaxHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();

                reader.NextResult();
                hierarchyListModel.MerchandiseHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();

                reader.NextResult();
                hierarchyListModel.BrowsingHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();

                reader.NextResult();
                hierarchyListModel.NationalHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();

                reader.NextResult();
                hierarchyListModel.FinancialHierarchyList = objectContext.Translate<HierarchyClassModel>(reader).ToList();
            }
            finally
            {
                this.context.Database.Connection.Close();
            }

            return hierarchyListModel;
        }
    }
}

