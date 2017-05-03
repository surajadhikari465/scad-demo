Option Strict Off
Option Explicit On
Friend Class frmAvgCostUpdate
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	
    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        If cmbStore.SelectedIndex > -1 Then
            cmdClose.Enabled = True
        Else
            cmdClose.Enabled = False
        End If

    End Sub
	
	
	Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
		
		On Error GoTo me_err
		
		SQLExecute2("EXEC UpdateAverageCost " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		
		MsgBox("Average Cost Update completed", MsgBoxStyle.Information, Me.Text)
		
		Exit Sub
		
me_err: 
		MsgBox("Average Cost Update failed: " & Err.Description, MsgBoxStyle.Critical, Me.Text)
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub frmAvgCostUpdate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		'-- Load Stores
		LoadStores(cmbStore)
		
	End Sub
End Class