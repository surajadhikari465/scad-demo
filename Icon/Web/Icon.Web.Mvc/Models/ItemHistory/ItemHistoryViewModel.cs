using Icon.Common;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Models
{

    public class ItemHistoryViewModel
    {
        public List<AttributeDisplayModel> Attributes { get; set; } = new List<AttributeDisplayModel>();
        public string GetAttributeDisplayName(string attributeName)
        {
            if (attributeName == Constants.Attributes.ItemTypeCode)
            {
                return "Item Type";
            }
            else
            {
                return this.Attributes.FirstOrDefault(x => x.AttributeName == attributeName)?.DisplayName ?? attributeName;
            }
        }
        public List<RevisionByDate> RevisionsByDate = new List<RevisionByDate>();
        public Dictionary<string, List<RevisionByAttribute>> RevisionsByAttribute = new Dictionary<string, List<RevisionByAttribute>>();
    }
}