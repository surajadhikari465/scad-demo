Option Strict Off
Option Explicit On
Friend Class frmZeroPriceReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		If cmbSubTeam.SelectedIndex = -1 Then
			MsgBox("SubTeam is required.", MsgBoxStyle.Information + MsgBoxStyle.OKOnly, Me.Text)
			cmbSubTeam.Focus()
			Exit Sub
		End If
		
		If cmbStore.SelectedIndex = -1 Then
			MsgBox("Store is required.", MsgBoxStyle.Information + MsgBoxStyle.OKOnly, Me.Text)
			cmbStore.Focus()
			Exit Sub
		End If
		
        '' SubTeam
        'crwReport.set_StoredProcParam(0, CrystalValue(cmbSubTeam))
        '' Store
        'crwReport.set_StoredProcParam(1, CrystalValue(cmbStore))
        '' Cost/Price
        'crwReport.set_StoredProcParam(2, "Price")

        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ZEROCOSTPRICE.RPT"
        'crwReport.ReportTitle = "Zero Prices:  " & cmbStore.Text
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)


        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("ZeroPriceReport.vb  cmdReport_Click(): The Crystal Report ZEROCOSTPRICE.RPT is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)
        'Dim crd As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        'crd.FileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ZEROCOSTPRICE.RPT"
        'crd.SetParameterValue(0, CrystalValue(cmbSubTeam))
        'crd.SetParameterValue(1, CrystalValue(cmbStore))
        'crd.SetParameterValue(2, "Price")

        'crd.SummaryInfo.ReportTitle = "Zero Prices:  " & cmbStore.Text
        'ReportViewer.ConnectReport(crd)
        'If chkPrintOnly.CheckState = 0 Then
        '    Dim rv As New ReportViewer
        '    rv.Report = crd
        '    rv.ShowDialog()
        '    rv.Dispose()
        'Else
        '    crd.PrintToPrinter(0, False, 0, 0)
        'End If
        'crd.Dispose()
		
	End Sub
	
	Private Sub frmZeroPriceReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
		
		'-- Load the combos
		LoadAllSubTeams(cmbSubTeam)
		
		LoadStore(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = 0
        End If
	End Sub
End Class