using Mammoth.Common.DataAccess;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetLocalesByRegionQuery : IQuery<List<Locales>>
    {
        public string Region { get; set; }
    }
}
