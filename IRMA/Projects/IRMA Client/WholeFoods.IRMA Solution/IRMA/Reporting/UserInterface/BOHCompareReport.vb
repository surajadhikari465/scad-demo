Imports WholeFoods.Utility

Public Class BOHCompareReport

    Private Sub BOHCompareReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String
        '--------------------------
        ' Setup Report URL
        '--------------------------
        filename = ConfigurationServices.AppSettings("Region")

        ' filename = filename + "_BOHCompareReport"
        ' Region name has been excluded from the report name.
        filename = "BOHCompareReport"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If txtSKU.Text.ToUpper.Trim = "ALL" Or txtSKU.Text.Trim = "" Or txtSKU.Text.Trim.Length = 0 Then
            sReportURL.Append("&SKU:isnull=true")
        Else
            sReportURL.Append("&SKU=" & txtSKU.Text.Trim)
        End If

        Call ReportingServicesReport(sReportURL.ToString)
    End Sub
End Class