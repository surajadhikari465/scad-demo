
Partial Class Item_ItemsMarkedIgnorePackUpdate
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    'Protected Sub cblVendorItemStatus_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles cbListVendorItemStatus.DataBound
    '    cbListVendorItemStatus.Items.FindByValue("A").Selected = True
    'End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sVendorItemStatuses As String = String.Empty

        For Each item As ListItem In cblVendorItemStatus.Items
            If item.Selected Then
                sVendorItemStatuses += item.Value & ","
            End If
        Next

        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("ItemsMarkedIgnorePackUpdate")
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If ddlSubteam.SelectedValue <> "" Then
            sReportURL.Append("&Subteam_No=" & ddlSubteam.SelectedValue)
        End If

        If ddlVendor.SelectedValue <> "" Then
            sReportURL.Append("&Vendor_ID=" & ddlVendor.SelectedValue)
        End If

        'sReportURL.Append("&Subteam_No=" & ddlSubteam.SelectedValue)
        'sReportURL.Append("&Vendor_ID=" & ddlVendor.SelectedValue)
        sReportURL.AppendFormat("&VendorItemStatuses={0}", sVendorItemStatuses)
        sReportURL.Append("&DefaultIdentifier=" & chkDefaultIdentifier.Checked)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cblVendorItemStatus_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles cblVendorItemStatus.DataBound
        For Each item As ListItem In cblVendorItemStatus.Items
            item.Selected = True
        Next
    End Sub
End Class
