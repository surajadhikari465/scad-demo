Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.Ordering.DataAccess
Imports log4net
Friend Class frmDistributionCreditOrder
    Inherits System.Windows.Forms.Form

    Dim plOrig_Transfer_SubTeam As Integer 'Used to determine if the original order is Purchase
    Dim mdt As DataTable
    Private m_VendorDiscountAmt As Decimal
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Dim bChanges As Boolean
        ' Dim rsReport As ADODB.Recordset

        '-- Check to see if anything was really modified
        Dim rsChanges As DAO.Recordset
        rsChanges = SQLOpenRecordSet("EXEC GetReturnOrderChanges " & glInstance & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)


        ' New code to fix the Bug 5534.
        If (rsChanges.BOF And rsChanges.EOF) Then
            bChanges = False  ' No Distribution Credit Order records entered into Grid.
        Else
            bChanges = True
        End If

        If rsChanges IsNot Nothing Then
            rsChanges.Close()
            rsChanges = Nothing
        End If


        ' The following lines has been commented to fix the bug 5534.

        'bChanges = (rsChanges.Fields("Changes").Value > 0)
        'rsChanges.Close()


        'THE FOLLOWING CODE HAS BEEN DEPRECATED AS A RESULT OF 
        'DISCONTINUING USE OF THE INVENTORY.MDB ACCESS DATABASE
        'gDBReport.BeginTrans()
        'rsReport = New ADODB.Recordset
        '      rsReport.Open("SELECT COUNT(*) AS Changes FROM ReturnOrder WHERE Instance = " & glInstance & " AND Quantity > 0", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        'bChanges = (rsReport.Fields("Changes").Value > 0)
        'rsReport.Close()
        '      rsReport = Nothing
        'gDBReport.CommitTrans()
        'gJetFlush.RefreshCache(gDBReport)

        '-- Make sure that they don't want to save their changes
        If bChanges Then
            If MsgBox("Really exit without saving your changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then Exit Sub
            logger.Debug("cmdExit_Click - exit without saving -No")
        End If

        Me.Close()

        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdSubmit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSubmit.Click

        logger.Debug("cmdSubmit_Click Entry")
        'Dim rsReport As ADODB.Recordset = Nothing
        Dim rsOrderItem As dao.Recordset = Nothing
        Dim rsDistributionCreditOrderChanges As DAO.Recordset = Nothing
        Dim lOrderHeaderID As Integer
        Dim lOrderItemID As Integer
        Dim sLineItemCost As Decimal
        Dim sLineItemFreight As Decimal
        Dim sLineItemReceivedCost As Decimal
        Dim sLineItemReceivedFreight As Decimal
        Dim sMarkupCost As Decimal
        Dim sLandedCost As Decimal
        Dim sHandlingCharge As Decimal
        Dim iPackage_Unit As Integer
        Dim sReturnRatio As Decimal
        Dim sReturnUnitRatio As Decimal
        Dim sUnitCost As Decimal
        Dim sTotalOrigWeight As Decimal
        Dim sdt() As DataRow
        Dim ugRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'Dim cu As tItemUnit
        'Dim iCost_Unit As Integer
        'Dim fu As tItemUnit
        'Dim iFreight_Unit As Integer

        sdt = mdt.Select("Quantity > 0")
        If sdt.Length = 0 Then
            sdt = Nothing
            MsgBox("There are no items to place on the Distribution Credit Order.", MsgBoxStyle.Critical, Me.Text)
            logger.Info("cmdSubmit_Click - There are no items to place on the Distribution Credit Order.")
            logger.Debug("cmdSubmit_Click Exit")
            Exit Sub
        End If
        sdt = Nothing

        sdt = mdt.Select("Quantity > 0 AND CreditReason_ID IS NULL")
        If sdt.Length > 0 Then
            MsgBox("You must select a credit reason for each return item", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, Me.Text)
            logger.Info("cmdSubmit_Click - You must select a credit reason for each return item")
            logger.Debug("cmdSubmit_Click Exit")
            Exit Sub
        End If
        sdt = Nothing

        'gDBReport.BeginTrans()
        'rsReport = New ADODB.Recordset

        Try
            rsOrderItem = SQLOpenRecordSet("EXEC InsertReturnOrderHeader " & glOrderHeaderID & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            lOrderHeaderID = rsOrderItem.Fields("OrderHeader_ID").Value
        Finally
            If rsOrderItem IsNot Nothing Then
                rsOrderItem.Close()
                rsOrderItem = Nothing
            End If
        End Try

        Try

            For Each ugRow In Me.ugrdReturn.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)

                'vl = New Infragistics.Win.ValueList
                'vl.ValueListItems.Add(ugRow.Cells("QuantityUnit").Value, ugRow.Cells("QuantityUnit_Text").Value)
                'If Not ugRow.Cells("QuantityUnit").Value = ugRow.Cells("Retail_Unit_ID").Value Then
                '    vl.ValueListItems.Add(ugRow.Cells("Retail_Unit_ID").Value, ugRow.Cells("RetailUnit_Text").Value)
                'End If
                'ugRow.Cells("ReturnUnit_Id").ValueList() = vl

                If (ugRow.Cells("Quantity").Value > 0) Then

                    With ugRow
                        If IsDBNull(.Cells("Package_Unit_ID").Value) Then
                            iPackage_Unit = 0
                        Else
                            iPackage_Unit = .Cells("Package_Unit_ID").Value
                        End If

                        m_VendorDiscountAmt = 0

                        ' The following line has been changed to fis the Bug 5534.
                        m_VendorDiscountAmt = .Cells("NetVendorItemDiscount").Value
                        ' m_VendorDiscountAmt = .Cells("VendorNetDiscount").Value

                        If .Cells("ReturnUnit_Id").Value = .Cells("QuantityUnit").Value Then
                            sReturnUnitRatio = 1
                        Else
                            sReturnUnitRatio = 1 / .Cells("PackageSize").Value
                        End If

                        sReturnRatio = .Cells("Quantity").Value / .Cells("Quantity_Previous").Value

                        sUnitCost = .Cells("UnitCost").Value
                        sTotalOrigWeight = .Cells("Total_Weight_Previous").Value

                        sLandedCost = .Cells("LandedCost").Value * sReturnUnitRatio
                        sLineItemCost = .Cells("LineItemCost").Value * sReturnRatio
                        sLineItemFreight = .Cells("LineItemFreight").Value * sReturnRatio
                        sMarkupCost = .Cells("MarkupCost").Value * sReturnUnitRatio
                        sHandlingCharge = .Cells("HandlingCharge").Value * sReturnUnitRatio

                        SQLExecute("EXEC InsertOrderItemCredit " & lOrderHeaderID & ", " & _
                                   .Cells("Item_Key").Value & ", " & _
                                   .Cells("Units_Per_Pallet").Value & ", " & _
                                   .Cells("ReturnUnit_Id").Value & ", " & _
                                   .Cells("Quantity").Value & ", " & _
                                   .Cells("Cost").Value & ", " & _
                                   .Cells("CostUnit").Value & ", " & _
                                   .Cells("Handling").Value & ", " & _
                                   .Cells("HandlingUnit").Value & ", " & _
                                   .Cells("Freight").Value & ", " & _
                                   .Cells("FreightUnit").Value & ", " & _
                                   0 & ", " & _
                                   .Cells("QuantityDiscount").Value & ", " & _
                                   .Cells("DiscountType").Value & ", " & _
                                   sLandedCost & ", " & _
                                   sLineItemCost & ", " & _
                                   sLineItemFreight & ", " & _
                                   0 & ", " & _
                                   .Cells("UnitCost").Value & ", " & _
                                   .Cells("UnitExtCost").Value & ", " & _
                                   .Cells("Package_Desc1").Value & ", " & _
                                   .Cells("Package_Desc2").Value & ", " & _
                                   .Cells("Package_Unit_ID").Value & ", " & _
                                   .Cells("MarkupPercent").Value & ", " & _
                                   sMarkupCost & ", " & _
                                   .Cells("Retail_Unit_ID").Value & ", " & _
                                   .Cells("CreditReason_ID").Value & "," & _
                                   giUserID & "," & _
                                   m_VendorDiscountAmt & "," & _
                                   sHandlingCharge, _
                                   DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        '", '" & !CreditReason & "', " &


                        ' Old Line (The following lines of code has been changed to fix the Bug 5532)

                        ' SQLOpenRS(rsOrderItem, "EXEC GetOrderItemInfoLast " & lOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        ' lOrderItemID = rsOrderItem.Fields("OrderItem_ID").Value
                        ' rsOrderItem.Close()

                        ' Accesing OrderItemId using Datafacatory(To Fix the bug 5532).
                        lOrderItemID = DistributionCreditOrderDAO.GetDistributionCreditOrderOrderItemID(lOrderHeaderID)


                        ' Receive only if this is a distribution Distribution Credit Order - not for Purchase orders
                        If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution Then

                            ' The following two lines of code has been modified to fix the COM Exception as part of the Bug Fix for 5532.
                            ' (Check the ErrorCode property of the exception to determine the HRESULT returned by the COM)
                            ' sLineItemReceivedCost = CostConversion(.Cells("Cost").Value, .Cells("CostUnit").Value, .Cells("QuantityUnit").Value, .Cells("Package_Desc1").Value, .Cells("Package_Desc2").Value, iPackage_Unit, .Cells("Total_Weight").Value, .Cells("Quantity").Value) * .Cells("Quantity").Value * (.Cells("MarkupPercent").Value + 100) / 100
                            ' sLineItemReceivedFreight = CostConversion(.Cells("Freight").Value, .Cells("FreightUnit").Value, .Cells("QuantityUnit").Value, .Cells("Package_Desc1").Value, .Cells("Package_Desc2").Value, iPackage_Unit, .Cells("Total_Weight").Value, .Cells("Quantity").Value) * .Cells("Quantity").Value * (.Cells("MarkupPercent").Value + 100) / 100

                            Dim dCost As Decimal
                            dCost = 0
                            dCost = .Cells("Cost").Value

                            Dim iCostUnit As Integer
                            iCostUnit = 0
                            iCostUnit = .Cells("CostUnit").Value

                            Dim iQuantityUnit As Integer
                            iQuantityUnit = 0
                            iQuantityUnit = .Cells("QuantityUnit").Value

                            Dim dPd1 As Decimal
                            dPd1 = 0
                            dPd1 = .Cells("Package_Desc1").Value

                            Dim dPd2 As Decimal
                            dPd2 = 0
                            dPd2 = .Cells("Package_Desc2").Value

                            Dim dTotWeight As Decimal
                            dTotWeight = 0
                            dTotWeight = .Cells("Total_Weight").Value

                            Dim dQty As Decimal
                            dQty = 0
                            dQty = .Cells("Quantity").Value

                            Dim dMp As Decimal
                            dMp = 0
                            dMp = .Cells("MarkupPercent").Value

                            Dim dFreight As Decimal
                            dFreight = 0
                            dFreight = .Cells("Freight").Value

                            Dim iFreightUnit As Integer
                            iFreightUnit = 0
                            iFreightUnit = .Cells("FreightUnit").Value

                            Dim dHandlingCharge As Decimal
                            dHandlingCharge = .Cells("HandlingCharge").Value
                            ' add handling charge to lineitemreceived cost.

                            If (dTotWeight <> 0) And (sTotalOrigWeight > dTotWeight) Then               ' Bug 1625: Checking for Partial Weight Cost Calculation
                                sLineItemReceivedCost = sUnitCost * dTotWeight
                            Else
                                sLineItemReceivedCost = (CostConversion(dCost, iCostUnit, iQuantityUnit, dPd1, dPd2, iPackage_Unit, dTotWeight, dQty)) * (dQty * (dMp + 100) / 100) + (dQty * sHandlingCharge)
                            End If

                            sLineItemReceivedFreight = (CostConversion(dFreight, iFreightUnit, iQuantityUnit, dPd1, dPd2, iPackage_Unit, dTotWeight, dQty)) * (dQty * (dMp + 100) / 100)

                            SQLExecute("EXEC UpdateOrderItemReceivingInfo " & lOrderItemID & ", " & .Cells("Quantity").Value & ", " & .Cells("Total_Weight").Value & ", " & sLineItemReceivedCost & ", " & sLineItemReceivedFreight & ", " & .Cells("CreditReason_ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            '", '" & !CreditReason & "', " &
                            SQLExecute("EXEC InsertReceivingItemHistory " & lOrderItemID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                        End If

                    End With

                End If

            Next ugRow
        Finally
            If rsDistributionCreditOrderChanges IsNot Nothing Then
                rsDistributionCreditOrderChanges.Close()
                rsDistributionCreditOrderChanges = Nothing
            End If
        End Try


        MsgBox("Distribution Credit Order #" & lOrderHeaderID & " has been created.", MsgBoxStyle.Information, "Notice!")

        Me.Close()

        logger.Info("cmdSubmit_Click -" & " Distribution Credit Order #" & lOrderHeaderID & " has been created.")
        logger.Debug("cmdSubmit_Click Exit")

    End Sub

    Private Sub frmDistributionCreditOrder_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        logger.Debug("frmDistributionCreditOrder_Activated Entry")
        If Me.Tag = "U" Then
            Me.Close()
        End If
        logger.Debug("frmDistributionCreditOrder_Activated Exit")
    End Sub

    Private Sub frmDistributionCreditOrder_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmDistributionCreditOrder_Load Entry")
        'Dim rsReport As ADODB.Recordset
        Dim rsOrderList As DAO.Recordset = Nothing

        CenterForm(Me)

        plOrig_Transfer_SubTeam = glSubTeamNo

        Try
            rsOrderList = SQLOpenRecordSet("EXEC GetCreditReasons ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsOrderList.EOF
                Me.ugrdReturn.DisplayLayout.ValueLists("CreditReasons").ValueListItems.Add(rsOrderList.Fields("CreditReason_ID").Value, rsOrderList.Fields("CreditReason").Value)
                rsOrderList.MoveNext()
            End While
        Finally
            If rsOrderList IsNot Nothing Then
                rsOrderList.Close()
                rsOrderList = Nothing
            End If
        End Try

        Call SetupDataTable()

        '-- Transfer the data to the ReturnOrder table
        '        SQLExecute("EXEC GetReturnOrderList " & glInstance & ", " & giUserID & ", " & glOrderHeaderID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        RefreshGrid()

        logger.Debug("frmDistributionCreditOrder_Load Exit")

    End Sub

    Public Sub RefreshGrid()

        logger.Debug("RefreshGrid Entry")
        'Dim rsReport As ADODB.Recordset
        Dim rsDistributionCreditOrderRecords As DAO.Recordset
        'Dim sSQL As String
        Dim row As DataRow
        Dim ugRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim vl As Infragistics.Win.ValueList

        '-- Clear the grid
        mdt.Rows.Clear()

        '-- Fill it up with what the user wants
        rsDistributionCreditOrderRecords = SQLOpenRecordSet("EXEC GetDistributionCreditOrderList " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        While Not rsDistributionCreditOrderRecords.EOF

            row = mdt.NewRow
            With rsDistributionCreditOrderRecords
                row("Item_Key") = .Fields("Item_Key").Value
                row("OrderItem_ID") = .Fields("OrderItem_ID").Value
                row("Identifier") = .Fields("Identifier").Value
                row("Item_Description") = .Fields("Item_Description").Value
                row("Units_Per_Pallet") = .Fields("Units_Per_Pallet").Value
                row("Quantity") = .Fields("Quantity").Value
                row("Quantity_Previous") = .Fields("Quantity_Previous").Value
                row("QuantityUnit_Text") = .Fields("QuantityUnit_Text").Value
                row("QuantityUnit") = .Fields("QuantityUnit").Value
                row("Package_Desc1") = .Fields("Package_Desc1").Value
                row("Package_Desc2") = .Fields("Package_Desc2").Value
                row("Package_Unit_ID") = .Fields("Package_Unit_ID").Value
                row("LandedCost") = .Fields("LandedCost").Value
                row("MarkupPercent") = .Fields("MarkupPercent").Value
                row("MarkupCost") = .Fields("MarkupCost").Value
                row("Cost") = .Fields("Cost").Value
                row("UnitCost") = .Fields("UnitCost").Value
                row("UnitExtCost") = .Fields("UnitExtCost").Value
                row("CostUnit") = .Fields("CostUnit").Value
                row("LineItemCost") = .Fields("LineItemCost").Value
                row("LineItemFreight") = .Fields("LineItemFreight").Value
                row("Freight") = .Fields("Freight").Value
                row("FreightUnit") = .Fields("FreightUnit").Value
                row("Handling") = .Fields("Handling").Value
                row("HandlingUnit") = .Fields("HandlingUnit").Value
                row("QuantityDiscount") = .Fields("QuantityDiscount").Value
                row("DiscountType") = .Fields("DiscountType").Value
                row("Total_Weight") = .Fields("Total_Weight").Value
                row("Total_Weight_Previous") = .Fields("Total_Weight_Previous").Value
                row("Retail_Unit_ID") = .Fields("Retail_Unit_ID").Value
                row("CreditReason") = .Fields("CreditReason").Value
                row("CreditReason_ID") = .Fields("CreditReason_ID").Value
                row("Total_ReceivedItemCost") = .Fields("Total_ReceivedItemCost").Value
                row("NetVendorItemDiscount") = .Fields("NetVendorItemDiscount").Value
                row("HandlingCharge") = .Fields("HandlingCharge").Value
                row("RetailUnit_Text") = .Fields("RetailUnit_Text").Value
                row("ReturnUnit_Id") = .Fields("ReturnUnit_Id").Value
                row("RetailWeight_Unit") = .Fields("RetailWeight_Unit").Value
                row("PackageSize") = .Fields("PackageSize").Value
            End With

            mdt.Rows.Add(row)

            rsDistributionCreditOrderRecords.MoveNext()
        End While

        mdt.AcceptChanges()

        Me.ugrdReturn.DataSource = mdt

        ' Update the Unit drop-down on a row-by-row basis. 
        For Each ugRow In Me.ugrdReturn.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            vl = New Infragistics.Win.ValueList
            vl.ValueListItems.Add(ugRow.Cells("QuantityUnit").Value, ugRow.Cells("QuantityUnit_Text").Value)
            If Not ugRow.Cells("QuantityUnit").Value = ugRow.Cells("Retail_Unit_ID").Value Then
                vl.ValueListItems.Add(ugRow.Cells("Retail_Unit_ID").Value, ugRow.Cells("RetailUnit_Text").Value)
            End If
            ugRow.Cells("ReturnUnit_Id").ValueList() = vl

        Next ugRow

        If Not (rsDistributionCreditOrderRecords Is Nothing) Then
            rsDistributionCreditOrderRecords.Close()
            rsDistributionCreditOrderRecords = Nothing
        End If

        logger.Debug("RefreshGrid Exit")

    End Sub

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("DistributionCreditOrder")
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("OrderItem_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("Units_Per_Pallet", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Quantity", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Quantity_Previous", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("QuantityUnit_Text", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Package_Desc1", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Package_Desc2", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Package_Unit_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("LandedCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("MarkupPercent", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("MarkupCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("UnitCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("UnitExtCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("CostUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("LineItemCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("LineItemFreight", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Freight", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("FreightUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Handling", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("HandlingUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("QuantityDiscount", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("DiscountType", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Total_Weight", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Total_Weight_Previous", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Retail_Unit_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("CreditReason", GetType(String)))
        mdt.Columns.Add(New DataColumn("CreditReason_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Total_ReceivedItemCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("NetVendorItemDiscount", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("HandlingCharge", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("RetailUnit_Text", GetType(String)))
        mdt.Columns.Add(New DataColumn("ReturnUnit_Id", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("RetailWeight_Unit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("PackageSize", GetType(Decimal)))


        logger.Debug("SetupDataTable Exit")

    End Sub

    Private Sub ugrdReturn_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdReturn.AfterCellActivate

        logger.Debug("ugrdReturn_AfterCellActivate Entry")
        If ugrdReturn.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

        logger.Debug("ugrdReturn_AfterCellActivate Exit")
    End Sub
    Private Sub ugrdReturn_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdReturn.AfterCellUpdate

        logger.Debug("ugrdReturn_AfterCellUpdate Entry")
        If e.Cell.Column.Key = "ReturnUnit_Id" Then

            '  If the user is changing the Return Unit, zero out the values and fix the quantity and costs displayed
            '  We're only adjusting the values displayed on-screen.  The rest are adjusted later.  

            e.Cell.Row.Cells("Quantity").Value = 0
            e.Cell.Row.Cells("Total_Weight").Value = 0

            If e.Cell.Value = e.Cell.Row.Cells("QuantityUnit").Value Then
                'Revert to Dist Unit
                If GetWeight_Unit(CInt(e.Cell.Row.Cells("Retail_Unit_ID").Value)) Then
                    e.Cell.Row.Cells("Total_Weight_Previous").Value = e.Cell.Row.Cells("Quantity_Previous").Value
                    'e.Cell.Row.Cells("Total_Weight_Previous").Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
                End If
                e.Cell.Row.Cells("Quantity_Previous").Value = e.Cell.Row.Cells("Quantity_Previous").Value / e.Cell.Row.Cells("PackageSize").Value
                e.Cell.Row.Cells("Cost").Value = e.Cell.Row.Cells("Cost").Value * e.Cell.Row.Cells("PackageSize").Value
            Else
                'Switch to Retail Unit
                e.Cell.Row.Cells("Quantity_Previous").Value = e.Cell.Row.Cells("Quantity_Previous").Value * e.Cell.Row.Cells("PackageSize").Value
                e.Cell.Row.Cells("Cost").Value = e.Cell.Row.Cells("Cost").Value / e.Cell.Row.Cells("PackageSize").Value
                e.Cell.Row.Cells("Freight").Value = e.Cell.Row.Cells("Freight").Value / e.Cell.Row.Cells("PackageSize").Value
                'e.Cell.Row.Cells("Total_Weight_Previous").Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            End If

        End If
        logger.Debug("ugrdReturn_AfterCellUpdate Exit")
    End Sub

    Private Sub ugrdReturn_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdReturn.AfterRowUpdate

        logger.Debug("ugrdReturn_AfterRowUpdate Entry")

        Dim CreditReasonID As Integer

        If Not IsDBNull(e.Row.Cells("CreditReason_ID").Value) Then
            CreditReasonID = e.Row.Cells("CreditReason_ID").Value
        Else
            CreditReasonID = -1
        End If

        logger.Debug("ugrdReturn_AfterRowUpdate Exit")

    End Sub

    Private Sub ugrdReturn_BeforeCellActivate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles ugrdReturn.BeforeCellActivate

        logger.Debug("ugrdReturn_BeforeCellActivate Entry")
        If e.Cell.Column.Key = "Quantity" Then
            If GetWeight_Unit(CInt(e.Cell.Row.Cells("ReturnUnit_Id").Value)) Then
                e.Cell.Column.MaskInput = "{double:5.2}"
            Else
                e.Cell.Column.MaskInput = "nnn"
            End If
        End If
        logger.Debug("ugrdReturn_BeforeCellActivate Exit")

    End Sub

    'Private Sub ugrdReturn_BeforeRowActivate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdReturn.BeforeRowActivate

    '    logger.Debug("ugrdReturn_BeforeRowActivate Entry")
    '    Dim vl As New Infragistics.Win.ValueList

    '    vl.ValueListItems.Add(e.Row.Cells("QuantityUnit").Value, e.Row.Cells("Unit_Name").Value)

    '    If Not e.Row.Cells("QuantityUnit").Value = e.Row.Cells("Retail_Unit_ID").Value Then
    '        vl.ValueListItems.Add(e.Row.Cells("Retail_Unit_ID").Value, e.Row.Cells("RetailUnit_Text").Value)
    '    End If

    '    e.Row.Cells("ReturnUnit_Id").ValueList() = vl

    '    logger.Debug("ugrdReturn_BeforeRowActivate Exit")

    'End Sub

    Private Sub ugrdReturn_BeforeRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles ugrdReturn.BeforeRowUpdate

        logger.Debug("ugrdReturn_BeforeRowUpdate Entry")
        If e.Row.Cells("Quantity").Value > e.Row.Cells("Quantity_Previous").Value Then
            MsgBox("More has been returned than received", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        If e.Row.Cells("Total_Weight").Value > e.Row.Cells("Total_Weight_Previous").Value Then
            MsgBox("More weight has been returned than received", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        If e.Row.Cells("Total_Weight").Value > 0 And e.Row.Cells("Quantity").Value = 0 Then
            MsgBox("Weight cannot be returned without quantity.", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        logger.Debug("ugrdReturn_BeforeRowUpdate Exit")

    End Sub

    Private Sub ugrdReturn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ugrdReturn.KeyDown

        logger.Debug("ugrdReturn_KeyDown Entry")
        'Change the navigational behavior
        Select Case e.KeyValue
            Case Keys.Up
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select
        logger.Debug("ugrdReturn_KeyDown Exit")
    End Sub

End Class
