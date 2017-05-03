
Partial Class Item_AuthorizationReport
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))


    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"

    End Sub

    Protected Sub cmbPriceType_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPriceType.DataBound

        'add the default item
        cmbPriceType.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_AuthorizationReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        sReportURL.Append("&PriceChgTypeID=" & cmbPriceType.SelectedValue)


        If cmbSubTeam.SelectedIndex >= 0 Then


            For i As Integer = 0 To cmbSubTeam.Items.Count - 1

                If cmbSubTeam.Items(i).Selected Then
                    
                    sReportURL.Append("&SubTeam_No=" & cmbSubTeam.Items(i).Value)
                End If
            Next
        End If


        If dtStartDate.Text <> "Null" Then
            sReportURL.Append("&Sale_Start_Date=" & dtStartDate.Text)
        Else
            sReportURL.Append("&Sale_Start_Date:isnull=true")
        End If
        If dtEndDate.Text <> "Null" Then
            sReportURL.Append("&Sale_End_Date=" & dtEndDate.Text)
        Else
            sReportURL.Append("&Sale_End_Date:isnull=true")
        End If



        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
