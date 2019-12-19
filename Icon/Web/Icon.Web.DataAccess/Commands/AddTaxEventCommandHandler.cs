using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Configuration;

namespace Icon.Web.DataAccess.Commands
{
    public class AddTaxEventCommandHandler : ICommandHandler<AddTaxEventCommand>
    {
        private IconContext context;

        public AddTaxEventCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddTaxEventCommand data)
        {
            if (String.IsNullOrEmpty(data.TaxAbbreviation))
            {
                return;
            }

            // Get tax class that we're working with
            HierarchyClass tax = context.HierarchyClass.Find(data.HierarchyClassId);

            // Create a row for each region setup for this event in the Configuration
            string[] regions = ConfigurationManager.AppSettings["TaxUpdateEventConfiguredRegions"].Split(',');

            foreach (string region in regions)
            {
                EventQueue eventQueue = new EventQueue();
                eventQueue.EventId = EventTypes.TaxNameUpdate;
                eventQueue.EventMessage = tax.hierarchyClassName.Split()[0];
                eventQueue.EventReferenceId = data.HierarchyClassId;
                eventQueue.RegionCode = region.Trim();
                eventQueue.InsertDate = DateTime.Now;

                context.EventQueue.Add(eventQueue);
            }

            context.SaveChanges();
        }
    }
}
