using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddSubTeamEventsCommandHandler : ICommandHandler<AddSubTeamEventsCommand>
    {
        private IconContext context;

        public AddSubTeamEventsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddSubTeamEventsCommand data)
        {
            var hierarchyClass = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Include(hc => hc.Hierarchy)
                .SingleOrDefault(hc => hc.hierarchyClassID == data.HierarchyClassId);

            if (hierarchyClass == null)
            {
                throw new ArgumentException(String.Format("Unable to generate events for HierarchyClass. No HierarchyClass was found with HierarchyClassId {0}", data.HierarchyClassId));
            }

            var nonAlignedSubteamTrait = hierarchyClass.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.NonAlignedSubteam);

            if (nonAlignedSubteamTrait != null)
            {
                return;
            }

            //Only generate events Sub Teams that have a POS Dept Number 
            var posDeptNumberTrait = hierarchyClass.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.PosDepartmentNumber);

            if (posDeptNumberTrait == null || String.IsNullOrWhiteSpace(posDeptNumberTrait.traitValue))
            {
                return;
            }

            //Get regions that are configured to receive Sub Team Events
            var regions = data.PosSubTeamRegionalSettingsList.Where(rs => rs.Value == true).Select(a => a.RegionCode).ToList();

            if (regions.Any())
            {
                List<EventQueue> events = new List<EventQueue>();

                foreach (var region in regions)
                {
                    events.Add(new EventQueue
                    {
                        EventId = EventTypes.SubTeamUpdate,
                        EventMessage = hierarchyClass.hierarchyClassName,
                        RegionCode = region,
                        EventReferenceId = hierarchyClass.hierarchyClassID,
                        InsertDate = DateTime.Now
                    });
                }

                context.EventQueue.AddRange(events);
                context.SaveChanges();
            }
        }
    }
}
