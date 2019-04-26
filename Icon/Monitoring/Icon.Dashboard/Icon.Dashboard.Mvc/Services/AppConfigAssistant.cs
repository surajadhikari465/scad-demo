using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public static class AppConfigAssistant
    {
        public static void RewriteAllAppSettings(string configFilePath, Dictionary<string,string> appSettingsDictionary)
        {
            try
            {
                var appConfig = XDocument.Load(configFilePath);

                // create XML elements for each setting
                var updatedElements = appSettingsDictionary.Select(i =>
                        new XElement("add",
                            new XAttribute("key", i.Key),
                            new XAttribute("value", i.Value ?? String.Empty)));

                //replace the existing appSettings node in the XML file with the new element collection
                var configAppSettingsElement = appConfig.Root.Element("appSettings");
                configAppSettingsElement.ReplaceNodes(updatedElements);
                appConfig.Save(configFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Dictionary<T, U> CombineDictionariesIgnoreDuplicates<T, U>(params Dictionary<T, U>[] dictionaries)
        {
            // In case there are any duplicates between the two dictionaries, convert the 
            // combined dictionary to a lookup (which handles multiple values per key) and 
            // then re -convert back to a dictionary using only the first value per key
            var combinedDictionaryWithDuplicatesIgnored = dictionaries.SelectMany(dict => dict)
                       .ToLookup(pair => pair.Key, pair => pair.Value)
                       .ToDictionary(group => group.Key, group => group.First());
            return combinedDictionaryWithDuplicatesIgnored;
        }
    }
}