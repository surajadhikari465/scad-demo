Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_WeeklyPurchasesSalesSummaryReport
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
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern
        Dim x As Integer
        Dim txt As TextBox
        Dim sSubTeamNo As String
        Dim sInvChange As String = ""

        'report name
        sReportURL.Append("WeeklyPurchasesSalesSummary")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&DC_Vendor_Id=" & cmbDCVendor.SelectedValue)
        sReportURL.Append("&Start_Date=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&End_Date=" & GetUniversalDateString(dteEndDate.Value))

        For x = 0 To grdInvChange.Rows.Count - 1
            txt = grdInvChange.Rows(x).FindControl("txtChange")
            sSubTeamNo = grdInvChange.Rows(x).Cells(0).Text

            If sInvChange = "" Then
                sInvChange = sSubTeamNo & "^" & txt.Text
            Else
                sInvChange = sInvChange & "|" & sSubTeamNo & "^" & txt.Text
            End If
        Next

        sReportURL.Append("&InvChg=" & sInvChange)

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub grdInvChange_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdInvChange.DataBound
        Dim txt As TextBox
        Dim x As Integer

        For x = 0 To grdInvChange.Rows.Count - 1
            txt = grdInvChange.Rows(x).FindControl("txtChange")
            txt.Attributes.Add("style", "text-align: center")
        Next
    End Sub
End Class
