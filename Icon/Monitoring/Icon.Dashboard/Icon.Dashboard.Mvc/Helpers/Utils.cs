using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                case EnvironmentEnum.Dev0:
                case EnvironmentEnum.Dev1:
                    environmentClass = "default";
                    break;
                case EnvironmentEnum.Tst0:
                case EnvironmentEnum.Tst1:
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
                case EnvironmentEnum.Custom:
                    environmentClass = "success";
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

        public static string Environment
        {
            get
            {
                return ConfigurationManager.AppSettings["activeEnvironment"] ?? "localhost";
            }
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
        
        public static List<string> SplitCommaSeparatedValuesToList(string commaSeparatedString)
        {
            var separator = new char[] { ',' };
            var splitValues = new List<string>();
            if (!String.IsNullOrWhiteSpace(commaSeparatedString))
            {
                splitValues.AddRange(
                    commaSeparatedString.Split(separator, StringSplitOptions.RemoveEmptyEntries));
            }
            return splitValues;
        }

        public static string[] SplitCommaSeparatedValuesToArray(string commaSeparatedString)
        {
            var separator = new char[] { ',' };
            var splitValues = new string[] { commaSeparatedString };
            if (!String.IsNullOrWhiteSpace(commaSeparatedString))
            {
                splitValues = commaSeparatedString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return splitValues;
        }

        public static Dictionary<T, U> CombineDictionariesIgnoreDuplicates<T, U>(params Dictionary<T, U>[] dictionaries)
        {
            if (dictionaries == null)
            {
                return new Dictionary<T, U>();
            }
            for (int i=0; i<dictionaries.Length; i++)
            {
                if (dictionaries[i]==null)
                {
                    dictionaries[i] = new Dictionary<T, U>();
                }
            }
            // In case there are any duplicates between the two dictionaries, convert the 
            // combined dictionary to a lookup (which handles multiple values per key) and 
            // then re -convert back to a dictionary using only the first value per key
            var combinedDictionaryWithDuplicatesIgnored = dictionaries.SelectMany(dict => dict)
                       .ToLookup(pair => pair.Key, pair => pair.Value)
                       .ToDictionary(group => group.Key, group => group.First());
            return combinedDictionaryWithDuplicatesIgnored;
        }

        public static List<string> SplitHostsFromServerUrlSetting(string serverUrlAppSettingValue)
        {
            if (String.IsNullOrWhiteSpace(serverUrlAppSettingValue)) return null;
            var hosts = new List<string>();
            // server url app setting could contain a single url or two urls separated by a comma
            var serverUrls = serverUrlAppSettingValue.Split(',');
            foreach (var serverUrl in serverUrls)
            {
                var systemUri = new Uri(serverUrl);
                if (systemUri.Host.Contains("."))
                {
                    //we only want the first element of the domain host (e.g. "myMachine" out of "myMachine.wfm.pvt")
                    hosts.Add(systemUri.Host.Split('.')[0].ToUpper());
                }
                else
                {
                    hosts.Add(systemUri.Host.ToUpper());
                }
            }
            return hosts;
        }

        public static EsbEnvironmentEnum ParseEsbEnvironment(string esbEnvironmentName)
        {
            // the name should match the enum
            Enum.TryParse(esbEnvironmentName, out EsbEnvironmentEnum esbEnum);
            // ... but in case it doesn't
            if (esbEnum == EsbEnvironmentEnum.None)
            {
                // let's do this the hard way
                if (esbEnvironmentName.ContainsCaseInsensitve("Dev"))
                {
                    if (esbEnvironmentName.ContainsCaseInsensitve("Dup"))
                    {
                        esbEnum = EsbEnvironmentEnum.DEV_DUP;
                    }
                    else
                    {
                        esbEnum = EsbEnvironmentEnum.DEV;
                    }
                }
                else if (esbEnvironmentName.ContainsCaseInsensitve("Test")
                    || esbEnvironmentName.ContainsCaseInsensitve("Tst"))
                {
                    if (esbEnvironmentName.ContainsCaseInsensitve("Dup"))
                    {
                        esbEnum = EsbEnvironmentEnum.TEST_DUP;
                    }
                    else
                    {
                        esbEnum = EsbEnvironmentEnum.TEST;
                    }
                }
                else if (esbEnvironmentName.ContainsCaseInsensitve("QA"))
                {
                    if (esbEnvironmentName.ContainsCaseInsensitve("Dup"))
                    {
                        esbEnum = EsbEnvironmentEnum.QA_DUP;
                    }
                    else if (esbEnvironmentName.ContainsCaseInsensitve("Perf"))
                    {
                        esbEnum = EsbEnvironmentEnum.QA_PERF;
                    }
                    else
                    {
                        esbEnum = EsbEnvironmentEnum.QA_FUNC;
                    }
                }
                else if (esbEnvironmentName.ContainsCaseInsensitve("Prd")
                    || esbEnvironmentName.ContainsCaseInsensitve("Prod"))
                {
                    esbEnum = EsbEnvironmentEnum.PRD;
                }
            }
            return esbEnum;
        }

        public static void ThrowIfFileNotFound(string externalConfigFilePath, string activityForErrorMessage )
        {
            VerifyExternalFilePath(externalConfigFilePath, activityForErrorMessage, true);
        }

        public static bool VerifyExternalFilePath(string externalConfigFilePath, string activityForErrorMessage, bool shouldThrowException = true)
        {
            if (string.IsNullOrEmpty(externalConfigFilePath))
            {
                if (shouldThrowException)
                {
                    throw new ArgumentNullException(nameof(externalConfigFilePath),
                        $"Error attempting to {activityForErrorMessage}. File path parameter must be provided but was null/empty.");
                }
                return false;
            }
            if (!File.Exists(externalConfigFilePath))
            {
                if (shouldThrowException)
                {
                    throw new FileNotFoundException
                        ($"Error attempting to {activityForErrorMessage}. File not found- path provided: \"", $"{externalConfigFilePath}\"");
                }
                return false;
            }
            return true;
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsCaseInsensitve(this string source, string toCheck)
        {
            var caseInsensitiveComparisonType = StringComparison.CurrentCultureIgnoreCase;
            return source?.IndexOf(toCheck, caseInsensitiveComparisonType) >= 0;
        }
    }
}