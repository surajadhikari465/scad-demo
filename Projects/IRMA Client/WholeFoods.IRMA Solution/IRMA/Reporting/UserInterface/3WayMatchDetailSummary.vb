Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class ThreeWayMatchDetailSummary
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub _3WayMatchDetailSummary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

        '  hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False

    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        '  Validation
        logger.Debug("cmdReport_Click Entry")

        If chkDateRange.Checked = True Then
            If dtpEndDate.Value < dtpStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
                dtpEndDate.Focus()
                Exit Sub
            End If
        End If
        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("3WayMatchDetailSummary")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If txtControlGroupID.Text = "ALL" Or txtControlGroupID.Text = "" Then
            sReportURL.Append("&ControlGroup_ID:isnull=true")
        Else
            sReportURL.Append("&ControlGroup_ID=" & Trim(txtControlGroupID.Text.ToString))
        End If

        If Trim(txtPurchaseOrderID.Text) = "" Then
            sReportURL.Append("&PoNumber_Id:isnull=true")
        Else
            sReportURL.Append("&PoNumber_Id=" & txtPurchaseOrderID.Text.ToString)
        End If

        If chkDateRange.Checked Then
            sReportURL.Append("&BeginDate=" & dtpStartDate.Value)
            sReportURL.Append("&EndDate=" & dtpEndDate.Value)
        Else
            sReportURL.Append("&BeginDate:isnull=true")
            sReportURL.Append("&EndDate:isnull=true")
        End If
        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("cmdReport_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub chkDateRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDateRange.CheckedChanged
        If chkDateRange.Checked Then
            dtpEndDate.Enabled = True
            dtpStartDate.Enabled = True
        Else
            dtpEndDate.Enabled = False
            dtpStartDate.Enabled = False
        End If
    End Sub

    Private Sub cmbControlGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub
End Class