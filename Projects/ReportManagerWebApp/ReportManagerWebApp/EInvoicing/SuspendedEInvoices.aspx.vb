
Partial Class EInvoicing_SuspendedEInvoices
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append("EInvSuspendedInvoices")

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append(String.Format("&StartDate={0}", Now.AddDays(-30).ToString("MM/dd/yyyy")))
        sReportURL.Append(String.Format("&EndDate={0}", Now.ToString("MM/dd/yyyy")))



        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
