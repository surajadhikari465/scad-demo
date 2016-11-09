using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class BulkScanCodeSearchViewModel
    {
        public BulkScanCodeSearchViewModel()
        {
            ItemSearchResults = new ItemSearchResultsViewModel();
        }

        public BulkScanCodeSearchTextAreaViewModel TextAreaViewModel { get; set; }

        public BulkScanCodeSearchFileUploadViewModel FileUploadViewModel { get; set; }

        public List<string> InvalidOrNotFoundScanCodes { get; set; }

        public int BulkScanCodeSearchLimit { get; set; }

        public int OverLimitScanCodeCount { get; set; }

        public ItemSearchResultsViewModel ItemSearchResults { get; set; }
    }
}