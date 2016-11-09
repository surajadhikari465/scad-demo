using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetLocalesByBusinessUnitIdQueryHandler : IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>>
    {
        private IRenewableContext<IconContext> context;

        public GetLocalesByBusinessUnitIdQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public List<Locale> Execute(GetLocalesByBusinessUnitIdQuery parameters)
        {
            var locales = context.Context.Locale.Where(l =>
                l.LocaleTrait.Any(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId && parameters.BusinessUnits.Contains(lt.traitValue))).ToList();

            return locales;
        }
    }
}
