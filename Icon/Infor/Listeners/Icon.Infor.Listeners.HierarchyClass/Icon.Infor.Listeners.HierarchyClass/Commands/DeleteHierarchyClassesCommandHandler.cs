using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using System;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class DeleteHierarchyClassesCommandHandler : ICommandHandler<DeleteHierarchyClassesCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public DeleteHierarchyClassesCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
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

            using (var context = contextFactory.CreateContext())
            {
                // ids in fin message are not db ids, so have to translate (for other ones it is the db id)
                if (financialIdsToDelete.Count > 0)
                {
                    // add hierarchyClassIds to the list. for the 4 digit number we got, compare it to 4 digit number in hierarchyClassName
                    idsToRemove.AddRange(context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.Financial
                            && financialIdsToDelete.Contains(hc.hierarchyClassName.Substring(hc.hierarchyClassName.Length - 5, 4)))
                        .Select(hc1 => hc1.hierarchyClassID));
                }

                var hierarchyClassesToRemove = context.HierarchyClass
                    .Where(hc => idsToRemove.Contains(hc.hierarchyClassID))
                    .ToList();

                context.HierarchyClassTrait
                        .RemoveRange(hierarchyClassesToRemove.SelectMany(hc => hc.HierarchyClassTrait));
                context.HierarchyClass
                    .RemoveRange(hierarchyClassesToRemove);

                context.SaveChanges();
            }
        }
    }
}
