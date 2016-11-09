using Icon.Framework;

namespace Icon.Testing.Builders
{
    public class TestItemTraitBuilder
    {
        private int traitId;
        private int itemId;
        private string uomId;
        private string traitValue;
        private int localeId;

        public TestItemTraitBuilder()
        {
            this.traitId = Traits.ProductDescription;
            this.itemId = 1;
            this.uomId = null;
            this.traitValue = "Test Product Description";
            this.localeId = 100;
        }

        public TestItemTraitBuilder WithTraitId(int traitId)
        {
            this.traitId = traitId;
            return this;
        }

        public TestItemTraitBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemTraitBuilder WithLocaleId(int localeId)
        {
            this.localeId = localeId;
            return this;
        }

        public TestItemTraitBuilder WithTraitValue(string traitValue)
        {
            this.traitValue = traitValue;
            return this;
        }

        public ItemTrait Build()
        {
            return new ItemTrait
            {
                traitID = this.traitId,
                itemID = this.itemId,
                uomID = this.uomId,
                traitValue = this.traitValue,
                localeID = this.localeId
            };
        }

        public static implicit operator ItemTrait(TestItemTraitBuilder builder)
        {
            return builder.Build();
        }
    }
}
