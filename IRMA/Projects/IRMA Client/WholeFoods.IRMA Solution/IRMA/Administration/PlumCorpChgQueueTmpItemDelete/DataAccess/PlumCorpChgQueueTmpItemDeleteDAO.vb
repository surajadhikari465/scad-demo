Imports System.Data.SqlClient
Imports System.Linq
Imports WholeFoods.Utility.DataAccess

Public Class PlumCorpChgQueueTmpItemDeleteDAO
  Public Shared Sub ValidateModel(ByVal model As PlumCorpChgDeleteModel)
    Dim factory As New DataFactory(DataFactory.ItemCatalog)
    model.IsExists = CBool(factory.ExecuteScalar("PlumCorpChgQueueTmpDeletedItemsValidate", Nothing, New SqlParameter("ScanCode", model.ScanCode)))
  End Sub

  Friend Shared Sub DeleteItems(models As List(Of PlumCorpChgDeleteModel))

    Dim factory As New DataFactory(DataFactory.ItemCatalog)
    Dim paramList As New ArrayList

    Try
      paramList.Add(New DBParam With {
                .Name = "ScanCode",
                .Value = String.Join("|", models.Select(Function(m) m.ScanCode).Distinct().ToArray()),
                .Type = DBParamType.String})

      factory.ExecuteStoredProcedure("PlumCorpChgQueueTmpDeletedItems", paramList)
    Catch ex As Exception
      Throw ex
    End Try
  End Sub
End Class