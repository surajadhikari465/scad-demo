using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.LocaleListener.Queries
{
    public class GetSequenceIdFromLocaleIdParameters : IQuery<int>
    {
        public int localeId;
    }
}