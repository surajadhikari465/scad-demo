Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_DCCasesShipped
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        'set minimum date
        rngValid_BeginDate.MinimumValue = dtMinDate
        rngValid_EndDate.MinimumValue = dtMinDate

        dteBeginDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        'set maximum date
        rngValid_BeginDate.MaximumValue = dtMaxDate
        rngValid_EndDate.MaximumValue = dtMaxDate

        dteBeginDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        dteBeginDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing
        oListItemDefault = Nothing

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDCVendor.DataBound

        'add the default item
        cmbDCVendor.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder
        Dim reportserver As String
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        reportserver = Application.Get("reportingServicesURL")

        'report name
        sReportURL.Append(Application.Get("region") + "_" + "DCCasesShipped")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&DC_Vendor_Id=" & cmbDCVendor.SelectedValue)

        sReportURL.Append("&StartDate=" & CDate(GetUniversalDateString(dteBeginDate.Value)).ToShortDateString)
        sReportURL.Append("&EndDate=" & CDate(GetUniversalDateString(dteEndDate.Value)).ToShortDateString)

        If optFromSubTeam.Checked Then
            sReportURL.Append("&FromSubTeam=1")
        Else
            sReportURL.Append("&FromSubTeam=0")
        End If

        Response.Redirect(reportserver + sReportURL.ToString())
    End Sub

End Class
