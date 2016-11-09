using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class AddVimEventCommandHandler : ICommandHandler<AddVimEventCommand>
    {
        private IconContext context;

        public AddVimEventCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddVimEventCommand data)
        {
            context.VimEventQueue.Add(new VimEventQueue
            {
                EventReferenceId = data.EventReferenceId,
                EventTypeId = data.EventTypeId,
                EventMessage = data.EventMessage,
                InsertDate = DateTime.Now
            });
            context.SaveChanges();
        }
    }
}