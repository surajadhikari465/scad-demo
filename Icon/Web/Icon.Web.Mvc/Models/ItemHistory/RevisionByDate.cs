using Icon.Web.Mvc.Models.ItemHistory;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{

    /// <summary>
    /// All changes made on this item save.
    /// </summary>
    public class RevisionByDate : AbstractRevision
    {
        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>(); //key:attributeName value:attributeValue
    }
}