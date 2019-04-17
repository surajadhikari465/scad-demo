using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Decorators
{
    public class AddBrandMammothEventDecorator : ICommandHandler<AddBrandCommand>
    {
        private readonly ICommandHandler<AddBrandCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public AddBrandMammothEventDecorator(
            ICommandHandler<AddBrandCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(AddBrandCommand command)
        {
            commandHandler.Execute(command);

            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            //Only generate mammoth events for Merchandise Hierarchy adds
            if (enableMammothEventGeneration)
            {
                SqlParameter eventReferenceId = new SqlParameter("eventReferenceId", SqlDbType.Int);
                SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);
                SqlParameter eventMessage = new SqlParameter("eventMessage", SqlDbType.VarChar);

                eventReferenceId.Value = command.Brand.hierarchyClassID;
                eventTypeId.Value = MommothEventTypes.Hierarchyclassadd;
                eventMessage.Value = HierarchyNames.Brands;

                string sql = "EXEC mammoth.InsertMammothEvent @eventTypeId, @eventReferenceId, @eventMessage";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, eventTypeId, eventReferenceId, eventMessage);
                    logger.Info(String.Format("Inserted 1 Mammoth Brand Add event. HierarchyClassId = ", command.Brand.hierarchyClassID));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}