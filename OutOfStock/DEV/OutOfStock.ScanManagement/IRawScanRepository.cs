using System.Collections.Generic;

namespace OutOfStock.ScanManagement
{
    public interface IRawScanRepository
    {
        void SaveRawScan(string message);
        IEnumerable<RawScanData> GetNextScans(int count);
        void SetScansAsFailed(int[] ids);
        void SetScanAsComplete(int id, long elapsedMs);

        long ProcessRawScan(ScanData rawScan);

        string[] RegionNames();
        string[] StoreNamesFor(string regionName);
        string[] RegionAbbreviations();
        string[] StoreAbbreviationsFor(string regionAbbrev);
        string GetConfiguration(string region, string store, string sessionId);
        void Login(string username, string useremail, string region, string store, string sessionId, string ipAddress);
        bool ValidateRegionStore(string region, string store, string sessionId);
    }
}
