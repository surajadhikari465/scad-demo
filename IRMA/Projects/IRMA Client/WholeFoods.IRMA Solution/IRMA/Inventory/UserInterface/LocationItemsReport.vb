Option Strict Off
Option Explicit On
Friend Class frmLocationItemsReport
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

	Private Sub frmLocationItemsReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		CenterForm(Me)
		
		Call LoadStore(cboStore, True)
		Call LoadSubTeamsCombo()
		Call LoadLocationsCombo()
		
        Call SetEnabled()

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
        End If
	End Sub
	
    Private Sub cboStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboStore.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        Call LoadSubTeamsCombo()

        cboLocation.Items.Clear()
        cboLocation.SelectedIndex = -1

        Call SetEnabled()

    End Sub
	
    Private Sub cboSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSubTeam.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        Call LoadLocationsCombo()

        Call SetEnabled()

    End Sub
	
    Private Sub cboLocation_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboLocation.SelectedIndexChanged

        If isinitializing Then Exit Sub

        Call SetEnabled()

    End Sub
	
	Private Sub LoadSubTeamsCombo()
		
		If cboStore.SelectedIndex > -1 Then
			'Load the user's sub-teams restricted to the selected store.
			Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.Store, cboSubTeam, VB6.GetItemData(cboStore, cboStore.SelectedIndex))
		Else
			cboSubTeam.Items.Clear()
		End If
		
	End Sub
	
	Private Sub LoadLocationsCombo()
		
		If cboSubTeam.SelectedIndex > -1 Then
			'Load the user's sub-teams restricted to the selected store.
			Call LoadLocations(cboLocation, VB6.GetItemData(cboStore, cboStore.SelectedIndex), VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
		Else
			cboLocation.Items.Clear()
		End If
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sReportURL As String
		
		sReportURL = "InvLocationItems&rs:Command=Render&rc:Parameters=false" & "&InvLocID=" & VB6.GetItemData(cboLocation, cboLocation.SelectedIndex) & "&Store_Name=" & Trim(cboStore.Text) & "&SubTeam_Name=" & Trim(cboSubTeam.Text) & "&Location_Name=" & Trim(cboLocation.Text)
		
		Call ReportingServicesReport(sReportURL)
		
	End Sub
	
	Private Sub SetEnabled()
		
		If cboLocation.SelectedIndex = -1 Or Me.cboSubTeam.SelectedIndex = -1 Or Me.cboLocation.SelectedIndex = -1 Then
			cmdReport.Enabled = False
		Else
			cmdReport.Enabled = True
		End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
End Class