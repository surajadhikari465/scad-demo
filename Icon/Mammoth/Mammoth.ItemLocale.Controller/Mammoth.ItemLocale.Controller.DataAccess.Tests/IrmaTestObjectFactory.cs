namespace Irma.Testing
{
    using Irma.Framework;
    using Mammoth.ItemLocale.Controller.DataAccess.Tests;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class IrmaTestObjectFactory
    {
        public static IrmaObjectBuilder<T> Build<T>() where T : class
        {
            return new IrmaObjectBuilder<T>();
        }

        public static IrmaObjectBuilder<Item> BuildItem()
        {
            return Build<Item>()
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

        public static IrmaObjectBuilder<ItemIdentifier> BuildItemIdentifier()
        {
            return Build<ItemIdentifier>()
                .With(x => x.Identifier, "1234598760")
                .With(x => x.CheckDigit, "0")
                .With(x => x.IdentifierType, "B");
        }

        public static IrmaObjectBuilder<Store> BuildStore()
        {
            return Build<Store>()
                .With(x => x.Store_Name, "Test Store");
        }

        public static IrmaObjectBuilder<ValidatedScanCode> BuildValidatedScanCode()
        {
            return Build<ValidatedScanCode>()
                .With(x => x.InsertDate, DateTime.Now);
        }

        public static IrmaObjectBuilder<SubTeam> BuildSubTeam()
        {
            return Build<SubTeam>()
                .With(x => x.EXEDistributed, false)
                .With(x => x.EXEWarehouseSent, false)
                .With(x => x.Retail, true)
                .With(x => x.SubTeam_Abbreviation, "TSTNG")
                .With(x => x.SubTeam_Name, "Test SubTeam")
                .With(x => x.SubTeam_No, 777)
                .With(x => x.Target_Margin, 0);
        }

        public static IrmaObjectBuilder<VendorCostHistory> BuildVendorCostHistory()
        {
            return Build<VendorCostHistory>()
                .With(x => x.EndDate, DateTime.Now.AddDays(100))
                .With(x => x.FromVendor, false)
                .With(x => x.InsertDate, DateTime.Now)
                .With(x => x.MSRP, 1)
                .With(x => x.StartDate, DateTime.Now.AddDays(-5))
                .With(x => x.UnitCost, 1)
                .With(x => x.UnitFreight, 1);
        }

        public static IrmaObjectBuilder<ItemOverride> BuildItemOverride()
        {
            return Build<ItemOverride>()
                .With(x => x.Average_Unit_Weight, 1)
                .With(x => x.Item_Description, "Test Override Item_Description")
                .With(x => x.Sign_Description, "Test Override Sign_Description")
                .With(x => x.POS_Description, "Test Override POS_Des");
        }
    }

    public class IrmaObjectBuilder<T> where T : class
    {
        private T irmaObject;

        public IrmaObjectBuilder()
        {
            this.irmaObject = Activator.CreateInstance(typeof(T)) as T;
        }

        public IrmaObjectBuilder<T> With<TPropertyType>(Expression<Func<T, TPropertyType>> propertyExpression, TPropertyType value)
        {
            var propertyInfo = typeof(T).GetProperty(GetPropertyName(propertyExpression));
            propertyInfo.SetValue(this.irmaObject, value);

            return this;
        }

        public T ToObject()
        {
            return this.irmaObject;
        }

        private static string GetPropertyName<TPropertyType>(Expression<Func<T, TPropertyType>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression != null)
            {
                var property = memberExpression.Member as PropertyInfo;
                var getMethod = property.GetGetMethod(true);
                return property.Name;
            }
            else
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                var property = unaryExpression.Operand as MemberExpression;
                return property.Member.Name;
            }
        }
    }
}