using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class Store
    {
        public const string Closed = "Closed";
        public const string Open = "Open";

        public int Id { get; private set; }


        public string Name { get; set; }
        public string Abbrev { get; set; }
        public Store(int id)
        {
            Id = id;
        }

        public string Status { get; set; }
        public int RegionId { get; set; }
        public string BusinessUnitNo { get; set; }
    }
}
