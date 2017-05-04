
Partial Class Distribution_DCPerformanceByMarkup
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder



        sReportURL.Append("DCPerformance")



        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Vendor_ID=" & cmbFacility.SelectedValue)

        If cmbSubTeam.SelectedValue = "" Then
            sReportURL.Append("&SubTeam:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If




        sReportURL.Append("&StartDate=" & dtStartDate.Value)
        sReportURL.Append("&EndDate=" & dtEndDate.Value)


        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub
End Class
