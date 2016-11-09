using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Core;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.Tests.DataAccess
{
    public class PriceModelBuilderTemplate : IObjectBuilderTemplate<Prices>, ISqlBuilderTemplate<Prices>
    {
        public string IdentityColumn { get { return string.Empty; } }

        public Dictionary<string, string> PropertyToColumnMapping
        {
            get { return null; }
        }

        public string TableName
        {
            get
            {
                return "dbo.Price_FL";
            }
        }

        public ObjectBuilder<Prices> BuildDefaults()
        {
            return new ObjectBuilder<Prices>()
                .With(p => p.Region, "FL")
                .With(p => p.CurrencyID, 1)
                .With(p => p.PriceUOM, "EA")
                .With(p => p.Multiple, 1)
                .With(p => p.AddedDate, DateTime.Now);
        }
    }
}
