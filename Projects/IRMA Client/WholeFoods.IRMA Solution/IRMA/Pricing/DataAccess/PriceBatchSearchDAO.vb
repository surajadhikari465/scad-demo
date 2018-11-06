Imports System.Data.SqlClient
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Class PriceBatchSearchDAO

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetHeaderSearchData(ByRef headerSearchInfo As PriceBatchSearchBO, ByVal formName As String) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim maxLoop As Integer = 1000

            Try
                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Type = DBParamType.String
                currentParam.Value = headerSearchInfo.StoreList
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Type = DBParamType.Char
                currentParam.Value = headerSearchInfo.StoreListSeparator
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemChgTypeID"
                currentParam.Type = DBParamType.SmallInt
                If headerSearchInfo.ItemChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.ItemChgTypeID
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceChgTypeID"
                currentParam.Type = DBParamType.SmallInt
                If headerSearchInfo.PriceChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.PriceChgTypeID
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceBatchStatusID"
                currentParam.Type = DBParamType.SmallInt
                If headerSearchInfo.PriceBatchStatusID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.PriceBatchStatusID
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                currentParam.Type = DBParamType.Int
                If headerSearchInfo.SubTeamNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.SubTeamNo
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FromStartDate"
                currentParam.Type = DBParamType.DateTime
                If headerSearchInfo.StartDate = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.StartDate
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ToStartDate"
                currentParam.Type = DBParamType.DateTime
                If headerSearchInfo.EndDate = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.EndDate
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                If headerSearchInfo.Identifier IsNot Nothing Then
                    currentParam.Value = headerSearchInfo.Identifier
                Else
                    currentParam.Value = DBNull.Value
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Description"
                currentParam.Type = DBParamType.String
                If headerSearchInfo.ItemDescription IsNot Nothing Then
                    currentParam.Value = headerSearchInfo.ItemDescription
                Else
                    currentParam.Value = DBNull.Value
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchDescription"
                currentParam.Type = DBParamType.String
                If headerSearchInfo.BatchDescription IsNot Nothing Then
                    If headerSearchInfo.BatchDescription.Length > 0 Then
                        currentParam.Value = headerSearchInfo.BatchDescription
                    Else
                        currentParam.Value = DBNull.Value
                    End If
                Else
                    currentParam.Value = DBNull.Value
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AutoApplyFlag"
                currentParam.Type = DBParamType.Bit
                If headerSearchInfo.AutoApplyFlag IsNot Nothing Then
                    If headerSearchInfo.AutoApplyFlag.Length > 0 Then
                        currentParam.Value = CInt(headerSearchInfo.AutoApplyFlag)
                    Else
                        currentParam.Value = DBNull.Value
                    End If
                Else
                    currentParam.Value = DBNull.Value
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ApplyDate"
                currentParam.Type = DBParamType.DateTime
                If headerSearchInfo.AutoApplyDate = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = headerSearchInfo.AutoApplyDate
                End If
                paramList.Add(currentParam)

                factory.CommandTimeout = 1200
                Return factory.GetStoredProcedureDataTable("GetPriceBatchSearch", paramList)
            Catch e As Exception
                Throw
            End Try
        End Function

        Public Shared Function GetDetailSearchData(ByVal itemSearchInfo As PriceBatchSearchBO) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' this limit is in place because of the math performed in PriceBatchItemSearch during processing -
            ' anything larger causes an overflow exception
            Dim MaxLoop As Integer = 1073741823

            Try
                currentParam = New DBParam
                currentParam.Name = "StoreList"
                If itemSearchInfo.StoreList.Length > 0 Then
                    currentParam.Value = itemSearchInfo.StoreList
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = itemSearchInfo.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemChgTypeID"
                If itemSearchInfo.ItemChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemSearchInfo.ItemChgTypeID
                End If
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PriceChgTypeID"
                If itemSearchInfo.PriceChgTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemSearchInfo.PriceChgTypeID
                End If
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                If itemSearchInfo.SubTeamNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemSearchInfo.SubTeamNo
                End If
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                ' The start date is the date the batch is sent to the POS System for processing.
                ' For regions that DO NOT print shelf tags from IRMA, this causes an issue because the
                ' query only returns items with a start data equal to or before the search start date. 
                ' This does not allow for time to print shelf tags from the regional POS system.
                ' IF the region does not print shelf tags from IRMA, the start date search will return items
                ' that have a start date equal to or before the search start date PLUS 2 days.
                ' This means that when the prices are applied in IRMA by the POS Push process it is possible 
                ' that they will be off by 2 days (IRMA shows a current price that is really the price for
                ' 2 days in the future), but this is an acceptable side-effect to allow for the region to 
                ' process the item at the store.
                currentParam = New DBParam
                currentParam.Name = "StartDate"

                If InstanceDataDAO.IsFlagActive("TwoDayBatchingBuffer") AndAlso InstanceDataDAO.IsFlagActive("BypassPrintShelfTags") Then
                    currentParam.Value = DateAdd(DateInterval.Day, 2, itemSearchInfo.StartDate)
                Else
                    currentParam.Value = itemSearchInfo.StartDate
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                currentParam.Value = itemSearchInfo.Identifier
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Description"
                currentParam.Type = DBParamType.String
                currentParam.Value = itemSearchInfo.ItemDescription
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IncScaleItems"
                currentParam.Type = DBParamType.Bit
                currentParam.Value = itemSearchInfo.IncScaleItems
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IncNonRetailItems"
                currentParam.Type = DBParamType.Bit
                currentParam.Value = itemSearchInfo.IncNonRetailItems
                paramList.Add(currentParam)

                factory.CommandTimeout = 1200
                Return factory.GetStoredProcedureDataTable("GetPriceBatchItemSearch", paramList)
            Catch e As Exception
                Throw
            End Try
        End Function

        Public Shared Function PricingPrintSignsSearch(ByVal storeNumber As Integer, ByVal subteamNumber As String, ByVal categoryId As String,
                                   ByVal signDescription As String, ByVal identifiers As String, ByVal brandId As String) As DataTable
            Dim results As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "StoreNumber"
            currentParam.Type = DBParamType.Int
            currentParam.Value = storeNumber
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubteamNumber"
            currentParam.Type = DBParamType.Int
            If subteamNumber = String.Empty Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = subteamNumber
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CategoryId"
            currentParam.Type = DBParamType.Int
            If categoryId = String.Empty Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = categoryId
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SignDescription"
            currentParam.Type = DBParamType.String
            If signDescription = String.Empty Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = signDescription
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifiers"
            currentParam.Type = DBParamType.String
            If identifiers = String.Empty Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = identifiers
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BrandId"
            currentParam.Type = DBParamType.Int
            If brandId = String.Empty Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = brandId
            End If
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("PricingPrintSignsSearch", paramList)

            Return results
        End Function

        Public Shared Function GetCompetitiveDate(ByVal strIdentifiers As String) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim strCompCheckDate As String = Nothing

            logger.Debug("PriceBatch Comp Date Check entry: strIdentifiers=" + strIdentifiers)

            Try
                currentParam = New DBParam
                currentParam.Name = "keyList"
                currentParam.Value = strIdentifiers
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Output.
                currentParam = New DBParam
                currentParam.Name = "@CheckDate"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                outputList = factory.ExecuteStoredProcedure("GetCompetitiveBatchItemDateChecked", paramList)
                strCompCheckDate = CStr(outputList(0))

                logger.Debug("PriceBatch Comp Date Check exit: strCompCheckDate=" + strCompCheckDate.ToString())

                Return strCompCheckDate
            Catch ex As Exception
                Throw
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return strCompCheckDate
        End Function

        Public Shared Function GetCompetitivePriceTypeStatus(ByVal intPriceChgType As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim boolCompPriceChgTypeStatus As Boolean = Nothing

            logger.Debug("PriceBatch Comp PriceChgType Status Check entry: intPriceChgType=" + intPriceChgType.ToString())

            Try
                currentParam = New DBParam
                currentParam.Name = "@PriceChgTypeID"
                currentParam.Value = intPriceChgType
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Output.
                currentParam = New DBParam
                currentParam.Name = "@CompStatus"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                outputList = factory.ExecuteStoredProcedure("GetCompetitivePriceChgTypeStatus", paramList)
                boolCompPriceChgTypeStatus = CBool(outputList(0))

                logger.Debug("PriceBatch Comp PriceChgType Status Check exit: intPriceChgType=" + intPriceChgType.ToString())

                Return boolCompPriceChgTypeStatus
            Catch ex As Exception
                Throw
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return boolCompPriceChgTypeStatus
        End Function
    End Class
End Namespace
