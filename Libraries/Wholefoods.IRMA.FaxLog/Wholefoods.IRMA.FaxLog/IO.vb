Imports System.Xml.Linq
Imports System.IO
Imports WholeFoods.IRMA.FaxLog.Log
Imports System.Configuration

''' <summary>
''' Manages the IO operations for the fax transmission log.
''' </summary>
''' <remarks></remarks>
Friend Class IO

#Region " Private Shared Fields"

    Private Shared LOG_FILE As String
    Private Shared RETENTION_DAYS As Integer = -1
    Private Shared EXPIRATION_MINUTES As Integer

#End Region

#Region " Protected Friend Properties"

    ''' <summary>
    ''' The location of the fax transmission log file.
    ''' </summary>
    Protected Friend Shared Property FaxLogFile() As String
        Get
            Return LOG_FILE
        End Get
        Set(ByVal value As String)
            LOG_FILE = value
        End Set
    End Property

    ''' <summary>
    ''' The transmission log Retention Policy (in days).
    ''' </summary>
    Protected Friend Shared Property RetentionPolicy() As Integer
        Get
            Return RETENTION_DAYS
        End Get
        Set(ByVal value As Integer)
            RETENTION_DAYS = value
        End Set
    End Property

    ''' <summary>
    ''' The transmission log Expiration Threshhold (in minutes).
    ''' </summary>
    Protected Friend Shared Property ExpirationThreshold() As Integer
        Get
            Return EXPIRATION_MINUTES
        End Get
        Set(ByVal value As Integer)
            EXPIRATION_MINUTES = value
        End Set
    End Property

#End Region

#Region " Protected Friend Shared Methods"

    ''' <summary>
    ''' Retrieves an XDocument containing the fax transmission log entries.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function GetTransmissionLog() As XDocument

        Try
            Dim _xDoc As XDocument = Nothing

            If Not File.Exists(LOG_FILE) Then
                _xDoc = <?xml version="1.0" encoding="utf-8"?>
                        <log></log>
                SaveTransmissionLog(_xDoc)
            Else
                _xDoc = XDocument.Load(LOG_FILE)
            End If

            Return _xDoc

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Saves the XDocument transmission log file to disk.
    ''' </summary>
    ''' <param name="Log">XDocument containing the transmission log entries.</param>
    ''' <returns>True indicates a successful write to disk.</returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function SaveTransmissionLog(ByVal Log As XDocument) As Boolean

        Try
            If Not Directory.Exists(Path.GetDirectoryName(LOG_FILE)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(LOG_FILE))
            End If
            Log.Save(LOG_FILE)
            Return True
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Retrieves an entry from the fax transmission log.
    ''' </summary>
    ''' <param name="PO">The PO number of the entry to retrieve.</param>
    ''' <returns>LogEntry object for the specified PO</returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function GetEntry(ByVal PO As Integer) As LogEntry

        Try

            Dim _xDoc As XDocument = Nothing

            If File.Exists(LOG_FILE) Then

                _xDoc = GetTransmissionLog()

                Dim _log = From entry In _xDoc...<entry> _
                            Where entry.@po = PO

                Dim _entry As New LogEntry(_log.@po, _log.@destination, CType([Enum].Parse(GetType(FaxStatus), _log.@status, True), FaxStatus), _log.@attempts, CDate(_log.@timestamp), _log.@resend, _log.<response>.Value)

                Return _entry

            Else
                Throw New IOException("The log file does not exist. Log entry could not be retrieved.")
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Adds a new entry in the fax transmission log.
    ''' </summary>
    ''' <param name="Entry">LogEntry object</param>
    ''' <returns>True indicates entry was added successfully.</returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function AddEntry(ByVal Entry As LogEntry) As Boolean

        Try

            Dim _xDoc As XDocument

            If File.Exists(LOG_FILE) Then
                _xDoc = XDocument.Load(LOG_FILE)
            Else
                _xDoc = <?xml version="1.0" encoding="utf-8"?>
                        <log></log>
            End If

            Dim _entry = From entries In _xDoc...<entry> _
                           Where entries.@po = Entry.PO

            Dim bResendAttempt As Boolean

            Dim iCount As Integer = _entry.@attempts

            If iCount > 0 Then
                bResendAttempt = True
            Else
                bResendAttempt = False
            End If

            _entry.Remove()

            Dim _newEntry = <entry
                                po=<%= Entry.PO %>
                                destination=<%= Entry.Destination %>
                                status=<%= Entry.Status.ToString %>
                                attempts=<%= iCount + 1 %>
                                timestamp=<%= Date.Now.ToString %>
                                resend=<%= bResendAttempt %>
                                >
                                <response><%= Entry.Response %></response>
                            </entry>

            _xDoc.Root.Add(_newEntry)

            _xDoc.Save(LOG_FILE)

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Updates an existing transmission log entry.
    ''' </summary>
    ''' <param name="Entry">Entry Object</param>
    ''' <returns></returns>
    ''' <remarks>If the entry specified in the Entry Object does not exist, a new entry is created from it.</remarks>
    Protected Friend Shared Function UpdateEntry(ByVal Entry As LogEntry) As Boolean

        Try
            Dim _xDoc As XDocument

            If File.Exists(LOG_FILE) Then
                _xDoc = XDocument.Load(LOG_FILE)
            Else
                _xDoc = <?xml version="1.0" encoding="utf-8"?>
                        <log></log>
            End If

            Dim _entry = From entries In _xDoc...<entry> _
                           Where entries.@po = Entry.PO

            If _entry.@po = Nothing Then
                AddEntry(Entry)
                Return True
            Else
                _entry.@timestamp = Date.Now.ToString
                _entry.<response>.Value = Entry.Response
                _entry.@status = Entry.Status.ToString
                _xDoc.Save(LOG_FILE)
                Return True
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Removes log entries that exceed the age specified in Retention Policy.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Shared Sub PurgeTransmissionHistory()

        Try
            If RETENTION_DAYS = -1 Then
                Throw New Exception("The Retention Days Policy has not been configured.")
            Else

                Dim _xDoc As XDocument = Nothing

                If File.Exists(LOG_FILE) Then

                    _xDoc = GetTransmissionLog()

                    Dim _log = From entries In _xDoc...<entry> _
                              Where CDate(entries.@timestamp) < Date.Today.AddDays(RetentionPolicy * -1)

                    _log.Remove()

                    _xDoc.Save(LOG_FILE)

                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Gets a list of log entries for transmissions that have not been confirmed by the fax service.
    ''' </summary>
    ''' <returns>DataTable containing log file entries meeting Expiration Threshhold age.</returns>
    ''' <remarks></remarks>
    Protected Friend Shared Function GetExpiredEntries() As DataTable

        Dim dt As New DataTable
        Dim dr As DataRow

        Try

            Dim _log = IO.GetTransmissionLog()

            Dim _unConf = (From entries In _log...<entry> _
                          Where (CType([Enum].Parse(GetType(FaxStatus), entries.@status, True), FaxStatus) = FaxStatus.TransmissionSent) _
                          And CDate(entries.@timestamp) < Date.Now.AddMinutes(ExpirationThreshold * -1)).ToArray

            dt.Columns.Add("PO")
            dt.Columns.Add("Destination")
            dt.Columns.Add("Status")
            dt.Columns.Add("Attempts")
            dt.Columns.Add("Timestamp")
            dt.Columns.Add("Resend")
            dt.Columns.Add("Response")

            For Each entry In _unConf

                dr = dt.NewRow()

                dr.Item("PO") = _unConf.@po
                dr.Item("Destination") = _unConf.@destination
                dr.Item("Status") = _unConf.@status
                dr.Item("Attempts") = _unConf.@attempts
                dr.Item("Timestamp") = _unConf.@timestamp
                dr.Item("Resend") = _unConf.@resend
                dr.Item("Response") = _unConf.<response>

                dt.Rows.Add(dr)

            Next

            Return dt

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

End Class
