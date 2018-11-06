Public Class ViewError

    Private Sub MenuItemClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemClose.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal err As ParsedCFFaultException)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TextBox_ErrorInfo.Text = err.StackTrace

    End Sub
End Class