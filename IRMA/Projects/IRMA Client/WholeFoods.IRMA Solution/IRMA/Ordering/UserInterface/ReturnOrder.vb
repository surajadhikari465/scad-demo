Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.Ordering.DataAccess
Imports log4net
Friend Class frmReturnOrder
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
        rsChanges = SQLOpenRecordSet("EXEC CheckForReturnOrderChanges " & glInstance & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)


        ' New code to fix the Bug 5534.
        If (rsChanges.BOF And rsChanges.EOF) Then
            bChanges = False  ' No return order records entered into Grid.
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
        Dim rsReturnOrderChanges As DAO.Recordset = Nothing
        Dim lOrderHeaderID As Integer
		Dim lOrderItemID As Integer
		Dim sLineItemCost As Decimal
		Dim sLineItemFreight As Decimal
		Dim sLineItemReceivedCost As Decimal
		Dim sLineItemReceivedFreight As Decimal
		Dim sUnitCost As Decimal
        Dim sUnitExtCost As Decimal
        Dim iPackage_Unit As Integer
        Dim sdt() As DataRow

        sdt = mdt.Select("QuantityReturned > 0")
        If sdt.Length = 0 Then
            sdt = Nothing
            MsgBox("There are no items to place on the return order.", MsgBoxStyle.Critical, Me.Text)
            logger.Info("cmdSubmit_Click - There are no items to place on the return order.")
            logger.Debug("cmdSubmit_Click Exit")
            Exit Sub
        End If
        sdt = Nothing

        sdt = mdt.Select("QuantityReturned > 0 AND CreditReason_ID IS NULL")
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
            '-- Check to see if anything was really modified
            rsReturnOrderChanges = SQLOpenRecordSet("EXEC CheckForReturnOrderChanges " & glInstance & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            ' rsReport.Open("SELECT * FROM ReturnOrder WHERE Instance = " & glInstance & " AND (Quantity > 0 OR Total_Weight > 0)", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            While Not rsReturnOrderChanges.EOF

                If (rsReturnOrderChanges.Fields("Quantity").Value > 0) Then

                    With rsReturnOrderChanges
                        If IsDBNull(.Fields("Package_Unit_ID").Value) Then
                            iPackage_Unit = 0
                        Else
                            iPackage_Unit = .Fields("Package_Unit_ID").Value
                        End If

                        m_VendorDiscountAmt = 0

                        ' The following line has been changed to fis the Bug 5534.
                        m_VendorDiscountAmt = .Fields("NetVendorItemDiscount").Value
                        ' m_VendorDiscountAmt = .Fields("VendorNetDiscount").Value


                        sLineItemCost = CostConversion(.Fields("Cost").Value, .Fields("CostUnit").Value, .Fields("QuantityUnit").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * .Fields("Quantity").Value * (.Fields("MarkupPercent").Value + 100) / 100
                        sLineItemFreight = CostConversion(.Fields("Freight").Value, .Fields("FreightUnit").Value, .Fields("QuantityUnit").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * .Fields("Quantity").Value * (.Fields("MarkupPercent").Value + 100) / 100

                        '-- Markup
                        sUnitCost = CostConversion(sLineItemCost / .Fields("Quantity").Value, .Fields("QuantityUnit").Value, .Fields("Retail_Unit_ID").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0)
                        sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / .Fields("Quantity").Value, .Fields("QuantityUnit").Value, .Fields("Retail_Unit_ID").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0)

                        SQLExecute("EXEC InsertOrderItemCredit " & lOrderHeaderID & ", " & _
                                   .Fields("Item_Key").Value & ", " & _
                                   .Fields("Units_Per_Pallet").Value & ", " & _
                                   .Fields("QuantityUnit").Value & ", " & _
                                   .Fields("Quantity").Value & ", " & _
                                   .Fields("Cost").Value & ", " & _
                                   .Fields("CostUnit").Value & ", 0, 1, " & _
                                   .Fields("Freight").Value & ", " & _
                                   .Fields("FreightUnit").Value & ", 0, " & _
                                   .Fields("QuantityDiscount").Value & ", " & _
                                   .Fields("DiscountType").Value & ", " & _
                                   .Fields("LandedCost").Value & ", " & _
                                   sLineItemCost & ", " & _
                                   sLineItemFreight & ", " & _
                                   0 & ", " & _
                                   sUnitCost & ", " & _
                                   sUnitExtCost & ", " & _
                                   .Fields("Package_Desc1").Value & ", " & _
                                   .Fields("Package_Desc2").Value & ", " & _
                                   .Fields("Package_Unit_ID").Value & ", " & _
                                   .Fields("MarkupPercent").Value & ", " & _
                                   .Fields("MarkupCost").Value & ", " & _
                                   .Fields("Retail_Unit_ID").Value & ", " & _
                                   .Fields("CreditReason_ID").Value & "," & _
                                   giUserID & "," & _
                                   m_VendorDiscountAmt & "," & _
                                   .Fields("handlingCharge").Value, _
                                   DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        '", '" & !CreditReason & "', " &


                        ' Old Line (The following lines of code has been changed to fix the Bug 5532)

                        ' SQLOpenRS(rsOrderItem, "EXEC GetOrderItemInfoLast " & lOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        ' lOrderItemID = rsOrderItem.Fields("OrderItem_ID").Value
                        ' rsOrderItem.Close()

                        ' Accesing OrderItemId using Datafacatory(To Fix the bug 5532).
                        lOrderItemID = ReturnOrderDAO.GetReturnOrderOrderItemID(lOrderHeaderID)


                        ' Receive only if this is a distribution return order - not for Purchase orders
                        If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution Then


                            ' The following two lines of code has been modified to fix the COM Exception as part of the Bug Fix for 5532.
                            ' (Check the ErrorCode property of the exception to determine the HRESULT returned by the COM)
                            ' sLineItemReceivedCost = CostConversion(.Fields("Cost").Value, .Fields("CostUnit").Value, .Fields("QuantityUnit").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, .Fields("Total_Weight").Value, .Fields("Quantity").Value) * .Fields("Quantity").Value * (.Fields("MarkupPercent").Value + 100) / 100
                            ' sLineItemReceivedFreight = CostConversion(.Fields("Freight").Value, .Fields("FreightUnit").Value, .Fields("QuantityUnit").Value, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, .Fields("Total_Weight").Value, .Fields("Quantity").Value) * .Fields("Quantity").Value * (.Fields("MarkupPercent").Value + 100) / 100

                            Dim dCost As Decimal
                            dCost = 0
                            dCost = .Fields("Cost").Value

                            Dim iCostUnit As Integer
                            iCostUnit = 0
                            iCostUnit = .Fields("CostUnit").Value

                            Dim iQuantityUnit As Integer
                            iQuantityUnit = 0
                            iQuantityUnit = .Fields("QuantityUnit").Value

                            Dim dPd1 As Decimal
                            dPd1 = 0
                            dPd1 = .Fields("Package_Desc1").Value

                            Dim dPd2 As Decimal
                            dPd2 = 0
                            dPd2 = .Fields("Package_Desc2").Value

                            Dim dTotWeight As Decimal
                            dTotWeight = 0
                            dTotWeight = .Fields("Total_Weight").Value

                            Dim dQty As Decimal
                            dQty = 0
                            dQty = .Fields("Quantity").Value

                            Dim dMp As Decimal
                            dMp = 0
                            dMp = .Fields("MarkupPercent").Value

                            Dim dFreight As Decimal
                            dFreight = 0
                            dFreight = .Fields("Freight").Value

                            Dim iFreightUnit As Integer
                            iFreightUnit = 0
                            iFreightUnit = .Fields("FreightUnit").Value

                            Dim dHandlingCharge As Decimal
                            dHandlingCharge = .Fields("HandlingCharge").Value
                            ' add handling charge to lineitemreceived cost.


                            sLineItemReceivedCost = (CostConversion(dCost, iCostUnit, iQuantityUnit, dPd1, dPd2, iPackage_Unit, dTotWeight, dQty)) * (dQty * (dMp + 100) / 100)
                            sLineItemReceivedFreight = (CostConversion(dFreight, iFreightUnit, iQuantityUnit, dPd1, dPd2, iPackage_Unit, dTotWeight, dQty)) * (dQty * (dMp + 100) / 100)

                            ' add handling charge to lineitemreceived cost.
                            sLineItemReceivedCost = sLineItemReceivedCost + (dHandlingCharge * dQty)

                            SQLExecute("EXEC UpdateOrderItemReceivingInfo " & lOrderItemID & ", " & .Fields("Quantity").Value & ", " & .Fields("Total_Weight").Value & ", " & sLineItemReceivedCost & ", " & sLineItemReceivedFreight & ", " & .Fields("CreditReason_ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            '", '" & !CreditReason & "', " &
                            SQLExecute("EXEC InsertReceivingItemHistory " & lOrderItemID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)


                        End If

                    End With

                End If

                rsReturnOrderChanges.MoveNext()
            End While
        Finally
            If rsReturnOrderChanges IsNot Nothing Then
                rsReturnOrderChanges.Close()
                rsReturnOrderChanges = Nothing
            End If
        End Try
        ' Commenting the MS Access dependent code.
        'gDBReport.CommitTrans()
        'gJetFlush.RefreshCache(gDBReport)

        MsgBox("Return Order #" & lOrderHeaderID & " has been created.", MsgBoxStyle.Information, "Notice!")

        Me.Close()

        logger.Info("cmdSubmit_Click -" & " Return Order #" & lOrderHeaderID & " has been created.")
        logger.Debug("cmdSubmit_Click Exit")

    End Sub
	
    Private Sub frmReturnOrder_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        logger.Debug("frmReturnOrder_Activated Entry")
        If Me.Tag = "U" Then
            Me.Close()
        End If
        logger.Debug("frmReturnOrder_Activated Exit")
    End Sub
	
	Private Sub frmReturnOrder_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmReturnOrder_Load Entry")
        'Dim rsReport As ADODB.Recordset
        Dim rsOrderList As dao.Recordset = Nothing

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
        SQLExecute("EXEC GetReturnOrderList " & glInstance & ", " & giUserID & ", " & glOrderHeaderID, DAO.RecordsetOptionEnum.dbSQLPassThrough)


'        rsReport = New ADODB.Recordset
'        gDBReport.BeginTrans()
'        gDBReport.Execute("DELETE FROM ReturnOrder WHERE Instance = " & glInstance)

'        rsReport.Open("ReturnOrder", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)
'        SQLOpenRS(rsOrderList, "EXEC GetReturnOrderList " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
'        While Not rsOrderList.EOF
'            rsReport.AddNew()
'            rsReport.Fields("Instance").Value = glInstance
'            rsReport.Fields("Item_Key").Value = rsOrderList.Fields("Item_Key").Value
'            rsReport.Fields("OrderItem_ID").Value = rsOrderList.Fields("OrderItem_ID").Value
''User_ID
'            rsReport.Fields("Identifier").Value = rsOrderList.Fields("Identifier").Value
'            rsReport.Fields("Item_Description").Value = rsOrderList.Fields("Item_Description").Value
'            rsReport.Fields("Units_Per_Pallet").Value = rsOrderList.Fields("Units_Per_Pallet").Value
'            rsReport.Fields("Quantity").Value = 0
'            rsReport.Fields("Quantity_Previous").Value = rsOrderList.Fields("QuantityReceived").Value
'            rsReport.Fields("QuantityUnit_Text").Value = rsOrderList.Fields("Unit_Name").Value
'            rsReport.Fields("QuantityUnit").Value = rsOrderList.Fields("QuantityUnit").Value
'            rsReport.Fields("Package_Desc1").Value = rsOrderList.Fields("Package_Desc1").Value
'            rsReport.Fields("Package_Desc2").Value = rsOrderList.Fields("Package_Desc2").Value
'            rsReport.Fields("Package_Unit_ID").Value = rsOrderList.Fields("Package_Unit_ID").Value
'            rsReport.Fields("LandedCost").Value = rsOrderList.Fields("LandedCost").Value
'            rsReport.Fields("MarkupPercent").Value = rsOrderList.Fields("MarkupPercent").Value
'            rsReport.Fields("MarkupCost").Value = rsOrderList.Fields("MarkupCost").Value
'            rsReport.Fields("Cost").Value = rsOrderList.Fields("Cost").Value
'            rsReport.Fields("CostUnit").Value = rsOrderList.Fields("CostUnit").Value
'            rsReport.Fields("Freight").Value = rsOrderList.Fields("Freight").Value
'            rsReport.Fields("FreightUnit").Value = rsOrderList.Fields("FreightUnit").Value
'            rsReport.Fields("QuantityDiscount").Value = rsOrderList.Fields("QuantityDiscount").Value
'            rsReport.Fields("DiscountType").Value = rsOrderList.Fields("DiscountType").Value
'            rsReport.Fields("Total_Weight").Value = 0
'            rsReport.Fields("Total_Weight_Previous").Value = rsOrderList.Fields("Total_Weight").Value
'            rsReport.Fields("Retail_Unit_ID").Value = rsOrderList.Fields("Retail_Unit_ID").Value
'            rsReport.Fields("Total_ReceivedItemCost").Value = rsOrderList.Fields("Total_ReceivedItemCost").Value
''NetVendorItemDiscount
'            rsReport.Update()

'            rsOrderList.MoveNext()
'        End While
'        rsReport.Close()
'        rsReport = Nothing
'        gDBReport.CommitTrans()
'        gJetFlush.RefreshCache(gDBReport)
'        rsOrderList.Close()

        RefreshGrid()
        Global.SetUltraGridSelectionStyle(ugrdReturn)
        logger.Debug("frmReturnOrder_Load Exit")

    End Sub
	
    Public Sub RefreshGrid()

        logger.Debug("RefreshGrid Entry")
        'Dim rsReport As ADODB.Recordset
        Dim rsReturnOrderRecords As DAO.Recordset
        'Dim sSQL As String
        Dim row As DataRow

        '-- Clear the grid
        mdt.Rows.Clear()

        '-- Fill it up with what the user wants
        rsReturnOrderRecords = SQLOpenRecordSet("EXEC GetReturnOrderRecords " & glInstance & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)


        'sSQL = "SELECT * FROM ReturnOrder WHERE Instance = " & glInstance & " ORDER BY "
        'If sSort = "" Then
        '	sSQL = sSQL & "OrderItem_ID"
        'Else
        '	sSQL = sSQL & sSort
        'End If
        'rsReport = New ADODB.Recordset
        'rsReport.Open(sSQL, gDBReport, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

'        While Not rsReport.EOF
        While Not rsReturnOrderRecords.EOF

            row = mdt.NewRow
            With rsReturnOrderRecords
                row("OrderItem_ID") = .Fields("OrderItem_ID").Value
                row("Identifier") = .Fields("Identifier").Value
                row("Item_Description") = .Fields("Item_Description").Value
                row("QuantityReceived") = .Fields("Quantity_Previous").Value
                row("QuantityUnit") = .Fields("QuantityUnit").Value
                row("Unit_Name") = .Fields("QuantityUnit_Text").Value
                row("QuantityReturned") = .Fields("Quantity").Value
                row("Total_Weight") = .Fields("Total_Weight_Previous").Value
                row("WeightReturned") = .Fields("Total_Weight").Value
                row("Cost") = .Fields("Total_ReceivedItemCost").Value
                row("CreditReason_ID") = .Fields("CreditReason").Value
            End With

            mdt.Rows.Add(row)

            rsReturnOrderRecords.MoveNext()
        End While

        mdt.AcceptChanges()

        Me.ugrdReturn.DataSource = mdt

        If Not (rsReturnOrderRecords Is Nothing) Then
            rsReturnOrderRecords.Close()
            rsReturnOrderRecords = Nothing
        End If

        logger.Debug("RefreshGrid Exit")

    End Sub

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("ReturnOrder")
        mdt.Columns.Add(New DataColumn("OrderItem_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Unit_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityReceived", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("QuantityReturned", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Total_Weight", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("WeightReturned", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("CreditReason_ID", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")

    End Sub

    Private Sub ugrdReturn_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdReturn.AfterCellActivate

        logger.Debug("ugrdReturn_AfterCellActivate Entry")
        If ugrdReturn.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            ugrdReturn.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

        logger.Debug("ugrdReturn_AfterCellActivate Exit")
    End Sub

    Private Sub ugrdReturn_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdReturn.AfterRowUpdate

        logger.Debug("ugrdReturn_AfterRowUpdate Entry")

        Dim CreditReasonID As Integer

        If Not IsDBNull(e.Row.Cells("CreditReason_ID").Value) Then
            CreditReasonID = e.Row.Cells("CreditReason_ID").Value
        Else
            CreditReasonID = -1
        End If


        ' The following line of code has been changed to fix the bug 5534.
        SQLExecute("EXEC UpdateReturnOrderRecord " & glInstance & ", " & _
                                                giUserID & ", " & _
                                                e.Row.Cells("OrderItem_ID").Value & ", " & _
                                                e.Row.Cells("QuantityReturned").Value & ", " & _
                                                e.Row.Cells("WeightReturned").Value & ", " & _
                                                CreditReasonID, _
                                                DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ' Before 5534 BugFix.
        'SQLExecute("EXEC GetReturnOrderList " & glInstance & ", " & _
        '                                                giUserID & ", " & _
        '                                                e.Row.Cells("OrderItem_ID").Value & ", " & _
        '                                                e.Row.Cells("QuantityReturned").Value & ", " & _
        '                                                e.Row.Cells("WeightReturned").Value & ", " & _
        '                                                CreditReasonID, _
        '                                                DAO.RecordsetOptionEnum.dbSQLPassThrough)

        logger.Debug("ugrdReturn_AfterRowUpdate Exit")

        '    gDBReport.Execute("UPDATE ReturnOrder " & "SET Quantity = " & e.Row.Cells("QuantityReturned").Value & ", " & "Total_Weight = " & e.Row.Cells("WeightReturned").Value & ", " & "CreditReason_ID = " & e.Row.Cells("CreditReason_ID").Value & " " & "WHERE Instance = " & glInstance & " AND OrderItem_ID = " & e.Row.Cells("OrderItem_ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        'Else
        '    gDBReport.Execute("UPDATE ReturnOrder " & "SET Quantity = " & e.Row.Cells("QuantityReturned").Value & ", " & "Total_Weight = " & e.Row.Cells("WeightReturned").Value & " WHERE Instance = " & glInstance & " AND OrderItem_ID = " & e.Row.Cells("OrderItem_ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        'End If

    End Sub

    Private Sub ugrdReturn_BeforeCellActivate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles ugrdReturn.BeforeCellActivate

        logger.Debug("ugrdReturn_BeforeCellActivate Entry")
        If e.Cell.Column.Key = "QuantityReturned" Then
            If GetWeight_Unit(CInt(e.Cell.Row.Cells("QuantityUnit").Value)) Then
                e.Cell.Column.MaskInput = "{double:5.2}"
            Else
                e.Cell.Column.MaskInput = "nnn"
            End If
        End If
        logger.Debug("ugrdReturn_BeforeCellActivate Exit")

    End Sub

    Private Sub ugrdReturn_BeforeRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles ugrdReturn.BeforeRowUpdate

        logger.Debug("ugrdReturn_BeforeRowUpdate Entry")
        If e.Row.Cells("QuantityReturned").Value > e.Row.Cells("QuantityReceived").Value Then
            MsgBox("More has been returned than received", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        If e.Row.Cells("WeightReturned").Value > e.Row.Cells("Total_Weight").Value Then
            MsgBox("More weight has been returned than received", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        If e.Row.Cells("WeightReturned").Value > 0 And e.Row.Cells("QuantityReturned").Value = 0 Then
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
