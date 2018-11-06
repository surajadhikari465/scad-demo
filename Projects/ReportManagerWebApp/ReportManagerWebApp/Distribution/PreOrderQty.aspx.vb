Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_PreOrderQty
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        'set minimum date
        rngValid_ExpectedDate.MinimumValue = dtMinDate
        dteExpectedDate.MinDate = dtMinDate

        'set maximum date
        rngValid_ExpectedDate.MaximumValue = dtMaxDate
        dteExpectedDate.MaxDate = dtMaxDate

        dteExpectedDate.NullDateLabel = "<Enter Date>"
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
        sReportURL.Append(Application.Get("region") + "_" + "PreOrderedQuantity")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&DC_Store_No=" & cmbDCStore.SelectedValue)
        sReportURL.Append("&ExpectedDate=" & GetUniversalDateString(dteExpectedDate.Value))
        sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        sReportURL.Append("&Pre_Order=" & optPreOrderTrue.Checked)

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub
End Class
