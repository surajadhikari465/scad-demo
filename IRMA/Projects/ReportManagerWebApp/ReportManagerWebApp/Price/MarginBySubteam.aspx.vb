
Partial Class Price_MarginBySubTeam
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_" + "MarginBySubTeam")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
        sReportURL.Append("&Minval=" & txtMinval.Text)
        sReportURL.Append("&Maxval=" & txtMaxval.Text)
        sReportURL.Append("&Range=" & IIf(rdbtn_InRange.Checked, "true", "false"))

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub
End Class
