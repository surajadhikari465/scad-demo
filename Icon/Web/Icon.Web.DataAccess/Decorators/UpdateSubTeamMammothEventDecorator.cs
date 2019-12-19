using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateSubTeamMammothEventDecorator : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public UpdateSubTeamMammothEventDecorator(
            ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler, 
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(UpdateHierarchyClassTraitCommand command)
        {   
            commandHandler.Execute(command);
            
            bool enableMammothEventGeneration;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableMammothEventGeneration"].ToString(), out enableMammothEventGeneration))
            {
                enableMammothEventGeneration = false;
            }

            if (enableMammothEventGeneration)
            {
                if (command.SubteamChanged)
                {
                    List<string> scanCodes = (from sc in context.ScanCode
                                              join i in context.Item on sc.itemID equals i.ItemId
                                              join ih in context.ItemHierarchyClass on i.ItemId equals ih.itemID
                                              where (ih.hierarchyClassID == command.UpdatedHierarchyClass.hierarchyClassID)
                                              select sc.scanCode).ToList();

                    SqlParameter eventMessageList = new SqlParameter("eventMessageList", SqlDbType.Structured);
                    eventMessageList.TypeName = "mammoth.EventMessageType";
                    SqlParameter eventTypeId = new SqlParameter("eventTypeId", SqlDbType.Int);

                    var eventMessageListData = scanCodes.ConvertAll(item => new
                    {
                        EventMessage = item.TrimStart('0'),
                    });

                    eventMessageList.Value = eventMessageListData.ToDataTable();
                    eventTypeId.Value = MommothEventTypes.Productupdate;

                    string sql = "EXEC mammoth.BulkInsertProductMammothEvents @eventMessageList, @eventTypeId";

                    try
                    {
                        context.Database.ExecuteSqlCommand(sql, eventMessageList, eventTypeId);
                        logger.Info(String.Format("Inserted {0} Mammoth Product Update event(s).", scanCodes.Count));
                    }
                    catch (Exception ex)
                    {
                        throw new CommandException("The CommandHandler threw an exception.", ex);
                    }
                }
            }
        }
    }
}
