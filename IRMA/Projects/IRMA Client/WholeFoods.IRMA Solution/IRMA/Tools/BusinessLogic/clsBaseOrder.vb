'********************************************************************************************************************************************************************************
'clsBaseOrder Summary


'UPDATED_BY         UPDATED_DATE        UPDATED_FUNCTION_NAME               UPDATION_SUMMARY
'--------------------------------------------------------------------------------------------
' vayals            02/05/10            AddOpenOrderItem              Added value for lCostAdjustmentReason_ID
' vayals            02/05/10            Class_Initialize_Renamed      Added prm for CostAdjustmentReason_ID

'********************************************************************************************************************************************************************************

Option Strict Off
Option Explicit On
Friend Class clsBaseOrder
    Implements IDisposable

    Private m_cmdInsertItem As SqlClient.SqlCommand
    Private m_cmdCreateOrd As SqlClient.SqlCommand
    Private m_oCon As SqlClient.SqlConnection
    Private m_oTran As SqlClient.SqlTransaction

    Public Sub CloseOrder(ByVal lOrderHeaderID As Integer, ByVal sSentDate As String)
        Dim cmdUpdateOrder As New SqlClient.SqlCommand("EXEC UpdateOrderClosed " & lOrderHeaderID & ", " & giUserID, m_oCon, m_oTran)
        cmdUpdateOrder.ExecuteNonQuery()
        cmdUpdateOrder.Dispose()

        Dim cmdSetOrderSentDate As New SqlClient.SqlCommand("EXEC SetOrderSentDate " & lOrderHeaderID & ", '" & sSentDate & "'", m_oCon, m_oTran)
        cmdSetOrderSentDate.ExecuteNonQuery()
        cmdSetOrderSentDate.Dispose()

    End Sub

    Public Function CreateOrder(ByVal lVendor As Integer, ByVal lPurchaseLocation As Integer, ByVal lReceiveLocation As Integer, ByVal lTransfer_SubTeam As Object, ByVal lTransfer_To_SubTeam As Object, ByVal RipeOrderID As Integer, ByVal sDistDate As String, ByVal sImportDateTime As String, ByVal bPurchase_Order As Boolean, ByVal lUserID As Integer) As Integer

        m_cmdCreateOrd.Parameters("@Vendor_ID").Value = lVendor
        m_cmdCreateOrd.Parameters("@OrderType_ID").Value = CShort(Global_Renamed.enumOrderType.Distribution)
        m_cmdCreateOrd.Parameters("@ProductType_ID").Value = CShort(Global_Renamed.enumProductType.Product)
        m_cmdCreateOrd.Parameters("@PurchaseLocation_ID").Value = lPurchaseLocation
        m_cmdCreateOrd.Parameters("@ReceiveLocation_ID").Value = lReceiveLocation
        m_cmdCreateOrd.Parameters("@Transfer_SubTeam").Value = lTransfer_SubTeam
        m_cmdCreateOrd.Parameters("@Transfer_To_SubTeam").Value = lTransfer_To_SubTeam
        m_cmdCreateOrd.Parameters("@Fax_Order").Value = 0
        m_cmdCreateOrd.Parameters("@Expected_Date").Value = sDistDate
        m_cmdCreateOrd.Parameters("@CreatedBy").Value = lUserID
        m_cmdCreateOrd.Parameters("@Return_Order").Value = 0
        m_cmdCreateOrd.Parameters("@CurrencyID").Value = System.DBNull.Value
        m_cmdCreateOrd.Parameters("@NewOrderHeader_ID").Value = System.DBNull.Value

        m_cmdCreateOrd.ExecuteNonQuery()

        If IsDBNull(m_cmdCreateOrd.Parameters("@NewOrderHeader_ID").Value) Then
            Err.Raise(ERR_RIPE_IMPORT_CAN_NOT_CREATE_ORDER, "Function CreateOrder", ERR_RIPE_IMPORT_CAN_NOT_CREATE_ORDER & ":" & vbCrLf & Err.Description)
        Else
            CreateOrder = m_cmdCreateOrd.Parameters("@NewOrderHeader_ID").Value
        End If

    End Function

    Public Sub AddOpenOrderItem(ByVal lOrderHeaderID As Integer, ByVal lItemKey As Integer, ByVal sQuantityOrd As Single, ByRef sQuantityRcv As Single, ByVal sDistDate As String, ByVal lQuantityUnit As Integer)
        Dim drOrderItem As SqlClient.SqlDataReader
        Dim sLandedCost As Decimal, sLineItemCost As Decimal, sLineItemFreight As Decimal, sUnitCost As Decimal, sUnitExtCost As Decimal
        Dim sDiscountCost As Decimal, sMarkupCost As Decimal, sMarkupPercent As Decimal, lUnits_Per_Pallet As Integer, sCost As Decimal
        Dim lCostUnit As Integer, sFreight As Decimal, lFreightUnit As Integer, sAdjustedCost As Decimal, sQuantityDiscount As Decimal
        Dim lDiscountType As Integer, sPackage_Desc1 As Single, sPackage_Desc2 As Single, lPackage_Unit_ID As Integer, lRetail_Unit_ID As Integer
        Dim lCostAdjustmentReason_ID As Integer
        Dim cmdAutoOrderItemInfo As New SqlClient.SqlCommand("EXEC AutomaticOrderItemInfo " & lItemKey & ", " & lOrderHeaderID & ", NULL", m_oCon, m_oTran)
        drOrderItem = cmdAutoOrderItemInfo.ExecuteReader
        If drOrderItem.HasRows Then
            drOrderItem.Read()
            If lQuantityUnit = 0 Then lQuantityUnit = drOrderItem!QuantityUnit

            lUnits_Per_Pallet = drOrderItem!Units_Per_Pallet
            sCost = drOrderItem!Cost
            lCostUnit = drOrderItem!CostUnit
            sFreight = drOrderItem!Freight
            lFreightUnit = drOrderItem!FreightUnit
            sAdjustedCost = drOrderItem!AdjustedCost
            sQuantityDiscount = drOrderItem!QuantityDiscount
            lDiscountType = drOrderItem!DiscountType
            sPackage_Desc1 = drOrderItem!Package_Desc1
            sPackage_Desc2 = drOrderItem!Package_Desc2
            lPackage_Unit_ID = drOrderItem!Package_Unit_ID
            sMarkupPercent = drOrderItem!MarkupPercent
            lRetail_Unit_ID = drOrderItem!Retail_Unit_ID
            Try
                lCostAdjustmentReason_ID = drOrderItem!lCostAdjustmentReason_ID
            Catch ex As Exception
                lCostAdjustmentReason_ID = 0
            End Try

        End If

        drOrderItem.Close()
        drOrderItem = Nothing

        Select Case lDiscountType
            Case 1 : sDiscountCost = sCost - sQuantityDiscount
                'Case 2 : sDiscountCost = sCost * ((100 - sQuantityDiscount) / 100)
            Case 2 : sDiscountCost = sCost - (sCost * (sQuantityDiscount / 100))
            Case Else : sDiscountCost = sCost
        End Select
        sLineItemCost = CostConversion(sDiscountCost, lCostUnit, lQuantityUnit, sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0) * sQuantityOrd

        '-- Pre markup Freight
        sLineItemFreight = CostConversion(sFreight, lFreightUnit, lQuantityUnit, sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0) * sQuantityOrd

        sLandedCost = (sLineItemCost + sLineItemFreight) / sQuantityOrd

        '-- Markup
        Dim cu As tItemUnit
        Dim fu As tItemUnit
        cu = GetItemUnit(lCostUnit)
        fu = GetItemUnit(lFreightUnit)

        sLineItemCost = CostConversion(sDiscountCost * (sMarkupPercent + 100) / 100, lCostUnit, lQuantityUnit, sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0) * sQuantityOrd
        sLineItemFreight = CostConversion(sFreight * (sMarkupPercent + 100) / 100, lFreightUnit, lQuantityUnit, sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0) * sQuantityOrd
        sUnitCost = CostConversion(sLineItemCost / sQuantityOrd, lQuantityUnit, IIf(cu.IsPackageUnit, giUnit, lCostUnit), sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0)
        sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / sQuantityOrd, lQuantityUnit, IIf(fu.IsPackageUnit, giUnit, lFreightUnit), sPackage_Desc1, sPackage_Desc2, lPackage_Unit_ID, 0, 0)

        sMarkupCost = (sLineItemCost + sLineItemFreight) / sQuantityOrd

        m_cmdInsertItem.Parameters("@OrderHeader_ID").Value = lOrderHeaderID
        m_cmdInsertItem.Parameters("@Item_Key").Value = lItemKey
        m_cmdInsertItem.Parameters("@Units_Per_Pallet").Value = lUnits_Per_Pallet
        m_cmdInsertItem.Parameters("@QuantityUnit").Value = lQuantityUnit
        m_cmdInsertItem.Parameters("@QuantityOrdered").Value = sQuantityOrd
        m_cmdInsertItem.Parameters("@Cost").Value = sCost
        m_cmdInsertItem.Parameters("@CostUnit").Value = lCostUnit
        m_cmdInsertItem.Parameters("@Handling").Value = 0
        m_cmdInsertItem.Parameters("@HandlingUnit").Value = 1
        m_cmdInsertItem.Parameters("@Freight").Value = sFreight
        m_cmdInsertItem.Parameters("@FreightUnit").Value = lFreightUnit
        m_cmdInsertItem.Parameters("@AdjustedCost").Value = sAdjustedCost
        m_cmdInsertItem.Parameters("@QuantityDiscount").Value = sQuantityDiscount
        m_cmdInsertItem.Parameters("@DiscountType").Value = lDiscountType
        m_cmdInsertItem.Parameters("@LandedCost").Value = sLandedCost
        m_cmdInsertItem.Parameters("@LineItemCost").Value = sLineItemCost
        m_cmdInsertItem.Parameters("@LineItemFreight").Value = sLineItemFreight
        m_cmdInsertItem.Parameters("@LineItemHandling").Value = "0"
        m_cmdInsertItem.Parameters("@UnitCost").Value = sUnitCost
        m_cmdInsertItem.Parameters("@UnitExtCost").Value = sUnitExtCost
        m_cmdInsertItem.Parameters("@Package_Desc1").Value = sPackage_Desc1
        m_cmdInsertItem.Parameters("@Package_Desc2").Value = sPackage_Desc2
        m_cmdInsertItem.Parameters("@Package_Unit_ID").Value = lPackage_Unit_ID
        m_cmdInsertItem.Parameters("@MarkupPercent").Value = sMarkupPercent
        m_cmdInsertItem.Parameters("@MarkupCost").Value = sMarkupCost
        m_cmdInsertItem.Parameters("@Retail_Unit_ID").Value = lRetail_Unit_ID
        If lCostAdjustmentReason_ID = 0 Then
            m_cmdInsertItem.Parameters("@CostAdjustmentReason_ID").Value = System.DBNull.Value
        Else
            m_cmdInsertItem.Parameters("@CostAdjustmentReason_ID").Value = lCostAdjustmentReason_ID
        End If
        m_cmdInsertItem.Parameters("@OrderItemID").Value = System.DBNull.Value
        m_cmdInsertItem.ExecuteNonQuery()

        Dim cmdRcvOrdItm As New SqlClient.SqlCommand("Exec ReceiveOrderItem3 " & m_cmdInsertItem.Parameters("@OrderItemID").Value & ", '" & sDistDate & "', " & sQuantityRcv & ", " & sPackage_Desc1 & ", 0, " & "NULL" & ", " & giUserID & ", 0," & sCost & ", " & sFreight & ", " & sLineItemCost & ", " & sLineItemFreight & ", " & sLineItemCost & ", " & sLineItemFreight, m_oCon, m_oTran)
        cmdRcvOrdItm.ExecuteNonQuery()
        cmdRcvOrdItm.Dispose()

    End Sub

    'Private Function GetLastOrderHeaderID(ByVal lUserID As Object, ByRef cnConnection As ADODB.Connection) As Integer

    '	Dim rsRecordset As ADODB.Recordset

    '	rsRecordset = New ADODB.Recordset
    '	rsRecordset.Open("GetUsersLastOrderHeaderID " & lUserID, cnConnection, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
    '	GetLastOrderHeaderID = rsRecordset.Fields("OrderHeader_ID").Value
    '	rsRecordset.Close()
    '	rsRecordset = Nothing

    'End Function

    Public Sub GetUnitCollection(ByRef colUnits As Collection)

        Dim objItemUnit As clsBaseItemUnit
        Dim drUnits As SqlClient.SqlDataReader
        Dim cmdGetUnits As New SqlClient.SqlCommand("EXEC GetUnitAndID", m_oCon, m_oTran)
        drUnits = cmdGetUnits.ExecuteReader
        If drUnits.HasRows Then
            Do While drUnits.Read
                objItemUnit = New clsBaseItemUnit
                objItemUnit.lUnitID = drUnits!Unit_ID
                objItemUnit.sUnitName = drUnits!Unit_Name
                objItemUnit.sUnitAbbreviation = drUnits!Unit_Abbreviation
                objItemUnit.bWeight = drUnits!Weight_Unit
                colUnits.Add(objItemUnit, Trim(Str(objItemUnit.lUnitID)))
            Loop
        End If
        drUnits.Close()
        drUnits = Nothing
        objItemUnit = Nothing

        cmdGetUnits.Dispose()

    End Sub

    Private Sub Class_Initialize_Renamed()

        '----------Create InsertOrderItemRtnID command and parramaters----------------------------------------
        Dim prm As SqlClient.SqlParameter

        m_cmdInsertItem = New SqlClient.SqlCommand("InsertOrderItemRtnID")
        m_cmdInsertItem.CommandType = CommandType.StoredProcedure
        m_cmdInsertItem.Connection = m_oCon
        m_cmdInsertItem.Transaction = m_oTran

        prm = New SqlClient.SqlParameter("@OrderHeader_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Item_Key", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Units_Per_Pallet", SqlDbType.SmallInt)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@QuantityUnit", SqlDbType.SmallInt)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@QuantityOrdered", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Cost", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ADODB.ParameterDirectionEnum.adParamInput
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@CostUnit", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Handling", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@HandlingUnit", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Freight", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@FreightUnit", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@AdjustedCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@QuantityDiscount", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@DiscountType", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@LandedCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@LineItemCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@LineItemFreight", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@LineItemHandling", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@UnitCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@UnitExtCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Package_Desc1", SqlDbType.Decimal)
        prm.Precision = 9
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Package_Desc2", SqlDbType.Decimal)
        prm.Precision = 9
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Package_Unit_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@MarkupPercent", SqlDbType.Decimal)
        prm.Precision = 18
        prm.Scale = 4
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@MarkupCost", SqlDbType.Money)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Retail_Unit_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdInsertItem.Parameters.Add(prm)

        Try

            prm = New SqlClient.SqlParameter("@CostAdjustmentReason_ID", SqlDbType.Int)
            prm.Direction = ParameterDirection.Input
            m_cmdInsertItem.Parameters.Add(prm)

        Catch ex As Exception

        End Try


        prm = New SqlClient.SqlParameter("@OrderItemID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Output
        m_cmdInsertItem.Parameters.Add(prm)

        '-------------Create InsertOrder2 Command and parramaters-------------------------------------
        m_cmdCreateOrd = New SqlClient.SqlCommand("InsertOrder2")
        m_cmdCreateOrd.CommandType = CommandType.StoredProcedure
        m_cmdCreateOrd.Connection = m_oCon
        m_cmdCreateOrd.Transaction = m_oTran

        prm = New SqlClient.SqlParameter("@Vendor_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@OrderType_ID", SqlDbType.TinyInt)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@ProductType_ID", SqlDbType.TinyInt)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@PurchaseLocation_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@ReceiveLocation_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Transfer_SubTeam", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Transfer_To_SubTeam", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Fax_Order", SqlDbType.Bit)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Expected_Date", SqlDbType.SmallDateTime)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@CreatedBy", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@Return_Order", SqlDbType.Bit)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@CurrencyID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        m_cmdCreateOrd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@NewOrderHeader_ID", SqlDbType.Int)
        prm.Direction = ParameterDirection.Output
        m_cmdCreateOrd.Parameters.Add(prm)

    End Sub
    Public Sub New(ByRef oCon As SqlClient.SqlConnection, ByVal oTran As SqlClient.SqlTransaction)
        MyBase.New()
        m_oCon = oCon
        m_oTran = oTran
        Class_Initialize_Renamed()
    End Sub


    Protected Overrides Sub Finalize()
        m_cmdInsertItem.Dispose()
        m_cmdCreateOrd.Dispose()
        MyBase.Finalize()
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
            m_cmdInsertItem.Dispose()
            m_cmdCreateOrd.Dispose()
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class