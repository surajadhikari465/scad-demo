using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class StoreItemModel
    {
        public decimal AverageCost { get; set; }
        public bool CanInventory { get; set; }
        public bool CostedByWeight { get; set; }
        public bool DiscontinueItem { get; set; }
        public int GlAccount { get; set; }
        public bool HfmItem { get; set; }
        public string Identifier { get; set; }
        public bool IsItemAuthorized { get; set; }
        public bool IsSellable { get; set; }
        public string ItemDescription { get; set; }
        public int ItemKey { get; set; }
        public int Multiple { get; set; }
        public bool NotAvailable { get; set; }
        public bool OnSale { get; set; }
        public string PosDescription { get; set; }
        public int PackageDesc1 { get; set; }
        public decimal PackageDesc2 { get; set; }
        public string PackageUnitAbbreviation { get; set; }
        public decimal Price { get; set; }
        public string PriceChangeTypeDescription { get; set; }
        public bool RetailSale { get; set; }
        public string RetailSubteamName { get; set; }
        public int RetailSubteamNo { get; set; }
        public int SaleEarnedDisc1 { get; set; }
        public int SaleEarnedDisc2 { get; set; }
        public int SaleEarnedDisc3 { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int SaleMultiple { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SaleStartDate { get; set; }
        public string SignDescription { get; set; }
        public bool SoldByWeight { get; set; }
        public int StoreNo { get; set; }
        public int StoreVendorID { get; set; }
        public string TransferToSubteamName { get; set; }
        public int TransferToSubteamNo { get; set; }
        public int UserId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int VendorUnitId { get; set; }
        public string VendorUnitName { get; set; }
        public bool WfmItem { get; set; }
        public int RetailUnitId { get; set; }
        public string RetailUnitName { get; set; }
        public decimal VendorCost { get; set; }
        public string VendorPack { get; set; }
    }
}
