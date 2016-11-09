using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ServiceViewModel : IconApplicationViewModel
    {
        public ServiceViewModel() : base() { }

        public ServiceViewModel(IApplication app) : base(app)
        {
        }
    }
}