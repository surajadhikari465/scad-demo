Option Strict Off
Imports WholeFoods.IRMA.RetailBulkLoad.DataAccess.ImportPricingDataDAO
Imports WholeFoods.Utility.DataAccess
Namespace WholeFoods.IRMA.RetailBulkLoad.BusinessLogic
    Public Class ImportPricingDataBO


#Region "Boa Constructor"
        Public Sub New()

        End Sub
        Public Sub New(ByVal sl As ArrayList)
            StoreList = sl
        End Sub
#End Region

#Region "Private Members"
        ' ***********************************************
        Dim az As String
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim itemKeyList As New ArrayList
        Dim currentParam As DBParam
        Dim StoreList As New ArrayList
        ' ***********************************************
#End Region

#Region "Public Methods"
        Public Sub IdentifierSearch()

        End Sub

        Public Sub InsertPriceBatchDetails(ByVal dt As DataTable)
            Dim store As String
            Dim i As Integer
            For i = 0 To dt.Rows.Count - 1
                If Not dt.Rows(i).Item(2) Is DBNull.Value Then
                    For Each store In StoreList
                        InsertRegularRecord(dt.Rows(i), store)
                    Next
                Else
                    For Each store In StoreList
                        InsertPromoRecord(dt.Rows(i), store)
                    Next
                End If
            Next
        End Sub
        Public Sub InsertPriceBatchDetails(ByVal dt As DataTable, ByVal pb As ToolStripProgressBar)
            pb.Visible = True
            pb.Maximum = dt.Rows.Count
            pb.Step = 1
            Dim store As String
            Dim i As Integer
            For i = 0 To dt.Rows.Count - 1
                pb.PerformStep()
                If Not dt.Rows(i).Item(2) Is DBNull.Value Then
                    For Each store In StoreList
                        InsertRegularRecord(dt.Rows(i), store)
                    Next
                Else
                    For Each store In StoreList
                        InsertPromoRecord(dt.Rows(i), store)
                    Next
                End If
            Next
            pb.Value = 0
            pb.Visible = False
        End Sub
#End Region


#Region "Private Methods"
        Private Sub InsertPromoRecord(ByVal row As DataRow, ByVal store As String)
            Dim item_key As Integer
            Dim PriceInfo As New Hashtable
            item_key = GetItemKeyByIdentifier(row.Item(0).ToString)
            PriceInfo = GetItemPriceInfo(item_key, store)
            ' ******* Set up Parameters for UpdatePriceDetail SP ***************
            If row.Item(9) Is DBNull.Value Then
                row.Item(9) = 0
            End If
            paramList.Clear()
            ' ***** Item_Key ********
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** User_ID *********
            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** User_ID_Date *********
            currentParam = New DBParam
            currentParam.Name = "User_ID_Date"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ***** Store_NO *********
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** PriceChangeType *********
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            If row.Item(9) = 0 Then
                currentParam.Value = 2
            Else
                currentParam.Value = 3
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** Start Date *********
            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = row.Item(7)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ***** Multiple *********
            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Value = PriceInfo("Multiple").ToString
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** Price *********
            currentParam = New DBParam
            currentParam.Name = "Price"
            currentParam.Value = PriceInfo("Price").ToString
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ***** POSPrice *********
            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Value = PriceInfo("POSPrice").ToString
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ***** MSRPPrice *********
            currentParam = New DBParam
            currentParam.Name = "MSRPPrice"
            If row.Item(9) = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PriceInfo("Price").ToString
            End If
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ***** MSRPMultiple *********
            currentParam = New DBParam
            currentParam.Name = "MSRPMultiple"
            If row.Item(9) = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PriceInfo("Multiple").ToString
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** PricingMethod *********
            currentParam = New DBParam
            currentParam.Name = "PricingMethod_ID"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** SaleMultiple *********
            currentParam = New DBParam
            currentParam.Name = "Sale_Multiple"
            currentParam.Value = row.Item(6)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** Sale Price *********
            currentParam = New DBParam
            currentParam.Name = "Sale_Price"
            currentParam.Value = row.Item(5)
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ***** POSSale Price *********
            currentParam = New DBParam
            currentParam.Name = "POSSale_Price"
            currentParam.Value = row.Item(5)
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ***** Sale_End_Date *********
            currentParam = New DBParam
            currentParam.Name = "Sale_End_Date"
            currentParam.Value = row.Item(8)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ***** Sale Disc1 *********
            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc1"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** Sale Disc2 *********
            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc2"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** Sale Disc3 *********
            currentParam = New DBParam
            currentParam.Name = "Sale_Earned_Disc3"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' *****PriceBatchDetailID *********
            currentParam = New DBParam
            currentParam.Name = "PriceBatchDetailID"
            currentParam.Value = -1
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** LineDrive *********
            currentParam = New DBParam
            currentParam.Name = "LineDrive"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)
            ' ****** Insert Application *********
            currentParam = New DBParam
            currentParam.Name = "InsertApplication"
            currentParam.Value = "Retail Bulk Load"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
            ' Execute SP
            df.ExecuteStoredProcedure("UpdatePriceBatchDetailPromo", paramList)
        End Sub
        Private Sub InsertRegularRecord(ByVal row As DataRow, ByVal store As String)
            Dim item_key As Integer
            item_key = GetItemKeyByIdentifier(row.Item(0).ToString)
            ' ******* Set up Parameters for UpdatePriceDetail SP ***************
            paramList.Clear()
            ' ***** Item_Key ********
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = item_key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** User_ID ********
            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ***** User_ID_Date ********
            currentParam = New DBParam
            currentParam.Name = "User_ID_Date"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ***** Store_NO *********
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ****** Start Date ***********
            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = row.Item(4)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ***** Multiple *********
            currentParam = New DBParam
            currentParam.Name = "Multiple"
            currentParam.Value = row.Item(3)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ****** POS Price *********
            currentParam = New DBParam
            currentParam.Name = "POSPrice"
            currentParam.Value = row.Item(2)
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)
            ' ****** Old StartDate *********
            currentParam = New DBParam
            currentParam.Name = "OldStartDate"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' ****** Insert Application *********
            currentParam = New DBParam
            currentParam.Name = "InsertApplication"
            currentParam.Value = "Retail Bulk Load"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
            ' ***** Execute SP ***************
            df.ExecuteStoredProcedure("UpdatePriceBatchDetailReg", paramList)
            ' *********************************
        End Sub
#End Region
    End Class
End Namespace
