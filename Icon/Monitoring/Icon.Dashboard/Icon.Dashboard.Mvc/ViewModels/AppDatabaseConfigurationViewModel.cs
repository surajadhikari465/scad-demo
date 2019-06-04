using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{

    public class AppDatabaseConfigurationViewModel
    {
        public AppDatabaseConfigurationViewModel()
        {
            this.Databases = new List<AppDatabaseViewModel>();
        }

        public AppDatabaseConfigurationViewModel(ApplicationDatabaseConfiguration dbConfig) : this()
        {
            foreach( var db in dbConfig.Connections)
            {
                Databases.Add(new AppDatabaseViewModel(db));
            }
            Summary = dbConfig.Summary;
            LoggingSummary = dbConfig.LoggingSummary;
        }

        public List<AppDatabaseViewModel> Databases { get; set; }

        public string Summary { get; set; }

        public string LoggingSummary { get; set; }
    }
}