
Partial Class Item_RandomWeightScalePLUs
    Inherits System.Web.UI.Page


    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.SelectedIndexChanged

        Call ChangeSelectedOption()
    End Sub


    Protected Sub cmbStores_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStores.DataBound

        'add the default item
        cmbStores.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub ChangeSelectedOption()

        Select Case RadioButton1.SelectedIndex
            Case 0  'Store/Sub Team
                trStores.Visible = True
                trSubTeam.Visible = True
                trSearchFor.Visible = False

            Case 1  'Free Text
                trStores.Visible = False
                trSubTeam.Visible = False
                trSearchFor.Visible = True

        End Select
    End Sub


    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_Random_Weight_Scale_PLUs")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If RadioButton1.SelectedIndex = 0 Then
            sReportURL.Append("&Store=" & cmbStores.SelectedValue)
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
            sReportURL.Append("&Search:isnull=true")
        Else
            sReportURL.Append("&Store:isnull=true")
            sReportURL.Append("&SubTeam_No:isnull=true")
            sReportURL.Append("&Search=" & txtSearchFor.Text)
        End If


        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
