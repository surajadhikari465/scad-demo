namespace Icon.Web.DataAccess.Models
{
    public class MerchTaxMappingModel
    {       
        public int MerchandiseHierarchyClassId { get; set; }      
        public virtual string MerchandiseHierarchyClassName { get; set; }
        public int TaxHierarchyClassId { get; set; }       
        public string TaxHierarchyClassName { get; set; }
        public string MerchandiseHierarchyClassLineage { get; set; }
        public string TaxHierarchyClassLineage { get; set; }
    }
}
