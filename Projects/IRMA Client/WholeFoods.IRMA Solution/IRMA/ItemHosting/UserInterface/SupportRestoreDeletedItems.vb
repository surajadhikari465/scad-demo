Imports System.Linq

Public Class SupportRestoreDeletedItems
    Private Manager As SupportRestoreDeletedItemsManagerBO
    Private maximumNumberOfIdentifiers As Integer

    Sub New()
        InitializeComponent()
        Manager = New SupportRestoreDeletedItemsManagerBO()
        maximumNumberOfIdentifiers = 1000
    End Sub

    Private Sub RestoreItemsBtn_Click(sender As Object, e As EventArgs) Handles RestoreItemsBtn.Click
        Dim models As List(Of SupportRestoreDeletedItemBO) = ItemIdentifiersTextBox _
                .Lines() _
                .Where(Function(i) Not String.IsNullOrWhiteSpace(i)) _
                .Select(Function(i) New SupportRestoreDeletedItemBO With {.Identifier = i.Trim()}) _
                .ToList()

        If Not models.Any() Then
            MessageBox.Show("No Item Identifiers entered.")
        ElseIf models.Count > maximumNumberOfIdentifiers Then
            MessageBox.Show(String.Format("More than the maximum allowed {0} Item Identifiers entered.", maximumNumberOfIdentifiers))
        Else
            Try
                Dim response As SupportRestoreDeletedItemsValidateResponseBO = Manager.ValidateModels(New SupportRestoreDeletedItemsValidateRequestBO With {.Models = models})
                If response.ErrorModels.Any(Function(r) Not String.IsNullOrWhiteSpace(r.ValidationError)) Then
                    Dim errorsWindow As SupportRestoreDeletedItemsErrors = New SupportRestoreDeletedItemsErrors(response.ErrorModels)
                    errorsWindow.ShowDialog()
                Else
                    Manager.RestoreDeletedItems(response.ValidModels)
                    MessageBox.Show("Restored all items successfully.", "Support Restore Deleted Items")
                End If
            Catch ex As Exception
                MessageBox.Show("Unexpected error occurred while restoring items. Error Details:" + ex.ToString(), "Support Restore Deleted Items Error")
            End Try
        End If
    End Sub

    Private Sub ClearRestoreItemKeysTableBtn_Click(sender As Object, e As EventArgs) Handles ClearRestoreItemKeysTableBtn.Click
        Try
            SupportRestoreDeletedItemsManagerBO.ClearRestoreItemKeysTable()
            MessageBox.Show("Deleted all records in SupportRestoreDeletedItemsItemKeys successfully.", "Support Restore Deleted Items")
        Catch ex As Exception
            MessageBox.Show("Unexpected error occurred while truncating the SupportRestoreDeletedItemsItemKeys table. Error Details:" + ex.ToString(), "Support Restore Deleted Items Error")
        End Try
    End Sub
End Class