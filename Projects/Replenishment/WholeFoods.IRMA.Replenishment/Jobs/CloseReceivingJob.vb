Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that closes receiving for all stores
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CloseReceivingJob

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

#End Region

        ''' <summary>
        ''' Kicks off the Close Receiving job.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main(ByVal userID As Integer) As Boolean
            Logger.LogDebug("Main entry", Me.GetType())

            Dim success As Boolean = False

            Try
                CloseReceiving.DataAccess.CloseReceivingDAO.AutoCloseReceiving(userID)
                success = True
            Catch ex As Exception
                _errorMessage = ex.Message
                _errorException = ex
            End Try

            Logger.LogDebug("Main exit: " & success.ToString(), Me.GetType())

            Return success
        End Function

    End Class

End Namespace