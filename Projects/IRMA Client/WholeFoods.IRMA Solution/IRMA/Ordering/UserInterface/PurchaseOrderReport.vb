Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Imports log4net
Friend Class frmPurchaseOrderReport
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
        Dim _appID As New Guid(ConfigurationManager.AppSettings("ApplicationGUID").ToString)
        Dim _envID As New Guid(ConfigurationManager.AppSettings("EnvironmentGUID").ToString)
        Debug.Print(_envID.ToString)
        If glAllowBarcodePOReport = True Then
            ReportingServicesReport(String.Format("PurchaseOrderReportBarcodes&rs:Command=Render&rc:Parameters=False&AppID={0}&EnvID={1}&OrderHeader_ID={2}&Item_ID={3}&SortType={4}&GroupByCat={5}", _
                _appID, _
                _envID, _
                glOrderHeaderID, _
                optIdentifier(0).Checked, _
                IIf(optSort(0).Checked, 1, IIf(optSort(1).Checked, 2, IIf(optSort(2).Checked, 3, 4))) _
                , IIf(Me.GroupByCatCheckBox.Checked, True, False)))
        Else
            ReportingServicesReport(String.Format("PurchaseOrderReport&rs:Command=Render&rc:Parameters=False&AppID={0}&EnvID={1}&OrderHeader_ID={2}&Item_ID={3}&SortType={4}&GroupByCat={5}", _
                _appID, _
                _envID, _
                glOrderHeaderID, _
                optIdentifier(0).Checked, _
                IIf(optSort(0).Checked, 1, IIf(optSort(1).Checked, 2, IIf(optSort(2).Checked, 3, 4))) _
                , IIf(Me.GroupByCatCheckBox.Checked, True, False)))

        End If
        logger.Debug("cmdReport_Click Exit")
    End Sub
	
	Private Sub frmPurchaseOrderReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
        If glAllowBarcodePOReport = True Then
            CheckBoxBarcodePOReport.Checked = True
        Else
            CheckBoxBarcodePOReport.Checked = False
        End If
       
    End Sub

    Private Sub GroupByCatCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupByCatCheckBox.CheckedChanged, CheckBoxBarcodePOReport.CheckedChanged
        If Me.GroupByCatCheckBox.Checked Then
            SortByFrame.Enabled = False
        Else
            SortByFrame.Enabled = True
        End If
    End Sub

End Class