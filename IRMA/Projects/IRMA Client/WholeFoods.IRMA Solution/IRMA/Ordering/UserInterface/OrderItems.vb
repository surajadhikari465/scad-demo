Option Strict Off
Option Explicit On

Imports log4net
Imports System.Collections.Generic
Imports WholeFoods.IRMA.Ordering.BusinessLogic.OrderingFunctions
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmOrdersItem
    Inherits System.Windows.Forms.Form

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private listPackSizes As List(Of packSizeItem) = New System.Collections.Generic.List(Of packSizeItem)

#Region "Constants"

    Const iRetail_Sale As Short = 19
    Const iKeep_Frozen As Short = 12
    Const iFull_Pallet_Only As Short = 10
    Const iShipper_Item As Short = 20
    Const iWFM_Item As Short = 21

#End Region

#Region "Members"

    Dim IsInitializing As Boolean
    Dim bLoading As Boolean
    Dim rsOrderItem As DAO.Recordset
    Dim pbDataChanged As Boolean
    Dim pbUseHostedCost As Boolean
    Dim pcUnitCost As Decimal
    Dim pcUnitExtCost As Decimal
    Dim pdOriginalDateReceived As Date
    Dim pbNoDistMarkup As Boolean
    Dim pcOrigItemCost As Decimal
    Dim pbAllocationAllowed As Boolean
    Dim pbAddAllowed As Boolean
    Dim pbDeleteAllowed As Boolean
    Dim pbPre_Order As Boolean
    Dim pbIsExternalVendor As Boolean
    Dim sOriginalQuantityReceived As String
    Dim dOriginalTotalWeight As Double
    Dim pbIsWeightRequired As Boolean
    Dim pbIsCostedByWeight As Boolean
    Dim pbIsCatchWeightItem As Boolean
    Dim CatchWeightUnitCostChanged As Boolean = False
    Dim CatchWeightUnitCost As Decimal
    Dim OrderStoreNo As Integer
    Dim currentVendorCost As Decimal
    Dim currentVendorPackSize As Single
    Dim ReceiveLocationIsDistribution As Boolean = False

    Dim hostedCost As Decimal

    Private m_VendorDiscountAmt As Decimal
    Private m_IsDSDOrder As Boolean
    Private m_IsCreditOrder As Boolean

    ' Qty Ordered value before being edited
    Private m_OriginalQtyOrdered As Decimal

    ' currently calc'ed 3rd Party Freight Avg
    Private m_Freight3PartyAvg As Decimal

    ' A flag used to indicate when an item is being added to an order for the first time.
    Private newOrderItem As Boolean = False

#End Region

#Region "Properties"

    Public ReadOnly Property VendorDiscountAmt() As Decimal
        Get
            Return m_VendorDiscountAmt
        End Get
    End Property

#End Region

