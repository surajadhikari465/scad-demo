Public Class dlgReturnOrReceive

    Private Res As String = "R"
    Public ReadOnly Property Result() As String
        Get
            Return Res
        End Get
    End Property

    Private Sub btnReceive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReceive.Click
        Res = "RDRT"
        Me.Close()
    End Sub

    Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Res = "RD"
        Me.Close()
    End Sub

    Private Sub dlgQuestion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.Top = 185
        Me.Left = 1
    End Sub

End Class