Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility
Imports System.Text


Friend Class frmReceivingList
    Inherits System.Windows.Forms.Form

    Private dtOrderItems As New DataTable
    Private dtOrderItemsNOIDNORD As New DataTable
    Private dtInfo As New DataTable
    Private RListDAO As New ReceivingListDAO
    Private EInvoice_Id As Integer
    Private OpenPO As Integer
    Private pbGridCtrlKey As Boolean
    Private piInsertOrder() As Short
    Private plStartOrderItem_ID As Integer
    Private m_bDataChanged As Boolean
    Private m_bProcessing As Boolean
	Private m_bHaveSomeReceived As Boolean
	Private bAtLeastOneReceived As Boolean
	Private bTransferOrder As Boolean
	Private bReturnOrder As Boolean
	Private itemsWithReasonCodes As Boolean
	Private bHideShipped As Boolean = True
	Private m_OrderHeaderId As Integer = 0
	Private m_TransferFromSubTeamNo As Integer = 0
	Private m_EInvoicingId As Integer = 0
	Private dtCachedRows As New DataTable
	Private drCachedRows As DataRow
	Private m_bAllowReceiveAll As Boolean

	Private OBCode As Integer

	' A flag to keep track of the update action to prevent an infinite loop
	Private updateReceivedValue As Boolean = False

	Private Const iALL As Short = 0
	Private Const iNONE As Short = 1
	Dim optNames As String
	Dim bCheckIfNo As Boolean = False

	Private OrderFuncs As OrderingFunctions = New WholeFoods.IRMA.Ordering.BusinessLogic.OrderingFunctions

	' Define the log4net logger for this class.
	Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Public Property OrderHeaderId() As Integer
		Get
			OrderHeaderId = m_OrderHeaderId
		End Get
		Set(ByVal value As Integer)
			m_OrderHeaderId = value
		End Set
	End Property

	Public Property TransferFromSubTeamNo() As Integer
		Get
			TransferFromSubTeamNo = m_TransferFromSubTeamNo
		End Get
		Set(ByVal value As Integer)
			m_TransferFromSubTeamNo = value
		End Set
	End Property

	Public Property EInvoicingId() As Integer
		Get
			EInvoicingId = m_EInvoicingId
		End Get
		Set(ByVal value As Integer)
			m_EInvoicingId = value
		End Set
	End Property

	Private Sub RefreshPartialShipmentCheckbox()

		Dim isEnabled As Boolean = OrderingDAO.AnyOrderItemReceived(glOrderHeaderID)

		If isEnabled And Not chkPastReceipt.Checked Then
			partialShippmentCheck.Enabled = True
		Else
			partialShippmentCheck.Enabled = False
			partialShippmentCheck.Checked = False
		End If

	End Sub

	Private Sub frmReceivingList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		'20090709 - Dave Stacey - TFS 10387
		Me.WindowState = FormWindowState.Maximized
		logger.Debug("frmReceivingList_Load Entry")

		'DN instantiate our Private ReceivingListDAO > RListDAO
		RListDAO = New ReceivingListDAO

		'Show invoice method in window title.
		Me.Text += " " & frmOrders.GetInvoiceMethodDisplayText(glOrderHeaderID, frmOrders.IsOrderPayAgreedCost, EInvoicingId)
		SetActive(cmdReceive, gbDistributor)

		CenterForm(Me)

		cmdSelectAll.Image = imgIcons.Images.Item(iALL)
		ToolTip1.SetToolTip(cmdSelectAll, "Select All")

		bTransferOrder = False
		bReturnOrder = False

		dtInfo = RListDAO.GetOrderReceivingDisplayInfo(glOrderHeaderID)
		If dtInfo.Rows.Count > 0 Then
			txtOrderHeader_ID.Text = dtInfo.Rows(0).Item("OrderHeader_ID")
			txtStoreName.Text = dtInfo.Rows(0).Item("Store_Name")
			txtTotalOrderCost.Text = Format(CDec(dtInfo.Rows(0).Item("OrderedCost")), "##,###0.00##")

			'MD 1/25/2010: Bug 13770: Effective date was null for Distribution Credit PO's added a check for null there
			If dtInfo.Rows(0).Item("Expected_Date") IsNot DBNull.Value Then
				txtExpectedDate.Text = dtInfo.Rows(0).Item("Expected_Date")
			Else
				txtExpectedDate.Text = System.DateTime.Today
			End If
			txtSubteamName.Text = dtInfo.Rows(0).Item("Subteam_Name")
			If dtInfo.Rows(0).Item("QtyShippedProvided") IsNot DBNull.Value AndAlso dtInfo.Rows(0).Item("QtyShippedProvided") = True Then
				bHideShipped = False
			End If
			If dtInfo.Rows(0).Item("EInvoice_Id") IsNot DBNull.Value AndAlso dtInfo.Rows(0).Item("OpenPO") = 1 And (gbSuperUser = True Or gbDistributor = True Or gbEInvoicing = True) Then
				EInvoice_Id = dtInfo.Rows(0).Item("EInvoice_Id")
				Me.cmdReparseEInvoice.Enabled = True
			Else
				EInvoice_Id = -1
				Me.cmdReparseEInvoice.Enabled = False
			End If

			'Hide the NOID/NORD grid if EInvoice information is not avilable. 
			grdNOIDNORD.Visible = (dtInfo.Rows(0).Item("EInvoice_Id") IsNot DBNull.Value)

			'If PastReceiptDate is ever entered into the DB, it cannot be changed
			If dtInfo.Rows(0).Item("PastReceiptDate") IsNot DBNull.Value Then
				chkPastReceipt.Checked = True
			End If
			dtpPastReceiptDate.Value = dtInfo.Rows(0).Item("PastReceiptDate")
			chkPastReceipt.Enabled = False
			dtpPastReceiptDate.Enabled = False

			If dtInfo.Rows(0).Item("OrderType_ID") = 3 Then
				bTransferOrder = True
			End If

			bReturnOrder = dtInfo.Rows(0).Item("Return_Order")
			Me.optExceptions.Enabled = grdNOIDNORD.Visible
		End If

		initGrid(Me.grdRL)
		initGrid(Me.grdNOIDNORD, True)


		Call PopGrid()

		If dtOrderItemsNOIDNORD.Rows.Count > 0 Then
			Me.cmdNOIDNORDReport.Enabled = True
		Else
			Me.cmdNOIDNORDReport.Enabled = False
		End If

		LoadReceivingDiscrepanyReasonCodes()
		InitializeCacheDataTable()

		'Check receiving rules to enable or disable Select All/Receive All functionality
		'   >> Receive All will be disabled for non-credit Purchase orders only 
		If geOrderType = enumOrderType.Purchase And frmOrders.IsCredit = False And frmOrders.AllowReceiveAll = False Then
			m_bAllowReceiveAll = False
		Else
			m_bAllowReceiveAll = True
		End If



		logger.Debug("frmReceivingList_Load Exit")

	End Sub

	Private Sub cmdSelectAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelectAll.Click

		logger.Debug("cmdSelectAll_Click Entry")
		Dim gridRow As UltraGridRow

		If cmdSelectAll.Tag = "Select" Then

			' Check receiving rules and determine if the user must manually enter a received quantity before selecting the row.
			'   >> Activate highlighting if Received cell is empty.
			If m_bAllowReceiveAll = False And optNotReceived.Checked Then
				For Each gridRow In grdRL.Rows
					If (IsDBNull(gridRow.GetCellValue("Received")) Or IsNothing(gridRow.GetCellValue("Received"))) Then
						Dim cell As UltraGridCell = gridRow.Cells.Item("Received")
						cell.Appearance.BackColor = Color.Crimson
					Else
						If gridRow.Hidden = False Then
							gridRow.Selected = True
						End If
					End If
					If grdRL.Selected.Rows.Count = grdRL.Rows.Count Then
						cmdSelectAll.Tag = "Deselect"
						cmdSelectAll.Image = imgIcons.Images.Item(iNONE)
						ToolTip1.SetToolTip(cmdSelectAll, "Deselect All")
					End If
				Next
			Else
				For Each gridRow In grdRL.Rows
					If gridRow.Hidden = False Then
						gridRow.Selected = True
					End If
				Next

				cmdSelectAll.Tag = "Deselect"
				cmdSelectAll.Image = imgIcons.Images.Item(iNONE)
				ToolTip1.SetToolTip(cmdSelectAll, "Deselect All")
			End If

		Else
			cmdSelectAll.Tag = "Select"
			cmdSelectAll.Image = imgIcons.Images.Item(iALL)
			ToolTip1.SetToolTip(cmdSelectAll, "Select All")

			'Unhighlight all the rows
			grdRL.Selected.Rows.Clear()
		End If
		logger.Debug("cmdSelectAll_Click Exit")
	End Sub

	Private Sub initGrid(ByVal ug As UltraGrid, Optional ByVal bNOIDNORD As Boolean = False)
		'-- Set up the grid
		If bNOIDNORD = False Then
			ug.DisplayLayout.Bands(0).Columns("OrderLineID").Hidden = True
			ug.DisplayLayout.Bands(0).Columns("Line").Width = 34
			ug.DisplayLayout.Bands(0).Columns("Description").Width = 178
			ug.DisplayLayout.Bands(0).Columns("Identifier").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Ordered").Width = 57

			ug.DisplayLayout.Bands(0).Columns("Unit").Width = 57
			ug.DisplayLayout.Bands(0).Columns("Received").Width = 63
			ug.DisplayLayout.Bands(0).Columns("Weight").Width = 87
			ug.DisplayLayout.Bands(0).Columns("CostedByWeight").Hidden = True
			ug.DisplayLayout.Bands(0).Columns("CatchweightRequired").Hidden = True
			ug.DisplayLayout.Bands(0).Columns("isPackageUnit").Hidden = True
			ug.DisplayLayout.Bands(0).Columns("WeightPerPackage").Hidden = True

			ug.DisplayLayout.Bands(0).Columns("VendorItemID").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Brand").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Cost").Width = 57
			ug.DisplayLayout.Bands(0).Columns("PkgDesc").Width = 87
			ug.DisplayLayout.Bands(0).Columns("eInvoiceWeight").Width = 63
			ug.DisplayLayout.Bands(0).Columns("eInvoiceCost").Width = 63

			ug.DisplayLayout.Bands(0).Columns("Shipped").Width = 57
			ug.DisplayLayout.Bands(0).Columns("Shipped").Hidden = bHideShipped
			ug.DisplayLayout.Bands(0).Columns("Ship Weight").Width = 57
			ug.DisplayLayout.Bands(0).Columns("Ship Weight").Hidden = bHideShipped
		Else
			ug.DisplayLayout.Bands(0).Columns("Line").Width = 34
			ug.DisplayLayout.Bands(0).Columns("Description").Width = 178
			ug.DisplayLayout.Bands(0).Columns("Identifier").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Ordered").Width = 57

			ug.DisplayLayout.Bands(0).Columns("Unit").Width = 57

			ug.DisplayLayout.Bands(0).Columns("Weight").Width = 87
			ug.DisplayLayout.Bands(0).Columns("CostedByWeight").Hidden = True

			ug.DisplayLayout.Bands(0).Columns("VendorItemID").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Brand").Width = 87
			ug.DisplayLayout.Bands(0).Columns("Cost").Width = 57

			ug.DisplayLayout.Bands(0).Columns("PkgDesc").Width = 87
			ug.DisplayLayout.Bands(0).Columns("eInvoiceWeight").Width = 63
			ug.DisplayLayout.Bands(0).Columns("eInvoiceQty").Width = 63

			ug.DisplayLayout.Bands(0).Columns("Shipped").Width = 57
			ug.DisplayLayout.Bands(0).Columns("Shipped").Hidden = bHideShipped
			ug.DisplayLayout.Bands(0).Columns("Ship Weight").Width = 57
			ug.DisplayLayout.Bands(0).Columns("Ship Weight").Hidden = bHideShipped


			ug.DisplayLayout.Bands(0).Columns("eInvoiceItemException").Width = 10
			ug.DisplayLayout.Bands(0).Columns("eInvoiceItemException").Hidden = True
		End If
	End Sub

	Private Sub LoadReceivingDiscrepanyReasonCodes()

		Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
		Dim dt As DataTable = bo.getReasonCodeDetailsForType(enumReasonCodeType.RD.ToString)

		uddReasonCode.DataSource = dt
		uddReasonCode.DisplayMember = "ReasonCode"
		uddReasonCode.ValueMember = "ReasonCodeDetailID"

		Dim dr As DataRow = dt.NewRow()
		dr("ReasonCodeDetailID") = -1
		dr("ReasonCode") = ""
		dr("ReasonCodeDesc") = ""
		dr("ReasonCodeExtDesc") = ""
		dt.Rows.InsertAt(dr, 0)

		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Header.Caption = "Code"
		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Header.Caption = "Description"
		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Width = 50
		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Width = 200
		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDetailID").Hidden = True
		uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeExtDesc").Hidden = True

		Dim dr1 As DataRow
		OBCode = 0
		For Each dr1 In dt.Rows
			If Trim(dr1("ReasonCode")) = "OB" Then
				OBCode = dr1("ReasonCodeDetailID")
				Exit For
			End If
		Next dr1

	End Sub

	Private Sub InitializeCacheDataTable()

		Dim column As DataColumn

		column = New DataColumn()
		column.DataType = System.Type.GetType("System.Int32")
		column.ColumnName = "OrderLineID"
		dtCachedRows.Columns.Add(column)

		column = New DataColumn()
		column.DataType = System.Type.GetType("System.Decimal")
		column.ColumnName = "Received"
		dtCachedRows.Columns.Add(column)

		column = New DataColumn()
		column.DataType = System.Type.GetType("System.Decimal")
		column.ColumnName = "Weight"
		dtCachedRows.Columns.Add(column)

	End Sub

	Private Sub PopGrid(Optional ByRef bRefreshData As Boolean = True)

		logger.Debug("PopGrid Entry")
		Dim iLoop As Integer
		Dim bAddRow As Boolean

		If bRefreshData Then
			Me.dtOrderItems.Rows.Clear()
			Me.dtOrderItemsNOIDNORD.Rows.Clear()

			dtOrderItems = RListDAO.GetReceivingList(glOrderHeaderID)
			dtOrderItemsNOIDNORD = RListDAO.GetReceivingListForNOIDNORD(glOrderHeaderID)
		End If

		RefreshPartialShipmentCheckbox()

		m_bDataChanged = False
		m_bHaveSomeReceived = False
		bAtLeastOneReceived = False
		itemsWithReasonCodes = False

		bAtLeastOneReceived = False

		udsReceivingList.Rows.Clear()
		udsNOIDNORD.Rows.Clear()

		If Me.dtOrderItems.Rows.Count > 0 Then
			udsReceivingList.Rows.SetCount(Me.dtOrderItems.Rows.Count)
		End If

		If Me.dtOrderItemsNOIDNORD.Rows.Count > 0 Then
			Me.udsNOIDNORD.Rows.SetCount(Me.dtOrderItemsNOIDNORD.Rows.Count)
		End If

		If bRefreshData Then Erase piInsertOrder

		Dim dr As DataRow
		For Each dr In dtOrderItems.Rows
			iLoop = iLoop + 1

			'If the grid's data was refreshed, then it is the original sort order - capture the order row numbers
			If bRefreshData Then
				If iLoop = 1 Then plStartOrderItem_ID = dr.Item("OrderItem_ID")
				ReDim Preserve piInsertOrder(dr.Item("OrderItem_ID") - plStartOrderItem_ID)
				piInsertOrder(UBound(piInsertOrder)) = iLoop
			End If

			iLoop = IIf(bRefreshData, iLoop, piInsertOrder(dr.Item("OrderItem_ID") - plStartOrderItem_ID))
			PopGridRow(dr, iLoop)

			If dr.Item("QuantityReceived") IsNot DBNull.Value Then
				If dr.Item("QuantityReceived") >= 0 Then
					bAtLeastOneReceived = True
				End If
			End If

			If optAll.Checked Then
				bAddRow = True
			ElseIf optNotReceived.Checked Then
				If dr.Item("QuantityReceived") Is DBNull.Value Then
					bAddRow = True
				End If
			ElseIf (optReceived.Checked) Then
				If dr.Item("QuantityReceived") IsNot DBNull.Value Then
					If dr.Item("QuantityReceived") >= 0 Then
						bAddRow = True
					End If
				End If
			End If
			If bAddRow Then
				If dr.Item("QuantityReceived") IsNot DBNull.Value Then
					If dr.Item("QuantityReceived") >= 0 Then
						m_bHaveSomeReceived = True
						bAddRow = False
					End If
				End If

				If dr.Item("ReceivingDiscrepancyReasonCodeID") IsNot DBNull.Value Then
					itemsWithReasonCodes = True
				End If
			End If

		Next dr

		'DN reset our loop for the new grid
		iLoop = 0
		For Each dr In Me.dtOrderItemsNOIDNORD.Rows
			iLoop = iLoop + 1

			PopGridRow(dr, iLoop, True)
		Next dr

		DisplayRows()

		Call SetActive(cmdReceive, IIf(Me.grdRL.Rows.Count > 0 And (gbDistributor), True, False))

		' Only enable the Delete Receiving button if the user is on the All or Received tabs.
		Call SetActive(cmdReceiveDelete, If((m_bHaveSomeReceived Or itemsWithReasonCodes) And (optAll.Checked Or optReceived.Checked) And gbDistributor, True, False))

		' Only enable Past Receipt Date for non-transfer, non-return orders that haven't got any line item received or marked as particial shipment
		Call SetActive(chkPastReceipt, (Not bTransferOrder) And (Not bReturnOrder) And (Not bAtLeastOneReceived) And (dtpPastReceiptDate.Value Is Nothing) And (Not partialShippmentCheck.Checked), False)
		Call SetActive(dtpPastReceiptDate, (Not bTransferOrder) And (Not bReturnOrder) And (Not bAtLeastOneReceived) And (dtpPastReceiptDate.Value Is Nothing) And (Not partialShippmentCheck.Checked), False)

		' reset
		cmdSelectAll.Tag = "Select"
        cmdSelectAll.Image = imgIcons.Images.Item(iALL)
        ToolTip1.SetToolTip(cmdSelectAll, "Select All")
        Call SetActive(cmdSelectAll, IIf(Me.grdRL.Rows.Count > 0 And (gbDistributor), True, False))

        With grdRL.DisplayLayout.Bands(0)
            .Columns("VendorItemDescription").Width = .Columns("Description").Width
            .Columns("VendorItemDescription").Hidden = Not chkShowVendorDescriptions.Checked
            .Columns("Description").Hidden = chkShowVendorDescriptions.Checked
        End With

        ' Date:         10/13/2011
        ' Updated By:   Denis Ng
        ' Bug #:        3016
        ' Comment:      Set the sort order for the column "Line". Alternatively, this can be done by setting
        '               the SortIndicator property at design time.
        '
        ' Begin Fix 3016
        Me.grdRL.DisplayLayout.Bands(0).Columns(1).SortIndicator = SortIndicator.Ascending
        ' End Fix 3016

        logger.Debug("PopGrid Exit")
    End Sub

    Private Sub PopGridRow(ByVal dr As DataRow, ByVal index As Integer, Optional ByVal bNOIDNORD As Boolean = False)

        logger.Debug("PopGridRow Entry")
        Dim row As UltraDataRow

        If bNOIDNORD = False Then
            'Get the first row.
            row = Me.udsReceivingList.Rows(index - 1)
            row("OrderLineID") = dr.Item("OrderItem_ID")
            row("Line") = CInt(index)
            row("Identifier") = dr.Item("Identifier")
            row("Description") = dr.Item("Item_Description")
            row("Ordered") = Trim(Str(CDec(dr.Item("QuantityOrdered"))))
            row("Unit") = dr.Item("QuantityUnitName")
            row("Received") = dr.Item("QuantityReceived")
            row("Weight") = IIf(Val(dr.Item("Weight")) = 0, DBNull.Value, dr.Item("Weight"))
            row("CostedByWeight") = dr.Item("CostedByWeight")
            row("CatchweightRequired") = dr.Item("CatchweightRequired")
            row("isPackageUnit") = dr.Item("isPackageUnit")
            row("WeightPerPackage") = dr.Item("WeightPerPackage")
            row("eInvoice") = IIf(Val(IIf(IsDBNull(dr.Item("eInvoiceQuantity")), 0, dr.Item("eInvoiceQuantity"))) = 0, DBNull.Value, dr.Item("eInvoiceQuantity"))
            row("VendorItemID") = dr.Item("VendorItemID")
            row("Brand") = dr.Item("Brand")
            'Task 2456, Only received items should display cost
            If (Val(IIf(IsDBNull(dr.Item("QuantityReceived")), 0, dr.Item("QuantityReceived"))) <> 0) Then
                row("Cost") = Math.Round(CDec(dr.Item("Cost")), 2)
            End If
            row("VendorItemID") = dr.Item("VendorItemID")
            If dr.Item("Package_Desc1") IsNot DBNull.Value AndAlso dr.Item("Package_Desc2") IsNot DBNull.Value Then
                row("PkgDesc") = CType(dr.Item("Package_Desc1"), Decimal).ToString("#####.##") & "/" & CType(dr.Item("Package_Desc2"), Decimal).ToString("#####.##") & " " & dr.Item("Package_Unit")
                row("PD2") = dr.Item("Package_Desc2")
            End If
            row("eInvoiceWeight") = dr.Item("eInvoiceWeight")
            row("eInvoiceCost") = dr.Item("eInvoiceCost")
            row("eInvoice Unit") = dr.Item("InvoiceQuantityUnitName")
            row("Shipped") = dr.Item("QuantityShipped")
            row("Ship Weight") = dr.Item("WeightShipped")
            row("VendorItemDescription") = dr.Item("VendorItemDescription") & ""
            row("Code") = dr.Item("ReceivingDiscrepancyReasonCodeID")
            ' 4.8 - Disable enhanced item refusal functionality until final requirements are delivered.
            ' row("Refused") = dr.Item("RefusedQuantity")
        Else
            'Get the first row.
            row = Me.udsNOIDNORD.Rows(index - 1)
            row("Line") = CInt(index)
            row("Identifier") = dr.Item("Identifier")
            row("Brand") = dr.Item("Brand")
            row("VendorItemID") = dr.Item("VendorItemID")
            row("Description") = dr.Item("Item_Description")
            row("Ordered") = Trim(Str(CDec(IIf(IsDBNull(dr.Item("QuantityOrdered")), 0, dr.Item("QuantityOrdered")))))
            row("Unit") = dr.Item("Package_Unit")
            row("eInvoiceQty") = IIf(IsDBNull(dr.Item("eInvoiceQuantity")), DBNull.Value, CType(dr.Item("eInvoiceQuantity"), Decimal).ToString("#.###"))
            row("eInvoice Unit") = dr.Item("InvoiceQuantityUnitName")
            row("eInvoiceWeight") = CType(IIf(IsDBNull(dr.Item("eInvoiceWeight")), 0, dr.Item("eInvoiceWeight")), Decimal).ToString("#.###")
            row("eInvoice Package") = IIf(IsDBNull(dr.Item("eInvoiceCase_Pack")), DBNull.Value, CType(dr.Item("eInvoiceCase_Pack"), Decimal).ToString("#.###"))
            row("Weight") = IIf(Val(dr.Item("Weight")) = 0, DBNull.Value, CType(dr.Item("Weight"), Decimal).ToString("#.###"))
            row("Cost") = Math.Round(CDec(dr.Item("Cost")), 2)
            If dr.Item("Package_Desc1") IsNot DBNull.Value AndAlso dr.Item("Package_Desc2") IsNot DBNull.Value Then
                row("PkgDesc") = CType(dr.Item("Package_Desc1"), Decimal).ToString("#####.##") & "/" & CType(dr.Item("Package_Desc2"), Decimal).ToString("#####.##") & " " & dr.Item("Package_Unit")
            End If
            row("CostedByWeight") = dr.Item("CostedByWeight")
            row("eInvoiceItemException") = dr.Item("eInvoiceItemException")
        End If
        logger.Debug("PopGridRow Exit")
    End Sub

    Private Sub PopulateDiscrepancyRows()

        Dim gridRow As UltraGridRow

        grdRL.EventManager.AllEventsEnabled = False
        For Each gridRow In grdRL.Rows
            For Each row As DataRow In dtCachedRows.Rows
                If gridRow.Cells("OrderLineID").Value = row.Item("OrderLineID").ToString Then
                    If Not IsDBNull(row.Item("Received")) Then gridRow.Cells("Received").Value = Val(row.Item("Received"))
                    If Not IsDBNull(row.Item("Weight")) Then gridRow.Cells("Weight").Value = Val(row.Item("Weight"))
                    If Val(gridRow.Cells("Received").Text) <> Val(gridRow.Cells("Einvoice").Text) Then
                        gridRow.Cells("Code").Appearance.BackColor = Color.LightPink
                        gridRow.ToolTipText = "Receiving Discrepancy!"

                    End If
                End If
            Next
        Next
        grdRL.EventManager.AllEventsEnabled = True

    End Sub

