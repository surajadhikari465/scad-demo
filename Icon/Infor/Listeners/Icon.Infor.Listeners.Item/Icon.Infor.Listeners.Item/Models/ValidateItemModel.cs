using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ValidateItemModel
    {
        public int ItemId { get; set; }
        public int BrandHierarchyClassId { get; set; }
        public string FinancialHierarchyClassId { get; set; }
        public int MerchandiseHierarchyClassId { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public string TaxHierarchyClassId { get; set; }

        public ValidateItemModel(ItemModel item)
        {
            ItemId = item.ItemId;
            BrandHierarchyClassId = int.Parse(item.BrandsHierarchyClassId);
            MerchandiseHierarchyClassId = int.Parse(item.MerchandiseHierarchyClassId);
            NationalHierarchyClassId = int.Parse(item.NationalHierarchyClassId);
            TaxHierarchyClassId = item.TaxHierarchyClassId;
            FinancialHierarchyClassId = item.FinancialHierarchyClassId;
        }
    }
}
