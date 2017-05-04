using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.IO
{
    public class FileInfoAccessor : IFileInfoAccessor
    {
        public IFileInfo GetFileInfo(string path)
        {
            return new FileInfoWrapper(new FileInfo(path));
        }
    }
}
