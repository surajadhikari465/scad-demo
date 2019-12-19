using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class FinancialGridViewModel
    {
        public IEnumerable<FinancialViewModel> SubTeams { get; set; }
    }
}