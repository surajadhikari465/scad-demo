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
    public class ItemBuilderTemplate : IObjectBuilderTemplate<Item>, ISqlBuilderTemplate<Item>
    {
        public string IdentityColumn
        {
            get
            {
                return PropertyHelper.GetPropertyName((Item i) => i.ItemID);
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
                return "dbo.Items";
            }
        }

        public ObjectBuilder<Item> BuildDefaults()
        {
            return new ObjectBuilder<Item>()
                .With(i => i.AddedDate, DateTime.Now);
        }
    }
}
