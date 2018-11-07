Public Class Form_IRMADelete
    ''' <summary>
    ''' Flag to keep track of the user cancel action
    ''' </summary>
    ''' <remarks></remarks>
    Protected _skipDeleteConfirm As Boolean

    ''' <summary>
    ''' The cancel button returns the user to the calling form.
    ''' Set the flag so they are not prompted to confirm.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        _skipDeleteConfirm = True
        Me.Close()
    End Sub
End Class
