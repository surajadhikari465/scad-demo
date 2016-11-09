using Irma.Framework;
using System;
using System.Collections.Generic;
using Testing.Core.Helpers;

namespace Testing.Core.Templates.Irma
{
    internal class ItemBuilderTemplate : IObjectBuilderTemplate<Item>, ISqlBuilderTemplate<Item>
    {
        public string TableName { get { return typeof(Item).Name; } }

        public string IdentityColumn
        {
            get
            {
               return PropertyHelper.GetPropertyName((Item i) => i.Item_Key);
            }
        }

        public Dictionary<string, string> PropertyToColumnMapping { get { return null; } }

        public ObjectBuilder<Item> BuildDefaults()
        {
            return new ObjectBuilder<Item>()
                .With(x => x.Item_Description, "Test Item Description")
                .With(x => x.Sign_Description, "Test Sign Description")
                .With(x => x.SubTeam_No, 101)
                .With(x => x.Package_Desc1, 1m)
                .With(x => x.Package_Desc2, 1m)
                .With(x => x.Package_Unit_ID, 1)
                .With(x => x.Tie, Convert.ToByte(8))
                .With(x => x.High, Convert.ToByte(10))
                .With(x => x.Yield, 100m)
                .With(x => x.Retail_Unit_ID, 1)
                .With(x => x.Vendor_Unit_ID, 1)
                .With(x => x.Distribution_Unit_ID, 1)
                .With(x => x.WFM_Item, true)
                .With(x => x.POS_Description, "Test POS Description")
                .With(x => x.Retail_Sale, true)
                .With(x => x.HFM_Item, true)
                .With(x => x.Insert_Date, DateTime.Now)
                .With(x => x.ClassID, 44020);
        }
    }
}
