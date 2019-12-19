using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OOS.Model
{
    public class OffShelfUpload
    {
        private DateTime uploadDate;
        private List<OffShelfScan> scans;
        public DateTime ScanDate { get { return uploadDate; } }

        public int Count { get { return scans.Count; } }

        public OffShelfUpload(DateTime uploadDate)
        {
            scans = new List<OffShelfScan>();
            this.uploadDate = uploadDate;
        }

        public OffShelfUpload(string scanDate)
        {
            uploadDate = Convert.ToDateTime(scanDate);
            scans = new List<OffShelfScan>();
        }

        public void Scan(string upc)
        {
            var scan = new OffShelfScan(upc);
            scans.Add(scan);
        }

        public IEnumerable<OffShelfScan> Scans
        {
            get { return scans.ToArray();} 
        }
    }
}
