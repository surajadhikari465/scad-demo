Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class ScaleIngredientsDAO
    Public Shared Function GetIngredientsByItem(ByVal item_Key As Integer) As IngredientsBO
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim ingredientsBO As IngredientsBO
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_Key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataReader("Scale_GetIngredientsByItem", paramList)
            ingredientsBO = New IngredientsBO()

            If results.HasRows Then
                results.Read()

                ingredientsBO.ID = results.GetInt32(results.GetOrdinal("Scale_Ingredient_ID"))
                ingredientsBO.Description = results.GetString(results.GetOrdinal("Description"))
                ingredientsBO.LabelTypeDescription = results.GetString(results.GetOrdinal("LabelTypeDescription"))
                ingredientsBO.Ingredients = results.GetString(results.GetOrdinal("Ingredients"))
                If results.IsDBNull(results.GetOrdinal("Scale_LabelType_ID")) Then
                    ingredientsBO.LabelTypeID = Nothing
                Else
                    ingredientsBO.LabelTypeID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                End If
            Else
                ingredientsBO.ID = 0
                ingredientsBO.Description = ""
                ingredientsBO.LabelTypeDescription = ""
                ingredientsBO.Ingredients = ""
                ingredientsBO.LabelTypeID = 0
            End If


        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return ingredientsBO
    End Function

    Public Shared Sub AddIngredientsToItem(ByVal item_Key As Integer, ByVal ingredientsBO As IngredientsBO)
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
            currentParam.Name = "IngredientsDescription"
            currentParam.Value = ingredientsBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ingredients"
            currentParam.Value = ingredientsBO.Ingredients
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Value = ingredientsBO.LabelTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_AddIngredientsToItem", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub

    Public Shared Sub UpdateIngredients(ByVal ingredientsBO As IngredientsBO)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim results As SqlDataReader = Nothing

        Try
            currentParam = New DBParam
            currentParam.Name = "Scale_Ingredient_ID"
            currentParam.Value = ingredientsBO.ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IngredientsDescription"
            currentParam.Value = ingredientsBO.Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ingredients"
            currentParam.Value = ingredientsBO.Ingredients
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Value = ingredientsBO.LabelTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_UpdateIngredients", paramList)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
    End Sub
End Class
