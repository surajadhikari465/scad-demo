using Icon.Web.DataAccess.Infrastructure;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetLocaleQuery : IQueryHandler<GetLocaleParameters, List<Locale>>
    {
        private readonly IconContext context;

        public GetLocaleQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Locale> Search(GetLocaleParameters parameters)
        {
            IQueryable<Locale> locale = null;

            locale = context.Locale
                .Include(l => l.LocaleTrait.Select(localeTrait => localeTrait.Trait))
                .Include(l => l.LocaleType)
                .Include(l => l.LocaleAddress)
                .Include(l => l.LocaleAddress.Select(la => la.Address))
                .Include(l => l.LocaleAddress.Select(la => la.Address.AddressType))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.PostalCode))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City.County));

            if (parameters.LocaleId != null)
            {
                locale = context.Locale.Where(l => l.localeID == parameters.LocaleId);
            }

            return locale.ToList();
        }
    }
}
