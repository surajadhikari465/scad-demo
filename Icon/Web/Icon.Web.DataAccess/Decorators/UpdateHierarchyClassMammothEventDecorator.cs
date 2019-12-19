using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using System;
using System.Data;
using System.Data.SqlClient;
using Icon.Web.Common;
using Icon.Logging;
using System.Configuration;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateHierarchyClassMammothEventDecorator : ICommandHandler<UpdateHierarchyClassCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public UpdateHierarchyClassMammothEventDecorator(
            ICommandHandler<UpdateHierarchyClassCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(UpdateHierarchyClassCommand command)
        {
            commandHandler.Execute(command);

            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            //Only generate mammoth events for Merchandise Hierarchy adds
            if (enableMammothEventGeneration && command.UpdatedHierarchyClass.hierarchyID == Hierarchies.Merchandise)
            {
                SqlParameter eventReferenceId = new SqlParameter("eventReferenceId", SqlDbType.Int);
                SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);
                SqlParameter eventMessage = new SqlParameter("eventMessage", SqlDbType.VarChar);

                eventReferenceId.Value = command.UpdatedHierarchyClass.hierarchyClassID;
                eventTypeId.Value = MommothEventTypes.Hierarchyclassupdate;
                eventMessage.Value = HierarchyNames.Merchandise;

                string sql = "EXEC mammoth.InsertMammothEvent @eventTypeId, @eventReferenceId, @eventMessage";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, eventTypeId, eventReferenceId, eventMessage);
                    logger.Info(String.Format("Inserted 1 Mammoth Hierarchy Class update event. HierarchyClassId = ", command.UpdatedHierarchyClass.hierarchyClassID));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}

