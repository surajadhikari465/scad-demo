using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class DeleteHierarchyClassesCommandHandler : ICommandHandler<DeleteHierarchyClassesCommand>
    {
        private IRenewableContext<IconContext> context;

        public DeleteHierarchyClassesCommandHandler(
            IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(DeleteHierarchyClassesCommand data)
        {
            string hierarchyName = data.HierarchyClasses.First().HierarchyName;

            List<int> hierarchyClassIds = null;

            hierarchyClassIds = data.HierarchyClasses
                .Select(hc => hc.HierarchyClassId)
                .ToList();

            var hierarchyClassesToRemove = context.Context.HierarchyClass
                .Where(hc => hierarchyClassIds.Contains(hc.hierarchyClassID))
                .ToList();
            var itemHierarchyClassesToRemove = context.Context.ItemHierarchyClass
                .Where(ihc => hierarchyClassIds.Contains(ihc.hierarchyClassID))
                .ToList();
            var hierarchyClassesAssociatedToItems = hierarchyClassesToRemove.Where(hc => hc.ItemHierarchyClass.Any()).ToList();

            if (hierarchyClassesToRemove.Except(hierarchyClassesAssociatedToItems).Any())
            {
                context.Context.HierarchyClassTrait.RemoveRange(hierarchyClassesToRemove.SelectMany(hc => hc.HierarchyClassTrait));
                context.Context.HierarchyClass.RemoveRange(hierarchyClassesToRemove.Except(hierarchyClassesAssociatedToItems));
                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var hierarchyClassToRemoveIds = hierarchyClassesToRemove.Except(hierarchyClassesAssociatedToItems).Select(hc => hc.hierarchyClassID);

                    string errorDetails = ApplicationErrors.Descriptions.DeleteHierarchyClassError + " Exception: " + ex.ToString();
                    foreach (var hierarchyClass in data.HierarchyClasses.Where(hc => hierarchyClassToRemoveIds.Contains(hc.HierarchyClassId)))
                    {
                        hierarchyClass.ErrorCode = ApplicationErrors.Codes.DeleteHierarchyClassError;
                        hierarchyClass.ErrorDetails = errorDetails;
                    }
                }
            }
            if (hierarchyClassesAssociatedToItems.Any())
            {
                var hierarchyClassesAssociatedToItemsIds = hierarchyClassesAssociatedToItems.Select(hc => hc.hierarchyClassID);
                
                foreach (var hierarchyClass in data.HierarchyClasses.Where(hc => hierarchyClassesAssociatedToItemsIds.Contains(hc.HierarchyClassId)).ToList())
                {
                    hierarchyClass.ErrorCode = ApplicationErrors.Codes.HierarchyClassAssociatedToItemsOnDelete;
                    hierarchyClass.ErrorDetails = ApplicationErrors.Descriptions.HierarchyClassAssociatedToItemsOnDelete;
                }
            }
        }
    }
}
