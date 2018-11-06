Option Strict Off
Option Explicit On
Friend Class colSelectedItems
	
	'local variable to hold collection
    Private m_DataGrid As Infragistics.Win.UltraWinGrid.UltraGrid
	Private mCol As Collection
	
    Public WriteOnly Property DataSource() As Infragistics.Win.UltraWinGrid.UltraGrid
        Set(ByVal Value As Infragistics.Win.UltraWinGrid.UltraGrid)
            m_DataGrid = Value
        End Set
    End Property
	
	Default Public ReadOnly Property Item(ByVal iIndex As Short) As clsSelectedItem
		Get
			Dim oSelectedItem As New clsSelectedItem

            Dim selectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            selectedRow = m_DataGrid.Selected.Rows(iIndex)
            If Not selectedRow Is Nothing Then

                With oSelectedItem
                    .Item_Key = selectedRow.Cells("Item_Key").Value
                    .ItemDescription = selectedRow.Cells("Description").Value
                    .ItemIdentifier = selectedRow.Cells("Identifier").Value
                    .ItemSubTeam = selectedRow.Cells("SubTeam_No").Value
                    '.ItemCategoryID = m_DataGrid.Columns(4).CellValue(vBook)
                    '.Brand_ID = m_DataGrid.Columns(5).CellValue(vBook)
                End With
                Item = oSelectedItem
            Else
                Item = Nothing
            End If
		End Get

    End Property
	
	Public ReadOnly Property Count() As Integer
		Get
            'Count = m_DataGrid.SelBookmarks.Count
            Count = m_DataGrid.Selected.Rows.Count
        End Get
	End Property

    Private Sub Class_Terminate_Renamed()
        'destroys collection when this class is terminated
        m_DataGrid = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub
End Class