using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteHierarchyClassCommandHandler : ICommandHandler<DeleteHierarchyClassCommand>
    {
        private IconContext context;

        public DeleteHierarchyClassCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(DeleteHierarchyClassCommand data)
        {
            try
            {
                string eventName = string.Empty;

                var deletedHierarchyClass = context.HierarchyClass.Find(data.DeletedHierarchyClass.hierarchyClassID);

                if (deletedHierarchyClass.HierarchyClassTrait.Count > 0)
                {
                    context.HierarchyClassTrait.RemoveRange(deletedHierarchyClass.HierarchyClassTrait);
                }
                context.HierarchyClass.Remove(deletedHierarchyClass);
                context.SaveChanges();

                if (deletedHierarchyClass.hierarchyID == Hierarchies.Brands)
                    eventName = "Brand Delete";
                else if (deletedHierarchyClass.hierarchyID == Hierarchies.National)
                    eventName = "National Class Delete";

                if (!String.IsNullOrEmpty(eventName))
                {
                    GenerateHierarchyDeleteEvents(deletedHierarchyClass.hierarchyClassID, deletedHierarchyClass.hierarchyClassName, eventName);
                }

            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error deleting Hierarchy Class ID {0}.  Error: {1}",
                    data.DeletedHierarchyClass.hierarchyClassID, exception.Message), exception);
            }
        }

        private void GenerateHierarchyDeleteEvents(int hierarchyClassId, string hierarchyClassName, string eventName)
        {
            int hierarchyClassDeleteEventId;
            string[] hierarchyClassDeleteConfiguredRegions = ConfigurationManager.AppSettings["HierarchyClassDeleteEventConfiguredRegions"].Split(',');
            var hierarchyDeleteEvents = new List<EventQueue>();

            hierarchyClassDeleteEventId = context.EventType.Single(et => et.EventName == eventName).EventId;

            foreach (string region in hierarchyClassDeleteConfiguredRegions)
            {
                var hierarchyDeleteEvent = new EventQueue();

                hierarchyDeleteEvent.EventId = hierarchyClassDeleteEventId;
                hierarchyDeleteEvent.EventMessage = hierarchyClassName;
                hierarchyDeleteEvent.EventReferenceId = hierarchyClassId;
                hierarchyDeleteEvent.RegionCode = region.Trim();
                hierarchyDeleteEvent.InsertDate = DateTime.Now;

                hierarchyDeleteEvents.Add(hierarchyDeleteEvent);
            }

            context.EventQueue.AddRange(hierarchyDeleteEvents);
            context.SaveChanges();
        }
    }
}