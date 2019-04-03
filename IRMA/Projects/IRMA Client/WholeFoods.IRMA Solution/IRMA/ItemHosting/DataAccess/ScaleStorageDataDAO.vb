Imports System.Data.SqlClient
Imports System.Linq
Imports WholeFoods.Utility.DataAccess

Public Enum ScaleStorageDataValidationStatus
    Valid
    Error_ScaleStorageDataDescriptionInvalidCharacters
    Error_ScaleStorageDataInvalidCharacters
End Enum

Public Class ScaleStorageDataDAO

    Public Const INVALID_CHARACTERS = "|"

    Public Shared Function GetStorageDataByItem(ByVal item_Key As Integer) As StorageDataBO
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim StorageDataBO As StorageDataBO
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_Key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataReader("Scale_GetStorageDataByItem", paramList)

            StorageDataBO = New StorageDataBO()

            If results.HasRows Then
                results.Read()
                StorageDataBO.ID = results.GetInt32(results.GetOrdinal("Scale_StorageData_ID"))

                If (StorageDataBO.ID = 0) Then
                    StorageDataBO.Description = results.GetString(results.GetOrdinal("Identifier"))
                Else
                    StorageDataBO.Description = results.GetString(results.GetOrdinal("Description"))
                End If

                StorageDataBO.StorageData = results.GetString(results.GetOrdinal("StorageData"))

            Else
                StorageDataBO.ID = 0
                StorageDataBO.Description = ""
                StorageDataBO.StorageData = ""
            End If

        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return StorageDataBO
    End Function

    Public Shared Sub AddStorageDataToItem(ByVal item_Key As Integer, ByVal StorageDataBO As StorageDataBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_Key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StorageDataDescription"
            currentParam.Value = StorageDataBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StorageData"
            currentParam.Value = StorageDataBO.StorageData
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_AddStorageDataToItem", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub

    Public Shared Sub UpdateStorageData(ByVal StorageDataBO As StorageDataBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Scale_StorageData_ID"
            currentParam.Value = StorageDataBO.ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StorageDataDescription"
            currentParam.Value = StorageDataBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StorageData"
            currentParam.Value = StorageDataBO.StorageData
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_UpdateStorageData", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub
    Public Shared Function ValidateStorageData(ByVal storageData As StorageDataBO) As ArrayList
        Dim statusList As New ArrayList

        ' -- Description
        If Not String.IsNullOrEmpty(storageData.Description) Then
            If storageData.Description.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                statusList.Add(ScaleStorageDataValidationStatus.Error_ScaleStorageDataDescriptionInvalidCharacters)
            End If
        End If

        ' -- StorageDatag
        If Not String.IsNullOrEmpty(storageData.StorageData) Then
            If storageData.StorageData.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                statusList.Add(ScaleStorageDataValidationStatus.Error_ScaleStorageDataInvalidCharacters)
            End If
        End If

        If statusList.Count = 0 Then
            statusList.Add(ScaleStorageDataValidationStatus.Valid)
        End If

        Return statusList
    End Function
End Class
