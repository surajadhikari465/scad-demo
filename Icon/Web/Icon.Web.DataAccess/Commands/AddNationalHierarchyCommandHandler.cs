using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Linq;
using System.Globalization;

namespace Icon.Web.DataAccess.Commands
{
    public class AddNationalHierarchyCommandHandler : ICommandHandler<AddNationalHierarchyCommand>
    {
        private IconContext context;

        public AddNationalHierarchyCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddNationalHierarchyCommand data)
        {           
            AddNationalClassTraitIfNotNullOrEmpty(data.NationalHierarchy, TraitCodes.NationalClassCode, data.NationalClassCode);
            AddNationalClassTraitIfNotNullOrEmpty(data.NationalHierarchy, TraitCodes.ModifiedUser, data.UserName);
            AddNationalClassTraitIfNotNullOrEmpty(data.NationalHierarchy, TraitCodes.ModifiedDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture));

            context.HierarchyClass.Add(data.NationalHierarchy);
            context.SaveChanges();
        }

        private void AddNationalClassTraitIfNotNullOrEmpty(HierarchyClass nationalClass, string traitCode, string traitValue)
        {
            if (!String.IsNullOrEmpty(traitValue))
            {

                HierarchyClassTrait addHierarchyClassTrait = new HierarchyClassTrait();
                addHierarchyClassTrait.traitID = context.Trait.Single(t => t.traitCode == traitCode).traitID;
                addHierarchyClassTrait.traitValue = traitValue;
                addHierarchyClassTrait.hierarchyClassID = nationalClass.hierarchyClassID;

                context.HierarchyClassTrait.Add(addHierarchyClassTrait);
            }
        }
       
    }
}
