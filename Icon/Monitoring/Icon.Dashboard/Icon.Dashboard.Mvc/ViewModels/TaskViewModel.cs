using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class TaskViewModel : IconApplicationViewModel
    {
        public TaskViewModel() : base() { }

        public TaskViewModel(ScheduledTask app) : base(app) { }

        public override void PopulateFromApplication(IApplication app)
        {
            base.PopulateFromApplication(app);
            var task = (ScheduledTask)app;
            this.LastRun = task.LastRun;
            this.NextRun = task.NextRun;
        }

        [DisplayName("Last Run")]
        public DateTime? LastRun { get; set; }

        [DisplayName("Next Run")]
        public DateTime? NextRun { get; set; }        
    }
}