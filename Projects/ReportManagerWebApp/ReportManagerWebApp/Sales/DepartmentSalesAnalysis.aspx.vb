Partial Class DepartmentSalesAnalysis
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("<All>", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim Item As ListItem
        Dim sAllStoresList As String



        sReportURL.Append("DepartmentSalesAnalysis")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&SortBy=" & cmbSortBy.SelectedValue)

        'report parameters
        sAllStoresList = ""
        If cmbStore.SelectedIndex = 0 Then
            cmbStore.Items.Remove(oListItemDefault)
            For Each Item In cmbStore.Items
                If sAllStoresList = "" Then
                    sAllStoresList = Item.Value
                Else
                    sAllStoresList = sAllStoresList & "|" & Item.Value
                End If
            Next
            sReportURL.Append("&Location=" & sAllStoresList)
        Else
            sReportURL.Append("&Location=" & cmbStore.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam:isnull=true")
        Else
            sReportURL.Append("&SubTeam=" & cmbSubTeam.SelectedValue)
        End If

        If txtResults.Text = "" Then
            sReportURL.Append("&Results=100")
        Else
            sReportURL.Append("&Results=" & txtResults.Text)
        End If

        If txtIdentifier.Text = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & txtIdentifier.Text)
        End If

        sReportURL.Append("&StartDate=" & dtStartDate.Value)
        sReportURL.Append("&EndDate=" & dtEndDate.Value)

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

End Class