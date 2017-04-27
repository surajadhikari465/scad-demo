Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility

Friend Class frmOrderList
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

	Private rsReport As ADODB.Recordset
	Private m_bItemsOrdered As Boolean
	Private m_bPre_Order As Boolean
	Private m_bLoading As Boolean
	Private m_bIsExternalVendor As Boolean
	
	Private m_bIsVendorAFacility As Boolean
	Private m_lLimitSubTeam_No As Integer
	Private m_bIsVendorStoreSame As Boolean
    Private m_lLimitStore_No As Integer
    Private m_ProductType_ID As Integer
	
    Private msWeightBeforeChange As String
    Private m_VendorDiscountAmt As Decimal

    Private mdt As DataTable
    Private mdv As DataView
    Private sOrder As String

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Const mcNumFieldLenNoDec As Short = 5

#Region " Private Properies"

    Private Property EXEOrder() As Boolean
        Get
            Return Me.fraEXE.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.fraEXE.Visible = value
        End Set
    End Property

    Public WriteOnly Property ProductType_ID() As Integer
        Set(ByVal value As Integer)
            Me.m_ProductType_ID = value
        End Set
    End Property


#End Region

    Private Sub frmOrderList_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrderList_FormClosing Entry")

        If e.CloseReason = CloseReason.None Then
            e.Cancel = True
        Else
            'Populate the OrderedCost for the newly created PO
            SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", glOrderHeaderID), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("frmOrderList_FormClosing Exit")
    End Sub
	
	Private Sub frmOrderList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderList_Load Entry")

        CenterForm(Me)

        btnConversionCalculator.Visible = OrderSearchDAO.IsMultipleJurisdiction()

		' Set the caption to reflect the order type
		Select Case Global_Renamed.geOrderType
			Case Global_Renamed.enumOrderType.Distribution
				Text = "Distribution Order - List View"
			Case Global_Renamed.enumOrderType.Purchase
				Text = "Purchase Order - List View"
			Case Global_Renamed.enumOrderType.Transfer
                Text = "Transfer Order - List View"
            Case Global_Renamed.enumOrderType.Flowthru
                Text = "Flowthrough Order - List View"
        End Select

        SetOptions()
        FilterGrid()

        logger.Debug("frmOrderList_Load Exit")
		
	End Sub
	
    Public Sub InitializeForm()
        logger.Debug("InitializeForm Entry")

        Dim sProductType As String
        Dim sSubTeam As String
        Dim sDistSubTeam As String
        sSubTeam = String.Empty
        sDistSubTeam = String.Empty
        sProductType = String.Empty

        Cursor = Cursors.WaitCursor

        m_bLoading = True

        CenterForm(Me)

        'Set the product type.
        Select Case Global_Renamed.geProductType
            Case Global_Renamed.enumProductType.Product
                sProductType = "Product"
            Case Global_Renamed.enumProductType.PackagingSupplies
                sProductType = "Packaging"
            Case Global_Renamed.enumProductType.OtherSupplies
                sProductType = "Supplies"
        End Select

        ' Set the caption to reflect the order type
        Select Case Global_Renamed.geOrderType
            Case Global_Renamed.enumOrderType.Distribution
                Text = "Distribution " & sProductType & " Order - List View"
            Case Global_Renamed.enumOrderType.Purchase
                Text = "Purchase " & sProductType & " Order - List View"
            Case Global_Renamed.enumOrderType.Transfer
                Text = "Transfer " & sProductType & " Order - List View"
            Case Global_Renamed.enumOrderType.Flowthru
                Text = "Flowthrough " & sProductType & " Order - List View"
        End Select

        m_bIsExternalVendor = frmOrders.IsExternalVendor

        '****************************
        Call SetupDataTable()
        '****************************

        'Setup SubTeam Stuff.
        If ((cmbSubTeam.SelectedIndex > -1) And (cmbSubTeam.Enabled = False)) Then
            ' the user is limited to the selected subteam, so limit the data coming back to the .mdb
            sSubTeam = CStr(VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        Else
            ' don't limit yet, user will need all subteams pulled over to .mdb for further filtering
            sSubTeam = "NULL"
        End If

        If ((cmbDistSubTeam.SelectedIndex > -1) And (cmbDistSubTeam.Items.Count > 1)) Then
            ' the user is limited to the selected distribution subteam, so limit the data coming back to the .mdb
            sDistSubTeam = CStr(VB6.GetItemData(cmbDistSubTeam, cmbDistSubTeam.SelectedIndex))
        Else
            ' don't limit yet, user will need all subteams pulled over to .mdb for further filtering
            sDistSubTeam = "NULL"
        End If

        If frmOrders.IsCredit Then Call LoadCreditReasonsII()
        Call LoadUnitsII()

		Me.chkVendorItemStatus_NotAvailable.CheckState = CheckState.Unchecked
		Me.chkVendorItemStatus_Seasonal.CheckState = CheckState.Unchecked
		Me.chkVendorItemStatus_MfgDiscontinued.CheckState = CheckState.Unchecked
		Me.chkVendorItemStatus_VendorDiscontinued.CheckState = CheckState.Unchecked

		'***********************************************************************
		Call LoadDataTable(sSubTeam, sDistSubTeam)
		'***********************************************************************

		Me.ugrdOrderList.DisplayLayout.Bands(0).Columns("CreditReason_ID").Hidden = Not frmOrders.IsCredit
		Me.ugrdOrderList.DisplayLayout.Bands(0).Columns("VendorItemStatusSort").Hidden = True

		If frmOrders.AllowQuantityUnitSelection Then
			Me.ugrdOrderList.DisplayLayout.Bands(0).Columns("QuantityUnit").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
		Else
			Me.ugrdOrderList.DisplayLayout.Bands(0).Columns("QuantityUnit").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		End If

        btnApplyAllCreditReason.Visible = frmOrders.IsCredit

		Call SetOptions()

		FilterGrid()

		Cursor = Cursors.Arrow

		logger.Debug("InitializeForm Exit")

me_exit:

		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
		m_bLoading = False
		logger.Debug("InitializeForm Exit")

	End Sub


	Private Sub SetupDataTable()
		logger.Debug("SetupDataTable Entry")

		' Create a data table
		mdt = New DataTable("OrderItemListView")
		Dim Keys(1) As DataColumn
		Dim dc As DataColumn

		mdt.PrimaryKey = Keys

		'Visible on grid.
		'--------------------
		mdt.Columns.Add(New DataColumn("VendorItemID", GetType(String)))
        mdt.Columns.Add(New DataColumn("Cost", GetType(Decimal)))
		mdt.Columns.Add(New DataColumn("Brand", GetType(String)))
		mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
		mdt.Columns.Add(New DataColumn("NA", GetType(String)))
		mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityOrdered", GetType(Decimal)))
		mdt.Columns.Add(New DataColumn("QuantityUnit", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Reg Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("PkgDesc", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemStatus", GetType(String)))
		mdt.Columns.Add(New DataColumn("CreditReason_ID", GetType(Integer)))

		'Hidden.
		'--------------------
		dc = New DataColumn("Item_Key", GetType(Integer))
		mdt.Columns.Add(dc)
		Keys(0) = dc

		mdt.Columns.Add(New DataColumn("OrderItem_ID", GetType(Integer)))
		mdt.Columns.Add(New DataColumn("Category_Name", GetType(String)))
		mdt.Columns.Add(New DataColumn("Category_ID", GetType(Integer)))
		mdt.Columns.Add(New DataColumn("Pre_Order", GetType(Boolean)))
		mdt.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
		mdt.Columns.Add(New DataColumn("EXEDistributed", GetType(Boolean)))
		mdt.Columns.Add(New DataColumn("DistSubTeam_No", GetType(Integer)))
		mdt.Columns.Add(New DataColumn("Discontinue_Item", GetType(Boolean)))
		mdt.Columns.Add(New DataColumn("VendorCostHistoryId", GetType(Integer)))
		mdt.Columns.Add(New DataColumn("VendorItemStatusFull", GetType(String)))
		mdt.Columns.Add(New DataColumn("VendorItemStatusSort", GetType(String)))

        mdt.PrimaryKey = Keys

        logger.Debug("SetupDataTable Exit")

    End Sub

    Private Function LoadDataTable(ByVal sSubTeam As String, ByVal sDistSubTeam As String) As Boolean

        logger.Debug("LoadDataTable Entry")
        Dim dQuantity As Decimal
        Dim rsOrderList As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim bSkippedRow As Boolean
        Dim sSQL As String = ""
        Dim ds As DataSet
        Dim showCost As Boolean = True 'in modifying this to set a variable, it seems the default was true, so it is being left as such

        If ConfigurationServices.AppSettings("OrderListViewCost") = "0" Then
            showCost = False
        End If

        Try
            If _ReceiveLocationIsDistribution Then
                ds = OrderingDAO.GetOrderItemList("AutomaticOrderListPackSizes", glOrderHeaderID, sSubTeam, sDistSubTeam, IIf(gbExcludeNot_Available = True, 0, "Null"), m_ProductType_ID)
            Else
                If showCost = False Then
                    ds = OrderingDAO.GetOrderItemList("AutomaticOrderListNoCost", glOrderHeaderID, sSubTeam, sDistSubTeam, IIf(gbExcludeNot_Available = True, 0, "Null"), m_ProductType_ID)
                Else
                    ds = OrderingDAO.GetOrderItemList("AutomaticOrderList", glOrderHeaderID, sSubTeam, sDistSubTeam, IIf(gbExcludeNot_Available = True, 0, "Null"), m_ProductType_ID)
                End If

            End If

            If ds.Tables(0).Rows.Count > 0 Then
                For x As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    With ds.Tables(0)
                        If (IIf(IsDBNull(.Rows(x)("OrderCount")), 0, .Rows(x)("OrderCount")) > 1) Or (CType(IIf(IsDBNull(.Rows(x)("QuantityReceived")), 0, .Rows(x)("QuantityReceived")), Decimal) > 0) Then
                            bSkippedRow = True
                        Else
                            row = mdt.NewRow
                            row("Identifier") = .Rows(x)("Identifier")
                            row("NA") = IIf(.Rows(x)("Not_Available"), IIf(Not IsDBNull(.Rows(x)("Not_AvailableNote")), "*..." & .Rows(x)("Not_AvailableNote"), "*"), "")
                            row("Item_Description") = .Rows(x)("Item_Description")
                            row("QuantityOrdered") = .Rows(x)("QuantityOrdered")
                            dQuantity = IIf(IsDBNull(.Rows(x)("QuantityOrdered")), 0, .Rows(x)("QuantityOrdered"))
                            If dQuantity > 0 Then m_bItemsOrdered = True
                            row("QuantityUnit") = .Rows(x)("QuantityUnit")

                            If .Rows(x)("Package_Desc1") IsNot DBNull.Value AndAlso .Rows(x)("Package_Desc2") IsNot DBNull.Value Then
                                row("PkgDesc") = CType(.Rows(x)("Package_Desc1"), Decimal).ToString("#####.##") & "/" & CType(.Rows(x)("Package_Desc2"), Decimal).ToString("#####.##") & " " & .Rows(x)("Package_Unit")
                            End If

                            row("CreditReason_ID") = .Rows(x)("CreditReason_ID")
                            row("Item_Key") = .Rows(x)("Item_Key")
                            row("OrderItem_ID") = .Rows(x)("OrderItem_ID")
                            row("Category_Name") = .Rows(x)("Category_Name")
                            row("Category_ID") = .Rows(x)("Category_ID")
                            row("Pre_Order") = .Rows(x)("Pre_Order")
                            If (.Rows(x)("Pre_Order")) And (dQuantity > 0) Then m_bPre_Order = True
                            row("SubTeam_No") = .Rows(x)("SubTeam_No")
                            row("EXEDistributed") = .Rows(x)("EXEDistributed")
                            row("DistSubTeam_No") = .Rows(x)("DistSubTeam_No")
                            row("Discontinue_Item") = .Rows(x)("Discontinue_Item")
                            row("VendorCostHistoryId") = .Rows(x)("VendorCostHistoryId")
                            row("VendorItemID") = .Rows(x)("VendorItemID")
                            row("Brand") = .Rows(x)("Brand")
                            If showCost = True Then
                                row("Cost") = .Rows(x)("Cost")
                                row("Reg Cost") = .Rows(x)("CurrentVendorCost")
                            End If
                            row("VendorItemStatus") = .Rows(x)("VendorItemStatus")
                            row("VendorItemStatusFull") = .Rows(x)("VendorItemStatusFull")
                            row("VendorItemStatusSort") = .Rows(x)("VendorItemStatusSort")
                            mdt.Rows.Add(row)
                        End If
                    End With
                Next
            End If
        Finally
            ds = Nothing
        End Try

        mdt.AcceptChanges()

        mdv = New System.Data.DataView(mdt)


        '################################################################################################################################
        ' BUYSIDE Vendor Item Attributes: Please Read! (Robin Eudy: 2/14/2011)
        ' Adding CLIENT SIDE filters for VendorItemStatus. The filters on the uderlying search Stored Proc are unchanged for now.
        ' The data to populate VendorItemStatus from IMHA/VIP does not exist at this point and time. All data in this field will be NULL
        ' To avoid slowing down the search queries with criteria that doesnt exist I have decided to make this filter CLIENT SIDE instead of 
        ' adding filters to  the Stored Proc. THIS SHOULD BE CHANGED once the VendorItemStatus starts to be populated.
        ' Brian Strickland - moved this to the filter grid function to keep it in one place and to have it where it could be actuated from
        ' events on the appropriate controls.
        '#################################################################################################################################


        'Dim _rowfilter As String = String.Empty
        '' ## Show NULL and Active values for VendorItemStatus by default.
        '_rowfilter = "isnull(VendorItemStatus,'null') = 'null' OR VendorItemStatus = '' OR VendorItemStatus = 'A' OR"

        '' ## Show other Statuses if they are checked.
        'If chkVendorItemStatus_MfgDiscontinued.Checked Then _rowfilter += " VendorItemStatus = 'M' OR"
        'If chkVendorItemStatus_NotAvailable.Checked Then _rowfilter += " VendorItemStatus = 'N' OR"
        'If chkVendorItemStatus_VendorDiscontinued.Checked Then _rowfilter += " VendorItemStatus = 'V' OR"
        'If chkVendorItemStatus_Seasonal.Checked Then _rowfilter += " VendorItemStatus = 'S' OR"

        '' ## Clean up the filter string we generated. (remove the final OR that isnt needed.)
        'If _rowfilter.EndsWith(" OR") Then _rowfilter = _rowfilter.Remove(_rowfilter.Length - 3)
        '' ## Apply filter.
        'mdv.RowFilter = _rowfilter

        '################################################################################################################################
        '################################################################################################################################


        Me.ugrdOrderList.DataSource = mdv

        If bSkippedRow Then MsgBox("Some items cannot be displayed because they are on the order more than once or they have been received", MsgBoxStyle.Exclamation, Me.Text)
        logger.Debug("LoadDataTable Exit")
ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("LoadDataTable Exit")
    End Function

    Private Sub LoadCreditReasonsII()
        logger.Debug("LoadCreditReasonsII Entry")

        Dim rsOrderList As DAO.Recordset = Nothing
        Try
            rsOrderList = SQLOpenRecordSet("EXEC GetCreditReasons ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsOrderList.EOF
                Me.ugrdOrderList.DisplayLayout.ValueLists("CreditReasons").ValueListItems.Add(rsOrderList.Fields("CreditReason_ID").Value, rsOrderList.Fields("CreditReason").Value)
                rsOrderList.MoveNext()
            End While
        Finally
            If rsOrderList IsNot Nothing Then
                rsOrderList.Close()
                rsOrderList = Nothing
            End If
        End Try

        logger.Debug("LoadCreditReasonsII Exit")

    End Sub

    Private Sub LoadUnitsII()
        logger.Debug("LoadUnitsII Entry")

        Dim rsOrderList As DAO.Recordset = Nothing

        Try
            rsOrderList = SQLOpenRecordSet("EXEC GetUnitAndID", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsOrderList.EOF
                Me.ugrdOrderList.DisplayLayout.ValueLists("QuantityUnits").ValueListItems.Add(rsOrderList.Fields("Unit_ID").Value, rsOrderList.Fields("Unit_Name").Value)
                rsOrderList.MoveNext()
            End While
        Finally
            If rsOrderList IsNot Nothing Then
                rsOrderList.Close()
                rsOrderList = Nothing
            End If
        End Try
        logger.Debug("LoadUnitsII Exit")

    End Sub

    Private Sub SetOptions()
        logger.Debug("SetOptions Entry")

        If Global_Renamed.geOrderType = enumOrderType.Flowthru Then
            Me.optPreOrder(1).Checked = True
            Me.optPreOrder(1).Enabled = False
            Me.optPreOrder(2).Enabled = False
        Else
            If m_bItemsOrdered Then
                If m_bPre_Order Then
                    Me.optPreOrder(1).Checked = True
                Else
                    Me.optPreOrder(2).Checked = True
                End If
            Else
                Me.optPreOrder(2).Checked = True
            End If
        End If

        If frmOrders.EXEWarehouse Then
            EXEOrder = True
            If m_bItemsOrdered Then
                optEXE(0).Enabled = False
                optEXE(1).Enabled = False
                If frmOrders.EXEOrder Then
                    optEXE(0).Checked = True
                Else
                    optEXE(1).Checked = True
                End If
            Else
                optEXE(0).Enabled = True
                optEXE(1).Enabled = True
            End If
        Else
            EXEOrder = False
        End If

        If Global_Renamed.geOrderType = enumOrderType.Purchase Or Global_Renamed.geOrderType = enumOrderType.Transfer Then
            chkIncludeNotAvailable.Checked = True
        Else
            chkIncludeNotAvailable.Checked = False
        End If

        logger.Debug("SetOptions Exit")
    End Sub

    Private Sub chkOrdered_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkOrdered.CheckStateChanged

        logger.Debug("chkOrdered_CheckStateChanged Entry")
        If Me.IsInitializing Then Exit Sub

        FilterGrid()
        logger.Debug("chkOrdered_CheckStateChanged Exit")

    End Sub

    Private Sub cmbDistSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDistSubTeam.KeyPress

        logger.Debug("cmbDistSubTeam_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbDistSubTeam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbDistSubTeam_KeyPress Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        '-- Make sure that they don't want to save their changes
        If Not (mdt.GetChanges Is Nothing) Then
            If MsgBox("Really exit without saving your changes?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
                logger.Debug("cmdExit_Click Exit without saving your changes ")
                Exit Sub
            End If
        End If

        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub

    Private Sub cmdSubmit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSubmit.Click

        logger.Debug("cmdSubmit_Click Entry")

        Dim rsOrderItem As DAO.Recordset = Nothing
        Dim dQuantityOrdered As Decimal
        Dim sLandedCost As Decimal
        Dim sLineItemCost As Decimal
        Dim sLineItemFreight As Decimal
        Dim sUnitCost As Decimal
        Dim sUnitExtCost As Decimal
        Dim sDiscountCost As Decimal
        Dim sHandlingCharge As Decimal
        Dim sMarkupCost As Decimal
        Dim sMarkupPercent As Decimal
        Dim sCost As Decimal
        Dim sFreight As Decimal
        Dim iCost_Unit As Integer
        Dim iFreight_Unit As Integer
        Dim i As Integer
        Dim lIgnoreErrNum(0) As Integer
        Dim newOrderItemId As Integer

        '-- Check to see if anything was really modified
        Dim cdt As DataTable = mdt.GetChanges(DataRowState.Modified)
        If Not (cdt Is Nothing) Then

            If frmOrders.IsCredit Then
                Dim sdr() As DataRow = mdt.Select("QuantityOrdered > 0 AND CreditReason_ID IS NULL")
                If sdr.Length > 0 Then
                    sdr = Nothing
                    MsgBox("You must enter a credit reason for items with a quantity greater than zero", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Credit Reason?")
                    logger.Info("cmdSubmit_Click " & " You must enter a credit reason for items with a quantity greater than zero")
                    logger.Debug("cmdSubmit_Click exit")
                    Exit Sub
                End If
                sdr = Nothing
            End If

            Dim dr As DataRow
            Dim cu As tItemUnit
            Dim fu As tItemUnit

            For Each dr In cdt.Rows

                dQuantityOrdered = IIf(IsDBNull(dr("QuantityOrdered")), 0, dr("QuantityOrdered"))

                If (dQuantityOrdered = 0) And (Not IsDBNull(dr("OrderItem_ID"))) Then

                    '-- Delete an order item
                    lIgnoreErrNum(0) = 50002

                    On Error Resume Next
                    SQLExecute3("EXEC DeleteOrderItem " & dr("OrderItem_ID") & "," & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
                    If Err.Number <> 0 Then
                        logger.Error("cmdSubmit_Click " & " Error in EXEC DeleteOrderItem " & Err.Description)
                        MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
                        On Error GoTo 0
                        SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        logger.Debug("cmdSubmit_Click exit")
                        Exit Sub
                    End If
                    On Error GoTo 0

                ElseIf (dQuantityOrdered > 0) And IsDBNull(dr("OrderItem_ID")) Then


                    '-- Create a new orderitem, use one SP for ListView with avgcost and another with ListView with no avgcost
                    If ConfigurationServices.AppSettings("OrderListViewCost").ToString = "0" Then
                        rsOrderItem = SQLOpenRecordSet("EXEC AutomaticOrderItemInfo " & dr("Item_Key") & ", " & glOrderHeaderID & ", NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    Else
                        rsOrderItem = SQLOpenRecordSet("EXEC AutomaticOrderItemInfoPackSize " & dr("Item_Key") & ", " & glOrderHeaderID & ", NULL, " & dr("VendorCostHistoryId"), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    End If

                    With rsOrderItem

                        '-- Pre markup Cost
                        sCost = IIf((frmOrders.OrderDiscountExists And frmOrders.OrderDiscountType = 2) Or .Fields("DiscountType").Value = 2, .Fields("OriginalCost").Value, .Fields("Cost").Value)

                        ' Fixes for bugs 5430 & 5418
                        If IsDBNull(.Fields("CostUnit").Value) Then
                            logger.Warn("cmdSubmit_Click - Unable to add the selected item to the order because it does not have a Cost Unit ID assigned: Item_Key=" + dr("Item_Key").ToString + ", glOrderHeaderID=" + glOrderHeaderID.ToString)
                            MsgBox(String.Format("BAD DATA: This item ({0}) has no Cost Unit ID assigned to it. This must be assigned in order to add it to the order." & _
                              vbCrLf & vbCrLf & "Please close the empty Line Item Information screen that follows.", dr("Identifier")), MsgBoxStyle.Critical)

                            rsOrderItem.Close()
                            logger.Debug("cmdSubmit_Click Exit")
                            Exit Sub
                        Else
                            iCost_Unit = .Fields("CostUnit").Value
                        End If

                        m_VendorDiscountAmt = .Fields("VendorNetDiscount").Value

                        Select Case .Fields("DiscountType").Value
                            Case 0
                                If m_VendorDiscountAmt > 0 Then
                                    ' Sekhara to fix the bug 6134.
                                    ' sDiscountCost = sCost - (m_VendorDiscountAmt * dQuantityOrdered)
                                    ' ' Bug 12492 & 12618 the vendor discount amount is now being subtracted in the stored proc
                                    sDiscountCost = sCost '- m_VendorDiscountAmt

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
                                'Case 2 : sDiscountCost = sCost * ((100 - .Fields("QuantityDiscount").Value) / 100)
                            Case 2 : sDiscountCost = sCost - (sCost * (.Fields("QuantityDiscount").Value / 100))
                            Case Else : sDiscountCost = sCost
                        End Select

                        sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered

                        '-- Pre markup Freight
                        sFreight = .Fields("Freight").Value
                        iFreight_Unit = .Fields("FreightUnit").Value
                        sLineItemFreight = CostConversion(sFreight, iFreight_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered

                        sLandedCost = (sLineItemCost + sLineItemFreight) / dQuantityOrdered

                        '-- Markup
                        cu = GetItemUnit(iCost_Unit)
                        fu = GetItemUnit(iFreight_Unit)

                        sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered
                        sLineItemFreight = CostConversion(sFreight * (.Fields("MarkupPercent").Value + 100) / 100, iFreight_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered
                        sUnitCost = CostConversion(sLineItemCost / dQuantityOrdered, dr("QuantityUnit"), IIf(cu.IsPackageUnit, giUnit, iCost_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
                        sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / dQuantityOrdered, dr("QuantityUnit"), IIf(fu.IsPackageUnit, giUnit, iFreight_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
                        sHandlingCharge = .Fields("HandlingCharge").Value
                        sMarkupCost = ((sLineItemCost + sLineItemFreight) / dQuantityOrdered) + sHandlingCharge

                        lIgnoreErrNum(0) = 50002

                        On Error Resume Next

                        Dim sSQL As String = "EXEC InsertOrderItemCredit " & glOrderHeaderID & ", " & _
                            dr("Item_Key") & ", " & _
                            .Fields("Units_Per_Pallet").Value & ", " & _
                            dr("QuantityUnit") & ", " & _
                            dQuantityOrdered & ", " & _
                            sCost & ", " & _
                            iCost_Unit & ", " & _
                            .Fields("Handling").Value & ", " & _
                            .Fields("HandlingUnit").Value & ", " & _
                            sFreight & ", " & _
                            iFreight_Unit & ", " _
                            & .Fields("AdjustedCost").Value & ", " & _
                            .Fields("QuantityDiscount").Value & ", " & _
                            .Fields("DiscountType").Value & ", " & _
                            sLandedCost & ", " & _
                            sLineItemCost & ", " & _
                            sLineItemFreight & ", " & _
                            0 & ", " & _
                            sUnitCost & ", " & _
                            sUnitExtCost & ", " & _
                            .Fields("Package_Desc1").Value & ", " & _
                            .Fields("Package_Desc2").Value & ", " & _
                            .Fields("Package_Unit_ID").Value & ", " & _
                            .Fields("MarkupPercent").Value & ", " & _
                            sMarkupCost & ", " & _
                            .Fields("Retail_Unit_ID").Value & ", " & _
                            IIf(IsDBNull(dr("CreditReason_ID")), "NULL", dr("CreditReason_ID")) & "," & _
                            giUserID & "," & _
                            m_VendorDiscountAmt & ", " & IIf(IsDBNull(.Fields("HandlingCharge").Value), 0, .Fields("HandlingCharge").Value)

                        'SQLExecute3(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
                        newOrderItemId = OrderingDAO.InsertOrderItem(sSQL)

                        If Err.Number <> 0 Then
                            logger.Error("cmdSubmit_Click " & " Error in EXEC InsertOrderItemCredit " & Err.Description)
                            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
                            On Error GoTo 0
                            SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            rsOrderItem.Close()
                            logger.Debug("cmdSubmit_Click exit")
                            Exit Sub
                        ElseIf newOrderItemId > 0 Then  'Fix for TFS #2097. If the OrderItem has been inserted, get the identity key and update the row.
                            mdt.Rows.Find(dr("Item_Key"))("OrderItem_ID") = newOrderItemId
                            mdt.Rows.Find(dr("Item_Key")).AcceptChanges()
                        End If
                        On Error GoTo 0
                    End With
                    rsOrderItem.Close()

                ElseIf (dQuantityOrdered > 0) And (Not IsDBNull(dr("OrderItem_ID"))) Then
                    '-- update an existing orderitem
                    rsOrderItem = SQLOpenRecordSet("EXEC GetOrderItemInfo " & dr("OrderItem_ID") & ", " & glOrderHeaderID & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    With rsOrderItem

                        '-- Pre markup Cost
                        sCost = .Fields("CurrentVendorCost").Value
                        iCost_Unit = .Fields("CostUnit").Value

                        If IsDBNull(rsOrderItem.Fields("NetVendorItemDiscount").Value) Or rsOrderItem.Fields("NetVendorItemDiscount").Value = 0 Then
                            m_VendorDiscountAmt = 0
                        Else
                            m_VendorDiscountAmt = Convert.ToDecimal(rsOrderItem.Fields("NetVendorItemDiscount").Value)
                        End If

                        Select Case .Fields("DiscountType").Value
                            Case 0
                                If m_VendorDiscountAmt > 0 Then

                                    ' Sekhara to fix the bug 6134.
                                    ' sDiscountCost = sCost - (m_VendorDiscountAmt * dQuantityOrdered)
                                    sDiscountCost = sCost - m_VendorDiscountAmt

                                ElseIf frmOrders.OrderDiscountExists Then
                                    Select Case frmOrders.OrderDiscountType
                                        Case 1 : sDiscountCost = sCost - frmOrders.OrderDiscountAmt
                                        Case 2 : sDiscountCost = sCost - (sCost * (frmOrders.OrderDiscountAmt / 100))
                                        Case 4 : sDiscountCost = sCost - (sCost * (frmOrders.OrderDiscountAmt / 100))
                                    End Select
                                Else  ' To fix the bug 6134.
                                    sDiscountCost = sCost
                                End If


                            Case 1 : sDiscountCost = sCost - .Fields("QuantityDiscount").Value
                                'Case 2 : sDiscountCost = sCost * ((100 - .Fields("QuantityDiscount").Value) / 100)

                            Case 2 : sDiscountCost = sCost - (sCost * (.Fields("QuantityDiscount").Value / 100))
                            Case Else : sDiscountCost = sCost
                        End Select
                        sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered

                        '-- Pre markup Freight
                        sFreight = .Fields("Freight").Value
                        iFreight_Unit = .Fields("FreightUnit").Value

                        sLineItemFreight = CostConversion(sFreight, iFreight_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered
                        sLandedCost = (sLineItemCost + sLineItemFreight) / dQuantityOrdered

                        '-- Markup
                        cu = GetItemUnit(iCost_Unit)
                        fu = GetItemUnit(iFreight_Unit)

                        sLineItemCost = CostConversion(sDiscountCost * (.Fields("MarkupPercent").Value + 100) / 100, iCost_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered
                        sLineItemFreight = CostConversion(.Fields("Freight").Value * (.Fields("MarkupPercent").Value + 100) / 100, iFreight_Unit, dr("QuantityUnit"), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * dQuantityOrdered
                        sUnitCost = CostConversion(sLineItemCost / dQuantityOrdered, dr("QuantityUnit"), IIf(cu.IsPackageUnit, giUnit, iCost_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
                        sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / dQuantityOrdered, dr("QuantityUnit"), IIf(fu.IsPackageUnit, giUnit, iFreight_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
                        sMarkupPercent = .Fields("MarkupPercent").Value
                        sMarkupCost = ((sLineItemCost + sLineItemFreight) / dQuantityOrdered) + .Fields("HandlingCharge").Value

                        lIgnoreErrNum(0) = 50002

                        ' For the following UpdateOrderItemInfo query execution, IsDBNull condition has been added to Freight3Party and LineItemFreight3Party.
                        ' To fix the Bug 6069.

                        On Error Resume Next
                        SQLExecute3("EXEC UpdateOrderItemInfo " & _
                         .Fields("OrderItem_ID").Value & ", " & _
                         dQuantityOrdered & ", " & _
                         "NULL, " & _
                         "0, " & _
                         .Fields("Units_Per_Pallet").Value & ", " & _
                         .Fields("Cost").Value & ", " & _
                         .Fields("Handling").Value & ", " & _
                         .Fields("Freight").Value & ", " & _
                         .Fields("AdjustedCost").Value & ", " & _
                         sLandedCost & ", " & _
                         .Fields("QuantityDiscount").Value & ", " & _
                         sLineItemCost & ", " & _
                         sLineItemFreight & ", " & _
                         "0, " & _
                         "0, " & _
                         "0, " & _
                         "0, " & _
                         IIf(IsDBNull(.Fields("Freight3Party").Value), "NULL", .Fields("Freight3Party").Value) & ", " & _
                         IIf(IsDBNull(.Fields("LineItemFreight3Party").Value), "NULL", .Fields("LineItemFreight3Party").Value) & ", " & _
                         sUnitCost & ", " & _
                         sUnitExtCost & ", " & _
                         dr("QuantityUnit") & ", " & _
                         .Fields("CostUnit").Value & ", " & _
                         If(IsDBNull(.Fields("HandlingUnit").Value), "NULL", .Fields("HandlingUnit").Value) & ", " & _
                         .Fields("FreightUnit").Value & ", " & _
                         .Fields("DiscountType").Value & ", " & _
                         TextValue("'" & .Fields("DateReceived").Value & "'") & ", " & _
                         IIf(IsDBNull(.Fields("OriginalDateReceived").Value), "NULL", "'" & .Fields("OriginalDateReceived").Value & "'") & ", " & _
                         TextValue("'" & .Fields("ExpirationDate").Value & "'") & ", " & _
                         sMarkupPercent & ", " & _
                         sMarkupCost & ", " & _
                         .Fields("Package_Desc1").Value & ", " & _
                         .Fields("Package_Desc2").Value & ", " & _
                         .Fields("Package_Unit_ID").Value & ", " & _
                         .Fields("Retail_Unit_ID").Value & ", " & _
                         IIf(IsDBNull(.Fields("Origin_ID").Value), "NULL", .Fields("Origin_ID").Value) & ", " & _
                         IIf(IsDBNull(.Fields("CountryProc_ID").Value), "NULL", .Fields("CountryProc_ID").Value) & ", " & _
                         IIf(IsDBNull(dr("CreditReason_ID")), "NULL", dr("CreditReason_ID")) & ", " & _
                         "NULL, " & _
                         giUserID & "," & _
                         IIf(IsDBNull(.Fields("Lot_no").Value), "NULL", "'" & .Fields("Lot_no").Value & "'") & ", " & _
                         IIf(IsDBNull(.Fields("CostAdjustmentReason_ID").Value), "NULL", .Fields("CostAdjustmentReason_ID").Value) & ", " & _
                         IIf(IsDBNull(.Fields("HandlingCharge").Value), 0, .Fields("HandlingCharge").Value) & ", " & _
                         "NULL, " & _
                         IIf(IsDBNull(.Fields("SustainabilityRankingID").Value), "NULL", .Fields("SustainabilityRankingID").Value), _
                         DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)

                        If Err.Number <> 0 Then
                            logger.Error("cmdSubmit_Click " & " Error in EXEC UpdateOrderItemInfo " & Err.Description)
                            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
                            On Error GoTo 0
                            SQLExecute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            rsOrderItem.Close()
                            logger.Debug("cmdSubmit_Click exit")
                            Exit Sub
                        End If
                        On Error GoTo 0

                        rsOrderItem.Close()

                    End With
                End If
            Next
            dr = Nothing
        End If
        cdt = Nothing

        Me.Close()

        logger.Debug("cmdSubmit_Click Exit")

    End Sub

	Private Sub frmOrderList_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

		logger.Debug("frmOrderList_Activated Entry")
		If Me.Tag = "U" Then
			Me.Close()
		End If
		logger.Debug("frmOrderList_Activated Exit")

	End Sub

	Private Sub frmOrderList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

		logger.Debug("frmOrderList_KeyPress Entry")
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

		If KeyAscii = 13 Then
			FilterGrid()
			If ugrdOrderList.Rows.Count = 0 Then MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
		End If

		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		logger.Debug("frmOrderList_KeyPress Exit")

	End Sub

	Private Sub optEXE_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optEXE.CheckedChanged

		logger.Debug("optEXE_CheckedChanged Entry")
		If Me.IsInitializing = True Then Exit Sub

		If eventSender.Checked Then
			Dim Index As Short = optEXE.GetIndex(eventSender)

			If Not m_bLoading Then FilterGrid()

		End If

		logger.Debug("optEXE_CheckedChanged Exit")
	End Sub

	Private Sub optOrder_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optOrder.CheckedChanged, RadioButton_SortByStatus.CheckedChanged

		logger.Debug("optOrder_CheckedChanged Entry")
		If Me.IsInitializing = True Then Exit Sub

		If eventSender.Checked Then

			'Dim Index As Short = optOrder.GetIndex(eventSender)

			If Not m_bLoading Then FilterGrid()

		End If
		logger.Debug("optOrder_CheckedChanged Exit")
	End Sub

	Private Sub optPreOrder_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optPreOrder.CheckedChanged

		logger.Debug("optPreOrder_CheckedChanged Entry")
		If Me.IsInitializing = True Then Exit Sub

		If eventSender.Checked Then
			Dim Index As Short = optPreOrder.GetIndex(eventSender)

			If Not m_bLoading Then
				If m_bItemsOrdered And (Not m_bIsExternalVendor) Then
					m_bLoading = True
					If Index = 1 Then
						optPreOrder(2).Checked = True
					Else
						optPreOrder(1).Checked = True
					End If
					m_bLoading = False
					MsgBox("Pre-Order and Non Pre-Order items cannot be mixed on a Distribution order.  Clear ordered quantities to allow switch.", MsgBoxStyle.Critical, Me.Text)
					logger.Info("optPreOrder_CheckedChanged " & " Pre-Order and Non Pre-Order items cannot be mixed on a Distribution order.  Clear ordered quantities to allow switch.")
				Else
					FilterGrid()
				End If
			End If
		End If

		logger.Debug("optPreOrder_CheckedChanged Exit")
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

	Public Sub LoadSubTeamCombo(ByRef bIsVendorAFacility As Boolean, ByRef bIsVendorStoreSame As Boolean, Optional ByRef lLimitStore_No As Integer = -1, Optional ByRef lOrderHeader_SubTeamNo As Integer = -1)

		logger.Debug("LoadSubTeamCombo Entry")
		Dim eSubTeamType As enumSubTeamType

		m_bIsVendorAFacility = bIsVendorAFacility
		m_bIsVendorStoreSame = bIsVendorStoreSame
		m_lLimitStore_No = lLimitStore_No

		' Apply the rules about what subteams to load
		If (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer) Then
			eSubTeamType = Global_Renamed.enumSubTeamType.All
		Else
			eSubTeamType = DetermineSubTeamType((Global_Renamed.enumSubTeamContext.Item_From), m_bIsVendorAFacility, m_bIsVendorStoreSame, ReceiveLocationIsDistribution)
		End If

		Call LoadSubTeamByType(eSubTeamType, cmbSubTeam, m_lLimitStore_No, lOrderHeader_SubTeamNo)

		If (cmbSubTeam.SelectedIndex < 0) Then
			If (cmbSubTeam.Items.Count = 1) Then
				cmbSubTeam.SelectedIndex = 0
			End If
		End If

		Call LoadDistSubTeam(cmbDistSubTeam)
        Call LoadBrandNew((cmbBrand))
		logger.Debug("LoadSubTeamCombo Exit")

	End Sub


	Private _ReceiveLocationIsDistribution As Boolean
	Public Property ReceiveLocationIsDistribution() As Boolean
		Get
			Return _ReceiveLocationIsDistribution
		End Get
		Set(ByVal value As Boolean)
			_ReceiveLocationIsDistribution = value
		End Set
	End Property


	Private _IsVendorExternal As Boolean
	Public Property IsVendorExternal() As Boolean
		Get
			Return _IsVendorExternal
		End Get
		Set(ByVal value As Boolean)
			_IsVendorExternal = value
		End Set
	End Property


	Public WriteOnly Property StatusLabel() As String
		Set(ByVal Value As String)
			lblStatus.Text = Value
		End Set

	End Property

	Public WriteOnly Property LimitSubTeam_No() As Integer

		Set(ByVal Value As Integer)
			If Value = 0 And cmbSubTeam.Items.Count = 1 Then
				' Use this case to assign the first one in the list as the limit
				' This is used for Packaging and Supplies product types
				Value = VB6.GetItemData(cmbSubTeam, 0)
			End If

			m_lLimitSubTeam_No = Value

			If Value > 0 Then
				SetCombo((cmbSubTeam), Value)
				cmbSubTeam.Enabled = False

				' Set up the Distribution subteam combo if this is locked down
				cmbDistSubTeam.Items.Clear()
				cmbDistSubTeam.Items.Add((cmbSubTeam.Text))
				cmbDistSubTeam.SelectedIndex = 0
				cmbDistSubTeam.Enabled = False
			Else
				cmbSubTeam.SelectedIndex = -1
				cmbSubTeam.Enabled = True
			End If
		End Set

	End Property

	Public WriteOnly Property SubTeam_No() As Integer

		Set(ByVal Value As Integer)
			Dim bEnableDistSubTeam As Boolean
			bEnableDistSubTeam = True

			If (Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Distribution) Then
				' if here, then subteam is not locked down
				' determine if orderheader transfer-from subteam is in the distsubteam combo
				Call SetCombo(cmbDistSubTeam, glSubTeamNo)
				If (cmbDistSubTeam.SelectedIndex > -1) Then
					' lock it down, cause user will never find items under any other distSubteam
					' in this scenario
					bEnableDistSubTeam = False
					'Clear the subteam combo and load with this one that matches this dist subteam
					cmbSubTeam.Items.Clear()
					Call SetRetailSubTeam(VB6.GetItemData(cmbDistSubTeam, cmbDistSubTeam.SelectedIndex), (cmbSubTeam))
				End If
			End If

			If (bEnableDistSubTeam) Then
				Call SetCombo((cmbSubTeam), Value)
				Call SetActive((cmbSubTeam), True)
			Else
				If (cmbSubTeam.SelectedIndex > -1) Then
					Call SetActive((cmbSubTeam), False)
				End If
			End If

			Call SetActive(cmbDistSubTeam, bEnableDistSubTeam)

		End Set

	End Property

	Public WriteOnly Property IncludeDiscontinued() As enumChkBoxValues

		Set(ByVal Value As enumChkBoxValues)
			m_bLoading = True
			Select Case True
				Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
					chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
					If frmOrders.IsCredit Then
						chkDiscontinued.Enabled = True
					Else
						chkDiscontinued.Enabled = False
					End If
				Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
					chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
					chkDiscontinued.Enabled = True
				Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
					chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
					If gbSuperUser Then
						chkDiscontinued.Enabled = True
					Else
						chkDiscontinued.Enabled = False
					End If
				Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
					chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
					chkDiscontinued.Enabled = True
			End Select
			m_bLoading = False
		End Set

	End Property


	Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
		logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")
		If IsInitializing Then Exit Sub

		If cmbSubTeam.SelectedIndex = -1 Then
			cmbCategory.Items.Clear()
		Else
			LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
		End If
		logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")

	End Sub

	Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged
		logger.Debug("cmbSubTeam_TextChanged Entry")
		If cmbSubTeam.Text = "" Then cmbSubTeam.SelectedIndex = -1
		logger.Debug("cmbSubTeam_TextChanged Exit")

	End Sub

	Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress

		logger.Debug("cmbSubTeam_KeyPress Entry")
		Dim KeyAscii As Short = Asc(e.KeyChar)

		If KeyAscii = 8 Then
			cmbSubTeam.SelectedIndex = -1
		End If


		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
		logger.Debug("cmbSubTeam_KeyPress Exit")
	End Sub

	Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = Keys.Back Then cmbBrand.SelectedIndex = -1
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = Keys.None Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress

		logger.Debug("cmbCategory_KeyPress Entry")
		Dim KeyAscii As Short = Asc(e.KeyChar)

		If KeyAscii = 8 Then
			cmbCategory.SelectedIndex = -1
		End If

		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
		logger.Debug("cmbCategory_KeyPress Exit")
	End Sub

	Private Sub ugrdOrderList_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdOrderList.AfterCellActivate

		logger.Debug("ugrdOrderList_AfterCellActivate Entry")
		If ugrdOrderList.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
			ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
		End If
		logger.Debug("ugrdOrderList_AfterCellActivate Exit")

	End Sub

	Private Sub ugrdOrderList_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdOrderList.AfterRowUpdate

		logger.Debug("ugrdOrderList_AfterRowUpdate Entry")
		Dim sdt() As DataRow = mdt.Select("QuantityOrdered > 0")
		If sdt.Length > 0 Then
			m_bItemsOrdered = True
			m_bPre_Order = Me.optPreOrder(1).Checked
			optEXE(0).Enabled = False
			optEXE(1).Enabled = False
		Else
			m_bItemsOrdered = False
			m_bPre_Order = False
			optEXE(0).Enabled = True
			optEXE(1).Enabled = True
		End If
		sdt = Nothing
		logger.Debug("ugrdOrderList_AfterRowUpdate Exit")

	End Sub

	Private Sub ugrdOrderList_BeforeCellActivate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles ugrdOrderList.BeforeCellActivate

		logger.Debug("ugrdOrderList_BeforeCellActivate Entry")
		If e.Cell.Column.Key = "QuantityOrdered" Then
			If GetWeight_Unit(CInt(e.Cell.Row.Cells("QuantityUnit").Value)) Then
				e.Cell.Column.MaskInput = "{double:5.2}"
			Else
				e.Cell.Column.MaskInput = "nnnn"
			End If
		End If
		logger.Debug("ugrdOrderList_BeforeCellActivate Exit")

	End Sub

	Public Sub FilterGrid()
		logger.Debug("FilterGrid Entry")

		Dim sWhere As String
		sWhere = String.Empty
		sOrder = String.Empty

		Me.ugrdOrderList.UpdateData()

		'-- Do the where statement
		If txtField(0).Text <> "" Then sWhere = "Item_Description LIKE '%" & txtField(0).Text & "%'"
		If txtField(1).Text <> "" Then sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Identifier LIKE '%" & txtField(1).Text & "%'"
		If TextBoxVIN.Text <> "" Then sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "VendorItemID LIKE '%" & TextBoxVIN.Text & "%'"
		If cmbBrand.Text <> "" Then sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Brand = '" & Replace(cmbBrand.Text, "'", "''") & "'"
		If chkOrdered.CheckState = 1 Then sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "QuantityOrdered > 0"
		If cmbCategory.SelectedIndex > -1 Then sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Category_ID = " & ComboVal((cmbCategory))
		Select Case True
			Case optPreOrder(1).Checked : sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Pre_Order = 1"
			Case optPreOrder(2).Checked : sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Pre_Order = 0"
		End Select

		If Me.fraEXE.Visible And (optEXE(0).Checked Or optEXE(1).Checked) Then
			Select Case True
				Case optEXE(0).Checked : sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "EXEDistributed <> 0"
				Case optEXE(1).Checked : sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "EXEDistributed = 0"
			End Select
		End If

		If cmbSubTeam.SelectedIndex > -1 Then
			sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "SubTeam_No = " & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
		End If

		If ((cmbDistSubTeam.SelectedIndex > -1) And (cmbDistSubTeam.Items.Count > 1)) Then
			sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "DistSubTeam_No = " & VB6.GetItemData(cmbDistSubTeam, cmbDistSubTeam.SelectedIndex)
		End If

		If chkDiscontinued.CheckState = CheckState.Unchecked Then
			sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "Discontinue_Item = 0"
		End If

		If chkIncludeNotAvailable.CheckState = CheckState.Unchecked Then
			sWhere = sWhere & IIf(sWhere = "", "", " AND ") & "NA = ''"
		End If

		'################################################################################################################################
		' BUYSIDE Vendor Item Attributes: Please Read! (Robin Eudy: 2/14/2011)
		' Adding CLIENT SIDE filters for VendorItemStatus. The filters on the uderlying search Stored Proc are unchanged for now.
		' The data to populate VendorItemStatus from IMHA/VIP does not exist at this point and time. All data in this field will be NULL
		' To avoid slowing down the search queries with criteria that doesnt exist I have decided to make this filter CLIENT SIDE instead of 
		' adding filters to  the Stored Proc. THIS SHOULD BE CHANGED once the VendorItemStatus starts to be populated.
		' Brian Strickland - moved this to the filter grid function to keep it in one place and to have it where it could be actuated from
		' events on the appropriate controls.
		'#################################################################################################################################

		sWhere = sWhere & IIf(sWhere = "", "( ", " AND ( ") & "VendorItemStatus IS NULL OR VendorItemStatus = '' OR VendorItemStatus = 'A'"
		If chkVendorItemStatus_MfgDiscontinued.CheckState = CheckState.Checked Or chkVendorItemStatus_NotAvailable.CheckState = CheckState.Checked Or chkVendorItemStatus_Seasonal.CheckState = CheckState.Checked Or chkVendorItemStatus_VendorDiscontinued.CheckState = CheckState.Checked Then
			If chkVendorItemStatus_MfgDiscontinued.CheckState = CheckState.Checked Then
				sWhere = sWhere & IIf(sWhere.EndsWith("( "), "", " OR ") & "VendorItemStatus = 'M'"
			End If
			If chkVendorItemStatus_NotAvailable.CheckState = CheckState.Checked Then
				sWhere = sWhere & IIf(sWhere.EndsWith("( "), "", " OR ") & "VendorItemStatus = 'N'"
			End If
			If chkVendorItemStatus_Seasonal.CheckState = CheckState.Checked Then
				sWhere = sWhere & IIf(sWhere.EndsWith("( "), "", " OR ") & "VendorItemStatus = 'S'"
			End If
			If chkVendorItemStatus_VendorDiscontinued.CheckState = CheckState.Checked Then
				sWhere = sWhere & IIf(sWhere.EndsWith("( "), "", " OR ") & "VendorItemStatus = 'V'"
			End If
		End If
		sWhere = sWhere & " ) "

		Select Case True
			Case optOrder(0).Checked : sOrder = "Category_Name, Item_Description, Identifier"
			Case optOrder(1).Checked : sOrder = "Item_Description, Identifier"
			Case optOrder(2).Checked : sOrder = "Identifier, Item_Description"
			Case optOrder(4).Checked : sOrder = "Brand, Item_Description, Identifier"
			Case RadioButton_SortByStatus.Checked : sOrder = " VendorItemStatusSort, Item_Description, Identifier"
			Case Else : sOrder = "VendorItemID, Item_Description, Identifier"
		End Select

		'*****************************************************
		mdv.RowFilter = sWhere
		mdv.Sort = sOrder
		Me.ugrdOrderList.DisplayLayout.Bands(0).SortedColumns.Clear()
		Me.ugrdOrderList.DataSource = mdv
		'*****************************************************
		logger.Debug("FilterGrid Exit")

ExitSub:
		System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
		logger.Debug("FilterGrid Exit")

	End Sub

	Private Sub ugrdOrderList_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles ugrdOrderList.BeforeCellUpdate

		logger.Debug("ugrdOrderList_BeforeCellUpdate Entry")

		If e.Cell.Column.Key = "QuantityOrdered" Then

			If IIf(IsDBNull(e.NewValue), 0, e.NewValue) > 1000 Then

				Dim response As MsgBoxResult = MessageBox.Show("Quantity exceeds 1000! Are you sure about that?", "Quantity Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

				If response = MsgBoxResult.No Then

					e.Cancel = True

					Exit Sub

				End If

			End If

			'If "Not Available" and user entered an order quantity
			If e.Cell.Row.Cells("NA").Value <> String.Empty And IIf(IsDBNull(e.NewValue), 0, e.NewValue) <> 0 Then

				If (frmOrders.VendorType = Global_Renamed.enumVendorType.RegionalFacilityDistCenter Or frmOrders.VendorType = Global_Renamed.enumVendorType.RegionalFacilityManufacturer) And Not (frmOrders.IsCredit) And Not gbWarehouse Then

					e.Cancel = True

					MsgBox("This item is not available", MsgBoxStyle.Critical, Me.Text)

				End If

			End If

		End If

		logger.Debug("ugrdOrderList_BeforeCellUpdate Exit")

	End Sub

	Private Sub ugrdOrderList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdOrderList.DoubleClickRow

		logger.Debug("ugrdOrderList_DoubleClickRow Entry")
		If e.Row.Cells("Item_Key").Text <> String.Empty Then

			Me.Enabled = False

			glItemID = e.Row.Cells("Item_Key").Value
			frmItem.ShowDialog()
			frmItem.Close()

			Me.Enabled = True

		Else
			MsgBox("Please select an item", MsgBoxStyle.Critical, Me.Text)
		End If
		logger.Debug("ugrdOrderList_DoubleClickRow Exit")

	End Sub

	Private Sub chkDiscontinued_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDiscontinued.CheckStateChanged
		logger.Debug("chkDiscontinued_CheckStateChanged Entry")
		If Me.IsInitializing Or m_bLoading Then Exit Sub
		FilterGrid()
		logger.Debug("chkDiscontinued_CheckStateChanged Exit")
	End Sub

	Private Sub ugrdOrderList_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ugrdOrderList.InitializeRow

		If e.Row.Cells("VendorCostHistoryId").Value.ToString().Equals(String.Empty) Then

			e.Row.Appearance.BorderColor = Color.Red
			e.Row.Appearance.BackColor = Color.LightPink
			e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
			e.Row.ToolTipText = "Cost information could not be found for this item. It cannot be ordered until this is fixed."
			e.Row.Cells("QuantityOrdered").Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled

		End If


		If Not e.Row.Cells("NA").Value.Equals(String.Empty) And Not Me.ReceiveLocationIsDistribution And Not Me.IsVendorExternal Then
			e.Row.Cells("QuantityOrdered").Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled
		End If

		If e.Row.Cells("Discontinue_Item").Value And Me.ReceiveLocationIsDistribution Then
			e.Row.Cells("QuantityOrdered").Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled
		End If



	End Sub

	'Use this for grid navigation capability.
	Private Sub ugrdOrderList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ugrdOrderList.KeyDown

		logger.Debug("ugrdOrderList_KeyDown Entry")
		Select Case e.KeyValue
			Case Keys.Up
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
				e.Handled = True
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
			Case Keys.Down
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
				e.Handled = True
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
			Case Keys.Right
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
				e.Handled = True
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
			Case Keys.Left
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
				e.Handled = True
				ugrdOrderList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
		End Select
		logger.Debug("ugrdOrderList_KeyDown Exit")
	End Sub

	Private Sub chkIncludeNotAvailable_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIncludeNotAvailable.CheckStateChanged
		logger.Debug("chkIncludeNotAvailable_CheckStateChanged Entry")

		If Me.IsInitializing Or m_bLoading Then Exit Sub
		FilterGrid()

		logger.Debug("chkIncludeNotAvailable_CheckStateChanged Exit")
	End Sub

	Private Sub ugrdOrderList_MouseEnterElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdOrderList.MouseEnterElement
		Dim cell As Infragistics.Win.UltraWinGrid.UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))
		If Not cell Is Nothing Then
			If cell.Column.Key = "VendorItemStatus" Then
				If Not IsDBNull(cell.Value) Then
					ToolTip1.Active = True
					ToolTip1.SetToolTip(sender, cell.Row.GetCellValue("VendorItemStatusFull") & "")
					Exit Sub
				End If

			End If
		End If
	End Sub

	Private Sub ugrdOrderList_MouseLeaveElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdOrderList.MouseLeaveElement
		ToolTip1.SetToolTip(ugrdOrderList, Nothing)
	End Sub

	Private Sub chkVendorItemStatus_NotAvailable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVendorItemStatus_NotAvailable.CheckedChanged
		If Not IsInitializing And Not m_bLoading Then
			FilterGrid()
		End If
	End Sub

	Private Sub chkVendorItemStatus_MfgDiscontinued_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVendorItemStatus_MfgDiscontinued.CheckedChanged
		If Not IsInitializing And Not m_bLoading Then
			FilterGrid()
		End If
	End Sub

	Private Sub chkVendorItemStatus_VendorDiscontinued_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVendorItemStatus_VendorDiscontinued.CheckedChanged
		If Not IsInitializing And Not m_bLoading Then
			FilterGrid()
		End If
	End Sub

	Private Sub chkVendorItemStatus_Seasonal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVendorItemStatus_Seasonal.CheckedChanged
		If Not IsInitializing And Not m_bLoading Then
			FilterGrid()
		End If
	End Sub

    Private Sub btnConversionCalculator_Click(sender As System.Object, e As System.EventArgs) Handles btnConversionCalculator.Click
        logger.Debug("btnConversionCalculator_Click Entry")

        Dim ccForm As New FrmConvertMeasures
        ccForm.ShowDialog(Me)
        ccForm.Dispose()

        logger.Debug("btnConversionCalculator_Click Exit")
    End Sub

    Private Sub btnApplyAllCreditReason_Click(sender As Object, e As EventArgs) Handles btnApplyAllCreditReason.Click
        If frmOrders.IsCredit Then
            Dim sdr() As DataRow = mdt.Select("QuantityOrdered > 0 AND CreditReason_ID IS NOT NULL", sOrder)
            If sdr.Length = 0 Then
                sdr = Nothing
                MsgBox("You must enter a credit reason for an item with a quantity greater than zero", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Credit Reason?")
                logger.Info("btnApplyAllCreditReason_Click " & " You must enter a credit reason for an item with a quantity greater than zero")
                logger.Debug("btnApplyAllCreditReason_Click exit")
                Exit Sub
            Else
                Dim sCreateReason_ID As String
                Dim dr As DataRow
                sCreateReason_ID = sdr(0)("CreditReason_ID")

                Dim sdrn() As DataRow = mdt.Select("QuantityOrdered > 0 AND CreditReason_ID IS NULL")

                For Each dr In sdrn
                    dr("CreditReason_ID") = sCreateReason_ID
                Next

                dr = Nothing
            End If
            sdr = Nothing
        End If
    End Sub
End Class