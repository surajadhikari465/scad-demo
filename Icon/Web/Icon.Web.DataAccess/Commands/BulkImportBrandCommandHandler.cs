using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Web.DataAccess.Extensions;
using Icon.Logging;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkImportBrandCommandHandler : ICommandHandler<BulkImportCommand<BulkImportBrandModel>>
    {
        private IconContext context;
        private ILogger logger;

        public BulkImportBrandCommandHandler(IconContext context,
            ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportBrandModel> data)
        {
            SqlParameter brands = new SqlParameter("brands", SqlDbType.Structured);
            brands.TypeName = "app.BrandImportType";

            brands.Value = data.BulkImportData.ConvertAll(b => new 
                {
                    BrandId = b.BrandId,
                    BrandName = b.BrandName,
                    BrandAbbreviation = b.BrandAbbreviation
                }).ToDataTable();

            string sql = "EXEC app.AddOrUpdateBrands @brands";

            context.Database.ExecuteSqlCommand(sql, brands);
            logger.Info(String.Format("User {0} imported {1} brand(s) through bulk brand import.", data.UserName, data.BulkImportData.Count));
        }
    }
}
