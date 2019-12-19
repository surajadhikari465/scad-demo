using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class PluRequestEditViewModel
    {
        private List<string> requestStatuses;
        
        public PluRequestEditViewModel()
        {
            requestStatuses = new List<string>
            {
                "New",
                "Approved",
               "Rejected" 
            };

            

        }
        public IEnumerable<SelectListItem> Status
        {
            get
            {
                return new SelectList(requestStatuses, "New");
            }
        }

        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }

        public int PluRequestId { get; set; }        

        [Display(Name = "National PLU")]       
        [ScanCode]
        public string NationalPLU { get; set; }

        [Display(Name = "Brand Name")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmpty = false)]
        public string BrandName { get; set; }

        [Display(Name = "Product Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.ProductDescription, CanBeNullOrEmpty = false)]
        public string ProductDescription { get; set; }

        [Display(Name = "POS Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.PosDescription, CanBeNullOrEmpty = false)]
        public string PosDescription { get; set; }

        [Display(Name = "Item Pack")]
        [PackageUnit]
        [Required]
        public string PackageUnit { get; set; }

        [Display(Name = "Size")]
        [RetailSize]
        [Required]
        public string RetailSize { get; set; }

        [Display(Name = "UOM")]
        [RetailUom]
        public string RetailUom { get; set; }
        public SelectList RetailUoms { get; set; }

        [Display(Name = "PLU Type")]
        public string PluType { get; set; }
        public SelectList PluTypes { get; set; }
      

        [Display(Name = "SubTeam")]
        public string SubTeamName { get; set; }

        public int? MerchandiseHierarchyClassId { get; set; }

        public int? FinancialHierarchyClassId { get; set; }
        public IEnumerable<SelectListItem> FinanacialHierarchyClasses { get; set; }
       
        public int? NationalHierarchyClassId { get; set; }
        public IEnumerable<SelectListItem> NationalHierarchyClasses { get; set; }

        [Display(Name = "National Class")]
        public string NationalHierarchyName { get; set; }

        [Display(Name = "Requsted By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Notes")]
        public string Note { get; set; }

        public List<PluRequestChangeHistoryViewModel> PluRequestChangeHistory { get; set; }
        

        public PluRequestEditViewModel(PLURequest pluRequest)
        {
            requestStatuses = new List<string>
            {                
                "New",
                "Approved",
               "Rejected" 
            };

            PluRequestId = pluRequest.pluRequestID;
            NationalPLU = pluRequest.nationalPLU;
            BrandName = pluRequest.brandName;
            ProductDescription = pluRequest.itemDescription;
            PosDescription = pluRequest.posDescription;
            PackageUnit = pluRequest.packageUnit.ToString();
            RetailSize = pluRequest.retailSize.ToString();
            RetailUom = pluRequest.retailUom; 
            FinancialHierarchyClassId = pluRequest.FinancialClassID;
            MerchandiseHierarchyClassId = pluRequest.merchandiseClassID;
            NationalHierarchyClassId = pluRequest.nationalClassID;
            RequestedBy = pluRequest.requesterName;
            RequestStatus = pluRequest.requestStatus;

        }
    }
}