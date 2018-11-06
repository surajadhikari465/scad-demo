Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Price_InStoreSpecials
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< ALL >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

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

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound

        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)

    End Sub

    Protected Sub lstSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.DataBound

        cmbSubteam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append(Application.Get("region") + "_" + "InStoreSpecialsReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)

        If cmbSubteam.SelectedIndex = 0 Then
            sReportURL.Append("&SubTeam_No=" & PipeDelimetedList(cmbSubteam))
        ElseIf cmbSubteam.SelectedIndex > 0 Then
            sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
        End If

        If dteBeginDate.Text = "Null" Then
            sReportURL.Append("&Sale_Start_Date:isnull=true")
        Else
            sReportURL.Append("&Sale_Start_Date=" & dteBeginDate.Value)
        End If

        If dteEndDate.Text = "Null" Then
            sReportURL.Append("&Sale_End_Date:isnull=true")
        Else
            sReportURL.Append("&Sale_End_Date=" & dteEndDate.Value)
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Function PipeDelimetedList(ByVal list As DropDownList) As String
        Dim itemsList As ListItem
        Dim strItemsList As New System.Text.StringBuilder

        For Each itemsList In list.Items
            If Not itemsList.Value.ToString = "0" Then
                strItemsList.Append(itemsList.Value.ToString() + "|")
            End If
        Next

        If strItemsList.Length > 1 Then
            strItemsList.Remove(strItemsList.Length - 1, 1)
        End If

        Return strItemsList.ToString()
    End Function

End Class
