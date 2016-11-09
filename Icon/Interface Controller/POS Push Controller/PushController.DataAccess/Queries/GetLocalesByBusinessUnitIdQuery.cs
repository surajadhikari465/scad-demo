using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetLocalesByBusinessUnitIdQuery : IQuery<List<Locale>>
    {
        public List<string> BusinessUnits { get; set; }
    }
}
