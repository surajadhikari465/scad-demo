Option Strict Off
Option Explicit On

Friend Class frmReportReceiving
    Inherits System.Windows.Forms.Form

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("Daily Receiving")
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If Len(Trim(dtpStartDate.Text)) <> 0 Then
            sReportURL.Append("&DateReceived=" & Trim(dtpStartDate.Value.ToString("yyyy-MM-dd")))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub frmReportReceiving_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
        dtpStartDate.Value = SystemDateTime()
    End Sub

End Class