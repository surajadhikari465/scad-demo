Public Class LongTextForm

    Private Sub ExtraTextForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.TextBoxLongText.Select(Me.TextBoxLongText.Text.Length, 0)
    End Sub
End Class