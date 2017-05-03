
Partial Class InventoryValueReport
  Inherits System.Web.UI.Page
  Private oListItemAll As New System.Web.UI.WebControls.ListItem("ALL", CType(0, String))

  Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound
    cmbTeam.Items.Insert(0, oListItemAll)
  End Sub

  Protected Sub cmbTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.SelectedIndexChanged

  End Sub

  Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder


        'report name
        sReportURL.Append(Application.Get("region") + "_" + "InventoryValueDetail")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 0 Or cmbStore.Text = "ALL" Then
            sReportURL.Append("&BusUnit:isnull=true")
        Else
            sReportURL.Append("&BusUnit=" & cmbStore.SelectedValue)
        End If

        If cmbTeam.SelectedValue = 0 Then
            sReportURL.Append("&TeamNo:isnull=true")
        Else
            sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        End If

        If cmbSubteam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeamNo:isnull=true")
        Else
            sReportURL.Append("&SubTeamNo=" & cmbSubteam.SelectedValue)
        End If

        If txtIdentifier.Text.Trim.Length = 0 Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & txtIdentifier.Text)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
  End Sub

  Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.DataBound
    cmbSubteam.Items.Insert(0, oListItemAll)
  End Sub

  Protected Sub cmbSubteam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.SelectedIndexChanged

  End Sub

  Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    oListItemAll = Nothing
  End Sub

End Class
