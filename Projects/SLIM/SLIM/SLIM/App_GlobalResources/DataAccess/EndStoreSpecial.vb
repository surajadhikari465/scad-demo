Option Strict Off

Public Class EndStoreSpecial


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
    

    Public Function ProcessEndSale(ByVal dr As StoreSpecials.PriceBatchDetailRow) As Integer

        Dim da As New StoreSpecialsTableAdapters.PriceBatchDetailTableAdapter
        dr.LineDrive = False
        dr.Insert_Date = Date.Now

        Dim validationCode As Integer = EndSaleEarly(dr)

        Return validationCode

        'da.UpdatePriceBatchDetailPromo1(dr.Item_Key, Nothing, Nothing, dr.Store_No, _
        'dr.PriceChgTypeID, dr.StartDate, dr.Multiple, dr.Price, dr.POSPrice, Nothing, _
        'Nothing, Nothing, dr.Sale_Multiple, dr.Sale_Price, dr.Sale_Price, dr.Sale_End_Date, _
        'Nothing, Nothing, Nothing, Nothing, Nothing, dr.LineDrive)
    End Function
#End Region


#Region "Private Methods"
    Private Function EndSaleEarly(ByVal row As StoreSpecials.PriceBatchDetailRow) As Integer

        Dim validationCode As Integer

        paramList.Clear()
        ' ***** PriceBatchDetailID *********
        currentParam = New DBParam
        currentParam.Name = "PriceBatchDetailID"
        currentParam.Value = row.PriceBatchDetailID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' ***** Item_Key ********
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = row.Item_Key
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' ***** Store_No *********
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = row.Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' ***** Start Date *********
        currentParam = New DBParam
        currentParam.Name = "NewSaleEndDate"
        currentParam.Value = Date.Today
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
        currentParam.Value = row.POSPrice
        currentParam.Type = DBParamType.Money
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
        currentParam.Value = Date.Now
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        ' ***** ValidationCode *********
        currentParam = New DBParam
        currentParam.Name = "ValidationCode"
        currentParam.Value = Nothing
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        
        ' Execute SP
        Try

            Dim x As ArrayList = df.ExecuteStoredProcedure("EndSaleEarly", paramList)

            validationCode = CInt(x.Item(0))

            Return validationCode

        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

    End Function

#End Region
End Class
