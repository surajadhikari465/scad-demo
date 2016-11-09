using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Testing.Builders
{
    public class TestEwicMappingBuilder
    {
        private string agencyId;
        private string aplScanCode;
        private ScanCode wfmScanCode;

        public TestEwicMappingBuilder() { }

        public TestEwicMappingBuilder WithAgencyId(string agencyId)
        {
            this.agencyId = agencyId;
            return this;
        }

        public TestEwicMappingBuilder WithAplScanCode(string aplScanCode)
        {
            this.aplScanCode = aplScanCode;
            return this;
        }

        public TestEwicMappingBuilder WithWfmScanCode(ScanCode wfmScanCode)
        {
            this.wfmScanCode = wfmScanCode;
            return this;
        }

        public Mapping Build()
        {
            Mapping mapping = new Mapping
            {
                AgencyId = this.agencyId,
                AplScanCode = this.aplScanCode,
                ScanCode = this.wfmScanCode
            };

            return mapping;
        }

        public static implicit operator Mapping(TestEwicMappingBuilder builder)
        {
            return builder.Build();
        }
    }
}
