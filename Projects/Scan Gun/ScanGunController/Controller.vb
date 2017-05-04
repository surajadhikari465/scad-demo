Public Class Controller
    Private m_user As ItemCatalog.User
    Private m_stores As ArrayList
    Private m_store As ItemCatalog.Store
    Private m_subteam As ItemCatalog.SubTeam
    Private m_store_item As ItemCatalog.StoreItem
    Private m_cycle_count As ItemCatalog.CycleCount
    Private m_inventory_location As ItemCatalog.InventoryLocation
    Private m_order As ItemCatalog.Order
    Private m_item_units As ArrayList
    Private m_inventory_locations As ArrayList

    Public Enum WasteCategory
        Spoilage = 0
        Foodbank = 1
        Sampling = 2
    End Enum

    Public WasteType As WasteCategory
    Public IsWasteSplit As Boolean = False

    Public Enum SignType
        Grocery = 0
        Nutrition = 1
    End Enum
    Public Structure CycleCountInfo
        Public Quantity As Decimal
        Public Weight As Decimal
    End Structure
    Public Structure InventoryLocation
        Public ID As Long
        Public Name As String
    End Structure

    Public Structure OrderInfo
        Public PrimaryVendorKey As String
        Public PrimaryVendorName As String
        Public OnOrder As Decimal
        Public InQueue As Decimal
        Public InQueueCredit As Decimal
        Public InQueueTransfer As Decimal
        Public LastReceivedDate As Date
        Public LastReceived As Decimal
    End Structure
    Public Structure OrderItem
        Public QuantityOrdered As Decimal
        Public QuantityReceived As Decimal
        Public Total_Weight As Decimal
        Public IsReceivedWeightRequired As Boolean
    End Structure
    Public Structure Store
        Public Store_No As Int32
        Public Store_Name As String
        Public Mega_Store As Boolean
        Public WFM_Store As Boolean
    End Structure
    Public Structure StoreItem
        Public Item_Key As Int32
        Public Identifier As String
        Public Sign_Description As String
        Public SubTeam_No As Int32
        Public SubTeam_Name As String
        Public Package_Desc1 As Decimal
        Public Package_Desc2 As Decimal
        Public Package_Unit_Abbr As String
        Public IsNotAvailable As Boolean
        Public IsDiscontinued As Boolean
        Public IsSoldByWeight As Boolean
        Public IsSoldWFM As Boolean
        Public IsSoldHFM As Boolean
        Public IsForRetailSale As Boolean
        Public StoreNo As Int32
        Public Price As Decimal
        Public Multiple As Byte
        Public IsOnSale As Boolean
        Public IsSaleEDLP As Boolean
        Public SaleStartDate As Date
        Public SaleEndDate As Date
        Public SaleMultiple As Byte
        Public SalePrice As Decimal
        Public SaleEarnedDisc1 As Byte
        Public SaleEarnedDisc2 As Byte
        Public SaleEarnedDisc3 As Byte
        Public AvgCost As Decimal
        Public VendorOrderUnitID As Int32
        Public VendorOrderUnitName As String
        Public PriceChgTypeDesc As String
        Public PackSize As Decimal
        Public VendorName As String
        'Public VendorPack As Decimal
    End Structure
    Public Structure SubTeam
        Public Number As Int32
        Public Name As String
        Public Abbreviation As String
        Public Sub New(ByVal Number As Int32, ByVal Name As String, ByVal Abbreviation As String)
            Me.Number = Number
            Me.Name = Name
            Me.Abbreviation = Abbreviation
        End Sub
    End Structure
    Public Structure Vendor
        Public ID As Long
        Public Key As String
        Public IsPrimary As Boolean
        Public Cost As Decimal
        Public Sub New(ByVal ID As Long, ByVal Key As String, ByVal IsPrimary As Boolean, ByVal Cost As Decimal)
            Me.ID = ID
            Me.Key = Key
            Me.IsPrimary = IsPrimary
            Me.Cost = Cost
        End Sub
    End Structure
    Public ReadOnly Property MaximumQuantity() As Decimal
        Get
            Return 9999
        End Get
    End Property
    Public ReadOnly Property IsAccountEnabled() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsAccountEnabled
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property IsBuyer() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsBuyer
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property IsShrinkUser() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsShrinkUser
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property IsCoordinator() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsCoordinator
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property IsInventoryAdministrator() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsInventoryAdministrator
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property IsReceiver() As Boolean
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.IsReceiver
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property StoreLimit() As Int32
        Get
            If Not (m_user Is Nothing) Then
                Return m_user.StoreLimit
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Store_No() As Int32
        Get
            If Not (m_store Is Nothing) Then
                Return m_store.Store_No
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Store_Name() As String
        Get
            If Not (m_store Is Nothing) Then
                Return m_store.Store_Name
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public ReadOnly Property Mega_Store() As Boolean
        Get
            If Not (m_store Is Nothing) Then
                Return m_store.Mega_Store
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property WFM_Store() As Boolean
        Get
            If Not (m_store Is Nothing) Then
                Return m_store.WFM_Store
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property User_ID() As Int32
        Get
            If Not (m_user Is Nothing) Then
                Return Me.m_user.User_ID
            Else
                Return 0
            End If
        End Get
    End Property
    Public Sub AddWaste(ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal WasteType As Decimal)

        '-- Keep them from entering a barcode as the quantity/weight
        If Quantity > Me.MaximumQuantity Then Throw New ScanGunController.Exception.UnitsOutOfRangeException
        If Weight > Me.MaximumQuantity Then Throw New ScanGunController.Exception.UnitsOutOfRangeException

        Dim reason As String = String.Empty

        Select Case WasteType
            Case 0 : reason = "Spoilage"
            Case 1 : reason = "Food Bank"
            Case 2 : reason = "Samples"
        End Select
        Me.m_store.AddItemHistoryShrink(Me.m_store_item.Item_Key, Me.m_store_item.SubTeam_No, _
                        ItemCatalog.enuItemAdjustment.Waste, reason, Date.Now, Quantity, _
                        Weight, Me.m_user.User_ID, WasteType)
    End Sub
    Public Sub Clear()
        Me.m_store_item = Nothing
        Me.m_cycle_count = Nothing
        Me.m_inventory_location = Nothing
        If Not (Me.m_inventory_locations Is Nothing) Then
            Me.m_inventory_locations.Clear()
            Me.m_inventory_locations = Nothing
        End If
        Me.m_order = Nothing
    End Sub
    Public Function GetCycleCountInfo(Optional ByVal InventoryLocationID As Long = 0) As ScanGunController.Controller.CycleCountInfo
        Dim result As New ScanGunController.Controller.CycleCountInfo
        If Not (Me.m_store_item Is Nothing) Then
            Dim ci As ItemCatalog.StoreItem.CycleCountInfo = Me.m_store_item.GetCycleCountInfo(InventoryLocationID)
            result.Quantity = ci.Quantity
            result.Weight = ci.Weight
        End If
        Return result
    End Function
    Public Function GetInventoryLocation() As ScanGunController.Controller.InventoryLocation
        Dim il As New ScanGunController.Controller.InventoryLocation
        If Not (Me.m_inventory_location Is Nothing) Then
            il.ID = Me.m_inventory_location.ID
            il.Name = Me.m_inventory_location.Name
        End If
        Return il
    End Function
    Public Function GetInventoryLocations() As ArrayList
        If Me.m_inventory_locations Is Nothing Then
            Me.m_inventory_locations = New ArrayList
            Dim il As ArrayList = ItemCatalog.InventoryLocation.GetInventoryLocations(Me.m_store.Store_No, Me.m_subteam.Number)
            Dim l As ItemCatalog.InventoryLocation
            Dim loc As ScanGunController.Controller.InventoryLocation
            Dim i As Long
            For i = 0 To il.Count - 1
                loc = New ScanGunController.Controller.InventoryLocation
                l = CType(il(i), ItemCatalog.InventoryLocation)
                loc.ID = l.ID
                loc.Name = l.Name
                Me.m_inventory_locations.Add(loc)
            Next
        End If
        Return Me.m_inventory_locations
    End Function
    Public Function GetItemUnitID(ByVal UnitName As String) As Int32
        If m_item_units Is Nothing Then
            m_item_units = ItemCatalog.Item.GetItemUnits
        End If
        Dim iu As New ItemCatalog.ItemUnit
        iu.UnitName = UnitName
        Dim i As Int32 = m_item_units.BinarySearch(iu, New ItemCatalog.ItemUnitNameGet)
        If i >= 0 Then Return CType(m_item_units(i), ItemCatalog.ItemUnit).UnitID
    End Function
    Public Function GetRetailStores() As ArrayList
        If Me.m_stores Is Nothing Then
            m_stores = New ArrayList
            Dim stores As ArrayList = ItemCatalog.Store.GetRetailStores
            Dim s As ScanGunController.Controller.Store
            Dim store As ItemCatalog.Store
            Dim i As Int32
            For i = 0 To stores.Count - 1
                s = New ScanGunController.Controller.Store
                store = CType(stores(i), ItemCatalog.Store)
                s.Store_No = store.Store_No
                s.Store_Name = store.Store_Name
                s.Mega_Store = store.Mega_Store
                s.WFM_Store = store.WFM_Store
                m_stores.Add(s)
            Next
        End If
        Return m_stores
    End Function
    Public Function GetThisRegionsStores() As ArrayList
        If Me.m_stores Is Nothing Then
            m_stores = New ArrayList
            Dim stores As ArrayList = ItemCatalog.Facility.GetThisRegionsFacilities()
            Dim s As ScanGunController.Controller.Store
            Dim store As ItemCatalog.Store
            Dim i As Int32
            For i = 0 To stores.Count - 1
                s = New ScanGunController.Controller.Store
                store = CType(stores(i), ItemCatalog.Store)
                s.Store_No = store.Store_No
                s.Store_Name = store.Store_Name
                s.Mega_Store = store.Mega_Store
                s.WFM_Store = store.WFM_Store
                m_stores.Add(s)
            Next
        End If
        Return m_stores
    End Function
    Public Function GetPONumber() As Long
        If Not (Me.m_order Is Nothing) Then
            Return Me.m_order.OrderHeader_ID
        Else
            Return 0
        End If
    End Function
    Public Function GetOrderInfo() As ScanGunController.Controller.OrderInfo
        Dim result As New ScanGunController.Controller.OrderInfo
        If Not (Me.m_store_item Is Nothing) Then
            Dim oi As ItemCatalog.StoreItem.OrderInfo = Me.m_store_item.GetOrderInfo
            result.PrimaryVendorKey = oi.PrimaryVendorKey
            result.PrimaryVendorName = oi.PrimaryVendorName
            result.InQueue = oi.InQueue
            result.InQueueCredit = oi.InQueueCredit
            result.InQueueTransfer = oi.InQueueTransfer
            result.OnOrder = oi.OnOrder
            result.LastReceivedDate = oi.LastReceivedDate
            result.LastReceived = oi.LastReceived
        End If
        Return result
    End Function
    Public Function GetOrderItem() As ScanGunController.Controller.OrderItem
        Dim result As New ScanGunController.Controller.OrderItem
        Dim oi As ItemCatalog.OrderItem = Me.m_order.GetOrderItem(Me.m_store_item.Item_Key)
        If Not (oi Is Nothing) Then
            result.QuantityOrdered = oi.QuantityOrdered
            result.QuantityReceived = oi.QuantityReceived
            result.Total_Weight = oi.Total_Weight
            result.IsReceivedWeightRequired = oi.IsReceivedWeightRequired
        End If
        Return result
    End Function
    Public Function GetRetailStore() As ScanGunController.Controller.Store
        Dim s As New ScanGunController.Controller.Store
        If Not (Me.m_store Is Nothing) Then
            s.Store_No = Me.m_store.Store_No
            s.Store_Name = Me.m_store.Store_Name
            s.Mega_Store = Me.m_store.Mega_Store
            s.WFM_Store = Me.m_store.WFM_Store
        End If
        Return s
    End Function
    Public Function GetPackSize(ByVal ItemKey As Int32, ByVal StoreNo As Int32) As Decimal
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim psize As Decimal = 0

        Try
            cmd.CommandType = Data.CommandType.Text
            cmd.CommandText = "SELECT dbo.fn_GetCurrentVendorPackage_Desc1(" + CStr(ItemKey) + ", " + CStr(StoreNo) + ") as PackSize"

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                If dr!PackSize Is DBNull.Value Then
                    psize = 0
                Else
                    psize = CType(dr!PackSize, Decimal)
                End If
            Else
                Throw New System.Exception("Invalid SubTeam No")
            End If

            Return psize
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Function GetItemKey(ByVal sIdentifier As String) As Integer
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim psize As Decimal = 0

        Try
            cmd.CommandType = Data.CommandType.Text
            cmd.CommandText = "SELECT Item_Key FROM ItemIdentifier WHERE Identifier = '" & sIdentifier & "' AND Deleted_Identifier = 0"

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows Then
                dr.Read()
                Return dr!Item_Key
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Function GetStoreItem() As ScanGunController.Controller.StoreItem
        Dim si As New ScanGunController.Controller.StoreItem
        si.Identifier = Me.m_store_item.Identifier
        si.IsDiscontinued = Me.m_store_item.IsDiscontinued
        si.IsForRetailSale = Me.m_store_item.IsForRetailSale
        si.IsNotAvailable = Me.m_store_item.IsNotAvailable
        si.IsOnSale = Me.m_store_item.IsOnSale
        si.IsSaleEDLP = Me.m_store_item.IsSaleEDLP
        si.IsSoldByWeight = Me.m_store_item.IsSoldByWeight
        si.IsSoldHFM = Me.m_store_item.IsSoldHFM
        si.IsSoldWFM = Me.m_store_item.IsSoldWFM
        si.Item_Key = Me.m_store_item.Item_Key
        si.Multiple = Me.m_store_item.Multiple
        si.Package_Desc1 = Me.m_store_item.Package_Desc1
        si.Package_Desc2 = Me.m_store_item.Package_Desc2
        si.Package_Unit_Abbr = Me.m_store_item.Package_Unit_Abbr
        si.Price = Me.m_store_item.Price
        si.PriceChgTypeDesc = Me.m_store_item.PriceChgTypeDesc
        si.SaleEarnedDisc1 = Me.m_store_item.SaleEarnedDiscount1
        si.SaleEarnedDisc2 = Me.m_store_item.SaleEarnedDiscount2
        si.SaleEarnedDisc3 = Me.m_store_item.SaleEarnedDiscount3
        si.SaleEndDate = Me.m_store_item.SaleEndDate
        si.SaleMultiple = Me.m_store_item.SaleMultiple
        si.SalePrice = Me.m_store_item.SalePrice
        si.SaleStartDate = Me.m_store_item.SaleStartDate
        si.Sign_Description = Me.m_store_item.Sign_Description
        si.StoreNo = Me.m_store_item.StoreNo
        si.SubTeam_Name = Me.m_store_item.SubTeamName
        si.SubTeam_No = Me.m_store_item.SubTeam_No
        si.VendorOrderUnitID = Me.m_store_item.VendorOrderUnitID
        si.VendorOrderUnitName = Me.m_store_item.VendorOrderUnitName
        si.PackSize = Me.GetPackSize(Me.m_store_item.Item_Key, Me.m_store_item.StoreNo)
        si.VendorName = Me.m_store_item.VendorName
 
        Return si
    End Function
    Public Function GetStoreSubTeams() As ArrayList
        Dim result As New ArrayList
        Dim subteams As ArrayList = m_store.GetSubTeams
        Dim subteam As ItemCatalog.SubTeam
        Dim i As Int32
        For i = 0 To subteams.Count - 1
            subteam = CType(subteams(i), ItemCatalog.SubTeam)
            'This left out Marketing expense, which they need to be able to order for.  The purpose was to limit the subteam list
            'that the users have to scroll through, but rather than add another SubTeamType_ID for Marketing Expense, we decided
            'to just show all the subteams.
            'If Not subteam.IsExpense Then result.Add(New Controller.SubTeam(subteam.Number, subteam.Name, subteam.Abbreviation))
            result.Add(New Controller.SubTeam(subteam.Number, subteam.Name, subteam.Abbreviation))
        Next
        Return result
    End Function
    Public Function GetSubTeam() As ScanGunController.Controller.SubTeam
        Dim s As New ScanGunController.Controller.SubTeam
        If Not (Me.m_subteam Is Nothing) Then
            s.Number = Me.m_subteam.Number
            s.Name = Me.m_subteam.Name
            If Me.m_subteam.Abbreviation.Length > 4 Then
                s.Abbreviation = Me.m_subteam.Abbreviation.Substring(0, 4)
            ElseIf Me.m_subteam.Abbreviation.Length > 0 Then
                s.Abbreviation = Me.m_subteam.Abbreviation
            Else
                s.Abbreviation = Me.m_subteam.Name.Substring(0, 4)
            End If
        Else
            s.Number = 0
            s.Name = String.Empty
            s.Abbreviation = String.Empty
        End If
        Return s
    End Function
    Public Function GetVendors() As ArrayList
        Dim result As New ArrayList
        If Not (Me.m_store_item Is Nothing) Then
            Dim list As ArrayList = Me.m_store_item.GetVendors
            Dim i As Long
            Dim vend As ItemCatalog.StoreItem.Vendor
            For i = 0 To list.Count - 1
                vend = CType(list(i), ItemCatalog.StoreItem.Vendor)
                Dim v As New ScanGunController.Controller.Vendor(vend.VendorID, IIf(vend.Key.Length > 0, vend.Key, IIf(vend.CompanyName.Length > 0, vend.CompanyName.Substring(0, IIf(vend.CompanyName.Length >= 10, 10, vend.CompanyName.Length)), String.Empty)).ToString.Trim, vend.IsPrimary, vend.UnitCost + vend.UnitFreight)
                result.Add(v)
            Next
        End If
        Return result
    End Function
    Public Function GetWirelessPrinters() As ArrayList
        If Not (Me.m_store Is Nothing) Then
            Dim list As ArrayList = Me.m_store.GetWirelessPrinters
            Dim p As ItemCatalog.ReferenceList
            Dim result As New ArrayList
            Dim i As Long
            For i = 0 To list.Count - 1
                p = list(i)
                result.Add(New ScanGunController.ReferenceList(p.ListID, p.ListDesc))
            Next
            Return result
        Else
            Return New ArrayList
        End If
    End Function
    Public Sub LogonUser(ByVal UserName As String, ByVal Password As String)
        Try
            m_user = New ItemCatalog.User(UserName, Password)
        Catch ex As ItemCatalog.Exception.InvalidLogonException
            Throw New ScanGunController.Exception.InvalidLogonException(ex.Message)
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
    Public Sub AddToCycleCount(ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal InvLocID As Long, ByVal IsCaseCnt As Boolean)

        '-- Keep them from entering a barcode as the quantity/weight
        If Quantity > Me.MaximumQuantity Then Throw New ScanGunController.Exception.UnitsOutOfRangeException
        If Weight > Me.MaximumQuantity Then Throw New ScanGunController.Exception.UnitsOutOfRangeException

        Try
            If Me.m_cycle_count Is Nothing Then Me.m_cycle_count = ItemCatalog.CycleCount.GetCycleCount(Me.m_store.Store_No, Me.m_subteam.Number)
            If Me.m_cycle_count Is Nothing Then
                Throw New ScanGunController.Exception.CycleCount.NoMasterException
            Else
                Me.m_cycle_count.AddItemInternal(Date.Now, InvLocID, Me.m_store_item, Quantity, Weight, Me.m_store_item.Package_Desc1, IsCaseCnt)
            End If
        Catch ex As ItemCatalog.Exception.CycleCount.BeforeStartScanException
            Throw New ScanGunController.Exception.CycleCount.BeforeStartScanException(ex.StartScan)
        Catch ex As ItemCatalog.Exception.CycleCount.ExceedsEndScanException
            Throw New ScanGunController.Exception.CycleCount.ExceedsEndScanException(ex.EndScan)
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
    Public Sub AddToInventoryLocation()
        Try
            Me.m_inventory_location.AddItem(Me.m_store_item)
        Catch ex As ItemCatalog.Exception.InventoryLocationItemExistsException
            Throw New ScanGunController.Exception.InventoryLocationItemExistsException
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
    Public Sub AddToOrderQueue(ByVal IsTransfer As Boolean, ByVal IsCredit As Boolean, ByVal Quantity As Decimal, ByVal UnitID As Int32)
        Me.m_store_item.AddToOrderQueue(IsTransfer, IsCredit, Quantity, UnitID, Me.m_user.User_ID)
    End Sub
    Public Sub AddToReprintSignQueue()
        Me.m_store_item.AddToReprintSignQueue(Me.m_user.User_ID, ItemCatalog.enuReprintSignSource.ScanGun)
    End Sub
    Public Sub PrintSignNow(ByVal Type As SignType, ByVal PrinterNetworkName As String, ByVal Copies As Integer)
        Dim print As New PrintShelfTagNow.Print
        print.PrintSignNow(Me.m_store_item, Me.m_store, Type, PrinterNetworkName, Copies)
        print = Nothing
    End Sub
    Public Sub ReceiveOrderItem(ByVal Quantity As Decimal, ByVal Weight As Decimal)
        Try
            Dim oi As ItemCatalog.OrderItem = Me.m_order.GetOrderItem(Me.m_store_item.Item_Key)
            If oi Is Nothing Then oi = Me.m_order.AddOrderItem(Me.m_store_item)
            Me.m_order.ReceiveOrderItem(Me.m_store_item.Item_Key, Quantity, Weight, Date.Now, False, Me.m_user.User_ID)
        Catch ex As ItemCatalog.Exception.Order.NotSentException
            Throw New ScanGunController.Exception.Order.NotSentException
        Catch ex As ItemCatalog.Exception.OrderItem.ReceivedWeightMissingException
            Throw New ScanGunController.Exception.OrderItem.ReceivedWeightMissingException
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function GetItemSubTeamNo(ByVal lItem_Key As Long) As Integer
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim SubTeamNo As Integer

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = "GetItem"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.Int32
            prm.ParameterName = "@Item_Key"
            prm.Value = CObj(lItem_Key)
            cmd.Parameters.Add(prm)

            Dim prm1 As New System.Data.SqlClient.SqlParameter
            prm1.Direction = Data.ParameterDirection.Input
            prm1.DbType = Data.DbType.String
            prm1.ParameterName = "@Identifier"
            prm1.Value = System.DBNull.Value
            cmd.Parameters.Add(prm1)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                SubTeamNo = CType(dr!SubTeam_No, Long)
            Else
                Throw New System.Exception("Invalid Item Key")
            End If

            Return SubTeamNo
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Shared Function IsSplitWasteCategory() As Boolean

        Dim flag As Boolean

        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = "GetInstanceDataFlagValue"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.String
            prm.ParameterName = "@FlagKey"
            prm.Value = "SplitWasteCategory"
            cmd.Parameters.Add(prm)

            Dim prm1 As New System.Data.SqlClient.SqlParameter
            prm1.Direction = Data.ParameterDirection.Input
            prm1.DbType = Data.DbType.Int32
            prm1.ParameterName = "@Store_No"
            prm1.Value = CObj(System.DBNull.Value)
            cmd.Parameters.Add(prm1)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                flag = CType(dr!FlagValue, Boolean)
            Else
                Throw New System.Exception("Invalid Instance Data Flag (SplitWasteCategory)")
            End If

            Return flag

            'Return SubTeamName
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try

    End Function

    Public Shared Function GetSubTeamName(ByVal SubTeamNo As Integer) As String
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim SubTeamName As String
        Dim charSeparators() As Char = {"-"c, " "c}
        Dim results() As String

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = "GetSubTeamName"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.Int32
            prm.ParameterName = "@SubTeam_No"
            prm.Value = CObj(SubTeamNo)
            cmd.Parameters.Add(prm)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                SubTeamName = Trim(CType(dr!SubTeam_Name, String))
            Else
                Throw New System.Exception("Invalid SubTeam No")
            End If

            results = SubTeamName.Split(charSeparators, StringSplitOptions.None)

            Dim s As String = results(results.Length - 1).ToLower
            Return s

            'Return SubTeamName
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Shared Function IsFixedSpoilage(ByVal SubTeamNo As Integer) As Boolean
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim flag As Boolean = False

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = "GetFixedSpoilageFlag"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.Int32
            prm.ParameterName = "@SubTeam_No"
            prm.Value = SubTeamNo
            cmd.Parameters.Add(prm)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                flag = CType(dr!FixedSpoilage, Boolean)
            Else
                Throw New System.Exception("Invalid SubTeam No")
            End If

            Return flag
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Shared Function IsRetailNotCostedByWeight(ByVal lItem_Key As Long) As Boolean
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim NotCostedByWeight As Boolean = False

        Try
            cmd.CommandType = Data.CommandType.Text
            cmd.CommandText = "SELECT dbo.fn_IsRetailUnitNotCostedByWeight(" + CStr(lItem_Key) + ") as RetailNotCBW"

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                If dr!RetailNotCBW Is DBNull.Value Then
                    NotCostedByWeight = False
                Else
                    NotCostedByWeight = CType(dr!RetailNotCBW, Boolean)
                End If
            Else
                Throw New System.Exception("Invalid SubTeam No")
            End If

            Return NotCostedByWeight
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try

    End Function

    Public Shared Function GetAverageUnitWeight(ByVal sIdentifier As String) As Decimal
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand

        Dim avgUnitWeight As Decimal = 0.0

        Try
            cmd.CommandType = Data.CommandType.Text
            cmd.CommandText = "SELECT dbo.fn_GetAverageUnitWeight('" + sIdentifier + "') as AverageUnitWeight"

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                If dr!AverageUnitWeight Is DBNull.Value Then
                    avgUnitWeight = 0.0
                Else
                    avgUnitWeight = CType(dr!AverageUnitWeight, Decimal)
                End If
            Else
                Throw New System.Exception("Invalid SubTeam No")
            End If

            Return avgUnitWeight
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try

    End Function

    Private Sub GetSubTeamInfo(ByVal procedureName As String, Optional ByVal SubTeam_No As Int32 = -1)
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = procedureName

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.Int32
            prm.ParameterName = "@User_ID"
            prm.Value = Me.User_ID
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = Data.ParameterDirection.Input
            prm.DbType = Data.DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)

            If SubTeam_No > -1 Then
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = Data.ParameterDirection.Input
                prm.DbType = Data.DbType.Int32
                prm.ParameterName = "@SubTeam_No"
                prm.Value = SubTeam_No
                cmd.Parameters.Add(prm)
            End If

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows AndAlso dr.Read() AndAlso Not IsDBNull(dr!SubTeam_No) Then
                Me.m_subteam = New ItemCatalog.SubTeam(dr!SubTeam_No, CStr(dr!SubTeam_Name).TrimEnd, CStr(dr!SubTeam_Abbreviation).TrimEnd, Not CBool(dr!SubTeam_Unrestricted), dr!IsExpense)
            Else
                Me.m_subteam = Nothing
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, ItemCatalog.DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

    Public Sub SetRetailStore(ByVal Store_No As Int32)

        If Not (Me.m_store Is Nothing) Then
            If Store_No = Me.m_store.Store_No Then Exit Sub
        End If

        Me.m_store = New ItemCatalog.Store(Store_No)

        GetSubTeamInfo("GetScanGunStoreSubTeam")
    End Sub

    Public Sub SetRetailStoreSubTeam(ByVal SubTeam_No As Int32)

        If Not (Me.m_subteam Is Nothing) Then
            If SubTeam_No = Me.m_subteam.Number Then Exit Sub
        End If

        GetSubTeamInfo("InsertScanGunStoreSubTeam", SubTeam_No)
    End Sub

    Public Sub SetWasteType(ByVal SelectedWasteType As Int32)
        Select Case SelectedWasteType
            Case 0
                WasteType = ScanGunController.Controller.WasteCategory.Spoilage
            Case 1
                WasteType = ScanGunController.Controller.WasteCategory.Sampling
            Case 2
                WasteType = ScanGunController.Controller.WasteCategory.Foodbank
        End Select
    End Sub

    Public Sub SetInventoryLocation(ByVal ID As Long)
        Me.m_inventory_location = ItemCatalog.InventoryLocation.GetInventoryLocation(ID)
    End Sub
    Public Function SetItem(ByVal Identifier As String, Optional ByVal UseCurrentSubteam As Boolean = True) As Boolean
        Try
            If (Me.m_subteam Is Nothing) Or (Not UseCurrentSubteam) Then
                Me.m_store_item = New ItemCatalog.StoreItem(Me.m_store.Store_No, Me.User_ID, Identifier, False)
            Else
                Me.m_store_item = New ItemCatalog.StoreItem(Me.m_store.Store_No, Me.m_subteam.Number, Me.User_ID, Identifier)
            End If
            Return True
        Catch ex As ItemCatalog.Exception.InvalidIdentifierException
            Return False
        Catch ex As ItemCatalog.Exception.ItemInventorySubTeamException
            Throw New ScanGunController.Exception.ItemInventorySubTeamException
        Catch ex As System.Exception
            Throw ex
        End Try
    End Function
    Public Sub SetOrder(ByVal PONumber As Long)
        Try
            Me.m_order = New ItemCatalog.Order(PONumber)
            If Me.m_order.ReceiveStore_No <> Me.m_store.Store_No Then Throw New ScanGunController.Exception.Order.WrongStoreException
            If Me.m_order.CloseDate > Date.MinValue Then Throw New ScanGunController.Exception.Order.ClosedException
            If Me.m_order.SentDate = Date.MinValue Then Throw New ScanGunController.Exception.Order.NotSentException
            Me.SetRetailStoreSubTeam(Me.m_order.Transfer_To_SubTeam)
        Catch ex As ItemCatalog.Exception.Order.NotFoundException
            Throw New ScanGunController.Exception.Order.NotFoundException
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
    Public Sub ValidateOrderable()
        Try
            Me.m_store_item.IsOrderable(True)
        Catch ex As ItemCatalog.Exception.OrderItemQueue.NotSoldException
            Throw New ScanGunController.Exception.OrderItemQueue.NotSoldException
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
End Class
