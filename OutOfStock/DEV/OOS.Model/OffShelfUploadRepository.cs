using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class OffShelfUploadRepository : IOffShelfUploadRepository
    {
        private List<OffShelfUpload> uploads;
        private IOOSEntitiesFactory entityFactory;
        public int Count {get { return uploads.Count; }}

        public OffShelfUploadRepository(IOOSEntitiesFactory entityFactory)
        {
            this.entityFactory = entityFactory;
            uploads = new List<OffShelfUpload>();
        }

        public void Add(OffShelfUpload newUpload)
        {
            if (uploads.Any(p => p.ScanDate == newUpload.ScanDate)) return;

            using (var dbContext = entityFactory.New())
            {
                var upcs = (from c in dbContext.REPORT_DETAIL where c.SCAN_DATE == newUpload.ScanDate select c.UPC).ToList();
                upcs.ForEach(newUpload.Scan);
            }

            uploads.Add(newUpload);
        }

        public IEnumerable<OffShelfUpload> FindAll()
        {
            using (var dbContext = entityFactory.New())
            {
                var scanDates = (from c in dbContext.REPORT_HEADER select c.CREATED_DATE).ToList();
                var offshelfUploads = scanDates.Select(p => new OffShelfUpload(p)).ToList();
                offshelfUploads.ForEach(Add);
            }
            return uploads.ToArray();
        }

        public IEnumerable<OffShelfUpload> FindBetween(string from, string to)
        {
            var fromDate = Convert.ToDateTime(from);
            var toDate = Convert.ToDateTime(to);
            using (var dbContext = entityFactory.New())
            {
                var selectedUploads = (from c in dbContext.REPORT_HEADER 
                                       where c.CREATED_DATE >= fromDate && c.CREATED_DATE <= toDate
                                       orderby c.CREATED_DATE
                                       select c.CREATED_DATE).Select(p => new OffShelfUpload(p)).ToList();
                selectedUploads.ForEach(Add);
            }
            return uploads.FindAll(p => p.ScanDate >= fromDate).FindAll(p => p.ScanDate <= toDate).ToArray();
        }

        public OffShelfUpload Find(string scanDate)
        {
            return Find(Convert.ToDateTime(scanDate));
        }

        public OffShelfUpload Find(DateTime scanDate)
        {
            if (!uploads.Any(p => p.ScanDate == scanDate))
            {
                using (var dbContext = entityFactory.New())
                {
                    var searched = (from c in dbContext.REPORT_HEADER where c.CREATED_DATE == scanDate select c).FirstOrDefault();
                    if (searched != null)
                    {
                        Add(new OffShelfUpload(scanDate));
                    }
                }
            }
            return uploads.FirstOrDefault(p => p.ScanDate == scanDate);
        }
    }
}
