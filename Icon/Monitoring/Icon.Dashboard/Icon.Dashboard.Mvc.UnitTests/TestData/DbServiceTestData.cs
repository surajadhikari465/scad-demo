using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.TestData
{
    public class DbServiceTestData
    {
        public DbServiceTestData(DateTime? timedDataStart)
        {
            TimedDataStart = timedDataStart.HasValue
                ? new DateTime(timedDataStart.Value.Year, timedDataStart.Value.Month, timedDataStart.Value.Hour, 12, 12, 12)
                : DefaultLoggingTimestamp;
        }
        public DateTime TimedDataStart { get; set; }

        public List<FakeApp> IconApps = new List<FakeApp>
        {
            new FakeApp(id: 1, name: "Web App"),
            new FakeApp(id: 2, name: "Interface Controller"),
            new FakeApp(id: 3, name: "ESB Subscriber"),
            new FakeApp(id: 4, name: "Icon Service"),
            new FakeApp(id: 5, name: "API Controller"),
            new FakeApp(id: 6, name: "POS Push Controller"),
            new FakeApp(id: 7, name: "Global Controller"),
            new FakeApp(id: 8, name: "Regional Controller"),
            new FakeApp(id: 9, name: "Subteam Controller"),
            new FakeApp(id: 10, name: "Icon Data Purge"),
            new FakeApp(id: 11, name: "TLog Controller"),
            new FakeApp(id: 12, name: "Nutrition Web API"),
            new FakeApp(id: 13, name: "Vim Locale Controller"),
            new FakeApp(id: 14, name: "Icon Monitoring"),
            new FakeApp(id: 15, name: "Infor New Item Service"),
            new FakeApp(id: 16, name: "Infor Hierarchy Class Listener"),
            new FakeApp(id: 17, name: "Infor Item Listener"),
            new FakeApp(id: 18, name: "Icon Web Api"),
        };

        public List<FakeApp> MammothApps = new List<FakeApp>
        {
            new FakeApp(id: 1, name: "Web Api"),
            new FakeApp(id: 2, name: "ItemLocale Controller"),
            new FakeApp(id: 3, name: "Price Controller"),
            new FakeApp(id: 4, name: "API Controller"),
            new FakeApp(id: 5, name: "Product Listener"),
            new FakeApp(id: 6, name: "Locale Listener"),
            new FakeApp(id: 7, name: "Hierarchy Class Listener"),
            new FakeApp(id: 8, name: "Mammoth Data Purge"),
            new FakeApp(id: 9, name: "Price Listener"),
            new FakeApp(id: 10, name: "Price Message Archiver"),
            new FakeApp(id: 11, name: "Active Price Service"),
            new FakeApp(id: 12, name: "Active Price Message Archiver"),
            new FakeApp(id: 13, name: "R10 Price Service"),
            new FakeApp(id: 14, name: "IRMA Price Service"),
            new FakeApp(id: 15, name: "Mammoth Web Support"),
            new FakeApp(id: 16, name: "Error Message Listener"),
            new FakeApp(id: 17, name: "Error Message Monitor"),
            new FakeApp(id: 18, name: "Job Scheduler"),
            new FakeApp(id: 19, name: "Expiring Tpr Service"),
            new FakeApp(id: 20, name: "Prime Affinity Listener"),
            new FakeApp(id: 21, name: "Prime Affinity Controller"),
            new FakeApp(id: 22, name: "Emergency Price Service"),
            new FakeApp(id: 23, name: "OnePlum Listener"),
            new FakeApp(id: 24, name: "Esl Listener"),
        };

        public FakeAppLog[] GetAppLogDataForFilterTest(LogErrorLevelEnum matchingErrorLevel)
        {
            var appLogId = DefaultAppLogIDStart;
            var appName = "My.Service";
            var appID = 4;
            var fakeAppLogs = new List<FakeAppLog>();

            fakeAppLogs.Add(
                GetTypicalFakeAppLog(appName, appID, appLogId, matchingErrorLevel, TimedDataStart));

            switch (matchingErrorLevel)
            {
                case LogErrorLevelEnum.Info:
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Warn, TimedDataStart));
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Error, TimedDataStart));
                    break;
                case LogErrorLevelEnum.Warn:
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Info, TimedDataStart));
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Error, TimedDataStart));
                    break;
                case LogErrorLevelEnum.Error:
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Warn, TimedDataStart));
                    fakeAppLogs.Add(
                        GetTypicalFakeAppLog(appName, appID, appLogId++, LogErrorLevelEnum.Info, TimedDataStart));
                    break;
                case LogErrorLevelEnum.Any:
                    // since no app name is specified and error level is any, any record would be valid,
                    //  so only return the first (valid) one
                    break;
                default:
                    break;

            } 
            return fakeAppLogs.ToArray();
        }

        /// <summary>
        /// Builds an array of IAppLog objects for use in verifying methods which filter AppLogs.
        ///  The first AppLog object in the array will match the provided parameters
        ///  (have the expected AppID, AppName, and Error Level). The second AppLog object will have a
        ///  different AppID and AppName. The third AppLog object will only be added if the matching
        ///  error level is not Any- in that case it will have the same AppID and AppName as the first 
        ///  (matching) element but have a different LogLevel.
        /// </summary>
        /// <param name="matchingAppID"></param>
        /// <param name="matchingAppName"></param>
        /// <param name="matchingErrorLevel"></param>
        /// <returns></returns>
        public FakeAppLog[] GetAppLogDataForFilterTest(int matchingAppID,
            string matchingAppName,
            LogErrorLevelEnum matchingErrorLevel)
        {
            var appLogId = DefaultAppLogIDStart;
            var fakeAppLogs = new List<FakeAppLog>();
            fakeAppLogs.Add(
                GetTypicalFakeAppLog(matchingAppName, matchingAppID, appLogId, matchingErrorLevel, TimedDataStart));
            fakeAppLogs.Add(
                GetTypicalFakeAppLog("A.Nother.Service", matchingAppID +1, appLogId++, matchingErrorLevel, TimedDataStart));
            if (matchingErrorLevel != LogErrorLevelEnum.Any)
            {
                var nonMatchingErrorLevel = LogErrorLevelEnum.Any;
                switch (matchingErrorLevel)
                {
                    case LogErrorLevelEnum.Info:
                        nonMatchingErrorLevel = LogErrorLevelEnum.Warn;
                        break;
                    case LogErrorLevelEnum.Warn:
                        nonMatchingErrorLevel = LogErrorLevelEnum.Error;
                        break;
                    case LogErrorLevelEnum.Error:
                        nonMatchingErrorLevel = LogErrorLevelEnum.Info;
                        break;
                    case LogErrorLevelEnum.Any:
                    default:
                        break;
                }
                fakeAppLogs.Add(
                    GetTypicalFakeAppLog(matchingAppName, matchingAppID, appLogId++, nonMatchingErrorLevel, TimedDataStart));
            }

            return fakeAppLogs.ToArray();
        }

        public List<FakeAppLog> GetFakeAppLogs(int numRecords, LogErrorLevelEnum errorLevel, DateTime? timestampStart = null)
        {
            return GetFakeAppLogs(numRecords, "A.Nother.Service", 5, errorLevel, timestampStart);
        }

        public List<FakeAppLog> GetFakeAppLogs(int numRecords,
            string appName, int appID, LogErrorLevelEnum errorLevel, DateTime? timestampStart = null)
        {
            var timestamp = timestampStart.HasValue ? timestampStart.Value : TimedDataStart;
            var appLogID = DefaultAppLogIDStart;
            var eachLogLevel = errorLevel;
            var logLevelEnumList = new List<LogErrorLevelEnum>
            {
                LogErrorLevelEnum.Error,
                LogErrorLevelEnum.Info,
                LogErrorLevelEnum.Warn
            };
            var logLevelIndex = 0;
            var msIncrement = 200D;

            var appLogs = new List<FakeAppLog>(numRecords);

            for (int i=0; i<numRecords; i++)
            {
                if (errorLevel == LogErrorLevelEnum.Any)
                {
                    eachLogLevel = logLevelEnumList[logLevelIndex];
                    logLevelIndex = logLevelIndex == (logLevelEnumList.Count - 1) ? 0 : logLevelIndex + 1;
                }
                appLogs.Add(GetTypicalFakeAppLog(appName, appID, appLogID, eachLogLevel, timestamp));
                appLogID++;
                timestamp.AddMilliseconds(msIncrement);
            }
            return appLogs;
        }

        public FakeAppLog GetTypicalFakeAppLog(string appName, int appID, int appLogID, LogErrorLevelEnum errorLevel, DateTime timestamp)
        {
            if (errorLevel == LogErrorLevelEnum.Any)
            {
                errorLevel = LogErrorLevelEnum.Error;
            }
            return new FakeAppLog(appName, appID, timestamp)
            {
                AppLogID = appLogID,
                UserName = @"wfm\someuser",
                InsertDate = timestamp,
                Level = errorLevel.ToString(),
                Logger = $"{appName}.service",
                Message = "something happened and was logged - it was very intersting",
                MachineName = "fakeMachine1234"
            };
        }

        public const int DefaultAppLogIDStart = 47518948;
        public static readonly DateTime DefaultLoggingTimestamp = new DateTime(2019, 7, 30, 12, 12, 12);
    }

    public class FakeApp : IApp
    {
        public FakeApp(int id, string name)
        {
            this.AppID = id;
            this.AppName = name;
        }
        public int AppID { get; set; }
        public string AppName { get; set; }
    }

    public class FakeAppLog : IAppLog
    {
        public FakeAppLog(string appName, int appID, DateTime timestamp)
        {
            this.AppName = appName;
            this.AppID = appID;
            this.LoggingTimestamp = timestamp;
        }
        public string AppName { get; private set; }
        public DateTime LoggingTimestamp { get; private set; }
        public int AppLogID { get; set; }
        public int AppID{get; set; }
        public string UserName{get; set; }
        public DateTime InsertDate{get; set; }
        public string Level{get; set; }
        public string Logger{get; set; }
        public string Message{get; set; }
        public string MachineName{get; set; }

    }
}
