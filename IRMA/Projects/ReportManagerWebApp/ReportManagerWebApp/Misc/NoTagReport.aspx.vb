Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class NoTagReport
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

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append("NoTagReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&StoreNo=" & cmbStore.SelectedValue)
        If (String.IsNullOrEmpty(txtBatchName.Text)) Then
            sReportURL.Append("&BatchDescription:isnull=true")
        Else
            sReportURL.Append("&BatchDescription=" & txtBatchName.Text)
        End If



        sReportURL.Append("&BeginStartDate=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&EndStartDate=" & GetUniversalDateString(dteEndDate.Value))


        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
