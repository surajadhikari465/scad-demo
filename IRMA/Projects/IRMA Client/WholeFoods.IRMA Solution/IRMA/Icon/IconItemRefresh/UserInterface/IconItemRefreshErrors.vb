Public Class IconItemRefreshErrors
    Public Property ItemRefreshModels As List(Of ItemRefreshModel)

    Sub New(itemRefreshModels As List(Of ItemRefreshModel), Optional systemName As String = "Icon")
        InitializeComponent()
        Me.Text = systemName & " Item Refresh Errors"

        Me.ItemRefreshModels = itemRefreshModels
        ErrorsDataGridView.AutoGenerateColumns = False

        ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                           .DataPropertyName = "Identifier",
                                           .HeaderText = "Identifier",
                                           .ReadOnly = True,
                                           .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                       })
        ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                           .DataPropertyName = "RefreshError",
                                           .HeaderText = "Error",
                                           .ReadOnly = True,
                                           .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                                       })

        ErrorsDataGridView.DataSource = itemRefreshModels
    End Sub
End Class