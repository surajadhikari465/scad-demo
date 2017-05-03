Option Explicit On
Option Strict On

Imports log4net
Imports System.IO
Imports System.Text
Imports System.Threading

Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility

Module POSPULLModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091 (v3.6)
    ' Tom Lux
    ' 3/11/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' 3) Removed _logs (list of messages) and WriteToConsole sub, as log4net takes care of all this.
    ' ---------------------------------------------------------------------------------------------------------------

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub Main()

        Try
            log4net.Config.XmlConfigurator.Configure()

            ' download the appSettings document
            Configuration.CreateAppSettings()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()

            logger.Info("POS PULL Job is starting.")

            ' Kick off the schedule POS Pull Job
            ' All configuration settings for the job are specified in the app.config file
            ' Start the job
            Dim pullJob As POSPullJob = New POSPullJob
            Dim jobStatus As Integer

            jobStatus = pullJob.Main()

            logger.Info("Job completed with success status = " & jobStatus.ToString)

        Catch ex As Exception
            logger.Error("Error occurred during POS Pull process.", ex)
            Thread.Sleep(5000)
        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

End Module
