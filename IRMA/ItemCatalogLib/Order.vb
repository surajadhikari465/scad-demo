Imports log4net
Imports System.Collections
Imports System.Text
Imports System.Text.RegularExpressions
Imports WFM.DataAccess.Setup
Imports System.Reflection


Public Enum enuOrderType
    Purchase = 1
    Distribution = 2
    Transfer = 3
    Flow = 4
End Enum
Public Enum enuProductType
    Product = 1
    Packaging = 2
    Supplies = 3
End Enum
Public Enum enuOrderSortOrder
    ID = 0 'OrderHeader_ID
    FKs = 1 'Foreign keys
End Enum
Public Enum enuOrderItemSortOrder
    ID = 0
    Item_Key = 1
    Identifier = 2
    IdentifierPackage_Desc1 = 3
End Enum
Public Enum CreditReason
    Shortage = 1
    Quality = 2
    Misship = 3
    Damaged = 4
    Cost = 5
    CustomerReturn = 6
    OutOfStock = 7
    ShortDated = 8
    QuarterlyCredit = 9
    MagazineCovers = 10
    Recalled = 11
    Mislabeled = 12
End Enum

Public Structure UnsentVendorDiscrepancy
    Dim VendorID As Integer
    Dim VendorEmail As String
End Structure

Public Class Order
    Dim m_order_header_id As Long
    Dim m_vendor_id As Long
    Dim m_order_type_id As Long
    Dim m_product_type_id As Long
    Dim m_purchase_location_id As Long
    Dim m_receive_location_id As Long
    Dim m_store_no As Long
    Dim m_ship_to_store_no As Long
    Dim m_created_by As Long
    Dim m_created_by_user As User
    Dim m_order_date As DateTime
    Dim m_close_date As DateTime
    Dim m_original_close_date As DateTime
    Dim m_sent As Boolean
    Dim m_fax As Boolean
    Dim m_expected_date As DateTime
    Dim m_sent_date As DateTime
    Dim m_transfer_subteam As Long
    Dim m_transfer_to_subteam As Long
    Dim m_transfer_to_subteam_name As String
    Dim m_return_order As Boolean
    Dim m_invoice_date As DateTime
    Dim m_invoice_no As String
    Dim m_invoice_freight As Decimal
    Dim m_invoice_cost As Decimal
    Dim m_uploaded_date As DateTime
    Dim m_buyer_name As String
    Dim m_buyer_email_address As String
    Dim m_tl_name As String
    Dim m_tl_email_address As String
    Dim m_order_allowance_discount_amount As String
    Dim m_order_notes As String
    Dim m_ps_vendor_id As String
    Dim m_ack_expected_delivery_date As DateTime
    Dim m_isa_receiver_qualifier As String
    Dim m_isa_receiver_id As String
    Dim m_store_phone As String
    Dim m_vendor_duns_plus_four As String
    Dim m_drop_shipment As Boolean
    Dim m_quantity_discount As Decimal
    Dim m_discount_type As Integer
    Dim _EXEWarehouse As Integer
    Dim _SubteamAbbreviation As String
    Dim _DistCenter As Integer
    Dim _Buyer As String
    Dim m_email As Boolean
    Dim m_sent_to_email_date As DateTime

    Dim m_order_items As ArrayList
    Dim m_VendorDiscrepancyList As ArrayList

    Public Sub New()
    End Sub
    Public Sub New(ByVal lOrderHeader_ID As Long)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim drTLEmail As System.Data.SqlClient.SqlDataReader


        Try
            Dim prm As System.Data.SqlClient.SqlParameter
            m_order_header_id = lOrderHeader_ID
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetOrderInfo"

            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(lOrderHeader_ID)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                With dr
                    If Not IsDBNull(!CloseDate) Then Me.m_close_date = CType(!CloseDate, DateTime)
                    Me.m_created_by = CType(!CreatedBy, Long)
                    If Not IsDBNull(!Expected_Date) Then Me.m_expected_date = CType(!Expected_Date, DateTime)
                    Me.m_fax = CType(!Fax_Order, Boolean)
                    Me.m_email = CType(!Email_Order, Boolean)
                    If Not IsDBNull(!InvoiceDate) Then Me.m_invoice_date = CType(!InvoiceDate, DateTime)
                    If Not IsDBNull(!InvoiceNumber) Then m_invoice_no = !InvoiceNumber
                    If Not IsDBNull(!OrderDate) Then Me.m_order_date = CType(!OrderDate, DateTime)
                    If Not IsDBNull(!OriginalCloseDate) Then Me.m_original_close_date = CType(!OriginalCloseDate, DateTime)
                    Me.m_purchase_location_id = CType(!PurchaseLocation_ID, Long)
                    Me.m_receive_location_id = CType(!ReceiveLocation_ID, Long)
                    Me.m_store_no = !Store_No
                    Me.m_return_order = CType(!Return_Order, Boolean)
                    m_sent = CType(!Sent, Boolean)
                    If Not IsDBNull(!SentDate) Then m_sent_date = CType(!SentDate, DateTime)
                    If Not IsDBNull(!Transfer_SubTeam) Then Me.m_transfer_subteam = CType(!Transfer_SubTeam, Long)
                    If Not IsDBNull(!Transfer_To_SubTeam) Then Me.m_transfer_to_subteam = CType(!Transfer_To_SubTeam, Long)
                    If Not IsDBNull(!Transfer_To_SubTeamName) Then Me.m_transfer_to_subteam_name = !Transfer_To_SubTeamName
                    If Not IsDBNull(!UploadedDate) Then Me.m_uploaded_date = CType(!UploadedDate, DateTime)
                    Me.m_vendor_id = CType(!Vendor_ID, Long)
                    Me.m_order_type_id = CType(!OrderType_ID, Integer)
                    Me.m_product_type_id = CType(!ProductType_ID, Integer)
                    Me.m_buyer_name = !BuyerName
                    Me.m_buyer_email_address = !BuyerEmail
                    Me.m_order_notes = !Notes
                    Me.m_ship_to_store_no = !ShipToStoreNo
                    If Not IsDBNull(!PSVendorid) Then Me.m_ps_vendor_id = !PSVendorID
                    Me.m_order_allowance_discount_amount = !AllowanceDiscountAmount
                    If Not IsDBNull(!ISAReceiverQualifier) Then m_isa_receiver_qualifier = !ISAReceiverQualifier
                    If Not IsDBNull(!ISAReceiverID) Then m_isa_receiver_id = !ISAReceiverID
                    If Not IsDBNull(!Store_Phone) Then m_store_phone = !Store_Phone
                    If Not IsDBNull(!DUNSPlusFour) Then m_vendor_duns_plus_four = !DUNSPlusFour
                    If Not IsDBNull(!IsDropShipment) Then m_drop_shipment = !IsDropShipment
                    If Not IsDBNull(!QuantityDiscount) Then m_quantity_discount = !QuantityDiscount
                    If Not IsDBNull(!DiscountType) Then m_discount_type = !DiscountType
                End With
                dr.Close()
            Else
                Throw New ItemCatalog.Exception.Order.NotFoundException(lOrderHeader_ID)
            End If

            'Get the order items
            cmd.CommandText = "GetOrderItems"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(lOrderHeader_ID)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                Me.m_order_items = New ArrayList
                While dr.Read
                    Dim orderItem As New ItemCatalog.OrderItem
                    orderItem.OrderItem_ID = dr!OrderItem_ID
                    If Not IsDBNull(dr!OrderUnit) Then orderItem.OrderUnit = dr!OrderUnit
                    orderItem.Item_Key = dr!Item_Key
                    orderItem.Identifier = dr!Identifier
                    orderItem.Package_Desc1 = dr!Package_Desc1
                    orderItem.Package_Desc2 = dr!Package_Desc2
                    orderItem.Package_Unit_ID = dr!Package_Unit_ID
                    orderItem.QuantityUnit = dr!QuantityUnit
                    orderItem.QuantityOrdered = dr!QuantityOrdered
                    orderItem.QuantityReceived = IIf(IsDBNull(dr!QuantityReceived), 0, dr!QuantityReceived)
                    orderItem.Total_Weight = dr!Total_Weight
                    orderItem.IsReceivedWeightRequired = dr!IsReceivedWeightRequired
                    If Not IsDBNull(dr!VendorItemID) Then orderItem.VendorItem_ID = dr!VendorItemID
                    orderItem.ItemDescription = dr!Item_Description
                    orderItem.LineItemCost = dr!LineItemCost
                    orderItem.LineItemNumber = dr!LineNumber
                    orderItem.LineItemFreight = dr!LineItemFreight
                    orderItem.ItemAllowanceDiscountAmount = dr!ItemAllowanceDiscountAmount
                    orderItem.UnitOfMeasureCost = dr!UOMUnitCost
                    If Not IsDBNull(dr!ItemUnit) Then orderItem.ItemUnit = dr!ItemUnit
                    orderItem.QuantityDiscount = dr!QuantityDiscount
                    orderItem.DiscountType = dr!DiscountType
                    If Not IsDBNull(dr!QuantityShipped) Then orderItem.QuantityShipped = dr!QuantityShipped
                    If Not IsDBNull(dr!CatchweightRequired) Then orderItem.CatchWeightRequired = dr!CatchWeightRequired
                    m_order_items.Add(orderItem)
                End While
            End If
            dr.Close()

            ' Get the Team Leader's email address for this order
            'cmd.CommandText = "GetOrderTLEmail"
            'cmd.Parameters.Clear()

            'cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(lOrderHeader_ID)))

            'drTLEmail = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            'If drTLEmail.HasRows Then
            '    drTLEmail.Read()
            '    If Not IsDBNull(drTLEmail!TLEmail) Then m_tl_email_address = drTLEmail!TLEmail
            '    If Not IsDBNull(drTLEmail!TLName) Then m_tl_name = drTLEmail!TLName
            'End If
            'drTLEmail.Close()

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            'ItemCatalog.DataAccess.ReleaseDataObject(drTLEmail, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddOrderItem(ByVal orderItem As OrderItem)
        m_order_items.Add(orderItem)
    End Sub
    Public Sub Close()
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateOrderClosed"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@OrderHeader_ID"
            prm.Value = Me.OrderHeader_ID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@User_ID"
            prm.Value = 0
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub DBAddOrderItems(Optional ByVal lCreditReason_ID As Long = 0, Optional ByVal dPackSize As Decimal = 0)
        Dim OrderItem As OrderItem
        For Each OrderItem In m_order_items
            OrderItem.AddDB(Me, lCreditReason_ID, dPackSize)
        Next
    End Sub
    Public Sub DeleteOrderReceiving()
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "DeleteOrderReceiving"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@OrderHeader_ID"
            prm.Value = Me.OrderHeader_ID
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub ReceiveAllItemsAsOrdered(ByVal lUser_ID As Long)
        If Not (Me.m_order_items Is Nothing) Then
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "ReceiveOrderItem2"
                Dim prm As New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@OrderItem_ID"
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.DateTime
                prm.ParameterName = "@DateReceived"
                prm.Value = Now
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@User_ID"
                prm.Value = lUser_ID
                cmd.Parameters.Add(prm)

                Dim i As Long
                For i = 0 To Me.m_order_items.Count - 1
                    cmd.Parameters("@OrderItem_ID").Value = CType(Me.m_order_items(i), ItemCatalog.OrderItem).OrderItem_ID
                    If cmd.Connection Is Nothing Then
                        ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
                    Else
                        cmd.ExecuteNonQuery()
                    End If
                Next
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        End If
    End Sub
    Public Sub ReceiveOrderItem(ByVal lItem_Key As Long, ByVal dQuantityReceived As Decimal, ByVal dWeight As Decimal, ByVal dtDate As DateTime, ByVal bCorrection As Boolean, Optional ByVal UserID As Long = 0)
        If Me.m_sent_date = Date.MinValue Then Throw New ItemCatalog.Exception.Order.NotSentException
        Dim orderitem As ItemCatalog.OrderItem = GetOrderItem(lItem_Key)
        orderitem.Receive(dQuantityReceived, dWeight, dtDate, bCorrection, , UserID)
    End Sub
    Public Sub ResetWarehouseSent(ByVal lOrderHeader_ID)

        Dim CancelOrder As Int16 = 1
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateOrderResetWarehouseSent"
            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(lOrderHeader_ID)))
            cmd.Parameters.Add(CreateParam("@CancelOrder", SqlDbType.Int, ParameterDirection.Input, CObj(CancelOrder)))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            m_order_header_id = lOrderHeader_ID
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub Send()
        Dim bDataChanged As Boolean
        Dim cmdSend As SqlClient.SqlCommand

        Try
            'Do any validation
            Me.Validate()
            'mark this order as sent
            cmdSend = New SqlClient.SqlCommand
            cmdSend.CommandText = "EXEC UpdateOrderSend " & Me.OrderHeader_ID
            cmdSend.CommandType = CommandType.Text
            DataAccess.ExecuteSqlCommand(cmdSend, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmdSend, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Function CreateEDI850(ByVal SuffixWithLineBreaks As Boolean) As String
        Dim sbString As New StringBuilder
        Dim intSegmentCount As Integer

        ' Converts a PO to an EDI850 format for transmission to a
        ' third-party integrator.  For details about EDI 850 formatting,
        ' see "WholeFoods 850 Rev 1.4.doc"

        '---------------------------------------------------------------------
        ' Insert transaction set header
        sbString.Append("ST*850*" & m_order_header_id & "~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)


        '---------------------------------------------------------------------
        ' Insert the order header

        If m_drop_shipment = True Then
            ' Create a drop shipment BEG segment
            sbString.Append("BEG*00*DS*" & m_order_header_id & "**" & (Format(m_order_date, "yyyyMMdd")) & "~")
        Else
            ' Create a stand-alone order BEG segment
            sbString.Append("BEG*00*SA*" & m_order_header_id & "**" & (Format(m_order_date, "yyyyMMdd")) & "~")
        End If
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

        sbString.Append("CUR*BY*USD~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

        If m_buyer_email_address.Trim().Length > 0 Then
            sbString.Append("PER*BD*" & m_buyer_name & "*EM*" & m_buyer_email_address & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_tl_email_address.Trim().Length > 0 Then
            sbString.Append("PER*PJ*" & m_tl_name & "*EM*" & m_tl_email_address & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_store_phone.Trim().Length > 0 Then
            sbString.Append("PER*IC**TE*" & m_store_phone & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If CDec(m_order_allowance_discount_amount) <> 0 Then
            sbString.Append("SAC*A*C312***" & m_order_allowance_discount_amount & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        sbString.Append("DTM*002*" & (Format(m_expected_date, "yyyyMMdd")).ToString() & "~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

        sbString.Append("N9*L1*Future Use~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

        If m_order_notes.Trim().Length > 0 Then
            sbString.Append("MTX**" & m_order_notes & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_ps_vendor_id.Trim().Length > 0 Then
            ' Add a "Vendor" identification line
            sbString.Append("N1*VN**92*" & m_ps_vendor_id & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_store_no > 0 Then
            ' Add a "Store Name" identification line
            sbString.Append("N1*SN**92*" & m_store_no & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_transfer_to_subteam > 0 Then
            ' Add a "Sub-team" identification line
            sbString.Append("N1*U4*" & m_transfer_to_subteam_name & "*92*" & m_transfer_to_subteam & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_ship_to_store_no > 0 Then
            ' Add a "Ship To" identification line
            sbString.Append("N1*ST**91*" & m_ship_to_store_no & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        If m_vendor_duns_plus_four.Trim().Length > 0 Then
            ' Add a "Ship From" identification line.  This was added at
            ' the request of Tree of Life.
            sbString.Append("N1*SF**9*" & m_vendor_duns_plus_four & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        End If

        ' Add a "Bill To" identification line (WFMs DUNS number).  This was added at
        ' the request of Tree of Life.
        sbString.Append("N1*BT**1*196175616~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)


        '---------------------------------------------------------------------
        ' Build order item entries from the arraylist

        Dim i As Integer
        Dim strVendorCodeIdentifier As String = String.Empty
        Dim strUPCCodeIdentifier As String = String.Empty
        Dim strCreditOrderIdentifier As String = String.Empty
        Dim OrderItemRecord As OrderItem
        'OrderItemRecord = CType(m_order_items(0), OrderItem)

        ' Loop through the order items array list and process its order items
        For i = 0 To m_order_items.Count - 1

            ' Save off an array list row
            OrderItemRecord = CType(m_order_items(i), OrderItem)

            ' Determine the value for the vendor code identifier
            strVendorCodeIdentifier = String.Empty
            If OrderItemRecord.VendorItem_ID.Trim().Length = 0 Then
                strVendorCodeIdentifier = String.Empty
            Else
                strVendorCodeIdentifier = "VC"
            End If

            ' Determine the value for the UPC code identifier
            strUPCCodeIdentifier = String.Empty
            If OrderItemRecord.Identifier.Trim().Length = 0 Then
                strUPCCodeIdentifier = String.Empty
            Else
                strUPCCodeIdentifier = "UP"
            End If

            sbString.Append("PO1*" & OrderItemRecord.LineItemNumber & "*" & OrderItemRecord.QuantityOrdered & "*" & OrderItemRecord.OrderUnit & "*" & OrderItemRecord.UnitOfMeasureCost & "**" & strUPCCodeIdentifier & "*" & OrderItemRecord.Identifier.PadLeft(12, CChar("0")) & "*" & strVendorCodeIdentifier & "*" & OrderItemRecord.VendorItem_ID & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

            sbString.Append("PID*F****" & OrderItemRecord.ItemDescription & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

            sbString.Append("PO4*" & CInt(OrderItemRecord.Package_Desc1) & "*" & OrderItemRecord.Package_Desc2 & "*" & OrderItemRecord.ItemUnit & "**************~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)

            If OrderItemRecord.LineItemFreight <> 0 Then
                ' There is a line item freight amount, so create a segment for it
                sbString.Append("SAC*C*D200***" & OrderItemRecord.LineItemFreight & "~")
                If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
            End If

            If OrderItemRecord.ItemAllowanceDiscountAmount <> 0 Then
                ' There is a line item allowance discount amount, so create a segment for it
                sbString.Append("SAC*A*D240***" & OrderItemRecord.ItemAllowanceDiscountAmount & "~")
                If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
            End If

            If m_return_order = True Then
                strCreditOrderIdentifier = "*C"
            Else
                strCreditOrderIdentifier = String.Empty
            End If

            sbString.Append("AMT*1*" & OrderItemRecord.LineItemCost & strCreditOrderIdentifier & "~")
            If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)
        Next


        '---------------------------------------------------------------------
        ' Insert the order summary

        ' This is the number of line items for the purchase order
        sbString.Append("CTT*" & m_order_items.Count & "~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)


        '---------------------------------------------------------------------
        ' Insert the functional group trailer

        ' Get a count of segments in the string, using the trailing tilde
        ' characters of each segment to represent one segment each.  Also,
        ' add one to the count for the trailing SE segment, which will be 
        ' created next.
        intSegmentCount = (Regex.Matches(sbString.ToString(), "~").Count.ToString()) + 1

        sbString.Append("SE*" & intSegmentCount & "*" & m_order_header_id & "~")
        If SuffixWithLineBreaks = True Then sbString.Append(Microsoft.VisualBasic.vbCrLf)


        '---------------------------------------------------------------------
        ' Return the created string
        Return sbString.ToString()


    End Function

    Public Shared Function ProcessEDI855(ByVal arrInputString() As String) As String
        Dim objOrder As Order
        Dim arrOrderItems As New ArrayList
        Dim objOrderItem As OrderItem
        Dim booPO1Found As Boolean = False
        'Dim strCurrent855Record As String
        Dim lngCurrentPO As Integer
        Dim strAllowanceOrCharge As String
        Dim strResults As String = String.Empty

        Dim strOHAckDate As String = String.Empty
        Dim strOHAckExpectedDeliveryDate As String = String.Empty
        Dim strOHAckDiscountAmount As String = String.Empty
        Dim strOIAckPackQuantity As String = String.Empty
        Dim strOIAckPackSize As String = String.Empty
        Dim strOIAckPackUnitID As String = String.Empty
        Dim strOIAckQuantity As String = String.Empty 'x
        Dim strOIAckQuantityUnitID As String = String.Empty
        Dim strOIAckCost As String = String.Empty
        Dim strOIAckDiscountAmount As String = String.Empty
        Dim strOIAckLotNo As String = String.Empty
        Dim strOIACK_UPCNo As String = String.Empty

        Dim arrParsedStrings As String()

        ' Find the PO number in the records
        For Each strCurrent855Record As String In arrInputString
            '''''strCurrent855Record = str855Record.ToString()
            Select Case Left(strCurrent855Record, 3)
                Case "ST*" ' 
                    arrParsedStrings = strCurrent855Record.Split("*")
                    ' "01" = Transaction Set Identifier Code (850 = purchase order)
                    ' "02" = Transaction Set Control Number (PO number)

                    lngCurrentPO = CLng(arrParsedStrings(2).ToString())

                    'instantiate a new Order class, using the PO number
                    objOrder = New Order(lngCurrentPO)

                    ReDim arrParsedStrings(0)

                Case "DTM"
                    arrParsedStrings = strCurrent855Record.Split("*")
                    ' "01" = Date/Time Qualifier (002 = Delivery Requested)
                    ' "02" = Date (YYYYMMDD)

                    ' Extract the expected delivery date
                    'strOHAckExpectedDeliveryDate = arrParsedStrings(2).ToString
                    Dim strHoldDate As String = arrParsedStrings(2).ToString

                    objOrder.m_ack_expected_delivery_date = strHoldDate.Substring(0, 4) & "/" & strHoldDate.Substring(4, 2) & "/" & strHoldDate.Substring(6, 2)

                    ReDim arrParsedStrings(0)
            End Select
        Next

        If lngCurrentPO = 0 Then
            strResults = "No PO number found"
            GoTo EndOfProcedure
        End If

        ' Get a listing of item units, so we can convert our EDI System Codes (EA, CA, LB...)
        ' into ItemID values (25, 1, 4...)
        arrOrderItems = Item.GetP2PItemUnits

        ' Process through the order records and extract values
        For Each str855Record As String In arrInputString

            Select Case Left(str855Record, 3)
                Case "PO1" ' Baseline Item Data 
                    ' "01" = Assigned ID (line number)
                    ' "02" = Quantity Ordered
                    ' "03" = Unit or Basis for Measurement Code
                    ' "04" = Unit Price
                    ' "06" = Product/Service ID Qualifier (UPC designator)
                    ' "07" = Product/Service ID (UPC code)
                    ' "08" = Product/ServiceID Qualifier (vendor's catalog number designator)
                    ' "09" = Product/Service ID (vendor's catalog number)

                    booPO1Found = True

                    ' Split the current record into individual elements
                    ReDim arrParsedStrings(0)
                    arrParsedStrings = str855Record.Split("*")

                    ' Get the order item, sending in the UPC code
                    objOrderItem = objOrder.GetOrderItem(arrParsedStrings(7).ToString())

                    ' Save off the PO1 parts
                    objOrderItem.AckQuantityOrdered = CDec(arrParsedStrings(2).ToString())

                    strOIAckPackUnitID = arrParsedStrings(3).ToString()
                    objOrderItem.QuantityUnit = objOrder.GetP2PUnitID(arrOrderItems, strOIAckPackUnitID)

                    'bubba - convert this to a proper ack cost
                    'itemcatalog.Item.ConvertCost (
                    '                    objOrderItem.AckCost = CDec(arrParsedStrings(4).ToString())
                    '                    objOrderItem.Lot_No = arrParsedStrings(11).ToString()

                Case "PID" ' Product/Item Description
                    ' "01" = Item Description Type 
                    ' "05" = Description

                Case "PO4" ' Item Physical Details 
                    ' "01" = Pack
                    ' "02" = Size
                    ' "03" = Unit or Basis for Measurement Code
                    ' "16" = Assigned Identification (product brand)
                    ' "17" = Assigned Identification (item size)

                    If booPO1Found Then
                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = str855Record.Split("*")

                        ' Save off the PO4 parts
                        objOrderItem.Package_Desc1 = CDec(arrParsedStrings(1).ToString())
                        objOrderItem.Package_Desc2 = CDec(arrParsedStrings(2).ToString())

                        strOIAckPackUnitID = arrParsedStrings(3).ToString()
                        objOrderItem.Package_Unit_ID = objOrder.GetP2PUnitID(arrOrderItems, strOIAckPackUnitID)
                    End If

                Case "SAC" ' Service, Promotion, Allowance, or Charge Information 
                    ' "01" = Allowance or Charge Indicator
                    ' "02" = Service, Promotion, Allowance, or Charge Code
                    ' "05" = Amount
                    If booPO1Found Then
                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = str855Record.Split("*")

                        ' Save off the SAC parts
                        strAllowanceOrCharge = arrParsedStrings(1).ToString

                        If strAllowanceOrCharge = "A" Then
                            ' Process an allowance
                            If CDec(arrParsedStrings(5).ToString) > 0 Then
                                objOrderItem.ItemAllowanceDiscountAmount = CDec(arrParsedStrings(5).ToString)
                            End If
                        End If

                        ' 17 April 2007, Ryan Roberts - Future enhancement.
                        'If strAllowanceOrCharge = "C" Then
                        '    ' Process a charge
                        '    If CDec(arrParsedStrings(3).ToString) > 0 Then
                        '        objOrderItem.ItemChargeAmount = CDec(arrParsedStrings(3).ToString)
                        '    End If
                        'End If
                    End If

                Case "AMT" ' Transaction Totals
                    ' "01" = Amount Qualifier Code
                    ' bubba - not used anymore
                    ' "02" = Monetary Amount
                    If booPO1Found Then
                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = str855Record.Split("*")

                        ' Save off the AMT parts
                        'objOrderItem.LineItemCost = arrParsedStrings(2).ToString()
                    End If

                Case Else
                    'strResults = String.Empty
            End Select

            'bubba - convert this to a proper ack cost.  Create local variables
            ' for each of the input parms.  Call this when all values are populated.
            ' Add additional control break processing at the end of each line item.
            objOrderItem.AckCost = ItemCatalog.Item.ConvertCost(objOrderItem.AckCost, objOrderItem.QuantityUnit, objOrderItem.CostUnit, objOrderItem.Package_Desc1, objOrderItem.Package_Desc2, objOrderItem.Package_Unit_ID, 0, 0)
            objOrderItem.AckCost = CDec(arrParsedStrings(4).ToString())

        Next

        ' Call a new procedure to update the Order and Order Item records
        objOrder.UpdateP2PAck855()
        strResults = "Success"

EndOfProcedure:
        Return strResults
    End Function

    Public Shared Function ProcessEDI810(ByVal InputString As String) As String
        Dim arrInvoices As ArrayList
        Dim arrInputString As String()
        Dim strResults As String
        Dim strReturnResults As String = String.Empty

        ' Separate the continuous lines of segments into discrete segment lines
        arrInputString = InputString.Split("~")

        ' Call a procedure to extract the invoices, leaving us with a series
        ' of ST to SE segments, each group of which will occupy one element
        ' in the invoices array
        arrInvoices = Separate810Invoices(arrInputString)

        ' Loop through the invoice records and process the invoices
        For Each InvoiceRecord As String In arrInvoices
            ' separate the continuous line of segments for an invoice, leaving
            ' us with with a series of discrete segment lines for one invoice
            ReDim arrInputString(0)
            arrInputString = InvoiceRecord.Split("~")

            ' Call a procedure to to process the invoice segments
            strResults = String.Empty
            strResults = ProcessEDI810Invoice(arrInputString)

            If strResults.Trim.Length > 0 Then
                ' There was a problem with the invoice, so save off the message
                ' to send it out to the package.
                strReturnResults += strResults & Microsoft.VisualBasic.vbCr
            End If
        Next


EndOfProcedure:
        Return strReturnResults
    End Function

    Private Shared Function Separate810Invoices(ByVal arrInputString() As String) As ArrayList
        Dim arrInvoices As New ArrayList
        Dim lngHoldControlNumber As Long = 0
        Dim booSaveSegment As Boolean
        Dim sbString As New StringBuilder
        Dim arrParsedLine As String()

        ' This function receives the contents of an EDI 810 file, then
        ' separates out the individual invoices, returning them as an arraylist.

        ' Find the first ST line, to be able to prime the PO number.
        For Each strLine As String In arrInputString
            Select Case Strings.Left(strLine, 3)


                Case "ST*" ' beginning segment for invoice
                    'lngHoldControlNumber = 
                    arrParsedLine = strLine.Split("*")
                    ' "01" = Transaction Set Identifier Code
                    ' "02" = Transaction Set Control Number (unique number)

                    lngHoldControlNumber = CLng(arrParsedLine(2).ToString())
                    Exit For
            End Select
        Next

        If lngHoldControlNumber = 0 Then
            arrInvoices.Add(CObj("No Invoices were found in the file"))
            GoTo BottomOfProcedure
        End If

        ' Now that we have our priming PO Number, process through the file and
        ' separate the invoices.

        booSaveSegment = False

        For Each strLine As String In arrInputString
            Select Case Strings.Left(strLine, 3)
                Case "ST*" ' beginning segment for invoice
                    arrParsedLine = strLine.Split("*")
                    ' "01" = Transaction Set Identifier Code
                    ' "02" = Transaction Set Control Number (unique number)

                    ' Save off the ST segment
                    sbString.Append(strLine & "~")

                    ' Denote that we're in "save line" mode
                    booSaveSegment = True

                    If lngHoldControlNumber <> CLng(arrParsedLine(2).ToString()) Then
                        ' This line's PO number is different from the previous one, so
                        ' update the current PO number
                        lngHoldControlNumber = CLng(arrParsedLine(2).ToString())
                    End If

                Case "SE*"
                    ' We're at the end of a PO, so use the current sbString to create a new 
                    ' invoice arrayList item.

                    ' Save off the current segment
                    sbString.Append(strLine & "~")

                    arrInvoices.Add(sbString.ToString())

                    'Reset the string builder
                    sbString.Length = 0

                    ' Denote that we're NOT in "save line" mode.  We'll start saving
                    ' lines again once we've found the next ST segment
                    booSaveSegment = False

                Case Else
                    If booSaveSegment Then
                        ' Save off the current line
                        sbString.Append(strLine & "~")
                    End If

            End Select
        Next

BottomOfProcedure:
        Return arrInvoices

    End Function

    Private Shared Function ProcessEDI810Invoice(ByVal arrInvoiceStrings() As String) As String
        Dim objOrder As Order
        Dim objOrderItem As OrderItem
        Dim arrOrderItems As New ArrayList
        Dim booWorkingWithLineItem As Boolean = False
        Dim strAllowanceOrCharge As String
        Dim strResults As String = String.Empty
        Dim lngInvQuantityUnit As Long
        Dim strOIUPC As String = String.Empty
        Dim strPackUnitID As String = String.Empty
        Dim arrParsedStrings As String()

        ' Find the PO number in the records
        For Each strCurrent810Record As String In arrInvoiceStrings
            Select Case Left(strCurrent810Record, 3)
                Case "BIG" ' beginning segment for invoice (invoice header)
                    arrParsedStrings = strCurrent810Record.Split("*")
                    ' "01" = Invoice Date
                    ' "02" = Invoice Number
                    ' "04" = Transaction Set Control Number (PO number)

                    If arrParsedStrings(4).ToString.Trim.Length = 0 Then
                        ' There is no PO number
                        strResults += "A BIG segment was found, but a PO number was not found in it (BIG04).  Invoice = " & arrParsedStrings(1).ToString() & ", Invoice segment = " & strCurrent810Record & Microsoft.VisualBasic.vbCr
                        GoTo EndOfProcedure
                    Else
                        ' There is a PO number, so instantiate a new Order class, using the PO number
                        objOrder = New Order(CLng(arrParsedStrings(4).ToString()))

                        If Not IsNothing(objOrder.m_uploaded_date) And objOrder.m_uploaded_date <> "#12:00:00 AM#" Then
                            ' There is an uploaded date value for this PO, so we can't process this invoice.
                            strResults += "The purchase order has already been uploaded to the accounts payable system.  PO Number = " & arrParsedStrings(4).ToString() & ", Invoice segment = " & strCurrent810Record & Microsoft.VisualBasic.vbCr
                            GoTo EndOfProcedure
                        Else
                            ' Save off some values for the order
                            objOrder.m_invoice_no = arrParsedStrings(2).ToString()
                            Dim strIDate As String = arrParsedStrings(1).ToString()
                            strIDate = strIDate.Substring(4, 2) & "/" & strIDate.Substring(6, 2) & "/" & strIDate.Substring(0, 4)
                            objOrder.m_invoice_date = CDate(strIDate)

                            ReDim arrParsedStrings(0)
                            Exit For
                        End If
                    End If

            End Select
        Next


        ' Get a listing of item units, so we can convert our EDI System Codes (EA, CA, LB...)
        ' into ItemID values (25, 1, 4...)
        arrOrderItems = Item.GetP2PItemUnits

        booWorkingWithLineItem = False

        ' Process through the order records and process values
        For Each strInvoiceRecord As String In arrInvoiceStrings

            ' We're working with line items
            Select Case Left(strInvoiceRecord, 3)
                Case "IT1" ' Baseline Item Data (line item)
                    ' "01" = Assigned ID (line number)
                    ' "02" = Quantity Invoiced
                    ' "03" = Unit or Basis for Measurement Code
                    ' "04" = Unit Price
                    ' "06" = Product/Service ID Qualifier (UPC designator)
                    ' "07" = Product/Service ID (UPC code)
                    ' "08" = Product/ServiceID Qualifier (vendor's catalog number designator)
                    ' "09" = Product/Service ID (vendor's catalog number)

                    booWorkingWithLineItem = True
                    'Set the ProcessingOrderItem to True

                    ' Split the current record into individual elements
                    ReDim arrParsedStrings(0)
                    arrParsedStrings = strInvoiceRecord.Split("*")

                    ' Save off the UPC, then get the order item, sending in the UPC code
                    strOIUPC = arrParsedStrings(7).ToString()

                    ' Remove any leading zeroes from the UPC
                    strOIUPC = CDbl(strOIUPC).ToString

                    'objOrderItem = Nothing
                    objOrderItem = objOrder.GetOrderItem(strOIUPC)

                    ' Save off the IT1 parts
                    objOrderItem.InvoiceQuantityShipped = CDec(arrParsedStrings(2).ToString())

                    strPackUnitID = arrParsedStrings(3).ToString()
                    lngInvQuantityUnit = objOrder.GetP2PUnitID(arrOrderItems, strPackUnitID)

                    If lngInvQuantityUnit <> objOrderItem.QuantityUnit Then
                        ' The quantity units are NOT the same, so convert the ordered
                        ' stuff to be the same as the invoice stuff, and save off the 
                        ' invoiced values.
                        objOrderItem.QuantityOrdered = ItemCatalog.Item.ConvertCost(objOrderItem.QuantityOrdered, lngInvQuantityUnit, objOrderItem.QuantityUnit, objOrderItem.Package_Desc1, objOrderItem.Package_Desc2, objOrderItem.Package_Unit_ID, 0, 0)
                        objOrderItem.QuantityUnit = lngInvQuantityUnit
                    End If

                    objOrderItem.InvoiceCost = CDec(arrParsedStrings(4).ToString())

                    If arrParsedStrings.Length > 10 Then
                        If arrParsedStrings(10).ToString() = "LT" Then
                            objOrderItem.Lot_No = arrParsedStrings(11).ToString()
                        End If
                    End If

                Case "IT3" ' Additional Item Data (line item)
                    ' "01" = Units Shipped - Weight 
                    ' "02" = Unit or Basis of Measurement Code 

                    If Not IsNothing(objOrderItem) Then
                        ' There is an order item object, so let's process this segment

                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = strInvoiceRecord.Split("*")

                        If arrParsedStrings(2).ToString() = "LB" Then
                            objOrderItem.Total_Weight = CDec(arrParsedStrings(1).ToString())
                        End If
                    End If

                Case "PAM" ' Period Amount (line item)
                    ' "04" = Amount Qualifier Code 
                    ' "05" = Monetary Amount

                    If Not IsNothing(objOrderItem) Then
                        ' There is an order item object, so let's process this segment

                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = strInvoiceRecord.Split("*")

                        Select Case arrParsedStrings(1).ToString()
                            Case "1"
                                objOrderItem.InvoiceExtendedCost = CDec(arrParsedStrings(2).ToString())
                            Case Else
                                ' Report the shortcoming with the SAC Allowance segment
                                strResults += "A PAM segment was provided, but it contained an invalid amount qualifier code (PAM01).  OrderItemID = " & objOrderItem.OrderItem_ID & ", UPC = " & strOIUPC & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                        End Select
                    End If

                Case "PO4"
                    ' "01" = Pack
                    ' "02" = Size
                    ' "03" = Unit or basis of Measurement Code

                    If Not IsNothing(objOrderItem) Then
                        ' There is an order item object, so let's process this segment

                        ' Split the current record into individual elements
                        ReDim arrParsedStrings(0)
                        arrParsedStrings = strInvoiceRecord.Split("*")

                        If arrParsedStrings(1).ToString.Trim.Length > 0 Then
                            ' Get the unit ID for the unit description
                            'lngInvQuantityUnit = objOrder.GetP2PUnitID(arrOrderItems, arrParsedStrings(1).ToString())

                            If CDec(arrParsedStrings(1).ToString()) <> objOrderItem.Package_Desc1 Then
                                ' the invoice's pack is different from the order item's pack, so save 
                                'off the invoice's pack to the order item
                                objOrderItem.Package_Desc1 = CDec(arrParsedStrings(1).ToString())
                            End If
                        Else
                            ' Report the shortcoming with the SAC Allowance segment
                            strResults += "A PO4 segment was provided, but it contained no pack value (PO401).  OrderItemID = " & objOrderItem.OrderItem_ID & ", UPC = " & strOIUPC & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                        End If
                    End If

                Case "TDS" 'Total Monetary Value Summary (invoice summary)
                    ' "01" = "Total Invoice Amount"

                    ' This is the first segment of an invoice summary, so denote
                    ' that we're no longer working on a line item.  Otherwise,
                    ' the invoice summary's SAC lines might be accidentally assigned to 
                    ' an invoice item record.
                    booWorkingWithLineItem = False

                    ' Split the current record into individual elements
                    ReDim arrParsedStrings(0)
                    arrParsedStrings = strInvoiceRecord.Split("*")

                    ' Save off the total invoice amount
                    If arrParsedStrings(1).ToString.Trim.Length > 0 Then
                        objOrder.m_invoice_cost = CDec(arrParsedStrings(1).ToString)
                    End If

                Case "SAC" ' Service, Promotion, Allowance, or Charge Information (line item)

                    ' Discount Types:
                    '     0 = "None"
                    '     1 = "Cash Discount"
                    '     2 = "Percent Discount"
                    '     3 = "Free"
                    '     4 = "Landed Percent"

                    ' Split the current record into individual elements
                    ReDim arrParsedStrings(0)
                    arrParsedStrings = strInvoiceRecord.Split("*")

                    ' Save off the allowance or charge indicator
                    ' "01" = Allowance or Charge Indicator
                    strAllowanceOrCharge = arrParsedStrings(1).ToString

                    If booWorkingWithLineItem Then
                        ' We're working with a Line Item SAC segment

                        ' "01" = Allowance or Charge Indicator
                        ' "02" = Service, Promotion, Allowance, or Charge Code
                        ' "05" = Amount
                        ' "06" = Allowance/Charge Percent Qualifier
                        ' "07" = Percent
                        ' "09" = Unit or Basis for Measurement Code
                        ' "10" = Quantity

                        If Not IsNothing(objOrderItem) Then
                            ' There is an order item object, so let's process this segment

                            If strAllowanceOrCharge = "A" Then
                                ' Process an allowance
                                Select Case arrParsedStrings(2).ToString
                                    Case "C310"
                                        If arrParsedStrings(5).ToString.Trim.Length > 0 Then
                                            ' use the discount amount
                                            objOrderItem.DiscountType = 1
                                            objOrderItem.QuantityDiscount = CDec(arrParsedStrings(5).ToString)
                                        Else
                                            ' Use the discount percent
                                            ' Save off the discount type
                                            objOrderItem.DiscountType = 2
                                            ' Save off the discount amount
                                            objOrderItem.QuantityDiscount = CDec(arrParsedStrings(7).ToString)
                                        End If
                                    Case "D170"
                                        If arrParsedStrings(10).ToString.Trim.Length > 0 Then
                                            ' Use the quantity of free items
                                            ' Save off the discount type
                                            objOrderItem.DiscountType = 3
                                            ' Save off the discount amount
                                            objOrderItem.QuantityDiscount = CDec(arrParsedStrings(10).ToString)
                                        End If
                                    Case Else
                                        ' Report the shortcoming with the SAC Allowance segment
                                        strResults += "A SAC segment was listed as an allowance, but it contained an invalid allowance code (SAC02).  OrderItemID = " & objOrderItem.OrderItem_ID & ", UPC = " & strOIUPC & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                                End Select
                            End If

                            If strAllowanceOrCharge = "C" Then
                                ' Process a charge
                                Select Case arrParsedStrings(2).ToString
                                    Case "D200"
                                        If arrParsedStrings(5).ToString.Trim.Length > 0 Then
                                            ' Save off the line item's invoice freight value
                                            objOrderItem.InvoiceExtendedFreight = CDec(arrParsedStrings(5).ToString)

                                            ' Calculate / recalculate the freight charge per unit
                                            objOrderItem.Freight = ItemCatalog.Item.ConvertCost(objOrderItem.InvoiceExtendedFreight / objOrderItem.InvoiceQuantityShipped, _
                                                                                                objOrderItem.QuantityUnit, _
                                                                                                objOrderItem.FreightUnit, _
                                                                                                objOrderItem.Package_Desc1, _
                                                                                                objOrderItem.Package_Desc2, _
                                                                                                objOrderItem.Package_Unit_ID, _
                                                                                                0, _
                                                                                                0)
                                        End If
                                    Case Else
                                        ' Report the shortcoming with the SAC Allowance segment
                                        strResults += "A SAC segment was listed as a charge, but it contained an invalid charge code (SAC02).  OrderItemID = " & objOrderItem.OrderItem_ID & ", UPC = " & strOIUPC & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                                End Select
                            End If
                        End If
                    Else
                        ' We're working with an invoice summary SAC segment

                        ' "01" = Allowance or Charge Indicator
                        ' "02" = Service, Promotion, Allowance, or Charge Code
                        ' "05" = Amount
                        ' "06" = Allowance/Charge Percent Qualifier
                        ' "07" = Percent

                        If strAllowanceOrCharge = "A" Then
                            ' Process an allowance
                            Select Case arrParsedStrings(2).ToString
                                Case "C310"
                                    If arrParsedStrings(7).ToString.Trim.Length > 0 Then
                                        ' Use the discount percent
                                        ' Save off the discount type
                                        objOrder.m_discount_type = 2
                                        ' Save off the discount amount
                                        objOrder.m_quantity_discount = CDec(arrParsedStrings(7).ToString)
                                    End If

                                Case Else
                                    ' Report the shortcoming with the SAC Allowance segment
                                    strResults += "A SAC segment was listed as an allowance, but it contained an invalid allowance code (SAC02).  OrderHeader_ID = " & objOrder.m_order_header_id & ", invoice = " & objOrder.m_invoice_no & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                            End Select
                        End If

                        If strAllowanceOrCharge = "C" Then
                            ' Process a charge
                            Select Case arrParsedStrings(2).ToString
                                Case "D200"
                                    If arrParsedStrings(5).ToString.Trim.Length > 0 Then
                                        objOrder.m_invoice_freight = CDec(arrParsedStrings(5).ToString)

                                        ' Since there is a freight charge, we need to update the
                                        ' cost to exclude the freight charge.  This will allow us
                                        ' to put a more accurate record into the OrderInvoice table.
                                        objOrder.m_invoice_cost = objOrder.m_invoice_cost - objOrder.m_invoice_freight
                                    End If

                                Case Else
                                    ' Report the shortcoming with the SAC Allowance segment
                                    strResults += "A SAC segment was listed as a charge, but it contained an invalid charge code (SAC02).  OrderHeader_ID = " & objOrder.m_order_header_id & ", invoice = " & objOrder.m_invoice_no & ", Invoice segment = " & strInvoiceRecord & Microsoft.VisualBasic.vbCr
                            End Select
                        End If
                    End If

                Case Else
                    ' Ignore the current record
            End Select

        Next strInvoiceRecord


        ' Now that we've updated the record objects for the current PO, use them to update the database
        If strResults.Trim.Length = 0 Then
            ' There are no issues to report, so update the order
            objOrder.UpdateP2PEInvoice()
        End If

EndOfProcedure:
        Return strResults
    End Function

    Public Shared Sub ProcessEdiEinvoice(ByVal myXmlDoc As Xml.XmlDocument)
        Dim myInvoices As Xml.XmlNodeList ' = myXmlDoc.SelectNodes("/invoiceList/invoice")
        Dim invoiceSummary As Xml.XmlNode
        Dim myLineItems As Xml.XmlNodeList
        Dim objOrder As Order
        Dim objOrderItem As OrderItem

        Dim OH_invoice_PoNum As Long
        Dim OH_invoice_no As String
        Dim OH_invoice_date As DateTime
        Dim OS_invoice_freight As Decimal
        Dim OS_invoice_cost As Decimal
        Dim OI_invoice_UPC As String
        Dim OI_invoice_quantity As Decimal
        Dim OI_invoice_cost As Decimal
        Dim OI_invoice_lot_no As String
        Dim OI_invoice_PackSize As String
        Dim OI_invoice_DiscountAmt As Decimal
        Dim OI_invoice_ExtendedCost As Decimal

        Dim i As Integer = 0

        Dim sDiscountType() As String
        ReDim sDiscountType(4)
        sDiscountType(0) = "None"
        sDiscountType(1) = "Cash Discount"
        sDiscountType(2) = "Percent Discount"
        sDiscountType(3) = "Free"
        sDiscountType(4) = "Landed Percent"

        Try
            myInvoices = myXmlDoc.SelectNodes("/invoiceList/invoice")
            For Each invoice As Xml.XmlNode In myInvoices
                ' Get the order header values
                OH_invoice_PoNum = CLng(invoice.Item("ponum").InnerText)

                If OH_invoice_PoNum = 0 Or IsNothing(OH_invoice_PoNum) Then
                    'intResults = 0 '"No PO number found"
                    Throw New ItemCatalog.Exception.Order.InvoiceMissingPONum
                End If

                ' Save off the invoice values for the order header table
                OH_invoice_no = invoice.Item("invno").InnerText
                OH_invoice_date = IIf(invoice.Item("invdte").InnerText = "", System.DBNull.Value, CDate(invoice.Item("invdte").InnerText))

                ' Save off the summary values for the order invoice table.  There should
                ' be only one summary.
                invoiceSummary = invoice.SelectSingleNode("summary")
                OS_invoice_freight = CDec(invoiceSummary.Item("invfreight").InnerText)
                OS_invoice_cost = CDec(invoiceSummary.Item("netinv").InnerText)

                'instantiate a new Order class, using the PO number
                objOrder = New Order(OH_invoice_PoNum)

                ' assign the order header and order summary values to the order header object
                objOrder.m_invoice_no = OH_invoice_no
                objOrder.m_invoice_date = OH_invoice_date
                objOrder.m_invoice_cost = OS_invoice_cost - OS_invoice_freight
                objOrder.m_invoice_freight = OS_invoice_freight

                If Not IsNothing(objOrder.m_uploaded_date) And objOrder.m_uploaded_date <> "#12:00:00 AM#" Then
                    ' There is an uploaded date value for this PO, so we can't process this invoice.
                    Throw New ItemCatalog.Exception.Order.InvoiceAlreadyUploadedToAP(objOrder.m_order_header_id)
                End If

                ' -------------------------------------------------
                ' Process line items
                myLineItems = invoice.SelectNodes("lineItems/lineItem")
                For Each lineItem As Xml.XmlNode In myLineItems
                    ' Save off the lineItem values for the order item table
                    OI_invoice_UPC = lineItem.Item("upc").InnerText.Trim
                    OI_invoice_quantity = CDec(lineItem.Item("qtyship").InnerText.Trim)
                    OI_invoice_cost = CDec(lineItem.Item("price").InnerText.Trim)
                    OI_invoice_lot_no = lineItem.Item("lotno").InnerText.Trim
                    OI_invoice_DiscountAmt = CDec(lineItem.Item("discount").InnerText.Trim)
                    OI_invoice_ExtendedCost = CDec(lineItem.Item("extprice").InnerText.Trim)


                    ' Remove any leading zeroes from the UPC
                    OI_invoice_UPC = CDbl(OI_invoice_UPC).ToString

                    ' Find the correct order item record
                    objOrderItem = objOrder.GetOrderItem(OI_invoice_UPC)

                    If Not IsNothing(objOrderItem) Then
                        ' We have an order item, so process it

                        ' Update a couple values
                        objOrderItem.InvoiceQuantityShipped = OI_invoice_quantity

                        ' We can send in only null, non-zero numbers, and characters for the lot number
                        objOrderItem.Lot_No = IIf((OI_invoice_lot_no.Trim.Length = 0 Or OI_invoice_lot_no = "0"), String.Empty, OI_invoice_lot_no)

                        '' Convert the cost.  David said so.  18 June 2007, 0930
                        '' Ummmm.  Nevermind the cost conversion.  David said so.  :)  19 June 2007, 1500
                        'objOrderItem.InvoiceCost = ItemCatalog.Item.ConvertCost(objOrderItem.InvoiceCost, objOrderItem.QuantityUnit, objOrderItem.CostUnit, objOrderItem.Package_Desc1, objOrderItem.Package_Desc2, objOrderItem.Package_Unit_ID, 0, 0)
                        objOrderItem.InvoiceCost = OI_invoice_cost

                        objOrderItem.QuantityDiscount = OI_invoice_DiscountAmt
                        objOrderItem.InvoiceExtendedCost = OI_invoice_ExtendedCost
                    End If

                Next lineItem

                ' Now that we've updated the record objects for the current PO, use them to update the database

                ' Update the order
                objOrder.UpdateP2PEInvoice()

            Next invoice

        Catch ex As System.Exception
            Throw ex
        End Try

    End Sub

    Private Function GetP2PUnitID(ByVal arrInput As ArrayList, ByVal UnitOfMeasure As String) As Integer
        Dim strResults As String = String.Empty

        ' The EDI 855 acknowledgement document contains a value like 'LB', 'CA', 'EA', and FO,
        ' which needs to be translated into its unit_id like 4, 1, 25, and 26.

        For Each ItemUnitRecord As ItemUnit In arrInput
            If ItemUnitRecord.EDISystemCode = UnitOfMeasure Then
                strResults = ItemUnitRecord.UnitID
                Exit For
            End If
        Next

        Return strResults

    End Function
    Private Sub UpdateP2PAck855()
        Dim i As Integer
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            '**********Update the Order****************

            'PROCEDURE dbo.UpdateP2POrderInfo
            '@OrderHeader_ID int,
            '@ExpectedDeliveryDate smalldatetime

            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateP2POrderInfo"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_order_header_id)))
            cmd.Parameters.Add(CreateParam("@ExpectedDeliveryDate", SqlDbType.SmallDateTime, ParameterDirection.Input, CObj(Me.m_ack_expected_delivery_date)))

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

            '**********Update the Order item****************
            '   PROCEDURE(dbo.UpdateP2POrderItemInfo)
            '   @OrderItem_ID int, 
            '   @AckQuantityOrdered decimal(18,4), 
            '   @AckCost money, 
            '   @Package_Desc1 decimal(9,4),
            '   @Package_Desc2 decimal(9,4),
            '   @Package_Unit_ID int,
            '   @QuantityDiscount decimal(18, 4), --ItemAllowanceDiscountAmount
            '   @QuantityUnit int, 
            '   @CostUnit int, 
            '   @Lot_No varchar(12)

            Dim objOrderItem As OrderItem

            ' Loop through the order items array list and process its order items
            For i = 0 To m_order_items.Count - 1

                ' Save off an array list row
                objOrderItem = CType(m_order_items(i), OrderItem)
                'objOrderItem.Package_Desc1 

                cmd.CommandText = "[UpdateP2POrderItemInfo]"
                cmd.Parameters.Clear()
                cmd.Parameters.Add(CreateParam("@OrderItem_ID", SqlDbType.Int, ParameterDirection.Input, CObj(objOrderItem.OrderItem_ID)))
                cmd.Parameters.Add(CreateParam("@AckQuantityOrdered", SqlDbType.Decimal, ParameterDirection.Input, CObj(objOrderItem.AckQuantityOrdered)))
                cmd.Parameters.Add(CreateParam("@AckCost", SqlDbType.Money, ParameterDirection.Input, CObj(objOrderItem.AckCost)))
                cmd.Parameters.Add(CreateParam("@Package_Desc1", SqlDbType.Decimal, ParameterDirection.Input, CObj(objOrderItem.Package_Desc1)))
                cmd.Parameters.Add(CreateParam("@Package_Desc2", SqlDbType.Decimal, ParameterDirection.Input, CObj(objOrderItem.Package_Desc2)))
                cmd.Parameters.Add(CreateParam("@Package_Unit_ID", SqlDbType.Int, ParameterDirection.Input, CObj(objOrderItem.Package_Unit_ID)))
                cmd.Parameters.Add(CreateParam("@QuantityDiscount", SqlDbType.Decimal, ParameterDirection.Input, CObj(objOrderItem.ItemAllowanceDiscountAmount)))
                cmd.Parameters.Add(CreateParam("@QuantityUnit", SqlDbType.Int, ParameterDirection.Input, CObj(objOrderItem.QuantityUnit)))
                cmd.Parameters.Add(CreateParam("@Lot_No", SqlDbType.VarChar, ParameterDirection.Input, CObj(objOrderItem.Lot_No)))

                ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Next i

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try

    End Sub
    Private Function ExtractValue(ByVal Segment As String, ByVal Element As String) As String
        Dim strResults As String = String.Empty

        Select Case Segment
            Case "PO1" ' Baseline Item Data 
                Select Case Element
                    Case "01" ' Assigned ID
                    Case "02" ' Quantity Ordered
                    Case "03" ' Unit or Basis for Measurement Code
                    Case "04" ' Unit Price
                    Case "06" ' Product/Service ID Qualifier (UPC)
                    Case "07" ' Product/Service ID
                    Case "08" ' Product/ServiceID Qualifier (vendor's catalog number)
                    Case "09" ' Product/Service ID
                    Case Else
                        strResults = String.Empty
                End Select
            Case "PID" ' Product/Item Description
                Select Case Element
                    Case "01" ' Item Description Type 
                    Case "05" ' Description
                    Case Else
                        strResults = String.Empty
                End Select
            Case "PO4" ' Item Physical Details 
                Select Case Element
                    Case "01" ' Pack
                    Case "02" ' Size
                    Case "03" ' Unit or Basis for Measurement Code
                    Case "16" ' Assigned Identification (product brand)
                    Case "17" ' Assigned Identification (item size)
                    Case Else
                        strResults = String.Empty
                End Select
            Case "SAC" ' Service, Promotion, Allowance, or Charge Information 
                Select Case Element
                    Case "01" ' Allowance or Charge Indicator
                    Case "02" ' Service, Promotion, Allowance, or Charge Code
                    Case "05" ' Amount
                    Case Else
                        strResults = String.Empty
                End Select
            Case "AMT" ' Transaction Totals
                Select Case Element
                    Case "01" ' Amount Qualifier Code
                    Case "02" ' Monetary Amount
                    Case Else
                        strResults = String.Empty
                End Select
            Case Else
                strResults = String.Empty
        End Select

        Return strResults
    End Function
    Public ReadOnly Property CreatedByName() As String
        Get
            If Not (m_created_by_user Is Nothing) Then
                Return Me.m_created_by_user.FullName
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public ReadOnly Property Sent() As Boolean
        Get
            Return m_sent
        End Get
    End Property
    Public ReadOnly Property SentDate() As DateTime
        Get
            Return m_sent_date
        End Get
    End Property
    Public Property OrderHeader_ID() As Long
        Get
            Return m_order_header_id
        End Get
        Set(ByVal Value As Long)
            m_order_header_id = Value
        End Set

    End Property
    Public Property Vendor_ID() As Long
        Get
            Return m_vendor_id
        End Get
        Set(ByVal Value As Long)
            m_vendor_id = Value
        End Set
    End Property
    Public Property OrderType_ID() As enuOrderType
        Get
            Return m_order_type_id
        End Get
        Set(ByVal Value As enuOrderType)
            m_order_type_id = Value
        End Set
    End Property
    Public Property ProductType_ID() As Integer
        Get
            Return m_product_type_id
        End Get
        Set(ByVal Value As Integer)
            m_product_type_id = Value
        End Set
    End Property
    Public Property PurchaseLocation_ID() As Long
        Get
            Return m_purchase_location_id
        End Get
        Set(ByVal Value As Long)
            m_purchase_location_id = Value
        End Set
    End Property
    Public Property ReceiveLocation_ID() As Long
        Get
            Return m_receive_location_id
        End Get
        Set(ByVal Value As Long)
            m_receive_location_id = Value
        End Set
    End Property
    Public Property ReceiveStore_No() As Long
        Get
            Return Me.m_store_no
        End Get
        Set(ByVal Value As Long)
            Me.m_store_no = Value
        End Set
    End Property
    Public Property CreatedByID() As Long
        Get
            Return m_created_by
        End Get
        Set(ByVal Value As Long)
            m_created_by = Value
            m_created_by_user = New User(Value)
        End Set
    End Property
    Public Property OrderDate() As DateTime
        Get
            Return m_order_date
        End Get
        Set(ByVal Value As DateTime)
            m_order_date = Value
        End Set
    End Property
    Public Property CloseDate() As DateTime
        Get
            Return m_close_date
        End Get
        Set(ByVal Value As DateTime)
            m_close_date = Value
        End Set
    End Property
    Public Property OriginalCloseDate() As DateTime
        Get
            Return m_original_close_date
        End Get
        Set(ByVal Value As DateTime)
            m_original_close_date = Value
        End Set
    End Property
    Public Property Fax() As Boolean
        Get
            Return m_fax
        End Get
        Set(ByVal Value As Boolean)
            m_fax = Value
        End Set
    End Property
    Public Property Expected_Date() As DateTime
        Get
            Return m_expected_date
        End Get
        Set(ByVal Value As DateTime)
            m_expected_date = Value
        End Set
    End Property
    Public Property Transfer_SubTeam() As Long
        Get
            Return m_transfer_subteam
        End Get
        Set(ByVal Value As Long)
            m_transfer_subteam = Value
        End Set
    End Property
    Public Property TransferToSubTeamName() As String
        Get
            Return m_transfer_to_subteam_name
        End Get
        Set(ByVal Value As String)
            m_transfer_to_subteam_name = Value
        End Set
    End Property
    Public Property Transfer_To_SubTeam() As Long
        Get
            Return m_transfer_to_subteam
        End Get
        Set(ByVal Value As Long)
            m_transfer_to_subteam = Value
        End Set
    End Property
    Public Property Return_Order() As Boolean
        Get
            Return m_return_order
        End Get
        Set(ByVal Value As Boolean)
            m_return_order = Value
        End Set
    End Property
    Public Property InvoiceDate() As DateTime
        Get
            Return m_invoice_date
        End Get
        Set(ByVal Value As DateTime)
            m_invoice_date = Value
        End Set
    End Property
    Public Property UploadedDate() As DateTime
        Get
            Return m_uploaded_date
        End Get
        Set(ByVal Value As DateTime)
            m_uploaded_date = Value
        End Set
    End Property
    Public Property SubteamAbbreviation() As String
        Get
            Return _SubteamAbbreviation
        End Get
        Set(ByVal value As String)
            _SubteamAbbreviation = value
        End Set
    End Property
    Public Property EXEWarehouse() As Integer
        Get
            Return _EXEWarehouse
        End Get
        Set(ByVal value As Integer)
            _EXEWarehouse = value
        End Set
    End Property
    Public Property Buyer() As String
        Get
            Return _Buyer
        End Get
        Set(ByVal value As String)
            _Buyer = value
        End Set
    End Property
    Public Property DistCenter() As Integer
        Get
            Return _DistCenter
        End Get
        Set(ByVal value As Integer)
            _DistCenter = value
        End Set
    End Property

    Public Property Email() As Boolean
        Get
            Return m_email
        End Get
        Set(ByVal value As Boolean)
            m_email = value
        End Set
    End Property

    Public Property Sent_To_Email_Date() As DateTime
        Get
            Return m_sent_to_email_date
        End Get
        Set(ByVal value As DateTime)
            m_sent_to_email_date = value
        End Set
    End Property

    Public Function DBAdd(ByVal ProductType As enuProductType, ByVal lOrigOrderHeaderID As Long) As Long
        '-------------------This DBAddOrder is for Return Orders as noted by the additional param lOrigOrderHeaderID

        'FV: add validation if me.ReturnOrder = false then throw validation exception
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Try
            Dim lNewOrderHeaderID As Long
            'Add this order to the DB just like any other order
            lNewOrderHeaderID = DBAdd(ProductType)

            If lOrigOrderHeaderID > 0 Then
                'Insert a reference to the original order
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "InsertOrgPO"

                cmd.Parameters.Add(CreateParam("@OrgOrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(lOrigOrderHeaderID)))
                cmd.Parameters.Add(CreateParam("@ReturnOrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.OrderHeader_ID)))

                ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            End If
            Return lNewOrderHeaderID
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function DBAdd(ByVal ProductType As enuProductType) As Long
        Dim lNewOrderHeaderID As Long
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertOrder2"
            With cmd.Parameters
                .Add(CreateParam("@Vendor_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Vendor_ID)))
                .Add(CreateParam("@ProductType_ID", SqlDbType.TinyInt, ParameterDirection.Input, CObj(ProductType)))
                .Add(CreateParam("@OrderType_ID", SqlDbType.TinyInt, ParameterDirection.Input, CObj(Me.OrderType_ID)))
                .Add(CreateParam("@PurchaseLocation_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.PurchaseLocation_ID)))
                .Add(CreateParam("@ReceiveLocation_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.ReceiveLocation_ID)))
                .Add(CreateParam("@Transfer_SubTeam", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(Me.Transfer_SubTeam = 0, System.DBNull.Value, Me.Transfer_SubTeam))))
                .Add(CreateParam("@Transfer_To_SubTeam", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(Me.Transfer_To_SubTeam = 0, System.DBNull.Value, Me.Transfer_To_SubTeam))))
                .Add(CreateParam("@Fax_Order", SqlDbType.Bit, ParameterDirection.Input, CObj(Me.Fax)))
                .Add(CreateParam("@Email_Order", SqlDbType.Bit, ParameterDirection.Input, CObj(Me.Email)))
                .Add(CreateParam("@Expected_Date", SqlDbType.DateTime, ParameterDirection.Input, CObj(Me.Expected_Date)))
                .Add(CreateParam("@CreatedBy", SqlDbType.Int, ParameterDirection.Input, CObj(Me.CreatedByID)))
                .Add(CreateParam("@Return_Order", SqlDbType.Bit, ParameterDirection.Input, CObj(Me.Return_Order)))
                .Add(CreateParam("@NewOrderHeader_ID", SqlDbType.Int, ParameterDirection.Output))
            End With
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            lNewOrderHeaderID = cmd.Parameters("@NewOrderHeader_ID").Value

            m_order_header_id = lNewOrderHeaderID
            Return lNewOrderHeaderID
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Friend Function AddOrderItem(ByVal SI As StoreItem, ByVal IsOrderableCheck As Boolean) As OrderItem
        If SI.StoreNo <> Me.m_store_no Then Throw New System.ApplicationException("Input StoreItem not for this store")
        If SI.SubTeam_No <> Me.m_transfer_to_subteam Then Throw New System.ApplicationException("Input StoreItem not for this subteam")
        If IsOrderableCheck Then SI.IsOrderable(True)

        If m_order_items Is Nothing Then m_order_items = New ArrayList
        Dim orderItem As New ItemCatalog.OrderItem(SI)
        orderItem.GetItemIdentifersForItem(orderItem.Item_Key)
        m_order_items.Add(orderItem)
        Return orderItem
    End Function
    Public Function AddOrderItem(ByVal SI As ItemCatalog.StoreItem) As ItemCatalog.OrderItem
        'If SI.StoreNo <> Me.m_store_no Then Throw New System.ApplicationException("Input StoreItem not for this store")
        'If SI.SubTeam_No <> Me.m_transfer_to_subteam Then Throw New System.ApplicationException("Input StoreItem not for this subteam")
        'SI.IsOrderable(True)

        'If m_order_items Is Nothing Then m_order_items = New ArrayList
        'Dim orderItem As New ItemCatalog.OrderItem(SI)
        'm_order_items.Add(orderItem)
        'Return orderItem
        Return AddOrderItem(SI, True)
    End Function

    Public Function GetOrderItem(ByVal lItem_Key As Long) As OrderItem
        If Not (m_order_items Is Nothing) Then
            m_order_items.Sort(New OrderItemSort(enuOrderItemSortOrder.Item_Key))
            Dim searchOrderItem As New ItemCatalog.OrderItem
            searchOrderItem.Item_Key = lItem_Key
            Dim i As Integer = m_order_items.BinarySearch(searchOrderItem, New OrderItemGetItem_Key)
            If i >= 0 Then Return CType(m_order_items(i), OrderItem)
        End If
    End Function
    Public Function GetOrderItem(ByVal sIdentifier As String) As OrderItem
        If Not (m_order_items Is Nothing) Then
            m_order_items.Sort(New OrderItemSort(enuOrderItemSortOrder.Identifier))
            Dim searchOrderItem As New ItemCatalog.OrderItem
            searchOrderItem.Identifier = sIdentifier
            Dim i As Integer = m_order_items.BinarySearch(searchOrderItem, New OrderItemGetIdentifier)
            If i >= 0 Then Return CType(m_order_items(i), OrderItem)
        End If
    End Function
    Public Function GetOrderItem(ByVal sIdentifier As String, ByVal dPackage_Desc1 As Decimal) As OrderItem
        If Not (m_order_items Is Nothing) Then
            m_order_items.Sort(New OrderItemSort(enuOrderItemSortOrder.IdentifierPackage_Desc1))
            Dim searchOrderItem As New ItemCatalog.OrderItem
            searchOrderItem.Identifier = sIdentifier
            searchOrderItem.Package_Desc1 = dPackage_Desc1
            Dim i As Integer = m_order_items.BinarySearch(searchOrderItem, New OrderItemGetIdentifierPackage_Desc1)
            If i >= 0 Then Return CType(m_order_items(i), OrderItem)
        End If
    End Function
    ''' <summary>
    ''' Currently this is used by the warehouse object to get the list of matching order items from the order.  This overload is needed because Exe 
    ''' has a slightly different definition of Pack Size.  If the item is a 'CostedByWeight' item, then ExePackSize = Item.PD1 * Item.PD2
    ''' Used by EXE
    ''' </summary>
    ''' <param name="criteria">contains the information needed to retrieve the order item.</param>
    ''' <returns>Returns the list of matching order items based on pack size</returns>
    ''' <remarks></remarks>
    Friend Function GetOrderItem(ByVal criteria As Warehouse.WarehouseOrderItemGetCriteria) As OrderItem

        Dim ReturnItem As OrderItem = Nothing
        If Not (m_order_items Is Nothing) Then
            Dim oi As OrderItem
            For Each oi In Me.m_order_items
                If oi.IsSoldByWeight Then
                    Dim PD2 As Integer = Math.Round(oi.Package_Desc2, 0, MidpointRounding.AwayFromZero)
                    If PD2 = 0 Then PD2 = 1
                    If (oi.Package_Desc1 * PD2 = criteria.ExePackSize) AndAlso oi.IsMyIdentifier(criteria.Identifier) Then
                        ReturnItem = oi
                        Exit For
                    End If
                Else
                    If (oi.Package_Desc1 = criteria.ExePackSize) AndAlso oi.IsMyIdentifier(criteria.Identifier) Then
                        ReturnItem = oi
                        Exit For
                    End If
                End If
            Next
        End If
        Return ReturnItem
    End Function

    Private Sub Validate()

        'Do expected date validation
        If Me.Expected_Date = Nothing Then
            Throw New System.Exception("Must have expected date")
        Else
            If Date.Compare(Me.Expected_Date, Now.ToShortDateString) < 0 Then
                Throw New ItemCatalog.Exception.InvalidOrderException("Expected Date Cannot Be in the past")
            End If
        End If

        'Make sure there are items on this order
        If m_order_items.Count = 0 Then
            Throw New ItemCatalog.Exception.InvalidOrderException("There are no items on this order - send not allowed")
        End If

        'make sure there is an OrderHeader_ID for this order
        If Me.OrderHeader_ID = Nothing Then
            Throw New ItemCatalog.Exception.InvalidOrderException("You must have an OrderHeader_ID")
        End If

    End Sub

    Public Shared Sub UpdateP2PSentDate(ByVal OrderHeader_ID As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        'Dim dr As System.Data.SqlClient.SqlDataReader = nothing
        'Dim drTLEmail As System.Data.SqlClient.SqlDataReader

        Try
            'Dim prm As System.Data.SqlClient.SqlParameter
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateP2POrderSentDate"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(OrderHeader_ID)))

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub


    Private Sub UpdateP2PEInvoice()
        ' Save off the order items
        For Each objOrderItem As OrderItem In Me.m_order_items
            objOrderItem.UpdateP2PeInvoiceLineItem()
        Next

        ' Save off the order header and recalculate the costs associated with the PO
        Call UpdateP2PeInvoiceHeader()

    End Sub

    Public Sub UpdateP2PeInvoiceHeader()

        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateP2PeInvoiceHeader"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(m_order_header_id)))
            cmd.Parameters.Add(CreateParam("@InvoiceNumber", SqlDbType.VarChar, ParameterDirection.Input, CObj(m_invoice_no)))
            cmd.Parameters.Add(CreateParam("@InvoiceDate", SqlDbType.SmallDateTime, ParameterDirection.Input, CObj(m_invoice_date)))
            cmd.Parameters.Add(CreateParam("@InvoiceCost", SqlDbType.Money, ParameterDirection.Input, CObj(m_invoice_cost)))
            cmd.Parameters.Add(CreateParam("@InvoiceFreight", SqlDbType.Money, ParameterDirection.Input, CObj(m_invoice_freight)))
            cmd.Parameters.Add(CreateParam("@InvoiceQuantityDiscount", SqlDbType.Decimal, ParameterDirection.Input, 9, 2, CObj(Me.m_quantity_discount))) ' Line Item Discount
            cmd.Parameters.Add(CreateParam("@InvoiceDiscountType", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_discount_type))) ' Discount Type

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub















    ' Continue here for P2P Vendor Discrepancies processing.  Finish building
    ' GetP2PUnsentVendorDiscrepancies and UpdateP2PVendorDiscrepancySentDate. 
    ' -Ryan-, 2 July 2007.
    Public Shared Function GetP2PUnsentVendorDiscrepancies() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim UnsentVendorDiscrepancyRecord As UnsentVendorDiscrepancy

        ' Used in Procurement to Payment (P2P).  Returns a listing of values
        ' associated with a particular vendor.  
        ' Called from the "Order - Integrators - Send 850 EDI.dtsx" script.
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetP2PUnsentVendorDiscrepancies"

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows Then
                While dr.Read
                    UnsentVendorDiscrepancyRecord.VendorID = dr!VendorID
                    UnsentVendorDiscrepancyRecord.VendorEmail = dr!VendorEmail

                    ' Add the current record to the list
                    results.Add(UnsentVendorDiscrepancyRecord)
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Shared Sub UpdateP2PVendorDiscrepancySentDate(ByVal Vendor_ID As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            'Dim prm As System.Data.SqlClient.SqlParameter
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateP2PVendorDiscrepancySentDate"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@Vendor_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Vendor_ID)))

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

End Class
Public Class OrderSort
    Implements IComparer
    Dim CompType As enuOrderSortOrder
    Public Sub New(ByVal xCompType As enuOrderSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuOrderSortOrder.FKs
                Dim iVendorComp As Integer = CType(x, Order).Vendor_ID.CompareTo(CType(y, Order).Vendor_ID)
                Dim iReceiveLocationIDComp As Integer = CType(x, Order).ReceiveLocation_ID.CompareTo(CType(y, Order).ReceiveLocation_ID)
                Dim iTransferSubTeamComp As Integer = CType(x, Order).Transfer_SubTeam.CompareTo(CType(y, Order).Transfer_SubTeam)
                Dim iTransferToSubTeamComp As Integer = CType(x, Order).Transfer_To_SubTeam.CompareTo(CType(y, Order).Transfer_To_SubTeam)
                Dim iReturnOrderComp As Integer = CType(x, Order).Return_Order.CompareTo(CType(y, Order).Return_Order)
                If (iVendorComp = 0) And (iReceiveLocationIDComp = 0) And (iTransferSubTeamComp = 0) And (iTransferToSubTeamComp = 0) And (iReturnOrderComp = 0) Then
                    Compare = 0
                Else
                    If (iVendorComp < 0) Or (iReceiveLocationIDComp < 0) Or (iTransferSubTeamComp < 0) Or (iTransferToSubTeamComp < 0) Or (iReturnOrderComp < 0) Then
                        Compare = -1
                    Else
                        Compare = 1
                    End If
                End If
            Case enuOrderSortOrder.ID
                Compare = CType(x, Order).OrderHeader_ID.CompareTo(CType(y, Order).OrderHeader_ID)
        End Select
    End Function
End Class
Public Class OrderGetFKs
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim iVendorComp As Integer = CType(x, Order).Vendor_ID.CompareTo(CType(y, Order).Vendor_ID)
        Dim iReceiveLocationIDComp As Integer = CType(x, Order).ReceiveLocation_ID.CompareTo(CType(y, Order).ReceiveLocation_ID)
        Dim iTransferSubTeamComp As Integer = CType(x, Order).Transfer_SubTeam.CompareTo(CType(y, Order).Transfer_SubTeam)
        Dim iTransferToSubTeamComp As Integer = CType(x, Order).Transfer_To_SubTeam.CompareTo(CType(y, Order).Transfer_To_SubTeam)
        Dim iReturnOrderComp As Integer = CType(x, Order).Return_Order.CompareTo(CType(y, Order).Return_Order)
        If (iVendorComp = 0) And (iReceiveLocationIDComp = 0) And (iTransferSubTeamComp = 0) And (iTransferToSubTeamComp = 0) And (iReturnOrderComp = 0) Then
            Compare = 0
        Else
            If (iVendorComp < 0) Or (iReceiveLocationIDComp < 0) Or (iTransferSubTeamComp < 0) Or (iTransferToSubTeamComp < 0) Or (iReturnOrderComp < 0) Then
                Compare = -1
            Else
                Compare = 1
            End If
        End If
    End Function
End Class
Public Class OrderGetID
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, Order).OrderHeader_ID.CompareTo(CType(y, Order).OrderHeader_ID)
    End Function
End Class
Public Class OrderItem
    Inherits ItemCatalog.Item
    Dim m_AdjustedCost As Decimal
    Dim m_DiscountType As Integer
    Dim m_order_item_id As Long
    Dim m_order_unit As String
    Dim m_package_desc1 As Decimal
    Dim m_package_desc2 As Decimal
    Dim m_package_unit_id As Long
    Dim m_QuantityDiscount As Decimal
    Dim m_quantity_ordered As Decimal
    Dim m_quantity_unit As Long
    Dim m_landed_cost As Decimal
    Dim m_line_number As Integer
    Dim m_line_item_cost As Decimal
    Dim m_line_item_freight As Decimal
    Dim m_received_item_cost As Decimal
    Dim m_received_item_freight As Decimal
    Dim m_unit_cost As Decimal
    Dim m_unit_extcost As Decimal
    Dim m_markup_cost As Decimal
    Dim m_markup_percent As Decimal
    Dim m_cost As Decimal
    Dim m_cost_unit As Long
    Dim m_freight As Decimal
    Dim m_freight_unit As Long
    Dim m_quantity_received As Decimal
    Dim m_quantity_shipped As Decimal
    Dim m_total_weight As Decimal
    Dim m_date_received As DateTime
    Dim m_Units_Per_Pallet As Integer
    Dim m_credit_reason_id As Integer
    Dim m_vendor_item_id As String
    Dim m_item_unit As String
    Dim m_item_description As String
    Dim m_uom_unit_cost As Decimal
    Dim m_item_allowance_discount_amount As Decimal
    Dim m_ack_quantity_ordered As Decimal
    Dim m_ack_cost As Decimal
    Dim m_lot_no As String
    Dim m_invoice_quantity_shipped As Decimal
    Dim m_invoice_cost As Decimal
    Dim m_invoice_extended_cost As Decimal
    Dim m_invoice_extended_freight As Decimal
    Dim m_HandlingCharge As Decimal
    Dim m_comments As String
    Dim m_item_case_pack As Decimal
    Dim m_origin As String
    Dim m_country_of_processing As String
    Dim m_discountLabel As String

    Public Property AdjustedCost() As Decimal
        Get
            Return m_AdjustedCost
        End Get
        Set(ByVal Value As Decimal)
            m_AdjustedCost = Value
        End Set
    End Property
    Public Property OrderItem_ID() As Long
        Get
            Return m_order_item_id
        End Get
        Set(ByVal Value As Long)
            m_order_item_id = Value
        End Set
    End Property

    Public Property Lot_No() As String
        Get
            Return m_lot_no
        End Get
        Set(ByVal Value As String)
            m_lot_no = Value
        End Set
    End Property

    Public Property OrderUnit() As String
        Get
            Return m_order_unit
        End Get
        Set(ByVal Value As String)
            m_order_unit = Value
        End Set
    End Property

    Public Shadows Property Package_Desc1() As Decimal
        Get
            If m_package_desc1 > 0 Then
                Return m_package_desc1
            Else
                Return MyBase.Package_Desc1
            End If
        End Get
        Set(ByVal Value As Decimal)
            m_package_desc1 = Value
        End Set
    End Property
    Public Shadows Property Package_Desc2() As Decimal
        Get
            If m_package_desc2 > 0 Then
                Return m_package_desc2
            Else
                Return MyBase.Package_Desc2
            End If
        End Get
        Set(ByVal Value As Decimal)
            m_package_desc2 = Value
        End Set
    End Property
    Public Shadows Property Package_Unit_ID() As Long
        'NOTE: Property should exist in base class Item but it does not currently
        Get
            If m_package_unit_id > 0 Then
                Return m_package_unit_id
            Else
                'Return MyBase.Package_Unit_ID
                Return 0
            End If
        End Get
        Set(ByVal Value As Long)
            m_package_unit_id = Value
        End Set
    End Property
    Public Property QuantityOrdered() As Decimal
        Get
            Return m_quantity_ordered
        End Get
        Set(ByVal Value As Decimal)
            m_quantity_ordered = Value
        End Set
    End Property
    Public Property QuantityShipped() As Decimal
        Get
            Return m_quantity_shipped
        End Get
        Set(ByVal value As Decimal)
            m_quantity_shipped = value
        End Set
    End Property
    Public Property QuantityUnit() As Long
        Get
            Return m_quantity_unit
        End Get
        Set(ByVal Value As Long)
            m_quantity_unit = Value
        End Set
    End Property
    Public Property LandedCost() As Decimal
        Get
            Return m_landed_cost
        End Get
        Set(ByVal Value As Decimal)
            Value = m_landed_cost
        End Set
    End Property

    Public Property LineItemNumber() As Long
        Get
            Return m_line_number
        End Get
        Set(ByVal Value As Long)
            m_line_number = Value
        End Set
    End Property

    Public Property LineItemCost() As Decimal
        Get
            Return m_line_item_cost
        End Get
        Set(ByVal Value As Decimal)
            m_line_item_cost = Value
        End Set
    End Property
    Public Property LineItemFreight() As Decimal
        Get
            Return m_line_item_freight
        End Get
        Set(ByVal Value As Decimal)
            m_line_item_freight = Value
        End Set
    End Property
    Public Property UnitCost() As Decimal
        Get
            Return m_unit_cost
        End Get
        Set(ByVal Value As Decimal)
            m_unit_cost = Value
        End Set
    End Property
    Public Property UnitExtCost() As Decimal
        Get
            Return m_unit_extcost
        End Get
        Set(ByVal Value As Decimal)
            m_unit_extcost = Value
        End Set
    End Property
    Public Property MarkupCost() As Decimal
        Get
            Return m_markup_cost
        End Get
        Set(ByVal Value As Decimal)
            m_markup_cost = Value
        End Set
    End Property
    Public Property MarkupPercent() As Decimal
        Get
            Return m_markup_percent
        End Get
        Set(ByVal Value As Decimal)
            m_markup_percent = Value
        End Set
    End Property
    Public Property Cost() As Decimal
        Get
            Return m_cost
        End Get
        Set(ByVal Value As Decimal)
            m_cost = Value
        End Set
    End Property
    Public Property CostUnit() As Long
        Get
            Return m_cost_unit
        End Get
        Set(ByVal Value As Long)
            m_cost_unit = Value
        End Set
    End Property
    Public Property Freight() As Decimal
        Get
            Return m_freight
        End Get
        Set(ByVal Value As Decimal)
            m_freight = Value
        End Set
    End Property
    Public Property FreightUnit() As Long
        Get
            Return m_freight_unit
        End Get
        Set(ByVal Value As Long)
            m_freight_unit = Value
        End Set
    End Property
    Public Property QuantityReceived() As Decimal
        Get
            Return m_quantity_received
        End Get
        Set(ByVal Value As Decimal)
            m_quantity_received = Value
        End Set
    End Property
    Public Property Total_Weight() As Decimal
        Get
            Return m_total_weight
        End Get
        Set(ByVal Value As Decimal)
            m_total_weight = Value
        End Set
    End Property
    Public Property IsReceivedWeightRequired() As Boolean
        Get
            Return Me.IsSoldByWeight
        End Get
        Set(ByVal Value As Boolean)
            Me.IsSoldByWeight = Value
        End Set
    End Property
    Public Property DateReceived() As DateTime
        Get
            Return m_date_received
        End Get
        Set(ByVal Value As DateTime)
            m_date_received = Value
        End Set
    End Property
    Public Property ReceivedItemCost() As Decimal
        Get
            Return m_received_item_cost
        End Get
        Set(ByVal Value As Decimal)
            m_received_item_cost = Value
        End Set
    End Property
    Public Property ReceivedItemFreight() As Decimal
        Get
            Return m_received_item_freight
        End Get
        Set(ByVal Value As Decimal)
            m_received_item_freight = Value
        End Set
    End Property

    Public Property VendorItem_ID() As String
        Get
            Return m_vendor_item_id
        End Get
        Set(ByVal Value As String)
            m_vendor_item_id = Value
        End Set
    End Property

    Public Property ItemUnit() As String
        Get
            Return m_item_unit
        End Get
        Set(ByVal Value As String)
            m_item_unit = Value
        End Set
    End Property

    Public Property UnitOfMeasureCost() As Decimal
        Get
            Return m_uom_unit_cost
        End Get
        Set(ByVal Value As Decimal)
            m_uom_unit_cost = Value
        End Set
    End Property

    Public Property ItemDescription() As String
        Get
            Return m_item_description
        End Get
        Set(ByVal Value As String)
            m_item_description = Value
        End Set
    End Property

    Public Property ItemAllowanceDiscountAmount() As Decimal
        Get
            Return m_item_allowance_discount_amount
        End Get
        Set(ByVal Value As Decimal)
            m_item_allowance_discount_amount = Value
        End Set
    End Property

    Public Property AckQuantityOrdered() As Decimal
        Get
            Return m_ack_quantity_ordered
        End Get
        Set(ByVal Value As Decimal)
            m_ack_quantity_ordered = Value
        End Set
    End Property

    Public Property AckCost() As Decimal
        Get
            Return m_ack_cost
        End Get
        Set(ByVal Value As Decimal)
            m_ack_cost = Value
        End Set
    End Property

    Public Property InvoiceQuantityShipped() As Decimal
        Get
            Return m_invoice_quantity_shipped
        End Get
        Set(ByVal Value As Decimal)
            m_invoice_quantity_shipped = Value
        End Set
    End Property

    Public Property InvoiceCost() As Decimal
        Get
            Return m_invoice_cost
        End Get
        Set(ByVal Value As Decimal)
            m_invoice_cost = Value
        End Set
    End Property

    Public Property InvoiceExtendedCost() As Decimal
        Get
            Return m_invoice_extended_cost
        End Get
        Set(ByVal Value As Decimal)
            m_invoice_extended_cost = Value
        End Set
    End Property

    Public Property InvoiceExtendedFreight() As Decimal
        Get
            Return m_invoice_extended_freight
        End Get
        Set(ByVal Value As Decimal)
            m_invoice_extended_freight = Value
        End Set
    End Property

    Public Property QuantityDiscount() As Decimal
        Get
            Return m_QuantityDiscount
        End Get
        Set(ByVal Value As Decimal)
            m_QuantityDiscount = Value
        End Set
    End Property

    Public Property DiscountType() As Integer
        Get
            Return m_DiscountType
        End Get
        Set(ByVal Value As Integer)
            m_DiscountType = Value
        End Set
    End Property
    Public Property Comments() As String
        Get
            Return m_comments
        End Get
        Set(ByVal Value As String)
            m_comments = Value
        End Set
    End Property
    Public Property ItemCasePack() As Decimal
        Get
            Return m_item_case_pack
        End Get
        Set(ByVal Value As Decimal)
            m_item_case_pack = Value
        End Set
    End Property
    Public Property Origin() As String
        Get
            Return m_origin
        End Get
        Set(ByVal Value As String)
            m_origin = Value
        End Set
    End Property
    Public Property CountryOfProcessing() As String
        Get
            Return m_country_of_processing
        End Get
        Set(ByVal Value As String)
            m_country_of_processing = Value
        End Set
    End Property
    Public Property DiscountLabel() As String
        Get
            Return m_discountLabel
        End Get
        Set(ByVal Value As String)
            m_discountLabel = Value
        End Set
    End Property


    Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()
    End Sub
    Protected Friend Sub New(ByRef oItem As ItemCatalog.Item)
        MyBase.New(oItem)
    End Sub

    Protected Friend Sub New(ByVal lItem_Key As Long)
        'Set Prop Vals
        MyBase.New(lItem_Key)

    End Sub

    <Obsolete("InsertsOrderItem")> Protected Friend Sub New(ByVal lItem_Key As Long, ByVal dQuantityOrdered As Decimal, ByVal lOrderHeader_ID As Long, ByVal lVendor_ID As Long, ByVal lReceiveLocation_ID As Long, ByVal lCreditReason_ID As Long, Optional ByVal dPackSize As Decimal = 0)
        'Caller handles errors
        MyBase.New(lItem_Key)
    End Sub

    Protected Friend Sub Receive(ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal dtDate As DateTime, ByVal bCorrection As Boolean, Optional ByVal dPackSize As Decimal = 0, Optional ByVal UserID As Long = 0)

        logger.Info("# Call OrderItem.Receive()")
        logger.Info(vbTab & "(input) dQuantity: " & dQuantity & ", dWeight: " & dWeight & ",  dtDate: " & dtDate & ", bCorrection: " & bCorrection & ", dPackSize: " & dPackSize & ", UserID: " & UserID)

        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            If Me.IsSoldByWeight And dWeight = 0 And dQuantity > 0 Then Throw New ItemCatalog.Exception.OrderItem.ReceivedWeightMissingException

            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "ReceiveOrderItem3"

            With cmd.Parameters
                .Add(CreateParam("@OrderItem_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.OrderItem_ID)))
                .Add(CreateParam("@Datereceived", SqlDbType.DateTime, ParameterDirection.Input, CObj(dtDate)))
                Me.DateReceived = dtDate
                .Add(CreateParam("@Quantity", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(dQuantity)))
                Me.QuantityReceived = dQuantity
                .Add(CreateParam("@Package_Desc1", SqlDbType.Decimal, ParameterDirection.Input, 9, 4, CObj(IIf(dPackSize > 0, dPackSize, System.DBNull.Value))))
                If dPackSize > 0 Then Me.Package_Desc1 = dPackSize
                .Add(CreateParam("@Weight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(dWeight)))
                Me.Total_Weight = dWeight
                .Add(CreateParam("@RecvDiscrepancyReasonID", SqlDbType.Int, ParameterDirection.Input, System.DBNull.Value))
                .Add(CreateParam("@User_ID", SqlDbType.Int, ParameterDirection.Input, CObj(UserID)))
                .Add(CreateParam("@Correction", SqlDbType.Bit, ParameterDirection.Input, CObj(bCorrection)))

                '##########################################################################################################################
                '    These parameters do not exist in the V3 schema and are most likely from the Souths most current version. They
                '    will need to be addressed again in the future.
                '##########################################################################################################################
                '.Add(CreateParam("@InCost", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(System.DBNull.Value)))
                '.Add(CreateParam("@InFreight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(System.DBNull.Value)))

                .Add(CreateParam("@Cost", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
                .Add(CreateParam("@Freight", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
                .Add(CreateParam("@LineItemCost", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
                .Add(CreateParam("@LineItemFreight", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
                .Add(CreateParam("@receivedItemCost", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
                .Add(CreateParam("@receivedItemFreight", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            End With

            logger.Info(vbTab & " (Stored Proc) ReceiveOrderItem3")
            For Each i As SqlClient.SqlParameter In cmd.Parameters
                logger.Info(vbTab & " (Parameter) " & i.ParameterName & ": " & i.Value & " (" & i.SqlDbType.ToString() & ")")
            Next
            logger.Info(vbTab & " (Execute) ReceiveOrderItem3")
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

            logger.Info(vbTab & " (Return Value) Cost: " & cmd.Parameters("@Cost").Value)
            logger.Info(vbTab & " (Return Value) Freight: " & cmd.Parameters("@Freight").Value)
            logger.Info(vbTab & " (Return Value) LineItemCost: " & cmd.Parameters("@LineItemCost").Value)
            logger.Info(vbTab & " (Return Value) LineItemFreight: " & cmd.Parameters("@LineItemFreight").Value)
            logger.Info(vbTab & " (Return Value) ReceivedItemCost: " & cmd.Parameters("@ReceivedItemCost").Value)
            logger.Info(vbTab & " (Return Value) ReceivedItemFreight: " & cmd.Parameters("@ReceivedItemFreight").Value)

            Me.Cost = IIf(IsDBNull(cmd.Parameters("@Cost").Value), 0, cmd.Parameters("@Cost").Value)
            Me.Freight = IIf(IsDBNull(cmd.Parameters("@Freight").Value), 0, cmd.Parameters("@Freight").Value)
            Me.LineItemCost = IIf(IsDBNull(cmd.Parameters("@LineItemCost").Value), 0, cmd.Parameters("@LineItemCost").Value)
            Me.LineItemFreight = IIf(IsDBNull(cmd.Parameters("@LineItemFreight").Value), 0, cmd.Parameters("@LineItemFreight").Value)
            Me.ReceivedItemCost = IIf(IsDBNull(cmd.Parameters("@ReceivedItemCost").Value), 0, cmd.Parameters("@ReceivedItemCost").Value)
            Me.ReceivedItemFreight = IIf(IsDBNull(cmd.Parameters("@ReceivedItemFreight").Value), 0, cmd.Parameters("@ReceivedItemFreight").Value)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

    Protected Friend Function AddDB(ByVal parentOrder As Order, ByVal lCreditReason_ID As Long, Optional ByVal dPackSize As Decimal = 0)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim cmd2 As New System.Data.SqlClient.SqlCommand

        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            'Get Remaining Order Item Info
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "AutomaticOrderItemInfo"
            With cmd.Parameters
                .Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
                ' Me.Item_Key = lItem_Key
                .Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(parentOrder.OrderHeader_ID)))
                .Add(CreateParam("@Package_Desc1", SqlDbType.Decimal, ParameterDirection.Input, 9, 4, CObj(IIf(dPackSize > 0, dPackSize, System.DBNull.Value))))
            End With

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()

                Me.m_package_desc1 = CType(dr!Package_Desc1, Decimal)
                Me.m_package_desc2 = CType(dr!Package_Desc2, Decimal)
                Me.m_package_unit_id = CType(dr!Package_Unit_ID, Integer)
                Me.m_AdjustedCost = 0
                Me.m_QuantityDiscount = CType(dr!QuantityDiscount, Decimal)
                Me.m_DiscountType = CType(dr!DiscountType, Integer)
                Me.m_markup_percent = CType(dr!MarkupPercent, Decimal)

                Me.QuantityUnit = CType(dr!QuantityUnit, Long)

                Me.Cost = dr!Cost
                Me.CostUnit = CType(dr!CostUnit, Long)
                Me.LineItemCost = 0
                Me.Freight = dr!Freight
                Me.FreightUnit = CType(dr!FreightUnit, Long)
                Me.LineItemFreight = 0
                Me.LandedCost = 0
                Me.LineItemCost = 0
                Me.LineItemFreight = 0
                Me.UnitCost = 0
                Me.UnitExtCost = 0
                Me.MarkupCost = 0
                Me.m_HandlingCharge = CType(dr!HandlingCharge, Decimal)
            Else
                Throw New ApplicationException("AutomaticOrderItemInfo returned no information")
            End If

            cmd2.CommandType = CommandType.StoredProcedure
            cmd2.CommandText = "InsertOrderItemCredit2"
            With cmd2.Parameters
                .Add(CreateParam("@OrderHeader_ID", SqlDbType.Int, ParameterDirection.Input, CObj(parentOrder.OrderHeader_ID)))
                .Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
                .Add(CreateParam("@Units_Per_Pallet", SqlDbType.SmallInt, ParameterDirection.Input, CObj(CType(dr!Units_Per_Pallet, Int16))))
                .Add(CreateParam("@QuantityUnit", SqlDbType.Int, ParameterDirection.Input, CObj(Me.QuantityUnit)))
                .Add(CreateParam("@QuantityOrdered", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.QuantityOrdered)))
                .Add(CreateParam("@Cost", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.Cost)))
                .Add(CreateParam("@CostUnit", SqlDbType.Int, ParameterDirection.Input, CObj(Me.CostUnit)))
                .Add(CreateParam("@Handling", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(0)))
                .Add(CreateParam("@handlingUnit", SqlDbType.Int, ParameterDirection.Input, CObj(-1)))
                .Add(CreateParam("@Freight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.Freight)))
                .Add(CreateParam("@FreightUnit", SqlDbType.Int, ParameterDirection.Input, CObj(Me.FreightUnit)))
                .Add(CreateParam("@Adjustedcost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.m_AdjustedCost)))
                .Add(CreateParam("@QuantityDiscount", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_QuantityDiscount)))
                .Add(CreateParam("@DiscountType", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_DiscountType)))
                .Add(CreateParam("@LandedCost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.LandedCost)))
                .Add(CreateParam("@LineItemCost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.LineItemCost)))
                .Add(CreateParam("@LineItemFreight", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.LineItemFreight)))
                .Add(CreateParam("@LineItemHandling", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(0)))
                .Add(CreateParam("@UnitCost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.UnitCost)))
                .Add(CreateParam("@UnitExtCost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.UnitExtCost)))
                .Add(CreateParam("@Package_Desc1", SqlDbType.Decimal, ParameterDirection.Input, 9, 4, CObj(Me.Package_Desc1)))
                .Add(CreateParam("@Package_Desc2", SqlDbType.Decimal, ParameterDirection.Input, 9, 4, CObj(Me.Package_Desc2)))
                .Add(CreateParam("@Package_Unit_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Package_Unit_ID)))
                .Add(CreateParam("@Markuppercent", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_markup_percent)))
                .Add(CreateParam("@MarkupCost", SqlDbType.Decimal, ParameterDirection.Input, 19, 4, CObj(Me.MarkupCost)))
                .Add(CreateParam("@Retail_Unit_ID", SqlDbType.Int, ParameterDirection.Input, CObj(CType(dr!Retail_Unit_ID, Integer))))
                .Add(CreateParam("@CreditReason_ID", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(lCreditReason_ID = 0, System.DBNull.Value, lCreditReason_ID))))
                .Add(CreateParam("@HandlingCharge", SqlDbType.SmallMoney, ParameterDirection.Input, CObj(Me.m_HandlingCharge)))
                .Add(CreateParam("@NewOrderItem_ID", SqlDbType.Int, ParameterDirection.Output))
            End With

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd2, DataAccess.enuDBList.ItemCatalog)
            Me.OrderItem_ID = cmd2.Parameters("@NewOrderItem_ID").Value
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd2, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Sub UpdateP2PeInvoiceLineItem()

        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateP2PeInvoiceLineItem"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@OrderItem_ID", SqlDbType.Int, ParameterDirection.Input, CObj(m_order_item_id)))
            cmd.Parameters.Add(CreateParam("@InvoiceQuantityOrdered", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_quantity_ordered))) ' Quantity Ordered
            cmd.Parameters.Add(CreateParam("@Package_Desc1", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.Package_Desc1))) ' Pack
            cmd.Parameters.Add(CreateParam("@InvoiceCost", SqlDbType.Money, ParameterDirection.Input, CObj(Me.m_invoice_cost))) ' Line Item price
            cmd.Parameters.Add(CreateParam("@InvoiceQuantityShipped", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_invoice_quantity_shipped))) ' Quantity Shipped
            cmd.Parameters.Add(CreateParam("@InvoiceQuantityUnit", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_quantity_unit))) ' Quantity Unit
            cmd.Parameters.Add(CreateParam("@InvoiceShippedWeight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_total_weight))) ' Total Weight
            cmd.Parameters.Add(CreateParam("@InvoiceExtendedFreight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.InvoiceExtendedFreight))) ' Line Item Freight
            cmd.Parameters.Add(CreateParam("@InvoiceLotNo", SqlDbType.VarChar, ParameterDirection.Input, CObj(Me.m_lot_no))) ' Lot Number
            cmd.Parameters.Add(CreateParam("@InvoiceFreightCharge", SqlDbType.Money, ParameterDirection.Input, CObj(Me.m_freight))) ' Line Item Charge
            cmd.Parameters.Add(CreateParam("@InvoiceDiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.m_QuantityDiscount))) ' Line Item Discount
            cmd.Parameters.Add(CreateParam("@InvoiceDiscountType", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_DiscountType))) ' Discount Type
            cmd.Parameters.Add(CreateParam("@InvoiceExtendedCost", SqlDbType.Money, ParameterDirection.Input, CObj(m_invoice_extended_cost))) ' Extended Price

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub UpdateQuantityShipped()

        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateOrderItemQuantityShipped"
            cmd.Parameters.Clear()

            cmd.Parameters.Add(CreateParam("@OrderItem_ID", SqlDbType.Int, ParameterDirection.Input, CObj(m_order_item_id)))

            If IsDBNull(Me.QuantityShipped) Or Me.QuantityShipped = 0 Then
                cmd.Parameters.Add(CreateParam("@QuantityShipped", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(DBNull.Value)))
            Else
                cmd.Parameters.Add(CreateParam("@QuantityShipped", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Me.QuantityShipped)))
            End If

            ' Make the database call
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(CObj(cmd), DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub

End Class
Public Class OrderItemSort
    Implements IComparer
    Dim CompType As enuOrderItemSortOrder
    Public Sub New(ByVal xCompType As enuOrderItemSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuOrderItemSortOrder.ID
                Compare = CType(x, OrderItem).OrderItem_ID.CompareTo(CType(y, OrderItem).OrderItem_ID)
            Case enuOrderItemSortOrder.Item_Key
                Compare = CType(x, OrderItem).Item_Key.CompareTo(CType(y, OrderItem).Item_Key)
            Case enuOrderItemSortOrder.Identifier
                Compare = CType(x, OrderItem).Identifier.CompareTo(CType(y, OrderItem).Identifier)
            Case enuOrderItemSortOrder.IdentifierPackage_Desc1
                Compare = String.Compare(CType(x, OrderItem).Identifier & CType(x, OrderItem).Package_Desc1.ToString("000000000000000000.0000"), CType(y, OrderItem).Identifier & CType(y, OrderItem).Package_Desc1.ToString("000000000000000000.0000"))
        End Select
    End Function
End Class
Public Class OrderItemGetID
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, OrderItem).OrderItem_ID.CompareTo(CType(y, OrderItem).OrderItem_ID)
    End Function

End Class
Public Class OrderItemGetItem_Key
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, OrderItem).Item_Key.CompareTo(CType(y, OrderItem).Item_Key)
    End Function
End Class
Public Class OrderItemGetIdentifier
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, OrderItem).Identifier.CompareTo(CType(y, OrderItem).Identifier)
    End Function
End Class
Public Class OrderItemGetIdentifierPackage_Desc1
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = String.Compare(CType(x, OrderItem).Identifier & CType(x, OrderItem).Package_Desc1.ToString("000000000000000000.0000"), CType(y, OrderItem).Identifier & CType(y, OrderItem).Package_Desc1.ToString("000000000000000000.0000"))
    End Function
End Class
Public Class Cool4010OrderItem

    Public Sub AddCOOL4010OrderItem(ByVal ActionCode As String, ByVal ShippingDC As String, ByVal ShippingWarehouse As String, _
        ByVal ISRID As String, ByVal OutboundPalletID As String, ByVal ShipmentDate As String, ByVal ShipmentTime As String, _
        ByVal Product As String, ByVal ProductDetail As String, ByVal ProductDescription As String, ByVal ProductSize As String, _
        ByVal ShippedQuantity As String, ByVal InvoiceID As String, ByVal RecipientName As String, ByVal RecipientAddress1 As String, _
        ByVal RecipientAddress2 As String, ByVal RecipientCity As String, ByVal RecipientState As String, _
        ByVal RecipientZip As String, ByVal RecipientPhone As String, ByVal RecipientCountry As String, _
        ByVal RecipientContact As String, ByVal SupplierName As String, ByVal SupplierAddress1 As String, _
        ByVal SupplierAddress2 As String, ByVal SupplierCity As String, ByVal SupplierState As String, _
        ByVal SupplierZip As String, ByVal SupplierPhone As String, ByVal SupplierCountry As String, _
        ByVal SupplierContact As String, ByVal ReceiptID As String, ByVal ReceiptDate As String, _
        ByVal ReceiptQuantity As String, ByVal Type As String, ByVal ExclusivelyFrom As String, ByVal Processed As String, _
        ByVal Caught As String, ByVal Hatch As String, ByVal Raise As String, ByVal Harvest As String, ByVal LotNumber As String, _
        ByVal OutboundCarrierName As String, ByVal OutboundCarrierAddress1 As String, ByVal OutboundCarrierAddress2 As String, _
        ByVal OutboundCarrierCity As String, ByVal OutboundCarrierState As String, ByVal OutboundCarrierZip As String, _
        ByVal OutboundCarrierPhone As String, ByVal OutboundCarrierCountry As String, ByVal OutboundDriverFirst As String, _
        ByVal OutboundDriverInitial As String, ByVal OutboundDriverLast As String, ByVal OutboundTrailerOwner As String, ByVal OutboundTrailerNumber As String, _
        ByVal InboundCarrierName As String, ByVal InboundCarrierAddress1 As String, ByVal InboundCarrierAddress2 As String, _
        ByVal InboundCarrierCity As String, ByVal InboundCarrierState As String, ByVal InboundCarrierZip As String, _
        ByVal InboundCarrierPhone As String, ByVal InboundCarrierCountry As String, ByVal IIPSRefNo As String, ByVal IIPSRefSeq As String)
        '20090106 - Insert Order Item COOL Detail - Dave Stacey
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertOrderItemCOOL4010Detail"
            With cmd.Parameters
                .Add(CreateParam("@ActionCode", SqlDbType.Char, ParameterDirection.Input, ActionCode))
                .Add(CreateParam("@ShippingDC", SqlDbType.NChar, ParameterDirection.Input, ShippingDC))
                .Add(CreateParam("@ShippingWarehouse", SqlDbType.NChar, ParameterDirection.Input, ShippingWarehouse))
                .Add(CreateParam("@ISRID", SqlDbType.NChar, ParameterDirection.Input, ISRID))
                .Add(CreateParam("@OutboundPalletID", SqlDbType.NChar, ParameterDirection.Input, OutboundPalletID))
                .Add(CreateParam("@ShipmentDate", SqlDbType.NChar, ParameterDirection.Input, ShipmentDate))
                .Add(CreateParam("@ShipmentTime", SqlDbType.NChar, ParameterDirection.Input, ShipmentTime))
                .Add(CreateParam("@Product", SqlDbType.VarChar, ParameterDirection.Input, Product))
                .Add(CreateParam("@ProductDetail", SqlDbType.NChar, ParameterDirection.Input, ProductDetail))
                .Add(CreateParam("@ProductDescription", SqlDbType.NChar, ParameterDirection.Input, ProductDescription))
                .Add(CreateParam("@ProductSize", SqlDbType.NChar, ParameterDirection.Input, ProductSize))
                .Add(CreateParam("@ShippedQuantity", SqlDbType.NChar, ParameterDirection.Input, ShippedQuantity))
                .Add(CreateParam("@InvoiceID", SqlDbType.NChar, ParameterDirection.Input, InvoiceID))
                .Add(CreateParam("@RecipientName", SqlDbType.NChar, ParameterDirection.Input, RecipientName))
                .Add(CreateParam("@RecipientAddress1", SqlDbType.NChar, ParameterDirection.Input, RecipientAddress1))
                .Add(CreateParam("@RecipientAddress2", SqlDbType.NChar, ParameterDirection.Input, RecipientAddress2))
                .Add(CreateParam("@RecipientCity", SqlDbType.NChar, ParameterDirection.Input, RecipientCity))
                .Add(CreateParam("@RecipientState", SqlDbType.NChar, ParameterDirection.Input, RecipientState))
                .Add(CreateParam("@RecipientZip", SqlDbType.NChar, ParameterDirection.Input, RecipientZip))
                .Add(CreateParam("@RecipientPhone", SqlDbType.NChar, ParameterDirection.Input, RecipientPhone))
                .Add(CreateParam("@RecipientCountry", SqlDbType.NChar, ParameterDirection.Input, RecipientCountry))
                .Add(CreateParam("@RecipientContact", SqlDbType.NChar, ParameterDirection.Input, RecipientContact))
                .Add(CreateParam("@SupplierName", SqlDbType.NChar, ParameterDirection.Input, SupplierName))
                .Add(CreateParam("@SupplierAddress1", SqlDbType.NChar, ParameterDirection.Input, SupplierAddress1))
                .Add(CreateParam("@SupplierAddress2", SqlDbType.NChar, ParameterDirection.Input, SupplierAddress2))
                .Add(CreateParam("@SupplierCity", SqlDbType.NChar, ParameterDirection.Input, SupplierCity))
                .Add(CreateParam("@SupplierState", SqlDbType.NChar, ParameterDirection.Input, SupplierState))
                .Add(CreateParam("@SupplierZip", SqlDbType.NChar, ParameterDirection.Input, SupplierZip))
                .Add(CreateParam("@SupplierPhone", SqlDbType.NChar, ParameterDirection.Input, SupplierPhone))
                .Add(CreateParam("@SupplierCountry", SqlDbType.NChar, ParameterDirection.Input, SupplierCountry))
                .Add(CreateParam("@SupplierContact", SqlDbType.NChar, ParameterDirection.Input, SupplierContact))
                .Add(CreateParam("@ReceiptID", SqlDbType.NChar, ParameterDirection.Input, ReceiptID))
                .Add(CreateParam("@ReceiptDate", SqlDbType.NChar, ParameterDirection.Input, ReceiptDate))
                .Add(CreateParam("@ReceiptQuantity", SqlDbType.NChar, ParameterDirection.Input, ReceiptQuantity))
                .Add(CreateParam("@Type", SqlDbType.NChar, ParameterDirection.Input, Type))
                .Add(CreateParam("@ExclusivelyFrom", SqlDbType.NChar, ParameterDirection.Input, ExclusivelyFrom))
                .Add(CreateParam("@Processed", SqlDbType.NChar, ParameterDirection.Input, Processed))
                .Add(CreateParam("@Caught", SqlDbType.NChar, ParameterDirection.Input, Caught))
                .Add(CreateParam("@Hatch", SqlDbType.NChar, ParameterDirection.Input, Hatch))
                .Add(CreateParam("@Raise", SqlDbType.NChar, ParameterDirection.Input, Raise))
                .Add(CreateParam("@Harvest", SqlDbType.NChar, ParameterDirection.Input, Harvest))
                .Add(CreateParam("@LotNumber", SqlDbType.NChar, ParameterDirection.Input, LotNumber))
                .Add(CreateParam("@OutboundCarrierName", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierName))
                .Add(CreateParam("@OutboundCarrierAddress1", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierAddress1))
                .Add(CreateParam("@OutboundCarrierAddress2", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierAddress2))
                .Add(CreateParam("@OutboundCarrierCity", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierCity))
                .Add(CreateParam("@OutboundCarrierState", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierState))
                .Add(CreateParam("@OutboundCarrierZip", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierZip))
                .Add(CreateParam("@OutboundCarrierPhone", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierPhone))
                .Add(CreateParam("@OutboundCarrierCountry", SqlDbType.NChar, ParameterDirection.Input, OutboundCarrierCountry))
                .Add(CreateParam("@OutboundDriverFirst", SqlDbType.NChar, ParameterDirection.Input, OutboundDriverFirst))
                .Add(CreateParam("@OutboundDriverInitial", SqlDbType.NChar, ParameterDirection.Input, OutboundDriverInitial))
                .Add(CreateParam("@OutboundDriverLast", SqlDbType.NChar, ParameterDirection.Input, OutboundDriverLast))
                .Add(CreateParam("@OutboundTrailerOwner", SqlDbType.NChar, ParameterDirection.Input, OutboundTrailerOwner))
                .Add(CreateParam("@OutboundTrailerNumber", SqlDbType.NChar, ParameterDirection.Input, OutboundTrailerNumber))
                .Add(CreateParam("@InboundCarrierName", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierName))
                .Add(CreateParam("@InboundCarrierAddress1", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierAddress1))
                .Add(CreateParam("@InboundCarrierAddress2", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierAddress2))
                .Add(CreateParam("@InboundCarrierCity", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierCity))
                .Add(CreateParam("@InboundCarrierState", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierState))
                .Add(CreateParam("@InboundCarrierZip", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierZip))
                .Add(CreateParam("@InboundCarrierPhone", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierPhone))
                .Add(CreateParam("@InboundCarrierCountry", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierCountry))
                .Add(CreateParam("@IIPSRefNumber", SqlDbType.NChar, ParameterDirection.Input, IIPSRefNo))
                .Add(CreateParam("@IIPSRefSequence", SqlDbType.NChar, ParameterDirection.Input, IIPSRefSeq))
            End With
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
End Class
Public Class Cool4020OrderItem

    Public Sub AddCOOL4020OrderItem(ByVal ActionCode As String, ByVal ReceivingDC As String, ByVal ReceivingWarehouse As String, _
        ByVal IPSID As String, ByVal Product As String, ByVal ProductDetail As String, ByVal ProductDescription As String, ByVal ProductSize As String, _
        ByVal SupplierName As String, ByVal SupplierAddress1 As String, _
        ByVal SupplierAddress2 As String, ByVal SupplierCity As String, ByVal SupplierState As String, _
        ByVal SupplierZip As String, ByVal SupplierPhone As String, ByVal SupplierCountry As String, _
        ByVal SupplierContact As String, ByVal EXEReceiptID As String, ByVal HostPOID As String, ByVal ReceiptDate As String, _
        ByVal ReceiptQuantity As String, ByVal LotNumber As String, _
        ByVal InboundCarrierName As String, ByVal InboundCarrierAddress1 As String, ByVal InboundCarrierAddress2 As String, _
        ByVal InboundCarrierCity As String, ByVal InboundCarrierState As String, ByVal InboundCarrierZip As String, _
        ByVal InboundCarrierPhone As String, ByVal InboundCarrierCountry As String, ByVal IIPSRefNo As String, ByVal IIPSRefSeq As String)
        '20090106 - Insert Order Item COOL Detail - Dave Stacey
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertOrderItemCool4020Detail"
            With cmd.Parameters
                .Add(CreateParam("@ActionCode", SqlDbType.Char, ParameterDirection.Input, ActionCode))
                .Add(CreateParam("@ReceivingDC", SqlDbType.NChar, ParameterDirection.Input, ReceivingDC))
                .Add(CreateParam("@ReceivingWarehouse", SqlDbType.NChar, ParameterDirection.Input, ReceivingWarehouse))
                .Add(CreateParam("@IPSID", SqlDbType.NChar, ParameterDirection.Input, IPSID))
                .Add(CreateParam("@Product", SqlDbType.NChar, ParameterDirection.Input, Product))
                .Add(CreateParam("@ProductDetail", SqlDbType.NChar, ParameterDirection.Input, ProductDetail))
                .Add(CreateParam("@ProductDescription", SqlDbType.NChar, ParameterDirection.Input, ProductDescription))
                .Add(CreateParam("@ProductSize", SqlDbType.NChar, ParameterDirection.Input, ProductSize))
                .Add(CreateParam("@SupplierName", SqlDbType.NChar, ParameterDirection.Input, SupplierName))
                .Add(CreateParam("@SupplierAddress1", SqlDbType.NChar, ParameterDirection.Input, SupplierAddress1))
                .Add(CreateParam("@SupplierAddress2", SqlDbType.NChar, ParameterDirection.Input, SupplierAddress2))
                .Add(CreateParam("@SupplierCity", SqlDbType.NChar, ParameterDirection.Input, SupplierCity))
                .Add(CreateParam("@SupplierState", SqlDbType.NChar, ParameterDirection.Input, SupplierState))
                .Add(CreateParam("@SupplierZip", SqlDbType.NChar, ParameterDirection.Input, SupplierZip))
                .Add(CreateParam("@SupplierPhone", SqlDbType.NChar, ParameterDirection.Input, SupplierPhone))
                .Add(CreateParam("@SupplierCountry", SqlDbType.NChar, ParameterDirection.Input, SupplierCountry))
                .Add(CreateParam("@SupplierContact", SqlDbType.NChar, ParameterDirection.Input, SupplierContact))
                .Add(CreateParam("@EXEReceiptID", SqlDbType.NChar, ParameterDirection.Input, EXEReceiptID))
                .Add(CreateParam("@HostPOID", SqlDbType.NChar, ParameterDirection.Input, HostPOID))
                .Add(CreateParam("@ReceiptDate", SqlDbType.NChar, ParameterDirection.Input, ReceiptDate))
                .Add(CreateParam("@ReceiptQuantity", SqlDbType.NChar, ParameterDirection.Input, ReceiptQuantity))
                .Add(CreateParam("@LotNumber", SqlDbType.NChar, ParameterDirection.Input, LotNumber))
                .Add(CreateParam("@InboundCarrierName", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierName))
                .Add(CreateParam("@InboundCarrierAddress1", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierAddress1))
                .Add(CreateParam("@InboundCarrierAddress2", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierAddress2))
                .Add(CreateParam("@InboundCarrierCity", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierCity))
                .Add(CreateParam("@InboundCarrierState", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierState))
                .Add(CreateParam("@InboundCarrierZip", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierZip))
                .Add(CreateParam("@InboundCarrierPhone", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierPhone))
                .Add(CreateParam("@InboundCarrierCountry", SqlDbType.NChar, ParameterDirection.Input, InboundCarrierCountry))
                .Add(CreateParam("@IIPSRefNo", SqlDbType.NChar, ParameterDirection.Input, IIPSRefNo))
                .Add(CreateParam("@IIPSRefSeq", SqlDbType.NChar, ParameterDirection.Input, IIPSRefSeq))
            End With
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
End Class
