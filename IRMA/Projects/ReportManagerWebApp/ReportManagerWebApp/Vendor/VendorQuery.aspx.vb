
Partial Class Vendor_VendorQuery
    Inherits System.Web.UI.Page

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"

    End Sub
    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append(Application.Get("region") + "_VendorQuery")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&SearchOption=" & cmbSeachOption.SelectedValue)

        If cmbSeachOption.SelectedValue = True Then
            sReportURL.Append("&SearchFor:isnull=true")
        Else
            sReportURL.Append("&SearchFor=" & txtSearchFor.Text)
        End If

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbSeachOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSeachOption.SelectedIndexChanged

        If cmbSeachOption.SelectedValue = False Then
            txtSearchFor.Enabled = True
            trSearchFor.Visible = True
        Else
            txtSearchFor.Enabled = False
            trSearchFor.Visible = False
        End If

    End Sub
End Class
