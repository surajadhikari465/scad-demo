using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Utility
{
    public class InfragisticsHelper : IInfragisticsHelper
    {
        public InfragisticsSortParameterPaseResult ParseSortParameterFromQueryString(NameValueCollection queryString)
        {
            var sortKey = queryString.AllKeys.FirstOrDefault(k => k.StartsWith("sort("));
            if (sortKey != null)
            {

                var result = new InfragisticsSortParameterPaseResult
                {
                    SortParameterExists = true,
                    SortColumn = sortKey.Substring(sortKey.IndexOf('(') + 1).TrimEnd(')'),
                    SortOrder = queryString[sortKey]
                };

                return result;
            }
            else
            {
                return new InfragisticsSortParameterPaseResult { SortParameterExists = false };
            }
        }
    }
}
