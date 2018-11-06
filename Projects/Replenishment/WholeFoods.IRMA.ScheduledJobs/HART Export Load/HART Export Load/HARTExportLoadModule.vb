Imports log4net
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Module HARTExportLoadModule
    ' Keeps track of log messages to write to the log file
    Private _Logs As List(Of String) = New List(Of String)
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const CLASSNAME As String = "HARTExportLoadJob"

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

            logger.Info("[ HART EXPORT LOAD JOB START ]")

            Try

                ' initialize and run the job
                Dim HARTJob As New HARTExportLoadJob
                HARTJob.Main()

                ' Add the log messages to the queue
                _Logs.AddRange(HARTJob.LogMessages)

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

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try
    End Sub
End Module
