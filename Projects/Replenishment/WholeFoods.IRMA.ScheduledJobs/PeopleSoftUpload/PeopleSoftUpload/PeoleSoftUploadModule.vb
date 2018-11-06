Imports log4net
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Module PeopleSoftUploadModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091 (v3.6)
    ' Tom Lux
    ' 3/11/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' ---------------------------------------------------------------------------------------------------------------

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const CLASSNAME As String = "PeopleSoftUploadJob"

    ''' <summary>
    ''' Execute the job that uploads invoice data from IRMA to PeopleSoft.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Main()

        Try
            Try
                ' download the appSettings cache
                Configuration.CreateAppSettings()
            Catch ex As Exception
                Throw New Exception("Configuration.CreateAppSettings()" & Environment.NewLine & ex.Message)
            End Try

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()
            logger.Info("Scheduled PeopleSoft Upload Job is starting.")
            Try
                ' initialize and run the job
                Dim doJob As New PeopleSoftUploadJob
                doJob.Main()
            Catch ex As Exception
                Throw New Exception("doJob.Main()" & Environment.NewLine & ex.Message)
            End Try

        Catch ex As Exception
            JobStatusDAO.UpdateJobStatus(CLASSNAME, DBJobStatus.Failed)
            JobStatusDAO.InsertJobError(CLASSNAME, CLASSNAME & " failed at step: " & ex.Message)
            logger.Error("Job processing stopped due to an exception during processing: ", ex)
        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

End Module
