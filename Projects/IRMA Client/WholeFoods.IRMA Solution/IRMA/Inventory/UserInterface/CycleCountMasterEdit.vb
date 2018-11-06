Option Strict Off
Option Explicit On
Friend Class frmCycleCountMasterEdit
	Inherits System.Windows.Forms.Form
	
	Private mlMasterCountID As Integer
	Private mbChanged As Boolean
	Private mbLoading As Boolean
	Private mbSubTeamCycleCountExists As Boolean
	Private msFunction As String
	
	Private mbUserReadOnly As Boolean
	Private mbOpen As Boolean
	Private mbReqExternalCount As Boolean
	
	Private mbManufacturingSubTeam As Boolean
	Private mbPastEntryDeadline As Boolean
    Private mdEntryDeadline As Date

    Private mdt As DataTable
    Private mdv As DataView

    Private IsInitializing As Boolean

	Private Enum eCountType
		Locations = 1
		SubTeam = 2
	End Enum
	
	Private Const mcOpenText As String = "-- Master Count is OPEN --"
	Private Const mcClosedText As String = "-- Master Count is CLOSED --"
	Private Const mcNewText As String = "-- NEW Master Count --"
	
	Private Const miGridScrollBarWidth As Short = 580
	
	Public Sub LoadForm(Optional ByRef lMasterCountID As Integer = 0, Optional ByRef bReqExternalCount As Boolean = False)
		
		mlMasterCountID = lMasterCountID
		
		mbChanged = False
		mbLoading = True
		mbOpen = True
		mbReqExternalCount = bReqExternalCount
		mbPastEntryDeadline = False
		mbManufacturingSubTeam = False
		
		cboStatus.SelectedIndex = 0
		
		lblMasterStatus.Text = mcNewText
		lblCountsLocked.Text = ""
        dtpStartScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, SystemDateTime(True)))
        dtpEndScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, SystemDateTime(True)))
		
        If gbInventoryAdministrator Or gbBuyer Then
            mbUserReadOnly = False
        Else
            mbUserReadOnly = True
        End If
		
		'-- Load Store combo.
		LoadInventoryStore(cboStore)
        If glStore_Limit > 0 Then
            Call SetActive(cboStore, False)
            Call SetCombo(cboStore, glStore_Limit)
        End If
		
		Call LoadMasterAndCounts()
		
		Call SetFormReadOnly()
		
		mbLoading = False
		
		Me.ShowDialog()
		
	End Sub
	
	Private Sub LoadMasterAndCounts()
        Dim rsMaster As DAO.Recordset = Nothing

		'-- Load Cycle Count Master data if we have a record id.
		If mlMasterCountID <> 0 Then

            Try
                rsMaster = SQLOpenRecordSet("EXEC GetCycleCountMaster " & mlMasterCountID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                Call SetCombo(cboStore, Convert.ToString(rsMaster.Fields("Store_No").Value))

                Call SetActive(cboStore, False)

                Call LoadSubTeams()

                Call SetCombo(cboSubTeam, Convert.ToString(rsMaster.Fields("SubTeam_No").Value))
                Call SetActive(cboSubTeam, False)

                mbManufacturingSubTeam = rsMaster.Fields("Manufacturing").Value
                mbReqExternalCount = rsMaster.Fields("ReqExternal").Value

                'Populate the form.
                dtpEndScan.Value = rsMaster.Fields("EndScan").Value
                SetActive(dtpEndScan, False)

                chkEndOfPeriod.CheckState = IIf(rsMaster.Fields("EndOfPeriod").Value = True, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)
                chkEndOfPeriod.Enabled = False

                lblMasterStatus.Text = IIf(IsDBNull(rsMaster.Fields("ClosedDate").Value), mcOpenText, mcClosedText)

                mbOpen = IIf(IsDBNull(rsMaster.Fields("ClosedDate").Value), True, False)

                Call CheckItemEntryDeadline()

            Finally
                If rsMaster IsNot Nothing Then
                    rsMaster.Close()
                    rsMaster = Nothing
                End If
            End Try

            Call LoadCountGrid()
        Else

            Call SetButtons()
        End If

    End Sub

    Private Sub chkEndOfPeriod_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkEndOfPeriod.CheckStateChanged

        If mbLoading Or IsInitializing Then Exit Sub

        If chkEndOfPeriod.CheckState = System.Windows.Forms.CheckState.Checked Then
            'get date at 11:59 PM (add 1 day, then subtract 1 minute)
            dtpEndScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, GetEndOfPeriodDate()))
            Call SetActive(dtpEndScan, False)
        Else
            dtpEndScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, SystemDateTime(True)))
            Call SetActive(dtpEndScan, True)
        End If

        Call Changed()

        Call CheckItemEntryDeadline()

    End Sub

    Private Sub CheckItemEntryDeadline()
        '------------------------------------------------------------------------------------------------------------------------
        'Purpose:   For a End of Period count, disallow item entry if 36 hours have passed since the master's end scan date/time.
        '           ...or disallow item entry if it's an Interim count and we are passed the end scan date.
        '------------------------------------------------------------------------------------------------------------------------

        lblCountsLocked.Text = ""
        mbPastEntryDeadline = False

        mdEntryDeadline = dtpEndScan.Value

        If chkEndOfPeriod.CheckState = System.Windows.Forms.CheckState.Checked Then 'Only check this if the count is not closed and if it's an End of Period Master.
            If SystemDateTime() > DateAdd(Microsoft.VisualBasic.DateInterval.Hour, 36, mdEntryDeadline) Then
                lblCountsLocked.Text = "Count Entry is Locked"
                mbPastEntryDeadline = True
                mdEntryDeadline = DateAdd(Microsoft.VisualBasic.DateInterval.Hour, 36, mdEntryDeadline)
                Call SetButtons()
            End If
        Else
            If SystemDateTime() > mdEntryDeadline Then
                lblCountsLocked.Text = "Count Entry is Locked"
                mbPastEntryDeadline = True
                Call SetButtons()
            End If
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        Dim sTypeToAdd As String
        Dim lLocationList(0) As Long
        Dim dStartScan As Date
        sTypeToAdd = String.Empty

        'If it's already saved, do not attempt again since all fields are locked upon saving.
        If mlMasterCountID = 0 Then
            msFunction = "Add Count"
            If Not SaveValidation(False) Then
                MsgBox("Cannot add Cycle Count until Master has been saved.", MsgBoxStyle.Exclamation, "Cannot add count.")
                Dim bValid As Boolean = SaveValidation()
                Exit Sub
            Else
                If Not Save() Then Exit Sub
            End If
        End If

        'If a sub-team item already exists, then disable it from the CycleCountAddForm.
        Call frmCycleCountAddCount.LoadForm(sTypeToAdd, mbSubTeamCycleCountExists, dtpEndScan.Value, mdEntryDeadline, dStartScan)
        frmCycleCountAddCount.Dispose()

        If UCase(sTypeToAdd) = "LOCATION" Then

            'Load all Locations related to this sub-team.
            Call frmCycleCountLocationList.LoadForm((cboStore.Text), VB6.GetItemData(cboStore, cboStore.SelectedIndex), (cboSubTeam.Text), VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex), lLocationList)
            frmCycleCountLocationList.Dispose()

            Call AddLocations(lLocationList, dStartScan)

        ElseIf UCase(sTypeToAdd) = "SUBTEAM" Then

            'Adding a count for the sub-team.
            Call AddSubTeamCount(dStartScan)

        Else

            Exit Sub

        End If

        Call ClearAndRefreshGrid()

    End Sub

    Private Sub AddLocations(ByRef lLocationList() As Long, ByRef dStartScan As Date)

        Dim rsList As DAO.Recordset = Nothing
        Dim bFailed As Boolean
        Dim iLocCnt As Short
        Dim strMsg As String
        Dim strTitle As String

        On Error GoTo EmptyList

        'Walk the array and add a location for each item added.
        For iLocCnt = 0 To UBound(lLocationList) - 1

            On Error Resume Next

            rsList = SQLOpenRecordSet("EXEC  InsertCycleCountHeader " & mlMasterCountID & "," & lLocationList(iLocCnt) & ",'" & dStartScan & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If rsList.Fields("Added").Value = False Then bFailed = True

            rsList.Close()
            rsList = Nothing

        Next iLocCnt

        'Display msg if some locations were already in the list.
        If bFailed Then
            If UBound(lLocationList) > 1 Then
                strTitle = "Location(s) already in the list."
                strMsg = "Some locations were already in the list and not added again."
            Else
                strTitle = "Location already in the list."
                strMsg = "The selected location was not added since it was already in the list."
            End If

            Call MsgBox(strMsg, MsgBoxStyle.Exclamation, strTitle)
        End If

EmptyList:

    End Sub

    Private Sub AddSubTeamCount(ByRef dStartScan As Date)

        SQLExecute("EXEC InsertCycleCountHeader " & mlMasterCountID & "," & "null" & ",'" & dStartScan & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough, True)

    End Sub

    Private Sub cmdCloseCounts_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseCounts.Click
        Dim iCnt As Short
        Dim bNotClosed As Boolean
        Dim bClosed As Boolean

        bNotClosed = False
        bClosed = False

        'Close Selected Counts.
        If ugrdCounts.Selected.Rows.Count > 0 Then

            If MsgBox("Close selected count(s)?", MsgBoxStyle.YesNo, "Close Cycle Counts") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iCnt = 0 To ugrdCounts.Selected.Rows.Count - 1

                If Trim(ugrdCounts.Selected.Rows(iCnt).Cells("Status").Value.ToString()) = "OPEN" Then
                    If Trim(ugrdCounts.Selected.Rows(iCnt).Cells("Start Scan").Value.ToString()) = "" Then
                        bNotClosed = True
                    Else
                        SQLExecute("EXEC CloseCycleCountHeader " & ugrdCounts.Selected.Rows(iCnt).Cells("CountID").Value.ToString() & ",'" & SystemDateTime() & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        bClosed = True
                    End If
                End If

            Next iCnt

            If bNotClosed = True Then
                Call MsgBox("Counts without a Start Scan date/time cannot be closed.", MsgBoxStyle.Exclamation, "Could not close selected count(s)")
            End If

            If bClosed = True Then
                '-- Refresh the grid
                Call LoadCountGrid()
            End If

        Else

            'Shouldn't happen, but just in case.
            MsgBox("You must first select a Count to close.", MsgBoxStyle.Exclamation, "Notice!")

        End If

    End Sub

    Private Sub cmdCloseMaster_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseMaster.Click

        msFunction = "Close"
        If Not SaveValidation() Then Exit Sub

        'Check to make sure cycle counts exits.
        If ugrdCounts.Rows.Count = 0 Then
            Call MsgBox("Master cannot be closed since no counts exist.", MsgBoxStyle.Exclamation, "Cannot close master!")
            Exit Sub
        End If

        'First see if ext count is required.
        If mbReqExternalCount Then
            If ExternalCountMissing(mlMasterCountID) Then
                Call MsgBox("This Master is for a sub-team that requires an 'external' cycle count." & Chr(13) & "It cannot be closed until the external count has been created.", MsgBoxStyle.Exclamation, "Selected master requires an 'external' cycle count!")
                Exit Sub
            End If
        End If

        'Check to make sure all cycle counts are closed before allowing the master to be closed.
        If Not AllCountsClosed(mlMasterCountID) Then
            Call MsgBox("Cannot close Master until all of it's cycle counts are closed.", MsgBoxStyle.Exclamation, "Cannot close master!")
            Exit Sub
        End If

        If frmCycleCountMasterClose.LoadForm(mlMasterCountID) Then
            mbOpen = False
            lblMasterStatus.Text = mcClosedText
            Call CheckItemEntryDeadline()
            Call SetFormReadOnly()
        End If
        frmCycleCountMasterClose.Dispose()

    End Sub

    Private Sub cmdCycleCountReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCycleCountReport.Click

        Dim iCnt As Short
        Dim lCycleCountID(0) As Long

        'Build an array of selected masters.
        If ugrdCounts.Selected.Rows.Count > 0 Then
            For iCnt = 0 To ugrdCounts.Selected.Rows.Count - 1
                ReDim Preserve lCycleCountID(iCnt)
                lCycleCountID(iCnt) = ugrdCounts.Selected.Rows(iCnt).Cells("CountID").Value
            Next iCnt
        End If

        Call frmCycleCountReport.LoadForm(mlMasterCountID, lCycleCountID)
        frmCycleCountReport.Dispose()

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        Dim iDelCnt As Short

        If ugrdCounts.Selected.Rows.Count > 0 Then

            '-- Make sure they really want to delete the Cycle Count.
            If MsgBox("Delete the selected Cycle Count(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Cycle Count(s)") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdCounts.Selected.Rows.Count - 1

                'Do not allow them to delete an "External" count (EOP or Interim).
                If ugrdCounts.Selected.Rows(iDelCnt).Cells("External").Value = False Then
                    SQLExecute("EXEC DeleteCycleCount " & ugrdCounts.Selected.Rows(iDelCnt).Cells("CountID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Else
                    Call MsgBox("The 'External' count cannot be deleted.", MsgBoxStyle.Exclamation, "Deleting count(s)...")
                End If

            Next iDelCnt

            '-- Refresh the grid
            Call LoadCountGrid()

        Else

            'Shouldn't happen, but just in case.
            MsgBox("You must first select a Count to delete.", MsgBoxStyle.Exclamation, "Notice!")

        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEdit.Click

        Dim lInvLocID As Integer

        lInvLocID = 0

        If Trim(ugrdCounts.Selected.Rows(0).Cells("InvLocID").Value.ToString()) <> "" Then
            lInvLocID = CInt(ugrdCounts.Selected.Rows(0).Cells("InvLocID").Value)
        End If

        Call frmCycleCountEdit.LoadForm((ugrdCounts.Selected.Rows(0).Cells("Name").Value), _
                                                            (ugrdCounts.Selected.Rows(0).Cells("Type").Value), _
                                                            mlMasterCountID, (ugrdCounts.Selected.Rows(0).Cells("CountID").Value), _
                                                            VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex), _
                                                            (ugrdCounts.Selected.Rows(0).Cells("Start Scan").Value), _
                                                            (dtpEndScan.Text), _
                                                            IIf(ugrdCounts.Selected.Rows(0).Cells("Status").Value = "OPEN", True, False), _
                                                            CBool(ugrdCounts.Selected.Rows(0).Cells("External").Value), _
                                                            (chkEndOfPeriod.CheckState), _
                                                            mbManufacturingSubTeam, _
                                                            lInvLocID)
        frmCycleCountEdit.Close()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Dim iAns As Short

        If mbChanged Then
            iAns = MsgBox("Save changes before closing?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data changed!")
            Select Case iAns
                Case MsgBoxResult.Yes
                    msFunction = "Save"
                    If Not Save() Then Exit Sub
                Case MsgBoxResult.Cancel
                    Exit Sub
            End Select
        End If

        Me.Close()

    End Sub

    Private Sub cmdMasterReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMasterReport.Click

        Dim MasterCountID(0) As Long

        MasterCountID(0) = mlMasterCountID

        Call frmCycleCountMasterReports.LoadForm(MasterCountID)
        frmCycleCountMasterReports.Dispose()

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click

        Call LoadCountGrid()

    End Sub

    Private Sub frmCycleCountMasterEdit_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 13 Then 'Shift+Enter.

            Call ClearAndRefreshGrid()

        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub ClearAndRefreshGrid()

        txtCountName.Text = CStr(Nothing)
        dtpStartScan.Checked = False
        dtpStartScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, SystemDateTime(True)))
        cboStatus.SelectedIndex = 0

        Call LoadCountGrid()

    End Sub

    Private Sub frmCycleCountMasterEdit_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        mlMasterCountID = 0
        mbSubTeamCycleCountExists = False
        mdEntryDeadline = System.DateTime.MinValue

    End Sub

    Private Sub LoadCountGrid()

        Dim rsList As DAO.Recordset = Nothing
        Dim sCountName As String
        Dim sStartScan As String
        Dim sStatus As String
        Dim sSQL As String
        sCountName = String.Empty
        sStartScan = String.Empty
        sStatus = String.Empty

        mbSubTeamCycleCountExists = False

        sCountName = "null"

        Select Case cboStatus.SelectedIndex
            Case 0 : sStatus = "null" 'Return all.
            Case 1 : sStatus = "'OPEN'"
            Case 2 : sStatus = "'CLOSED'"
        End Select

        If Trim(txtCountName.Text) <> vbNullString Then
            sCountName = "'" & Trim(txtCountName.Text) & "'"
        End If

        If dtpStartScan.Checked Then
            sStartScan = "'" & dtpStartScan.Text & "'"
        Else
            sStartScan = "null"
        End If

        sSQL = "EXEC GetCycleCountList " & mlMasterCountID & "," & sCountName & "," & sStartScan & "," & sStatus

        Call SetupDataTable()
        Call LoadDataTable(sSQL)

        Call SetButtons()

    End Sub

    Private Sub LoadSubTeams()

        '-- Limit sub-teams user-assigned and only those related to the selected location.
        If cboStore.SelectedIndex > -1 Then
            'Load the user's sub-teams restricted to the selected store.
            Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.StoreUser, cboSubTeam, VB6.GetItemData(cboStore, cboStore.SelectedIndex), 0)
        Else
            cboSubTeam.Items.Clear()
        End If

    End Sub

    Private Sub cboStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboStore.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboStore.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboSubTeam.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboSubTeam.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboStore.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        Call LoadSubTeams()
        Call Changed()

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click

        msFunction = "Save"
        Call Save()

    End Sub

    Private Function Save() As Boolean
        Dim rsMaster As DAO.Recordset = Nothing

        Save = False

        If mbChanged Then

            '-- First validate the form's data.
            If Not SaveValidation() Then Exit Function

            '-- Check to see if there is already a master count open for this store and sub-team.
            If OpenMasterExists() Then Exit Function

            '-- Check to see if the location already exists.
            If DupeExists() Then Exit Function

            Try
                '--Update the record.
                rsMaster = SQLOpenRecordSet("EXEC UpdateCycleCountMaster " & IIf(mlMasterCountID = 0, "null", mlMasterCountID) & "," & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & "," & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex) & ",'" & dtpEndScan.Text & "'," & "null,'" & chkEndOfPeriod.CheckState & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                mlMasterCountID = rsMaster.Fields("MasterCountID").Value
                mbManufacturingSubTeam = rsMaster.Fields("Manufacturing").Value
                mbReqExternalCount = rsMaster.Fields("ReqExternal").Value

                lblMasterStatus.Text = mcOpenText

                Save = True

                'Lock the store and sub-team after a save is performed. They can't be changing this after entering items.
                Call SetActive(cboStore, False)
                Call SetActive(cboSubTeam, False)

                SetActive(dtpEndScan, False)
                chkEndOfPeriod.Enabled = False

                Call CheckItemEntryDeadline()

                Call Changed(False)

                Call SetButtons()
            Finally
                If rsMaster IsNot Nothing Then
                    rsMaster.Close()
                    rsMaster = Nothing
                End If
            End Try

        End If

    End Function

    Private Function DupeExists() As Boolean

        Dim rsMaster As DAO.Recordset = Nothing

        DupeExists = False

        Try
            rsMaster = SQLOpenRecordSet("EXEC CheckForDuplicateCycleCountMaster " & mlMasterCountID & ", " & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & ", " & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex) & ", '" & dtpEndScan.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If rsMaster.Fields("Found").Value > 0 Then
                Call MsgBox("A Master Cycle Count already exists for this store, sub-team, and end-scan date.", MsgBoxStyle.Exclamation, "Cannot " & msFunction & ".")
                cboStore.Focus()
                DupeExists = True
            End If

        Finally
            If rsMaster IsNot Nothing Then
                rsMaster.Close()
                rsMaster = Nothing
            End If
        End Try

    End Function

    Private Function OpenMasterExists() As Boolean

        Dim rsMaster As DAO.Recordset = Nothing

        OpenMasterExists = False

        Try
            rsMaster = SQLOpenRecordSet("EXEC CheckForOpenCycleCountMaster " & mlMasterCountID & ", " & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & ", " & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If rsMaster.Fields("Found").Value > 0 Then
                Call MsgBox("An open Cycle Count Master already exists for this store and sub-team.", MsgBoxStyle.Exclamation, "Cannot " & msFunction & ".")
                cboStore.Focus()
                OpenMasterExists = True
            End If

        Finally
            If rsMaster IsNot Nothing Then
                rsMaster.Close()
                rsMaster = Nothing
            End If
        End Try

    End Function

    Private Function SaveValidation(Optional ByRef bMsg As Boolean = True) As Boolean

        Dim bInvalidDate As Boolean = False

        SaveValidation = True

        'Only test this if the record hasn't been saved before.
        If mlMasterCountID <> 0 Then Exit Function

        'Test Required fields.
        '-------------------------
        If cboStore.SelectedIndex = -1 OrElse cboSubTeam.SelectedIndex = -1 Then

            SaveValidation = False

            If bMsg Then
                MsgBox("Store, Sub-Team and End Scan are required.", MsgBoxStyle.Exclamation, "Cannot " & msFunction & ".")
                If cboStore.SelectedIndex = -1 Then
                    cboStore.Focus()
                ElseIf cboSubTeam.SelectedIndex = -1 Then
                    cboSubTeam.Focus()
                End If
            End If

            Exit Function
        End If

        If dtpEndScan.Value < SystemDateTime() Then
            If bMsg Then
                MsgBox("End Scan date/time cannot be prior to the current date/time.", MsgBoxStyle.Exclamation, "Cannot " & msFunction & ".")
                dtpEndScan.Focus()
            End If
            SaveValidation = False
        End If

    End Function

    Private Sub cboSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSubTeam.SelectedIndexChanged

        If mbLoading Or IsInitializing Then Exit Sub

        Call Changed()

    End Sub

    Private Sub Changed(Optional ByRef bChanged As Boolean = True)

        If mbLoading Or mbUserReadOnly Then Exit Sub

        If bChanged = True Then
            mbChanged = True
        Else
            mbChanged = False
        End If

        Call SetButtons()

    End Sub

    Private Sub SetButtons()

        'Set Add button.
        cmdAdd.Enabled = False
        If Not mbUserReadOnly And mbOpen And Not mbPastEntryDeadline Then
            cmdAdd.Enabled = True
        End If

        'Set Edit button.
        cmdEdit.Enabled = False
        If ugrdCounts.Selected.Rows.Count = 1 Then
            Me.cmdEdit.Enabled = True
        End If

        'Set Delete button.
        cmdDelete.Enabled = False
        If ugrdCounts.Selected.Rows.Count > 0 Then
            If Not mbUserReadOnly And mbOpen And Not mbPastEntryDeadline Then
                cmdDelete.Enabled = True
            End If
        End If

        'Set Cycle Count Report button.
        cmdCycleCountReport.Enabled = False
        If ugrdCounts.Selected.Rows.Count > 0 Then
            cmdCycleCountReport.Enabled = True
        End If

        'Set Master Close button.
        cmdCloseMaster.Enabled = False
        If mbOpen And gbInventoryAdministrator Then
            cmdCloseMaster.Enabled = True
        End If

        'Set Closed button.
        cmdCloseCounts.Enabled = False
        If ugrdCounts.Selected.Rows.Count > 0 Then
            If AllSelectedCountsOpen() Then
                cmdCloseCounts.Enabled = True
            End If
        End If

        'Set the Apply button.
        cmdApply.Enabled = False
        If mbChanged Then
            cmdApply.Enabled = True
        End If

    End Sub

    Private Function AllSelectedCountsOpen() As Boolean

        Dim iCnt As Short

        AllSelectedCountsOpen = True

        If ugrdCounts.Selected.Rows.Count > 0 Then
            For iCnt = 0 To ugrdCounts.Selected.Rows.Count - 1

                If ugrdCounts.Selected.Rows(iCnt).Cells("Status").Value.ToString() = "CLOSED" Then
                    AllSelectedCountsOpen = False
                    Exit Function
                End If

            Next iCnt
        End If

    End Function

    Private Sub txtCountName_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtCountName.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtCountName.Tag), txtCountName, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub SetFormReadOnly()
        '----------------------------------------------------
        'Purpose: Lock up the form if it should be read only
        '----------------------------------------------------

        If mbUserReadOnly Or Not mbOpen Then
            SetActive(dtpEndScan, False)
            SetActive(dtpEndScan, False)
            chkEndOfPeriod.Enabled = False
            cmdCloseMaster.Enabled = False
            cmdAdd.Enabled = False
            cmdDelete.Enabled = False
            cmdApply.Enabled = False
        End If

        Call SetButtons()

    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("CountList")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Type", GetType(String)))
        mdt.Columns.Add(New DataColumn("Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Start Scan", GetType(String)))
        mdt.Columns.Add(New DataColumn("Status", GetType(String)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("CountID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("External", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("InvLocID", GetType(Integer)))


    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        Dim rsList As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000
        Dim dt As DateTime

        Try
            rsList = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsList.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("CountID") = rsList.Fields("CycleCountID").Value
                row("External") = rsList.Fields("External").Value
                row("InvLocID") = rsList.Fields("InvLoc_ID").Value
                row("Type") = IIf(rsList.Fields("External").Value = True, "External", IIf(IsDBNull(rsList.Fields("InvLoc_Name").Value), "SubTeam", "Location"))
                row("Name") = IIf(IsDBNull(rsList.Fields("InvLoc_Name").Value), rsList.Fields("SubTeam_Name").Value, rsList.Fields("InvLoc_Name").Value)
                dt = Convert.ToDateTime(rsList.Fields("StartScan").Value)
                row("Start Scan") = dt.ToString("MM/dd/yy HH:mm")
                row("Status") = IIf(IsDBNull(rsList.Fields("ClosedDate").Value), "OPEN", "CLOSED")
                mdt.Rows.Add(row)

                'Determine if a cycle count exists for a subteam.
                If mbSubTeamCycleCountExists = False Then
                    mbSubTeamCycleCountExists = IIf(IsDBNull(rsList.Fields("InvLoc_Name").Value) And rsList.Fields("External").Value = False, True, False)
                End If

                rsList.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()

            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Name"
            ugrdCounts.DataSource = mdv

            If ugrdCounts.Rows.Count > 0 Then
                ugrdCounts.Rows(0).Selected = True
            End If

        Finally
            If rsList IsNot Nothing Then
                rsList.Close()
                rsList = Nothing
            End If
        End Try

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub ugrdCounts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdCounts.Click

        Call SetButtons()

    End Sub

    Private Sub ugrdCounts_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdCounts.DoubleClickRow

        If cmdEdit.Enabled Then
            cmdEdit.PerformClick()
        End If

    End Sub

    Private Sub dtpEndScan_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpEndScan.ValueChanged

        If mbLoading Or IsInitializing Then Exit Sub

        Call Changed()

    End Sub
End Class