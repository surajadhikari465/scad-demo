using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Infrastructure.ItemSearch
{
    /// <summary>
    /// SearchCriteria is responsible for parsing search query strings and for holding the criteria that will be used by the 
    /// ItemSearchQueryBuilder to build SQL statements
    /// </summary>
    public class ItemSearchCriteria
    {
        public string AttributeName { get; private set; }
        public List<string> Values { get; private set; } = new List<string>();
        public AttributeSearchOperator SearchOperator { get; private set; }

        public ItemSearchCriteria(string attributeName, AttributeSearchOperator searchOperator, string query)
        {
            this.AttributeName = attributeName;
            this.SearchOperator = searchOperator;
            this.Values = this.ParseValues(searchOperator, query);
        }

        private string Sanitize(string input)
        {
            return input.Replace("'", "''").Replace("%","");
        }

        /// <summary>
        /// ParseValues is responsible for parsing a query string into a list of trimmed values.
        /// We replace " with ` and then replace all spaces with the ^ characters. We then split the 
        /// string by ^ and clean up the values. This lets us handle cases where there may be spaces in a quoted section of the query like
        /// Texas "New York" Oklahoma. This will return Texas,New York,Oklahoma. 
        /// </summary>
        /// <param name="searchOperator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private List<string> ParseValues(AttributeSearchOperator searchOperator, string values)
        {
            if (string.IsNullOrWhiteSpace(values))
            {
                return new List<string>();
            }

            // When searching using ExactlyMatchesAll we want to return the exact string they searched for
            // except we're stripping out " in case the user searched that way. 
            if (searchOperator == AttributeSearchOperator.ExactlyMatchesAll)
            {
                if (this.AttributeName == "ItemId")
                {
                    if (int.TryParse(values, out _))
                    {
                        return new List<string>() { values.Replace("\"", "") };
                    }
                    else
                    {
                        return new List<string>() { };
                    }
                }
                else
                {
                    return new List<string>() { values.Replace("\"", "") };
                }
            }


            // Replace quotes with the back tick. This makes the string parsing easier due to how the escape charactere are added to strings.
            // Then split string into individual tokens. Cat Dog "Water Buffalo is broken into 3 tokens Cat,Dog,Water Buffalo. 
            // The tokens are delimited by spaced but we allow double quotes around entries that contain spaces but is really one entry. 
            Func<string, string[]> replaceCharactersInStringWithBackTickAndReplaceSpacesWithCarot = (input) =>
            {
                input = input.Replace("\"", "`").Trim();
                // convert our string to a char array so we can modify individual characters
                char[] characters = input.ToCharArray();

                bool inQuotes = false;

                for (int i = 0; i < input.Length; i++)
                {
                    if (!inQuotes && input[i] == ' ') // if we're not in a quoted part of the string replace space with the ^ character
                    {
                        characters[i] = '^';
                    }

                    // if we found a backtick which is our quote identifier set the inQuotes var so we know if we should replace spaces later in this for loop
                    if (input[i] == '`')
                    {
                        if (inQuotes)
                        {
                            inQuotes = false;
                        }
                        else
                        {
                            inQuotes = true;
                        }
                    }
                }
                return new string(characters).Split('^');
            };

           
            // itemId can only be numbers because in the sql query we itemId is an int. Process
            // all of the entries and remove any that can't be casted to an int. 
            Func<string[],List<string>> processItemIdLogic = (input) =>
            {
                List<string> response = new List<string>();
                foreach (var item in input)
                {
                    string replacedItem = item.Replace("`", "");
                    if (!string.IsNullOrWhiteSpace(replacedItem))
                    {
                        if (this.AttributeName == "ItemId")
                        {
                            if (int.TryParse(replacedItem, out _))
                            {
                                response.Add(replacedItem);
                            }
                        }
                        else
                        {
                            response.Add(replacedItem);
                        }
                    }
                }
                return response;
            };
     
            return processItemIdLogic(replaceCharactersInStringWithBackTickAndReplaceSpacesWithCarot(this.Sanitize(values)));
        }
    }

    public enum AttributeSearchOperator
    {
        ContainsAny,
        ContainsAll,
        ExactlyMatchesAny,
        ExactlyMatchesAll,
        HasAttribute,
        DoesNotHaveAttribute
    }
}
