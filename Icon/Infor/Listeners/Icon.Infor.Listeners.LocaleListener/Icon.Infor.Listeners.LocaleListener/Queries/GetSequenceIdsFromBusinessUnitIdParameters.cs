using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.LocaleListener.Queries
{
    public class GetSequenceIdFromBusinessUnitIdParameters : IQuery<int>
    {
        public int businessUnitId;
    }
}