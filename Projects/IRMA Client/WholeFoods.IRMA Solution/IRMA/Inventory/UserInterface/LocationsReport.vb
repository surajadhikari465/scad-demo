Option Strict Off
Option Explicit On
Friend Class frmLocationsReport
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

    Private Sub cboStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboStore.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        Call SetEnabled()

    End Sub
	
	Private Sub frmLocationsReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		CenterForm(Me)
		
		Call LoadStore(cboStore, True)
		
        cboStore.Items.Add(("All Stores"))

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = 0
        End If
		
		Call SetEnabled()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
		Dim sReportURL As String
		Dim sStoreNo As String
		
		sStoreNo = CStr(VB6.GetItemData(cboStore, cboStore.SelectedIndex))
		
		sReportURL = "InvLocations&rs:Command=Render&rc:Parameters=false&Store_No" & IIf(sStoreNo = "0", ":isnull=true", "=" & sStoreNo)
		
		Call ReportingServicesReport(sReportURL)
		
	End Sub
	
	Private Sub SetEnabled()
		
		If cboStore.SelectedIndex > -1 Then
			Me.cmdReport.Enabled = True
		Else
			Me.cmdReport.Enabled = False
		End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
End Class