
Partial Class Price_ZeroCostItemsReport
    Inherits System.Web.UI.Page
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("ALL", CType(0, String))
    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("ZeroCostItemsReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 0 Or cmbStore.Text = "ALL" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If cmbSubteam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        cmbStore.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.DataBound
        cmbSubteam.Items.Insert(0, oListItemAll)
    End Sub
End Class
