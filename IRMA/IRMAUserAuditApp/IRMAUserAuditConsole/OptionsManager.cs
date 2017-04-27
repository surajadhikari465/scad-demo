using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    public enum IRMAEnvironment : int { Test, Dev, QualityAssurance, Production }
    public enum UserAuditFunction : int { Backup, Restore, Import, Export, None }

    class OptionsManager
    {

        #region Members / Properties

        private string connectionString;
        private string region;
        private UserAuditFunction function;
        private IRMAEnvironment environment;

        public IRMAEnvironment Environment
        {
            get { return environment; }
        }

        public UserAuditFunction Function
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

        public OptionsManager(string _region, string _env,string _conString)
        {
            region = _region;
            //function = ConvertStringToFunction(_function);
            environment = ConvertStringToEnvironment(_env);
            connectionString = _conString; // DefaultConnectionString(ConvertStringToEnvironment(_env)).Replace("XX", region);
        }

        public UserAuditFunction ConvertStringToFunction(string _funcIn)
        {
            switch (_funcIn.ToLower())
            {
                case "backup":
                    return UserAuditFunction.Backup;
                case "restore":
                    return UserAuditFunction.Restore;
                case "import":
                    return UserAuditFunction.Import;
                case "export":
                    return UserAuditFunction.Export;
                default:
                    return UserAuditFunction.None;
            }
        }

        public static IRMAEnvironment ConvertStringToEnvironment(string _envIn)
        {
            switch (_envIn.ToUpper())
            {
                case "QA":
                    return IRMAEnvironment.QualityAssurance;
                case "DEV":
                    return IRMAEnvironment.Dev;
                case "TEST":
                    return IRMAEnvironment.Test;
                case "PROD":
                default:
                    return IRMAEnvironment.Production;
            }
        }

        // These convert the enum to the ShortName values in AppConfigEnv
        public static string ConvertIRMAEnvironmentToString(IRMAEnvironment _envIn)
        {
            switch (_envIn)
            {
                case IRMAEnvironment.Dev:
                    return "DEV";
                case IRMAEnvironment.QualityAssurance:
                    return "QA";
                case IRMAEnvironment.Test:
                    return "TEST";
                case IRMAEnvironment.Production:
                default:
                    return "PROD";
            }
        }

    }
}
