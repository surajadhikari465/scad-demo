using Icon.Web.Attributes;
using Icon.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Web.Mvc.Models
{
    public class PluMappingViewModel
    {
        public int ItemId { get; set; }

        [Display(Name = "National PLU")]
        public string NationalPlu { get; set; }
        public string PluType { get; set; }
        public string Brand { get; set; }

        [Display(Name = "PLU Description")]
        public string PluDescription { get; set; }
        
        [Display(Name = "Florida PLU")]
        [Plu]
        public string flPLU { get; set; }

        
        [Display(Name = "Mid-Atlantic PLU")]
        [Plu]
        public string maPLU { get; set; }

        [Display(Name = "Midwest PLU")]
        [Plu]
        public string mwPLU { get; set; }

        [Display(Name = "North Atlantic PLU")]
        [Plu]
        public string naPLU { get; set; }

        [Display(Name = "Northern California PLU")]
        [Plu]
        public string ncPLU { get; set; }

        [Display(Name = "Northeast PLU")]
        [Plu]
        public string nePLU { get; set; }

        [Display(Name = "Pacific Northwest PLU")]
        [Plu]
        public string pnPLU { get; set; }

        [Display(Name = "Rocky Mountain PLU")]
        [Plu]
        public string rmPLU { get; set; }

        [Display(Name = "South PLU")]
        [Plu]
        public string soPLU { get; set; }

        [Display(Name = "Southern Pacific PLU")]
        [Plu]
        public string spPLU { get; set; }

        [Display(Name = "Southwest PLU")]
        [Plu]
        public string swPLU { get; set; }

        [Display(Name = "United Kingdom PLU")]
        [Plu]
        public string ukPLU { get; set; }

        public PluMappingViewModel() { }

        public PluMappingViewModel(Item item)
        {
            ItemId = item.itemID;
            NationalPlu = item.ScanCode.Single().scanCode;
            PluType = item.ScanCode.Single().ScanCodeType.scanCodeTypeDesc;

            var brandQuery = item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Brands);
            Brand = brandQuery.Count() == 0 ? String.Empty : brandQuery.Single().HierarchyClass.hierarchyClassName;

            var pluDescriptionQuery = item.ItemTrait.Where(it => it.Trait.traitCode == TraitCodes.ProductDescription);
            PluDescription = pluDescriptionQuery.Count() == 0 ? String.Empty : pluDescriptionQuery.Single().traitValue;

            flPLU = item.PLUMap.flPLU ?? String.Empty;
            maPLU = item.PLUMap.maPLU ?? String.Empty;
            mwPLU = item.PLUMap.mwPLU ?? String.Empty;
            naPLU = item.PLUMap.naPLU ?? String.Empty;
            ncPLU = item.PLUMap.ncPLU ?? String.Empty;
            nePLU = item.PLUMap.nePLU ?? String.Empty;
            pnPLU = item.PLUMap.pnPLU ?? String.Empty;
            rmPLU = item.PLUMap.rmPLU ?? String.Empty;
            soPLU = item.PLUMap.soPLU ?? String.Empty;
            spPLU = item.PLUMap.spPLU ?? String.Empty;
            swPLU = item.PLUMap.swPLU ?? String.Empty;
            ukPLU = item.PLUMap.ukPLU ?? String.Empty;
        }
    }
}