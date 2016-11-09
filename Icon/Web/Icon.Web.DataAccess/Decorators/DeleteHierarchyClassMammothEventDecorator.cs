using Icon.Framework;
using Icon.Web.DataAccess.Commands;
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
using Icon.Web.Common;
using Icon.Logging;
using System.Configuration;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Decorators
{
    public class DeleteHierarchyClassMammothEventDecorator : ICommandHandler<DeleteHierarchyClassCommand>
    {
        private readonly ICommandHandler<DeleteHierarchyClassCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public DeleteHierarchyClassMammothEventDecorator(
            ICommandHandler<DeleteHierarchyClassCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(DeleteHierarchyClassCommand command)
        {
            commandHandler.Execute(command);

            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            //Generate mammoth events
            if (enableMammothEventGeneration && (command.DeletedHierarchyClass.hierarchyID == Hierarchies.Merchandise 
                                                || command.DeletedHierarchyClass.hierarchyID == Hierarchies.Tax
                                                || command.DeletedHierarchyClass.hierarchyID == Hierarchies.Brands))
            {
                SqlParameter eventReferenceId = new SqlParameter("eventReferenceId", SqlDbType.Int);
                SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);
                SqlParameter eventMessage = new SqlParameter("eventMessage", SqlDbType.VarChar);

                eventReferenceId.Value = command.DeletedHierarchyClass.hierarchyClassID;
                eventTypeId.Value = MommothEventTypes.Hierarchyclassdelete;
                if (command.DeletedHierarchyClass.hierarchyID == Hierarchies.Merchandise)
                    eventMessage.Value = HierarchyNames.Merchandise;
                else if (command.DeletedHierarchyClass.hierarchyID == Hierarchies.Tax)
                    eventMessage.Value = HierarchyNames.Tax;
                else if (command.DeletedHierarchyClass.hierarchyID == Hierarchies.Brands)
                    eventMessage.Value = HierarchyNames.Brands;

                string sql = "EXEC mammoth.InsertMammothEvent @eventTypeId, @eventReferenceId, @eventMessage";

                try
                {
                    context.Database.ExecuteSqlCommand(sql, eventTypeId, eventReferenceId, eventMessage);
                    logger.Info(String.Format("Inserted 1 Mammoth Hierarchy Class Delete event. HierarchyClassId = ", command.DeletedHierarchyClass.hierarchyClassID));
                }
                catch (Exception ex)
                {
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}
