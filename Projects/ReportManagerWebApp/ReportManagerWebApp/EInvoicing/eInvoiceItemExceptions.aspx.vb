Imports System.Diagnostics

Partial Class eInvoiceItemExceptions
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    'Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing
        'oListItemDefault = Nothing

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("eInvoiceItemExceptions")

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If Me.cmbStore.SelectedValue <> 0 Then
            sReportURL.Append("&ReceiveLocation_ID=" & cmbStore.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        If cmbVendor.SelectedValue <> 0 Then
            sReportURL.Append("&Vendor_ID=" & cmbVendor.SelectedValue)
        End If

        If Me.OrderDateFromChooser.Text = "Null" Then
            sReportURL.Append("&OrderDateStart:isnull=true")
        Else
            sReportURL.Append("&OrderDateStart=" & Me.OrderDateFromChooser.Text & "")
        End If
        If Me.OrderDateToChooser.Text = "Null" Then
            sReportURL.Append("&OrderDateEnd:isnull=true")
        Else
            sReportURL.Append("&OrderDateEnd=" & Me.OrderDateToChooser.Value & "")
        End If

        sReportURL.Append("&OrderHeader_Id:isnull=true")

        'Show The Report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemAll)
    End Sub

End Class
