using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemDetailInformation
    {
        public string InforItemID { get; set; }
        public string Allergens { get; set; }
        public string Brand { get; set; }
        public string UPC { get; set; }
        public string Ingredients { get; set; }
        public string PackageDesc1 { get; set; }
        public string PackageDesc2 { get; set; }
        public string PackageUnitAbbr { get; set; }
        public string NationalItemClass { get; set; }
        public string NationalItemClassID { get; set; }
        public string SubTeamName { get; set; }
        public string SubTeamNumber { get; set; }
        public string VendorName { get; set; }
        public string NationalFamily { get; set; }
        public int NationalFamilyId { get; set; }
        public string NationalCategory { get; set; }
        public int NationalCategoryId { get; set; }
        public string NationalSubCategory { get; set; }
        public int NationalSubCategoryId { get; set; }
        public string TaxClassHCID { get; set; }
        public string TaxClassDesc { get; set; }
    }
}
