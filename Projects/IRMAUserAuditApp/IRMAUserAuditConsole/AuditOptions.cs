using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    //public enum IRMAEnvironment : int { Test, Dev, QualityAssurance, Production }
    //public enum UserAuditFunction : int { Backup, Restore, Import, Export, None }

    public class AuditOptions
    {

        #region Members / Properties

        private string connectionString;
        private string region;
        private UserAuditFunctionEnum function;
        private IRMAEnvironmentEnum environment;
        public bool IsError
        {
            get
            {
                return !String.IsNullOrWhiteSpace(ErrorMessage);
            }
        }
        public bool IsWarning
        {
            get
            {
                return !String.IsNullOrWhiteSpace(WarningMessage);
            }
        }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }

        public IRMAEnvironmentEnum Environment
        {
            get { return environment; }
        }

        public UserAuditFunctionEnum Function
        {
            get { return function; }
            set { function = value; }
        }

        public string Region
        {
            get { return region; }
            set { 
                region = value;
               // connectionString = DefaultConnectionString(environment).Replace("XX", region);
            }
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }


        //private string DefaultConnectionString(IRMAEnvironment? env)
        //{
        //    switch (env)
        //    {
        //        case IRMAEnvironment.Test:
        //            return @"Data Source=idt-XX\XXt;Initial Catalog=ItemCatalog_test;Integrated Security=True";
        //        case IRMAEnvironment.Dev:
        //            return @"Data Source=idd-XX\XXd;Initial Catalog=ItemCatalog_Dev;Integrated Security=True";
        //        case IRMAEnvironment.QualityAssurance:
        //            return @"Data Source=idq-XX\XXq;Initial Catalog=ItemCatalog;Integrated Security=True";
        //    }
        //    return @"Data Source=idp-XX\XXp;Initial Catalog=ItemCatalog;Integrated Security=True";
        //}
        #endregion

        public AuditOptions() { }

        public AuditOptions(string _region, string _env, string _conString) : this()
        {
            SetOptions(_region, _env, _conString);
        }

        public void SetOptions(string _region, string _env, string _conString)
        {
            this.region = _region;
            //function = ConvertStringToFunction(_function);
            this.environment = ConvertStringToEnvironment(_env);
            this.connectionString = _conString; // DefaultConnectionString(ConvertStringToEnvironment(_env)).Replace("XX", region);
        }

        public static UserAuditFunctionEnum ConvertStringToFunction(string _funcIn)
        {
            switch ((_funcIn ?? String.Empty).ToLower())
            {
                case "backup":
                    return UserAuditFunctionEnum.Backup;
                case "restore":
                    return UserAuditFunctionEnum.Restore;
                case "import":
                    return UserAuditFunctionEnum.Import;
                case "export":
                    return UserAuditFunctionEnum.Export;
                default:
                    return UserAuditFunctionEnum.None;
            }
        }

        public static IRMAEnvironmentEnum ConvertStringToEnvironment(string _envIn)
        {
            switch ((_envIn??String.Empty).ToUpper())
            {
                case "QA":
                case "QUALITYASSURANCE":
                    return IRMAEnvironmentEnum.QualityAssurance;
                case "DEV":
                    return IRMAEnvironmentEnum.Dev;
                case "TEST":
                    return IRMAEnvironmentEnum.Test;
                case "PROD":
                default:
                    return IRMAEnvironmentEnum.Production;
            }
        }

        // These convert the enum to the ShortName values in AppConfigEnv
        public static string ConvertIRMAEnvironmentToString(IRMAEnvironmentEnum _envIn)
        {
            switch (_envIn)
            {
                case IRMAEnvironmentEnum.Dev:
                    return "DEV";
                case IRMAEnvironmentEnum.QualityAssurance:
                    return "QA";
                case IRMAEnvironmentEnum.Test:
                    return "TEST";
                case IRMAEnvironmentEnum.Production:
                default:
                    return "PROD";
            }
        }

    }
}
