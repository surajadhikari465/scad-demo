
Partial Class OrderGuides
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
        'sReportURL.Append(Application.Get("region") + "_" + "ProductOrderGuide")

        'Bug 10796: use only one report for all regions
        sReportURL.Append("ProductOrderGuide")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        sReportURL.Append("&StoreNo=" & cmbStore.SelectedValue)
        sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)

        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        If cmbVendor.SelectedValue <> 0 Then
            sReportURL.Append("&VendorID=" & cmbVendor.SelectedValue)
        End If

        sReportURL.Append("&Perishable=" & optPerishable.Checked.ToString)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound

        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item
        cmbTeam.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound

        'add the default item
        cmbVendor.Items.Insert(0, oListItemAll)

    End Sub

End Class
