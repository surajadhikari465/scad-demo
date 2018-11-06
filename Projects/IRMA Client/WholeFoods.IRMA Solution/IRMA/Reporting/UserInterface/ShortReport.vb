Public Class ShortReport

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPrintOnly.CheckedChanged

    End Sub

    Private Sub ShortReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

        '-- Load the combo boxes
        LoadDistributionCenters(cboWarehouse)
        If cboWarehouse.Items.Count > 0 Then
            cboWarehouse.SelectedIndex = 0
        End If

        
        LoadAllSubTeams(cboSubTeam)
        cboSubTeam.Items.Insert(0, "ALL")
        If cboSubTeam.Items.Count > 0 Then
            cboSubTeam.SelectedIndex = 0
        End If

        'hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        If cboWarehouse.SelectedIndex = -1 Then
            MsgBox("Business Unit must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        '--------------------------
        ' Setup Report URL
        '--------------------------

        sReportURL.Append("ShortReport")
        
        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------
        If cboWarehouse.Text = "ALL" Or cboWarehouse.Text = "" Then
            sReportURL.Append("&Warehouse:isnull=true")
        Else
            sReportURL.Append("&Warehouse=" & VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))
        End If


        If cboSubTeam.Text = "ALL" Or cboSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam:isnull=true")
        Else
            sReportURL.Append("&SubTeam=" & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
        End If


        sReportURL.Append("&BeginDate=" & Me.cmbFromDate.Text)

        sReportURL.Append("&EndDate=" & Me.cmbToDate.Text)


        '--------------------------
        ' Display Report
        '--------------------------
        'MsgBox("Test: URL = " & sReportURL.ToString)
        Call ReportingServicesReport(sReportURL.ToString)
    End Sub
End Class