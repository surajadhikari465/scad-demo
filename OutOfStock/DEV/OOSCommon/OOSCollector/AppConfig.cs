using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using StructureMap;

namespace OOSCommon.OOSCollector
{
    public class AppConfig
    {
        /// <summary>
        /// This must be initialized
        /// </summary>
        public static OOSCommon.IOOSLog oosLogging { get; set; }

        public const string oosEFConnectionStringName = "OOSEntities";
        public const string oosConnectionStringName = "OOSConnectionString";

        public const string oosVIMServiceNameAppSetting = "VIM_SERVICE_NAME";
        public const string oosMovementServiceNameAppSetting = "MOVEMENT_SERVICE_NAME";
        public const string oosUploadedBasePathAppSetting = "UploadedBasePath";
        public const string oosReportedOOSPostImportMoveToPathAppSetting = "ReportedOOSPostImportMoveToPath";
        public const string oosReportedOOSPostImportDeleteAppSetting = "ReportedOOSPostImportDelete";
        public const string oosKnownOOSPostImportMoveToPathAppSetting = "KnownOOSPostImportMoveToPath";
        public const string oosKnownOOSPostImportDeleteAppSetting = "KnownOOSPostImportDelete";
        public const string oosRegionPrefixAppSetting = "RegionPrefix";
        public const string oosStorePrefixAppSetting = "StorePrefix";
        public const string oosNLogBasePathAppSetting = "NLogBasePath";
        public const string oosUNFIFtpURLAppSetting = "UnfiDataFtp";
        public const string oosValidationModeAppSetting = "ValidationMode";
        public const string oosAutorunModeAppSetting = "AutorunMode";
        public const string oosRunOnTimerAppSetting = "RunOnTimer";
        public const string oosRunTimeAppSetting = "RunTime";     // value="17:00" comment="24 hour clock hh:mm"
        public const string oosRunDaysAppSetting = "RunDays";     // value="All" comment="Sun,Mon,Tue,Wed,Thu,Fri,Sat,All"


        public const string oosNLogLoggerName = "NLogEventLog";

        public static string oosConnectionString
        {
            get
            {
                if (_oosConnectionString.Length < 1)
                    _oosConnectionString = OOSCommon.AppConfig.ConnectionStrings[oosConnectionStringName].ConnectionString;
                return _oosConnectionString;
            }
            set { _oosConnectionString = value; }
        } static string _oosConnectionString = string.Empty;
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
        public static OOSCommon.VIM.IVIMRepository vimRepository
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
            set { _vimRepository = value; }
        } static OOSCommon.VIM.IVIMRepository _vimRepository = null;
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

        public static bool isValidationMode
        {
            get
            {
                if (!_isValidationMode.HasValue)
                    _isValidationMode = OOSCommon.AppConfig.AppSettings[oosValidationModeAppSetting].Equals("true", StringComparison.OrdinalIgnoreCase);
                return _isValidationMode.GetValueOrDefault(false); ;
            }
            set { _isValidationMode = value; }
        } static bool? _isValidationMode = null;

        public static bool isAutorunMode
        {
            get
            {
                if (!_isAutorunMode.HasValue)
                    _isAutorunMode = OOSCommon.AppConfig.AppSettings[oosAutorunModeAppSetting].Equals("true", StringComparison.OrdinalIgnoreCase);
                return _isAutorunMode.GetValueOrDefault(false); ;
            }
            set { _isAutorunMode = value; }
        } static bool? _isAutorunMode = null;
        public static bool isRunOnTimer
        {
            get
            {
                if (!_isRunOnTimer.HasValue)
                    _isRunOnTimer = OOSCommon.AppConfig.AppSettings[oosRunOnTimerAppSetting].Equals("true", StringComparison.OrdinalIgnoreCase);
                return _isRunOnTimer.GetValueOrDefault(false); ;
            }
            set { _isRunOnTimer = value; }
        } static bool? _isRunOnTimer = null;
        public static TimeSpan runTime
        {
            get
            {
                if (!_runTime.HasValue)
                {
                    TimeSpan ts;
                    if (TimeSpan.TryParse(OOSCommon.AppConfig.AppSettings[oosRunTimeAppSetting], out ts))
                        _runTime = ts;
                    if (!_runTime.HasValue || _runTime.Value.TotalHours > 23)
                        _runTime = TimeSpan.FromHours(0);
                }
                return _runTime.GetValueOrDefault(TimeSpan.FromHours(0));
            }
            set { _runTime = value; }
        } static TimeSpan? _runTime = null;

