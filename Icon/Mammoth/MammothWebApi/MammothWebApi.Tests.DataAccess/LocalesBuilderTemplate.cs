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
    public class LocalesBuilderTemplate : IObjectBuilderTemplate<Locales>, ISqlBuilderTemplate<Locales>
    {
        public string IdentityColumn
        {
            get
            {
                return PropertyHelper.GetPropertyName((Locales i) => i.LocaleID);
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
                return "dbo.Locales_FL";
            }
        }

        public ObjectBuilder<Locales> BuildDefaults()
        {
            return new ObjectBuilder<Locales>()
                .With(l => l.AddedDate, DateTime.Now);
        }
    }
}
