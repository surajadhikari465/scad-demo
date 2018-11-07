''' <summary>
''' A class for managing the fax transmission log.
''' </summary>
''' <remarks></remarks>
Public Class Log

#Region " Public Shared WriteOnly Properties"

    ''' <summary>
    ''' The location of the fax transmission log.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Shared WriteOnly Property FaxLogLocation() As String
        Set(ByVal value As String)
            IO.FaxLogFile = value
        End Set
    End Property

    ''' <summary>
    ''' The number of days to retain entries in the transmission log.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Shared WriteOnly Property RetentionPolicy() As Integer
        Set(ByVal value As Integer)
            IO.RetentionPolicy = value
        End Set
    End Property

    ''' <summary>
    ''' The number of minutes to wait for a response from the fax server before notifying support staff of a possible transmission problem.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Shared WriteOnly Property ExpirationThreshold() As Integer
        Set(ByVal value As Integer)
            IO.ExpirationThreshold = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' A class for managing a fax transmission log entry.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LogEntry

#Region " Private Fields"

        Private _destination As String
        Private _poId As Integer
        Private _status As FaxStatus
        Private _attempts As Integer
        Private _timeStamp As DateTime
        Private _resendAttempt As Boolean
        Private _responseMessage As String

#End Region

#Region " Public Properties"

        ''' <summary>
        ''' The fax number where the fax is being sent.
        ''' </summary>
        Public Property Destination() As String
            Get
                Return _destination
            End Get
            Set(ByVal value As String)
                _destination = value
            End Set
        End Property

        ''' <summary>
        ''' The PO number associated with the fax.
        ''' </summary>
        Public Property PO() As Integer
            Get
                Return _poId
            End Get
            Set(ByVal value As Integer)
                _poId = value
            End Set
        End Property

        ''' <summary>
        ''' The transmission status of the fax.
        ''' </summary>
        Public Property Status() As FaxStatus
            Get
                Return _status
            End Get
            Set(ByVal value As FaxStatus)
                _status = value
            End Set
        End Property

        ''' <summary>
        ''' The number of times an attempt has been made to send the fax.
        ''' </summary>
        Public Property Attempts() As Integer
            Get
                Return _attempts
            End Get
            Set(ByVal value As Integer)
                _attempts = value
            End Set
        End Property

        ''' <summary>
        ''' Indicates an attemp to resend the same fax transmission.
        ''' </summary>
        Public ReadOnly Property ResentAttempt() As Boolean
            Get
                Return _resendAttempt
            End Get
        End Property

        ''' <summary>
        ''' The response text from the fax service for a fax transmission.
        ''' </summary>
        Public ReadOnly Property Response() As String
            Get
                Return _responseMessage
            End Get
        End Property

        ''' <summary>
        ''' The last activity timestamp for a log entry in the tranmission log.
        ''' </summary>
        Public ReadOnly Property Timestamp() As DateTime
            Get
                Return _timeStamp
            End Get
        End Property

#End Region

#Region " Protected Friend Methods"

        ''' <summary>
        ''' Protected method for creating a new instance of LogEntry.
        ''' Only use this method when retrieving an entry from the transmission log.
        ''' </summary>
        ''' <param name="PO">The PO Number of the entry.</param>
        ''' <param name="Destination">The fax number of the entry.</param>
        ''' <param name="Status">The transmission status of the entry.</param>
        ''' <param name="Attempts">The number of attempts made to transmit the fax.</param>
        ''' <param name="TimeStamp">The last activity timestamp for the entry.</param>
        ''' <param name="Resend">Indicates an attempt to resend the same fax.</param>
        ''' <param name="Response">The transmission response from the fax service.</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal PO As Integer, _
                                 ByVal Destination As String, _
                                 ByVal Status As FaxStatus, _
                                 ByVal Attempts As Integer, _
                                 ByVal TimeStamp As DateTime, _
                                 ByVal Resend As Boolean, _
                                 ByVal Response As String)

            _poId = PO
            _destination = Destination
            _status = Status
            _attempts = Attempts
            _timeStamp = TimeStamp
            _resendAttempt = Resend
            _responseMessage = Response

        End Sub

        ''' <summary>
        ''' Protected method for creating a new instance of LogEntry.
        ''' Only use this method when creating a new entry in the transmission log.
        ''' </summary>
        ''' <param name="PO">The PO number being transmitted.</param>
        ''' <param name="Destination">The fax number the PO is being sent to.</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal PO As Integer, ByVal Destination As String, ByVal Status As FaxStatus)

            _poId = PO
            _destination = Destination
            _status = Status

        End Sub

        ''' <summary>
        ''' Protected method for creating a new instance of LogEntry.
        ''' Only use this method when recording a response from the fax service in the transmission log.
        ''' </summary>
        ''' <param name="PO">The PO number the response was received for.</param>
        ''' <param name="Response">The text of the response message from the fax server.</param>
        ''' <param name="Status">The status of the fax transmission.</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal PO As Integer, ByVal Destination As String, ByVal Response As String, ByVal Status As FaxStatus)

            _poId = PO
            _destination = Destination
            _responseMessage = Response
            _status = Status

        End Sub

#End Region

    End Class

#Region " Public Shared Methods"

    ''' <summary>
    ''' Creates a new entry in the order transmission log.
    ''' </summary>
    ''' <remarks>
    ''' Use this method from the SendOrders job for adding entries in the transmission log.
    ''' </remarks>
    Public Shared Sub CreateEntry(ByVal PO As Integer, ByVal Destination As String)

        Dim _entry As New LogEntry(PO, Destination, FaxStatus.TransmissionSent)

        IO.AddEntry(_entry)

    End Sub

    ''' <summary>
    ''' Updates the status of an existing fax transmission in the transmission log.
    ''' </summary>
    ''' <remarks>
    ''' Use this method from the CheckOrderStatus job to update fax transmission status.
    ''' </remarks>
    Public Shared Sub UpdateStatus(ByVal PO As Integer, ByVal Destination As String, ByVal Response As String, ByVal Status As FaxStatus)

        Dim _entry As New LogEntry(PO, Destination, Response, Status)

        IO.UpdateEntry(_entry)

    End Sub

    ''' <summary>
    ''' Gets a list of log entries for transmissions that have not been confirmed by the fax service.
    ''' </summary>
    ''' <returns>DataTable containing log file entries meeting Expiration Threshhold age.</returns>
    ''' <remarks></remarks>
    Public Shared Function GetUnconfirmedTransmissions() As DataTable

        Dim dt As DataTable = IO.GetExpiredEntries

        Return dt

    End Function

    ''' <summary>
    ''' Removes log entries that exceed the age specified in Retention Policy.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub PurgeTransmissionHistory()

        IO.PurgeTransmissionHistory()

    End Sub

    ''' <summary>
    ''' Retrieves an entry from the fax transmission log.
    ''' </summary>
    ''' <param name="PO">The PO number of the entry to retrieve.</param>
    ''' <returns>LogEntry object for the specified PO</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEntry(ByVal PO As Integer) As LogEntry

        Return IO.GetEntry(PO)

    End Function

#End Region

End Class
