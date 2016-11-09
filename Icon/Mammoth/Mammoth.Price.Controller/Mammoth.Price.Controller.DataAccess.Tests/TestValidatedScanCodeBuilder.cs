using System;
using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestValidatedScanCodeBuilder
    {
        private int id;
        private string scanCode;
        private System.DateTime insertDate;

        public TestValidatedScanCodeBuilder()
        {
            this.id = 0;
            this.scanCode = null;
            this.insertDate = DateTime.Now;
        }

        public TestValidatedScanCodeBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public TestValidatedScanCodeBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestValidatedScanCodeBuilder WithInsertDate(System.DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public ValidatedScanCode Build()
        {
            ValidatedScanCode validatedScanCode = new ValidatedScanCode();

            validatedScanCode.Id = this.id;
            validatedScanCode.ScanCode = this.scanCode;
            validatedScanCode.InsertDate = this.insertDate;

            return validatedScanCode;
        }

        public static implicit operator ValidatedScanCode(TestValidatedScanCodeBuilder builder)
        {
            return builder.Build();
        }
    }
}