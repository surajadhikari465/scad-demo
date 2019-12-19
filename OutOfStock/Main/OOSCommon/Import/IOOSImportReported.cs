using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{
    public enum OOSImportIsMyFormat : int { NotDetermined, Yes, No, Maybe, NotSupported }

    public interface IOOSImportReported
    {
        OOSImportIsMyFormat IsMyFormat(string filePath);
        bool Import(string filePath);
    }
}
