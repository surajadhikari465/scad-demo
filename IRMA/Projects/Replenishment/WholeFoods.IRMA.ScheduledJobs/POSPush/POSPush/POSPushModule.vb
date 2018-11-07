Imports log4net
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility

Module POSPushModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091 (v3.6)
    ' Tom Lux
    ' 3/11/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' ---------------------------------------------------------------------------------------------------------------

    Dim WithEvents pushJob As New POSPushJob

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Update Logs based on scheduled job events"
    Private Sub pushJob_POSApplyChangesToIRMA() Handles pushJob.POSApplyChangesToIRMA
        logger.Info("Completed: Batch Data Updates Applied to IRMA")
    End Sub

    Private Sub pushJob_POSCompleteError() Handles pushJob.POSCompleteError
        logger.Info("ERROR during POS Push")
    End Sub

    Private Sub pushJob_POSCompleteSuccess() Handles pushJob.POSCompleteSuccess
        logger.Info("Completed: POS Push Completed Successfully")
    End Sub

    Private Sub pushJob_POSGeneratedPOSControlFiles() Handles pushJob.POSGeneratedPOSControlFiles
        logger.Info("Completed: Generated POS Control File(s)")
    End Sub

    Private Sub pushJob_POSPushStarted() Handles pushJob.POSPushStarted
        logger.Info("Completed: POS Push Started")
    End Sub

    Private Sub pushJob_POSReadItemDeletes() Handles pushJob.POSReadItemDeletes
        logger.Info("Completed: Read Item Delete Data")
    End Sub

    Private Sub pushJob_POSReadItemIdAdds() Handles pushJob.POSReadItemIdAdds
        logger.Info("Completed: Read Item ID Add Data")
    End Sub

    Private Sub pushJob_POSReadItemIdDeletes() Handles pushJob.POSReadItemIdDeletes
        logger.Info("Completed: Read Item ID Delete Data")
    End Sub

    Private Sub pushJob_POSReadItemPriceChanges() Handles pushJob.POSReadItemPriceChanges
        logger.Info("Completed: Read Item/Price Change Data")
    End Sub

    Private Sub pushJob_POSReadPromoOffers() Handles pushJob.POSReadPromoOffers
        logger.Info("Completed: Read Promo Offer Data")
    End Sub

    Private Sub pushJob_POSReadStoreConfigurationData(ByVal NumStores As Integer) Handles pushJob.POSReadStoreConfigurationData
        logger.Info("Completed: Read POS Store Configuration Data.  # Stores Read? " & NumStores.ToString)
    End Sub
    Private Sub pushJob_POSReadTaxflagData() Handles pushJob.POSReadStoreBatchesAdds
        logger.Info("Completed: Read Tax Flags Data")
    End Sub

    Private Sub pushJob_POSReadStoreBatchesAdds() Handles pushJob.POSReadStoreBatchesAdds
        logger.Info("Completed: Read Store Batches Add")
    End Sub

    Private Sub pushJob_POSReadVendorAdds() Handles pushJob.POSReadVendorAdds
        logger.Info("Completed: Read Vendor ID Adds Data")
    End Sub

    Private Sub pushJob_POSStartedRemoteJobs() Handles pushJob.POSStartedRemoteJobs
        logger.Info("Completed: Remote Jobs Started on Store POS Servers")
    End Sub

    Private Sub pushJob_POSTransferFiles(ByVal FileStatus As String) Handles pushJob.POSTransferFiles
        logger.Info("Completed: POS FTP Completed. " & FileStatus)
    End Sub

    Private Sub pushJob_ScaleCompleteError() Handles pushJob.ScaleCompleteError
        logger.Info("ERROR during Scale Push")
    End Sub

    Private Sub pushJob_ScaleCompleteSuccess() Handles pushJob.ScaleCompleteSuccess
        logger.Info("Completed: Scale Push Completed Successfully")
    End Sub

    Private Sub pushJob_ScaleCorpTempQueueCleared() Handles pushJob.ScaleCorpTempQueueCleared
        logger.Info("Completed: Corporate Scale Data Cleared from IRMA Queue")
    End Sub

    Private Sub pushJob_ScalePushStarted(ByVal IsRegional As Boolean) Handles pushJob.ScalePushStarted
        logger.Info("Completed: Scale Push Started.  Regional Scale Configuration? " & IsRegional.ToString)
    End Sub

    Private Sub pushJob_ScaleReadItemIdAdds() Handles pushJob.ScaleReadItemIdAdds
        logger.Info("Completed: Read Corporate Item ID Adds Data")
    End Sub

    Private Sub pushJob_ScaleReadItemIdDeletes() Handles pushJob.ScaleReadItemIdDeletes
        logger.Info("Completed: Read Corporate Item ID Deletes Data")
    End Sub

    Private Sub pushJob_ScaleReadItemPriceChanges() Handles pushJob.ScaleReadItemPriceChanges
        logger.Info("Completed: Read Corporate Item Change Data")
    End Sub

    Private Sub pushJob_ScaleReadStoreConfigurationData(ByVal NumStores As Integer) Handles pushJob.ScaleReadStoreConfigurationData
        logger.Info("Completed: Read Scale Store Configuration Data.  # Stores Read? " & NumStores.ToString)
    End Sub

    Private Sub pushJob_ScaleReadZoneDeletes() Handles pushJob.ScaleReadZoneDeletes
        logger.Info("Completed: Read Zone Delete Data")
    End Sub

    Private Sub pushJob_ScaleReadZonePriceChanges() Handles pushJob.ScaleReadZonePriceChanges
        logger.Info("Completed: Read Zone Price Change Data")
    End Sub

    Private Sub pushJob_ScaleTransferFiles(ByVal FileStatus As String) Handles pushJob.ScaleTransferFiles
        logger.Info("Completed: Scale FTP Completed. " & FileStatus)
    End Sub
