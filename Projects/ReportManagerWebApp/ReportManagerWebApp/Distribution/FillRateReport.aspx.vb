
Partial Class Distribution_FillRateReport
    Inherits System.Web.UI.Page
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        'set minimum date
        dteStartDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        'set maximum date
        dteStartDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        dteStartDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"

        EnableDateFields()
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        cmbVendor.Items.Insert(0, oListItemAll)
    End Sub
    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        cmbStore.Items.Insert(0, oListItemAll)
    End Sub

    Protected Function GetUniversalDateString(ByRef DateValue As Date) As String
        Dim sDateString As String
        Const DATE_DELIMITER As Char = "-"
        sDateString = Year(DateValue).ToString & DATE_DELIMITER _
                        & Month(DateValue).ToString & DATE_DELIMITER _
                        & Day(DateValue).ToString
        Return sDateString
    End Function

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim dStartDate As Date
        Dim dEndDate As Date
        Dim iReportFlag As Integer
        Dim bDetail As Boolean
        Dim sReportName As String



        bDetail = rbDetail.Checked
        sReportName = "FillRateReportSummary"
        Select Case True
            Case rbLast7Days.Checked
                dStartDate = DateAdd(DateInterval.Day, -7, Today.Date)
                dEndDate = DateAdd(DateInterval.Day, 6, dStartDate)

                If bDetail Then
                    sReportName = "FillRateReport7DayDetail"
                Else
                    iReportFlag = 1
                End If
            Case rbLast4Weeks.Checked
                dStartDate = DateAdd(DateInterval.Day, -28, Today.Date)
                dEndDate = DateAdd(DateInterval.Day, 27, dStartDate)

                If bDetail Then
                    sReportName = "FillRateReport4WeekDetail"
                Else
                    iReportFlag = 2
                End If
            Case rbFPtoDate.Checked
                dStartDate = Today.Date

                If bDetail Then
                    sReportName = "FillRateReportFPtD"
                Else
                    iReportFlag = 3
                End If

            Case rbCustomRange.Checked
                If bDetail Then
                    sReportName = "FillRateReport7DayDetail"
                Else
                    iReportFlag = 4
                End If

                dStartDate = GetUniversalDateString(dteStartDate.Value)
                dEndDate = GetUniversalDateString(dteEndDate.Value)
        End Select

        sReportURL.Append(sReportName)

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Facility=" & cmbFacility.SelectedValue)
        sReportURL.Append("&FacilityName=" & cmbFacility.SelectedItem.Text)

        If bDetail = False Then
            sReportURL.Append("&ReportFlag=" & iReportFlag)
        End If

        sReportURL.Append("&StartDate=" & dStartDate)

        If rbCustomRange.Checked Then
            sReportURL.Append("&EndDate=" & dEndDate)
        Else
            sReportURL.Append("&EndDate:isnull=true")
        End If

        sReportURL.Append("&TimeFrame=" & dStartDate.ToShortDateString & " - " & dEndDate.ToShortDateString)

        If cmbSubTeam.SelectedValue = "0" Then
            sReportURL.Append("&Subteam_No:isnull=true")
        Else
            sReportURL.Append("&Subteam_No=" & cmbSubTeam.SelectedValue)
            sReportURL.Append("&SubteamName=" & cmbSubTeam.SelectedItem.Text)
        End If

        If cmbVendor.SelectedValue = "0" Then
            sReportURL.Append("&Vendor_ID:isnull=true")
        Else
            sReportURL.Append("&Vendor_ID=" & cmbVendor.SelectedValue)
            'sReportURL.Append("&Vendor_Name=" & cmbVendor.SelectedItem.Text)
        End If

        If cmbStore.SelectedValue = "0" Then
            sReportURL.Append("&Store_No:isnull=true")
            sReportURL.Append("&Store_Name=none")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            sReportURL.Append("&Store_Name=" & cmbStore.SelectedItem.Text)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub rbCustomRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCustomRange.CheckedChanged
        EnableDateFields()
    End Sub

    Protected Sub rbLast7Days_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbLast7Days.CheckedChanged
        EnableDateFields()
    End Sub

    Protected Sub rbLast4Weeks_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbLast4Weeks.CheckedChanged
        EnableDateFields()
    End Sub

    Protected Sub rbFPtoDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbFPtoDate.CheckedChanged
        EnableDateFields()
    End Sub

    Private Sub EnableDateFields()
        lblStartDate.Enabled = rbCustomRange.Checked
        lblEndDate.Enabled = rbCustomRange.Checked
        dteStartDate.Enabled = rbCustomRange.Checked
        dteEndDate.Enabled = rbCustomRange.Checked
        rfvStartDate.Enabled = rbCustomRange.Checked
        rfvEndDate.Enabled = rbCustomRange.Checked
    End Sub
End Class
