using Icon.Framework;

namespace Icon.Testing.Builders
{
    public class TestUomBuilder
    {
        private string uomCode;
        private string uomName;

        public TestUomBuilder()
        {
            this.uomCode = "ZZ";
            this.uomName = "Zed";
        }

        public UOM Build()
        {
            return new UOM
            {
                uomCode = this.uomCode,
                uomName = this.uomName
            };
        }

        public static implicit operator UOM(TestUomBuilder builder)
        {
            return builder.Build();
        }
    }
}
