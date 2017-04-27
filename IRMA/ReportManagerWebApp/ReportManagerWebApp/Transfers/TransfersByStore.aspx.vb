
Partial Class Transfers_TransfersByStore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("TransfersByStore")
        sReportURL.Append("&rs:Command=Render")

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class