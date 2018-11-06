Option Strict Off
Option Explicit On

Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess


Friend Class frmVendorCostDetail
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Public plStore_No As Integer
    Public poItemInfo As ItemBO
    Public poVendorInfo As VendorBO
    Public IgnoreCasePackUpdates As Boolean = False

    Private m_iReturnState As enumReturnState
    Private pbDataChanged As Boolean

    Private mdtStores As DataTable
    Private mdtItemOverrides As DataSet
    Private mbFilling As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Enum geStoreCol
        StoreNo = 0
        StoreName = 1
        ZoneID = 2
        State = 3
        WFMStore = 4
        MegaStore = 5
        CustomerType = 6
        Store = 7
    End Enum

    Private Sub frmVendorCostDetail_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmVendorCostDetail_Load Entry")

        m_iReturnState = Global_Renamed.enumReturnState.Active
        _optType_1.Visible = False
        dtpStartDate.Value = Today
        dtpEndDate.Value = Nothing

        SetActive(dtpStartDate, True)
        SetActive(dtpEndDate, False)

        btnConversionCalculator.Visible = OrderSearchDAO.IsMultipleJurisdiction()

        '-- Fill out the store list
        mdtStores = StoreDAO.GetStoreItemVendorList(poItemInfo.Item_Key, poVendorInfo.VendorID)
        ugrdStoreList.DataSource = mdtStores

        '-- Fill out the Retail Pack table
        mdtItemOverrides = StoreJurisdictionDAO.GetRetailUnitOverrideList(poItemInfo.Item_Key)
        ugrdRetailPackList.DataSource = mdtItemOverrides

        '-- Highlight current store
        Call StoreListGridSelectStore(ugrdStoreList, plStore_No)

        'set read-only costed by weight indicator
        Me.CheckBox_CostedByWeight.Checked = poItemInfo.CostedByWeight

        ' set the read-only catchweight required indicator
        Me.CheckBox_CatchweightRequired.Checked = poItemInfo.CatchweightRequired

        '4.22.08 - Decision reversed for 3.1 to revert back to original functionality
        Dim itemUnitList As ArrayList = ItemUnitDAO.GetItemUnitList(poItemInfo.CostedByWeight)

        Me.ComboBox_CostUnit.DataSource = itemUnitList
        Me.ComboBox_CostUnit.DisplayMember = "UnitName"
        Me.ComboBox_CostUnit.ValueMember = "UnitID"
        Me.ComboBox_CostUnit.SelectedIndex = -1

        Me.ComboBox_FreightUnit.DataSource = itemUnitList
        Me.ComboBox_FreightUnit.DisplayMember = "UnitName"
        Me.ComboBox_FreightUnit.ValueMember = "UnitID"
        Me.ComboBox_FreightUnit.SelectedIndex = -1


        'get the item's unit descriptions
        ItemUnitDAO.GetItemUnitInfo(poItemInfo)

        LoadZone(cmbZones)

        LoadJurisdiction(cmbJurisdiction)

        lblCurrency.Text = poVendorInfo.VendorCurrencyCode
        'LoadCurrency(cmbCurrency)

        CenterForm(Me)

        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

        Call SetCombos()

        If ugrdRetailPackList.Rows.Count = 1 Then
            Me._labelVendorPackUOM.Text = ugrdRetailPackList.Rows(0).Cells(6).Text
        Else
            Me._labelVendorPackUOM.Text = String.Empty
        End If
        chkIgnorePackUpdates.Checked = Me.IgnoreCasePackUpdates
        TextBox_RetailCasePack.Enabled = chkIgnorePackUpdates.Checked

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If

        logger.Debug("frmVendorCostDetail_Load Exit")

    End Sub

    Private Sub frmVendorCostDetail_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmVendorCostDetail_FormClosing Entry")

        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason

        If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
            Cancel = True
            Me.Hide()
        End If

        eventArgs.Cancel = Cancel

        logger.Debug("frmVendorCostDetail_FormClosing Exit")
    End Sub

    Private Sub frmVendorCostDetail_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        plStore_No = 0
    End Sub

    Public ReadOnly Property ReturnState() As enumReturnState
        Get
            ReturnState = m_iReturnState
        End Get
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        m_iReturnState = Global_Renamed.enumReturnState.Cancel
        Me.Hide()
    End Sub

    Public Function CheckCostByWeightUOM() As Boolean

        Dim storeJurisdiction As Integer

        'check the selected stores to make sure there is only one jurisdiction selected.
        storeJurisdiction = CheckForMultipleJurisdiction()
        If storeJurisdiction = -1 Then
            MsgBox("You can only select stores for one Jurisdiction at a time")
        Else
            'match the selected UOM for cost against the jurisdictions UOM from the retail pack grid.
            'If they match, return true.

            'if the Cost UOM is a Box, no validation is required
            If CType(Me.ComboBox_CostUnit.SelectedItem, ItemUnitBO).UnitId = giBox Then
                Return True
            Else
                If CheckForUOMOverride(storeJurisdiction) = True Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return False
        End If

    End Function

    Public Function CheckForUOMOverride(ByVal storeJurisdiction As Integer) As Boolean

        'compare the CostUOM Desc with the costUOM desc in the retail pack grid with the JurisdictionID from the stores selected
        Dim costUOM As String
        costUOM = CType(Me.ComboBox_CostUnit.SelectedItem, ItemUnitBO).UnitName

        Dim dr As Data.DataRow
        Dim dt As DataTable

        dt = mdtItemOverrides.Tables(0)

        For Each dr In dt.Rows
            If CType(dr.Item("storeJurisdictionID"), Integer) = storeJurisdiction Then
                If dr.Item("Unit_Name").ToString = costUOM Then
                    Return True
                End If
            End If
        Next

        Return False

    End Function

    Public Function CheckForMultipleJurisdiction() As Integer
        Dim JurisdictionID As Integer
        Dim x As Integer
        Dim rowSelected As Infragistics.Win.UltraWinGrid.UltraGridRow

        'Grab each of the selected store rows, and check to see if more than one jurisdiction is selected
        With ugrdStoreList.Selected

            x = .Rows(0).Cells("StoreJurisdictionID").Value
            JurisdictionID = .Rows(0).Cells("StoreJurisdictionID").Value

            For Each rowSelected In .Rows
                If x <> rowSelected.Cells("StoreJurisdictionID").Value Then
                    JurisdictionID = -1
                End If
            Next
        End With

        Return JurisdictionID
    End Function

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click

        Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        ' Dim currentParam As DBParam
        Dim sql As String = String.Empty
        Dim dt As DataTable = Nothing

        logger.Debug("cmdSelect_Click Entry")

        Dim sUnitCost As String
        Dim iCostUnit As Integer
        Dim sUnitFreight As String
        Dim iFreightUnit As Integer
        Dim sPkgDesc1 As String
        Dim sStartDate As String
        Dim sEndDate As String
        Dim sMSRP As String
        Dim nStore As Short
        Dim sStoreNo(0) As String
        Dim sStoreNoList As String
        Dim rs As DAO.Recordset = Nothing
        Dim lOverlap As Integer
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim tempSmallMoneyValue As Decimal
        Dim smallMoneyMaxValue As Decimal = 214748.3647
        Dim bIgnorePackUpdates As Boolean
        Dim iCurrency As Integer

        sStartDate = "NULL"
        sEndDate = "NULL"

        ' validate at least one store is selected
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesIRMA.GetString("SelectStore"))
            logger.Debug("cmdSelect_Click Exit")
            Exit Sub
        End If

        If Len(txtCost.Text) > 0 Then
            sUnitCost = txtCost.Text

            'validate COST; smallmoney data range = +214,748.3647 to -214,748.3647 
            tempSmallMoneyValue = CType(sUnitCost, Decimal)
            If tempSmallMoneyValue > smallMoneyMaxValue Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblCost.Text), MsgBoxStyle.Critical, Me.Text)
                logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblCost.Text))
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            End If
        Else
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblCost.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblCost.Text.Replace(":", "")))
            txtCost.Focus()
            logger.Debug("cmdSelect_Click Exit")
            Exit Sub
        End If

        'validate cost unit
        If Me.ComboBox_CostUnit.SelectedIndex < 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("CostUnit")), MsgBoxStyle.Critical, Me.Text)
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("CostUnit")))
            Me.ComboBox_CostUnit.Focus()
            logger.Debug("cmdSelect_Click Exit")
            Exit Sub
        Else
            iCostUnit = CType(Me.ComboBox_CostUnit.SelectedItem, ItemUnitBO).UnitId
        End If

        If Len(txtFreight.Text) > 0 Then
            sUnitFreight = txtFreight.Text

            'validate FREIGHT; smallmoney data range = +214,748.3647 to -214,748.3647 
            tempSmallMoneyValue = CType(sUnitFreight, Decimal)
            If tempSmallMoneyValue > smallMoneyMaxValue Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblFreight.Text), MsgBoxStyle.Critical, Me.Text)
                logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblFreight.Text))
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            End If
        Else
            sUnitFreight = "NULL"
        End If

        'validate freight unit
        If Me.ComboBox_FreightUnit.SelectedIndex < 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("FreightUnit")), MsgBoxStyle.Critical, Me.Text)
            Me.ComboBox_FreightUnit.Focus()
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("FreightUnit")))
            logger.Debug("cmdSelect_Click Exit")
            Exit Sub
        Else
            iFreightUnit = CType(Me.ComboBox_FreightUnit.SelectedItem, ItemUnitBO).UnitId
        End If

        If Len(TextBox_RetailCasePack.Text) > 0 Then
            sPkgDesc1 = txtPkgDesc1.Text
        Else
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("PkgDesc1")), MsgBoxStyle.Critical, Me.Text)
            logger.Debug("cmdSelect_Click Exit")
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("PkgDesc1")))
            txtPkgDesc1.Focus()
            logger.Debug("cmdSelect_Click Exit")
            Exit Sub
        End If

        ' TFS 1861:
        ' Modify IRMA to allow costs to be entered with a past start date.
        ' Developer: Denis Ng
        ' Date: 08/08/2011

        sStartDate = IIf(CDate(dtpStartDate.Value) > System.DateTime.FromOADate(0), "'" & CDate(dtpStartDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat")) & "'", "NULL")

        If dtpEndDate.Value <> Nothing Then
            If dtpStartDate.Value <> Nothing AndAlso (dtpEndDate.Value < dtpStartDate.Value) Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreater"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesIRMA.GetString("EndDateGreater"))
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            Else
                sEndDate = IIf(CDate(dtpEndDate.Value) > System.DateTime.FromOADate(0), "'" & CDate(dtpEndDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat")) & "'", "NULL")
            End If
        End If

        If optType(1).Checked Then
            If sStartDate = "NULL" OrElse sEndDate = "NULL" Then
                MsgBox(ResourcesItemHosting.GetString("BothDateRequiredPromotions"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("BothDateRequiredPromotions"))
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            End If
        End If

        If Len(txtMSRP.Text) > 0 Then
            sMSRP = txtMSRP.Text
            'validate MSRP; smallmoney data range = +214,748.3647 to -214,748.3647 
            tempSmallMoneyValue = CType(sMSRP, Decimal)
            If tempSmallMoneyValue > smallMoneyMaxValue Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblMSRP.Text), MsgBoxStyle.Critical, Me.Text)
                logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), lblMSRP.Text))
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            End If
        Else
            sMSRP = "NULL"
        End If

        'Set the IgnorePackUpdate
        If chkIgnorePackUpdates.Checked = True Then
            bIgnorePackUpdates = 1
        Else
            bIgnorePackUpdates = 0
        End If

        'If the Ignore Vendor Pack flag is set, the Retail Case Pack value is required and not allow save and exit without the value
        Dim validationResult As Integer

        If bIgnorePackUpdates And Len(TextBox_RetailCasePack.Text.Trim) = 0 Then
            MsgBox(ResourcesItemHosting.GetString("msg_error_RetailCasePack_Missing"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("msg_error_RetailCasePack_Missing"))
            logger.Debug("cmdSelect_Click Exit")
            TextBox_RetailCasePack.Focus()
            Exit Sub
        ElseIf Not Integer.TryParse(TextBox_RetailCasePack.Text, validationResult) Then
            MsgBox(ResourcesItemHosting.GetString("msg_error_RetailCasePack_NotInteger"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("msg_error_RetailCasePack_NotInteger"))
            logger.Debug("cmdSelect_Click Exit")
            TextBox_RetailCasePack.Focus()
            Exit Sub
        End If

        'Set the currency
        'If cmbCurrency.SelectedItem Is Nothing Then
        '    iCurrency = -1

        'Else
        '    iCurrency = VB6.GetItemData(cmbCurrency, cmbCurrency.SelectedIndex)
        'End If

        iCurrency = poVendorInfo.VendorCurrencyID

        'Get the list of stores
        nStore = 0
        For Each row In ugrdStoreList.Selected.Rows
            ReDim Preserve sStoreNo(nStore)
            sStoreNo(nStore) = CInt(row.Cells("Store_No").Value)
            nStore = nStore + 1
        Next

        sStoreNoList = Join(sStoreNo, "|")

        Try
            'rs = SQLOpenRecordSet("EXEC CheckVendorCostHistoryOverlap '" & sStoreNoList & "','|'," & poItemInfo.Item_Key & "," & poVendorInfo.VendorID & "," & sStartDate & "," & sEndDate, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            sql = "EXEC CheckVendorCostHistoryOverlap '" & sStoreNoList & "','|'," & poItemInfo.Item_Key & "," & poVendorInfo.VendorID & "," & sStartDate & "," & sEndDate
            dt = factory.GetDataTable(sql)
            lOverlap = dt.Rows(0)(0)
        Finally
            If dt IsNot Nothing Then
                dt.Dispose()
            End If
        End Try
        If lOverlap > 0 Then
            If MsgBox(ResourcesItemHosting.GetString("OverlappingCostHistory"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                logger.Info(ResourcesItemHosting.GetString("OverlappingCostHistory") & "NO")
                logger.Debug("cmdSelect_Click Exit")
                Exit Sub
            End If
        End If

        'perform final validation - check if new cost will result in a negative net cost
        Dim vendorDeal As New VendorDealBO
        Dim result As DialogResult

        vendorDeal.ItemKey = poItemInfo.Item_Key
        vendorDeal.VendorID = poVendorInfo.VendorID
        vendorDeal.StoreList = sStoreNoList
        vendorDeal.StoreListSeparator = "|"
        vendorDeal.CaseAmt = CType(Me.txtCost.Text, Decimal) 'new proposed cost
        vendorDeal.StartDate = CType(Me.dtpStartDate.Value, Date)

        Dim storeCostConflicts As ArrayList = vendorDeal.GetNegativeCostStoreConflicts_CostChange(vendorDeal)

        If storeCostConflicts.Count > 0 Then
            'build error msg to include list of conflict stores
            Dim errorMsg As New StringBuilder
            Dim currentStore As StoreBO

            errorMsg.Append(ResourcesItemHosting.GetString("msg_confirm_negativeNetCost"))
            errorMsg.Append(Environment.NewLine)
            errorMsg.Append(Environment.NewLine)

            For Each currentStore In storeCostConflicts
                errorMsg.Append("       * ") 'INDENT
                errorMsg.Append(currentStore.StoreName)
                errorMsg.Append(Environment.NewLine)
            Next

            'warn user and ask if they wish to proceed
            result = MessageBox.Show(errorMsg.ToString, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        Else
            result = Windows.Forms.DialogResult.Yes
        End If


        If result = Windows.Forms.DialogResult.Yes Then
            sql = String.Format("Update ItemVendor	SET IgnoreCasePack = {0} , RetailCasePack = case when {0} = 1 then {1} else {2} end 	WHERE Vendor_ID = {3} AND Item_Key = {4}", IIf(bIgnorePackUpdates, 1, 0), TextBox_RetailCasePack.Text.Trim, sPkgDesc1, poVendorInfo.VendorID, poItemInfo.Item_Key)
            factory.ExecuteNonQuery(sql)

            sql = String.Format("EXEC InsertVendorCostHistory @StoreList='{0}', @StoreListSeparator='{1}', @Item_Key={2}, @Vendor_ID={3}, @UnitCost={4}, @UnitFreight={5}, @Package_Desc1={6}, @StartDate={7}, @EndDate={8}, @Promotional={9}, @MSRP={10}, @FromVendor={11}, @CostUnit_ID={12}, @FreightUnit_ID={13}, @Currency={14}", sStoreNoList, "|", poItemInfo.Item_Key, poVendorInfo.VendorID, sUnitCost, sUnitFreight, sPkgDesc1, sStartDate, sEndDate, System.Math.Abs(CInt(optType(1).Checked)), sMSRP, 0, iCostUnit, iFreightUnit, IIf(iCurrency = -1, "null", iCurrency))
            factory.ExecuteNonQuery(sql)

            m_iReturnState = Global_Renamed.enumReturnState.Apply
            Me.Hide()
        End If
        logger.Debug("cmdSelect_Click Exit")
    End Sub

    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        logger.Debug("optType_CheckedChanged Entry")

        If Me.IsInitializing Then Exit Sub
        If eventSender.Checked Then
            Dim Index As Short = optType.GetIndex(eventSender)

            pbDataChanged = True

            If Index = 0 Then
                SetActive(dtpEndDate, False)
            Else
                SetActive(dtpEndDate, True)
            End If

        End If

        logger.Debug("optType_CheckedChanged Exit")
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        logger.Debug("OptSelection_CheckedChanged Entry")
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            Select Case Index
                Case 0
                    '-- Manual.
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1
                    cmbJurisdiction.SelectedIndex = -1
                Case 1
                    '-- All Stores or All 365 for RM
                    If CheckAllStoreSelectionEnabled() Then
                        Call StoreListGridSelectAll(ugrdStoreList, True)
                    Else
                        Call StoreListGridSelectAll365(ugrdStoreList)
                    End If

                Case 2
                    '-- By Zone
                    If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                Case 3
                    '-- By State.
                    If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                Case 4
                    '-- All WFM.
                    Call StoreListGridSelectAllWFM(ugrdStoreList)
                Case 5
                    '--Jurisdiction
                    If cmbJurisdiction.SelectedIndex > -1 Then Call StoreListGridSelectByJurisdiction(ugrdStoreList, VB6.GetItemData(cmbJurisdiction, cmbJurisdiction.SelectedIndex))

            End Select

            mbFilling = False

        End If
        logger.Debug("OptSelection_CheckedChanged Exit")
    End Sub

    Private Sub SetCombos()

        logger.Debug("SetCombos Entry")

        mbFilling = True

        'Zones.
        If optSelection(2).Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'Jurisdictions.
        If optSelection(5).Checked = True Then
            cmbJurisdiction.Enabled = True
            cmbJurisdiction.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbJurisdiction.SelectedIndex = -1
            cmbJurisdiction.Enabled = False
            cmbJurisdiction.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If optSelection(3).Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False
        logger.Debug("SetCombos Exit")

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged

        logger.Debug("cmbStates_SelectedIndexChanged Entry")
        If IsInitializing Or mbFilling Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False
        logger.Debug("cmbStates_SelectedIndexChanged Exit")

    End Sub

    Private Sub cmbJurisdiction_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbJurisdiction.SelectedIndexChanged
        If mbFilling Or IsInitializing Then Exit Sub
        OptSelection_CheckedChanged(optSelection.Item(5), New System.EventArgs())
    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        logger.Debug("cmbZones_SelectedIndexChanged Entry")
        If mbFilling Or IsInitializing Then Exit Sub
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())
        logger.Debug("cmbZones_SelectedIndexChanged Exit")
    End Sub

    Private Function GetFreight(ByVal lStore As Integer) As Single

        logger.Debug("GetFreight Entry with lStore=" + lStore.ToString)

        Dim rsFreight As DAO.Recordset = Nothing
        Dim sFreight As Single

        Try
            rsFreight = SQLOpenRecordSet("EXEC GetVendCostData " & poItemInfo.Item_Key & ", " & poVendorInfo.VendorID & ", " & lStore & ", '" & SystemDateTime() & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            sFreight = rsFreight.Fields("UnitFreight").Value
        Finally
            If rsFreight IsNot Nothing Then
                rsFreight.Close()
                rsFreight = Nothing
            End If
        End Try

        logger.Debug("GetFreight -Exit with Freight value =" + sFreight.ToString)

        Return sFreight
    End Function

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange

        logger.Debug("ugrdStoreList_AfterSelectChange Entry")

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        optSelection.Item(0).Checked = True
        mbFilling = False

        logger.Debug("ugrdStoreList_AfterSelectChange Exit")

    End Sub

    Private Sub txtCost_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCost.Enter

        logger.Debug("txtCost_Enter Entry")
        HighlightText(txtCost)
        logger.Debug("txtCost_Enter Exit")

    End Sub

    Private Sub txtCost_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCost.KeyPress

        logger.Debug("txtCost_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtCost.Tag), txtCost, 0, 0, 0)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("txtCost_KeyPress Exit")
    End Sub

    Private Sub txtCost_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCost.TextChanged
        logger.Debug("txtCost_TextChanged Entry")
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True
        logger.Debug("txtCost_TextChanged Exit")
    End Sub

    Private Sub txtFreight_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFreight.Enter
        logger.Debug("txtFreight_Enter Entry")
        HighlightText(txtFreight)
        logger.Debug("txtFreight_Enter Exit")
    End Sub

    Private Sub txtFreight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFreight.KeyPress
        logger.Debug("txtFreight_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtFreight.Tag), txtFreight, 0, 0, 0)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("txtFreight_KeyPress Exit")
    End Sub

    Private Sub txtFreight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFreight.TextChanged
        logger.Debug("txtFreight_TextChanged Entry")
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True
        logger.Debug("txtFreight_TextChanged Exit")
    End Sub

    Private Sub txtMSRP_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMSRP.Enter
        logger.Debug("txtMSRP_Enter Entry")
        HighlightText(txtMSRP)
        logger.Debug("txtMSRP_Enter Exit")
    End Sub

    Private Sub txtMSRP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMSRP.KeyPress
        logger.Debug("txtMSRP_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtMSRP.Tag), txtMSRP, 0, 0, 0)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("txtMSRP_KeyPress Exit")
    End Sub

    Private Sub txtMSRP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMSRP.TextChanged

        logger.Debug("txtMSRP_TextChanged Entry")
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True
        logger.Debug("txtMSRP_TextChanged Exit")

    End Sub

    Private Sub txtPkgDesc1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgDesc1.Enter

        logger.Debug("txtPkgDesc1_Enter Entry")
        HighlightText(txtPkgDesc1)
        logger.Debug("txtPkgDesc1_Enter Exit")

    End Sub

    Private Sub txtPkgDesc1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPkgDesc1.KeyPress

        logger.Debug("txtPkgDesc1_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtPkgDesc1.Tag), txtPkgDesc1, 0, 0, 0)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("txtPkgDesc1_KeyPress Exit")
    End Sub

    Private Sub txtPkgDesc1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgDesc1.TextChanged

        logger.Debug("txtPkgDesc1_TextChanged Entry")
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True
        logger.Debug("txtPkgDesc1_TextChanged Exit")

    End Sub

    Private Sub dtpStartDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        logger.Debug("dtpStartDate_ValueChanged Entry")
        If Me.IsInitializing Then Exit Sub

        If dtpStartDate.Value <> Nothing Then
            pbDataChanged = True
        End If
        logger.Debug("dtpStartDate_ValueChanged Exit")

    End Sub

    Private Sub dtpEndDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        logger.Debug("dtpEndDate_ValueChanged Entry")

        If Me.IsInitializing Then Exit Sub

        If dtpEndDate.Enabled AndAlso dtpEndDate.Value <> Nothing Then
            pbDataChanged = True
        End If
        logger.Debug("dtpEndDate_ValueChanged Exit")

    End Sub

    Private Sub Button_MarginInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.Click

        logger.Debug("Button_MarginInfo_Click Entry")
        Dim marginInfoForm As New MarginInfo

        marginInfoForm.ItemBO = poItemInfo
        marginInfoForm.ShowDialog()
        marginInfoForm.Dispose()

        logger.Debug("Button_MarginInfo_Click Exit")
    End Sub

    Private Sub ugrdRetailPackList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdRetailPackList.Click

        If ugrdRetailPackList.Selected.Rows.Count > 0 Then
            Me._labelVendorPackUOM.Text = ugrdRetailPackList.Selected.Rows.Item(0).Cells("Unit_Name").Text
        End If

    End Sub

    Private Sub ugrdRetailPackList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdRetailPackList.InitializeLayout
        e.Layout.Bands(0).Columns("Item_Key").Hidden = True
        e.Layout.Bands(0).Columns("Slash").Header.Caption = "/"
        e.Layout.Bands(0).Columns("Package_Desc1").Header.Caption = "Pack"
        e.Layout.Bands(0).Columns("Package_Desc2").Header.Caption = "Size"
        e.Layout.Bands(0).Columns("Slash").Width = 1
        e.Layout.Bands(0).Columns("Package_Desc1").Format = "#####0"
        e.Layout.Bands(0).Columns("Package_Desc2").Format = "#####0.##"
    End Sub

    ' for bug 5442: not being able to tab to the "By State" option button (_optSelection_3) from cboZones
    ' Rick Kelleher 3/4/08
    ' start
    Private CurrentKey As Integer

    Private Sub cmbZones_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbZones.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
        If CurrentKey = 9 Then
            _optSelection_3.Focus()
            CurrentKey = Nothing
        End If
    End Sub

    Private Sub cmbStates_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbStates.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbStates_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.Leave
        If CurrentKey = 9 Then
            _optSelection_5.Focus()
            CurrentKey = Nothing
        End If
    End Sub

    Private Sub cmbJurisdiction_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbJurisdiction.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbJurisdiction_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbJurisdiction.Leave
        If CurrentKey = 9 Then
            ugrdStoreList.Focus()
            CurrentKey = Nothing
        End If
    End Sub

    ' for bug 5442: end

    Private Sub ComboBox_CostUnit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_CostUnit.SelectedIndexChanged
        ComboBox_FreightUnit.SelectedItem = ComboBox_CostUnit.SelectedItem
    End Sub

    Private Sub ComboBox_FreightUnit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_FreightUnit.SelectedIndexChanged

        ComboBox_CostUnit.SelectedItem = ComboBox_FreightUnit.SelectedItem

    End Sub

    Private Sub chkIgnorePackUpdates_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnorePackUpdates.CheckedChanged
        TextBox_RetailCasePack.Enabled = chkIgnorePackUpdates.Checked
        If chkIgnorePackUpdates.Checked Then
            MsgBox(" Note: Retail Case Pack updates apply for all stores, regardless of store selection.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
            Call StoreListGridSelectAll(ugrdStoreList, True)
        End If
    End Sub

    Private Sub btnConversionCalculator_Click(sender As System.Object, e As System.EventArgs) Handles btnConversionCalculator.Click

        logger.Debug("btnConversionCalculator_Click Entry")
        Dim ccForm As New FrmConvertMeasures

        ccForm.ShowDialog(Me)
        ccForm.Dispose()

        logger.Debug("btnConversionCalculator_Click Exit")
    End Sub
End Class
