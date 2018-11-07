using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LogPurge
{
    public class LogLevels
    {
        /// <summary>
        /// LogLevels class to keep values from custom config file
        /// </summary>
        public LogLevels()
        { }

          public int Debug { get; set; }
          public int DebugArchive { get; set; }
          public int Info { get; set; }
          public int InfoArchive { get; set; }
          public int Warn { get; set; }
          public int WarnArchive { get; set; }
          public int Error { get; set; }
          public int ErrorArchive { get; set; }
          public int Fatal { get; set; }
          public int FatalArchive { get; set; }
          public int TimeOut { get; set; }
    }
    public enum enumLogLevels
    {
        Debug,
        DebugArchive,
        Info,
        InfoArchive,
        Warn,
        WarnArchive,
        Error,
        ErrorArchive,
        Fatal,
        FatalArchive,
        TimeOut
    }
}
