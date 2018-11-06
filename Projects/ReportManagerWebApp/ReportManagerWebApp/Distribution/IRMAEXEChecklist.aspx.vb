Partial Class Distribution_IRMAEXEChecklist
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("FL_IRMAEXEChecklist")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        If cmbDCStore.SelectedValue <> 0 Then
            sReportURL.Append("&DC_Store_No=" & cmbDCStore.SelectedValue)
        End If

        If cmbVendor.SelectedValue <> 0 Then
            sReportURL.Append("&DC_Vendor_Id=" & cmbVendor.SelectedValue)
        End If

        If cmbStore.SelectedValue <> 0 Then
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        sReportURL.Append("&Sort_By=" & cmbSortBy.SelectedValue)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub cmbDCStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDCStore.DataBound
        'add the default item
        cmbDCStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)
    End Sub
End Class
