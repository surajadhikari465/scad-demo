using Icon.Framework;

namespace Icon.Web.Mvc.Models
{
    public class PluRequestViewModel
    {
        public int PluRequestID { get; set; }
        public int FinancialClassID { get; set; }
        public int? MerchandiseClassID { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public bool IsNewBrand { get; set; }
        public string ItemDescription { get; set; }
        public string PosDescription { get; set; }
        public int PackageUnit { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUom { get; set; }
        public int NationalClassID { get; set; }
        public int ScanCodeTypeID { get; set; }
        public string NationalPLU { get; set; }
        public string RequestStatus { get; set; }
        public string RequesterName { get; set; }
        public string RequesterEmail { get; set; }
        public System.DateTime RequestedDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
    
        
        public PluRequestViewModel() {}

        public PluRequestViewModel(PLURequest pluRequest)
        {
            this.PluRequestID = pluRequest.pluRequestID;
            this.BrandName = pluRequest.brandName;
            this.NationalPLU = pluRequest.nationalPLU;
            this.ItemDescription = pluRequest.itemDescription;
            this.PosDescription = pluRequest.posDescription;
            this.PackageUnit = pluRequest.packageUnit;
            this.RetailUom = pluRequest.retailUom;
            this.RetailSize = pluRequest.retailSize;
            this.MerchandiseClassID = pluRequest.merchandiseClassID.HasValue ? pluRequest.merchandiseClassID.Value : -1;
            this.NationalClassID = pluRequest.nationalClassID;
            this.RequesterName = pluRequest.requesterName;
            this.RequestStatus = pluRequest.requestStatus;
            this.FinancialClassID = pluRequest.FinancialClassID;
        }
    }
}