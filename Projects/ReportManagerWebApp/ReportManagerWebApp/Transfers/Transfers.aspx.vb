
Partial Class Transfers_Transfers
    Inherits System.Web.UI.Page
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("<All>", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        dteOrderDateStart.NullDateLabel = "< Enter Date >"
        dteOrderDateEnd.NullDateLabel = "< Enter Date >"
        dteExpectedDateStart.NullDateLabel = "< Enter Date >"
        dteExpectedDateEnd.NullDateLabel = "< Enter Date >"
    End Sub
    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceivingStore.DataBound
        cmbReceivingStore.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim reportServer As String

        reportServer = Application.Get("reportingServicesURL")

        If rbSummary.Checked Then
            sReportURL.Append(Application.Get("region") + "_" + "TransfersSummary")
        Else
            sReportURL.Append(Application.Get("region") + "_" + "TransferDetail")
        End If

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If dteOrderDateStart.Value Is DBNull.Value Then
            sReportURL.Append("&OrderDateStart:isnull=true")
        Else
            sReportURL.Append("&OrderDateStart=" & CDate(GetUniversalDateString(dteOrderDateStart.Value)).ToShortDateString)
        End If

        If dteOrderDateEnd.Value Is DBNull.Value Then
            sReportURL.Append("&OrderDateEnd:isnull=true")
        Else
            sReportURL.Append("&OrderDateEnd=" & CDate(GetUniversalDateString(dteOrderDateEnd.Value)).ToShortDateString)
        End If

        If dteExpectedDateStart.Value Is DBNull.Value Then
            sReportURL.Append("&ExpectedDateStart:isnull=true")
        Else
            sReportURL.Append("&ExpectedDateStart=" & CDate(GetUniversalDateString(dteExpectedDateStart.Value)).ToShortDateString)
        End If

        If dteExpectedDateEnd.Value Is DBNull.Value Then
            sReportURL.Append("&ExpectedDateEnd:isnull=true")
        Else
            sReportURL.Append("&ExpectedDateEnd=" & CDate(GetUniversalDateString(dteExpectedDateEnd.Value)).ToShortDateString)
        End If

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&SubTeamNo:isnull=true")
        Else
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        If cmbTransferringStore.SelectedValue = "0" Then
            sReportURL.Append("&TransferringStore:isnull=true")
            sReportURL.Append("&TransferringStoreName:isnull=true")
        Else
            sReportURL.Append("&TransferringStore=" & cmbTransferringStore.SelectedValue)
            sReportURL.Append("&TransferringStoreName=" & cmbTransferringStore.SelectedItem.Text)
        End If

        If cmbReceivingStore.SelectedValue = "0" Then
            sReportURL.Append("&ReceivingStore:isnull=true")
            sReportURL.Append("&ReceivingStoreName:isnull=true")
        Else
            sReportURL.Append("&ReceivingStore=" & cmbReceivingStore.SelectedValue)
            sReportURL.Append("&ReceivingStoreName=" & cmbReceivingStore.SelectedItem.Text)
        End If

        Response.Redirect(reportServer + sReportURL.ToString())
    End Sub

    Protected Function GetUniversalDateString(ByRef DateValue As Date) As String
        Dim sDateString As String
        Const DATE_DELIMITER As Char = "-"
        sDateString = Year(DateValue).ToString & DATE_DELIMITER _
                        & Month(DateValue).ToString & DATE_DELIMITER _
                        & Day(DateValue).ToString
        Return sDateString
    End Function
End Class
