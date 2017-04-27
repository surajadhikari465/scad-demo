Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Enum BatchStatus
        Building
        AllButProcessedAndBuilding
        AllButProcessed
    End Enum

    Public Class PriceBatchDetailDAO

        Public Shared Function GetDetailSearchData(ByVal priceBatchHeaderID As Integer, ByVal formName As String) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow
            Dim table As New DataTable("BatchDetail")
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Dim iLoop As Integer = 0
            Dim maxLoop As Short = 1000

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = priceBatchHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetPriceBatchDetail", paramList)

                'add columns to table
                table.Columns.Add(New DataColumn("Identifier", GetType(String)))
                table.Columns.Add(New DataColumn("Item_Description", GetType(String)))
                table.Columns.Add(New DataColumn("POSPrice", GetType(Decimal)))
                table.Columns.Add(New DataColumn("POSPriceWithMultiple", GetType(String)))
                table.Columns.Add(New DataColumn("StartDate", GetType(Date)))
                table.Columns.Add(New DataColumn("Sale_End_Date", GetType(Date)))
                table.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
                table.Columns.Add(New DataColumn("PriceBatchDetailID", GetType(Integer)))
                table.Columns.Add(New DataColumn("PrintSign", GetType(Boolean)))

                While results.Read And (iLoop < maxLoop)
                    iLoop = iLoop + 1

                    row = table.NewRow

                    If results.GetValue(results.GetOrdinal("Identifier")).GetType IsNot GetType(DBNull) Then
                        row("Identifier") = results.GetString(results.GetOrdinal("Identifier"))
                    End If
                    If results.GetValue(results.GetOrdinal("Item_Description")).GetType IsNot GetType(DBNull) Then
                        row("Item_Description") = results.GetString(results.GetOrdinal("Item_Description"))
                    End If
                    If results.GetValue(results.GetOrdinal("POSPrice")).GetType IsNot GetType(DBNull) Then
                        row("POSPrice") = results.GetDecimal(results.GetOrdinal("POSPrice"))
                    End If
                    If results.GetValue(results.GetOrdinal("POSPriceWithMultiple")).GetType IsNot GetType(DBNull) Then
                        row("POSPriceWithMultiple") = results.GetString(results.GetOrdinal("POSPriceWithMultiple"))
                    End If
                    If results.GetValue(results.GetOrdinal("StartDate")).GetType IsNot GetType(DBNull) Then
                        row("StartDate") = results.GetDateTime(results.GetOrdinal("StartDate"))
                    End If
                    If results.GetValue(results.GetOrdinal("Sale_End_Date")).GetType IsNot GetType(DBNull) Then
                        row("Sale_End_Date") = results.GetDateTime(results.GetOrdinal("Sale_End_Date"))
                    End If
                    If results.GetValue(results.GetOrdinal("Item_Key")).GetType IsNot GetType(DBNull) Then
                        row("Item_Key") = results.GetInt32(results.GetOrdinal("Item_Key"))
                    End If
                    If results.GetValue(results.GetOrdinal("PriceBatchDetailID")).GetType IsNot GetType(DBNull) Then
                        row("PriceBatchDetailID") = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))
                    End If
                    If results.GetValue(results.GetOrdinal("PrintSign")).GetType IsNot GetType(DBNull) Then
                        row("PrintSign") = results.GetBoolean(results.GetOrdinal("PrintSign"))
                    End If

                    table.Rows.Add(row)

                    If iLoop = 1 Then
                        ' populate the header fields - only need the first row for these values

                    End If
                End While

                If iLoop = 0 Then
                    MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly, formName)
                End If
            Catch e As Exception
                'TODO handle exception
                Throw e
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return table
        End Function

        Public Shared Sub UpdatePriceBatchHeader(ByVal batchHeader As PriceBatchHeaderBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                '' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchHeaderID"
                currentParam.Value = batchHeader.PriceBatchHeaderId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchDescription"
                currentParam.Value = batchHeader.BatchDescription
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AutoApplyflag"
                currentParam.Value = batchHeader.AutoApplyFlag
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ApplyDate"
                If batchHeader.AutoApplyDate = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = batchHeader.AutoApplyDate
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSBatchId"
                If batchHeader.POSBatchId = Nothing Or String.Equals(batchHeader.POSBatchId, "") Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = batchHeader.POSBatchId
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to Create Price Batch Detail records for the price change
                factory.ExecuteStoredProcedure("UpdatePriceBatchHeader", paramList)
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        ''' <summary>
        ''' checks for the existence of batches in the PriceBatchDetail table for the given
        ''' store and BatchStatus passed in
        ''' </summary>
        ''' <param name="storeList"></param>
        ''' <param name="storeListSeparator"></param>
        ''' <param name="status"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckForPendingPriceBatchesByStore(ByVal storeList As String, ByVal storeListSeparator As Char, ByVal status As BatchStatus) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim isPendingBatches As Boolean

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = storeList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = storeListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchStatus"
                Select Case status
                    Case BatchStatus.Building
                        currentParam.Value = "1"
                    Case BatchStatus.AllButProcessedAndBuilding
                        currentParam.Value = "2,3,4,5"
                    Case BatchStatus.AllButProcessed
                        currentParam.Value = "1,2,3,4,5"
                End Select
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsExistingUnprocessedBatch"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetIsBatchedByStatusForStore", paramList)

                If results.Read Then
                    isPendingBatches = results.GetBoolean(results.GetOrdinal("IsBatched"))
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return isPendingBatches
        End Function

        ''' <summary>
        ''' checks for the existence of batches in the PriceBatchDetail table for the given
        ''' item and BatchStatus passed in
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="storeList"></param>
        ''' <param name="storeListSeparator"></param>
        ''' <param name="status"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckForPendingPriceBatches(ByVal itemKey As Integer, ByVal storeList As String, ByVal storeListSeparator As Char, ByVal status As BatchStatus) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim isPendingBatches As Boolean

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = storeList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = storeListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BatchStatus"
                Select Case status
                    Case BatchStatus.Building
                        currentParam.Value = "1"
                    Case BatchStatus.AllButProcessedAndBuilding
                        currentParam.Value = "2,3,4,5"
                    Case BatchStatus.AllButProcessed
                        currentParam.Value = "1,2,3,4,5"
                End Select
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsExistingUnprocessedBatch"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetIsBatchedByStatus", paramList)

                If results.Read Then
                    isPendingBatches = results.GetBoolean(results.GetOrdinal("IsBatched"))
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return isPendingBatches
        End Function

        Public Shared Function ValidateRegularPriceChange(ByRef priceChangeData As PriceChangeBO) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New StringBuilder
            Dim validationCode As Integer

            ' setup parameters for stored proc
            paramList.Append(priceChangeData.ItemKey)
            paramList.Append(",")
            paramList.Append(priceChangeData.StoreNo)
            paramList.Append(",")
            paramList.Append(priceChangeData.PriceChgType.PriceChgTypeID)
            paramList.Append(",'")
            paramList.Append(priceChangeData.StartDate.ToString(ResourcesIRMA.GetString("DateStringFormat")))
            paramList.Append("',")
            paramList.Append(priceChangeData.RegMultiple)
            paramList.Append(",")
            paramList.Append(priceChangeData.RegPrice)
            paramList.Append(",")
            If (priceChangeData.OldStartDate > System.DateTime.FromOADate(0)) Then
                paramList.Append("'")
                paramList.Append(priceChangeData.OldStartDate.ToString(ResourcesIRMA.GetString("DateStringFormat")))
                paramList.Append("'")
            Else
                paramList.Append("NULL")
            End If

            ' Execute the function
            validationCode = CInt(factory.ExecuteScalar("SELECT dbo.fn_ValidateRegularPriceChange(" & paramList.ToString & ")"))

            Return validationCode
        End Function

        Public Shared Function ValidatePromoPriceChange(ByRef priceChangeData As PriceChangeBO) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New StringBuilder
            Dim validationCode As Integer

            ' setup parameters for stored proc
            paramList.Append(priceChangeData.ItemKey)
            paramList.Append(",")
            paramList.Append(priceChangeData.StoreNo)
            paramList.Append(",")
            paramList.Append(priceChangeData.PriceChgType.PriceChgTypeID)
            paramList.Append(",'")
            paramList.Append(priceChangeData.StartDate.ToString(ResourcesIRMA.GetString("DateStringFormat")))
            paramList.Append("','")
            paramList.Append(priceChangeData.SaleEndDate.ToString(ResourcesIRMA.GetString("DateStringFormat")))
            paramList.Append("',")
            paramList.Append(priceChangeData.RegMultiple)
            paramList.Append(",")
            paramList.Append(priceChangeData.RegPrice)
            paramList.Append(",")
            paramList.Append(priceChangeData.SaleMultiple)
            paramList.Append(",")
            paramList.Append(priceChangeData.SalePrice)
            paramList.Append(",")
            paramList.Append(priceChangeData.MSRPMultiple)
            paramList.Append(",")
            paramList.Append(priceChangeData.MSRPPrice)
            paramList.Append(",")
            paramList.Append("0") ' this is not part of end sale early
            paramList.Append(",")
            paramList.Append(priceChangeData.PriceBatchDetailId)

            ' Execute the function
            validationCode = CInt(factory.ExecuteScalar("SELECT dbo.fn_ValidatePromoPriceChange(" & paramList.ToString & ")"))

            Return validationCode
        End Function

        Public Shared Function SaveRegularPriceChange(ByRef priceChangeData As PriceChangeBO) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputList As ArrayList
            Dim currentParam As DBParam
            Dim validationCode As Integer

            ' setup parameters for stored proc
            ' -- input --
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = priceChangeData.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            If priceChangeData.UserId = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = priceChangeData.UserId
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID_Date"
            If priceChangeData.UserIdDate = Date.MinValue Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = priceChangeData.UserIdDate
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = priceChangeData.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = priceChangeData.StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Value = priceChangeData.RegMultiple
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Price"
            currentParam.Value = priceChangeData.RegPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Value = priceChangeData.RegPOSPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OldStartDate"
            If (priceChangeData.OldStartDate > System.DateTime.FromOADate(0)) Then
                currentParam.Value = priceChangeData.OldStartDate
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InsertApplication"
            currentParam.Value = priceChangeData.InsertApplication
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("UpdatePriceBatchDetailReg", paramList)

            validationCode = CInt(outputList(0))

            Return validationCode
        End Function

        Public Shared Function SavePromoPriceChange(ByRef priceChangeData As PriceChangeBO) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputList As ArrayList
            Dim currentParam As DBParam
            Dim validationCode As Integer

            ' setup parameters for stored proc
            ' -- input --
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = priceChangeData.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            If priceChangeData.UserId = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = priceChangeData.UserId
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID_Date"
            If priceChangeData.UserIdDate = Date.MinValue Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = priceChangeData.UserIdDate
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = priceChangeData.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Value = priceChangeData.PriceChgType.PriceChgTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = priceChangeData.StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Value = priceChangeData.RegMultiple
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Price"
            currentParam.Value = priceChangeData.RegPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Value = priceChangeData.RegPOSPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPPrice"
            currentParam.Value = priceChangeData.MSRPPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MSRPMultiple"
            currentParam.Value = priceChangeData.MSRPMultiple
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PricingMethod_ID"
            currentParam.Value = priceChangeData.PricingMethodId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Multiple"
            currentParam.Value = priceChangeData.SaleMultiple
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Price"
            currentParam.Value = priceChangeData.SalePrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSSale_Price"
            currentParam.Value = priceChangeData.SalePOSPrice
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_End_Date"
            currentParam.Value = priceChangeData.SaleEndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc1"
            currentParam.Value = priceChangeData.Sale_EarnedDisc1
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc2"
            currentParam.Value = priceChangeData.Sale_EarnedDisc2
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc3"
            currentParam.Value = priceChangeData.Sale_EarnedDisc3
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceBatchDetailID"
            currentParam.Value = priceChangeData.PriceBatchDetailId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LineDrive"
            currentParam.Value = priceChangeData.LineDrive
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InsertApplication"
            currentParam.Value = priceChangeData.InsertApplication
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("UpdatePriceBatchDetailPromo", paramList)

            validationCode = CInt(outputList(0))

            Return validationCode
        End Function

        Public Shared Function GetPriceBatchDetailsForBuildingOrSent() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim table As DataTable = New DataTable
            Dim row As DataRow
            Dim sqlText As String = "select" & vbCrLf &
        "pbd.item_key," & vbCrLf &
        "ii.identifier, " & vbCrLf &
        "pbd.store_no, " & vbCrLf &
        "pbh.PriceBatchStatusID, " & vbCrLf &
        "pbh.pricebatchheaderid, " & vbCrLf &
        "pbh.ItemChgTypeID, " & vbCrLf &
        "pbh.PriceChgTypeID, " & vbCrLf &
        "pbd.PriceChgTypeID" & vbCrLf &
    "from " & vbCrLf &
        "pricebatchdetail pbd " & vbCrLf &
        "join store s on pbd.store_no = s.store_no And (s.Mega_Store = 1 Or s.WFM_Store = 1)" & vbCrLf &
        "join pricebatchheader pbh on pbd.pricebatchheaderid = pbh.pricebatchheaderid" & vbCrLf &
        "join itemidentifier ii on pbd.item_key = ii.item_key" & vbCrLf &
    "where " & vbCrLf &
        "pbh.PriceBatchStatusID = 5 " & vbCrLf &
        "OR pbh.pricebatchstatusid = 1"

            Try
                results = factory.GetDataReader(sqlText)

                table.Columns.Add(New DataColumn("Item_Key", GetType(String)))
                table.Columns.Add(New DataColumn("Identifier", GetType(String)))
                table.Columns.Add(New DataColumn("Store_no", GetType(Integer)))
                table.Columns.Add(New DataColumn("PriceBatchStatusID", GetType(Integer)))
                table.Columns.Add(New DataColumn("PriceBatchHeaderID", GetType(Integer)))
                table.Columns.Add(New DataColumn("ItemChgTypeID", GetType(Integer)))
                table.Columns.Add(New DataColumn("PriceChgTypeID", GetType(Integer)))

                While results.Read
                    row = table.NewRow
                    row("Item_Key") = results.Item("Item_Key")
                    row("Identifier") = results.Item("Identifier")
                    row("Store_no") = results.Item("Store_no")
                    row("PriceBatchStatusID") = results.Item("PriceBatchStatusID")
                    row("PriceBatchHeaderID") = results.Item("PriceBatchHeaderID")
                    row("ItemChgTypeID") = results.Item("ItemChgTypeID")
                    row("PriceChgTypeID") = results.Item("PriceChgTypeID")
                    table.Rows.Add(row)

                End While
            Catch e As Exception
                Throw e
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return table
        End Function
