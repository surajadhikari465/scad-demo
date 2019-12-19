namespace Icon.Web.Mvc.Excel.Models
{
    public class MerchTaxMappingExcelModel
    {
        [ExcelColumn("Merchandise", 200)]
        public string MerchandiseLineage { get; set; }

        [ExcelColumn("Tax", 200)]
        public string TaxLineage { get; set; }
    }
}