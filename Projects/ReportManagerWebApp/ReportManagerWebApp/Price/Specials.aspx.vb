Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Specials
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

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound

        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)

    End Sub
    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item
        cmbTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append(Application.Get("region") + "_" + "Specials")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&StoreNo=" & cmbStore.SelectedValue)

        If cmbTeam.SelectedValue > 0 Then
            sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue > 0 Then
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        sReportURL.Append("&StartDate=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&EndDate=" & GetUniversalDateString(dteEndDate.Value))

        sReportURL.Append("&UseEndDate=" & chkEndDate.Checked.ToString)

        If chkIncludeOngoing.Checked = True Then
            sReportURL.Append("&IncludeOngoing=" & Boolean.TrueString)
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
