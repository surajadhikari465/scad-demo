using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetLocalesByChainQuery : IQueryHandler<GetLocalesByChainParameters, List<Locale>>
    {
        private readonly IconContext context;

        public GetLocalesByChainQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Locale> Search(GetLocalesByChainParameters parameters)
        {
            List<Locale> locales = context.Locale
                .Include(l => l.LocaleTrait.Select(localeTrait => localeTrait.Trait))
                .Include(l => l.LocaleType)
                .Include(l => l.LocaleAddress)
                .Include(l => l.LocaleAddress.Select(la => la.Address))
                .Include(l => l.LocaleAddress.Select(la => la.Address.AddressType))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.PostalCode))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City.County))
                .ToList();

            var localesInChain = new List<Locale>();

            var chainLocale = locales.First(l => l.localeName == parameters.ChainName);

            localesInChain.Add(chainLocale);
            AddChildLocales(localesInChain, chainLocale, locales);

            return localesInChain.ToList();
        }

        private void AddChildLocales(List<Locale> localesInChain, Locale locale, List<Locale> allLocales)
        {
            var childLocales = allLocales.Where(l => l.parentLocaleID == locale.localeID);
            if (childLocales.Any())
            {
                localesInChain.AddRange(childLocales);
                foreach (var childLocale in childLocales)
                {
                    AddChildLocales(localesInChain, childLocale, allLocales);
                }
            }
        }
    }
}
