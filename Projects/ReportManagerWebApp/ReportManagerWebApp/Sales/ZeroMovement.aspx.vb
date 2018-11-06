Imports System.Globalization

Partial Class Sales_ZeroMovement
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder

            Dim sAllStoresList As String = String.Empty
            Dim item As ListItem

            Dim beginDate, endDate As DateTime
            Dim marketCulture, usCulture As CultureInfo
            marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture
            usCulture = New CultureInfo("en-US")
            beginDate = DateTime.ParseExact(dteBegin.Text, "d", marketCulture)
            endDate = DateTime.ParseExact(dteEnd.Text, "d", marketCulture)

            'report name
            sReportURL.Append(Application.Get("region") + "_" + "ZeroMovementReport")

            'report display
            If cmbReportFormat.SelectedValue <> "HTML" Then
                sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
            End If

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            sReportURL.Append("&BeginDate=" & beginDate.ToString("d", usCulture))
            sReportURL.Append("&EndDate=" & endDate.ToString("d", usCulture))

            'report parameters
            sAllStoresList = ""
            If cmbStore.SelectedIndex = 0 Then
                cmbStore.Items.RemoveAt(0)
                For Each item In cmbStore.Items
                    If sAllStoresList = "" Then
                        If item.Value <> "0" Then sAllStoresList = Mid(item.Value, 1, InStr(1, item.Value, "|") - 1)
                    Else
                        If item.Value <> "0" Then sAllStoresList = sAllStoresList & "|" & Mid(item.Value, 1, InStr(1, item.Value, "|") - 1)
                    End If
                Next
                sReportURL.Append("&Store_No=" & sAllStoresList)
            Else
                sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            End If

            If cmbSubteam.SelectedValue = "ALL" Then
                sReportURL.Append("&SubTeam_No:isnull=true")
            Else
                sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
            End If

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            cmbStore.Items.Add("ALL")
            cmbStore.AppendDataBoundItems = True
            cmbStore.DataSourceID = "ICStores"
            cmbStore.DataTextField = "Store_Name"
            cmbStore.DataValueField = "Store_No"

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
