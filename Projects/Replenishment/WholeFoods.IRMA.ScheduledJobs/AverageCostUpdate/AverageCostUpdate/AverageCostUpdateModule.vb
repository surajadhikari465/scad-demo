Imports System
Imports System.Configuration
Imports System.Reflection
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports System.Threading
Imports log4net

Module AverageCostUpdateModule
    ' Keeps track of log messages to write to the log file
    Private _Logs As List(Of String) = New List(Of String)
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const CLASSNAME As String = "AverageCostUpdate"

    Sub Main()


        Try

            ' Configure logging specified in app-config file.
            log4net.Config.XmlConfigurator.Configure()

            ' download the appSettings document
            Configuration.CreateAppSettings()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()

            Dim averageCostUpdateJob As New AverageCostUpdateJob()

            logger.Info("Schedule Average Cost Update Job is starting.")

            If averageCostUpdateJob.Main() Then
                logger.Info("Schedule Average Cost Update Job completed successfully.")
            Else
                logger.Info(String.Format("Schedule Average Cost Update Job failed with message: {0}", averageCostUpdateJob.ErrorMessage))
            End If
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

    Private Sub WriteToConsole(ByVal msg As String)
        logger.Info(String.Format("{0}", msg))
        Console.WriteLine(DateTime.Now.ToString() & " " & msg)
    End Sub

End Module
