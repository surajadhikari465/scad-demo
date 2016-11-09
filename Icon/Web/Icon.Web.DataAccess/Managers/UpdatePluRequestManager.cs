
namespace Icon.Web.DataAccess.Managers
{
    public class UpdatePluRequestManager
    {
        public int PluRequestId { get; set; }
        public string NationalPlu { get; set; }
        public string PluType { get; set; }
        public string BrandName { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public int PackageUnit { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUom { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public string MerchandiseHierarchyClassName { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public string UserName { get; set; }
        public string RequestStatus { get; set; }
        public int scanCodeTypeID { get; set; }
        public string Notes { get; set; }
        public int FinancialHierarchyClassId { get; set; }
    }
}
