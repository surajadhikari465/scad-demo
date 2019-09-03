Option Strict Off
Option Explicit On
Friend Class frmAVCIAudit
	Inherits System.Windows.Forms.Form
	Private Const APP_ID As Short = 4
    Private m_lVendID As Integer
    Private IsInitializing As Boolean
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFind.Click
		giSearchType = iSearchAllVendors
        frmSearch.ShowDialog()
        frmSearch.Close()
        frmSearch.Dispose()
		m_lVendID = glVendorID
		Me.txtVend.Text = gsVendorName
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim rs As DAO.Recordset = Nothing

        If m_lVendID <= 0 Then
            MsgBox("You must select a vendor to report on.", MsgBoxStyle.Exclamation, Me.Text)
            txtVend.Focus()
            Exit Sub
        End If

        If dtpStartDate.Checked AndAlso dtpEndDate.Checked Then
            If dtpEndDate.Value < dtpStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("frmAVCIAudit.vb  cmdReport_Click(): The Crystal Report ExReport.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

		'crwReport.Reset()
		'crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ExReport.rpt"
		'crwReport.Connect = gsCrystal_Connect
		'crwReport.Destination = IIf(chkPrintOnly.Checked = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)

		''@ExRuleID INT,
		''@SubTeam_No INT,
		''@Team_No INT,
		''@Severity INT
		''@vendor_id INT
		''@Status int
		''@BeginDate varchar(20)
		''@EndDate varchar(20)
		'crwReport.set_StoredProcParam(0, IIf(cmbExType.SelectedIndex = 0, "crwnull", VB6.GetItemData(cmbExType, cmbExType.SelectedIndex)))
		'crwReport.set_StoredProcParam(3, "crwnull")
		'crwReport.set_StoredProcParam(4, m_lVendID)
		'Select Case cmbStatus.SelectedIndex
		'    Case 0
		'        crwReport.set_StoredProcParam(5, "crwnull")
		'    Case 1
		'        'pending
		'        crwReport.set_StoredProcParam(5, 0)
		'    Case 2
		'        'applied
		'        crwReport.set_StoredProcParam(5, 1)
		'    Case 3
		'        'Deleted
		'        crwReport.set_StoredProcParam(5, -1)
		'End Select

		'If dtpStartDate.Checked Then
		'    crwReport.set_StoredProcParam(6, dtpStartDate.Text)
		'Else
		'    crwReport.set_StoredProcParam(6, "crwNull")
		'End If

		'If dtpEndDate.Checked Then
		'    crwReport.set_StoredProcParam(7, dtpEndDate.Text)
		'Else
		'    crwReport.set_StoredProcParam(7, "crwNull")
		'End If

		'If Me.optTeam.Checked = True Then
		'    crwReport.set_StoredProcParam(1, -9999)
		'    crwReport.set_StoredProcParam(2, VB6.GetItemData(cmbTeam, cmbTeam.SelectedIndex))
		'    rs = SQLOpenRecordSet("EXEC ExReport " & IIf(crwReport.get_StoredProcParam(0) = "crwnull", "null", crwReport.get_StoredProcParam(0)) & ", " & crwReport.get_StoredProcParam(1) & ", " & crwReport.get_StoredProcParam(2) & ", " & IIf(crwReport.get_StoredProcParam(3) = "crwnull", "null", crwReport.get_StoredProcParam(3)) & ", " & IIf(crwReport.get_StoredProcParam(4) = "crwnull", "null", crwReport.get_StoredProcParam(4)) & ", " & IIf(crwReport.get_StoredProcParam(5) = "crwnull", "null", crwReport.get_StoredProcParam(5)) & ", " & "null, null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		'    If rs.EOF = False Or rs.BOF = False Then
		'        PrintReport(crwReport)
		'    Else
		'        MsgBox("None Found.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
		'    End If
		'    rs.Close()
		'    rs = Nothing
		'Else
		'    crwReport.set_StoredProcParam(1,  cmbSubTeam.SelectedItem.SubTeamNo)
		'    crwReport.set_StoredProcParam(2, -9999)
		'    rs = SQLOpenRecordSet("EXEC ExReport " & IIf(crwReport.get_StoredProcParam(0) = "crwnull", "null", crwReport.get_StoredProcParam(0)) & ", " & IIf(crwReport.get_StoredProcParam(1) = "crwnull", "null", crwReport.get_StoredProcParam(1)) & ", " & IIf(crwReport.get_StoredProcParam(2) = "crwnull", "null", crwReport.get_StoredProcParam(2)) & ", " & IIf(crwReport.get_StoredProcParam(3) = "crwnull", "null", crwReport.get_StoredProcParam(3)) & ", " & IIf(crwReport.get_StoredProcParam(4) = "crwnull", "null", crwReport.get_StoredProcParam(4)) & ", " & IIf(crwReport.get_StoredProcParam(5) = "crwnull", "null", crwReport.get_StoredProcParam(5)) & ", " & "null, null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		'    If rs.EOF = False Or rs.BOF = False Then
		'        PrintReport(crwReport)
		'    Else
		'        MsgBox("None Found.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
		'    End If
		'    rs.Close()
		'    rs = Nothing
		'End If
	End Sub
	
	Private Sub frmAVCIAudit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		glVendorID = 0
		gsVendorName = ""
		
        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

        Call SetActive(txtVend, False)

		Call LoadTeam((Me.cmbTeam))
		Call Me.cmbTeam.Items.Insert(0, "All Teams")
		VB6.SetItemData(Me.cmbTeam, 0, -9999)
		Me.cmbTeam.SelectedIndex = 0

		Dim subTeams As List(Of SubTeamBO) = WholeFoods.IRMA.ItemHosting.DataAccess.SubTeamDAO.GetSubteams()
		subTeams.Insert(0, New SubTeamBO() With {.SubTeamNo = -9999, .SubTeamName = "All Sub Teams"})
		cmbSubTeam.DataSource = subTeams
		Me.cmbSubTeam.SelectedIndex = 0

		Call LoadExType((Me.cmbExType), APP_ID)
		Call Me.cmbExType.Items.Insert(0, "All Exception Types")
		Me.cmbExType.SelectedIndex = 0

		Me.cmbStatus.Items.Clear()
		Call Me.cmbStatus.Items.Insert(0, "All Status Types")
		Call Me.cmbStatus.Items.Insert(1, "Pending")
		Call Me.cmbStatus.Items.Insert(2, "Applied")
		Call Me.cmbStatus.Items.Insert(3, "Deleted")
		Me.cmbStatus.SelectedIndex = 0

		Call CenterForm(Me)
	End Sub

	Private Sub opt_Click(sender As Object, e As EventArgs) Handles optTeam.Click, optSubTeam.Click
		cmbTeam.Visible = optTeam.Checked
		cmbSubTeam.Visible = optSubTeam.Checked
	End Sub
End Class