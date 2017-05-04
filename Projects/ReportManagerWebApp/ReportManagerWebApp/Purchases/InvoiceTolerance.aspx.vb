
Partial Class Purchases_InvoiceToleranceReport
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", "ALL")
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", "")


    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("InvoiceTolerance")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&StartDate=" & dtStartDate.Text)
        sReportURL.Append("&EndDate=" & dtEndDate.Text)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
