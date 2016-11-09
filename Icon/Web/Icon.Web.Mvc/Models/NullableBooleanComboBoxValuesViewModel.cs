using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class NullableBooleanComboBoxValuesViewModel
    {
        public List<Tuple<bool?, string>> Values { get; set; }

        public NullableBooleanComboBoxValuesViewModel()
        {
            Values = new List<Tuple<bool?, string>>
                {
                    new Tuple<bool?, string>(null, null),
                    new Tuple<bool?, string>(true, "Y"),
                    new Tuple<bool?, string>(false, "N")
                };
        }
    }
}