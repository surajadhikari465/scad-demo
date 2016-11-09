using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetTaxCodeToTaxIdMappingQuery : IQuery<Dictionary<string, int>> { }
}
