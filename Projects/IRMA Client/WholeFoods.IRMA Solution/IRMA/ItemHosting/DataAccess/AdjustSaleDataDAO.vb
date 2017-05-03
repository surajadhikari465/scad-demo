Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Imports log4net


Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class AdjustSaleDataDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "read methods"

        ''' <summary>
        ''' checks for the existence of batches in the PriceBatchDetail table for the given
        ''' item and BatchStatus passed in
        ''' </summary>
        ''' <param name="adjustSaleData"></param>
        ''' <param name="status"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckForPendingBatches(ByVal adjustSaleData As AdjustSaleDataBO, ByVal status As BatchStatus) As Boolean
            Return PriceBatchDetailDAO.CheckForPendingPriceBatches(adjustSaleData.ItemKey, adjustSaleData.StoreList, adjustSaleData.StoreListSeparator, status)
        End Function

        ''' <summary>
        ''' checks to see if an item is currently on sale for the selected store OR if there are
        ''' any pending, but not processed, sale price batch detail records for the item-store
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsItemOnSaleOrSalePendingForStore(ByVal itemKey As Integer, ByVal storeNo As Integer) As Boolean

            logger.Debug("IsItemOnSaleOrSalePendingForStore Entry with itemKey = " + itemKey.ToString + ", storeNo= " & storeNo.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim isOnSaleOrSalePending As Boolean

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetIsItemOnSaleOrSalePendingForStore", paramList)

                If results.Read Then
                    isOnSaleOrSalePending = results.GetBoolean(results.GetOrdinal("IsOnSaleOrSalePending"))
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("IsItemOnSaleOrSalePendingForStore Exit")

            Return isOnSaleOrSalePending
        End Function

        ''' <summary>
        ''' returns any SALE records that are in the PriceBatchDetail as processed
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentProcessedSales(ByVal itemKey As Integer, ByVal storeNo As Integer) As DataTable

            logger.Debug("GetCurrentProcessedSales Entry with itemKey = " + itemKey.ToString + ", storeNo = " + storeNo.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim row As DataRow
            Dim table As New DataTable

            Dim cPrice As Decimal
            Dim cPOSPrice As Decimal
            Dim iMultiple As Short
            Dim dtDate As Date

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)


                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetCurrentProcessedSaleBatches", paramList)

                'add columns to table
                table.Columns.Add(New DataColumn("ID", GetType(String)))
                table.Columns.Add(New DataColumn("Store Name", GetType(String)))
                table.Columns.Add(New DataColumn("Start Date", GetType(String)))
                table.Columns.Add(New DataColumn("Type", GetType(String)))
                table.Columns.Add(New DataColumn("POS Price", GetType(String)))
                table.Columns.Add(New DataColumn("Price", GetType(String)))
                table.Columns.Add(New DataColumn("Priority", GetType(String)))
                table.Columns.Add(New DataColumn("Sale End", GetType(String)))

                While results.Read
                    row = table.NewRow

                    'determine if item is on sale or not; get appropriate price/multiple values
                    If results.GetBoolean(results.GetOrdinal("On_Sale")) = True Then
                        If Not results.IsDBNull(results.GetOrdinal("Sale_Price")) Then
                            cPrice = results.GetDecimal(results.GetOrdinal("Sale_Price"))
                        Else
                            cPrice = 0
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("POSSale_Price")) Then
                            cPOSPrice = results.GetDecimal(results.GetOrdinal("POSSale_Price"))
                        Else
                            cPOSPrice = 0
                        End If

                        'default Multiple to 1 if not a valid value
                        If Not results.IsDBNull(results.GetOrdinal("Sale_Multiple")) AndAlso results.GetByte(results.GetOrdinal("Sale_Multiple")) > 0 Then
                            iMultiple = results.GetByte(results.GetOrdinal("Sale_Multiple"))
                        Else
                            iMultiple = 1
                        End If
                    Else
                        If Not results.IsDBNull(results.GetOrdinal("Price")) Then
                            cPrice = results.GetDecimal(results.GetOrdinal("Price"))
                        Else
                            cPrice = 0
                        End If

                        If Not results.IsDBNull(results.GetOrdinal("POSPrice")) Then
                            cPOSPrice = results.GetDecimal(results.GetOrdinal("POSPrice"))
                        Else
                            cPOSPrice = 0
                        End If

                        'default Multiple to 1 if not a valid value
                        If Not results.IsDBNull(results.GetOrdinal("Multiple")) AndAlso results.GetByte(results.GetOrdinal("Multiple")) > 0 Then
                            iMultiple = results.GetByte(results.GetOrdinal("Multiple"))
                        Else
                            iMultiple = 1
                        End If
                    End If

                    row("ID") = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))
                    row("Store Name") = results.GetString(results.GetOrdinal("Store_Name"))

                    'get sale start date
                    dtDate = results.GetDateTime(results.GetOrdinal("StartDate"))
                    row("Start Date") = IIf(dtDate > DateTime.MinValue, dtDate.ToString("d"), String.Empty)

                    'get sale end date
                    dtDate = results.GetDateTime(results.GetOrdinal("Sale_End_Date"))
                    row("Sale End") = IIf(dtDate > DateTime.MinValue, dtDate.ToString("d"), String.Empty)

                    row("Type") = results.GetString(results.GetOrdinal("PriceChgTypeDesc"))
                    row("POS Price") = IIf(cPOSPrice > 0, iMultiple & " @ " & VB6.Format(cPOSPrice, "####0.00"), "")
                    row("Price") = IIf(cPrice > 0, iMultiple & " @ " & VB6.Format(cPrice, "####0.00"), "")
                    row("Priority") = results.GetInt16(results.GetOrdinal("Priority"))

                    table.Rows.Add(row)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetCurrentProcessedSales Exit")

            Return table
        End Function

#End Region

#Region "write methods"

        ''' <summary>
        ''' ends a specific sale early for the passed in item and store
        ''' </summary>
        ''' <param name="adjustSaleData"></param>
        ''' <remarks></remarks>
        Public Function EndSaleEarly(ByVal adjustSaleData As AdjustSaleDataBO) As Integer

            logger.Debug("EndSaleEarly Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputList As ArrayList
            Dim currentParam As DBParam
            Dim validationCode As Integer

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceBatchDetailID"
                currentParam.Value = adjustSaleData.PriceBatchDetailIdToEndEarly
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = adjustSaleData.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = adjustSaleData.StoreList
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NewSaleEndDate"
                currentParam.Value = adjustSaleData.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Multiple"
                currentParam.Value = adjustSaleData.RegMultiple
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Price"
                currentParam.Value = adjustSaleData.Price
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSPrice"
                currentParam.Value = adjustSaleData.POSPrice
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = adjustSaleData.UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID_Date"
                currentParam.Value = adjustSaleData.UserIDDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                ' -- output --
                currentParam = New DBParam
                currentParam.Name = "ValidationCode"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("EndSaleEarly", paramList)

                validationCode = CInt(outputList(0))

            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("EndSaleEarly Exit")
            Return validationCode
        End Function

        ''' <summary>
        ''' cancel all sales for the passed in item and store list
        ''' </summary>
        ''' <param name="adjustSaleData"></param>
        ''' <remarks></remarks>
        Public Sub CancelAllSales(ByVal adjustSaleData As AdjustSaleDataBO)

            logger.Debug("CancelAllSales Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = adjustSaleData.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = adjustSaleData.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = adjustSaleData.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = adjustSaleData.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = adjustSaleData.UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID_Date"
                currentParam.Value = adjustSaleData.UserIDDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ValidationCode"
                currentParam.Value = Nothing
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("CancelAllSales", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("CancelAllSales Exit")
        End Sub

#End Region

    End Class

End Namespace