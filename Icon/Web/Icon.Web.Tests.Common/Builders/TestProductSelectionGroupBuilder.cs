using Icon.Framework;
using System.Linq;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestProductSelectionGroupBuilder
    {
        private int productSelectionGroupId;
        private string productSelectionGroupName;
        private int productSelectionGroupTypeId;
        private int traitId;
        private string traitValue;

        private ProductSelectionGroupType productSelectionGroupType;
        private Trait trait;
        private IconContext context;
        private bool includeNavigationPropertiesFromContext;

        internal TestProductSelectionGroupBuilder()
        {
            productSelectionGroupId = 1;
            productSelectionGroupName = "Test PSG";
            productSelectionGroupId = 1;
            traitId = 1;
            traitValue = "Test Trait Value";
        }

        internal TestProductSelectionGroupBuilder WithPsgId(int productSelectionGroupId)
        {
            this.productSelectionGroupId = productSelectionGroupId;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithPsgName(string productSelectionGroupName)
        {
            this.productSelectionGroupName = productSelectionGroupName;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithPsgTypeId(int productSelectionGroupTypeId)
        {
            this.productSelectionGroupTypeId = productSelectionGroupTypeId;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithTraitId(int traitId)
        {
            this.traitId = traitId;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithTraitValue(string traitValue)
        {
            this.traitValue = traitValue;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithTrait(Trait trait)
        {
            this.trait = trait;
            return this;
        }

        internal TestProductSelectionGroupBuilder WithPsgType(ProductSelectionGroupType productSelectionGroupType)
        {
            this.productSelectionGroupType = productSelectionGroupType;
            return this;
        }

        internal TestProductSelectionGroupBuilder IncludeNavigationPropertiesFromContext(IconContext context)
        {
            this.includeNavigationPropertiesFromContext = true;
            this.context = context;
            return this;
        }

        public ProductSelectionGroup Build()
        {
            var psg = new ProductSelectionGroup
            {
                ProductSelectionGroupId = productSelectionGroupId,
                ProductSelectionGroupName = productSelectionGroupName,
                TraitValue = traitValue
            };

            if (includeNavigationPropertiesFromContext)
            {
                if (context == null)
                {
                    context = new IconContext();
                }
                if (trait == null)
                {
                    trait = context.Trait.First(t => t.traitID == traitId);
                }
                if (productSelectionGroupType == null)
                {
                    productSelectionGroupType = context.ProductSelectionGroupType.First(p => p.ProductSelectionGroupTypeId == productSelectionGroupTypeId);
                }
            }
            else
            {
                psg.TraitId = traitId;
                psg.ProductSelectionGroupTypeId = productSelectionGroupTypeId;
            }

            return psg;
        }

        public static implicit operator ProductSelectionGroup(TestProductSelectionGroupBuilder builder)
        {
            return builder.Build();
        }
    }
}
