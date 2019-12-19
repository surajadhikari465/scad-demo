using Icon.Web.Common;
using System.Linq;

namespace Icon.Web.Mvc.Models
{
    public class NationalClassSearchViewModel
    {        
        public IQueryable<NationalClassGridViewModel> NationalClasses { get; set; }

        public Enums.WriteAccess UserWriteAccess { get; set; }
    }
}