using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateHierarchyClassCommandHandler : ICommandHandler<UpdateHierarchyClassCommand>
    {
        private IconContext context;

        public UpdateHierarchyClassCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateHierarchyClassCommand data)
        {
            HierarchyClass subteamHierachyClass = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == data.SubTeamHierarchyClassId);

            var duplicateHierarchyClasses = context.HierarchyClass.ContainsDuplicateName(data.UpdatedHierarchyClass.hierarchyClassName,
                data.UpdatedHierarchyClass.hierarchyLevel, data.UpdatedHierarchyClass.hierarchyID, data.UpdatedHierarchyClass.hierarchyClassID, subteamHierachyClass, data.UpdatedHierarchyClass.hierarchyParentClassID, true);

            if (duplicateHierarchyClasses)
            {
                throw new ArgumentException(String.Format("The name {0} already exists.", data.UpdatedHierarchyClass.hierarchyClassName));
            }

            HierarchyClass updatedHierarchyClass = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.UpdatedHierarchyClass.hierarchyClassID);
            string oldHierarchyName = updatedHierarchyClass.hierarchyClassName;
            updatedHierarchyClass.hierarchyClassName = data.UpdatedHierarchyClass.hierarchyClassName;
            data.ClassNameChanged = true;
            context.SaveChanges();
        }
    }
}
