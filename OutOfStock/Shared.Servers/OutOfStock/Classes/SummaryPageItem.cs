using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutOfStock.Classes
{
    public class SummaryPageItem
    {
        public string Name { get; set; }
        public int OosCount { get; set; }
        public double OosPercent { get; set; }
        public string Type { get; set; }
        public int Parent { get; set; }
        public int Id { get; set; }
        public string HasChildren { get; set; }
        public string Collapsed { get; set; }
        public int scanCount { get; set; }
        public string type { get; set; }
    }
}
