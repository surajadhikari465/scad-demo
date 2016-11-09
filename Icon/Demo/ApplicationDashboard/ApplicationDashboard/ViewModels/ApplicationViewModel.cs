using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDashboard.Models;

namespace ApplicationDashboard.ViewModels
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel(Application application)
        {
            Id = application.Id;
            Name = application.Name;
            DisplayName = application.DisplayName;
            MachineName = application.MachineName;
            Status = application.Status;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string MachineName { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
    }
}
