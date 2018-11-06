Option Explicit On
Option Strict On


Imports WholeFoods.IRMA.Replenishment.Tlog


Imports IRMA.TLog


Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_TestDriver
    Inherits System.Windows.Forms.Form

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Logger.LogDebug("Button1_Click entry", Me.GetType())

        'Try
        '    POSPushLabel.Text = "Executing the POS Push process...."
        '    Dim pushJob As POSPushJob = New POSPushJob
        '    pushJob.Main()
        '    POSPushLabel.Text = "TestPOSPush success"
        'Catch e1 As Exception
        '    Logger.LogError("Exception during TestPOSPush processing: ", Me.GetType(), e1)
        '    POSPushLabel.Text = "Exception during processing: " & e1.StackTrace
        'End Try

        'Logger.LogDebug("Button1_Click exit", Me.GetType())
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim p As Parser = New Parser()
        p.ParseDataFromFile("c:\EJ060129.LOG")
        MsgBox("done")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Dim result As Boolean
        Dim sendOrdersBO As WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO = New WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO
        Dim sendOrdersDAO As WholeFoods.IRMA.Replenishment.SendOrders.DataAccess.SendOrdersDAO = New WholeFoods.IRMA.Replenishment.SendOrders.DataAccess.SendOrdersDAO
        sendOrdersBO.SendAllOrders()
        'result = sendOrdersBO.SendEXEOrders()
        'Dim po As New WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.POHeaderBO
        'po = WholeFoods.IRMA.Replenishment.SendOrders.DataAccess.SendOrdersDAO.GetPOHeader(49)
        'sendOrdersBO.createHTMLFromXSL(po)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim sendOrdersBO As WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO = New WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO
        sendOrdersBO.CheckWFMFaxAndEmailStatus()
    End Sub
End Class