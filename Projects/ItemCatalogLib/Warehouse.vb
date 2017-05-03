Imports log4net
Imports System.Reflection

Public Enum enuWarehouseSortOrder
    DistributionCenter_Warehouse = 0 'All that is useful now
End Enum

Public Class Warehouse
    Inherits Facility
    Dim m_business_unit_id As Long
    Dim m_distribution_center As Long
    Dim m_warehouse As Integer
    Dim m_vendor_id As Long
    Dim m_return_orders As ArrayList
    Dim m_receiving_orders As ArrayList
    Dim m_shipping_orders As ArrayList
    Dim m_cycle_counts As ArrayList
    Dim m_orders As ArrayList


    Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
    Public Property BusinessUnit() As Long
        Get
            Return m_business_unit_id
        End Get
        Set(ByVal Value As Long)
            m_business_unit_id = Value
        End Set
    End Property
    Public Property DistributionCenter() As Long
        Get
            If m_business_unit_id > 0 Then
                Return m_business_unit_id Mod 100
            Else
                Return m_distribution_center
            End If
        End Get
        Set(ByVal Value As Long)
            m_distribution_center = Value
        End Set
    End Property
    Public Property Warehouse() As Integer
        Get
            Return m_warehouse
        End Get
        Set(ByVal Value As Integer)
            m_warehouse = Value
        End Set
    End Property
    Public Property VendorID() As Long
        Get
            Return m_vendor_id
        End Get
        Set(ByVal Value As Long)
            m_vendor_id = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal lStore_No As Long, ByVal sStore_Name As String, ByVal lBusinessUnitID As Long, ByVal iWarehouse As Integer)
        Me.Store_No = lStore_No
        Me.Store_Name = sStore_Name
        Me.BusinessUnit = lBusinessUnitID
        Me.Warehouse = iWarehouse
    End Sub

    Public Sub New(ByVal lDistributionCenter As Long, ByVal iWarehouse As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetWarehouse"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@DistributionCenter"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.Value = lDistributionCenter
            Me.DistributionCenter = lDistributionCenter
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@EXEWarehouse"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int16
            prm.Value = iWarehouse
            Me.Warehouse = iWarehouse
            cmd.Parameters.Add(prm)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                Me.Warehouse = iWarehouse
                Me.Store_No = dr.GetInt32(0)
                Me.Store_Name = dr.GetString(1)
                Me.BusinessUnit = dr.GetInt32(2)
                Me.VendorID = dr.GetInt32(3)
            Else
                Throw New System.Exception("Warehouse not found based on input DistributionCenter and Warehouse numbers")
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

    Public Function GetItemChanges() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetWarehouseItemChanges"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@Store_No"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add( _
                        New ItemCatalog.Warehouse.WarehouseItemChange( _
                                 dr.GetInt32(dr.GetOrdinal("WarehouseItemChangeId")) _
                            , dr.GetString(dr.GetOrdinal("ChangeType")) _
                            , dr.GetInt32(dr.GetOrdinal("Item_Key")) _
                            , dr.GetString(dr.GetOrdinal("Identifier")) _
                            , dr.GetString(dr.GetOrdinal("Item_Description")) _
                            , dr.GetString(dr.GetOrdinal("POS_Description")) _
                            , dr.GetInt32(dr.GetOrdinal("SubTeam_No")) _
                            , dr.GetString(dr.GetOrdinal("SubTeam_Name")) _
                            , dr.GetDecimal(dr.GetOrdinal("Package_Desc1")) _
                            , dr.GetDecimal(dr.GetOrdinal("Package_Desc2")) _
                            , dr.GetBoolean(dr.GetOrdinal("Not_Available")) _
                            , dr.GetBoolean(dr.GetOrdinal("Cool")) _
                            , dr.GetBoolean(dr.GetOrdinal("BIO")) _
                            , dr.GetBoolean(dr.GetOrdinal("CatchWeightRequired")) _
                            , dr.GetString(dr.GetOrdinal("Package_Unit_Abbrev")) _
                            ) _
                        )
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Sub ClearItemChanges(ByVal itemchanges As ArrayList)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            If itemchanges.Count = 0 Then Exit Sub

            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "DeleteWarehouseItemChange"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@WarehouseItemChangeID"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            cmd.Parameters.Add(prm)
            Dim i As Long = 0
            Dim itemchange As ItemCatalog.Warehouse.WarehouseItemChange
            For i = 0 To itemchanges.Count - 1
                itemchange = itemchanges(i)
                cmd.Parameters(0).Value = itemchange.WarehouseItemChangeID
                If i = 0 Then
                    'First time - connect command object to the database
                    ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Next
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

    Public Function GetVendorChanges() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetWarehouseVendChanges"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@Store_No"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@Customer"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Boolean
            prm.Value = 0
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Warehouse.WarehouseVendorChange(dr!WarehouseVendorChangeID, dr!ChangeType, dr!Vendor_ID, dr!Vendor_Key, dr!CompanyName, dr!Address_Line_1, dr!Address_Line_2, dr!City, dr!State, dr!Zip_Code, dr!Country, dr!Phone))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Function GetCustomerChanges() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetWarehouseVendChanges"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@Store_No"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@Customer"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Boolean
            prm.Value = 1
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Warehouse.WarehouseVendorChange(dr!WarehouseVendorChangeID, dr!ChangeType, dr!Vendor_ID, dr!Vendor_Key, dr!CompanyName, dr!Address_Line_1, dr!Address_Line_2, dr!City, dr!State, dr!Zip_Code, dr!Country, dr!Phone))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Sub ClearVendorChanges(ByVal vendorChanges As ArrayList)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            If vendorChanges.Count = 0 Then Exit Sub

            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "DeleteWarehouseVendorChange"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@WarehouseVendorChangeID"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            cmd.Parameters.Add(prm)
            Dim i As Long = 0
            Dim vendorChange As ItemCatalog.Warehouse.WarehouseVendorChange
            For i = 0 To vendorChanges.Count - 1
                vendorChange = vendorChanges(i)
                cmd.Parameters(0).Value = vendorChange.WarehouseVendorChangeID
                If i = 0 Then
                    'First time - connect command object to the database
                    ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Next
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub ProcessShipping(ByVal lOrderHeader_ID As Long, ByVal sIdentifier As String, ByVal dQuantityReceived As Decimal, ByVal lPackSize As Long, ByVal dCatchWeight As Decimal, ByVal dtDate As DateTime, ByVal blnIsFlow As Boolean)
        Dim order As ItemCatalog.Order = Nothing
        Dim orderItem As ItemCatalog.OrderItem
        ' Dim item As New Item(sIdentifier)
        order = GetOrder(lOrderHeader_ID)
        If order Is Nothing Then
            Throw New System.Exception("OrderHeader_ID " & lOrderHeader_ID.ToString & " not found")
        Else
            orderItem = order.GetOrderItem(sIdentifier)
            If orderItem Is Nothing Then
                Dim storeitem = New ItemCatalog.StoreItem(CInt(order.ReceiveStore_No), CInt(order.Transfer_To_SubTeam), sIdentifier)
                orderItem = order.AddOrderItem(storeitem)
                orderItem.QuantityOrdered = dQuantityReceived
                If orderItem.IsSoldByWeight Then
                    orderItem.Package_Desc1 = 1
                    orderItem.Package_Desc2 = lPackSize
                Else
                    orderItem.Package_Desc1 = lPackSize
                    orderItem.Package_Desc2 = storeitem.Package_Desc2
                End If
                orderItem.AddDB(order, 0) ', lPackSize)
            End If
        End If

        logger.InfoFormat("CatchWeightRequired: {0}", orderItem.CatchWeightRequired)
        'If this is a distribution and the item requires catchweight and we don't have it yet, delay receiving by recording the cases received in the invoice quantity (shipped).  Catchweight processing will use this value to complete receiving.
        If order.OrderType_ID = enuOrderType.Distribution AndAlso orderItem.CatchWeightRequired AndAlso orderItem.Total_Weight = 0 Then
            orderItem.QuantityShipped = dQuantityReceived
            orderItem.UpdateQuantityShipped()
        Else
            Me.ReceiveOrderItem(lOrderHeader_ID, sIdentifier, dQuantityReceived, lPackSize, dCatchWeight, dtDate, blnIsFlow)
        End If
    End Sub
    Public Sub AdjustInventory(ByVal dtDate As DateTime, ByVal lOrderHeaderID As Long, ByVal sIdentifier As String, ByVal dQuantity As Decimal, ByVal lPackSize As Long, ByVal dWeight As Decimal, ByVal sReason As String, ByVal lReceiptID As Long, ByVal lVendorID As Long, ByVal lDocumentNo As Long)

        logger.InfoFormat("* AdjustInventory() Reason: {0} Identifier: {1}", sReason, sIdentifier)
        Dim iInvAdjCode_Id As Integer

        iInvAdjCode_Id = GetInventoryAdjustmentIDFromCode(sReason)

        If iInvAdjCode_Id = -99 Then
            logger.InfoFormat("   !! Adjustment Reason Code was not found in InventoryAdjustmentCode table: [{0}] ", sReason)
            logger.InfoFormat("   !! This record has not been processed.")

            Throw New System.Exception(String.Format("Invalid Reason Code: {0}", sReason))

        Else


            Select Case sReason
                'Case "CR" 'Customer Return (positive from EXE)
                '    'logger.InfoFormat(vbTab & "ReturnOrderItem(): {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", lOrderHeaderID, sIdentifier, Math.Abs(dQuantity), lPackSize, Math.Abs(dWeight), CreditReason.CustomerReturn, lVendorID, dtDate, sReason)
                '    'TODO:This errors out and needs to be fixed
                '    'Me.ReturnOrderItem(lOrderHeaderID, sIdentifier, Math.Abs(dQuantity), lPackSize, Math.Abs(dWeight), CreditReason.CustomerReturn, lVendorID, dtDate, sReason)
                '    '--Remove from the warehouse inventory
                '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Customer Return", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                'Case "VR" ' Vendor Return (negative from EXE)
                '    logger.InfoFormat(vbTab & "ReturnOrderItem('VR Adjustment'): {0} {1} {2} {3} {4} {5} {6} {7} orignal {8} {9} ", sIdentifier, enuItemAdjustment.ManualAdjustment, "Vendor Return", dtDate, -1 * Math.Abs(dQuantity), dWeight, 0, lPackSize, dQuantity, dWeight)
                '    'TODO:This errors out and needs to be fixed
                '    '--Me.ReturnOrderItem(lOrderHeaderID, sIdentifier, Math.Abs(dQuantity), lPackSize, Math.Abs(dWeight), CreditReason.CustomerReturn, lVendorID, dtDate, sReason)
                '    '--Remove from the warehouse inventory
                'Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Vendor Return", dtDate, -1 * Math.Abs(dQuantity), dWeight, 0, lPackSize)
                Case "RC" 'Receiving Correction
                    logger.InfoFormat(vbTab & "ReceivingCorrection(): {0} {1} {2} {3} {4} {5}", lOrderHeaderID, sIdentifier, dQuantity, lPackSize, dWeight, dtDate)
                    Me.ReceivingCorrection(lOrderHeaderID, sIdentifier, dQuantity, lPackSize, dWeight, dtDate)

                    '
                    'Case "PC", "PI" 'Physical Count or Physical Inventory
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.CycleCount, "Physical Count or Inventory", dtDate, dQuantity, dWeight, 0, lPackSize)
                    'Case "RA" 'Recoup Adjustment
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Recoup", dtDate, dQuantity, dWeight, 0, lPackSize)
                    'Case "CC" 'Cycle Count
                    '    logger.InfoFormat(vbTab & "AddItemHistory(): {0} {1} {2} {3} {4} {5} {6}", sIdentifier, enuItemAdjustment.ManualAdjustment, "Cycle Count", dtDate, dQuantity, dWeight, 0, lPackSize)
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Cycle Count", dtDate, dQuantity, dWeight, 0, lPackSize)

                    ' TFS 7988 BR - added CM 
                Case "MK", "CM" 'Selector Markout
                    logger.InfoFormat(vbTab & "AddItemHistory(): {0} {1} {2} {3} {4} {5} {6}", sIdentifier, enuItemAdjustment.Waste, "Selector Markout", dtDate, dQuantity, dWeight, 0, lPackSize)
                    'Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Selector Markout", dtDate, dQuantity, dWeight, 0, lPackSize)
                    '--Remove from the shipping PO
                    Me.ReceivingCorrection(lOrderHeaderID, sIdentifier, -1 * dQuantity, lPackSize, -1 * dWeight, dtDate)
                    '--Remove from the warehouse inventory
                    'Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Markout", dtDate, -1 * dQuantity, -1 * dWeight, 0, lPackSize)

                    'Case "BP" 'Bulk Packaging
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "Bulk Packaging", dtDate, dQuantity, dWeight, 0, lPackSize)
                    'Case "DA" 'Damage Adjustment
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Damaged in Warehouse", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    '    '   added for  bug 7830
                    'Case "RE" 'Recall
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Recall", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "SM" 'Samples
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Samples", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "SS" 'Supplies
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Supplies", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "FB" 'Food Bank
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Food Bank", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "DO" 'Donation
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Donation", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "SP" 'Perishable Spoilage
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.Waste, "Perishable Spoilage", dtDate, Math.Abs(dQuantity), Math.Abs(dWeight), 0, lPackSize)
                    'Case "ZH" 'Hold
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "ZH - Hold", dtDate, dQuantity, dWeight, 0, lPackSize)
                    'Case "ZA" 'Available
                    '    Me.AddItemHistory(sIdentifier, enuItemAdjustment.ManualAdjustment, "ZA - Available", dtDate, dQuantity, dWeight, 0, lPackSize)

                    'Case Else
                    '    Throw New System.Exception("Unsupported Reason Code: " & sReason)
            End Select

            Me.AddItemHistory(sIdentifier, iInvAdjCode_Id, dtDate, dQuantity, dWeight, 0, lPackSize)
        End If
    End Sub

    Public Sub AddCycleCountItem(ByVal InDate As DateTime, ByVal sIdentifier As String, ByVal Quantity As Decimal, ByVal PackSize As Decimal, ByVal Weight As Decimal)
        Dim item As New ItemCatalog.StoreItem(Me.Store_No, sIdentifier)
        Dim cycleCount As ItemCatalog.CycleCount = Me.GetCycleCount(item.SubTeam_No)
        If cycleCount Is Nothing Then
            cycleCount = ItemCatalog.CycleCount.GetCycleCount(Me.Store_No, item.SubTeam_No)
            If cycleCount Is Nothing Then Throw New ItemCatalog.Exception.CycleCount.NoMasterException
            If Me.m_cycle_counts Is Nothing Then Me.m_cycle_counts = New ArrayList
            Me.m_cycle_counts.Add(cycleCount)
        End If
        cycleCount.AddItemInternal(InDate, 0, item, Quantity * PackSize, Weight, PackSize, True)
    End Sub

    Private Function GetCycleCount(ByVal lSubTeam_No As Long) As ItemCatalog.CycleCount
        If Not (m_cycle_counts Is Nothing) Then
            m_cycle_counts.Sort(New CycleCountSort(enuCycleCountSortOrder.SubTeam_No))
            Dim i As Integer = m_cycle_counts.BinarySearch(New CycleCount(Me.Store_No, lSubTeam_No), New CycleCountGetSubTeam_No)
            If i >= 0 Then Return CType(m_cycle_counts(i), CycleCount)
        End If
    End Function

    Private Sub ReturnOrderItem(ByVal lOrderHeaderID As Long, ByVal sIdentifier As String, ByVal dQuantity As Decimal, ByVal lPackSize As Long, ByVal dWeight As Decimal, ByVal lCreditReason_ID As CreditReason, ByVal lVendorID As Long, ByVal dtDate As DateTime, ByVal sReason As String)
        Dim item As New ItemCatalog.StoreItem(Me.Store_No, sIdentifier) 'Get the item info first
        Dim order As ItemCatalog.Order

        Select Case sReason
            Case "CR" 'Customer Return
                order = GetOrder(Me.VendorID, lVendorID, item.SubTeam_No, item.SubTeam_No, True, Me.m_return_orders)
                If order Is Nothing Then
                    If lOrderHeaderID > 0 Then
                        order = New ItemCatalog.Order
                        With order
                            .Vendor_ID = Me.VendorID
                            .PurchaseLocation_ID = lVendorID
                            .ReceiveLocation_ID = lVendorID
                            .Transfer_SubTeam = item.SubTeam_No
                            .Transfer_To_SubTeam = item.SubTeam_No
                            .Fax = False
                            .Return_Order = False
                            .Expected_Date = Now
                            .CreatedByID = 0
                            .DBAdd(enuProductType.Product, lOrderHeaderID)
                        End With

                        Me.m_return_orders.Add(order)
                    Else
                        Throw New System.Exception("Original Order Number is required for all Customer Returns")
                    End If
                End If

            Case "VR" 'Vendor Return
                order = GetOrder(lVendorID, Me.VendorID, 0, 0, True, Me.m_return_orders)
                If order Is Nothing Then
                    With order
                        .Vendor_ID = lVendorID
                        .PurchaseLocation_ID = Me.VendorID
                        .ReceiveLocation_ID = Me.VendorID
                        .Transfer_SubTeam = 0
                        .Transfer_To_SubTeam = 0
                        .Fax = False
                        .Return_Order = True
                        .Expected_Date = Now
                        .CreatedByID = 0
                        .DBAdd(enuProductType.Product, lOrderHeaderID)
                    End With
                    Me.m_return_orders.Add(order)
                End If
            Case Else
                Throw New System.Exception(sReason & " not supported")
        End Select

        'Add the return order item
        Dim newReturnOrderItem As New OrderItem
        newReturnOrderItem = order.AddOrderItem(item)
        newReturnOrderItem.QuantityOrdered = dQuantity
        newReturnOrderItem.AddDB(order, lCreditReason_ID, lPackSize)
    End Sub

    Private Function GetOrder(ByVal lVendorID As Long, ByVal lReceiveLocationID As Long, ByVal lTransferSubTeam As Long, ByVal lTransferToSubTeam As Long, ByVal bReturnOrder As Boolean, ByRef orderList As ArrayList) As ItemCatalog.Order
        Dim targetOrder As New ItemCatalog.Order
        With targetOrder
            .Vendor_ID = lVendorID
            .ReceiveLocation_ID = lReceiveLocationID
            .Transfer_SubTeam = lTransferSubTeam
            .Transfer_To_SubTeam = lTransferToSubTeam
            .Return_Order = bReturnOrder
        End With
        If orderList Is Nothing Then orderList = New ArrayList
        orderList.Sort(New ItemCatalog.OrderSort(enuOrderSortOrder.FKs))
        Dim i As Integer = orderList.BinarySearch(targetOrder, New ItemCatalog.OrderGetFKs)
        If i >= 0 Then Return CType(orderList(i), ItemCatalog.Order)
    End Function
    Public Function GetOrder(ByVal lOrderHeader_ID As Long) As ItemCatalog.Order

        Dim stubOrder As ItemCatalog.Order
        Dim i As Integer

        If Not (Me.m_orders Is Nothing) Then
            Me.m_orders.Sort(New ItemCatalog.OrderSort(enuOrderSortOrder.ID))
            'FV: Research a better way of searching the array.  If possible it would be nice to have OrderHeader_ID a readonly property
            stubOrder = New ItemCatalog.Order
            stubOrder.OrderHeader_ID = lOrderHeader_ID
            i = Me.m_orders.BinarySearch(stubOrder, New ItemCatalog.OrderGetID)
            stubOrder = Nothing
            If i >= 0 Then
                Return CType(Me.m_orders(i), ItemCatalog.Order)
                Exit Function
            End If
        End If

        'If we haven't exited yet,
        'Go get it from the database
        Dim order As New ItemCatalog.Order(lOrderHeader_ID)
        If Me.m_orders Is Nothing Then Me.m_orders = New ArrayList
        Me.m_orders.Add(order)
        Return order

    End Function

    Public Function GetOrder(ByVal lOrderHeader_ID As Long, ByRef orderList As ArrayList) As ItemCatalog.Order
        If orderList Is Nothing Then orderList = New ArrayList

        orderList.Sort(New ItemCatalog.OrderSort(enuOrderSortOrder.ID))
        Dim stubOrder As New ItemCatalog.Order
        'FV: Research a better way of searching the array.  If possible it would be nice to have OrderHeader_ID a readonly property
        stubOrder.OrderHeader_ID = lOrderHeader_ID
        Dim i As Integer = orderList.BinarySearch(stubOrder, New ItemCatalog.OrderGetID)
        stubOrder = Nothing
        If i >= 0 Then
            Return CType(orderList(i), ItemCatalog.Order)
        Else
            'Go get it from the database
            Dim order As New ItemCatalog.Order(lOrderHeader_ID)
            orderList.Add(order)
            Return order
        End If
    End Function

    Public Sub ReceiveCloseReturnOrders()
        If Not (m_return_orders Is Nothing) Then
            Dim i As Long
            Dim order As ItemCatalog.Order
            For i = 0 To m_return_orders.Count - 1
                logger.InfoFormat("ReceiveCloseReturnOrders: {0}", CType(m_return_orders(i), ItemCatalog.Order).OrderHeader_ID.ToString())
                order = CType(m_return_orders(i), ItemCatalog.Order)
                order.ReceiveAllItemsAsOrdered(0)
                order.Close()
            Next
        End If
    End Sub

    Public Sub CloseShippingOrders()
        If Not (m_shipping_orders Is Nothing) Then
            Dim i As Long
            For i = 0 To m_shipping_orders.Count - 1
                logger.InfoFormat("CloseShippingOrder: {0}", CType(m_shipping_orders(i), ItemCatalog.Order).OrderHeader_ID.ToString())
                CType(m_shipping_orders(i), ItemCatalog.Order).Close()
            Next
        End If
    End Sub

    Private Sub ClearOrders()
        Try
            ClearReturnOrders()
            ClearReceivingOrders()
            ClearShippingOrders()
        Catch ex As System.Exception
            'Don't care - just clean-up
        End Try
    End Sub

    Public Sub ClearReturnOrders()
        If Not (Me.m_return_orders Is Nothing) Then
            m_return_orders.Clear()
            m_return_orders.TrimToSize()
            m_return_orders = Nothing
        End If
    End Sub

    Public Sub ClearReceivingOrders()
        If Not (Me.m_receiving_orders Is Nothing) Then
            m_receiving_orders.Clear()
            m_receiving_orders.TrimToSize()
            m_receiving_orders = Nothing
        End If
    End Sub

    Public Sub ClearShippingOrders()
        If Not (Me.m_shipping_orders Is Nothing) Then
            m_shipping_orders.Clear()
            m_shipping_orders.TrimToSize()
            m_shipping_orders = Nothing
        End If
    End Sub

    Public Sub ReceiveOrderItem(ByVal lOrderHeader_ID As Long, ByVal sIdentifier As String, ByVal dQuantityReceived As Decimal, ByVal lPackSize As Long, ByVal dCatchWeight As Decimal, ByVal dtDate As DateTime, ByVal blnIsFlow As Boolean)
        Dim order As ItemCatalog.Order
        Try
            order = GetOrder(lOrderHeader_ID, Me.m_shipping_orders)
            If order Is Nothing Then
                Throw New System.Exception("OrderHeader_ID " & lOrderHeader_ID.ToString & " not found")
            Else
                Dim orderItem As ItemCatalog.OrderItem = order.GetOrderItem(sIdentifier, lPackSize)
                If orderItem Is Nothing Then
                    Dim item As New ItemCatalog.StoreItem(CInt(order.ReceiveStore_No), CInt(order.Transfer_To_SubTeam), sIdentifier)
                    orderItem = order.AddOrderItem(item)
                    orderItem.QuantityOrdered = dQuantityReceived
                    orderItem.Package_Desc1 = lPackSize
                    orderItem.AddDB(order, 0, lPackSize)
                End If
                If dCatchWeight = 0 And orderItem.IsReceivedWeightRequired Then
                    dCatchWeight = ItemCatalog.Item.ConvertCost(dQuantityReceived, orderItem.Package_Unit_ID, orderItem.QuantityUnit, lPackSize, orderItem.Package_Desc2, orderItem.Package_Unit_ID, 0, 0)
                End If
                logger.InfoFormat("ReceiveOrderItem() [{0}]", sIdentifier)
                orderItem.Receive(dQuantityReceived, dCatchWeight, dtDate, blnIsFlow, lPackSize)
            End If
        Catch ex As System.Exception
            Dim sEx2Msg As String = String.Empty
            Try
                If Not (order Is Nothing) Then order.DeleteOrderReceiving()
            Catch ex2 As System.Exception
                sEx2Msg = "DeleteOrderReceiving failed: " & ex2.ToString
            End Try
            If sEx2Msg.Length > 0 Then
                Throw New System.Exception(sEx2Msg, ex)
            Else
                Throw ex
            End If
        End Try
    End Sub
    ''' <summary>
    ''' This class is used to provide the criteria needed retrieve an order item from an order class.
    ''' </summary>
    ''' <remarks>
    ''' The main purpose of this class is to provide a way to call the correct Order.GetOrderItem overload.
    ''' </remarks>
    Friend Class WarehouseOrderItemGetCriteria
        Private mIdentifier As String
        Private mExePackSize As Integer
        Friend Sub New(ByVal Identifier As String, ByVal ExePackSize As Integer)
            mIdentifier = Identifier
            mExePackSize = ExePackSize
        End Sub
        Friend ReadOnly Property Identifier() As String
            Get
                Return mIdentifier
            End Get
        End Property
        Friend ReadOnly Property ExePackSize() As Integer
            Get
                Return mExePackSize
            End Get
        End Property
    End Class

    Public Sub ReceivePOItem(ByVal lOrderHeader_ID As Long, ByVal sIdentifier As String, ByVal dQuantityReceived As Decimal, ByVal lPackSize As Long, ByVal dCatchWeight As Decimal, ByVal dtDate As DateTime)
        Dim order As ItemCatalog.Order
        Try
            order = GetOrder(lOrderHeader_ID, Me.m_receiving_orders)
            If order Is Nothing Then
                Throw New System.Exception("OrderHeader_ID " & lOrderHeader_ID.ToString & " not found")
            Else
                Dim orderItem As ItemCatalog.OrderItem = order.GetOrderItem(sIdentifier, lPackSize)
                If orderItem Is Nothing Then
                    Dim item As New ItemCatalog.StoreItem(CInt(order.ReceiveStore_No), CInt(order.Transfer_To_SubTeam), sIdentifier)
                    orderItem = order.AddOrderItem(item)
                    orderItem.QuantityOrdered = dQuantityReceived
                    orderItem.Package_Desc1 = lPackSize
                    orderItem.AddDB(order, 0, lPackSize)
                End If
                If dCatchWeight = 0 And orderItem.IsReceivedWeightRequired Then
                    dCatchWeight = ItemCatalog.Item.ConvertCost(dQuantityReceived, orderItem.Package_Unit_ID, orderItem.QuantityUnit, lPackSize, orderItem.Package_Desc2, orderItem.Package_Unit_ID, 0, 0)
                End If
                orderItem.Receive(dQuantityReceived, dCatchWeight, dtDate, False, lPackSize)
            End If
        Catch ex As System.Exception
            Dim sEx2Msg As String = String.Empty
            Try
                If Not (order Is Nothing) Then order.DeleteOrderReceiving()
            Catch ex2 As System.Exception
                sEx2Msg = "DeleteOrderReceiving failed: " & ex2.ToString
            End Try
            If sEx2Msg.Length > 0 Then
                Throw New System.Exception(sEx2Msg, ex)
            Else
                Throw ex
            End If
        End Try
    End Sub

    Public Sub ReceivingCorrection(ByVal lOrderHeader_ID As Long, ByVal sIdentifier As String, ByVal dQuantityReceived As Decimal, ByVal lPackSize As Long, ByVal dCatchWeight As Decimal, ByVal dtDate As DateTime)
        Dim order As New ItemCatalog.Order(lOrderHeader_ID)
        If order Is Nothing Then
            Throw New System.Exception("OrderHeader_ID " & lOrderHeader_ID.ToString & " not found")
        Else
            Dim orderItem As ItemCatalog.OrderItem
            orderItem = order.GetOrderItem(sIdentifier, lPackSize)

            If orderItem Is Nothing Then
                Dim item As New ItemCatalog.StoreItem(CInt(order.ReceiveStore_No), CInt(order.Transfer_To_SubTeam), sIdentifier)
                orderItem = order.AddOrderItem(item)
                orderItem.QuantityOrdered = dQuantityReceived
                orderItem.Package_Desc1 = lPackSize
                orderItem.AddDB(order, 0, lPackSize)
            End If

            'If (orderItem.QuantityReceived + dQuantityReceived) >= 0 Then
            If (orderItem.QuantityReceived <> Nothing) Then
                If dCatchWeight = 0 And orderItem.IsReceivedWeightRequired Then
                    dCatchWeight = ItemCatalog.Item.ConvertCost(dQuantityReceived, orderItem.Package_Unit_ID, orderItem.QuantityUnit, lPackSize, orderItem.Package_Desc2, orderItem.Package_Unit_ID, 0, 0)
                End If
                orderItem.Receive(dQuantityReceived, dCatchWeight, dtDate, True, lPackSize)
            Else
                Throw New ItemCatalog.Exception.Warehouse.NegativeReceivingCorrectionNotReceivedException

            End If

        End If
    End Sub

    Public Sub ResetWarehouseSent(ByVal lOrderHeader_ID As Long)
        Dim order As New ItemCatalog.Order
        order.ResetWarehouseSent(lOrderHeader_ID)
    End Sub

    Public Shared Function GetWarehouses() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetWarehouses"
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Warehouse(dr.GetInt32(0), dr.GetString(1), dr.GetInt32(2), dr.GetInt16(3)))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Sub UpdateAverageCost(ByVal iWarehouseStore_No As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateAverageCost"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.ParameterName = "@InStore_No"
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            cmd.Parameters.Add(prm)
            cmd.Parameters(0).Value = iWarehouseStore_No
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

    Public Class WarehouseItemChange
        Inherits Item
        Dim m_warehouse_item_change_id As Long
        Dim m_change_type As String

        Public ReadOnly Property WarehouseItemChangeID() As Long
            Get
                Return m_warehouse_item_change_id
            End Get
        End Property

        Public Property ChangeType() As String
            Get
                Return m_change_type
            End Get
            Set(ByVal Value As String)
                m_change_type = Value
            End Set
        End Property

        Public Sub New(ByVal lWarehouseItemChangeID As Long, ByVal sChangeType As String, ByVal lItem_Key As Long, ByVal sIdentifier As String, ByVal sItem_Description As String, ByVal sPOSDescription As String, ByVal lSubTeam_No As Long, ByVal sSubTeamName As String, ByVal dPackageDesc1 As Decimal, ByVal dPackageDesc2 As Decimal, ByVal bNotAvailable As Boolean, ByVal bCOOL As Boolean, ByVal bBIO As Boolean, ByVal bCatchWeightRequired As Boolean, Optional ByVal sPackageUnitAbbr As String = "")
            MyBase.New(lItem_Key, sIdentifier, sItem_Description, sPOSDescription, lSubTeam_No, sSubTeamName, dPackageDesc1, dPackageDesc2, bNotAvailable, bCOOL, bBIO, bCatchWeightRequired, sPackageUnitAbbr)
            m_warehouse_item_change_id = lWarehouseItemChangeID
            m_change_type = sChangeType
        End Sub
    End Class

    Public Class WarehouseVendorChange
        Inherits Vendor
        Dim m_warehouse_vendor_change_id As Long
        Dim m_change_type As String

        Public ReadOnly Property WarehouseVendorChangeID() As Long
            Get
                Return m_warehouse_vendor_change_id
            End Get
        End Property

        Public Property ChangeType() As String
            Get
                Return m_change_type
            End Get
            Set(ByVal Value As String)
                m_change_type = Value
            End Set
        End Property

        Public Sub New(ByVal lWarehouseVendorChangeID As Long, ByVal sChangeType As String, ByVal lVendorID As Long, ByVal Key As String, ByVal sCompanyName As String, ByVal sAddressLine1 As String, ByVal sAddressLine2 As String, ByVal sCity As String, ByVal sState As String, ByVal sZipCode As String, ByVal sCountry As String, ByVal sPhone As String)
            MyBase.New(lVendorID, Key, sCompanyName, sAddressLine1, sAddressLine2, sCity, sState, sZipCode, sCountry, sPhone)
            m_warehouse_vendor_change_id = lWarehouseVendorChangeID
            m_change_type = sChangeType
        End Sub
    End Class

End Class

Public Class WarehouseSort
    Implements IComparer
    Dim CompType As enuWarehouseSortOrder

    Public Sub New(ByVal xCompType As enuWarehouseSortOrder)
        CompType = xCompType
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuWarehouseSortOrder.DistributionCenter_Warehouse
                Dim iDistCenterComp As Integer = CType(x, Warehouse).DistributionCenter.CompareTo(CType(y, Warehouse).DistributionCenter)
                Dim iWarehouseComp As Integer = CType(x, Warehouse).Warehouse.CompareTo(CType(y, Warehouse).Warehouse)
                If iDistCenterComp = 0 And iWarehouseComp = 0 Then
                    Compare = 0
                Else
                    If (iDistCenterComp < 0) Or (iWarehouseComp < 0) Then
                        Compare = -1
                    Else
                        Compare = 1
                    End If
                End If
        End Select
    End Function
End Class

Public Class WarehouseGet
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim iDistCenterComp As Integer = CType(x, Warehouse).DistributionCenter.CompareTo(CType(y, Warehouse).DistributionCenter)
        Dim iWarehouseComp As Integer = CType(x, Warehouse).Warehouse.CompareTo(CType(y, Warehouse).Warehouse)
        If iDistCenterComp = 0 And iWarehouseComp = 0 Then
            Compare = 0
        Else
            If (iDistCenterComp < 0) Or (iWarehouseComp < 0) Then
                Compare = -1
            Else
                Compare = 1
            End If
        End If
    End Function
End Class
