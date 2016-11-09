using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Common;
using Icon.Web.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class PluRequestSearchViewModel
    {

        private IEnumerable<DropDownViewModel> requestStatuses;
        private SelectList allowedRequestStatuses;
        

        public PluRequestSearchViewModel()
        {
            requestStatuses = new List<DropDownViewModel>
            {
                new DropDownViewModel { Id = 1, Name = "All" },
                new DropDownViewModel { Id = 2, Name = "New" },
                new DropDownViewModel { Id = 3, Name = "Approved" },
                new DropDownViewModel { Id = 4, Name = "Rejected" }
            };

            SelectedStatusId = requestStatuses.First().Id;
            var allowedStatus = new List<string>() { "New", "Approved", "Rejected" };
            allowedRequestStatuses = new SelectList(allowedStatus);

        }
        public IEnumerable<SelectListItem> Status
        {
            get
            {
                return requestStatuses.ToSelectListItem();
            }
        }

        public SelectList AllowedStatus
        {
            get
            {
                return allowedRequestStatuses;
            }
        }

        [Display(Name = "Request Status")]
        public int SelectedStatusId { get; set; }

        public List<PluRequestViewModel> PluRequests { get; set; }
        public SelectList RetailUoms { get; set; }
        public List<HierarchyClassViewModel> BrandHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> NationalHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> FinanacialHierarchyClasses { get; set; }

    }
}