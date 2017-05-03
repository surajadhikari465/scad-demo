Option Strict Off
Option Explicit On
Imports log4net

Friend Class frmOrderGuideReport
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        logger.Debug("cmdReport_Click Entry")

        Dim strStore As String
        Dim strWareHouse As String
        Dim strSubTeam As String
        Dim strWFM As String
        Dim strPreOrder As String
        Dim sReportFile As String = String.Empty
        Dim sReportURL As New System.Text.StringBuilder

        ' GET THE STORE SELECTED
        If cboStore.SelectedIndex > -1 Then
            strStore = CStr(VB6.GetItemData(cboStore, cboStore.SelectedIndex))
        Else
            logger.Info("cmdReport_Click - Store must be selected")
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Error")
            cboStore.Focus()
            Exit Sub
        End If

        ' GET THE WAREHOUSE SELECTED
        If cboWarehouse.SelectedIndex > -1 Then
            strWareHouse = CStr(VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))
        Else
            logger.Info("cmdReport_Click - Warehouse must be selected.")
            MsgBox("Warehouse must be selected.", MsgBoxStyle.Exclamation, "Error")
            cboWarehouse.Focus()
            Exit Sub
        End If

        ' GET THE SUBTEAM SELECTED
        If cboSubTeam.SelectedIndex > -1 Then
            strSubTeam = CStr(VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
        Else
            logger.Info("cmdReport_Click - Sub-Team must be selected.")
            MsgBox("Sub-Team must be selected.", MsgBoxStyle.Exclamation, "Error")
            cboSubTeam.Focus()
            Exit Sub
        End If

        ' GET WFM ITEMS
        strWFM = CStr(System.Math.Abs(chkWFMItems.CheckState))

        ' GET PREODER INFORMATION
        strPreOrder = CStr(System.Math.Abs(chkPreOrder.CheckState))

        Me.Text = "Running the Order Guide Report..."

        ' Setup Report URL for Reporting Services
        sReportURL.Append("MA_OrderGuide")

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters
        sReportURL.Append("&Store_No=" & strStore)
        sReportURL.Append("&Warehouse_ID=" & strWareHouse)
        sReportURL.Append("&SubTeam_No=" & strSubTeam)
        sReportURL.Append("&WFM_Item=" & chkWFMItems.CheckState)
        sReportURL.Append("&Pre_Order=" & chkPreOrder.CheckState)

        ' Display Report
        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("cmdReport_Click Exit")

    End Sub
	
	Private Sub frmOrderGuideReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderGuideReport_Load Entry")

        '-- Center the form
		CenterForm(Me)
        '-- Load the combo boxes
		LoadInventoryStore(cboStore)
		LoadInventoryStore(cboWarehouse)
        LoadAllSubTeams(cboSubTeam)

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
        End If

        logger.Debug("frmOrderGuideReport_Load Exit")
		
	End Sub
End Class