Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmLocationItemList
	Inherits System.Windows.Forms.Form

	Dim mlLocID As Integer
	Dim mlSubTeamID As Integer
	Dim mlStoreID As Integer
	Dim mbIsManufacturer As Boolean
    Private mdt As DataTable

    Private Const miGridScrollBarWidth As Short = 580

    Private Sub frmLocationItemList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 13 Then 'Shift + Enter.
            Call ClearAndRefresh()
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
	
	Private Sub ClearAndRefresh()
		
		'-- Clear Search Criteria
		txtIdentifier.Text = ""
		txtItem.Text = ""
		
        LoadDataTable()
		
	End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
		
		'UPGRADE_WARNING: Arrays in structure rsStorageItem may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim rsStorageItem As dao.Recordset = Nothing
		Dim lItemId As Integer
		Dim iSelected As Short
		Dim sItemName As String
		
        '-- Open the search form
        frmItemSearch.InitForm()
        If Not mbIsManufacturer Then
            'Limit the list of sub-teams to the current subteam of the location.
            frmItemSearch.LimitSubTeam_No = mlSubTeamID
        Else
            'Set the initial sub-team number.  List should only contain retail products.
            frmItemSearch.SubTeam_No = mlSubTeamID
        End If

        frmItemSearch.IncludeDeletedItems = Global_Renamed.enumChkBoxValues.UncheckedDisabled
        frmItemSearch.ExcludeNotAvailable = Global_Renamed.enumChkBoxValues.UncheckedDisabled
        frmItemSearch.SoldAtHFM = Global_Renamed.enumChkBoxValues.CheckedDisabled
        frmItemSearch.SoldAtWFM = Global_Renamed.enumChkBoxValues.CheckedDisabled
        frmItemSearch.MultiSelect = True

        frmItemSearch.ShowDialog()

        '-- if its not zero, then something was found
        If frmItemSearch.SelectedItems.Count > 0 Then

            Try
                For iSelected = 0 To frmItemSearch.SelectedItems.Count - 1

                    lItemId = frmItemSearch.SelectedItems.Item(iSelected).Item_Key
                    sItemName = frmItemSearch.SelectedItems.Item(iSelected).ItemDescription
                    Debug.Print(glItemID)

                    '--Check to see if the item already exists for this location.
                    rsStorageItem = SQLOpenRecordSet("EXEC CheckForDuplicateInvLocationItem " & mlLocID & ", " & lItemId, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    If rsStorageItem.Fields("Found").Value > 0 Then
                        MsgBox("[" & VB.Left(sItemName, 25) & "] is already in the list.", MsgBoxStyle.Exclamation, "Duplicated Item!")
                    Else
                        '-- Add the new record
                        SQLExecute("EXEC InsertInventoryLocationItem " & mlLocID & ", " & lItemId, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    End If

                Next iSelected

            Finally
                If rsStorageItem IsNot Nothing Then
                    rsStorageItem.Close()
                    rsStorageItem = Nothing
                End If
            End Try

            LoadDataTable()

        End If

        frmItemSearch.Close()
        frmItemSearch.Dispose()

	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
		
		Dim iDelCnt As Short

        If ugrdItemList.Selected.Rows.Count > 0 Then

            '-- Make sure they really want to delete the item.
            If MsgBox("Delete selected item(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdItemList.Selected.Rows.Count - 1
                SQLExecute("EXEC DeleteInventoryLocationItems " & mlLocID & ", " & ugrdItemList.Selected.Rows(iDelCnt).Cells("Item_Key").Value.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next iDelCnt

            '-- Refresh the grid and seek the new one of its place
            LoadDataTable()

        Else

            '-- No vendor was selected
            MsgBox("An item must be selected to be deleted.", MsgBoxStyle.Exclamation, "No item Selected!")

        End If
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Close the form
		Me.Close()
		
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
        Call LoadDataTable()
		
	End Sub
	
	Private Sub frmLocationItemList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form
		CenterForm(Me)
		
        SetupDataTable()
        LoadDataTable()
		
	End Sub
	
	Public Sub LoadForm(ByRef lLocID As Integer, ByRef lSubTeamID As Integer, ByRef lStoreID As Integer, ByRef sLocName As String, Optional ByRef bIsManufacturer As Boolean = False)
		
		mlLocID = lLocID
		mlSubTeamID = lSubTeamID
		mbIsManufacturer = bIsManufacturer
		
		mlStoreID = lStoreID
		txtLocName.Text = sLocName
		
		Me.ShowDialog()
		
	End Sub
	
	Private Sub SetFormPermissions()
		
        If Not gbInventoryAdministrator And Not gbBuyer Then
            cmdAdd.Enabled = False
            cmdDelete.Enabled = False
        End If
		
	End Sub
	
    Private Sub LoadDataTable()

        Dim rsItemList As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Clear()

        '-- Set up the databound stuff
        Dim sIdentity As String
        Dim sItem As String

        sIdentity = "null"
        sItem = "null"

        Try
            If Trim(txtIdentifier.Text) <> vbNullString Then sIdentity = "'" & Trim(Me.txtIdentifier.Text) & "'"
            If Trim(txtItem.Text) <> vbNullString Then sItem = "'" & Trim(Me.txtItem.Text) & "'"
            rsItemList = SQLOpenRecordSet("EXEC GetInventoryLocationItems " & mlLocID & "," & sIdentity & "," & sItem, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsItemList.EOF
                row = mdt.NewRow
                row("Item_Key") = rsItemList.Fields("Item_Key").Value
                row("Identifier") = rsItemList.Fields("Identifier").Value
                row("Item_Description") = rsItemList.Fields("Item_Description").Value

                mdt.Rows.Add(row)
                rsItemList.MoveNext()
            End While

        Finally
            If rsItemList IsNot Nothing Then
                rsItemList.Close()
                rsItemList = Nothing
            End If
        End Try

        mdt.AcceptChanges()
        ugrdItemList.DataSource = mdt

        If ugrdItemList.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdItemList.Rows(0).Selected = True
        End If

        SetButtons()
    End Sub

    Private Sub SetupDataTable()
        ' Create a data table
        mdt = New DataTable("LocationItem")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))

    End Sub

    Private Sub SetButtons()
        If ugrdItemList.Selected.Rows.Count > 0 Then
            Me.cmdDelete.Enabled = True
        Else
            Me.cmdDelete.Enabled = False
        End If

        Call SetFormPermissions()

    End Sub

    Private Sub frmLocationItemList_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        mlLocID = 0
        mlSubTeamID = 0
        mlStoreID = 0
        mbIsManufacturer = False

    End Sub

    Private Sub ugrdItemList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdItemList.Click
        SetButtons()
    End Sub

    Private Sub ugrdItemList_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdItemList.CellChange
        SetButtons()
    End Sub

    Private Sub txtIdentifier_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtItem_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtItem.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtItem.Tag), txtItem, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

End Class