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
            {
                StackTrace stackTrace = new StackTrace(true);
                if (stackTrace != null)
                {
                    StackFrame stackFrame = stackTrace.GetFrame(2);
                    if (stackFrame != null)
                    {
                        fileName = stackFrame.GetFileName();
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            fileName = fileName.Replace(oosBasePath, string.Empty);
                            lineNumberText = stackFrame.GetFileLineNumber().ToString();
                        }
                        MethodBase methodBase = stackFrame.GetMethod();
                        if (methodBase != null)
                        {
                            methodName = methodBase.Name;
                            if (string.IsNullOrEmpty(fileName))
                            {
                                fileName = (methodBase.ReflectedType == null ? "?" :  methodBase.ReflectedType.FullName);
                                lineNumberText = "?";
                            }
                        }
                    }
                }
            }

            string sessionID = string.Empty;
            if (this.getSessionId != null)
                sessionID = getSessionId();
            if (string.IsNullOrWhiteSpace(sessionID))
                sessionID = "NoSession";
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");

            // '(' <date> ' ' <time with ms> ')' <separator> <filename> '[' <line number>
            // ']' <separator> <log level> '[' <sessionid> ']' <separator> <message>
            string logEntry = "(" + timeStamp + ")" +
                oosLogSeparator + fileName + "[" + lineNumberText + "]" +
                oosLogSeparator + level.ToString() + "[" + sessionID + "]" +
                oosLogSeparator + message;
            oosLogger.Log(level, logEntry);
            if (echoEntry != null)
                echoEntry(level, timeStamp, fileName, lineNumberText, methodName, sessionID, message);
        }

    }

}