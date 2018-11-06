Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmVCAI_ExReport
    Inherits System.Windows.Forms.Form
	Private Const APP_ID As Short = 4
    Private IsInitializing As Integer
	Private m_sTeamList As String
    Private m_lVendorID As Integer
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Sub New(ByRef sTeamList As String, ByRef Vendor_ID As Integer)

        logger.Debug("New entry")

        ' This call is required by the Windows Form Designer.
        IsInitializing = True
        InitializeComponent()
        IsInitializing = False

        ' Add any initialization after the InitializeComponent() call.
        m_sTeamList = sTeamList
        m_lVendorID = Vendor_ID

        Call LoadSubTeam((Me.cmbSubTeam), sTeamList)
        Call Me.cmbSubTeam.Items.Insert(0, ResourcesItemHosting.GetString("AllSubTeams"))
        Me.cmbSubTeam.SelectedIndex = 0

        Call LoadExType(Me.cmbExType, APP_ID)
        Call Me.cmbExType.Items.Insert(0, ResourcesItemHosting.GetString("AllExceptionTypes"))
        Me.cmbExType.SelectedIndex = 0

        Me.cmbStatus.Items.Clear()
        Call Me.cmbStatus.Items.Insert(0, ResourcesItemHosting.GetString("AllStatusTypes"))
        Call Me.cmbStatus.Items.Insert(1, ResourcesItemHosting.GetString("Pending"))
        Call Me.cmbStatus.Items.Insert(2, ResourcesItemHosting.GetString("Applied"))
        Call Me.cmbStatus.Items.Insert(3, ResourcesItemHosting.GetString("Deleted"))
        Me.cmbStatus.SelectedIndex = 0


        logger.Debug("New Exit")

    End Sub
    
    Public Property TeamList() As String
        Get
            TeamList = m_sTeamList
        End Get
        Set(ByVal Value As String)
            m_sTeamList = Value
        End Set
    End Property
	Public Property VendorID() As Integer
		Get
			VendorID = m_lVendorID
		End Get
		Set(ByVal Value As Integer)
			m_lVendorID = Value
		End Set
	End Property

	Private Sub cmbExType_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbExType.SelectedIndexChanged
        logger.Debug("cmbExType_SelectedIndexChanged Entry")

        If IsInitializing = True Then Exit Sub
        cmbSeverity.Items.Clear()
        If cmbExType.SelectedIndex <> 0 Then
            Call LoadExSeverity(cmbSeverity, APP_ID, VB6.GetItemData(cmbExType, cmbExType.SelectedIndex))
        End If
        Call cmbSeverity.Items.Insert(0, ResourcesItemHosting.GetString("AllSeverityLevels"))
        cmbSeverity.SelectedIndex = 0

        logger.Debug("cmbExType_SelectedIndexChanged Exit")
    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        Me.Close()

        logger.Debug("cmdExit_Click Exit")
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        MsgBox("frmVCAI_ExReport.vb cmdReport_Click(): Contains Crystal Report Dependencies that need to be removed", MsgBoxStyle.Exclamation)
        Dim bNoneFound As Boolean
        bNoneFound = True

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ExReport.rpt"
        'crwReport.Connect = gsCrystal_Connect
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)

        ''@ExRuleID INT,
        ''@SubTeam_No INT,
        ''@Team_No INT,
        ''@Severity INT
        ''@vendor_id INT
        ''@Status int
        ''@BeginDate varchar(20)
        ''@EndDate varchar(20)
        'crwReport.set_StoredProcParam(0, IIf(cmbExType.SelectedIndex = 0, "crwnull", VB6.GetItemData(cmbExType, cmbExType.SelectedIndex)))
        'crwReport.set_StoredProcParam(3, IIf(cmbSeverity.SelectedIndex = 0, "crwnull", VB6.GetItemData(cmbSeverity, cmbSeverity.SelectedIndex)))
        'crwReport.set_StoredProcParam(4, m_lVendorID)
        'Select Case cmbStatus.SelectedIndex
        '	Case 0
        '		crwReport.set_StoredProcParam(5, "crwnull")
        '	Case 1
        '		'pending
        '		crwReport.set_StoredProcParam(5, 0)
        '	Case 2
        '		'applied
        '		crwReport.set_StoredProcParam(5, 1)
        '	Case 3
        '		'Delted
        '		crwReport.set_StoredProcParam(5, -1)
        'End Select
        'crwReport.set_StoredProcParam(6, "crwNull")
        'crwReport.set_StoredProcParam(7, "crwNull")

        'Dim rs As dao.Recordset
        'If cmbSubTeam.SelectedIndex = 0 Then
        '	crwReport.set_StoredProcParam(1, -9999)
        '	aryTeams = Split(m_sTeamList, "|")
        '	'loop through team list
        '	For iCnt = 0 To UBound(aryTeams)
        '		crwReport.set_StoredProcParam(2, aryTeams(iCnt))
        '              rs = SQLOpenRecordSet("EXEC ExReport " & IIf(crwReport.get_StoredProcParam(0) = "crwnull", "null", crwReport.get_StoredProcParam(0)) & ", " & crwReport.get_StoredProcParam(1) & ", " & crwReport.get_StoredProcParam(2) & ", " & IIf(crwReport.get_StoredProcParam(3) = "crwnull", "null", crwReport.get_StoredProcParam(3)) & ", " & IIf(crwReport.get_StoredProcParam(4) = "crwnull", "null", crwReport.get_StoredProcParam(4)) & ", " & IIf(crwReport.get_StoredProcParam(5) = "crwnull", "null", crwReport.get_StoredProcParam(5)) & ", " & "null, null", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        '              If rs.EOF = False Or rs.BOF = False Then
        '                  bNoneFound = False
        '                  crwReport.WindowLeft = crwReport.WindowLeft + 50
        '                  crwReport.WindowTop = crwReport.WindowTop + 50
        '                  PrintReport(crwReport)
        '              End If
        '		rs.Close()
        '		rs = Nothing
        '	Next iCnt
        'Else
        '	crwReport.set_StoredProcParam(1, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        '	crwReport.set_StoredProcParam(2, -9999)
        '          rs = SQLOpenRecordSet("EXEC ExReport " & IIf(crwReport.get_StoredProcParam(0) = "crwnull", "null", crwReport.get_StoredProcParam(0)) & ", " & crwReport.get_StoredProcParam(1) & ", " & crwReport.get_StoredProcParam(2) & ", " & IIf(crwReport.get_StoredProcParam(3) = "crwnull", "null", crwReport.get_StoredProcParam(3)) & ", " & IIf(crwReport.get_StoredProcParam(4) = "crwnull", "null", crwReport.get_StoredProcParam(4)) & ", " & IIf(crwReport.get_StoredProcParam(5) = "crwnull", "null", crwReport.get_StoredProcParam(5)) & ", " & "null, null", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        '	If rs.EOF = False Or rs.BOF = False Then
        '              bNoneFound = False
        '              PrintReport(crwReport)
        '          End If
        '	rs.Close()
        '	rs = Nothing
        '      End If

        If bNoneFound = True Then
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
        End If
	End Sub
	
	Private Sub frmVCAI_ExReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
	End Sub
	
End Class