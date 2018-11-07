Public Class POAdminReasonCodesinSPOT
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("PO+Admin+Reason+Codes+in+SPOT")

        sReportURL.Append("&rs:Command=Render")

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class