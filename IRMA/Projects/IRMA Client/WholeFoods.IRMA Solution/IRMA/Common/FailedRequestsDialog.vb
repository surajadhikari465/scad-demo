Friend Class FailedRequestsDialog

    Public Shared Sub HandleError(ByVal inShortMessage As String, ByVal inLongMessage As String, ByVal inError As String)
        Dim theDialog As New FailedRequestsDialog()
        With theDialog
            .ShortMessage = inShortMessage
            .LongMessage = inLongMessage
            .ErrorMessage = inError
        End With

        theDialog.ShowDialog()

        If theDialog IsNot Nothing Then
            theDialog.Dispose()
            theDialog = Nothing
        End If
    End Sub

    Private Property ShortMessage() As String
        Get
            Return Me.TextBoxShortMessage.Text
        End Get
        Set(ByVal value As String)
            Me.TextBoxShortMessage.Text = value
        End Set
    End Property

    Private Property LongMessage() As String
        Get
            Return Me.RichTextBoxFullError.Text
        End Get
        Set(ByVal value As String)
            Me.RichTextBoxFullError.Text = value
        End Set
    End Property

    Private Property ErrorMessage() As String
        Get
            Return Me.TextBoxError.Text
        End Get
        Set(ByVal value As String)
            Me.TextBoxError.Text = value
        End Set
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class
