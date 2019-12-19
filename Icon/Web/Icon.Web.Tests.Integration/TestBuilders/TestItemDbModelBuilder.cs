using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.Tests.Integration.TestBuilders
{
    public class TestItemDbModelBuilder
    {
        private ItemDbModel model;

        public TestItemDbModelBuilder() { }

        public TestItemDbModelBuilder(ItemDbModel model)
        {
            this.model = model;
        }

        public TestItemDbModelBuilder WithChanges(Func<ItemDbModel, ItemDbModel> editFunction)
        {
            this.model = editFunction(this.model);
            return this;
        }

        public ItemDbModel Build()
        {
            return model;
        }

        public static implicit operator ItemDbModel(TestItemDbModelBuilder builder) => builder.Build();
    }
}
