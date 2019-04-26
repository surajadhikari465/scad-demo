﻿using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Icon.Dashboard.Mvc.Services
{
    public class IconDatabaseServiceWrapper : IIconDatabaseServiceWrapper
    {
        public IIconDatabaseService DatabaseService { get; private set; }

        public IconDatabaseServiceWrapper(IIconDatabaseService dataService = null)
        {
            DatabaseService = dataService ?? new IconDataService();
        }

        public IApp GetApp(string appName)
        {
            return DatabaseService.GetApp(appName);
        }

        public IApp GetApp(int appID)
        {
            return DatabaseService.GetApp(appID);
        }

        public List<IconLoggedAppViewModel> GetApps()
        {
            var apps = DatabaseService.GetApps();
            var appViewModels = new List<IconLoggedAppViewModel>(apps.Count());
            foreach (var app in apps)
            {
                appViewModels.Add(new IconLoggedAppViewModel(app));
            }
            return appViewModels;
        }

        public List<IconLogEntryViewModel> GetPagedAppLogsByApp(
            string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error)
        {
            if (errorLevelEnum == LogErrorLevelEnum.Any)
            {
                var appLogs = DatabaseService.GetPagedAppLogsByApp(appName, page, pageSize);
                var logViewModels = AppLogViewModelsFromEntities(appLogs);
                return logViewModels;
            }
            else
            {
                var appId = GetAppIdForAppName(appName);
                Expression<Func<IAppLog, bool>> filter = appLog => appLog.AppID == appId && appLog.Level == errorLevelEnum.ToString(); 
                return GetPagedFilteredAppLogs(filter, page, pageSize, QuerySortOrder.Descending);
            }
        }

        public List<IconLogEntryViewModel> GetPagedAppLogs(int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error)
        {
            if (errorLevelEnum == LogErrorLevelEnum.Any)
            {
                var appLogs = DatabaseService.GetPagedAppLogs(page, pageSize);
                var logViewModels = AppLogViewModelsFromEntities(appLogs);
                return logViewModels;
            }
            else
            {
                Expression<Func<IAppLog, bool>> filter = appLog => appLog.Level == errorLevelEnum.ToString(); 
                return GetPagedFilteredAppLogs(filter, page, pageSize, QuerySortOrder.Descending);
            }
        }

        public List<IconLogEntryViewModel> GetPagedFilteredAppLogs(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var appLogs = DatabaseService.GetPagedAppLogsWithFilter(filter, page, pageSize, sortOrder);
            var logViewModels = AppLogViewModelsFromEntities(appLogs);
            return logViewModels;
        }

        private List<IconLogEntryViewModel> AppLogViewModelsFromEntities(IEnumerable<IAppLog> appLogs)
        {
            var logViewModels = new List<IconLogEntryViewModel>(appLogs.Count());
            foreach (var appLog in appLogs)
            {
                logViewModels.Add(new IconLogEntryViewModel(appLog));
            }
            return logViewModels;
        }

        public IconLogEntryViewModel GetSingleAppLog(int appLogId)
        {
            var appLog = DatabaseService.GetSingleAppLog(appLogId);
            var logViewModel = new IconLogEntryViewModel(appLog);
            return logViewModel;
        }

        public RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = DatabaseService.GetRecentLogEntriesReport(appName, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        public RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = DatabaseService.GetRecentLogEntriesReport(appID, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        public RecentLogEntriesReportViewModel LogEntryReportViewModelFromEntity(IAppLogSummary logEntryReport)
        {
            var logEntryReportViewModel = new RecentLogEntriesReportViewModel(logEntryReport);
            return logEntryReportViewModel;
        }

        public IEnumerable<RecentLogEntriesReportViewModel> GetEmptyLogEntriesReportList(IEnumerable<IconLoggedAppViewModel> iconAppDefinitions)
        {
            if (iconAppDefinitions != null)
            {
                var emptyLogEntryReports = iconAppDefinitions
                    .Where(appDef => appDef.AppID > 0)
                    .GroupBy(appDef => appDef.AppID)
                    .Select(appDefWithId => new RecentLogEntriesReportViewModel(appDefWithId.First()))
                    .ToList();

                return emptyLogEntryReports;
            }
            return null;
        }

        public int GetAppIdForAppName(string appName)
        {
            var app = DatabaseService.GetApp(appName);
            return app.AppID;
        }

        public List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            var jobSummaries = DatabaseService.GetPagedApiJobSummaries(page, pageSize);
            var jobSummaryViewModels = JobSummaryViewModelsFromEntities(jobSummaries);
            return jobSummaryViewModels;
        }

        public List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummariesByMessageType(
            string messageType,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            var jobSummaries = DatabaseService.GetPagedApiJobSummariesByMessageType(messageType, page, pageSize);
            var jobSummaryViewModels = JobSummaryViewModelsFromEntities(jobSummaries);
            return jobSummaryViewModels;
        }

        public List<ApiMessageJobSummaryViewModel> JobSummaryViewModelsFromEntities(IEnumerable<IAPIMessageMonitorLog> jobSummaries)
        {
            var jobSummaryViewModels = new List<ApiMessageJobSummaryViewModel>(jobSummaries.Count());
            foreach (var entity in jobSummaries)
            {
                jobSummaryViewModels.Add(new ApiMessageJobSummaryViewModel(entity));
            }
            return jobSummaryViewModels;
        }

        public ApiMessageJobTimedReportViewModel GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime)
        {
            var jobSummaryReport = DatabaseService.GetApiJobSummaryReport(messageType, startTime, endTime);
            var jobSummaryReportViewModel = JobSummaryReportViewModelFromEntity(jobSummaryReport);
            return jobSummaryReportViewModel;
        }

        public ApiMessageJobTimedReportViewModel JobSummaryReportViewModelFromEntity(IApiJobSummary jobSummaryReport)
        {
            var jobSummaryReportViewModel = new ApiMessageJobTimedReportViewModel(jobSummaryReport);
            return jobSummaryReportViewModel;
        }

        public PendingMessagesViewModel GetPendingMessages()
        {
            var pendingMessages = DatabaseService.GetPendingMessageCountByMessageType();
            return new PendingMessagesViewModel(pendingMessages);            
        }
    }
}