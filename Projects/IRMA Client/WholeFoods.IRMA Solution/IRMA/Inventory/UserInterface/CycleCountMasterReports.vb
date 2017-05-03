Option Strict Off
Option Explicit On
Friend Class frmCycleCountMasterReports
	Inherits System.Windows.Forms.Form
	
    Private mvMasterCountID() As Long

    Private IsInitializing As Boolean
	
    Public Sub LoadForm(ByRef MasterCountID() As Long)

        mvMasterCountID = VB6.CopyArray(MasterCountID)

        Me.ShowDialog()

    End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sReportURL As String
		Dim iCnt As Integer
        sReportURL = String.Empty

		For iCnt = 0 To UBound(mvMasterCountID)
			
			If Me.optMasterSummary.Checked = True Then
                sReportURL = "CycleCountMasterSummary&rs:Command=Render&rc:Parameters=false" & "&MasterCountID=" & mvMasterCountID(iCnt) & "&CycleCountID:isnull=true" & "&RetailItems=1" & "&IngredientItems=1"
				
			ElseIf Me.optMasterDetail.Checked = True Then 
                sReportURL = "CycleCountMaster&rs:Command=Render&rc:Parameters=false" & "&MasterCountID=" & mvMasterCountID(iCnt) & "&CycleCountID:isnull=true" & "&RetailItems=" & chkRetail.CheckState & "&IngredientItems=" & chkIngredients.CheckState
			End If
			
			Call ReportingServicesReport(sReportURL)
			
		Next iCnt
		
	End Sub
	
    Private Sub optMasterDetail_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optMasterDetail.CheckedChanged

        If IsInitializing Then Exit Sub

        If eventSender.Checked Then

            chkIngredients.Enabled = True
            chkRetail.Enabled = True

            chkIngredients.CheckState = System.Windows.Forms.CheckState.Checked
            chkRetail.CheckState = System.Windows.Forms.CheckState.Checked

            Call SetReportButton()

        End If
    End Sub
	
    Private Sub optMasterSummary_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optMasterSummary.CheckedChanged

        If IsInitializing Then Exit Sub

        If eventSender.Checked Then

            chkIngredients.Enabled = False
            chkRetail.Enabled = False

            chkIngredients.CheckState = System.Windows.Forms.CheckState.Unchecked
            chkRetail.CheckState = System.Windows.Forms.CheckState.Unchecked

            Call SetReportButton()

        End If
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

        If Me.optMasterSummary.Checked = True Then
            Me.cmdReport.Enabled = True
        Else
            If Me.chkIngredients.Checked = True Or Me.chkRetail.Checked = True Then
                Me.cmdReport.Enabled = True
            Else
                Me.cmdReport.Enabled = False
            End If
        End If

    End Sub

End Class