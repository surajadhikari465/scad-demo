Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ScaleDetailsDAO

#Region "read methods"

        ''' <summary>
        ''' returns TRUE or FALSE if identifier passed in matches criteria for a scale identifier
        ''' </summary>
        ''' <param name="identifier"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsScaleIdentifier(ByVal identifier As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_IsScaleIdentifier('" & identifier & "')"), Boolean)
        End Function

        Public Shared Sub GetScaleDetailCombos(ByRef scaleDetailsBO As ScaleDetailsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleEatBy As ScaleEatByBO
            Dim scaleGrade As ScaleGradeBO
            Dim scaleLabelStyle As ScaleLabelStyleBO
            Dim scaleNutrifact As ScaleNutrifactBO
            Dim scaleRandomWeightType As ScaleRandomWeightTypeBO
            Dim scaleTare As ScaleTareBO
            Dim scaleUOM As ScaleUOMsBO
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetScaleDetailCombos")

                While results.Read
                    scaleEatBy = New ScaleEatByBO()
                    scaleEatBy.ID = results.GetInt32(results.GetOrdinal("Scale_EatBy_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleEatBy.Description = ""
                    Else
                        scaleEatBy.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleEatBy(scaleEatBy)
                End While

                results.NextResult()
                While results.Read
                    scaleGrade = New ScaleGradeBO()
                    scaleGrade.ID = results.GetInt32(results.GetOrdinal("Scale_Grade_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleGrade.Description = ""
                    Else
                        scaleGrade.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleGrade(scaleGrade)
                End While

                results.NextResult()
                While results.Read
                    scaleLabelStyle = New ScaleLabelStyleBO()
                    scaleLabelStyle.ID = results.GetInt32(results.GetOrdinal("Scale_LabelStyle_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleLabelStyle.Description = ""
                    Else
                        scaleLabelStyle.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleLabelStyle(scaleLabelStyle)
                End While

                results.NextResult()
                While results.Read
                    scaleNutrifact = New ScaleNutrifactBO()
                    scaleNutrifact.ID = results.GetInt32(results.GetOrdinal("NutrifactsID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleNutrifact.Description = ""
                    Else
                        scaleNutrifact.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleNutrifact(scaleNutrifact)
                End While

                results.NextResult()
                While results.Read
                    scaleRandomWeightType = New ScaleRandomWeightTypeBO()
                    scaleRandomWeightType.ID = results.GetInt32(results.GetOrdinal("Scale_RandomWeightType_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleRandomWeightType.Description = ""
                    Else
                        scaleRandomWeightType.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleRandomWeightType(scaleRandomWeightType)
                End While

                results.NextResult()
                While results.Read
                    scaleTare = New ScaleTareBO()
                    scaleTare.ID = results.GetInt32(results.GetOrdinal("Scale_Tare_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleTare.Description = ""
                    Else
                        scaleTare.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleTare(scaleTare)
                    scaleDetailsBO.Add_ScaleTareAlternate(scaleTare)
                End While

                results.NextResult()
                While results.Read
                    scaleUOM = New ScaleUOMsBO()
                    scaleUOM.ID = results.GetInt32(results.GetOrdinal("Unit_ID"))

                    If results.IsDBNull(results.GetOrdinal("Description")) Then
                        scaleUOM.Description = ""
                    Else
                        scaleUOM.Description = results.GetString(results.GetOrdinal("Description"))
                    End If

                    scaleDetailsBO.Add_ScaleUOM(scaleUOM)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

        End Sub

        Public Shared Sub GetScaleDetails(ByRef scaleDetailsBO As ScaleDetailsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = scaleDetailsBO.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetItemScaleDetails", paramList)

                ' Map all DB NULL id values to -1 so that the pull down boxes are properly initialized.
                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("ItemScale_ID"))) Then
                        scaleDetailsBO.ItemScaleID = results.GetInt32(results.GetOrdinal("ItemScale_ID"))
                    Else
                        scaleDetailsBO.ItemScaleID = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Nutrifact_ID"))) Then
                        scaleDetailsBO.Nutrifact = results.GetInt32(results.GetOrdinal("Nutrifact_ID"))
                    Else
                        scaleDetailsBO.Nutrifact = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText_ID"))) Then
                        scaleDetailsBO.ExtraTextID = results.GetInt32(results.GetOrdinal("Scale_ExtraText_ID"))
                    Else
                        scaleDetailsBO.ExtraTextID = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ExtraText"))) Then
                        scaleDetailsBO.ExtraText = results.GetString(results.GetOrdinal("Scale_ExtraText"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Ingredient_ID"))) Then
                        scaleDetailsBO.IngredientID = results.GetInt32(results.GetOrdinal("Scale_Ingredient_ID"))
                    Else
                        scaleDetailsBO.IngredientID = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Ingredient"))) Then
                        scaleDetailsBO.Ingredient = results.GetString(results.GetOrdinal("Scale_Ingredient"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Allergen_ID"))) Then
                        scaleDetailsBO.AllergenID = results.GetInt32(results.GetOrdinal("Scale_Allergen_ID"))
                    Else
                        scaleDetailsBO.AllergenID = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Allergen"))) Then
                        scaleDetailsBO.Allergen = results.GetString(results.GetOrdinal("Scale_Allergen"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Tare_ID"))) Then
                        scaleDetailsBO.Tare = results.GetInt32(results.GetOrdinal("Scale_Tare_ID"))
                    Else
                        scaleDetailsBO.Tare = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Alternate_Tare_ID"))) Then
                        scaleDetailsBO.TareAlternate = results.GetInt32(results.GetOrdinal("Scale_Alternate_Tare_ID"))
                    Else
                        scaleDetailsBO.TareAlternate = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_EatBy_ID"))) Then
                        scaleDetailsBO.EatBy = results.GetInt32(results.GetOrdinal("Scale_EatBy_ID"))
                    Else
                        scaleDetailsBO.EatBy = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Grade_ID"))) Then
                        scaleDetailsBO.Grade = results.GetInt32(results.GetOrdinal("Scale_Grade_ID"))
                    Else
                        scaleDetailsBO.Grade = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_LabelStyle_ID"))) Then
                        scaleDetailsBO.LabelStyle = results.GetInt32(results.GetOrdinal("Scale_LabelStyle_ID"))
                    Else
                        scaleDetailsBO.LabelStyle = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_RandomWeightType_ID"))) Then
                        scaleDetailsBO.RandomWeightType = results.GetInt32(results.GetOrdinal("Scale_RandomWeightType_ID"))
                    Else
                        scaleDetailsBO.RandomWeightType = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ScaleUOMUnit_ID"))) Then
                        scaleDetailsBO.UOM = results.GetInt32(results.GetOrdinal("Scale_ScaleUOMUnit_ID"))
                    Else
                        scaleDetailsBO.UOM = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_FixedWeight"))) Then
                        scaleDetailsBO.FixedWeight = results.GetString(results.GetOrdinal("Scale_FixedWeight"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ByCount"))) Then
                        scaleDetailsBO.ByCount = results.GetInt32(results.GetOrdinal("Scale_ByCount"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ForceTare"))) Then
                        scaleDetailsBO.ForceTare = results.GetBoolean(results.GetOrdinal("ForceTare"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankShelfLife"))) Then
                        scaleDetailsBO.PrintBlankShelfLife = results.GetBoolean(results.GetOrdinal("PrintBlankShelfLife"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankEatBy"))) Then
                        scaleDetailsBO.PrintBlankShelfEatBy = results.GetBoolean(results.GetOrdinal("PrintBlankEatBy"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankPackDate"))) Then
                        scaleDetailsBO.PrintBlankPackDate = results.GetBoolean(results.GetOrdinal("PrintBlankPackDate"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankWeight"))) Then
                        scaleDetailsBO.PrintBlankWeight = results.GetBoolean(results.GetOrdinal("PrintBlankWeight"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankUnitPrice"))) Then
                        scaleDetailsBO.PrintBlankUnitPrice = results.GetBoolean(results.GetOrdinal("PrintBlankUnitPrice"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrintBlankTotalPrice"))) Then
                        scaleDetailsBO.PrintBlankTotalPrice = results.GetBoolean(results.GetOrdinal("PrintBlankTotalPrice"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Description1"))) Then
                        scaleDetailsBO.ScaleDescription1 = results.GetString(results.GetOrdinal("Scale_Description1"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Description2"))) Then
                        scaleDetailsBO.ScaleDescription2 = results.GetString(results.GetOrdinal("Scale_Description2"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Description3"))) Then
                        scaleDetailsBO.ScaleDescription3 = results.GetString(results.GetOrdinal("Scale_Description3"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_Description4"))) Then
                        scaleDetailsBO.ScaleDescription4 = results.GetString(results.GetOrdinal("Scale_Description4"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ShelfLife_Length"))) Then
                        scaleDetailsBO.ShelfLifeLength = CInt(results.GetValue(results.GetOrdinal("ShelfLife_Length")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("CustomerFacingScaleDepartment"))) Then
                        scaleDetailsBO.CustomerFacingScaleDepartment = CBool(results.GetValue(results.GetOrdinal("CustomerFacingScaleDepartment")))
                    Else
                        scaleDetailsBO.CustomerFacingScaleDepartment = Nothing
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("SendToScale"))) Then
                        scaleDetailsBO.SendToScale = CBool(results.GetValue(results.GetOrdinal("SendToScale")))
                    Else
                        scaleDetailsBO.SendToScale = Nothing
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

        End Sub
#End Region

#Region "Write methods"

        Public Shared Sub SaveScaleData(ByVal scaleDetailsBO As ScaleDetailsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ItemScale_ID"
            currentParam.Value = scaleDetailsBO.ItemScaleID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = scaleDetailsBO.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Nutrifact_ID"
            If scaleDetailsBO.Nutrifact > 0 Then
                currentParam.Value = scaleDetailsBO.Nutrifact
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ExtraText_ID"
            If scaleDetailsBO.ExtraTextID > -1 Then
                currentParam.Value = scaleDetailsBO.ExtraTextID
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Tare_ID"
            If scaleDetailsBO.Tare > -1 Then
                currentParam.Value = scaleDetailsBO.Tare
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Alternate_Tare_ID"
            If scaleDetailsBO.TareAlternate > -1 Then
                currentParam.Value = scaleDetailsBO.TareAlternate
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_LabelStyle_ID"
            If scaleDetailsBO.LabelStyle > -1 Then
                currentParam.Value = scaleDetailsBO.LabelStyle
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_EatBy_ID"
            If scaleDetailsBO.EatBy > -1 Then
                currentParam.Value = scaleDetailsBO.EatBy
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Grade_ID"
            If scaleDetailsBO.Grade > -1 Then
                currentParam.Value = scaleDetailsBO.Grade
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_RandomWeightType_ID"
            If scaleDetailsBO.RandomWeightType > -1 Then
                currentParam.Value = scaleDetailsBO.RandomWeightType
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ScaleUOMUnit_ID"
            If scaleDetailsBO.UOM > -1 Then
                currentParam.Value = scaleDetailsBO.UOM
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_FixedWeight"
            If scaleDetailsBO.FixedWeight.Length > 0 Then
                currentParam.Value = scaleDetailsBO.FixedWeight
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ByCount"
            If scaleDetailsBO.ByCount < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = scaleDetailsBO.ByCount
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ForceTare"
            currentParam.Value = scaleDetailsBO.ForceTare
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankShelfLife"
            currentParam.Value = scaleDetailsBO.PrintBlankShelfLife
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankEatBy"
            currentParam.Value = scaleDetailsBO.PrintBlankShelfEatBy
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankPackDate"
            currentParam.Value = scaleDetailsBO.PrintBlankPackDate
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankWeight"
            currentParam.Value = scaleDetailsBO.PrintBlankWeight
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankUnitPrice"
            currentParam.Value = scaleDetailsBO.PrintBlankUnitPrice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankTotalPrice"
            currentParam.Value = scaleDetailsBO.PrintBlankTotalPrice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description1"
            If scaleDetailsBO.ScaleDescription1.Length > 0 Then
                currentParam.Value = scaleDetailsBO.ScaleDescription1
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description2"
            If scaleDetailsBO.ScaleDescription2.Length > 0 Then
                currentParam.Value = scaleDetailsBO.ScaleDescription2
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description3"
            If scaleDetailsBO.ScaleDescription3.Length > 0 Then
                currentParam.Value = scaleDetailsBO.ScaleDescription3
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description4"
            If scaleDetailsBO.ScaleDescription4.Length > 0 Then
                currentParam.Value = scaleDetailsBO.ScaleDescription4
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ShelfLife_Length"
            If scaleDetailsBO.ShelfLifeLength < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = scaleDetailsBO.ShelfLifeLength
            End If
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID_Date"
            currentParam.Value = Now
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CustomerFacingScaleDepartment"
            currentParam.Value = If(scaleDetailsBO.CustomerFacingScaleDepartment.HasValue, scaleDetailsBO.CustomerFacingScaleDepartment.Value, DBNull.Value)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SendToScale"
            currentParam.Value = If(scaleDetailsBO.SendToScale.HasValue, scaleDetailsBO.SendToScale.Value, DBNull.Value)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Scale_InsertUpdateItemScaleDetails", paramList)

            ' If SendToScale or CustomerFacingScaleDepartment have a value then this is a 365 non-scale PLU, and nutrifacts and extra text changes should not be queued up.
            If Not (scaleDetailsBO.SendToScale.HasValue Or scaleDetailsBO.CustomerFacingScaleDepartment.HasValue) Then
                InsertNutriFactsChgQueue(scaleDetailsBO)
                InsertExtraTextChgQueue(scaleDetailsBO)
            End If
        End Sub

        Friend Shared Sub GenerateCustomerFacingScaleMaintenance(itemKey As Integer, actionCode As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ActionCode"
            currentParam.Value = actionCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("GenerateCustomerFacingScaleMaintenance", paramList)
        End Sub

        Public Shared Sub InsertNutriFactsChgQueue(ByVal scaleDetailsBO As ScaleDetailsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "NutriFactsID"
            If scaleDetailsBO.Nutrifact > -1 Then
                currentParam.Value = scaleDetailsBO.Nutrifact
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ActionCode"
            currentParam.Value = "A"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_InsertNutriFactsChgQueue", paramList)
        End Sub

        Public Shared Sub InsertExtraTextChgQueue(ByVal scaleDetailsBO As ScaleDetailsBO)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ScaleExtraTextID"
            If scaleDetailsBO.ExtraTextID > -1 Then
                currentParam.Value = scaleDetailsBO.ExtraTextID
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ActionCode"
            currentParam.Value = "A"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_InsertScaleExtraTextChgQueue", paramList)
        End Sub

#End Region

    End Class

End Namespace