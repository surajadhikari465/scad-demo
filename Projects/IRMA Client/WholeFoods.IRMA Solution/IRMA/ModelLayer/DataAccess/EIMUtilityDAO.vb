

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Custom Data Access Object class for EIM utility stored procs.
    '''
    ''' Created By:	David Marine
    ''' Created   :	July 23, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EIMUtilityDAO

#Region "Static Singleton Accessor"

        Private Shared _instance As EIMUtilityDAO = Nothing

        Public Shared ReadOnly Property Instance() As EIMUtilityDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New EIMUtilityDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Looks up and returns the hierarchy ids
        ''' based on the names.
        ''' This is not as simple as it seems since the names of
        ''' all but the subteam are not guaranteed to be unique.
        ''' This requires looking up the category and level 3 and 4
        ''' ids by name and by the found id of the parent.
        ''' </summary>
        ''' <param name="inUploadRow"></param>
        ''' <remarks></remarks>
        Public Overridable Sub LookUpHierarchyIds(
                ByRef inUploadRow As UploadRow)

            Dim theSubteamName As String = "NO_NAME"
            Dim theCategoryName As String = "NO_NAME"
            Dim theLevel3Name As String = "NO_NAME"
            Dim theLevel4Name As String = "NO_NAME"

            Dim theSubteamId As Integer = -1
            Dim theCategoryId As Integer = -1
            Dim theLevel3Id As Integer = -1
            Dim theLevel4Id As Integer = -1

            Dim theSubTeamUploadValue As UploadValue =
                     inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY)

            Dim theCategoryUploadValue As UploadValue =
                    inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_CATEGORY_ID_ATTR_KEY)

            Dim theLevel3UploadValue As UploadValue =
                    inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_LEVEL_3_ATTR_KEY)

            Dim theLevel4UploadValue As UploadValue =
                    inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.ITEM_LEVEL_4_ATTR_KEY)

            If Not IsNothing(theSubTeamUploadValue) Then
                theSubteamName = CStr(IIf(IsNothing(theSubTeamUploadValue.Value), Nothing, theSubTeamUploadValue.Value))
            End If

            If Not IsNothing(theCategoryUploadValue) Then
                theCategoryName = CStr(IIf(IsNothing(theCategoryUploadValue.Value), Nothing, theCategoryUploadValue.Value))
            End If

            If Not IsNothing(theLevel3UploadValue) Then
                theLevel3Name = CStr(IIf(IsNothing(theLevel3UploadValue.Value), Nothing, theLevel3UploadValue.Value))
            End If

            If Not IsNothing(theLevel4UploadValue) Then
                theLevel4Name = CStr(IIf(IsNothing(theLevel4UploadValue.Value), Nothing, theLevel4UploadValue.Value))
            End If

            LookUpHierarchyIds(theSubteamName, theCategoryName, theLevel3Name, theLevel4Name,
                theSubteamId, theCategoryId, theLevel3Id, theLevel4Id)

            If Not IsNothing(theSubTeamUploadValue) Then
                theSubTeamUploadValue.Value = CStr(IIf(theSubteamId > -1, theSubteamId, Nothing))
            End If

            If Not IsNothing(theCategoryUploadValue) Then
                theCategoryUploadValue.Value = CStr(IIf(theCategoryId > -1, theCategoryId, Nothing))
            End If

            If Not IsNothing(theLevel3UploadValue) Then
                theLevel3UploadValue.Value = CStr(IIf(theLevel3Id > -1, theLevel3Id, Nothing))
            End If

            If Not IsNothing(theLevel4UploadValue) Then
                theLevel4UploadValue.Value = CStr(IIf(theLevel4Id > -1, theLevel4Id, Nothing))
            End If

        End Sub

        ''' <summary>
        ''' Looks up and returns the hierarchy ids
        ''' based on the names.
        ''' This is not as simple as it seems since the names of
        ''' all but the subteam are not guaranteed to be unique.
        ''' This requires looking up the category and level 3 and 4
        ''' ids by name and by the found id of the parent.
        ''' </summary>
        ''' <param name="inSubteamName"></param>
        ''' <param name="inCategoryName"></param>
        ''' <param name="inLevel3Name"></param>
        ''' <param name="inLevel4Name"></param>
        ''' <param name="outSubteamId"></param>
        ''' <param name="outCategoryId"></param>
        ''' <param name="outLevel3Id"></param>
        ''' <param name="outLevel4Id"></param>
        ''' <remarks></remarks>
        Public Overridable Sub LookUpHierarchyIds(
                ByVal inSubteamName As String,
                ByVal inCategoryName As String,
                ByVal inLevel3Name As String,
                ByVal inLevel4Name As String,
                ByRef outSubteamId As Integer,
                ByRef outCategoryId As Integer,
                ByRef outLevel3Id As Integer,
                ByRef outLevel4Id As Integer)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim resultList As ArrayList

            ' setup the incoming params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "SubteamName"
            currentParam.Type = DBParamType.String
            If IsNothing(inSubteamName) Or String.IsNullOrEmpty(inSubteamName) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = inSubteamName
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CategoryName"
            currentParam.Type = DBParamType.String
            If IsNothing(inCategoryName) Or String.IsNullOrEmpty(inCategoryName) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = inCategoryName
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level3Name"
            currentParam.Type = DBParamType.String
            If IsNothing(inLevel3Name) Or String.IsNullOrEmpty(inLevel3Name) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = inLevel3Name
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level4Name"
            currentParam.Type = DBParamType.String
            If IsNothing(inLevel4Name) Or String.IsNullOrEmpty(inLevel4Name) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = inLevel4Name
            End If
            paramList.Add(currentParam)

            ' setup the output params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "SubteamID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CategoryID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level3ID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level4ID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            resultList = factory.ExecuteStoredProcedure("EIM_LookUpHierarchyIds", paramList)

            outSubteamId = CInt(resultList(0))
            outCategoryId = CInt(resultList(1))
            outLevel3Id = CInt(resultList(2))
            outLevel4Id = CInt(resultList(3))

        End Sub


        ''' <summary>
        ''' Returns the item key for the provided item identifier.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub GetItemHierarchyByIdentifier(ByVal inIdentifier As String,
                ByRef outSubTeamNo As Integer,
                ByRef outCategoryId As Integer,
                ByRef outLevel3Id As Integer,
                ByRef outLevel4Id As Integer)

            Dim itemKey As Integer = -1
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup identifier for stored proc
                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                currentParam.Value = inIdentifier
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemHierarchyByIdentifier", paramList)

                If results.Read Then
                    If Not results.IsDBNull(results.GetOrdinal("SubTeam_No")) Then
                        outSubTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("Category_ID")) Then
                        outCategoryId = results.GetInt32(results.GetOrdinal("Category_ID"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("ProdHierarchyLevel3_ID")) Then
                        outLevel3Id = results.GetInt32(results.GetOrdinal("ProdHierarchyLevel3_ID"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("ProdHierarchyLevel4_ID")) Then
                        outLevel4Id = results.GetInt32(results.GetOrdinal("ProdHierarchyLevel4_ID"))
                    End If
                End If

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

        End Sub

        Public Overridable Function ValidatePriceChange(ByRef inUploadRow As UploadRow,
                ByVal inStoreList As String,
                ByRef inOutValidationCode As Integer,
                ByRef inOutValidationMessage As String
                ) As ArrayList

            Dim validationLevel As Integer = 0

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Dim thePriceChangeTypeId As Integer = CInt(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_CHANGE_TYPE_ATTR_KEY))
            Dim theMultiple As Integer
            Dim thePOSPrice As Decimal
            Dim theMSRPPrice As Decimal
            Dim theMSRPMultiple As Integer
            Dim theSaleMultiple As Integer
            Dim thePOSSalePrice As Decimal

            Dim theEndDate As DateTime
            Dim theEndDateValue As String = inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY)
            If IsNothing(theEndDateValue) OrElse String.IsNullOrEmpty(theEndDateValue) OrElse
                    Not DateTime.TryParse(theEndDateValue, theEndDate) Then
                theEndDate = DateTime.Today
            End If

            Dim theStartDate As DateTime

            ' get either the reg or promo price start date depending on the price change type
            If PriceChgType.IsPromoPriceChgType(thePriceChangeTypeId) Then
                theStartDate = CDate(inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY).Value)
            Else
                Dim theRegPriceStartDateUploadValue As UploadValue = inUploadRow.FindUploadValueByAttributeKey(EIM_Constants.PRICE_START_DATE_ATTR_KEY)

                If Not IsNothing(theRegPriceStartDateUploadValue) AndAlso
                        (Not IsNothing(theRegPriceStartDateUploadValue.Value) And Not String.IsNullOrEmpty(theRegPriceStartDateUploadValue.Value)) Then
                    theStartDate = CDate(theRegPriceStartDateUploadValue.Value)
                Else
                    ' default the reg start date to today
                    theStartDate = DateTime.Today
                End If
            End If

            ' setup identifier for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inUploadRow.ItemKey
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = thePriceChangeTypeId
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Type = DBParamType.DateTime
            currentParam.Value = theStartDate
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_End_Date"
            currentParam.Type = DBParamType.DateTime
            currentParam.Value = theEndDate
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Type = DBParamType.Int
            If Integer.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_MULTIPLE_ATTR_KEY), theMultiple) Then
                currentParam.Value = theMultiple
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Type = DBParamType.Money
            If Decimal.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_ATTR_KEY), thePOSPrice) Then
                currentParam.Value = thePOSPrice
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPPrice"
            currentParam.Type = DBParamType.Money
            If Decimal.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_MSRP_PRICE_ATTR_KEY), theMSRPPrice) Then
                currentParam.Value = theMSRPPrice
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPMultiple"
            currentParam.Type = DBParamType.Int
            If Integer.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_MSRP_MULTIPLE_ATTR_KEY), theMSRPMultiple) Then
                currentParam.Value = theMSRPMultiple
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Multiple"
            currentParam.Type = DBParamType.Int
            If Integer.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_PROMO_MULTIPLE_ATTR_KEY), theSaleMultiple) Then
                currentParam.Value = theSaleMultiple
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSSale_Price"
            currentParam.Type = DBParamType.Money
            If Decimal.TryParse(inUploadRow.FindValueByAttributeKey(EIM_Constants.PRICE_PROMO_ATTR_KEY), thePOSSalePrice) Then
                currentParam.Value = thePOSSalePrice
            Else
                currentParam.Value = DBNull.Value
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreList"
            currentParam.Type = DBParamType.String
            If IsNothing(inStoreList) Then
                currentParam.Value = ""
            Else
                currentParam.Value = inStoreList
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreListSeparator"
            currentParam.Type = DBParamType.String
            currentParam.Value = ","
            paramList.Add(currentParam)

            ' add the output params

            currentParam = New DBParam
            currentParam.Name = "ValidationLevel"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ValidationMessage"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Dim theOutputList As ArrayList = factory.ExecuteStoredProcedure("EIM_PriceChangeValidation", paramList)

            validationLevel = CInt(theOutputList(0))
            inOutValidationCode = CInt(theOutputList(1))
            inOutValidationMessage = CStr(theOutputList(2))

            Return theOutputList

        End Function

        Public Overridable Function ValidateDeleteVendor(ByVal Item_Key As Integer,
        ByVal Vendor_ID As Integer,
        ByRef StoreListForItem As String,
        ByVal Delete_Vendor As Integer) As Integer()

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.String
            currentParam.Value = Item_Key
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = Vendor_ID
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "StoreListForItem"
            currentParam.Type = DBParamType.String
            If IsNothing(StoreListForItem) Then
                currentParam.Value = ""
            Else
                currentParam.Value = StoreListForItem
            End If
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Delete_Vendor"
            currentParam.Type = DBParamType.Int
            currentParam.Value = Delete_Vendor
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("EIM_ValidateDeleteVendor", paramList)

            Dim WarningDeauthCount As Integer = 0 ' store count where items will be deauth
            Dim WarningSwapCount As Integer = 0   ' store count where prim vendor will swap
            Dim ErrorCount As Integer = 0         ' store count where primary vendor cannot be deleted due to multiple secondary vendors

            If results.Read Then
                If Not results.IsDBNull(results.GetOrdinal("WarningDeauthCount")) Then
                    WarningDeauthCount = results.GetInt32(results.GetOrdinal("WarningDeauthCount"))
                End If
                If Not results.IsDBNull(results.GetOrdinal("WarningSwapCount")) Then
                    WarningSwapCount = results.GetInt32(results.GetOrdinal("WarningSwapCount"))
                End If
                If Not results.IsDBNull(results.GetOrdinal("ErrorCount")) Then
                    ErrorCount = results.GetInt32(results.GetOrdinal("ErrorCount"))
                End If
            End If

            Dim ReturnResults(2) As Integer

            ReturnResults(0) = WarningDeauthCount
            ReturnResults(1) = WarningSwapCount
            ReturnResults(2) = ErrorCount

            Return ReturnResults
        End Function

        Public Function ValidateForPriceUploadCollision(
                                                        ByVal Item_Key As Integer,
                                                        ByVal Identifier As String,
                                                        ByVal StoreListForItem As String,
                                                        ByVal PriceChgTypeID As Integer,
                                                        ByVal PriceStartDate As String,
                                                        ByVal Sale_Start_Date As String,
                                                        ByVal Sale_End_Date As String,
                                                        ByVal Multiple As Integer,
                                                        ByVal POSPrice As String,
                                                        ByVal MSRPPrice As String,
                                                        ByVal MSRPMultiple As Integer,
                                                        ByVal POSSale_Price As String,
                                                        ByVal Sale_Multiple As Integer,
                                                        ByVal isPriceChange As Boolean) As String
            If IsNothing(PriceStartDate) Then
                PriceStartDate = ""
            End If

            If IsNothing(Sale_Start_Date) Then
                Sale_Start_Date = ""
            End If

            If IsNothing(Sale_End_Date) Then
                Sale_End_Date = ""
            End If

            If IsNothing(isPriceChange) Then
                isPriceChange = True
            End If

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            currentParam.Value = Item_Key
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Type = DBParamType.String
            currentParam.Value = Identifier
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreListForItem"
            currentParam.Type = DBParamType.String
            currentParam.Value = StoreListForItem
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = PriceChgTypeID
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceStartDate"
            currentParam.Type = DBParamType.String
            currentParam.Value = PriceStartDate
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Start_Date"
            currentParam.Type = DBParamType.String
            currentParam.Value = Sale_Start_Date
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_End_Date"
            currentParam.Type = DBParamType.String
            currentParam.Value = Sale_End_Date
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Type = DBParamType.Int
            If IsNothing(Multiple) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = Multiple
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Type = DBParamType.Money
            If IsNothing(POSPrice) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = POSPrice
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPPrice"
            currentParam.Type = DBParamType.Money
            If IsNothing(MSRPPrice) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = MSRPPrice
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPMultiple"
            currentParam.Type = DBParamType.Int
            If IsNothing(MSRPMultiple) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = MSRPMultiple
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSSale_Price"
            currentParam.Type = DBParamType.Money
            If IsNothing(POSSale_Price) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = POSSale_Price
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Multiple"
            currentParam.Type = DBParamType.Int
            If IsNothing(Sale_Multiple) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = Sale_Multiple
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "isPriceChange"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = isPriceChange
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ErrorCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceChangeValidationErrorMessage"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Dim theOutputList As ArrayList = factory.ExecuteStoredProcedure("EIM_ValidateForPriceUploadCollision", paramList)

            Dim ErrorCount As Integer = CInt(theOutputList(0))
            Dim PriceChangeErrorMessage As String = CStr(theOutputList(1))

            Return PriceChangeErrorMessage
        End Function

        Public Overridable Function ValidateJurisdiction(ByVal inItemKey As Integer,
                ByVal inIsDefaultJurisdiction As Boolean,
                ByVal inJurisdictionId As Integer
        ) As EIM_Constants.ValidationLevels

            Dim theValidationCode As Integer
            Dim theValidationLevel As EIM_Constants.ValidationLevels

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inItemKey
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsDefaultJurisdiction"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = inIsDefaultJurisdiction
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreJurisdictionId"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inJurisdictionId
            paramList.Add(currentParam)

            ' add the output params

            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Dim theOutputList As ArrayList = factory.ExecuteStoredProcedure("EIM_JurisdictionValidation", paramList)

            theValidationCode = CInt(theOutputList(0))

            If theValidationCode = 0 Then
                theValidationLevel = EIM_Constants.ValidationLevels.Valid
            Else
                theValidationLevel = EIM_Constants.ValidationLevels.Invalid
            End If

            Return theValidationLevel

        End Function

        Public Function ValidateVendorUOM(ByVal inItemKey As Integer, ByVal inStoreList As String,
                 ByVal inVendorUomID As Integer) As EIM_Constants.ValidationLevels

            Dim theValidationCode As Integer
            Dim theValidationLevel As EIM_Constants.ValidationLevels

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup the incoming params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inItemKey
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreList"
            currentParam.Type = DBParamType.String
            If IsNothing(inStoreList) Or String.IsNullOrEmpty(inStoreList) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = inStoreList
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorUomId"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inVendorUomID
            paramList.Add(currentParam)

            ' setup the output params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Dim theOutputList As ArrayList = factory.ExecuteStoredProcedure("EIM_ValidateVendorUOM", paramList)

            theValidationCode = CInt(theOutputList(0))

            If theValidationCode = 0 Then
                theValidationLevel = EIM_Constants.ValidationLevels.Valid
            Else
                theValidationLevel = EIM_Constants.ValidationLevels.Invalid
            End If

            Return theValidationLevel

        End Function

        Public Function ValidateCostedByWeightUOM(ByVal inUomID As Integer, ByVal inCheckWeightedUnit As Boolean) As EIM_Constants.ValidationLevels

            Dim theValidationCode As Integer
            Dim theValidationLevel As EIM_Constants.ValidationLevels

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup the incoming params for the stored proc

            currentParam = New DBParam
            currentParam.Name = "UomID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inUomID
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CheckWeightedUnit"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = inCheckWeightedUnit
            paramList.Add(currentParam)

            ' setup the output params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Dim theOutputList As ArrayList = factory.ExecuteStoredProcedure("EIM_ValidateCostedByWeightUnit", paramList)

            theValidationCode = CInt(theOutputList(0))

            If theValidationCode = 0 Then
                theValidationLevel = EIM_Constants.ValidationLevels.Valid
            Else
                theValidationLevel = EIM_Constants.ValidationLevels.Invalid
            End If

            Return theValidationLevel

        End Function


        Public Function ValidateItemDelete(ByVal inItemKey As Integer) As DataTable

            Dim dt As New DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup the incoming params for the stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inItemKey
            paramList.Add(currentParam)


            ' Execute the stored procedure 
            dt = factory.GetStoredProcedureDataTable("EIM_DeleteItemValidation", paramList)

            'theValidationCode = CInt(theOutputList(0))

            'If theValidationCode = 0 Then
            '    theValidationLevel = EIM_Constants.ValidationLevels.Valid
            'Else
            '    theValidationLevel = EIM_Constants.ValidationLevels.Invalid
            'End If

            Return dt


        End Function

#End Region

    End Class

End Namespace
