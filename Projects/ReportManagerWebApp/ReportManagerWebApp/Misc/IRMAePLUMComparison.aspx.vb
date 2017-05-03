Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Misc_IRMAePlumCompare
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder
        Dim reportserver As String
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        reportserver = Application.Get("reportingServicesURL")

        'report name
        If optSummary.Checked Then
            sReportURL.Append("IRMAePLUMComparisonReport_Summary")
        Else
            sReportURL.Append("IRMAePLUMComparisonReport_Detail")
        End If

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 0 Then
            sReportURL.Append("&StoreList=" & GetAllStoreList())
        Else
            sReportURL.Append("&StoreList=" & cmbStore.SelectedValue)
        End If

        If cmbTeam.SelectedValue = 0 Then
            sReportURL.Append("&TeamNo:isnull=true")
        Else
            sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        End If

        If optSummary.Checked Then
            sReportURL.Append("&Summary=1")
        Else
            sReportURL.Append("&Summary=0")
        End If

        Response.Redirect(reportserver + sReportURL.ToString())

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Private Function GetAllStoreList() As String
        Dim x As Integer = 0
        Dim sResults As String = ""

        For x = 1 To cmbStore.Items.Count - 1
            If sResults = "" Then
                sResults = cmbStore.Items(x).Value
            Else
                sResults = sResults & "|" & cmbStore.Items(x).Value
            End If
        Next

        Return sResults
    End Function

    Protected Sub optDetail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optDetail.CheckedChanged
        rngValid_cmbStore.Enabled = True
    End Sub

    Protected Sub optSummary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optSummary.CheckedChanged
        rngValid_cmbStore.Enabled = False
    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound
        cmbTeam.Items.Insert(0, oListItemDefault)
    End Sub
End Class
