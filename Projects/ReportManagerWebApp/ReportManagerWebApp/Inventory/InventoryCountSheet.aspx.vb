
Partial Class Inventory_InventoryCountSheet
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("All", CType(1, String))

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemDefault = Nothing
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        cmbStore.Items.Insert(0, oListItemDefault)
        'cmbStore.Items.Insert(1, oListItemAll)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim reportserver As String
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        reportserver = Application.Get("reportingServicesURL")

        'report name
        sReportURL.Append("InventoryCountSheet")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 1 Then
            sReportURL.Append("&StoreNo=All")
        Else
            sReportURL.Append("&StoreNo=" & cmbStore.SelectedValue)
        End If

        sReportURL.Append("&Subteam_No=" & cmbSubTeam.SelectedValue)
        sReportURL.Append("&OrderHistoryDays=" & txtOrderHistoryDays.Text)

        Response.Redirect(reportserver + sReportURL.ToString())
    End Sub
End Class
