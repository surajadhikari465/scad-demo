namespace Icon.Web.Mvc.Excel.Models
{
    public class CertificationAgencyExcelModel
    {
        [ExcelColumn("Agency", 200)]
        public string CertificationAgencyLineage { get; set; }
    }
}