namespace Icon.Web.Mvc.Excel.Models
{
    public class HierarchyClassExcelModel
    {
        [ExcelColumn("Hierarchy Class Lineage", 200)]
        public string HierarchyClassLineage { get; set; }
    }
}
