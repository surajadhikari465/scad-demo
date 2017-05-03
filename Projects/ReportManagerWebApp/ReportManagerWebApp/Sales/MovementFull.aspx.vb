Imports ReportManagerWebApp.WFM_Common

Partial Class Sales_MovementFull
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))

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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item for all teams
        cmbTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item for all subteams
        cmbSubTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.DataBound

        'add the default item
        cmbCategory.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub btnSetDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetDate.Click
        'set date range to the previous week

        dteBeginDate.Value = DateAdd(DateInterval.Day, -7, Today())
        dteEndDate.Value = DateAdd(DateInterval.Day, -1, Today())

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append("MovementFull")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        sReportURL.Append("&StartDate=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&EndDate=" & GetUniversalDateString(dteEndDate.Value))

        If cmbTeam.SelectedValue <> 0 Then
            sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        If cmbCategory.SelectedValue <> 0 Then
            sReportURL.Append("&Category_ID=" & cmbCategory.SelectedValue)
        End If

        If Trim(textBoxIdentifier.Text) <> String.Empty Then
            sReportURL.Append("&Identifier=" & Trim(textBoxIdentifier.Text))
        End If

        If Trim(textBoxItemDescription.Text) <> String.Empty Then
            sReportURL.Append("&Item_Description=" & Trim(textBoxItemDescription.Text))
        End If

        Dim i As Integer
        Dim loc As Integer
        Dim s As String = String.Empty

        If (listBoxBrands.SelectedIndex() >= 0) Then
            Dim strBrandList As String

            strBrandList = String.Empty
            For i = 0 To listBoxBrands.Items.Count - 1
                If listBoxBrands.Items(i).Selected Then
                    strBrandList = strBrandList & listBoxBrands.Items(i).Value & ","
                End If
            Next

            loc = strBrandList.Length - 1

            If (loc >= 0) Then
                s = strBrandList.Remove(loc)
            End If
            sReportURL.Append("&Brand_ID_List=" & s)
        End If

        If (listBoxVendors.SelectedIndex() >= 0) Then
            Dim strVendorList As String
            strVendorList = String.Empty
            For i = 0 To listBoxVendors.Items.Count - 1
                If listBoxVendors.Items(i).Selected Then
                    strVendorList = strVendorList & listBoxVendors.Items(i).Value & ","
                End If
            Next

            loc = strVendorList.Length - 1
            s = String.Empty
            If (loc >= 0) Then
                s = strVendorList.Remove(loc)
            End If
            sReportURL.Append("&Vendor_ID_List=" & s)
        End If

        If Trim(textBoxVendorItemID.Text) <> String.Empty Then
            sReportURL.Append("&VendorItemID=" & Trim(textBoxVendorItemID.Text))
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

End Class
