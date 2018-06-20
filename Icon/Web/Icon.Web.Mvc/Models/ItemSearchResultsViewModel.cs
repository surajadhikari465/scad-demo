using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class ItemSearchResultsViewModel
    {
        public ItemSearchResultsViewModel()
        {
            Items = new List<ItemViewModel>();
        }

        public List<ItemViewModel> Items { get; set; }
        public SelectList RetailUoms { get; set; }
        public SelectList DeliverySystems { get; set; }
        public List<HierarchyClassViewModel> BrandHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> TaxHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> NationalHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> AnimalWelfareRatings { get; set; }
        public List<HierarchyClassViewModel> MilkTypes { get; set; }
        public List<HierarchyClassViewModel> EcoScaleRatings { get; set; }
        public List<HierarchyClassViewModel> SeafoodFreshOrFrozenTypes { get; set; }
        public List<HierarchyClassViewModel> SeafoodCatchTypes { get; set; }   
        public BooleanComboBoxValuesViewModel BooleanComboBoxValues { get; set; }
        public List<CertificationAgencyModel> GlutenFreeAgencies { get; set; }
        public List<CertificationAgencyModel> KosherAgencies { get; set; }
        public List<CertificationAgencyModel> NonGmoAgencies { get; set; }
        public List<CertificationAgencyModel> OrganicAgencies { get; set; }
        public List<CertificationAgencyModel> VeganAgencies { get; set; }
        public NullableBooleanComboBoxValuesViewModel NullableBooleanComboBoxValues { get; set; }
    }
}