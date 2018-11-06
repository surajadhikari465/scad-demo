
Partial Class Inventory_WarehouseOutOfStockReport
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("<All>", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder



        sReportURL.Append("WarehouseOutOfStock")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = "0" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If cmbVendor.SelectedValue = "0" Then
            sReportURL.Append("&VendorID:isnull=true")
        Else
            sReportURL.Append("&VendorID=" & cmbVendor.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        If cmbStore.SelectedValue = "0" Then
            sReportURL.Append("&Store_Name=All")
        Else
            sReportURL.Append("&Store_Name=" & cmbStore.SelectedItem.Text)
        End If

        If cmbVendor.SelectedValue = "0" Then
            sReportURL.Append("&Vendor_Name=All")
        Else
            sReportURL.Append("&Vendor_Name=" & cmbVendor.SelectedItem.Text)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_Name=All")
        Else
            sReportURL.Append("&SubTeam_Name=" & cmbSubTeam.SelectedItem.Text)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

End Class
