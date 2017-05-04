Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net
Public Class ControlGroup3WayMatchLog

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        '  Validation
        logger.Debug("cmdReport_Click Entry")

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("ControlGroup3WayMatchLog")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------


        If txtControlGroupID.Text = "ALL" Or txtControlGroupID.Text = "" Then
            sReportURL.Append("&ControlGroup_ID:isnull=true")
        Else
            sReportURL.Append("&ControlGroup_ID=" & Trim(txtControlGroupID.Text.ToString))
        End If


        If cmbUserID.Text = "ALL" Or Trim(cmbUserID.Text) = "" Then
            sReportURL.Append("&User_ID:isnull=true")
        Else
            sReportURL.Append("&User_ID=" & Trim(cmbUserID.SelectedValue.ToString))
        End If


        If cmbStatus.Text = "ALL" Or Trim(cmbStatus.Text) = "" Then
            sReportURL.Append("&Status_ID:isnull=true")
        Else
            sReportURL.Append("&Status_ID=" & Trim(cmbStatus.SelectedValue.ToString))
        End If
        '--------------------------
        ' Display Report
        '--------------------------
        'Dim s As String = sReportURL.ToString()
        Call ReportingServicesReport(sReportURL.ToString)
        logger.Debug("cmdReport_Click Exit")
    End Sub

    Private Sub ControlGroup3WayMatchLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        logger.Debug("ControlGroup3WayMatchLog_Load Entry")
        '-- Center the form
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim userList As ArrayList = UserAccessDAO.GetUserDetails()
        With cmbUserID
            .DataSource = userList
            .DisplayMember = "UserName"
            .ValueMember = "UserID"
        End With


        Dim ControlGroupStatusList As ArrayList = ControlGroupStatusDAO.GetControlGroupStatus()
        With cmbStatus
            .DataSource = ControlGroupStatusList
            .DisplayMember = "ControlGroupStatusName"
            .ValueMember = "ControlGroupStatusID"
        End With

        '  hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
        chkPrintOnly.Visible = False
        logger.Debug("ControlGroup3WayMatchLog_Load Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class