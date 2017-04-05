using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.DataAccess.Queries
{
    public class GetCurrenciesQuery : IQueryHandler<GetCurrenciesParameters, IEnumerable<Currency>>
    {
        private IDbProvider db;

        public GetCurrenciesQuery(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Currency> Search(GetCurrenciesParameters parameters)
        {
            string sql = "SELECT CurrencyID, CurrencyCode, CurrencyDesc FROM Currency";
            IEnumerable<Currency> currencies = this.db.Connection.Query<Currency>(sql, null, this.db.Transaction);
            return currencies;
        }
    }
}
