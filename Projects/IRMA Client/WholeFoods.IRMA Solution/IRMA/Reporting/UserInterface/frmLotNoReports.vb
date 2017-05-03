Option Strict Off
Option Explicit On
Friend Class frmLotNoReports
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim sEndDate As String
        Dim sBeginDate As String
        Dim sReportURL As String
        sBeginDate = String.Empty
        sEndDate = String.Empty
        sReportURL = String.Empty

		If cmbVendors.SelectedIndex = -1 Then
			MsgBox("You must select a Vendor to report on.")
			cmbVendors.Focus()
			Exit Sub
		End If
		
        Select Case True
            Case optRptBy(0).Checked
                If Len(Trim(txtLotNumber.Text)) = 0 Then
                    MsgBox("You must enter A Lot No to report on.")
                    txtLotNumber.Focus()
                    Exit Sub
                End If

                sReportURL = "LotNoByLotNo&rs:Command=Render&rc:Parameters=false&Lot_No=" & txtLotNumber.Text & "&Vendor_ID=" & ComboValue(cmbVendors)
            Case optRptBy(1).Checked

                If Len(txtBeginDate.Text) > 0 Then
                    If Len(txtBeginDate.Text) < 10 Then
                        MsgBox("Start Date is invalid", MsgBoxStyle.Critical, Me.Text)
                        txtBeginDate.Focus()
                        Exit Sub
                    Else
                        sBeginDate = txtBeginDate.Text
                    End If


                Else
                    MsgBox("You must have a min Date for the report.")
                End If

                If Len(txtEndDate.Text) > 0 Then
                    If Len(txtEndDate.Text) < 10 Then
                        MsgBox("End Date is invalid", MsgBoxStyle.Critical, Me.Text)
                        txtEndDate.Focus()
                        Exit Sub
                    Else
                        sEndDate = txtEndDate.Text
                    End If
                Else
                    MsgBox("You must have a Max Date for the report.")
                End If

                If CDate(Replace(sEndDate, "'", "")) < CDate(Replace(sBeginDate, "'", "")) Then
                    MsgBox("End date is older than begin date.", MsgBoxStyle.Exclamation, "Invalid Range!")
                    txtBeginDate.Focus()
                    Exit Sub
                End If

                If Len(Me.txtIdentifier.Text) = 0 Then
                    MsgBox("You must enter an Identifier to report on.")
                    txtIdentifier.Focus()
                    Exit Sub
                End If

                sReportURL = "LotNoByIdentifier&rs:Command=Render&rc:Parameters=false&Identifier=" & txtIdentifier.Text & "&Vendor_ID=" & ComboValue(cmbVendors) & "&MinDate=" & sBeginDate & "&MaxDate=" & sEndDate
        End Select
		
		Call ReportingServicesReport(sReportURL)
		
	End Sub
	
	Private Sub frmLotNoReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
        LoadVendors(cmbVendors)
		txtBeginDate.Text = VB6.Format(SystemDateTime, "mm/dd/yyyy")
		txtEndDate.Text = VB6.Format(SystemDateTime, "mm/dd/yyyy")
	End Sub
	
    Private Sub optRptBy_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optRptBy.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optRptBy.GetIndex(eventSender)
            If Index = 0 Then
                frmLotNo.Visible = True
                frmIdentifier.Visible = False
            Else
                frmLotNo.Visible = False
                frmIdentifier.Visible = True
            End If
        End If
    End Sub
	
    Private Sub txtBeginDate_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBeginDate.Enter
        txtBeginDate.SelectionStart = 0
        txtBeginDate.SelectionLength = Len(txtBeginDate.Text)
    End Sub
	
	Private Sub txtBeginDate_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBeginDate.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        KeyAscii = ValidateKeyPressEvent(KeyAscii, "Date", txtBeginDate, 0, 0, 0)
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub txtEndDate_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtEndDate.Enter
		txtEndDate.SelectionStart = 0
		txtEndDate.SelectionLength = Len(txtEndDate.Text)
	End Sub
	
	Private Sub txtEndDate_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtEndDate.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "Date", txtEndDate, 0, 0, 0)
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

    Private Sub txtIdentifier_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)

        If Chr(KeyAscii) = "0" And Len(Trim(txtIdentifier.Text)) = 0 Then KeyAscii = 0

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtLotNumber_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNumber.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)
     
        If Not txtLotNumber.ReadOnly Then
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtLotNumber.Tag, txtLotNumber, 0, 0, 0)
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtLotNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLotNumber.TextChanged

    End Sub
End Class