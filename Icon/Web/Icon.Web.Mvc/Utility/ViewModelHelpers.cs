using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Utility
{
    public static class ViewModelHelpers
    {
        public static SelectList BuildYesOrNoSelectList(bool includeBlankSpace = true)
        {
            if (includeBlankSpace)
            {
                return new SelectList(new[] { String.Empty, "No", "Yes"}, String.Empty);
            }
            else 
            {
                return new SelectList(new[] { "No", "Yes"}, "No");
            }
        }

        public static SelectList BuildSelectListFromDictionary(Dictionary<int, string> dictionary, bool includeBlankSpace = false, bool orderValuesAlphabetically = false)
        {
            var dictionaryValues = dictionary.Select(kvp => new SelectListItem
                {
                    Value = kvp.Key.ToString(),
                    Text = kvp.Value
                });

            if(orderValuesAlphabetically)
            {
                dictionaryValues = dictionaryValues.OrderBy(v => v.Text);
            }

            if (includeBlankSpace)
            {
                dictionaryValues = Enumerable.Repeat(new SelectListItem
                    {
                        Value = null,
                        Text = String.Empty
                    }, 1)
                    .Concat(dictionaryValues);
            }

            return new SelectList(dictionaryValues, "Value", "Text",  String.Empty);
        }
    }
}