namespace Irma.Testing
{
    using Framework;
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
            var property = memberExpression.Member as PropertyInfo;
            var getMethod = property.GetGetMethod(true);
            return property.Name;
        }
    }
}