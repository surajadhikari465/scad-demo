using Icon.Framework;

namespace Icon.Testing.Builders
{
    public class TestScanCodeBuilder
    {
        private string scanCode;
        private int scanCodeTypeId = ScanCodeTypes.Upc;

        public TestScanCodeBuilder() { }

        public TestScanCodeBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public ScanCode Build()
        {
            ScanCode scanCode = new ScanCode
            {
                scanCode = this.scanCode,
                scanCodeTypeID = this.scanCodeTypeId
            };

            return scanCode;
        }

        public static implicit operator ScanCode(TestScanCodeBuilder builder)
        {
            return builder.Build();
        }
    }
}
