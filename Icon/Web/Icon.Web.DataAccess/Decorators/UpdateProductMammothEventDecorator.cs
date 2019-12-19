using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateProductMammothEventDecorator : ICommandHandler<BulkImportCommand<BulkImportItemModel>>
    {
        private readonly ICommandHandler<BulkImportCommand<BulkImportItemModel>> commandHandler;
        private IconContext context;
        private ILogger logger;

        public UpdateProductMammothEventDecorator(
            ICommandHandler<BulkImportCommand<BulkImportItemModel>> commandHandler, 
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportItemModel> command)
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
                SqlParameter eventMessageList = new SqlParameter("eventMessageList", SqlDbType.Structured);
                eventMessageList.TypeName = "mammoth.EventMessageType";
                SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);

                var eventMessageListData = command.BulkImportData.ConvertAll(item => new
                {
                    EventMessage = item.ScanCode.TrimStart('0'),
                });

                eventMessageList.Value = eventMessageListData.ToDataTable();
                eventTypeId.Value = MommothEventTypes.Productupdate;

                string sql = "EXEC mammoth.BulkInsertProductMammothEvents @eventMessageList, @eventTypeId";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, eventMessageList, eventTypeId);
                    logger.Info(String.Format("Inserted {0} Mammoth Product Update event(s).", command.BulkImportData.Count));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}
