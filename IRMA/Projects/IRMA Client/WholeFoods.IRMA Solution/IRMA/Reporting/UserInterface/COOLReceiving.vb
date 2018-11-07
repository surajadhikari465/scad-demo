Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class COOLReceiving
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub _COOLReceiving_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

        '  hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False

    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        '  Validation
        logger.Debug("cmdReport_Click Entry")

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("COOLReceivingReport")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If txtItemIdentifier.Text = "ALL" Or txtItemIdentifier.Text = "" Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & Trim(txtItemIdentifier.Text.ToString))
        End If

        If Trim(txtPurchaseOrderID.Text) = "" Then
            sReportURL.Append("&POID:isnull=true")
        Else
            sReportURL.Append("&POID=" & txtPurchaseOrderID.Text.ToString)
        End If

        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("cmdReport_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub


End Class