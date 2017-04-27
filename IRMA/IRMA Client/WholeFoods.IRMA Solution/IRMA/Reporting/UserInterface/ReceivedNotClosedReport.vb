Option Strict Off
Option Explicit On
Friend Class frmReceivedNotClosedReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = cmbField.GetIndex(eventSender)
		
		If KeyAscii = 8 Then cmbField(Index).SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub
	
    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        If cmbField(0).SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblLabel(1).Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbField(0).Focus()
            Exit Sub
        End If

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        Dim sReportURL As New System.Text.StringBuilder

        '--------------------------
        ' Setup Report URL
        '--------------------------
        'sReportURL.Append(gsRegionCode)
        sReportURL.Append("Received Not Closed")

        'This chooses the region and based on the results points to the correct report.

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        '--------------------------
        ' Add Report Parameters
        '--------------------------

        If Len(Trim(_cmbField_0.Text)) <> 0 Then
            sReportURL.Append("&Store_No=" & VB6.GetItemData(_cmbField_0, _cmbField_0.SelectedIndex))
        Else
            sReportURL.Append("&Store_No:isnull=true")
        End If

        If Len(Trim(_cmbField_1.Text)) <> 0 Then
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(_cmbField_1, _cmbField_1.SelectedIndex))
        Else
            sReportURL.Append("&SubTeam_No:isnull=true")
        End If

        If Len(Trim(dtpStartDate.Text)) <> 0 Then
            sReportURL.Append("&BeginDate=" & dtpStartDate.Value.ToString("yyyy-MM-dd"))
        End If

        If Len(Trim(dtpEndDate.Text)) <> 0 Then
            sReportURL.Append("&EndDate=" & dtpEndDate.Value.ToString("yyyy-MM-dd"))
        End If


        '--------------------------
        ' Display Report
        '--------------------------
        Call ReportingServicesReport(sReportURL.ToString)


    End Sub
	
	Private Sub frmReceivedNotClosedReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		LoadInventoryStore(cmbField(0))
        LoadAllSubTeams(cmbField(1))

        If glStore_Limit > 0 Then
            SetActive(cmbField(0), False)
            SetCombo(cmbField(0), glStore_Limit)
        Else
            cmbField(0).SelectedIndex = -1
        End If

        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

	End Sub
	
End Class