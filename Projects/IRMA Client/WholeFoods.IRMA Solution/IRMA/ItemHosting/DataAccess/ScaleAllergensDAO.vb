Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class ScaleAllergensDAO
    Public Shared Function GetAllergensByItem(ByVal item_Key As Integer) As AllergensBO
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim allergensBO As AllergensBO
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_Key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataReader("Scale_GetAllergensByItem", paramList)

            allergensBO = New AllergensBO()

            If results.HasRows Then
                results.Read()
                allergensBO.ID = results.GetInt32(results.GetOrdinal("Scale_Allergen_ID"))
                allergensBO.Description = results.GetString(results.GetOrdinal("Description"))
                allergensBO.LableTypeDescription = results.GetString(results.GetOrdinal("LabelTypeDescription"))
                allergensBO.Allergens = results.GetString(results.GetOrdinal("Allergens"))
                If results.IsDBNull(results.GetOrdinal("Scale_LabelType_ID")) Then
                    allergensBO.LabelTypeID = Nothing
                Else
                    allergensBO.LabelTypeID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                End If
            Else
                allergensBO.ID = 0
                allergensBO.Description = ""
                allergensBO.LableTypeDescription = ""
                allergensBO.Allergens = ""
                allergensBO.LabelTypeID = 0
            End If


        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return allergensBO
    End Function

    Public Shared Sub AddAllergensToItem(ByVal item_Key As Integer, ByVal allergensBO As AllergensBO)
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
            currentParam.Name = "AllergensDescription"
            currentParam.Value = allergensBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Allergens"
            currentParam.Value = allergensBO.Allergens
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Value = allergensBO.LabelTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_AddAllergensToItem", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub

    Public Shared Sub UpdateAllergens(ByVal allergensBO As AllergensBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Scale_Allergen_ID"
            currentParam.Value = allergensBO.ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AllergensDescription"
            currentParam.Value = allergensBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Allergens"
            currentParam.Value = allergensBO.Allergens
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Value = allergensBO.LabelTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_UpdateAllergens", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub
End Class
