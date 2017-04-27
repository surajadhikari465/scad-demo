Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.DataAccess
    Public Class PriceBatchDetail
        Inherits WholeFoods.IRMA.ItemChaining.BusinessLogic.PriceBatchDetail

        Public Overrides Function IsBatchedPriceChange(ByVal ItemIDs As String, ByVal StoreIDs As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList
            Dim results As DataSet

            paramList.Add(New DBParam("Items", DBParamType.String, ItemIDs))
            paramList.Add(New DBParam("Stores", DBParamType.String, StoreIDs))

            results = factory.GetStoredProcedureDataSet("CheckBatchedPriceChange", paramList)

            For Each row As Data.DataRow In results.Tables(0).Rows
                If row("Found").ToString <> "0" Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Overrides Function IsPendingRegularPriceChange(ByVal ItemIDs As String, ByVal StoreIDs As String, ByVal [Date] As Date) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList
            Dim results As DataSet

            paramList.Add(New DBParam("Items", DBParamType.String, ItemIDs))
            paramList.Add(New DBParam("Stores", DBParamType.String, StoreIDs))
            paramList.Add(New DBParam("StartDate", DBParamType.DateTime, [Date]))

            results = factory.GetStoredProcedureDataSet("PendingRegularPriceChange", paramList)

            For Each row As Data.DataRow In results.Tables(0).Rows
                If row("Found").ToString <> "1" Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Sub InsertPriceBatchDetailReg(ByVal factory As DataFactory, ByVal startDate As DateTime, ByVal row As DataRow, ByVal transaction As SqlTransaction)
            Dim paramList As New ArrayList

            paramList.Add(New DBParam("Item_Key", DBParamType.Int, CInt(row("Item_Key"))))
            paramList.Add(New DBParam("User_ID", DBParamType.Int, DBNull.Value))
            paramList.Add(New DBParam("User_ID_Date", DBParamType.DateTime, DBNull.Value))
            paramList.Add(New DBParam("Store_No", DBParamType.Int, CInt(row("store_no"))))
            paramList.Add(New DBParam("StartDate", DBParamType.DateTime, startDate.Date))
            paramList.Add(New DBParam("Multiple", DBParamType.SmallInt, CInt(row("NewMultiple"))))
            paramList.Add(New DBParam("POSPrice", DBParamType.Money, CDec(row("NewPrice"))))
            paramList.Add(New DBParam("Price", DBParamType.Money, CDec(row("NewPrice"))))
            paramList.Add(New DBParam("OldStartDate", DBParamType.DateTime, DBNull.Value))
            paramList.Add(New DBParam("InsertApplication", DBParamType.String, "IRMA Price Change Wizard"))
            paramList.Add(New DBParam("ValidationCode", DBParamType.Int, Nothing))

            factory.ExecuteStoredProcedure("UpdatePriceBatchDetailReg", paramList, transaction)
        End Sub

        Public Sub InsertPriceBatchDetailPromo(ByVal factory As DataFactory, ByVal priceChangeTypeID As Integer, _
            ByVal priceChangeIsLineDrive As Boolean, ByVal startDate As DateTime, ByVal endDate As DateTime, _
            ByVal row As DataRow, ByVal transaction As SqlTransaction)

            Dim paramList As New ArrayList

            ' -1 signifies new row
            paramList.Add(New DBParam("PriceBatchDetailID", DBParamType.Int, -1))
            paramList.Add(New DBParam("Item_Key", DBParamType.Int, CInt(row("Item_Key"))))
            paramList.Add(New DBParam("Store_No", DBParamType.Int, CInt(row("store_no"))))
            paramList.Add(New DBParam("User_ID", DBParamType.Int, DBNull.Value))
            paramList.Add(New DBParam("User_ID_Date", DBParamType.DateTime, DBNull.Value))
            
            ' Pricing info
            paramList.Add(New DBParam("PriceChgTypeID", DBParamType.Int, priceChangeTypeID))
            paramList.Add(New DBParam("PricingMethod_ID", DBParamType.SmallInt, DBNull.Value))
            paramList.Add(New DBParam("LineDrive", DBParamType.Bit, priceChangeIsLineDrive))
            paramList.Add(New DBParam("Sale_Earned_Disc1", DBParamType.SmallInt, DBNull.Value))
            paramList.Add(New DBParam("Sale_Earned_Disc2", DBParamType.SmallInt, DBNull.Value))
            paramList.Add(New DBParam("Sale_Earned_Disc3", DBParamType.SmallInt, DBNull.Value))
            paramList.Add(New DBParam("Multiple", DBParamType.SmallInt, DBNull.Value))
            ' The regular price does not change
            paramList.Add(New DBParam("Price", DBParamType.Money, DBNull.Value))
            paramList.Add(New DBParam("POSPrice", DBParamType.Money, DBNull.Value))

            If row.IsNull("NewPrice") OrElse row.IsNull("NewMultiple") Then
                paramList.Add(New DBParam("Sale_Multiple", DBParamType.SmallInt, DBNull.Value))
                paramList.Add(New DBParam("Sale_Price", DBParamType.Money, DBNull.Value))
                paramList.Add(New DBParam("POSSale_Price", DBParamType.Money, DBNull.Value))
            Else
                ' If the new price was calculated as a percentage there may be rounding issues
                Dim newPrice As Decimal = Math.Round(CDec(row("NewPrice")), 2)

                paramList.Add(New DBParam("Sale_Multiple", DBParamType.SmallInt, CInt(row("NewMultiple"))))
                paramList.Add(New DBParam("Sale_Price", DBParamType.Money, newPrice))
                paramList.Add(New DBParam("POSSale_Price", DBParamType.Money, newPrice))
            End If

            If row.IsNull("NewMSRPPrice") OrElse row.IsNull("NewMSRPMultiple") Then
                paramList.Add(New DBParam("MSRPPrice", DBParamType.Money, DBNull.Value))
                paramList.Add(New DBParam("MSRPMultiple", DBParamType.Int, DBNull.Value))
            Else
                paramList.Add(New DBParam("MSRPPrice", DBParamType.Money, CDec(row("NewMSRPPrice"))))
                paramList.Add(New DBParam("MSRPMultiple", DBParamType.Int, CDec(row("NewMSRPMultiple"))))
            End If

            ' Promotion dates
            paramList.Add(New DBParam("StartDate", DBParamType.DateTime, startDate.Date))
            paramList.Add(New DBParam("Sale_End_Date", DBParamType.DateTime, endDate.Date))

            ' Identify the tool being used
            paramList.Add(New DBParam("InsertApplication", DBParamType.String, "IRMA Price Change Wizard"))

            ' validation code output
            paramList.Add(New DBParam("ValidationCode", DBParamType.Int, Nothing))

            factory.ExecuteStoredProcedure("UpdatePriceBatchDetailPromo", paramList, transaction)
        End Sub
    End Class
End Namespace