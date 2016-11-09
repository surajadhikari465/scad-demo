using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestLocaleBuilder
    {
        private int localeID;
        private int ownerOrgPartyId;
        private string localeName;
        private DateTime? localeOpenDate;
        private DateTime? localeCloseDate;
        private int localeTypeId;
        private int? parentLocaleId;
        private int? businessUnitId;

        public TestLocaleBuilder()
        {
            this.localeID = 0;
            this.ownerOrgPartyId = 1;
            this.localeName = "Test Locale";
            this.localeOpenDate = null;
            this.localeCloseDate = null;
            this.localeTypeId = LocaleTypes.Store;
            this.parentLocaleId = null;
            this.businessUnitId = null;
        }

        public TestLocaleBuilder WithLocaleId(int localeId)
        {
            this.localeID = localeId;
            return this;
        }

        public TestLocaleBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestLocaleBuilder WithLocaleTypeId(int localeTypeId)
        {
            this.localeTypeId = localeTypeId;
            return this;
        }

        public TestLocaleBuilder WithLocaleName(string localeName)
        {
            this.localeName = localeName;
            return this;
        }

        public TestLocaleBuilder WithParentLocaleId(int parentLocaleId)
        {
            this.parentLocaleId = parentLocaleId;
            return this;
        }

        public Locale Build()
        {
            var locale = new Locale();
            locale.localeID = this.localeID;

            if (this.businessUnitId != null)
            {
                var localeTraits = new List<LocaleTrait>
                {
                    new LocaleTrait
                    {
                        traitID = Traits.PsBusinessUnitId,
                        traitValue = this.businessUnitId.ToString(),
                        localeID = locale.localeID
                    }
                };

                locale.LocaleTrait = localeTraits;
            }

            locale.ownerOrgPartyID = this.ownerOrgPartyId;
            locale.localeName = this.localeName;
            locale.localeOpenDate = this.localeOpenDate;
            locale.localeCloseDate = this.localeCloseDate;
            locale.localeTypeID = this.localeTypeId;
            locale.parentLocaleID = this.parentLocaleId;

            return locale;
        }

        public static implicit operator Locale(TestLocaleBuilder builder)
        {
            return builder.Build();
        }
    }
}
