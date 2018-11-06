Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports System.ComponentModel
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Ordering.DataAccess
    Public Class OrderingDAO
        
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function LoadSubTeamGLAcct(ByVal OrderHeader_Id As Integer) As BindingList(Of SubTeamBO)
            Dim subTeamGLAcctList As New BindingList(Of SubTeamBO)
            Dim subTeamGLAcct As SubTeamBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            logger.Debug("LoadSubTeamGLAcct Entry")

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_Id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("OrderInvoice_GetGLAcctSubteams", paramList)

                While results.Read
                    subTeamGLAcct = New SubTeamBO()
                    subTeamGLAcct.SubTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
                    subTeamGLAcct.SubTeamName = results.GetString(results.GetOrdinal("SubTeamGLAcct"))

                    subTeamGLAcctList.Add(subTeamGLAcct)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("LoadSubTeamGLAcct Exit")

            Return subTeamGLAcctList
        End Function
        Public Shared Function LoadTotalRefusedAmount(ByVal OrderHeader_Id As Integer) As Decimal
            Dim refusedTotal As Decimal
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim outputList As ArrayList
            Dim currentParam As DBParam

            Try
                ' Execute the stored procedure 
                logger.Debug("LoadTotalRefusedAmount entry")

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_Id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' -- output --
                currentParam = New DBParam
                currentParam.Name = "@TotalRefusedAmount"
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                outputList = factory.ExecuteStoredProcedure("dbo.GetTotalRefused", paramList)
                refusedTotal = CDec(outputList(0))

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("LoadTotalRefusedAmount Exit")

            Return refusedTotal
        End Function


        Public Shared Function LoadOrderInvoiceSACTotal(ByVal OrderHeader_Id As Integer) As Decimal
            Dim invoiceSACTotal As Decimal
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim outputList As ArrayList
            Dim currentParam As DBParam

            Try
                ' Execute the stored procedure 
                logger.Debug("LoadOrderInvoiceSACTotal Entry")

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeader_Id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' -- output --
                currentParam = New DBParam
                currentParam.Name = "@SACTotal"
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_GetOrderInvoiceSACTotal", paramList)
                invoiceSACTotal = CDec(outputList(0))

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("LoadOrderInvoiceSACTotal Exit")

            Return invoiceSACTotal
        End Function

        Public Shared Function IsOrderEinvoice(ByVal OrderHeader_Id As Integer, ByRef EinvoiceId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure 
            logger.Debug("IsOrderEinvoice entry")

            EinvoiceId = CType(factory.ExecuteScalar(String.Format("SELECT dbo.fn_IsEinvoice({0})", OrderHeader_Id.ToString())), Integer)

            Return Not (EinvoiceId = -1)

            logger.Debug("IsOrderEinvoice Exit")
        End Function

        Public Shared Function IsVendorEinvoice(ByVal OrderHeaderId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure 
            logger.Debug("IsVendorEinvoice entry")

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_IsVendorEInvoice(" & OrderHeaderId & ")"), Boolean)

            logger.Debug("IsVendorEinvoice Exit")
        End Function

        Public Shared Function LoadOrderInvoiceSpecChargeTable(ByVal OrderHeader_Id As Integer) As DataTable
            Dim invoiceSpecChargesList As New DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dr As DataRow = Nothing


            invoiceSpecChargesList.Columns.Add("Charge_ID", GetType(Integer))
            invoiceSpecChargesList.Columns.Add("SubTeamGLAcct", GetType(String))
            ' Execute the stored procedure 
            logger.Debug("OrderInvoice_GetOrderInvoiceSpecCharges entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            invoiceSpecChargesList = factory.GetStoredProcedureDataTable("OrderInvoice_GetOrderInvoiceSpecCharges", paramList)

            logger.Debug("LoadSubTeamGLAcct Exit")

            Return invoiceSpecChargesList
        End Function

        ''' <summary>
        ''' Update the invoice and document data in the OrderHeader table.
        ''' The stored procedure enforces order state business rules, so the save may not succeed.  The return status
        ''' should be checked.
        ''' </summary>
        ''' <param name="invoiceBO"></param>
        ''' <param name="transaction"></param>
        ''' <returns>TRUE if the save was a success;  FALSE otherwise</returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateOrderHeaderInvoiceData(ByRef invoiceBO As OrderHeaderInvoiceBO, ByRef transaction As SqlTransaction) As Boolean
            logger.Debug("UpdateOrderHeaderInvoiceData entry: OrderHeader_ID=" + invoiceBO.OrderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim changeAllowed As Boolean

            ' Execute the stored procedures
            '-- Save the invoice or document data to the OrderHeader table
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = invoiceBO.OrderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            If Not invoiceBO.InvoiceNumber Is Nothing Then
                currentParam.Value = invoiceBO.InvoiceNumber
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            If Not invoiceBO.InvoiceDate = Nothing Then
                currentParam.Value = invoiceBO.InvoiceDate.ToString(ResourcesIRMA.GetString("DateStringFormat"))
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorDoc_ID"
            If Not invoiceBO.VendorDocId Is Nothing Then
                currentParam.Value = invoiceBO.VendorDocId
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorDocDate"
            If Not invoiceBO.VendorDocDate = Nothing Then
                currentParam.Value = invoiceBO.VendorDocDate.ToString(ResourcesIRMA.GetString("DateStringFormat"))
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PartialShipment"
            If invoiceBO.PartialShippment = True Then
                currentParam.Value = invoiceBO.PartialShippment
            Else
                currentParam.Value = False
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ChangeAllowed"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("dbo.UpdateOrderStatus", paramList, transaction)
            changeAllowed = CBool(outputList(0))

            logger.Debug("UpdateOrderHeaderInvoiceData exit: changeAllowed=" + changeAllowed.ToString)
            Return changeAllowed
        End Function

        ''' <summary>
        ''' Update OrderHeader to save Receiving Refusal Reason Code and Close the order.
        ''' </summary>
        ''' <param name="OrderHeader_id"></param>
        ''' <param name="RefusalID"></param>
        ''' <remarks></remarks>
        Public Shared Sub RefuseReceivingAndClose(ByVal OrderHeader_ID As Integer, ByVal RefusalID As Integer)
            logger.Debug("RefuseReceivingAndClose entry: OrderHeader_ID=" + OrderHeader_ID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RefuseReceivingReasonID"
            If RefusalID = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = RefusalID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateOrderHeaderRefuseReceivingAndClose", paramList)

            logger.Debug("RefuseReceivingAndClose exit")

        End Sub

        ''' <summary>
        ''' Move a SUSPENED invoice to the APPROVED state, using the logic to pay by the purchase order amount.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <remarks></remarks>
        Public Shared Sub ApproveInvoiceUsingPayByPurchaseOrderOption(ByVal orderHeaderID As Integer, ByVal ResolutionId As Integer)
            logger.Debug("ApproveInvoiceUsingPayByPurchaseOrderOption entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            '-- Update the order status to approved, setting the validation code = 502
            ' setup parameters for stored proc
            paramList = New ArrayList()
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Value = "502"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ResolutionCodeId"
            currentParam.Value = ResolutionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateOrderApproved", paramList)

            logger.Debug("ApproveInvoiceUsingPayByPurchaseOrderOption exit")
        End Sub

        Public Shared Sub ApproveLineItem(ByVal iOrderItemID As Integer, ByVal iResolutionCodeId As Integer, ByVal blnPayByPO As Boolean, ByVal iPaymentTypeID As Integer)
            logger.Debug("ApproveLineItemUsingPayByPurchaseOrderOption entry: OrderItem_ID=" + iOrderItemID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            '-- Update the order status to approved, setting the validation code = 502
            ' setup parameters for stored proc
            paramList = New ArrayList()
            currentParam = New DBParam
            currentParam.Name = "OrderItemID"
            currentParam.Value = iOrderItemID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ResolutionCodeId"
            currentParam.Value = iResolutionCodeId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PayByAgreedCost"
            currentParam.Value = blnPayByPO
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PaymentTypeID"
            currentParam.Value = iPaymentTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateLineItemApproved", paramList)

            logger.Debug("ApproveLineItemUsingPayByPurchaseOrderOption exit")
        End Sub

        ''' <summary>
        ''' Move a SUSPENED invoice to the APPROVED state, using the logic to pay by the invoice amount.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <remarks></remarks>
        Public Shared Sub ApproveInvoiceUsingPayByInvoiceOption(ByVal orderHeaderID As Integer, ByVal ResolutionId As Integer)
            logger.Debug("ApproveInvoiceUsingPayByInvoiceOption entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            '-- Update the order status to approved, setting the validation code = 503
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Value = "503"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ResolutionCodeId"
            currentParam.Value = ResolutionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateOrderApproved", paramList)

            logger.Debug("ApproveInvoiceUsingPayByInvoiceOption exit")
        End Sub

        ''' <summary>
        ''' Move a SUSPENED invoice to the APPROVED state that has document data instead of invoice data.
        ''' This order will not be uploaded to PeopleSoft for payment.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <remarks></remarks>
        Public Shared Sub ApproveInvoiceUsingDocumentDataOption(ByVal orderHeaderID As Integer, ByVal resolutionId As Integer)
            logger.Debug("ApproveInvoiceUsingDocumentDataOption entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            '-- Update the order status to approved, setting the validation code = 506
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Value = "506"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ResolutionCodeId"
            currentParam.Value = resolutionId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateOrderApproved", paramList)

            logger.Debug("ApproveInvoiceUsingDocumentDataOption exit")
        End Sub

        ''' <summary>
        ''' Move a SUSPENED invoice to the APPROVED state that has document data instead of invoice data.
        ''' This order will not be uploaded to PeopleSoft for payment.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <remarks></remarks>
        Public Shared Sub ApproveInvoiceFromLineItem(ByVal orderHeaderID As Integer)
            logger.Debug("ApproveInvoiceUsingDocumentDataOption entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            '-- Update the order status to approved, setting the validation code = 506
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ResolutionCodeId"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.UpdateOrderApproved", paramList)

            logger.Debug("ApproveInvoiceUsingDocumentDataOption exit")
        End Sub

        ''' <summary>
        ''' Delete all of the OrderInvoice records associated with an OrderHeader_ID.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteOrderInvoices(ByVal orderHeaderID As Integer, ByRef transaction As SqlTransaction)
            logger.Debug("DeleteOrderInvoices entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("dbo.DeleteOrderInvoice", paramList, transaction)
            logger.Debug("DeleteOrderInvoices exit")
        End Sub

        ''' <summary>
        ''' Create a new OrderInvoice record.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <param name="subteamNo"></param>
        ''' <param name="invoiceCost"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Shared Sub CreateOrderInvoice(ByVal orderHeaderID As Integer, ByVal subteamNo As Integer, ByVal invoiceCost As String, ByRef transaction As SqlTransaction)
            logger.Debug("CreateOrderInvoice entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = subteamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = invoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("dbo.InsertOrderInvoice", paramList, transaction)
            logger.Debug("CreateOrderInvoice exit")
        End Sub

        '' <summary>
        '' Distribute the invoice freight value for an order invoice among all of the OrderItem records.
        '' </summary>
        '' <param name="orderHeaderID"></param>
        '' <param name="invoiceFreightTotal"></param>
        '' <param name="transaction"></param>
        '' <remarks></remarks>
        Public Shared Sub DistributeInvoiceFreight(ByVal orderHeaderID As Integer, ByVal invoiceFreightTotal As String, ByRef transaction As SqlTransaction)
            '--logger.Debug("DistributeInvoiceFreight entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TotInvFrght"
            currentParam.Value = invoiceFreightTotal
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("dbo.DistInvFrght", paramList, transaction)
            'logger.Debug("DistributeInvoiceFreight exit")
        End Sub

        ''' <summary>
        ''' Update the currency of an order.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <param name="currencyID"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateOrderCurrency(ByVal orderHeaderID As Integer, ByVal currencyID As Integer, ByRef transaction As SqlTransaction)
            logger.Debug("UpdateOrderCurrency entry: OrderHeader_ID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CurrencyID"
            currentParam.Value = currencyID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("dbo.UpdateOrderCurrency", paramList, transaction)
            logger.Debug("UpdateOrderCurrency exit")
        End Sub
        ''' <summary>
        ''' Check to see if any item has been received fully or partially. The received value must be greater than 0 
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <returns>TRUE if any item have been received; FALSE otherwise</returns>
        ''' <remarks></remarks>
        Public Shared Function AnyOrderItemReceived(ByVal orderHeaderID As Integer) As Boolean
            logger.Debug("AnyOrderItemReceived entry: orderHeaderID=" + orderHeaderID.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim anyThingReceived As Boolean

            Try
                ' Execute the stored procedures
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = orderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("dbo.CountReceivedOrderItems", paramList)

                Dim receivedCount As Integer = 0
                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("ReceivedOrderItemCount"))) Then
                        receivedCount = results.GetInt32(results.GetOrdinal("ReceivedOrderItemCount"))
                    End If
                End While

                If receivedCount > 0 Then
                    anyThingReceived = True
                Else
                    anyThingReceived = False
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("AnyOrderItemReceived exit: AnyOrderItemReceived=" + anyThingReceived.ToString)
            Return anyThingReceived
        End Function
        ''' <summary>
        ''' Check to see if all items for an order have been fully received.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <returns>TRUE if all items have been received; FALSE otherwise</returns>
        ''' <remarks></remarks>
        Public Shared Function IsOrderReceivingComplete(ByVal orderHeaderID As Integer) As Boolean
            logger.Debug("IsOrderReceivingComplete entry: orderHeaderID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isReceivingComplete As Boolean

            Try
                ' Execute the stored procedures
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = orderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("dbo.CountOrderItems", paramList)

                Dim notReceivedCount As Integer = 0
                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("OrderItemCount"))) Then
                        notReceivedCount = results.GetInt32(results.GetOrdinal("OrderItemCount"))
                    End If
                End While

                ' If there are order items that have not been fully received (QuantityOrdered > QuantityReceived), 
                ' the count will be greater than zero.
                If notReceivedCount > 0 Then
                    isReceivingComplete = False
                Else
                    isReceivingComplete = True
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("IsOrderReceivingComplete exit: isReceivingComplete=" + isReceivingComplete.ToString)
            Return isReceivingComplete
        End Function


        ''' <summary>
        ''' Check to see if ANY items for an order have been  received.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <returns>TRUE if ANY items have been received; FALSE otherwise</returns>
        ''' <remarks>Bug 4737 4/23/3012 VA </remarks>
        Public Shared Function ReceivedItemsExist(ByVal orderHeaderID As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute function 
            logger.Debug("ReceivedItemsExist entry")

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_ReceivedItemsExist(" & orderHeaderID & ")"), Boolean)

            logger.Debug("ReceivedItemsExist Exit")
        End Function
        'TFS 7548 Faisal Ahmed
        'This function closes PO automatically for Intra-Store Transfers and returns true if successful
        '
        Public Shared Function AutoCloseIntraStoreTransfer(ByVal orderHeaderID As Integer, ByVal iUser_ID As Integer) As Boolean
            logger.Debug("AutoCloseIntraStoreTransfer entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim success As Boolean

            ' Execute the stored procedures
            '-- Save the invoice or document data to the OrderHeader table
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = iUser_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "@IsSuccessful"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("dbo.AutoCloseIntraStoreTransfer", paramList)
            success = CBool(outputList(0))

            logger.Debug("AutoCloseIntraStoreTransfer exit: Success=" + success.ToString)
            Return success
        End Function

        ''' <summary>
        ''' Move an order from the SENT state to the CLOSED state.  This will result in the order
        ''' being in the APPROVED or SUSPENDED state, based on the result of three way matching.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <returns>Flag to let the user know if the order is suspended (TRUE) or approved (FALSE)</returns>
        ''' <remarks></remarks>
        Public Shared Function CloseOrder(ByVal orderHeaderID As Integer) As Boolean
            logger.Debug("CloseOrder entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim isSuspended As Boolean

            ' Start the database transaction
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            ' Execute the stored procedures
            '-- Close the order
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "IsSuspended"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("dbo.UpdateOrderClosed", paramList, transaction)
            If Not outputList(0).Equals(DBNull.Value) Then
                isSuspended = CBool(outputList(0))
            End If

            '-- Update any origins that were entered
            ' setup parameters for stored proc
            paramList = New ArrayList()
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.AutomaticOrderOriginUpdate", paramList, transaction)

            ' Commit the database transaction
            transaction.Commit()

            logger.Debug("CloseOrder exit: isSuspended=" + isSuspended.ToString)
            Return isSuspended
        End Function

        ''' <summary>
        ''' Re-open an order that is in the closed state.
        ''' </summary>
        ''' <param name="orderHeaderID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReOpenClosedOrder(ByVal orderHeaderID As Integer) As Boolean
            logger.Debug("ReOpenClosedOrder entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim success As Boolean

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "Success"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.UpdateOrderOpen", paramList)
            success = CBool(outputList(0))
            logger.Debug("ReOpenClosedOrder exit: success=" + success.ToString)
            Return success
        End Function

        ''' <summary>
        ''' This function verifies that the order state being used by the application matches the current state of
        ''' the order stored in the database.
        ''' </summary>
        ''' <param name="isOrderClosed"></param>
        ''' <param name="isOrderApproved"></param>
        ''' <param name="isOrderUploaded"></param>
        ''' <param name="orderHeaderID"></param>
        ''' <returns>TRUE if the application and database are in sync; FALSE otherwise</returns>
        ''' <remarks></remarks>
        Public Shared Function IsOrderStateCurrent(ByVal isOrderClosed As Boolean, ByVal isOrderApproved As Boolean, ByVal isOrderUploaded As Boolean, ByVal orderHeaderID As Integer) As Boolean
            logger.Debug("IsOrderStateCurrent entry: isOrderClosed=" + isOrderClosed.ToString + ", isOrderApproved=" + isOrderApproved.ToString + ", isOrderUploaded=" + isOrderUploaded.ToString + ", orderHeaderID=" + orderHeaderID.ToString)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataSet
            Dim isDBClosed As Boolean = False
            Dim isDBApproved As Boolean = False
            Dim isDBUploaded As Boolean = False

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("dbo.GetOrderStatus", paramList)

            For Each row As DataRow In results.Tables(0).Rows
                isDBClosed = Not row.IsNull("CloseDate")
                isDBApproved = Not row.IsNull("ApprovedDate")
                isDBUploaded = Not (row.IsNull("UploadedDate") And row.IsNull("AccountingUploadDate"))
            Next

            If (isOrderClosed = isDBClosed) AndAlso (isOrderApproved = isDBApproved) AndAlso (isOrderUploaded = isDBUploaded) Then
                ' The application and database are in sync.
                logger.Info("IsOrderStateCurrent - The order state in the database matches the application order state: OrderHeader.OrderHeader_ID=" + orderHeaderID.ToString)
                Return True
            Else
                ' The application and database are not in sync.
                logger.Info("IsOrderStateCurrent - The order state in the database does not match the application order state: OrderHeader.OrderHeader_ID=" + orderHeaderID.ToString)
                Return False
            End If
        End Function

        Public Shared Function SearchForSuspendedOrders(ByRef searchData As OrderSearchBO, ByRef sStoreList As String, ByRef sState As String, ByRef iZoneID As Integer) As ArrayList
            logger.Debug("GetSuspendedOrderSearch entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Dim results As SqlDataReader = Nothing
            Dim returnArray As New ArrayList

            Dim remediatedSuspendedPOList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                If searchData.OrderHeaderID = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = searchData.OrderHeaderID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderInvoice_ControlGroup_ID"
                If searchData.OrderInvoiceControlGroupID = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = searchData.OrderInvoiceControlGroupID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                If searchData.VendorID = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = searchData.VendorID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_Key"
                If searchData.VendorKey = String.Empty Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = searchData.VendorKey
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvoiceNumber"
                If searchData.InvoiceNumber Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = searchData.InvoiceNumber
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvoiceDateStart"
                If searchData.InvoiceDateStart <> Date.MinValue Then
                    currentParam.Value = searchData.InvoiceDateStart
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvoiceDateEnd"
                If searchData.InvoiceDateEnd <> Date.MinValue Then
                    currentParam.Value = searchData.InvoiceDateEnd
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderDateStart"
                If searchData.OrderDateStart <> Date.MinValue Then
                    currentParam.Value = searchData.OrderDateStart
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderDateEnd"
                If searchData.OrderDateEnd <> Date.MinValue Then
                    currentParam.Value = searchData.OrderDateEnd
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VIN"
                If searchData.VIN <> Nothing Then
                    currentParam.Value = searchData.VIN
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                If searchData.Identifier <> Nothing Then
                    currentParam.Value = searchData.Identifier
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = sStoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EInvoiceOnly"
                currentParam.Value = searchData.EInvoiceOnly
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ResolutionCodeID"
                currentParam.Value = searchData.ResolutionCodeID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("dbo.GetSuspendedOrderSearch", paramList)
                Dim currentOrder As AllSuspendedInvoiceBO

                While results.Read
                    ' Add a BO for this record to the array
                    currentOrder = New AllSuspendedInvoiceBO(results)
                    remediatedSuspendedPOList.Add(currentOrder)
                End While

                returnArray.Add(remediatedSuspendedPOList)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetSuspendedOrderSearch exit")
            Return returnArray
        End Function

        ''' <summary>
        ''' Add a new charge to a control group invoice.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub InsertControlGroupInvoiceCharge(ByRef OrderHeader_Id As Integer, ByRef SACType_ID As Integer, ByRef Description As String, ByRef SubTeam_No As Integer, ByRef isAllowance As Boolean, ByRef Value As Decimal)

            logger.Debug("InsertControlGroupInvoiceCharge entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_Id"
            currentParam.Value = OrderHeader_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SACType_ID"
            currentParam.Value = SACType_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Description"
            currentParam.Value = Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "isAllowance"
            currentParam.Value = isAllowance
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Value"
            currentParam.Value = Value
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("dbo.OrderInvoice_InsOrderInvoiceSpecCharge", paramList)

            logger.Debug("InsertControlGroupInvoiceCharge exit")
        End Sub
        ''' <summary>
        ''' Delete selected charge from a control group invoice.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteControlGroupInvoiceCharge(ByRef Charge_Id As Integer)

            logger.Debug("DeleteControlGroupInvoiceCharge entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Charge_Id"
            currentParam.Value = Charge_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("dbo.OrderInvoice_DelOrderInvoiceSpecCharge", paramList)

            logger.Debug("DeleteControlGroupInvoiceCharge exit")
        End Sub
        ''' <summary>
        ''' Delete selected charge from a control group invoice.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateAllocationLineItemCharge(ByRef OrderHeader_Id As Integer)

            logger.Debug("UpdateAllocationLineItemCharge entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_Id"
            currentParam.Value = OrderHeader_Id
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("dbo.EInvoicing_UpdAllocLineItemCharge", paramList)

            logger.Debug("UpdateAllocationLineItemCharge exit")
        End Sub
        ''' <summary>
        ''' Copy Existing Purchase Order
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CopyExistingPO(ByRef iOrderHeader_Id As Integer, ByVal iInvalidCopyPOItems_ID As Integer, ByVal dExpectedDate As Date, ByVal iUser_ID As Integer, ByVal iCopyToStoreNo As Integer, ByVal iIsDeleted As Boolean) As String()
            logger.Debug("CopyExistingPO entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim itemList() As String
            Dim itemArrayList As New ArrayList
            Dim index As Integer = 0
            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_Id"
                currentParam.Value = iOrderHeader_Id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvalidCopyPOItems_ID"
                currentParam.Value = iInvalidCopyPOItems_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ExpectedDate"
                currentParam.Value = dExpectedDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = iUser_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CopyToStoreNo"
                currentParam.Value = iCopyToStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsDeleted"
                currentParam.Value = iIsDeleted
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("CopyExistingPO", paramList)

                While results.Read
                    If results.IsDBNull(results.GetOrdinal("CreditReason_ID")) Then
                        itemArrayList.Add(CStr(results.GetInt32(results.GetOrdinal("Item_Key"))) & "^" & CStr(results.GetDecimal(results.GetOrdinal("QuantityOrdered"))) & "^" & "NONE")
                    Else
                        itemArrayList.Add(CStr(results.GetInt32(results.GetOrdinal("Item_Key"))) & "^" & CStr(results.GetDecimal(results.GetOrdinal("QuantityOrdered"))) & "^" & CStr(results.GetInt32(results.GetOrdinal("CreditReason_ID"))))
                    End If

                End While

            Finally
                factory = Nothing
            End Try

            ReDim itemList(itemArrayList.Count - 1)
            For index = 0 To itemArrayList.Count - 1
                itemList(index) = CStr(itemArrayList.Item(index))
            Next

            Return itemList

            logger.Debug("CopyExistingPO exit")
        End Function

        Public Shared Sub UpdateOrderInfo(ByVal iOrderHeader_Id As Integer, _
                                          ByVal iTemperature As Integer?, _
                                          ByVal fQuantityDiscount As Decimal, _
                                          ByVal dExpectedDate As Date, _
                                          ByVal iDiscountType As Integer, _
                                          ByVal sReasonCodeDetailID As String, _
                                          ByVal iPurchaseLocation_ID As Integer, _
                                          ByVal iReceiveLocation_ID As Integer, _
                                          ByVal bFax_Order As Boolean, _
                                          ByVal bEmail_Order As Boolean, _
                                          ByVal bElectronic_Order As Boolean, _
                                          ByVal bReturn_Order As Boolean, _
                                          ByVal iOrigOrderHeader_ID As Integer, _
                                          ByVal iUser_ID As Integer, _
                                          ByVal iDropShip As Integer)
            logger.Debug("UpdateOrderInfo entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            'Dim results As SqlDataReader = Nothing

            Try

                ' Start the database transaction
                Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_Id"
                currentParam.Value = iOrderHeader_Id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Temperature"
                If iTemperature.HasValue Then
                    currentParam.Value = iTemperature.Value
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "QuantityDiscount"
                currentParam.Value = fQuantityDiscount
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Expected_Date"
                currentParam.Value = dExpectedDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscountType"
                currentParam.Value = iDiscountType
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ReasonCodeDetailID"
                currentParam.Value = sReasonCodeDetailID
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PurchaseLocation_ID"
                currentParam.Value = iPurchaseLocation_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)


                currentParam = New DBParam
                currentParam.Name = "ReceiveLocation_ID"
                currentParam.Value = iReceiveLocation_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Fax_Order"
                currentParam.Value = bFax_Order
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Email_Order"
                currentParam.Value = bEmail_Order
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Electronic_Order"
                currentParam.Value = bElectronic_Order
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Return_Order"
                currentParam.Value = bReturn_Order
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrigOrderHeader_ID"
                If iOrigOrderHeader_ID = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iOrigOrderHeader_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = iUser_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DropShip"
                currentParam.Value = iDropShip
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("UpdateOrderInfo", paramList, transaction)

                ' Commit the database transaction
                transaction.Commit()

            Finally
                factory = Nothing
            End Try

            logger.Debug("UpdateOrderInfo exit")
        End Sub

        ''' Overloaded version which sets all parameters to the default values and searches only by OrderHeader_ID
        Public Shared Function GetOrderSearch(ByVal lOrderHeader_ID As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As New DataTable
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = lOrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sent"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderStatus"
            currentParam.Value = 1
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderType_ID"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Type3"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Type4"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Created_By"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_SubTeam"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_To_SubTeam"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ReceiveLocation"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Description"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LotNo"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FromQueue"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EInvoice"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SourceID"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PartialShipment"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RefuseReceivingReasonID"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("GetOrderSearch", paramList)

            Return dt

        End Function

        Public Shared Function GetOrderSearch(ByVal lOrderHeader_ID As Integer, ByVal dOrderDate As Date, ByVal dSentDate As Date, ByVal sInvoiceNumber As String, ByVal iSend As Integer, ByVal iOrderStatus As Integer, _
                                              ByVal iOrderType_ID As Integer, ByVal iType3 As Integer, ByVal iType4 As Integer, ByVal sVendor As String, ByVal iCreatedBy As Integer, _
                                              ByVal lTransfer_SubTeam As Integer, ByVal lTransfer_To_SubTeam As Integer, ByVal iReceiveLocation As Integer, ByVal sidentifier As String, ByVal sItem_Description As String, _
                                              ByVal dWarehouseSentDate As Date, ByVal dExpectedDate As Date, ByVal sLotNo As String, ByVal iFromQueue As Integer, ByVal iEInvoice As Integer, _
                                              ByVal iSourceID As Integer, ByVal iPartialShipment As Integer, ByVal iRefusedPO As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim ds As DataSet = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = lOrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            If dOrderDate <> Nothing Then
                currentParam = New DBParam
                currentParam.Name = "OrderDate"
                currentParam.Value = dOrderDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If

            If dSentDate <> Nothing Then
                currentParam = New DBParam
                currentParam.Name = "SentDate"
                currentParam.Value = dSentDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If


            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = sInvoiceNumber
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sent"
            currentParam.Value = iSend
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderStatus"
            currentParam.Value = iOrderStatus
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderType_ID"
            currentParam.Value = iOrderType_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Type3"
            currentParam.Value = iType3
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Type4"
            currentParam.Value = iType4
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor"
            currentParam.Value = sVendor
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Created_By"
            currentParam.Value = iCreatedBy
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_SubTeam"
            currentParam.Value = lTransfer_SubTeam
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_To_SubTeam"
            currentParam.Value = lTransfer_To_SubTeam
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ReceiveLocation"
            currentParam.Value = iReceiveLocation
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = sidentifier
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Description"
            currentParam.Value = sItem_Description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            If dWarehouseSentDate <> Nothing Then
                currentParam = New DBParam
                currentParam.Name = "WarehouseSentDate"
                currentParam.Value = dWarehouseSentDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If

            If dExpectedDate <> Nothing Then
                currentParam = New DBParam
                currentParam.Name = "Expected_Date"
                currentParam.Value = dExpectedDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If


            currentParam = New DBParam
            currentParam.Name = "LotNo"
            currentParam.Value = sLotNo
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FromQueue"
            currentParam.Value = iFromQueue
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EInvoice"
            currentParam.Value = iEInvoice
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SourceID"
            currentParam.Value = iSourceID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PartialShipment"
            currentParam.Value = iPartialShipment
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RefuseReceivingReasonID"
            currentParam.Value = iRefusedPO
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("GetOrderSearch", paramList)

            Return ds.Tables(0)
        End Function

        Public Shared Function GetOrderItemList(ByVal sProcedure As String, ByVal iOrderHeaderId As Integer, ByVal sSubTeamNo As String, ByVal sDistSubTeamNo As String, ByVal sNotAvailable As String, ByVal iProductTypeId As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim ds As DataSet

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = iOrderHeaderId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SearchSubTeam_No"

            If UCase(sSubTeamNo) = "NULL" Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = sSubTeamNo
            End If

            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DistSubTeam_No"
            If UCase(sDistSubTeamNo) = "NULL" Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = sDistSubTeamNo
            End If

            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Not_Available"

            If UCase(sNotAvailable) = "NULL" Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = sNotAvailable
            End If

            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ProductType_ID"
            currentParam.Value = iProductTypeId
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet(sProcedure, paramList)
            Return ds
        End Function

        ''' <summary>
        ''' Returns integer ID for the user that is locking the specified PO, or -1 if NULL (no user lock).
        ''' </summary>
        ''' <param name="ponum">PO for which user ID lock will be retrieved.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetUserLockingPO(ByVal poNum As Integer) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' We don't have the item key, so we need to pass NULL.
            Dim result As Object = factory.ExecuteScalar(String.Format("exec dbo.GetOrderHeaderLockStatus {0}", poNum))
            If IsDBNull(result) Or result Is Nothing Then
                Return -1
            Else
                Return CType(result, Integer)
            End If
        End Function

        ''' <summary>
        ''' Sets the editor (lock owner) for a PO to the specified user.
        ''' </summary>
        ''' <param name="poNum">PO to lock.</param>
        ''' <param name="userId">User to hold lock for PO.</param>
        ''' <remarks></remarks>
        Public Shared Sub LockPO(ByVal poNum As Integer, ByVal userId As Integer)
            SQLExecute("EXEC LockOrderHeader " & poNum & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Sub

        ''' <summary>
        ''' Sets the editor (lock owner) for a PO to the specified user.
        ''' </summary>
        ''' <param name="poNum">PO to lock.</param>
        ''' <remarks></remarks>
        Public Shared Sub UnlockPO(ByVal poNum As Integer)
            SQLExecute("EXEC UnlockOrderHeader " & poNum, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Sub

        ''' <summary>
        ''' Insert a new OrderItem record.
        ''' </summary>
        ''' <param name="sqlScript">Stored proc name and the parameter values to be past to the stored proc.</param>
        ''' <returns>Identity key (OrderItem id for the inserted record)</returns>
        ''' <remarks></remarks>
        Public Shared Function InsertOrderItem(ByVal sqlScript As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim result As Object = factory.ExecuteScalar(sqlScript)
            If IsDBNull(result) Or result Is Nothing Then
                Return -1
            Else
                Return CType(result, Integer)
            End If

        End Function
        Public Shared Sub ModifyInReviewStatus(ByVal orderHeaderId As Integer, ByVal UserId As Integer, ByVal status As Integer)
            SQLExecute(String.Format("EXEC UpdateInReviewStatus {0},{1},{2}", orderHeaderId, UserId, status), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Sub

        Public Shared Function GetSuspendedPONotes(ByVal iOrderHeaderID As Integer, ByVal iOrderItemID As Integer) As String
            Dim dt As DataTable = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderHeaderID"

                If iOrderHeaderID = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iOrderHeaderID
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderItemID"

                If iOrderItemID = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iOrderItemID
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                dt = factory.GetStoredProcedureDataTable("GetSuspendedPONotes", paramList)

                Return dt.Rows(0)("AdminNotes").ToString
            Catch ex As Exception
                Throw ex
            End Try

            Return Nothing
        End Function

        Public Shared Sub UpdateSuspendedPONotes(ByVal iOrderHeaderID As Integer, ByVal iOrderItemID As Integer, ByVal sNotes As String, ByVal iResolutionCodeID As Integer)
            Dim dt As DataTable = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderHeaderID"

                If iOrderHeaderID = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iOrderHeaderID
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderItemID"

                If iOrderItemID = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iOrderItemID
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Notes"
                currentParam.Value = sNotes
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ResolutionCodeID"

                If iResolutionCodeID = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iResolutionCodeID
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("UpdateSuspendedPOAdminNotesAndResolution", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Function CheckVendorIsDSDForStore(ByVal Vendor_ID As Integer, ByVal Store_No As Integer) As Boolean
            logger.Debug("CheckVendorIsDSDForStore Entry")
            Dim sql As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnValue As Boolean
            returnValue = False

            Try
                sql = "EXEC CheckVendorIsDSDForStore @Vendor_ID = " + CType(Vendor_ID, String) + ", @Store_No = " + CType(Store_No, String)
                logger.Debug("executing: " + sql)
                Dim result As Object = factory.ExecuteScalar(sql)
                If IsDBNull(result) Or result Is Nothing Then
                    returnValue = False
                Else
                    returnValue = CType(result, Boolean)
                End If

            Catch ex As Exception
                logger.Error("CheckVendorIsDSDForStore ERROR: " + ex.Message)
                returnValue = False
            End Try

            logger.Debug("CheckVendorIsDSDForStore Exit")
            Return returnValue
        End Function

        Public Shared Function CheckDSDVendorWithPurchasingStore(ByVal Vendor_ID As Integer, ByVal Purchasing_Store_Vendor_ID As Integer) As Boolean
            logger.Debug("CheckDSDVendorWithPurchasingStore Entry")

            Dim sql As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnValue As Boolean = False
            Try
                sql = "EXEC CheckDSDVendorWithPurchasingStore @Vendor_ID = " + CType(Vendor_ID, String) + ", @Purchasing_Store_Vendor_ID = " + CType(Purchasing_Store_Vendor_ID, String)
                logger.Debug("executing: " + sql)
                Dim result As Object = factory.ExecuteScalar(sql)
                If IsDBNull(result) Or result Is Nothing Then
                    returnValue = False
                Else
                    returnValue = CType(result, Boolean)
                End If
            Catch ex As Exception
                logger.Error("CheckDSDVendorWithPurchasingStore: " + ex.Message)
                returnValue = False
            End Try

            logger.Debug("CheckDSDVendorWithPurchasingStore Exit")
            Return returnValue
        End Function

        Public Shared Sub UpdateOrderRefreshCosts(ByVal orderHeaderID As Integer, ByVal orderRefreshCostSource As String)
            logger.Debug("UpdateOrderRefreshCosts entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = orderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RefreshSource"
                currentParam.Value = orderRefreshCostSource
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("dbo.UpdateOrderRefreshCosts", paramList)

            Catch ex As Exception
                Throw ex
            End Try
            logger.Debug("UpdateOrderRefreshCosts exit")
        End Sub

        Public Shared Sub CheckOrderSuspension(ByVal orderHeaderID As Integer, ByVal userID As Integer)
            logger.Debug("CheckOrderSuspension entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for storec procedure
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = orderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = userID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                'Execute stored procedure
                factory.ExecuteStoredProcedure("dbo.MatchOrderInvoiceCosts", paramList)

            Catch ex As Exception
                Throw ex
            End Try
            logger.Debug("CheckOrderSuspension exit")
        End Sub

        'TFS 8312
        'This method was created to execute DeleteOrderHeader stored procedure
        Public Shared Function DeleteOrderHeader(ByVal orderHeaderID As Integer, ByVal userID As Integer, ByVal reasonCode As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for storec procedure
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = orderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = userID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DeletedReasonCode"
                currentParam.Value = reasonCode
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                'Execute stored procedure
                factory.ExecuteStoredProcedure("dbo.DeleteOrderHeader", paramList)
                Return True
            Catch ex As Exception
                logger.Error("cmdDeleteOrder_Click " & CStr(orderHeaderID) & " Delete Error in " & "EXEC DeleteOrderHeader" & ex.InnerException.ToString())
                MsgBox(Err.Description, MsgBoxStyle.Critical, ex.Message)
                logger.Debug("cmdDeleteOrder_Click Exit")
                Return False
            End Try
        End Function


        ''' <summary>
        ''' Insert a record into OrderItemRefused table
        ''' </summary>
        Public Shared Sub InsertOrderItemRefused( _
                        ByVal orderHeaderID As Integer, _
                        ByVal identifier As String, _
                        ByVal vendorItemNumber As String, _
                        ByVal description As String, _
                        ByVal unit As String, _
                        ByVal invoiceQuantity As Decimal, _
                        ByVal invoiceCost As Decimal, _
                        ByVal refusedQuantity As Decimal, _
                        ByVal reasonCode As Integer, _
                        ByVal userAddedEntry As Boolean)

            logger.Debug("InsertOrderItemRefused entry: OrderHeader_ID=" + orderHeaderID.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderItem_ID"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = identifier
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorItemNumber"
            currentParam.Value = vendorItemNumber
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Description"
            currentParam.Value = description
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Unit"
            currentParam.Value = unit
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceQuantity"
            currentParam.Value = invoiceQuantity
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = invoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RefusedQuantity"
            currentParam.Value = refusedQuantity
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DiscrepancyCodeID"
            currentParam.Value = reasonCode
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserAddedEntry"
            currentParam.Value = userAddedEntry
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "eInvoiceId"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("dbo.InsertOrderItemRefused", paramList)

            logger.Debug("InsertOrderItemRefused exit")
        End Sub
    End Class
End Namespace
