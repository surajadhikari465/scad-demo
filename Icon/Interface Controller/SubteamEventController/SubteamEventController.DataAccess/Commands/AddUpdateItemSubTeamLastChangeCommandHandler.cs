using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.Commands
{
    public class AddUpdateItemSubTeamLastChangeCommandHandler : ICommandHandler<AddUpdateItemSubTeamLastChangeCommand>
    {
        private IrmaContext context;

        public AddUpdateItemSubTeamLastChangeCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(AddUpdateItemSubTeamLastChangeCommand command)
        {
            IconItemLastChange lastChange = this.context.IconItemLastChange.SingleOrDefault(lc => lc.Identifier == command.UpdatedItem.ScanCode);

            // Only Update or Add if something has changed.
            if (lastChange != null)
            {
                var entry = this.context.Entry(lastChange);

                lastChange.Subteam_No = command.UpdatedItem.SubTeamNo;
                lastChange.InsertDate = DateTime.Now;

                entry.State = EntityState.Modified;
            }
            else
            {
                // Add new version if it doesn't exist already.
                lastChange = new IconItemLastChange
                {
                    Identifier = command.UpdatedItem.ScanCode,
                    Subteam_No = command.UpdatedItem.SubTeamNo,
                    InsertDate = DateTime.Now
                };

                this.context.IconItemLastChange.Add(lastChange);
            }
        }
    }
}
