using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
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
            if (!String.IsNullOrEmpty(brandAbbreviation))
            {
                if (context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue.ToLower() == brandAbbreviation.ToLower()))
                {
                    throw new DuplicateValueException(String.Format("The brand abbreviation {0} already exists.", brandAbbreviation));
                }

                brand.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.BrandAbbreviation,
                    traitValue = brandAbbreviation
                });
            }
        }
    }
}
