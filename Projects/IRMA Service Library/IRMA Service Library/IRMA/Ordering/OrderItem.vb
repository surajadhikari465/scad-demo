Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports log4net.Repository.Hierarchy
Imports System.Data.SqlClient

Namespace IRMA

    <DataContract()>
    Public Class OrderItem

        <DataMember()>
        Public Property OrderItem_ID As Integer
        <DataMember(isrequired:=True)>
        Public Property Item_Key As Integer
        <DataMember()>
        Public Property Identifier As String
        <DataMember()>
        Public Property Package_Desc1 As Decimal
        <DataMember()>
        Public Property Package_Desc2 As Decimal
        <DataMember(isrequired:=True)>
        Public Property QuantityUnit As Integer
        <DataMember(isrequired:=True)>
        Public Property QuantityOrdered As Decimal
        <DataMember()>
        Public Property QuantityReceived As Decimal
        <DataMember()>
        Public Property Total_Weight As Decimal
        <DataMember()>
        Public Property IsReceivedWeightRequired As Boolean
        <DataMember()>
        Public Property VendorItemID As String
        <DataMember()>
        Public Property Item_Description As String
        <DataMember()>
        Public Property LineItemCost As Decimal
        <DataMember()>
        Public Property LineItemFreight As Decimal
        <DataMember()>
        Public Property UOMUnitCost As Decimal
        <DataMember()>
        Public Property LineNumber As Integer
        <DataMember()>
        Public Property ItemAllowanceDiscountAmount As Decimal
        <DataMember(isrequired:=True)>
        Public Property QuantityDiscount As Decimal
        <DataMember(isrequired:=True)>
        Public Property DiscountType As Integer
        <DataMember()>
        Public Property CostUnit As Integer
        <DataMember()>
        Public Property UOMAdjustedUnitCost As Decimal
        <DataMember()>
        Public Property QuantityShipped As Decimal
        <DataMember()>
        Public Property CatchweightRequired As Boolean
        <DataMember()>
        Public Property Cost As Decimal
        <DataMember()>
        Public Property Freight As Decimal
        <DataMember()>
        Public Property ReceivedItemCost As Decimal
        <DataMember()>
        Public Property ReceivedItemFreight As Decimal
        <DataMember()>
        Public Property PackageUnitAbbr As String
        <DataMember()>
        Public Property OrderUOMAbbr As String
        <DataMember()>
        Public Property UnitCost As Decimal
        <DataMember()>
        Public Property Units_Per_Pallet As Integer
        <DataMember()>
        Public Property Package_Unit_ID As Integer
        <DataMember()>
        Public Property LineItemHandling As Decimal
        <DataMember()>
        Public Property UnitExtCost As Decimal
        <DataMember()>
        Public Property MarkupPercent As Decimal
        <DataMember()>
        Public Property MarkupCost As Decimal
        <DataMember()>
        Public Property FreightUnit As Integer
        <DataMember()>
        Public Property LandedCost As Decimal
        <DataMember()>
        Public Property Handling As Decimal
        <DataMember()>
        Public Property HandlingUnit As Integer
        <DataMember(isrequired:=True)>
        Public Property AdjustedCost As Decimal
        <DataMember()>
        Public Property NetVendorItemDiscount As Decimal
        <DataMember()>
        Public Property HandlingCharge As Decimal
        <DataMember()>
        Public Property CreditReason_ID As Integer
        <DataMember()>
        Public Property Retail_Unit_ID As Integer
        <DataMember(isrequired:=True)>
        Public Property ReasonCodeDetailID As Integer
        <DataMember()>
        Public Property eInvoiceQuantity As Decimal
        <DataMember()>
        Public Property eInvoiceWeight As Decimal
        <DataMember()>
        Public Property eInvoiceQuantityUnit As String
        <DataMember()>
        Public Property ReceivingDiscrepancyReasonCodeID As Integer
        <DataMember()>
        Public Property AlreadyClosed As Integer

        Public Property ResultObject As Result

        Public Sub New()

        End Sub

        Public Function InsertOrderItem(ByVal OrderHeader_ID As Integer, ByVal User_ID As Integer, ByVal Adjusted_Cost As Decimal, ByVal Adjusted_Reason As Integer) As Result
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                Me.ResultObject = New Result()

                AutomaticOrderItemInfo(OrderHeader_ID) 'This step loads the OrderItem object before insert. Sets Me.ResultObject if failed.

                If Me.ResultObject.IRMA_PONumber = -1 Then 'AutomaticOrderItemInfo failed
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
                currentParam.Name = "Item_Key"
                currentParam.Value = Item_Key 'Loaded by client
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Units_Per_Pallet"
                If Units_Per_Pallet <> Nothing Then
                    currentParam.Value = Units_Per_Pallet
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "QuantityUnit"
                currentParam.Value = QuantityUnit 'Loaded by client
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "QuantityOrdered"
                currentParam.Value = QuantityOrdered 'Loaded by client
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Cost"
                currentParam.Value = Cost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "CostUnit"
                currentParam.Value = CostUnit
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Handling"
                If Handling <> Nothing Then
                    currentParam.Value = Handling
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "HandlingUnit"
                If HandlingUnit <> Nothing Then
                    currentParam.Value = HandlingUnit
                Else
                    currentParam.Value = 1
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Freight"
                If Freight <> Nothing Then
                    currentParam.Value = Freight
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "FreightUnit"
                If FreightUnit <> Nothing Then
                    currentParam.Value = FreightUnit
                Else
                    currentParam.Value = 1
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "AdjustedCost"
                If Adjusted_Cost > 0 Then
                    currentParam.Value = Adjusted_Cost
                ElseIf AdjustedCost <> Nothing Then
                    currentParam.Value = AdjustedCost
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "QuantityDiscount"
                If QuantityDiscount <> Nothing Then
                    currentParam.Value = QuantityDiscount
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "DiscountType"
                If DiscountType <> Nothing Then
                    currentParam.Value = DiscountType
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "LandedCost"
                If LandedCost <> Nothing Then
                    currentParam.Value = LandedCost
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "LineItemCost"
                currentParam.Value = LineItemCost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "LineItemFreight"
                If LineItemFreight <> Nothing Then
                    currentParam.Value = LineItemFreight
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "LineItemHandling"
                If LineItemHandling <> Nothing Then
                    currentParam.Value = LineItemHandling
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "UnitCost"
                currentParam.Value = UnitCost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "UnitExtCost"
                currentParam.Value = UnitExtCost
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Package_Desc1"
                currentParam.Value = Package_Desc1
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Package_Desc2"
                currentParam.Value = Package_Desc2
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Package_Unit_ID"
                currentParam.Value = Package_Unit_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "MarkupPercent"
                If MarkupPercent <> Nothing Then
                    currentParam.Value = MarkupPercent
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "MarkupCost"
                If MarkupCost <> Nothing Then
                    currentParam.Value = MarkupCost
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "Retail_Unit_ID"
                If Retail_Unit_ID <> Nothing Then
                    currentParam.Value = Retail_Unit_ID
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "CreditReason_ID"
                currentParam.Value = CreditReason_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = User_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "VendorDiscountAmt"
                If NetVendorItemDiscount <> Nothing Then
                    currentParam.Value = NetVendorItemDiscount
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "HandlingCharge"
                If HandlingCharge <> Nothing Then
                    currentParam.Value = HandlingCharge
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)
                ' ******* Parameters ************
                currentParam = New DBParam
                currentParam.Name = "ReasonCodeDetailID"
                If Adjusted_Reason > 0 Then
                    currentParam.Value = Adjusted_Reason
                ElseIf ReasonCodeDetailID <> Nothing Then
                    currentParam.Value = ReasonCodeDetailID
                Else
                    currentParam.Value = Convert.DBNull.value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                Dim dt As DataTable = factory.GetStoredProcedureDataTable("InsertOrderItemCredit", paramList)

                If dt.Rows.Count > 0 Then
                    OrderItem_ID = dt.Rows(0).Item(0)
                End If

                Me.ResultObject.Load(OrderItem_ID, True, "InsertOrderItem")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Create Order failed! PONumber -" & OrderHeader_ID & "; " & factory.GetSQLString("InsertOrderItemCredit", paramList), ex)
                Me.ResultObject = New Result()
                Me.ResultObject.Load(-1, False, "InsertOrderItem", "InsertOrderItem Failed! PONumber -" & OrderHeader_ID, ex.ToString, factory.GetSQLString("InsertOrderItemCredit", paramList))
                Return Me.ResultObject
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Sub AutomaticOrderItemInfo(ByVal OrderHeader_ID As Integer)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As DataTable

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = Item_Key
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Package_Desc1"
            currentParam.Value = Package_Desc1
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("AutomaticOrderItemInfo", paramList)

                If dt.Rows.Count > 0 Then

                    If IsDBNull(dt.Rows(0).Item("Units_Per_Pallet")) Then
                        Me.Units_Per_Pallet = 0
                    Else
                        Me.Units_Per_Pallet = dt.Rows(0).Item("Units_Per_Pallet")
                    End If

                    Me.Cost = IIf(IsDBNull(dt.Rows(0).Item("OriginalCost")), 0, dt.Rows(0).Item("OriginalCost"))

                    If IsDBNull(dt.Rows(0).Item("CostUnit")) Then
                        Me.CostUnit = 0
                    Else
                        Me.CostUnit = dt.Rows(0).Item("CostUnit")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Package_Unit_ID")) Then
                        Me.Package_Unit_ID = 0
                    Else
                        Me.Package_Unit_ID = dt.Rows(0).Item("Package_Unit_ID")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Package_Desc1")) Then
                        Me.Package_Desc1 = 0
                    Else
                        Me.Package_Desc1 = dt.Rows(0).Item("Package_Desc1")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Package_Desc2")) Then
                        Me.Package_Desc2 = 0
                    Else
                        Me.Package_Desc2 = dt.Rows(0).Item("Package_Desc2")
                    End If

                    If IsDBNull(dt.Rows(0).Item("MarkupPercent")) Then
                        Me.MarkupPercent = 0
                    Else
                        Me.MarkupPercent = dt.Rows(0).Item("MarkupPercent")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Retail_Unit_ID")) Then
                        Me.Retail_Unit_ID = 0
                    Else
                        Me.Retail_Unit_ID = dt.Rows(0).Item("Retail_Unit_ID")
                    End If

                    If IsDBNull(dt.Rows(0).Item("VendorNetDiscount")) Then
                        Me.NetVendorItemDiscount = 0
                    Else
                        Me.NetVendorItemDiscount = dt.Rows(0).Item("VendorNetDiscount")
                    End If

                    If IsDBNull(dt.Rows(0).Item("FreightUnit")) Then
                        Me.FreightUnit = 1
                    Else
                        Me.FreightUnit = dt.Rows(0).Item("FreightUnit")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Freight")) Then
                        Me.Freight = 0
                    Else
                        Me.Freight = dt.Rows(0).Item("Freight")
                    End If

                    If IsDBNull(dt.Rows(0).Item("Handling")) Then
                        Me.Handling = 0
                    Else
                        Me.Handling = dt.Rows(0).Item("Handling")
                    End If

                    If IsDBNull(dt.Rows(0).Item("HandlingUnit")) Then
                        Me.HandlingUnit = 1
                    Else
                        Me.HandlingUnit = dt.Rows(0).Item("HandlingUnit")
                    End If

                    If IsDBNull(dt.Rows(0).Item("HandlingCharge")) Then
                        Me.HandlingCharge = 0
                    Else
                        Me.HandlingCharge = dt.Rows(0).Item("HandlingCharge")
                    End If

                    If IsDBNull(dt.Rows(0).Item("AdjustedCost")) Then
                        Me.AdjustedCost = 0
                    Else
                        Me.AdjustedCost = dt.Rows(0).Item("AdjustedCost")
                    End If


                    'All discount and cost conversion calculations will be done as part of the SendOrder() using the stored proc - UpdateOrderRefreshCosts

                    'Me.LineItemCost = Me.QuantityOrdered * Me.Cost 'This is just the basic cost. It will be updated as part of SendOrder() if there is any discount
                    'Me.UnitCost = Me.Cost 'This is just the basic cost. It will be updated as part of SendOrder() if there is any discount

                    Dim cu As tItemUnit
                    Dim fu As tItemUnit

                    '-- Markup
                    cu = GetItemUnit(Me.CostUnit)
                    fu = GetItemUnit(Me.FreightUnit)
                    If giUnit = 0 Then
                        LoadItemUnits()
                    End If
                    Me.LineItemCost = CostConversion(Me.Cost * (Me.MarkupPercent + 100) / 100, Me.CostUnit, Me.QuantityUnit, Me.Package_Desc1, Me.Package_Desc2, Me.Package_Unit_ID, 0, 0) * Me.QuantityOrdered
                    Me.UnitCost = CostConversion(Me.LineItemCost / Me.QuantityOrdered, Me.QuantityUnit, IIf(cu.IsPackageUnit, giUnit, Me.CostUnit), Me.Package_Desc1, Me.Package_Desc2, Me.Package_Unit_ID, 0, 0)

                End If

            Catch ex As Exception
                Common.logger.Info("Create Order failed! PONumber -" & OrderHeader_ID & "; " & factory.GetSQLString("AutomaticOrderItemInfo", paramList), ex)
                Me.ResultObject.Load(-1, False, "AutomaticOrderItemInfo", "Create Order Failed! PONumber -" & OrderHeader_ID, ex.ToString, factory.GetSQLString("AutomaticOrderItemInfo", paramList))

            Finally
                connectionCleanup(factory)
            End Try

        End Sub

        Public Function ReceiveOrderItem(ByVal dQuantity As Decimal, _
                                    ByVal dWeight As Decimal, _
                                    ByVal dtDate As DateTime, _
                                    ByVal bCorrection As Boolean, _
                                    ByVal iOrderItem_ID As Integer, _
                                    ByVal reasonCodeID As Integer, _
                                    Optional ByVal dPackSize As Decimal = 0, _
                                    Optional ByVal UserID As Long = 0) As Result

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outParamList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderItem_ID"
                currentParam.Value = iOrderItem_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DateReceived"
                currentParam.Value = Common.GetSystemDate()
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Quantity"
                currentParam.Value = dQuantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Package_Desc1"
                currentParam.Value = IIf(dPackSize > 0, dPackSize, DBNull.Value)
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                currentParam.Value = dWeight
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RecvDiscrepancyReasonID"
                If reasonCodeID <> Nothing Then
                    currentParam.Value = reasonCodeID
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Cost"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Freight"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LineItemCost"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LineItemFreight"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "receivedItemCost"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "receivedItemFreight"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ReceivedViaGun"
                currentParam.Value = 1
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AlreadyClosed"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                outParamList = factory.ExecuteStoredProcedure("ReceiveOrderItem3", paramList)

                Me.Cost = outParamList.Item(0)
                Me.Freight = outParamList.Item(1)
                Me.LineItemCost = outParamList(2)
                Me.LineItemFreight = outParamList(3)
                Me.ReceivedItemCost = outParamList(4)
                Me.ReceivedItemFreight = outParamList(5)
                Me.AlreadyClosed = outParamList(6)

                If Me.ResultObject Is Nothing Then
                    Me.ResultObject = New Result()
                End If

                If Me.AlreadyClosed = 1 Then
                    Me.ResultObject.ErrorCode = Common.ReceivingErrorCodes.OrderClosed
                    Me.ResultObject.ErrorMessage = "This PO is already closed."
                    Me.ResultObject.Load(Me.OrderItem_ID, False, "ReceiveOrderItem")
                Else
                    Me.ResultObject.Load(Me.OrderItem_ID, True, "ReceiveOrderItem")
                End If

                Return Me.ResultObject
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function ReceiveOrderItem(Optional ByVal DSDOrder As Boolean = False, Optional ByVal User_ID As Integer = 0) As Result
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "OrderItem_ID"
                currentParam.Value = Me.OrderItem_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Datereceived"
                currentParam.Value = Common.GetSystemDate()
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Quantity"
                currentParam.Value = Me.QuantityOrdered
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                currentParam.Value = DBNull.Value 'ReceiveOrderItem4 will populate Weight
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                '**********TFS 2895, Defaulting the Reason Code to Null while Receiving****************
                'This will allow Hand Held receiving to work with the updated Receiving Stored Procedure for v 4.3
                currentParam = New DBParam
                currentParam.Name = "RecvDiscrepancyReasonID"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = User_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ReceivedViaGun"
                If DSDOrder = True Then
                    currentParam.Value = 1
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                al = factory.ExecuteStoredProcedure("ReceiveOrderItem4", paramList) 'ReceiveOrderItem4 will populate Weight and calls ReceiveOrderItem3
                Me.ResultObject.Load(Me.OrderItem_ID, True, "ReceiveOrderItem")
                Return Me.ResultObject

            Catch ex As Exception
                Common.logger.Info("Receive Order failed! OrderItemID -" & Me.OrderItem_ID & "; " & factory.GetSQLString("ReceiveOrderItem4", paramList), ex)
                Me.ResultObject.Load(-1, False, "ReceiveOrderItem", "ReceiveOrderItem Failed! OrderItemID -" & Me.OrderItem_ID, ex.ToString, factory.GetSQLString("ReceiveOrderItem4", paramList))
                Return Me.ResultObject
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetReceivingListEinvoiceExceptions(ByVal OrderHeader_ID) As List(Of OrderItem)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As New DataTable
            Dim rlist As New List(Of OrderItem)

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = OrderHeader_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetReceivingListForNOIDNORD", paramList)

                For Each row As DataRow In dt.Rows
                    Dim oi As New OrderItem
                    oi.Identifier = row.Item("Identifier")

                    If IsDBNull(row.Item("Item_Description")) Then
                        oi.Item_Description = 0
                    Else
                        oi.Item_Description = row.Item("Item_Description")
                    End If

                    If IsDBNull(row.Item("Weight")) Then
                        oi.Total_Weight = 0
                    Else
                        oi.Total_Weight = row.Item("Weight")
                    End If

                    If IsDBNull(row.Item("eInvoiceQuantity")) Then
                        oi.eInvoiceQuantity = 0
                    Else
                        oi.eInvoiceQuantity = row.Item("eInvoiceQuantity")
                    End If

                    If IsDBNull(row.Item("eInvoiceWeight")) Then
                        oi.eInvoiceWeight = 0
                    Else
                        oi.eInvoiceWeight = row.Item("eInvoiceWeight")
                    End If

                    If IsDBNull(row.Item("QuantityOrdered")) Then
                        oi.QuantityOrdered = 0
                    Else
                        oi.QuantityOrdered = row.Item("QuantityOrdered")
                    End If

                    rlist.Add(oi)
                Next
                Return rlist

            Catch ex As Exception
                Common.logger.Info("GetReceivingListEinvoiceExceptions() failed for OrderHeader_ID " & OrderHeader_ID)
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

            Return Nothing
        End Function

    End Class
End Namespace