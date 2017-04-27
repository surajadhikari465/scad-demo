Public Class dlgQuestion

    Private Res As String = "A"
    Public ReadOnly Property Result() As String
        Get
            Return Res
        End Get
    End Property

    Public WriteOnly Property BodyText() As String
        Set(ByVal value As String)
            lblMessage.Text = value
        End Set
    End Property

    Private Sub btnADD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnADD.Click
        Res = "A"
        Me.Close()
    End Sub

    Private Sub btnOverwrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOverwrite.Click
        Res = "O"
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Res = "C"
        Me.Close()
    End Sub

    Private Sub dlgQuestion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.Top = 185
        Me.Left = 1
    End Sub

End Class