Imports System.Net
Imports System.Net.Sockets
Imports System.ComponentModel

#Region " History "

''
'' 10-20-2005 - Changed buffer mechanism to allow storing of multiple objects
''                 Required if you use multiple sends on bitmaps otherwise
''                 they stack as one bitmap and don't retrieve properly.
''                 Utilizes the EOT (end of transmission) (ascii 4) to do
''                 the separating, although it still checks for a count
''                 less than the byte length for backwards compatability.
''            - Sending data now appends an EOT at the end of the data
'' 10-19-2005 - Added support for direct byte() sending and retrieving
''            - Added support for bitmap sending and retrieving
'' 08-24-2005 - Released
''

#End Region

<System.ComponentModel.DefaultEvent("HandleError")> _
Public Class Winsock
    Inherits System.ComponentModel.Component

#Region " Events "
    Public Event Connected(ByVal sender As Winsock)
    Public Event Disconnected(ByVal sender As Winsock)
    Public Event DataArrival(ByVal sender As Winsock, ByVal BytesTotal As Integer)
    Public Event ConnectionRequest(ByVal sender As Winsock, ByRef requestID As Socket)
    Public Event SendComplete(ByVal sender As Winsock)
    Public Event HandleError(ByVal sender As Winsock, ByVal Description As String, ByVal Method As String, ByVal myEx As String)
    Public Event StateChanged(ByVal sender As Winsock, ByVal state As WinsockStates)
#End Region

#Region " Variables "
    Private _RemoteIP As String = "127.0.0.1"
    Private _LocalPort As Integer = 80
    Private _RemotePort As Integer = 80
    Private _State As WinsockStates = WinsockStates.Closed
    Private _sBuffer As String
    Private _buffer() As Byte
    Private _bufferCol As Collection
    Private _byteBuffer(1024) As Byte
    Private _sockList As Socket
    Private _Client As Socket
#End Region

#Region " Constructors "
    Public Sub New()
        Me.New(80)
    End Sub
    Public Sub New(ByVal Port As Long)
        Me.New("127.0.0.1", Port)
    End Sub
    Public Sub New(ByVal IP As String)
        Me.New(IP, 80)
    End Sub
    Public Sub New(ByVal IP As String, ByVal Port As Long)
        RemoteIP = IP
        RemotePort = Port
        LocalPort = Port
        _bufferCol = New Collection
    End Sub
#End Region

#Region " Properties "
    Public Property LocalPort() As Integer
        Get
            Return _LocalPort
        End Get
        Set(ByVal Value As Integer)
            If GetState = WinsockStates.Closed Then
                _LocalPort = Value
            Else
                Throw New Exception("Must be idle to change the local port")
            End If
        End Set
    End Property
    Public Property RemotePort() As Integer
        Get
            Return _RemotePort
        End Get
        Set(ByVal Value As Integer)
            If GetState <> WinsockStates.Connected Then
                _RemotePort = Value
            Else
                Throw New Exception("Can't be connected to a server and change the remote port.")
            End If
        End Set
    End Property
    Public Property RemoteIP() As String
        Get
            Return _RemoteIP
        End Get
        Set(ByVal Value As String)
            If GetState = WinsockStates.Closed Then
                _RemoteIP = Value
            Else
                Throw New Exception("Must be closed to set the remote ip.")
            End If
        End Set
    End Property
    <Browsable(False)> Public ReadOnly Property GetState() As WinsockStates
        Get
            Return _State
        End Get
    End Property
#End Region

#Region " Methods "

    Public Sub Listen()
        Dim x As New System.Threading.Thread(AddressOf DoListen)
        x.Start()
    End Sub
    Public Sub Close()
        Try
            Select Case GetState
                Case WinsockStates.Listening
                    ChangeState(WinsockStates.Closing)
                    _sockList.Close()
                Case WinsockStates.Connected, WinsockStates.Connecting, WinsockStates.ConnectionPending, WinsockStates.HostResolved, WinsockStates.Open, WinsockStates.ResolvingHost
                    ChangeState(WinsockStates.Closing)
                    _Client.Close()
                Case WinsockStates.Closed
                    'do nothing
            End Select
            ChangeState(WinsockStates.Closed)
        Catch ex As Exception
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Public Sub Accept(ByVal requestID As Socket)
        Try
            _sockList.Close() 'Make sure we are no longer listening in case the connection request did not start on this socket
            ChangeState(WinsockStates.ConnectionPending)
            _Client = requestID
            RaiseEvent Connected(Me)
            ChangeState(WinsockStates.Connected)
            _Client.BeginReceive(_byteBuffer, 0, 1024, SocketFlags.None, AddressOf DoStreamReceive, Nothing)
        Catch ex As Exception
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Public Sub Connect()
        If GetState = WinsockStates.Connected Or GetState = WinsockStates.Listening Then
            RaiseEvent HandleError(Me, "Already open, must be closed first", "Connect", "Nothing here")
            Exit Sub
        End If
        Try
            Dim remIP As String
            ChangeState(WinsockStates.ResolvingHost)
            Try
                Dim x As System.Net.IPAddress
                x = IPAddress.Parse(_RemoteIP)
                remIP = x.ToString
            Catch ex1 As Exception
                'not a valid IP address - resolve DNS
                Try
                    Dim ip As IPHostEntry = Dns.GetHostEntry(_RemoteIP)
                    Dim t() As IPAddress = ip.AddressList
                    remIP = t(0).ToString
                Catch ex2 As Exception
                    ChangeState(WinsockStates.Error)
                    RaiseEvent HandleError(Me, ex2.Message, ex2.TargetSite.Name, ex2.ToString)
                End Try
            End Try
            ChangeState(WinsockStates.HostResolved)
            _Client = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Dim rEP As New IPEndPoint(IPAddress.Parse(remIP), RemotePort)
            '_Client.Connect(rEP)
            ChangeState(WinsockStates.Connecting)
            _Client.BeginConnect(rEP, New AsyncCallback(AddressOf OnConnected), Nothing)
        Catch ex As Exception
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Public Sub Connect(ByVal IP As String, ByVal Port As Integer)
        RemoteIP = IP
        RemotePort = Port
        Connect()
    End Sub

