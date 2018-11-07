Option Strict Off
Option Explicit On
Friend Class frmShipperItemsReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("ShipperItemsReport.vb  cmdReport_Click(): The Crystal Report ShipperItems.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ShipperItems.rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)
		
	End Sub
	
	Private Sub frmShipperItemsReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
	End Sub
End Class