Partial Class EInvoicing_ClosedPendingEinvoices
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append("ClosedPendingEinvoices")

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")





        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub
End Class