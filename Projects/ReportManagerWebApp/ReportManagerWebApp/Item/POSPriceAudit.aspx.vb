
Partial Class Price_POSPriceAudit
    Inherits System.Web.UI.Page
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub cmbStores_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStores.DataBound

        'add the default item
        cmbStores.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_" + "POS_IRMA")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

       
        sReportURL.Append("&Store_No=" & cmbStores.SelectedValue)
        sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        'sReportURL.Append("&Search:isnull=true")

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
