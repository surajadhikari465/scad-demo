Imports log4net
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.SendOrders.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that sends receiving data from IRMA to PeopleSoft to allow for the payment of external vendors.
    ''' </summary>
    Public Class CheckSendOrderStatusJob
        Inherits ScheduledJob
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        ''' <summary>
        ''' The console application writes log data to a file.  This is a collection of messages to
        ''' include in this log.
        ''' </summary>
        ''' <remarks></remarks>
        Private _logMessages As List(Of String) = New List(Of String)

        ' Flag to track the status of this job.  Any errors that are encountered, but handled by the job
        ' should set this flag to FALSE and provide an error description to the user in _jobExecutionMessage.
        Dim jobSuccess As Boolean = True

        ''' <summary>
        ''' This is the method that performs the work for the job.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Function RunProcess() As Boolean
            logger.Debug("RunProcess entry")

            Dim sendBO As SendOrdersBO = New SendOrdersBO()
            Dim logText As ArrayList
            Dim text As String = String.Empty

            logText = sendBO.CheckWFMFaxAndEmailStatus

            'For Each text In logText
            '    logger.Info(text)
            '    'ScheduledLog(text)
            'Next

            logger.Debug("RunProcess exit: jobSuccess=" + jobSuccess.ToString)
            Return jobSuccess
        End Function
        ''' <summary>
        ''' Add a status message to the scheduled job log file during application execution.
        ''' </summary>
        ''' <param name="msg">text of the message</param>
        ''' <remarks></remarks>
        Private Sub ScheduledLog(ByVal msg As String)
            _logMessages.Add(DateTime.Now.ToString() & " " & msg)
        End Sub
        Public ReadOnly Property LogMessages() As List(Of String)
            Get
                Return _logMessages
            End Get
        End Property
    End Class
End Namespace


