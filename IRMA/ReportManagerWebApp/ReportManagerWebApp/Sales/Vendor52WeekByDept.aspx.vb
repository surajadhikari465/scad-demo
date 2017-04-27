Imports System.Globalization

Partial Class Sales_Vendor52WeekByDept
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder


            Dim marketCulture, usCulture As CultureInfo
            usCulture = New CultureInfo("en-US")
            marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture



            'report name
            sReportURL.Append(Application.Get("region") + "_" + "Vendor52WeekByDept")

            'report display
            If cmbReportFormat.SelectedValue <> "HTML" Then
                sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
            End If

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim market As String = Application.Get("market")
            If market.Equals("UK") Then
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB")
            End If

        End If
    End Sub

End Class
