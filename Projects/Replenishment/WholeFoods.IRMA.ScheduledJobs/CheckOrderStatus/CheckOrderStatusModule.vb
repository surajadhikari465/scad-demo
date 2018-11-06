Option Explicit On
Option Strict On

Imports log4net
Imports System.IO
Imports System.Text
Imports System.Threading
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility


Module CheckOrderStatusModule
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091
    ' Tom Lux
    ' 3/10/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' ---------------------------------------------------------------------------------------------------------------

    ' Keeps track of log messages to write to the log file
    Private _Logs As List(Of String) = New List(Of String)

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub Main()

        Try
            ' Configure logging specified in app-config file.
            log4net.Config.XmlConfigurator.Configure()

            ' Download app settings doc.
            Configuration.CreateAppSettings()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()

            WriteToConsole("Schedule Check Order Status Job is starting.", True)

            ' Kick off the schedule Send Orders Job
            ' All configuration settings for the job are specified in the appSettings.config cache file
            ' Start the job
            Dim sendOrdersLogName As String = "CheckOrderStatusJob-" & FixDate(DateTime.Now) & ".log"
            Dim chksendOrdersJob As CheckSendOrderStatusJob = New CheckSendOrderStatusJob
            Dim jobStatus As Integer

            jobStatus = CInt(chksendOrdersJob.Main())

            ' Add the log messages to the queue
            _Logs.AddRange(chksendOrdersJob.LogMessages)
            WriteToConsole("Job completed with success status = " & jobStatus.ToString, True)

            If _Logs.Count > 1 Then
                For Each i As String In _Logs
                    logger.Info(i)
                Next
                'Dim txt As TextWriter = New StreamWriter(sendOrdersLogName, True)
                'txt.WriteLine(String.Join(vbCrLf, _Logs.ToArray()))
                'txt.Close()
                'txt.Dispose()
            End If

        Catch ex As Exception

            WriteToConsole(ex.Message, True)
            Thread.Sleep(5000)

        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

    ''' <summary>
    ''' Add a status message to the console during application execution.
    ''' </summary>
    ''' <param name="msg">text of the message</param>
    ''' <param name="log">flag set to true if the message should also be added to a log file</param>
    ''' <remarks></remarks>
    Private Sub WriteToConsole(ByVal msg As String, ByVal log As Boolean)
        Console.WriteLine(DateTime.Now.ToString() & " " & msg)
        If log Then logger.Info(msg)
    End Sub

    ''' <summary>
    ''' Format the a date for including in the logfile name: MMDDYYYY
    ''' </summary>
    ''' <param name="d">date</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FixDate(ByVal d As DateTime) As String
        Dim retval As New StringBuilder
        If d.Month < 10 Then
            retval.Append("0")
            retval.Append(d.Month.ToString())
        Else
            retval.Append(d.Month.ToString)
        End If

        If d.Day < 10 Then
            retval.Append("0")
            retval.Append(d.Day.ToString())
        Else
            retval.Append(d.Day.ToString)
        End If

        retval.Append(d.Year.ToString)
        Return retval.ToString
    End Function


End Module