#End Region

#Region " Public Functions/Subs "
    Public Function LocalIP() As String
        'Dim h As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName)
        Dim h As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName)
        Return CType(h.AddressList.GetValue(0), Net.IPAddress).ToString
    End Function
    Public Function RemoteHostIP() As String
        Dim iEP As IPEndPoint = _Client.RemoteEndPoint
        Return iEP.Address.ToString
    End Function
#End Region

#Region " Send Overloads "

    Public Sub Send(ByVal Data As String)
        Dim sendBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(Data)
        Me.Send(sendBytes)
    End Sub
    Public Sub Send(ByVal Data() As Byte)
        Select Case GetState
            Case WinsockStates.Closed
                'can't send - not connected
            Case WinsockStates.Listening
                'listening
            Case WinsockStates.Connected
                Try
                    'send the bytes that are passed
                    ReDim Preserve Data(UBound(Data) + 1)
                    Data(UBound(Data)) = 4
                    _Client.Send(Data)
                    RaiseEvent SendComplete(Me)
                Catch ex As Exception
                    Me.Close()
                    ChangeState(WinsockStates.Error)
                    RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
                End Try
        End Select
    End Sub
    Public Sub Send(ByVal Data As Bitmap)
        Dim str As New System.IO.MemoryStream
        Data.Save(str, System.Drawing.Imaging.ImageFormat.Bmp)
        Dim sendBytes(str.Length - 1) As Byte
        str.Position = 0
        str.Read(sendBytes, 0, str.Length)
        Me.Send(sendBytes)
    End Sub

#End Region

#Region " GetData Overloads "

    Public Sub GetData(ByRef data As String)
        Dim byt() As Byte
        GetData(byt)
        For i As Integer = 0 To UBound(byt)
            If byt(i) = 10 Then
                data &= vbLf
            Else
                data &= ChrW(byt(i))
            End If
        Next
    End Sub
    Public Sub GetData(ByRef bytes() As Byte)
        If _bufferCol.Count = 0 Then Throw New IndexOutOfRangeException("Nothing in buffer.")
        Dim byt() As Byte = Me._bufferCol.Item(1)
        _bufferCol.Remove(1)
        ReDim bytes(UBound(byt))
        byt.CopyTo(bytes, 0)
    End Sub
    Public Sub GetData(ByRef bitmap As Bitmap)
        Dim byt() As Byte
        GetData(byt)
        Dim str As New System.IO.MemoryStream(byt, False)
        bitmap = Image.FromStream(str)
    End Sub

#End Region

