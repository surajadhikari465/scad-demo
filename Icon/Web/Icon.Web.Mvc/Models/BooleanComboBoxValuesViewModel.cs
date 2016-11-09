using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class BooleanComboBoxValuesViewModel
    {
        public List<Tuple<bool?, string>> Values { get; set; }

        public BooleanComboBoxValuesViewModel()
        {
            Values = new List<Tuple<bool?, string>>
                {
                    new Tuple<bool?, string>(true, "Y"),
                    new Tuple<bool?, string>(false, "N")
                };
        }
    }
}