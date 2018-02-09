Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Data
Imports System.Text
Imports Microsoft.WindowsCE.Forms
Imports System.ServiceModel


Public Class MainForm

    Private mySession As Session
    Private notify As Notification = Nothing
    Private serviceFault As ParsedCFFaultException = Nothing
    Private errorMethod As String
    Private _servername As String
    Private _serviceURI As String
    Private _regionCode As String
    Private _userName As String
    Private _userEmail As String
    Private _userAuthenticted As Boolean
    Private _userID As Integer
    Private _pluginName As String
    Private _isShrinkUser As Boolean
    Private _isBuyerUser As Boolean
    Private _isReceiverUser As Boolean
    Private serverDateTime As DateTime

#Region " Public Properties"
    Public Property ServerName() As String
        Get
            Return _servername
        End Get
        Set(ByVal value As String)
            _servername = value
        End Set
    End Property

    Public Property ServiceURI() As String
        Get
            Return _serviceURI
        End Get
        Set(ByVal value As String)
            _serviceURI = value
        End Set
    End Property

    Public Property RegionCode() As String
        Get
            Return _regionCode
        End Get
        Set(ByVal value As String)
            _regionCode = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Public Property UserEmail() As String
        Get
            Return _userEmail
        End Get
        Set(ByVal value As String)
            _userEmail = value
        End Set
    End Property

    Public Property UserAuthenticated() As Boolean
        Get
            Return _userAuthenticted
        End Get
        Set(ByVal value As Boolean)
            _userAuthenticted = value
        End Set
    End Property

    Public Property PluginName() As String
        Get
            Return _pluginName
        End Get
        Set(ByVal value As String)
            _pluginName = value
        End Set
    End Property

#End Region

#Region " Constructors"

    Public Shared Sub Main()
        Application.Run(New MainForm())
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        '' Add any initialization after the InitializeComponent() call.
        '' TEST PURPOSES ONLY:
        'Me.ServiceURI = "http://cewd6589.wfm.pvt:8081/Irma.svc/basic"

        ''Me.ServiceURI = "http://iat-ma/IRMAService/IRMA.svc/basic"
        'Me.UserName = "2096654"
        'Me.RegionCode = "RM"
        'Me.UserAuthenticated = True

    End Sub

#End Region

