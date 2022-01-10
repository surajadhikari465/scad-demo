using System.Text.RegularExpressions;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    /// <summary>
    /// This class is responsible for parsing hierarchy names and ids into the correct format for ESB messages.
    /// </summary>
    public class HierarchyValueParser : IHierarchyValueParser
    {
        public string ParseHierarchyNameForContract(string hierarchyClassName, int hierarchyClassId, int hierarchyId)
        {
            string parsedContractClassId = this.ParseHierarchyClassIdForContract(hierarchyClassName, hierarchyClassId, hierarchyId);

            if (hierarchyId.Equals(Framework.Hierarchies.Financial))
            {
                return parsedContractClassId == "0000" ? "na" : hierarchyClassName;
            }
            else
            {
                return hierarchyClassName;
            }
        }

        public string ParseHierarchyClassIdForContract(string hierarchyClassName, int hierarchyClassId, int hierarchyId)
        {
            if (string.IsNullOrWhiteSpace(hierarchyClassName) || hierarchyClassId <= 0)
            {
                return string.Empty;
            }

            string response = string.Empty;

            if (hierarchyId.Equals(Framework.Hierarchies.Tax))
            {
                response = hierarchyClassName.Split(' ')[0];
            }
            else if (hierarchyId.Equals(Icon.Framework.Hierarchies.Financial))
            {
                // matching numeric characters inside the parenthesis
                // the regex string uses lookbehind (?<=...) and lookahead (?=...) syntax
                Match match = Regex.Match(hierarchyClassName, @"(?<=\()\d+?(?=\))");
                response = match.Value;
            }
            else
            {
                response = hierarchyClassId.ToString();
            }

            return response;
        }
    }
}