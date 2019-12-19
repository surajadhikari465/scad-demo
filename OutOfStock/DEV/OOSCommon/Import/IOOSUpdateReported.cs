using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{
    public interface IOOSUpdateReported
    {
        bool BeginBatch(DateTime dtScan);
        bool WriteUPCs(List<string> upc);
    }
}