#Region " Form Events"

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()

        mySession = New Session(Me.ServiceURI)

        Dim tmpSecurity() As UserRole = Nothing

        Try
            tmpSecurity = mySession.WebProxyClient.GetUserRole(Me.UserName)
            serverDateTime = mySession.WebProxyClient.GetSystemDate()

            ' Explicitly handle service faults, timeouts, and connection failures.  Since this is the application's main form, 
            ' there is no fallback state, and the app will close on any connection-related errors.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.Close()

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MainForm_Load")
            Me.Close()

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MainForm_Load")
            Me.Close()

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "MainForm_Load")
        End Try

        If tmpSecurity Is Nothing Then
            ' The service call failed.  Allow the form to close.
            Exit Sub
        Else
            If tmpSecurity.Length > 0 Then
                ' The user's account was found.

                If tmpSecurity(0).IsAccountEnabled Then
                    ' The user's account is enabled.

                    SetPermissions(tmpSecurity)

                    mySession.UserName = Me.UserName
                    mySession.UserID = tmpSecurity(0).UserID
                    mySession.UserStore = tmpSecurity(0).TelxonStoreLimit
                    mySession.CurrentScreen = Session.CurrentScreenType.MainForm

                    Try
                        PopulateDataTables()

                        Dim dr As DataRow
                        Dim dtStores As New DataTable

                        dtStores = mySession.Stores
                        dr = dtStores.NewRow()
                        dr.Item("DisplayMember") = "-- Select Store --"
                        dr.Item("ValueMember") = -1
                        dtStores.Rows.InsertAt(dr, 0)

                        Dim dtSubteams As New DataTable
                        dtSubteams = mySession.Subteams
                        dr = dtSubteams.NewRow()
                        dr.Item("DisplayMember") = "-- Select Subteam --"
                        dr.Item("ValueMember") = -1
                        dtSubteams.Rows.InsertAt(dr, 0)

                        Me.StoreComboBox.DataSource = dtStores
                        Me.StoreComboBox.DisplayMember = "DisplayMember"
                        Me.StoreComboBox.ValueMember = "ValueMember"

                        Me.SubteamComboBox.DataSource = dtSubteams
                        Me.SubteamComboBox.DisplayMember = "DisplayMember"
                        Me.SubteamComboBox.ValueMember = "ValueMember"

                        If mySession.UserStore <> -1 Then
                            Me.StoreComboBox.SelectedValue = mySession.UserStore
                        Else
                            Me.StoreComboBox.SelectedValue = -1
                        End If

                        ' Users can only change their location if they are a SuperUser, a Coordinator, or have ALL stores in their IRMA setup.
                        mySession.UserAllStoresAccess = IIf(tmpSecurity(0).TelxonStoreLimit = -1, True, (tmpSecurity(0).IsCoordinator Or tmpSecurity(0).IsSuperUser))

                        Me.StoreComboBox.Enabled = mySession.UserAllStoresAccess
                        Me.SubteamComboBox.SelectedValue = -1

                        Cursor.Current = Cursors.Default

                        ' Explicitly handle service faults, timeouts, and connection failures.
                    Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                        serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                        Dim err As New ErrorHandler(serviceFault)
                        err.ShowErrorNotification()
                        Me.Close()
                    Catch ex As TimeoutException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "MainForm_Load")
                        Me.Close()
                    Catch ex As CommunicationException
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "MainForm_Load")
                        Me.Close()
                    Catch ex As Exception
                        Dim err As New ErrorHandler()
                        err.DisplayErrorMessage(ex.Message, "MainForm_Load")
                    End Try

                    SetVisibility()
                Else
                    SetVisibility(True)
                    MessageBox.Show(Messages.NO_ACCTAUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                    DialogResult = Windows.Forms.DialogResult.Abort
                End If

            Else
                SetVisibility(True)
                MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                DialogResult = Windows.Forms.DialogResult.Abort
            End If
        End If

    End Sub

#End Region

#Region " Control Events"

    Private Sub cmdShrink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdShrink.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf _isShrinkUser = False Then
            MessageBox.Show(Messages.NO_ROLEAUTH & "(Shrink Role)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        ElseIf mySession.IsSubTeamFixedSpoilage(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.FIXED_SPOILAGE, Me.SubteamComboBox.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        ElseIf Not mySession.CanAcceptShrink(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.INVALID_SHRINK_SUBTEAM, Me.SubteamComboBox.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        Else
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False
            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)

            Dim main As ShrinkType = New ShrinkType(Me.mySession)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        End If

    End Sub

    Private Sub cmdOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOrder.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf _isBuyerUser = False Then
            MessageBox.Show(Messages.NO_ROLEAUTH & "(Buyer Role)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        ElseIf mySession.IsExpenseSubteam(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.EXPENSE_SUBTEAM, Trim(Me.SubteamComboBox.Text)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        ElseIf mySession.IsPackagingSubteam(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.PACKAGING_SUBTEAM, Trim(Me.SubteamComboBox.Text)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        Else
            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.Order
            mySession.StartTime = serverDateTime
            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)

            Dim main As ItemScan = New ItemScan(Me.mySession, False)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        End If

    End Sub

    Private Sub cmdTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTransfer.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf _isBuyerUser = False Then
            MessageBox.Show(Messages.NO_ROLEAUTH & "(Buyer Role)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else

            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.Transfer
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)
            Dim main As TransferMain = New TransferMain(Me.mySession)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If

    End Sub

    Private Sub cmdCredit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCredit.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf _isBuyerUser = False Then
            MessageBox.Show(Messages.NO_ROLEAUTH & "(Buyer Role)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        ElseIf mySession.IsExpenseSubteam(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.EXPENSE_SUBTEAM, Trim(Me.SubteamComboBox.Text)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        ElseIf mySession.IsPackagingSubteam(Me.SubteamComboBox.SelectedValue) Then
            MessageBox.Show(String.Format(Messages.PACKAGING_SUBTEAM, Trim(Me.SubteamComboBox.Text)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        Else
            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.Credit
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)
            Dim main As ItemScan = New ItemScan(Me.mySession, False)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If

    End Sub

    Private Sub cmdItemCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemCheck.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        End If

        If Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString()) Then
            MessageBox.Show("Please select a store to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else
            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.ItemCheck
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)

            Dim main As ItemScan = New ItemScan(Me.mySession, True)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

            main.Dispose()

        End If

    End Sub

    Private Sub cmdPrintSign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrintSign.Click

        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        End If

        If (Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else

            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.PrintSign
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)
            Dim main As ItemScan = New ItemScan(Me.mySession, True)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If
    End Sub

    Private Sub cmdReceive_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReceive.Click
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me._isReceiverUser = False Then
            MessageBox.Show(Messages.NO_ROLEAUTH & "(Receiver Role)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        End If

        If (Me.StoreComboBox.SelectedIndex = 0 Or String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else
            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.Receive
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            If Me.SubteamComboBox.SelectedValue >= 0 Then
                mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            Else
                mySession.SetSubteamKey("-1,")
            End If

            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)

            Cursor.Current = Cursors.Default
            Dim uMessage As New dlgReceivingDocument
            uMessage.ShowDialog()

            Dim dlgResult As String = uMessage.results
            uMessage = Nothing

            If dlgResult = "R" Then
                mySession.CurrentScreen = Session.CurrentScreenType.ReceiveOrder

                Dim main As ReceiveOrder = New ReceiveOrder(Me.mySession)
                If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                End If
                main.Dispose()
            ElseIf dlgResult = "RD" Then
                mySession.CurrentScreen = Session.CurrentScreenType.ReceivingDocumentScan

                Dim rdMain As ReceiveDocumentMain = New ReceiveDocumentMain(Me.mySession)
                Me.mySession.IsLoadedSession = False
                If rdMain.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
                rdMain.Dispose()
            End If

            mySession.CurrentScreen = Session.CurrentScreenType.MainForm
        End If
    End Sub

    Private Sub cmdCycleTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCycleTeam.Click
        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else

            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.CycleCountTeam
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)
            Dim main As CycleCountTeam = New CycleCountTeam(Me.mySession)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If

    End Sub

    Private Sub cmdCycleLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCycleLocation.Click
        'check to see needed vars are available
        If (String.IsNullOrEmpty(Me.ServiceURI)) Then
            MessageBox.Show(Messages.NULL_SERVICEURI, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.RegionCode)) Then
            MessageBox.Show(Messages.NULL_REGION, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf (String.IsNullOrEmpty(Me.UserName)) Then
            MessageBox.Show(Messages.NULL_USERNAME, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        ElseIf Me.UserAuthenticated = False Then
            MessageBox.Show(Messages.NO_AUTH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            DialogResult = Windows.Forms.DialogResult.Abort
            Exit Sub
        End If

        If (String.IsNullOrEmpty(Me.SubteamComboBox.SelectedItem.ToString()) Or _
                Me.SubteamComboBox.SelectedIndex = 0 Or _
                Me.StoreComboBox.SelectedIndex = 0 Or _
                String.IsNullOrEmpty(Me.StoreComboBox.SelectedItem.ToString())) Then
            MessageBox.Show("Please select a store and subteam to continue.", "IRMA Mobile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Else

            'set session variables
            mySession.ServiceURI = Me.ServiceURI
            mySession.Region = Me.RegionCode
            mySession.UserName = Me.UserName
            mySession.ActionType = Enums.ActionType.CycleCountLocation
            mySession.StartTime = serverDateTime
            mySession.IsLoadedSession = False

            mySession.MyScanner = Nothing

            'set selected store/subteam
            mySession.SetSubteamKey(Me.SubteamComboBox.SelectedValue & "," & Me.SubteamComboBox.Text)
            mySession.SetStore(Me.StoreComboBox.SelectedValue & "," & Me.StoreComboBox.Text)
            Dim main As CycleCountTeam = New CycleCountTeam(Me.mySession)

            If main.ShowDialog() = Windows.Forms.DialogResult.Abort Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If

        End If

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        'check scanner to restore user set settings
        Try
            If Not Me.mySession.MyScanner.Equals(Nothing) Then
                Me.mySession.MyScanner.restoreScannerSettings()
            End If
        Catch ex As Exception
            ' fail quietly
        End Try

        DialogResult = Windows.Forms.DialogResult.OK

    End Sub

#End Region

#Region " Private Methods"

    Private Sub SetPermissions(ByVal UserRoles() As UserRole)

        ' shrink
        _isShrinkUser = (UserRoles(0).IsShrink Or UserRoles(0).IsCoordinator Or UserRoles(0).IsSuperUser)

        ' buyer
        _isBuyerUser = (UserRoles(0).IsBuyer Or UserRoles(0).IsCoordinator Or UserRoles(0).IsSuperUser)

        ' receiver
        _isReceiverUser = (UserRoles(0).IsDistributor Or UserRoles(0).IsSuperUser)

    End Sub

    Private Function SearchShrinkType(ByVal shrinkType As String, ByVal sTypeResult As ListsShrinkAdjustmentReason()) As Boolean

        For Each stype In sTypeResult
            If shrinkType = stype.Abbreviation Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Sub PopulateDataTables()

        Dim dtSubteams As DataTable
        Dim dtStores As DataTable
        Dim dtsTypes As DataTable
        Dim dtsTypesAdj As DataTable
        Dim dr As DataRow

        '----Sub Types----'
        Dim shrinkSubTypeResult As ShrinkSubType() = Me.mySession.WebProxyClient.GetShrinkSubTypes()

        mySession.ShrinkSubTypes = shrinkSubTypeResult

        '----STORES----
        Dim storeResult As ListsStore() = Me.mySession.WebProxyClient.GetStores(False)

        If (storeResult.Length > 0) Then

            dtStores = New DataTable
            dtStores.Columns.Add(New DataColumn("DisplayMember"))
            dtStores.Columns.Add(New DataColumn("ValueMember"))

            For Each store In storeResult

                dr = dtStores.NewRow()
                dr.Item("DisplayMember") = store.StoreName
                dr.Item("ValueMember") = store.StoreNo

                dtStores.Rows.Add(dr)

            Next

            mySession.Stores = dtStores

        End If

        '----SUBTEAMS----
        Dim subteamResult As ListsSubteam() = Me.mySession.WebProxyClient.GetSubteams()
        If (subteamResult.Length > 0) Then

            dtSubteams = New DataTable

            dtSubteams.Columns.Add(New DataColumn("DisplayMember"))
            dtSubteams.Columns.Add(New DataColumn("ValueMember"))
            dtSubteams.Columns.Add(New DataColumn("SubTeamType_ID", System.Type.GetType("System.Int16")))
            dtSubteams.Columns.Add(New DataColumn("IsFixedSpoilage", System.Type.GetType("System.Boolean")))
            dtSubteams.Columns.Add(New DataColumn("IsUnrestricted", System.Type.GetType("System.Boolean")))

            For Each subTeam In subteamResult
                dr = dtSubteams.NewRow()

                dr.Item("DisplayMember") = subTeam.SubteamName
                dr.Item("ValueMember") = subTeam.SubteamNo
                dr.Item("IsFixedSpoilage") = subTeam.SubteamIsFixedSpoilage
                dr.Item("IsUnrestricted") = subTeam.SubteamIsUnrestricted
                dr.Item("SubTeamType_ID") = subTeam.SubteamType

                dtSubteams.Rows.Add(dr)
            Next

            mySession.Subteams = dtSubteams

        End If

        '----SHRINKTYPES----
        Dim sTypeResult As ListsShrinkAdjustmentReason() = Me.mySession.WebProxyClient.GetShrinkAdjustmentReasons()

        If (sTypeResult.Length > 0) Then
            dtsTypes = New DataTable
            dtsTypes.Columns.Add(New DataColumn("DisplayMember"))
            dtsTypes.Columns.Add(New DataColumn("ValueMember"))

            dr = dtsTypes.NewRow()
            dr.Item("DisplayMember") = "Select Type..."
            dr.Item("ValueMember") = "NN"
            dtsTypes.Rows.Add(dr)

            If SearchShrinkType("SP", sTypeResult) = True Then
                dr = dtsTypes.NewRow()
                dr.Item("DisplayMember") = "Spoilage"
                dr.Item("ValueMember") = "SP"
                dtsTypes.Rows.Add(dr)
            End If

            If SearchShrinkType("SM", sTypeResult) = True Then
                dr = dtsTypes.NewRow()
                dr.Item("DisplayMember") = "Samples"
                dr.Item("ValueMember") = "SM"
                dtsTypes.Rows.Add(dr)
            End If

            If SearchShrinkType("FB", sTypeResult) = True Then
                dr = dtsTypes.NewRow()
                dr.Item("DisplayMember") = "Food Bank"
                dr.Item("ValueMember") = "FB"
                dtsTypes.Rows.Add(dr)
            End If

            mySession.ShrinkTypes = dtsTypes

            '----SHRINKADJUSTMENTS (same method call result) ----
            dtsTypesAdj = New DataTable
            dtsTypesAdj.Columns.Add(New DataColumn("DisplayMember"))
            dtsTypesAdj.Columns.Add(New DataColumn("ValueMember"))

            For Each sTypeAdj In sTypeResult
                dr = dtsTypesAdj.NewRow()
                dr.Item("DisplayMember") = sTypeAdj.AdjustmentDescription
                dr.Item("ValueMember") = sTypeAdj.AdjustmentID

                dtsTypesAdj.Rows.Add(dr)
            Next

            mySession.ShrinkAdjustmentIds = dtsTypesAdj

        End If

    End Sub

    Private Sub SetVisibility(Optional ByVal hide As Boolean = False)
        If hide = True Then
            Dim aryHiddenControls() As String = New String() {""}
            Array.Sort(aryHiddenControls)
            Common.ToggleControlVisibility(aryHiddenControls, Me.Controls, False)
        Else
            cmdShrink.Visible = _isShrinkUser
            cmdOrder.Visible = _isBuyerUser
            cmdTransfer.Visible = _isBuyerUser
            cmdCredit.Visible = _isBuyerUser
            cmdReceive.Visible = _isReceiverUser
            cmdCycleLocation.Visible = IIf(IsNothing(Me.UserName), False, True)
            cmdCycleTeam.Visible = IIf(IsNothing(Me.UserName), False, True)
            cmdItemCheck.Visible = IIf(IsNothing(Me.UserName), False, True)
        End If
    End Sub

#End Region

    Private Sub MenuInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInfo.Click

        Try
            Me.ServerName = mySession.WebProxyClient.GetServerName()

            ' The ServerName property is not critical to the application, so it will be left blank if the service call fails.
        Catch ex As Exception
            Me.ServerName = "Unavailable"
        End Try

        notify = New Notification()
        AddHandler notify.ResponseSubmitted, AddressOf Notification_ResponseSubmitted
        AddHandler notify.BalloonChanged, AddressOf Notification_BaloonChanged
        AddHandler notify.Disposed, AddressOf Notification_Disposed

        Dim HTMLString As New StringBuilder()

        HTMLString.Append("<html><body>")
        HTMLString.Append("<b>Configuration Information</b><font size=2></br> ")
        HTMLString.Append("ServiceURI: " & Me.ServiceURI & "<br/>")
        HTMLString.Append("Username: " & Me.UserName & "<br/>")
        HTMLString.Append("Region Code: " & Me.RegionCode & "<br/>")
        HTMLString.Append("Authenticated: " & IIf(Me.UserAuthenticated, "Yes", "No") & "<br/>")
        HTMLString.Append("Build: " & Me.ComponentVersion & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
        HTMLString.Append("Server Name: " & Me.ServerName.ToUpper())

        'color code the permission. oooh fancy.
        HTMLString.Append("<table align=center width=100% colspacing=0 colpadding=0><tr><td colspan=3 align='center'><b>Permissions</b></td></tr><tr><td align='center'>")
        HTMLString.AppendFormat("<font color='{0}'>", IIf(Me._isBuyerUser, "Green", "Red"))
        HTMLString.Append("Buyer</font></td><td  align='center'>")
        HTMLString.AppendFormat("<font color='{0}'>", IIf(Me._isReceiverUser, "Green", "Red"))
        HTMLString.Append("Receiver</font></td><td  align='center'>")
        HTMLString.AppendFormat("<font color='{0}'>", IIf(Me._isShrinkUser, "Green", "Red"))
        HTMLString.Append("Shrink</font></td></tr></table>")
        HTMLString.Append("</font></body></html>")

        notify.Caption = "Plugin Information"
        notify.Critical = False
        notify.InitialDuration = 10
        notify.Text = HTMLString.ToString
        notify.Visible = True

    End Sub

    Public ReadOnly Property ComponentVersion() As String
        Get
            Dim VersionInfo As Version = System.Reflection.Assembly.GetExecutingAssembly.GetName.Version
            Return VersionInfo.Major & "." & VersionInfo.Minor & "." & VersionInfo.Build & "." & VersionInfo.Revision
        End Get
    End Property

    Private Sub Notification_ResponseSubmitted(ByVal obj As Object, ByVal resevent As ResponseSubmittedEventArgs)
        notify.Dispose()
    End Sub

    Private Sub Notification_BaloonChanged(ByVal obj As Object, ByVal baloonArgs As BalloonChangedEventArgs)
        If baloonArgs.Visible = False Then
            notify.Dispose()
        End If
    End Sub

    Private Sub Notification_Disposed(ByVal obj As Object, ByVal args As EventArgs)
        RemoveHandler notify.ResponseSubmitted, AddressOf Notification_ResponseSubmitted
        RemoveHandler notify.BalloonChanged, AddressOf Notification_BaloonChanged
        RemoveHandler notify.BalloonChanged, AddressOf Notification_Disposed
    End Sub

End Class