using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateNationalHierarchyCommandHandler : ICommandHandler<UpdateNationalHierarchyCommand>
    {
        private IconContext context;

        public UpdateNationalHierarchyCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateNationalHierarchyCommand data)
        {
            var nationalClassToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.NationalHierarchy.hierarchyClassID);
            
            UpdateNationalHierarchyName(nationalClassToUpdate, data.NationalHierarchy.hierarchyClassName);
        }

        private void UpdateNationalHierarchyName(HierarchyClass nationalClassToUpdate, string updatedNationalHierarchyName)
        {
           nationalClassToUpdate.hierarchyClassName = updatedNationalHierarchyName;

            context.SaveChanges();
        }
    }
}
