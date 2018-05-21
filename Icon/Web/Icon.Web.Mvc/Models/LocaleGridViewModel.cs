using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class LocaleGridViewModel
    {
        public LocaleGridRowViewModel ChainLocale { get; set; }
        public IEnumerable<CountryViewModel> Countries { get; set; }
        public IEnumerable<TerritoryViewModel> Territories { get; set; }
        public IEnumerable<TimeZoneViewModel> TimeZones { get; set; }
        public IEnumerable<Agency> EwicAgencies { get; set; }
        public IEnumerable<string> StorePosTypes { get; set; }
        public IEnumerable<CurrencyViewModel> Currencies { get; set; }
    }
}