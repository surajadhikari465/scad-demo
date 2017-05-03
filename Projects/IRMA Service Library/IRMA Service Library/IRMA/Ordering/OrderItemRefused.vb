Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports log4net.Repository.Hierarchy
Imports System.Data.SqlClient

Namespace IRMA

    <DataContract()>
    Public Class OrderItemRefused

        <DataMember()>
        Public Property OrderItemRefusedID As Integer
        <DataMember()>
        Public Property Identifier As String
        <DataMember()>
        Public Property VendorItemNumber As String
        <DataMember()>
        Public Property Description As String
        <DataMember()>
        Public Property Unit As String
        <DataMember()>
        Public Property QuantityOrdered As Decimal
        <DataMember()>
        Public Property QuantityReceived As Decimal
        <DataMember()>
        Public Property eInvoiceQuantity As Decimal
        <DataMember()>
        Public Property eInvoiceCost As Decimal
        <DataMember()>
        Public Property InvoiceQuantity As Decimal
        <DataMember()>
        Public Property InvoiceCost As Decimal
        <DataMember()>
        Public Property RefusedQuantity As Decimal
        <DataMember()>
        Public Property DiscrepancyCodeID As Integer
        <DataMember()>
        Public Property UserAddedEntry As Boolean
        <DataMember()>
        Public Property eInvoice_ID As Integer


        Public Property ResultObject As Result

        Public Sub New()

        End Sub

        Public Function InsertOrderItemRefused( _
                        ByVal iOrderHeader_ID As Integer, _
                        ByVal iOrderItem_ID As Integer, _
                        ByVal sIdentifier As String, _
                        ByVal sVIN As String, _
                        ByVal sDescription As String, _
                        ByVal sUnit As String, _
                        ByVal dInvoiceQuantity As Decimal, _
                        ByVal dInvoiceCost As Decimal, _
                        ByVal reasonCodeID As Integer) As Result

            Common.logger.Info("InsertOrderItemRefused() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outParamList As New ArrayList
            Dim currentParam As DBParam

            Try
                Me.ResultObject = New Result()

                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = iOrderHeader_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderItem_ID"
                currentParam.Value = iOrderItem_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = sIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VendorItemNumber"
                currentParam.Value = sVIN
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Unit"
                currentParam.Value = sUnit
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = sDescription
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvoiceQuantity"
                currentParam.Value = dInvoiceQuantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvoiceCost"
                currentParam.Value = dInvoiceCost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RefusedQuantity"
                currentParam.Value = dInvoiceQuantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscrepancyCodeID"
                currentParam.Value = reasonCodeID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserAddedEntry"
                currentParam.Value = 1
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("InsertOrderItemRefused", paramList)

                Me.ResultObject.Load(iOrderHeader_ID, True, "InsertOrderItemRefused")

                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("InsertOrderItemRefused() failed for OrderHeader_ID " & iOrderHeader_ID)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function UpdateOrderItemRefused( _
                       ByVal iOrderItemRefused_ID As Integer, _
                       ByVal sIdentifier As String, _
                       ByVal sVIN As String, _
                       ByVal sDescription As String, _
                       ByVal sUnit As String, _
                       ByVal dInvoiceQuantity As Decimal, _
                       ByVal dInvoiceCost As Decimal, _
                       ByVal reasonCodeID As Integer, _
                       ByVal userAddedEntry As Integer) As Result

            Common.logger.Info("UpdateOrderItemRefused() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outParamList As New ArrayList
            Dim currentParam As DBParam

            Try
                Me.ResultObject = New Result()

                currentParam = New DBParam
                currentParam.Name = "OrderItemRefusedID"
                currentParam.Value = iOrderItemRefused_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = sIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VendorItemNumber"
                currentParam.Value = sVIN
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Unit"
                currentParam.Value = sUnit
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = sDescription
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Cost"
                currentParam.Value = dInvoiceCost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RefusedQuantity"
                currentParam.Value = dInvoiceQuantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscrepancyCodeID"
                currentParam.Value = reasonCodeID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserAddedEntry"
                currentParam.Value = userAddedEntry
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("UpdateOrderItemRefused", paramList)

                Me.ResultObject.Load(-1, True, "UpdateOrderItemRefused")

                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("UpdateOrderItemRefused() failed for OrderItemRefused_ID " & iOrderItemRefused_ID)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function DeleteOrderItemRefused(ByVal iOrderItemRefused_ID As Integer) As Result

            Common.logger.Info("DeleteOrderItemRefused() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outParamList As New ArrayList
            Dim currentParam As DBParam

            Try
                Me.ResultObject = New Result()

                currentParam = New DBParam
                currentParam.Name = "OrderItemRefusedID"
                currentParam.Value = iOrderItemRefused_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("DeleteOrderItemRefused", paramList)

                Me.ResultObject.Load(-1, True, "DeleteOrderItemRefused")

                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("DeleteOrderItemRefused() failed for OrderItemRefused_ID " & iOrderItemRefused_ID)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetOrderItemsRefused(ByVal OrderHeader_ID) As List(Of OrderItemRefused)

            Common.logger.Info("GetOrderItemsRefused() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As New DataTable
            Dim rlist As New List(Of OrderItemRefused)

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetOrderItemsRefused", paramList)

                For Each row As DataRow In dt.Rows
                    Dim oir As New OrderItemRefused

                    oir.OrderItemRefusedID = row.Item("OrderItemRefusedID")

                    oir.Identifier = row.Item("Identifier")
                    oir.Description = row.Item("Description")

                    If IsDBNull(row.Item("VendorItemNumber")) Then
                        oir.VendorItemNumber = 0
                    Else
                        oir.VendorItemNumber = row.Item("VendorItemNumber")
                    End If

                    oir.Unit = row.Item("Unit")

                    If IsDBNull(row.Item("DiscrepancyCodeID")) Then
                        oir.DiscrepancyCodeID = 0
                    Else
                        oir.DiscrepancyCodeID = row.Item("DiscrepancyCodeID")
                    End If
                    If IsDBNull(row.Item("QuantityOrdered")) Then
                        oir.QuantityOrdered = 0
                    Else
                        oir.QuantityOrdered = row.Item("QuantityOrdered")
                    End If

                    If IsDBNull(row.Item("QuantityReceived")) Then
                        oir.QuantityReceived = 0
                    Else
                        oir.QuantityReceived = row.Item("QuantityReceived")
                    End If

                    If IsDBNull(row.Item("eInvoiceQuantity")) Then
                        oir.eInvoiceQuantity = 0
                    Else
                        oir.eInvoiceQuantity = row.Item("eInvoiceQuantity")
                    End If

                    If IsDBNull(row.Item("eInvoiceCost")) Then
                        oir.eInvoiceCost = 0
                    Else
                        oir.eInvoiceCost = row.Item("eInvoiceCost")
                    End If

                    If IsDBNull(row.Item("InvoiceCost")) Then
                        oir.InvoiceCost = 0
                    Else
                        oir.InvoiceCost = row.Item("InvoiceCost")
                    End If

                    If IsDBNull(row.Item("InvoiceQuantity")) Then
                        oir.InvoiceQuantity = 0
                    Else
                        oir.InvoiceQuantity = row.Item("InvoiceQuantity")
                    End If

                    If IsDBNull(row.Item("RefusedQuantity")) Then
                        oir.RefusedQuantity = 0
                    Else
                        oir.RefusedQuantity = row.Item("RefusedQuantity")
                    End If

                    oir.UserAddedEntry = CBool(row.Item("UserAddedEntry"))

                    If IsDBNull(row.Item("eInvoice_ID")) Then
                        oir.eInvoice_ID = 0
                    Else
                        oir.eInvoice_ID = row.Item("eInvoice_ID")
                    End If
                    rlist.Add(oir)
                Next
                Return rlist

            Catch ex As Exception
                Common.logger.Info("GetOrderItemsRefused() failed for OrderHeader_ID " & OrderHeader_ID)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

            Return Nothing
        End Function

        Public Function UpdateRefusedItemsList(ByVal columnValuesList As String, ByVal separator1 As Char, ByVal separator2 As Char) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "DataList"
            currentParam.Value = columnValuesList
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
                al = factory.ExecuteStoredProcedure("UpdateRefusedItemsList", paramList)
 
                ResultObject.Load(-1, True, "UpdateRefusedItemsList")
                Return Me.ResultObject

            Catch ex As Exception
                Me.ResultObject = New Result()
                ResultObject.Load(-1, False, "UpdateRefusedItemsList", "UpdateRefusedItemsList() failed")
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function UpdateRefusedQuantity(ByVal orderHeaderID As Integer, ByVal Identifier As String, ByVal Quantity As Decimal) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = Identifier
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Quantity"
            currentParam.Value = Quantity
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            Try
                Me.ResultObject = New Result()
                al = factory.ExecuteStoredProcedure("UpdateRefusedQuantityByIdentifier", paramList)

                ResultObject.Load(-1, True, "UpdateRefusedQuantity")
                Return Me.ResultObject

            Catch ex As Exception
                Me.ResultObject = New Result()
                ResultObject.Load(-1, False, "UpdateRefusedQuantity", "UpdateRefusedQuantity() failed")
                Return Me.ResultObject

            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function GetRefusedTotal(ByVal OrderHeader_ID As Integer) As Decimal

            Common.logger.Info("GetRefusedTotal() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim outputList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim refusedTotal As Decimal

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@TotalRefusedAmount"
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            Try
                outputList = factory.ExecuteStoredProcedure("GetTotalRefused", paramList)
                refusedTotal = CType(outputList(0), Decimal)
                Return refusedTotal

            Catch ex As Exception
                Common.logger.Error("GetRefusedTotal failed.")
                Throw ex

            Finally
                connectionCleanup(factory)

            End Try

        End Function

    End Class
End Namespace