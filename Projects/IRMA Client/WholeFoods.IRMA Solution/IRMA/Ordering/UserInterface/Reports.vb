Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmReports
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


	Private Sub cmdCheckList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCheckList.Click

        logger.Debug("cmdCheckList_Click Entry")
        frmReceivingCheckList.ShowDialog()
        frmReceivingCheckList.Dispose()
        logger.Debug("cmdCheckList_Click Exit")

	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub
	
	Private Sub cmdInvoice_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdInvoice.Click
        logger.Debug("cmdInvoice_Click Entry")
        frmInvoiceReport.ShowDialog()
        frmInvoiceReport.Dispose()
        logger.Debug("cmdInvoice_Click Exit")
		
	End Sub
	
	Private Sub cmdPurchaseOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPurchaseOrder.Click
        logger.Debug("cmdPurchaseOrder_Click Entry")
        frmPurchaseOrderReport.ShowDialog()
        frmPurchaseOrderReport.Dispose()
        logger.Debug("cmdPurchaseOrder_Click Exit")
		
	End Sub
	
	Private Sub frmReports_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmReports_Load Entry")
        CenterForm(Me)
        logger.Debug("frmReports_Load Exit")
		
	End Sub
End Class