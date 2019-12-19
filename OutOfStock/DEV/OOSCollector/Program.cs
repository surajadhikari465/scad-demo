using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;
using OOSCommon.OOSCollector;

namespace OOSCollector
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        public const string helpMessage =
            "OOSCollector usage\r\n" +
            "\r\n" +
            "OOSCollector\r\n" +
            "   Run in interactive mode\r\n" +
            "OOSCollector /Help\r\n" +
            "   Display help and quit no matter where /Help is on the command line\r\n" +
            "OOSCollector [/Validate] [/Autorun]\r\n" +
            "   where /Validate suppresses any database writes or updates\r\n" +
            "   and /Autorun runs an import scan and exits without user intervention\r\n" +
            "\r\n" +
            "Keywords can be abbreviated to the first letter\r\n" +
            "/Validate anywhere on the command line applies to the entire run\r\n";

        public static OOSCollectorUI formOOSCollectorUI { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Redirect console to parent process; Must be before any calls to Console
            AttachConsole(ATTACH_PARENT_PROCESS);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppConfig.oosLogging = new OOSCommon.OOSLog(AppConfig.oosNLogLoggerName, 
                AppConfig.nLogBasePath, null, new OOSCommon.OOSLog.EchoEntry(OOSEchoEntry));

            foreach (string arg in args)
            {
                switch (arg.ToLower())
                {
                    case "/?":
                    case "/h":
                    case "/help":
                        Console.Write(helpMessage);
                        return;
                    case "/v":
                    case "/validate":
                        AppConfig.isValidationMode = true;
                        break;
                    case "/a":
                    case "/autorun":
                        AppConfig.isAutorunMode = true;
                        break;
                    default:
                        if (arg.Length < 1 || arg[0] == '/')
                        {
                            Console.Write("Unknown option: " + arg + " (use /h for help)\r\n");
                            return;
                        }
                        Console.Write("Extra parameter: " + arg + " (use /h for help)\r\n");
                        return;
                }
            }
            Bootstrap();
            formOOSCollectorUI = new OOSCollectorUI();
            formOOSCollectorUI.checkBox_Validate.Checked = AppConfig.isValidationMode;
            formOOSCollectorUI.checkBox_Validate.Enabled = !AppConfig.isAutorunMode;
            formOOSCollectorUI.checkBox_Autorun.Checked = AppConfig.isAutorunMode;
            Application.Run(formOOSCollectorUI);
        }

        /// <summary>
        /// Delegate for logging to console and form testbox
        /// </summary>
        /// <param name="logEntry"></param>
        public static void OOSEchoEntry(NLog.LogLevel level, string timeStamp, string fileName,
            string lineNumberText, string methodName, string sessionID, string message)
        {
            LogLevel[] levelsEnabled = new LogLevel[] {NLog.LogLevel.Debug, 
                NLog.LogLevel.Error, NLog.LogLevel.Fatal,
                NLog.LogLevel.Info, NLog.LogLevel.Warn};
            if (!levelsEnabled.Contains(level))
                return;
            string strEntry = DateTime.Now.ToString("hh:mm:ss.fff") + " " + level.ToString() +
                " " + message + "\r\n";
            Console.Write(strEntry);
            if (formOOSCollectorUI != null)
            {
                // Cross thread -- so you don't get the cross theading exception
                if (formOOSCollectorUI.InvokeRequired)
                {
                    formOOSCollectorUI.BeginInvoke((MethodInvoker)delegate
                    {
                        formOOSCollectorUI.textBox_Log.Text += strEntry;
                        formOOSCollectorUI.Update();
                    });
                    return;
                }
                formOOSCollectorUI.textBox_Log.Text += strEntry;
                formOOSCollectorUI.Update();
            }
        }

        private static void Bootstrap()
        {
            Bootstrapper.Bootstrap();
        }

    }
}
