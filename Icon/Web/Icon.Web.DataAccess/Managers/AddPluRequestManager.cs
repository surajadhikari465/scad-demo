
namespace Icon.Web.DataAccess.Managers
{
    public class AddPluRequestManager
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
        public int? FinancialHierarchyClassId { get; set; }
        public string FinancialHierarchyClassName { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }
    }
}
