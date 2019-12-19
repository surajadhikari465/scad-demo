using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSImport
{
    public class ImportThreadProgressArgs : EventArgs
    {
        public enum ProgressType : int { None = 0, Info = 1, Starting = 2, Stopping = 3, FileStart = 4, FileFailed = 5, FileComplete = 6 }
        public ProgressType progressType { get; private set; }
        public NLog.LogLevel logLevel { get; private set; }
        public string logEntry { get; private set; }
        public object other { get; private set; }

        public ImportThreadProgressArgs(ProgressType progressType, NLog.LogLevel logLevel, string logEntry, object other)
        {
            this.progressType = progressType;
            this.logLevel = logLevel;
            this.logEntry = logEntry;
            this.other = other;
        }
    }
}
