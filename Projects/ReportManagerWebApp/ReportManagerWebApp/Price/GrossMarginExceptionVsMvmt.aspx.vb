Imports System.Globalization

Partial Class GrossMarginExceptionVsMvmt
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim beginDate, endDate As DateTime
        Dim marketCulture, usCulture As CultureInfo

        marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        usCulture = New CultureInfo("en-US")
        beginDate = DateTime.ParseExact(dteBegin.Text, "d", marketCulture)
        endDate = DateTime.ParseExact(dteEnd.Text, "d", marketCulture)



        'report name
        sReportURL.Append(Application.Get("region") + "_" + "GrossMarginExceptionReportVsMvmt")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbMinTeam.SelectedValue = "ALL" Then
            sReportURL.Append("&MinTeam_No:isnull=true")
        Else
            sReportURL.Append("&MinTeam_No=" & cmbMinTeam.SelectedValue)
        End If

        If cmbMaxTeam.SelectedValue = "ALL" Then
            sReportURL.Append("&MaxTeam_No:isnull=true")
        Else
            sReportURL.Append("&MaxTeam_No=" & cmbMaxTeam.SelectedValue)
        End If

        If cmbMinSubteam.SelectedValue = "ALL" Then
            sReportURL.Append("&MinSubTeam_No:isnull=true")
        Else
            sReportURL.Append("&MinSubTeam_No=" & cmbMinSubteam.SelectedValue)
        End If
        If cmbMaxSubteam.SelectedValue = "ALL" Then
            sReportURL.Append("&MaxSubTeam_No:isnull=true")
        Else
            sReportURL.Append("&MaxSubTeam_No=" & cmbMaxSubteam.SelectedValue)
        End If

        If cmbVendor.SelectedValue = "ALL" Then
            sReportURL.Append("&Vendor:isnull=true")
        Else
            sReportURL.Append("&Vendor=" & cmbVendor.SelectedValue)
        End If

        If cmbStores.SelectedValue = "ALL" Then
            sReportURL.Append("&StoreNo:isnull=true")
        Else
            sReportURL.Append("&StoreNo=" & cmbStores.SelectedValue)
        End If

        sReportURL.Append("&MinGM=" & txtMinGM.Text)
        sReportURL.Append("&MaxGM=" & txtMaxGM.Text)
        sReportURL.Append("&MinDate=" & beginDate.ToString("d", usCulture))
        sReportURL.Append("&MaxDate=" & endDate.ToString("d", usCulture))

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbMinTeam.Items.Add("ALL")
        cmbMinTeam.AppendDataBoundItems = True
        cmbMinTeam.DataSourceID = "ICTeams"
        cmbMinTeam.DataTextField = "Team_Name"
        cmbMinTeam.DataValueField = "Team_No"

        cmbMaxTeam.Items.Add("ALL")
        cmbMaxTeam.AppendDataBoundItems = True
        cmbMaxTeam.DataSourceID = "ICTeams"
        cmbMaxTeam.DataTextField = "Team_Name"
        cmbMaxTeam.DataValueField = "Team_No"

        cmbMinSubteam.Items.Add("ALL")
        cmbMinSubteam.AppendDataBoundItems = True
        cmbMinSubteam.DataSourceID = "ICSubTeams"
        cmbMinSubteam.DataTextField = "SubTeam_Name"
        cmbMinSubteam.DataValueField = "SubTeam_No"

        cmbMaxSubteam.Items.Add("ALL")
        cmbMaxSubteam.AppendDataBoundItems = True
        cmbMaxSubteam.DataSourceID = "ICSubTeams"
        cmbMaxSubteam.DataTextField = "SubTeam_Name"
        cmbMaxSubteam.DataValueField = "SubTeam_No"

        cmbVendor.Items.Add("ALL")
        cmbVendor.AppendDataBoundItems = True
        cmbVendor.DataSourceID = "ICVendors"
        cmbVendor.DataTextField = "CompanyName"
        cmbVendor.DataValueField = "Vendor_Id"

        cmbStores.Items.Add("ALL")
        cmbStores.AppendDataBoundItems = True
        cmbStores.DataSourceID = "ICStores"
        cmbStores.DataTextField = "Store_Name"
        cmbStores.DataValueField = "Store_No"
    End Sub
End Class
