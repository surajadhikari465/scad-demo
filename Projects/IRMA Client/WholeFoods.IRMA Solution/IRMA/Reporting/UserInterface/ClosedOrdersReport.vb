Option Strict Off
Option Explicit On
Friend Class frmClosedOrdersReport
	Inherits System.Windows.Forms.Form
	
    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
    Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbStore.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
        Dim sTitle As String = String.Empty
        'Dim crd As CrystalDecisions.CrystalReports.Engine.ReportDocument
		
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        Me.Enabled = False

        Dim sReportURL As New System.Text.StringBuilder

        If chkSummary.CheckState Then
            sReportURL.Append("Closed Orders Summary")
        Else
            sReportURL.Append("Closed Orders Report")
        End If

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------
        If cmbStore.Text = "ALL" Or Trim(cmbStore.Text) = "" Then
            sReportURL.Append("&Vendor_Id:isnull=true")
        Else
            sReportURL.Append("&Vendor_Id=" & CrystalValue(cmbStore))
        End If

        If cmbSubTeam.Text = "ALL" Or Trim(cmbSubTeam.Text) = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & CrystalValue(cmbSubTeam))
        End If


        sReportURL.Append("&StartDate=" & dtpStartDate.Text)
        sReportURL.Append("&EndDate=" & dtpEndDate.Text)
     
        sReportURL.Append("&InternalCustomer=" & System.Math.Abs(chkField.CheckState))

        Call ReportingServicesReport(sReportURL.ToString)




        Try
            '' ###########################################################################
            '' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
            '' ###########################################################################
            'MsgBox("ClosedOrdersreport.vb  cmdReport_Click(): The Crystal Report ClosedOrdersSummary.rpt or ClosedOrders.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

            'crd = New CrystalDecisions.CrystalReports.Engine.ReportDocument

            'If chkSummary.CheckState Then
            '    crd.FileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ClosedOrdersSummary.rpt"
            '    sTitle = "Closed Orders Summary" & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
            'Else
            '    crd.FileName = My.Application.Info.DirectoryPath & gsReportDirectory & "ClosedOrders.rpt"
            '    sTitle = "Closed Orders Report" & vbCrLf & dtpStartDate.Text & " - " & dtpEndDate.Text
            'End If

            'crd.SummaryInfo.ReportTitle = sTitle

            'crd.SetParameterValue(0, CrystalValue2(cmbSubTeam))
            'crd.SetParameterValue(1, dtpStartDate.Text)
            'crd.SetParameterValue(2, dtpEndDate.Text)
            'crd.SetParameterValue(3, System.Math.Abs(chkField.CheckState))
            'crd.SetParameterValue(4, CrystalValue2(cmbStore))

            '' ReportViewer.ConnectReport(crd)
            'If chkPrintOnly.CheckState = 0 Then
            '    Dim rv As New ReportViewer
            '    rv.Report = crd
            '    rv.ShowDialog()
            '    rv.Dispose()
            'Else
            '    crd.PrintToPrinter(0, False, 0, 0)
            'End If
            'crd.Dispose()
        Finally
            Me.Enabled = True
        End Try

    End Sub

    Private Sub frmClosedOrdersReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        LoadInternalCustomer(cmbStore)
        LoadAllSubTeams(cmbSubTeam)

        dtpStartDate.Value = SystemDateTime()
        dtpEndDate.Value = SystemDateTime()
    End Sub
End Class