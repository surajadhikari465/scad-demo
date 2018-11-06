Option Strict Off
Option Explicit On
Friend Class frmTGMPrintByRow
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Call frmTGMLast.ReBindGrid()

		glMaxRows = 0
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		'-- See if there is anything to print
		If txtLength.Text = vbNullString Then
			MsgBox("Number of rows to print cannot be blank.", MsgBoxStyle.Exclamation, "Rows to Print")
			txtLength.Focus()
			Exit Sub
		End If
		
        glMaxRows = Val(txtLength.Text)

        If glMaxRows = 0 Then
            MsgBox("Number of rows to print cannot be zero.", MsgBoxStyle.Exclamation, "Rows to Print")
            txtLength.Focus()
            Exit Sub
        End If

        Call frmTGMLast.PopGridTopRows()
        Call frmTGMLast.Print_TGM()

        txtLength.SelectionStart = 0
        txtLength.SelectionLength = Len(txtLength.Text)
    End Sub
	
	Private Sub frmTGMPrintByRow_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
	End Sub
	
	Private Sub txtLength_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLength.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		'-- Restrict key presses to that type of field
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "Integer", txtLength, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class