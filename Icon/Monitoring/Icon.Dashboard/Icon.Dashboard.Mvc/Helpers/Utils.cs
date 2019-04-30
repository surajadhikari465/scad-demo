using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Helpers
{
    public static class Utils
    {
        public static string GetBootstrapClassForEnvironment()
        {
            return GetBootstrapClassForEnvironment(Utils.Environment);
        }

        public static string GetBootstrapClassForEnvironment(string environment)
        {
            var parsedEnum = EnvironmentEnum.Undefined;
            Enum.TryParse<EnvironmentEnum>(environment, out parsedEnum);
            return GetBootstrapClassForEnvironment(parsedEnum);
        }

        public static string GetBootstrapClassForEnvironment(EnvironmentEnum environment)
        {
            string environmentClass = "default";
            switch (environment)
            {
                case EnvironmentEnum.Dev:
                    environmentClass = "default";
                    break;
                case EnvironmentEnum.Test:
                    environmentClass = "primary";
                    break;
                case EnvironmentEnum.QA:
                    environmentClass = "warning";
                    break;
                case EnvironmentEnum.Perf:
                    environmentClass = "info";
                    break;
                case EnvironmentEnum.Prd:
                    environmentClass = "danger";
                    break;
                case EnvironmentEnum.Undefined:
                default:
                    environmentClass = "basic";
                    break;
            }
            return environmentClass;
        }

        public static string GetBootstrapClassForLevel(string level)
        {
            string levelClass = "secondary";
            switch (level.ToLower())
            {
                case "error":
                    levelClass = "danger";
                    break;
                case "warning":
                    levelClass = "warning";
                    break;
                case "info":
                    levelClass = "info";
                    break;
                case "debug":
                    levelClass = "primary";
                    break;
                default:
                    break;
            }
            return levelClass;
        }

        public static string GetIdParameterFronUrl(Uri currentUri)
        {
            string requestIdParameter = String.Empty;
            const string parameterName = "id";
            // look for an id parameter in the query string
            var requestQueryDictionary = HttpUtility.ParseQueryString(currentUri.Query);
            if (requestQueryDictionary != null && requestQueryDictionary.Count > 0 && requestQueryDictionary[parameterName] != null)
            {
                requestIdParameter = requestQueryDictionary[parameterName].ToString();
            }
            // did we not find a value in the query?
            if (!String.IsNullOrWhiteSpace(requestIdParameter))
            {
                // look in the url itself
                requestIdParameter = currentUri.Segments.Last();
            }
            return requestIdParameter;
        }

        public static string Environment
        {
            get
            {
                return ConfigurationManager.AppSettings["activeEnvironment"] ?? "localhost";
            }
        }

        public static string DataFileName
        {
            get
            {
                return ConfigurationManager.AppSettings["pathToXmlDataFile"] ?? DefaultDataFileName;
            }
        }

        private const string DefaultDataFileName = "SampleDataFile.xml";

        public static string GetPathForDataFile(HttpServerUtilityBase serverUtility, string dataFileName)
        //string pathToXsdSchema = "Applications.xsd", bool validateXml = true)
        {
            if (serverUtility == null) throw new ArgumentNullException(nameof(serverUtility));
            if (String.IsNullOrWhiteSpace(dataFileName)) throw new ArgumentNullException(nameof(dataFileName));

            // only map path on the server if it is not already mapped!
            var mappedDataFilePath = dataFileName.Contains("App_Data")
                    ? dataFileName
                    : serverUtility.MapPath("~/App_Data/" + dataFileName);

            if (!File.Exists(mappedDataFilePath))
            {
                throw new FileNotFoundException(
                    String.Format("Unable to find or read application data file for dashboard ('{0}')", mappedDataFilePath)
                    , mappedDataFilePath);
            }

            return mappedDataFilePath;
        }

        public const StringComparison StrcmpOption = StringComparison.InvariantCultureIgnoreCase;

        public static int ServiceCommandTimeout
        {
            get
            {
                var stringVal = ConfigurationManager.AppSettings["serviceCommandTimeoutMilliseconds"];
                if (!int.TryParse(stringVal, out int val)) val = 10000;
                return val;
            }
        }
        
        public static bool GetMammothDbEnabledFlag()
        {
            bool.TryParse(ConfigurationManager.AppSettings["mammothDatabaseEnabled"], out bool isEnabled);
            return isEnabled;
        }
    }
}