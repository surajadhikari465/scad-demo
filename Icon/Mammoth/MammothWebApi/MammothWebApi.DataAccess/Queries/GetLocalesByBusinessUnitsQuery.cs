using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetLocalesByBusinessUnitsQuery : IQuery<IEnumerable<Locales>>
    {
        public IEnumerable<int> BusinessUnits { get; set; }
    }
}
