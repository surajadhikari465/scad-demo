using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace OOSImport
{
    public class ImportThreadLogging : OOSCommon.IOOSLog
    {
        public ImportThread importThread { get; set; }

        public ImportThreadLogging(ImportThread importThread)
        {
            this.importThread = importThread;
        }

        public void Debug(string message) { Log(LogLevel.Debug, message); }
        public void Error(string message) { Log(LogLevel.Error, message); }
        public void Fatal(string message) { Log(LogLevel.Fatal, message); }
        public void Info(string message) { Log(LogLevel.Info, message); }
        public void Trace(string message) { Log(LogLevel.Trace, message); }
        public void Warn(string message) { Log(LogLevel.Warn, message); }

        public void Log(NLog.LogLevel loggingLevel, string logEntry)
        {
            importThread.OnProgressChanged(new ImportThreadProgressArgs(
                ImportThreadProgressArgs.ProgressType.Info, loggingLevel, logEntry, null));
        }
    }
}
