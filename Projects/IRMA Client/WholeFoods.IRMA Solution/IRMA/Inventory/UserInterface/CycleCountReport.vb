Option Strict Off
Option Explicit On
Friend Class frmCycleCountReport
	Inherits System.Windows.Forms.Form
	
	Private miReportType As Short
    Private mlMasterCountID As Long
    Private mlCycleCountID() As Long
	
    Public Sub LoadForm(ByRef MasterCountID As Integer, ByRef lCycleCountID() As Long)

        mlMasterCountID = MasterCountID
        mlCycleCountID = VB6.CopyArray(lCycleCountID)

        Me.ShowDialog()

    End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sReportURL As String
		Dim iCnt As Short
		
        For iCnt = 0 To UBound(mlCycleCountID)

            sReportURL = "CycleCount&rs:Command=Render&rc:Parameters=false" & "&MasterCountID=" & mlMasterCountID & "&CycleCountID=" & mlCycleCountID(iCnt) & "&RetailItems=" & chkRetail.CheckState & "&IngredientItems=" & chkIngredients.CheckState

            Call ReportingServicesReport(sReportURL)

        Next iCnt
		
	End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub chkRetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRetail.CheckedChanged
        Call SetReportButton()
    End Sub

    Private Sub chkIngredients_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIngredients.CheckedChanged
        Call SetReportButton()
    End Sub

    Private Sub SetReportButton()

        If Me.chkIngredients.Checked = True Or Me.chkRetail.Checked = True Then
            Me.cmdReport.Enabled = True
        Else
            Me.cmdReport.Enabled = False
        End If

    End Sub
End Class