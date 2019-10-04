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
        public AppServerViewModel(string name) : this()
        {
            ServerName = name;
        }

        [Required]
        public string ServerName { get; set; }

        /// <summary>
        /// used in javascript model for Vue.js
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// used in javascript model for Vue.js
        /// </summary>
        public int parentId { get; set; }
    }
}