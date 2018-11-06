
Partial Class Distribution_StoreOrdersReport
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("<All>", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder



        sReportURL.Append("StoreOrders")

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

        If cmbStartDate.SelectedValue = "0" Then
            sReportURL.Append("&StartDate:isnull=true")
        Else
            sReportURL.Append("&StartDate=" & cmbStartDate.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        If rblCompiled.SelectedValue = "True" Then
            sReportURL.Append("&Compiled=True")
        Else
            sReportURL.Append("&Compiled=False")
        End If

        If txtIdentifier.Text = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & txtIdentifier.Text)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

End Class
