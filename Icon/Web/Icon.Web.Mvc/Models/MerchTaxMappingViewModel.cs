using Icon.Web.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class MerchTaxMappingViewModel
    {
        public int? MerchandiseHierarchyClassId { get; set; }
        [Display(Name = "Merchandise Hierarchy")]

        public string MerchandiseHierarchyClassName { get; set; }

        public int TaxHierarchyClassId { get; set; }
        [Display(Name = "Tax Hierarchy")]

        public string TaxHierarchyClassName { get; set; }
        [Display(Name = "Merchandise Hierarchy")]
        public string MerchandiseHierarchyClassLineage { get; set; }

        [Display(Name = "Tax Hierarchy")]
        public string TaxHierarchyClassLineage { get; set; }

        public MerchTaxMappingViewModel() { }

        public MerchTaxMappingViewModel(MerchTaxMappingModel model)
        {
            MerchandiseHierarchyClassId = model.MerchandiseHierarchyClassId;
            MerchandiseHierarchyClassName = model.MerchandiseHierarchyClassName;
            TaxHierarchyClassId = model.TaxHierarchyClassId;
            TaxHierarchyClassName = model.TaxHierarchyClassName;
        }
    }
}
