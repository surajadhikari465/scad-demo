Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Friend Class frmNotAvailableReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sSubTeam As String
		Dim sWFMItems As String
		
		' GET THE SUBTEAM SELECTED
		If cboSubTeam.SelectedIndex > -1 Then
			sSubTeam = CStr(VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
		Else
			MsgBox("Sub-Team must be selected.", MsgBoxStyle.Exclamation, "Error")
			Exit Sub
		End If
		
		' GET WFM ITEMS
        sWFMItems = CStr(System.Math.Abs(chkWFMItems.CheckState))

        ReportingServicesReport(String.Format("{0}_NotAvailableItems&rs:Command=Render&rc:Parameters=False&SubTeam_No={1}&WFM_Item={2}&SubTeam_Name={3}", _
                    ConfigurationServices.AppSettings("Region"), _
                    sSubTeam, _
                    sWFMItems, _
                    cboSubTeam.Text))
	End Sub
	
	Private Sub frmNotAvailableReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		'-- Load the combos
		LoadAllSubTeams(cboSubTeam)
		
		'-- Set it to the first sub team
        If cboSubTeam.Items.Count > 0 Then cboSubTeam.SelectedIndex = -1
		
	End Sub
End Class