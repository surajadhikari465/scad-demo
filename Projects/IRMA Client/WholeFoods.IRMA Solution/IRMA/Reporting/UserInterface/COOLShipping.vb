Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class COOLShipping
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub _COOLShipping_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

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

        sReportURL.Append("COOLShippingReport")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If chkDateRange.Checked Then
            sReportURL.Append("&BeginDate=" & dtpStartDate.Value)
            sReportURL.Append("&EndDate=" & dtpEndDate.Value)
        Else
            sReportURL.Append("&BeginDate:isnull=true")
            sReportURL.Append("&EndDate:isnull=true")
        End If

        If txtStore_No.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & Trim(txtStore_No.Text.ToString))
        End If

        If chkBIO.Checked Then
            sReportURL.Append("&BIO=True")
        Else
            sReportURL.Append("&BIO=False")
        End If
        If chkCOOL.Checked Then
            sReportURL.Append("&COOL=True")
        Else
            sReportURL.Append("&COOL=False")
        End If
        If txtDC.Text = "" Then
            sReportURL.Append("&DC:isnull=true")
        Else
            sReportURL.Append("&DC=" & Trim(txtDC.Text.ToString))
        End If

        If txtSubTeam_No.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & Trim(txtSubTeam_No.Text.ToString))
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

End Class