Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports WholeFoods.Utility

Friend Class frmTelxon
	Inherits System.Windows.Forms.Form
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		If m_vb6FormDefInstance Is Nothing Then
			If m_InitializingDefInstance Then
				m_vb6FormDefInstance = Me
			Else
				Try 
					'For the start-up form, the first instance created is the default instance.
					If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
						m_vb6FormDefInstance = Me
					End If
				Catch
				End Try
			End If
		End If
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents wsClient As Winsock_Control.Winsock
    Friend WithEvents wsSymbol As Winsock_Control.Winsock
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents Timer1 As System.Windows.Forms.Timer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTelxon))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.wsClient = New Winsock_Control.Winsock
        Me.wsSymbol = New Winsock_Control.Winsock
        Me.lblStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 60000
        '
        'wsClient
        '
        Me.wsClient.LocalPort = 6667
        Me.wsClient.RemoteIP = "127.0.0.1"
        Me.wsClient.RemotePort = 80
        '
        'wsSymbol
        '
        Me.wsSymbol.LocalPort = 6665
        Me.wsSymbol.RemoteIP = "127.0.0.1"
        Me.wsSymbol.RemotePort = 80
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Info
        Me.lblStatus.Location = New System.Drawing.Point(12, 9)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(281, 21)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Tag = ""
        Me.lblStatus.Text = "Listening..."
        '
        'frmTelxon
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(308, 43)
        Me.Controls.Add(Me.lblStatus)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.Name = "frmTelxon"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Wireless Server"
        Me.ResumeLayout(False)

    End Sub
#End Region
#Region "Upgrade Support "
    Private Shared m_vb6FormDefInstance As frmTelxon
    Private Shared m_InitializingDefInstance As Boolean
    Public Shared Property DefInstance() As frmTelxon
        Get
            If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
                m_InitializingDefInstance = True
                m_vb6FormDefInstance = New frmTelxon()
                m_InitializingDefInstance = False
            End If
            DefInstance = m_vb6FormDefInstance
        End Get
        Set(ByVal value As frmTelxon)
            m_vb6FormDefInstance = value
        End Set
    End Property