#Region "ePromotion"

        ''' <summary>
        ''' Create/Maintain Price Batch Details for a Promotional Offer
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdatePromotionPriceBatchDetails(ByVal ItemID As Integer, ByVal StoreID As Integer,
        ByVal OfferID As Integer, ByVal OfferChgTypeID As Integer, Optional ByRef transaction As SqlTransaction = Nothing) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean = True

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = ItemID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = StoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OfferID"
                currentParam.Value = OfferID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OfferChgTypeID"
                currentParam.Value = OfferChgTypeID
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                ' Execute Stored Procedure to Create/Maintain Price Batch Details for the offer
                factory.ExecuteStoredProcedure("EPromotions_CreatePriceBatchDetail", paramList, transaction)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PriceBatchDetailDAO:UpdatePromotionPriceBatchDetails")
                success = False
            End Try

            Return success

        End Function

        Public Function DeleteUnbatchedPriceDetails(ByVal OfferID As Integer, Optional ByVal ItemID As Integer = 0, Optional ByVal StoreID As Integer = 0,
        Optional ByRef transaction As SqlTransaction = Nothing) As Boolean
            Dim success As Boolean = True
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemId"
                currentParam.Value = ItemID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "storeId"
                currentParam.Value = StoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("EPromotions_DeleteUnbatchedPriceBatchDetails", paramList, transaction)

            Catch ex As Exception
                success = False
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:DeleteUnbatchedPriceDetails")

            End Try

            Return success
        End Function

        Public Function GetUnbatchedPriceDetails(Optional ByVal OfferId As Integer = 0, Optional ByVal StoreID As Integer = 0) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim outputList As ArrayList

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                Dim outputIDX As Integer

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreId"
                currentParam.Value = StoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Count"
                currentParam.Type = DBParamType.Int
                ' store index of output parameter
                outputIDX = paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("EPromotions_ReturnUnbatchedPriceBatchDetailCount", paramList)

                If outputList.Count > 0 Then
                    Return CType(outputList(0), Integer)
                Else
                    MsgBox("No output value returned from EPromotions_ReturnUnbatchedPriceBatchDetailCount. Returning value of 1.", MsgBoxStyle.Critical, "PromotionalOffer:GetPendingPriceBatchDetailCount")
                    Return 1
                End If

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetPendingPriceBatchDetailCount")
                Return 0
            End Try

        End Function

        Public Function GetPriceBatchDetailsByOfferItem(ByVal OfferID As Integer, ByVal ItemID As Integer, ByVal Unbatched As Boolean) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow
            Dim table As New DataTable("BatchDetail")
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Dim iLoop As Integer
            Dim maxLoop As Short = 1000

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OfferID"
                currentParam.Value = OfferID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemID"
                currentParam.Value = ItemID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Unbatched"
                currentParam.Value = Unbatched
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetPriceBatchDetailsByOfferItem", paramList)

                'add columns to table
                table.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
                table.Columns.Add(New DataColumn("OfferChgTypeID", GetType(Byte)))
                table.Columns.Add(New DataColumn("PriceBatchDetailID", GetType(Integer)))

                While results.Read And (iLoop < maxLoop)
                    iLoop = iLoop + 1

                    row = table.NewRow

                    If results.GetValue(results.GetOrdinal("OfferChgTypeID")).GetType IsNot GetType(DBNull) Then
                        row("OfferChgTypeID") = results.GetByte(results.GetOrdinal("OfferChgTypeID"))
                    End If
                    If results.GetValue(results.GetOrdinal("Item_Key")).GetType IsNot GetType(DBNull) Then
                        row("Item_Key") = results.GetInt32(results.GetOrdinal("Item_Key"))
                    End If
                    If results.GetValue(results.GetOrdinal("PriceBatchDetailID")).GetType IsNot GetType(DBNull) Then
                        row("PriceBatchDetailID") = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))
                    End If

                    table.Rows.Add(row)
                End While

                If iLoop >= maxLoop Then
                    MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly)
                End If
            Catch e As Exception
                'TODO handle exception
                Throw e
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return table
        End Function

#End Region

    End Class
End Namespace
