using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetNationalClassCodeToClassIdMappingQuery : IQuery<Dictionary<int, int>> { }
}
