
Partial Class Sales_Top20Report
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"

    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)

    End Sub
    Protected Sub cmbStores_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStores.DataBound

        'add the default item
        cmbStores.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        'sReportURL.Append(Application.Get("region") + " Ad Plan Audit Report")
        sReportURL.Append("Top 20 Items for All Sub Team")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        If cmbSubTeam.SelectedIndex > 0 Then
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)

        End If

        If cmbStores.SelectedIndex > 0 Then
            sReportURL.Append("&Store_No=" & cmbStores.SelectedValue)

        End If


        sReportURL.Append("&StartDate=" & dtStartDate.Text)
        sReportURL.Append("&EndDate=" & dtEndDate.Text)



        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
