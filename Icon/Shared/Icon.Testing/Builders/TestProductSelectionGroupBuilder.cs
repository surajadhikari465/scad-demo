using Icon.Framework;

namespace Icon.Testing.Builders
{
    public class TestProductSelectionGroupBuilder
    {
        private int psgId;
        private string psgName;
        private int psgTypeId;
        private int? traitId;
        private string traitValue;
        private int? merchandiseHierarchyClassId;
        private ProductSelectionGroupType psgType;

        public TestProductSelectionGroupBuilder()
        {
            psgId = 0;
            psgName = "Test PSG";
            psgTypeId = ProductSelectionGroupTypes.Consumable;
            traitId = null;
            traitValue = null;
            merchandiseHierarchyClassId = null;
        }

        public TestProductSelectionGroupBuilder WithProductSelectionGroupId(int id)
        {
            this.psgId = id;
            return this;
        }

        public TestProductSelectionGroupBuilder WithProductSelectionGroupName(string name)
        {
            this.psgName = name;
            return this;
        }

        public TestProductSelectionGroupBuilder WithProductSelectionGroupTypeId(int typeId)
        {
            this.psgTypeId = typeId;
            return this;
        }

        public TestProductSelectionGroupBuilder WithTraitId(int traitId)
        {
            this.traitId = traitId;
            return this;
        }

        public TestProductSelectionGroupBuilder WithTraitValue(string traitValue)
        {
            this.traitValue = traitValue;
            return this;
        }

        public TestProductSelectionGroupBuilder WithMerchandiseHierarchyClassId(int id)
        {
            this.merchandiseHierarchyClassId = id;
            return this;
        }

        public TestProductSelectionGroupBuilder WithProductSelectionGroupType(ProductSelectionGroupType productSelectionGroupType)
        {
            this.psgType = productSelectionGroupType;
            return this;
        }

        public ProductSelectionGroup Build()
        {
            var psg = new ProductSelectionGroup
            {
                ProductSelectionGroupId = psgId,
                ProductSelectionGroupName = psgName,
                ProductSelectionGroupTypeId = psgTypeId,
                MerchandiseHierarchyClassId = merchandiseHierarchyClassId,
                TraitId = traitId,
                TraitValue = traitValue
            };

            if(psgType != null)
            {
                psg.ProductSelectionGroupType = psgType;
                psg.ProductSelectionGroupTypeId = psgTypeId;
            }

            return psg;
        }

        public static implicit operator ProductSelectionGroup(TestProductSelectionGroupBuilder builder)
        {
            return builder.Build();
        }
    }
}
