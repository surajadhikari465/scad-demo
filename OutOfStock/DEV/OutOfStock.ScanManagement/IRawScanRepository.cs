using System.Collections.Generic;

namespace OutOfStock.ScanManagement
{
    public interface IRawScanRepository
    {
        void SaveRawScan(string message);
        IEnumerable<RawScanData> GetNextScans(int count);
        void SetScansAsFailed(int[] ids);
        void SetScansAsComplete(int[] ids);
    }
}
