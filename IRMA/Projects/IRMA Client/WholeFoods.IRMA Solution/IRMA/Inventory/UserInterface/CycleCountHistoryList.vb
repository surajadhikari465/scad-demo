Option Strict Off
Option Explicit On
Friend Class frmCycleCountHistoryList
	Inherits System.Windows.Forms.Form
	
	Private mlCycleCountItemID As Integer
	Private mlItemKey As Integer
	Private msItemDesc As String
    Private mdPD1 As Decimal
    Private mdPD2 As Decimal
    Private mlPDU As Integer

	Private mbWeightedItem As Boolean
	
	Private mbUpdated As Boolean
	Private mbFormReadOnly As Boolean
	Private mbOpen As Boolean
    Private mbExternal As Boolean

    Private mdt As DataTable
    Private mdv As DataView
	
    Public Function LoadForm(ByRef lCycleCountItemID As Integer, ByRef lItemKey As Integer, ByRef sIdentifier As String, ByRef sItemDesc As String, ByRef PD1 As Decimal, ByRef PD2 As Decimal, ByRef PDU As Integer, ByRef bWeightedItem As Boolean, ByRef bOpen As Boolean, ByRef bExternal As Boolean) As Boolean

        mbUpdated = False
        mbOpen = bOpen
        mbExternal = bExternal

        mbFormReadOnly = False
        If Not bOpen Or bExternal Then mbFormReadOnly = True

        mlCycleCountItemID = lCycleCountItemID
        mlItemKey = lItemKey
        mdPD1 = PD1
        mdPD2 = PD2
        mlPDU = PDU
        mbWeightedItem = bWeightedItem

        txtIdentifier.Text = sIdentifier
        txtItem_Desc.Text = sItemDesc

        Call LoadGrid()

        Call SetReadOnly()

        Me.ShowDialog()

        'Set the return value.
        LoadForm = mbUpdated

    End Function
	
    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        Dim dDateTime As Date
        If frmCycleCountHistoryEdit.LoadForm(mlCycleCountItemID, txtItem_Desc.Text, mbOpen, mbExternal, mdPD1, mdPD2, mlPDU, mbWeightedItem, dDateTime) Then
            'If updated were made, refresh the grid.
            mbUpdated = True
            Call LoadGrid()
        End If

        frmCycleCountHistoryEdit.Dispose()

    End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
		Dim iDelCnt As Short

        If ugrdList.Selected.Rows.Count > 0 Then

            '-- Make sure they really want to delete the items.
            If MsgBox("Delete the selected Item(s)?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Cycle Count History Item(s)") = MsgBoxResult.No Then
                Exit Sub
            End If

            For iDelCnt = 0 To ugrdList.Selected.Rows.Count - 1
                SQLExecute("EXEC DeleteCycleCountHistoryItem " & mlCycleCountItemID & ",'" & ugrdList.Selected.Rows(iDelCnt).Cells("Date").Value & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next iDelCnt

            '-- Refresh the grid
            Call LoadGrid()

        Else

            'Shouldn't happen, but just in case.
            MsgBox("You must first select an item to delete.", MsgBoxStyle.Exclamation, "Notice!")
        End If
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEdit.Click
		
        If frmCycleCountHistoryEdit.LoadForm(mlCycleCountItemID, txtItem_Desc.Text, mbOpen, mbExternal, mdPD1, mdPD2, mlPDU, mbWeightedItem, ugrdList.Selected.Rows(0).Cells("Date").Value) Then
            'If updated were made, refresh the grid.
            mbUpdated = True
            Call LoadGrid()
        End If

        frmCycleCountHistoryEdit.Dispose()

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdSearch_Click()
		
		Call LoadGrid()
		
	End Sub
	
    Private Sub LoadGrid()

        Dim rsList As DAO.Recordset = Nothing

        Call SetupDataTable()
        Call LoadDataTable()

        Call SetButtons()

    End Sub
	
	Private Sub SetButtons()
		
        If ugrdList.Selected.Rows.Count = 1 Then
            cmdEdit.Enabled = True
        Else
            cmdEdit.Enabled = False
        End If
		
        If ugrdList.Selected.Rows.Count > 0 Then
            cmdDelete.Enabled = IIf(mbFormReadOnly, False, True)
        Else
            cmdDelete.Enabled = False
        End If
		
		cmdAdd.Enabled = IIf(mbFormReadOnly, False, True)
		
	End Sub
	
	Private Sub SetReadOnly()
		
		If mbFormReadOnly Then
			cmdAdd.Enabled = False
			cmdDelete.Enabled = False
		End If
		
    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("HistoryList")

        mdt.Columns.Add(New DataColumn("Date", GetType(String)))
        mdt.Columns.Add(New DataColumn("Pack Size", GetType(String)))
        mdt.Columns.Add(New DataColumn("Count", GetType(String)))
        mdt.Columns.Add(New DataColumn("Weight", GetType(String)))

    End Sub

    Private Sub LoadDataTable()

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000
        Dim dt As DateTime
        Dim sSQL As String

        Try
            sSQL = "EXEC GetCycleCountHistoryList " & mlCycleCountItemID

            rsSearch = SQLOpenRecordSet(sSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1

                row = mdt.NewRow
                dt = Convert.ToDateTime(rsSearch.Fields("ScanDateTime").Value)
                row("Date") = dt.ToString("MM/dd/yy HH:mm:ss")
                row("Pack Size") = String.Format("{0:########0.00##}", rsSearch.Fields("PackSize").Value)
                row("Count") = String.Format("{0:########0.00##}", rsSearch.Fields("Count").Value)
                row("Weight") = String.Format("{0:########0.00##}", rsSearch.Fields("Weight").Value) & IIf(IsDBNull(rsSearch.Fields("Weight")), "", " lbs")
                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Date"
            ugrdList.DataSource = mdv

            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then
                ugrdList.Rows(0).Selected = True
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

    Private Sub ugrdList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdList.Click

        Call SetButtons()

    End Sub

    Private Sub ugrdList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdList.DoubleClickRow

        If cmdEdit.Enabled Then
            cmdEdit.PerformClick()
        End If

    End Sub

End Class