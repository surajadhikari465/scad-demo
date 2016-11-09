using System.Collections.Generic;

namespace Icon.Esb.EwicAplListener.Common.Models
{
    public class AuthorizedProductListModel
    {
        public string MessageXml { get; set; }
        public List<EwicItemModel> Items { get; set; }
    }
}
