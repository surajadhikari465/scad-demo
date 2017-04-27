Option Strict Off
Option Explicit On
Friend Class frmPriceHistory
	Inherits System.Windows.Forms.Form
	
	Private Sub chkInclude_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkInclude.Enter
		Dim Index As Short = chkInclude.GetIndex(eventSender)
		chkInclude(Index).BackColor = System.Drawing.SystemColors.Highlight
	End Sub
	
	
	Private Sub chkInclude_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkInclude.Leave
		Dim Index As Short = chkInclude.GetIndex(eventSender)
		chkInclude(Index).BackColor = System.Drawing.SystemColors.Control
	End Sub
	
	
	Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
        Dim sTitle As String
		
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreater"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If
		
		If (chkInclude(0).CheckState + chkInclude(1).CheckState + chkInclude(2).CheckState = 0) And (chkExcludeNew.CheckState = System.Windows.Forms.CheckState.Checked) Then
            MsgBox(ResourcesPricing.GetString("SelectIncluded"), MsgBoxStyle.Exclamation, Me.Text)
			Exit Sub
		End If
		
		sTitle = ""
		
        If chkInclude(0).CheckState = 1 Then sTitle = sTitle & ResourcesPricing.GetString("Price")
        If chkInclude(1).CheckState = 1 Then sTitle = sTitle & IIf(sTitle <> "", ", ", "") & ResourcesIRMA.GetString("Item")
        If chkInclude(2).CheckState = 1 Then sTitle = sTitle & IIf(sTitle <> "", ", ", "") & ResourcesPricing.GetString("Promo")
		
        sTitle = sTitle & " " & ResourcesPricing.GetString("ChangesReport")
		If cmbSubTeam.SelectedIndex > -1 Then
			sTitle = sTitle & " (" & cmbSubTeam.Text & ")"
		End If
		
        If chkWFMItems.CheckState Then sTitle = "WFM " & sTitle
		
        sTitle = String.Format(sTitle & vbCrLf & "{0} - {1}", dtpStartDate.Text, dtpEndDate.Text)

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("PriceHistory.vb  cmdReport_Click(): The Crystal Report PriceChanges.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
        '      crwReport.Reset()
        '      crwReport.set_StoredProcParam(0, dtpStartDate.Text)
        '      crwReport.set_StoredProcParam(1, dtpEndDate.Text)
        'crwReport.set_StoredProcParam(2, CrystalValue(cmbStore))
        'crwReport.set_StoredProcParam(3, CrystalValue(cmbSubTeam))
        'crwReport.set_StoredProcParam(4, chkWFMItems.CheckState)
        'crwReport.set_StoredProcParam(5, chkInclude(0).CheckState)
        'crwReport.set_StoredProcParam(6, chkInclude(1).CheckState)
        'crwReport.set_StoredProcParam(7, chkInclude(2).CheckState)
        'crwReport.set_StoredProcParam(8, IIf(chkExcludeNew.CheckState = System.Windows.Forms.CheckState.Checked, 0, 1))

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "PriceChanges.rpt"
        'crwReport.Connect = gsCrystal_Connect
        'crwReport.ReportTitle = sTitle
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'PrintReport(crwReport)
		
	End Sub
	
	Private Sub frmPriceHistory_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		'-- Load combo boxes
		LoadStore(cmbStore)
		LoadAllSubTeams(cmbSubTeam)
		
		'-- Put Data in the store list
        If cmbStore.Items.Count > 0 Then
            If glStore_Limit > 0 Then
                SetActive(cmbStore, False)
                SetCombo(cmbStore, glStore_Limit)
            Else
                cmbStore.SelectedIndex = -1
            End If
        End If
		
        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, System.DateTime.Today)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, System.DateTime.Today)

	End Sub
	
 End Class