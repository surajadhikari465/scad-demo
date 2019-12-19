using System;
using System.Collections.Generic;
using System.Linq;

namespace OOS.Model
{
    public class OOSCustomReport
    {
        private List<CustomReportEntry> entries = new List<CustomReportEntry>();
        private DateTime startDate;
        private DateTime endDate;

        public OOSCustomReport(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public void Add(CustomReportEntry reportEntry)
        {
            entries.Add(reportEntry);
        }

        public IEnumerable<string> ReportUPCs()
        {
            return entries.Select(d => d.UPC).Distinct();
        }

        public IEnumerable<CustomReportEntry> Entries()
        {
            return entries.ToList();
        }

        public int Days { get { return Math.Abs(endDate.Subtract(startDate).Days); } }
    }
}
