using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Testing.Builders
{
    public class TestAgencyBuilder
    {
        private string agencyId;
        private List<string> exclusions;

        public TestAgencyBuilder() 
        {
            this.exclusions = new List<string>();
        }

        public TestAgencyBuilder WithAgencyId(string agencyId)
        {
            this.agencyId = agencyId;
            return this;
        }

        public TestAgencyBuilder WithExclusion(string scanCode)
        {
            this.exclusions.Add(scanCode);
            return this;
        }

        public Agency Build()
        {
            Agency agency = new Agency
            {
                AgencyId = this.agencyId,
                Locale = new List<Locale>()
            };
            
            return agency;
        }

        public static implicit operator Agency(TestAgencyBuilder builder)
        {
            return builder.Build();
        }
    }
}
