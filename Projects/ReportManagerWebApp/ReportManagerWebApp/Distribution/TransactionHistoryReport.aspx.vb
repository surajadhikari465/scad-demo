Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_TransactionHistory
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        dteStartDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        dteStartDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        dteStartDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing
        oListItemDefault = Nothing

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDCStore.DataBound

        'add the default item
        cmbDCStore.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        'report name
        sReportURL.Append("TransactionHistoryReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&DC_Store_No=" & cmbDCStore.SelectedValue)
        sReportURL.Append("&Identifier=" & txtIdentifier.Text)

        If dteStartDate.Value Is DBNull.Value Then
            sReportURL.Append("&StartDate:isnull=true")
        Else
            sReportURL.Append("&StartDate=" & CDate(dteStartDate.Value).ToString("MM/dd/yyyy"))
        End If

        If dteEndDate.Value Is DBNull.Value Then
            sReportURL.Append("&EndDate:isnull=true")
        Else
            sReportURL.Append("&EndDate=" & CDate(dteEndDate.Value).ToString("MM/dd/yyyy"))
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub
End Class
