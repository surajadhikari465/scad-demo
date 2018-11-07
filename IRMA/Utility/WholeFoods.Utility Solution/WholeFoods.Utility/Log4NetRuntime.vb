Imports log4net

Namespace WholeFoods.Utility

    ' TFS 12091
    ' Tom Lux
    ' 3/8/2010
    ' Added this class.
    ' This class handles all aspects of configuring Log4Net at runtime.
    ' This is the alternative to putting all settings in an app.config file and allows dynamic configuration.
    Public Class Log4NetRuntime

        Public Shared Sub ConfigureLogging()
            ' TODO: Configure different log4net appenders/settings based on app-settings.
            ' Examples:
            ' - Enable/disable a certain appender.
            ' - Change log level or other settings.


            ' Wrapping all of this to avoid issues with sched jobs.
            Try
                ConfigureDBLogging()
            Catch ex As Exception
                ' TODO: Handle exception.
                System.Console.WriteLine(ex.ToString)
            End Try

        End Sub
        Private Shared Sub ConfigureDBLogging()

            Dim log4netHierarchy As log4net.Repository.Hierarchy.Hierarchy = CType(log4net.LogManager.GetRepository(), Repository.Hierarchy.Hierarchy)
            AddAdoNetAppenderIfEnabled(log4netHierarchy)
            AddConsoleAppenderIfEnabled(log4netHierarchy)
            log4netHierarchy.Root.Repository.Configured = True

        End Sub
        Private Shared Sub AddConsoleAppenderIfEnabled(ByRef log4netHierarchy As log4net.Repository.Hierarchy.Hierarchy)
            ' Get app setting that controls enabling or disabling a console appender.
            Dim consoleLogEnabled As Boolean = False
            Try
                consoleLogEnabled = CBool(ConfigurationServices.AppSettings("Log4NetConsoleLogEnabled"))
            Catch ex As Exception
            End Try
            If consoleLogEnabled Then
                log4netHierarchy.Root.AddAppender(GetConsoleAppender())
            End If

        End Sub
        Private Shared Function GetConsoleAppender() As log4net.Appender.ConsoleAppender
            Dim consoleAppender As log4net.Appender.ConsoleAppender = New log4net.Appender.ConsoleAppender()
            With consoleAppender
                .Name = "ConsoleAppender"
                .Layout = New log4net.Layout.PatternLayout("%date{dd-MM-yyyy HH:mm:ss,fff} %5level [%2thread] %message (%logger{1}:%line)%n")
                .Threshold = log4net.Core.Level.Info
                .ActivateOptions()
            End With
            log4net.Config.BasicConfigurator.Configure(consoleAppender)

            Return consoleAppender
        End Function
        Private Shared Sub AddAdoNetAppenderIfEnabled(ByRef log4netHierarchy As log4net.Repository.Hierarchy.Hierarchy)
            ' Get app setting that controls enabling or disabling an ADO Net (database-logging) appender.
            ' We are defaulting to true/enabled here because we need some kind of logging.
            Dim dbLogEnabled As Boolean = True
            Try
                dbLogEnabled = CBool(ConfigurationServices.AppSettings("Log4NetAdoNetLogEnabled"))
            Catch ex As Exception
            End Try
            If dbLogEnabled Then
                log4netHierarchy.Root.AddAppender(GetAdoNetAppender())
            End If
        End Sub
        Private Shared Function GetAdoNetAppender() As log4net.Appender.AdoNetAppender ' *** This was working with all other log4net stuff in app.config.
            Dim adoAppender As log4net.Appender.AdoNetAppender = Nothing
            ' Get the Hierarchy object that organizes the loggers.
            Dim log4netHierarchy As log4net.Repository.Hierarchy.Hierarchy = CType(log4net.LogManager.GetRepository(), Repository.Hierarchy.Hierarchy)
            If Not log4netHierarchy Is Nothing Then
                ' Get appender.
                adoAppender = CType(log4netHierarchy.Root.GetAppender("AdoNetAppender"), Appender.AdoNetAppender)
                If Not adoAppender Is Nothing Then

                    ' TODO: What do we do when a log4net setting is missing?
                    Dim bufferSize As Integer
                    Try
                        bufferSize = CInt(ConfigurationServices.AppSettings("Log4NetDBLogBufferSize"))
                    Catch ex As Exception
                        bufferSize = 100
                    End Try

                    ' Get app setting that stores the unique text we should find in the connection string command line argument.
                    Dim searchStr As String = Nothing
                    Try
                        searchStr = ConfigurationServices.AppSettings("ConnectionStringSearchString")
                    Catch ex As Exception
                    End Try
                    If String.IsNullOrEmpty(searchStr) Then
                        searchStr = "initial catalog="
                    End If

                    Dim connectionString As String = Nothing
                    Try
                        connectionString = FindCommandLineArg(searchStr)
                    Catch ex As Exception
                    End Try
                    If connectionString Is Nothing Then
                        connectionString = "<not found in command line arguments>"
                    End If

                    ' Get app setting for controlling the log level.
                    ' **NOTE: I didn't find a way to set at the appender level and make it actually update log4net to accept the change;
                    ' we have to set the Root.Level attribute and raise the appropriate event.
                    Dim logThreshold As String = Nothing
                    ' Tom Lux, 10/22/15, IRMA-TFS Task 17462: Tech - Fix Debug level logging capability in the POS Push (VSOL PBI 12205).
                    ' Default level is INFO.
                    Dim thresholdLevel As log4net.Core.Level = log4net.Core.Level.Info
                    Try
                        logThreshold = ConfigurationServices.AppSettings("Log4NetDBLogThreshold")
                    Catch ex As Exception
                    End Try
                    ' If we got a setting, make it lowercase for mapping to log4net.Core.Level.
                    If Not String.IsNullOrEmpty(logThreshold) Then
                        logThreshold = logThreshold.ToLower
                    End If

                    ' Tom Lux, 10/22/15, IRMA-TFS Task 17462: Tech - Fix Debug level logging capability in the POS Push (VSOL PBI 12205).
                    ' Log levels are:  ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF
                    If logThreshold = log4net.Core.Level.All.ToString.ToLower Then thresholdLevel = log4net.Core.Level.All
                    If logThreshold = log4net.Core.Level.Debug.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Debug
                    If logThreshold = log4net.Core.Level.Info.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Info
                    If logThreshold = log4net.Core.Level.Warn.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Warn
                    If logThreshold = log4net.Core.Level.Error.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Error
                    If logThreshold = log4net.Core.Level.Fatal.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Fatal
                    If logThreshold = log4net.Core.Level.Off.ToString.ToLower Then thresholdLevel = log4net.Core.Level.Off

                    With adoAppender
                        .ConnectionString = connectionString
                        .BufferSize = bufferSize
                        .ActivateOptions() ' Refresh appender settings.
                    End With

                    ' Tom Lux, 10/22/15, IRMA-TFS Task 17462: Tech - Fix Debug level logging capability in the POS Push (VSOL PBI 12205).
                    ' Again, we are using the app-setting specific to the DB-logger to update the level, which will actually apply to
                    ' all loggers, not just the DB logger, but that's okay, as it's the primary logger in most apps.
                    log4netHierarchy.Root.Level = thresholdLevel
                    log4netHierarchy.RaiseConfigurationChanged(EventArgs.Empty)

                End If
            End If

            Return adoAppender
        End Function

        Public Shared Function FindCommandLineArg( _
        ByVal searchStr As String) _
        As String
            'TODO: Move this to a "Runtime" class.
            'MsgBox("*** Move to runtime class ***")
            Dim arg As String = Nothing
            For argIndex As Integer = 1 To Environment.GetCommandLineArgs.Length
                Try
                    arg = Environment.GetCommandLineArgs().GetValue(argIndex).ToString
                Catch ex As Exception
                End Try
                If arg.ToLower.Contains(searchStr.ToLower) Then
                    Return arg
                End If
            Next
            ' If we're here, we didn't find it.
            Return Nothing
        End Function
    End Class

End Namespace
