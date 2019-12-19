using System.Collections.Generic;
using System.Collections.Specialized;
using Infragistics.Web.Mvc;

namespace Icon.Web.Mvc.InfragisticsHelpers
{
    public interface IInfragisticsHelper
    {
        List<Transaction<T>> LoadTransactions<T>(NameValueCollection formData) where T : class;
    }
}