
Partial Class CostAudit_1
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing
        oListItemDefault = Nothing

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        '20100223-BS-Consolidated Regional versions of the xx_CostAuditReport(s) to CostAuditReport
        'sReportURL.Append(Application.Get("region") + "_" + "CostAuditReport")
        sReportURL.Append("CostAuditReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        sReportURL.Append("&Vendor_ID=" & cmbVendor.SelectedValue)
        sReportURL.Append("&IsRegional=True")
        sReportURL.Append("&Zone_ID:isnull=true")
        sReportURL.Append("&Store_No_List:isnull=true")
        sReportURL.Append("&Team_No=" & cmbTeam.SelectedValue)
        sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        sReportURL.Append("&Category_ID:isnull=true")
        sReportURL.Append("&Brand_ID:isnull=true")
        sReportURL.Append("&Identifier:isnull=true")

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item
        cmbTeam.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound

        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)

    End Sub

End Class
