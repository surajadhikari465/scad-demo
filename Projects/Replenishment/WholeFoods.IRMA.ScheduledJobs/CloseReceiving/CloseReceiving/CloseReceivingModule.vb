Imports log4net
Imports System
Imports System.Configuration
Imports System.Threading
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility

Module CloseReceivingModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091 (v3.6)
    ' Tom Lux
    ' 3/10/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' 3) Added log4net logger.
    ' 4) Removed 'WriteToConsole()' sub, replacing with Logger-class calls, as new dynamic log4net config can build/enable
    ' a console logger based on a DB app setting.
    ' ---------------------------------------------------------------------------------------------------------------

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Sub Main()

        Try
            ' Configure logging specified in app-config file.
            log4net.Config.XmlConfigurator.Configure()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()
        Catch ex As Exception
            System.Console.WriteLine("Error during log setup/config: " + ex.ToString)
            System.Console.WriteLine(ex.StackTrace)
        End Try

        Try
            ' get the appSettings cache from the database
            Configuration.CreateAppSettings()

            logger.Info("Schedule Close Receiving Job is starting.")

            Dim closeReceivingJob As New CloseReceivingJob()
            Dim userId As Integer

            If Not Integer.TryParse(ConfigurationServices.AppSettings("SystemUserID").ToString(), userId) Then
                logger.Error("Schedule Close Receiving Job failed; invalid SystemUserID in app.config.")
                Return
            End If

            If closeReceivingJob.Main(userId) Then
                logger.Info("Schedule Close Receiving Job completed successfully.")
            Else
                logger.Error(String.Format("Schedule Close Receiving Job failed with message: {0}", closeReceivingJob.ErrorMessage))
            End If

        Catch ex As Exception

            logger.Error("An error occurred in CloseReceivingJob.", ex)
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
