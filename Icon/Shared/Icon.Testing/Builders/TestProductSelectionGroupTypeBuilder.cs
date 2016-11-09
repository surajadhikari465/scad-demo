using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestProductSelectionGroupTypeBuilder
    {
        private int id;
        private string name;

        public TestProductSelectionGroupTypeBuilder()
        {
            id = 1;
            name = "Consumable";
        }

        public TestProductSelectionGroupTypeBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public TestProductSelectionGroupTypeBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ProductSelectionGroupType Build()
        {
            return new ProductSelectionGroupType
            {
                ProductSelectionGroupTypeId = id,
                ProductSelectionGroupTypeName = name
            };
        }

        public static implicit operator ProductSelectionGroupType(TestProductSelectionGroupTypeBuilder builder)
        {
            return builder.Build();
        }
    }
}
