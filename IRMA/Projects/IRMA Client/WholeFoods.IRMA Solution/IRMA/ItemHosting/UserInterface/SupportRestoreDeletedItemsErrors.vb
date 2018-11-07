Public Class SupportRestoreDeletedItemsErrors
    Public Property Models As List(Of SupportRestoreDeletedItemBO)

    Sub New(models As List(Of SupportRestoreDeletedItemBO))
        InitializeComponent()

        Me.Models = models
        ErrorsDataGridView.AutoGenerateColumns = False

        ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                           .DataPropertyName = "Identifier",
                                           .HeaderText = "Identifier",
                                           .ReadOnly = True,
                                           .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                       })
        ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                           .DataPropertyName = "ValidationError",
                                           .HeaderText = "Error",
                                           .ReadOnly = True,
                                           .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                                       })

        ErrorsDataGridView.DataSource = models
    End Sub
End Class