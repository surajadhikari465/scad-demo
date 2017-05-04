Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ItemSignAttributeDAO

        Private Shared _instance As ItemSignAttributeDAO = Nothing

        Public Shared ReadOnly Property Instance() As ItemSignAttributeDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New ItemSignAttributeDAO()
                End If

                Return _instance
            End Get
        End Property

        ''' <summary>
        ''' Returns the item key for the provided item identifier.
        ''' </summary>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetItemKeyByIdentifier(ByVal identifier As String) As Integer
            Dim itemKey As Integer = -1
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                currentParam.Value = identifier
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetItemInfoByIdentifier", paramList)

                If results.Read Then
                    itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return itemKey
        End Function

        Public Shared Function GetItemSignAttributeByItemKey(ByRef itemKey As System.Int32) As ItemSignAttributeBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim itemSignAttributeBO As New ItemSignAttributeBO

            Try
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetItemSignAttributeByItemKey", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("ItemSignAttributeID"))) Then
                        itemSignAttributeBO.ItemSignAttributeID = results.GetInt32(results.GetOrdinal("ItemSignAttributeID"))
                        itemSignAttributeBO.ItemKey = results.GetInt32(results.GetOrdinal("Item_Key"))

                        If results.IsDBNull(results.GetOrdinal("Locality")) Then
                            itemSignAttributeBO.Locality = String.Empty
                        Else
                            itemSignAttributeBO.Locality = results.GetString(results.GetOrdinal("Locality"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("SignRomanceTextLong")) Then
                            itemSignAttributeBO.SignRomanceLong = String.Empty
                        Else
                            itemSignAttributeBO.SignRomanceLong = results.GetString(results.GetOrdinal("SignRomanceTextLong"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("SignRomanceTextShort")) Then
                            itemSignAttributeBO.SignRomanceShort = String.Empty
                        Else
                            itemSignAttributeBO.SignRomanceShort = results.GetString(results.GetOrdinal("SignRomanceTextShort"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("AirChilled")) Then
                            itemSignAttributeBO.AirChilled = results.GetBoolean(results.GetOrdinal("AirChilled"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("AnimalWelfareRating")) Then
                            itemSignAttributeBO.AnimalWelfareRating = String.Empty
                        Else
                            itemSignAttributeBO.AnimalWelfareRating = results.GetString(results.GetOrdinal("AnimalWelfareRating"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Biodynamic")) Then
                            itemSignAttributeBO.Biodynamic = results.GetBoolean(results.GetOrdinal("Biodynamic"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("CheeseMilkType")) Then
                            itemSignAttributeBO.CheeseMilkType = String.Empty
                        Else
                            itemSignAttributeBO.CheeseMilkType = results.GetString(results.GetOrdinal("CheeseMilkType"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("CheeseRaw")) Then
                            itemSignAttributeBO.CheeseRaw = results.GetBoolean(results.GetOrdinal("CheeseRaw"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("EcoScaleRating")) Then
                            itemSignAttributeBO.EcoScaleRating = String.Empty
                        Else
                            itemSignAttributeBO.EcoScaleRating = results.GetString(results.GetOrdinal("EcoScaleRating"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("GlutenFree")) Then
                            itemSignAttributeBO.GlutenFree = results.GetBoolean(results.GetOrdinal("GlutenFree"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("HealthyEatingRating")) Then
                            itemSignAttributeBO.HealthyEatingRating = String.Empty
                        Else
                            itemSignAttributeBO.HealthyEatingRating = results.GetString(results.GetOrdinal("HealthyEatingRating"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Kosher")) Then
                            itemSignAttributeBO.Kosher = results.GetBoolean(results.GetOrdinal("Kosher"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("NonGmo")) Then
                            itemSignAttributeBO.NonGmo = results.GetBoolean(results.GetOrdinal("NonGmo"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("FreshOrFrozen")) Then
                            itemSignAttributeBO.FreshOrFrozen = String.Empty
                        Else
                            itemSignAttributeBO.FreshOrFrozen = results.GetString(results.GetOrdinal("FreshOrFrozen"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("SeafoodCatchType")) Then
                            itemSignAttributeBO.SeafoodCatchType = String.Empty
                        Else
                            itemSignAttributeBO.SeafoodCatchType = results.GetString(results.GetOrdinal("SeafoodCatchType"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Vegan")) Then
                            itemSignAttributeBO.Vegan = results.GetBoolean(results.GetOrdinal("Vegan"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Vegetarian")) Then
                            itemSignAttributeBO.Vegetarian = results.GetBoolean(results.GetOrdinal("Vegetarian"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("WholeTrade")) Then
                            itemSignAttributeBO.WholeTrade = results.GetBoolean(results.GetOrdinal("WholeTrade"))
                        End If

                        If results.IsDBNull(results.GetOrdinal("UomRegulationChicagoBaby")) Then
                            itemSignAttributeBO.ChicagoBaby = String.Empty
                        Else
                            itemSignAttributeBO.ChicagoBaby = results.GetString(results.GetOrdinal("UomRegulationChicagoBaby"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("UomRegulationTagUom")) Then
                            itemSignAttributeBO.TagUom = results.GetInt32(results.GetOrdinal("UomRegulationTagUom"))
                        Else
                            itemSignAttributeBO.TagUom = Nothing
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Msc")) Then
                            itemSignAttributeBO.Msc = results.GetBoolean(results.GetOrdinal("Msc"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("GrassFed")) Then
                            itemSignAttributeBO.GrassFed = results.GetBoolean(results.GetOrdinal("GrassFed"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("PastureRaised")) Then
                            itemSignAttributeBO.PastureRaised = results.GetBoolean(results.GetOrdinal("PastureRaised"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("PremiumBodyCare")) Then
                            itemSignAttributeBO.PremiumBodyCare = results.GetBoolean(results.GetOrdinal("PremiumBodyCare"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("FreeRange")) Then
                            itemSignAttributeBO.FreeRange = results.GetBoolean(results.GetOrdinal("FreeRange"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("DryAged")) Then
                            itemSignAttributeBO.DryAged = results.GetBoolean(results.GetOrdinal("DryAged"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("MadeInHouse")) Then
                            itemSignAttributeBO.MadeInHouse = results.GetBoolean(results.GetOrdinal("MadeInHouse"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("Exclusive")) Then
                            itemSignAttributeBO.Exclusive = results.GetDateTime(results.GetOrdinal("Exclusive"))
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("ColorAdded")) Then
                            itemSignAttributeBO.ColorAdded = results.GetBoolean(results.GetOrdinal("ColorAdded"))
                        End If
                    End If
                End While
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return itemSignAttributeBO
        End Function

        Public Shared Function Save(ByVal itemSignAttribute As ItemSignAttributeBO) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemSignAttribute.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Locality"
            If String.IsNullOrEmpty(itemSignAttribute.Locality) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.Locality
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SignRomanceTextLong"
            If String.IsNullOrEmpty(itemSignAttribute.SignRomanceLong) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.SignRomanceLong
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SignRomanceTextShort"
            If String.IsNullOrEmpty(itemSignAttribute.SignRomanceShort) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.SignRomanceShort
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Exclusive"
            If IsNothing(itemSignAttribute.Exclusive) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.Exclusive
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ColorAdded"
            If IsNothing(itemSignAttribute.ColorAdded) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.ColorAdded
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UomRegulationChicagoBaby"
            If String.IsNullOrEmpty(itemSignAttribute.ChicagoBaby) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.ChicagoBaby
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UomRegulationTagUom"
            If IsNothing(itemSignAttribute.TagUom) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemSignAttribute.TagUom
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.InsertOrUpdateItemSignAttribute", paramList)
        End Function
    End Class
End Namespace
