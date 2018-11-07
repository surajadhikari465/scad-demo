Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.Utility

Public Class IBMProcessStart

    Private _ftpInfo As StoreFTPConfigBO
    Private _storeNo As Integer
    Private _lcount As Integer
    Private _lPasswordCnt As Integer
    Private mbSendDataCompleted As Boolean

#Region "properties"

    Public Property FTPInfo() As StoreFTPConfigBO
        Get
            Return _ftpInfo
        End Get
        Set(ByVal value As StoreFTPConfigBO)
            _ftpInfo = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _storeNo
        End Get
        Set(ByVal value As Integer)
            _storeNo = value
        End Set
    End Property

#End Region

    Public Function StartIBMProcess(ByVal ftpInfo As StoreFTPConfigBO, ByVal StoreNo As Integer) As Boolean

        _storeNo = StoreNo
        _ftpInfo = ftpInfo

        Me.Show()

        Call IBMProcessStart_Load()

    End Function

    'Private Sub IBMProcessStart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Private Sub IBMProcessStart_Load()
        'Me.Hide()

        'tmrTimer.Enabled = False
        'tmrTimer.Interval = 60000
        'tmrTimer.Enabled = True

        ''-- Start EAMDMSIL
        'Debug.Print("StoreNo = " & Me.StoreNo.ToString)
        'Try
        '    wsClient.Close()
        '    wsClient.LocalPort = 80
        '    'wsClient.Connect(_ftpInfo.IPAddress, 23) '_ftpInfo.Port) 'use Telnet port 23
        '    wsClient.Connect(_ftpInfo.IPAddress, 21) '_ftpInfo.Port)  'use FTP port 21

        'Catch ex As Exception
        '    Logger.LogError("Error connecting to run batches for store " & _storeNo & ": " & ex.Message, Me.GetType)

        '    'send message about exception
        '    Dim args(3) As String
        '    args(0) = "EAMDMSIL (Error connecting to run batches for store)"
        '    args(1) = _storeNo.ToString
        '    args(2) = ex.Message
        '    ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

        'End Try

        'With wsClient
        '    Debug.Print("   RemoteHostIP = " & .RemoteHostIP)
        '    Debug.Print("   RemoteIP = " & .RemoteIP)
        '    Debug.Print("   LocalIP = " & .LocalIP)
        '    Debug.Print("   RemoteHostPort = " & .RemoteHostPort.ToString)
        '    Debug.Print("   RemotePort = " & .RemotePort.ToString)
        '    Debug.Print("   LocalPort = " & .LocalPort.ToString)
        '    Debug.Print("   GetState = " & .GetState.ToString & " (before loop)")

        '    Dim nPauseTime As Double = Timer + 3   'wait 3 seconds
        '    Do While .GetState = Winsock_Control.WinsockStates.Connecting
        '        'wait for connection
        '        If Timer > nPauseTime Then
        '            Debug.Print("   < Connection timeout! >")
        '            Exit Do
        '        End If
        '    Loop
        '    Debug.Print("   GetState = " & .GetState.ToString & " (after loop)")
        'End With

        'Debug.Print("StoreNo = " & Me.StoreNo.ToString & " [wsREXEC]")
        'Try
        '    wsREXEC.Close()
        '    wsREXEC.LocalPort = 2023
        '    wsREXEC.Connect(_ftpInfo.IPAddress, 23) '_ftpInfo.Port)

        'Catch ex As Exception
        '    Logger.LogError("Error connecting to run batches for store " & _storeNo & ": " & ex.Message, Me.GetType)

        '    'send message about exception
        '    Dim args(3) As String
        '    args(0) = "EAMDMSIL (Error connecting to run batches for store)"
        '    args(1) = _storeNo.ToString
        '    args(2) = ex.Message
        '    ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

        'End Try

        'With wsREXEC
        '    Debug.Print("   Created = " & .Created.ToString)
        '    Debug.Print("   RemotePort = " & .RemotePort.ToString)
        '    Debug.Print("   RemoteHost = " & .RemoteHost)
        '    Debug.Print("   RemoteHostIP = " & .RemoteHostIP)
        '    Debug.Print("   CtlState = " & .CtlState.ToString & " (before loop)")

        '    Dim nPauseTime As Double = Timer + 3   'wait 10 seconds
        '    Do While .CtlState = 6  'MSWinsockLib.StateConstants.sckConnecting
        '        'wait for connection
        '        If Timer > nPauseTime Then
        '            Debug.Print("   < Connection timeout! >")
        '            Exit Do
        '        End If
        '    Loop
        '    Debug.Print("   CtlState = " & .CtlState.ToString & " (after loop)")
        'End With
    End Sub

    '    Private Sub AxWinsock1_ConnectEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsREXEC.ConnectEvent
    '        ' causes the tcp control to attempt to transmit any buffered data previously submitted
    '        _lcount = 0
    '        _lPasswordCnt = 0
    '    End Sub

    '    Private Sub AxWinsock1_DataArrival(ByVal sender As Object, ByVal e As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles wsREXEC.DataArrival
    '        Dim sTemp As String
    '        sTemp = String.Empty

    '        On Error GoTo me_err

    '        wsREXEC.GetData(CType(sTemp, Object))

    '        Select Case True
    '            Case InStr(1, sTemp, "Operator", CompareMethod.Binary) > 0
    '                'wsREXEC.SendData(_ftpInfo.FTPUser & vbCrLf)
    '                wsREXEC.SendData("4690" & vbCrLf)
    '            Case InStr(1, sTemp, "Password", CompareMethod.Binary) > 0
    '                If _lPasswordCnt >= 3 Then
    '                    Logger.LogError("POS Telnet password, '" & _ftpInfo.FTPPassword & "', invalid for store: " & _storeNo, Me.GetType)

    '                    'send message about exception
    '                    Dim args(3) As String
    '                    args(0) = "EAMDMSIL (POS Telnet password, '" & _ftpInfo.FTPPassword & "', invalid for store)"
    '                    args(1) = _storeNo.ToString
    '                    args(2) = "N/A"
    '                    ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

    '                    wsREXEC.Close()
    '                    Me.Close()
    '                Else
    '                    _lPasswordCnt += 1
    '                    'wsREXEC.SendData(_ftpInfo.FTPPassword & vbCrLf)
    '                    wsREXEC.SendData("4690" & vbCrLf)
    '                End If
    '            Case InStr(1, sTemp, "selection", CompareMethod.Binary) > 0
    '                wsREXEC.SendData("7" & vbCrLf)
    '            Case InStr(1, sTemp, "C:>", CompareMethod.Binary) > 0
    '                If _lcount >= 3 Then
    '                    wsREXEC.Close()
    '                    Me.Close()
    '                Else
    '                    _lcount += 1
    '                    'If _lcount = 1 Then mbSendDataCompleted = False 'reset SendData complete flag only for the first attempt so we can make sure it is successful at least once
    '                    wsREXEC.SendData("EAMDMSIL" & vbCrLf)
    '                End If
    '        End Select

    '        Exit Sub

    'me_err:
    '        'send message about exception
    '        Dim args2(3) As String
    '        args2(0) = "EAMDMSIL (wsREXEC_DataArrival failed with error: " & Err.Number & ")"
    '        args2(1) = _storeNo.ToString
    '        args2(2) = Err.Description
    '        ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args2, SeverityLevel.Fatal)
    '    End Sub

    '    Private Sub AxWinsock1_Error(ByVal sender As Object, ByVal e As AxMSWinsockLib.DMSWinsockControlEvents_ErrorEvent) Handles wsREXEC.Error
    '        Logger.LogError("Error running batches, wsREXEC_Error, for store " & _storeNo & ": " & e.description, Me.GetType)

    '        'send message about exception
    '        Dim args(3) As String
    '        args(0) = "EAMDMSIL (Error running batches, wsREXEC_Error, for store)"
    '        args(1) = _storeNo.ToString
    '        args(2) = e.description
    '        ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

    '        'mbSendDataCompleted = False
    '        Me.Close()
    '    End Sub


    '    Private Sub AxWinsock1_SendComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsREXEC.SendComplete
    '        'mbSendDataCompleted = True
    '    End Sub

    'Private Sub wsClient_Connected(ByVal sender As Winsock_Control.Winsock) Handles wsClient.Connected
    '    ' causes the tcp control to attempt to transmit any buffered data previously submitted
    '    _lcount = 0
    '    _lPasswordCnt = 0
    'End Sub

    'Private Sub wsClient_ConnectionRequest(ByVal sender As Winsock_Control.Winsock, ByRef requestID As System.Net.Sockets.Socket) Handles wsClient.ConnectionRequest

    'End Sub

    'Private Sub wsClient_DataArrival(ByVal sender As Winsock_Control.Winsock, ByVal BytesTotal As Integer) Handles wsClient.DataArrival

    '    Dim sOutputData As String = String.Empty
    '    Dim sOperatorID As String = "4690" '= _ftpInfo.FTPUser
    '    Dim sPassword As String = "4690" '= _ftpInfo.FTPPassword

    '    tmrTimer.Enabled = False
    '    tmrTimer.Interval = 60000
    '    tmrTimer.Enabled = True

    '    Try
    '        wsClient.GetData(sOutputData)

    '    Catch ex As Exception
    '        'send message about exception
    '        Dim args2(3) As String
    '        args2(0) = "EAMDMSIL: wsClient.GetData() failed with error [wsClient.DataArrival]"
    '        args2(1) = _storeNo.ToString
    '        args2(2) = ex.Message
    '        ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args2, SeverityLevel.Fatal)

    '        Exit Sub

    '    End Try

    '    Logger.LogError(String.Format("Store {0} telnet output: '{1}'", _storeNo, sOutputData), Me.GetType)

    '    Select Case True
    '        Case InStr(1, sOutputData, "Operator", CompareMethod.Binary) > 0
    '            wsClient.Send(sOperatorID & vbCrLf)

    '        Case InStr(1, sOutputData, "Password", CompareMethod.Binary) > 0
    '            If _lPasswordCnt >= 3 Then
    '                Logger.LogError(String.Format("POS Telnet password, '{0}', invalid for store: {1}", sPassword, _storeNo), Me.GetType)

    '                'send message about exception
    '                Dim args(3) As String
    '                args(0) = String.Format("EAMDMSIL (POS Telnet password, '{0}', invalid for store)", sPassword)
    '                args(1) = _storeNo.ToString
    '                args(2) = "N/A"
    '                ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

    '                wsClient.Close()
    '                Me.Close()
    '            Else
    '                _lPasswordCnt += 1
    '                wsClient.Send(sPassword & vbCrLf)
    '            End If

    '        Case InStr(1, sOutputData, "selection", CompareMethod.Binary) > 0
    '            wsClient.Send("7" & vbCrLf)     ' select menu option 7 (Command Mode)

    '        Case InStr(1, sOutputData, "C:>", CompareMethod.Binary) > 0, _
    '            InStr(1, sOutputData, "C>", CompareMethod.Binary) > 0
    '            If _lcount >= 3 Then
    '                wsClient.Close()
    '                Me.Close()
    '            Else
    '                _lcount += 1
    '                If _lcount = 1 Then
    '                    'reset SendData complete flag only for the first attempt to ensure it is successful at least once
    '                    mbSendDataCompleted = False
    '                End If
    '                wsClient.Send("EAMDMSIL" & vbCrLf)
    '            End If

    '            '----------------------------------------------------------------------
    '        Case InStr(1, sOutputData, "User", CompareMethod.Binary) > 0
    '            wsClient.Send(_ftpInfo.FTPUser & vbCrLf)

    '        Case InStr(1, sOutputData, "Password:", CompareMethod.Binary) > 0
    '            wsClient.Send(_ftpInfo.FTPPassword & vbCrLf)

    '        Case InStr(1, sOutputData, "ready", CompareMethod.Binary) > 0
    '            wsClient.Send("User")

    '        Case Else
    '            'wsClient.Send("help")
    '    End Select

    'End Sub

    'Private Sub wsClient_Disconnected(ByVal sender As Winsock_Control.Winsock) Handles wsClient.Disconnected

    'End Sub

    'Private Sub wsClient_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsClient.Disposed

    'End Sub

    'Private Sub wsClient_HandleError(ByVal sender As Winsock_Control.Winsock, ByVal Description As String, ByVal Method As String, ByVal myEx As String) Handles wsClient.HandleError
    '    Logger.LogError(String.Format("Error running batches, wsREXEC_Error, for store {0} : {1}", _storeNo, Description), Me.GetType)

    '    'send message about exception
    '    Dim args(3) As String
    '    args(0) = "EAMDMSIL (Error running batches, wsREXEC_Error, for store)"
    '    args(1) = _storeNo.ToString
    '    args(2) = Description
    '    ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Fatal)

    '    mbSendDataCompleted = False

    '    Me.Close()
    'End Sub

    'Private Sub wsClient_SendComplete(ByVal sender As Winsock_Control.Winsock) Handles wsClient.SendComplete

    'End Sub

    'Private Sub wsClient_StateChanged(ByVal sender As Winsock_Control.Winsock, ByVal state As Winsock_Control.WinsockStates) Handles wsClient.StateChanged

    'End Sub

    'Private Sub tmrTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrTimer.Tick

    '    tmrTimer.Enabled = False

    '    Try
    '        wsClient.Close()

    '        'If (Not mbSendDataCompleted) Or (lcount = 0) Then 'Did not run command
    '        '    LogError("Did not complete batch run for store " & glStoreNo)
    '        '    SendMail(gsDCEmail, "Replenishment: Batch Run Failed", "Did not complete batch run for store " & glStoreNo)
    '        'End If

    '    Catch ex As Exception

    '    Finally
    '        Me.Close()
    '    End Try
    'End Sub
End Class