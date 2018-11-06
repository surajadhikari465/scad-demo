Public Class ChooseSingleValueFromList

    Private _dataSource As DataTable
    Public Property DataSource() As DataTable
        Get
            Return _dataSource
        End Get
        Set(ByVal value As DataTable)
            _dataSource = value
        End Set
    End Property

    Private _selection As Object
    Public ReadOnly Property Selection() As Object
        Get
            Return _selection
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetDataSource()
        DataGridViewValues.DataSource = DataSource
    End Sub

    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        _selection = DataGridViewValues.SelectedRows.Item(0).Cells.Item(0).Value
        DialogResult = DialogResult.OK
    End Sub

    Private Sub DataGridViewValues_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewValues.CellContentDoubleClick
        _selection = DataGridViewValues.Rows(e.RowIndex).Cells.Item(0).Value
        DialogResult = DialogResult.OK
    End Sub
End Class