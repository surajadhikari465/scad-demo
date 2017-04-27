Option Strict Off
Option Explicit On
Friend Class frmCycleCountEdit
	Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView

	Private mlCycleCountID As Integer
	Private mlMasterCountID As Integer
	Private mlSubTeamID As Integer
	Private mlInvLocID As Integer
	Private msMasterEndScan As String
	Private mbReadOnlyUser As Boolean
	Private mbOpen As Boolean
	Private mbPastEntryDeadline As Boolean
	Private mbEndOfProcess As Boolean
	Private mbManufacturingSubTeam As Boolean
    Private mbExternal As Boolean

    Private Const mcOpenText As String = "--- Count is OPEN ---"
	Private Const mcClosedText As String = "--- Count is CLOSED ---"
	
	Private Const miGridScrollBarWidth As Short = 580

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("CycleCounts")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("CountTotal", GetType(String)))
        mdt.Columns.Add(New DataColumn("WeightTotal", GetType(String)))
        
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("CycleCountItemID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("CostedByWeight", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Package_Desc1", GetType(Double)))
        mdt.Columns.Add(New DataColumn("Package_Desc2", GetType(Double)))
        mdt.Columns.Add(New DataColumn("Package_Unit_ID", GetType(Integer)))

    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)
        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("CycleCountItemID") = rsSearch.Fields("CycleCountItemID").Value
                row("Item_Key") = rsSearch.Fields("Item_Key").Value
                row("CostedByWeight") = rsSearch.Fields("CostedByWeight").Value
                row("Identifier") = rsSearch.Fields("Identifier").Value
                row("Item_Description") = rsSearch.Fields("Item_Description").Value
                row("CountTotal") = IIf(IsDBNull(rsSearch.Fields("CountTotal").Value) Or rsSearch.Fields("CountTotal").Value = 0, "", rsSearch.Fields("CountTotal").Value)
                row("WeightTotal") = IIf(IsDBNull(rsSearch.Fields("WeightTotal").Value) Or rsSearch.Fields("WeightTotal").Value = 0, "", VB6.Format(CStr(rsSearch.Fields("WeightTotal").Value), "########0.00##") & " lbs")
                row("Package_Desc1") = rsSearch.Fields("Package_Desc1").Value
                row("Package_Desc2") = rsSearch.Fields("Package_Desc2").Value
                row("Package_Unit_ID") = rsSearch.Fields("Package_Unit_ID").Value
                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "CycleCountItemID"

            ugrdItems.DataSource = mdv

            If rsSearch.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdItems.Rows(0).Selected = True
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

    Public Sub LoadForm(ByRef sName As String, ByRef sCountType As String, ByRef lMasterCountID As Integer, ByRef lCountID As Integer, ByRef lSubTeamID As Integer, ByRef sStartScan As String, ByRef sMasterEndScan As String, ByRef bOpen As Boolean, ByRef bExternal As Boolean, ByRef bEndOfProcess As Boolean, ByRef bManufacturingSubTeam As Boolean, Optional ByRef lInvLocID As Integer = 0)

        Call SetupDataTable()

        mlMasterCountID = lMasterCountID
        mlCycleCountID = lCountID
        mlSubTeamID = lSubTeamID
        mlInvLocID = lInvLocID
        msMasterEndScan = sMasterEndScan
        mbOpen = bOpen
        mbPastEntryDeadline = False
        mbEndOfProcess = bEndOfProcess
        mbManufacturingSubTeam = bManufacturingSubTeam
        mbExternal = bExternal

        'Set mbReadOnlyUser.
        If gbInventoryAdministrator Or gbBuyer Then
            mbReadOnlyUser = False
        Else
            mbReadOnlyUser = True
        End If

        Call SetActive(txtName, False)
        Call SetActive(txtType, False)
        Call SetActive(txtStartScan, False)

        txtName.Text = sName
        txtType.Text = sCountType
        txtStartScan.Text = VB6.Format(CDate(sStartScan), "MM/DD/YYYY HH:SS")

        Call CheckIfLocked()

        If bOpen Then
            lblCountStatus.Text = mcOpenText
        Else
            lblCountStatus.Text = mcClosedText
        End If

        Call LoadGrid()

        Me.ShowDialog()

    End Sub

    Private Sub CheckIfLocked()
        '-------------------------------------------------------------------------------------
        'Purpose:   Check for "locked" conditions in which editing is not allowed.
        '-------------------------------------------------------------------------------------
        Dim dEndDate As Date

        lblCountsLocked.Text = ""

        mbPastEntryDeadline = False

        If mbExternal Then
            lblCountsLocked.Text = "External Count - Entry Locked"
        End If

        dEndDate = CDate(msMasterEndScan)
        If mbEndOfProcess Then
            If SystemDateTime() > DateAdd(Microsoft.VisualBasic.DateInterval.Hour, 36, dEndDate) Then
                If lblCountsLocked.Text = "" Then lblCountsLocked.Text = "Past End Scan Date - Entry Locked"
                mbPastEntryDeadline = True
                Call SetButtons()
            End If
        Else
            If SystemDateTime() > dEndDate Then
                If lblCountsLocked.Text = "" Then lblCountsLocked.Text = "Past End Scan Date - Entry Locked"
                mbPastEntryDeadline = True
                Call SetButtons()
            End If
        End If

    End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
		
		If frmCycleCountItemAdd.LoadForm(mlCycleCountID, mlSubTeamID, mbManufacturingSubTeam, mlInvLocID) = True Then
			
			Call ClearAndRefreshGrid()
			
        End If

        frmCycleCountItemAdd.Dispose()

	End Sub
	
	Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
		
		If MsgBox("Close this count?", MsgBoxStyle.YesNo, "Close Cycle Count") = MsgBoxResult.Yes Then
			
			SQLExecute("EXEC CloseCycleCountHeader " & mlCycleCountID & ",'" & SystemDateTime & "'", dao.RecordsetOptionEnum.dbSQLPassThrough)
			
			lblCountStatus.Text = mcClosedText
			
			mbOpen = False
			
			Call CheckIfLocked()
			
		End If
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
		Dim iDelCnt As Short
		
        If ugrdItems.Selected.Rows.Count > 0 Then

            '-- Make sure they really want to delete the items.
            If MsgBox("Delete the selected Item(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Cycle Count Item(s)") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdItems.Selected.Rows.Count - 1
                SQLExecute("EXEC DeleteCycleCountItem " & ugrdItems.Selected.Rows(iDelCnt).Cells("CycleCountItemID").Value(), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next iDelCnt

            '-- Refresh the grid
            Call LoadGrid()
        Else
            'Shouldn't happen, but just in case.
            MsgBox("You must first select an item to delete.", MsgBoxStyle.Exclamation, "Notice!")
        End If
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEdit.Click
        If ugrdItems.Selected.Rows.Count = 1 Then
            Call frmCycleCountHistoryList.LoadForm(ugrdItems.Selected.Rows(0).Cells("CycleCountItemID").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Item_Key").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Identifier").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Item_Description").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Package_Desc1").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Package_Desc2").Value, _
                                                   ugrdItems.Selected.Rows(0).Cells("Package_Unit_ID").Value, _
                                                   CBool(ugrdItems.Selected.Rows(0).Cells("CostedByWeight").Value), _
                                                   IIf(Not mbOpen, False, Not mbPastEntryDeadline), mbExternal)

            frmCycleCountHistoryList.Dispose()

            Call LoadGrid()
        Else
            MsgBox("You can only edit one item at a time.")
        End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
        Dim vCycleCountID(0) As Long

        vCycleCountID(0) = mlCycleCountID

        Call frmCycleCountReport.LoadForm(mlMasterCountID, vCycleCountID)
        frmCycleCountReport.Dispose()
		
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
		Call LoadGrid()
		
	End Sub
	
	Private Sub frmCycleCountEdit_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		
		mlCycleCountID = 0
		
	End Sub
	
	
	Private Sub LoadGrid()
        Dim sSql As String
        Dim sIdentifier As String
        Dim sDesc As String
        Dim sVendor As String
        Dim sVendorID As String

        sIdentifier = "null"
        sDesc = "null"
        sVendor = "null"
        sVendorID = "null"

        If Trim(txtIdentifier.Text) <> vbNullString Then sIdentifier = "'" & Trim(txtIdentifier.Text) & "'"
        If Trim(txtDesc.Text) <> vbNullString Then sDesc = "'" & Trim(txtDesc.Text) & "'"
        If Trim(txtVendor.Text) <> vbNullString Then sVendor = "'" & Trim(txtVendor.Text) & "'"
        If Trim(txtVendorID.Text) <> vbNullString Then sVendorID = "'" & Trim(txtVendorID.Text) & "'"
        sSql = "EXEC GetCycleCountItemList " & mlCycleCountID & "," & sIdentifier & "," & sDesc & "," & sVendor & "," & sVendorID
        Call LoadDataTable(sSql)

        Call SetButtons()

    End Sub
	
	Private Sub SetButtons()
		
		'Set Edit button.
		cmdEdit.Enabled = False
        If ugrdItems.Selected.Rows.Count = 1 Then
            cmdEdit.Enabled = True
        End If
		
		'Set Delete button.
		cmdDelete.Enabled = False
        If ugrdItems.Selected.Rows.Count > 0 Then
            If Not mbReadOnlyUser And Not mbExternal And mbOpen And Not mbPastEntryDeadline Then
                cmdDelete.Enabled = True
            End If
        End If
		
		'Set Add button.
		cmdAdd.Enabled = False
		If Not mbReadOnlyUser And Not mbExternal And mbOpen And Not mbPastEntryDeadline Then
			cmdAdd.Enabled = True
		End If
		
		'Set Close button.
		cmdClose.Enabled = False
		If Not mbReadOnlyUser And mbOpen Then
			cmdClose.Enabled = True
		End If
		
	End Sub
	
	Private Sub frmCycleCountEdit_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
		
		txtIdentifier.Text = CStr(Nothing)
		txtDesc.Text = CStr(Nothing)
        txtVendor.Text = CStr(Nothing)
		txtVendorID.Text = CStr(Nothing)
		
		Call LoadGrid()
		
	End Sub
	
	Private Sub txtDesc_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDesc.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtDesc.Tag), txtDesc, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub txtIdentifier_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
    Private Sub txtVendor_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtVendor.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtVendor.Tag), txtVendor, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
	
	Private Sub txtVendorID_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtVendorID.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtVendorID.Tag), txtVendorID, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

    Private Sub ugrdItems_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdItems.DoubleClickRow

        If cmdEdit.Enabled Then
            cmdEdit.PerformClick()
        End If
    End Sub
End Class