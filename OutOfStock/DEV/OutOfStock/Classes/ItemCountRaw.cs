using System.Globalization;
using OOSCommon.DataContext;

namespace OutOfStock.Classes
{
    public class ItemCountRaw
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int Grocery { get; set; }
        public int WholeBody { get; set; }

        private const string SaveSql = "EXEC dbo.SaveItemCount @StoreId = {0}, @TeamName = {1},  @ItemCount = {2} ";

        public bool Save(OOSEntities context)
        {
            context.ExecuteStoreCommand(SaveSql, this.StoreId.ToString(CultureInfo.InvariantCulture),"Grocery", this.Grocery);
            context.ExecuteStoreCommand(SaveSql, this.StoreId.ToString(CultureInfo.InvariantCulture), "Whole Body", this.WholeBody);
                
            return true;
        }

    }
}