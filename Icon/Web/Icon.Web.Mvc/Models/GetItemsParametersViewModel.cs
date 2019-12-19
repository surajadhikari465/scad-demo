using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class GetItemsParametersViewModel
    {
        public List<GetItemsAttributesParameters> GetItemsAttributesParameters { get; set; }
        public GetItemsParametersViewModel()
        {
            this.GetItemsAttributesParameters = new List<GetItemsAttributesParameters>();
        }
    }
}