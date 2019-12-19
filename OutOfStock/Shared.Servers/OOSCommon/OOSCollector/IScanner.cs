using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.OOSCollector
{
    public interface IScanner
    {
        string GetRegionFirst();
        string GetRegionNext();
        string GetRegion();
        OOSCommon.DataContext.STORE GetStoreFirst(string regionAbbreviation);
        OOSCommon.DataContext.STORE GetStoreNext();
        OOSCommon.DataContext.STORE GetStore();
        string GetFileFirst(string regionAbbreviation, string storeAbbreviation);
        string GetFileNext();
        string GetFile();
        void SetFileDone(string uploadedFile, bool hasError, string regionAbbreviation, string storeAbbreviation);
    }
}