#Region "Helper Methods"

    Private Sub CheckNoOrderItems()
        logger.Debug("CheckNoOrderItems Entry")

        '-- Make sure there is data for them to be in this form
        If glOrderItemID = -1 Then
            If MsgBox("No line items found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "No Items Found") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If glOrderItemID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoOrderItems Exit")
    End Sub

    ''' <summary>
    '''  This method adds a new item to an order, saving the change to the database.
    ''' </summary>
    ''' <param name="lItem_Key"></param>
    ''' <remarks></remarks>
    Private Sub AddNewItem(ByRef lItem_Key As Integer)
        logger.Debug("AddNewItem Entry: lItem_Key=" + lItem_Key.ToString)

        Dim rsOrderItem As DAO.Recordset
        Dim sLandedCost As Decimal
        Dim sLineItemCost As Decimal
        Dim sLineItemFreight As Decimal
        Dim sUnitCost As Decimal
        Dim sUnitExtCost As Decimal
        Dim sDiscountCost As Decimal
        Dim sMarkupCost As Decimal
        Dim sMarkupPercent As Decimal
        Dim sCost As Decimal
        Dim sFreight As Decimal
        Dim sQuantityOrdered As Decimal
        Dim iCost_Unit As Integer
        Dim iFreight_Unit As Integer
        Dim iQuantityUnit As Integer

        '-- Get the last occurances information
        rsOrderItem = SQLOpenRecordSet("EXEC AutomaticOrderItemInfo " & lItem_Key & ", " & glOrderHeaderID & ", NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Dim cu As tItemUnit
        Dim fu As tItemUnit
        Dim lIgnoreErrNum(0) As Integer

        With rsOrderItem
            '-- Bug Fix 4848 OrderItem edit screen 
            If (.Fields("ItemCount").Value >= 1) Then
                Dim dialogResult As DialogResult
                dialogResult = MessageBox.Show(String.Format("[ {0} ] already exists on this order.", .Fields("Item_Description").Value, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation))
                Exit Sub
            End If

            sQuantityOrdered = .Fields("QuantityOrdered").Value
            iQuantityUnit = .Fields("QuantityUnit").Value

            If Not IsDBNull(.Fields("VendorNetDiscount").Value) Then
                m_VendorDiscountAmt = .Fields("VendorNetDiscount").Value
            Else
                m_VendorDiscountAmt = 0
            End If

            '-- Pre markup Cost
            '-- Use the Current Vendor Cost (Reg Cost) if the a percent discount is applieds.
            If (.Fields("DiscountType").Value = 2 Or (frmOrders.OrderDiscountExists And frmOrders.OrderDiscountType = 2)) Then
                sCost = .Fields("OriginalCost").Value
            Else
                sCost = .Fields("Cost").Value
            End If

            If IsDBNull(.Fields("CostUnit").Value) Then
                logger.Warn("AddNewItem - Unable to add the selected item to the order because it does not have a Cost Unit ID assigned: lItem_Key=" + lItem_Key.ToString + ", glOrderHeaderID=" + glOrderHeaderID.ToString)
                MsgBox("BAD DATA: This item has no Cost Unit ID assigned to it. This must be assigned in order to add it to the order." & _
                        vbCrLf & vbCrLf & "Please close the empty Line Item Information screen that follows.", MsgBoxStyle.Critical)
                rsOrderItem.Close()
                logger.Debug("AddNewItem Exit")
                Exit Sub
            Else
                iCost_Unit = .Fields("CostUnit").Value
            End If

            Select Case .Fields("DiscountType").Value
                Case 0
                    If m_VendorDiscountAmt > 0 Then
                        ' Bug 12492 the vendor discount amount is now being subtracted in the stored proc
                        sDiscountCost = sCost
                    ElseIf frmOrders.OrderDiscountExists Then
                        Select Case frmOrders.OrderDiscountType
                            Case 1 : sDiscountCost = sCost - frmOrders.OrderDiscountAmt
                            Case 2 : sDiscountCost = sCost - (sCost * (frmOrders.OrderDiscountAmt / 100))
                            Case 4 : sDiscountCost = sCost - (sCost * (frmOrders.OrderDiscountAmt / 100))
                        End Select
                    Else
                        sDiscountCost = sCost
                    End If
                Case 1 : sDiscountCost = sCost - .Fields("QuantityDiscount").Value
                Case 2 : sDiscountCost = sCost * ((100 - .Fields("QuantityDiscount").Value) / 100)
                Case Else : sDiscountCost = sCost
            End Select

            sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered

            '-- Pre markup Freight
            sFreight = .Fields("Freight").Value
            iFreight_Unit = .Fields("FreightUnit").Value
            sLineItemFreight = CostConversion(sFreight, iFreight_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered

            sLandedCost = (sLineItemCost + sLineItemFreight) / sQuantityOrdered

            '-- Markup
            cu = GetItemUnit(iCost_Unit)
            fu = GetItemUnit(iFreight_Unit)

            sLineItemCost = CostConversion(sDiscountCost * (.Fields("MarkupPercent").Value + 100) / 100, iCost_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered
            sLineItemFreight = CostConversion(.Fields("Freight").Value * (.Fields("MarkupPercent").Value + 100) / 100, iFreight_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered
            sUnitCost = CostConversion(sLineItemCost / sQuantityOrdered, iQuantityUnit, IIf(cu.IsPackageUnit, giUnit, iCost_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
            sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / sQuantityOrdered, iQuantityUnit, IIf(fu.IsPackageUnit, giUnit, iFreight_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
            sMarkupCost = (sLineItemCost + sLineItemFreight) / sQuantityOrdered

            lIgnoreErrNum(0) = 50002

            On Error Resume Next
            SQLExecute3("EXEC InsertOrderItemCredit " & _
                         glOrderHeaderID & ", " & _
                         lItem_Key & ", " & _
                         .Fields("Units_Per_Pallet").Value & ", " & _
                         iQuantityUnit & ", " & _
                         sQuantityOrdered & ", " & _
                         sCost & ", " & _
                         iCost_Unit & ", " & _
                         .Fields("Handling").Value & ", " & _
                         .Fields("HandlingUnit").Value & ", " & _
                         sFreight & ", " & _
                         iFreight_Unit & ", " & _
                         .Fields("AdjustedCost").Value & ", " & _
                         .Fields("QuantityDiscount").Value & ", " & _
                         .Fields("DiscountType").Value & ", " & _
                         sLandedCost & ", " & _
                         sLineItemCost & ", " & _
                         sLineItemFreight & ", " & _
                         0 & ", " & sUnitCost & ", " & _
                         sUnitExtCost & ", " & _
                         .Fields("Package_Desc1").Value & ", " & _
                         .Fields("Package_Desc2").Value & ", " & _
                         .Fields("Package_Unit_ID").Value & ", " & _
                         .Fields("MarkupPercent").Value & ", " & _
                         sMarkupCost & ", " & _
                         .Fields("Retail_Unit_ID").Value & _
                         ", Null," & _
                         giUserID & ", " & _
                         m_VendorDiscountAmt & ", " & IIf(IsDBNull(.Fields("HandlingCharge").Value), 0, .Fields("HandlingCharge").Value), _
                         DAO.RecordsetOptionEnum.dbSQLPassThrough, _
                         lIgnoreErrNum)

            If Err.Number <> 0 Then
                logger.Error("AddNewItem " & "Error in EXEC InsertOrderItemCredit " & Err.Description)
                MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            End If
            On Error GoTo 0
        End With

        rsOrderItem.Close()

        logger.Debug("AddNewItem Exit")
    End Sub

    ''' <summary>
    ''' This function saves updates made to the order item to the database.  It will return FALSE if the
    ''' data does not pass data entry validation.
    ''' </summary>
    ''' <param name="bLeavingScreen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SaveData(ByRef bLeavingScreen As Boolean) As Boolean
        logger.Debug("SaveData Entry: bLeavingScreen=" + bLeavingScreen.ToString)

        SaveData = True
        bLoading = True

        Dim bSent As Boolean
        Dim sQuantityDistributed As Decimal
        Dim lErrNum As Integer
        Dim sErrDesc As String
        Dim lReason As Long
        Dim bIsReceiving As Boolean
        Dim packSize1 As Double

        If ValidateFormValues() Then

            If frmOrders.IsCredit And cmbField(iOrderItemCreditReason).SelectedIndex > -1 Then
                lReason = VB6.GetItemData(cmbField(iOrderItemCreditReason), cmbField(iOrderItemCreditReason).SelectedIndex)
            Else
                lReason = -1
            End If

            System.Windows.Forms.Application.DoEvents()

            If glOrderItemID = -1 Then Exit Function

            Dim lIgnoreErrNum(0) As Integer
            Dim sSQLText As String
            If pbDataChanged Then

                If Not txtField(iOrderItemPackage_Desc1).ReadOnly AndAlso (CDbl(FixNumber(txtField(iOrderItemPackage_Desc1).Text)) = 0) Then
                    MsgBox("Please enter a value for Package Description 1 that is greater than 0.", MsgBoxStyle.Information, Me.Text)
                    txtField(iOrderItemPackage_Desc1).Focus()
                    SaveData = False
                    logger.Info("SaveData " & " Package Description 1 must be > 0  SaveData = False")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If

                If ComboBox_VendorPack.Visible Then
                    packSize1 = CType(ComboBox_VendorPack.Text, Double)
                Else
                    packSize1 = CType(txtField(iOrderItemPackage_Desc1).Text, Double)
                End If

                If pbIsCatchWeightItem = True And IsNumeric(Me.CatchWeightUnitCostTextBox.Text) Then
                    CatchWeightUnitCost = Math.Round(CDec(CatchWeightUnitCostTextBox.Text), 2)
                End If

                '-- See if they can add another
                '-- Make sure they ordered something
                If CDec(FixValue(txtField(iOrderItemQuantityOrdered).Text)) + IIf(ComboVal(cmbField(iOrderItemDiscountType)) = 3, CDec(FixValue(txtField(iOrderItemQuantityDiscount).Text)), 0) = 0 Then
                    MsgBox("Please enter an ordered quantity greater than 0.", MsgBoxStyle.Information, "No Quantity Entered")
                    SaveData = False
                    txtField(iOrderItemQuantityOrdered).Text = "0"
                    txtField(iOrderItemQuantityOrdered).Focus()
                    logger.Info("SaveData " & " Cannot make an order for an item of zero quantity.  SaveData = False")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If

                If dtpReceivedDate.Value IsNot Nothing AndAlso dtpReceivedDate.IsDateValid Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object pdOriginalDateReceived. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pdOriginalDateReceived = dtpReceivedDate.Value
                End If

                lIgnoreErrNum(0) = 50002
                SQLExecute3("BEGIN TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

                Dim OrigCost As Double
                If Trim(Me.OriginalCostTextBox.Text) = "" Then
                    OrigCost = 0.0
                Else
                    OrigCost = CDbl(Me.OriginalCostTextBox.Text)
                End If

                sSQLText = "EXEC [dbo].[UpdateOrderItemInfo] " _
                                    & "@OrderItem_ID = {0}, " _
                                    & "@QuantityOrdered = {1}, " _
                                    & "@QuantityReceived = {2}, " _
                                    & "@Total_Weight = {3}, " _
                                    & "@Units_Per_Pallet = {4}, " _
                                    & "@Cost = {5}, " _
                                    & "@Handling = {6}, " _
                                    & "@Freight = {7}, " _
                                    & "@AdjustedCost = {8}, " _
                                    & "@LandedCost = {9}, " _
                                    & "@QuantityDiscount = {10}, " _
                                    & "@LineItemCost = {11}, " _
                                    & "@LineItemFreight = {12}, " _
                                    & "@LineItemHandling = {13}, " _
                                    & "@ReceivedItemCost = {14}, " _
                                    & "@ReceivedItemFreight = {15}, " _
                                    & "@ReceivedItemHandling = {16}, " _
                                    & "@Freight3Party = {17}, " _
                                    & "@LineItemFreight3Party = {18}, " _
                                    & "@UnitCost = {19}, " _
                                    & "@UnitExtCost = {20}, " _
                                    & "@QuantityUnit = {21}, " _
                                    & "@CostUnit = {22}, " _
                                    & "@HandlingUnit = {23}, " _
                                    & "@FreightUnit = {24}, " _
                                    & "@DiscountType = {25}, " _
                                    & "@DateReceived = {26}, " _
                                    & "@OriginalDateReceived = {27}, " _
                                    & "@ExpirationDate = {28}, " _
                                    & "@MarkupPercent = {29}, " _
                                    & "@MarkupCost = {30}, " _
                                    & "@Package_Desc1 = {31}, " _
                                    & "@Package_Desc2 = {32}, " _
                                    & "@Package_Unit_ID = {33}, " _
                                    & "@Retail_Unit_ID = {34}, " _
                                    & "@Origin_ID = {35}, " _
                                    & "@CountryProc_ID = {36}, " _
                                    & "@CreditReason_ID = {37}, " _
                                    & "@QuantityAllocated = {38}, " _
                                    & "@User_ID = {39}, " _
                                    & "@Lot_No = {40}, " _
                                    & "@ReasonCodeDetailID = {41}, " _
                                    & "@HandlingCharge = {42}, " _
                                    & "@CatchWeightCostPerWeight = {43}, " _
                                    & "@SustainabilityRankingID = {44}"

                sSQLText = String.Format(sSQLText, _
                                    glOrderItemID, _
                                    txtField(iOrderItemQuantityOrdered).Text, _
                                    IIf(txtField(iOrderItemQuantityReceived).Text.Trim() = "", "NULL", txtField(iOrderItemQuantityReceived).Text), _
                                    IIf(Trim(txtField(iOrderItemTotal_Weight).Text.Trim()) = "", 0, txtField(iOrderItemTotal_Weight).Text), _
                                    txtField(iOrderItemUnits_Per_Pallet).Text, _
                                    txtField(iOrderItemCost).Text, _
                                    txtField(iOrderItemHandling).Text, _
                                    txtField(iOrderItemFreight).Text, _
                                    txtField(iOrderItemAdjustedCost).Text, _
                                    txtField(iOrderItemLandingCost).Text, _
                                    txtField(iOrderItemQuantityDiscount).Text, _
                                    txtField(iOrderItemLineItemCost).Text, _
                                    txtField(iOrderItemLineItemFreight).Text, _
                                    txtField(iOrderItemLineItemHandling).Text, _
                                    txtField(iOrderItemReceivedItemCost).Text, _
                                    txtField(iOrderItemReceivedItemFreight).Text, _
                                    txtField(iOrderItemReceivedItemHandling).Text, _
                                    txt3rdPartyFreightAvg.Text, _
                                    txt3rdPartyFreightLine.Text, _
                                    pcUnitCost, _
                                    pcUnitExtCost, _
                                    ComboValue(cmbField(iOrderItemQuantityUnit)), _
                                    ComboValue(cmbField(iOrderItemCostUnit)), _
                                    ComboValue(cmbField(iOrderItemHandlingUnit)), _
                                    ComboValue(cmbField(iOrderItemFreightUnit)), _
                                    ComboValue(cmbField(iOrderItemDiscountType)), _
                                    TextValue("'" & dtpReceivedDate.Value & "'"), _
                                    IIf(pdOriginalDateReceived = Date.MinValue, "NULL", "'" & VB6.Format(pdOriginalDateReceived, "MM/DD/YYYY Hh:Mm:Ss") & "'"), _
                                    TextValue("'" & dtpExpirationDate.Value & "'"), _
                                    IIf(optPercent.Checked, IIf(IsNumeric(txtField(iOrderItemMarkupPercent).Text.Trim), txtField(iOrderItemMarkupPercent).Text.Trim, 0), 0), _
                                    txtField(iOrderItemMarkupCost).Text, _
                                    packSize1.ToString, _
                                    txtField(iOrderItemPackage_Desc2).Text, _
                                    ComboValue(cmbField(iOrderItemPackage_Unit_ID)), _
                                    ComboValue(cmbField(iOrderItemRetail_Unit_ID)), _
                                    ComboValue(cmbField(iOrderItemOrigin_ID)), _
                                    ComboValue(cmbField(iOrderItemCountryProc_ID)), _
                                    IIf(lReason = -1, "NULL", lReason), _
                                    TextValue(txtField(iOrderItemQuantityAllocated).Text), _
                                    giUserID, _
                                    IIf(Trim(txtField(iOrderItemLotNo).Text) = "", "NULL", "'" & txtField(iOrderItemLotNo).Text.Trim & "'"), _
                                    IIf(IsNothing(ucReasonCode.Value), "NULL", ucReasonCode.Value), _
                                    IIf(optDollars.Checked, txtField(iOrderItemMarkupPercent).Text, 0), _
                                    CatchWeightUnitCost, _
                                    IIf(cmbSustainRanking.SelectedIndex <= 0, "NULL", cmbSustainRanking.SelectedIndex.ToString()))

                On Error Resume Next
                SQLExecute3(sSQLText, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

                If Err.Number <> 0 Then
                    logger.Error("SaveData Error in EXEC UpdateOrderItemInfo " & Err.Description)
                    MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
                    On Error GoTo 0
                    SaveData = False
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
                On Error GoTo 0

                If (bIsReceiving = True) Then
                    sSQLText = String.Format("EXEC dbo.InsertReceivingItemHistory @OrderItem_ID = {0}, @User_ID = {1}", glOrderItemID, giUserID)
                    SQLExecute3(sSQLText, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
                End If

                SQLExecute3("COMMIT TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

                pbDataChanged = False

            End If
        Else
            SaveData = False
        End If

        bLoading = False

        logger.Debug("SaveData Exit")
    End Function

    Private Sub RefreshData(ByRef lRecord As Integer)
        logger.Debug("RefreshData Entry: lRecord=" + lRecord.ToString)

        Dim blnCreditReasonCodeAutoSet As Boolean = False

        Dim sql As String = String.Format("EXEC dbo.GetOrderItemInfo @OrderItem_ID = {0}, @OrderHeader_ID = {1}, @Record = {2}", glOrderItemID, glOrderHeaderID, lRecord)

        logger.Info("RefreshData - calling procedure to pre-fill the data entry screen for the order item: " + sql)
        SQLOpenRS(rsOrderItem, sql, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        '-- Collect the data
        bLoading = True

        If rsOrderItem.EOF Then
            glOrderItemID = -1
            pbNoDistMarkup = False
        Else
            '-- Load data
            ReceiveLocationIsDistribution = DirectCast(rsOrderItem.Fields("ReceiveLocationIsDistribution").Value, Boolean)
            glOrderItemID = rsOrderItem.Fields("OrderItem_ID").Value
            glItemID = rsOrderItem.Fields("Item_Key").Value

            If rsOrderItem.Fields("CurrentVendorCost").Value.Equals(DBNull.Value) Then
                currentVendorCost = 0
            Else
                currentVendorCost = rsOrderItem.Fields("CurrentVendorCost").Value
            End If

            If rsOrderItem.Fields("CurrentVendorPack").Value.Equals(DBNull.Value) Then
                currentVendorPackSize = 0
            Else
                currentVendorPackSize = rsOrderItem.Fields("CurrentVendorPack").Value
            End If

            If Not IsDBNull(rsOrderItem.Fields("OriginalDateReceived").Value) Then
                pdOriginalDateReceived = CDate(rsOrderItem.Fields("OriginalDateReceived").Value)
            End If

            glItemSubTeam = rsOrderItem.Fields("SubTeam_No").Value

            If IsDBNull(rsOrderItem.Fields("Brand_Name").Value) Then
                Me.txtBrandName.Text = ""
            Else
                Me.txtBrandName.Text = rsOrderItem.Fields("Brand_Name").Value
            End If

            If IsDBNull(rsOrderItem.Fields("NetVendorItemDiscount").Value) OrElse rsOrderItem.Fields("NetVendorItemDiscount").Value = 0 Then
                m_VendorDiscountAmt = 0
            Else
                m_VendorDiscountAmt = Convert.ToDecimal(rsOrderItem.Fields("NetVendorItemDiscount").Value)
            End If

            txtField(iOrderItemQuantityOrdered).Text = VB6.Format(rsOrderItem.Fields("QuantityOrdered").Value, "#####0.0###")
            m_OriginalQtyOrdered = rsOrderItem.Fields("QuantityOrdered").Value
            txtField(iOrderItemQuantityAllocated).Text = VB6.Format(rsOrderItem.Fields("QuantityAllocated").Value, "#####0.0###")

            If IsDBNull(rsOrderItem.Fields("QuantityReceived").Value) Then
                txtField(iOrderItemQuantityReceived).Text = String.Empty
                TextBoxLineItemReceivedCost.Text = String.Empty
            Else
                txtField(iOrderItemQuantityReceived).Text = VB6.Format(rsOrderItem.Fields("QuantityReceived").Value, "#####0.0###")
                TextBoxLineItemReceivedCost.Text = VB6.Format(rsOrderItem.Fields("ReceivedItemCost").Value, "#0.00##")
            End If

            sOriginalQuantityReceived = IIf(IsDBNull(rsOrderItem.Fields("QuantityReceived").Value), "", rsOrderItem.Fields("QuantityReceived").Value)

            If rsOrderItem.Fields("Units_Per_Pallet").Value > 0 Then
                txtField(iOrderItemNumber_Of_Pallets).Text = VB6.Format(rsOrderItem.Fields("QuantityOrdered").Value / rsOrderItem.Fields("Units_Per_Pallet").Value, IIf(rsOrderItem.Fields("Full_Pallet_Only").Value, "#####0", "#####0.0#"))
            Else
                txtField(iOrderItemNumber_Of_Pallets).Text = "0"
            End If

            If rsOrderItem.Fields("Full_Pallet_Only").Value Then
                txtField(iOrderItemNumber_Of_Pallets).Tag = "NUMBER"
            Else
                txtField(iOrderItemNumber_Of_Pallets).Tag = "CURRENCY"
            End If

            txtField(iOrderItemTotal_Weight).Text = VB6.Format(rsOrderItem.Fields("Total_Weight").Value, "#####0.0###")
            dOriginalTotalWeight = rsOrderItem.Fields("Total_Weight").Value
            txtField(iOrderItemUnits_Per_Pallet).Text = rsOrderItem.Fields("Units_Per_Pallet").Value

            txtField(iOrderItemCost).Text = VB6.Format(rsOrderItem.Fields("Cost").Value, "#####0.00##")

            'TFS 2552, 02/04/2014, Faisal Ahmed - Use hosted cost if there is no last paid cost
            pbUseHostedCost = False
            If geOrderType = enumOrderType.Transfer And CInt(txtField(iOrderItemCost).Text) = 0 Then
                hostedCost = ItemDAO.GetCurrentNetCost(glItemID, frmOrders.StoreNo)
                txtField(iOrderItemCost).Text = VB6.Format(hostedCost, "#####0.00##")
                pbUseHostedCost = True
            End If

            Me.OriginalCostTextBox.Text = VB6.Format(rsOrderItem.Fields("OriginalCost").Value, "#####0.00##")
            Me.OriginalCostUnitTextBox.Text = rsOrderItem.Fields("OriginalCostUnitName").Value
            pcOrigItemCost = rsOrderItem.Fields("Cost").Value
            txtField(iOrderItemHandling).Text = VB6.Format(rsOrderItem.Fields("Handling").Value, "#####0.00##")
            txtField(iOrderItemFreight).Text = VB6.Format(rsOrderItem.Fields("Freight").Value, "#####0.00##")
            txtField(iOrderItemAdjustedCost).Text = VB6.Format(rsOrderItem.Fields("AdjustedCost").Value, "####0.00##")
            txtField(iOrderItemLandingCost).Text = VB6.Format(rsOrderItem.Fields("LandedCost").Value, "####0.00##")
            txtField(iOrderItemQuantityDiscount).Text = VB6.Format(rsOrderItem.Fields("QuantityDiscount").Value, "####0.00##")
            txtField(iOrderItemLineItemCost).Text = VB6.Format(rsOrderItem.Fields("LineItemCost").Value, "#0.00##")
            txtField(iOrderItemLineItemFreight).Text = VB6.Format(rsOrderItem.Fields("LineItemFreight").Value, "#0.00##")
            txtField(iOrderItemLineItemHandling).Text = VB6.Format(rsOrderItem.Fields("LineItemHandling").Value, "#0.00##")
            txtField(iOrderItemReceivedItemCost).Text = VB6.Format(rsOrderItem.Fields("PaidLineItemCost").Value, "#0.00##")
            txtField(iOrderItemReceivedItemFreight).Text = VB6.Format(rsOrderItem.Fields("ReceivedItemFreight").Value, "#0.00##")
            txtField(iOrderItemReceivedItemHandling).Text = VB6.Format(rsOrderItem.Fields("ReceivedItemHandling").Value, "#0.00##")

            If CBool(rsOrderItem.Fields("IsVendorDC").Value) Then
                If Not IsDBNull(rsOrderItem.Fields("HandlingCharge").Value) Then
                    If rsOrderItem.Fields("HandlingCharge").Value > 0 Then
                        txtField(iOrderItemMarkupPercent).Text = VB6.Format(rsOrderItem.Fields("HandlingCharge").Value, "##0.00##")
                        optDollars.Checked = True
                    Else
                        txtField(iOrderItemMarkupPercent).Text = VB6.Format(rsOrderItem.Fields("MarkupPercent").Value, "##0.0##")
                        optPercent.Checked = True
                    End If
                Else
                    optPercent.Checked = True
                End If
            Else
                txtField(iOrderItemMarkupPercent).Text = VB6.Format(0, "##0.00##")
            End If

            txtField(iOrderItemMarkupCost).Text = VB6.Format(rsOrderItem.Fields("MarkupCost").Value, "#0.00##")

            If IsDBNull(rsOrderItem.Fields("Freight3Party").Value) Then
                m_Freight3PartyAvg = 0
            Else
                m_Freight3PartyAvg = rsOrderItem.Fields("Freight3Party").Value
            End If

            txt3rdPartyFreightAvg.Text = VB6.Format(m_Freight3PartyAvg, "#0.00##")

            If IsDBNull(rsOrderItem.Fields("LineItemFreight3Party").Value) Then
                txt3rdPartyFreightLine.Text = "0.00"
            Else
                txt3rdPartyFreightLine.Text = VB6.Format(rsOrderItem.Fields("LineItemFreight3Party").Value, "#0.00##")
            End If

            If IsDBNull(rsOrderItem.Fields("Lot_no").Value) Then
                txtField(iOrderItemLotNo).Text = ""
            Else
                txtField(iOrderItemLotNo).Text = rsOrderItem.Fields("Lot_no").Value
            End If

            pcUnitCost = rsOrderItem.Fields("UnitCost").Value
            pcUnitExtCost = rsOrderItem.Fields("UnitExtCost").Value

            'TFS 2552, 02/04/2014, Faisal Ahmed
            If pbUseHostedCost Then
                pcUnitCost = hostedCost
                pcUnitExtCost = hostedCost
            End If

            SetCombo(cmbField(iOrderItemPackage_Unit_ID), CInt(rsOrderItem.Fields("Package_Unit_ID").Value))
            SetCombo(cmbField(iOrderItemLandingUnit), CInt(rsOrderItem.Fields("QuantityUnit").Value))
            SetCombo(cmbField(iOrderItemMarkupUnit), CInt(rsOrderItem.Fields("QuantityUnit").Value))
            SetCombo(cmbField(iOrderItemDiscountType), CInt(rsOrderItem.Fields("DiscountType").Value))

            If IsDBNull(rsOrderItem.Fields("HandlingUnit").Value) Then
                SetCombo(cmbField(iOrderItemHandlingUnit), String.Empty)
            Else
                SetCombo(cmbField(iOrderItemHandlingUnit), CInt(rsOrderItem.Fields("HandlingUnit").Value))
            End If

            If (Not IsDBNull(rsOrderItem.Fields("Origin_ID").Value)) Then
                SetCombo(cmbField(iOrderItemOrigin_ID), CInt(rsOrderItem.Fields("Origin_ID").Value))
            End If

            If (Not IsDBNull(rsOrderItem.Fields("CountryProc_ID").Value)) Then
                SetCombo(cmbField(iOrderItemCountryProc_ID), CInt(rsOrderItem.Fields("CountryProc_ID").Value))
            End If

            ' populate the sustainability ranking drop down value and make the label red or not
            If (Not IsDBNull(rsOrderItem.Fields("SustainabilityRankingID").Value)) Then
                cmbSustainRanking.SelectedIndex = CInt(rsOrderItem.Fields("SustainabilityRankingID").Value)
            Else
                cmbSustainRanking.SelectedIndex = -1
            End If

            If CType(rsOrderItem.Fields("SustainabilityRankingRequired").Value, Boolean) And geOrderType <> enumOrderType.Transfer Then
                Me.lblSustainRanking.ForeColor = Color.Red
            Else
                Me.lblSustainRanking.ForeColor = System.Drawing.SystemColors.ControlText
            End If

            If (Not IsDBNull(rsOrderItem.Fields("CreditReason_ID").Value)) Then
                SetCombo(cmbField(iOrderItemCreditReason), CInt(rsOrderItem.Fields("CreditReason_ID").Value))
            ElseIf frmOrders.chkApplyAllCreditReason.Checked AndAlso Not String.IsNullOrEmpty(frmOrders.lblCreditReasonCode.Text) Then
                SetCombo(cmbField(iOrderItemCreditReason), CInt(frmOrders.lblCreditReasonCode.Text))
                blnCreditReasonCodeAutoSet = True
            Else
                ' If value is NULL, we want to clear credit reason dropdown, and force user to fill it out
                SetCombo(cmbField(iOrderItemCreditReason), "")
            End If

            If (cmbField(iOrderItemDiscountType).SelectedIndex <> 0 Or Val(txtField(iOrderItemAdjustedCost).Text) <> 0) And (Not IsDBNull(rsOrderItem.Fields("ReasonCodeDetailID").Value)) Then
                ucReasonCode.Value = rsOrderItem.Fields("ReasonCodeDetailID").Value
            Else
                ucReasonCode.Value = -1
            End If

            LoadUnit(cmbField(iOrderItemCostUnit))
            LoadItemUnitsCost(cmbField(iOrderItemQuantityUnit), CInt(rsOrderItem.Fields("CostedByWeight").Value))

            ' ***** Setup/Load Catch weight Controls ******
            pbIsCatchWeightItem = CBool(rsOrderItem.Fields("CatchWeightRequired").Value)
            If pbIsCatchWeightItem = True Then
                CatchWeightUnitCost = IIf(rsOrderItem.Fields("CatchWeightCostPerWeight").Value Is DBNull.Value, 0.0, rsOrderItem.Fields("CatchWeightCostPerWeight").Value)
                Me.CatchWeightUnitCostTextBox.Text = VB6.Format(CatchWeightUnitCost, "#####0.00##")
                LoadUnitAbbreviation(Me.CatchWeightUnitDescCmb)
                cmbField(iOrderItemCostUnit).Items.Clear()
                LoadUnitAbbreviation(cmbField(iOrderItemCostUnit))
                SetCombo(Me.CatchWeightUnitDescCmb, CInt(rsOrderItem.Fields("Retail_Unit_ID").Value))

                ' ****** Set up Controls for CatchWeight Items - AZ ********
                _cmbField_1.Width = 45
                CatchWeightUnitCostTextBox.Visible = True
                CatchWeightUnitDescCmb.Visible = True
                Slashlbl.Visible = True
            Else
                _cmbField_1.Width = 129
                CatchWeightUnitCostTextBox.Visible = False
                CatchWeightUnitDescCmb.Visible = False
                Slashlbl.Visible = False
            End If

            ReplicateCombo(cmbField(iOrderItemCostUnit), cmbField(iOrderItemFreightUnit))

            If (Not IsDBNull(rsOrderItem.Fields("CostUnit").Value)) Then
                SetCombo(cmbField(iOrderItemCostUnit), CInt(rsOrderItem.Fields("CostUnit").Value))
            End If

            SetCombo(cmbField(iOrderItemQuantityUnit), CInt(rsOrderItem.Fields("QuantityUnit").Value))

            If (Not IsDBNull(rsOrderItem.Fields("FreightUnit").Value)) Then
                SetCombo(cmbField(iOrderItemFreightUnit), CInt(rsOrderItem.Fields("FreightUnit").Value))
            End If

            dtpReceivedDate.Value = VB6.Format(rsOrderItem.Fields("DateReceived").Value, "MM/DD/YYYY Hh:Mm:Ss")
            dtpExpirationDate.Value = VB6.Format(rsOrderItem.Fields("ExpirationDate").Value, "MM/DD/YYYY")
            txtField(iOrderItemItem_Description).Text = rsOrderItem.Fields("ItemDescription").Value & ""
            txtField(iOrderItemIdentifier).Text = rsOrderItem.Fields("Identifier").Value
            txtField(iOrderItemUnits_Per_Pallet).Text = rsOrderItem.Fields("Units_Per_Pallet").Value

            If Not IsDBNull(txtField(iOrderItemPackage_Desc1).Text) Then
                txtField(iOrderItemPackage_Desc1).Text = CDec(rsOrderItem.Fields("Package_Desc1").Value).ToString("####0.##")
            Else
                txtField(iOrderItemPackage_Desc1).Text = ""
            End If

            If ReceiveLocationIsDistribution Then
                listPackSizes = LoadPackSizes(glOrderItemID)
                ComboBox_VendorPack.Items.Clear()

                Dim _index As Integer
                For Each item As packSizeItem In listPackSizes
                    _index = ComboBox_VendorPack.Items.Add(item)

                    If item.PackSize = rsOrderItem.Fields("Package_Desc1").Value Then
                        ComboBox_VendorPack.SelectedIndex = _index
                    End If
                Next

                ComboBox_VendorPack.Visible = True
                ComboBox_VendorPack.BringToFront()
                txtField(iOrderItemPackage_Desc1).Visible = False
                ComboBox_VendorPack.DisplayMember = "PackSize"
                ComboBox_VendorPack.ValueMember = "UnitPrice"
            Else
                ComboBox_VendorPack.Visible = False
                txtField(iOrderItemPackage_Desc1).Visible = True
                txtField(iOrderItemPackage_Desc1).BringToFront()
                Application.DoEvents()
            End If

            If Not IsDBNull(txtField(iOrderItemPackage_Desc2).Text) Then
                txtField(iOrderItemPackage_Desc2).Text = CDec(rsOrderItem.Fields("Package_Desc2").Value).ToString("####0.##")
            Else
                txtField(iOrderItemPackage_Desc2).Text = ""
            End If

            SetCombo(cmbField(iOrderItemRetail_Unit_ID), CInt(rsOrderItem.Fields("Retail_Unit_ID").Value))
            chkField(iRetail_Sale).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("Retail_Sale").Value))
            chkField(iKeep_Frozen).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("Keep_Frozen").Value))
            chkField(iFull_Pallet_Only).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("Full_Pallet_Only").Value))
            chkField(iShipper_Item).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("Shipper_Item").Value))
            chkField(iWFM_Item).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("WFM_Item").Value))
            chkField(iOrderItemCOOL).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("OrderItemCOOL").Value))
            chkField(iOrderItemBIO).CheckState = System.Math.Abs(CInt(rsOrderItem.Fields("OrderItemBIO").Value))

            If Not IsDBNull(rsOrderItem.Fields("Carrier").Value) Then
                txtField(iOrderItemCarrier).Text = rsOrderItem.Fields("Carrier").Value
            End If

            gbWFMItem = rsOrderItem.Fields("WFM_Item").Value
            pbNoDistMarkup = rsOrderItem.Fields("NoDistMarkup").Value
            pbIsWeightRequired = rsOrderItem.Fields("IsWeightRequired").Value
            pbIsCostedByWeight = rsOrderItem.Fields("CostedByWeight").Value

            '-- Make sure the unit attributes are set for receiving/ordering
            ' The business rules for Received value is that it should always be an integer UNLESS the
            ' item is costed by weight.  
            If pbIsCostedByWeight Then
                txtField(iOrderItemQuantityReceived).Tag = "ExtCurrency"
            Else
                txtField(iOrderItemQuantityReceived).Tag = "Number"
            End If

            '-- Get the line number and line count
            Me.Text = "Line Item Information  [" & Trim(rsOrderItem.Fields("CurrentRecord").Value) & " of " & Trim(rsOrderItem.Fields("TotalRecords").Value) & "]"

            m_IsDSDOrder = frmOrders.IsDSDOrder

            SetEditPermissions()

            If Not txtField(iOrderItemQuantityOrdered).ReadOnly Then
                txtField(iOrderItemUnits_Per_Pallet).Text = rsOrderItem.Fields("Units_Per_Pallet").Value
            End If

            If OrderingDAO.IsOrderEinvoice(glOrderHeaderID, frmOrders.EInvoiceId) Then
                cmdDisplayEInvoice.Enabled = True
            Else
                cmdDisplayEInvoice.Enabled = False
            End If
            End If

            rsOrderItem.Close()

            SetDataTypesBasedOnOrderedQuantityUnit(iOrderItemQuantityUnit)

            bLoading = False

            CatchWeightAdjustCost()

            If lRecord <> -1 And glOrderItemID = -1 Then
                ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
                ' It has already been successfully added to the order, passing all data validation.
                newOrderItem = False
                RefreshData(-1)
                CheckNoOrderItems()
            End If

            If geOrderType = enumOrderType.Distribution And Not gbClosedOrder Then
                If (gbBuyer And gbWarehouse) Or gbSuperUser Then
                    SetActive(txtField(iOrderItemAdjustedCost), True)
                    ucReasonCode.Enabled = True
                Else
                    SetActive(txtField(iOrderItemAdjustedCost), False)
                    ucReasonCode.Enabled = False
                End If
            End If

        If blnCreditReasonCodeAutoSet Or pbUseHostedCost Then
            pbDataChanged = True
        Else
            pbDataChanged = False
        End If

            logger.Debug("RefreshData Exit")
    End Sub

    Private Function LoadPackSizes(ByRef _orderItemId As Integer) As List(Of packSizeItem)

        Dim rsPackSizes As DAO.Recordset = Nothing
        Dim listPackSizes As List(Of packSizeItem) = New List(Of packSizeItem)
        Dim foundPackSizes As List(Of Single) = New List(Of Single)
        Dim packSizeItem As packSizeItem

        SQLOpenRS(rsPackSizes, "EXEC getPackSizesByOrderItem " & _orderItemId, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        While Not rsPackSizes.EOF
            If Not foundPackSizes.Contains(CSng(rsPackSizes("PackSize").Value)) Then
                foundPackSizes.Add(CSng(rsPackSizes("PackSize").Value))
                packSizeItem = New packSizeItem()
                packSizeItem.VendorCostHistoryId = rsPackSizes("vendorcosthistoryid").Value
                packSizeItem.PackSize = rsPackSizes("PackSize").Value
                packSizeItem.UnitCost = rsPackSizes("UnitCost").Value
                listPackSizes.Add(packSizeItem)
            End If

            rsPackSizes.MoveNext()
        End While

        rsPackSizes.Close()

        Return listPackSizes

    End Function

    Private Sub SetEditPermissions()
        logger.Debug("SetEditPermissions Entry")

        Dim OrderingDAO As New OrderingDAO
        Dim bBuyerDistributor As Boolean = gbBuyer Or gbDistributor
        Dim canAdjustCost As Boolean = (gbCoordinator OrElse gbBuyer OrElse gbSuperUser) And (Not gbClosedOrder) And ((frmOrders.OriginalOrder_ID = -1) Or frmOrders.IsCredit) And (Global_Renamed.geOrderType <> enumOrderType.Distribution And Global_Renamed.geOrderType <> enumOrderType.Flowthru)
        Dim iLoop As Integer

        '-- Set Buyer rights
        For iLoop = iOrderItemQuantityOrdered To iOrderItemAdjustedCost ' iOrderItemReceivedItemFreight
            Select Case iLoop
                Case iOrderItemHandling, _
                        iOrderItemLineItemCost, _
                        iOrderItemReceivedItemCost, _
                        iOrderItemMarkupCost, _
                        iOrderItemMarkupPercent, _
                        iOrderItemLandingCost, _
                        iOrderItemLineItemFreight, _
                        iOrderItemLineItemHandling, _
                        iOrderItemReceivedItemFreight, _
                        iOrderItemReceivedItemHandling, _
                        iOrderItemCost
                    SetActive(txtField(iLoop), False)

                Case iOrderItemNumber_Of_Pallets, _
                    iOrderItemQuantityOrdered

                    SetActive(txtField(iLoop), gbBuyer And (Not gbClosedOrder) And (Not frmOrders.EXEWarehouse Or Not frmOrders.Sent Or (gbWarehouse And pbAllocationAllowed) Or frmOrders.ItemsReceived))

                    If Global_Renamed.geOrderType = enumOrderType.Distribution Or Global_Renamed.geOrderType = enumOrderType.Flowthru Then
                        SetActive(CatchWeightUnitCostTextBox, False, Not pbIsCatchWeightItem)
                    Else
                        SetActive(CatchWeightUnitCostTextBox, gbBuyer And (Not gbClosedOrder) And (Not frmOrders.EXEWarehouse Or Not frmOrders.Sent Or (gbWarehouse And pbAllocationAllowed) Or frmOrders.ItemsReceived), Not pbIsCatchWeightItem)
                    End If

                Case iOrderItemAdjustedCost
                    SetActive(txtField(iLoop), canAdjustCost)

                Case iOrderItemFreight
                    'Freight is now made read-only for everyone
                    SetActive(txtField(iLoop), False)

                Case iOrderItemQuantityDiscount
                    'If order is not closed and is not a return order, Coordinators can change discount; buyers and distributors can change if m_bEXEWarehousePurchaseOrder
                    SetActive(txtField(iLoop), (gbCoordinator Or (gbBuyer And (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase))) And (Not gbClosedOrder) And (frmOrders.OriginalOrder_ID = -1) And (Global_Renamed.geOrderType <> enumOrderType.Distribution And Global_Renamed.geOrderType <> enumOrderType.Flowthru))

                Case Else
                    ' This try catch was added because txtField(19) did not exist and was causing an error.
                    Try
                        SetActive(txtField(iLoop), gbCoordinator And (Not gbClosedOrder) And (frmOrders.OriginalOrder_ID = -1) And (Not frmOrders.EXEWarehouse Or Not frmOrders.Sent Or (gbWarehouse And pbAllocationAllowed) Or frmOrders.ItemsReceived))
                    Catch
                    End Try

            End Select
        Next iLoop

        SetActive(txtField(iOrderItemQuantityAllocated), pbAllocationAllowed)

        For iLoop = cmbField.LBound To cmbField.UBound
            Select Case iLoop
                Case iOrderItemHandlingUnit, iOrderItemLandingUnit, iOrderItemPackage_Unit_ID, iOrderItemMarkupUnit, iOrderItemRetail_Unit_ID : SetActive(cmbField(iLoop), False)
                Case iOrderItemDiscountType : SetActive(cmbField(iLoop), (Not txtField(iOrderItemQuantityDiscount).ReadOnly))
                Case iOrderItemFreightUnit
                    SetActive(cmbField(iLoop), ((gbCoordinator And IsDBNull(rsOrderItem.Fields("Item_Freight_Unit_ID").Value)) Or ((gbBuyer Or gbCoordinator) And (frmOrders.IsStore_Vend))) And (Not gbClosedOrder))
                Case iOrderItemQuantityUnit
                    SetActive(cmbField(iLoop), Not gbClosedOrder And ((cmbField(iOrderItemQuantityUnit).SelectedIndex = -1) Or frmOrders.IsStore_Vend Or frmOrders.IsCredit) And (Not frmOrders.EXEWarehouse Or Not frmOrders.Sent Or (gbWarehouse And pbAllocationAllowed) Or frmOrders.ItemsReceived))
                Case iOrderItemOrigin_ID, iOrderItemCountryProc_ID : SetActive(cmbField(iLoop), Not gbClosedOrder And (gbBuyer Or gbCoordinator Or gbWarehouse))
                Case iOrderItemCreditReason
                    SetActive(cmbField(iLoop), frmOrders.IsCredit And Not gbClosedOrder)
                Case Else : SetActive(cmbField(iLoop), (gbCoordinator And (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase)) And Not gbClosedOrder)
            End Select
        Next iLoop

        ucReasonCode.Enabled = Not txtField(iOrderItemAdjustedCost).ReadOnly
        SetActive(cmdAdd, gbBuyer And Not gbClosedOrder And pbAddAllowed)
        SetActive(cmdDelete, gbBuyer And Not gbClosedOrder And pbDeleteAllowed)
        SetActive(cmdHistory, gbBuyer)
        SetActive(txtField(iOrderItemLotNo), (Not gbClosedOrder) And gbDistributor)
        SetActive(cmdOriginAdd, gbCoordinator And (Not gbClosedOrder) And Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase)

        ' Can Adjust Costs/Discounts TFS 10291
        ucReasonCode.Enabled = canAdjustCost
        SetActive(txtField(iOrderItemQuantityDiscount), canAdjustCost)
        SetActive(cmbField(iOrderItemDiscountType), canAdjustCost)

        '-- Set distributor rights
        ' TFS 9833, just leave this field to be editable by Receiver
        ' also made Receive Icon to be not visible, this can be physically removed later
        'TFS 9899, the buyer role should have access to the Expiration Date field 
        For iLoop = iOrderItemQuantityReceived To iOrderItemExpirationDate
            Select Case iLoop
                Case iOrderItemDateReceived
                    SetActive(dtpReceivedDate, False)
                Case iOrderItemExpirationDate
                    SetActive(dtpExpirationDate, (gbBuyer Or gbDistributor) And Not gbClosedOrder)
                Case iOrderItemTotal_Weight
                    SetActive(txtField(iLoop), False)
                Case Else
                    SetActive(txtField(iLoop), False)
            End Select
        Next iLoop

        SetActive(cmdPrevious, rsOrderItem.Fields("CurrentRecord").Value > 1)
        SetActive(cmdNext, rsOrderItem.Fields("CurrentRecord").Value < rsOrderItem.Fields("TotalRecords").Value)
        SetActive(cmbField(iOrderItemQuantityUnit), False)

        'Activation depends on other control, so put here at the end
        SetActive(Me.cmbSustainRanking, cmbField(iOrderItemOrigin_ID).Enabled)

        'Buyer can edit UOM field if this is a return order
        If gbBuyer And OrderingDAO.IsOrderStateCurrent(False, False, False, glOrderHeaderID) Then
            If frmOrders.IsCredit = True And geOrderType = enumOrderType.Purchase Or geOrderType = enumOrderType.Transfer Then
                SetActive(cmbField(iOrderItemQuantityUnit), True)
            End If
        End If

        'PO Editor role can edit the Pack, Qty, and UOM fields for all POs with a status prior to "Uploaded"
        If gbPOEditor And Not OrderingDAO.IsOrderStateCurrent(True, True, True, glOrderHeaderID) Then
            If ComboBox_VendorPack.Visible Then
                SetActive(ComboBox_VendorPack, True)
            Else
                SetActive(txtField(iOrderItemPackage_Desc1), True)
            End If

            SetActive(txtField(iOrderItemQuantityOrdered), True)
            SetActive(cmbField(iOrderItemQuantityUnit), True)
        End If

        If gbDistributor And Not gbClosedOrder Then
            SetActive(cmbField(iOrderItemQuantityUnit), True)
        End If

        If (m_IsDSDOrder) Then
            _lblLabel_2.Visible = False
            _txtField_5.Visible = False

            ' copy UOM value to Qty Received label
            Dim uom As String
            uom = _cmbField_0.Text
            _lblLabel_15.Text = ""
            _lblLabel_15.Text = "Qty Received (" + Trim(uom) + ") :"

            ' then hide the uom combo
            _cmbField_0.Visible = False

            ' Qty Allocated
            _lblLabel_14.Visible = False
            _txtField_25.Visible = False

            ' Pallets
            _lblLabel_4.Visible = False
            _txtField_6.Visible = False
        End If

        logger.Debug("SetEditPermissions Exit")
    End Sub

    Private Sub CalculateCost(ByVal iFrom As Short, ByVal iTo As Short)
        logger.Debug("CalculateCost Entry")

        Dim cCost As Decimal
        Dim iRetail As Short
        Dim dFreeUnits As Decimal
        Dim totalUnits As Decimal
        Dim quantOrdered As Decimal
        Dim cu As tItemUnit
        Dim fu As tItemUnit

        ' TFS 11974 - it's not pretty I know this.
        If iFrom = iOrderItemMarkupCost And iTo = iOrderItemMarkupCost And pbIsCatchWeightItem Then
            Exit Sub
        End If

        dFreeUnits = IIf(VB6.GetItemData(cmbField(iOrderItemDiscountType), cmbField(iOrderItemDiscountType).SelectedIndex) = 3, _
                         CDec(FixNumber(txtField(iOrderItemQuantityDiscount).Text)), 0)
        quantOrdered = CDec(FixNumber(txtField(iOrderItemQuantityOrdered).Text))
        totalUnits = quantOrdered - dFreeUnits

        bLoading = True

        Dim tempQuantityReceived As Decimal
        If txtField(iOrderItemQuantityReceived).Text = "" Then
            tempQuantityReceived = 0
        Else
            tempQuantityReceived = CDec(txtField(iOrderItemQuantityReceived).Text)
        End If

        '-- Convert Cost, Handling, Freight to their respective units
        For iRetail = iFrom To iTo

            If iRetail >= iOrderItemLineItemCost Then
                Select Case iRetail
                    Case iOrderItemLineItemCost, iOrderItemReceivedItemCost
                        Dim adjustedCost As Decimal

                        '-- Use the non-zero adjusted cost above all else. Otherwise if the order has a Percent Discount use the CurrentVendorCost
                        If (Decimal.TryParse(FixNumber(txtField(iOrderItemAdjustedCost).Text), adjustedCost) AndAlso adjustedCost > 0) Then
                            cCost = adjustedCost
                        ElseIf ((frmOrders.OrderDiscountExists And frmOrders.OrderDiscountType = 2) Or _
                                    VB6.GetItemData(cmbField(iOrderItemDiscountType), cmbField(iOrderItemDiscountType).SelectedIndex) = 2) Then
                            cCost = currentVendorCost
                        Else
                            cCost = CDec(FixNumber(txtField(iOrderItemCost).Text))
                        End If

                        Select Case VB6.GetItemData(cmbField(iOrderItemDiscountType), cmbField(iOrderItemDiscountType).SelectedIndex)
                            Case 0
                                If m_VendorDiscountAmt > 0 Then
                                    ' Sekhara To fix the bug 6069.( IN case of AdjustedCost is >0 then no vendorItem Discount).
                                    If adjustedCost = 0 Then
                                        'cCost = cCost - m_VendorDiscountAmt
                                    End If
                                    ' End of the fix for 6069.
                                ElseIf frmOrders.OrderDiscountExists Then
                                    Select Case frmOrders.OrderDiscountType
                                        Case 1 : cCost = cCost - frmOrders.OrderDiscountAmt
                                        Case 2 : cCost = cCost - (cCost * (frmOrders.OrderDiscountAmt / 100))
                                        Case 3 : cCost = cCost - (cCost * (frmOrders.OrderDiscountAmt / 100))
                                        Case 4 : cCost = cCost - (cCost * (frmOrders.OrderDiscountAmt / 100))
                                    End Select
                                End If

                            Case 1 : cCost = cCost - CDec(FixNumber(txtField(iOrderItemQuantityDiscount).Text))
                            Case 2 : cCost = cCost * ((100 - CDec(FixNumber(txtField(iOrderItemQuantityDiscount).Text))) / 100)
                            Case 3 : cCost = (cCost * totalUnits) / quantOrdered
                        End Select

                        If iRetail = iOrderItemLineItemCost Then
                            cCost = CostConversion(cCost, ComboVal(cmbField(iOrderItemCostUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(FixNumber(txtField(iOrderItemTotal_Weight).Text)), CDec(FixNumber(txtField(iOrderItemQuantityReceived).Text))) '* (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100
                        Else
                            cCost = CostConversion(cCost, ComboVal(cmbField(iOrderItemCostUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(FixNumber(txtField(iOrderItemTotal_Weight).Text)), CDec(FixNumber(txtField(iOrderItemQuantityReceived).Text))) * (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100
                        End If

                    Case iOrderItemLineItemFreight, iOrderItemReceivedItemFreight
                        cCost = CostConversion(CDec(FixNumber(txtField(iOrderItemFreight).Text)), ComboVal(cmbField(iOrderItemFreightUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(FixNumber(txtField(iOrderItemTotal_Weight).Text)), CDec(FixNumber(txtField(iOrderItemQuantityReceived).Text))) * (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100
                    Case iOrderItemLineItemHandling, iOrderItemReceivedItemHandling
                        cCost = CostConversion(CDec(FixNumber(txtField(iOrderItemHandling).Text)), ComboVal(cmbField(iOrderItemHandlingUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(FixNumber(txtField(iOrderItemTotal_Weight).Text)), CDec(FixNumber(txtField(iOrderItemQuantityReceived).Text)))
                End Select

                If iRetail >= iOrderItemReceivedItemCost Then
                    If CDec(FixNumber(txtField(iOrderItemQuantityReceived).Text)) = 0 Then
                        cCost = 0
                    Else
                        If ComboVal(cmbField(iOrderItemDiscountType)) = 3 And iRetail = iOrderItemReceivedItemCost Then
                            If CDec(txtField(iOrderItemQuantityReceived).Text) < CDec(txtField(iOrderItemQuantityDiscount).Text) Then
                                cCost = 0
                            Else
                                cCost = cCost * (CDec(txtField(iOrderItemQuantityReceived).Text) - CDec(txtField(iOrderItemQuantityDiscount).Text))
                            End If
                        Else
                            cCost = cCost * CDec(txtField(iOrderItemQuantityReceived).Text)
                        End If
                    End If
                Else
                    cCost = cCost * quantOrdered
                End If

            Else

                ' If adjusted cost = 0, calculate the actual cost
                ' otherwise use adjusted cost
                If CDec(FixNumber(txtField(iOrderItemAdjustedCost).Text)) = 0 Then

                    '-- If the order has a Percent Discount use the CurrentVendorCost
                    If ((frmOrders.OrderDiscountExists And frmOrders.OrderDiscountType = 2) Or _
                                VB6.GetItemData(cmbField(iOrderItemDiscountType), cmbField(iOrderItemDiscountType).SelectedIndex) = 2) Then
                        cCost = currentVendorCost
                    Else
                        cCost = CDec(FixNumber(txtField(iOrderItemCost).Text))
                    End If

                    Select Case VB6.GetItemData(cmbField(iOrderItemDiscountType), cmbField(iOrderItemDiscountType).SelectedIndex)
                        Case 0
                            If frmOrders.OrderDiscountExists Then
                                Select Case frmOrders.OrderDiscountType
                                    Case 1 : cCost = cCost - frmOrders.OrderDiscountAmt
                                    Case 2 : cCost = cCost - (cCost * (frmOrders.OrderDiscountAmt / 100))
                                    Case 4 : cCost = cCost - (cCost * (frmOrders.OrderDiscountAmt / 100))
                                End Select
                            End If
                        Case 1 : cCost = cCost - CDec(FixNumber(txtField(iOrderItemQuantityDiscount).Text))
                        Case 2 : cCost = cCost * ((100 - CDec(FixNumber(txtField(iOrderItemQuantityDiscount).Text))) / 100)
                        Case 3
                            If CDec(txtField(iOrderItemQuantityOrdered).Text) <> 0 Then
                                cCost = (cCost * totalUnits) / quantOrdered
                            Else
                                cCost = 0
                            End If
                    End Select

                    cu = GetItemUnit(ComboVal(cmbField(iOrderItemCostUnit)))
                    fu = GetItemUnit(ComboVal(cmbField(iOrderItemFreightUnit)))
                    cCost = CostConversion(cCost, ComboVal(cmbField(iOrderItemCostUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)
                    pcUnitCost = CostConversion(cCost, ComboVal(cmbField(iOrderItemQuantityUnit)), IIf(cu.IsPackageUnit, giUnit, ComboValue(cmbField(iOrderItemCostUnit))), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)
                    cCost = cCost + CostConversion(CDec(FixNumber(txtField(iOrderItemFreight).Text)), ComboVal(cmbField(iOrderItemFreightUnit)), ComboVal(cmbField(iOrderItemQuantityUnit)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)
                    pcUnitExtCost = CostConversion(cCost, ComboVal(cmbField(iOrderItemQuantityUnit)), IIf(fu.IsPackageUnit, giUnit, ComboValue(cmbField(iOrderItemFreightUnit))), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)

                    If iRetail >= iOrderItemMarkupCost Then
                        pcUnitCost = pcUnitCost * (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100
                        pcUnitExtCost = pcUnitExtCost * (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100

                        'calculate based on whether the Handling Charge is a percent or dollar amount
                        If optPercent.Checked Then
                            cCost = cCost * (100 + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))) / 100
                        Else
                            cCost = cCost + CDec(FixNumber(txtField(iOrderItemMarkupPercent).Text))
                        End If

                    End If

                    cCost = cCost + CostConversion(CDec(txtField(iOrderItemHandling).Text), ComboVal(cmbField(iOrderItemHandlingUnit)), ComboVal(cmbField(iRetail - 8)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)
                Else
                    ' ...otherwise use adjusted cost
                    If iRetail = iOrderItemLandingCost Then
                        cCost = CDec(txtField(iOrderItemAdjustedCost).Text)
                    Else
                        cCost = CostConversion(CDec(txtField(iOrderItemAdjustedCost).Text), ComboVal(cmbField(iOrderItemQuantityUnit)), ComboVal(cmbField(iRetail - 8)), CDec(txtField(iOrderItemPackage_Desc1).Text), CDec(txtField(iOrderItemPackage_Desc2).Text), ComboVal(cmbField(iOrderItemPackage_Unit_ID)), CDec(txtField(iOrderItemTotal_Weight).Text), tempQuantityReceived)
                    End If
                End If
            End If

            ' Include 3rd party Freight in Landed Cost calc
            If iRetail = iOrderItemLandingCost Or iRetail = iOrderItemMarkupCost Then
                cCost += m_Freight3PartyAvg
            End If

            txtField(iRetail).Text = VB6.Format(cCost, "#####0.00##")
        Next iRetail

        bLoading = False

        logger.Debug("CalculateCost Exit")
    End Sub

#End Region

#Region "Form Event Handlers"

    Private Sub frmOrdersItem_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrdersItem_FormClosing Entry")

        Dim lIgnoreErrNum(0) As Integer

        If glOrderHeaderID > -1 Then
            e.Cancel = Not SaveData(True)
            If e.Cancel = True And ValidateFormValues() Then
                If MsgBox("Close without saving?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    e.Cancel = False

                    ' If the order item has been saved to the DB, perform additional checks before closing the form.
                    If glOrderItemID <> -1 Then
                        ' Is this a new item being added to the order?  If so, it should be removed from the order
                        ' because it did not pass data entry validation and the user elected to exit from the edit screen
                        ' without saving.
                        If newOrderItem Then
                            logger.Info("frmOrdersItem_FormClosing - calling DeleteOrderItem to remove the new item from the order: glOrderItemID=" + glOrderItemID.ToString + ", giUserID=" + giUserID.ToString)

                            lIgnoreErrNum(0) = 50002
                            SQLExecute3("EXEC DeleteOrderItem " & glOrderItemID & "," & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

                            If Err.Number <> 0 Then
                                logger.Error("frmOrdersItem_FormClosing Error in EXEC DeleteOrderItem " & glOrderItemID & "," & giUserID & Err.Description)
                                MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
                            End If
                            On Error GoTo 0

                        ElseIf CDec(txtField(iOrderItemQuantityOrdered).Text) <> m_OriginalQtyOrdered Then
                            ' If the Qty Ordered value has been changed, restore the old value
                            logger.Debug("Restoring old amt = " & m_OriginalQtyOrdered.ToString)
                            txtField(iOrderItemQuantityOrdered).Text = m_OriginalQtyOrdered.ToString

                            Dim dTotal3Party As Decimal
                            dTotal3Party = GetFreight3PartyTotal(glOrderHeaderID)

                            ' Run update to the line items
                            DistributeFreight(glOrderHeaderID, dTotal3Party)
                        End If
                    End If
                Else
                    Exit Sub
                End If
            ElseIf Not ValidateFormValues() Then
                MsgBox("Invalid data exists that must be corrected before changes can be saved." & Environment.NewLine & Environment.NewLine & "Please correct it before exiting.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                e.Cancel = True
            End If
        End If

        logger.Debug("frmOrdersItem_FormClosing Exit")

        If Not e.Cancel Then
            If frmOrders.IsCredit AndAlso frmOrders.chkApplyAllCreditReason.Visible AndAlso cmbField(iOrderItemCreditReason).SelectedIndex > -1 Then
                frmOrders.lblCreditReasonCode.Text = VB6.GetItemData(cmbField(iOrderItemCreditReason), cmbField(iOrderItemCreditReason).SelectedIndex)
            End If
            'call update order refreshcosts. ghetto style. 
            'add string parameter 'OrderItemForm' to track where UORC is called
            SQLExecute3(String.Format("EXEC UpdateOrderRefreshCosts {0}, {1}", glOrderHeaderID, enumRefreshCostSource.OrderItemForm.ToString()), DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
        End If
    End Sub

    Private Sub frmOrdersItem_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmOrdersItem_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = ASCII_ENTER Then
            KeyAscii = ASCII_NULL
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("frmOrdersItem_KeyPress Exit")
    End Sub

    Private Sub frmOrdersItem_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrdersItem_Load Entry")

        Dim iLoop As Short

        CenterForm(Me)

        btnConversionCalculator.Visible = OrderSearchDAO.IsMultipleJurisdiction()

        '-- Load some combos
        For iLoop = cmbField.LBound To cmbField.UBound
            Select Case iLoop
                Case iOrderItemDiscountType
                    LoadDiscount(cmbField(iLoop), True)

                    sDiscountType(0) = "No Discount"
                    sDiscountType(1) = "Cash Discount"
                    sDiscountType(2) = "Percent Discount"
                    sDiscountType(3) = "Free Items"

                Case iOrderItemOrigin_ID : LoadOrigin(cmbField(iLoop))
                Case iOrderItemCountryProc_ID : LoadOrigin(cmbField(iLoop))
                Case iOrderItemQuantityUnit : LoadUnit(cmbField(iLoop))
                Case iOrderItemCostUnit, iOrderItemFreightUnit
                Case Else : ReplicateCombo(cmbField(iOrderItemQuantityUnit), cmbField(iLoop))
            End Select
        Next iLoop

        LoadCreditReasons(cmbField(10))

        'MD, TFS 2095: Using Reason Codes Framework to retrive the codes for selection
        LoadReasonCodesUltraCombo(ucReasonCode, enumReasonCodeType.CA)

        bLoading = True
        LoadSustainabilityRankings(cmbSustainRanking)
        bLoading = False

        '-- Capture Order Header info - direct references caused problems later - these should be properties set by the caller instead
        pbAllocationAllowed = frmOrders.cmdWarehouseSend.Enabled
        pbAddAllowed = frmOrders.cmdAddItem.Enabled
        pbDeleteAllowed = frmOrders.cmdDeleteItem.Enabled
        pbPre_Order = frmOrders.PreOrder
        pbIsExternalVendor = frmOrders.IsExternalVendor

        '-- Add the new item if its a new item
        If glItemID <> 0 Then
            ' Set the new order item flag to TRUE so that the item can be deleted if necessary.
            newOrderItem = True

            ' This saves the new item to the order before the order item details are completed.  The
            ' order details are then updated on exit.
            AddNewItem(glItemID)
            RefreshData(-1)
        Else
            ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
            ' It has already been successfully added to the order, passing all data validation.
            newOrderItem = False
            RefreshData(0)
        End If

        If geOrderType = enumOrderType.Distribution And Not gbClosedOrder Then
            If (gbBuyer And gbWarehouse) Or gbSuperUser Then
                SetActive(txtField(iOrderItemAdjustedCost), True)
                ucReasonCode.Enabled = True
            Else
                SetActive(txtField(iOrderItemAdjustedCost), False)
                ucReasonCode.Enabled = False
            End If
        End If

        'gbDistributors are either gbPO_Accountants or else Receivers
        'The fields Adjusted Cost and Discount are inaccessible to Receivers but accessible to PO Accountants
        If (gbDistributor And Not gbPOAccountant) And Not gbClosedOrder Then
            txtField(iOrderItemAdjustedCost).Enabled = (False Or txtField(iOrderItemAdjustedCost).Enabled)
            ucReasonCode.Enabled = (False Or ucReasonCode.Enabled)
            txtField(iOrderItemQuantityDiscount).Enabled = (False Or txtField(iOrderItemQuantityDiscount).Enabled)
            cmbField(iOrderItemDiscountType).Enabled = (False Or cmbField(iOrderItemDiscountType).Enabled)
        End If

        If gbPOAccountant And Not gbClosedOrder Then
            txtField(iOrderItemAdjustedCost).Enabled = (True And txtField(iOrderItemAdjustedCost).Enabled)
            ucReasonCode.Enabled = (True And ucReasonCode.Enabled)
            txtField(iOrderItemQuantityDiscount).Enabled = (True And txtField(iOrderItemQuantityDiscount).Enabled)
            cmbField(iOrderItemDiscountType).Enabled = (True And cmbField(iOrderItemDiscountType).Enabled)
        End If

        SetActive(optPercent, False)
        SetActive(optDollars, False)

        CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
        CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)

        'ItemDAO.GetCurrentNetCost(glItemID, frmOrders.StoreNo)

        If Not pbIsExternalVendor Then
            ' if this is a transfer PO, warn the user when there is no cost
            If CDbl(txtField(iOrderItemCost).Text) = 0 And CDbl(txtField(iOrderItemAdjustedCost).Text) = 0 Then
                MessageBox.Show("The cost for this item is 0.00, or it could not be determined." & Environment.NewLine & _
                                "If this is incorrect, please enter the Adjusted Cost for the item.", "Cost Warning", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        logger.Debug("frmOrdersItem_Load Exit")
    End Sub

#End Region

#Region "cmbField Event Handlers"

    Private Sub SetDataTypesBasedOnOrderedQuantityUnit(ByVal Index As Short)
        ' this code originally (v1) existed in cmbField_SelectedIndexChanged.
        ' I am moving it out so it can be called while initially loading an item without making any CalculateCost calls as well. Don't judge me.

        If cmbField(Index).SelectedIndex <= -1 Then Exit Sub

        If GetWeight_Unit(VB6.GetItemData(cmbField(Index), cmbField(Index).SelectedIndex)) And Not (giBox = VB6.GetItemData(cmbField(Index), cmbField(Index).SelectedIndex)) Then
            txtField(iOrderItemQuantityOrdered).Tag = "ExtCurrency"
            txtField(iOrderItemQuantityReceived).Tag = "ExtCurrency"
        Else
            txtField(iOrderItemQuantityOrdered).Tag = "Number"
            txtField(iOrderItemQuantityOrdered).Text = CStr(Int(CDbl(txtField(iOrderItemQuantityOrdered).Text)))
            txtField(iOrderItemQuantityReceived).Tag = "Number"
            txtField(iOrderItemQuantityReceived).Text = CStr(Int(CDbl(IIf(txtField(iOrderItemQuantityReceived).Text = "", 0, txtField(iOrderItemQuantityReceived).Text))))
            If txtField(iOrderItemQuantityReceived).Text = "0" Then txtField(iOrderItemQuantityReceived).Text = ""
        End If

    End Sub

    Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged
        logger.Debug("cmbField_SelectedIndexChanged Entry")

        If Me.IsInitializing = True Then Exit Sub
        If bLoading Then Exit Sub

        Dim Index As Short = cmbField.GetIndex(eventSender)

        pbDataChanged = True

        '-- Call the correct calculate event
        Select Case Index
            Case iOrderItemQuantityUnit
                SetCombo(cmbField(iOrderItemLandingUnit), VB6.GetItemData(cmbField(iOrderItemQuantityUnit), cmbField(iOrderItemQuantityUnit).SelectedIndex))
                SetCombo(cmbField(iOrderItemMarkupUnit), VB6.GetItemData(cmbField(iOrderItemQuantityUnit), cmbField(iOrderItemQuantityUnit).SelectedIndex))

                SetDataTypesBasedOnOrderedQuantityUnit(Index)

                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemFreight)
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)

            Case iOrderItemCostUnit
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemCost)
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemCost)

            Case iOrderItemDiscountType
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemFreight)
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)

            Case iOrderItemHandlingUnit
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemHandling, iOrderItemLineItemHandling)
                CalculateCost(iOrderItemReceivedItemHandling, iOrderItemReceivedItemHandling)

            Case iOrderItemFreightUnit
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemFreight, iOrderItemLineItemFreight)
                CalculateCost(iOrderItemReceivedItemFreight, iOrderItemReceivedItemFreight)
        End Select

        If chkField(iFull_Pallet_Only).CheckState And Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase And CInt(txtField(iOrderItemUnits_Per_Pallet).Text) <> 0 Then
            txtField(iOrderItemQuantityOrdered).Text = VB6.Format(Trim(FixValue(CStr(CDbl((txtField(iOrderItemNumber_Of_Pallets)).Text) * CInt(FixValue(txtField(iOrderItemUnits_Per_Pallet).Text))))), "#####0.0###")
        End If

        logger.Debug("cmbField_SelectedIndexChanged Exit")
    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        logger.Debug("cmbField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        Select Case Index
            Case iOrderItemOrigin_ID, iOrderItemCountryProc_ID
                If KeyAscii = ASCII_BACKSPACE Then
                    cmbField(Index).SelectedIndex = -1
                End If
        End Select

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbField_KeyPress Exit")
    End Sub

    Private Sub cmbSustainRanking_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSustainRanking.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = ASCII_BACKSPACE Then
            cmbSustainRanking.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmbSustainRanking_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSustainRanking.SelectedIndexChanged
        If bLoading Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub ucReasonCode_RowSelected(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ucReasonCode.RowSelected
        logger.Debug("=> ucReasonCode_RowSelected")

        If Me.IsInitializing Then Exit Sub

        If Not bLoading Then
            If Not ucReasonCode.ReadOnly And IsNumeric(txtField(8).Text) And cmbField(iOrderItemDiscountType).SelectedIndex <> 0 Then
                pbDataChanged = True
            End If
        End If

        logger.Debug("<= ucReasonCode_RowSelected")
    End Sub

#End Region

#Region "txtField Event Handlers"

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        If Not txtField(Index).ReadOnly Then
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        logger.Debug("txtField_TextChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

        Dim Index As Short = txtField.GetIndex(eventSender)
        Dim dDate As Date

        '-- Auto calculate the fields on the form
        If bLoading Then Exit Sub

        pbDataChanged = True

        Select Case Index
            Case iOrderItemNumber_Of_Pallets
                If IsNumeric(txtField(Index).Text) Then
                    If CDec(FixValue(txtField(Index).Text)) <> 0 And Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase And CShort(FixValue(txtField(iOrderItemUnits_Per_Pallet).Text)) <> 0 Then
                        txtField(iOrderItemQuantityOrdered).Text = Trim(Str(Int(CDec(FixValue(txtField(iOrderItemNumber_Of_Pallets).Text)) * CShort(FixValue(txtField(iOrderItemUnits_Per_Pallet).Text)))))
                    End If
                End If

            Case iOrderItemQuantityReceived
                If CDec(FixValue(txtField(Index).Text)) > 0 Then
                    If dtpReceivedDate.Value Is Nothing Then
                        If pdOriginalDateReceived = Date.MinValue Then
                            dDate = SystemDateTime()
                            dtpReceivedDate.Value = dDate
                        Else
                            dtpReceivedDate.Value = pdOriginalDateReceived
                        End If
                    End If
                    If pbIsWeightRequired Then
                        'Default weight
                        bLoading = True
                        txtField(iOrderItemTotal_Weight).Text = CStr(CDbl(FixValue(txtField(Index).Text)) * Val(txtField(iOrderItemPackage_Desc1).Text) * Val(txtField(iOrderItemPackage_Desc2).Text))
                        bLoading = False
                    End If
                Else
                    dtpReceivedDate.Value = String.Empty
                    bLoading = True
                    txtField(iOrderItemTotal_Weight).Text = "0.0"
                    bLoading = False
                End If

                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)

                If txtField(iOrderItemQuantityReceived).Text <> "" Then
                    SetActive(cmdReceiveDelete, frmOrders.ReceivingAllowed And (CInt(txtField(iOrderItemQuantityReceived).Text) > 0))
                End If

            Case iOrderItemTotal_Weight
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)

            Case iOrderItemQuantityOrdered
                If IsNumeric(txtField(Index).Text) Then
                    ' Recalc Freight3Party:
                    ' get difference current qty vs qty before editing
                    If CDec(FixValue(txtField(Index).Text)) = 0 Then
                        Exit Sub
                    End If

                    Dim NewQtyOrdered As Decimal = CDec(FixValue(txtField(Index).Text))
                    Dim DiffTotalQty As Decimal = NewQtyOrdered - m_OriginalQtyOrdered
                    Dim dTotal3Party As Decimal = GetFreight3PartyTotal(glOrderHeaderID)

                    ' pass to DistributeFreight() with diff
                    DistributeFreight(glOrderHeaderID, dTotal3Party, DiffTotalQty)

                    ' Set the value of the form-level variable m_Freight3PartyAvg here, 
                    ' so it can be used whenever the costs are recalc'ed
                    m_Freight3PartyAvg = GetFreight3PartyItemAvg(glOrderItemID, glOrderHeaderID)
                    txt3rdPartyFreightAvg.Text = m_Freight3PartyAvg
                    txt3rdPartyFreightLine.Text = m_Freight3PartyAvg * NewQtyOrdered

                    CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                    CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                    CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemFreight)
                    CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)
                End If

            Case iOrderItemPackage_Desc1
                If IsNumeric(txtField(Index).Text) Then
                    If CDbl(FixNumber(txtField(iOrderItemPackage_Desc1).Text)) <> 0 Then

                        CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemFreight)
                        CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                        CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                    End If
                End If

            Case iOrderItemQuantityDiscount
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemFreight)
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemFreight)

            Case iOrderItemAdjustedCost, iOrderItemCost
                CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemCost, iOrderItemLineItemCost)
                CalculateCost(iOrderItemReceivedItemCost, iOrderItemReceivedItemCost)

            Case iOrderItemHandling : CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemHandling, iOrderItemLineItemHandling)
                CalculateCost(iOrderItemReceivedItemHandling, iOrderItemReceivedItemHandling)

            Case iOrderItemFreight : CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
                CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
                CalculateCost(iOrderItemLineItemFreight, iOrderItemLineItemFreight)
                CalculateCost(iOrderItemReceivedItemFreight, iOrderItemReceivedItemFreight)
        End Select

        logger.Debug("txtField_TextChanged Exit")
    End Sub

    Private Sub txtField_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Leave
        logger.Debug("txtField_Leave Entry")

        Dim Index As Short = txtField.GetIndex(eventSender)
        Dim cWrkCurr As Decimal

        '-- If they can't change it, don't do anything
        If txtField(Index).ReadOnly Then
            Exit Sub
        End If

        bLoading = True

        Select Case Index
            Case iOrderItemQuantityOrdered, iOrderItemQuantityDiscount, iOrderItemQuantityReceived
                If Index = iOrderItemQuantityOrdered Then
                    '-- Find out about doing full pallet
                    If CShort(txtField(iOrderItemUnits_Per_Pallet).Text) <> 0 Then
                        If chkField(iFull_Pallet_Only).CheckState Then
                            If Int(CDec(txtField(iOrderItemQuantityOrdered).Text) / CShort(txtField(iOrderItemUnits_Per_Pallet).Text)) <> CDec(txtField(iOrderItemQuantityOrdered).Text) / CDec(txtField(iOrderItemUnits_Per_Pallet).Text) And Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase Then
                                cWrkCurr = (Int((CDec(txtField(iOrderItemQuantityOrdered).Text) / CDec(txtField(iOrderItemUnits_Per_Pallet).Text)) + 0.5)) * CDec(txtField(iOrderItemUnits_Per_Pallet).Text)
                                If CDbl(txtField(iOrderItemQuantityOrdered).Text) <> cWrkCurr Then txtField(iOrderItemQuantityOrdered).Text = CStr(cWrkCurr)
                            End If
                            cWrkCurr = CDec(txtField(iOrderItemQuantityOrdered).Text) / CShort(txtField(iOrderItemUnits_Per_Pallet).Text)
                            If txtField(iOrderItemNumber_Of_Pallets).Text <> Trim(Str(cWrkCurr)) Then txtField(iOrderItemNumber_Of_Pallets).Text = Trim(Str(cWrkCurr))
                        Else
                            cWrkCurr = CDec(txtField(iOrderItemQuantityOrdered).Text) / CShort(txtField(iOrderItemUnits_Per_Pallet).Text)
                            If txtField(iOrderItemNumber_Of_Pallets).Text <> VB6.Format(cWrkCurr, "#0.##") Then txtField(iOrderItemNumber_Of_Pallets).Text = VB6.Format(cWrkCurr, "#0.##")
                        End If
                    Else
                        If CDbl(txtField(iOrderItemNumber_Of_Pallets).Text) <> 0 Then txtField(iOrderItemNumber_Of_Pallets).Text = CStr(0)
                    End If
                End If

                If Index = iOrderItemQuantityDiscount Or Index = iOrderItemQuantityReceived Then
                    If Trim(txtField(Index).Text) = "" Then txtField(Index).Text = "0"
                Else
                    If Trim(txtField(Index).Text) = "" Then txtField(Index).Text = "1"
                End If

            Case iOrderItemNumber_Of_Pallets
                If txtField(Index).Text = "" Then txtField(Index).Text = "0"

            Case iOrderItemTotal_Weight
                If txtField(Index).Text = "" Then txtField(Index).Text = "0"

            Case iOrderItemCost, iOrderItemHandling, iOrderItemFreight, iOrderItemAdjustedCost
                '-- Auto Calculate the new margin
                cWrkCurr = CDec(FixValue(txtField(Index).Text))

                If txtField(Index).Text <> VB6.Format(cWrkCurr, "####0.00##") Then txtField(Index).Text = VB6.Format(cWrkCurr, "####0.00##")

                ''**** Adjust Cost Per Weight if Adjusted Cost changes for Catchweight Items ***** AZ
                If pbIsCatchWeightItem = True Then
                    CatchWeightAdjustCost()
                End If
        End Select

        bLoading = False

        If chkField(iFull_Pallet_Only).CheckState And Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Purchase And CInt(txtField(iOrderItemUnits_Per_Pallet).Text) <> 0 Then
            txtField(iOrderItemQuantityOrdered).Text = VB6.Format(Trim(FixValue(CStr(CDbl((txtField(iOrderItemNumber_Of_Pallets)).Text) * CInt(FixValue(txtField(iOrderItemUnits_Per_Pallet).Text))))), "#####0.0###")
        End If

        logger.Debug("txtField_Leave Exit")
    End Sub

#End Region

#Region "Button Event Handlers"

#Region "Navigation"

    Private Sub cmdNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNext.Click
        logger.Debug("cmdNext_Click Entry")

        '-- Go to next record
        If SaveData(True) Then

            ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
            ' It has already been successfully added to the order, passing all data validation.
            newOrderItem = False
            RefreshData(-2)

            CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
            CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
        End If

        logger.Debug("cmdNext_Click Exit")
    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrevious.Click
        logger.Debug("cmdPrevious_Click Entry")

        '-- Go to previous record
        If SaveData(True) Then
            ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
            ' It has already been successfully added to the order, passing all data validation.
            newOrderItem = False
            RefreshData(-3)

            CalculateCost(iOrderItemMarkupCost, iOrderItemMarkupCost)
            CalculateCost(iOrderItemLandingCost, iOrderItemLandingCost)
        End If

        logger.Debug("cmdPrevious_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

#End Region

#Region "Commands"

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        '-- Validate any data they are currently on
        If Not SaveData(True) Then Exit Sub

        Dim lCurrItemID As Integer
        Dim frmSearchInstance As New frmOrderItemSearch

        lCurrItemID = glItemID

        Call frmOrders.ConfigureOrderItemSearch(frmSearchInstance)

        frmSearchInstance.ShowDialog()

        '-- Add the new selected item
        If glItemID > 0 Then
            ' Set the new order item flag to TRUE so that the item can be deleted if necessary.
            newOrderItem = True

            frmOrders.PreOrder = frmSearchInstance.IsPreOrderItem
            frmOrders.IsEXEDistributed = frmSearchInstance.IsEXEDistributed

            '-- Add that item
            AddNewItem(glItemID)
            RefreshData(-1)
        Else
            ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
            ' It has already been successfully added to the order, passing all data validation.
            newOrderItem = False
            glItemID = lCurrItemID
        End If

        frmSearchInstance.Close()
        frmSearchInstance.Dispose()

        If Me.Visible Then txtField(iOrderItemQuantityOrdered).Focus()

        logger.Debug("cmdAdd_Click Exit")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        logger.Debug("cmdDelete_Click Entry")

        '-- Make sure they really want to delete that OrderItem
        If MsgBox("Really delete this line item?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.Question, "Delete Line Item") = MsgBoxResult.No Then
            Exit Sub
        End If

        SQLExecute("BEGIN TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        '-- Delete the OrderItem from the database
        Dim lIgnoreErrNum(0) As Integer
        lIgnoreErrNum(0) = 50002

        On Error Resume Next
        logger.Info("cmdDelete_Click - calling DeleteOrderItem to delete an item from an order: glOrderItemID=" + glOrderItemID.ToString + ", giUserID=" + giUserID.ToString)

        SQLExecute3("EXEC DeleteOrderItem " & glOrderItemID & "," & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
        If Err.Number <> 0 Then
            logger.Error(" cmdDelete_Click" & "Error in EXEC DeleteOrderItem " & Err.Description)
            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            On Error GoTo 0
            SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
        End If
        On Error GoTo 0

        SQLExecute("COMMIT TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        '-- Refresh the grid and seek the new one of its place
        ' Set the new order item flag to FALSE so that the item is not deleted on form closing.  
        ' It has already been successfully added to the order, passing all data validation.
        newOrderItem = False
        If cmdNext.Enabled Then
            RefreshData(-2)
        Else
            RefreshData(-1)
        End If

        '-- Make sure there are still records
        CheckNoOrderItems()

        logger.Debug("cmdDelete_Click Exit")
    End Sub

    Private Sub cmdOriginAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOriginAdd.Click
        logger.Debug("cmdOriginAdd_Click Entry")

        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glOriginID = 0

        frmOriginAdd.ShowDialog()
        frmOriginAdd.Dispose()

        If glOriginID = -2 Then
            LoadOrigin(cmbField(iOrderItemOrigin_ID))
            For lLoop = 0 To cmbField(iOrderItemOrigin_ID).Items.Count - 1
                If VB6.GetItemData(cmbField(iOrderItemOrigin_ID), lLoop) > lMaxValue Then
                    lMaxValue = VB6.GetItemData(cmbField(iOrderItemOrigin_ID), lLoop)
                    lMax = lLoop
                End If
            Next lLoop
            cmbField(iOrderItemOrigin_ID).SelectedIndex = lMax
        End If

        logger.Debug("cmdOriginAdd_Click Exit")
    End Sub

    Private Sub cmdHistory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdHistory.Click
        logger.Debug("cmdHistory_Click Entry")

        frmItemHistory.Identifier = Me.txtField(1).Text
        frmItemHistory.Text = "Order History For " & txtField(iOrderItemItem_Description).Text
        frmItemHistory.ShowDialog()
        frmItemHistory.Dispose()

        logger.Debug("cmdHistory_Click Exit")
    End Sub

    Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click
        logger.Debug("cmdItemEdit_Click Entry")

        If SaveData(True) Then
            Me.Enabled = False
            frmItem.ShowDialog()
            frmItem.Close()
            Me.Enabled = True
        End If

        logger.Debug("cmdItemEdit_Click Exit")
    End Sub

    Private Sub cmdItemOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemOrder.Click
        logger.Debug("cmdItemOrder_Click Entry")

        frmOrderCost.Text = "Order Item For " & txtField(iOrderItemItem_Description).Text
        frmOrderCost.ShowDialog()
        frmOrderCost.Dispose()

        logger.Debug("cmdItemOrder_Click Exit")
    End Sub

    Private Sub cmdOrderNotes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOrderNotes.Click
        logger.Debug("cmdOrderNotes_Click Entry")

        frmOrdersItemDesc.ShowDialog()
        frmOrdersItemDesc.Dispose()

        logger.Debug("cmdOrderNotes_Click Exit")
    End Sub

    Private Sub cmdReceiveDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReceiveDelete.Click
        logger.Debug("cmdReceiveDelete_Click Entry")

        SQLExecute("EXEC DeleteReceiving " & glOrderItemID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        'clear values on the screen
        txtField(iOrderItemQuantityReceived).Text = ""
        txtField(iOrderItemTotal_Weight).Text = "0.0"
        sOriginalQuantityReceived = ""
        dOriginalTotalWeight = 0

        logger.Debug("cmdReceiveDelete_Click Exit")
    End Sub

    Private Sub cmdDisplayEInvoice_Click(sender As System.Object, e As System.EventArgs) Handles cmdDisplayEInvoice.Click
        EInvoiceHTMLDisplay.EinvoiceId = frmOrders.EInvoiceId
        EInvoiceHTMLDisplay.StoreNo = frmOrders.StoreNo
        EInvoiceHTMLDisplay.ShowDialog()
    End Sub

    Private Sub btnConversionCalculator_Click(sender As System.Object, e As System.EventArgs) Handles btnConversionCalculator.Click
        logger.Debug("btnConversionCalculator_Click Entry")

        Dim ccForm As New FrmConvertMeasures
        ccForm.ShowDialog(Me)
        ccForm.Dispose()

        logger.Debug("btnConversionCalculator_Click Exit")
    End Sub

#End Region

#End Region

    Private Sub ComboBox_VendorPack_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_VendorPack.SelectedIndexChanged
        If ReceiveLocationIsDistribution Then
            ' -- if/end if condition added for bug 11384. Only get new vendor cost when orders are NOT closed.
            If (Not gbClosedOrder) Then
                pcUnitCost = CType(ComboBox_VendorPack.SelectedItem, packSizeItem).UnitCost
                Dim item As packSizeItem
                item = ComboBox_VendorPack.SelectedItem
                pcUnitCost = (currentVendorCost / currentVendorPackSize) * item.PackSize
                txtField(iOrderItemCost).Text = pcUnitCost
            End If

            ''**** Adjust Cost if Vendor Pack changes for Catchweight Items ***** AZ
            If pbIsCatchWeightItem = True And bLoading = False Then
                CatchWeightUnitCostChanged = True
                CatchWeightAdjustCost()
                CatchWeightUnitCostChanged = False
            End If
        End If
    End Sub

    Private Sub CatchWeightUnitCostTextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CatchWeightUnitCostTextBox.Leave
        If Me.IsInitializing = True Then Exit Sub
        If bLoading = False Then
            CatchWeightUnitCostChanged = True
            CatchWeightAdjustCost()
            CatchWeightUnitCostChanged = False
        End If
    End Sub

    Private Sub CatchWeightAdjustCost()
        If Me.IsInitializing = True Then Exit Sub

        Dim AdjustedCost As Decimal
        Dim CatchWeightUnitCost As Decimal
        Dim VendorPack As Decimal

        If IsNumeric(CatchWeightUnitCostTextBox.Text) And IsNumeric(txtField(iOrderItemAdjustedCost).Text) Then
            CatchWeightUnitCost = CatchWeightUnitCostTextBox.Text

            If listPackSizes.Count > 0 Then
                VendorPack = CDec(ComboBox_VendorPack.SelectedItem.ToString)
            Else
                VendorPack = FixNumber(txtField(iOrderItemPackage_Desc1).Text)
            End If

            AdjustedCost = txtField(iOrderItemAdjustedCost).Text

            If VendorPack > 0 And CatchWeightUnitCost > 0 And _
                CatchWeightUnitCostChanged = True Then

                ' **** Vendor Pack or Cost perWeight has changed - sync up Adjusted Cost
                AdjustedCost = VendorPack * CatchWeightUnitCost

                CatchWeightUnitCostTextBox.Text = Math.Round(CatchWeightUnitCost, 2)
                CatchWeightUnitCostTextBox.Text = VB6.Format(CatchWeightUnitCostTextBox.Text, "#####0.00##")
                txtField(iOrderItemAdjustedCost).Text = VB6.Format(CStr(Math.Round(AdjustedCost, 2)), "#####0.00##")
            Else
                ' ***** The Adjusted Cost has changed - sync up the cost per weight *****
                If VendorPack > 0 And AdjustedCost > 0 And AdjustedCost > CatchWeightUnitCost And CatchWeightUnitCost > 0 Then
                    CatchWeightUnitCost = AdjustedCost / VendorPack
                    CatchWeightUnitCostTextBox.Text = Math.Round(CatchWeightUnitCost, 2)
                    txtField(iOrderItemAdjustedCost).Text = VB6.Format(txtField(iOrderItemAdjustedCost).Text, "#####00.00##")
                End If
            End If
        Else
            CatchWeightUnitCostTextBox.Text = Math.Round(0, 2)
        End If
    End Sub

    Private Function ValidateFormValues() As Boolean

        Dim retVal As Boolean = True

        ' quantity ordered
        If IsNumeric(txtField(5).Text) And cmbField(0).SelectedIndex = -1 Then
            If CInt(txtField(5).Text) > 0 Then
                FormValidator.SetError(cmbField(0), "A selection is required!")
                retVal = False
            Else
                FormValidator.SetError(cmbField(0), "")
            End If
        Else
            FormValidator.SetError(cmbField(0), "")
        End If

        ' adjusted cost - Reason code selection is required - TFS 2095
        If ucReasonCode.Enabled AndAlso IsNumeric(txtField(24).Text) And IsNothing(ucReasonCode.Value) Then
            ' default value is 0, but that is valid
            If CDbl(txtField(24).Text) > 0 Then
                FormValidator.SetError(ucReasonCode, "A selection is required, please select a Reason Code for Adjusted Cost!")
                retVal = False
            Else
                FormValidator.SetError(ucReasonCode, "")
            End If
        Else
            FormValidator.SetError(ucReasonCode, "")
        End If

        ' freight
        If IsNumeric(txtField(10).Text) And cmbField(4).SelectedIndex = -1 Then
            ' default value is 0, but that is valid
            If CInt(txtField(10).Text) > 0 Then
                FormValidator.SetError(cmbField(4), "A selection is required!")
                retVal = False
            Else
                FormValidator.SetError(cmbField(4), "")
            End If
        Else
            FormValidator.SetError(cmbField(4), "")
        End If

        ' discount
        If cmbField(iOrderItemDiscountType).Enabled AndAlso IsNumeric(txtField(8).Text) And cmbField(iOrderItemDiscountType).SelectedIndex = 0 Then
            ' default value is 0, but that is valid
            If CInt(txtField(8).Text) > 0 Then
                FormValidator.SetError(cmbField(2), "A selection is required, please select a Discount Type!")
                retVal = False
            Else
                FormValidator.SetError(cmbField(2), "")
            End If
        Else
            FormValidator.SetError(cmbField(2), "")
        End If

        ' discount - Reason code selection is required - TFS 2095
        If retVal Then
            If ucReasonCode.Enabled AndAlso IsNumeric(txtField(8).Text) And IsNothing(ucReasonCode.Value) And cmbField(iOrderItemDiscountType).SelectedIndex <> 0 Then
                ' default value is 0, but that is valid
                If CInt(txtField(8).Text) > 0 Then
                    FormValidator.SetError(ucReasonCode, "A selection is required, please select a Reason Code for the Discount!")
                    retVal = False
                Else
                    FormValidator.SetError(ucReasonCode, "")
                End If
            Else
                FormValidator.SetError(ucReasonCode, "")
            End If
        End If

        If cmbField(iOrderItemDiscountType).SelectedIndex = 0 And Val(txtField(iOrderItemAdjustedCost).Text) = 0 Then
            ucReasonCode.Value = -1
        End If

        'For credit POs, Credit Reason is required
        If frmOrders.IsCredit AndAlso cmbField(iOrderItemCreditReason).SelectedIndex = -1 Then
            FormValidator.SetError(cmbField(iOrderItemCreditReason), "A selection is required, please select a Credit Reason Code for Credit PO!")
            retVal = False
        End If

        Return retVal

    End Function

End Class


Public Class packSizeItem
    Public VendorCostHistoryId As Integer
    Public PackSize As Single
    Public UnitCost As Single

    Public Overrides Function ToString() As String
        Return PackSize.ToString()
    End Function
End Class
