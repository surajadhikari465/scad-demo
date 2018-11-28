Option Strict Off
Option Explicit On
Friend Class frmBigText
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Public psValidationTag As String
	Public pbCancel As Boolean
	Public pbChanged As Boolean
	
	
	Private Sub CancelButton_Renamed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelButton_Renamed.Click
		pbCancel = True
		Me.Hide()
	End Sub
	
	Private Sub frmBigText_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Me.txtBigText.Tag = Me.psValidationTag
	End Sub
	
	Private Sub frmBigText_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			If Me.pbChanged Then
				pbCancel = False
			Else
				pbCancel = True
			End If
			Me.Hide()
		End If
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub OKButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OKButton.Click
		Me.Hide()
	End Sub
	
	Private Sub txtBigText_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBigText.TextChanged
        If Me.IsInitializing = True Then Exit Sub
        If Me.Visible Then pbChanged = True
	End Sub


  Private Sub txtBigText_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBigText.Enter
    txtBigText.SelectAll()
  End Sub


  Private Sub txtBigText_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBigText.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    '-- psValidationTag must be set by the calling form
    KeyAscii = ValidateKeyPressEvent(KeyAscii, (Me.psValidationTag), txtBigText, 0, 0, 0)

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub
End Class