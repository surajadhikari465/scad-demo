using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Decorators
{
    public class BulkImportBrandMammothEventDecorator : ICommandHandler<BulkImportCommand<BulkImportBrandModel>>
    {
        private readonly ICommandHandler<BulkImportCommand<BulkImportBrandModel>> commandHandler;
        private IconContext context;
        private ILogger logger;

        public BulkImportBrandMammothEventDecorator(
            ICommandHandler<BulkImportCommand<BulkImportBrandModel>> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportBrandModel> command)
        {
            commandHandler.Execute(command);

            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            if (enableMammothEventGeneration)
            {
                //Generate events
                SqlParameter hierarchyClassList = new SqlParameter("hierarchyClassList", SqlDbType.Structured);
                hierarchyClassList.TypeName = "mammoth.BulkImportedHierarchyClassType";
                SqlParameter eventMessage = new SqlParameter("eventMessage", SqlDbType.VarChar);
                SqlParameter hierarchyID = new SqlParameter("hierarchyID", SqlDbType.Int);
                SqlParameter hierarchyLevel = new SqlParameter("hierarchyLevel", SqlDbType.Int);

                var hierarchyClassListData = command.BulkImportData.ConvertAll(brand => new
                {
                    HierarchyClassId = brand.BrandId,
                    HierarchyClassName = brand.BrandName,
                    MammothEventTypeId = String.IsNullOrEmpty(brand.BrandId) || int.Parse(brand.BrandId) == 0 ? MommothEventTypes.Hierarchyclassadd : MommothEventTypes.Hierarchyclassupdate
                });

                hierarchyClassList.Value = hierarchyClassListData.ToDataTable();
                eventMessage.Value = HierarchyNames.Brands;
                hierarchyID.Value = Hierarchies.Brands;
                hierarchyLevel.Value = 1;

                string sql = "EXEC mammoth.BulkImportHierarchyClassMammothEvents @hierarchyClassList, @hierarchyID, @hierarchyLevel, @eventMessage";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, hierarchyClassList, hierarchyID, hierarchyLevel, eventMessage);
                    logger.Info(String.Format("Inserted {0} Mammoth Brand Add event(s).", command.BulkImportData.Count));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}
