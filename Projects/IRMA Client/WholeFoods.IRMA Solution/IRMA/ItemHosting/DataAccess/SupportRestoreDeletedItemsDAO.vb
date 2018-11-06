Imports System.Data.SqlClient
Imports System.Linq
Imports WholeFoods.Utility.DataAccess

Public Class SupportRestoreDeletedItemsDAO
    Public Shared Function ValidateModel(ByVal model As SupportRestoreDeletedItemBO) As SupportRestoreDeletedItemBO
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim sqlReader As SqlDataReader = Nothing

        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = model.Identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        sqlReader = factory.GetStoredProcedureDataReader("SupportRestoreDeletedItemsValidate", paramList)

        While sqlReader.Read()
            model.ValidationError = sqlReader.GetString(sqlReader.GetOrdinal("ValidationError"))
        End While

        Return model
    End Function

    Friend Shared Sub RestoreItems(models As List(Of SupportRestoreDeletedItemBO))
        Dim identifiers As String = String.Join("|", models.Select(Function(m) m.Identifier))

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim sqlReader As SqlDataReader = Nothing

        Try
            currentParam = New DBParam With {
                .Name = "Identifiers",
                .Value = identifiers,
                .Type = DBParamType.String
            }
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("SupportRestoreDeletedItems", paramList)
        Catch ex As Exception
            Throw ex
        Finally
            If sqlReader IsNot Nothing Then
                sqlReader.Close()
            End If
        End Try
    End Sub

    Friend Shared Sub ClearRestoreItemKeysTable()
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        factory.ExecuteNonQuery("DELETE FROM dbo.SupportRestoreDeletedItemsItemKeys")
    End Sub
End Class
