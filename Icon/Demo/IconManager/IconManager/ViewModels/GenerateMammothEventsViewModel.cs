using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconManager.ViewModels
{
    public class GenerateMammothEventsViewModel
    {
        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }
    }
}
