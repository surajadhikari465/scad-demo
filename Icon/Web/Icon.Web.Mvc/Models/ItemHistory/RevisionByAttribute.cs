using Icon.Web.Mvc.Models.ItemHistory;

namespace Icon.Web.Mvc.Models
{

    /// <summary>
    /// A single change made to an attribute
    /// </summary>
    public class RevisionByAttribute : AbstractRevision
    {
        public string NewValue { get; set; }
    }
}