
Partial Class Inventory_InventoryCasesBySubteam
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder



        sReportURL.Append("InventoryCasesBySubteam")

        

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Warehouse_ID=" & cmbFacility.SelectedValue)

        If cmbSubTeam.SelectedValue = "" Then
            sReportURL.Append("&SubTeam:isnull=true")
        Else
            sReportURL.Append("&SubTeam=" & cmbSubTeam.SelectedValue)
        End If




        sReportURL.Append("&OldCountDate=" & dtStartDate.Value)
        sReportURL.Append("&NewCountDate=" & dtEndDate.Value)


        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub
End Class
