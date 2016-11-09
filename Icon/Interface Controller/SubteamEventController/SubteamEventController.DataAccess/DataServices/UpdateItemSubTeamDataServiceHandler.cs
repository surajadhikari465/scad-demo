using GlobalEventController.Common;
using SubteamEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using GlobalEventController.DataAccess.DataServices;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.DataServices
{
    public class UpdateItemSubTeamDataServiceHandler : IDataService<UpdateItemSubTeamDataService>
    {
        private ICommandHandler<AddUpdateItemSubTeamLastChangeCommand> lastChangeHandler;
        private ICommandHandler<UpdateItemSubTeamCommand> itemHandler;
        private IQueryHandler<GetUserQuery, Users> getUserHandler;

        public UpdateItemSubTeamDataServiceHandler(
            ICommandHandler<AddUpdateItemSubTeamLastChangeCommand> lastChangeHandler,
            ICommandHandler<UpdateItemSubTeamCommand> itemHandler,            
            IQueryHandler<GetUserQuery, Users> getUserHandler)
        {
            this.lastChangeHandler = lastChangeHandler;
            this.itemHandler = itemHandler;           
            this.getUserHandler = getUserHandler;
        }
        public void Process(UpdateItemSubTeamDataService service)
        {
            bool hasDefaultIdentifier = service.ItemIdentifiers.Any(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0);

            if (hasDefaultIdentifier)
            {
                UpdateItemSubTeamCommand updateItem = service.ItemCommand;
                service.ItemCommand.UserId = GetUserId(service.Region);
                this.itemHandler.Handle(updateItem);

                AddUpdateItemSubTeamLastChangeCommand lastChange = service.LastChangeCommand;
                this.lastChangeHandler.Handle(lastChange);

            }           
        }

        private int GetUserId(string region)
        {
            int userId = Cache.InterfaceControllerUserId[region];
            if (userId == -1)
            {
                Users user = getUserHandler.Handle(new GetUserQuery { UserName = Cache.InterfaceControllerUserName });

                if (user == null)
                {
                    throw new NullReferenceException(String.Format("The User: {0} was not found for region: {1}.", Cache.InterfaceControllerUserName, region));
                }

                Cache.InterfaceControllerUserId[region] = user.User_ID;
                userId = user.User_ID;
            }
            return userId;
        }
    }
}
