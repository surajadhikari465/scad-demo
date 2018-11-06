Imports WholeFoods.IRMA.Replenishment.Jobs

Public Class Form_CloseReceivingJobController

    Private _userId As Integer = 0

    Public Property UserId() As Integer
        Get
            Return _userId
        End Get
        Set(ByVal value As Integer)
            _userId = value
        End Set
    End Property

    Private Sub btnCloseReceiving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseReceiving.Click
        Dim closeReceivingJob As New CloseReceivingJob()
        Dim success As Boolean

        lblError.Visible = False

        success = closeReceivingJob.Main(_userId)

        If success Then
            MessageBox.Show("Close receiving successful!")
            Me.Close()
        Else
            lblError.Text = closeReceivingJob.ErrorMessage
            lblError.Visible = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class