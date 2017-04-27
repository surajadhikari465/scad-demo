Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmSelectList
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
	
	Public Sub LoadForm(ByRef lVendorID As Integer, ByRef sStores As String, ByRef lItemKey As Integer)
		Dim sSql As String
        Dim rsStores As DAO.Recordset = Nothing
        Dim row As DataRow

        'setup datagrid
        Call SetupDataTable()

        'With gridStoreList
        '	.Columns(0).visible = False
        '          .Columns(0).Name = ResourcesIRMA.GetString("StoreNumber")
        '	.Columns(1).visible = True
        '          .Columns(1).Name = ResourcesIRMA.GetString("StoreName")
        'End With

		'-- Fill out the store list
        Try
            sSql = "EXEC GetStoreItemVendorStores null, " & lVendorID
            rsStores = SQLOpenRecordSet(sSql, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsStores.EOF

                row = mdt.NewRow
                row("Store_No") = rsStores.Fields("Store_No").Value
                row("Store_Name") = rsStores.Fields("Store_Name").Value

                mdt.Rows.Add(row)

                rsStores.MoveNext()
            End While

            mdt.AcceptChanges()
            ugrdSelectList.DataSource = mdt
        Finally
            'close down rs for new grid
            If rsStores IsNot Nothing Then
                rsStores.Close()
                rsStores = Nothing
            End If
        End Try

        If ugrdSelectList.Rows.Count > 0 Then
            ugrdSelectList.Rows(0).Selected = True
        End If

        Me.ShowDialog()

        sStores = GetStoreList()

    End Sub
	
	Private Function GetStoreList() As String
		
		Dim iLoop As Short
		Dim sStores As String

        sStores = String.Empty

        For iLoop = 0 To ugrdSelectList.Selected.Rows.Count - 1
            sStores = sStores & "|" & ugrdSelectList.Selected.Rows(iLoop).Cells("Store_no").Value.ToString
        Next iLoop
		
		If Len(sStores) > 0 Then sStores = VB.Right(sStores, Len(sStores) - 1)
		
		GetStoreList = sStores
		
	End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
		
        If Me.ugrdSelectList.Selected.Rows.Count > 0 Then
            Me.Close()
        Else
            MsgBox(ResourcesIRMA.GetString("MustSelect"))
        End If
		
    End Sub

    Private Sub SetupDataTable()
        mdt = New DataTable("VendorStores")
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))

        'Visible.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))

    End Sub
End Class