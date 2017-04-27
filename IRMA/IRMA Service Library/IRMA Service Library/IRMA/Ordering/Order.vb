Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Configuration
Imports System.Security.SecurityElement


Namespace IRMA
    <DataContract()>
    Public Class Order
        <DataMember()>
        Public Property OrderHeader_ID As Integer
        <DataMember()>
        Public Property Temperature As Integer
        <DataMember()>
        Public Property Accounting_In_DateStamp As DateTime
        <DataMember()>
        Public Property CompanyName As String
        <DataMember(isrequired:=True)>
        Public Property CreatedByName As String
        <DataMember()>
        Public Property SubTeam_Name As String
        <DataMember()>
        Public Property SubteamInvoiceNumber As Integer
        <DataMember()>
        Public Property Transfer_To_SubTeamName As String
        <DataMember()>
        Public Property OrderDate As DateTime
        <DataMember()>
        Public Property CloseDate As DateTime
        <DataMember()>
        Public Property SentDate As DateTime
        <DataMember(isrequired:=True)>
        Public Property Expected_Date As DateTime
        <DataMember()>
        Public Property InvoiceDate As DateTime
        <DataMember()>
        Public Property UploadedDate As DateTime
        <DataMember()>
        Public Property ApprovedDate As DateTime
        <DataMember(isrequired:=True)>
        Public Property OrderType_Id As Integer
        <DataMember(isrequired:=True)>
        Public Property ProductType_ID As Integer
        <DataMember(isrequired:=True)>
        Public Property CreatedBy As Integer
        <DataMember(isrequired:=True)>
        Public Property Transfer_SubTeam As Integer
        <DataMember(isrequired:=True)>
        Public Property Transfer_To_SubTeam As Integer
        <DataMember(isrequired:=True)>
        Public Property Vendor_ID As Integer
        <DataMember()>
        Public Property Vendor_Store_No As Integer
        <DataMember(isrequired:=True)>
        Public Property PurchaseLocation_ID As Integer
        <DataMember(isrequired:=True)>
        Public Property ReceiveLocation_ID As Integer
        <DataMember()>
        Public Property Fax_Order As Boolean
        <DataMember()>
        Public Property Email_Order As Boolean
        <DataMember()>
        Public Property Electronic_Order As Boolean
        <DataMember()>
        Public Property Sent As Boolean
        <DataMember(isrequired:=True)>
        Public Property Return_Order As Boolean
        <DataMember()>
        Public Property OriginalCloseDate As DateTime
        <DataMember()>
        Public Property User_ID As Integer
        <DataMember()>
        Public Property InvoiceNumber As String
        <DataMember()>
        Public Property ReturnOrder_ID As Integer
        <DataMember()>
        Public Property OverrideTransmissionMethod As Boolean
        <DataMember()>
        Public Property isDropShipment As Boolean
        <DataMember()>
        Public Property OriginalOrder_ID As Integer
        <DataMember()>
        Public Property Store_No As Integer
        <DataMember()>
        Public Property POTransmissionTypeID As Integer
        <DataMember()>
        Public Property WFM_Store As Boolean
        <DataMember()>
        Public Property HFM_Store As Boolean
        <DataMember()>
        Public Property Store_Vend As Boolean
        <DataMember()>
        Public Property RecvLog_No As Integer
        <DataMember()>
        Public Property WarehouseSent As Boolean
        <DataMember()>
        Public Property WarehouseSentDate As DateTime
        <DataMember()>
        Public Property EXEWarehouse As Integer
        <DataMember()>
        Public Property From_SubTeam_Unrestricted As Integer
        <DataMember()>
        Public Property To_SubTeam_Unrestricted As Integer
        <DataMember()>
        Public Property ItemsReceived As Integer
        <DataMember()>
        Public Property OrderEnd As String
        <DataMember()>
        Public Property CurrSysTime As DateTime
        <DataMember()>
        Public Property Distribution_Center As Boolean
        <DataMember()>
        Public Property ReceivingStore_Distribution_Center As Boolean
        <DataMember()>
        Public Property Manufacturer As Boolean
        <DataMember()>
        Public Property WFM As Boolean
        <DataMember()>
        Public Property IsEXEDistributed As Boolean
        <DataMember()>
        Public Property InvoiceAmount As Decimal
        <DataMember()>
        Public Property InvoiceCost As Decimal
        <DataMember()>
        Public Property InvoiceFreight As Decimal
        <DataMember()>
        Public Property ClosedByUserName As String
        <DataMember()>
        Public Property ApprovedByUserName As String
        <DataMember()>
        Public Property Freight3Party_OrderCost As Decimal
        <DataMember()>
        Public Property PSVendorID As String
        <DataMember()>
        Public Property ShipToStoreNo As Integer
        <DataMember()>
        Public Property BuyerName As String
        <DataMember()>
        Public Property BuyerEmail As String
        <DataMember()>
        Public Property Notes As String
        <DataMember()>
        Public Property DiscountAmount As Decimal
        <DataMember()>
        Public Property AllowanceDiscountAmount As Decimal
        <DataMember()>
        Public Property Store_Phone As String
        <DataMember()>
        Public Property QuantityDiscount As Decimal
        <DataMember()>
        Public Property DiscountType As Integer
        <DataMember()>
        Public Property StoreCompanyName As String
        <DataMember()>
        Public Property ShipToStoreCompanyName As String
        <DataMember()>
        Public Property IsVendorExternal As Boolean
        <DataMember()>
        Public Property SupplyType_SubTeamName As String
        <DataMember()>
        Public Property AccountingUploadDate As DateTime
        <DataMember()>
        Public Property OrderedCost As Decimal
        <DataMember()>
        Public Property AdjustedReceivedCost As Decimal
        <DataMember()>
        Public Property UploadedCost As Decimal
        <DataMember()>
        Public Property WarehouseCancelled As DateTime
        <DataMember()>
        Public Property PayByAgreedCost As Boolean
        <DataMember()>
        Public Property TotalHandlingCharge As Decimal
        <DataMember()>
        Public Property POCostDate As DateTime
        <DataMember()>
        Public Property EinvoiceID As Integer
        <DataMember()>
        Public Property FromQueue As Boolean
        <DataMember(isrequired:=True)>
        Public Property SupplyTransferToSubTeam As Integer
        <DataMember()>
        Public Property DSDOrder As Boolean
        <DataMember()>
        Public Property PSLocationCode As String
        <DataMember()>
        Public Property PSAddressSequence As String
        <DataMember()>
        Public Property VendorDocID As String
        <DataMember()>
        Public Property VendorDocDate As Date
        <DataMember()>
        Public Property CurrencyID As Integer
        <DataMember()>
        Public Property RefuseReceivingReasonID As Integer
        <DataMember()>
        Public Property EinvoiceRequired As Boolean
        <DataMember()>
        Public Property OrderFreight As Decimal
        <DataMember()>
        Public Property PartialShipment As Boolean
        <DataMember()>
        Public Property DeletedOrder As Boolean
        <DataMember(isrequired:=True)>
        Public Property OrderItems As List(Of OrderItem)
        <DataMember()>
        Public Property ResultObject As Result

        Public Sub New()

        End Sub

        Public Sub New(ByVal OrderHeader_ID)
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As New DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_Id"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CreatedBy"
                currentParam.Value = 0
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                dt = factory.GetStoredProcedureDataTable("GetOrderInfo", paramList)

                If dt.Rows.Count > 0 Then
                    Me.OrderHeader_ID = dt.Rows(0).Item("OrderHeader_ID")
                    Me.Temperature = NotNull(dt.Rows(0).Item("Temperature"), Nothing)
                    Me.Accounting_In_DateStamp = NotNull(dt.Rows(0).Item("Accounting_In_DateStamp"), Nothing)
                    Me.CompanyName = dt.Rows(0).Item("CompanyName")
                    Me.CreatedByName = dt.Rows(0).Item("CreatedByName")
                    Me.SubTeam_Name = NotNull(dt.Rows(0).Item("SubTeam_Name"), Nothing)
                    Me.Transfer_To_SubTeamName = NotNull(dt.Rows(0).Item("Transfer_To_SubTeamName"), Nothing)
                    Me.OrderDate = dt.Rows(0).Item("OrderDate")
                    Me.CloseDate = NotNull(dt.Rows(0).Item("CloseDate"), Nothing)
                    Me.SentDate = NotNull(dt.Rows(0).Item("SentDate"), Nothing)
                    Me.Expected_Date = NotNull(dt.Rows(0).Item("Expected_Date"), Nothing)
                    Me.InvoiceDate = NotNull(dt.Rows(0).Item("InvoiceDate"), Nothing)
                    Me.UploadedDate = NotNull(dt.Rows(0).Item("UploadedDate"), Nothing)
                    Me.ApprovedDate = NotNull(dt.Rows(0).Item("ApprovedDate"), Nothing)
                    Me.OrderType_Id = dt.Rows(0).Item("OrderType_Id")
                    Me.ProductType_ID = dt.Rows(0).Item("ProductType_ID")
                    Me.CreatedBy = NotNull(dt.Rows(0).Item("CreatedBy"), Nothing)
                    Me.Transfer_SubTeam = NotNull(dt.Rows(0).Item("Transfer_SubTeam"), Nothing)
                    Me.Transfer_To_SubTeam = dt.Rows(0).Item("Transfer_To_SubTeam")
                    Me.Vendor_ID = dt.Rows(0).Item("Vendor_ID")
                    Me.Vendor_Store_No = dt.Rows(0).Item("Vendor_Store_No")
                    Me.PurchaseLocation_ID = dt.Rows(0).Item("PurchaseLocation_ID")
                    Me.ReceiveLocation_ID = dt.Rows(0).Item("ReceiveLocation_ID")
                    Me.Fax_Order = NotNull(dt.Rows(0).Item("Fax_Order"), False)
                    Me.Email_Order = NotNull(dt.Rows(0).Item("Email_Order"), False)
                    Me.Electronic_Order = NotNull(dt.Rows(0).Item("Electronic_Order"), False)
                    Me.Sent = NotNull(dt.Rows(0).Item("Sent"), False)
                    Me.Return_Order = NotNull(dt.Rows(0).Item("Return_Order"), False)
                    Me.OriginalCloseDate = NotNull(dt.Rows(0).Item("OriginalCloseDate"), Nothing)
                    Me.User_ID = NotNull(dt.Rows(0).Item("User_ID"), -1)
                    Me.InvoiceNumber = NotNull(dt.Rows(0).Item("InvoiceNumber"), Nothing)
                    Me.ReturnOrder_ID = NotNull(dt.Rows(0).Item("ReturnOrder_ID"), Nothing)
                    Me.OverrideTransmissionMethod = NotNull(dt.Rows(0).Item("OverrideTransmissionMethod"), False)
                    Me.isDropShipment = NotNull(dt.Rows(0).Item("isDropShipment"), False)
                    Me.OriginalOrder_ID = NotNull(dt.Rows(0).Item("OriginalOrder_ID"), Nothing)
                    Me.Store_No = NotNull(dt.Rows(0).Item("Store_No"), Nothing)
                    Me.POTransmissionTypeID = NotNull(dt.Rows(0).Item("POTransmissionTypeID"), Nothing)
                    Me.WFM_Store = dt.Rows(0).Item("WFM_Store")
                    Me.HFM_Store = dt.Rows(0).Item("HFM_Store")
                    Me.Store_Vend = dt.Rows(0).Item("Store_Vend")
                    Me.RecvLog_No = NotNull(dt.Rows(0).Item("RecvLog_No"), Nothing)
                    Me.WarehouseSent = NotNull(dt.Rows(0).Item("WarehouseSent"), False)
                    Me.WarehouseSentDate = NotNull(dt.Rows(0).Item("WarehouseSentDate"), Nothing)
                    Me.EXEWarehouse = NotNull(dt.Rows(0).Item("EXEWarehouse"), Nothing)
                    Me.From_SubTeam_Unrestricted = dt.Rows(0).Item("From_SubTeam_Unrestricted")
                    Me.To_SubTeam_Unrestricted = dt.Rows(0).Item("To_SubTeam_Unrestricted")
                    Me.ItemsReceived = dt.Rows(0).Item("ItemsReceived")
                    Me.OrderEnd = dt.Rows(0).Item("OrderEnd")
                    Me.CurrSysTime = dt.Rows(0).Item("CurrSysTime")
                    Me.Distribution_Center = dt.Rows(0).Item("Distribution_Center")
                    Me.ReceivingStore_Distribution_Center = dt.Rows(0).Item("ReceivingStore_Distribution_Center")
                    Me.Manufacturer = dt.Rows(0).Item("Manufacturer")
                    Me.WFM = dt.Rows(0).Item("WFM")
                    Me.IsEXEDistributed = dt.Rows(0).Item("IsEXEDistributed")
                    Me.InvoiceAmount = dt.Rows(0).Item("InvoiceAmount")
                    Me.ClosedByUserName = dt.Rows(0).Item("ClosedByUserName")
                    Me.ApprovedByUserName = dt.Rows(0).Item("ApprovedByUserName")
                    Me.Freight3Party_OrderCost = dt.Rows(0).Item("Freight3Party_OrderCost")
                    Me.PSVendorID = NotNull(dt.Rows(0).Item("PSVendorID"), Nothing)
                    Me.ShipToStoreNo = NotNull(dt.Rows(0).Item("ShipToStoreNo"), Nothing)
                    Me.BuyerName = dt.Rows(0).Item("BuyerName")
                    Me.BuyerEmail = dt.Rows(0).Item("BuyerEmail")
                    Me.Notes = dt.Rows(0).Item("Notes")
                    Me.DiscountAmount = dt.Rows(0).Item("DiscountAmount")
                    Me.AllowanceDiscountAmount = dt.Rows(0).Item("AllowanceDiscountAmount")
                    Me.Store_Phone = dt.Rows(0).Item("Store_Phone")
                    Me.QuantityDiscount = dt.Rows(0).Item("QuantityDiscount")
                    Me.DiscountType = dt.Rows(0).Item("DiscountType")
                    Me.StoreCompanyName = dt.Rows(0).Item("StoreCompanyName")
                    Me.ShipToStoreCompanyName = NotNull(dt.Rows(0).Item("ShipToStoreCompanyName"), Nothing)
                    Me.IsVendorExternal = dt.Rows(0).Item("IsVendorExternal")
                    Me.SupplyType_SubTeamName = NotNull(dt.Rows(0).Item("SupplyType_SubTeamName"), Nothing)
                    Me.AccountingUploadDate = NotNull(dt.Rows(0).Item("AccountingUploadDate"), Nothing)
                    Me.OrderedCost = NotNull(dt.Rows(0).Item("OrderedCost"), Nothing)
                    Me.AdjustedReceivedCost = NotNull(dt.Rows(0).Item("AdjustedReceivedCost"), Nothing)
                    Me.UploadedCost = dt.Rows(0).Item("UploadedCost")
                    Me.WarehouseCancelled = NotNull(dt.Rows(0).Item("WarehouseCancelled"), Nothing)
                    Me.PayByAgreedCost = NotNull(dt.Rows(0).Item("PayByAgreedCost"), Nothing)
                    Me.TotalHandlingCharge = dt.Rows(0).Item("TotalHandlingCharge")
                    Me.POCostDate = NotNull(dt.Rows(0).Item("POCostDate"), Nothing)
                    Me.EinvoiceID = NotNull(dt.Rows(0).Item("eInvoice_id"), Nothing)
                    Me.VendorDocID = NotNull(dt.Rows(0).Item("VendorDoc_ID"), Nothing)
                    Me.VendorDocDate = NotNull(dt.Rows(0).Item("VendorDocDate"), Nothing)
                    Me.CurrencyID = NotNull(dt.Rows(0).Item("CurrencyID"), Nothing)
                    Me.RefuseReceivingReasonID = NotNull(dt.Rows(0).Item("RefuseReceivingReasonID"), Nothing)
                    Me.EinvoiceRequired = dt.Rows(0).Item("EinvoiceRequired")
                    Me.InvoiceCost = NotNull(dt.Rows(0).Item("InvoiceCost"), Nothing)
                    Me.InvoiceFreight = NotNull(dt.Rows(0).Item("InvoiceFreight"), Nothing)
                    Me.OrderFreight = NotNull(dt.Rows(0).Item("OrderFreight"), 0)
                    Me.DeletedOrder = dt.Rows(0).Item("IsDeleted")
                    Me.PartialShipment = NotNull(dt.Rows(0).Item("PartialShipment"), False)
                End If

                paramList.RemoveAt(1)

                Dim dtOrderItem As New DataTable
                Dim oiList As New List(Of OrderItem)

                dtOrderItem = factory.GetStoredProcedureDataTable("GetOrderItems", paramList)

                If Not dtOrderItem Is Nothing Then
                    For Each dr As DataRow In dtOrderItem.Rows
                        Dim oi As New OrderItem
                        oi.OrderItem_ID = dr("OrderItem_ID")
                        oi.Item_Key = dr("Item_Key")
                        oi.Identifier = dr("Identifier")
                        oi.Package_Desc1 = dr("Package_Desc1")
                        oi.Package_Desc2 = dr("Package_Desc2")
                        oi.QuantityUnit = dr("QuantityUnit")
                        oi.QuantityOrdered = dr("QuantityOrdered")
                        oi.QuantityReceived = NotNull(dr("QuantityReceived"), Nothing)
                        oi.Total_Weight = dr("Total_Weight")
                        oi.IsReceivedWeightRequired = dr("IsReceivedWeightRequired")
                        oi.VendorItemID = dr("VendorItemID")
                        oi.Item_Description = dr("Item_Description")
                        oi.LineItemCost = dr("LineItemCost")
                        oi.LineItemFreight = dr("LineItemFreight")
                        oi.UOMUnitCost = dr("UOMUnitCost")
                        oi.LineNumber = dr("LineNumber")
                        oi.ItemAllowanceDiscountAmount = dr("ItemAllowanceDiscountAmount")
                        oi.QuantityDiscount = dr("QuantityDiscount")
                        oi.DiscountType = dr("DiscountType")
                        oi.CostUnit = NotNull(dr("CostUnit"), Nothing)
                        oi.UOMAdjustedUnitCost = dr("UOMAdjustedUnitCost")
                        oi.QuantityShipped = NotNull(dr("QuantityShipped"), Nothing)
                        oi.CatchweightRequired = dr("CatchweightRequired")
                        oi.PackageUnitAbbr = dr("PackageUnitAbbr")
                        oi.OrderUOMAbbr = dr("OrderUOMAbbr")
                        oi.eInvoiceQuantity = NotNull(dr("eInvoiceQuantity"), Nothing)
                        oi.eInvoiceWeight = NotNull(dr("eInvoiceWeight"), Nothing)
                        oi.eInvoiceQuantityUnit = NotNull(dr("InvoiceQuantityUnitName"), Nothing)
                        oi.ReceivingDiscrepancyReasonCodeID = NotNull(dr("ReceivingDiscrepancyReasonCodeID"), Nothing)
                        oiList.Add(oi)
                    Next

                    Me.OrderItems = oiList
                End If

            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Sub

        Public Function GetOrderItem(ByVal lItem_Key As Long) As OrderItem
            If Not (Me.OrderItems Is Nothing) Then
                Dim oiQuery = From oi In OrderItems _
                              Where oi.Item_Key = lItem_Key _
                              Select New OrderItem _
                              With { _
                                    .OrderItem_ID = oi.OrderItem_ID, _
                                    .Item_Key = oi.Item_Key, _
                                    .Identifier = oi.Identifier, _
                                    .Package_Desc1 = oi.Package_Desc1, _
                                    .Package_Desc2 = oi.Package_Desc2, _
                                    .QuantityUnit = oi.QuantityUnit, _
                                    .QuantityOrdered = oi.QuantityOrdered, _
                                    .QuantityReceived = oi.QuantityReceived, _
                                    .Total_Weight = oi.Total_Weight, _
                                    .IsReceivedWeightRequired = oi.IsReceivedWeightRequired, _
                                    .VendorItemID = oi.VendorItemID, _
                                    .Item_Description = oi.Item_Description, _
                                    .LineItemCost = oi.LineItemCost, _
                                    .LineItemFreight = oi.LineItemFreight, _
                                    .UOMUnitCost = oi.UOMUnitCost, _
                                    .LineNumber = oi.LineNumber, _
                                    .ItemAllowanceDiscountAmount = oi.ItemAllowanceDiscountAmount, _
                                    .QuantityDiscount = oi.QuantityDiscount, _
                                    .DiscountType = oi.DiscountType, _
                                    .CostUnit = oi.CostUnit, _
                                    .UOMAdjustedUnitCost = oi.UOMAdjustedUnitCost, _
                                    .QuantityShipped = oi.QuantityShipped, _
                                    .CatchweightRequired = oi.CatchweightRequired, _
                                    .eInvoiceQuantity = oi.eInvoiceQuantity, _
                                    .ReceivingDiscrepancyReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                                    }
                Return oiQuery.First
            End If
            Return Nothing
        End Function

        Public Function GetOrderItem(ByVal sIdentifier As String) As OrderItem
            If Not (Me.OrderItems Is Nothing) Then
                Dim oiQuery = From oi In OrderItems _
                              Where oi.Identifier = sIdentifier _
                              Select New OrderItem _
                              With { _
                                    .OrderItem_ID = oi.OrderItem_ID, _
                                    .Item_Key = oi.Item_Key, _
                                    .Identifier = oi.Identifier, _
                                    .Package_Desc1 = oi.Package_Desc1, _
                                    .Package_Desc2 = oi.Package_Desc2, _
                                    .QuantityUnit = oi.QuantityUnit, _
                                    .QuantityOrdered = oi.QuantityOrdered, _
                                    .QuantityReceived = oi.QuantityReceived, _
                                    .Total_Weight = oi.Total_Weight, _
                                    .IsReceivedWeightRequired = oi.IsReceivedWeightRequired, _
                                    .VendorItemID = oi.VendorItemID, _
                                    .Item_Description = oi.Item_Description, _
                                    .LineItemCost = oi.LineItemCost, _
                                    .LineItemFreight = oi.LineItemFreight, _
                                    .UOMUnitCost = oi.UOMUnitCost, _
                                    .LineNumber = oi.LineNumber, _
                                    .ItemAllowanceDiscountAmount = oi.ItemAllowanceDiscountAmount, _
                                    .QuantityDiscount = oi.QuantityDiscount, _
                                    .DiscountType = oi.DiscountType, _
                                    .CostUnit = oi.CostUnit, _
                                    .UOMAdjustedUnitCost = oi.UOMAdjustedUnitCost, _
                                    .QuantityShipped = oi.QuantityShipped, _
                                    .CatchweightRequired = oi.CatchweightRequired, _
                                    .eInvoiceQuantity = oi.eInvoiceQuantity, _
                                    .ReceivingDiscrepancyReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                                    }
                Return oiQuery.First
            End If
            Return Nothing
        End Function

        Public Function GetOrderItem(ByVal sIdentifier As String, ByVal dPackage_Desc1 As Decimal) As OrderItem
            If Not (Me.OrderItems Is Nothing) Then
                Dim oiQuery = From oi In OrderItems _
                              Where oi.Identifier = sIdentifier _
                              And oi.Package_Desc1 = dPackage_Desc1 _
                              Select New OrderItem _
                              With { _
                                    .OrderItem_ID = oi.OrderItem_ID, _
                                    .Item_Key = oi.Item_Key, _
                                    .Identifier = oi.Identifier, _
                                    .Package_Desc1 = oi.Package_Desc1, _
                                    .Package_Desc2 = oi.Package_Desc2, _
                                    .QuantityUnit = oi.QuantityUnit, _
                                    .QuantityOrdered = oi.QuantityOrdered, _
                                    .QuantityReceived = oi.QuantityReceived, _
                                    .Total_Weight = oi.Total_Weight, _
                                    .IsReceivedWeightRequired = oi.IsReceivedWeightRequired, _
                                    .VendorItemID = oi.VendorItemID, _
                                    .Item_Description = oi.Item_Description, _
                                    .LineItemCost = oi.LineItemCost, _
                                    .LineItemFreight = oi.LineItemFreight, _
                                    .UOMUnitCost = oi.UOMUnitCost, _
                                    .LineNumber = oi.LineNumber, _
                                    .ItemAllowanceDiscountAmount = oi.ItemAllowanceDiscountAmount, _
                                    .QuantityDiscount = oi.QuantityDiscount, _
                                    .DiscountType = oi.DiscountType, _
                                    .CostUnit = oi.CostUnit, _
                                    .UOMAdjustedUnitCost = oi.UOMAdjustedUnitCost, _
                                    .QuantityShipped = oi.QuantityShipped, _
                                    .CatchweightRequired = oi.CatchweightRequired, _
                                    .eInvoiceQuantity = oi.eInvoiceQuantity, _
                                    .ReceivingDiscrepancyReasonCodeID = oi.ReceivingDiscrepancyReasonCodeID _
                                    }
                Return oiQuery.First
            End If
            Return Nothing
        End Function

        Public Function CreateOrder() As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = Vendor_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderType_ID"
            currentParam.Value = OrderType_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ProductType_ID"
            currentParam.Value = ProductType_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PurchaseLocation_ID"
            currentParam.Value = PurchaseLocation_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ReceiveLocation_ID"
            currentParam.Value = ReceiveLocation_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_SubTeam"
            If Transfer_SubTeam <> Nothing Then
                currentParam.Value = Transfer_SubTeam
            Else
                currentParam.Value = Convert.DBNull.value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_To_SubTeam"
            currentParam.Value = Transfer_To_SubTeam
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Fax_Order"
            If Fax_Order <> Nothing Then
                currentParam.Value = Fax_Order
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Expected_Days"
            currentParam.Value = (Now - Expected_Date).Days
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CreatedBy"
            currentParam.Value = CreatedBy
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Return_Order"
            currentParam.Value = Return_Order
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FromQueue"
            If FromQueue <> Nothing Then
                currentParam.Value = FromQueue
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SupplyTransferToSubTeam"
            If SupplyTransferToSubTeam <> Nothing Then
                currentParam.Value = SupplyTransferToSubTeam
            Else
                currentParam.Value = Convert.DBNull.value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DSDOrder"
            If DSDOrder <> Nothing Then
                currentParam.Value = DSDOrder
            Else
                currentParam.Value = 0
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            If InvoiceNumber <> Nothing Then
                currentParam.Value = InvoiceNumber
            Else
                currentParam.Value = Convert.DBNull.value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                Dim dt As DataTable = factory.GetStoredProcedureDataTable("InsertOrder", paramList)

                If dt.Rows.Count > 0 Then
                    OrderHeader_ID = dt.Rows(0).Item("OrderHeader_ID")
                End If

                If OrderHeader_ID <> 0 Then
                    Dim OI As OrderItem
                    For Each OI In OrderItems
                        Me.ResultObject = OI.InsertOrderItem(Me.OrderHeader_ID, Me.CreatedBy, OI.AdjustedCost, OI.ReasonCodeDetailID)
                        If Me.ResultObject.IRMA_PONumber = -1 Then
                            Return Me.ResultObject
                        End If
                    Next
                End If

                logger.Info("CreateOrder Successful for PO Number - " & Me.OrderHeader_ID)
                Me.ResultObject.Load(Me.OrderHeader_ID, True, "CreateOrder()")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Create Order failed!" & factory.GetSQLString("InsertOrder", paramList), ex)
                Me.ResultObject.Load(-1, False, "CreateOrder()", "Create Order Failed!", ex.ToString, factory.GetSQLString("InsertOrder", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function UpdateOrderHeaderCosts() As Boolean
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim SprocSuccess As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)


                al = factory.ExecuteStoredProcedure("UpdateOrderHeaderCosts", paramList)
                SprocSuccess = True
                Return SprocSuccess

            Catch ex As Exception
                Common.logger.Info("Send Order failed for PONumber -" & Me.OrderHeader_ID & "; " & factory.GetSQLString("UpdateOrderHeaderCosts", paramList), ex)
                Me.ResultObject.Load(-1, False, "UpdateOrderHeaderCosts()", "Send Order failed for PONumber -" & Me.OrderHeader_ID, ex.ToString, factory.GetSQLString("UpdateOrderHeaderCosts", paramList))
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function UpdateOrderRefreshCosts() As Boolean
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim SprocSuccess As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = CreatedBy
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' *******************************

                al = factory.ExecuteStoredProcedure("UpdateOrderRefreshCosts", paramList)
                SprocSuccess = True
                Return SprocSuccess

            Catch ex As Exception
                Common.logger.Info("UpdateOrderRefreshCosts failed for PONumber -" & Me.OrderHeader_ID & "; " & factory.GetSQLString("UpdateOrderRefreshCosts", paramList), ex)
                Me.ResultObject.Load(-1, False, "UpdateOrderRefreshCosts()", "UpdateOrderRefreshCosts failed for PONumber -" & Me.OrderHeader_ID, ex.ToString, factory.GetSQLString("UpdateOrderRefreshCosts", paramList))
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function SendOrder() As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                UpdateOrderHeaderCosts()

                If Me.ResultObject.IRMA_PONumber = -1 Then
                    Return Me.ResultObject
                End If

                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Fax_Order"
                If Fax_Order <> Nothing Then
                    currentParam.Value = Fax_Order
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Email_Order"
                If Email_Order <> Nothing Then
                    currentParam.Value = Email_Order
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Electronic_Order"
                If Electronic_Order <> Nothing Then
                    currentParam.Value = Electronic_Order
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OverrideTransmissionMethod"
                If OverrideTransmissionMethod <> Nothing Then
                    currentParam.Value = OverrideTransmissionMethod
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Target"
                currentParam.Value = Convert.DBNull.value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                al = factory.ExecuteStoredProcedure("UpdateOrderSend", paramList)

                logger.Info("SendOrder Successful for PO Number - " & Me.OrderHeader_ID)
                Me.ResultObject.Load(Me.OrderHeader_ID, True, "SendOrder()")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Send Order failed for PONumber -" & Me.OrderHeader_ID & "; " & factory.GetSQLString("UpdateOrderSend", paramList), ex)
                Me.ResultObject.Load(-1, False, "SendOrder()", "Send Order failed for PONumber -" & Me.OrderHeader_ID, ex.ToString, factory.GetSQLString("UpdateOrderSend", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function ReceiveOrder() As Result

            Dim OI As OrderItem

            Try
                For Each OI In OrderItems
                    Me.ResultObject = OI.ReceiveOrderItem(Me.DSDOrder, Me.CreatedBy)
                    If Me.ResultObject.IRMA_PONumber = -1 Then
                        Return Me.ResultObject
                    End If
                Next

                logger.Info("ReceiveOrder Successful for PO Number - " & Me.OrderHeader_ID)
                Me.ResultObject.Load(Me.OrderHeader_ID, True, "ReceiveOrder()")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Receive Order failed for PONumber -" & Me.OrderHeader_ID, ex)
                Me.ResultObject.Load(-1, False, "ReceiveOrder()", "Receive Order failed for PONumber -" & Me.OrderHeader_ID, ex.ToString)
                Return Me.ResultObject

            End Try

        End Function

        Public Function CloseOrder(ByVal OrderHeader_ID As Integer, ByVal User_ID As Integer) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim outputList As ArrayList = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim isSuspended As Boolean

            Try
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = User_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsSuspended"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                outputList = factory.ExecuteStoredProcedure("UpdateOrderClosed", paramList)

                If Not outputList(0).Equals(DBNull.Value) Then
                    isSuspended = CBool(outputList(0))
                End If

                logger.Info("CloseOrder Successful for PO Number - " & OrderHeader_ID)

                If Me.ResultObject Is Nothing Then
                    Me.ResultObject = New Result()
                End If

                Me.ResultObject.Load(OrderHeader_ID, True, "CloseOrder()")
                Me.ResultObject.Flag = isSuspended

                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Close Order failed for PONumber -" & OrderHeader_ID & "; " & factory.GetSQLString("UpdateOrderClosed", paramList), ex)
                Me.ResultObject.Load(-1, False, "CloseOrder()", "Close Order failed for PONumber -" & OrderHeader_ID, ex.ToString, factory.GetSQLString("UpdateOrderClosed", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function DeleteOrder() As Boolean

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim SprocSuccess As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = CreatedBy
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                al = factory.ExecuteStoredProcedure("DeleteOrderHeader", paramList)
                SprocSuccess = True
                logger.Info("DeleteOrder Successful for PO Number - " & Me.OrderHeader_ID)
                Return SprocSuccess

            Catch ex As Exception
                Throw

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function IntraStore_AutoSendReceiveCloseOrder() As Result
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try

                UpdateOrderRefreshCosts() 'Updates all costs

                If Me.ResultObject.IRMA_PONumber = -1 Then 'UpdateOrderRefreshCosts failed
                    Return Me.ResultObject
                End If

                UpdateOrderHeaderCosts() 'Updates OrderHeader with costs

                If Me.ResultObject.IRMA_PONumber = -1 Then 'UpdateOrderHeaderCosts failed
                    Return Me.ResultObject
                End If

                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************                
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = CreatedBy
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                al = factory.ExecuteStoredProcedure("AutoCloseIntraStoreTransfer", paramList)

                'IntraStore_AutoSendReceiveCloseOrder success
                logger.Info("IntraStore_AutoSendReceiveCloseOrder Successful for PO Number - " & Me.OrderHeader_ID)
                Me.ResultObject.Load(Me.OrderHeader_ID, True, "IntraStore_AutoSendReceiveCloseOrder()")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("IntraStore_AutoSendReceiveCloseOrder failed for PONumber -" & Me.OrderHeader_ID & "; " & factory.GetSQLString("AutoCloseIntraStoreTransfer", paramList), ex)
                Me.ResultObject.Load(-1, False, "IntraStore_AutoSendReceiveCloseOrder()", "IntraStore_AutoSendReceiveCloseOrder failed for PONumber -" & Me.OrderHeader_ID, ex.ToString, factory.GetSQLString("AutoCloseIntraStoreTransfer", paramList))
                Return Me.ResultObject
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function SendElectronicOrder() As Result

            Dim svc As New ElectronicOrderWebService
            Dim sResult As String
            Dim aryResult() As String
            Dim Message As String
            Dim sPOItemData As String

            Try
                sPOItemData = GetPOItemData()

                If sPOItemData <> String.Empty Then

                    ' Result comes back as ErrorCodeNumber|ErrorDescription|DVOPONumber
                    sResult = svc.saveItemData(sPOItemData)
                    aryResult = Split(sResult, "|")

                    Select Case aryResult(0)
                        Case 0
                            Message = "Your PO has been successfully sent to DVO (DVO PO# " & aryResult(2) & ").  DVO will notify you with an e-mail when it has been delivered to the vendor.  This PO will remain locked until that time."
                            Me.ResultObject.Load(Me.OrderHeader_ID, True, "SendElectronicOrder()", Message, "", "", CInt(aryResult(2)))
                            logger.Info("SendElectronicOrder Successful for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                        Case -1
                            Message = "There was an issue sending the PO to DVO.  This vendor requires an account set up in DVO and the account is missing.  Please contact Global Help Desk to get this resolved."
                            Me.ResultObject.Load(-1, False, "SendElectronicOrder()", Message)
                            logger.Info("SendElectronicOrder Failed for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                        Case -2
                            Message = "There was an issue sending the PO to DVO due to a data error with one of the items on your PO.  Please check the items and resend the PO.  If the problem continues please contact Global Help Desk to get this resolved."
                            Me.ResultObject.Load(-1, False, "SendElectronicOrder()", Message)
                            logger.Info("SendElectronicOrder Failed for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                        Case -3
                            ' If the order has already been sent and processed by DVO we don't want to mark it as NOT sent. 
                            Message = "This PO has already been received and processed by DVO. Please do not resend it. If the problem continues please contact Global Help Desk  to get this resolved."
                            Me.ResultObject.Load(Me.OrderHeader_ID, True, "SendElectronicOrder()", Message, "", "", CInt(aryResult(2)))
                            logger.Info("SendElectronicOrder Successful for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                        Case 999
                            Message = "There was an issue sending the PO to DVO.  There is no matching vendor for the PeopleSoft Vendor ID " & GetVendorPSVendorId(Me.Vendor_ID) & ".  Please contact Global Help Desk to get this resolved."
                            Me.ResultObject.Load(-1, False, "SendElectronicOrder()", Message)
                            logger.Info("SendElectronicOrder Failed for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                        Case Else
                            Message = "Unexpected error code sent by DVO web service.  Please try again later and if the problem continues contact Global Help Desk to get this resolved."
                            Me.ResultObject.Load(-1, False, "SendElectronicOrder()", Message)
                            logger.Info("SendElectronicOrder Failed for PO Number - " & Me.OrderHeader_ID & ". " & Message)
                    End Select

                    Return Me.ResultObject

                Else
                    ' GetPOITemData() Failed. Rollback and Return ResultObject.
                    Return Me.ResultObject
                    logger.Info("SendElectronicOrder Failed for PO Number - " & Me.OrderHeader_ID & ". GetPOITemData() Failed")
                End If

            Catch ex As Exception
                Common.logger.Info("SendElectronicOrder failed for PONumber -" & Me.OrderHeader_ID, ex)
                Me.ResultObject.Load(-1, False, "SendElectronicOrder()", "SendElectronicOrder failed for PONumber -" & Me.OrderHeader_ID, ex.ToString)
                Return Me.ResultObject

            End Try

        End Function

        Public Function GetPOItemData() As String

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As DataTable
            Dim ElectronicOrderHeaderRow As DataRow
            Dim ElectronicOrderItemRow As DataRow
            Dim sItemInfo As String = String.Empty
            Dim sXML As String
            Dim sErrorMessage As String = String.Empty

            Try
                ' Create a regex to remove any characters that might cause problems with the XML
                Dim textCleaner As Regex = New Regex("[^a-zA-Z0-9 ]")

                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                'StoreSubTeamRelationship validation occurs in the WFM Mobile client before order creation
                dt = factory.GetStoredProcedureDataTable("GetElectronicOrderHeaderInfo", paramList)
                ElectronicOrderHeaderRow = dt.Rows(0)

                ' Create XML order header information - clean the notes field
                sXML = "<order>" & _
                          "<irma_po>" & Me.OrderHeader_ID & "</irma_po>" & _
                          "<error_email>" & Escape(ElectronicOrderHeaderRow.Item("Email").ToString()) & "</error_email>" & _
                          "<success_email>" & Escape(ElectronicOrderHeaderRow.Item("Email").ToString()) & "</success_email>" & _
                          "<ps_number>" & ElectronicOrderHeaderRow.Item("PS_Vendor_ID") & "</ps_number>" & _
                          "<req_receive_date>" & ElectronicOrderHeaderRow.Item("Expected_Date") & "</req_receive_date>" & _
                          "<po_notes>" & Escape(ElectronicOrderHeaderRow.Item("OrderHeaderDesc").ToString()) & "</po_notes>" & _
                          "<business_unit>" & ElectronicOrderHeaderRow.Item("BusinessUnit_ID") & "</business_unit>" & _
                          "<sub_team>" & ElectronicOrderHeaderRow.Item("SubTeam_No") & "</sub_team>" & _
                          "<pos_dept>" & ElectronicOrderHeaderRow.Item("POS_Dept") & "</pos_dept>" & _
                          "<external_system>" & Escape(ElectronicOrderHeaderRow.Item("description").ToString()) & "</external_system>" & _
                          "<external_order_id>" & ElectronicOrderHeaderRow.Item("OrderExternalSourceOrderId") & "</external_order_id>" & _
                          "<iscredit>" & ElectronicOrderHeaderRow.Item("isCredit") & "</iscredit>" & _
                          "<invoice_number>" & If(IsDBNull(ElectronicOrderHeaderRow.Item("InvoiceNumber")), String.Empty, ElectronicOrderHeaderRow.Item("InvoiceNumber")) & "</invoice_number>" & _
                          "<items>"

                dt = New DataTable
                dt = factory.GetStoredProcedureDataTable("GetElectronicOrderItemInfo", paramList)

                ' Loop through the items on the order and create the XML item records - clean the description and brand fields
                For Each ElectronicOrderItemRow In dt.Rows
                    sItemInfo &= "<item upc='" & ElectronicOrderItemRow.Item("UPC") & "' " & _
                                   "vid='" & ElectronicOrderItemRow.Item("VID") & "' " & _
                                   "case_pack='" & CInt(ElectronicOrderItemRow.Item("Case_Pack")) & "' " & _
                                   "qty='" & CInt(ElectronicOrderItemRow.Item("Qty")) & "' " & _
                                   "unit_cost='" & ElectronicOrderItemRow.Item("Unit_Cost") & "' " & _
                                   "pack_size='" & CInt(ElectronicOrderItemRow.Item("Pack_Size")) & "' " & _
                                   "item_uom='" & Trim(ElectronicOrderItemRow.Item("Item_UOM")) & "' " & _
                                   "description='" & textCleaner.Replace(Trim(ElectronicOrderItemRow.Item("Description")), "") & "' " & _
                                   "pos_dept='" & ElectronicOrderItemRow.Item("POS_Dept") & "' " & _
                                   "brand='" & textCleaner.Replace(Trim(ElectronicOrderItemRow.Item("Brand")), "") & "' " & _
                                   "case_uom='" & Trim(ElectronicOrderItemRow.Item("Case_UOM")) & "' " & _
                                   "item_uom_iscase='" & Trim(ElectronicOrderItemRow.Item("UOM_IsCase")) & "' " & _
                                   "credit_reason_id='" & ElectronicOrderItemRow.Item("CreditReason_Id") & "' " & _
                                   "credit_lot_no='" & ElectronicOrderItemRow.Item("Lot_No") & "' " & _
                                   "credit_expire_date='' credit_notes=''/>" ' credit_expire_date and credit_notes left blank. for future use?
                Next

                Return sXML & sItemInfo & "</items></order>"

            Catch ex As Exception
                Common.logger.Info("Send Electronic Order failed for PONumber -" & Me.OrderHeader_ID & "; Step failed - GetPOItemData()", ex)
                Me.ResultObject.Load(-1, False, "GetPOItemData()", "Send Electronic Order failed for PONumber -" & Me.OrderHeader_ID & "; Step failed - GetPOItemData()", ex.ToString)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function GetVendorPSVendorId(ByVal sVendorId As String) As String
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Try
                ' Execute the function
                GetVendorPSVendorId = CType(factory.ExecuteScalar("SELECT dbo.fn_GetVendorPSVendorId(" & sVendorId & ")"), String)
            Catch ex As Exception
                logger.Debug("GetVendorPSVendor_Id Failed!")
                GetVendorPSVendorId = "" 'This PSVendorID is only used to add info to error message, so no need to throw exception.
            End Try

        End Function

        Public Shared Function GetOrderHeaderByIdentifier(ByVal UPC As String, _
                                                   ByVal StoreNumber As Integer) As List(Of Order)

            logger.Info("GetOrderHeaderByIdentifier() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "UPC"
            currentParameter.Value = UPC
            currentParameter.Type = DBParamType.String
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "StoreNumber"
            currentParameter.Value = StoreNumber
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Dim orderList As New List(Of Order)

            Try
                dataTable = factory.GetStoredProcedureDataTable("GetOrderHeaderByIdentifier", parameterList)
                For Each dataRow As DataRow In dataTable.Rows
                    Dim order As New Order
                    order.OrderHeader_ID = dataRow.Item("OrderHeader_ID")
                    order.SubTeam_Name = dataRow.Item("Subteam_Name")
                    order.OrderedCost = NotNull(dataRow.Item("OrderedCost"), Nothing)
                    order.Expected_Date = dataRow.Item("Expected_Date")
                    order.EinvoiceID = NotNull(dataRow.Item("eInvoice_Id"), Nothing)

                    orderList.Add(order)
                Next
                Return orderList
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function UpdateReceivingDiscrepancyCode(ByVal reasonCodeList As String, ByVal separator1 As Char, ByVal separator2 As Char) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ReceivingDiscrepancyReasonCodeList"
            currentParam.Value = reasonCodeList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ListSeparator1"
            currentParam.Value = separator1
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ListSeparator2"
            currentParam.Value = separator2
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            Try
                Me.ResultObject = New Result()
                al = factory.ExecuteStoredProcedure("UpdateReceivingDiscrepancyReasonCodeID", paramList)
                Common.logger.Info("UpdateReceivingDiscrepancyReasonCodeID was successfull for OrderItem_ID and Code pairs:" & reasonCodeList)
                ResultObject.Load(Me.OrderHeader_ID, True, "UpdateReceivingDiscrepancyCode")
                Return Me.ResultObject
            Catch ex As Exception
                Me.ResultObject = New Result()
                ResultObject.Load(Me.OrderHeader_ID, False, "UpdateReceivingDiscrepancyCode", _
                                  "UpdateReceivingDiscrepancyReasonCodeID Failed for the OrderItem_ID & Reason Code pair list: " & reasonCodeList, _
                                  ex.ToString, factory.GetSQLString("UpdateReceivingDiscrepancyReasonCodeID", paramList))
                Return Me.ResultObject
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function UpdateOrderBeforeClose(ByVal OrderHeader_ID As Integer, _
                                               ByVal InvoiceNumber As String, _
                                               ByVal InvoiceDate As Date, _
                                               ByVal InvoiceCost As Decimal, _
                                               ByVal VendorDoc_ID As String, _
                                               ByVal VendorDocDate As Date, _
                                               ByVal SubTeam_No As Integer, _
                                               ByVal PartialShipment As Boolean) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            If InvoiceNumber <> Nothing Then
                currentParam.Value = InvoiceNumber
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            If InvoiceDate <> Nothing Then
                currentParam.Value = InvoiceDate
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = InvoiceCost
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorDoc_ID"
            If VendorDoc_ID <> Nothing Then
                currentParam.Value = VendorDoc_ID.ToString
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorDocDate"
            If VendorDocDate <> Nothing Then
                currentParam.Value = VendorDocDate
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            If SubTeam_No <> Nothing Then
                currentParam.Value = SubTeam_No
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PartialShipment"
            currentParam.Value = PartialShipment
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            Try
                Me.ResultObject = New Result()
                al = factory.ExecuteStoredProcedure("UpdateOrderBeforeClose", paramList)

                Common.logger.Info("UpdateOrderBeforeClose was successful for OrderHeader_ID:" & OrderHeader_ID)
                ResultObject.Load(Me.OrderHeader_ID, True, "UpdateOrderBeforeClose", _
                                    "UpdateOrderBeforeClose Succeeded for OrderHeader_ID: " & OrderHeader_ID, _
                                    String.Empty, factory.GetSQLString("UpdateOrderBeforeClose", paramList))
                Return Me.ResultObject

            Catch ex As Exception
                Me.ResultObject = New Result()
                ResultObject.Load(Me.OrderHeader_ID, False, "UpdateOrderBeforeClose", _
                                    "UpdateOrderBeforeClose Failed for OrderHeader_ID: " & OrderHeader_ID, _
                                    ex.ToString, factory.GetSQLString("UpdateOrderBeforeClose", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function CheckInvoiceNumber(ByVal VendorID As Integer, ByVal InvoiceNumber As String, ByVal OrderHeader_ID As Integer) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim outputList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim orderCount As Integer

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = VendorID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = InvoiceNumber
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ThisOrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RecordCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                Me.ResultObject = New Result

                outputList = factory.ExecuteStoredProcedure("GetInvoiceNumberUse", paramList)

                orderCount = outputList(0)

                Me.ResultObject.Load(OrderHeader_ID, True, "CheckInvoiceNumber()")
                Me.ResultObject.Count = orderCount
                If orderCount > 0 Then
                    Me.ResultObject.Flag = True
                End If

                Return Me.ResultObject

            Catch ex As Exception
                logger.Error("Check Invoice Number failed for Invoice Number -" & InvoiceNumber & "; " & factory.GetSQLString("GetInvoiceNumberUse", paramList), ex)
                Me.ResultObject.Load(OrderHeader_ID, False, "CheckInvoiceNumber()", "Check Invoice Number failed for Invoice Number -" & InvoiceNumber, ex.ToString, factory.GetSQLString("GetInvoiceNumberUse", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function ReOpenOrder(ByVal OrderHeader_ID As Integer) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim outputList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Success"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            Try
                Me.ResultObject = New Result
                outputList = factory.ExecuteStoredProcedure("UpdateOrderOpen", paramList)

                success = CBool(outputList(0))

                Me.ResultObject.Flag = success
                Me.ResultObject.Load(OrderHeader_ID, True, "ReOpenOrder")

                Return Me.ResultObject

            Catch ex As Exception
                logger.Error("ReOpen Order failed for PO: " & OrderHeader_ID, ex)
                Me.ResultObject.Load(OrderHeader_ID, False, "ReOpenOrder", "ReOpen Order failed for PO: " & OrderHeader_ID, ex.ToString, factory.GetSQLString("UpdateOrderOpen", paramList))
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function RefuseReceiving(ByVal orderHeaderID As Integer, ByVal userID As Integer, ByVal refuseReceivingReasonCodeID As Integer) As Result

            logger.Info("RefuseReceiving() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_ID"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "User_ID"
            currentParameter.Value = userID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "RefuseReceivingReasonID"
            currentParameter.Value = refuseReceivingReasonCodeID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Try
                factory.ExecuteStoredProcedure("UpdateOrderHeaderRefuseReceivingAndClose", parameterList)

                ResultObject = New Result
                ResultObject.Load(orderHeaderID, True, "RefuseReceiving")
                Return ResultObject

            Catch ex As Exception
                logger.Error("RefuseReceiving failed for PO: " & orderHeaderID, ex)
                ResultObject.Load(orderHeaderID, False, "RefuseReceiving", "RefuseReceiving failed for PO: " & orderHeaderID, ex.ToString, factory.GetSQLString("UpdateOrderHeaderRefuseReceivingAndClose", parameterList))
                Return ResultObject

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Shared Function CountSustainabilityRankingRequiredItems(ByVal orderHeaderID As Integer) As Integer

            logger.Info("CountSustainabilityRankingRequiredItems() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As New DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_ID"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Try
                dataTable = factory.GetStoredProcedureDataTable("CountUnrankedSustainabilityRequiredOrderItems", parameterList)
                Return dataTable.Rows(0).Item("OrderItemCount")

            Catch ex As Exception
                logger.Error("CountUnrankedSustainabilityRequiredOrderItems failed.")
                Throw ex

            Finally
                connectionCleanup(factory)

            End Try
        End Function

        Public Shared Function UpdateOrderHeaderCosts(ByVal orderHeaderID) As Result

            logger.Info("UpdateOrderHeaderCosts() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_ID"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Try
                factory.ExecuteStoredProcedure("UpdateOrderHeaderCosts", parameterList)

                Dim result = New Result
                result.Load(orderHeaderID, True, "UpdateOrderHeaderCosts")
                Return result

            Catch ex As Exception
                logger.Error("UpdateOrderHeaderCosts failed for PO: " & orderHeaderID, ex)

                Dim result = New Result
                result.Load(orderHeaderID, False, "UpdateOrderHeaderCosts", "UpdateOrderHeaderCosts failed for PO: " & orderHeaderID, ex.ToString, factory.GetSQLString("UpdateOrderHeaderRefuseReceivingAndClose", parameterList))
                Return result

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Shared Function GetOrderCurrency(ByVal orderHeaderId As Integer, regionCode As String) As String

            logger.Info("GetOrderCurrency() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As New DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderID"
            currentParameter.Value = orderHeaderId
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "RegionCode"
            currentParameter.Value = regionCode
            currentParameter.Type = DBParamType.String
            parameterList.Add(currentParameter)

            Try
                dataTable = factory.GetStoredProcedureDataTable("GetOrderCurrency", parameterList)
                Return dataTable.Rows(0).Item("CurrencyCode")

            Catch ex As Exception
                logger.Error("GetOrderCurrency failed for PO: " & orderHeaderId, ex)
                Throw ex

            Finally
                connectionCleanup(factory)

            End Try

        End Function

    End Class
End Namespace