Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Friend Class frmCasesByDepartment
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        If cmbStore.SelectedIndex = -1 Then
            MsgBox("The Store must be entered.", MsgBoxStyle.Exclamation, "Invalid Entry")
            cmbStore.Focus()
            Exit Sub
        End If

        ReportingServicesReport(String.Format("{0}_CasesByDepartment&rs:Command=Render&rc:Parameters=False&Store_No={1}&Store_Name={2}", _
                    ConfigurationServices.AppSettings("Region"), _
                    VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), _
                    cmbStore.Text))
        'crwReport.ReportTitle = cmbStore.Text & " Cases By Sub-Team Report"
	End Sub
	
	Private Sub frmCasesByDepartment_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)

        LoadInventoryStore(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
    End Sub
End Class