#End Region

    Sub Main()
        ' Kick off the scheduled PeopleSoftUploadJob.  The job writes log messages using the
        ' log4net framework.
        ' All configuration settings for the job are specified in the app.config file
        Try
            log4net.Config.XmlConfigurator.Configure()

            ' download the appSettings document
            Configuration.CreateAppSettings()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()
            logger.Info("Scheduled POS Push Job is starting")

            pushJob.Main()

            logger.Info("Scheduled job completed.")

        Catch ex As Exception
            My.Application.Log.WriteException(ex, TraceEventType.Error, String.Empty)
            logger.Error("Job processing stopped due to an exception during processing: ", ex)
        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

    ''' <summary>
    ''' Removes old scheduled job log files older than a number of days secified in the app.config file.
    ''' Files are kept for x number of days for troubleshooting purposes.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CleanupOldFiles(ByVal logDir As String)
        Try
            Dim removeFilesOlderThanDays As Integer
            If ConfigurationServices.AppSettings("numDaysToKeepScheduledLogs") IsNot Nothing Then
                removeFilesOlderThanDays = Integer.Parse(ConfigurationServices.AppSettings("numDaysToKeepScheduledLogs"))
            End If

            If removeFilesOlderThanDays > 0 Then
                Dim dirInfo As New DirectoryInfo(logDir)
                Dim fileList As FileInfo() = dirInfo.GetFiles()
                Dim fileInfo As FileInfo

                Dim currentDay As Date = Today
                Dim timespan As New TimeSpan(removeFilesOlderThanDays, 0, 0, 0)

                'get list of files in logDir
                For Each fileInfo In fileList
                    If fileInfo.LastWriteTime < currentDay.Subtract(timespan) Then
                        'remove file
                        fileInfo.Delete()
                    End If
                Next
            End If
        Catch ex As Exception
            ' Exceptions are ignored.  The log directory can be manually cleaned up, if needed.
        End Try
    End Sub

End Module
