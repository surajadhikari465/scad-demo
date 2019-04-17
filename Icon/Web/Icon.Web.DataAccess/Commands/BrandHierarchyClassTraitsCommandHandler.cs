using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class BrandHierarchyClassTraitsCommandHandler : ICommandHandler<UpdateBrandHierarchyClassTraitsCommand>
    {
        private IconContext context;

        public BrandHierarchyClassTraitsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateBrandHierarchyClassTraitsCommand data)
        {
            string traitValue;
            var brand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Brand.hierarchyClassID);
            if(brand == null) return;

            HierarchyClassTrait existingTrait;
            var allTraits = brand.HierarchyClassTrait.Where(x => x.traitID != Traits.SentToEsb).ToArray();
            
            foreach(var code in new int[]{ Traits.BrandAbbreviation,  Traits.Designation, Traits.ParentCompany, Traits.ZipCode, Traits.Locality })
            {
                existingTrait = allTraits.SingleOrDefault(x => x.traitID == code);

                switch(code)
                {
                    case Traits.BrandAbbreviation:
                        traitValue = data.BrandAbbreviation;
                        break;
                    case Traits.Designation:
                        traitValue = data.Designation;
                        break;
                    case Traits.ParentCompany:
                        traitValue = data.ParentCompany;
                        break;
                    case Traits.ZipCode:
                        traitValue = data.ZipCode;
                        break;
                    case Traits.Locality:
                        traitValue = data.Locality;
                        break;
                    default:
                        traitValue = null;
                        break;
                }

                if(existingTrait == null && traitValue == null) continue;

                if(existingTrait == null)
                {
                    brand.AddHierarchyClassTrait(context, code, traitValue);
                }
                else
                {
                    existingTrait.UpdateHierarchyClassTrait(context, traitValue, removeIfNullOrEmpty: true, saveChanges: false);
                }
            }

            context.SaveChanges();
        }
    }
}