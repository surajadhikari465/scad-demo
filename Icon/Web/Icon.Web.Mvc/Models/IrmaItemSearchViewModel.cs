using Icon.Framework;
using Icon.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Models
{
    public class IrmaItemSearchViewModel
    {
        [ScanCode]     
        [Display(Name = "Scan Code")]
        public string Identifier { get; set; }

        [Display(Name = "Product Description")]
        public string ItemDescription { get; set; }
        
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
        
        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }

        [PartialScanCodeAttribute("Identifier")]
        [Display(Name = "Allow Partial Searches")]
        public bool PartialScanCode { get; set; }
        
        [Display(Name = "Tax Class")]
        public string TaxHierarchyName { get; set; }

        public List<IrmaItemViewModel> Items { get; set; }
        public List<string> Uoms { get; set; }
        public List<string> DeliverySystems { get; set; }
        public List<HierarchyClassViewModel> AllBrands { get; set; }
        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> TaxHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> NationalHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> AnimalWelfareRatings { get; set; }
        public List<HierarchyClassViewModel> MilkTypes { get; set; }
        public List<HierarchyClassViewModel> EcoScaleRatings { get; set; }
        public List<HierarchyClassViewModel> ProductionClaims { get; set; }
        public List<HierarchyClassViewModel> SeafoodFreshOrFrozenTypes { get; set; }
        public List<HierarchyClassViewModel> SeafoodCatchTypes { get; set; }
        public List<CertificationAgencyModel> GlutenFreeAgencies { get; set; }
        public List<CertificationAgencyModel> KosherAgencies { get; set; }
        public List<CertificationAgencyModel> NonGmoAgencies { get; set; }
        public List<CertificationAgencyModel> OrganicAgencies { get; set; }
        public List<CertificationAgencyModel> VeganAgencies { get; set; }
        public NullableBooleanComboBoxValuesViewModel NullableBooleanComboBoxValues { get; set; }

    }
}