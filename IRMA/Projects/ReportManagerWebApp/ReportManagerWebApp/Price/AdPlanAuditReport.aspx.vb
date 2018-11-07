
Partial Class Price_Ad_Plan_Audit_Report
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"

    End Sub



    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_AdPlanAuditReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        If cmbStores.SelectedIndex >= 0 Then


            For i As Integer = 0 To cmbStores.Items.Count - 1

                If cmbStores.Items(i).Selected Then
                    sReportURL.Append("&Store=" & cmbStores.Items(i).Value)
                End If
            Next
        End If


        If cmbSubTeam.SelectedIndex >= 0 Then


            For i As Integer = 0 To cmbSubTeam.Items.Count - 1

                If cmbSubTeam.Items(i).Selected Then
                    sReportURL.Append("&SubTeam_No=" & cmbSubTeam.Items(i).Value)
                End If
            Next
        End If



        sReportURL.Append("&Start_Date=" & dtStartDate.Text)
        sReportURL.Append("&End_Date=" & dtEndDate.Text)



        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
