Option Strict Off
Option Explicit On

Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports System.Net.Mail
Imports WFM.UserAuthentication
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports log4net

Friend Class frmLineItems

    Dim daItemCatalog As New System.Data.SqlClient.SqlDataAdapter

    Public Property POCreator As String
    Public Property POCloser As String
    Public Property POAdminNotes As String
    Public Property POOrderHeaderId As Integer
    Public Property EInvoicedOrder As Boolean = False
    Public Property VendorId As Integer
    Public Property IsPayByAgreedCost As Boolean

    Public mdt As DataTable = New DataTable("LineItems")
    Public mdtExceptions As DataTable = New DataTable("Exceptions")
    Private mdv As DataView

    Private Delegate Sub PaymentAction()
    Private callingAction As PaymentAction = Nothing

    Private gridLayoutFilePath As String = String.Format("{0}\GridLayout\{1}\{2}\", Application.StartupPath, My.Application.Info.Version, My.Application.CurrentUserID)
    Private gridLayoutFileName As String = "UltraGridLineItemsLayout.xml"

    Dim changedRows As List(Of UltraGridRow) = New List(Of UltraGridRow)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Sub New(ByVal OrderHeaderId As Integer)
        InitializeComponent()
        Me.POOrderHeaderId = OrderHeaderId
    End Sub

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub SetupDataTable()
        mdt.Columns.Add(New DataColumn("OrderHeader_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("OrderItem_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("SubTeam_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdt.Columns.Add(New DataColumn("Brand_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("VIN", GetType(String)))
        mdt.Columns.Add(New DataColumn("Case_Pack", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Effective_Case_Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Effective_Unit_Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("From_Vendor_Flag", GetType(String)))
        mdt.Columns.Add(New DataColumn("Cost_Effective_Date", GetType(DateTime)))
        mdt.Columns.Add(New DataColumn("Cost_Insert_Date", GetType(DateTime)))
        mdt.Columns.Add(New DataColumn("EInvoice_Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("ReceivedItemCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("PO_Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("POEffectiveCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("PO_Adjusted_Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("IRMACurrentCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Adjusted_Cost_Reason", GetType(String)))
        mdt.Columns.Add(New DataColumn("Vendor_Order_UOM", GetType(String)))
        mdt.Columns.Add(New DataColumn("Cost_UOM", GetType(String)))
        mdt.Columns.Add(New DataColumn("eInvoice_UOM", GetType(String)))
        mdt.Columns.Add(New DataColumn("cbW", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Ordered_Quantity", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Received_Quantity", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("eInvoice_Quantity", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Einvoice_Case_Pack", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("ReceivingReasonCode", GetType(String)))
        mdt.Columns.Add(New DataColumn("InvoiceNumber", GetType(String)))
        mdt.Columns.Add(New DataColumn("Einvoice_Id", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("POInvCostDiff", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("POExtendedCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("InvoiceExtendedCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("POInvExtCostDiff", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("ResolutionCode", GetType(String)))
        mdt.Columns.Add(New DataColumn("AdminNotes", GetType(String)))
        mdt.Columns.Add(New DataColumn("PaymentType", GetType(String)))
        mdt.Columns.Add(New DataColumn("LineItemSuspended", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("ApprovedByUserId", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("ApprovedDate", GetType(DateTime)))
    End Sub

    Private Sub SetupExceptionsDataTable()
        mdtExceptions.Columns.Add(New DataColumn("Line", GetType(Integer)))
        mdtExceptions.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Brand", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("VendorItemID", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Description", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Ordered", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Unit", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("eInvoiceQty", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("eInvoicePackage", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("eInvoiceUnit", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("eInvoiceWeight", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Weight", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("Cost", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("PkgDesc", GetType(String)))
        mdtExceptions.Columns.Add(New DataColumn("eInvoiceItemException", GetType(String)))
    End Sub

    Private Sub LoadDataTable()
        Dim EInvoiceDAO As New ReceivingListDAO
        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000

        Try
            rsSearch = SQLOpenRecordSet("EXEC GetLineItemDetails " & Me.POOrderHeaderId, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1
                row = mdt.NewRow

                row("OrderItem_ID") = rsSearch.Fields("OrderItem_ID").Value
                row("OrderHeader_ID") = rsSearch.Fields("OrderHeader_ID").Value
                row("Item_Key") = rsSearch.Fields("Item_Key").Value
                row("Identifier") = rsSearch.Fields("Identifier").Value
                row("SubTeam_Name") = rsSearch.Fields("SubTeam_Name").Value
                row("CompanyName") = rsSearch.Fields("CompanyName").Value
                row("Brand_Name") = rsSearch.Fields("Brand_Name").Value
                row("Item_Description") = rsSearch.Fields("Item_Description").Value
                row("VIN") = rsSearch.Fields("VIN").Value
                row("Case_Pack") = rsSearch.Fields("Case_Pack").Value
                row("Effective_Case_Cost") = rsSearch.Fields("Effective_Case_Cost").Value
                row("Effective_Unit_Cost") = rsSearch.Fields("Effective_Unit_Cost").Value
                row("From_Vendor_Flag") = rsSearch.Fields("From_Vendor_Flag").Value
                row("Cost_Effective_Date") = rsSearch.Fields("Cost_Effective_Date").Value
                row("Cost_Insert_Date") = rsSearch.Fields("Cost_Insert_Date").Value
                row("EInvoice_Cost") = rsSearch.Fields("EInvoice_Cost").Value
                row("ReceivedItemCost") = rsSearch.Fields("ReceivedItemCost").Value
                row("InvoiceExtendedCost") = rsSearch.Fields("InvoiceExtendedCost").Value
                row("PO_Cost") = rsSearch.Fields("PO_Cost").Value
                row("IRMACurrentCost") = rsSearch.Fields("IRMACurrentCost").Value

                If rsSearch.Fields("DiscountType").Value > 0 Then
                    row("POEffectiveCost") = GetLineItemCostWithDiscount(rsSearch.Fields("DiscountType").Value, rsSearch.Fields("QuantityDiscount").Value, _
                                                                 rsSearch.Fields("Ordered_Quantity").Value, rsSearch.Fields("POEffectiveCost").Value, _
                                                                 rsSearch.Fields("Freight").Value)
                Else
                    row("POEffectiveCost") = rsSearch.Fields("POEffectiveCost").Value
                End If

                If rsSearch.Fields("PO_Adjusted_Cost").Value = 0 Then
                    row("PO_Adjusted_Cost") = rsSearch.Fields("PO_Adjusted_Cost").Value
                Else
                    row("PO_Adjusted_Cost") = GetLineItemCostWithDiscount(rsSearch.Fields("DiscountType").Value, rsSearch.Fields("QuantityDiscount").Value, _
                                                                 rsSearch.Fields("Ordered_Quantity").Value, rsSearch.Fields("PO_Adjusted_Cost").Value, _
                                                                 rsSearch.Fields("Freight").Value)
                End If

                row("Adjusted_Cost_Reason") = rsSearch.Fields("Adjusted_Cost_Reason").Value
                row("Vendor_Order_UOM") = rsSearch.Fields("Vendor_Order_UOM").Value
                row("Cost_UOM") = rsSearch.Fields("Cost_UOM").Value
                row("eInvoice_UOM") = rsSearch.Fields("eInvoice_UOM").Value
                row("cbW") = rsSearch.Fields("cbW").Value
                row("Ordered_Quantity") = Format(rsSearch.Fields("Ordered_Quantity").Value, "#####0.0###")
                row("Received_Quantity") = Format(rsSearch.Fields("Received_Quantity").Value, "#####0.0###")
                row("eInvoice_Quantity") = Format(rsSearch.Fields("eInvoice_Quantity").Value, "#####0.0###")
                row("Vendor_ID") = rsSearch.Fields("Vendor_ID").Value
                row("Store_No") = rsSearch.Fields("Store_No").Value
                row("Einvoice_Case_Pack") = rsSearch.Fields("Einvoice_Case_Pack").Value
                row("ReceivingReasonCode") = rsSearch.Fields("ReceivingReasonCode").Value
                row("InvoiceNumber") = rsSearch.Fields("InvoiceNumber").Value
                row("Einvoice_Id") = rsSearch.Fields("Einvoice_Id").Value
                row("POInvCostDiff") = IIf(IsDBNull(row("EInvoice_Cost")), 0, row("EInvoice_Cost")) - IIf(IsDBNull(row("POEffectiveCost")), 0, row("POEffectiveCost"))
                row("POExtendedCost") = rsSearch.Fields("POExtendedCost").Value
                row("InvoiceExtendedCost") = rsSearch.Fields("InvoiceExtendedCost").Value
                row("POInvExtCostDiff") = IIf(IsDBNull(rsSearch.Fields("InvoiceExtendedCost").Value), 0, rsSearch.Fields("InvoiceExtendedCost").Value) - IIf(IsDBNull(rsSearch.Fields("ReceivedItemCost").Value), 0, rsSearch.Fields("ReceivedItemCost").Value)
                row("ResolutionCode") = rsSearch.Fields("ResolutionCodeID").Value
                row("AdminNotes") = rsSearch.Fields("AdminNotes").Value
                row("ApprovedByUserId") = rsSearch.Fields("ApprovedByUserId").Value
                row("ApprovedDate") = rsSearch.Fields("ApprovedDate").Value
                row("LineItemSuspended") = rsSearch.Fields("LineItemSuspended").Value

                If Not IsDBNull(rsSearch.Fields("PaymentTypeID").Value) Then
                    If rsSearch.Fields("PaymentTypeID").Value = 0 Then
                        row("PaymentType") = "Pay By Invoice"
                    ElseIf rsSearch.Fields("PaymentTypeID").Value = 1 Then
                        row("PaymentType") = "Pay By PO"
                    Else
                        row("PaymentType") = ""
                    End If
                Else
                    row("PaymentType") = ""
                End If

                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            rsSearch.MovePrevious()
            If rsSearch.Fields("IsEInvoicedOrder").Value = 1 Then Me.EInvoicedOrder = True

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            grdLineItems.DataSource = mdv

            For Each NonGroupByDataRow As UltraGridRow In grdLineItems.Rows.GetAllNonGroupByRows()
                If AllLineItemsResolved() And Not IsDBNull(NonGroupByDataRow.Cells("InvoiceNumber").Value) Then
                    cmdApprove.Enabled = True
                    cmdApprove.BackColor = Color.Red
                Else
                    cmdApprove.Enabled = False
                    cmdApprove.BackColor = Color.LightCoral
                End If
            Next
        Catch ex As Exception
            logger.Debug(String.Format("Exception occurred: {0}. Inner Exception: {1}", ex.Message, ex.InnerException))
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If

            Cursor.Current = Cursors.Default
        End Try

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub ReturnSelection()
        logger.Debug("ReturnSelection Entry")

        '-- Make sure one item was selected
        If grdLineItems.Selected.Rows.Count = 1 Then
            glOrderHeaderID = CInt(grdLineItems.Selected.Rows(0).Cells(1).Text)
            Me.Close()
        ElseIf grdLineItems.Selected.Rows.Count > 1 Then
            MsgBox("Only one item from the list can be selected.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("ReturnSelection- Only one item from the list can be selected.")
        Else
            MsgBox("An item from the list must be selected.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("ReturnSelection -An item from the list must be selected.")
        End If
        logger.Debug("ReturnSelection Exit")
    End Sub

    Private Sub FormatDataGrid()
        logger.Debug("FormatDataGrid entry")
        Dim currentPos As Integer

        If grdLineItems.DisplayLayout.Bands(0).Columns.Count > 0 Then
            currentPos = 0

            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.Caption = "Resolution Code"
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").Style = ColumnStyle.DropDownList
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.Fixed = True
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").Header.Caption = "PO Admin Notes"
            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").CellActivation = Activation.AllowEdit
            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").Header.Fixed = True
            grdLineItems.DisplayLayout.Bands(0).Columns("AdminNotes").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").Header.Caption = "Identifier"
            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").Header.Fixed = True
            grdLineItems.DisplayLayout.Bands(0).Columns("Identifier").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").Header.Caption = "Description"
            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").Header.Fixed = True
            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Description").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Brand_Name").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Brand_Name").Header.Caption = "Brand"
            grdLineItems.DisplayLayout.Bands(0).Columns("Brand_Name").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Brand_Name").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("OrderHeader_ID").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderHeader_ID").Header.Caption = "PO Number"
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderHeader_ID").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderHeader_ID").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceNumber").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceNumber").Header.Caption = "Invoice Number"
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceNumber").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceNumber").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("CompanyName").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("CompanyName").Header.Caption = "Vendor"
            grdLineItems.DisplayLayout.Bands(0).Columns("CompanyName").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("CompanyName").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("VIN").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("VIN").Header.Caption = "VIN"
            grdLineItems.DisplayLayout.Bands(0).Columns("VIN").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("VIN").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Cost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Cost").Header.Caption = "EInvoice Unit Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Cost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Cost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Cost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("POEffectiveCost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("POEffectiveCost").Header.Caption = "PO Effective Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("POEffectiveCost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("POEffectiveCost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("POEffectiveCost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("POInvCostDiff").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvCostDiff").Header.Caption = "PO/Inv Cost Diff"
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvCostDiff").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvCostDiff").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvCostDiff").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceExtendedCost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceExtendedCost").Header.Caption = "Invoice Extended Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceExtendedCost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceExtendedCost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("InvoiceExtendedCost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivedItemCost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivedItemCost").Header.Caption = "Received Item Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivedItemCost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivedItemCost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivedItemCost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("POInvExtCostDiff").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvExtCostDiff").Header.Caption = "PO/Inv Ext Cost Diff"
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvExtCostDiff").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvExtCostDiff").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("POInvExtCostDiff").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Adjusted_Cost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Adjusted_Cost").Header.Caption = "PO Adjusted Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Adjusted_Cost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Adjusted_Cost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Adjusted_Cost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Adjusted_Cost_Reason").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Adjusted_Cost_Reason").Header.Caption = "Adjusted Cost Reason"
            grdLineItems.DisplayLayout.Bands(0).Columns("Adjusted_Cost_Reason").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Adjusted_Cost_Reason").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Cost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Cost").Header.Caption = "PO Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Cost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Cost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("PO_Cost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("IRMACurrentCost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("IRMACurrentCost").Header.Caption = "IRMA Current Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("IRMACurrentCost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("IRMACurrentCost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("IRMACurrentCost").Format = "#,##0.00"

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_UOM").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_UOM").Header.Caption = "PO Cost UOM"
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_UOM").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_UOM").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_UOM").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_UOM").Header.Caption = "eInvoice UOM"
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_UOM").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_UOM").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Vendor_Order_UOM").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Vendor_Order_UOM").Header.Caption = "Vendor Order UOM"
            grdLineItems.DisplayLayout.Bands(0).Columns("Vendor_Order_UOM").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Vendor_Order_UOM").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Case_Pack").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Case_Pack").Header.Caption = "PO Casepack"
            grdLineItems.DisplayLayout.Bands(0).Columns("Case_Pack").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Case_Pack").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Einvoice_Case_Pack").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Einvoice_Case_Pack").Header.Caption = "Invoice Casepack"
            grdLineItems.DisplayLayout.Bands(0).Columns("Einvoice_Case_Pack").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Einvoice_Case_Pack").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("cbW").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("cbW").Header.Caption = "Costed By Weight"
            grdLineItems.DisplayLayout.Bands(0).Columns("cbW").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("cbW").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Ordered_Quantity").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Ordered_Quantity").Header.Caption = "Ordered Qty"
            grdLineItems.DisplayLayout.Bands(0).Columns("Ordered_Quantity").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Ordered_Quantity").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Received_Quantity").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Received_Quantity").Header.Caption = "Received Qty"
            grdLineItems.DisplayLayout.Bands(0).Columns("Received_Quantity").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Received_Quantity").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_Quantity").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_Quantity").Header.Caption = "EInvoice Qty"
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_Quantity").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("eInvoice_Quantity").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivingReasonCode").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivingReasonCode").Header.Caption = "Receiver Reason Code"
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivingReasonCode").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("ReceivingReasonCode").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Effective_Date").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Effective_Date").Header.Caption = "Cost Effective Date"
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Effective_Date").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Effective_Date").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Insert_Date").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Insert_Date").Header.Caption = "Cost Insert Date"
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Insert_Date").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Insert_Date").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("From_Vendor_Flag").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("From_Vendor_Flag").Header.Caption = "From Vendor Flag"
            grdLineItems.DisplayLayout.Bands(0).Columns("From_Vendor_Flag").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("From_Vendor_Flag").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("SubTeam_Name").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("SubTeam_Name").Header.Caption = "SubTeam"
            grdLineItems.DisplayLayout.Bands(0).Columns("SubTeam_Name").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("SubTeam_Name").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").Header.Caption = "Effective Case Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").Format = "#,##0.00"
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Case_Cost").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").Header.Caption = "Effective Unit Cost"
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").Format = "#,##0.00"
            grdLineItems.DisplayLayout.Bands(0).Columns("Effective_Unit_Cost").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("PaymentType").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("PaymentType").Header.Caption = "Payment Type"
            grdLineItems.DisplayLayout.Bands(0).Columns("PaymentType").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("PaymentType").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("PaymentType").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("OrderItem_ID").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderItem_ID").Header.Caption = "Line Item #"
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderItem_ID").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderItem_ID").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("OrderItem_ID").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("LineItemSuspended").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("LineItemSuspended").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("LineItemSuspended").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("LineItemSuspended").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedByUserId").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedByUserId").Header.Caption = "Approved User"
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedByUserId").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedByUserId").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedByUserId").Hidden = True

            currentPos = currentPos + 1

            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedDate").Header.VisiblePosition = currentPos
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedDate").Header.Caption = "Approved Date"
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedDate").CellActivation = Activation.ActivateOnly
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedDate").CellClickAction = CellClickAction.RowSelect
            grdLineItems.DisplayLayout.Bands(0).Columns("ApprovedDate").Hidden = True

            grdLineItems.DisplayLayout.Bands(0).Columns("Item_Key").Hidden = True
            grdLineItems.DisplayLayout.Bands(0).Columns("Vendor_ID").Hidden = True
            grdLineItems.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
            grdLineItems.DisplayLayout.Bands(0).Columns("POExtendedCost").Hidden = True
            grdLineItems.DisplayLayout.Bands(0).Columns("EInvoice_Id").Hidden = True
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Effective_Date").CellActivation = Activation.NoEdit
            grdLineItems.DisplayLayout.Bands(0).Columns("Cost_Insert_Date").CellActivation = Activation.NoEdit

            grdLineItems.DisplayLayout.Bands(0).Override.MaxSelectedRows = 200
            grdLineItems.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

            LoadDisplayLayout()

            ' Load Resolution Codes after we calle LoadDisplayLayout so that reason code management is not affected by a saved xml file
            grdLineItems.DisplayLayout.Bands(0).Columns("ResolutionCode").ValueList = GetResolutionCodeValueList()

            ' Default the first row to selected
            If grdLineItems.Rows.Count > 0 Then
                With grdLineItems.DisplayLayout.Bands(0)
                    .Columns("OrderHeader_ID").CellAppearance.ForeColor = Color.Blue
                    .Columns("OrderHeader_ID").CellAppearance.FontData.Underline = DefaultableBoolean.True
                    .Columns("OrderHeader_ID").CellAppearance.Cursor = Cursors.Hand
                    .Columns("OrderHeader_ID").Tag = "[Left Click] to view the Purchase Order."

                    .Columns("Identifier").CellAppearance.ForeColor = Color.Blue
                    .Columns("Identifier").CellAppearance.FontData.Underline = DefaultableBoolean.True
                    .Columns("Identifier").CellAppearance.Cursor = Cursors.Hand
                    .Columns("Identifier").Tag = "[Left Click] to view the Item Information.  [Right Click] to view the PO Line Item Detail."

                    .Columns("CompanyName").CellAppearance.ForeColor = Color.Blue
                    .Columns("CompanyName").CellAppearance.FontData.Underline = DefaultableBoolean.True
                    .Columns("CompanyName").CellAppearance.Cursor = Cursors.Hand
                    .Columns("CompanyName").Tag = "[Left Click] to view the Vendor Information.  [Right Click] to view the Vendor Item Cost."

                    .Columns("InvoiceNumber").CellAppearance.ForeColor = Color.Blue
                    .Columns("InvoiceNumber").CellAppearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                    .Columns("InvoiceNumber").CellAppearance.Cursor = Cursors.Hand
                    .Columns("InvoiceNumber").Tag = "[Left Click] to view the Invoice."
                End With
            End If
        End If
    End Sub

    Private Sub FormatExceptionsDataGrid()
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Line").Width = 34
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Identifier").Width = 127
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Brand").Width = 127
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("VendorItemID").Width = 107
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Description").Width = 198
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Ordered").Width = 67
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Unit").Width = 67
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("eInvoiceQty").Width = 73
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("eInvoiceWeight").Width = 73
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Weight").Width = 97
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("Cost").Width = 67
        grdEInvoiceExceptions.DisplayLayout.Bands(0).Columns("PkgDesc").Width = 77
    End Sub

    Private Sub LoadEInvoiceExceptions()
        Dim EInvoiceDAO As New ReceivingListDAO
        Dim dt As DataTable
        Dim row As DataRow
        Dim iCounter As Integer = 1

        dt = EInvoiceDAO.GetReceivingListForNOIDNORD(Me.POOrderHeaderId)

        For Each dr As DataRow In dt.Rows
            row = mdtExceptions.NewRow
            row("Line") = CInt(iCounter)
            row("Identifier") = dr.Item("Identifier")
            row("Brand") = dr.Item("Brand")
            row("VendorItemID") = dr.Item("VendorItemID")
            row("Description") = dr.Item("Item_Description")
            row("Ordered") = Trim(Str(CDec(IIf(IsDBNull(dr.Item("QuantityOrdered")), 0, dr.Item("QuantityOrdered")))))
            row("Unit") = dr.Item("Package_Unit")
            row("eInvoiceQty") = IIf(IsDBNull(dr.Item("eInvoiceQuantity")), DBNull.Value, CType(dr.Item("eInvoiceQuantity"), Decimal).ToString("#.###"))
            row("eInvoicePackage") = IIf(IsDBNull(dr.Item("eInvoiceCase_Pack")), DBNull.Value, CType(dr.Item("eInvoiceCase_Pack"), Decimal).ToString("#.###"))
            row("eInvoiceUnit") = dr.Item("InvoiceQuantityUnitName")
            row("eInvoiceWeight") = CType(IIf(IsDBNull(dr.Item("eInvoiceWeight")), 0, dr.Item("eInvoiceWeight")), Decimal).ToString("#.###")
            row("Weight") = IIf(Val(dr.Item("Weight")) = 0, DBNull.Value, CType(dr.Item("Weight"), Decimal).ToString("#.###"))
            row("Cost") = Math.Round(CDec(dr.Item("Cost")), 2)

            If dr.Item("Package_Desc1") IsNot DBNull.Value AndAlso dr.Item("Package_Desc2") IsNot DBNull.Value Then
                row("PkgDesc") = CType(dr.Item("Package_Desc1"), Decimal).ToString("#####.##") & "/" & CType(dr.Item("Package_Desc2"), Decimal).ToString("#####.##") & " " & dr.Item("Package_Unit")
            End If

            row("eInvoiceItemException") = dr.Item("eInvoiceItemException")

            mdtExceptions.Rows.Add(row)
            iCounter = iCounter + 1
        Next

        grdEInvoiceExceptions.DataSource = mdtExceptions
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Dim result As DialogResult = Nothing

        If changedRows.Count > 0 Then
            result = MessageBox.Show("There are unsaved changes that will be lost if you exit. Do you wish to save now?", "Warning - Unsaved Data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            If result = DialogResult.Yes Then
                SaveData()
            End If
        End If

        Me.Close()
    End Sub

    Private Sub frmLineItems_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text + " for PO# " + Me.POOrderHeaderId.ToString()

        SetupDataTable()
        SetupExceptionsDataTable()

        LoadDataTable()
        LoadEInvoiceExceptions()

        If grdEInvoiceExceptions.Rows.Count > 0 Then
            cmdEInvoiceExceptionReport.Enabled = True
        Else
            cmdEInvoiceExceptionReport.Enabled = False
        End If

        UltraPanel_ResolutionCodes.Hide()
        UltraPanel_ResolutionCodes.SendToBack()

        UltraPanel_SavingChanges.Hide()
        UltraPanel_SavingChanges.SendToBack()

        LoadResolutionCodes()
        ComboBox_ResolutionCodesCopy.SelectedIndex = -1

        CType(UltraToolbarsManager1.Tools("ComboBox_GridFilter"), Infragistics.Win.UltraWinToolbars.ComboBoxTool).SelectedIndex = 2
        changedRows.Clear()

        If Me.IsPayByAgreedCost Then
            UltraToolbarsManager1.Tools("Pay By Invoice").SharedProps.Enabled = True
            UltraToolbarsManager1.Tools("Pay By PO").SharedProps.Enabled = True
        Else
            UltraToolbarsManager1.Tools("Pay By Invoice").SharedProps.Enabled = True
            UltraToolbarsManager1.Tools("Pay By PO").SharedProps.Enabled = False
        End If
    End Sub

    Private Sub DisplayPO()
        If grdLineItems.ActiveRow Is Nothing Then
            If grdLineItems.Selected.Rows.Count > 0 Then
                If grdLineItems.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(grdLineItems.Selected.Rows(0).Cells("OrderHeader_ID").Value)
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If grdLineItems.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(grdLineItems.ActiveRow.Cells("OrderHeader_ID").Value)
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        bSpecificOrder = True

        frmOrders.ShowDialog()
        frmOrders.Dispose()

        bSpecificOrder = False
    End Sub

    Private Sub DisplayInvoice()
        Dim iEInvoiceID As Integer
        Dim iStoreNo As Integer

        If Me.EInvoicedOrder Then
            If grdLineItems.ActiveRow Is Nothing Then
                If grdLineItems.Selected.Rows.Count > 0 Then
                    If grdLineItems.Selected.Rows(0).IsDataRow Then
                        iEInvoiceID = CInt(grdLineItems.Selected.Rows(0).Cells("Einvoice_Id").Value)
                        iStoreNo = CInt(grdLineItems.Selected.Rows(0).Cells("Store_No").Value)
                    Else
                        MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                If grdLineItems.ActiveRow.IsDataRow Then
                    iEInvoiceID = CInt(grdLineItems.ActiveRow.Cells("Einvoice_Id").Value)
                    iStoreNo = CInt(grdLineItems.ActiveRow.Cells("Store_No").Value)
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            End If

            EInvoiceHTMLDisplay.EinvoiceId = iEInvoiceID
            EInvoiceHTMLDisplay.StoreNo = iStoreNo
            EInvoiceHTMLDisplay.ShowDialog()
        Else
            glOrderHeaderID = Me.POOrderHeaderId
            frmOrderStatus.ShowDialog()
            frmOrderStatus.Close()
            frmOrderStatus.Dispose()
        End If
    End Sub

    Private Sub DisplayVendor()
        glVendorID = Me.VendorId
        bSpecificVendor = True

        frmVendor.ShowDialog()
        frmVendor.Dispose()

        bSpecificVendor = False
    End Sub

    Private Sub DisplayVendorItemCost()
        Dim itemDesc As String

        If grdLineItems.ActiveRow.IsDataRow Then
            grdLineItems.ActiveRow.Selected = True
        Else
            Exit Sub
        End If

        If grdLineItems.ActiveRow Is Nothing Then
            If grdLineItems.Selected.Rows.Count > 0 Then
                If grdLineItems.Selected.Rows(0).IsDataRow Then
                    glItemID = CInt(grdLineItems.Selected.Rows(0).Cells("Item_Key").Value)
                    itemDesc = grdLineItems.Selected.Rows(0).Cells("Item_Description").Value
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If grdLineItems.ActiveRow.IsDataRow Then
                glItemID = CInt(grdLineItems.ActiveRow.Cells("Item_Key").Value)
                itemDesc = grdLineItems.Selected.Rows(0).Cells("Item_Description").Value
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim fVendorCost As New frmVendorCost
        Dim vendorInfo As New VendorBO
        Dim lOldStoreId As Integer

        lOldStoreId = glStoreID
        glStoreID = CInt(grdLineItems.Selected.Rows(0).Cells("Store_No").Value)

        bSpecificStoreItemVendorCost = True

        vendorInfo.VendorID = CInt(grdLineItems.Selected.Rows(0).Cells("Vendor_ID").Value)
        vendorInfo.VendorName = grdLineItems.Selected.Rows(0).Cells("CompanyName").Value

        fVendorCost.poVendorInfo = vendorInfo
        fVendorCost.plItem_Key = glItemID
        fVendorCost.ShowDialog()

        glStoreID = lOldStoreId
        bSpecificStoreItemVendorCost = False

    End Sub

    Private Sub DisplayItem()
        If grdLineItems.ActiveRow Is Nothing Then
            If grdLineItems.Selected.Rows.Count > 0 Then
                If grdLineItems.Selected.Rows(0).IsDataRow Then
                    glItemID = CInt(grdLineItems.Selected.Rows(0).Cells("Item_Key").Value)
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If grdLineItems.ActiveRow.IsDataRow Then
                glItemID = CInt(grdLineItems.ActiveRow.Cells("Item_Key").Value)
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If
        frmItem.ShowDialog()
        frmItem.Dispose()
    End Sub

    Private Sub DisplayOrderItem()
        If grdLineItems.ActiveRow.IsDataRow Then
            grdLineItems.ActiveRow.Selected = True
        Else
            Exit Sub
        End If

        If grdLineItems.Selected.Rows.Count = 1 Then
            glItemID = 0
            glOrderHeaderID = Me.POOrderHeaderId
            glOrderItemID = CInt(grdLineItems.Selected.Rows(0).Cells("OrderItem_ID").Value)

            frmOrdersItem.ShowDialog()
            frmOrdersItem.Close()
            frmOrdersItem.Dispose()

            glOrderItemID = -1
            glItemID = -1
        Else
            MsgBox("You must select a line item to edit.", MsgBoxStyle.Exclamation, "Notice!")
        End If
    End Sub

    Private Sub ExportToExcel()
        Dim dlg As SaveFileDialog = New SaveFileDialog()

        dlg.Filter = "(Excel files)|*.xls"
        dlg.Title = "Enter destination Excel filename"

        Dim res As DialogResult = dlg.ShowDialog()
        Dim str As String = dlg.FileName

        If str <> String.Empty Then
            UltraGridExcelExporter1.Export(grdLineItems, str)
        End If
    End Sub

    Private Sub EmailPOCreator()
        Dim SendMailClient As SmtpClient
        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage

        Dim currentUser As String

        If grdLineItems.ActiveRow Is Nothing Then
            If grdLineItems.Selected.Rows.Count > 0 Then
                If grdLineItems.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(grdLineItems.Selected.Rows(0).Cells("OrderHeader_ID").Value)
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If grdLineItems.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(grdLineItems.ActiveRow.Cells("OrderHeader_ID").Value)
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim u As UserBO = New UserBO(POCreator)
        If u.Email = vbNullString Or Trim(u.Email) = String.Empty Then
            MsgBox("IRMA does not have PO creator's email is the database", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        emailTo = u.Email

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = "Hello " + u.FullName + ": " + vbCrLf + _
                    currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes for the following items:" + vbCrLf + _
                    GetLineItemAdminNotes() + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + u.FullName, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub

    Private Sub EmailPOCloser()
        Dim SendMailClient As SmtpClient

        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage
        Dim currentUser As String

        If grdLineItems.ActiveRow Is Nothing Then
            If grdLineItems.Selected.Rows.Count > 0 Then
                If grdLineItems.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(grdLineItems.Selected.Rows(0).Cells("OrderHeader_ID").Value)
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If grdLineItems.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(grdLineItems.ActiveRow.Cells("OrderHeader_ID").Value)
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim u As UserBO = New UserBO(POCloser)
        If u.Email = vbNullString Or Trim(u.Email) = String.Empty Then
            MsgBox("IRMA does not have PO closer's email is the database", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        emailTo = u.Email

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = "Hello " + u.FullName + ": " + vbCrLf + _
                    currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes for the following line items:" + vbCrLf + _
                    GetLineItemAdminNotes() + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + u.FullName, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub

    Private Sub grdLineItems_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles grdLineItems.BeforeCellUpdate
        If e.Cell.Band.Index = 1 Then Exit Sub

        If e.Cell.Column.Key = "AdminNotes" Then

            If e.NewValue Is System.DBNull.Value Then
                e.Cancel = True
            End If
        End If

        If e.Cancel = False Then
            If e.Cell.OriginalValue Is Nothing And e.NewValue IsNot Nothing Then

                changedRows.Add(e.Cell.Row)
            Else
                If Not e.Cell.OriginalValue.Equals(e.NewValue) Then
                    If Not changedRows.Contains(e.Cell.Row) Then
                        changedRows.Add(e.Cell.Row)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub grdLineItems_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdLineItems.InitializeLayout
        FormatDataGrid()
    End Sub

    Private Sub grdLineItems_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles grdLineItems.MouseDown
        Dim mousePoint As New Point(e.X, e.Y)
        Dim element As UIElement = DirectCast(sender, UltraGrid).DisplayLayout.UIElement.ElementFromPoint(mousePoint)
        Dim cell As UltraGridCell = TryCast(element.GetContext(GetType(UltraGridCell)), UltraGridCell)

        If Not cell Is Nothing Then
            'don't clear the active row if the resolution drop down is clicked as this will prevent the dropdown from showing
            If Not cell.Column.Key.Equals("ResolutionCode") Then
                grdLineItems.Selected.Rows.Clear()
                grdLineItems.ActiveRow = Nothing
                cell.Row.Activate()
            End If

            If e.Button = MouseButtons.Left Then
                If cell.Column.Key.Equals("InvoiceNumber") Then
                    DisplayInvoice()
                ElseIf cell.Column.Key.Equals("OrderHeader_ID") Then
                    DisplayPO()
                ElseIf cell.Column.Key.Equals("CompanyName") Then
                    DisplayVendor()
                ElseIf cell.Column.Key.Equals("Identifier") Then
                    DisplayItem()
                ElseIf cell.Column.Key.Equals("AdminNotes") Then
                    DisplayAdminNotes()
                End If
            ElseIf e.Button = MouseButtons.Right Then
                If cell.Column.Key.Equals("CompanyName") Then
                    DisplayVendorItemCost()
                ElseIf cell.Column.Key.Equals("Identifier") Then
                    DisplayOrderItem()
                End If
            End If
        End If
    End Sub

    Private Sub grdLineItems_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles grdLineItems.InitializeRow
        Dim OrderingDAO As New OrderingDAO
        Dim a As UltraGridCell = Nothing
        Dim b As UltraGridCell = Nothing
        Dim c As UltraGridCell = Nothing
        Dim problemColor As Color = Color.Tomato

        a = e.Row.Cells("Einvoice_Cost")
        b = e.Row.Cells("POEffectiveCost")
        c = e.Row.Cells("PO_Adjusted_Cost")

        If Me.EInvoicedOrder And Not a.Value.Equals(b.Value) Then
            a.Appearance.BackColor = problemColor
            b.Appearance.BackColor = problemColor
        End If

        If Me.EInvoicedOrder And Not a.Equals(c.Value) And Not c.Value.Equals(String.Empty) And c.Value > 0 Then
            a.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If

        If Not b.Value.Equals(c.Value) And Not c.Value.Equals(String.Empty) And c.Value > 0 Then
            b.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If

        '############################################################################################################

        a = e.Row.Cells("Vendor_Order_UOM")
        b = e.Row.Cells("Cost_UOM")
        c = e.Row.Cells("eInvoice_UOM")

        If Not a.Value.Equals(b.Value) Then
            a.Appearance.BackColor = problemColor
            b.Appearance.BackColor = problemColor
        End If

        If Me.EInvoicedOrder And Not a.Value.Equals(c.Value) Then
            a.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If


        If Me.EInvoicedOrder And Not b.Value.Equals(c.Value) Then
            b.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If


        '############################################################################################################


        a = e.Row.Cells("einvoice_case_pack")
        b = e.Row.Cells("Case_Pack")
        c = Nothing

        If Me.EInvoicedOrder And Not a.Value.Equals(b.Value) Then
            a.Appearance.BackColor = problemColor
            b.Appearance.BackColor = problemColor
        End If

        '############################################################################################################

        a = e.Row.Cells("Ordered_quantity")
        b = e.Row.Cells("Received_Quantity")
        c = e.Row.Cells("einvoice_Quantity")

        If Not a.Value.Equals(b.Value) Then
            a.Appearance.BackColor = problemColor
            b.Appearance.BackColor = problemColor
        End If

        If Me.EInvoicedOrder And Not a.Value.Equals(c.Value) Then
            a.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If

        If Me.EInvoicedOrder And Not b.Value.Equals(c.Value) Then
            b.Appearance.BackColor = problemColor
            c.Appearance.BackColor = problemColor
        End If
    End Sub

    Private Sub UltraToolbarsManager1_ToolClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles UltraToolbarsManager1.ToolClick
        Select Case e.Tool.Key
            Case "Export Excel"
                ExportToExcel()

            Case "Email PO Creator"
                EmailPOCreator()

            Case "Email PO Closer"
                EmailPOCloser()

            Case "Email Other"
                ' Do nothing here. Handled in UltraTextEditor1_EditorButtonClick

            Case "Pay By PO"
                SaveData()
                PayByPO()

            Case "Pay By Invoice"
                SaveData()
                PayByInvoice()

            Case "SaveChanges"
                SaveData()
                LoadDataTable()
        End Select

    End Sub

    Private Sub UltraGridExcelExporter1_BeginExport(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.ExcelExport.BeginExportEventArgs) Handles UltraGridExcelExporter1.BeginExport
        Dim column As UltraWinGrid.UltraGridColumn

        For Each column In e.Layout.Bands(0).Columns
            column.Hidden = False
        Next
    End Sub

    Private Sub ApplyGridFilter(ByVal Filter As String)
        With grdLineItems.DisplayLayout.Bands(0)
            .ColumnFilters.ClearAllFilters() ' clear all filters to start with a clean slate. 
            Select Case Filter.ToLower()
                Case "all"
                    'do nothing so all rows show
                Case "resolved"
                    .ColumnFilters("ApprovedByUserId").FilterConditions.Add(FilterComparisionOperator.GreaterThan, 0)
                Case "unresolved"
                    .ColumnFilters("LineItemSuspended").FilterConditions.Add(FilterComparisionOperator.Match, True)
            End Select
        End With

        If Not IsNothing(grdLineItems.Rows.GetRowAtVisibleIndex(0)) Then
            grdLineItems.ActiveRow = grdLineItems.Rows.GetRowAtVisibleIndex(0)
        End If
    End Sub

    Private Sub UltraToolbarsManager1_ToolValueChanged(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolEventArgs) Handles UltraToolbarsManager1.ToolValueChanged
        ' when user changes the filter dropdown. Apply chosen filter.
        Dim cb As Infragistics.Win.UltraWinToolbars.ComboBoxTool
        cb = e.Tool

        If grdLineItems.DisplayLayout.Rows.Count > 0 Then
            If e.Tool.Key.Equals("ComboBox_GridFilter") Then
                ApplyGridFilter(cb.SelectedItem.ToString())
            End If
        End If
    End Sub

    Private Sub DisplayAdminNotes()
        Dim frm As New frmOrdersDesc

        frm.blnIsFromSuspendedPOTool = True
        frm.iOrderHeaderID = 0
        frm.iOrderItemID = grdLineItems.ActiveRow.Cells("OrderItem_ID").Value

        frm.ShowDialog()

        grdLineItems.ActiveRow.Cells("AdminNotes").Value = frm.txtField.Text

        frm.Close()
        frm.Dispose()
        SaveData()
    End Sub

    Private Sub grdEInvoiceExceptions_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdEInvoiceExceptions.InitializeLayout
        FormatExceptionsDataGrid()
    End Sub

    Private Sub cmdEInvoiceExceptionReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEInvoiceExceptionReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim sReportingServicesURL As String = ConfigurationServices.AppSettings("reportingServicesURL")
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        filename = "eInvoiceExceptionReport"
        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If glOrderHeaderID > 0 Then
            sReportURL.Append("&OrderHeader_ID=" & Me.POOrderHeaderId)
        End If

        Process.Start(sReportingServicesURL & sReportURL.ToString())
    End Sub

    Private Sub PayByPO()
        logger.Debug("PayByAgreedCost entry")
        Dim row As UltraGridRow
        Dim currentOrderItemID As Integer
        Dim resolutionDesc As String = String.Empty
        Dim iResolutionId As Integer

        ' set the value of callingAction to the AddressOf the PayByAgreedCost Sub Routine. So we know where the call came from and can call it again later. The magic of delegates.
        callingAction = AddressOf PayByPO

        If grdLineItems.Selected.Rows.Count >= 1 Then
            If ValidateResolutionCodes() Then
                If MsgBox("The selected line item(s) will be paid according to the PO costs.", MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    For Each row In grdLineItems.Selected.Rows
                        currentOrderItemID = CInt(row.Cells("OrderItem_ID").Value)
                        resolutionDesc = row.Cells("ResolutionCode").Text
                        If Integer.TryParse(row.Cells("ResolutionCode").Value, iResolutionId) = False Then
                            If ComboBox_ResolutionCodesCopy.SelectedItem IsNot Nothing Then
                                iResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                            End If
                        End If
                        logger.Info("btnPayByPO_Click - approving the selected invoice using the Pay by PO option: OrderItem_ID=" + currentOrderItemID.ToString)
                        OrderingDAO.ApproveLineItem(currentOrderItemID, iResolutionId, False, 1)
                    Next

                    LoadDataTable()
                Else
                    logger.Info("btnPayByPO_Click - user elected not to pay the selected invoices using the Pay by PO option after the confirmation was presented")
                End If
            End If
        ElseIf grdLineItems.ActiveRow.IsDataRow Then
            grdLineItems.Selected.Rows.Add(grdLineItems.ActiveRow)

            If ValidateResolutionCodes() Then
                If MsgBox("The selected line item(s) will be paid according to the PO costs.", MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    currentOrderItemID = CInt(grdLineItems.ActiveRow.Cells("OrderItem_ID").Value)
                    resolutionDesc = grdLineItems.ActiveRow.Cells("ResolutionCode").Text

                    If Integer.TryParse(grdLineItems.ActiveRow.Cells("ResolutionCode").Value, iResolutionId) = False Then
                        iResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                    End If

                    logger.Info("PayByAgreedCost - approving the selected invoice using the Pay by PO option: OrderItem_ID=" + currentOrderItemID.ToString)
                    OrderingDAO.ApproveLineItem(currentOrderItemID, iResolutionId, False, 1)

                    LoadDataTable()
                Else
                    logger.Info("PayByAgreedCost - user elected not to pay the selected invoices using the Pay by PO option after the confirmation was presented")
                End If
            End If
        Else
            ' Alert the user that they must select at least one invoice
            MsgBox("An invoice from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("PayByAgreedCost exit")
    End Sub

    Private Sub PayByInvoice()
        logger.Debug("PayByInvoice entry")

        Dim row As UltraGridRow
        Dim iOrderItemID As Integer
        Dim resolutionDesc As String = String.Empty
        Dim ResolutionId As Integer

        ' set the value of callingAction to the AddressOf the PayByInvoice Sub Routine. So we know where the call came from and can call it again later. The magic of delegates.
        callingAction = AddressOf PayByInvoice

        If grdLineItems.Selected.Rows.Count >= 1 Then
            If ValidateResolutionCodes() Then

                If MsgBox("The selected line item(s) will be paid according to the invoice costs.", MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    For Each row In grdLineItems.Selected.Rows
                        iOrderItemID = CInt(row.Cells("OrderItem_ID").Value)
                        resolutionDesc = row.Cells("ResolutionCode").Text
                        If Integer.TryParse(row.Cells("ResolutionCode").Value, ResolutionId) = False Then
                            If ComboBox_ResolutionCodesCopy.SelectedItem IsNot Nothing Then
                                ResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                            End If
                        End If

                        logger.Info("PayByInvoice - approving the selected invoice using the Pay by Invoice option: OrderItem_ID=" + iOrderItemID.ToString)
                        OrderingDAO.ApproveLineItem(iOrderItemID, ResolutionId, True, 0)
                    Next

                    LoadDataTable()
                Else
                    logger.Info("btnPayByInvoice_Click - user elected not to pay the selected invoices using the Pay by Invoice option after the confirmation was presented")
                End If
            End If
        ElseIf grdLineItems.ActiveRow.IsDataRow Then
            grdLineItems.Selected.Rows.Add(grdLineItems.ActiveRow)
            If ValidateResolutionCodes() Then
                If MsgBox("The selected line item(s) will be paid according to the invoice costs.", MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    iOrderItemID = CInt(grdLineItems.ActiveRow.Cells("OrderItem_ID").Value)
                    resolutionDesc = grdLineItems.ActiveRow.Cells("ResolutionCode").Text

                    If Integer.TryParse(grdLineItems.ActiveRow.Cells("ResolutionCode").Value, ResolutionId) = False Then
                        ResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                    End If

                    logger.Info("PayByInvoice - approving the selected invoice using the Pay by Invoice option: orderHeader_ID=" + iOrderItemID.ToString)

                    OrderingDAO.ApproveLineItem(iOrderItemID, ResolutionId, True, 0)
                    LoadDataTable()
                Else
                    logger.Info("PayByInvoice - user elected not to pay the selected invoices using the Pay by Invoice option after the confirmation was presented")
                End If
            End If
        Else
            MsgBox("An invoice from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("PayByInvoice exit")
    End Sub

    Private Function ValidateResolutionCodes() As Boolean
        Dim blnAllCodesValid As Boolean = True

        'loop through selected and make sure they all have ResonCodes.
        For Each ugr As UltraGridRow In grdLineItems.Selected.Rows
            If ugr.Cells("ResolutionCode").Text.Equals(String.Empty) Then
                blnAllCodesValid = False
            End If
        Next

        'if they dont, prompt the user for one. 
        If Not blnAllCodesValid Then ShowResolutionCodePopup()

        Return blnAllCodesValid
    End Function

    Private Sub ShowResolutionCodePopup()
        ComboBox_ResolutionCodesCopy.SelectedItem = Nothing
        UltraPanel_ResolutionCodes.BringToFront()
        UltraPanel_ResolutionCodes.Show()
    End Sub

    Private Sub UltraButton_ApplyResolutionCodes_Apply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraButton_ApplyResolutionCodes_Apply.Click
        If Not ComboBox_ResolutionCodesCopy.SelectedItem Is Nothing Then
            'hide the popup panel.
            UltraPanel_ResolutionCodes.Hide()
            UltraPanel_ResolutionCodes.SendToBack()

            ApplyChosenResolutionCode()

            'at this point callingAction will be AddressOf PayByAgreedCost or PayByInvoice (whichever one was called.) Since changes have been made. Lets call it again! 
            SaveData()
            callingAction()
        Else
            ComboBox_ResolutionCodesCopy.Focus()
        End If
    End Sub

    Private Sub ApplyChosenResolutionCode()
        ' apply the chosen Resolution Code to every row that we have selected.
        Dim resolution As String = ComboBox_ResolutionCodesCopy.SelectedItem.ToString()

        For Each ugr As UltraGridRow In grdLineItems.Selected.Rows
            If ugr.Cells("ResolutionCode").Text.Equals(String.Empty) Then
                ugr.Cells("ResolutionCode").Value = resolution
            End If
        Next
    End Sub

    Private Sub UltraButton_ApplyResolutionCodes_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton_ApplyResolutionCodes_Cancel.Click
        UltraPanel_ResolutionCodes.Hide()
        UltraPanel_ResolutionCodes.SendToBack()
    End Sub

    Private Function AllLineItemsResolved() As Boolean
        Dim blnResolved As Boolean = True

        For Each dr As UltraGridRow In grdLineItems.Rows.GetAllNonGroupByRows()
            If dr.Cells("LineItemSuspended").Value Then
                blnResolved = False
            End If
        Next

        Return blnResolved
    End Function

    Private Sub SendEmailOther(ByVal emailaddress As String)
        Dim currentGrid As UltraGrid
        Dim SendMailClient As SmtpClient
        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage
        Dim currentUser As String
        Dim notes As String

        currentGrid = grdLineItems

        If currentGrid.ActiveRow Is Nothing Then
            If currentGrid.Selected.Rows.Count > 0 Then
                If currentGrid.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(currentGrid.Selected.Rows(0).Cells("OrderHeaderID").Value)
                    notes = ""
                    If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                        notes = currentGrid.Selected.Rows(0).Cells("POAdminNotes").Value
                    End If
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If currentGrid.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(currentGrid.ActiveRow.Cells("OrderHeader_ID").Value)
                notes = ""
                If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                    notes = currentGrid.ActiveRow.Cells("POAdminNotes").Value
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        emailTo = emailaddress

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes for the following items:" + vbCrLf + _
                    GetLineItemAdminNotes() + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + emailTo, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub

    Private Sub uteEmailOther_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles uteEmailOther.EditorButtonClick
        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool

        If e.Button.Key = "SendEmailOther" Then

            'make sure its a valid email address.
            Dim validation As Infragistics.Win.Misc.Validation = Me.UltraValidatorEmail.Validate(uteEmailOther)

            'if validated send email. 
            If validation Is Nothing Then
                SendEmailOther(e.Button.Editor.Value)

                'close the popup after email is sent.
                _popup = UltraToolbarsManager1.Toolbars(0).Tools("Email Other")
                _popup.ClosePopup()
            End If
        End If
    End Sub

    Private Sub grdLineItems_MouseEnterElement(ByVal sender As Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles grdLineItems.MouseEnterElement
        Dim acell As UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))

        If Not acell Is Nothing Then
            If Not acell.Tag Is Nothing Then
                ToolTip1.Active = True
                ToolTip1.SetToolTip(sender, acell.Tag.ToString())
            Else
                If Not acell.Column.Tag Is Nothing Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(sender, acell.Column.Tag.ToString())
                End If
            End If
        End If
    End Sub

    Private Sub grdLineItems_MouseLeaveElement(ByVal sender As Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles grdLineItems.MouseLeaveElement
        Dim acell As UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))

        If Not acell Is Nothing Then
            ToolTip1.Active = False
        End If
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        Try
            OrderingDAO.ApproveInvoiceFromLineItem(Me.POOrderHeaderId)
            SuspendedPOTool.blnPOApprovedByLineItem = True

            'Populate the OrderedCost for the newly created PO
            SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", Me.POOrderHeaderId), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Catch ex As Exception
            SuspendedPOTool.blnPOApprovedByLineItem = False
            MsgBox("PO not approved - " & ex.Message, MsgBoxStyle.Critical, "PO Approval Error")
        End Try

        Me.Close()
    End Sub

    Private Sub SaveData()
        Dim sNotes As String = String.Empty
        Dim iResolutionCodeID As Integer = Nothing
        Dim iOrderItemID As Integer = Nothing

        grdLineItems.UpdateData()

        If changedRows.Count > 0 Then
            With UltraProgressBar_SavingChanges
                .Minimum = 0
                .Maximum = changedRows.Count
                .Value = .Minimum
                UltraLabel_SaveChangesCount.Text = String.Format("{0} of {1}", .Value + 1, .Maximum)
                UltraLabel_SaveChangesCount.Refresh()
                ShowSaveChangesPopup()

                For Each row As UltraGridRow In changedRows
                    If row.Cells("AdminNotes").Value IsNot Nothing Or _
                            row.Cells("ResolutionCode").Value.ToString <> String.Empty Then

                        iOrderItemID = row.Cells("OrderItem_ID").Value
                        If row.Cells("AdminNotes").Value IsNot Nothing Then
                            sNotes = row.Cells("AdminNotes").Value.ToString.Replace("'", "''")
                        Else
                            sNotes = String.Empty
                        End If

                        If Integer.TryParse(row.Cells("ResolutionCode").Value.ToString, iResolutionCodeID) = False Then
                            If ComboBox_ResolutionCodesCopy.SelectedItem IsNot Nothing Then
                                iResolutionCodeID = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                            End If
                        End If

                        OrderingDAO.UpdateSuspendedPONotes(0, iOrderItemID, sNotes, iResolutionCodeID)
                    End If
                    .IncrementValue(1)
                    Thread.Sleep(200)

                    UltraLabel_SaveChangesCount.Text = String.Format("{0} of {1}", .Value, .Maximum)
                    UltraLabel_SaveChangesCount.Refresh()
                    .Refresh()
                Next

                changedRows.Clear()
                UltraPanel_SavingChanges.SendToBack()
                UltraPanel_SavingChanges.Hide()
            End With
        End If
    End Sub

    Private Sub ShowSaveChangesPopup()
        UltraPanel_SavingChanges.BringToFront()
        UltraPanel_SavingChanges.Show()
        UltraPanel_SavingChanges.Refresh()
    End Sub

    Private Function GetResolutionCodeValueList() As ValueList
        Dim dt As DataTable = InvoiceMatchingDAO.GetResolutionCodes("SP", 0)
        Dim vl As New ValueList

        For Each dr As DataRow In dt.Rows
            vl.ValueListItems.Add(dr("ReasonCodeDetailId").ToString, dr("ReasonCodeDesc").ToString)
        Next

        Return vl
    End Function

    Public Sub LoadResolutionCodes()
        Dim dt As DataTable = InvoiceMatchingDAO.GetResolutionCodes("SP", 0)

        For Each dr As DataRow In dt.Rows
            ComboBox_ResolutionCodesCopy.Items.Add(New VB6.ListBoxItem(dr("ReasonCodeDesc"), dr("ReasonCodeDetailId")))
        Next
    End Sub

    Private Function GetLineItemCostWithDiscount(ByVal iDiscountType As Integer, ByVal dQuantityDiscount As Decimal, ByVal iQuantityOrdered As Integer, ByVal dLineItemCost As Decimal, ByVal dLineItemFreight As Decimal) As Decimal
        Dim dcost As Decimal = 0

        Select Case iDiscountType
            Case 0 'No discount
                dcost = dLineItemCost
            Case 1 'Cash discount
                dcost = dLineItemCost - (dQuantityDiscount / iQuantityOrdered)
            Case 2 'Percent discount
                dcost = dLineItemCost * ((100 - dQuantityDiscount) / 100)
            Case 3 'Free Items
                dcost = (dLineItemCost * (iQuantityOrdered - dQuantityDiscount)) / iQuantityOrdered
        End Select

        Return dcost
    End Function

    Private Function GetLineItemAdminNotes() As String
        Dim sResult As String = ""

        For Each dr As UltraGridRow In grdLineItems.Rows
            If dr.Cells("AdminNotes").Value.ToString <> "" Then
                sResult = sResult & dr.Cells("Identifier").Value & " - " & dr.Cells("AdminNotes").Value & vbCrLf
            End If
        Next

        Return sResult
    End Function

    Private Sub uteEmailOther_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles uteEmailOther.KeyDown
        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool

        If e.KeyCode = Keys.Enter Then
            'make sure its a valid email address.
            Dim validation As Infragistics.Win.Misc.Validation = Me.UltraValidatorEmail.Validate(uteEmailOther)

            'if validated send email. 
            If validation Is Nothing Then
                SendEmailOther(uteEmailOther.Text)

                'close the popup after email is sent.
                _popup = UltraToolbarsManager1.Toolbars(0).Tools("Email Other")
                _popup.ClosePopup()
            End If
        End If
    End Sub



    Private Sub SaveDisplayLayout()
        If Not System.IO.Directory.Exists(gridLayoutFilePath) Then
            System.IO.Directory.CreateDirectory(gridLayoutFilePath)
        End If

        Try
            grdLineItems.DisplayLayout.SaveAsXml(gridLayoutFilePath + gridLayoutFileName)
        Catch ex As Exception
            logger.Debug(String.Format("Error saving UltraGrid layout file: {0}", ex.Message))
        End Try
    End Sub

    Private Sub LoadDisplayLayout()
        Try
            grdLineItems.DisplayLayout.LoadFromXml(gridLayoutFilePath + gridLayoutFileName)
        Catch ex As Exception
            logger.Debug(String.Format("Unable to load the UltraGrid layout file.  Proceeding with default layout.  Error: {0}", ex.Message))
        End Try
    End Sub

    Private Sub frmLineItems_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        SaveDisplayLayout()
    End Sub
End Class