Option Explicit On
Option Strict On

Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Jobs


Module CheckOrderStatusModule

    ' Keeps track of log messages to write to the log file
    Private _Logs As List(Of String) = New List(Of String)

    Sub Main()
        ' Kick off the schedule Send Orders Job
        ' All configuration settings for the job are specified in the app.config file
        ' Start the job
        Dim sendOrdersLogName As String = "SendOrdersJob-" & FixDate(DateTime.Now) & ".log"
        Dim chksendOrdersJob As CheckSendOrderStatusJob = New CheckSendOrderStatusJob
        Dim jobStatus As Integer

        WriteToConsole("Schedule Send Orders Job is starting.", True)

        jobStatus = CInt(chksendOrdersJob.Main())

        ' Add the log messages to the queue
        _Logs.AddRange(chksendOrdersJob.LogMessages)
        WriteToConsole("Job completed with success status = " & jobStatus.ToString, True)

        If _Logs.Count > 1 Then
            Dim txt As TextWriter = New StreamWriter(sendOrdersLogName, True)
            txt.WriteLine(String.Join(vbCrLf, _Logs.ToArray()))
            txt.Close()
            txt.Dispose()
        End If

    End Sub

    ''' <summary>
    ''' Add a status message to the console during application execution.
    ''' </summary>
    ''' <param name="msg">text of the message</param>
    ''' <param name="log">flag set to true if the message should also be added to a log file</param>
    ''' <remarks></remarks>
    Private Sub WriteToConsole(ByVal msg As String, ByVal log As Boolean)
        Console.WriteLine(DateTime.Now.ToString() & " " & msg)
        If log Then _Logs.Add(DateTime.Now.ToString() & " " & msg)
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

