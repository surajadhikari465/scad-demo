Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_FacilityOrderReport
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
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

        dteBeginDate.Value = Now.Date
        dteEndDate.Value = Now.Date

        lblSortingError.Visible = False
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub cmbFacility_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFacility.DataBound
        'add the default item
        cmbFacility.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        If Not CheckSortingOptions() Then Exit Sub



        'report name
        sReportURL.Append("FacilityOrderReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&StartDate=" & GetUniversalDateString(dteBeginDate.Value))
        sReportURL.Append("&EndDate=" & GetUniversalDateString(dteEndDate.Value))
        sReportURL.Append("&Facility_ID=" & cmbFacility.SelectedValue)

        sReportURL.Append("&SortLevel1=" & GetSortingID(1))
        sReportURL.Append("&SortLevel2=" & GetSortingID(2))
        sReportURL.Append("&SortLevel3=" & GetSortingID(3))

        If chkAll_SubTeam.Checked Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & GetItemList(lbSubTeamList))
        End If

        If chkAll_Store.Checked Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & GetItemList(lbStoreList))
        End If

        If cmbPOStatus.SelectedValue <> 0 Then
            sReportURL.Append("&Status_ID=" & cmbPOStatus.SelectedValue)
        Else
            sReportURL.Append("&Status_ID:isnull=true")
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Private Function GetSortingID(ByVal iLevel As Integer) As String
        GetSortingID = 1

        If iLevel = 1 Then
            If optSortLevel1_Store.Checked Then GetSortingID = "1"
            If optSortLevel1_SubTeam.Checked Then GetSortingID = "2"
            If optSortLevel1_ExpectedDate.Checked Then GetSortingID = "3"
        End If

        If iLevel = 2 Then
            If optSortLevel2_Store.Checked Then GetSortingID = "1"
            If optSortLevel2_SubTeam.Checked Then GetSortingID = "2"
            If optSortLevel2_ExpectedDate.Checked Then GetSortingID = "3"
        End If

        If iLevel = 3 Then
            If optSortLevel3_Store.Checked Then GetSortingID = "1"
            If optSortLevel3_SubTeam.Checked Then GetSortingID = "2"
            If optSortLevel3_ExpectedDate.Checked Then GetSortingID = "3"
        End If
    End Function

    Private Function CheckSortingOptions() As Boolean
        'make sure sorting options are correct - each option can only be selected once.
        If (optSortLevel1_Store.Checked And optSortLevel2_Store.Checked) Or _
           (optSortLevel1_Store.Checked And optSortLevel3_Store.Checked) Or _
           (optSortLevel2_Store.Checked And optSortLevel3_Store.Checked) Or _
           (optSortLevel1_SubTeam.Checked And optSortLevel2_SubTeam.Checked) Or _
           (optSortLevel1_SubTeam.Checked And optSortLevel3_SubTeam.Checked) Or _
           (optSortLevel2_SubTeam.Checked And optSortLevel3_SubTeam.Checked) Or _
           (optSortLevel1_ExpectedDate.Checked And optSortLevel2_ExpectedDate.Checked) Or _
           (optSortLevel2_ExpectedDate.Checked And optSortLevel3_ExpectedDate.Checked) Or _
           (optSortLevel2_ExpectedDate.Checked And optSortLevel3_ExpectedDate.Checked) Then
            lblSortingError.Visible = True
            CheckSortingOptions = False
        Else
            lblSortingError.Visible = False
            CheckSortingOptions = True
        End If
    End Function

    Protected Sub optSortLevel2_Store_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optSortLevel1_Store.CheckedChanged, optSortLevel1_ExpectedDate.CheckedChanged, optSortLevel1_SubTeam.CheckedChanged, _
                                                                                                                  optSortLevel2_Store.CheckedChanged, optSortLevel2_ExpectedDate.CheckedChanged, optSortLevel2_SubTeam.CheckedChanged, _
                                                                                                                  optSortLevel3_Store.CheckedChanged, optSortLevel3_ExpectedDate.CheckedChanged, optSortLevel3_SubTeam.CheckedChanged
        Dim blnSort As Boolean

        blnSort = CheckSortingOptions()
    End Sub

    Private Sub DoSelection(ByVal lbList As ListBox, ByVal blnSelect As Boolean)
        Dim x As Integer

        For x = 0 To lbList.Items.Count - 1
            lbList.Items(x).Selected = blnSelect
        Next
    End Sub

    Protected Sub lbStoreList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStoreList.DataBound
        DoSelection(lbStoreList, True)
    End Sub

    Protected Sub lbSubTeamList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSubTeamList.DataBound
        DoSelection(lbSubTeamList, True)
    End Sub

    Protected Sub chkAll_Store_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll_Store.CheckedChanged
        If chkAll_Store.Checked Then
            DoSelection(lbStoreList, True)
        Else
            DoSelection(lbStoreList, False)
        End If
    End Sub

    Protected Sub chkAll_SubTeam_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll_SubTeam.CheckedChanged
        If chkAll_SubTeam.Checked Then
            DoSelection(lbSubTeamList, True)
        Else
            DoSelection(lbSubTeamList, True)
        End If
    End Sub

    Private Function GetItemList(ByVal lbList As ListBox) As String
        Dim x As Integer

        GetItemList = ""

        For x = 0 To lbList.Items.Count - 1
            If lbList.Items(x).Selected Then
                If GetItemList = "" Then
                    GetItemList = lbList.Items(x).Value
                Else
                    GetItemList = GetItemList & "|" & lbList.Items(x).Value
                End If
            End If
        Next
    End Function
End Class
