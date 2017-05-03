Imports System.Text

Public Class ClosedOrdersMissingInvoiceDataReport


    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()

    End Sub

    Private Sub ClosedOrdersMissingInvoiceDataReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RadioButton_Both.Checked = True
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        sReportURL.Append("Closed Orders Missing Invoice Data")
        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If RadioButton_Both.Checked Then
            sReportURL.Append("&filter=all")
        ElseIf RadioButton_Other.Checked Then
            sReportURL.Append("&filter=other")
        ElseIf RadioButton_None.Checked Then
            sReportURL.Append("&filter=none")
        End If
        Call ReportingServicesReport(sReportURL.ToString)

    End Sub
End Class