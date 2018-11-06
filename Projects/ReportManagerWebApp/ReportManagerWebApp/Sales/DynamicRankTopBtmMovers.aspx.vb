Imports ReportManagerWebApp.WFM_Common

Partial Class Sales_DynamicRankTopBtmMovers
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< ALL >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = Today()

        'set minimum date
        rngValid_BeginDate.MinimumValue = dtMinDate
        rngValid_EndDate.MinimumValue = dtMinDate

        dteBeginDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        'set maximum date
        rngValid_BeginDate.MaximumValue = dtMaxDate
        rngValid_EndDate.MaximumValue = dtMaxDate

        dteBeginDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        dteBeginDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub lstStores_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStores.DataBound
        lstStores.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub lstCommodityCode_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstCommodityCode.DataBound
        lstCommodityCode.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub lstSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSubTeam.DataBound
        lstSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub lstVenue_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstVenue.DataBound
        lstVenue.Items.Insert(0, oListItemAll)
    End Sub

    Protected Function CommaDelimetedList(ByVal list As ListBox) As String
        Dim itemsList As ListItem
        Dim strItemsList As New System.Text.StringBuilder

        For Each itemsList In list.Items
            If itemsList.Selected Then
                strItemsList.Append(itemsList.Value.ToString() + ",")
            End If
        Next

        If strItemsList.Length > 1 Then
            strItemsList.Remove(strItemsList.Length - 1, 1)
        End If

        Return strItemsList.ToString()
    End Function

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") & "_DynamicRankTopMovers")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If lstStores.SelectedValue = "0" Or lstStores.SelectedValue = "" Then
            sReportURL.Append("&StoreList:isnull=true")
        ElseIf CommaDelimetedList(lstStores) <> "" Then
            sReportURL.Append("&StoreList=" & CommaDelimetedList(lstStores))
        End If

        If lstCommodityCode.SelectedValue = "0" Or lstCommodityCode.SelectedValue = "" Then
            sReportURL.Append("&CommodityList:isnull=true")
        ElseIf CommaDelimetedList(lstCommodityCode) <> "" Then
            sReportURL.Append("&CommodityList=" & CommaDelimetedList(lstCommodityCode))
        End If

        If lstSubTeam.SelectedValue = "0" Or lstSubTeam.SelectedValue = "" Then
            sReportURL.Append("&SubteamList:isnull=true")
        ElseIf CommaDelimetedList(lstSubTeam) <> "" Then
            sReportURL.Append("&SubteamList=" & CommaDelimetedList(lstSubTeam))
        End If

        If lstVenue.SelectedValue = "0" Or lstVenue.SelectedValue = "" Then
            sReportURL.Append("&VenueList:isnull=true")
        ElseIf CommaDelimetedList(lstVenue) <> "" Then
            sReportURL.Append("&VenueList=" & CommaDelimetedList(lstVenue))
        End If

        If txtTopNumber.Text.ToString() Is Nothing Or txtTopNumber.Text.ToString() = "" Then
            sReportURL.Append("&TopNumber:isnull=true")
        Else
            sReportURL.Append("&TopNumber=" & txtTopNumber.Text.ToString())
        End If

        If txtItemList.Text.ToString() Is Nothing Or txtItemList.Text.ToString() = "" Then
            sReportURL.Append("&ItemList:isnull=true")
        Else
            Dim itemListString As String
            itemListString = Regex.Replace(txtItemList.Text.ToString(), "\s+", ",")
            sReportURL.Append("&ItemList=" & itemListString.ToString())
        End If

        sReportURL.Append("&StartDate=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&EndDate=" & GetUniversalDateString(dteEndDate.Value))

        sReportURL.Append("&MovementTable=" & radMovementTable.SelectedValue.ToString())
        sReportURL.Append("&RankBy=" & cmbRankBy.SelectedValue.ToString())
        sReportURL.Append("&BreakBy=" & radBreakBy.SelectedValue.ToString())
        sReportURL.Append("&SortOrder=" & radSortBy.SelectedValue.ToString())

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        'Response.Write(sReportServer + sReportURL.ToString())
    End Sub

    Protected Sub radMovementTable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radMovementTable.SelectedIndexChanged
        lblMovement.Text = radMovementTable.SelectedValue.ToString()
        If lblMovement.Text.ToString() = "1" Then
            If dteBeginDate.Value.ToString() <> "" Then
                calendarChange(dteBeginDate, "Monday")
            End If

            If dteEndDate.Value IsNot Nothing Then
                calendarChange(dteEndDate, "Sunday")
            End If
        End If
    End Sub

    Protected Sub calendarChange(ByVal calendar As Infragistics.WebUI.WebSchedule.WebDateChooser, ByVal weekDay As String)
        Dim valueDate As Date
        valueDate = calendar.Value
        If valueDate.DayOfWeek.ToString() <> weekDay.ToString() Then
            calendar.Value = "< Enter Date >"
        End If
    End Sub
End Class
