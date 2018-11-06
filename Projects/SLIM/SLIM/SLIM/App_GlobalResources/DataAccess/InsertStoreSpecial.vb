Option Strict Off

Public Class InsertStoreSpecial


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

    Public Function InsertPriceBatchDetails(ByVal dr As StoreSpecials.PriceBatchDetailRow) As Integer

        Dim da As New StoreSpecialsTableAdapters.PriceBatchDetailTableAdapter
        dr.LineDrive = False
        dr.Insert_Date = Date.Now

        Dim validationCode As Integer = InsertPromoRecord(dr)

        Return validationCode

        'da.UpdatePriceBatchDetailPromo1(dr.Item_Key, Nothing, Nothing, dr.Store_No, _
        'dr.PriceChgTypeID, dr.StartDate, dr.Multiple, dr.Price, dr.POSPrice, Nothing, _
        'Nothing, Nothing, dr.Sale_Multiple, dr.Sale_Price, dr.Sale_Price, dr.Sale_End_Date, _
        'Nothing, Nothing, Nothing, Nothing, Nothing, dr.LineDrive)
    End Function
#End Region


#Region "Private Methods"
    Private Function InsertPromoRecord(ByVal row As StoreSpecials.PriceBatchDetailRow) As Integer

        Dim validationCode As Integer

        paramList.Clear()

        ' ***** Item_Key ********
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = row.Item_Key
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
        currentParam.Value = row.Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** PriceChangeType *********
        currentParam = New DBParam
        currentParam.Name = "PriceChgTypeID"
        currentParam.Value = HttpContext.Current.Application.Get("IIS_PriceChgTypeID")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** Start Date *********
        currentParam = New DBParam
        currentParam.Name = "StartDate"
        currentParam.Value = row.StartDate
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ***** Multiple *********
        currentParam = New DBParam
        currentParam.Name = "Multiple"
        currentParam.Value = row.Multiple
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** Price *********
        currentParam = New DBParam
        currentParam.Name = "Price"
        currentParam.Value = row.Price
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)
        ' ***** POSPrice *********
        currentParam = New DBParam
        currentParam.Name = "POSPrice"
        currentParam.Value = row.Price
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)
        ' ***** MSRPPrice *********
        currentParam = New DBParam
        currentParam.Name = "MSRPPrice"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)
        ' ***** MSRPMultiple *********
        currentParam = New DBParam
        currentParam.Name = "MSRPMultiple"
        currentParam.Value = DBNull.Value
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
        currentParam.Value = row.Sale_Multiple
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** Sale Price *********
        currentParam = New DBParam
        currentParam.Name = "Sale_Price"
        currentParam.Value = row.Sale_Price
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)
        ' ***** POSSale Price *********
        currentParam = New DBParam
        currentParam.Name = "POSSale_Price"
        currentParam.Value = row.POSSale_Price
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)
        ' ***** Sale_End_Date *********
        currentParam = New DBParam
        currentParam.Name = "Sale_End_Date"
        currentParam.Value = row.Sale_End_Date
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ***** Sale Disc1 *********
        currentParam = New DBParam
        currentParam.Name = "Sale_Earned_Disc1"
        currentParam.Value = 0
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** Sale Disc2 *********
        currentParam = New DBParam
        currentParam.Name = "Sale_Earned_Disc2"
        currentParam.Value = 0
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** Sale Disc3 *********
        currentParam = New DBParam
        currentParam.Name = "Sale_Earned_Disc3"
        currentParam.Value = 99
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** PriceBatchDetailID *********
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

        ' ***** RequestID *********
        currentParam = New DBParam
        currentParam.Name = "SLIMRequestID"
        currentParam.Value = row.SLIMRequestID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ***** InsertApplication *********
        currentParam = New DBParam
        currentParam.Name = "InsertApplication"
        currentParam.Value = "Slim"
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        ' ***** ValidationCode *********
        currentParam = New DBParam
        currentParam.Name = "ValidationCode"
        currentParam.Value = Nothing
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' Execute SP
        Try

            Dim x As ArrayList = df.ExecuteStoredProcedure("UpdatePriceBatchDetailPromo", paramList)

            validationCode = CInt(x.Item(0))

            Return validationCode

        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

    End Function

    Private Sub InsertRegularRecord(ByVal row As DataRow, ByVal store As String)
        Dim item_key As Integer

        'item_key = GetItemKeyByIdentifier(row.Item(0).ToString)

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

        ' ***** Execute SP ***************
        df.ExecuteStoredProcedure("UpdatePriceBatchDetailReg", paramList)

    End Sub
#End Region
End Class
