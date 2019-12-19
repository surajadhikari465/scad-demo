Public Class SplashScreenForm
    Private Sub SplashScreenForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label_Version.Text = AppVersion

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
    End Sub


    Public Sub UpdateStatus(ByVal statusMessage As String, ByVal progressTotal As Integer, _
                            ByVal progressCurrent As Integer)
        With ProgressBar_SplashScreen
            .Minimum = 0
            .Maximum = progressTotal
            .Value = progressCurrent
            .Refresh()
        End With

        With Label_Status
            .Text = statusMessage
            .Refresh()
        End With
    End Sub
End Class