        public static ERunDays runDays
        {
            get
            {
                if (!_runDays.HasValue)
                {
                    string[] days = OOSCommon.AppConfig.AppSettings[oosRunDaysAppSetting].Split(new char[] { ',' });
                    if (days.Length > 0)
                    {
                        _runDays = ERunDays.none;
                        foreach (string day in days)
                        {
                            ERunDays eDay = ERunDays.none;
                            if (Enum.TryParse<ERunDays>(day.ToLower(), out eDay))
                                _runDays = _runDays.Value | eDay;
                        }
                    }
                }
                return _runDays.GetValueOrDefault(ERunDays.none);
            }
            set { _runDays = value; }
        } static ERunDays? _runDays = null;

        public static string uploadedBasePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_uploadedBasePath))
                    _uploadedBasePath = OOSCommon.AppConfig.AppSettings[oosUploadedBasePathAppSetting];
                return _uploadedBasePath;
            }
            set { _uploadedBasePath = value; }
        } static string _uploadedBasePath = string.Empty;
        public static string reportedOOSPostImportMoveToPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_reportedOOSPostImportMoveToPath))
                    _reportedOOSPostImportMoveToPath = OOSCommon.AppConfig.AppSettings[oosReportedOOSPostImportMoveToPathAppSetting];
                return _reportedOOSPostImportMoveToPath;
            }
            set { _reportedOOSPostImportMoveToPath = value; }
        } static string _reportedOOSPostImportMoveToPath = string.Empty;
        public static bool reportedOOSPostImportDelete
        {
            get
            {
                if (!_reportedOOSPostImportDelete.HasValue)
                    _reportedOOSPostImportDelete = OOSCommon.AppConfig.AppSettings[oosReportedOOSPostImportDeleteAppSetting].Equals("true", StringComparison.OrdinalIgnoreCase);
                return _reportedOOSPostImportDelete.GetValueOrDefault(false); ;
            }
            set { _reportedOOSPostImportDelete = value; }
        } static bool? _reportedOOSPostImportDelete = null;
        public static string knownOOSPostImportMoveToPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_knownOOSPostImportMoveToPath))
                    _knownOOSPostImportMoveToPath = OOSCommon.AppConfig.AppSettings[oosReportedOOSPostImportMoveToPathAppSetting];
                return _knownOOSPostImportMoveToPath;
            }
            set { _knownOOSPostImportMoveToPath = value; }
        } static string _knownOOSPostImportMoveToPath = string.Empty;
        public static bool knownOOSPostImportDelete
        {
            get
            {
                if (!_knownOOSPostImportDelete.HasValue)
                    _knownOOSPostImportDelete = OOSCommon.AppConfig.AppSettings[oosReportedOOSPostImportDeleteAppSetting].Equals("true", StringComparison.OrdinalIgnoreCase);
                return _knownOOSPostImportDelete.GetValueOrDefault(false); ;
            }
            set { _knownOOSPostImportDelete = value; }
        } static bool? _knownOOSPostImportDelete = null;
        public static string regionPrefix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_regionPrefix))
                    _regionPrefix = OOSCommon.AppConfig.AppSettings[oosRegionPrefixAppSetting];
                return _regionPrefix;
            }
            set { _regionPrefix = value; }
        } static string _regionPrefix = string.Empty;
        public static string storePrefix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_storePrefix))
                    _storePrefix = OOSCommon.AppConfig.AppSettings[oosStorePrefixAppSetting];
                return _storePrefix;
            }
            set { _storePrefix = value; }
        } static string _storePrefix = string.Empty;

        public static string ftpUrlUNFI
        {
            get
            {
                if (_ftpUrlUNFI == (string)null)
                    _ftpUrlUNFI = OOSCommon.AppConfig.AppSettings[oosUNFIFtpURLAppSetting];
                return _ftpUrlUNFI;
            }
        } static string _ftpUrlUNFI = null;

        [Flags]
        public enum ERunDays : int { none = 0, sun = 1, mon = 2, tue = 4, wed = 8, thu = 16, fri = 32, sat = 64, all = sun | mon | tue | wed | thu | fri | sat }

        public static ERunDays ToERunDays(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday: return ERunDays.sun;
                case DayOfWeek.Monday: return ERunDays.mon;
                case DayOfWeek.Tuesday: return ERunDays.tue;
                case DayOfWeek.Wednesday: return ERunDays.wed;
                case DayOfWeek.Thursday: return ERunDays.thu;
                case DayOfWeek.Friday: return ERunDays.fri;
                case DayOfWeek.Saturday: return ERunDays.sat;
            }
            return ERunDays.none;
        }

    }
}
