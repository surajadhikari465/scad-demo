using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddBrandCommandHandler : ICommandHandler<AddBrandCommand>
    {
        private IconContext context;

        public AddBrandCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddBrandCommand data)
        {
            Validate(data);
            AddBrandAbbreviationIfNotNullOrEmpty(data.Brand, data.BrandAbbreviation);
            AddTrait(data.Brand, Traits.Designation, data.Designation);
            AddTrait(data.Brand, Traits.ParentCompany, data.ParentCompany);
            AddTrait(data.Brand, Traits.ZipCode, data.ZipCode);
            AddTrait(data.Brand, Traits.Locality, data.Locality);
            
            data.Brand.hierarchyID = Hierarchies.Brands;
            data.Brand.hierarchyParentClassID = null;
            data.Brand.hierarchyLevel = HierarchyLevels.Parent;           
            data.Brand.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.SentToEsb,
                    traitValue = null
                });

            context.HierarchyClass.Add(data.Brand);
            context.SaveChanges();
        }

        private void Validate(AddBrandCommand data)
        {
            if (context.HierarchyClass.Any(hc => hc.hierarchyID == Hierarchies.Brands && hc.hierarchyClassName.ToLower() == data.Brand.hierarchyClassName.ToLower()))
            {
                throw new DuplicateValueException(String.Format("The brand {0} already exists.", data.Brand.hierarchyClassName));
            }

            if (data.Brand.hierarchyClassName.Length >= Constants.IrmaBrandNameMaxLength)
            {
                string trimmedBrandName = data.Brand.hierarchyClassName.Substring(0, Constants.IrmaBrandNameMaxLength).ToLower();

                if (context.HierarchyClass.Any(hc => hc.hierarchyID == Hierarchies.Brands && hc.hierarchyClassName.Substring(0, Constants.IrmaBrandNameMaxLength).ToLower() == trimmedBrandName))
                {
                    throw new DuplicateValueException(String.Format("This brand trimmed to {0} characters {1} already exists. " +
                        "Change the brand name so that the first {0} characters are unique.", Constants.IrmaBrandNameMaxLength, data.Brand.hierarchyClassName.Substring(0, Constants.IrmaBrandNameMaxLength)));
                }
            }
        }

        private void AddBrandAbbreviationIfNotNullOrEmpty(HierarchyClass brand, string brandAbbreviation)
        {
            if (!String.IsNullOrWhiteSpace(brandAbbreviation))
            {
                if (context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue.ToLower() == brandAbbreviation.ToLower()))
                {
                    throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", brandAbbreviation));
                }

                AddTrait(brand, Traits.BrandAbbreviation, brandAbbreviation);
            }
        }

        private void AddTrait(HierarchyClass brand, int id, string traitValue)
        {
            if(!string.IsNullOrWhiteSpace(traitValue))
            {
                brand.HierarchyClassTrait.Add(new HierarchyClassTrait{ traitID = id, traitValue = traitValue.Trim() });
            }
        }
    }
}