using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkGetValidatedItemsQueryHandler : IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>
    {
        private Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory;
        private GlobalControllerSettings settings;

        public BulkGetValidatedItemsQueryHandler(Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory, GlobalControllerSettings settings)
        {
            this.contextFactory = contextFactory;
            this.settings = settings;
        }

        public List<ValidatedItemModel> Handle(BulkGetValidatedItemsQuery parameters)
        {
            SqlParameter scanCodes = new SqlParameter("ScanCodes", SqlDbType.Structured);
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ScanCode");
            foreach (var e in parameters.Events)
            {
                dataTable.NewRow();
                dataTable.Rows.Add(e.EventMessage);
            }

            scanCodes.Value = dataTable;
            scanCodes.TypeName = "app.ScanCodeListType";

            string sql = @"EXEC infor.GetValidatedItemModel @ScanCodes";

            using (var context = contextFactory.CreateContext())
            {
                var validatedItems = context.Database.SqlQuery<ValidatedItemModel>(sql, scanCodes).ToList();

                // Set EventTypeId
                foreach (var item in validatedItems)
                {
                    item.EventTypeId = parameters.Events.First(e => e.EventMessage == item.ScanCode).EventId;
                }

                return validatedItems;
            }
        }
    }
}
