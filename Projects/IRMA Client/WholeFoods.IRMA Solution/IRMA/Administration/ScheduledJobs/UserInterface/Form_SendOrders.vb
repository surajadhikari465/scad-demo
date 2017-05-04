'Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
'Imports WholeFoods.IRMA.Replenishment.SendOrders.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic

Public Class Form_SendOrders

    Private Sub SendOrdersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendOrdersButton.Click
        Dim uploadJob As SendOrdersJob = New SendOrdersJob()
        'Dim logText As ArrayList
        Dim text As String
        ToolStripStatusLabel1.Text = "Processing..."
        ' The job finished executing - update the status and enable the button
        ' Display the job status to the user.
        uploadJob.Main()

        LogTextBox1.Text = ScheduledJobBO.GetJobCompletionStatusForUI(CType(uploadJob, ScheduledJob))


        For Each Text In uploadJob.LogMessages
            LogTextBox1.AppendText(Text)
            LogTextBox1.AppendText(Environment.NewLine)
        Next
        ToolStripStatusLabel1.Text = "Done"
    End Sub

    Private Sub CheckFaxStatusButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFaxStatusButton.Click
        Dim uploadJob As CheckSendOrderStatusJob = New CheckSendOrderStatusJob()
        Dim text As String

        ToolStripStatusLabel1.Text = "Processing..."
        uploadJob.Main()
        LogTextBox1.Text = ScheduledJobBO.GetJobCompletionStatusForUI(CType(uploadJob, ScheduledJob))
        For Each Text In uploadJob.LogMessages
            LogTextBox1.AppendText(Text)
            LogTextBox1.AppendText(Environment.NewLine)
        Next
        ToolStripStatusLabel1.Text = "Done"
    End Sub
End Class