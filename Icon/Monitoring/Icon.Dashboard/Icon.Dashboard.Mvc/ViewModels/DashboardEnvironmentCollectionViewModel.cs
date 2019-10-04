﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class DashboardEnvironmentCollectionViewModel
    {
        public DashboardEnvironmentCollectionViewModel()
        {
            Environments = new List<EnvironmentViewModel>();
        }

        public int SelectedEnvIndex { get; set; }

        public List<EnvironmentViewModel> Environments { get; set; }
    }
}