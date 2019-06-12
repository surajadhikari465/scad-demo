Imports System.Linq

Public Class PlumCorpChgQueueTmpItemDelete
  Private Manager As PlumCorpChgQueueTmpItemDeleteBO
  Private maximumNumberOfScanCodes As Integer

  Sub New()
    InitializeComponent()
    Manager = New PlumCorpChgQueueTmpItemDeleteBO()
    maximumNumberOfScanCodes = 1000
  End Sub

  Private Sub DeletedItemsBtn_Click(sender As Object, e As EventArgs) Handles DeleteItemsBtn.Click
    Dim models As List(Of PlumCorpChgDeleteModel) = ScanCodeTextBox _
            .Lines() _
            .Where(Function(i) Not String.IsNullOrWhiteSpace(i)) _
            .Select(Function(i) i.Trim()).Distinct() _ 
            .Select(Function(i) New PlumCorpChgDeleteModel With {.ScanCode = i}) _
            .ToList()

    If Not models.Any() Then
      MessageBox.Show("Invalid ScanCodes entered")
    ElseIf models.Count > maximumNumberOfScanCodes Then
      MessageBox.Show(String.Format("More than the maximum allowed {0} ScanCodes entered.", maximumNumberOfScanCodes))
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
          MessageBox.Show("Deleted all ScanCodes successfully.", "Support Deleted ScanCodes")
        End If
      Catch ex As Exception
        MessageBox.Show("Unexpected error occurred while Deleting ScanCodes. Error Details:" + ex.ToString(), "Support Deleted ScanCodes Error")
      End Try
    End If
  End Sub
End Class