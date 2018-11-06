Imports System.Globalization

Partial Class Purchases_StoreOrdersTotalBySKU
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    'Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))

    'Protected Sub Page_Load()

    'End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click


        Dim marketCulture, usCulture As CultureInfo
        marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture

        Dim sReportURL As New System.Text.StringBuilder

        Dim startDate, endDate As DateTime

        usCulture = New CultureInfo("en-US")
        startDate = DateTime.ParseExact(dStartDate.Text, "d", marketCulture)
        endDate = DateTime.ParseExact(dEndDate.Text, "d", marketCulture)


        'report name
        sReportURL.Append(Application.Get("region") + "_" + "StoreOrdersTotBySKU")
        Debug.WriteLine(sReportURL.ToString() + " 1")
        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If cmbVendor.SelectedValue <> 0 Then
            sReportURL.Append("&Warehouse=" & cmbVendor.SelectedValue)
        End If

        Debug.WriteLine(sReportURL.ToString() + " 2")
        If cmbTeam.SelectedValue <> 0 Then
            sReportURL.Append("&Team=" & cmbTeam.SelectedValue)
        Else
            sReportURL.Append("&Team:isnull=true")
        End If
        Debug.WriteLine(sReportURL.ToString() + " 3")
        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeam=" & cmbSubTeam.SelectedValue)
        Else
            sReportURL.Append("&SubTeam:isnull=true")
        End If
        Debug.WriteLine(sReportURL.ToString() + " 4")
        sReportURL.Append("&StartDate=" & startDate.ToString("d", usCulture))
        Debug.WriteLine(sReportURL.ToString() + " 5")
        sReportURL.Append("&EndDate=" & endDate.ToString("d", usCulture))
        Debug.WriteLine(sReportURL.ToString() + " 6")


        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub



    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item
        cmbTeam.Items.Insert(0, oListItemAll)

    End Sub



End Class
