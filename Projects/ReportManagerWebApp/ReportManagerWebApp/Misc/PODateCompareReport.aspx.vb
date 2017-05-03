Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Misc_PODateCompare
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
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadDateTypeCombo(cmbDateDiff1)
            LoadDateTypeCombo(cmbDateDiff2)
            LoadDateTypeCombo(cmbDateCompare)

            'set drop down defaults
            cmbDateDiff1.SelectedIndex = 3    'Sent Date
            cmbDateDiff2.SelectedIndex = 1    'Invoice Date
            cmbDateCompare.SelectedIndex = 0  'Close Date
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append("PODateComparisonReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 0 Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        If cmbUser.SelectedValue = 0 Then
            sReportURL.Append("&User_Id:isnull=true")
        Else
            sReportURL.Append("&User_Id=" & cmbUser.SelectedValue)
        End If

        If cmbVendor.SelectedValue = 0 Then
            sReportURL.Append("&Vendor_Id:isnull=true")
        Else
            sReportURL.Append("&Vendor_Id=" & cmbVendor.SelectedValue)
        End If

        sReportURL.Append("&DateDiffId1=" & CInt(cmbDateDiff1.SelectedIndex) + 1)
        sReportURL.Append("&DateDiffId2=" & CInt(cmbDateDiff2.SelectedIndex) + 1)
        sReportURL.Append("&DateCompareId=" & CInt(cmbDateCompare.SelectedIndex) + 1)
        sReportURL.Append("&StartDate=" & CDate(GetUniversalDateString(dteBeginDate.Value)).ToString("MM/dd/yyyy"))
        sReportURL.Append("&EndDate=" & CDate(GetUniversalDateString(dteEndDate.Value)).ToString("MM/dd/yyyy"))

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

    Protected Sub cmbUser_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbUser.DataBound
        'add the default item
        cmbUser.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)
    End Sub

    Private Sub LoadDateTypeCombo(ByVal cmb As DropDownList)
        cmb.Items.Insert(0, "Close Date")
        cmb.Items.Insert(1, "Invoice Date")
        cmb.Items.Insert(2, "Order Date")
        cmb.Items.Insert(3, "Sent Date")
    End Sub
End Class
