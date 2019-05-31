Imports System.Linq

Public Class PlumCorpChgQueueTmpDeleteError
  Sub New(value As PlumCorpChgDeleteValidateRequestBO)
    InitializeComponent()

    ErrorsDataGridView.AutoGenerateColumns = False

    ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                          .DataPropertyName = "ItemKey",
                                          .HeaderText = "Item Key",
                                          .ReadOnly = True,
                                          .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                          })
    ErrorsDataGridView.Columns.Add(New DataGridViewTextBoxColumn With {
                                          .DataPropertyName = "Error",
                                          .HeaderText = "Error",
                                          .ReadOnly = True,
                                          .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                                          })

    ErrorsDataGridView.DataSource = value.ErrorModels.Select(Function(x) New With {x.ItemKey, .error = "Item Key does not Exist"}).ToArray()
  End Sub
End Class