Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility
Imports System.IO
Imports System.Configuration
Imports WholeFoods.IRMA.Replenishment
Imports WholeFoods.IRMA.Replenishment.EST.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.EST.BusinessLogic
Imports System.Net.Mail

Imports log4net
Imports WholeFoods.IRMA.Replenishment.Jobs

Module ESTModule
    ' Keeps track of log messages to write to the log file
    Private _Logs As List(Of String) = New List(Of String)
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const CLASSNAME As String = "ESTImportJob"

    Sub Main()

        Try
            Try
                ' Configure logging specified in app-config file.
                log4net.Config.XmlConfigurator.Configure()

                ' Download app settings doc.
                Configuration.CreateAppSettings()

                ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
                Log4NetRuntime.ConfigureLogging()

            Catch ex As Exception

                Throw New Exception("Configuration.CreateAppSettings()" & Environment.NewLine & ex.Message)

            End Try

            logger.Info("[ EST IMPORT JOB START ]")

            Try

                ' initialize and run the job
                Dim ESTJob As New ESTImportJob
                ESTJob.Main()

                ' Add the log messages to the queue
                _Logs.AddRange(ESTJob.LogMessages)

            Catch ex As Exception

                Throw New Exception("doJob.Main()" & Environment.NewLine & ex.Message)

            End Try

            If _Logs.Count > 1 Then
                For Each i As String In _Logs
                    logger.Info(i)
                Next
            End If

        Catch ex As Exception

            JobStatusDAO.UpdateJobStatus(CLASSNAME, DBJobStatus.Failed)

            JobStatusDAO.InsertJobError(CLASSNAME, CLASSNAME & " failed at step: " & ex.Message)

            logger.Error("Job processing stopped due to an exception during processing: ", ex)

        Finally

            logger.Info("---------------------------")

        End Try
    End Sub
End Module