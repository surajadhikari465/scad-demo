
Partial Class IRMAvsPOSAudit
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
        sReportURL.Append("IRMAvsPOSAudit")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If cmbAuditCategory.SelectedIndex = 1 Then
            sReportURL.Append("&auditCategory:isnull=true")
        Else
            sReportURL.Append("&auditCategory=" & cmbAuditCategory.SelectedValue)
        End If

        If cmbStore.SelectedIndex = 1 Then
            sReportURL.Append("&store:isnull=true")
        Else
            sReportURL.Append("&store=" & cmbStore.SelectedValue)
        End If

        If cmbSubTeam.SelectedIndex = 1 Then
            sReportURL.Append("&subteam:isnull=true")
        Else
            sReportURL.Append("&subteam=" & cmbSubTeam.SelectedValue)
        End If

        If cmbPriceType.SelectedIndex = 1 Then
            sReportURL.Append("&priceType:isnull=true")
        Else
            sReportURL.Append("&priceType=" & cmbPriceType.SelectedValue)
        End If

        sReportURL.Append("&showFilteredItems=" & CBool(chkShowFilteredItems.Checked))

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default and all item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
        cmbSubTeam.Items.Insert(1, oListItemAll)
    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default and all item
        cmbStore.Items.Insert(0, oListItemDefault)
        cmbStore.Items.Insert(1, oListItemAll)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAuditCategory.DataBound
        'add the default and all item
        cmbAuditCategory.Items.Insert(0, oListItemDefault)
        cmbAuditCategory.Items.Insert(1, oListItemAll)
    End Sub

    Protected Sub cmbPriceType_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPriceType.DataBound
        'add the default and all item
        cmbPriceType.Items.Insert(0, oListItemDefault)
        cmbPriceType.Items.Insert(1, oListItemAll)
    End Sub
End Class
