Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.AverageCostUpdate.DataAccess
Imports WholeFoods.Utility.SMTP
Imports log4net


Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that updates the average cost for a given store
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AverageCostUpdateJob
        Inherits ScheduledJob
#Region "Member Variables"

        ''' <summary>
        ''' Contains a message describing the error condition if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Contains any exception caught during processing if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorException As Exception

        ''' <summary>
        ''' The date should be the same for all POS Push and Scale Push stored procedure calls to keep the
        ''' process consistent.  This is important if the job starts before midnight and completes after midnight.
        ''' </summary>
        ''' <remarks></remarks>
        Private _jobRunDate As Date = Now

        ''' <summary>
        ''' Store and Subteam Numbers
        ''' </summary>
        ''' <remarks></remarks>
        Private _store As Integer = 0
        Private _subteam As Integer = 0


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
#End Region

#Region "Property Access Methods"

        Public Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal Value As String)
                _errorMessage = Value
            End Set
        End Property
        Public ReadOnly Property LogMessages() As List(Of String)
            Get
                Return _logMessages
            End Get
        End Property

        Public Property ErrorException() As Exception
            Get
                Return _errorException
            End Get
            Set(ByVal Value As Exception)
                _errorException = Value
            End Set
        End Property

        Public Property JobRunDate() As Date
            Get
                Return _jobRunDate
            End Get
            Set(ByVal value As Date)
                _jobRunDate = value
            End Set
        End Property

        Public Property Store() As Integer
            Get
                Return _store
            End Get
            Set(ByVal value As Integer)
                _store = value
            End Set
        End Property

        Public Property Subteam() As Integer
            Get
                Return _subteam
            End Get
            Set(ByVal value As Integer)
                _subteam = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Kicks off the Average Cost Update job.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Protected Overrides Function RunProcess() As Boolean
            logger.Debug("Main entry")
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("Error_ToEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("DCAvgCostJob_Email").ToString


            Dim success As Boolean = False
            Try
                ScheduledLog("Running AverageCostUpdate Job - StartTime - " & Date.Now)
                AverageCostUpdate.DataAccess.AverageCostUpdateDAO.UpdateAverageCost(Store, Subteam)
                success = True
                SendMail(sSuccessEmailAddress, "AverageCostUpdate Job", "The AverageCostUpdate Job has completed successfully!")
                ScheduledLog("Finished AverageCostUpdate Job - StopTime - " & Date.Now)
            Catch ex As Exception
                _errorMessage = ex.Message
                _errorException = ex
                SendMail(sErrorEmailAddress, "AverageCostUpdate Job", "The AverageCostUpdate Job did not complete due to the following error:  " & ex.Message & " - " & ex.InnerException.Message)
                Return False
            End Try

            logger.Debug("Main exit: " & success.ToString())

            Return success

        End Function

        ''' <summary>
        ''' Add a status message to the scheduled job log file during application execution.
        ''' </summary>
        ''' <param name="msg">text of the message</param>
        ''' <remarks></remarks>
        ''' 
        Private Sub ScheduledLog(ByVal msg As String)
            _logMessages.Add(DateTime.Now.ToString() & " " & msg)
        End Sub

        ''' <summary>
        ''' Sends Notifications for success/failures.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SendMail(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String)
            Try
                Dim sSMTPHost As String = ConfigurationServices.AppSettings("SMTPHost")
                Dim sFromEmailAddress As String = "UpdateAverageCostJob@WholeFoods.com"
                Dim smtp As New SMTP(sSMTPHost)
                smtp.send(sMessage, sRecipient, Nothing, sFromEmailAddress, sSubject)
            Catch ex As Exception
                _errorMessage = ex.Message
                _errorException = ex
                ScheduledLog("Failed to send Email Notifications")
            End Try
           
        End Sub
    End Class


End Namespace