Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols


Public Class ItemInsert

#Region "Members"


    Private ItemID As Integer
    Private VendorID As Integer
    Private ItemKey As Integer
    Private BrandID As Integer
    Private VendorRequestID As Integer


#End Region

#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal id As Integer)
        ItemID = id
    End Sub
#End Region

#Region "Properties"


    Public Property ItemRequestID() As Integer
        Get
            Return ItemID
        End Get
        Set(ByVal value As Integer)
            ItemID = value
        End Set
    End Property

    Public Property IrmaItemKey()
        Get
            Return ItemKey
        End Get
        Set(ByVal value)
            ItemKey = value
        End Set
    End Property
#End Region

#Region "Public Subs/Functions"

    Public Sub InsertItemIntoIrma()
        ' ***** Get the selected Request *******
        Dim dt As New NewItemRequest.ItemRequestDataTable
        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter
        Dim dr As NewItemRequest.ItemRequestRow = dt.NewItemRequestRow()
        Try
            dt = da.GetLatestRequest(ItemID)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        dr = dt.Rows(0)
        dr.Identifier = dr.Identifier.Trim
        VendorID = CInt(dr.VendorNumber)
        VendorRequestID = CInt(dr.VendorRequest_ID)
        ItemRequestID = CInt(dr.ItemRequest_ID)
        
        ' **********************************************
        ' ***** If Vendor is new --- Insert First ******
        ' **********************************************
        If Not dr.VendorRequest_ID = 1 And VendorID = 0 _
        Then
            If VendorExists(VendorRequestID) = 0 Then
                InsertVendor()
                ' ********** Update ItemRequest with new Vendor_ID *********
                If Not VendorID = 0 Then
                    da.UpdateVendorNumber(VendorID, ItemRequestID)
                End If
                ' **********************************************************
            Else
                VendorID = VendorExists(VendorRequestID)
                If Not VendorID = 0 Then
                    da.UpdateVendorNumber(VendorID, ItemRequestID)
                End If
            End If
        End If
        ' ********************************************************
        ' ***************  Insert Brand into IRMA *****************
        ' ********************************************************
        If Not dr.BrandName Is Nothing And dr.Brand_ID = -1 Then
            ' ***********  New Brand **********
            Try
                'dr.Brand_ID = InsertNewBrand(dr.BrandName)
                If ItemBrandExists(dr.BrandName) = 0 Then
                    dr.Brand_ID = InsertNewBrand(dr.BrandName)
                    ' ********** Update ItemRequest with new Brand_ID *********
                    If Not dr.Brand_ID = -1 Then
                        da.UpdateBrandID(dr.Brand_ID, ItemRequestID)
                    End If
                    ' **********************************************************
                Else
                    dr.Brand_ID = ItemBrandExists(dr.BrandName)
                    If Not dr.Brand_ID = -1 Then
                        da.UpdateBrandID(dr.Brand_ID, ItemRequestID)
                    End If
                End If
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        End If
        ' ********************************************************
        ' ***************  Insert Item into IRMA *****************
        ' ********************************************************
        'commented out while we are submitting to EIM to process.
        'Try
        '    ItemKey = InsertNewItem(dr)
        '    ' ******** Update SLIM Attribute ******
        '    UpdateSLIMAttribute(ItemKey)
        '    ' ******** Update POS Flags ***********
        '    UpdatePOSFlags(ItemKey, dr.ItemUnit, dr.AgeCode, dr.CRV, dr.FoodStamp, dr.User_Store)
        '    ' *************************************
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        '' ********************************************************
        '' ***************  Insert Scale Info *****************
        '' ********************************************************
        'If dr.ItemType_ID = 2 Then
        '    Try
        '        InsertScaleInfo(ItemKey, ItemRequestID, dr.Identifier)
        '    Catch ex As Exception
        '        Debug.WriteLine(ex.Message)
        '        Error_Log.throwException(ex.Message, ex)
        '    End Try
        'End If
        '' ********************************************************
        '' ***************  Insert ItemVendor into IRMA ***********
        '' ********************************************************
        'Try
        '    InsertItemVendor(ItemKey, VendorID, dr.Warehouse)
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        '' ********************************************************
        '' ************  Insert StoreItemVendor into IRMA *********
        '' ********************************************************
        'Try
        '    InsertStoreItemVendor(VendorID, dr.User_Store, ItemKey)
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        '' ********************************************************
        '' ***************  Set Primary Vendor ********************
        '' ********************************************************
        'Try
        '    'SetPrimaryVendor(CStr(dr.User_Store), ItemKey, VendorID)
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        '' ********************************************************
        '' ***************  Insert Price into Irma ********************
        '' ********************************************************
        'Try
        '    InsertPrice(dr)
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        '' ********************************************************
        '' ***************  Insert Cost into Irma ********************
        '' ********************************************************
        'Try
        '    InsertVendorCost(dr)
        'Catch ex As Exception
        '    Debug.WriteLine(ex.Message)
        '    Error_Log.throwException(ex.Message, ex)
        'End Try
        ' ********************************************************
        ' *********  Update Item Request Status ******************
        ' ********************************************************
        Try
            UpdateItemRequestStatus()
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try

        ' ********************************************************
        ' *********  UpdateVendor Request Status ******************
        ' ********************************************************
        If Not VendorRequestID = 1 Then
            Try
                UpdateVendorRequestStatus()
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        End If
    End Sub
