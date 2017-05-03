Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility


Namespace WholeFoods.IRMA.Replenishment.SendOrders.DataAccess
    Public Class SendOrdersDAO
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.SendOrders.DataAccess.SendOrdersDAO")
        Public Shared NextID As Integer = 0
        Public Shared BUYER_EMAIL As Integer = 1
        Public Shared TL_EMAIL As Integer = 2
        Public Shared COMPANY As Integer = 3

        Public Shared Sub InsertPOSItems(ByVal storeNo As Integer, ByVal pathFileName As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If storeNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = storeNo
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PathFileName"
                If pathFileName Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = pathFileName
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("Replenishment_POSPull_InsertPOSItem", paramList)

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            'stored procedure name is: Replenishment_POSPull_InsertPOSItem
        End Sub
        Public Shared Function GetWarehouseOrders() As Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim order As ItemCatalog.Order = New ItemCatalog.Order
            Dim orderItem As ItemCatalog.OrderItem
            Dim itemList As ArrayList = New ArrayList
            Dim orderMap As Hashtable = New Hashtable
            Dim lPrevOrderHeader_ID As Integer = 0
            Try
                results = factory.GetStoredProcedureDataReader("GetWarehousePurchaseOrders")
                While results.Read
                    If lPrevOrderHeader_ID <> getIntValue(results.GetValue(results.GetOrdinal("OrderHeader_ID"))) Then
                        If (lPrevOrderHeader_ID <> 0) Then
                            orderMap.Add(order, itemList)
                            itemList = New ArrayList
                            order = New ItemCatalog.Order
                        End If
                        'Format the header record
                        order = New ItemCatalog.Order
                        lPrevOrderHeader_ID = getIntValue(results.GetValue(results.GetOrdinal("OrderHeader_ID")))
                        order.OrderHeader_ID = lPrevOrderHeader_ID
                        order.DistCenter = getIntValue(results.GetValue(results.GetOrdinal("DistCenter")))

                        order.EXEWarehouse = getIntValue(results.GetValue(results.GetOrdinal("EXEWarehouse")))
                        order.Vendor_ID = getIntValue(results.GetValue(results.GetOrdinal("Vendor_ID")))
                        order.Buyer = getStringValue(results.GetValue(results.GetOrdinal("Buyer")))
                        order.Expected_Date = getDateValue(results.GetValue(results.GetOrdinal("Expected_Date")))
                    End If
                    'Format the detail record
                    orderItem = New ItemCatalog.OrderItem
                    orderItem.Identifier = getStringValue(results.GetValue(results.GetOrdinal("Identifier")))
                    orderItem.QuantityOrdered = getDecValue(results.GetValue(results.GetOrdinal("QuantityOrdered"))) 'use a holder for qty
                    orderItem.Package_Desc1 = getDecValue(results.GetValue(results.GetOrdinal("Package_Desc1")))
                    itemList.Add(orderItem)
                End While
                If itemList.Count > 0 Then
                    orderMap.Add(order, itemList)
                End If

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return orderMap
        End Function
        Public Shared Function GetWarehouseCustomerOrders() As Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim order As ItemCatalog.Order = New ItemCatalog.Order
            Dim orderItem As ItemCatalog.OrderItem
            Dim itemList As ArrayList = New ArrayList
            Dim orderMap As Hashtable = New Hashtable
            Dim lPrevOrderHeader_ID As Integer = 0
            Try


                results = factory.GetStoredProcedureDataReader("GetWarehouseCustomerOrders")
                While results.Read
                    If lPrevOrderHeader_ID <> getIntValue(results.GetValue(results.GetOrdinal("OrderHeader_ID"))) Then
                        If (lPrevOrderHeader_ID <> 0) Then
                            orderMap.Add(order, itemList)
                            itemList = New ArrayList
                            order = New ItemCatalog.Order
                        End If
                        'Format the header record
                        order = New ItemCatalog.Order
                        lPrevOrderHeader_ID = getIntValue(results.GetValue(results.GetOrdinal("OrderHeader_ID")))
                        order.OrderHeader_ID = lPrevOrderHeader_ID
                        order.SubteamAbbreviation = getStringValue(results.GetValue(results.GetOrdinal("SubTeam_Abbreviation")))
                        If order.SubteamAbbreviation.Length > 4 Then order.SubteamAbbreviation = Left(order.SubteamAbbreviation, 4)
                        order.DistCenter = getIntValue(results.GetValue(results.GetOrdinal("DistCenter")))
                        order.ReceiveStore_No = getIntValue(results.GetValue(results.GetOrdinal("DistCenter")))
                        order.PurchaseLocation_ID = getIntValue(results.GetValue(results.GetOrdinal("EXEWarehouse"))) ' use as a holder for EXEWarehouse
                        order.Vendor_ID = getIntValue(results.GetValue(results.GetOrdinal("Vendor_ID")))
                        order.CreatedByID = getLongValue(results.GetValue(results.GetOrdinal("Buyer")))
                        order.Expected_Date = getDateValue(results.GetValue(results.GetOrdinal("Expected_Date")))
                        order.ReceiveLocation_ID = getIntValue(results.GetValue(results.GetOrdinal("ReceiveLocation_ID")))
                        order.EXEWarehouse = getIntValue(results.GetValue(results.GetOrdinal("EXEWarehouse")))
                        order.OrderDate = getDateValue(results.GetValue(results.GetOrdinal("OrderDate")))
                        order.OrderType_ID = CType(getIntValue(results.GetValue(results.GetOrdinal("OrderType_ID"))), ItemCatalog.enuOrderType)
                    End If
                    'Format the detail record
                    orderItem = New ItemCatalog.OrderItem
                    orderItem.Identifier = getStringValue(results.GetValue(results.GetOrdinal("Identifier")))
                    orderItem.QuantityOrdered = getDecValue(results.GetValue(results.GetOrdinal("QuantityAllocated"))) 'use a holder for qty
                    orderItem.Package_Desc1 = getDecValue(results.GetValue(results.GetOrdinal("Package_Desc1")))
                    itemList.Add(orderItem)
                End While
                If itemList.Count > 0 Then
                    orderMap.Add(order, itemList)
                End If
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return orderMap
        End Function
        Public Shared Sub UpdateOrderWarehouseSent(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderWarehouseSent entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderWarehouseSent", paramList)

            Logger.LogDebug("UpdateOrderWarehouseSent exit", CLASSTYPE)
        End Sub
        Public Function GetItemDataset(ByVal strsql As String, ByRef ItemDataset As DataSet, ByVal strFillTableName As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As DataSet = Nothing
            Try
                strsql = "SELECT Item_Key, LEFT(Identifier,12) AS Identifier FROM [ItemCatalog].[dbo].[ItemIdentifier] "

                strsql = strsql + "  WHERE Deleted_Identifier = 0 AND Add_Identifier = 0 "

                reader = factory.GetDataSet(strsql, ItemDataset, strFillTableName)

            Catch ex As Exception

                'TODO handle exception

                Throw ex

            End Try

            Return reader

        End Function
        Public Shared Function GetNextOrder() As Integer
            Logger.LogDebug("GetNextOrder entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim lOrderHeader_ID As Integer = 0

            Try
                results = factory.GetStoredProcedureDataReader("FindOpenOrders")
                If results.Read Then
                    lOrderHeader_ID = CInt(results((results.GetOrdinal("FirstOrder"))))
                End If
            Catch ex As Exception
            End Try
            Logger.LogDebug("GetNextOrder exit", CLASSTYPE)
            NextID = lOrderHeader_ID

            Return lOrderHeader_ID

        End Function
        Public Shared Sub UpdateOrderCancelSend(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderCancelSend entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderCancelSend", paramList)

            Logger.LogDebug("UpdateOrderCancelSend exit", CLASSTYPE)
        End Sub
        Public Shared Sub UpdateOrderNotSent(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderNotSent entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderNotSent", paramList)

            Logger.LogDebug("UpdateOrderNotSent exit", CLASSTYPE)
        End Sub
        Public Shared Sub UpdateOrderSentDate(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderSentDate entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderSentDate", paramList)

            Logger.LogDebug("UpdateOrderSentDate exit", CLASSTYPE)
        End Sub
        Public Shared Sub UpdateOrderSentToFaxDate(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderSentToFaxDate entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderSentToFaxDate", paramList)

            Logger.LogDebug("UpdateOrderSentToFaxDate exit", CLASSTYPE)
        End Sub

        Public Shared Sub UpdateOrderSentToEmailDate(ByVal dOrderHeaderID As Integer)
            Logger.LogDebug("UpdateOrderSentToEmailDate entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.ExecuteStoredProcedure("UpdateOrderSentToEmailDate", paramList)

            Logger.LogDebug("UpdateOrderSentToEmailDate exit", CLASSTYPE)
        End Sub

        Public Shared Function GetPOHeader(ByVal dOrderHeaderID As Integer) As POHeaderBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim order As POHeaderBO = New POHeaderBO
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            
            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("GetPOHeader", paramList)
                If results.Read Then
                    order.OrderHeader_ID = dOrderHeaderID
                    order.Return_Order = results.GetBoolean(results.GetOrdinal("Return_Order"))
                    order.Fax_Order = results.GetBoolean(results.GetOrdinal("Fax_Order"))
                    order.Email_Order = results.GetBoolean(results.GetOrdinal("Email_Order"))
                    order.Printer = getStringValue(results.GetValue(results.GetOrdinal("Printer")))
                    order.Electronic_Transfer = results.GetBoolean(results.GetOrdinal("Electronic_Transfer"))
                    order.FTP_Addr = getStringValue(results.GetValue(results.GetOrdinal("FTP_Addr")))
                    order.FTP_Path = getStringValue(results.GetValue(results.GetOrdinal("FTP_Path")))
                    order.FTP_User = getStringValue(results.GetValue(results.GetOrdinal("FTP_User")))
                    order.FTP_Password = getStringValue(results.GetValue(results.GetOrdinal("FTP_Password")))
                    order.PeopleSoftNumber = getStringValue(results.GetValue(results.GetOrdinal("PS_Vendor_ID")))
                    order.Sender_Email = getStringValue(results.GetValue(results.GetOrdinal("Email")))
                    If (order.Sender_Email.Equals("")) Then
                        order.Sender_Email = getStringValue(results.GetValue(results.GetOrdinal("TEmail")))
                    End If
                    order.Transfer_SubTeam = getIntValue(results.GetValue(results.GetOrdinal("Transfer_SubTeam")))
                    order.TransferToSubTeamName = getStringValue(results.GetValue(results.GetOrdinal("TransferToSubteamName")))
                    order.Cover_Page = getStringValue(results.GetValue(results.GetOrdinal("CoverPage")))
                    order.Full_Name = getStringValue(results.GetValue(results.GetOrdinal("FullName")))
                    order.Expected_Date = results.GetDateTime(results.GetOrdinal("Expected_Date"))
                    order.OrderDate = results.GetDateTime(results.GetOrdinal("OrderDate"))
                    order.QtyDiscount = getDblValue(results.GetValue(results.GetOrdinal("QuantityDiscount")))
                    order.DiscountType = getIntValue(results.GetValue(results.GetOrdinal("DiscountType")))
                    order.UserPhone = getStringValue(results.GetValue(results.GetOrdinal("Phone_Number")))
                    order.UserFaxNumber = getStringValue(results.GetValue(results.GetOrdinal("Fax_Number")))
                    order.UserTitle = getStringValue(results.GetValue(results.GetOrdinal("Title")))
                    order.ReceiveLocation_ID = CInt(results.GetValue(results.GetOrdinal("ReceiveLocation_ID")))
                    order.PODescription = getStringValue(results.GetValue(results.GetOrdinal("OrderHeaderDesc")))
                    order.FileType = getFileTypeValue(results.GetValue(results.GetOrdinal("FILE_TYPE")))
                    order.OverrideTransmissionMethod = results.GetBoolean(results.GetOrdinal("OverrideTransmissionMethod"))
                    order.OverrideTransmissionTarget = getStringValue(results.GetValue(results.GetOrdinal("OverrideTransmissionTarget")))
                    order.Vendor_Email = getStringValue(results.GetValue(results.GetOrdinal("EmailAddr")))
                    order.StoreNo = CInt(results.GetValue(results.GetOrdinal("Store_No")))

                    order.vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("CompanyName")))
                    order.Vendor_Fax = getStringValue(results.GetValue(results.GetOrdinal("Fax")))
                    order.vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("Address_Line_1")))
                    order.vendor.AddressLine2 = getStringValue(results.GetValue(results.GetOrdinal("Address_Line_2")))
                    order.vendor.City = getStringValue(results.GetValue(results.GetOrdinal("City")))
                    order.vendor.State = getStringValue(results.GetValue(results.GetOrdinal("State")))
                    order.vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("Zip_Code")))
                    order.vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("Country")))
                    order.vendor.Phone = getStringValue(results.GetValue(results.GetOrdinal("Phone")))
                    order.vendor.Key = getStringValue(results.GetValue(results.GetOrdinal("Vendor_Key")))

                    order.receiving_vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("RLCompanyName")))
                    order.Receiving_Vendor_Fax = getStringValue(results.GetValue(results.GetOrdinal("RLFax")))
                    order.receiving_vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("RLAddress_Line_1")))
                    order.receiving_vendor.AddressLine2 = getStringValue(results.GetValue(results.GetOrdinal("RLAddress_Line_2")))
                    order.receiving_vendor.City = getStringValue(results.GetValue(results.GetOrdinal("RLCity")))
                    order.receiving_vendor.State = getStringValue(results.GetValue(results.GetOrdinal("RLState")))
                    order.receiving_vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("RLZip_Code")))
                    order.receiving_vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("RLCountry")))
                    order.receiving_vendor.Phone = getStringValue(results.GetValue(results.GetOrdinal("RLPhone")))

                    order.purchase_vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("PLCompanyName")))
                    order.Purchase_Vendor_Fax = getStringValue(results.GetValue(results.GetOrdinal("PLFax")))
                    order.purchase_vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("PLAddress_Line_1")))
                    order.purchase_vendor.AddressLine2 = getStringValue(results.GetValue(results.GetOrdinal("PLAddress_Line_2")))
                    order.purchase_vendor.City = getStringValue(results.GetValue(results.GetOrdinal("PLCity")))
                    order.purchase_vendor.State = getStringValue(results.GetValue(results.GetOrdinal("PLState")))
                    order.purchase_vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("PLZip_Code")))
                    order.purchase_vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("PLCountry")))
                    order.purchase_vendor.Phone = getStringValue(results.GetValue(results.GetOrdinal("PLPhone")))

                End If

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return order
        End Function
        Public Shared Function getFileTypeValue(ByVal myObject As Object) As String
            If (myObject Is DBNull.Value) Then
                Return SendOrdersBO.HTML_FILE_TYPE
            Else
                If CStr(myObject).ToUpper.Equals(SendOrdersBO.EDI_FILE_TYPE) Then
                    Return SendOrdersBO.EDI_FILE_TYPE
                ElseIf CStr(myObject).ToUpper.Equals(SendOrdersBO.XML_FILE_TYPE) Then
                    Return SendOrdersBO.XML_FILE_TYPE
                ElseIf CStr(myObject).ToUpper.Equals(SendOrdersBO.ORD_FILE_TYPE) Then
                    Return SendOrdersBO.ORD_FILE_TYPE
                Else
                    Return SendOrdersBO.HTML_FILE_TYPE
                End If
            End If
        End Function
        Public Shared Function getStringValue(ByVal myObject As Object) As String
            If (myObject Is DBNull.Value) Then
                Return ""
            Else
                Return CStr(myObject)
            End If
        End Function
        Public Shared Function getIntValue(ByVal myObject As Object) As Integer
            If (myObject Is DBNull.Value) Then
                Return 0
            Else
                Return CInt(myObject)
            End If
        End Function
        Public Shared Function getDblValue(ByVal myObject As Object) As Double
            If (myObject Is DBNull.Value) Then
                Return 0
            Else
                Return CInt(myObject)
            End If
        End Function
        Public Shared Function getDecValue(ByVal myObject As Object) As Decimal
            If (myObject Is DBNull.Value) Then
                Return 0
            Else
                Return CDec(myObject)
            End If
        End Function
        Public Shared Function getLongValue(ByVal myObject As Object) As Long
            If (myObject Is DBNull.Value) Then
                Return 0
            Else
                Return CLng(myObject)
            End If
        End Function
        Public Shared Function getDateValue(ByVal myObject As Object) As Date
            If (myObject Is DBNull.Value) Then
                Return Nothing
            Else
                Return CDate(myObject)
            End If
        End Function
        Public Shared Function getBoolValue(ByVal myObject As Object) As Boolean
            If (myObject Is DBNull.Value) Then
                Return False
            Else
                Return CBool(myObject)
            End If
        End Function
        
        Public Shared Function GetPODetails(ByVal dOrderHeaderID As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim item As ItemCatalog.OrderItem = New ItemCatalog.OrderItem
            Dim listOfItems As ArrayList = New ArrayList
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("GetFaxOrderItemList", paramList)
                While results.Read
                    item.OrderItem_ID = getLongValue(results.GetValue(results.GetOrdinal("OrderItem_ID")))
                    item.Identifier = getStringValue(results.GetValue(results.GetOrdinal("Identifier")))
                    item.Item_Description = getStringValue(results.GetValue(results.GetOrdinal("Item_Description")))
                    item.QuantityOrdered = getDecValue(results.GetValue(results.GetOrdinal("QuantityOrdered")))
                    item.QuantityReceived = getDecValue(results.GetValue(results.GetOrdinal("QuantityReceived")))
                    item.Total_Weight = getDecValue(results.GetValue(results.GetOrdinal("Total_Weight")))
                    item.LineItemCost = getDecValue(results.GetValue(results.GetOrdinal("LineItemCost")))
                    item.LineItemFreight = getDecValue(results.GetValue(results.GetOrdinal("LineItemFreight")))
                    item.Package_Unit_Abbr = getStringValue(results.GetValue(results.GetOrdinal("Unit_Name")))
                    item.Package_Desc1 = getDecValue(results.GetValue(results.GetOrdinal("Package_Desc1")))
                    item.Package_Desc2 = getDecValue(results.GetValue(results.GetOrdinal("Package_Desc2")))
                    item.Package_Unit_ID = getLongValue(results.GetValue(results.GetOrdinal("Package_Unit")))
                    item.Cost = getDecValue(results.GetValue(results.GetOrdinal("Cost")))
                    item.SubTeamName = getStringValue(results.GetValue(results.GetOrdinal("SubTeam_Name")))
                    item.SubTeam_No = getLongValue(results.GetValue(results.GetOrdinal("SubTeam_No")))
                    listOfItems.Add(item)
                End While

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return listOfItems
        End Function

        Public Shared Function GetUNFIOrder(ByRef poheader As POHeaderBO) As POHeaderBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim item As ItemCatalog.OrderItem = New ItemCatalog.OrderItem
            Dim listOfItems As ArrayList = New ArrayList
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = poheader.OrderHeader_ID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("GetUNFIOrder", paramList)
                While results.Read
                    poheader.UNFIStore = getStringValue(results.GetValue(results.GetOrdinal("UNFI_Store")))
                    item.OrderItem_ID = getLongValue(results.GetValue(results.GetOrdinal("Item_ID")))
                    item.Identifier = getStringValue(results.GetValue(results.GetOrdinal("Identifier")))
                    item.QuantityOrdered = getDecValue(results.GetValue(results.GetOrdinal("QuantityOrdered")))
                    item.QuantityUnit = getLongValue(results.GetValue(results.GetOrdinal("QuantityUnit")))
                    listOfItems.Add(item)
                End While
                poheader.PODetailsList = listOfItems
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

            Return poheader
        End Function
        Public Shared Function GetANSOrderHeader(ByVal dOrderHeaderID As Integer) As POHeaderBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim poheader As POHeaderBO = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)
            Try
                results = factory.GetStoredProcedureDataReader("GetANSOrderHeader", paramList)
                If results.Read Then
                    poheader = New POHeaderBO
                    poheader.OrderDate = getDateValue(results.GetValue(results.GetOrdinal("OrderDate")))
                    poheader.OrderHeader_ID = getIntValue(results.GetValue(results.GetOrdinal("referenceNumber")))
                    poheader.PODescription = getStringValue(results.GetValue(results.GetOrdinal("comment")))
                    poheader.BusinessUnit = getIntValue(results.GetValue(results.GetOrdinal("customer")))
                    poheader.vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("VendCountry")))
                    poheader.vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("VendName")))
                    poheader.vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("VendAddress")))
                    poheader.vendor.City = getStringValue(results.GetValue(results.GetOrdinal("VendCity")))
                    poheader.vendor.State = getStringValue(results.GetValue(results.GetOrdinal("VendState")))
                    poheader.vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("VendZip")))
                    poheader.purchase_vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("billToCountry")))
                    poheader.purchase_vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("billToName")))
                    poheader.purchase_vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("billToAddress")))
                    poheader.purchase_vendor.State = getStringValue(results.GetValue(results.GetOrdinal("billToState")))
                    poheader.purchase_vendor.City = getStringValue(results.GetValue(results.GetOrdinal("billToCity")))
                    poheader.purchase_vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("billToZip")))
                    poheader.receiving_vendor.Country = getStringValue(results.GetValue(results.GetOrdinal("shipToCountry")))
                    poheader.receiving_vendor.CompanyName = getStringValue(results.GetValue(results.GetOrdinal("shipToName")))
                    poheader.receiving_vendor.AddressLine1 = getStringValue(results.GetValue(results.GetOrdinal("shipToAddress")))
                    poheader.receiving_vendor.City = getStringValue(results.GetValue(results.GetOrdinal("shipToCity")))
                    poheader.receiving_vendor.State = getStringValue(results.GetValue(results.GetOrdinal("shipToState")))
                    poheader.receiving_vendor.ZipCode = getStringValue(results.GetValue(results.GetOrdinal("shipToZip")))
                    poheader.Return_Order = getBoolValue(results.GetValue(results.GetOrdinal("credit")))

                End If

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

            Return poheader
        End Function
        Public Shared Function GetOrderEmail(ByVal orderHeaderId As Long) As Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim emailMap As Hashtable = New Hashtable
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderId
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)
            Try
                results = factory.GetStoredProcedureDataReader("GetOrderEmail", paramList)
                If results.Read Then
                    emailMap.Add(BUYER_EMAIL, getStringValue(results.GetValue(results.GetOrdinal("Email"))))
                    emailMap.Add(TL_EMAIL, getStringValue(results.GetValue(results.GetOrdinal("TEmail"))))
                    emailMap.Add(COMPANY, getStringValue(results.GetValue(results.GetOrdinal("CompanyName"))))
                End If
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return emailMap
        End Function
        Public Shared Function GetANSOrderItems(ByVal dOrderHeaderID As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim item As ItemCatalog.OrderItem = New ItemCatalog.OrderItem
            Dim listOfItems As ArrayList = New ArrayList
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = dOrderHeaderID
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("GetANSOrderItems", paramList)
                While results.Read
                    item = New ItemCatalog.OrderItem
                    item.VendorItem_ID = Trim(getStringValue(results.GetValue(results.GetOrdinal("vendItemId"))))
                    item.OrderItem_ID = getLongValue(results.GetValue(results.GetOrdinal("vendorPartNum")))
                    item.Identifier = Trim(getStringValue(results.GetValue(results.GetOrdinal("WFMSKU"))))
                    item.Item_Description = Trim(getStringValue(results.GetValue(results.GetOrdinal("productName"))))
                    item.QuantityOrdered = getDecValue(results.GetValue(results.GetOrdinal("quantity")))
                    item.Package_Unit_Abbr = Trim(getStringValue(results.GetValue(results.GetOrdinal("UOM"))))
                    item.Package_Desc1 = getDecValue(results.GetValue(results.GetOrdinal("casePack"))) ' This is the vendor's case pack.
                    item.Package_Desc2 = getDecValue(results.GetValue(results.GetOrdinal("packSize")))
                    ' 1/4/09 Tom Lux: Unit cost was being stored in the OrderItem class 'cost' field, but this is more appropriately the
                    ' vendor unit cost (usually a case), as in the OrderItem table's 'cost' column.
                    ' No other classes besides SendOrdersBO are using this function, so it's safe to change.
                    ' I changed this to set 'UnitCost' and 'Cost' is set to the value in OrderItem table's 'cost' column below.
                    item.UnitCost = getDecValue(results.GetValue(results.GetOrdinal("USPrice")))
                    item.SubTeam_No = getLongValue(results.GetValue(results.GetOrdinal("posDept")))
                    item.Comments = Trim(getStringValue(results.GetValue(results.GetOrdinal("comment"))))
                    ' 1/4/09 Tom Lux: Added fields below.
                    item.BrandName = Trim(getStringValue(results.GetValue(results.GetOrdinal("BrandName"))))
                    item.ItemCasePack = getDecValue(results.GetValue(results.GetOrdinal("ItemCasePack"))) ' This is the # of units in an item pack.  Ex: 6 x 12oz cans of soda, so itemCasePack is 6.
                    item.Origin = Trim(getStringValue(results.GetValue(results.GetOrdinal("Origin"))))
                    item.CountryOfProcessing = Trim(getStringValue(results.GetValue(results.GetOrdinal("CountryOfProcessing"))))
                    item.ItemUnit = Trim(getStringValue(results.GetValue(results.GetOrdinal("ItemUOMName"))))
                    item.VendorOrderUnitName = Trim(getStringValue(results.GetValue(results.GetOrdinal("VendorOrderUOMName")))) ' In Item class because order unit comes from item data, not vendor.
                    item.ItemAllowanceDiscountAmount = getDecValue(results.GetValue(results.GetOrdinal("SumAllowances"))) + getDecValue(results.GetValue(results.GetOrdinal("SumDiscounts")))
                    item.QuantityDiscount = getDecValue(results.GetValue(results.GetOrdinal("QuantityDiscount")))
                    item.DiscountLabel = getStringValue(results.GetValue(results.GetOrdinal("DiscountType")))
                    item.AdjustedCost = getDecValue(results.GetValue(results.GetOrdinal("AdjustedCost")))
                    item.Cost = getDecValue(results.GetValue(results.GetOrdinal("VendorOrderUOMCost")))
                    item.LineItemCost = getDecValue(results.GetValue(results.GetOrdinal("NetLineItemCost"))) ' Qty ordered * (UOM cost - allowances/discounts).
                    listOfItems.Add(item)
                End While

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
            Return listOfItems
        End Function
        Public Shared Sub SystemDateTime(ByRef dDate As Date, ByRef dTime As Date)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                results = factory.GetStoredProcedureDataReader("GetSystemDate")
                If results.Read Then
                    dDate = results.GetDateTime(results.GetOrdinal("SystemDate"))
                End If

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

        End Sub
    End Class
End Namespace
