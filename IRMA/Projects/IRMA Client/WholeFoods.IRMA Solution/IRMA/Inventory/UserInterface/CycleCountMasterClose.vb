Option Strict Off
Option Explicit On
Friend Class frmCycleCountMasterClose
	Inherits System.Windows.Forms.Form
	
	Private mbClosed As Boolean
    Private mlMasterCountID As Integer

    Private IsInitializing As Boolean
	
    Private Sub chkResetInvCnt_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkResetInvCnt.CheckStateChanged

        If IsInitializing Then Exit Sub

        If chkResetInvCnt.CheckState = System.Windows.Forms.CheckState.Checked Then
            Me.chkZeroItems.Enabled = True
        Else
            Me.chkZeroItems.Enabled = False
        End If

    End Sub
	
	Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
		
		SQLExecute("EXEC UpdateCycleCountMasterClosed " & mlMasterCountID & ",'" & SystemDateTime & "'," & Me.chkResetInvCnt.CheckState & "," & Me.chkZeroItems.CheckState, dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		
		mbClosed = True
		
		Me.Close()
		
	End Sub
	
	Public Function LoadForm(ByRef lMasterCountID As Integer) As Boolean
		
		mbClosed = False
		mlMasterCountID = lMasterCountID
		
		Me.ShowDialog()
		
		LoadForm = mbClosed
		
	End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
End Class