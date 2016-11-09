using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Helpers
{
    public static class Utils
    {

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
                case Icon.Dashboard.DataFileAccess.Models.EnvironmentEnum.Dev:
                    environmentClass = "primary";
                    break;
                case Icon.Dashboard.DataFileAccess.Models.EnvironmentEnum.Test:
                    environmentClass = "info";
                    break;
                case Icon.Dashboard.DataFileAccess.Models.EnvironmentEnum.QA:
                    environmentClass = "warning";
                    break;
                case Icon.Dashboard.DataFileAccess.Models.EnvironmentEnum.Prod:
                    environmentClass = "danger";
                    break;
                case Icon.Dashboard.DataFileAccess.Models.EnvironmentEnum.Undefined:
                default:
                    environmentClass = "default";
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
        public static IList<SelectListItem> GetDataFlowSystemSelections()
        {
            var systemItems = new List<SelectListItem>();
            foreach (var system in Enum.GetValues(typeof(DataFlowSystemEnum)).Cast<DataFlowSystemEnum>())
            {
                if (system == DataFlowSystemEnum.None) continue;
                var selectItem = new SelectListItem()
                {
                    Text = system.ToString(),
                    Value = ((int)system).ToString()
                };
            }
            return systemItems;
        }

        //public static int GetIdParameterFronUrl(Uri currentUri)
        //{
        //    int requestIdParameter = 0;
        //    const string parameterName = "id";
        //    // look for an id parameter in the query string
        //    var requestQueryDictionary = HttpUtility.ParseQueryString(currentUri.Query);
        //    if (requestQueryDictionary != null && requestQueryDictionary.Count > 0 && requestQueryDictionary[parameterName] != null)
        //    {
        //        Int32.TryParse(requestQueryDictionary[parameterName], out requestIdParameter);
        //    }
        //    // did we not find a value in the query?
        //    if (requestIdParameter < 1)
        //    {
        //        // look in the url itself
        //        var lastSegment = currentUri.Segments.Last();
        //        if (!String.IsNullOrWhiteSpace(lastSegment))
        //        {
        //            Int32.TryParse(lastSegment, out requestIdParameter);
        //        }
        //    }
        //    return requestIdParameter;
        //}

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
    }
}