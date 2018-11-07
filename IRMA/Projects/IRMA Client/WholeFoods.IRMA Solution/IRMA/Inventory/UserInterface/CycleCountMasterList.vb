Option Strict Off
Option Explicit On

Imports WholeFoods.Utility.DataAccess

Friend Class frmCycleCountMasterList
	Inherits System.Windows.Forms.Form

    Private factory As DataFactory

    Private mbReadOnly As Boolean

    Private mdt As DataTable
    Private mdv As DataView

	Private Sub frmCycleCountMasterList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		mbReadOnly = True
        If gbInventoryAdministrator Then mbReadOnly = False
		
		'-- Load store(s).
		Call LoadStore(cboStore, True)
        If glStore_Limit > 0 Then
            'If the user is restricted to a single store, lock the combo box to that store.
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        End If
		
		cboStatus.SelectedIndex = 0
		cboType.SelectedIndex = 0

        dtpEndScan.CustomFormat = gsUG_DateMask + " HH:mm"
        dtpEndScan.Value = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, SystemDateTime(True)))

        Call RefreshGrid()
        Call SetReadOnly()

        ' Set permissions for new cmdGenerateRecs button per Lawrence Priest
        ' (Different business rules than the other read-only buttons on this form)
        If gbWarehouse And (gbInventoryAdministrator Or gbAccountant) Then
            cmdGenerateRecs.Enabled = True
        ElseIf gbSuperUser Then
            cmdGenerateRecs.Enabled = True
        Else
            cmdGenerateRecs.Enabled = False
        End If

    End Sub

    Private Sub SetReadOnly()

        If mbReadOnly Then
            cmdAddMaster.Enabled = False
            cmdDeleteMaster.Enabled = False
            cmdCloseMaster.Enabled = False
        End If

    End Sub

    Private Sub cmdGenerateRecs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGenerateRecs.Click

        If factory Is Nothing Then
            factory = New DataFactory(DataFactory.ItemCatalog)
        End If
        Dim dt As DataTable = factory.GetStoredProcedureDataTable("dbo.ImportCycleCount")
        RefreshGrid()

       

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Dim iCnt As Short
        Dim MasterCountID(0) As Long

        'Build an array of selected masters.
        If ugrdMaster.Selected.Rows.Count > 0 Then
            For iCnt = 0 To ugrdMaster.Selected.Rows.Count - 1
                ReDim Preserve MasterCountID(iCnt)
                MasterCountID(iCnt) = ugrdMaster.Selected.Rows(iCnt).Cells("MasterCountID").Value
            Next iCnt
        End If

        Call frmCycleCountMasterReports.LoadForm(MasterCountID)
        frmCycleCountMasterReports.Close()
        frmCycleCountMasterReports.Dispose()

    End Sub

    Private Sub cmdAddMaster_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddMaster.Click

        Call frmCycleCountMasterEdit.LoadForm()
        frmCycleCountMasterEdit.Close()
        frmCycleCountMasterEdit.Dispose()
        Call ClearAndRefreshGrid()

    End Sub
	
	Private Sub cmdCloseMaster_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseMaster.Click
		
		'Check to make sure all cycle counts are closed before allowing the master to be closed.
        If ugrdMaster.Selected.Rows(0).Cells("SubCounts").Value() = 0 Then
            Call MsgBox("This Master does not contain any counts and cannot be closed.", MsgBoxStyle.Exclamation, "Selected master contains no counts!")
            Exit Sub
        End If

        If ugrdMaster.Selected.Rows(0).Cells("ReqExternalCount").Value Then
            If ExternalCountMissing(ugrdMaster.Selected.Rows(0).Cells("MasterCountID").Value) Then
                Call MsgBox("This Master is for a sub-team that requires an 'external' cycle count." & Chr(13) & "It cannot be closed until the external count has been created.", MsgBoxStyle.Exclamation, "Selected master requires an 'external' cycle count!")
                Exit Sub
            End If
        End If

        If Not AllCountsClosed(ugrdMaster.Selected.Rows(0).Cells("MasterCountID").Value) Then
            Call MsgBox("Cannot close Master until all of it's cycle counts are closed.", MsgBoxStyle.Exclamation, "Selected master contains open cycle counts!")
            Exit Sub
        End If

        Call frmCycleCountMasterClose.LoadForm(ugrdMaster.Selected.Rows(0).Cells("MasterCountID").Value)
        frmCycleCountMasterClose.Close()
        frmCycleCountMasterClose.Dispose()

        Call RefreshGrid()
		
	End Sub
	
	Private Sub cmdDeleteMaster_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteMaster.Click
		Dim iDelCnt As Short
		
        If ugrdMaster.Selected.Rows.Count > 0 Then
            '-- Make sure they really want to delete the Master.
            If MsgBox("Delete the selected Cycle Count Master(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete cycle count master(s)") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdMaster.Selected.Rows.Count - 1
                SQLExecute("EXEC DeleteCycleCountMaster " & ugrdMaster.Selected.Rows(iDelCnt).Cells("MasterCountID").Value.ToString(), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next iDelCnt
            '-- Refresh the grid
            RefreshGrid()
        Else
            'Shouldn't happen, but just in case.
            MsgBox("You must first select a Master to delete.", MsgBoxStyle.Exclamation, "Notice!")
        End If
		
	End Sub
	
	Private Sub cmdEditMaster_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditMaster.Click
        If ugrdMaster.Selected.Rows.Count = 1 Then
            '-- Show the edit screen
            Call frmCycleCountMasterEdit.LoadForm(ugrdMaster.Selected.Rows(0).Cells("MasterCountID").Value, ugrdMaster.Selected.Rows(0).Cells("ReqExternalCount").Value)
            frmCycleCountMasterEdit.Close()
            Call RefreshGrid()
        Else
            MsgBox("Please select a line to edit.", MsgBoxStyle.Exclamation, "Notice!")
        End If
    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Call RefreshGrid()
    End Sub
	
    Private Sub gridMaster_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Call SetButtons()
    End Sub
	
    Private Sub gridMaster_DblClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        If cmdEditMaster.Enabled Then
            cmdEditMaster.PerformClick()
        End If
    End Sub
	
	Private Sub RefreshGrid()
        Dim rsList As DAO.Recordset = Nothing
        Dim sStoreID As String
        Dim sSubTeamID As String
		Dim sStatus As String
		Dim sType As String
        Dim sEndScan As String
        Dim sSQL As String
        sStatus = String.Empty
        sType = String.Empty
        sEndScan = String.Empty

        sStoreID = "null"
        sSubTeamID = "null"

        If cboStore.SelectedIndex > -1 Then sStoreID = VB6.GetItemData(cboStore, cboStore.SelectedIndex).ToString
        If cboSubTeam.SelectedIndex > -1 Then sSubTeamID = VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex).ToString
		
		Select Case cboStatus.SelectedIndex
			Case 0 : sStatus = "null" 'Return all.
			Case 1 : sStatus = "'OPEN'"
			Case 2 : sStatus = "'CLOSED'"
		End Select
		
		Select Case cboType.SelectedIndex
			Case 0 : sType = "null" 'Return all.
			Case 1 : sType = "'EOP'"
			Case 2 : sType = "'INTERIM'"
		End Select
		
        If dtpEndScan.Checked Then
            sEndScan = "'" & dtpEndScan.Value.ToString("yyyy-MM-dd HH:mm") & "'"
        Else
            sEndScan = "null"
        End If

        sSQL = "EXEC GetCycleCountMasterList " & sStoreID & ", " & sSubTeamID & ", " & sStatus & ", " & sType & ", " & sEndScan

        Call SetupDataTable()
        Call LoadDataTable(sSQL)

		Call SetButtons()
		
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
		
		If KeyAscii = 8 Then cboSubTeam.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
    End Sub

    Private Sub cboStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboStore.SelectedIndexChanged
        Call LoadSubTeams()
    End Sub
	
	Private Sub frmCycleCountMasterList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
		
        If cboStore.Enabled Then
            cboStore.SelectedIndex = -1
        End If
		
		'-- Clear Search Criteria
		cboSubTeam.SelectedIndex = -1
        dtpEndScan.Checked = False
		cboStatus.SelectedIndex = 0
		cboType.SelectedIndex = 0
		
		Call RefreshGrid()
		
	End Sub
	
	Private Sub SetButtons()
		
        If ugrdMaster.Selected.Rows.Count > 0 Then
            cmdDeleteMaster.Enabled = IIf(mbReadOnly, False, True)
            cmdReport.Enabled = IIf(mbReadOnly, False, True)
        Else
            cmdDeleteMaster.Enabled = False
            cmdReport.Enabled = False
        End If
		
        If ugrdMaster.Selected.Rows.Count = 1 Then
            Me.cmdEditMaster.Enabled = True
            If UCase(ugrdMaster.Selected.Rows(0).Cells("Status").Value) = "OPEN" Then
                Me.cmdCloseMaster.Enabled = IIf(mbReadOnly, False, True)
            Else
                Me.cmdCloseMaster.Enabled = False
            End If
        Else
            Me.cmdEditMaster.Enabled = False
            Me.cmdCloseMaster.Enabled = False
        End If

    End Sub
	
	Private Sub LoadSubTeams()
		
        '-- Limit sub-teams user-assigned and only those related to the selected location.
		If cboStore.SelectedIndex > -1 Then
			'Load the user's sub-teams restricted to the selected store.
            'Took out passing the array for "unrestricted"... during conversion to .Net.
            Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.StoreUser, cboSubTeam, VB6.GetItemData(cboStore, cboStore.SelectedIndex), 0)
        Else
            cboSubTeam.Items.Clear()
		End If
		
	End Sub

    Private Sub gridMastert_Click()
        Call SetButtons()
    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("MasterList")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Sub Team Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("End Scan", GetType(String)))
        mdt.Columns.Add(New DataColumn("Status", GetType(String)))
        mdt.Columns.Add(New DataColumn("Type", GetType(String)))


        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("MasterCountID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("ReqExternalCount", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("SubCounts", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("BOHFileDate", GetType(DateTime)))

    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000
        Dim dt As DateTime
        Dim dBohFileDate As String = Now().ToString()

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("MasterCountID") = rsSearch.Fields("MasterCountID").Value
                row("ReqExternalCount") = rsSearch.Fields("ReqExternal").Value
                row("SubCounts") = rsSearch.Fields("SubCounts").Value
                row("Store Name") = rsSearch.Fields("Store_Name").Value
                row("Sub Team Name") = rsSearch.Fields("SubTeam_Name").Value
                dt = Convert.ToDateTime(rsSearch.Fields("EndScan").Value)
                row("End Scan") = dt.ToString("yyyy/MM/dd HH:mm")
                dBohFileDate = IIf(rsSearch.Fields("BOHFileDate").Value Is DBNull.Value, String.Empty, rsSearch.Fields("BOHFileDate").Value.ToString())
                row("BOHFileDate") = IIf(dBohFileDate.Equals(String.Empty), DBNull.Value, dBohFileDate)
                row("Status") = IIf(IsDBNull(rsSearch.Fields("ClosedDate").Value), "OPEN", "CLOSED")
                row("Type") = IIf(rsSearch.Fields("EndOfPeriod").Value = 0, "Interim", "EOP")
                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Store Name"
            ugrdMaster.DataSource = mdv
            If dBohFileDate.Equals(String.Empty) Then
                Label_BOHFileDate.Text = "The BOH File Date associated with this data is uknown."
            Else
                Label_BOHFileDate.Text = String.Format("The BOH File Date associated with this data is {0}", dBohFileDate)
            End If


            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdMaster.Rows(0).Selected = True
            Else
                MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub ugrdMaster_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdMaster.AfterSelectChange
        Call SetButtons()
    End Sub

    Private Sub ugrdMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdMaster.Click
        Call SetButtons()
    End Sub

    Private Sub ugrdMaster_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdMaster.DoubleClickRow
        If cmdEditMaster.Enabled Then
            cmdEditMaster.PerformClick()
        End If
    End Sub

End Class