#End Region

#Region "Private Functions"

    Private Function InsertNewItem(ByVal row As NewItemRequest.ItemRequestRow) As Integer
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Dim itemKey As Integer
        Dim ScaleIdentifier As Boolean
        Dim IdentifierType As Char
        Dim DistributionID As Integer

        'TODO: *************** Verify that Identifier is scale *************************** 
        If row.ItemType_ID = 2 Then
            ScaleIdentifier = True
            IdentifierType = "O"
        ElseIf row.ItemType_ID = 3 Then
            IdentifierType = "P"
        End If
        DistributionID = da.GetDistributionUnitID

        itemKey = da.InsertItem((row.POS_Description.ToUpper), (row.Item_Description.ToUpper), _
                    row.SubTeam_No, row.Category_ID, row.ItemUnit, row.ItemUnit, _
                    row.PackSize, row.ItemSize, IdentifierType, row.Identifier, "", _
                    0, True, row.ClassID, False, DistributionID, _
                    DistributionID, row.TaxClass_ID, _
                        Nothing, row.Brand_ID, 0, 0, ScaleIdentifier, Nothing, "SLIM")
        Debug.WriteLine("Request Inserted into Item Table")
        Return itemKey
    End Function

    Private Sub InsertItemVendor(ByVal item_Key As Integer, _
    ByVal vendor_ID As Integer, ByVal warehouse As String)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.InsertItemVendor(vendor_ID, item_Key)
        da.UpdateItemVendor(item_Key, vendor_ID, warehouse)
    End Sub

    Private Sub UpdateItemBrand(ByVal itemKey As Integer, ByVal brandID As Integer)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Try
            da.UpdateItemBrand(brandID, itemKey)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
    End Sub

    Private Function InsertNewBrand(ByVal brandName As String) As Integer
        Dim BrandID
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        BrandID = da.InsertItemBrand(brandName.ToUpper)
        Return BrandID
    End Function

    Private Sub InsertStoreItemVendor(ByVal vendor_id As Integer, _
    ByVal store As Integer, ByVal item_key As Integer)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.InsertStoreItemVendor(vendor_id, store, item_key)
    End Sub

    Private Sub SetPrimaryVendor(ByVal store As String, _
    ByVal item_key As Integer, ByVal vendor_id As Integer)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.SetPrimaryVendor(store, ItemKey, vendor_id)
    End Sub

    Private Sub InsertPrice(ByVal row As NewItemRequest.ItemRequestRow)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.InsertPrice(ItemKey, Nothing, Nothing, row.User_Store, Date.Now, _
        row.PriceMultiple, row.Price, row.Price, Date.Now, "SLIM")
    End Sub

    Private Sub InsertVendorCost(ByVal row As NewItemRequest.ItemRequestRow)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Dim unitCost As Decimal
        unitCost = row.CaseCost
        da.InsertCost(CStr(row.User_Store), "|", ItemKey, VendorID, _
        unitCost, Nothing, row.CaseSize, Date.Today, Nothing, False, _
        Nothing, False, row.ItemUnit, row.ItemUnit)
    End Sub

    Private Sub UpdateItemRequestStatus()
        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter
        da.UpdateItemRequestStatus(itemID)
    End Sub

    Private Sub UpdateVendorRequestStatus()
        Dim da As New VendorRequestTableAdapters.VendorRequestTableAdapter
        da.UpdateVendorRequestStatus(VendorRequestID)
    End Sub

    Private Sub InsertVendor()
        Dim da As New VendorRequest2TableAdapters.VendorRequestTableAdapter
        Dim dt As New VendorRequest2.VendorRequestDataTable
        Dim dr As VendorRequest2.VendorRequestRow = dt.NewVendorRequestRow()
        Dim da1 As New IrmaVendorTableAdapters.VendorTableAdapter
        Dim dt1 As New IrmaVendor.VendorDataTable
        Dim dr1 As IrmaVendor.VendorRow = dt1.NewVendorRow
        Try
            dt = da.GetVendorRequestData(VendorRequestID)
            dr = dt.Rows(0)
            da = Nothing
            dt = Nothing
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
        Try
            VendorID = da1.InsertSLIMVendor(dr.CompanyName, dr.PS_Vendor_ID, Nothing, dr.PS_Export_Vendor_ID)
            ' MD 8/31/2009: Bug 8890, added new parameters (CurrencyID = 1 (USD), AccountingContactEmail = nothing) 
            ' to be in sync with IRMA sp UpdateVendorInfo 
            da1.UpdateVendorInfo(VendorID, dr.Vendor_Key, dr.CompanyName, dr.Address_Line_1, dr.Address_Line_2, _
                    dr.City, dr.State, dr.ZipCode, Nothing, Nothing, dr.Phone, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, Nothing, dr.PS_Vendor_ID, dr.PS_Export_Vendor_ID, Nothing, Nothing, dr.Comment, _
                     True, False, True, Nothing, Nothing, 0, dr.Email, False, _
                    Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 0, 1, Nothing)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
        
    End Sub

    Private Function ItemBrandExists(ByVal BrandName As String) As Integer
        Dim BrandID As Integer
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        BrandID = da.CheckBrandExists(BrandName)

        Return BrandID
    End Function

    Private Function VendorExists(ByVal VendorName As String) As Integer
        Dim VendorID As Integer
        Dim da As New VendorRequestIDTableAdapters.VendorRequestTableAdapter
        Dim dt As New VendorRequestID.VendorRequestDataTable
        'Dim dr As VendorRequestID.VendorRequestRow = dt.NewVendorRequestRow()
        Dim dr As DataRow = Nothing
        Try
            dt = da.GetData(VendorRequestID)
            dr = dt.Rows(0)
            da = Nothing
            dt = Nothing
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
        Dim da1 As New IrmaItemTableAdapters.ItemTableAdapter
        VendorID = da1.CheckVendorExists(dr.Item(0))
        Return VendorID
    End Function

    Private Sub UpdateSLIMAttribute(ByVal Item_Key As Integer)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.UpdateSLIMItemAttribute(Item_Key)
    End Sub
    Private Sub UpdatePOSFlags(ByVal Item_Key As Integer, ByVal ItemUnit As Integer, ByVal AgeCode As Integer, ByVal Linked As String, ByVal FoodStamp As Boolean, ByVal Store As Integer)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Dim CostedByWeight As Boolean
        Dim Restricted As Boolean
        If Not AgeCode = 2 Then
            Restricted = False
            AgeCode = 0
        Else
            Restricted = True
            AgeCode = 2
        End If
        If ItemUnit = 2 Or ItemUnit = 40 Then
            CostedByWeight = True
        End If
        da.UpdateItemFlags(FoodStamp, CostedByWeight, Item_Key)
        da.UpdatePriceFlags(Restricted, True, 0, AgeCode, Item_Key)
        If Not Linked = 0 Then
            da.UpdatePriceLinkCode(Linked, Item_Key, Store)
        End If
    End Sub
    Private Sub InsertScaleInfo(ByVal ItemKey As Integer, ByVal ItemRequestID As Integer, ByVal Identifier As String)
        ' ****** Get the Scale Row ************
        Dim Extra_Text_ID As Integer
        Dim da As New ItemScaleRequestTableAdapters.ItemScaleRequestTableAdapter
        Dim dt As New ItemScaleRequest.ItemScaleRequestDataTable
        Dim dr As ItemScaleRequest.ItemScaleRequestRow = dt.NewItemScaleRequestRow
        dt = da.GetScaleDataByItemRequestID(ItemRequestID)
        dr = dt.Rows(0)
        
        ' ************ Insert the Ingredients *****************
        Dim da1 As New IrmaScaleItemTableAdapters.ItemScaleTableAdapter
        Extra_Text_ID = da1.InsertIngredients(1, Identifier, dr.Ingredients)
        da1.Scale_InsertUpdateItemScaleDetails(0, ItemKey, Nothing, Extra_Text_ID, Nothing, _
        Nothing, Nothing, Nothing, Nothing, Nothing, dr.ScaleUomUnit_ID, _
        dr.FixedWeight, dr.ByCount, False, False, False, False, False, False, False, _
        dr.ScaleDescription1, dr.ScaleDescription2, dr.ScaleDescription3, dr.ScaleDescription4, _
        dr.ShelfLife)
    End Sub
#End Region

End Class
