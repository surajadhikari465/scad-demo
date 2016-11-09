using System.Collections.Specialized;

namespace Icon.Web.Mvc.Utility
{
    public interface IInfragisticsHelper
    {
        InfragisticsSortParameterPaseResult ParseSortParameterFromQueryString(NameValueCollection queryString);
    }
}
