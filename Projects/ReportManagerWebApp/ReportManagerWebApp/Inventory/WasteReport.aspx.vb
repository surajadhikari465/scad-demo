
Partial Class Inventory_WasteReport
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", "ALL")
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", "")


    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"
        'dtStartDate.NullDateLabel = "< Enter Date >"
        'dtEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append("AdjustmentSummaryReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")
        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        If cmbSubTeam.SelectedValue = "ALL" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If
        'sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        sReportURL.Append("&BeginDate=" & dtStartDate.Text)
        sReportURL.Append("&EndDate=" & dtEndDate.Text)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
