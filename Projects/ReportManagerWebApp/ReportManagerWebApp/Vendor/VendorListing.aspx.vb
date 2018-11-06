
Partial Class Vendor_VendorListing
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"

    End Sub


    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_VendorListing")
        'sReportURL.Append("Top 20 Items for All Sub Team")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")




        If cmbVendors.SelectedIndex > 0 Then


            For i As Integer = 0 To cmbVendors.Items.Count - 1

                If cmbVendors.Items(i).Selected Then

                    sReportURL.Append("&Vendor_ID=" & cmbVendors.Items(i).Value)
                End If
            Next
        Else

            sReportURL.Append("&Vendor_ID=")
            If cmbVendors.SelectedValue.Contains(" All") Then
                sReportURL.Append(cmbVendors.SelectedValue)

                Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
                Exit Sub
            End If
        End If




        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
