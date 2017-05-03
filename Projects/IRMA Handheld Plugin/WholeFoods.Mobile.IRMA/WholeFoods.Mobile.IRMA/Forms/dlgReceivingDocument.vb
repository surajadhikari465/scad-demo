Public Class dlgReceivingDocument
    'Private Sub ZOMG()
    '    MessageBox.Show("zomg pick one", "weeee", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
    'End Sub

    Private Res As String = "R"
    Public ReadOnly Property results() As String
        Get
            Return Res
        End Get
    End Property

    Private Sub btnR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnR.Click
        Res = "R"
        Cursor.Current = Cursors.WaitCursor
        Me.Close()
    End Sub

    Private Sub btnRD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRD.Click
        Res = "RD"
        Cursor.Current = Cursors.WaitCursor
        Me.Close()
    End Sub

    Private Sub btnC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnC.Click
        Res = "C"
        Me.Close()
    End Sub

    Private Sub dlgQuestion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.Top = 88
        Me.Left = 44
    End Sub

End Class