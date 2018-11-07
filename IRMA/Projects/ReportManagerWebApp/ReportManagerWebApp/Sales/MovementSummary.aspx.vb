Imports System.Globalization

Partial Class Sales_MovementSummary
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder


            Dim beginDate, endDate As DateTime
            Dim marketCulture, usCulture As CultureInfo
            usCulture = New CultureInfo("en-US")
            marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture
            beginDate = DateTime.ParseExact(dteBegin.Text, "d", marketCulture)
            endDate = DateTime.ParseExact(dteEnd.Text, "d", marketCulture)



            'report name
            sReportURL.Append(Application.Get("region") + "_" + "TopMoversSummary")

            'report display
            If cmbReportFormat.SelectedValue <> "HTML" Then
                sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
            End If

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            sReportURL.Append("&BeginDate=" & beginDate.ToString("d", usCulture))
            sReportURL.Append("&EndDate=" & endDate.ToString("d", usCulture))
            sReportURL.Append("&Top=" & txtResults.Text)
            sReportURL.Append("&Zone_ID=" & cmbZone.SelectedValue)
            If cmbSubteam.SelectedValue = "ALL" Then
                sReportURL.Append("&SubTeam_No:isnull=true")
            Else
                sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
            End If
            sReportURL.Append("&Category_ID:isnull=true")
            sReportURL.Append("&FamilyCode:isnull=true")
            sReportURL.Append("&Vendor_ID:isnull=true")
            sReportURL.Append("&ReverseOrder=" & IIf(rdbtnBottom.Checked, 1, 0))

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cmbSubteam.Items.Add("ALL")
            cmbSubteam.AppendDataBoundItems = True
            cmbSubteam.DataSourceID = "ICSubteams"
            cmbSubteam.DataTextField = "SubTeam_Name"
            cmbSubteam.DataValueField = "SubTeam_No"

            Dim market As String = Application.Get("market")
            If market.Equals("UK") Then
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB")
            End If
        End If
    End Sub
End Class
