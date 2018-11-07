using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WFM.Helpers
{
    public class Log
    {
        #region Members (huhuhuh....you said 'members')

        private string fileName = "default.log";
        private bool timeStamp = false;
        private FileInfo file = null;

        #endregion

        #region ctors
        /// <summary>
        /// Create a new instance of the Log class with default options
        /// (filename = "default.log" and timestamping is OFF)
        /// </summary>
        public Log()
        {
            SetupFile();
        }
        /// <summary>
        /// Create a new instance of the Log class with the default
        /// filename (default.log) and specifying whether or not to timestamp
        /// </summary>
        /// <param name="_timeStamp">whether to timestamp log messages</param>
        public Log(bool _timeStamp)
        {
            timeStamp = _timeStamp;
            SetupFile();
        }
        /// <summary>
        /// Create a new instance of the Log class, specifying filename
        /// and whether or not to timestamp
        /// </summary>
        /// <param name="_fileName">Filename to write log to</param>
        /// <param name="_timeStamp">whether to timestamp log messages</param>
        public Log(string _fileName, bool _timeStamp)
        {
            fileName = _fileName;
            _timeStamp = timeStamp;
            SetupFile(); 
        }

        #endregion

        #region Log Methods

        /// <summary>
        /// write a plain message to the log
        /// </summary>
        /// <param name="message">the message to write</param>
        public void Message(string message)
        {
            try
            {
                StreamWriter sw = file.AppendText();
                sw.WriteLine(GetTimestamp() + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                // TODO:  decide what to do with this.
                // if writing to the log errors, I can't
                // exactly write the error to the log
                // now can I?  :)
            }
        }
        /// <summary>
        /// write a warning level message to log.  Will prefix message with ## WARNING ##
        /// </summary>
        /// <param name="message">the message to write</param>
        public void Warning(string message)
        {
            Message("## WARNING ## " + message);
        }
        /// <summary>
        /// write an error level message to log.  Will prefix message with !! ERROR !!
        /// </summary>
        /// <param name="message">the message to write</param>
        public void Error(string message)
        {
            Message("!! ERROR !! " + message);
        }

        #endregion

        #region Methods

        private void SetupFile()
        {
            if (file != null)
                file = null;
            file = new FileInfo(fileName);

        }
        /// <summary>
        /// Get the timestamp string.  If timeStamp == false, returns an empty string.
        /// </summary>
        /// <returns></returns>
        private string GetTimestamp()
        {
            if (timeStamp)
            {
                DateTime now = DateTime.Now;
                StringBuilder sb = new StringBuilder("[");
                sb.Append(now.ToString("s"));
                sb.Append("] ");
                return sb.ToString();
            }
            return "";
        }

        #endregion

        #region Props
        /// <summary>
        /// The Log Filename
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                SetupFile();
            }
        }

        /// <summary>
        /// Turn timestamping on or off
        /// </summary>
        public bool TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        #endregion
    }
}
