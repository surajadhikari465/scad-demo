using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class DashboardEnvironmentCollectionViewModel
    {
        public DashboardEnvironmentCollectionViewModel()
        {
            Environments = new List<DashboardEnvironmentViewModel>();
        }
        //public DashboardEnvironmentCollectionViewModel(int count)
        //{
        //    Environments = new List<DashboardEnvironmentViewModel>(count);
        //}

        //private int selectedEnvIndex = 0;

        //public int SelectedEnvIndex
        //{
        //    get
        //    {
        //        return selectedEnvIndex;
        //    }
        //    set
        //    {
        //        if (value > Environments.Count) throw new IndexOutOfRangeException();
        //        for (int i=0; i<Environments.Count; i++)
        //        {
        //            Environments[i].Selected = i == value;
        //        }
        //        selectedEnvIndex = value;
        //    }
        //

        public int SelectedEnvIndex { get; set; }

        public List<DashboardEnvironmentViewModel> Environments { get; set; }
    }
}