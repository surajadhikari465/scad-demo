using System.Collections.Generic;

namespace OutOfStock.Classes
{
    public class SageCall
    {
        // ReSharper disable InconsistentNaming
        public string text { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
        public int current { get; set; }
        public int count { get; set; }
        public int start { get; set; }
        public int end { get; set; }

        public List<SageStore> Stores { get; set; }
    }
}