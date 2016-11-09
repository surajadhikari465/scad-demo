using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Core;
using Testing.Core.Helpers;

namespace MammothWebApi.Tests.DataAccess
{
    public class CurrencyBuilderTemplate : IObjectBuilderTemplate<Currency>, ISqlBuilderTemplate<Currency>
    {
        public string IdentityColumn
        {
            get
            {
                return PropertyHelper.GetPropertyName((Currency i) => i.CurrencyID);
            }
        }

        public Dictionary<string, string> PropertyToColumnMapping
        {
            get
            {
                return null;
            }
        }

        public string TableName
        {
            get
            {
                return "dbo.Currency";
            }
        }

        public ObjectBuilder<Currency> BuildDefaults()
        {
            return new ObjectBuilder<Currency>()
                .With(l => l.CurrencyCode, "USD")
                .With(l => l.CurrencyDesc, "US Dollar");
        }
    }
}
