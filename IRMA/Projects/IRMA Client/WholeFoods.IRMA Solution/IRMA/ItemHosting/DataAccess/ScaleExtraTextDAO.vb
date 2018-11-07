Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ScaleExtraTextDAO

        Public Shared Function GetComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleExtraText As ScaleExtraTextBO
            Dim scaleExtraTextList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetExtraTexts")

                While results.Read
                    scaleExtraText = New ScaleExtraTextBO()
                    scaleExtraText.ID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
                    scaleExtraText.Description = results.GetString(results.GetOrdinal("Description"))
                    scaleExtraText.Scale_LabelType_ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                    scaleExtraText.ExtraText = results.GetString(results.GetOrdinal("ExtraText"))

                    scaleExtraTextList.Add(scaleExtraText)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleExtraTextList
        End Function

        Public Shared Function GetComboListDataTable() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("Scale_GetExtraTextCombo")

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Shared Function GetExtraTextByItemList(ByVal item_Key As Integer, ByVal storeJurisdictionID As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleExtraText As ScaleExtraTextBO
            Dim scaleExtraTextList As New ArrayList
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreJurisdictionID"
                currentParam.Value = storeJurisdictionID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetExtraTextByItem", paramList)

                While results.Read
                    scaleExtraText = New ScaleExtraTextBO()
                    scaleExtraText.ID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
                    scaleExtraText.Description = results.GetString(results.GetOrdinal("Description"))
                    scaleExtraText.Scale_LabelType_ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                    scaleExtraText.ExtraText = results.GetString(results.GetOrdinal("ExtraText"))

                    scaleExtraTextList.Add(scaleExtraText)
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleExtraTextList
        End Function

        Public Shared Function GetExtraTextByItem(ByVal item_Key As Integer, ByVal storeJurisdictionID As Integer) As ScaleExtraTextBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim extraTextBO As New ScaleExtraTextBO

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreJurisdictionID"
                currentParam.Value = storeJurisdictionID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetExtraTextByItem", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText_ID"))) Then
                        extraTextBO.Description = results.GetString(results.GetOrdinal("Description"))
                        extraTextBO.ExtraText = results.GetString(results.GetOrdinal("ExtraText"))
                        extraTextBO.ID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
                        extraTextBO.Scale_LabelType_ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                    End If
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return extraTextBO

        End Function

        Public Shared Function GetExtraTextForNonScaleItemByItem(ByVal item_Key As Integer) As ItemExtraTextBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim extraTextBO As New ItemExtraTextBO

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Item_GetExtraTextByItem", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Item_ExtraText_ID"))) Then
                        extraTextBO.ItemNutritionId = results.GetInt32(results.GetOrdinal("ItemNutritionId"))
                        extraTextBO.ExtraTextID = results.GetInt32(results.GetOrdinal("Item_ExtraText_ID"))
                        extraTextBO.Scale_LabelType_ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                        extraTextBO.Description = results.GetString(results.GetOrdinal("Description"))
                        extraTextBO.ExtraText = results.GetString(results.GetOrdinal("ExtraText"))

                    End If
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return extraTextBO

        End Function

        Public Shared Function GetIngredientForNonScaleItemByItem(ByVal item_Key As Integer) As ItemIngredientBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim ingredientBO As New ItemIngredientBO

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Item_GetIngredientsByItem", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Ingredient_ID"))) Then
                        ingredientBO.ItemNutritionId = results.GetInt32(results.GetOrdinal("ItemNutritionId"))
                        ingredientBO.ScaleIngredientID = results.GetInt32(results.GetOrdinal("Scale_Ingredient_ID"))
                        ingredientBO.Description = results.GetString(results.GetOrdinal("Description"))
                        ingredientBO.Ingredients = results.GetString(results.GetOrdinal("Ingredients"))

                    End If
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return ingredientBO

        End Function
        Public Shared Function GetAllergenForNonScaleItemByItem(ByVal item_Key As Integer) As ItemAllergenBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim allergenBO As New ItemAllergenBO

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Item_GetAllergensByItem", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Allergen_ID"))) Then
                        allergenBO.ItemNutritionId = results.GetInt32(results.GetOrdinal("ItemNutritionId"))
                        allergenBO.ScaleAllergenID = results.GetInt32(results.GetOrdinal("Scale_Allergen_ID"))
                        allergenBO.Description = results.GetString(results.GetOrdinal("Description"))
                        allergenBO.Allergens = results.GetString(results.GetOrdinal("Allergens"))

                    End If
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return allergenBO

        End Function
        Public Shared Function GetExtraText(ByVal ScaleExtraTextID As Integer) As ScaleExtraTextBO

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim extraTextBO As New ScaleExtraTextBO

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ScaleExtraTextID"
                currentParam.Value = ScaleExtraTextID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetExtraText", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText_ID"))) Then
                        extraTextBO.Description = results.GetString(results.GetOrdinal("Description"))
                        extraTextBO.ExtraText = results.GetString(results.GetOrdinal("ExtraText"))
                        extraTextBO.ID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
                        extraTextBO.Scale_LabelType_ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                    End If
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return extraTextBO

        End Function
        Public Shared Function GetItemScaleCountWithExtraText(ByVal ItemScale_ID As Integer) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CInt(factory.ExecuteScalar(String.Format("SELECT COUNT(*) FROM ItemScale WHERE Scale_ExtraText_ID = {0}", ItemScale_ID))) _
                + CInt(factory.ExecuteScalar(String.Format("SELECT COUNT(*) FROM ItemScaleOverride WHERE Scale_ExtraText_ID = {0}", ItemScale_ID)))
        End Function
        Public Shared Function Save(ByVal scaleExtraText As ScaleExtraTextBO) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleExtraText.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Scale_LabelType_ID"
                currentParam.Value = scaleExtraText.Scale_LabelType_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleExtraText.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ExtraText"
                currentParam.Value = scaleExtraText.ExtraText
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' output parameter for newly inserted ID value
                currentParam = New DBParam
                currentParam.Name = "NEW_ID"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("Scale_InsertUpdateExtraText", paramList)

            Catch ex As Exception
                Throw ex
            End Try

        End Function
        Public Shared Function Add(ByVal scaleExtraText As ScaleExtraTextBO) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As ArrayList = Nothing
            Dim isSuccess As Boolean = True
            Dim newID As Integer = 0

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleExtraText.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleExtraText.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.ExecuteStoredProcedure("Scale_CheckForDuplicateExtraText", paramList)
                If results.Count > 0 Then
                    isSuccess = False
                End If

                If isSuccess Then
                    currentParam = New DBParam
                    currentParam.Name = "Scale_LabelType_ID"
                    currentParam.Value = scaleExtraText.Scale_LabelType_ID
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ExtraText"
                    currentParam.Value = scaleExtraText.ExtraText
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    ' output parameter for newly inserted ID value
                    currentParam = New DBParam
                    currentParam.Name = "NEW_ID"
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    results = factory.ExecuteStoredProcedure("Scale_InsertUpdateExtraText", paramList)

                    newID = CInt(results.Item(0))

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return newID

        End Function

    End Class

End Namespace
