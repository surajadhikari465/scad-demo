Option Strict Off
Option Explicit On

Imports System.Text
Imports WholeFoods.Utility

Friend Class frmInventoryBalanceReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim reportUrlBuilder As StringBuilder = New StringBuilder()

        ' GET THE WAREHOUSE SELECTED
		If cboWarehouse.SelectedIndex = -1 Then
			MsgBox("Warehouse must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
		End If
		
		' GET THE SUBTEAM SELECTED
		If cboSubTeam.SelectedIndex = -1 Then
			MsgBox("Sub-Team must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
        End If

        reportUrlBuilder.AppendFormat("InventoryBalance&rs:Command=Render&rc:Parameters=False")
        'reportUrlBuilder.AppendFormat("&Store_No={0}", VB6.GetItemData(cboStore, cboStore.SelectedIndex))
        reportUrlBuilder.AppendFormat("&Warehouse_ID={0}", VB6.GetItemData(cboWarehouse, cboWarehouse.SelectedIndex))
        reportUrlBuilder.AppendFormat("&Subteam_No={0}", VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
        ReportingServicesReport(reportUrlBuilder.ToString())
		
	End Sub
	
	Private Sub frmInventoryBalanceReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form
		CenterForm(Me)

		LoadInventoryStore(cboWarehouse)
        cboWarehouse.SelectedIndex = -1
		
		LoadAllSubTeams(cboSubTeam)
        cboSubTeam.SelectedIndex = -1
		
	End Sub
End Class