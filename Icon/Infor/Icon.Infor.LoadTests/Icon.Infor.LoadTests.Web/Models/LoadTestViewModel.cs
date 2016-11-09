using Icon.Infor.LoadTests.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Infor.LoadTests.Web.Models
{
    public class LoadTestViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }

        [Display(Name="Elapsed Time")]
        public string ElapsedTime { get; set; }

        #region Configuration

        [Display(Name="Number of Entities")]
        public int EntityCount { get; set; }

        [Display(Name="Run Time")]
        public string TestRunTime { get; set; }

        [Display(Name="Data Interval")]
        public string PopulateTestDataInterval { get; set; }

        [Display(Name="Email")]
        [DataType(DataType.MultilineText)]
        public string EmailRecipients { get; set; }

        #endregion

        public LoadTestViewModel()
        {
        }

        public LoadTestViewModel(string name, string status, string elapsedTime)
        {
            this.Name = name;
            this.Status = status;
            this.ElapsedTime = elapsedTime;
        }

        public LoadTestViewModel(LoadTestModel test)
        {
            this.Name = test.Name;
            this.Status = test.Status;
            this.TestRunTime = test.TestRunTime;
            this.PopulateTestDataInterval = test.PopulateTestDataInterval;
            this.ElapsedTime = test.ElapsedTime;
        } 
    }
}