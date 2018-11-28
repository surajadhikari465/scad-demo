Option Strict Off
Option Explicit On
Friend Class frmCostExceptionReport
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Dim iType As Short
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		' GET THE STORE SELECTED
		If cboStore.SelectedIndex = -1 Then
			MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
        End If

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("CostExceptionReport.vb  cmdReport_Click(): The Crystal Report CostException.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
		
        '' SET STORED PROCEDURE PARAMETERS
        'crwReport.set_StoredProcParam(0, CrystalValue(cboStore))
        'crwReport.set_StoredProcParam(1, CrystalValue(cboSubTeam))
        'crwReport.set_StoredProcParam(2, iType)
        'crwReport.set_StoredProcParam(3, "0" & txtCost.Text)
        'crwReport.set_StoredProcParam(4, System.Math.Abs(chkIncludeDiscontinued.CheckState))

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "CostException.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.ReportTitle = "Cost Exception - " & cboStore.Text
        'Select Case iType
        '	Case 1 : crwReport.ReportTitle = crwReport.ReportTitle & vbCrLf & "Cost > Price"
        '	Case 2 : crwReport.ReportTitle = crwReport.ReportTitle & vbCrLf & "Cost > " & txtCost.Text
        'End Select
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)
		
	End Sub
	
	Private Sub frmCostExceptionReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form
		CenterForm(Me)
		
		'-- Load the combo boxes
		LoadInventoryStore(cboStore)
		LoadAllSubTeams(cboSubTeam)
		iType = 1

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
            SetActive(cboStore, True)
        End If
	End Sub
	
    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optType.GetIndex(eventSender)

            iType = Index

        End If
    End Sub

  Private Sub txtCost_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtCost.Enter
    txtCost.SelectAll()
  End Sub

  Private Sub txtCost_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtCost.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtCost.Tag), txtCost, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class