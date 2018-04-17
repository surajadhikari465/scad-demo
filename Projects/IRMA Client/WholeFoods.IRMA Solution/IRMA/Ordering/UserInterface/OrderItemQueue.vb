Option Strict Off
Option Explicit On

Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports System.Text.RegularExpressions
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmOrderItemQueue
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private SkipAutoSelect As Boolean
    Private mdt As DataTable
    Private m_rsOrderItems As DAO.Recordset
    Private m_bIsVendorDistributorManufacturer As Boolean
    Private m_bIsVendorStoreSame As Boolean
    Private m_abSubTeam_From_UnRestricted() As Boolean
    Private m_abSubTeam_To_UnRestricted() As Boolean
    Private m_lOrderHeader_ID As Integer
    Private m_bIsFax As Boolean
    Private m_bIsEmail As Boolean
    Private m_bIselectronic As Boolean
    Private m_bIsSend As Boolean
    Private m_bOverrideTransmission As Boolean
    Private m_sOverrideTarget As String
    Private m_VendorDiscountAmt As Decimal
    Private bSeafoodMissingCountryInfo As Boolean
    Private Const iALL As Short = 0
    Private Const iNONE As Short = 1
    Private Const packagingSupplies = "Packaging Supplies"
    Private Const product = "Product"
    Private Const otherSupplies = "Other Supplies"

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub frmOrderItemQueue_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrderItemQueue_FormClosing Entry")

        If e.CloseReason = CloseReason.None Then
            e.Cancel = True
        ElseIf e.CloseReason = CloseReason.UserClosing Then
            grdList.UpdateData()
            If Not ClearGrid() Then e.Cancel = True
        End If

        logger.Debug("frmOrderItemQueue_FormClosing Exit")
    End Sub

    Private Sub frmOrderItemQueue_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderItemQueue_Load Entry")

        Call CenterForm(Me)
        Call InitializeGrid()
        Call PopulateProductType(cmbProductType)
        Call DefaultControls()

        logger.Debug("frmOrderItemQueue_Load Exit")
    End Sub

    Private Sub InitializeGrid()
        logger.Debug("InitializeGrid Entry")

        Dim rsReasons As DAO.Recordset = Nothing

        mdt = New DataTable("OrderItemQueue")
        Dim Keys(1) As DataColumn
        Dim dc As DataColumn

        mdt.PrimaryKey = Keys

        dc = New DataColumn("OrderItemQueue_ID", GetType(Integer))
        mdt.Columns.Add(dc)
        Keys(0) = dc

        dc = New DataColumn("Item_Key", GetType(Integer))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Selected", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Identifier", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Item_Description", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("SubTeam_No", GetType(Integer))
        mdt.Columns.Add(dc)
        dc = New DataColumn("SubTeamName", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("NotAvailable", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Quantity", GetType(Decimal))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Unit_ID", GetType(Integer))
        mdt.Columns.Add(dc)
        dc = New DataColumn("QuantityUnitName", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Cost", GetType(Decimal))
        mdt.Columns.Add(dc)
        dc = New DataColumn("PrimaryVendor", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("UserName", GetType(String))
        mdt.Columns.Add(dc)
        dc = New DataColumn("CreditReason_ID", GetType(Integer))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Insert_Date", GetType(Date))
        mdt.Columns.Add(dc)
        dc = New DataColumn("Discontinued", GetType(Boolean))
        mdt.Columns.Add(dc)

        Try
            ' Load these in advance
            rsReasons = SQLOpenRecordSet("EXEC GetCreditReasons ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsReasons.EOF
                Me.grdList.DisplayLayout.ValueLists(0).ValueListItems.Add(rsReasons.Fields("CreditReason_ID").Value, rsReasons.Fields("CreditReason").Value)
                rsReasons.MoveNext()
            End While
        Finally
            If rsReasons IsNot Nothing Then
                rsReasons.Close()
                rsReasons = Nothing
            End If
        End Try

        logger.Debug("InitializeGrid exit")
    End Sub

    Private Sub chkCredit_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkCredit.CheckStateChanged
        logger.Debug("chkCredit_CheckStateChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

        If (chkCredit.CheckState) Then
            ' Show the credit reason combo column in the grid
            grdList.DisplayLayout.Bands(0).Columns("CreditReason_ID").Hidden = False
            btnApplyAllCreditReason.Visible = True
        Else
            ' Hide the credit reason combo column in the grid
            grdList.DisplayLayout.Bands(0).Columns("CreditReason_ID").Hidden = True
            btnApplyAllCreditReason.Visible = False
        End If

        AdjustGridColWidths()

        If chkCredit.Checked Then
            IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
        Else
            IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
        End If

        logger.Debug("chkCredit_CheckStateChanged Exit")
    End Sub

    Private Sub cmbPurchasing_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbPurchasing.KeyPress
        logger.Debug("cmbPurchasing_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = ASCII_BACKSPACE Then
            cmbPurchasing.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbPurchasing_KeyPress Exit")
    End Sub

    Private Sub cmbTransferToSubteam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbTransferToSubteam.KeyPress
        logger.Debug("cmbTransferToSubteam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = ASCII_BACKSPACE Then
            cmbTransferToSubteam.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbTransferToSubteam_KeyPress Exit")
    End Sub

    Private Sub cmdCreateOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreateOrder.Click
        logger.Debug("cmdCreateOrder_Click Entry")

        Dim sdr() As DataRow
        Dim Purchasing_Store_Vendor_ID As Integer = ComboValue(Me.cmbPurchasing)
        Dim IsDSDVendorForStore As Boolean = OrderingDAO.CheckDSDVendorWithPurchasingStore(glVendorID, Purchasing_Store_Vendor_ID)

        If IsDSDVendorForStore Then
            logger.Info("CmdAdd_Click - Vendor is DSD Vendor for Purchasing Store.  Can't create order.")
            MsgBox("This item(s) is from a Guaranteed Sale Supplier.  Please use IRMA Mobile to create a Receiving Document for this supplier.", MsgBoxStyle.Information, "Invalid Vendor")
            logger.Info("CmdAdd_Click Exit")
            Exit Sub
        End If

        If grdList.Rows.Count = 0 Then
            MsgBox("Please search for item(s) to include on the order.", MsgBoxStyle.Information, Me.Text)
            cmdSearch.Focus()
            Exit Sub
        End If

        sdr = mdt.Select("Selected = '*'")
        If sdr.Length = 0 Then
            sdr = Nothing
            logger.Info("cmdCreateOrder_Click - Please select item(s).")
            MsgBox("Please select item(s) to add to the order.", MsgBoxStyle.Information, Me.Text)
            cmdSelect.Focus()
            Exit Sub
        End If
        sdr = Nothing

        If Not dtpStartDate.Checked Then
            logger.Info("cmdCreateOrder_Click - Expected Date is required.")
            MsgBox("Please enter an Expected Date.", MsgBoxStyle.Information, Me.Text)
            dtpStartDate.Checked = True
            dtpStartDate.Focus()
            Exit Sub
        End If

        If chkCredit.CheckState = 1 Then
            ' Check the items that are selected
            sdr = mdt.Select("Selected = '*' AND Quantity > 0 AND CreditReason_ID IS NULL")
            If sdr.Length > 0 Then
                sdr = Nothing
                logger.Info("cmdCreateOrder_Click - You must enter a credit reason for items with a quantity greater than zero")
                MsgBox("Please enter a credit reason for items with a quantity greater than zero.", MsgBoxStyle.Information, "Credit Reason")
                grdList.Focus()
                Exit Sub
            End If
            sdr = Nothing
        End If

        ' Save all rows in the Queue table first
        Call SaveQueueItems()

        ' Sets up a transaction here
        SQLExecute("BEGIN TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Call SaveOrderHeader()

        If m_lOrderHeader_ID > 0 Then

            Call SaveOrderItems()
            Call DeleteQueueItems()

            ' Have to commit the transaction before sending, or it can't find the orderitems
            ' to check the seafood country info
            SQLExecute("COMMIT TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            logger.Info("cmdCreateOrder_Click " & " A new order has been created with PO Number: " & CStr(m_lOrderHeader_ID) & ".")

            If MessageBox.Show("A new order has been created with PO Number: " & CStr(m_lOrderHeader_ID) & "." & vbNewLine & vbNewLine &
                                "Would you like to send the order now?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                ' set the global order header if
                glOrderHeaderID = m_lOrderHeader_ID

                ' create and open the Orders form and instruct it to send the order now
                Dim orders As New frmOrders
                bSpecificOrder = True
                frmOrders.OpenAndSend = True
                frmOrders.ShowDialog()
                frmOrders.Dispose()
                bSpecificOrder = False

            End If

            Call RefreshQueueCount()

        Else
            SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            logger.Info("cmdCreateOrder_Click " & " An error has occurred while creating this order." & CStr(m_lOrderHeader_ID) & ".")
            MsgBox("An error has occurred while creating this order.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        cmdReset_Click(cmdReset, New System.EventArgs())

        logger.Debug("cmdCreateOrder_Click Exit")
    End Sub

    Private Sub RefreshQueueCount()
        logger.Debug("RefreshQueueCount Entry")

        Dim rsQueueCount As DAO.Recordset = Nothing
        Dim sItem As String

        Try
            ' Get the count of the remaining items for this Store and ToSubteam
            rsQueueCount = SQLOpenRecordSet("EXEC GetOrderItemQueueCount " & VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex) & ", " & VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not (rsQueueCount.EOF And rsQueueCount.BOF) Then
                rsQueueCount.MoveFirst()
                sItem = IIf(rsQueueCount.Fields("ItemCount").Value = 1, " item", " items")
                lblDisplayCount.Text = cmbPurchasing.Text & "/" & cmbTransferToSubteam.Text & " has " & CStr(rsQueueCount.Fields("ItemCount").Value) & sItem & " in the queue."
            End If
        Finally
            If rsQueueCount IsNot Nothing Then
                rsQueueCount.Close()
                rsQueueCount = Nothing
            End If
        End Try

        logger.Debug("RefreshQueueCount Exit")
    End Sub

    Private Sub SaveOrderHeader()
        logger.Debug("SaveOrderHeader Entry")

        'Set up list of handled errors - all user-defined validations done on the server
        Dim lIgnoreErrNum(1) As Integer
        lIgnoreErrNum(0) = 50002
        lIgnoreErrNum(1) = 50003

        Dim sSQLText As String
        Dim iFax_Order As Short
        Dim sTransferFromSubTeam As String
        Dim rsOrderID As DAO.Recordset

        If geOrderType <> enumOrderType.Purchase Then
            iFax_Order = 0
            sTransferFromSubTeam = ComboValue(cmbTransferFromSubteam)
        Else
            'External Purchase Order
            iFax_Order = IIf(m_bIsFax, 1, 0)
            sTransferFromSubTeam = "NULL"
        End If

        On Error Resume Next

        '-- Add the new record
        sSQLText = "EXEC InsertOrder " & glVendorID & ", " & CShort(geOrderType) & ", " & CShort(geProductType) & ", " & ComboValue(cmbPurchasing) & ", " & ComboValue(cmbPurchasing) & ", " & sTransferFromSubTeam & ", " & ComboValue(cmbTransferToSubteam) & ", " & iFax_Order & ", " & DateDiff(DateInterval.Day, SystemDateTime, dtpStartDate.Value) + 1 & ", " & giUserID & ", " & System.Math.Abs(chkCredit.CheckState) & ", 1" 'From Queue

        ' Inserts the new order header and returns the OrderHeader_ID in a recordset
        rsOrderID = SQLOpenRS3(sSQLText, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

        If Err.Number <> 0 Then
            logger.Error("SaveOrderHeader - error in  " & sSQLText & " " & Err.Description)
            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            On Error GoTo 0
            m_lOrderHeader_ID = -1
        Else
            On Error GoTo 0
            If rsOrderID.EOF Then
                m_lOrderHeader_ID = -1
            Else
                m_lOrderHeader_ID = rsOrderID.Fields("OrderHeader_ID").Value
            End If
            logger.Info("SaveOrderHeader " & CStr(m_lOrderHeader_ID) & " Orderheader information saved successfully")
            rsOrderID.Close()
        End If

        logger.Debug("SaveOrderHeader Exit")
    End Sub

    Private Sub SaveOrderItems()
        logger.Debug("SaveOrderItems Entry")

        Dim rsOrderItem As DAO.Recordset = Nothing
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
        Dim iCost_Unit As Integer
        Dim iFreight_Unit As Integer
        Dim iPackage_Unit As Integer
        Dim lLoop As Integer
        Dim lIgnoreErrNum(0) As Integer
        Dim sdr() As DataRow = mdt.Select("Selected = '*'")

        If sdr.Length > 0 Then
            Dim dr As DataRow
            For lLoop = sdr.GetLowerBound(0) To sdr.GetUpperBound(0)
                dr = sdr(lLoop)
                '-- Create a new orderitem
                rsOrderItem = SQLOpenRecordSet("EXEC AutomaticOrderItemInfo " & dr("Item_Key") & ", " & m_lOrderHeader_ID & ", NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                With rsOrderItem
                    If IsDBNull(.Fields("CostUnit").Value) Then
                        '20101123 Dave Stacey  TFS 13734 - Skip Order Items which don't have Cost Units after warning message
                        'instead of disallowing entire order
                        logger.Warn("SaveOrderItems - Unable to add the selected item to the order because it does not have a Cost Unit ID assigned: Item_Key=" + dr("Item_Key").ToString + ", m_lOrderHeader_ID=" + m_lOrderHeader_ID.ToString)
                        MsgBox("Item [" + dr("Identifier").ToString + "] has no Cost Unit ID assigned to it.  This value must be assigned to add the item to the order." &
                                vbCrLf & vbCrLf & "Please close the empty Line Item Information screen that follows.", MsgBoxStyle.Exclamation, "Missing Data")
                        Continue For
                    Else
                        iCost_Unit = .Fields("CostUnit").Value
                    End If

                    If IsDBNull(.Fields("Package_Unit_ID").Value) Then
                        iPackage_Unit = 0
                    Else
                        iPackage_Unit = .Fields("Package_Unit_ID").Value
                    End If

                    If IsDBNull(.Fields("VendorNetDiscount").Value) Then
                        m_VendorDiscountAmt = 0
                    Else
                        m_VendorDiscountAmt = .Fields("VendorNetDiscount").Value
                    End If

                    '-- Pre markup Cost
                    sCost = .Fields("Cost").Value

                    '-- Discount Cost 
                    '--DBS 20110825 TFS 1158 - discounts are calced coming in now so took out discounting logic that was throwing cost off
                    sDiscountCost = sCost
                    sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, dr("Unit_ID"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * dr("Quantity")

                    '-- Pre markup Freight
                    sFreight = .Fields("Freight").Value
                    iFreight_Unit = .Fields("FreightUnit").Value
                    sLineItemFreight = CostConversion(sFreight, iFreight_Unit, dr("Unit_ID"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * dr("Quantity")
                    sLandedCost = (sLineItemCost + sLineItemFreight) / dr("Quantity")

                    '-- Markup
                    Dim cu As tItemUnit
                    Dim fu As tItemUnit

                    cu = GetItemUnit(iCost_Unit)
                    fu = GetItemUnit(iFreight_Unit)

                    sLineItemCost = CostConversion(sDiscountCost * (.Fields("MarkupPercent").Value + 100) / 100, iCost_Unit, dr("Unit_ID"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * dr("Quantity")
                    sLineItemFreight = CostConversion(sFreight * (.Fields("MarkupPercent").Value + 100) / 100, iFreight_Unit, dr("Unit_ID"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0) * dr("Quantity")
                    sUnitCost = CostConversion(sLineItemCost / dr("Quantity"), dr("Unit_ID"), IIf(cu.IsPackageUnit, giUnit, iCost_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0)
                    sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / dr("Quantity"), dr("Unit_ID"), IIf(fu.IsPackageUnit, giUnit, iFreight_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, iPackage_Unit, 0, 0)
                    sMarkupCost = (sLineItemCost + sLineItemFreight) / dr("Quantity")

                    lIgnoreErrNum(0) = 50002

                    On Error Resume Next
                    SQLExecute3("EXEC InsertOrderItemCredit " &
                                            m_lOrderHeader_ID &
                                            ", " & dr("Item_Key") &
                                            ", " & .Fields("Units_Per_Pallet").Value &
                                            ", " & dr("Unit_ID") &
                                            ", " & dr("Quantity") &
                                            ", " & sCost &
                                            ", " & iCost_Unit &
                                            ", 0, NULL, " & sFreight &
                                            ", " & iFreight_Unit &
                                            ", " & .Fields("AdjustedCost").Value &
                                            ", " & .Fields("QuantityDiscount").Value &
                                            ", " & .Fields("DiscountType").Value &
                                            ", " & sLandedCost &
                                            ", " & sLineItemCost &
                                            ", " & sLineItemFreight &
                                            ", " & 0 &
                                            ", " & sUnitCost &
                                            ", " & sUnitExtCost &
                                            ", " & .Fields("Package_Desc1").Value &
                                            ", " & .Fields("Package_Desc2").Value &
                                            ", " & .Fields("Package_Unit_ID").Value &
                                            ", " & .Fields("MarkupPercent").Value &
                                            ", " & sMarkupCost &
                                            ", " & .Fields("Retail_Unit_ID").Value &
                                            ", " & IIf(IsDBNull(dr("CreditReason_ID")), "NULL", dr("CreditReason_ID")) &
                                            "," & giUserID &
                                            ", " & m_VendorDiscountAmt &
                                            ", " & IIf(IsDBNull(.Fields("HandlingCharge").Value), 0, .Fields("HandlingCharge").Value),
                                            DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
                    If Err.Number <> 0 Then
                        logger.Error("SaveOrderItems - Error in EXEC InsertOrderItemCredit " & Err.Description)
                        MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
                        On Error GoTo 0
                        SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        rsOrderItem.Close()
                        Exit Sub
                    End If
                    On Error GoTo 0
                End With
            Next
        End If

        rsOrderItem.Close()

        logger.Debug("SaveOrderItems Exit")
    End Sub

    Private Sub DeleteQueueItems()
        logger.Debug("DeleteQueueItems Entry")

        Dim lLoop As Integer
        Dim sdr() As DataRow = mdt.Select("Selected = '*'")

        If sdr.Length > 0 Then
            Dim dr As DataRow
            For lLoop = sdr.GetLowerBound(0) To sdr.GetUpperBound(0)
                dr = sdr(lLoop)
                SQLExecute("Exec DeleteOrderItemQueue " & dr("OrderItemQueue_ID"), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next
        End If

        logger.Debug("DeleteQueueItems Exit")
    End Sub

    Private Sub SaveQueueItems()
        logger.Debug("SaveQueueItems Entry")

        If mdt Is Nothing Then Exit Sub

        Dim cdt As DataTable = mdt.GetChanges(DataRowState.Modified)

        If Not (cdt Is Nothing) Then
            Dim row As DataRow
            Dim i As Integer
            For i = 0 To cdt.Rows.Count - 1
                row = cdt.Rows(i)
                SQLExecute("EXEC UpdateOrderItemQueue " & row("OrderItemQueue_ID") & ", " & row("Quantity") & ", " & IIf(Not IsDBNull(row("CreditReason_ID")), row("CreditReason_ID"), "NULL"), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Next
        End If

        mdt.AcceptChanges()

        logger.Debug("SaveQueueItems Exit")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        logger.Debug("cmdDelete_Click Entry")
        Call DeleteItems()
        logger.Debug("cmdDelete_Click Exit")
    End Sub

    Private Sub DeleteItems(Optional ByRef bShowMsg As Boolean = True)
        logger.Debug("DeleteItems Entry")

        If bShowMsg = True Then
            If (grdList.Rows.Count = 0) Then
                MsgBox("Please search for items.", MsgBoxStyle.Information, Me.Text)
                cmdSearch.Focus()
                Exit Sub
            End If
        End If

        If bShowMsg = True Then
            If grdList.Selected.Rows.Count = 0 Then
                logger.Info("DeleteItems - Please highlight item(s) to delete from the Order Item Queue")
                MsgBox("Please highlight item(s) to delete from the Order Item Queue.", MsgBoxStyle.Information, Me.Text)
                logger.Debug("DeleteItems Exit")
                Exit Sub
            End If

            If MsgBox("Delete these items from the Queue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                logger.Debug("DeleteItems Exit")
                Exit Sub
            End If
        End If

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim rowSelected As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim drv As DataRowView
        Dim dr As DataRow

        For Each rowSelected In grdList.Selected.Rows
            drv = DirectCast(rowSelected.ListObject, DataRowView)
            dr = drv.Row
            SQLExecute("Exec DeleteOrderItemQueue " & dr("OrderItemQueue_ID"), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            rowSelected.Delete()
            dr.AcceptChanges()
        Next

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("DeleteItems Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        If ClearGrid() Then Call Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdItemEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemEdit.Click
        logger.Debug("cmdItemEdit_Click Entry")

        If grdList.Rows.Count = 0 Then
            MsgBox("Please search for items.", MsgBoxStyle.Information, Me.Text)
            cmdSearch.Focus()
            logger.Debug("cmdItemEdit_Click Exit")
            Exit Sub
        End If

        If grdList.Selected.Rows.Count = 0 Then
            MsgBox("Please highlight one item.", MsgBoxStyle.Information, Me.Text)
            logger.Debug("cmdItemEdit_Click Exit")
            Exit Sub
        Else
            If grdList.Selected.Rows.Count > 1 Then
                MsgBox("Please highlight only one item.", MsgBoxStyle.Information, Me.Text)
                logger.Debug("cmdItemEdit_Click Exit")
                Exit Sub
            End If
        End If

        glItemID = grdList.Selected.Rows(0).Cells("Item_Key").Value

        frmItem.ShowDialog()
        frmItem.Close()

        logger.Debug("cmdItemEdit_Click Exit")
    End Sub

    Private Sub cmdReset_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReset.Click
        logger.Debug("cmdReset_Click Entry")

        If ClearGrid() Then
            Call DefaultControls()

            txtVendor.Text = String.Empty

            SetActive(cmdVendorSearch, True)

            cmdVendorSearch.Focus()
        End If

        logger.Debug("cmdReset_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")
        If Len(Trim(txtVendor.Text)) = 0 Then
            MsgBox("Please select a Vendor.", MsgBoxStyle.Information, Me.Text)
            txtVendor.Focus()
            logger.Debug("cmdSearch_Click Exit")
            Exit Sub
        End If

        If cmbPurchasing.SelectedIndex = -1 Then
            MsgBox("Please select a Purchasing store.", MsgBoxStyle.Information, Me.Text)
            cmbPurchasing.Focus()
            logger.Debug("cmdSearch_Click Exit")
            Exit Sub
        End If

        If (geOrderType <> enumOrderType.Purchase) Then
            '-- Make sure a transfer from subteam is selected
            If cmbTransferFromSubteam.SelectedIndex = -1 Then
                MsgBox("Please enter a value for Transfer From.", MsgBoxStyle.Information, Me.Text)
                cmbTransferFromSubteam.Focus()
                logger.Debug("cmdSearch_Click Exit")
                Exit Sub
            End If
        End If

        '-- Make sure a transfer to subteam is selected, always
        If cmbTransferToSubteam.SelectedIndex = -1 Then
            MsgBox("Please enter a value for Transfer To.", MsgBoxStyle.Information, Me.Text)
            cmbTransferToSubteam.Focus()
            logger.Debug("cmdSearch_Click Exit")
            Exit Sub
        End If

        ' check subteam choices
        If Not (geOrderType = enumOrderType.Purchase) Then
            ' make sure that subteam-from is same as subteam-to if both of
            ' the subteams are not manufacturing subteams
            If (m_abSubTeam_From_UnRestricted(cmbTransferFromSubteam.SelectedIndex) = False) And (m_abSubTeam_To_UnRestricted(cmbTransferToSubteam.SelectedIndex) = False) Then
                ' user has selected subteams that are NOT manufacturing
                ' this requires that the transfer-from subteam match the transfer-to subteam
                ' unless the from subteam is packaging or supplies
                If ((geProductType <> enumProductType.PackagingSupplies) And (geProductType <> enumProductType.OtherSupplies)) Then
                    If cmbTransferFromSubteam.Text <> cmbTransferToSubteam.Text Then
                        logger.Info("cmdSearch_Click " & " Transfer From and To must be the same when both subteams are a non-manufacturing subteam.")
                        MsgBox("Transfer From and Transfer To must be the same when both subteams are non-manufacturing subteams.", MsgBoxStyle.Information, Me.Text)
                        cmbTransferToSubteam.Focus()
                        logger.Debug("cmdSearch_Click Exit")
                        Exit Sub
                    End If
                End If

                ' if this is a transfer and non-manufacturing subteams and same store, that's impossible...
                If geOrderType = enumOrderType.Transfer Then
                    'If this is an intra-store transfer
                    If VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex) = glVendorID Then
                        logger.Info("cmdSearch_Click " & "You cannot transfer from a non-manufacturing subteam to a non-manufacturing subteam within the same facility.")
                        MsgBox("Transfers cannot be made from a non-manufacturing subteam to a non-manufacturing subteam within the same facility.", MsgBoxStyle.Information, Me.Text)
                        cmbTransferToSubteam.Focus()
                        logger.Debug("cmdSearch_Click Exit")
                        Exit Sub
                    End If
                End If
            Else
                ' one of the subteams IS a manufacturing subteam
                If geOrderType = enumOrderType.Distribution Then
                    ' for distribution, either the to-subteam is manufacturing OR Both are mfg.
                    ' so if only the from-subteam is manufacturing... that is not allowed.
                    If (m_abSubTeam_To_UnRestricted(cmbTransferToSubteam.SelectedIndex) = False) Then
                        logger.Info("cmdSearch_Click " & " You cannot distribute from a manufacturing subteam to a non-manufacturing subteam.")
                        MsgBox("Manufacturing subteams cannot distribute to a non-manufacturing subteam.", MsgBoxStyle.Information, Me.Text)
                        cmbTransferToSubteam.Focus()
                        logger.Debug("cmdSearch_Click Exit")
                        Exit Sub
                    End If
                    ' if this is a transfer in the same store, the subteams MUST be different
                ElseIf geOrderType = enumOrderType.Transfer Then
                    'If this is an intra-store transfer
                    If VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex) = glVendorID Then
                        'Tranfer From and To must be different
                        If cmbTransferFromSubteam.Text = cmbTransferToSubteam.Text Then
                            logger.Info("cmdSearch_Click " & "Transfer From and To must be different when transfering within the same facility.")
                            MsgBox("Transfer From and Transfer To must be different when transfering within the same facility.", MsgBoxStyle.Information, Me.Text)
                            cmbTransferToSubteam.Focus()
                            logger.Debug("cmdSearch_Click Exit")
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If

        lblDisplayCount.Text = String.Empty

        Call RefreshDataSource()

        logger.Debug("cmdSearch_Click Exit")
    End Sub

    Private Sub RefreshDataSource(Optional ByRef bShowMsg As Boolean = True)
        logger.Debug("RefreshDataSource Entry")

        Dim sSQLText As String
        Dim sTransferFromSubTeam As String
        Dim sLimitFromSubTeam As String

        If (ClearGrid(bShowMsg) = False) Then Exit Sub

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'default
        sLimitFromSubTeam = "NULL"

        If (geOrderType = enumOrderType.Purchase) Then
            sTransferFromSubTeam = "NULL"
            If (m_abSubTeam_To_UnRestricted(cmbTransferToSubteam.SelectedIndex) = False) Then
                ' Limit items to only this retail subteam
                sLimitFromSubTeam = CStr(VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex))
            End If
        Else
            sTransferFromSubTeam = CStr(VB6.GetItemData(cmbTransferFromSubteam, cmbTransferFromSubteam.SelectedIndex))
            If (m_abSubTeam_From_UnRestricted(cmbTransferFromSubteam.SelectedIndex) = False) Then
                ' Limit items to only this retail subteam
                sLimitFromSubTeam = sTransferFromSubTeam
            End If
        End If

        ' Search for the items
        sSQLText = "EXEC GetOrderItemQueueSearch " & CShort(geOrderType) & ", '" & glVendorID & "', " & VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex) & ", " & sTransferFromSubTeam & ", " & VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex) & ", " & sLimitFromSubTeam & ", " & CShort(geProductType) & ", " & IIf(m_bIsVendorDistributorManufacturer, 1, 0) & ", " & IIf(m_bIsVendorStoreSame, 1, 0) & ", " & Val(CStr(chkDiscontinued.CheckState)) & ", " & Val(CStr(chkCredit.CheckState))
        SQLOpenRS(m_rsOrderItems, sSQLText, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If Not (m_rsOrderItems.EOF And m_rsOrderItems.BOF) Then
            Dim row As DataRow

            m_rsOrderItems.MoveFirst()

            While Not m_rsOrderItems.EOF
                row = mdt.NewRow
                row("OrderItemQueue_ID") = m_rsOrderItems.Fields("OrderItemQueue_ID").Value
                row("Item_Key") = m_rsOrderItems.Fields("Item_Key").Value
                row("Identifier") = m_rsOrderItems.Fields("Identifier").Value
                row("Item_Description") = m_rsOrderItems.Fields("Item_Description").Value
                row("SubTeam_No") = m_rsOrderItems.Fields("SubTeam_No").Value
                row("SubTeamName") = m_rsOrderItems.Fields("SubTeamName").Value
                row("NotAvailable") = m_rsOrderItems.Fields("NotAvailable").Value
                row("Quantity") = m_rsOrderItems.Fields("Quantity").Value
                row("Unit_ID") = m_rsOrderItems.Fields("Unit_ID").Value
                row("QuantityUnitName") = m_rsOrderItems.Fields("QuantityUnitName").Value
                row("Cost") = m_rsOrderItems.Fields("Cost").Value
                row("PrimaryVendor") = m_rsOrderItems.Fields("PrimaryVendor").Value
                row("UserName") = m_rsOrderItems.Fields("UserName").Value
                If m_rsOrderItems.Fields("CreditReason_ID").Value > 0 Then row("CreditReason_ID") = m_rsOrderItems.Fields("CreditReason_ID").Value
                row("Insert_Date") = m_rsOrderItems.Fields("Insert_Date").Value
                row("Discontinued") = m_rsOrderItems.Fields("Discontinue_Item").Value
                row("Selected") = String.Empty
                mdt.Rows.Add(row)

                m_rsOrderItems.MoveNext()
            End While

            mdt.AcceptChanges()

            Me.grdList.DataSource = mdt

            ' don't allow them to modify the orderheader info after searching
            '   User must either reset (using squeegie or make an order)
            SetActive(cmdVendorSearch, False)

            Call EnableHeaderControls(False)
            Call EnableDetailButtons(True)

        Else
            logger.Info("RefreshDataSource " & "No matching items found." & vbNewLine & " Try changing the search criteria.")
            MsgBox("No matching items found." & vbNewLine & "Please try changing the search criteria.", MsgBoxStyle.Information, Me.Text)
        End If

        m_rsOrderItems.Close()

        FormatDataGrid()
        If grdList.Rows.Count > 0 Then grdList.ActiveRow = grdList.Rows(0)

        logger.Debug("RefreshDataSource Exit")
    End Sub

    ''' <summary>
    ''' Used to do any custom formatting for the UltraGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        If grdList.DisplayLayout.Bands(0).Columns.Count > 0 Then

            ' The 'Cost' column is the only column that needs specific formatting
            grdList.DisplayLayout.Bands(0).Columns("Cost").Format = "###,###,##0.00"

        End If
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        logger.Debug("cmdSelect_Click Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim dr() As DataRow
        Dim bChanged As Boolean
        Dim bContainsSelected As Boolean
        Dim bContainsUnselected As Boolean

        If grdList.Rows.Count = 0 Then
            MsgBox("Search for items.", MsgBoxStyle.Information, Me.Text)
            cmdSearch.Focus()
            Exit Sub
        End If

        If grdList.Selected.Rows.Count = 0 Then
            MsgBox("Please highlight item(s).", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        ' Iterate through the selected items first to determine if there is a mixture.
        '   If there are some selected and some unselected, this will select them all
        For Each row In Me.grdList.Selected.Rows
            If row.Cells("Selected").Value = "*" Then
                bContainsSelected = True
            Else
                bContainsUnselected = True
            End If
            If (bContainsSelected And bContainsUnselected) Then Exit For
        Next

        grdList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate
        SkipAutoSelect = True

        For Each row In Me.grdList.Selected.Rows
            dr = mdt.Select("OrderItemQueue_ID = " & row.Cells("OrderItemQueue_ID").Value.ToString)
            bChanged = (dr(0).RowState = DataRowState.Modified)
            If (bContainsSelected And bContainsUnselected) Then
                ' there is a mixture, so select them all
                row.Cells("Selected").Value = "*"
            Else
                If row.Cells("Selected").Value = "*" Then
                    row.Cells("Selected").Value = ""
                Else
                    row.Cells("Selected").Value = "*"
                End If
            End If

            grdList.UpdateData()

            If Not bChanged Then dr(0).AcceptChanges()
        Next

        SkipAutoSelect = False
        grdList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus

        ' Calculate Total Ordered Cost based on what is selected or deselected
        CalculateOrderedCost()

        logger.Debug("cmdSelect_Click Exit")
    End Sub

    Private Sub cmdSelectAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelectAll.Click
        logger.Debug("cmdSelectAll_Click Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim dr() As DataRow
        Dim bChanged As Boolean

        If grdList.Rows.Count = 0 Then
            MsgBox("Pleaes search for items.", MsgBoxStyle.Information, Me.Text)
            cmdSearch.Focus()
            Exit Sub
        End If

        Dim sValue As String

        If cmdSelectAll.Tag = "Select" Then
            sValue = "*"
            cmdSelectAll.Tag = "Deselect"
            cmdSelectAll.Image = imgIcons.Images.Item(iNONE)
            ToolTip1.SetToolTip(cmdSelectAll, "Deselect All")
        Else
            sValue = String.Empty
            cmdSelectAll.Tag = "Select"
            cmdSelectAll.Image = imgIcons.Images.Item(iALL)
            ToolTip1.SetToolTip(cmdSelectAll, "Select All")
        End If

        grdList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate
        SkipAutoSelect = True

        For Each row In grdList.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            dr = mdt.Select("OrderItemQueue_ID = " & row.Cells("OrderItemQueue_ID").Value.ToString)
            bChanged = (dr(0).RowState = DataRowState.Modified)
            row.Cells("Selected").Value = sValue
            grdList.UpdateData()
            If Not bChanged Then dr(0).AcceptChanges()
        Next row

        SkipAutoSelect = False
        grdList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus

        ' Calculate Ordered Cost based on what is selected or deselected
        CalculateOrderedCost()

        logger.Debug("cmdSelectAll_Click Exit")
    End Sub

    ''' <summary>
    ''' Calculates the Total Cost of the Selected Items
    ''' </summary>
    ''' <remarks>The result is shown with the lblOrderedTotalAmount label</remarks>
    Private Sub CalculateOrderedCost()
        logger.Debug("CalculateOrderedCost() Entry")
        Dim orderTotal As Decimal = 0

        Dim row As UltraGridRow
        For Each row In Me.grdList.DisplayLayout.Bands(0).GetRowEnumerator(GridRowType.DataRow)
            If row.Cells("Selected").Value = "*" Then
                orderTotal = orderTotal + row.Cells("Cost").Value
            End If
        Next

        ' Show Total with lblOrderedTotalAmount Label
        lblOrderedTotalAmount.Text = orderTotal.ToString("C")

        logger.Debug("CalculateOrderedCost() Exit")
    End Sub

    Private Sub cmdSubmit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSubmit.Click
        logger.Debug("cmdSubmit_Click Entry")

        If grdList.Rows.Count = 0 Then
            MsgBox("Please search for items.", MsgBoxStyle.Information, Me.Text)
            cmdSearch.Focus()
            Exit Sub
        End If

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        ' Persist all records in the grid back to the queue
        '   At the very minimum, it will update the insert_date
        Call SaveQueueItems()

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("cmdSubmit_Click Exit")
    End Sub

    Private Sub cmdVendorSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdVendorSearch.Click
        logger.Debug("cmdVendorSearch_Click Entry")

        Dim lOriginalVendorID As Integer

        '-- Set search criteria
        lOriginalVendorID = glVendorID
        glVendorID = 0
        giSearchType = iSearchVendorCompany
        glOrderHeaderID = 0

        '-- Open the search form
        frmSearch.Text = "Vendor Search"
        frmSearch.ShowDialog()
        frmSearch.Dispose()

        '-- If they added a vendor then populate the screen
        If glVendorID > 0 Then
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Call EnableHeaderControls(True)

            '-- See if it is a distribution or transfer order
            If DetermineVendorInternal() Then
                If ((gbDistribution_Center) Or (gbManufacturer)) Then
                    m_bIsVendorDistributorManufacturer = True
                    geOrderType = enumOrderType.Distribution
                Else
                    geOrderType = enumOrderType.Transfer
                End If
            Else
                ' if no records returned, then this is an external vendor
                geOrderType = enumOrderType.Purchase
            End If

            '-- Set Order Type option buttons
            Call DisplayOrderType()

            ' Populate the Purchasing stores combo and Transfer From combo with list of subteams for this vendor
            If geOrderType = enumOrderType.Purchase Then
                Call LoadRegionCustomer(cmbPurchasing)
                dtpStartDate.Checked = False
                cmbTransferFromSubteam.Items.Clear()
                SetActive(cmbTransferFromSubteam, False)
            Else
                dtpStartDate.Checked = True
                dtpStartDate.Value = DateAdd(DateInterval.Day, 1, System.DateTime.Today)

                If geOrderType = enumOrderType.Distribution Then
                    Call DisplayDistribution()
                Else ' Transfer
                    Call DisplayTransfer()
                End If
            End If

            Call SetCombo(cmbPurchasing, GetSetting("IRMA", "OrderItemQueue", "Purchaser"))

            txtVendor.Text = ReturnVendorName(glVendorID)

            Call LimitStore()

        Else
            If Len(txtVendor.Text) > 0 And lOriginalVendorID > 0 Then
                ' User simply canceled the vendor search form
                glVendorID = lOriginalVendorID
            Else
                Call DefaultControls()
            End If
        End If

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        cmbPurchasing.Focus()

        logger.Debug("cmdVendorSearch_Click Exit")
    End Sub

    Private Sub LimitStore()
        logger.Debug("LimitStore Entry")

        Dim i As Short

        ' unselect the purchasing and Transfer-To SubTeam
        Call UnselectCombos()

        '-- Limit them to one store
        If (gbBuyer Or gbDistributor) And (Not gbCoordinator) And (Not IsDBNull(glVendor_Limit)) Then
            For i = 0 To cmbPurchasing.Items.Count - 1
                If VB6.GetItemData(cmbPurchasing, i) = glVendor_Limit Then
                    cmbPurchasing.SelectedIndex = i
                    SetActive(cmbPurchasing, False)
                    Exit For
                End If
            Next i
        End If

        If (cmbPurchasing.BackColor <> COLOR_INACTIVE) Then
            Call SetCombo(cmbPurchasing, GetSetting("IRMA", "OrderItemQueue", "Purchaser"))
        End If

        logger.Debug("LimitStore Exit")
    End Sub

    Private Sub DefaultControls()
        logger.Debug("DefaultControls Entry")

        ' Until the vendor is selected
        cmbTransferFromSubteam.Items.Clear()
        chkCredit.CheckState = False

        If glStore_Limit <> 0 Then
            Call SetCombo(cmbPurchasing, glStore_Limit)
        Else
            Call SetCombo(cmbPurchasing, GetSetting("IRMA", "OrderItemQueue", "Purchaser"))
        End If

        Call SetCombo(cmbTransferToSubteam, GetSetting("IRMA", "OrderItemQueue", "TransferToSubTeam"))
        Call EnableHeaderControls(False)
        Call EnableDetailButtons(False)

        cmdSelectAll.Image = imgIcons.Images.Item(iALL)

        ToolTip1.SetToolTip(cmdSelectAll, "Select All")

        ' Prevent expected dates in the past.
        dtpStartDate.MinDate = Date.Today
        dtpStartDate.Value = System.DateTime.Today
        dtpStartDate.ShowCheckBox = True
        dtpStartDate.Checked = False
        optPurchase.Checked = False
        optDistribution.Checked = False
        optTransfer.Checked = False
        chkDiscontinued.CheckState = False

        grdList.DisplayLayout.Bands(0).Columns("NotAvailable").Hidden = True
        grdList.DisplayLayout.Bands(0).Columns("CreditReason_ID").Hidden = True
        AdjustGridColWidths()

        ' Set Total Ordered Cost label to include Currency label
        lblOrderedTotalAmount.Text = Decimal.Parse("0.00").ToString("C")

        logger.Debug("DefaultControls Exit")
    End Sub

    Private Sub EnableHeaderControls(ByRef bValue As Boolean)
        logger.Debug("EnableHeaderControls Entry")

        ' turn controls on or off
        SetActive(cmbPurchasing, bValue)
        SetActive(cmbTransferFromSubteam, bValue)
        SetActive(optPurchase, bValue)
        SetActive(optDistribution, bValue)
        SetActive(optTransfer, bValue)
        SetActive(cmbTransferToSubteam, bValue)
        SetActive(cmbProductType, bValue)
        SetActive(chkCredit, bValue)
        SetActive(chkDiscontinued, bValue)

        logger.Debug("EnableHeaderControls Exit")
    End Sub

    Private Sub EnableDetailButtons(ByRef bValue As Boolean)
        logger.Debug("EnableDetailButtons Entry")

        ' buttons below the grid
        SetActive(cmdDelete, bValue)
        SetActive(cmdSubmit, bValue)
        SetActive(cmdItemEdit, bValue)
        SetActive(cmdSelectAll, bValue)
        SetActive(cmdSelect, bValue)
        SetActive(btnApplyAllCreditReason, bValue And btnApplyAllCreditReason.Visible, Not (bValue And btnApplyAllCreditReason.Visible))
        SetActive(cmdCreateOrder, bValue)
        SetActive(cmdSearch, Not bValue)

        logger.Debug("EnableDetailButtons Exit")
    End Sub

    Private Sub DisplayDistribution()
        logger.Debug("DisplayDistribution Entry")

        SetActive(chkCredit, True)

        Call LoadStoreCustomer(cmbPurchasing, glStoreNo)
        Call LimitStore()

        ' Change From-Subteam to supplier subteams (zoneSubTeams)
        Call LoadSubTeamByType(enumSubTeamType.Supplier, cmbTransferFromSubteam, m_abSubTeam_From_UnRestricted, glStoreNo, 0)

        logger.Debug("DisplayDistribution Exit")
    End Sub

    Private Sub DisplayTransfer()
        logger.Debug("DisplayTransfer Entry")

        SetActive(chkCredit, False)

        ' Load internal customers as purchasers
        Call LoadRegionCustomer(cmbPurchasing)
        Call LimitStore()

        ' if vendor is a distributor, load only non-retail subteams (storesubteams minus non-mfg zonesubteams)
        ' note, if/when user selects purchaser as the same distributor/manufacturing facility, need to open
        ' list back up to all storesubteams - but still follow the manufacturing/non-retail subteam rules
        If m_bIsVendorDistributorManufacturer Then
            Call LoadSubTeamByType(enumSubTeamType.StoreMinusSupplier, cmbTransferFromSubteam, m_abSubTeam_From_UnRestricted, glStoreNo, 0)
        Else
            ' Change From-Subteam to storeSubTeams
            Call LoadSubTeamByType(enumSubTeamType.Store, cmbTransferFromSubteam, m_abSubTeam_From_UnRestricted, glStoreNo, 0)
        End If

        logger.Debug("DisplayTransfer Exit")
    End Sub

    Private Sub UnselectCombos()
        logger.Debug("UnselectCombos Entry")

        If (cmbPurchasing.Enabled = True) Then
            cmbPurchasing.SelectedIndex = -1
            cmbTransferToSubteam.SelectedIndex = -1
        End If

        logger.Debug("UnselectCombos Exit")
    End Sub

    Private Sub cmdViewQueue_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdViewQueue.Click
        logger.Debug("cmdViewQueue_Click Entry")

        ' Go to the maintenance screen
        Call frmOrderItemQueueView.ShowDialog()
        frmOrderItemQueueView.Dispose()

        If grdList.Rows.Count > 0 Then grdList.DataBind()

        cmdVendorSearch.Focus()

        logger.Debug("cmdViewQueue_Click Exit")
    End Sub

    Private Sub frmOrderItemQueue_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmOrderItemQueue_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If cmdSearch.Enabled Then
            If KeyAscii = ASCII_ENTER Then Call cmdSearch_Click(cmdSearch, New System.EventArgs())
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("frmOrderItemQueue_KeyPress Exit")
    End Sub

    Private Function ClearGrid(Optional ByRef bShowMsg As Boolean = True) As Boolean
        logger.Debug("ClearGrid Entry")

        Dim cdt As DataTable = mdt.GetChanges

        If Not (cdt Is Nothing) Then
            Dim iAnswer As Short

            If bShowMsg Then
                iAnswer = MsgBox("Save your changes?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Warning!")
            Else
                iAnswer = MsgBoxResult.Yes
            End If

            If iAnswer = MsgBoxResult.Yes Then
                Call SaveQueueItems()
            ElseIf iAnswer = MsgBoxResult.Cancel Then
                Return False
            End If
        End If

        mdt.Rows.Clear()

        ' save these off for convenience
        If (cmbPurchasing.SelectedIndex > -1) Then
            SaveSetting("IRMA", "OrderItemQueue", "Purchaser", CStr(VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex)))
        End If
        If (cmbTransferToSubteam.SelectedIndex > -1) Then
            SaveSetting("IRMA", "OrderItemQueue", "TransferToSubTeam", CStr(VB6.GetItemData(cmbTransferToSubteam, cmbTransferToSubteam.SelectedIndex)))
        End If

        Return True

        logger.Debug("ClearGrid Exit")
    End Function

    Private Sub cmbPurchasing_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbPurchasing.SelectedIndexChanged
        logger.Debug("cmbPurchasing_SelectedIndexChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

        Dim lPurchasingID As Integer

        ' Load the list of Transfer To subteams
        If (cmbPurchasing.SelectedIndex > -1) Then
            lPurchasingID = VB6.GetItemData(cmbPurchasing, cmbPurchasing.SelectedIndex)
            Call SetActive(cmbTransferToSubteam, True)
            ' default
            IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
        Else
            lPurchasingID = 0
            cmbTransferToSubteam.Items.Clear()
        End If

        If (geOrderType = enumOrderType.Purchase) And (lPurchasingID > 0) Then
            ' Determine if Purchaser is a distributor/manufacturer
            If (DeterminePurchaserFacility(lPurchasingID) = True) Then
                Call LoadSubTeamByType(enumSubTeamType.SupplierByVendor, cmbTransferToSubteam, m_abSubTeam_To_UnRestricted, lPurchasingID, 0)
            Else
                'Load the purchasing store's subteam list from the StoreSubteam table
                Call LoadVendStoreSubteam(cmbTransferToSubteam, lPurchasingID, m_abSubTeam_To_UnRestricted)
            End If
            If (DeterminePurchaserStore(lPurchasingID)) Then
                ' Don't let retail stores order discontinued items from external vendors
                IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
            End If

        ElseIf (lPurchasingID > 0) Then
            'Load the purchasing store's subteam list from the StoreSubteam table
            Call LoadVendStoreSubteam(cmbTransferToSubteam, lPurchasingID, m_abSubTeam_To_UnRestricted)

            If geOrderType = enumOrderType.Transfer Then
                ' check if purchaser and vendor are the same
                If (lPurchasingID = glVendorID) Then
                    ' can only happen for transfer orders
                    m_bIsVendorStoreSame = True
                End If
            End If
        End If

        Call SetCombo(cmbTransferToSubteam, GetSetting("IRMA", "OrderItemQueue", "TransferToSubTeam"))

        logger.Debug("cmbPurchasing_SelectedIndexChanged Exit")
    End Sub

    Private Sub optDistribution_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optDistribution.CheckedChanged
        logger.Debug("optDistribution_CheckedChanged Entry")

        If Me.IsInitializing = True Then Exit Sub
        If eventSender.Checked Then
            ' assign new order type
            geOrderType = enumOrderType.Distribution
            Call DisplayDistribution()
        End If

        logger.Debug("optDistribution_CheckedChanged Exit")
    End Sub

    Private Sub optTransfer_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optTransfer.CheckedChanged
        logger.Debug("optTransfer_CheckedChanged Entry")

        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            ' assign new order type
            geOrderType = enumOrderType.Transfer
            Call DisplayTransfer()
        End If

        logger.Debug("optTransfer_CheckedChanged Exit")
    End Sub

    Private Sub txtVendor_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtVendor.KeyPress
        logger.Debug("txtVendor_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Call ProcessKeyPress(KeyAscii, txtVendor)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtVendor_KeyPress Exit")
    End Sub

    Private Sub cmbTransferFromSubteam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbTransferFromSubteam.KeyPress
        logger.Debug("cmbTransferFromSubteam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = ASCII_BACKSPACE Then
            cmbTransferFromSubteam.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbTransferFromSubteam_KeyPress Exit")
    End Sub

    Private Sub ProcessKeyPress(ByRef KeyAscii As Short, ByRef currentTextbox As System.Windows.Forms.TextBox)
        logger.Debug("ProcessKeyPress Entry")

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (currentTextbox.Tag), currentTextbox, 0, 0, 0)

        If KeyAscii = ASCII_ENTER Then
            Call RefreshDataSource()
            KeyAscii = ASCII_NULL
        End If

        logger.Debug("ProcessKeyPress Exit")
    End Sub

    Private Sub DisplayOrderType()
        logger.Debug("DisplayOrderType Entry")

        ' Clear the controls and disable
        optPurchase.Checked = False
        optDistribution.Checked = False
        optTransfer.Checked = False
        optPurchase.Enabled = False
        optDistribution.Enabled = False
        optTransfer.Enabled = False

        grdList.DisplayLayout.Bands(0).Columns("NotAvailable").Hidden = True

        ' Turn one on, and enable if appropriate
        Select Case geOrderType
            Case enumOrderType.Distribution
                ' default value
                optDistribution.Checked = True
                optDistribution.Enabled = True
                ' allow user to change to transfer
                optTransfer.Enabled = True
                grdList.DisplayLayout.Bands(0).Columns("NotAvailable").Hidden = False
            Case enumOrderType.Purchase
                optPurchase.Checked = True
                ' all controls stay disabled
            Case enumOrderType.Transfer
                optTransfer.Checked = True
                ' all controls stay disabled
        End Select

        AdjustGridColWidths()

        logger.Debug("DisplayOrderType Exit")
    End Sub

    Public WriteOnly Property IncludeDiscontinued() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = enumChkBoxValues.UncheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDiscontinued.Enabled = False
                Case Value = enumChkBoxValues.UncheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDiscontinued.Enabled = True
                Case Value = enumChkBoxValues.CheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = False
                Case Value = enumChkBoxValues.CheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = True
            End Select
        End Set
    End Property

    Private Sub cmbProductType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbProductType.KeyPress
        logger.Debug("cmbProductType_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbProductType_KeyPress Exit")
    End Sub

    Private Sub grdList_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdList.AfterCellActivate
        logger.Debug("grdList_AfterCellActivate Entry")

        If grdList.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

        logger.Debug("grdList_AfterCellActivate Exit")
    End Sub

    Private Sub grdList_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles grdList.AfterRowUpdate
        logger.Debug("grdList_AfterRowUpdate Entry")

        If e.Row.Cells("Quantity").Value = 0 Then
            DeleteItem(e.Row.Cells("OrderItemQueue_ID").Value)
        Else
            If Not SkipAutoSelect Then e.Row.Cells("Selected").Value = "*"
        End If

        logger.Debug("grdList_AfterRowUpdate Exit")
    End Sub

    Private Sub grdList_BeforeCellActivate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles grdList.BeforeCellActivate
        logger.Debug("grdList_BeforeCellActivate Entry")

        If e.Cell.Column.Key = "Quantity" Then
            If GetWeight_Unit(CInt(e.Cell.Row.Cells("Unit_ID").Value)) Then
                e.Cell.Column.MaskInput = "{double:5.2}"
            Else
                e.Cell.Column.MaskInput = "nnn"
            End If
        End If

        logger.Debug("grdList_BeforeCellActivate Exit")
    End Sub

    Private Sub grdList_BeforeRowsDeleted(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs) Handles grdList.BeforeRowsDeleted
        e.DisplayPromptMsg = False
    End Sub

    Public Sub DeleteItem(ByVal OrderItemQueueID As Integer)
        logger.Debug("DeleteItem Entry")

        SQLExecute("Exec DeleteOrderItemQueue " & OrderItemQueueID.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Dim dr() As DataRow
        dr = mdt.Select("OrderItemQueue_ID = " & OrderItemQueueID.ToString)

        If dr.Length > 0 Then
            dr(0).Delete()
            dr(0).AcceptChanges()
        End If

        dr = Nothing

        logger.Debug("DeleteItem Exit")
    End Sub

    Private Sub grdList_BeforeRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles grdList.BeforeRowUpdate
        logger.Debug("grdList_BeforeRowUpdate Entry")

        If e.Row.Cells("Quantity").Value = 0 Then
            If MsgBox("Delete this item from the Queue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then e.Cancel = True
        End If

        logger.Debug("grdList_BeforeRowUpdate Exit")
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        logger.Debug("grdList_InitializeLayout Entry")
        grdList.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
        logger.Debug("grdList_InitializeLayout Exit")
    End Sub

    Private Sub AdjustGridColWidths()
        logger.Debug("AdjustGridColWidths Entry")

        Dim i As Integer
        Dim usedWidth As Integer
        Dim newWidth As Integer

        For i = 0 To grdList.DisplayLayout.Bands(0).Columns.Count - 1
            If grdList.DisplayLayout.Bands(0).Columns(i).Key <> "Item_Description" Then
                If grdList.DisplayLayout.Bands(0).Columns(i).Hidden = False Then
                    usedWidth = usedWidth + grdList.DisplayLayout.Bands(0).Columns(i).Width
                End If
            End If
        Next i

        newWidth = grdList.Size.Width - (usedWidth + 40)
        grdList.DisplayLayout.Bands(0).Columns("Item_Description").Width = newWidth
        grdList.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns

        logger.Debug("AdjustGridColWidths Exit")
    End Sub

    'Use this for grid navigation capability.
    Private Sub grdList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdList.KeyDown
        logger.Debug("grdList_KeyDown Entry")

        Select Case e.KeyValue
            Case Keys.Up
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select

        logger.Debug("grdList_KeyDown Exit")
    End Sub

    Private Sub btnApplyAllCreditReason_Click(sender As Object, e As EventArgs) Handles btnApplyAllCreditReason.Click
        If chkCredit.CheckState = 1 Then
            ' Check the items that are selected
            Dim sdr() As DataRow = mdt.Select("Selected = '*' AND Quantity > 0 AND CreditReason_ID IS NOT NULL")
            If sdr.Length = 0 Then
                sdr = Nothing
                logger.Info("btnApplyAllCreditReason_Click - You must enter a credit reason for at least one item with a quantity greater than zero")
                MsgBox("Please enter a credit reason for at least one item with a quantity greater than zero.", MsgBoxStyle.Information, "Credit Reason")
                grdList.Focus()
                Exit Sub
            Else
                Dim sCreateReason_ID As String
                Dim dr As DataRow
                sCreateReason_ID = sdr(0)("CreditReason_ID")

                Dim sdrn() As DataRow = mdt.Select("Selected = '*' AND Quantity > 0 AND CreditReason_ID IS NULL")

                For Each dr In sdrn
                    dr("CreditReason_ID") = sCreateReason_ID
                Next

                dr = Nothing
            End If
            sdr = Nothing
        End If

    End Sub

    Private Sub cmbProductType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProductType.SelectedIndexChanged
        setProductType()
    End Sub

    Private Sub setProductType()
        Dim productTypeSelected As String = cmbProductType.Text

        Select Case productTypeSelected
            Case packagingSupplies
                geProductType = enumProductType.PackagingSupplies
            Case otherSupplies
                geProductType = enumProductType.OtherSupplies
            Case Else
                geProductType = enumProductType.Product
        End Select

    End Sub

End Class