Public Class SupportRestoreDeletedItemsManagerBO
    Public Function ValidateModels(ByVal request As SupportRestoreDeletedItemsValidateRequestBO) As SupportRestoreDeletedItemsValidateResponseBO
        Dim response As SupportRestoreDeletedItemsValidateResponseBO = New SupportRestoreDeletedItemsValidateResponseBO

        For Each model As SupportRestoreDeletedItemBO In request.Models
            SupportRestoreDeletedItemsDAO.ValidateModel(model)
            If String.IsNullOrWhiteSpace(model.ValidationError) Then
                response.ValidModels.Add(model)
            Else
                response.ErrorModels.Add(model)
            End If
        Next

        Return response
    End Function

    Public Sub RestoreDeletedItems(ByVal models As List(Of SupportRestoreDeletedItemBO))
        SupportRestoreDeletedItemsDAO.RestoreItems(models)
    End Sub

    Friend Shared Sub ClearRestoreItemKeysTable()
        SupportRestoreDeletedItemsDAO.ClearRestoreItemKeysTable()
    End Sub
End Class
