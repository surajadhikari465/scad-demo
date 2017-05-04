Option Strict Off
Option Explicit On
Friend Class frmCycleCountLocationList
	Inherits System.Windows.Forms.Form
	
    Private mvLocationList() As Long
	Private msStoreName As String
	Private msSubTeamName As String
	Private mlStoreID As Integer
    Private mlSubTeamID As Integer

    Private mdt As DataTable
    Private mdv As DataView

    Public Sub LoadForm(ByRef sStoreName As String, ByRef lStoreID As Integer, ByRef sSubTeamName As String, ByRef lSubTeamID As Integer, ByRef vLocationList() As Long)

        ReDim vLocationList(0)

        msStoreName = sStoreName
        msSubTeamName = sSubTeamName

        mlStoreID = lStoreID
        mlSubTeamID = lSubTeamID

        txtStore.Text = msStoreName
        txtSubTeam.Text = msSubTeamName

        Call RefreshGrid()

        Me.ShowDialog()

        vLocationList = VB6.CopyArray(mvLocationList)

    End Sub
	
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click

        Call RefreshGrid()

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click

        Call AddLocationsToArray()

        Me.Close()

    End Sub

    Private Sub frmCycleCountLocationList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
        txtName.Text = ""
        txtDesc.Text = ""

        Call RefreshGrid()

    End Sub

    Private Sub RefreshGrid()

        Call SetupDataTable()
        Call LoadDataTable()

        Call SetButtons()

    End Sub

    Private Sub SetButtons()

        If ugrdLoc.Selected.Rows.Count > 0 Then
            Me.cmdSelect.Enabled = True
        Else
            Me.cmdSelect.Enabled = False
        End If

    End Sub

    Private Sub frmCycleCountLocationList_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        msStoreName = CStr(Nothing)
        msSubTeamName = CStr(Nothing)

    End Sub

    Private Sub AddLocationsToArray()
        '---------------------------------------------------------------------
        'Purpose:   Walks the grid and adds each selected location to an array
        '           that will be passed back to the calling program.
        '---------------------------------------------------------------------

        Dim iAddCnt As Short
        '   Dim vBook As Object

        If ugrdLoc.Selected.Rows.Count > 0 Then

            ReDim mvLocationList(ugrdLoc.Selected.Rows.Count)

            For iAddCnt = 0 To ugrdLoc.Selected.Rows.Count - 1

                'Add the Location to the array.
                mvLocationList(iAddCnt) = ugrdLoc.Selected.Rows(iAddCnt).Cells("InvLocID").Value

            Next iAddCnt
        End If

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

        ' Create a data table
        mdt = New DataTable("LocationList")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Location Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Location Desc", GetType(String)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("InvLocID", GetType(Integer)))

    End Sub

    Private Sub LoadDataTable()

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000
        Dim sSQL As String

        Try
            sSQL = "EXEC GetInventoryLocationsByStore " & mlStoreID & ", " & mlSubTeamID

            rsSearch = SQLOpenRecordSet(sSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("InvLocID") = rsSearch.Fields("InvLoc_ID").Value
                row("Location Name") = rsSearch.Fields("InvLoc_Name").Value
                row("Location Desc") = rsSearch.Fields("InvLoc_Desc").Value

                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Location Name"
            ugrdLoc.DataSource = mdv

            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdLoc.Rows(0).Selected = True
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

    Private Sub ugrdLoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdLoc.Click

        Call SetButtons()

    End Sub

    Private Sub ugrdLoc_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdLoc.DoubleClickRow

        If cmdSelect.Enabled Then
            cmdSelect.PerformClick()
        End If

    End Sub
End Class