#End Region
    Dim iPasswordRetryCnt As Short
    Dim sPreviousUserName As String

    Dim cmdLineArg As String = String.Empty

    Private Sub SendClientData(ByRef sSend As String)

        Try
            wsClient.Send(sSend)
        Catch ex As Exception
            LogError(ex.ToString)
            EndApp()
        End Try

    End Sub

    Private Sub frmTelxon_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim sCommand As String
        Dim sEnvironment As String

        On Error GoTo me_err

        sEnvironment = ConfigurationServices.AppSettings("environment")
        gsSupportEmail = ConfigurationServices.AppSettings("emailErrorsTo")

        '-- Set up the arrays
        InitVars()

        'Listen for PSC
        ' 7.21.10 - RDS - Tidal can start this exe and will pass in the port number
        If Environment.GetCommandLineArgs.Length > 1 Then
            cmdLineArg = String.Format("{0} {1} {2} {3}", Environment.GetCommandLineArgs(1), """" & Environment.GetCommandLineArgs(2) & """", Environment.GetCommandLineArgs(3), Environment.GetCommandLineArgs(4))
            wsClient.LocalPort = CInt(Environment.GetCommandLineArgs(3))
            wsSymbol.LocalPort = CInt(Environment.GetCommandLineArgs(4))
        Else
            wsClient.LocalPort = 6667
            wsSymbol.LocalPort = 6665
        End If

        wsClient.Listen()
        wsSymbol.Listen()

        Me.WindowState = System.Windows.Forms.FormWindowState.Normal

        Me.Text = sEnvironment & " " & Me.Text & " " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart & "." & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart & IIf(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileBuildPart = 0, "", "." & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileBuildPart)

        controller = New ScanGunController.Controller

        Timer1.Enabled = True

        Exit Sub

me_err:
        LogError("Error in " & Me.Name & ".Form_Load: " & Err.Description)
        EndApp()

    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick

        On Error GoTo me_err

        If (wsClient.GetState <> Winsock_Control.WinsockStates.Listening) And (UserState.Status > 0) Then
            If UserState.LastUse.AddMinutes(30) < Date.Now Then
                EndApp()
            End If
        End If

me_exit:
        Exit Sub

me_err:
        LogError("Timer1_Timer: " & Err.Number & " - " & Err.Description)

    End Sub

    Private Sub SetupScreen(ByVal Screen As Short)

        Try
            Dim sSendString As String = String.Empty

            '-- Set status for future reference
            If UserState.Status <> StatusClosed Then
                If (UserState.Status <> ErrorMsg) Then
                    UserState.PrevStatus = UserState.Status
                End If
                UserState.Status = Screen
            End If

            '-- retrieve screen data and send it to the user
            Select Case Screen
                Case InvalidLogon : sSendString = sClrScr & ShowHeader("ERROR") & CenterString(5, "INVALID LOGON.") & CenterString(6, "CONTACT HELPDESK.") & ShowFooter(FooterType.Enter)
                Case ErrorMsg : sSendString = sClrScr & ShowHeader("ERROR") & CenterString(5, "LAST ACTION FAILED.") & CenterString(6, "MUST RE-DO LAST") & CenterString(7, "ACTION.") & ShowFooter(FooterType.Enter)
                Case UserName : sSendString = sEndScan(UserState.TerminalType) & sClrScr & ShowHeader("LOGIN INFO") & GotoXY(1, 5) & "LOGIN   : " & sCursor(UserState.TerminalType)
                Case Password : sSendString = GotoXY(1, 6) & "PASSWORD: " & sCursor(UserState.TerminalType)
                Case StoreMenu
                    controller.IsWasteSplit = controller.IsSplitWasteCategory()
                    FillStoreSelection()
                    sSendString = sClrScr & ShowHeader("STORE MENU") & ShowSelection() & ShowFooter(FooterType.ESC)
                Case StoreSubteam
                    If controller.GetSubTeam.Number > 0 Then
                        sSendString = sClrScr & ShowHeader("SUB-TEAM") & GotoXY(1, 4) & sClrEOS & CenterString(5, controller.GetSubTeam.Name) & CenterString(7, "TO CHANGE") & CenterString(8, "USE 'Set Sub-Team'") & ShowFooter(FooterType.Enter) & Chr(7)
                    Else
                        SetupScreen(FunctionMenu)
                    End If
                Case SetStoreSubteam
                    FillStoreSubTeamSelection()
                    sSendString = sClrScr & ShowHeader("Set Sub-Team") & ShowSelection() & ShowFooter(FooterType.ESC)

                Case FunctionMenu : FillFunctionSelection()
                    sSendString = sClrScr & ShowHeader("FUNCTION MENU") & ShowSelection() & ShowFooter(FooterType.ESC)

                Case WasteTypeMenu : FillWasteTypeSelection()
                    sSendString = sClrScr & ShowHeader("SHRINK TYPES") & ShowSelection() & ShowFooter(FooterType.ESC)

                    '-- Defaults
                Case PriceNoItem, WasteNoItem, PerpetualNoItem, OrderNoItem, ReceiveNoItem, PrintSignsNowNoItem, InvLocNoItem

                    sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ITEM NOT FOUND") & ShowFooter(FooterType.Enter)

                Case WasteNoCost

                    'sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ITEM HAS NO COST, UNABLE TO SHRINK") & ShowFooter(FooterType.Enter)
                    sSendString = sClrScr & ShowHeader("SHRINK") & CenterString(5, "ITEM HAS NO COST") & CenterString(6, "UNABLE TO SHRINK") & GotoXY(1, 14) & ShowFooter(FooterType.Enter)

                    '-- SubTeam cannot inventory item
                Case InvLocInventorySubTeamConflict, OrderItemInventorySubTeamConflict, PerpetualItemInventorySubTeamConflict, ReceiveInventorySubTeamException, WasteInventorySubTeamConflict
                    sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "SUBTEAM CANNOT") & CenterString(7, "INVENTORY THIS ITEM") & ShowFooter(FooterType.Enter)

                    '-- Price Check Menu
                Case PriceCheckMenu : sSendString = sClrScr & ShowHeader("ITEM CHECK") & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case PriceViewItem : sSendString = sClrScr & ShowHeader("ITEM CHECK") & GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(True) & ShowFooter(FooterType.More) & Chr(7)
                Case PriceItemVendorList
                    FillVendorSelection()
                    sSendString = sClrScr & ShowHeader("ITEM VENDORS") & ShowSelection() & ShowFooter(FooterType.ESC)

                    '-- Waste Display
                Case WasteSubTeam
                    If controller.GetSubTeam.Number > 0 Then

                        If controller.IsFixedSpoilage(controller.GetSubTeam.Number) Then
                            SetupScreen(WasteSubTeamFixedSpoilage)
                        Else
                            If (controller.IsWasteSplit = True) Then
                                SetupScreen(WasteTypeMenu)
                            Else
                                SetupScreen(WasteMenu)
                            End If
                        End If

                    Else
                        FillStoreSubTeamSelection()
                        sSendString = sClrScr & ShowHeader("SHRINK") & ShowSelection() & ShowFooter(FooterType.ESC)
                    End If

                Case WasteSubTeamFixedSpoilage : sSendString = sClrScr & ShowHeader("SHRINK") & _
                    CenterString(5, controller.GetSubTeam.Name) & _
                    CenterString(6, "IS FIXED SPOILAGE") & _
                    CenterString(7, "UNABLE TO SHRINK") & _
                    GotoXY(1, 14) & ShowFooter(FooterType.ESC)

                Case WasteMenu : sSendString = sClrScr & ShowHeader("SHRINK") & _
                    CenterString(5, "SCAN ITEM OR") & _
                    CenterString(6, "MANUALLY ENTER CODE") & _
                    GotoXY(1, 14) & ShowFooter(FooterType.ESC) & _
                    GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)

                Case WasteSubTeamVerify : sSendString = GotoXY(1, 11) & _
                    sClrEOS & CenterString(11, "THIS IS A " + ScannedSubTeamName) & _
                    GotoXY(1, 12) & CenterString(12, " ITEM AND YOU ARE") & _
                    GotoXY(1, 13) & CenterString(13, "SPOILING IT TO " + CurrentSubTeamName) & _
                    GotoXY(1, 14) & CenterString(14, "ARE YOU SURE(Y/N)?") & sCursor(UserState.TerminalType) & _
                        ShowFooter(FooterType.ESC) & GotoXY(21, 14)

                Case WasteEnterQuantity : sSendString = sSendString & GotoXY(1, 4) & sClrEOS & _
                        GotoXY(1, 5) & GetItemInfo(False) & _
                        GotoXY(1, 10) & sClrEOL & "SHRINK TYPE: " & controller.WasteType.ToString & _
                        GotoXY(1, 12) & IIf(UserState.Status = Screen, "QUANTITY: ", "WEIGHT: ") & _
                        sCursor(UserState.TerminalType) & ShowFooter(FooterType.ESC) & GotoXY(10, 12)

                Case WasteEnterUnit : sSendString = GotoXY(1, 13) & _
                        "QUANTITY: " & sCursor(UserState.TerminalType)
                    'IIf(UserState.Status = Screen, "(U)NIT OR (C)ASE: ", "QUANTITY: ") & sCursor(UserState.TerminalType)

                Case WasteVerify : sSendString = sClrEOL & CenterString(15, "SPOIL(Y/N)?") & sCursor(UserState.TerminalType)

                Case WasteUnitsOutofRange
                        sSendString = sClrEOL & CenterString(15, "Must be < " & (controller.MaximumQuantity + 1) & "(U)") & sCursor(UserState.TerminalType)

                Case AverageUnitWeightMissing
                    sSendString = sClrEOL & CenterString(15, "Average unit Weight is missing in the database for the item") & sCursor(UserState.TerminalType)

                    '-- Perpetual Display
                Case PerpetualSubTeam, PerpetualSubTeamLoc
                        If controller.GetSubTeam.Number > 0 Then
                            If Screen = PerpetualSubTeam Then
                                SetupScreen(PerpetualMenu)
                            Else
                                SetupScreen(PerpetualInvLoc)
                            End If
                        Else
                            FillStoreSubTeamSelection()
                            sSendString = sClrScr & ShowHeader("CYCLE COUNT") & ShowSelection() & ShowFooter(FooterType.ESC)
                        End If

                Case PerpetualInvLoc
                        FillInventoryLocationSelection()
                        sSendString = sClrScr & ShowHeader("INVENTORY LOCATION") & ShowSelection() & ShowFooter(FooterType.ESC)

                Case PerpetualMenu
                        If UserState.InvLocID > 0 Then
                            sSendString = sClrScr & ShowHeader("CYCLE COUNT") & CenterString(5, UserState.InvLocName) & CenterString(6, "SCAN ITEM OR") & CenterString(7, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                        Else
                            sSendString = sClrScr & ShowHeader("CYCLE COUNT") & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                        End If

                Case PerpetualEnterQuantity : sSendString = GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(False) & GetItemCycleCountInfo(UserState.InvLocID)
                        Select Case UserState.Status
                            Case PerpetualEnterQuantity : sSendString = sSendString & ShowFooter(FooterType.ESC) & GotoXY(1, 13) & "QUANTITY: " & sCursor(UserState.TerminalType)
                            Case PerpetualEnterWeight : sSendString = sSendString & ShowFooter(FooterType.ESC) & GotoXY(1, 13) & "WEIGHT: " & sCursor(UserState.TerminalType)
                        End Select
                Case PerpetualEnterUnit : sSendString = GotoXY(1, 14) & IIf(UserState.Status = Screen, "(U)NIT OR (C)ASE: ", "QUANTITY: ") & sCursor(UserState.TerminalType)
                Case PerpetualVerify : sSendString = CenterString(15, "COUNT(Y/N)?") & sCursor(UserState.TerminalType)
                Case PerpetualNoMaster
                        sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, controller.GetSubTeam.Name) & CenterString(6, "NO OPEN MASTER") & CenterString(7, "CYCLE COUNT") & ShowFooter(FooterType.Enter)
                Case PerpetualBeforeStartScan
                        sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, "BEFORE") & CenterString(6, "START SCAN") & CenterString(7, UserState.StartScan.ToString("g")) & ShowFooter(FooterType.Enter)
                Case PerpetualAfterEndScan
                        sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, "AFTER") & CenterString(6, "END SCAN") & CenterString(7, UserState.EndScan.ToString("g")) & ShowFooter(FooterType.Enter)
                Case PerpetualUnitsOutofRange
                        sSendString = CenterString(15, "Must be < " & (controller.MaximumQuantity + 1) & "(U)") & sCursor(UserState.TerminalType)

                        '-- Order Display
                Case OrderTransferToSubTeamMenu, TransferTransferToSubTeamMenu, CreditTransferToSubTeamMenu
                        UserState.IsTransferOrder = (UserState.Status = TransferTransferToSubTeamMenu)
                        UserState.IsCreditOrder = (UserState.Status = CreditTransferToSubTeamMenu)

                        If controller.GetSubTeam.Number > 0 Then
                            SetupScreen(OrderItemMenu)
                        Else
                            FillStoreSubTeamSelection()
                            sSendString = sClrScr & ShowHeader("SUBTEAM") & ShowSelection() & ShowFooter(FooterType.ESC)
                        End If
                Case OrderItemMenu
                        If UserState.NextString.Length > 0 Then
                            UserState.CurrentString = UserState.NextString
                            UserState.NextString = String.Empty
                            ProcessOrderItemSelection()
                        Else
                            sSendString = sClrScr & ShowHeader(IIf(UserState.IsTransferOrder, "TRANSFER", IIf(UserState.IsCreditOrder, "CREDIT", "ORDER"))) & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                        End If
                Case OrderEnterQuantity
                        sSendString = sClrScr & ShowHeader(IIf(UserState.IsTransferOrder, "TRANSFER", IIf(UserState.IsCreditOrder, "CREDIT", "ORDER"))) & GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(False) & GetItemOrderInfo()
                    sSendString = sSendString & ShowFooter(FooterType.ESC) & GotoXY(1, 15) & GetVendorUnitQuantity() & sCursor(UserState.TerminalType) & " :" '& sBeginScan(UserState.TerminalType)

                Case OrderEnterUnit
                        sSendString = GotoXY(1, 13) & "(U)NIT, (B)OX, " & GotoXY(1, 14) & "(P)OUND, (C)ASE " & sCursor(UserState.TerminalType)

                Case OrderItemNotSold
                        sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ITEM NOT SOLD") & ShowFooter(FooterType.Enter)

                        '-- Receiving
                Case ReceiveMenu
                        sSendString = sClrScr & ShowHeader("RECEIVE") & CenterString(5, "ENTER PO #") & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType)
                Case ReceiveNoOrder : sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ORDER NOT FOUND") & ShowFooter(FooterType.Enter)
                Case ReceiveClosedOrder : sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ORDER IS CLOSED") & ShowFooter(FooterType.Enter)
                Case ReceiveWrongStore : sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ORDER NOT FOR") & CenterString(7, "THIS STORE") & ShowFooter(FooterType.Enter)
                Case ReceiveNotSent : sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ORDER NOT SENT") & ShowFooter(FooterType.Enter)
                Case ReceiveEnterItem : sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, controller.GetPONumber) & CenterString(6, "SCAN ITEM OR") & CenterString(7, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case ReceiveEnterQuantity
                        sSendString = sClrScr & ShowHeader("RECEIVE") & GotoXY(1, 5) & GetItemInfo(False)
                        Dim oi As ScanGunController.Controller.OrderItem = controller.GetOrderItem()
                        sSendString = sSendString & ShowFooter(FooterType.ESC) & GotoXY(1, 12) & "ORDERED " & oi.QuantityOrdered.ToString("N") & GotoXY(1, 13) & "RECEIVED " & IIf(oi.QuantityReceived > 0, "(" & oi.QuantityReceived.ToString("N") & "): ", ":") & sCursor(UserState.TerminalType)
                Case ReceiveEnterWeight
                        Dim oi As ScanGunController.Controller.OrderItem = controller.GetOrderItem()
                        sSendString = sSendString & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & "WEIGHT " & IIf(oi.Total_Weight > 0, "(" & oi.Total_Weight.ToString("N") & "): ", ":") & sCursor(UserState.TerminalType)

                        '-- Print Signs (Queue)
                Case PrintSignsMenu
                        sSendString = sClrScr & ShowHeader("PRINT SIGN") & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case PrintSignsViewItem
                        sSendString = GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(True) & GotoXY(1, 14) & sClrEOL & CenterString(14, "PRINT SIGN(Y/N)?") & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case PrintSignsNoItem
                    sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ITEM NOT FOUND") & ShowFooter(FooterType.Enter) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)

                        '-- Print Signs (Now) - using mobile printer
                Case PrintSignsNowMenu
                        'Display a list of mobile printers for the user's store
                        FillMobilePrinterSelection()
                        sSendString = sClrScr & ShowHeader("PRINTER MENU") & ShowSelection() & ShowFooter(FooterType.ESC)
                Case PrintSignsNowScanItem
                        sSendString = sClrScr & ShowHeader("PRINT SIGN NOW") & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case PrintSignsNowViewItem
                        sSendString = sClrScr & ShowHeader("PRINT SIGN NOW") & GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(False) & GotoXY(1, 14) & "COPIES "
                Case PrintSignsNowSignType
                        FillSignTypeSelection()
                        sSendString = sClrScr & ShowHeader("SIGN TYPE MENU") & ShowSelection() & ShowFooter(FooterType.ESC)

                        '-- Inventory Location
                Case InvLocSubTeam
                        If controller.GetSubTeam.Number > 0 Then
                            SetupScreen(InvLocLocation)
                        Else
                            FillStoreSubTeamSelection()
                            sSendString = sClrScr & ShowHeader("INVENTORY LOCATION") & ShowSelection() & ShowFooter(FooterType.ESC)
                        End If
                Case InvLocLocation
                        FillInventoryLocationSelection()
                        sSendString = sClrScr & ShowHeader("INVENTORY LOCATION") & ShowSelection() & ShowFooter(FooterType.ESC)
                Case InvLocScanItem
                        sSendString = sClrScr & ShowHeader("INVENTORY LOCATION") & CenterString(5, "SCAN ITEM OR") & CenterString(6, "MANUALLY ENTER CODE") & CenterString(7, controller.GetInventoryLocation.Name) & GotoXY(1, 14) & ShowFooter(FooterType.ESC) & GotoXY(1, 14) & sCursor(UserState.TerminalType) & sBeginScan(UserState.TerminalType)
                Case InvLocItemExists
                        sSendString = GotoXY(1, 4) & sClrEOS & CenterString(5, UserState.CurrentString) & CenterString(6, "ITEM EXISTS") & CenterString(7, controller.GetInventoryLocation.Name) & ShowFooter(FooterType.Enter)
                Case InvLocViewItem
                        sSendString = GotoXY(1, 4) & sClrEOS & GotoXY(1, 5) & GetItemInfo(False) & CenterString(13, "ITEM ADDED") & CenterString(14, controller.GetInventoryLocation.Name) & ShowFooter(FooterType.Enter)

                        '-- Default to Function menu or close based on status - for Restore User function
                Case Else
                        If (UserState.Status <> StatusClosed) And (UserState.Status > Password) And (UserState.Status <> AlreadyLoggedIn) Then
                            SetupScreen(FunctionMenu)
                        Else
                            EndApp()
                        End If
            End Select

            '-- Set Previous status again if it changed
            If UserState.Status <> StatusClosed Then
                If UserState.Status <> Screen Then
                    UserState.PrevStatus = Screen
                End If
            End If

            '-- Push the screen to the user
            If (UserState.Status <> StatusClosed) And (sSendString.Length > 0) Then
                wsClient.Send(sSendString)
            End If

        Catch ex As Exception
            LogError(ex.ToString)
            SetupScreen(ErrorMsg)
        End Try

    End Sub
    Private Sub ProcessCycleCount(ByVal ItemUnitCode As String)
        Try
            Select Case LCase(ItemUnitCode)
                Case "w"
                    controller.AddToCycleCount(0, CDec(UserState.ProcessString), UserState.InvLocID, False)

                Case "u"
                    controller.AddToCycleCount(CDec(UserState.ProcessString), 0, UserState.InvLocID, False)
                Case "c"
                    controller.AddToCycleCount(CDec(UserState.ProcessString) * controller.GetStoreItem.Package_Desc1, 0, UserState.InvLocID, True)
                Case Else
                    Throw New System.ApplicationException("Unknown ItemUnitCode = " & ItemUnitCode)
            End Select
            SetupScreen(PerpetualMenu)
        Catch ex As ScanGunController.Exception.CycleCount.NoMasterException
            SetupScreen(PerpetualNoMaster)
        Catch ex As ScanGunController.Exception.CycleCount.BeforeStartScanException
            UserState.StartScan = ex.StartScan
            SetupScreen(PerpetualBeforeStartScan)
        Catch ex As ScanGunController.Exception.CycleCount.ExceedsEndScanException
            UserState.EndScan = ex.EndScan
            SetupScreen(PerpetualAfterEndScan)
        Catch ex As ScanGunController.Exception.UnitsOutOfRangeException
            SetupScreen(PerpetualUnitsOutofRange)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ProcessOrderItemSelection()
        Try
            FillItemSelection(UserState.CurrentString)
            Select Case UBound(UserState.SelectionList)
                Case 0
                    SetupScreen(OrderNoItem)
                Case 1
                    controller.ValidateOrderable()
                    SetupScreen(OrderEnterQuantity)
            End Select
        Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
            SetupScreen(OrderItemInventorySubTeamConflict)
        Catch ex As ScanGunController.Exception.OrderItemQueue.NotSoldException
            SetupScreen(OrderItemNotSold)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ProcessCycleCountItemSelection()
        Try
            FillItemSelection(UserState.CurrentString)
            Select Case UBound(UserState.SelectionList)
                Case 0
                    SetupScreen(PerpetualNoItem)
                Case 1
                    SetupScreen(PerpetualEnterQuantity)
                Case Else
                    SetupScreen(PerpetualNoItem)
            End Select
        Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
            SetupScreen(PerpetualItemInventorySubTeamConflict)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ProcessInvLocItemSelection()
        Try
            FillItemSelection(UserState.CurrentString)
            Select Case UBound(UserState.SelectionList)
                Case 0
                    SetupScreen(InvLocNoItem)
                Case 1
                    controller.AddToInventoryLocation()
                    SetupScreen(InvLocViewItem)
                Case Else
                    SetupScreen(InvLocNoItem)
            End Select
        Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
            SetupScreen(InvLocInventorySubTeamConflict)
        Catch ex As ScanGunController.Exception.InventoryLocationItemExistsException
            SetupScreen(InvLocItemExists)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ProcessItemReceiving()
        Try
            controller.ReceiveOrderItem(UserState.Quantity, UserState.Weight)
            UserState.Quantity = 0
            UserState.Weight = 0
            SetupScreen(ReceiveEnterItem)
        Catch ex As ScanGunController.Exception.OrderItem.ReceivedWeightMissingException
            SetupScreen(ReceiveEnterWeight)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ProcessWasteType()
        Dim sValue As Single
        Dim sType As New VB6.FixedLengthString(1)

        '-- Parse the users input
        sType.Value = UserState.CurrentString.Substring(UserState.CurrentString.Length - 1, 1)
        sValue = Val(Mid(UserState.CurrentString, 1, Len(UserState.CurrentString) - 1))

        Select Case sType.Value
            Case "p"
                controller.WasteType = ScanGunController.Controller.WasteCategory.Spoilage
            Case "f"
                controller.WasteType = ScanGunController.Controller.WasteCategory.Foodbank
            Case "s"
                controller.WasteType = ScanGunController.Controller.WasteCategory.Sampling
        End Select

    End Sub

    Private Sub ProcessWaste()
        Dim sValue As Single
        Dim sType As New VB6.FixedLengthString(1)
        Dim dPkg1 As Decimal
        Dim dQty As Decimal
        Dim dWt As Decimal

        Dim dAverageUnitWeight As Decimal

        '-- Parse the users input
        sType.Value = UserState.CurrentString.Substring(UserState.CurrentString.Length - 1, 1)
        sValue = Val(Mid(UserState.CurrentString, 1, Len(UserState.CurrentString) - 1))

        dPkg1 = controller.GetStoreItem.PackSize


        Select Case sType.Value
            Case "w"
                dQty = 0
            Case "c"
                dQty = (IIf(dPkg1 = 0, 1, dPkg1) * sValue)
            Case "u"
                dQty = CDec(sValue)
        End Select

        Select Case sType.Value
            Case "w"
                dWt = CDec(sValue)
            Case "c", "u"
                dWt = 0
        End Select

        Try
            If ScanGunController.Controller.IsRetailNotCostedByWeight(controller.GetStoreItem.Item_Key) Then
                dAverageUnitWeight = ScanGunController.Controller.GetAverageUnitWeight(controller.GetStoreItem.Identifier)
                If dAverageUnitWeight > 0.0 Then
                    dWt = dQty * dAverageUnitWeight
                    dQty = 0
                End If
            End If

            controller.AddWaste(dQty, dWt, controller.WasteType)
            SetupScreen(WasteMenu)
        Catch ex As ScanGunController.Exception.UnitsOutOfRangeException
            SetupScreen(WasteUnitsOutofRange)

            'Catch ex As ScanGunController.Exception.UnitsOutOfRangeException
            '    SetupScreen(AverageUnitWeightMissing)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub wsClient_Connected(ByVal sender As Winsock_Control.Winsock) Handles wsClient.Connected

        'Spawn another instance to listen for new connections

        Dim p As Process = New Process

        Try

            p.StartInfo.UseShellExecute = True
            p.StartInfo.FileName = VB6.GetPath & "\" & VB6.GetEXEName() & ".exe"
            p.StartInfo.Arguments = cmdLineArg
            p.Start()
            Application.DoEvents()

        Catch ex As Exception

            LogError(ex.ToString)

        End Try

    End Sub

    Private Sub wsClient_ConnectionRequest(ByVal sender As Winsock_Control.Winsock, ByRef requestID As System.Net.Sockets.Socket) Handles wsClient.ConnectionRequest
        Try
            wsSymbol.Close()
            UserState.TerminalType = lPSC
            HandleConnectionRequest(sender, requestID)
        Catch ex As Exception
            LogError(ex.ToString)
            EndApp()
        End Try
    End Sub

    Private Sub wsClient_DataArrival(ByVal sender As Winsock_Control.Winsock, ByVal BytesTotal As Integer) Handles wsClient.DataArrival

        Dim objData() As Byte
        Dim ba As Byte()
        Dim i As Int32
        Dim sString, sSendString As String
        Dim lMenu As Int32
        Dim bProcessStringNow As Boolean
        Dim bPrintTag As Boolean
        Dim sPrintString As String
        Dim x As Integer

        Try
            UserState.LastUse = Now

            bProcessStringNow = False
            sString = ""
            sSendString = ""
            sPrintString = ""

            wsClient.GetData(objData)
            ba = CType(objData, Byte())
            For i = 0 To ba.GetUpperBound(0)
                sString &= Chr(ba(i))
                sPrintString &= sString
                bProcessStringNow = ProcessString(sString, UserState.CurrentString, sSendString, tStatus(UserState.Status).PromptType)
            Next

            If bProcessStringNow Then
                If UserState.Status <> StatusClosed Then SendClientData(sSendString)

                '-- Clear any extra scans made
                While Len(UserState.CurrentString) > 7 And InStr(1, UserState.CurrentString, Chr(13)) > 0 And tStatus(UserState.Status).PromptType <> IdentifierString
                    If Len(UserState.CurrentString) = InStr(1, UserState.CurrentString, Chr(13)) Then
                        UserState.CurrentString = ""
                    Else
                        UserState.CurrentString = Mid(UserState.CurrentString, InStr(1, UserState.CurrentString, Chr(13)) + 1)
                    End If
                End While

                Select Case UserState.Status
                    Case InvalidLogon
                        EndApp()

                    Case ErrorMsg

                        EndApp() 'End, but always display the error message first to make sure the user knows that whatever they did failed and must be re-done when they get back in

                    Case UserName
                        If UserState.CurrentString = Chr(27) Then
                            EndApp()
                        Else
                            UserState.UserName = UserState.CurrentString

                            If UserState.UserName <> sPreviousUserName Then
                                sPreviousUserName = UserState.UserName
                                iPasswordRetryCnt = 0
                            End If

                            SetupScreen(Password)
                        End If
                    Case Password
                        If UserState.CurrentString = Chr(27) Then
                            EndApp()
                        Else
                            If LogonUser(UserState.CurrentString) Then
                                SetupScreen(StoreMenu)
                            Else
                                iPasswordRetryCnt = iPasswordRetryCnt + 1
                                If iPasswordRetryCnt < 6 Then
                                    UserState.UserName = ""
                                    SetupScreen(UserName)
                                Else
                                    UserState.Status = InvalidLogon
                                    SetupScreen(UserState.Status)
                                End If
                            End If
                        End If
                    Case StoreMenu
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                EndApp()
                            Case "p" : UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n" : UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStore(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                SetupScreen(StoreSubteam)
                        End Select

                    Case StoreSubteam
                        SetupScreen(FunctionMenu)

                    Case SetStoreSubteam
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStoreSubTeam(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                SetupScreen(FunctionMenu)
                        End Select

                    Case FunctionMenu
                        Select Case (UserState.CurrentString)
                            Case Chr(27) : SetupScreen(StoreMenu)
                            Case "p" : UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n" : UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                lMenu = UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data

                                InitState()

                                Select Case lMenu
                                    Case PrintSignsNowMenu
                                        If Len(UserState.MobilePrinter_NetworkName) = 0 Then
                                            SetupScreen(lMenu)
                                        Else
                                            SetupScreen(PrintSignsNowScanItem)
                                        End If
                                    Case Else
                                        SetupScreen(lMenu)
                                End Select
                        End Select

                        '-- Price Check Stuff
                    Case PriceCheckMenu
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PriceCheckMenu)
                        Else
                            FillItemSelection(UserState.CurrentString, , False)
                            Select Case UBound(UserState.SelectionList)
                                Case 0 : SetupScreen(PriceNoItem)
                                Case 1 : SetupScreen(PriceViewItem)
                                Case Else : SetupScreen(PriceNoItem)
                            End Select
                        End If
                    Case PriceNoItem : SetupScreen(PriceCheckMenu)
                    Case PriceViewItem
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(PriceCheckMenu)
                        Else
                            SetupScreen(PriceItemVendorList)
                        End If
                    Case PriceItemVendorList
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        Select Case (UserState.CurrentString)
                            Case Chr(27) : SetupScreen(PriceCheckMenu)
                            Case "p" : UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n" : UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                        End Select

                        '-- Waste Stuff
                    Case WasteSubTeam
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStoreSubTeam(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                If (controller.IsWasteSplit = True) Then
                                    SetupScreen(WasteTypeMenu)
                                Else
                                    controller.WasteType = ScanGunController.Controller.WasteCategory.Spoilage
                                    SetupScreen(WasteMenu)
                                End If
                        End Select

                    Case WasteTypeMenu
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "0"
                                controller.SetWasteType(0)
                                SetupScreen(WasteMenu)
                            Case "1"
                                controller.SetWasteType(1)
                                SetupScreen(WasteMenu)
                            Case "2"
                                controller.SetWasteType(2)
                                SetupScreen(WasteMenu)
                        End Select

                    Case WasteSubTeamFixedSpoilage
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                        End Select

                    Case WasteMenu, WasteNoCost
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(WasteMenu)
                        Else
                            Try
                                UserState.IsSameSubTeamScannedForWaste = True
                                FillItemSelection(UserState.CurrentString)

                                Select Case UBound(UserState.SelectionList)
                                    Case 0 : SetupScreen(WasteNoItem)
                                    Case 1
                                        If (UserState.IsSameSubTeamScannedForWaste = True) Then
                                            SetupScreen(WasteEnterQuantity)
                                        Else
                                            SetupScreen(WasteSubTeamVerify)
                                        End If
                                    Case 2 : SetupScreen(WasteNoCost)
                                    Case Else : SetupScreen(WasteNoItem)
                                End Select
                            Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
                                SetupScreen(WasteInventorySubTeamConflict)
                            Catch ex As System.Exception
                                Throw ex
                            End Try
                        End If

                    Case WasteNoItem, WasteInventorySubTeamConflict : SetupScreen(WasteMenu)
                    Case WasteEnterQuantity
                        Select Case VB.Right(UserState.CurrentString, 1)
                            Case Chr(27), "" : SetupScreen(WasteMenu)
                            Case "c", "u" : UserState.ProcessString = UserState.CurrentString
                                SetupScreen(WasteVerify)
                            Case Else : UserState.ProcessString = UserState.CurrentString
                                'SetupScreen(WasteEnterUnit)
                                UserState.ProcessString = UserState.ProcessString & "u"
                                SetupScreen(WasteVerify)
                        End Select
                    Case WasteEnterWeight
                        If UserState.CurrentString = Chr(27) Or Val(UserState.CurrentString) = 0 Then
                            SetupScreen(WasteMenu)
                        Else
                            UserState.ProcessString = UserState.CurrentString & "w"
                            SetupScreen(WasteVerify)
                        End If
                    Case WasteEnterUnit
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(WasteMenu)
                        Else
                            UserState.ProcessString = UserState.ProcessString & UserState.CurrentString
                            SetupScreen(WasteVerify)
                        End If
                    Case WasteVerify
                        If UserState.CurrentString = "y" Then
                            UserState.CurrentString = UserState.ProcessString
                            ProcessWaste()
                        Else
                            SetupScreen(WasteMenu)
                        End If
                    Case WasteSubTeamVerify
                        If UserState.CurrentString = "y" Then
                            UserState.CurrentString = UserState.ProcessString
                            SetupScreen(WasteEnterQuantity)
                        Else
                            SetupScreen(WasteMenu)
                        End If
                    Case WasteUnitsOutofRange
                        SetupScreen(WasteEnterQuantity)

                        '-- Inventory Location
                    Case InvLocNoItem, InvLocItemExists, InvLocInventorySubTeamConflict, InvLocViewItem : SetupScreen(InvLocScanItem)
                    Case InvLocSubTeam
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStoreSubTeam(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                SetupScreen(InvLocLocation)
                        End Select
                    Case InvLocLocation
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetInventoryLocation(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                SetupScreen(InvLocScanItem)
                        End Select
                    Case InvLocScanItem
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(InvLocLocation)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(InvLocScanItem)
                        Else
                            ProcessInvLocItemSelection()
                        End If

                        '-- Perpetual Stuff
                    Case PerpetualNoMaster, PerpetualBeforeStartScan, PerpetualAfterEndScan : SetupScreen(FunctionMenu)
                    Case PerpetualSubTeam, PerpetualSubTeamLoc
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStoreSubTeam(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                If UserState.Status = PerpetualSubTeam Then
                                    SetupScreen(PerpetualMenu)
                                Else
                                    SetupScreen(PerpetualInvLoc)
                                End If
                        End Select
                    Case PerpetualInvLoc
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                UserState.InvLocID = UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data
                                UserState.InvLocName = UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Info
                                SetupScreen(PerpetualMenu)
                        End Select
                    Case PerpetualMenu
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PerpetualMenu)
                        Else
                            ProcessCycleCountItemSelection()
                        End If
                    Case PerpetualNoItem, PerpetualItemInventorySubTeamConflict : SetupScreen(PerpetualMenu)
                    Case PerpetualEnterQuantity
                        Select Case VB.Right(UserState.CurrentString, 1)
                            Case Chr(27), ""
                                SetupScreen(PerpetualMenu)
                            Case "c", "u"
                                UserState.ProcessString = UserState.CurrentString.Substring(0, UserState.CurrentString.Length - 1)
                                ProcessCycleCount(VB.Right(UserState.CurrentString, 1))
                            Case Else
                                'If they scanned the next item, then save it off and remove from the quantity string before processing it
                                'NOTE:  This does not work unless this status in SetupScreen turns on scanning and then turns off once scanning is done in DataArrival - see ordering
                                'This scan ahead feature does not seem to be necessary for cycle counting because they do not have to press enter after entring the quantity and unit
                                'If Len(sSendString) > 10 Then
                                '    UserState.NextString = sSendString
                                '    UserState.CurrentString = UserState.CurrentString.Substring(0, UserState.CurrentString.Length - sSendString.Length)
                                'End If
                                Select Case VB.Right(UserState.CurrentString, 1)
                                    Case "c", "u"
                                        UserState.ProcessString = UserState.CurrentString.Substring(0, UserState.CurrentString.Length - 1)
                                        ProcessCycleCount(VB.Right(UserState.CurrentString, 1))
                                    Case Else
                                        UserState.ProcessString = UserState.CurrentString
                                        ProcessCycleCount("u")
                                        SetupScreen(PerpetualMenu)
                                End Select
                        End Select
                    Case PerpetualEnterWeight
                        If UserState.CurrentString = Chr(27) Or Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PerpetualMenu)
                        Else
                            UserState.ProcessString = UserState.CurrentString
                            ProcessCycleCount("w")
                        End If
                    Case PerpetualEnterUnit
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(PerpetualMenu)
                        Else
                            Select Case LCase(UserState.CurrentString)
                                Case "c", "u"
                                    ProcessCycleCount(LCase(UserState.CurrentString))
                                Case Else
                                    SetupScreen(PerpetualEnterUnit)
                            End Select
                        End If
                    Case PerpetualUnitsOutofRange
                        SetupScreen(PerpetualEnterQuantity)

                        '-- Orders, Transfers and Credits
                    Case OrderTransferToSubTeamMenu, TransferTransferToSubTeamMenu, CreditTransferToSubTeamMenu
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p"
                                UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n"
                                UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                controller.SetRetailStoreSubTeam(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data)
                                SetupScreen(OrderItemMenu)
                        End Select

                    Case OrderItemMenu
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(OrderItemMenu)
                        Else
                            ProcessOrderItemSelection()
                        End If

                    Case OrderNoItem, OrderItemInventorySubTeamConflict, OrderItemNotSold : SetupScreen(OrderItemMenu)

                    Case OrderEnterQuantity, OrderEnterWeight

                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))

                        If VB.Right(UserState.CurrentString, 1) = Chr(27) Then
                            SetupScreen(OrderItemMenu)
                        Else
                            'If they scanned the next item, then save it off and remove from the quantity string before processing it
                            If Len(sSendString) >= 10 Then
                                UserState.CurrentString = UserState.CurrentString.Substring(0, UserState.CurrentString.Length - sSendString.Length)
                                UserState.NextString = String.Empty
                                ProcessString(sSendString, UserState.NextString, sString, IdentifierString)
                            End If

                            Select Case VB.Right(UserState.CurrentString, 1)
                                Case ""
                                    'If they scanned the next item, they must enter quantity or esc so we know whether they want the current item or not
                                    If UserState.NextString.Length > 0 Then
                                        UserState.NextString = String.Empty
                                        SetupScreen(OrderEnterQuantity)
                                    Else
                                        UserState.ProcessString = "1"

                                        If UserState.vUnit_id = 0 Then
                                            SetupScreen(OrderEnterUnit)
                                        Else
                                            controller.AddToOrderQueue(UserState.IsTransferOrder, UserState.IsCreditOrder, UserState.ProcessString, UserState.vUnit_id)
                                            UserState.ProcessString = vbNullString
                                            SetupScreen(OrderItemMenu)
                                        End If
                                    End If
                                Case Else
                                    Select Case VB.Right(UserState.CurrentString, 1)
                                        Case "c", "u", "b", "p"
                                            GetUnitID(VB.Right(UserState.CurrentString, 1))
                                            UserState.ProcessString = Mid(UserState.CurrentString, 1, Len(UserState.CurrentString) - 1)
                                        Case Else
                                            UserState.ProcessString = UserState.CurrentString
                                    End Select

                                    If UserState.vUnit_id = 0 Then
                                        SetupScreen(OrderEnterUnit)
                                    Else
                                        controller.AddToOrderQueue(UserState.IsTransferOrder, UserState.IsCreditOrder, UserState.ProcessString, UserState.vUnit_id)
                                        UserState.ProcessString = vbNullString
                                        SetupScreen(OrderItemMenu)
                                    End If
                            End Select
                        End If

                    Case OrderEnterUnit

                        Select Case VB.Right(UserState.CurrentString, 1)
                            Case Chr(27), ""
                                SetupScreen(OrderItemMenu)

                            Case "c", "u", "b", "p"
                                GetUnitID(VB.Right(UserState.CurrentString, 1))

                                controller.AddToOrderQueue(UserState.IsTransferOrder, UserState.IsCreditOrder, UserState.ProcessString, UserState.vUnit_id)
                                UserState.ProcessString = vbNullString
                                SetupScreen(OrderItemMenu)

                            Case Else
                                'Ask them to enter again until they enter a correct value
                                SetupScreen(OrderEnterUnit)

                        End Select

                        '-- Receiving
                    Case ReceiveMenu
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Len(UserState.CurrentString) = 0 Then
                            SetupScreen(ReceiveMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(ReceiveMenu)
                        Else
                            Try
                                controller.SetOrder(UserState.CurrentString)
                                SetupScreen(ReceiveEnterItem)
                            Catch ex As ScanGunController.Exception.Order.ClosedException
                                SetupScreen(ReceiveClosedOrder)
                            Catch ex As ScanGunController.Exception.Order.NotFoundException
                                SetupScreen(ReceiveNoOrder)
                            Catch ex As ScanGunController.Exception.Order.WrongStoreException
                                SetupScreen(ReceiveWrongStore)
                            Catch ex As ScanGunController.Exception.Order.NotSentException
                                SetupScreen(ReceiveNotSent)
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End If
                    Case ReceiveNoOrder, ReceiveClosedOrder, ReceiveWrongStore, ReceiveNotSent : SetupScreen(ReceiveMenu)
                    Case ReceiveEnterItem
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(ReceiveMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(ReceiveEnterItem)
                        Else
                            Try
                                FillItemSelection(UserState.CurrentString)
                                Select Case UBound(UserState.SelectionList)
                                    Case 0
                                        SetupScreen(ReceiveNoItem)
                                    Case 1
                                        SetupScreen(ReceiveEnterQuantity)
                                    Case Else
                                        SetupScreen(ReceiveNoItem)
                                End Select
                            Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
                                SetupScreen(ReceiveInventorySubTeamException)
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End If
                    Case ReceiveNoItem, ReceiveInventorySubTeamException
                        SetupScreen(ReceiveEnterItem)
                    Case ReceiveEnterQuantity
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(ReceiveEnterItem)
                            Case String.Empty
                                SetupScreen(ReceiveEnterQuantity)
                            Case Else
                                UserState.Quantity = UserState.CurrentString
                                If UserState.Quantity = 0 Then
                                    SetupScreen(ReceiveEnterQuantity)
                                Else
                                    ProcessItemReceiving()
                                End If
                        End Select
                    Case ReceiveEnterWeight
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(ReceiveEnterItem)
                            Case String.Empty
                                SetupScreen(ReceiveEnterWeight)
                            Case Else
                                UserState.Weight = UserState.CurrentString
                                If UserState.Weight = 0 Then
                                    SetupScreen(ReceiveEnterWeight)
                                Else
                                    ProcessItemReceiving()
                                End If
                        End Select

                        '-- Print Sign
                    Case PrintSignsMenu

                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PrintSignsMenu)
                        Else
                            FillItemSelection(UserState.CurrentString, , False)
                            Select Case UBound(UserState.SelectionList)
                                Case 0
                                    SetupScreen(PrintSignsNoItem)
                                Case 1
                                    SetupScreen(PrintSignsViewItem)
                                Case Else
                                    SetupScreen(PrintSignsNoItem)
                            End Select
                        End If
                    Case PrintSignsNoItem
                        SetupScreen(PrintSignsMenu)

                        SendClientData(UserState.CurrentString)
                        SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PrintSignsMenu)
                        Else
                            FillItemSelection(UserState.CurrentString, , False)
                            Select Case UBound(UserState.SelectionList)
                                Case 0
                                    SetupScreen(PrintSignsNoItem)
                                Case 1
                                    SetupScreen(PrintSignsViewItem)
                                Case Else
                                    SetupScreen(PrintSignsNoItem)
                            End Select
                        End If
                    Case PrintSignsViewItem

                        For x = 1 To Len(sPrintString)
                            bPrintTag = ProcessString(Mid(sPrintString, x, 1), UserState.CurrentString, sSendString, tStatus(PrintSignsMenu).PromptType)
                        Next

                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf UCase(sPrintString) = "Y" Or UCase(sPrintString) = "Z" Or UCase(sPrintString) = "T" Or sPrintString = "#" Then 'different conditions on HHP unit, makes it easier for the user
                            controller.AddToReprintSignQueue()
                            SetupScreen(PrintSignsMenu)
                        ElseIf UCase(sPrintString) = "N" Then
                            SetupScreen(PrintSignsMenu)
                        Else
                            SendClientData("n")
                            SetupScreen(PrintSignsMenu)
                            SendClientData(UserState.CurrentString)
                            SendClientData(sEndScan(UserState.TerminalType))

                            If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                            If UserState.CurrentString = Chr(27) Then
                                SetupScreen(FunctionMenu)
                            ElseIf Val(UserState.CurrentString) = 0 Then
                                SetupScreen(PrintSignsMenu)
                            Else
                                FillItemSelection(UserState.CurrentString, , False)
                                Select Case UBound(UserState.SelectionList)
                                    Case 0
                                        SetupScreen(PrintSignsNoItem)
                                    Case 1
                                        SetupScreen(PrintSignsViewItem)
                                    Case Else
                                        SetupScreen(PrintSignsNoItem)
                                End Select
                            End If
                        End If

                        '-- Print Sign (Now)
                    Case PrintSignsNowMenu
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(FunctionMenu)
                            Case "p" : UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n" : UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                UserState.MobilePrinter_NetworkName = Trim(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Info)
                                SetupScreen(PrintSignsNowScanItem)
                        End Select
                    Case PrintSignsNowScanItem
                        If UserState.Status <> StatusClosed Then SendClientData(sEndScan(UserState.TerminalType))
                        If UserState.CurrentString = Chr(27) Then
                            SetupScreen(FunctionMenu)
                        ElseIf Val(UserState.CurrentString) = 0 Then
                            SetupScreen(PrintSignsNowScanItem)
                        Else
                            FillItemSelection(UserState.CurrentString, , False)
                            Select Case UBound(UserState.SelectionList)
                                Case 0
                                    SetupScreen(PrintSignsNowNoItem)
                                Case 1
                                    SetupScreen(PrintSignsNowViewItem)
                                Case Else
                                    SetupScreen(PrintSignsNowScanItem)
                            End Select
                        End If
                    Case PrintSignsNowNoItem
                        SetupScreen(PrintSignsNowScanItem)
                    Case PrintSignsNowViewItem
                        Select Case VB.Right(UserState.CurrentString, 1)
                            Case Chr(27)
                                SetupScreen(PrintSignsNowScanItem)
                            Case "" 'Default to 1
                                UserState.SignCopies = 1
                                SetupScreen(PrintSignsNowSignType)
                            Case Else
                                UserState.SignCopies = CShort(UserState.CurrentString)
                                SetupScreen(PrintSignsNowSignType)
                        End Select
                    Case PrintSignsNowSignType
                        Select Case (UserState.CurrentString)
                            Case Chr(27)
                                SetupScreen(PrintSignsNowScanItem)
                            Case "p" : UserState.SelectionStart = UserState.SelectionStart - 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case "n" : UserState.SelectionStart = UserState.SelectionStart + 10
                                If UserState.Status <> StatusClosed Then SendClientData(ShowSelection())
                            Case Else
                                UserState.CurrentString = Trim(CStr(UserState.SelectionList(UserState.SelectionStart + Val(UserState.CurrentString)).Data))
                                controller.PrintSignNow(UserState.CurrentString, UserState.MobilePrinter_NetworkName, UserState.SignCopies)
                                SetupScreen(PrintSignsNowScanItem)
                        End Select
                End Select

                UserState.CurrentString = ""
            Else
                If UserState.Status <> StatusClosed Then SendClientData(sSendString)
            End If

        Catch ex As Exception
            LogError(ex.ToString)
            SetupScreen(ErrorMsg)
        End Try
    End Sub

    Private Sub wsClient_Disconnected(ByVal sender As Winsock_Control.Winsock) Handles wsClient.Disconnected
        EndApp()
    End Sub

    Private Sub wsClient_HandleError(ByVal sender As Winsock_Control.Winsock, ByVal Description As String, ByVal Method As String, ByVal myEx As String) Handles wsClient.HandleError
        LogError("wsClient_HandleError: Description=" & Description & " Method=" & Method & " myEx=" & myEx)
        EndApp()
    End Sub

    Private Sub HandleConnectionRequest(ByVal sender As Winsock_Control.Winsock, ByVal requestID As System.Net.Sockets.Socket)
        Try
            wsClient.Accept(requestID)

            'Wait until connected before proceeding - added because getting "Wrong protocol or connection state for the requested transaction or request" in SetupScreen step after this
            Dim i As Short = 0
            While (wsClient.GetState <> Winsock_Control.WinsockStates.Connected) And (i <= 6)
                i = i + 1
                System.Threading.Thread.Sleep(10000)
                System.Windows.Forms.Application.DoEvents()
            End While
            If wsClient.GetState <> Winsock_Control.WinsockStates.Connected Then
                LogError("HandleConnectionRequest failed to connect in connection check loop")
                EndApp()
            End If

            UserState.LastUse = Now
            UserState.Status = Connected
            UserState.UserName = String.Empty

            '-- Set them to do the login stuff
            SetupScreen(UserName)

        Catch ex As Exception
            LogError(ex.ToString)
            EndApp()
        End Try
    End Sub

    Private Sub wsSymbol_ConnectionRequest(ByVal sender As Winsock_Control.Winsock, ByRef requestID As System.Net.Sockets.Socket) Handles wsSymbol.ConnectionRequest
        Try
            UserState.TerminalType = lSymbol
            HandleConnectionRequest(sender, requestID)
        Catch ex As Exception
            LogError(ex.ToString)
            EndApp()
        End Try
    End Sub

    Private Sub wsClient_SendComplete(ByVal sender As Winsock_Control.Winsock) Handles wsClient.SendComplete
        Try
            Dim sStore As String

            If controller.Store_Name.Length > 0 Then
                sStore = " - " & controller.Store_Name & " - "
            Else
                sStore = " "
            End If

            lblStatus.Text = CStr(UserState.UserName & sStore & UserState.Status & " " & tStatus(UserState.Status).Description).TrimStart

        Catch ex As Exception
            'Swallow - don't care
        End Try
    End Sub
    Private Sub EndApp()

        On Error Resume Next

        If Not (wsClient Is Nothing) Then
            If (wsClient.GetState <> Winsock_Control.WinsockStates.Closed) And (wsClient.GetState <> Winsock_Control.WinsockStates.Closing) Then
                wsClient.Close()
            End If
            wsClient.Dispose()
        End If

        If Not (wsSymbol Is Nothing) Then
            If (wsSymbol.GetState <> Winsock_Control.WinsockStates.Closed) And (wsSymbol.GetState <> Winsock_Control.WinsockStates.Closing) Then
                wsSymbol.Close()
            End If
            wsSymbol.Dispose()
        End If

        End

    End Sub
End Class