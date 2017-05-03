Option Strict Off
Option Explicit On
Friend Class frmCycleCountAddCount
	Inherits System.Windows.Forms.Form
	
	Private msTypeToAdd As String
	Private mbSubTeamExists As Boolean
	Private mdMasterEndScan As Date
	Private mdEntryDeadline As Date
    Private msStartScan As String

    Private IsInitializing As Boolean
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		If ValidateDate Then
			
			If optLocation.Checked = True Then
				msTypeToAdd = "LOCATION"
			Else
				msTypeToAdd = "SUBTEAM"
			End If
			
			Me.Close()
			
		End If
		
	End Sub
	
	Public Sub LoadForm(ByRef sTypeToAdd As String, ByRef bSubTeamExists As Boolean, ByRef dMasterEndScan As Date, ByRef dEntryDeadline As Date, ByRef dStartScan As Date)
		
		msTypeToAdd = "CANCEL"
		
		mdEntryDeadline = dEntryDeadline
		mdMasterEndScan = dMasterEndScan
		
		If bSubTeamExists Then
			optLocation.Checked = True
			optSubTeam.Enabled = False
		Else
			optSubTeam.Enabled = True
			optSubTeam.Enabled = True
		End If
		
		Me.ShowDialog()
		
		sTypeToAdd = msTypeToAdd
		If msStartScan <> "" Then dStartScan = CDate(msStartScan)
		
	End Sub
	
    Private Function ValidateDate() As Boolean

        ValidateDate = True

        If Trim(txtStartScan.Text) = "" Or (Trim(txtStartScan.Text) <> "" And Len(Trim(txtStartScan.Text)) <> 16) Then
            MsgBox("Valid Start Scan date/time must be entered.", MsgBoxStyle.Exclamation, "Cannot Create Count.")
            txtStartScan.Focus()
            ValidateDate = False
            Exit Function
        End If

        'Start scan must be before the end scan of the master.
        If CDate(txtStartScan.Text) < SystemDateTime() Or CDate(txtStartScan.Text) >= CDate(mdEntryDeadline) Then
            MsgBox("Start Scan date/time must be between the current date/time of " & SystemDateTime() & " and the Master End Scan date/time of " & mdMasterEndScan & ".", MsgBoxStyle.Exclamation, "Cannot Create Count.")
            txtStartScan.Focus()
            ValidateDate = False
            Exit Function
        End If

    End Function
	
    Private Sub txtStartScan_ValueChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtStartScan.ValueChanged

        If IsInitializing Then Exit Sub

        msStartScan = txtStartScan.Value

    End Sub
	
    Private Sub txtStartScan_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
End Class