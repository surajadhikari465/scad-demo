Public Class MultipleValueInput

    Private Sub RefreshItemsBtn_Click(sender As Object, e As EventArgs) Handles RefreshItemsBtn.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        Me.TextBoxInput.Clear()
        Me.TextBoxInput.Focus()
    End Sub

    Private Sub MultipleValueInput_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Me.TextBoxInput.Focus()
    End Sub
End Class