using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class AppServerViewModel
    {
        public AppServerViewModel() { }

        [Required]
        public string ServerName { get; set; }

        public int id { get; set; }
        public int parentId { get; set; }
    }
}