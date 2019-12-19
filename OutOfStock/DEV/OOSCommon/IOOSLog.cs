using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace OOSCommon
{
    public interface IOOSLog
    {
        void Debug(string message);
        void Error(string message);
        void Fatal(string message);
        void Info(string message);
        void Log(LogLevel level, string message);
        void Trace(string message);
        void Warn(string message);

        
    }
}