#Region "Command Buttons"
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdReceive_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReceive.Click
        logger.Debug("cmdReceive_Click Entry")
        Dim nTotalSelRows As Short
        Dim iLoop As Short
        Dim strMessage As String = ""
        Dim FlagCostedByWeight As Boolean = False
        Dim FlagCatchWeightRequired As Boolean = False
        Dim FlagEinvoiceQtyExists As Boolean = False
        Dim iReasonCodeId As Integer
        Dim cellWithNoReceivedQuantity As Boolean = False

        'Clear the Cached Rows before we start receiving and validating rows
        dtCachedRows.Clear()

        'Get the number of selected items
        nTotalSelRows = Me.grdRL.Selected.Rows.Count

        If (nTotalSelRows = 0) Then
            MsgBox("Please select item(s) to receive.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        'Check receiving rules - determine if the user must manually enter a received quantity
        If m_bAllowReceiveAll = False Then
            For Each selectedRow As UltraGridRow In grdRL.Selected.Rows
                If selectedRow.GetCellValue("Received").ToString = String.Empty Then
                    Dim cell As UltraGridCell = selectedRow.Cells.Item("Received")
                    cell.Appearance.BackColor = Color.Crimson
                    cellWithNoReceivedQuantity = True
                End If
            Next
            If cellWithNoReceivedQuantity Then
                grdRL.Selected.Rows.Clear()
                MsgBox("Please enter a received quantity for each line.", MsgBoxStyle.OkOnly + _
                    MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
        End If

        ' Added for task 5733 - begin
        ' Recalculate 3rdParty Freight distribution here
        Dim nThirdpartyTotal As Decimal
        nThirdpartyTotal = OrderingFunctions.GetFreight3PartyTotal(glOrderHeaderID)
        OrderingFunctions.DistributeFreight(glOrderHeaderID, nThirdpartyTotal)
        ' Added for task 5733 - end

        SQLExecute("BEGIN TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Dim sReceivedValue As String = String.Empty
        Dim sReceivedWeight As String = String.Empty
        'Dim sRefusedValue As String = String.Empty

        For iLoop = 0 To nTotalSelRows - 1

            ' # If viewing the EInvoice Exceptions screen only, 
            ' # if Received has a value, use that. Otherwise use EinvoicingQty instead of
            ' # OrderedQuantity for the default. Also allows for 0 values to be used.

            ' # if not viewing the Einvoice Exceptions screen, 
            ' # Original rules apply. If Received is left blank, OrderedQuanity is used by default
            ' # otherwise Received value is used. 0 values are now accepted.

            FlagCostedByWeight = CBool(grdRL.Selected.Rows(iLoop).Cells("CostedByWeight").Text)
            FlagCatchWeightRequired = CBool(grdRL.Selected.Rows(iLoop).Cells("CatchweightRequired").Text)

            With grdRL.Selected.Rows(iLoop)
                If optExceptions.Checked Then
                    If .Cells("Received").Text = String.Empty Then
                        If .Cells("eInvoice").Text = String.Empty Then
                            sReceivedValue = "0"
                        Else
                            sReceivedValue = .Cells("eInvoice").Text
                            FlagEinvoiceQtyExists = True
                        End If
                    Else
                        sReceivedValue = .Cells("Received").Text

                        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                        'Dim str As String = IIf(.Cells("Refused").Text = String.Empty, "0", .Cells("Refused").Text)
                        'If CDbl(str) > 0 Then
                        '    sReceivedValue = (CDbl(.Cells("Received").Text) + CDbl(.Cells("Refused").Text)).ToString
                        'Else
                        '    sReceivedValue = .Cells("Received").Text
                        'End If
                    End If

                ElseIf bHideShipped = False Then
                    If .Cells("Received").Text = String.Empty Then
                        If .Cells("Shipped").Text = String.Empty Then
                            sReceivedValue = 0
                        Else
                            sReceivedValue = .Cells("Shipped").Text
                        End If
                        If FlagCostedByWeight And CBool(grdRL.Selected.Rows(iLoop).Cells("isPackageUnit").Text) _
                                And sReceivedValue > 0 _
                                And Val(grdRL.Selected.Rows(iLoop).Cells("Weight").Text) = 0 Then
                            ' Tom Lux, TFS 11498, 12/17/09: Changed ship-weight ref in line below from .Text to .Value because if
                            ' ship-weight is NULL, the receiving screen gets a "could not convert string to decimal" error throw by
                            ' the grid object.
                            If Not FlagCatchWeightRequired Then
                                .Cells("Weight").Value = .Cells("Ship Weight").Value
                            End If
                        End If
                    Else
                        sReceivedValue = .Cells("Received").Text

                        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                        'Dim str As String = IIf(.Cells("Refused").Text = String.Empty, "0", .Cells("Refused").Text)
                        'If CDbl(str) > 0 Then
                        '    sReceivedValue = (CDbl(.Cells("Received").Text) + CDbl(.Cells("Refused").Text)).ToString
                        'Else
                        '    sReceivedValue = .Cells("Received").Text
                        'End If
                    End If
                Else
                    If .Cells("Received").Text = String.Empty Then sReceivedValue = "NULL" Else sReceivedValue = .Cells("Received").Text

                    ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                    'If .Cells("Received").Text = String.Empty Then
                    '    sReceivedValue = "NULL"
                    'Else
                    '    Dim str As String = IIf(.Cells("Refused").Text = String.Empty, "0", .Cells("Refused").Text)
                    '    If CDbl(str) > 0 Then
                    '        sReceivedValue = (CDbl(.Cells("Received").Text) + CDbl(.Cells("Refused").Text)).ToString
                    '    Else
                    '        sReceivedValue = .Cells("Received").Text
                    '    End If
                    'End If
                End If

                sReceivedWeight = IIf(Val(.Cells("Weight").Text) = 0, "NULL", Val(.Cells("Weight").Text))

            End With

            If FlagCostedByWeight And _
                CBool(grdRL.Selected.Rows(iLoop).Cells("isPackageUnit").Text) And _
                Not FlagCatchWeightRequired _
                And Val(sReceivedValue) > 0 And _
                Not FlagEinvoiceQtyExists And _
                Val(sReceivedWeight) = 0 Then

                ' required weight is missing for a CostedByWeight item, CatchweightRequired items are handled below
                strMessage = strMessage + "Line: " + grdRL.Selected.Rows(iLoop).Cells("Line").Text + ", Please enter the weight for item ordered by the box." + vbCrLf

            Else
                ' Logic to handle CatchWeightRequired Items, weight must be entered manually - TFS 2456
                '' Add rows to the discrepancy list to allow saving the data user must have entered 
                If FlagCatchWeightRequired AndAlso sReceivedWeight = "NULL" AndAlso sReceivedValue = "NULL" Then

                    strMessage = strMessage + "Line: " + grdRL.Selected.Rows(iLoop).Cells("Line").Text + ", Catchweight Item, please enter the weight manually." + vbCrLf
                    'Not adding discrepancy rows here as the Received Qty and Weight were not entered 

                ElseIf FlagCatchWeightRequired AndAlso Val(sReceivedWeight) > 0 AndAlso Val(sReceivedValue) = 0 Then

                    strMessage = strMessage + "Line: " + grdRL.Selected.Rows(iLoop).Cells("Line").Text + ", Weight is Received for a Catchweight item, please enter the Quantity or delete the Weight." + vbCrLf

                    drCachedRows = dtCachedRows.NewRow()
                    drCachedRows("OrderLineID") = grdRL.Selected.Rows(iLoop).Cells("OrderLineID").Value
                    drCachedRows("Received") = grdRL.Selected.Rows(iLoop).Cells("Received").Value
                    drCachedRows("Weight") = grdRL.Selected.Rows(iLoop).Cells("Weight").Value
                    dtCachedRows.Rows.Add(drCachedRows)

                ElseIf FlagCatchWeightRequired AndAlso Val(sReceivedWeight) = 0 AndAlso Val(sReceivedValue) > 0 Then

                    strMessage = strMessage + "Line: " + grdRL.Selected.Rows(iLoop).Cells("Line").Text + ", Qty is Received for a Catchweight item, please enter the Weight or delete the Quantity." + vbCrLf

                    drCachedRows = dtCachedRows.NewRow()
                    drCachedRows("OrderLineID") = grdRL.Selected.Rows(iLoop).Cells("OrderLineID").Value
                    drCachedRows("Received") = grdRL.Selected.Rows(iLoop).Cells("Received").Value
                    drCachedRows("Weight") = grdRL.Selected.Rows(iLoop).Cells("Weight").Value
                    dtCachedRows.Rows.Add(drCachedRows)

                ElseIf sReceivedValue <> "NULL" AndAlso EInvoice_Id <> -1 AndAlso grdRL.Selected.Rows(iLoop).Cells("Code").Text = "" AndAlso _
                      (Val(sReceivedValue) <> Val(grdRL.Selected.Rows(iLoop).Cells("eInvoice").Text)) AndAlso Not partialShippmentCheck.Checked Then

                    'Logic to handle Receiving Discrepancies for capturing a Reason Code - TFS 2459
                    'Per bug 2813, Receiving Discrepancy occurs only when Received Qty does not match Einvoice Qty
                    'Weight differences are ignored (for both - costed by weight and catch weight items)
                    strMessage = strMessage + "Line: " + grdRL.Selected.Rows(iLoop).Cells("Line").Text + ", Received Qty does not match the eInvoice Qty, please select a Reason Code for the Receiving Discrepancy." + vbCrLf

                    drCachedRows = dtCachedRows.NewRow()
                    drCachedRows("OrderLineID") = grdRL.Selected.Rows(iLoop).Cells("OrderLineID").Value
                    drCachedRows("Received") = grdRL.Selected.Rows(iLoop).Cells("Received").Value
                    drCachedRows("Weight") = grdRL.Selected.Rows(iLoop).Cells("Weight").Value
                    dtCachedRows.Rows.Add(drCachedRows)
                    'End If
                Else

                    If grdRL.Selected.Rows(iLoop).Cells("Code").Text <> "" Then
                        iReasonCodeId = grdRL.Selected.Rows(iLoop).Cells("Code").Value
                    Else
                        iReasonCodeId = -1
                    End If

					'Receive the item
					RListDAO.ReceiveOrderItem(grdRL.Selected.Rows(iLoop).Cells("OrderLineID").Value, sReceivedValue, sReceivedWeight, iReasonCodeId)
                    frmOrders.ItemsReceived = True

                End If
            End If

        Next iLoop

		If frmOrders.ItemsReceived And chkPastReceipt.Enabled Then
			RListDAO.SavePastReceiptDate(frmOrders.OrderHeader_ID, dtpPastReceiptDate.Value)
		End If

		'##########################################################################################################################
		'#  bug 6759: The folliwng COMMIT Tran cal must be called BEFORE 3rd Party Freight is calculated.
		'# otherwise 3rd party freight will hang because OrderItems is locked.
		'##########################################################################################################################

		SQLExecute("COMMIT TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If strMessage <> "" Then
            MsgBox(strMessage, MsgBoxStyle.Exclamation, Me.Text)
        End If

        PopGrid()

        If dtCachedRows.Rows.Count > 0 Then
            PopulateDiscrepancyRows()
        End If

        logger.Debug("cmdReceive_Click Exit")

    End Sub

    Private Sub cmdReceiveDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReceiveDelete.Click

        logger.Debug("cmdReceiveDelete_Click Entry")

        Dim nTotalSelRows As Short
        Dim sSQLText As String
        Dim iLoop As Short
        Dim bSuccess As Boolean = True


        '-- Get the number of selected items
        nTotalSelRows = Me.grdRL.Selected.Rows.Count

        If (nTotalSelRows = 0) Then
            MsgBox("Please select item(s) to delete receiving info.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        SQLExecute("BEGIN TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)


        For iLoop = 0 To nTotalSelRows - 1
            ' Only delete info for items that have a Received Quantity or a Reason Code.
            If (Not grdRL.Selected.Rows(iLoop).Cells("Received").Text.Trim().Equals(String.Empty)) Or (Not grdRL.Selected.Rows(iLoop).Cells("Code").Text.Trim().Equals(String.Empty)) Then
                Try
                    sSQLText = "EXEC DeleteReceiving " & CInt(grdRL.Selected.Rows(iLoop).Cells("OrderLineID").Value)
                    SQLExecute(sSQLText, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Catch ex As Exception
                    bSuccess = False
                End Try
            End If
        Next iLoop

        SQLExecute("COMMIT TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        ' Added for task 5733 - begin
        ' Recalculate 3rdParty Freight distribution here

        Dim nThirdpartyTotal As Decimal
        nThirdpartyTotal = OrderingFunctions.GetFreight3PartyTotal(glOrderHeaderID)
        OrderingFunctions.DistributeFreight(glOrderHeaderID, nThirdpartyTotal)
        ' Added for task 5733 - end
        logger.Debug("cmdReceiveDelete_Click Exit")


        PopGrid()

        If Not bSuccess Then MessageBox.Show("One or more items that you selected were unable to be unreceived.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)



    End Sub
#End Region
    Private Sub DisplayRows()

        logger.Debug("DisplayRows Entry")

        Dim bAll As Boolean
        Dim bShowReceived As Boolean
        Dim bShowNotReceived As Boolean
        Dim bShowInvoiceExceptions As Boolean

        Dim row As UltraGridRow
        Dim bShow As Boolean



        If Me.optAll.Checked Then bAll = True
        If Me.optReceived.Checked Then bShowReceived = True
        bAll = optAll.Checked
        bShowReceived = optReceived.Checked
        bShowNotReceived = optNotReceived.Checked
        bShowInvoiceExceptions = optExceptions.Checked

        ' Date:         01/18/2012
        ' Updated By:   Ben Sims
        ' Bug #:        4317
        ' Comment:      Set a visual for the receiver to indicate to them which row needed to be updated
        '               with a reason code for a receiving discrepancy.  This only affects items where
        '               optAll and optReceived are checked.
        ' Date:         01/18/2012
        ' Updated By:   Ben Sims
        ' Bug #:        4523
        ' Comment:      Added logic to bug fix 4317 to not highlight code field for non-einvoice orders

        For Each row In grdRL.Rows
            Dim ReceivedQty As Decimal = 0
            Dim EInvoiceQty As Decimal = 0

            If IsNumeric(row.Cells("Received").Text) Then
                ReceivedQty = Decimal.Parse(row.Cells("Received").Text)
            End If

            If IsNumeric(row.Cells("Einvoice").Text) Then
                EInvoiceQty = Decimal.Parse(row.Cells("Einvoice").Text)
            End If

            If bAll Then
                If ReceivedQty <> EInvoiceQty And row.Cells("Code").Text = "" And EInvoice_Id <> -1 Then
                    row.Cells("Code").Appearance.BackColor = Color.LightPink
                End If
                bShow = True
            ElseIf bShowReceived And row.Cells("Received").Text.Length > 0 Then
                If ReceivedQty <> EInvoiceQty And row.Cells("Code").Text = "" And EInvoice_Id <> -1 Then
                    row.Cells("Code").Appearance.BackColor = Color.LightPink
                End If
                bShow = True
            ElseIf bShowNotReceived And row.Cells("Received").Text.Length = 0 Then
                bShow = True
            ElseIf bShowInvoiceExceptions Then
                If Not row.Cells("eInvoice").Value.Equals(0) Then
                    If Not row.Cells("eInvoice").Value.Equals(row.Cells("Ordered").Value) _
                    Or (Not row.Cells("eInvoice Unit").Value Is DBNull.Value _
                        And Not row.Cells("eInvoice Unit").Value.Equals(row.Cells("Unit").Value) _
                    ) Then
                        bShow = True
                    Else
                        bShow = False
                    End If
                Else
                    bShow = False
                End If

            Else
                bShow = False
            End If
            row.Hidden = Not bShow

            If row.Cells("CatchweightRequired").Text Then

                row.Cells("Weight").Appearance.BackColor = Color.LightGreen
                row.ToolTipText = "Catchweight Required Item!"

            End If

        Next row

        For Each row In Me.grdNOIDNORD.Rows
            If bAll OrElse bShowInvoiceExceptions Then
                bShow = True
            ElseIf bShowNotReceived Then
                bShow = True
            Else
                bShow = False
            End If
            row.Hidden = Not bShow
        Next row

        logger.Debug("DisplayRows Exit")
    End Sub

    Private Sub frmReceivingList_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmReceivingList_FormClosing Entry")

        'if user changed values, ask if they want to save changes
        If (m_bDataChanged = True) Then
            If (MsgBox("Exit without saving changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Me.Text) = MsgBoxResult.Yes) Then
                m_bDataChanged = False
            Else
                e.Cancel = True
                Exit Sub
            End If
        End If

        'Refresh OrderHeaderCosts
        'Add string parameter 'ReceivingListForm' to track where UpdateOrderRefreshCost is called
        Cursor = Cursors.WaitCursor
        OrderingDAO.UpdateOrderRefreshCosts(glOrderHeaderID, enumRefreshCostSource.ReceivingListForm.ToString())
        Cursor = Cursors.Default

        logger.Debug("frmReceivingList_FormClosing Exit")
    End Sub

    Private Sub opt_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) _
            Handles optAll.CheckedChanged, optNotReceived.CheckedChanged, optExceptions.CheckedChanged
        logger.Debug("opt_CheckedChanged Entry")
        If IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Call RefreshDisplay()
        Else
            optNames = eventSender.Name
        End If

        Me.cmdNOIDNORDReport.Visible = True

        If dtOrderItemsNOIDNORD.Rows.Count > 0 Then
            Me.grdNOIDNORD.Visible = True
            Me.cmdNOIDNORDReport.Enabled = True
        Else
            Me.grdNOIDNORD.Visible = False
            Me.cmdNOIDNORDReport.Enabled = False
        End If

        Me.grdRL.Height = Panel1.Height - grdNOIDNORD.Height

        logger.Debug("opt_CheckedChanged Exit")
    End Sub
    'DN added 11/12/2009 to handle Received rdo click event for the clearing of the NOIDNORD grid
    Private Sub opt_CheckedChangedReceived(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) _
                Handles optReceived.CheckedChanged
        logger.Debug("opt_CheckedChanged Entry")
        If IsInitializing Then Exit Sub
        If eventSender.Checked Then
            Call RefreshDisplay()
        Else
            optNames = eventSender.Name
        End If

        Me.grdRL.Height = Panel1.Height - 5


        logger.Debug("opt_CheckedChanged Exit")
    End Sub

    Private Sub RefreshDisplay()

        logger.Debug("RefreshDisplay Entry")
        ' if user selected Do not Continue already, skip the below code
        If bCheckIfNo = False Then
            'if user changed values, ask if they want to save changes
            If (m_bDataChanged = True) Then
                If Not (m_bProcessing) Then
                    If (MsgBox("Continue without saving changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Me.Text) = MsgBoxResult.Yes) Then
                        PopGrid()
                        bCheckIfNo = False
                    Else
                        bCheckIfNo = True
                        RestoreOptChecked(optNames)

                    End If
                End If
            Else
                PopGrid()
            End If
        End If
        bCheckIfNo = False
        logger.Debug("RefreshDisplay Exit")

    End Sub

    ' Restores the Radio button on Do not Continue user selection
    Private Sub RestoreOptChecked(ByRef optNames As String)
        If optNames.Contains(optNotReceived.Name) Then
            optNotReceived.Checked = True
        ElseIf optNames.Contains(optAll.Name) Then
            optAll.Checked = True
        ElseIf optNames.Contains(optReceived.Name) Then
            optReceived.Checked = True
        ElseIf optNames.Contains(optExceptions.Name) Then
            optExceptions.Checked = True
        End If
    End Sub

    Private Sub grdRL_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRL.AfterCellActivate

        logger.Debug("grdRL_AfterCellActivate Entry")
        If grdRL.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If
        logger.Debug("grdRL_AfterCellActivate Exit")

    End Sub


    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress

        logger.Debug("txtField_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        Select Case Index
            Case 1
                KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
            Case 0
                KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
        End Select

        If KeyAscii = 13 Then
            FilterGrid()
            KeyAscii = 0
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

    Public Sub FilterGrid()

        logger.Debug("FilterGrid Entry")

        Me.Cursor = Cursors.WaitCursor

        Dim band As UltraGridBand = Me.grdRL.DisplayLayout.Bands("RecordSet")

        band.Override.AllowRowFiltering = DefaultableBoolean.True
        band.Override.RowFilterMode = RowFilterMode.AllRowsInBand

        RemoveFilter()

        If Me._txtField_1.Text.Length > 0 Then
            band.ColumnFilters("Identifier").FilterConditions.Add(FilterComparisionOperator.Like, "*" & Me._txtField_1.Text & "*")
        End If

        If Me.TextBoxVIN.Text.Length > 0 Then
            band.ColumnFilters("VendorItemID").FilterConditions.Add(FilterComparisionOperator.Like, "*" & Me.TextBoxVIN.Text & "*")
        End If

        If Me.TextBoxBrand.Text.Length > 0 Then
            band.ColumnFilters("Brand").FilterConditions.Add(FilterComparisionOperator.Like, "*" & Me.TextBoxBrand.Text & "*")
        End If

        Me.Cursor = Cursors.Default

        logger.Debug("FilterGrid Exit")

    End Sub

    Private Sub cmdFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFilter.Click

        FilterGrid()

    End Sub

    Private Sub cmdRemoveFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveFilter.Click

        RemoveFilter()

    End Sub

    Private Sub RemoveFilter()

        Dim band As UltraGridBand = Me.grdRL.DisplayLayout.Bands("RecordSet")

        band.Override.AllowRowFiltering = DefaultableBoolean.False

        band.ColumnFilters("Identifier").FilterConditions.Clear()
        band.ColumnFilters("VendorItemID").FilterConditions.Clear()
        band.ColumnFilters("Brand").FilterConditions.Clear()

    End Sub

    Private Sub grdNOIDNORD_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles grdNOIDNORD.InitializeRow
        Dim _redAppearance As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        _redAppearance.BackColor = Color.IndianRed 'PaleVioletRed 'LightCoral 'IndianRed
        _redAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
        If Not e.Row.Cells("eInvoiceItemException").Value.Equals(DBNull.Value) Then
            e.Row.Appearance = _redAppearance
            e.Row.ToolTipText = e.Row.Cells("eInvoiceItemException").Value
        Else
            e.Row.ToolTipText = "This item is on the eInvoice but not on the Purchase Order."
        End If
    End Sub

    'DN added block 11/02/2009
    Private Sub cmdReparseEInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReparseEInvoice.Click

        If EInvoice_Id <> -1 Then

            DisablePanel()
            lblReparseStatus.Visible = True
            pbrStatus.Visible = True
            ReparseWorker.RunWorkerAsync(EInvoice_Id)

        End If

    End Sub

    Private Sub cmdNOIDNORDReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNOIDNORDReport.Click
        logger.Debug("cmdNOIDNORDReport_Click Entry")
        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        filename = "eInvoiceExceptionReport"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        ' Passing parameters to report.
        If glOrderHeaderID > 0 Then
            sReportURL.Append("&OrderHeader_ID=" & glOrderHeaderID)
        End If

        Dim sReportingServicesURL As String = ConfigurationServices.AppSettings("reportingServicesURL")
        System.Diagnostics.Process.Start(sReportingServicesURL & sReportURL.ToString())

        logger.Debug("cmdNOIDNORDReport_Click Exit")

    End Sub
    Private Function ReceivedAnyItem() As Boolean
        Dim receivedItemCount As Integer = 0
        Dim row As UltraGridRow
        Dim val As Decimal
        For Each row In grdRL.Rows
            If (Decimal.TryParse(Trim(row.Cells("Received").Text), val)) Then
                If Trim(row.Cells("Received").Text).Length <> 0 And val > 0.0 Then
                    receivedItemCount += 1
                End If
            End If
        Next row
        If receivedItemCount > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ReceivedAllItems() As Boolean
        Dim row As UltraGridRow
        Dim val As Decimal
        Dim cellValue As String

        For Each row In grdRL.Rows
            cellValue = Trim(row.Cells("Received").Text)
            If cellValue = String.Empty Then Return False

            If (Decimal.TryParse(cellValue, val)) Then
                If val < 0.0 Then Return False
            Else
                Return False
            End If
        Next row

        Return True
    End Function

    Private Sub cmdCloseOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCloseOrder.Click
        logger.Debug("cmdCloseOrder_Click Entry")
        If Me.OrderHeaderId > 0 Then
            glOrderHeaderID = Me.OrderHeaderId

            ' Check that all line items have a received quantity, even if that quantity is zero.  An exception will be made for orders marked as partial shipments.
            If Not ReceivedAllItems() And partialShippmentCheck.Checked = False Then

                If Not OrderingDAO.IsOrderReceivingComplete(glOrderHeaderID) Then
                    MsgBox("Not all line items have been received.  Please enter a received quantity for each line.", MsgBoxStyle.Exclamation, Me.Text)

                    optNotReceived.Checked = True
                    Exit Sub
                End If
            ElseIf partialShippmentCheck.Checked = True And Not ReceivedAnyItem() Then
                MsgBox("Items not received. Please enter received quantity.", MsgBoxStyle.Exclamation, Me.Text)
                optNotReceived.Checked = True
                Exit Sub
            End If

            'For e-invoiced POs, force Receiving Discrepancy Reason Code to be filled in if the eInvoice quantity and received quantity don't match. 
            If EInvoice_Id <> -1 Then
                Dim plReceivingDiscrepancyItemCount As Integer = 0
                Dim row As UltraGridRow
                Dim reasonCodeList As New StringBuilder
                Dim separator1 As Char = "|"
                Dim separator2 As Char = ","

                For Each row In grdRL.Rows
                    Dim ReceivedQty As Decimal = 0
                    Dim EInvoiceQty As Decimal = 0

                    If Trim(row.Cells("Received").Text) <> "" And IsNumeric(row.Cells("Received").Text) Then
                        ReceivedQty = Decimal.Parse(row.Cells("Received").Text)
                    End If

                    If Trim(row.Cells("Einvoice").Text) <> "" And IsNumeric(row.Cells("Einvoice").Text) Then
                        EInvoiceQty = Decimal.Parse(row.Cells("Einvoice").Text)
                    End If

                    If ReceivedQty <> EInvoiceQty And row.Cells("Code").Text = "" And (ReceivedQty + EInvoiceQty > 0) And Not partialShippmentCheck.Checked Then
                        row.Cells("Code").Appearance.BackColor = Color.LightPink
                        plReceivingDiscrepancyItemCount = plReceivingDiscrepancyItemCount + 1
                    End If
                Next row

                '-- Make sure that receiving discrepancy reason codes have been entered for any items
                '-- that were received via the handheld and the received qty does not match the einvoice qty
                If plReceivingDiscrepancyItemCount > 0 Then
                    logger.Info("CloseOrder exit=False;  There are items that need receiving reason codes entered, OrderHeader_ID=" + glOrderHeaderID.ToString)
                    MsgBox(String.Format(ResourcesOrdering.GetString("msg_ReceivingDiscrepanciesExist"), Environment.NewLine, plReceivingDiscrepancyItemCount), MsgBoxStyle.OkOnly, Me.Text)
                    Exit Sub
                End If

                'Update the Receiving Discrepancy Reason Code if it's populated
                For Each row In grdRL.Rows
                    If row.Cells("Code").Text <> "" Then
                        reasonCodeList.Append(row.Cells("OrderLineID").Text)
                        reasonCodeList.Append(separator2)
                        reasonCodeList.Append(row.Cells("Code").Value.ToString())
                        reasonCodeList.Append(separator1)
                    End If
                Next row
                If reasonCodeList.Length > 0 Then
                    SQLExecute(String.Format("UpdateReceivingDiscrepancyReasonCodeID '{0}','{1}','{2}'", reasonCodeList.ToString(), separator1, separator2), DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    m_bDataChanged = False
                End If
            End If

            'Refresh OrderHeaderCosts
            Cursor = Cursors.WaitCursor
            OrderingDAO.UpdateOrderRefreshCosts(glOrderHeaderID, enumRefreshCostSource.ReceivingListForm.ToString())
            Cursor = Cursors.Default

        End If

        glSubTeamNo = Me.TransferFromSubTeamNo

        frmOrderStatus.EInvoiceid = Me.EInvoicingId
        frmOrderStatus.PartialShippment = Me.partialShippmentCheck.Checked
        frmOrderStatus.ShowDialog()
        frmOrderStatus.Close()
        frmOrderStatus.Dispose()

        Me.Close()
        logger.Debug("cmdCloseOrder_Click Exit")
    End Sub



    Private Sub grdRL_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles grdRL.AfterCellUpdate
        logger.Debug("grdRL_AfterCellUpdate Entry")

        Dim orderedQuantity As Double
        orderedQuantity = e.Cell.Row.Cells("Ordered").Value

        If e.Cell.Column.Key = "Received" AndAlso Not updateReceivedValue Then
            ' Set the flag to TRUE to prevent an infinite loop
            updateReceivedValue = True

            ' The business rules for Received value is that it should always be an integer UNLESS the
            ' item is costed by weight.  Strip off any decimal places entered by the user.
            If Not (grdRL.ActiveRow.Cells("CostedByWeight").Value) Then
                If Not IsDBNull(grdRL.ActiveRow.Cells("Received").Value) Then
                    grdRL.ActiveRow.Cells("Received").Value = Decimal.Truncate(CDec(grdRL.ActiveRow.Cells("Received").Value))
                End If
            End If
        End If

        If e.Cell.Column.Key = "Received" AndAlso updateReceivedValue Then

            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'If IIf(IsDBNull(e.Cell.Value), 0, e.Cell.Value) > orderedQuantity Then
            '    MessageBox.Show("Received quantity is greater than ordered quantity. 'OB-Refused Item - Shipped/billed more than ordered' will be applied to this item. All excess quantities will be added to the Refused Items page.", "Excess Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            '    e.Cell.Row.Cells("Code").Value = OBCode
            '    e.Cell.Row.Cells("Refused").Value = IIf(IsDBNull(e.Cell.Value), 0, e.Cell.Value) - orderedQuantity
            '    grdRL.ActiveRow.Cells("Received").Value = orderedQuantity
            'End If

            If grdRL.ActiveRow.Cells("CostedByWeight").Value And grdRL.ActiveRow.Cells("isPackageUnit").Value And (Not IsDBNull(e.Cell.Value)) Then
                If Not (grdRL.ActiveRow.Cells("CatchweightRequired").Value) Then
                    grdRL.ActiveRow.Cells("weight").Value = e.Cell.Value * grdRL.ActiveRow.Cells("WeightPerPackage").Value
                End If
            End If

            ' Set the flag to FALSE to allow for processing to continue after the next update
            updateReceivedValue = False
        End If

        'Highlight Received cell based on the criteria:
        '   Received Qty = 0 ? Peach
        '   Received Qty Is NULL or > 0 ? No highlight
        If e.Cell.Column.Key = "Received" Then
            If IsDBNull(e.Cell.Value) Then
                e.Cell.Appearance.BackColor = Nothing
            ElseIf e.Cell.Value > 0 Then
                e.Cell.Appearance.BackColor = Nothing
            Else
                e.Cell.Appearance.BackColor = Color.PeachPuff
            End If
        End If

        logger.Debug("grdRL_AfterCellUpdate Exit")
    End Sub

    Private Sub grdRL_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles grdRL.BeforeCellUpdate

        logger.Debug("grdRL_BeforeCellUpdate Entry")

        If e.Cell.Column.Key = "Received" Then

            If IIf(IsDBNull(e.NewValue), 0, e.NewValue) > 1000 Then

                Dim response As MsgBoxResult = MessageBox.Show("Quantity exceeds 1000! Are you sure about that?", "Quantity Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                If response = MsgBoxResult.No Then

                    e.Cancel = True

                End If
            End If

        End If

        logger.Debug("grdRL_BeforeCellUpdate Exit")

    End Sub

    Private Sub ReparseWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ReparseWorker.DoWork

        Dim eInv As EInvoicingJob = New EInvoicingJob()
        eInv.LoadEInvoicingDataFromString(eInv.GetEInvoiceXML(DirectCast(e.Argument, Integer)))
        eInv.ClearEInvoiceData(DirectCast(e.Argument, Integer))
        eInv.ParseInvoicesFromXML(eInv.XMLData, DirectCast(e.Argument, Integer))

    End Sub

    Private Sub ReparseWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ReparseWorker.RunWorkerCompleted

        RefreshDisplay()
        EnablePanel()
        pbrStatus.Visible = False
        lblReparseStatus.Visible = False

    End Sub

    Private Sub DisablePanel()

        Me.ControlBox = False
        grpReceiveCommands.Enabled = False
        fraDisplay.Enabled = False
        cmdReparseEInvoice.Enabled = False
        Panel1.Enabled = False
        filterGroup.Enabled = False

    End Sub

    Private Sub EnablePanel()

        Me.ControlBox = True
        grpReceiveCommands.Enabled = True
        fraDisplay.Enabled = True
        cmdReparseEInvoice.Enabled = True
        Panel1.Enabled = True
        filterGroup.Enabled = True

    End Sub

    Private Sub grdRL_BeforeEnterEditMode(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles grdRL.BeforeEnterEditMode
        logger.Debug("grdRL_BeforeEnterEditMode Entry")
        If grdRL.ActiveCell.Column.Key = "Weight" Then
            If grdRL.ActiveRow.Cells("CostedByWeight").Value = False Then
                '  Or grdRL.ActiveRow.Cells("isPackageUnit").Value = False
                ' removed this condtion as per the bug fix for bug number # 1287 in 4.2 Release (Active bug list) 
                ' and the requirement document path is ... Connect/team/IRMA/IRMA Requirements/Costed by weight flag vs retail uom.docx
                e.Cancel = True
            End If
        End If

        logger.Debug("grdRL_BeforeEnterEditMode Exit")
    End Sub

    Private Sub grdRL_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles grdRL.CellChange
        logger.Debug("grdRL_CellChange Entry")
        If IsInitializing Then Exit Sub

        If e.Cell.DataChanged Then m_bDataChanged = True
        logger.Debug("grdRL_CellChange Exit")
    End Sub

    Private Sub grdRL_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdRL.InitializeLayout

        e.Layout.Bands(0).Columns("Code").ValueList = Me.uddReasonCode

    End Sub
    Private Sub grdRL_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdRL.KeyDown
        logger.Debug("grdRL_KeyDown Entry")
        Select Case e.KeyValue
            Case Keys.Up
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right Or Keys.Tab
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                grdRL.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select
        logger.Debug("grdRL_KeyDown Exit")

    End Sub


    Private Sub chkShowVendorDescriptions_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowVendorDescriptions.CheckedChanged
        With grdRL.DisplayLayout.Bands(0)
            .Columns("VendorItemDescription").Width = .Columns("Description").Width
            .Columns("VendorItemDescription").Hidden = Not chkShowVendorDescriptions.Checked
            .Columns("Description").Hidden = chkShowVendorDescriptions.Checked
        End With
    End Sub

    Private Sub partialShippmentCheck_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles partialShippmentCheck.CheckedChanged
        If partialShippmentCheck.Checked Then
            With grdRL.DisplayLayout.Bands(0)
                .Columns("Code").Hidden = True
            End With
        Else
            With grdRL.DisplayLayout.Bands(0)
                .Columns("Code").Hidden = False
            End With
        End If
    End Sub

	Private Sub uddReasonCode_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles uddReasonCode.InitializeLayout

	End Sub

	Private Sub chkPastReceipt_CheckedChanged(sender As Object, e As EventArgs) Handles chkPastReceipt.CheckedChanged
		If (chkPastReceipt.Checked) Then
			dtpPastReceiptDate.Enabled = True
		Else
			dtpPastReceiptDate.Value = Nothing
			dtpPastReceiptDate.Enabled = False
		End If
	End Sub

	Private Sub dtpPastReceiptDate_Leave(sender As Object, e As EventArgs) Handles dtpPastReceiptDate.Leave
		If chkPastReceipt.Checked And dtpPastReceiptDate.Value < Date.Today.AddDays(-90) Or dtpPastReceiptDate.Value >= Date.Today Then
			MessageBox.Show("Past Receipt Date has to be within the last 90 days from today.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
			dtpPastReceiptDate.Focus()
		End If
	End Sub
End Class
