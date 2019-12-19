using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfStock.Models
{
    public class ColumnDataModel
    {
        public string Name;  // text displayed to user.
        public string DataName; // property name to pull data from
        public bool CenterDetailText;
        public bool CenterHeaderText;
        public bool isString;


        public ColumnDataModel(string Name, string DataName)
        {
            this.Name = Name;
            this.DataName = DataName;
        }

        public ColumnDataModel(string Name, string DataName, bool CenterDetailText=false, bool CenterHeaderText=false, bool isString=false)
        {
            this.Name = Name;
            this.DataName = DataName;
            this.CenterDetailText = CenterDetailText;
            this.CenterHeaderText = CenterHeaderText;
            this.isString = isString;
        }

    }
}