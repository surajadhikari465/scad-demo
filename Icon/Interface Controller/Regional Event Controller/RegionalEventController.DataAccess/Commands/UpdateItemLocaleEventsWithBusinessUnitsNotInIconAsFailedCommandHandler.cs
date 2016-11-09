using Icon.Framework;
using RegionalEventController.DataAccess.Infrastructure;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionalEventController.DataAccess.Commands
{
    public class UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommandHandler : ICommandHandler<UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommand>
    {
        private IIconContextManager contextManager;

        public UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommandHandler(IIconContextManager contextManager)
        {
            this.contextManager = contextManager;
        }

        public void Execute(UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommand command)
        {
            var businessUnits = contextManager.Context.LocaleTrait.Where(lt => lt.traitID == Traits.PsBusinessUnitId).ToList();

            foreach (var itemLocaleEvent in command.ItemLocaleEvents.Where(il => String.IsNullOrWhiteSpace(il.OutputError)))
            {
                if(!businessUnits.Any(bu => bu.traitValue == itemLocaleEvent.BusinessUnit.ToString()))
                {
                    itemLocaleEvent.OutputError = String.Format("Invalid entry. Business Unit {0} does not exist in Icon.", itemLocaleEvent.BusinessUnit);
                }
            }
        }
    }
}
