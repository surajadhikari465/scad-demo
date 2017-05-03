Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class StoreJurisdictionDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "read methods"

        ''' <summary>
        ''' Read complete list of StoreJurisdiction data and return ArrayList of StoreJurisdictionBO objects.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJurisdictionList() As ArrayList
            ' Pass in -1 as the default jurisdiction so all values will be returned
            Return GetJurisdictionList(-1)
        End Function

        ''' <summary>
        ''' Read complete list of StoreJurisdiction data and return ArrayList of StoreJurisdictionBO objects, except the
        ''' default jurisdiction is excluded from the return list.
        ''' </summary>
        ''' <param name="defaultJurisdictionId">Set to -1 to return all values; otherwise, set to ID for value to be excluded</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJurisdictionList(ByVal defaultJurisdictionId As Integer) As ArrayList

            logger.Debug("GetJurisdictionList Entry")

            Dim jurisdictionList As New ArrayList
            Dim jurisdictionBO As StoreJurisdictionBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoreJurisdictions")

                Dim currentJursidctionID As Integer
                While results.Read
                    currentJursidctionID = results.GetInt32(results.GetOrdinal("StoreJurisdictionID"))
                    ' Add the option to the list if all options should be returned or this is not the default value.
                    If (defaultJurisdictionId = -1) Or (defaultJurisdictionId <> currentJursidctionID) Then
                        jurisdictionBO = New StoreJurisdictionBO()
                        jurisdictionBO.StoreJurisdictionId = currentJursidctionID
                        jurisdictionBO.StoreJurisdictionDesc = results.GetString(results.GetOrdinal("StoreJurisdictionDesc"))
                        jurisdictionList.Add(jurisdictionBO)
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetJurisdictionList Exit")
            Return jurisdictionList

        End Function

        ''' <summary>
        ''' Read the store override data for a particular item and store jurisdiction.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="jurisdictionId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreOverrideData(ByVal itemKey As Integer, ByVal jurisdictionId As Integer) As SqlDataReader

            logger.Debug("GetStoreOverrideData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            ' Setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdictionID"
            currentParam.Value = jurisdictionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetItemOverride", paramList)

            logger.Debug("GetStoreOverrideData Exit")

            Return results
        End Function

        ''' <summary>
        ''' Read the retail package unit overrides for a particular item.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRetailUnitOverrideList(ByVal ItemKey As Integer) As DataSet
           
            logger.Debug("GetAllItemOverrides Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataSet = Nothing

            ' Setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataSet("GetAllItemOverrides", paramList)
            logger.Debug("GetAllItemOverrides Exit")
            Return results

        End Function

        ''' <summary>
        ''' Read the store scale override data for a particular item and store jurisdiction.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="jurisdictionId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreScaleOverrideData(ByVal itemKey As Integer, ByVal jurisdictionId As Integer) As SqlDataReader

            logger.Debug("GetStoreScaleOverrideData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            ' Setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdictionID"
            currentParam.Value = jurisdictionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("Scale_GetItemScaleOverride", paramList)

            logger.Debug("GetStoreScaleOverrideData Exit")
            Return results

        End Function

#End Region

#Region "Update/Save Methods"

        ''' <summary>
        ''' Save the store override data to the database for a particular item and store jurisdiction.
        ''' This stored procedure will insert a new record or update an existing record.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="jurisdictionId"></param>
        ''' <param name="overrideItemData"></param>
        ''' <param name="overridePOSData"></param>
        ''' <remarks></remarks>
        Public Shared Sub SaveStoreOverrideData(ByVal itemKey As Integer, ByVal jurisdictionId As Integer, ByRef overrideItemData As ItemBO, ByRef overridePOSData As POSItemBO)

            logger.Debug("SaveStoreOverrideData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            ' Setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdictionID"
            currentParam.Value = jurisdictionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Description"
            currentParam.Value = ConvertQuotes(overrideItemData.ItemDescription)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sign_Description"
            currentParam.Value = ConvertQuotes(overrideItemData.SignCaption)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Package_Desc1"
            currentParam.Value = overrideItemData.PackageDesc1
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Package_Desc2"
            currentParam.Value = overrideItemData.PackageDesc2
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Package_Unit_ID"
            If overrideItemData.PackageUnitID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.PackageUnitID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Retail_Unit_ID"
            If overrideItemData.RetailUnitID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.RetailUnitID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_Unit_ID"
            If overrideItemData.VendorUnitID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.VendorUnitID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Distribution_Unit_ID"
            If overrideItemData.DistributionUnitID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.DistributionUnitID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POS_Description"
            currentParam.Value = Replace(ConvertQuotes(overrideItemData.POSDescription), ",", "", , , CompareMethod.Text)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Food_Stamps"
            currentParam.Value = overridePOSData.FoodStamps
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Price_Required"
            currentParam.Value = overridePOSData.PriceRequired
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Quantity_Required"
            currentParam.Value = overridePOSData.QuantityRequired
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Manufacturing_Unit_ID"
            If overrideItemData.ManufacturingUnitID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.ManufacturingUnitID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "QtyProhibit"
            currentParam.Value = overridePOSData.QuantityProhibit
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GroupList"
            If overridePOSData.GroupList Is Nothing Or overridePOSData.GroupList.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.GroupList
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Case_Discount"
            currentParam.Value = overridePOSData.CaseDiscount
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Coupon_Multiplier"
            currentParam.Value = overridePOSData.CouponMultiplier
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Misc_Transaction_Sale"
            If overridePOSData.MiscTransactionSale Is Nothing Or overridePOSData.MiscTransactionSale.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.MiscTransactionSale
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Misc_Transaction_Refund"
            If overridePOSData.MiscTransactionRefund Is Nothing Or overridePOSData.MiscTransactionRefund.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.MiscTransactionRefund
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ice_Tare"
            If overridePOSData.IceTare Is Nothing Or overridePOSData.IceTare.Equals("") Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.IceTare
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Brand_ID"
            If overrideItemData.BrandID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.BrandID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Origin_Id"
            If overrideItemData.OriginID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.OriginID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CountryProc_Id"
            If overrideItemData.CountryOfProcID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.CountryOfProcID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SustainabilityRankingRequired"
            currentParam.Value = overrideItemData.SustainabilityRankingRequired
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SustainabilityRankingID"
            If overrideItemData.SustainabilityRankingID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.SustainabilityRankingID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelType_ID"
            If overrideItemData.LabelTypeID Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.LabelTypeID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CostedByWeight"
            currentParam.Value = overrideItemData.CostedByWeight
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Average_Unit_Weight"
            If overrideItemData.AverageUnitWeight = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.AverageUnitWeight
            End If
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ingredient"
            currentParam.Value = overrideItemData.Ingredient
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Recall_Flag"
            currentParam.Value = overrideItemData.Recall
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LockAuth"
            currentParam.Value = overrideItemData.LockAuth
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Not_Available"
            currentParam.Value = overrideItemData.NotAvailable
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Not_AvailableNote"
            If overrideItemData.NotAvailableNote = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideItemData.NotAvailableNote
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FSA_Eligible"
            currentParam.Value = overridePOSData.FSAEligible
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Product_Code"
            If overridePOSData.ProductCode = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.ProductCode
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Unit_Price_Category"
            If overridePOSData.UnitPriceCategory = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overridePOSData.UnitPriceCategory
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("InsertUpdateItemOverride", paramList)

            logger.Debug("SaveStoreOverrideData Exit")

        End Sub

        ''' <summary>
        ''' Save the store override data to the database for a particular item and store jurisdiction.
        ''' This stored procedure will insert a new record or update an existing record.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="jurisdictionId"></param>
        ''' <param name="overrideData"></param>
        ''' <remarks></remarks>
        Public Shared Sub SaveStoreScaleOverrideData(ByVal itemKey As Integer, ByVal jurisdictionId As Integer, ByRef overrideData As ScaleDetailsBO)

            logger.Debug("SaveStoreScaleOverrideData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            ' Setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdictionID"
            currentParam.Value = jurisdictionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description1"
            currentParam.Value = overrideData.ScaleDescription1
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description2"
            currentParam.Value = overrideData.ScaleDescription2
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description3"
            currentParam.Value = overrideData.ScaleDescription3
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Description4"
            currentParam.Value = overrideData.ScaleDescription4
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ExtraText_ID"
            If overrideData.ExtraTextID > -1 Then
                currentParam.Value = overrideData.ExtraTextID
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Tare_ID"
            If overrideData.Tare > -1 Then
                currentParam.Value = overrideData.Tare
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_LabelStyle_ID"
            If overrideData.LabelStyle > -1 Then
                currentParam.Value = overrideData.LabelStyle
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ScaleUOMUnit_ID"
            If overrideData.UOM > -1 Then
                currentParam.Value = overrideData.UOM
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_RandomWeightType_ID"
            If overrideData.RandomWeightType > -1 Then
                currentParam.Value = overrideData.RandomWeightType
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_FixedWeight"
            If overrideData.FixedWeight.Length > 0 Then
                currentParam.Value = overrideData.FixedWeight
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ByCount"
            If overrideData.ByCount < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideData.ByCount
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ShelfLife_Length"
            If overrideData.ShelfLifeLength < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = overrideData.ShelfLifeLength
            End If
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ForceTare"
            currentParam.Value = overrideData.ForceTare
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Alternate_Tare_ID"
            If overrideData.TareAlternate > -1 Then
                currentParam.Value = overrideData.TareAlternate
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_EatBy_ID"
            If overrideData.EatBy > -1 Then
                currentParam.Value = overrideData.EatBy
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_Grade_ID"
            If overrideData.Grade > -1 Then
                currentParam.Value = overrideData.Grade
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankEatBy"
            currentParam.Value = overrideData.PrintBlankShelfEatBy
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankPackDate"
            currentParam.Value = overrideData.PrintBlankPackDate
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankShelfLife"
            currentParam.Value = overrideData.PrintBlankShelfLife
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankTotalPrice"
            currentParam.Value = overrideData.PrintBlankTotalPrice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankUnitPrice"
            currentParam.Value = overrideData.PrintBlankUnitPrice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PrintBlankWeight"
            currentParam.Value = overrideData.PrintBlankWeight
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Nutrifact_ID"
            If overrideData.Nutrifact > -1 Then
                currentParam.Value = overrideData.Nutrifact
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("Scale_InsertUpdateItemScaleOverride", paramList)

            ' Queue the Nutrifact so that it is sent down to the scale systems.
            Try
                ScaleDetailsDAO.InsertNutriFactsChgQueue(overrideData)
            Catch ex As Exception
                Throw ex
            End Try

            ' Queue the ExtraText so that it is sent down to the scale systems.
            Try
                ScaleDetailsDAO.InsertExtraTextChgQueue(overrideData)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("SaveStoreScaleOverrideData Exit")

        End Sub

#End Region

    End Class

End Namespace