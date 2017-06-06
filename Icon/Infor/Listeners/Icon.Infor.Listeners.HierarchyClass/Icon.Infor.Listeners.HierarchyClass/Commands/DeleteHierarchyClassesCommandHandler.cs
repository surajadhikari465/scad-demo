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

        public DeleteHierarchyClassesCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(DeleteHierarchyClassesCommand data)
        {
            // get name from first item - will tell you type of hierarchy class (brand, financial, national, merch...)
            string hierarchyName = data.HierarchyClasses.First().HierarchyName;

            //get ids for non-financial
            var idsToRemove = data.HierarchyClasses.Where(model => model.HierarchyName != HierarchyNames.Financial)
                .Select(model => model.HierarchyClassId)
                .ToList();
            // if data has financial hierarchy then HierarchyClassId will have 4 digit number (value for Trait code Fin)
            var financialIdsToDelete = data.HierarchyClasses.Where(model => model.HierarchyName == HierarchyNames.Financial)
                .Select(model => model.HierarchyClassId.ToString())
                .ToList();
         
            // ids in fin message are not db ids, so have to translate (for other ones it is the db id)
            if (financialIdsToDelete.Count > 0)
            {    // add hierarchyClassIds to the list. for the 4 digit number we got, compare it to 4 digit number in hierarchyClassName
                idsToRemove.AddRange(context.Context.HierarchyClass
                    .Where(hc => hc.hierarchyID == Hierarchies.Financial
                        && financialIdsToDelete.Contains(hc.hierarchyClassName.Substring(hc.hierarchyClassName.Length - 5, 4)))
                    .Select(hc1 => hc1.hierarchyClassID));
            }

            var hierarchyClassesToRemove = context.Context.HierarchyClass
                .Where(hc => idsToRemove.Contains(hc.hierarchyClassID))
                .ToList();
            // check if there are still items associated with any of the classes to be removed
            var hierarchyClassesAssociatedToItems = hierarchyClassesToRemove
                .Where(hc => hc.ItemHierarchyClass.Any()).ToList();
            // check whether matching records could not be found for any of the requested data
            var idsRequestedToRemoveButNotFound = data.HierarchyClasses
                .Select(model => model.HierarchyClassId)
                .Where(modelIds => !hierarchyClassesToRemove
                    .Select(matchingClass => matchingClass.hierarchyClassID).Contains(modelIds));

            if (hierarchyClassesToRemove.Except(hierarchyClassesAssociatedToItems).Any())
            {
                context.Context.HierarchyClassTrait
                    .RemoveRange(hierarchyClassesToRemove.SelectMany(hc => hc.HierarchyClassTrait));
                context.Context.HierarchyClass
                    .RemoveRange(hierarchyClassesToRemove.Except(hierarchyClassesAssociatedToItems));

                try
                {
                    context.Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var hierarchyClassToRemoveIds = hierarchyClassesToRemove
                        .Except(hierarchyClassesAssociatedToItems)
                        .Select(hc => hc.hierarchyClassID);

                    string errorDetails = ApplicationErrors.Descriptions.DeleteHierarchyClassError + " Exception: " + ex.ToString();
                    foreach (var hierarchyClass in data.HierarchyClasses
                        .Where(hc => hierarchyClassToRemoveIds.Contains(hc.HierarchyClassId)))
                    {
                        hierarchyClass.ErrorCode = ApplicationErrors.Codes.DeleteHierarchyClassError;
                        hierarchyClass.ErrorDetails = errorDetails;
                    }
                }
            }
            if (hierarchyClassesAssociatedToItems.Any())
            {
                var hierarchyClassesAssociatedToItemsIds = hierarchyClassesAssociatedToItems.Select(hc => hc.hierarchyClassID);
                
                foreach (var hierarchyClass in data.HierarchyClasses
                    .Where(hc => hierarchyClassesAssociatedToItemsIds.Contains(hc.HierarchyClassId)).ToList())
                {
                    hierarchyClass.ErrorCode = ApplicationErrors.Codes.HierarchyClassAssociatedToItemsOnDelete;
                    hierarchyClass.ErrorDetails = ApplicationErrors.Descriptions.HierarchyClassAssociatedToItemsOnDelete;
                }
            }
            if (idsRequestedToRemoveButNotFound.Any())
            {
                // asked to delete but found no matching records
                foreach (var hierarchyClass in data.HierarchyClasses
                    .Where(hc => idsRequestedToRemoveButNotFound.Contains(hc.HierarchyClassId)))
                {
                    hierarchyClass.ErrorCode = ApplicationErrors.Codes.UnableToFindMatchingHierarchyClass;
                    hierarchyClass.ErrorDetails = ApplicationErrors.Descriptions.UnableToFindMatchingHierarchyClassToDeleteMessage;
                }
            }
        }
    }
}
