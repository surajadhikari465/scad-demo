Option Strict Off
Option Explicit On
Friend Class frmWasteReport
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
		
		Dim dtBegin, dtEnd As Date
        Dim sTitle As String
        sTitle = String.Empty

		If Len(Trim(txtField(0).Text)) <> 10 Then
			MsgBox("Begin date must be completed.", MsgBoxStyle.Exclamation, "Error!")
			txtField(0).Focus()
			Exit Sub
		End If
		
		If Len(Trim(txtField(1).Text)) <> 10 Then
			MsgBox("End date must be completed.", MsgBoxStyle.Exclamation, "Error!")
			txtField(1).Focus()
			Exit Sub
		End If
		
		dtBegin = CDate(txtField(0).Text)
		dtEnd = CDate(txtField(1).Text)
		
		If dtEnd < dtBegin Then
			MsgBox("End date is older than begin date.", MsgBoxStyle.Exclamation, "Invalid Range!")
			txtField(1).Focus()
			Exit Sub
        End If

        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String = "Waste"

        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If Me.cmbStore.SelectedIndex <> -1 Then
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        Else
            MsgBox("You must select a Store.", MsgBoxStyle.Exclamation)
            cmbStore.Focus()
            Exit Sub
        End If

        If Me.cmbSubTeam.SelectedIndex <> -1 Then
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        Else
            MsgBox("You must select a Sub-Team.", MsgBoxStyle.Exclamation)
            cmbStore.Focus()
            Exit Sub
        End If

        sReportURL.Append("&BeginDate=" & Format(dtBegin, "M/dd/yyyy"))
        sReportURL.Append("&EndDate=" & Format(dtEnd, "M/dd/yyyy"))

        Call ReportingServicesReport(sReportURL.ToString)


	End Sub
	
	Private Sub frmWasteReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		LoadInventoryStore(cmbField(0))
		LoadAllSubTeams(cmbField(1))
		txtField(0).Text = VB6.Format(System.Date.FromOADate(SystemDateTime.ToOADate - 1), "MM/DD/YYYY")
		txtField(1).Text = VB6.Format(System.Date.FromOADate(SystemDateTime.ToOADate - 1), "MM/DD/YYYY")
		
	End Sub
	
	Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
		Dim Index As Short = txtField.GetIndex(eventSender)
		
		HighlightText(txtField(Index))
		
	End Sub
	
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = txtField.GetIndex(eventSender)
		
		'-- Restrict key presses to that type of field
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "Date", txtField(Index), 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class