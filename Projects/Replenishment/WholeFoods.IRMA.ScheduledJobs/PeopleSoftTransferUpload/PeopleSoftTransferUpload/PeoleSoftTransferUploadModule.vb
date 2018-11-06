Imports log4net
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Module PeopleSoftTransferUploadModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 11384 (v4.7.1)
    ' Faisal Ahmed
    ' 03/21/2013
    ' 1) Initial Version. Created this job from PeopleSoftUploadJob
    ' ---------------------------------------------------------------------------------------------------------------

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const CLASSNAME As String = "PeopleSoftTransferUploadJob"

    ''' <summary>
    ''' Execute the job that uploads GL transfers data from IRMA to PeopleSoft.
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
            logger.Info("Scheduled PeopleSoft Transfer Upload Job is starting.")
            Try
                ' initialize and run the job
                Dim doJob As New PeopleSoftTransferUploadJob
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
