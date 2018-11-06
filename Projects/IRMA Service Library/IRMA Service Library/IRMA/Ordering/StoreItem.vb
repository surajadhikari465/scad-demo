Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA
    <DataContract()>
    Public Class StoreItem
        <DataMember()>
        Public Property StoreNo As Integer
        <DataMember()>
        Public Property TransferToSubteamNo As Integer
        <DataMember()>
        Public Property UserID As Integer
        <DataMember()>
        Public Property ItemKey As Integer
        <DataMember()>
        Public Property Identifier As String
        <DataMember()>
        Public Property Price As Decimal
        <DataMember()>
        Public Property Multiple As Integer
        <DataMember()>
        Public Property OnSale As Boolean
        <DataMember()>
        Public Property SaleStartDate As DateTime
        <DataMember()>
        Public Property SaleEndDate As DateTime
        <DataMember()>
        Public Property SaleMultiple As Integer
        <DataMember()>
        Public Property SalePrice As Decimal
        <DataMember()>
        Public Property SaleEarnedDisc1 As Integer
        <DataMember()>
        Public Property SaleEarnedDisc2 As Integer
        <DataMember()>
        Public Property SaleEarnedDisc3 As Integer
        <DataMember()>
        Public Property AvgCost As Decimal
        <DataMember()>
        Public Property CanInventory As Boolean
        <DataMember()>
        Public Property IsSellable As Boolean
        <DataMember()>
        Public Property ItemDescription As String
        <DataMember()>
        Public Property POSDescription As String
        <DataMember()>
        Public Property RetailSubteamNo As Integer
        <DataMember()>
        Public Property TransferToSubteamName As String
        <DataMember()>
        Public Property PackageDesc1 As Integer
        <DataMember()>
        Public Property PackageDesc2 As Decimal
        <DataMember()>
        Public Property PackageUnitAbbr As String
        <DataMember()>
        Public Property NotAvailable As Boolean
        <DataMember()>
        Public Property DiscontinueItem As Boolean
        <DataMember()>
        Public Property SignDescription As String
        <DataMember()>
        Public Property SoldByWeight As Boolean
        <DataMember()>
        Public Property WFMItem As Boolean
        <DataMember()>
        Public Property HFMItem As Boolean
        <DataMember()>
        Public Property RetailSale As Boolean
        <DataMember()>
        Public Property VendorUnitId As Integer
        <DataMember()>
        Public Property VendorUnitName As String
        <DataMember()>
        Public Property PriceChgTypeDesc As String
        <DataMember()>
        Public Property VendorName As String
        <DataMember()>
        Public Property CostedByWeight As Boolean
        <DataMember()>
        Public Property RetailSubteamName As String
        <DataMember()>
        Public Property VendorID As Integer
        <DataMember()> _
        Public Property retailUnitName() As String
        <DataMember()> _
        Public Property retailUnitId As Integer
        <DataMember()> _
        Public Property vendorPack() As String
        <DataMember()> _
        Public Property vendorCost() As Decimal
        <DataMember()> _
        Public Property GLAcct() As Integer
        <DataMember()>
        Public Property StoreVendorID As Integer
        <DataMember()>
        Public Property IsItemAuthorized As Boolean

        Public Sub New()

        End Sub

        Public Sub New(ByVal dt As DataTable)
            If dt.Rows.Count > 0 Then
                Me.ItemKey = dt.Rows(0).Item("Item_Key")
                Me.Identifier = dt.Rows(0).Item("Identifier")
                Me.ItemDescription = dt.Rows(0).Item("Item_Description")
                If dt.Columns.Contains("POS_Description") Then
                    Me.POSDescription = dt.Rows(0).Item("POS_Description")
                End If
                If dt.Columns.Contains("RetailSubTeam_No") Then
                    Me.RetailSubteamNo = dt.Rows(0).Item("RetailSubTeam_No")
                End If
                If dt.Columns.Contains("Package_Desc1") Then
                    Me.PackageDesc1 = dt.Rows(0).Item("Package_Desc1")
                End If
                If dt.Columns.Contains("Package_Desc2") Then
                    Me.PackageDesc2 = dt.Rows(0).Item("Package_Desc2")
                End If
                If dt.Columns.Contains("Package_Unit_Abbr") Then
                    Me.PackageUnitAbbr = dt.Rows(0).Item("Package_Unit_Abbr")
                End If
                If dt.Columns.Contains("Not_Available") Then
                    Me.NotAvailable = dt.Rows(0).Item("Not_Available")
                End If
                If dt.Columns.Contains("Discontinue_Item") Then
                    Me.DiscontinueItem = dt.Rows(0).Item("Discontinue_Item")
                End If
                If dt.Columns.Contains("Sign_Description") Then
                    Me.SignDescription = dt.Rows(0).Item("Sign_Description")
                End If
                If dt.Columns.Contains("Sold_By_Weight") Then
                    Me.SoldByWeight = dt.Rows(0).Item("Sold_By_Weight")
                End If
                If dt.Columns.Contains("WFM_Item") Then
                    Me.WFMItem = dt.Rows(0).Item("WFM_Item")
                End If
                If dt.Columns.Contains("HFM_Item") Then
                    Me.HFMItem = dt.Rows(0).Item("HFM_Item")
                End If
                If dt.Columns.Contains("Retail_Sale") Then
                    Me.RetailSale = dt.Rows(0).Item("Retail_Sale")
                End If
                If dt.Columns.Contains("Vendor_Unit_ID") Then
                    Me.VendorUnitId = dt.Rows(0).Item("Vendor_Unit_ID")
                End If
                If dt.Columns.Contains("Vendor_Unit_Name") Then
                    Me.VendorUnitName = dt.Rows(0).Item("Vendor_Unit_Name")
                End If
                If dt.Columns.Contains("Multiple") Then
                    Me.Multiple = dt.Rows(0).Item("Multiple")
                End If
                If dt.Columns.Contains("Price") Then
                    Me.Price = dt.Rows(0).Item("Price")
                End If
                If dt.Columns.Contains("PriceChgTypeDesc") Then
                    Me.PriceChgTypeDesc = dt.Rows(0).Item("PriceChgTypeDesc")
                End If
                If dt.Columns.Contains("On_Sale") Then
                    Me.OnSale = dt.Rows(0).Item("On_Sale")
                End If
                If dt.Columns.Contains("Sale_Multiple") Then
                    Me.SaleMultiple = dt.Rows(0).Item("Sale_Multiple")
                End If
                If dt.Columns.Contains("Sale_Start_Date") Then
                    Me.SaleStartDate = NotNull(dt.Rows(0).Item("Sale_Start_Date"), Nothing)
                End If
                If dt.Columns.Contains("Sale_End_Date") Then
                    Me.SaleEndDate = NotNull(dt.Rows(0).Item("Sale_End_Date"), Nothing)
                End If
                If dt.Columns.Contains("Sale_Price") Then
                    Me.SalePrice = dt.Rows(0).Item("Sale_Price")
                End If
                If dt.Columns.Contains("Sale_Earned_Disc1") Then
                    Me.SaleEarnedDisc1 = dt.Rows(0).Item("Sale_Earned_Disc1")
                End If
                If dt.Columns.Contains("Sale_Earned_Disc2") Then
                    Me.SaleEarnedDisc2 = dt.Rows(0).Item("Sale_Earned_Disc2")
                End If
                If dt.Columns.Contains("Sale_Earned_Disc3") Then
                    Me.SaleEarnedDisc3 = dt.Rows(0).Item("Sale_Earned_Disc3")
                End If
                If dt.Columns.Contains("AvgCost") Then
                    Me.AvgCost = dt.Rows(0).Item("AvgCost")
                End If
                If dt.Columns.Contains("CanInventory") Then
                    Me.CanInventory = dt.Rows(0).Item("CanInventory")
                End If
                If dt.Columns.Contains("IsSellable") Then
                    Me.IsSellable = dt.Rows(0).Item("IsSellable")
                End If
                If dt.Columns.Contains("VendorName") Then
                    Me.VendorName = dt.Rows(0).Item("VendorName")
                End If
                If dt.Columns.Contains("CostedByWeight") Then
                    Me.CostedByWeight = dt.Rows(0).Item("CostedByWeight")
                End If
                If dt.Columns.Contains("RetailSubTeam_Name") Then
                    Me.RetailSubteamName = dt.Rows(0).Item("RetailSubTeam_Name")
                End If
                If dt.Columns.Contains("Vendor_ID") Then
                    Me.VendorID = dt.Rows(0).Item("Vendor_ID")
                End If
                If dt.Columns.Contains("Retail_Unit_Name") Then
                    Me.retailUnitName = dt.Rows(0).Item("Retail_Unit_Name")
                End If
                If dt.Columns.Contains("Retail_Unit_ID") Then
                    Me.retailUnitId = dt.Rows(0).Item("Retail_Unit_ID")
                End If
                If dt.Columns.Contains("Vendor_Cost") Then
                    Me.vendorCost = dt.Rows(0).Item("Vendor_Cost")
                End If
                If dt.Columns.Contains("VendorPack") Then
                    Me.vendorPack = dt.Rows(0).Item("VendorPack")
                End If
                If dt.Columns.Contains("GLAcct") Then
                    Me.GLAcct = dt.Rows(0).Item("GLAcct")
                End If
                If dt.Columns.Contains("StoreVendor_ID") Then
                    Me.StoreVendorID = dt.Rows(0).Item("StoreVendor_ID")
                End If
                If dt.Columns.Contains("IsItemAuthorized") Then
                    Me.IsItemAuthorized = dt.Rows(0).Item("IsItemAuthorized")
                End If
            End If
        End Sub

        Public Function GetStoreItem(ByVal iStoreNo As Integer, _
                                     ByVal iTransferToSubteam_To As Integer, _
                                     ByVal iUser_ID As Integer, _
                                     ByVal iItem_Key As Integer, _
                                     ByVal sIdentifier As String) As StoreItem

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Try
                Dim dtResult As New DataTable
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If iStoreNo = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iStoreNo
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransferToSubTeam_No"
                If iTransferToSubteam_To = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iTransferToSubteam_To
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                If iUser_ID = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iUser_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                If iItem_Key = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iItem_Key
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                If sIdentifier = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = sIdentifier
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                dtResult = factory.GetStoredProcedureDataTable("WFMM_GetItem", paramList)

                Dim si As New StoreItem(dtResult)

                si.StoreNo = iStoreNo
                si.TransferToSubteamNo = iTransferToSubteam_To
                si.UserID = iUser_ID

                Return si
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function AddToOrderQueue(ByVal IsTransfer As Boolean, _
                                        ByVal IsCredit As Boolean, _
                                        ByVal Quantity As Decimal, _
                                        ByVal UnitID As Integer, _
                                        ByVal iUser_ID As Integer) As Boolean

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = Me.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransferToSubTeam_No"
                currentParam.Value = Me.TransferToSubteamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = Me.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Transfer"
                currentParam.Value = IsTransfer
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = iUser_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Quantity"
                currentParam.Value = Quantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Unit_ID"
                currentParam.Value = UnitID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Credit"
                currentParam.Value = IsCredit
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("InsertOrderItemQueue", paramList)

                Return True
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function AddToReprintSignQueue(ByVal lUser_ID As Long, _
                                         ByVal iSourceType As Integer, _
                                         ByVal sItemList As String,
                                         ByVal sItemListSeperator As String, _
                                         ByVal iStoreNo As Integer) As Boolean

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "ItemList"
                currentParam.Value = sItemList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemListSeparator"
                currentParam.Value = CType(sItemListSeperator, Char)
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = iStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Printed"
                currentParam.Value = False
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = lUser_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Type"
                currentParam.Value = iSourceType
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("UpdateSignQueuePrinted", paramList)
                Return True
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetStoreItemCycleCountInfo(Optional ByVal lInventoryLocationID As Long = 0) As CycleCountInfo

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outParamList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = Me.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                currentParam.Value = Me.TransferToSubteamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvLocID"
                currentParam.Value = lInventoryLocationID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = Me.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Count"
                currentParam.Value = Nothing
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                currentParam.Value = Nothing
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                outParamList = factory.ExecuteStoredProcedure("GetStoreItemCycleCountInfo", paramList)

                Dim cci As New CycleCountInfo
                cci.Quantity = outParamList.Item(0).ToString
                cci.Weight = outParamList.Item(1).ToString

                Return cci
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetTransferItem(ByVal iItem_Key As Integer, _
                             ByVal sIdentifier As String, _
                             ByVal iProductType_ID As Integer, _
                             ByVal iVendorStore_No As Integer, _
                             ByVal iVendor_ID As Integer, _
                             ByVal iTransfer_SubTeam As Integer, _
                             ByVal iSupplySubTeam_No As Integer) As StoreItem

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Try
                Dim dtResult As New DataTable
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                If iItem_Key = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iItem_Key
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                If sIdentifier = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = sIdentifier
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ProductType_ID"
                If iProductType_ID = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iProductType_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VendStore_No"
                If iVendorStore_No = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iVendorStore_No
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                If iVendorStore_No = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iVendor_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Transfer_SubTeam"
                If iTransfer_SubTeam = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iTransfer_SubTeam
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SupplySubTeam_No"
                If iSupplySubTeam_No = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iSupplySubTeam_No
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                dtResult = factory.GetStoredProcedureDataTable("WFMM_GetTransferItem", paramList)

                Dim item As New StoreItem(dtResult)

                Return item

            Catch ex As Exception
                Throw

            Finally
                connectionCleanup(factory)

            End Try

        End Function

    End Class
End Namespace