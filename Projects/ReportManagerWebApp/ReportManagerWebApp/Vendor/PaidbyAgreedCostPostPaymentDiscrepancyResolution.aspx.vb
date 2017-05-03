Public Class PaidbyAgreedCostPostPaymentDiscrepancyResolution
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("Paid+by+Agreed+Cost+Post+Payment+Discrepancy+Resolution")

        sReportURL.Append("&rs:Command=Render")

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class