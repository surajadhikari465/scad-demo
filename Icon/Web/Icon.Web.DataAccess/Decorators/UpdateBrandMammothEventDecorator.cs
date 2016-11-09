using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateBrandMammothEventDecorator : ICommandHandler<UpdateBrandCommand>
    {
        private readonly ICommandHandler<UpdateBrandCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public UpdateBrandMammothEventDecorator(
            ICommandHandler<UpdateBrandCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(UpdateBrandCommand command)
        {
            commandHandler.Execute(command);

            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            //Generate mammoth events
            if (enableMammothEventGeneration)
            {
                SqlParameter eventReferenceId = new SqlParameter("eventReferenceId", SqlDbType.Int);
                SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);
                SqlParameter eventMessage = new SqlParameter("eventMessage", SqlDbType.VarChar);

                eventReferenceId.Value = command.Brand.hierarchyClassID;
                eventTypeId.Value = MommothEventTypes.Hierarchyclassupdate;
                eventMessage.Value = HierarchyNames.Brands;

                string sql = "EXEC mammoth.InsertMammothEvent @eventTypeId, @eventReferenceId, @eventMessage";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, eventTypeId, eventReferenceId, eventMessage);
                    logger.Info(String.Format("Inserted 1 Mammoth Brand update event. HierarchyClassId = ", command.Brand.hierarchyClassID));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}