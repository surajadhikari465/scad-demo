
Partial Class Inventory_POExceptionReport
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("<All>", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder



        sReportURL.Append("POExceptionReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = "0" Then
            sReportURL.Append("&BusinessUnit:isnull=true")
        Else
            sReportURL.Append("&BusinessUnit=" & cmbStore.SelectedValue)
        End If

        If cmbVendor.SelectedValue = "0" Then
            sReportURL.Append("&VendorNo:isnull=true")
        Else
            sReportURL.Append("&VendorNo=" & cmbVendor.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&PSSubTeam:isnull=true")
        Else
            sReportURL.Append("&PSSubTeam=" & cmbSubTeam.SelectedValue)
        End If


        sReportURL.Append("&Tolerance=" & txtTolerance.Text)
        sReportURL.Append("&MinAmount=" & txtMinAmount.Text)
        sReportURL.Append("&DateStart=" & dtStartDate.Value)
        sReportURL.Append("&DateEnd=" & dtEndDate.Value)


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
