using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SubteamEventController.DataAccess.Commands
{
    public class UpdateItemSubTeamCommandHandler : ICommandHandler<UpdateItemSubTeamCommand>
    {
        private readonly IrmaContext context;

        public UpdateItemSubTeamCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(UpdateItemSubTeamCommand command)
        {
            Item item = this.context.Item
                .Include(i => i.ItemIdentifier)
                .SingleOrDefault(i => i.ItemIdentifier
                    .Any(ii => ii.Identifier == command.UpdatedItemSubTeam.ScanCode && ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1));

            item.SubTeam_No = command.UpdatedItemSubTeam.SubTeamNo;
            item.Category_ID = command.Category_ID;
            item.LastModifiedUser_ID = command.UserId;
            item.LastModifiedDate = DateTime.Now;
        }
    }
}
