Option Strict Off
Option Explicit On
Friend Class frmItemPriceReport
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Private Sub cmbCategory_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbCategory.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        '-- Change avail categories when subteam is changed
        If cmbSubTeam.SelectedIndex = -1 Then
            cmbCategory.Items.Clear()
        Else
            LoadSubTeamCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim sTitle As String

        If cmbStore.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblStore.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If


        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblSubTeam.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbSubTeam.Focus()
            Exit Sub
        End If

        If cmbCategory.SelectedIndex = -1 Then     ' added error checking for category
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblCategory.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbCategory.Focus()
            Exit Sub
        End If

        sTitle = ResourcesItemHosting.GetString("ItemPriceReportTitle") & vbCrLf
        If cmbCategory.SelectedIndex > -1 Then
            sTitle = sTitle & cmbCategory.Text
        Else
            sTitle = sTitle & cmbSubTeam.Text
        End If

        If chkWFMItems.CheckState Then sTitle = ResourcesIRMA.GetString("WFM") & " " & sTitle

        'crwReport.set_StoredProcParam(0, CrystalValue(cmbStore))
        'crwReport.set_StoredProcParam(1, CrystalValue(cmbSubTeam))
        'crwReport.set_StoredProcParam(2, CrystalValue(cmbCategory))
        'crwReport.set_StoredProcParam(3, chkDiscontinued.CheckState)
        'crwReport.set_StoredProcParam(4, chkWFMItems.CheckState)

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ItemPrice.rpt"
        'crwReport.Connect = gsCrystal_Connect
        'crwReport.ReportTitle = sTitle
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'PrintReport(crwReport)

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("ItemPriceReport.vb  cmdReport_Click(): The Crystal ItemPrice.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        'Dim crd As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        'crd.FileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ItemPrice.rpt"
        'crd.SetParameterValue(0, CrystalValue(cmbStore))
        'crd.SetParameterValue(1, CrystalValue(cmbSubTeam))
        'crd.SetParameterValue(2, CrystalValue(cmbCategory))
        'crd.SetParameterValue(3, chkDiscontinued.CheckState)
        'crd.SetParameterValue(4, chkWFMItems.CheckState)
        'crd.SummaryInfo.ReportTitle = sTitle
        'ReportViewer.ConnectReport(crd)
        'If chkPrintOnly.CheckState = 0 Then
        '    Dim rv As New ReportViewer
        '    rv.Report = crd
        '    rv.ShowDialog()
        '    rv.Dispose()
        'Else
        '    crd.PrintToPrinter(0, False, 0, 0)
        'End If

        'crd.Dispose()
		
	End Sub
	
	Private Sub frmItemPriceReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		Call CenterForm(Me)
		'-- Load the combo boxes
		LoadInventoryStore(cmbStore)
        LoadAllSubTeams(cmbSubTeam)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
	End Sub
End Class