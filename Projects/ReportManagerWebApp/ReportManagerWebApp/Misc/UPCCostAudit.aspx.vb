Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Misc_UPCCostAudit
    Inherits System.Web.UI.Page
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))

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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemDefault = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim reportserver As String
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern
        Dim sIdentifiers As String = ""

        reportserver = Application.Get("reportingServicesURL")

        'report name
        sReportURL.Append("UPCCostAudit")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbTeam.SelectedValue = 0 Then
            sReportURL.Append("&TeamNo:isnull=true")
        Else
            sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeamNo:isnull=true")
        Else
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        If cmbVendor.SelectedValue = 0 Then
            sReportURL.Append("&VendorId:isnull=true")
        Else
            sReportURL.Append("&VendorId=" & cmbVendor.SelectedValue)
        End If

        sReportURL.Append("&StartDate=" & CDate(GetUniversalDateString(dteBeginDate.Value)).ToString("MM/dd/yyyy"))
        sReportURL.Append("&EndDate=" & CDate(GetUniversalDateString(dteEndDate.Value)).ToString("MM/dd/yyyy"))

        If txtIdentifiers.Text = "" Then
            sReportURL.Append("&IdentifierList:isnull=true")
        Else
            sReportURL.Append("&IdentifierList=" & Replace(RemoveLeadingZeros(txtIdentifiers.Text), vbCrLf, "|") & "|")  'NOTE: a pipe character "|" is required at the end to prevent an infinte loop
        End If

        Response.Redirect(reportserver + sReportURL.ToString())

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound
        'add the default item
        cmbTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)
    End Sub

    Private Function RemoveLeadingZeros(ByVal sIdentifierList As String) As String
        Dim aryIdentifiers() As String
        Dim sData As String = ""
        Dim x As Integer = 0

        aryIdentifiers = Split(sIdentifierList, vbCrLf)

        For x = 0 To UBound(aryIdentifiers)
            If aryIdentifiers(x) <> "" Then
                If sData = "" Then
                    sData = Convert.ToString(Convert.ToInt64(aryIdentifiers(x))) & vbCrLf
                Else
                    sData = sData & Convert.ToString(Convert.ToInt64(aryIdentifiers(x))) & vbCrLf
                End If
            End If
        Next

        Return sData
    End Function
End Class
