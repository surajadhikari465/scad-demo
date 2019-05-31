Imports System.Linq

Public Class PlumCorpChgQueueTmpItemDelete
  Private Manager As PlumCorpChgQueueTmpItemDeleteBO
  Private maximumNumberOfItemKeys As Integer

  Sub New()
    InitializeComponent()
    Manager = New PlumCorpChgQueueTmpItemDeleteBO()
    maximumNumberOfItemKeys = 1000
  End Sub

  Private Sub DeletedItemsBtn_Click(sender As Object, e As EventArgs) Handles DeleteItemsBtn.Click
    Dim models As List(Of PlumCorpChgDeleteModel) = ItemKeysTextBox _
    .Lines() _
    .Where(Function(i) IsNumeric(If(String.IsNullOrWhiteSpace(i), String.Empty, i.Trim()))) _
    .Select(Function(i) CInt(i.Trim())).Distinct() _
    .Select(Function(i) New PlumCorpChgDeleteModel With {.ItemKey = i}) _
    .ToList()

    If Not models.Any() Then
      MessageBox.Show("Invalid Item Key entered")
    ElseIf models.Count > maximumNumberOfItemKeys Then
      MessageBox.Show(String.Format("More than the maximum allowed {0} Item Key entered.", maximumNumberOfItemKeys))
    Else
      Try
        Dim value As PlumCorpChgDeleteValidateRequestBO = New PlumCorpChgDeleteValidateRequestBO With {.Models = models}
        Manager.ValidateModels(value)

        If models.Any(Function(x) Not x.IsExists) Then 
          Using form As PlumCorpChgQueueTmpDeleteError = New PlumCorpChgQueueTmpDeleteError(value)
            form.ShowDialog()
          End Using
        Else
          Manager.DeletedItems(value)
          MessageBox.Show("Deleted all items successfully.", "Support Deleted Items")
        End If
      Catch ex As Exception
        MessageBox.Show("Unexpected error occurred while Deleting items. Error Details:" + ex.ToString(), "Support Deleted Items Error")
      End Try
    End If
  End Sub
End Class