#Region " Private Functions/Subs "
    Private Sub ChangeState(ByVal new_state As WinsockStates)
        _State = new_state
        RaiseEvent StateChanged(Me, _State)
    End Sub
    Private Sub OnConnected(ByVal asyn As IAsyncResult)
        Try
            _Client.EndConnect(asyn)
            Me.ClientFinalizeConnection()
        Catch ex As Exception
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Private Sub ClientFinalizeConnection()
        ChangeState(WinsockStates.Connected)
        _Client.BeginReceive(_byteBuffer, 0, 1024, SocketFlags.None, AddressOf DoRead, Nothing)
        RaiseEvent Connected(Me)
    End Sub
    Private Sub DoListen()
        Try
            _sockList = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Dim ipLocal As New IPEndPoint(IPAddress.Any, LocalPort)
            _sockList.Bind(ipLocal)
            _sockList.Listen(1)
            ChangeState(WinsockStates.Listening)
            _sockList.BeginAccept(New AsyncCallback(AddressOf OnClientConnect), Nothing)
        Catch ex As Exception
            Me.Close()
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Private Sub OnClientConnect(ByVal asyn As IAsyncResult)
        Try
            Dim tmpSock As Socket
            If GetState = WinsockStates.Listening Then
                tmpSock = _sockList.EndAccept(asyn)
                _sockList.Close() 'Don't accept connections - hosting app. will spawn a new process to listen for new connections
                RaiseEvent ConnectionRequest(Me, tmpSock)
                '_sockList.BeginAccept(New AsyncCallback(AddressOf OnClientConnect), Nothing)
            End If
        Catch ex As Exception
            Me.Close()
            ChangeState(WinsockStates.Error)
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Private Sub DoStreamReceive(ByVal ar As IAsyncResult)
        Dim intCount As Integer
        Try
            SyncLock _Client
                intCount = _Client.EndReceive(ar)
            End SyncLock
            If intCount < 1 Then
                Me.Close()
                ReDim _byteBuffer(1024)
                RaiseEvent Disconnected(Me)
                Exit Sub
            End If
            AddToBuffer(_byteBuffer, intCount)
            'BuildString(_byteBuffer, 0, intCount)
            Array.Clear(_byteBuffer, 0, intCount)
            SyncLock _Client
                _Client.BeginReceive(_byteBuffer, 0, 1024, SocketFlags.None, AddressOf DoStreamReceive, Nothing)
            End SyncLock
        Catch ex As Exception
            Me.Close()
            ReDim _byteBuffer(1024)
            RaiseEvent Disconnected(Me)
        End Try
    End Sub
    Private Sub DoRead(ByVal ar As IAsyncResult)
        Dim intCount As Integer
        Try
            intCount = _Client.EndReceive(ar)
            If intCount < 1 Then
                Me.Close()
                ReDim _byteBuffer(1024)
                RaiseEvent Disconnected(Me)
                Exit Sub
            End If
            AddToBuffer(_byteBuffer, intCount)
            'BuildString(_byteBuffer, 0, intCount)
            Array.Clear(_byteBuffer, 0, intCount)
            _Client.BeginReceive(_byteBuffer, 0, 1024, SocketFlags.None, AddressOf DoRead, Nothing)
        Catch ex As Exception
            Me.Close()
            ReDim _byteBuffer(1024)
            RaiseEvent Disconnected(Me)
        End Try
    End Sub
    Private Sub BuildString(ByVal Bytes() As Byte, ByVal offset As Integer, ByVal count As Integer)
        Try
            Dim intIndex As Integer
            For intIndex = offset To offset + count - 1
                If Bytes(intIndex) = 10 Then
                    _sBuffer &= vbLf
                Else
                    _sBuffer &= ChrW(Bytes(intIndex))
                End If
            Next
            If _sBuffer.IndexOf(vbCrLf) <> -1 Then
                RaiseEvent DataArrival(Me, count)
                Array.Clear(_byteBuffer, 0, _byteBuffer.Length)
            End If
        Catch ex As Exception
            RaiseEvent HandleError(Me, ex.Message, ex.TargetSite.Name, ex.ToString)
        End Try
    End Sub
    Private Sub AddToBuffer(ByVal bytes() As Byte, ByVal count As Integer)
        Dim curUB As Integer
        If Not _buffer Is Nothing Then curUB = UBound(_buffer) Else curUB = -1
        Dim newUB As Integer = curUB + count
        ReDim Preserve _buffer(newUB)
        Array.Copy(bytes, 0, _buffer, curUB + 1, count)
        Dim byterm As Byte = 4
        Dim idx As Integer = Array.IndexOf(_buffer, byterm)
        Dim byObj() As Byte
        Dim byTmp() As Byte
        While idx <> -1
            'found an EOT (end of transmission) marker split it if necessary and
            'put it in the buffer to get - call DataArrival
            Dim ln As Integer = _buffer.Length - (idx + 1)
            ReDim byObj(idx - 1)
            ReDim byTmp(ln - 1)
            Array.Copy(_buffer, 0, byObj, 0, idx)
            Array.Copy(_buffer, idx + 1, byTmp, 0, ln)
            ReDim _buffer(UBound(byTmp))
            Array.Copy(byTmp, _buffer, byTmp.Length)
            Me._bufferCol.Add(byObj)
            RaiseEvent DataArrival(Me, byObj.Length)
            idx = Array.IndexOf(_buffer, byterm)
        End While
        If count < bytes.Length - 1 And _buffer.Length > 0 Then
            _bufferCol.Add(_buffer)
            RaiseEvent DataArrival(Me, _buffer.Length)
            _buffer = Nothing
        End If
    End Sub
#End Region

End Class

#Region " Enumerations "

Public Enum WinsockStates
    Closed = 0
    Open = 1
    Listening = 2
    ConnectionPending = 3
    ResolvingHost = 4
    HostResolved = 5
    Connecting = 6
    Connected = 7
    Closing = 8
    [Error] = 9
    'Listening = 1
    'Connected = 2
End Enum

#End Region