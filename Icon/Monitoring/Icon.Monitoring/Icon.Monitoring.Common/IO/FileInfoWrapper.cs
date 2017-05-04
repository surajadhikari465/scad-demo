using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.IO
{
    public class FileInfoWrapper : IFileInfo
    {
        private FileInfo fileInfo;
        private string path;

        public string Name => fileInfo.Name;
        public DateTime CreationTime => fileInfo.CreationTime;
        public bool Exists => fileInfo.Exists;
        public DateTime LastWriteTime => fileInfo.LastWriteTime;

        public FileInfoWrapper(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }
    }
}
