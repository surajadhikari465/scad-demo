using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class AppDatabaseViewModel
    {
        public AppDatabaseViewModel() { }

        public AppDatabaseViewModel(DatabaseModel dbModel) : this()
        {
            ServerName = dbModel.ServerName;
            DatabaseName = dbModel.DatabaseName;
            ConnectionStringName = dbModel.ConnectionStringName;
            Environment = dbModel.Environment;
            Category = dbModel.Category;
            IsEntityFramework = dbModel.IsEntityFramework;
            IsUsedForLogging = dbModel.IsUsedForLogging;
        }

        [DisplayName("Description")]
        public string Summary
        {
            get
            {
                if (IsUsedForLogging)
                {
                    return $"Logging ({Category}-{Environment})";
                }
                else if (Category == DatabaseCategoryEnum.Encrypted)
                {
                    return "{Encrypted}";
                }
                else
                {
                    return $"{Category}-{Environment}";
                }
            }
        }

        [DisplayName("Server")]
        public string ServerName { get; set; }

        [DisplayName("Database")]
        public string DatabaseName { get; set; }

        [DisplayName("Connection String")]
        public string ConnectionStringName { get; set; }

        public EnvironmentEnum Environment { get; set; }

        public DatabaseCategoryEnum Category { get; set; }

        [DisplayName("Uses EF")]
        public bool IsEntityFramework { get; set; }

        public bool IsUsedForLogging { get; set; }
    }
}