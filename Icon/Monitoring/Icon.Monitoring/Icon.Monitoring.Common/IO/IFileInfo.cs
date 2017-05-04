using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.IO
{
    public interface IFileInfo
    {
        string Name { get; }
        DateTime CreationTime { get; }
        bool Exists { get; }
        DateTime LastWriteTime { get; }
    }
}
