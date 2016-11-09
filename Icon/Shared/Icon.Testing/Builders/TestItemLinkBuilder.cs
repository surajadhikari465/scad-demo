using Icon.Framework;

namespace Icon.Testing.Builders
{
    public class TestItemLinkBuilder
    {
        private int parentItemId;
        private int childItemId;
        private int localeId;

        public TestItemLinkBuilder WithParentItemId(int itemId)
        {
            this.parentItemId = itemId;
            return this;
        }

        public TestItemLinkBuilder WithChildItemId(int itemID)
        {
            this.childItemId = itemID;
            return this;
        }

        public TestItemLinkBuilder WithLocaleId(int localeId)
        {
            this.localeId = localeId;
            return this;
        }

        public ItemLink Build()
        {
            return new ItemLink
            {
                parentItemID = parentItemId,
                childItemID = childItemId,
                localeID = localeId
            };
        }

        public static implicit operator ItemLink(TestItemLinkBuilder builder)
        {
            return builder.Build();
        }
    }
}
