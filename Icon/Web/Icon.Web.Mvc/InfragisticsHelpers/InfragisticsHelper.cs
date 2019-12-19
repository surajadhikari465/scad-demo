using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Icon.Web.Mvc.InfragisticsHelpers
{
    public class InfragisticsHelper : IInfragisticsHelper
    {
        private const string IG_TRANSACTIONS_KEY = "ig_transactions";

        public List<Transaction<T>> LoadTransactions<T>(NameValueCollection formData) where T : class
        {
            return new GridModel().LoadTransactions<T>(formData[IG_TRANSACTIONS_KEY]);
        }
    }
}