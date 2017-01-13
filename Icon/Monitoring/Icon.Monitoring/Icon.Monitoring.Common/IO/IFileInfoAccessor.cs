using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.IO
{
    public interface IFileInfoAccessor
    {
        IFileInfo GetFileInfo(string path);
    }
}
