Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Friend Class frmAPUploadReports
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	
	Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbStore.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	
    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        Dim lReportIndex As Integer
      
        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        filename = ConfigurationServices.AppSettings("Region")
        filename = "APUPClosed"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'Get the report index
        For lReportIndex = optReport.LBound To optReport.UBound
            If optReport(lReportIndex).Checked Then
                sReportURL.Append("&lReportIndex=" & lReportIndex)
                sReportURL.Append("&optSelected=" & optReport(lReportIndex).Text)
                Exit For
            End If
        Next

        If Len(Trim(dtpStartDate.Text)) <> 0 Then
            sReportURL.Append("&sBegin_Date=" & dtpStartDate.Value.ToString("yyyy-MM-dd"))
        End If

        If Len(Trim(dtpEndDate.Text)) <> 0 Then
            sReportURL.Append("&sEnd_Date=" & dtpEndDate.Value.ToString("yyyy-MM-dd"))
        End If

        If cmbStore.SelectedIndex = -1 Or cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&sStore_No:isnull=true")
        Else
            sReportURL.Append("&sStore_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub
	
	Private Sub frmAPUploadReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
        CenterForm(Me)
        dtpStartDate.Text = Date.Today.ToShortDateString()
        dtpEndDate.Text = Date.Today.ToShortDateString()
		
        LoadStore(cmbStore, bInclude_Dist:=True)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
	End Sub

  Private Sub txtDate_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDate.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtDate_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDate.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtDate.GetIndex(eventSender)

    If txtDate(Index).ReadOnly Then GoTo EventExitSub

    '-- Restrict key presses to that type of field
    KeyAscii = ValidateKeyPressEvent(KeyAscii, "Date", txtDate(Index), 0, 0, 0)

EventExitSub:
    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

End Class