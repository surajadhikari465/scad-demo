Imports System.Globalization

Partial Class Item_RetailPriceGuides
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder

            ''report name
            sReportURL.Append("RetailPriceGuides")

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            sReportURL.Append("&Store_No:isnull=true")

            If cmbStore.SelectedValue = 0 Then
                sReportURL.Append("&Store_No:isnull=true")
            Else
                sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            End If

            If cmbSubTeam.SelectedValue = 0 Then
                sReportURL.Append("&SubTeam_No:isnull=true")
            Else
                sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
            End If

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If cmbSubTeam.Enabled = False Then
            cmbSubTeam.Enabled = True
        End If
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

End Class
