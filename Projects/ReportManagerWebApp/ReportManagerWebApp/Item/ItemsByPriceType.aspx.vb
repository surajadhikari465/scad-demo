
Partial Class Item_ItemsByPriceType
    Inherits System.Web.UI.Page


    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append("ItemsByPriceType")
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=True")
        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
