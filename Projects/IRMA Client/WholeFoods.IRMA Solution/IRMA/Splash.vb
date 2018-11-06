Option Strict Off
Option Explicit On
Friend Class frmSplash
	Inherits System.Windows.Forms.Form

	Private Sub frmSplash_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Me.Close()
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub frmSplash_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'Enables the text to be transparent.
        lblProductName.Parent = imgLogo
        lblVersion.Parent = imgLogo

        lblVersion.Text = "Version " & sVersion

    End Sub
	
    Private Sub imgLogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgLogo.Click
        Me.Close()
    End Sub
End Class