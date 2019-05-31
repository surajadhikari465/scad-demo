Public Class PlumCorpChgQueueTmpItemDeleteBO
  Public Sub ValidateModels(ByVal request As PlumCorpChgDeleteValidateRequestBO)
    For Each model As PlumCorpChgDeleteModel In request.Models
      PlumCorpChgQueueTmpItemDeleteDAO.ValidateModel(model)
    Next
  End Sub

  Public Sub DeletedItems(ByVal models As PlumCorpChgDeleteValidateRequestBO)
    PlumCorpChgQueueTmpItemDeleteDAO.DeleteItems(models.ValidModels)
  End Sub
End Class