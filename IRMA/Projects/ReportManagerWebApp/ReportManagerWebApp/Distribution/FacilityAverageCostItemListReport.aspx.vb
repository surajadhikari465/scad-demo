Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_FacilityAvgCost
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        Me.Webdatechooser1.Value = Format(Now(), "nn/dd/yyyy")

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
        sReportURL.Append("Facility+Avg+Cost+Item+List")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store=" & Me.cmbDCStore.SelectedItem.Value.ToString)
        sReportURL.Append("&SubTeam=" & cmbSubTeam.SelectedItem.Value.ToString)

        If Me.cmbCategory.SelectedValue <> 0 Then
            sReportURL.Append("&Category=" & Me.cmbCategory.SelectedItem.Value.ToString)
        Else
            sReportURL.Append("&Category:isnull=true")
        End If

        If Me.Webdatechooser1.Text.ToString <> "Null" AndAlso Me.Webdatechooser1.Text.ToString <> "" Then
            sReportURL.Append("&Date=" & CDate(Now()).ToString("MM/dd/yyyy"))
        Else
            sReportURL.Append("&Date=" & CDate(Me.Webdatechooser1.Value).ToString("MM/dd/yyyy"))
        End If

        'MsgBox(sReportURL.ToString)
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.DataBound

        'add the default item
        cmbCategory.Items.Insert(0, oListItemAll)   'oListItemDefault)

    End Sub

End Class
