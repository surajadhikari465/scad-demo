Option Strict Off
Option Explicit On
Friend Class frmLocationList
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mvSubTeamList() As Boolean

    Private IsInitializing As Boolean
	
	Private Sub cmdAddLoc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddLoc.Click
		
        Call frmLocationEdit.Load_Form()

        frmLocationEdit.Dispose()
		
		Call RefreshGrid()
		
	End Sub
	
	Private Sub cmdDeleteLoc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteLoc.Click
		
		Dim iDelCnt As Short
        'Dim vBook As Object
		
        If ugrdLocationList.Selected.Rows.Count > 0 Then

            '-- Make sure they really want to delete.
            If MsgBox("Delete the selected Location(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Location(s)") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdLocationList.Selected.Rows.Count - 1

                ' vBook = gridLoc.SelBookmarks(iDelCnt)

                '   SQLExecute("EXEC DeleteInventoryLocations " & ugrdLocationList.Columns(0).CellValue(vBook).ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                SQLExecute("EXEC DeleteInventoryLocations " & ugrdLocationList.Selected.Rows(iDelCnt).Cells("InvLoc_ID").Value.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next iDelCnt

            '-- Refresh the grid
            RefreshGrid()

        Else

            'Shouldn't happen, but just in case.
            MsgBox("You must first select a location to delete.", MsgBoxStyle.Exclamation, "Notice!")

        End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
		Call RefreshGrid()
		
	End Sub
	
	Private Sub cmdLocItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLocItems.Click
		
        If ugrdLocationList.Selected.Rows.Count = 1 Then
            Call frmLocationItemList.LoadForm((ugrdLocationList.Selected.Rows(0).Cells("InvLoc_ID").Value), (ugrdLocationList.Selected.Rows(0).Cells("SubTeam_No").Value), (ugrdLocationList.Selected.Rows(0).Cells("Store_No").Value), (ugrdLocationList.Selected.Rows(0).Cells("InvLoc_Name").Text), (ugrdLocationList.Selected.Rows(0).Cells("Manufacturing").Value))

            frmLocationItemList.Dispose()

        Else
            Call MsgBox("Please select a location.", MsgBoxStyle.Exclamation, "Notice!")
        End If
		
    End Sub

    Private Sub frmLocationList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        Dim s As String
        s = ugrdLocationList.Selected.Rows(0).Cells("InvLoc_ID").Value
        MsgBox(s)
    End Sub
	
	Private Sub frmLocationList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 13 Then 'Shift+Enter.
			Call ClearAndRefresh()
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub ClearAndRefresh()
		
		'-- Clear Search Criteria
        If cboStore.Enabled Then cboStore.SelectedIndex = -1
        If cboSubTeam.Enabled Then cboSubTeam.SelectedIndex = -1
		txtName.Text = ""
		txtDesc.Text = ""
		
		Call RefreshGrid()
		
	End Sub
	
	Private Sub frmLocationList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'-- Center form
		CenterForm(Me)
		
		'-- Load store(s).
		Call LoadStore(cboStore, True)
        If glStore_Limit > 0 Then
            'If the user is restricted to a single store, lock the combo box to that store.
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        End If
        'call setup of new grid
        Call SetupDataTable()

		Call LoadSubTeams()
		
        Call RefreshGrid()
		
	End Sub
	
	Private Sub SetFormPermissions()
		
		'-- Disable functionality based on user privileges.
        If Not gbInventoryAdministrator Then
            cmdAddLoc.Enabled = False
            cmdDeleteLoc.Enabled = False
        End If
		
	End Sub
	
	Private Sub LoadSubTeams()
		
		'-- Limit sub-teams user-assigned and only those related to the selected location.
		If cboStore.SelectedIndex > -1 Then
			'Load the user's sub-teams restricted to the selected store.
            Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.StoreUser, cboSubTeam, mvSubTeamList, VB6.GetItemData(cboStore, cboStore.SelectedIndex), 0)
		Else
			cboSubTeam.Items.Clear()
		End If
		
	End Sub
	
	Private Sub RefreshGrid()
        'Dim rsList As dao.Recordset = Nothing
        Dim sStoreID As String
        Dim sSubTeamID As String
		Dim sLocName As String
		Dim sLocDesc As String
        'new grid
        Dim rsLocList As DAO.Recordset = Nothing
        Dim row As DataRow

        'gridLoc.RemoveAll()
		
        sStoreID = "null"
        sSubTeamID = "null"
		sLocName = "null"
		sLocDesc = "null"
		
        If cboStore.SelectedIndex > -1 Then sStoreID = VB6.GetItemData(cboStore, cboStore.SelectedIndex)
        If cboSubTeam.SelectedIndex > -1 Then sSubTeamID = VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex)
		If Trim(txtName.Text) <> vbNullString Then sLocName = "'" & Trim(txtName.Text) & "'"
		If Trim(txtDesc.Text) <> vbNullString Then sLocDesc = "'" & Trim(txtDesc.Text) & "'"

        Try
            'rsList = SQLOpenRecordSet("EXEC GetInventoryLocations " & sStoreID & ", " & sSubTeamID & ", " & sLocName & ", " & sLocDesc, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
            'new grid rs
            rsLocList = SQLOpenRecordSet("EXEC GetInventoryLocations " & sStoreID & ", " & sSubTeamID & ", " & sLocName & ", " & sLocDesc, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            'While Not rsList.EOF
            '    Dim sGridAdd As String
            '    sGridAdd = rsList.Fields("InvLoc_ID").Value & vbTab & rsList.Fields("SubTeam_No").Value & vbTab & rsList.Fields("Manufacturing").Value & vbTab & rsList.Fields("Store_No").Value & vbTab & rsList.Fields("Store_Name").Value & vbTab & rsList.Fields("SubTeam_Name").Value & vbTab & rsList.Fields("InvLoc_Name").Value & vbTab & rsList.Fields("InvLoc_Desc").Value & vbTab
            '    gridLoc.AddItem(sGridAdd)

            '    rsList.MoveNext()
            'End While
            'rsList.Close()

            'clear new grid
            mdt.Rows.Clear()

            'loads new grid
            While Not rsLocList.EOF

                row = mdt.NewRow
                row("InvLoc_ID") = rsLocList.Fields("InvLoc_ID").Value
                row("SubTeam_No") = rsLocList.Fields("SubTeam_No").Value
                row("Manufacturing") = rsLocList.Fields("Manufacturing").Value
                row("Store_No") = rsLocList.Fields("Store_No").Value
                row("Store_Name") = rsLocList.Fields("Store_Name").Value
                row("SubTeam_Name") = rsLocList.Fields("SubTeam_Name").Value
                row("InvLoc_Name") = rsLocList.Fields("InvLoc_Name").Value
                row("InvLoc_Desc") = rsLocList.Fields("InvLoc_Desc").Value

                mdt.Rows.Add(row)

                rsLocList.MoveNext()
            End While

            mdt.AcceptChanges()
            ugrdLocationList.DataSource = mdt
            'close down rs for new grid
        Finally
            If rsLocList IsNot Nothing Then
                rsLocList.Close()
                rsLocList = Nothing
            End If
        End Try

        If ugrdLocationList.Rows.Count > 0 Then
            ugrdLocationList.Rows(0).Selected = True
        End If

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

    End Sub
	
	Private Sub frmLocationList_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

		ReDim mvSubTeamList(0)
		
	End Sub
	
    Private Sub gridLoc_DblClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        If cmdEditLoc.Enabled Then
            cmdEditLoc.PerformClick()
        End If

    End Sub
	
	Private Sub cmdEditLoc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditLoc.Click
		
        If ugrdLocationList.Selected.Rows.Count = 1 Then

            '-- Show the edit screen
            Call frmLocationEdit.Load_Form((ugrdLocationList.Selected.Rows(0).Cells("InvLoc_ID").Value), (ugrdLocationList.Selected.Rows(0).Cells("SubTeam_No").Value), (ugrdLocationList.Selected.Rows(0).Cells("Manufacturing").Value), (ugrdLocationList.Selected.Rows(0).Cells("Store_No").Value))

            frmLocationEdit.Dispose()

            Call RefreshGrid()

        Else

            MsgBox("Please select a line to edit.", MsgBoxStyle.Exclamation, "Notice!")

        End If
		
	End Sub
	
	
	Private Sub SetButtons()
		
        If ugrdLocationList.Selected.Rows.Count > 0 Then
            Me.cmdDeleteLoc.Enabled = True
        Else
            Me.cmdDeleteLoc.Enabled = False
        End If

        If ugrdLocationList.Selected.Rows.Count = 1 Then
            Me.cmdLocItems.Enabled = True
            Me.cmdEditLoc.Enabled = True
        Else
            Me.cmdLocItems.Enabled = False
            Me.cmdEditLoc.Enabled = False
        End If

        Call SetFormPermissions()

    End Sub

    Private Sub txtDesc_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDesc.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtDesc.Tag), txtDesc, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtName_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtName.Tag), txtName, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
    Private Sub SetupDataTable()
        mdt = New DataTable("InventoryLocation")
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("InvLoc_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Manufacturing", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        'Visible.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("SubTeam_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("InvLoc_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("InvLoc_Desc", GetType(String)))

    End Sub

    Private Sub ugrdLocationList_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdLocationList.CellChange
        Call SetButtons()
    End Sub

    Private Sub ugrdLocationList_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdLocationList.Click
        Call SetButtons()
    End Sub

    Private Sub ugrdLocationList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdLocationList.DoubleClickRow

        If cmdEditLoc.Enabled Then
            cmdEditLoc.PerformClick()
        End If

    End Sub

End Class