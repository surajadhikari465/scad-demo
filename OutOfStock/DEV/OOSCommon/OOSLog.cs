using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace OOSCommon
{

    public class OOSLog : IOOSLog
    {
        private const string oosLogSeparator = " ";
        private const string oosLogQuote = "";
        public delegate string GetSessionId();
        public delegate void EchoEntry(NLog.LogLevel level, string timeStamp, string fileName, string lineNumberText, string methodName, string sessionID, string message);

        public NLog.Logger oosLogger { get; set; }
        protected string oosBasePath { get; set; }
        protected GetSessionId getSessionId { get; set; }
        protected EchoEntry echoEntry { get; set; }

        public OOSLog(string oosNLogLoggerName, string oosBasePath, GetSessionId getSessionId, EchoEntry echoEntry)
        {
            this.oosLogger = NLog.LogManager.GetLogger(oosNLogLoggerName);
            this.oosBasePath = oosBasePath;
            this.getSessionId = getSessionId;
            this.echoEntry = echoEntry;
        }

       


        public void Debug(string message) { LogInner(LogLevel.Debug, message); }
        public void Error(string message) { LogInner(LogLevel.Error, message); }
        public void Fatal(string message) { LogInner(LogLevel.Fatal, message); }
        public void Info(string message) { LogInner(LogLevel.Info, message); }
        public void Log(LogLevel level, string message) { LogInner(level, message); }
        public void Trace(string message) { LogInner(LogLevel.Trace, message); }
        public void Warn(string message) 
        { LogInner(LogLevel.Warn, message); }

        /// <summary>
        /// Log message at level in the following format
        /// '(' <date> <separator> <time to ms> ')' <separator> <filename> '[' <line number>
        /// ']' <separator> <log level> '[' <sessionid> ']' <separator> <message>
        /// The BA was advised that this format, particularly using ' ' as a separator and not 
        /// quoting filenames and the message, would result in a log file that could not be readily
        /// imported into Excel
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected void LogInner(LogLevel level, string message)
        {
            string fileName = string.Empty;
            string methodName = string.Empty;
            string lineNumberText = string.Empty;
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
            string logEntry = "[" + timeStamp + "] " + level.ToString().PadRight(5, ' ');
            logEntry = logEntry + oosLogSeparator + message;
            oosLogger.Log(level, logEntry);
        }

    }

}