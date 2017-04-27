Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Friend Class frmCasesByDepartmentAudit
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
		
		If cmbSubTeam.SelectedIndex = -1 Then
			MsgBox("The Sub-Team must be entered.", MsgBoxStyle.Exclamation, "Invalid Entry")
			cmbSubTeam.Focus()
			Exit Sub
		End If

        ReportingServicesReport(String.Format("{0}_CasesByDepartmentAudit&rs:Command=Render&rc:Parameters=False&Store_No={1}&SubTeam_No={2}&Store_Name={3}&SubTeam_Name={4}", _
                    ConfigurationServices.AppSettings("Region"), _
                    VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), _
                    VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex), _
                    cmbStore.Text, _
                    cmbSubTeam.Text))

        'crwReport.ReportTitle = cmbStore.Text & " Cases By Sub-Team Audit Report " & vbCrLf & " For " & cmbSubTeam.Text
        
	End Sub
	
	Private Sub frmCasesByDepartmentAudit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		'-- Load the combo boxes
        LoadInventoryStore(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If

		LoadAllSubTeams(cmbSubTeam)
        If cmbSubTeam.Items.Count > 0 Then cmbSubTeam.SelectedIndex = -1
		
	End Sub
End Class