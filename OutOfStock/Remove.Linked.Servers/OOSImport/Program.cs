using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OOSImport.Test")]

namespace OOSImport
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        public const string oosEFConnectionStringName = "OOSEntities";
        public const string oosConnectionStringName = "OOSConnectionString";
        public const string oosVIMServiceNameAppSetting = "VIM_SERVICE_NAME";
        public const string oosMovementServiceNameAppSetting = "MOVEMENT_SERVICE_NAME";
        public const string oosNLogBasePathAppSetting = "NLogBasePath";
        public const string oosNLogLoggerName = "NLogEventLog";
        public const string helpMessage =
            "OOSImport usage\r\n" +
            "\r\n" +
            "OOSImport\r\n" +
            "   Run in interactive mode\r\n" +
            "OOSImport /Help\r\n" +
            "   Display help and quit no matter where /Help is on the command line\r\n" +
            "OOSImport [/Validate] [/Date:<scan date>] /Format:<format> /Store:<store id> <file1> <file2> ...\r\n" +
            "   where /Validate suppresses any database writes or updates\r\n" +
            "   and <scan date> is the scan date mm/dd/yyyy needed for formats 'T' and   'U'\r\n" +
            "   and <store id> is a store PS_BY number or a store abbreviation\r\n" +
            "   and <format> is 'W' for WIMP, 'T' for Tagnetics, or 'U' for raw UPC without check digit\r\n" +
            "\r\n" +
            "Keywords can be abbreviated to the first letter\r\n" +
            "/Validate anywhere on the command line applies to the entire run\r\n" +
            "/Date, /Format and /Store can be repeated and apply to the files that follow\r\n";

        public static bool isValidationMode = false;

        public static string oosEFConnectionString
        {
            get
            {
                if (_oosEFConnectionString.Length < 1)
                    _oosEFConnectionString = OOSCommon.AppConfig.ConnectionStrings[oosEFConnectionStringName].ConnectionString;
                return _oosEFConnectionString;
            }
            set { _oosEFConnectionString = value; }
        } static string _oosEFConnectionString = string.Empty;
        public static OOSCommon.VIM.VIMRepository vimRepository
        {
            get
            {
                if (_vimRepository == null)
                {
                    try
                    {
                        string oosVIMServiceName =
                            OOSCommon.AppConfig.AppSettings[oosVIMServiceNameAppSetting];
                        string oosConnectionString =
                            OOSCommon.AppConfig.ConnectionStrings[oosConnectionStringName].ConnectionString;
                        _vimRepository =
                            new OOSCommon.VIM.VIMRepository(oosConnectionString, oosVIMServiceName, oosLogging);
                    }
                    catch (Exception)
                    {
                        // TODO: Log error
                    }
                }
                return _vimRepository;
            }
            set { _vimRepository = value;  }
        } static OOSCommon.VIM.VIMRepository _vimRepository = null;
        public static OOSCommon.Movement.IMovementRepository movementRepository
        {
            get
            {
                if (_movementRepository == null)
                {
                    try
                    {
                        string oosMovementServiceName =
                            OOSCommon.AppConfig.AppSettings[oosMovementServiceNameAppSetting];
                        string oosConnectionString =
                            OOSCommon.AppConfig.ConnectionStrings[oosConnectionStringName].ConnectionString;
                        _movementRepository =
                            new OOSCommon.Movement.MovementRepository(oosConnectionString, oosMovementServiceName, oosLogging);
                    }
                    catch (Exception)
                    {
                        // TODO: Log error
                    }
                }
                return _movementRepository;
            }
            set { _movementRepository = value; }
        } static OOSCommon.Movement.IMovementRepository _movementRepository = null;
        public static string nLogBasePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_nLogBasePath))
                {
                    _nLogBasePath = string.Empty;
                    try
                    {
                        _nLogBasePath =
                            OOSCommon.AppConfig.AppSettings[oosNLogBasePathAppSetting];
                    }
                    catch (Exception) { }
                    if (string.IsNullOrWhiteSpace(_nLogBasePath))
                        _nLogBasePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return _nLogBasePath;
            }
            set { _nLogBasePath = value; }
        } static string _nLogBasePath = string.Empty;
        public static OOSCommon.IOOSLog oosLogging
        {
            get
            {
                if (_oosLogging == null)
                    _oosLogging = new OOSCommon.OOSLog(oosNLogLoggerName, nLogBasePath, null,
                        new OOSCommon.OOSLog.EchoEntry(OOSEchoEntry));
                return _oosLogging;
            }
            set { _oosLogging = value; }
        } static OOSCommon.IOOSLog _oosLogging = null;
        public static OOSImportUI formOOSImportUI { get; set; }

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

            foreach (string arg in args.Where(t => t.StartsWith("/")).Select(t => t))
            {
                int ix = arg.IndexOf(':');
                switch (arg.Substring(0,ix < 1 ? arg.Length : ix).ToLower())
                {
                    case "/?":
                    case "/h":
                    case "/help":
                        Console.Write(helpMessage);
                        return;
                    case "/v":
                    case "/validate":
                        isValidationMode = true;
                        break;
                    case "/f":
                    case "/format":
                        if (ix > 0 && arg.Length > (ix + 1))
                        {
                            switch (arg.Substring(ix + 1).Trim().ToLower())
                            {
                                case "t":
                                case "tagnetics":
                                case "u":
                                case "upc":
                                case "w":
                                case "wimp":
                                    break;
                                default:
                                    ix = -1;
                                    break;
                            }
                            if (ix <= 0)
                            {
                                Console.Write("Unrecognized format value: " + arg + "\r\n");
                                Console.Write(helpMessage);
                                return;
                            }
                        }
                        break;
                    case "/s":
                    case "/store":
                        if (arg.IndexOf(':') <= 0)
                        {
                            Console.Write("Store value not provided: " + arg + "\r\n");
                            Console.Write(helpMessage);
                            return;
                        }
                        break;
                    case "/d":
                    case "/date":
                        if (arg.IndexOf(':') <= 0)
                        {
                            Console.Write("Date value not provided: " + arg + "\r\n");
                            Console.Write(helpMessage);
                            return;
                        }
                        break;
                    default:
                        Console.Write("Unknown option: " + arg + "\r\n");
                        Console.Write(helpMessage);
                        return;
                }
            }

            formOOSImportUI = new OOSImportUI();
            if (args.Length < 1)
                Application.Run(formOOSImportUI);
            else
            {
                ImportBatch importBatch = new ImportBatch(args, isValidationMode, oosLogging);
                while (importBatch.ImportNextFile())
                    ;
            }
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
            if (formOOSImportUI != null)
            {
                // Cross thread -- so you don't get the cross theading exception
                if (formOOSImportUI.InvokeRequired)
                {
                    formOOSImportUI.BeginInvoke((MethodInvoker)delegate
                    {
                        formOOSImportUI.textBox_Log.Text += strEntry;
                        formOOSImportUI.Update();
                    });
                    return;
                }
                formOOSImportUI.textBox_Log.Text += strEntry;
                formOOSImportUI.Update();
            }
        }

    }
}
