Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Class OrderingFunctions

        Private Shared factory As New DataFactory(DataFactory.ItemCatalog)

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' ************************************************************************
        ' This class is also duplicated in the EXEtoIRMA project
        ' It should be combined and the duplicate removed when 
        ' a common library of business logic is created the two projects can share
        ' ************************************************************************

#Region "Constructors"
        ''' <summary>
        ''' Empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()

        End Sub

#End Region

        ' Rick Kelleher (12/2007) 3rd Party Freight changes - begin
        ''' <summary>
        ''' GetSafeDecimal
        ''' </summary>
        ''' <param name="val">Value as Object</param>
        ''' <returns>returns the value converted to a decimal, or 0 if the value is DBNull</returns>
        ''' <remarks>designed for use with this OrderingFunctions class
        ''' by Rick Kelleher</remarks>
        Private Shared Function GetSafeDecimal(ByVal val As Object) As Decimal
            logger.Debug("GetSafeDecimal Entry")
            ' by Rick Kelleher
            If IsDBNull(val) Then
                Return 0
            Else
                Return CDec(val)
            End If
            logger.Debug("GetSafeDecimal Exit")
        End Function

        ''' <summary>
        ''' GetFreight3PartyTotal
        ''' </summary>
        ''' <param name="OrderHeaderID"> the OrderHeaderID of the order to retrieve</param>
        ''' <returns>Returns the total amount of expected 3rd Party Freight entered when the order was created</returns>
        ''' <remarks>by Rick Kelleher</remarks>
        Public Shared Function GetFreight3PartyTotal(ByVal OrderHeaderID As Integer) As Decimal
            logger.Debug("GetFreight3PartyTotal Entry")
            Dim rsTotal3Party As DAO.Recordset = Nothing
            Dim dTotal3Party As Decimal
            Dim ds As DataSet
            Try
                ds = factory.GetDataSet("EXEC dbo.GetOrderInfo " & OrderHeaderID & ", 0")
                If ds.Tables(0).Rows.Count = 0 Then
                    dTotal3Party = 0
                Else
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    dTotal3Party = CDec(GetSafeDecimal(dr.Item("Freight3Party_OrderCost")))
                End If
            Finally
            End Try
            Return dTotal3Party
            logger.Debug("GetFreight3PartyTotal Exit")
        End Function

        ''' <summary>
        ''' GetFreight3PartyLineTotal
        ''' </summary>
        ''' <param name="OrderHeaderID"> the OrderHeaderID of the order to retrieve</param>
        ''' <returns>Returns the total amount of 3rd Party Freight assigned to the line items</returns>
        ''' <remarks>by Rick Kelleher</remarks>
        Public Shared Function GetFreight3PartyLineTotal(ByVal OrderHeaderID As Integer) As Decimal
            logger.Debug("GetFreight3PartyLineTotal Entry")
            Dim dTotal3Party As Decimal
            Dim ds As DataSet
            Try
                ds = factory.GetDataSet("select sum(LineItemFreight3Party) as Total from dbo.OrderItem (nolock) where orderheader_id =  " & OrderHeaderID)
                If ds.Tables(0).Rows.Count = 0 Then
                    dTotal3Party = 0
                Else
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    dTotal3Party = CDec(GetSafeDecimal(dr.Item("Total")))
                End If
            Finally
            End Try
            Return dTotal3Party
            logger.Debug("GetFreight3PartyLineTotal Exit")
        End Function

        ''' <summary>
        ''' GetFreight3PartyItemAvg()
        ''' </summary>
        ''' <param name="OrderItemID"></param>
        ''' <param name="OrderHeaderID"> the OrderHeaderID of the order to retrieve</param>
        ''' <returns>returns the value of the Freight3Party field (the Item Avg) from the GetOrderItemInfo stored proc
        ''' for a particular OrderItemID</returns>
        ''' <remarks>by Rick Kelleher</remarks>
        Public Shared Function GetFreight3PartyItemAvg(ByVal OrderItemID As Integer, ByVal OrderHeaderID As Integer) As Decimal
            logger.Debug("GetFreight3PartyItemAvg Entry")
            Dim d3Party As Decimal
            Dim ds As DataSet
            Try
                ds = factory.GetDataSet("EXEC dbo.GetOrderItemInfo " & OrderItemID & ", " & OrderHeaderID & ", 0")
                If ds.Tables(0).Rows.Count = 0 Then
                    d3Party = 0
                Else
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    d3Party = CDec(GetSafeDecimal(dr.Item("Freight3Party")))
                End If
            Finally
            End Try
            Return d3Party
            logger.Debug("GetFreight3PartyItemAvg Exit")
        End Function

        ''' <summary>
        ''' UpdateFreight3PartyTotal()
        ''' </summary>
        ''' <param name="OrderHeaderID"> the OrderHeaderID of the order to update</param>
        ''' <param name="ThirdPartyFreightTotal"></param>
        ''' <remarks>Updates the Freight3Party_OrderCost field of the OrderHeader table
        ''' for a particular OrderHeaderID.
        ''' by Rick Kelleher</remarks>
        Private Shared Sub UpdateFreight3PartyTotal(ByVal OrderHeaderID As Integer, ByVal ThirdPartyFreightTotal As Decimal)

            logger.Debug("=> UpdateFreight3PartyTotal")
            logger.DebugFormat("   * UpdateOrderHeaderFreight3Party {0},{1}", OrderHeaderID, ThirdPartyFreightTotal.ToString)
            factory.ExecuteNonQuery("EXEC UpdateOrderHeaderFreight3Party " _
                            & OrderHeaderID & ", " _
                            & ThirdPartyFreightTotal.ToString)
            If Err.Number <> 0 Then
                MsgBox(Err.Description, MsgBoxStyle.Critical)
                logger.Error("UpdateFreight3PartyTotal " & " Error in EXEC UpdateOrderHeaderFreight3Party " & Err.Description)
            End If
            logger.Debug("<= UpdateFreight3PartyTotal")
        End Sub

        ''' <summary>
        ''' DistributeFreight()
        ''' </summary>
        ''' <param name="OrderHeaderID"> the OrderHeaderID of the order to update</param>
        ''' <param name="ThirdPartyFreightTotal"></param>
        ''' <param name="TotalQtyNetDifference">Optional parameter TotalQtyNetDifference is to handle Qty Ordered changes 
        ''' on the OrderItem screen that are not saved to database</param>
        ''' <remarks>Used for updating 3rd Party Freight values for line items
        ''' by Rick Kelleher</remarks>
        Public Shared Sub DistributeFreight(ByVal OrderHeaderID As Integer, _
                                            ByVal ThirdPartyFreightTotal As Decimal, _
                                            Optional ByVal TotalQtyNetDifference As Decimal = 0)

            logger.Debug("=> DistributeFreight")
            Dim dTotQtyOrdered As Decimal
            Dim dTotQtyReceived As Decimal
            Dim dTotQty As Decimal
            Dim UseQtyReceived As Boolean
            Dim dAvgFreight As Decimal
            Dim sAvgFreight As String
            Dim sSQL As String

            Dim ds As DataSet = Nothing

            'TFS 6715 - Always distribute third part freight - even when 0 so that
            'users may clear out the value if they entered it incorrectly  6/11/08
            'If ThirdPartyFreightTotal = 0 Then
            'Exit Sub
            'End If

            'MsgBox("The order has 3rd Party Freight amount to distribute.")
            ThirdPartyFreightTotal = Decimal.Round(ThirdPartyFreightTotal, 2)
            Try
                'ds = factory.GetDataSet("EXEC dbo.GetOrderItemSumQty " & OrderHeaderID)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                ' Execute the stored procedures
                '-- Save the invoice or document data to the OrderHeader table
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                ds = factory.GetStoredProcedureDataSet("getorderitemsumqty", paramList)

                Dim dr As DataRow = ds.Tables(0).Rows(0)
                dTotQtyOrdered = CDec(GetSafeDecimal(dr.Item("SumQtyOrdered")))
                dTotQtyReceived = CDec(GetSafeDecimal(dr.Item("SumQtyReceived")))

            Catch
                logger.Debug("   GetOrderItemSumQty failed.")
            Finally
            End Try

            If dTotQtyOrdered = 0 Then
                logger.Debug("   The order has no line items, so the 3rd Party Freight amount will not be saved.")
                Exit Sub
            End If

            If dTotQtyReceived = 0 Then
                ' calc based on qty ordered
                UseQtyReceived = False
                dTotQty = dTotQtyOrdered
            Else
                ' calc based on qty received
                UseQtyReceived = True
                dTotQty = dTotQtyReceived
            End If
            logger.DebugFormat("   dTotQty: {0}  UseQtyRecevied: {1}", dTotQty, UseQtyReceived.ToString())

            dAvgFreight = (Decimal.Floor(100 * (ThirdPartyFreightTotal / (dTotQty + TotalQtyNetDifference)))) / 100     ' keep 2 decimal places
            sAvgFreight = Format(dAvgFreight, "##,###0.00##")

            ' Update the OrderItem table
            sSQL = "EXEC dbo.UpdateOrderItemFreight3PartyAll " & OrderHeaderID.ToString & ", " & sAvgFreight & ", " & UseQtyReceived.ToString
            logger.DebugFormat("   * {0}", sSQL)
            'SQLExecute(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            factory.ExecuteNonQuery(sSQL)

            ' Update the OrderHeader table
            UpdateFreight3PartyTotal(OrderHeaderID, ThirdPartyFreightTotal)

            ' Check against original totals for a remainder from the rounding
            ' *** NOT YET FUNCTIONAL ***
            ' Round the total to 2 decimal places
            'dTotFreightNew = OrderingFunctions.GetFreight3PartyLineTotal(OrderHeaderID)
            '
            '
            'If ThirdPartyFreightTotal <> dTotFreightNew Then
            '    ' There is a remainder from the rounding
            '    Dim dDiff As Decimal
            '    Dim dLineItemFreightNew As Decimal
            '    Dim n As Decimal
            '    Dim tbl As DataTable
            '    Dim dv As DataView

            '    ' Handle the remainder amount from the rounding
            '    dDiff = (ThirdPartyFreightTotal - dTotFreightNew)

            '    ds = factory.GetDataSet("EXEC dbo.GetOrderItemLines " & OrderHeaderID)
            '    tbl = ds.Tables(0)
            '    dv = ds.DefaultViewManager.CreateDataView(tbl)
            '    dv.RowFilter = "QuantityReceived > 0 "

            '    For Each drv As DataRowView In dv
            '        ' increment dDiff line items by one cent each until the total is correct 
            '        dLineItemFreightNew = dv.Item(CInt(n * 100) - 1).Item("LineItemFreight3Party") + 0.01
            '        Dim s As String = "EXEC dbo.UpdateOrderItemFreight3PartyOne " & dv.Item(CInt(n * 100) - 1).Item("OrderItem_ID").ToString & ", " & _
            '                    dLineItemFreightNew.ToString & ", " & UseQtyReceived.ToString
            '        'SQLExecute(s, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            '        factory.ExecuteNonQuery(s)
            '    Next

            '    ' Check the results
            '    dTotFreightNew = OrderingFunctions.GetFreight3PartyLineTotal(OrderHeaderID)
            '    If ThirdPartyFreightTotal <> dTotFreightNew Then
            '        MsgBox("The line items of the order have been updated with the new 3rd party freight amounts," & vbCrLf & _
            '                "but there was an internal calculation error for 3rd party freight amounts." & vbCrLf & _
            '                "Please check the amounts." & vbCrLf & vbCrLf & _
            '                "(Diff after = " & (ThirdPartyFreightTotal - dTotFreightNew).ToString() & ")")
            '        logger.Info("DistributeFreight - " & "The line items of the order have been updated with the new 3rd party freight amounts," & vbCrLf & _
            '         "but there was an internal calculation error for 3rd party freight amounts." & vbCrLf & _
            '         "Please check the amounts." & vbCrLf & vbCrLf & _
            '         "(Diff after = " & (ThirdPartyFreightTotal - dTotFreightNew).ToString() & ")")
            '    End If
            'End If

            logger.Debug("<= DistributeFreight")
        End Sub
        ' Rick Kelleher (12/2007) 3rd Party Freight changes - end

    End Class
End Namespace
