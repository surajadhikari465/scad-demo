using GlobalEventController.Common;
using SubteamEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.Controller.EventServices;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.Controller.EventServices
{
    public class BulkItemSubTeamEventService : IBulkItemSubTeamEventService
    {
        private readonly IrmaContext context;
        private ICommandHandler<BulkUpdateItemSubTeamCommand> bulkItemHandler;
        private ICommandHandler<BulkAddUpdateLastChangeSubTeamCommand> bulkLastChangeHandler;
        

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public List<ItemSubTeamModel> ItemSubTeamModelList { get; set; }

        public BulkItemSubTeamEventService(IrmaContext context,            
            ICommandHandler<BulkUpdateItemSubTeamCommand> bulkItemHandler,
            ICommandHandler<BulkAddUpdateLastChangeSubTeamCommand> bulkLastChangeHandler)
        {
            this.context = context;            
            this.bulkItemHandler = bulkItemHandler;
            this.bulkLastChangeHandler = bulkLastChangeHandler;
        }

        public void Run()
        {
            DbContextTransaction transaction = this.context.Database.BeginTransaction();
            using (this.context)
            {
                using (transaction)
                {
                    try
                    {
                        BulkAddUpdateLastChangeSubTeamCommand bulkLastChangeCommand = new BulkAddUpdateLastChangeSubTeamCommand { ItemSubTeamModels = ItemSubTeamModelList };
                        bulkLastChangeHandler.Handle(bulkLastChangeCommand);

                        BulkUpdateItemSubTeamCommand bulkItemCommand = new BulkUpdateItemSubTeamCommand { ItemSubTeamModels = ItemSubTeamModelList };
                        bulkItemHandler.Handle(bulkItemCommand);
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        if (transaction.UnderlyingTransaction != null && transaction.UnderlyingTransaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        throw;
                    }
                }
            }
        }
    }
}
