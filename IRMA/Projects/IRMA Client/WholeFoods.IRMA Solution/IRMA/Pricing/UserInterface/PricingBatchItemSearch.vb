Option Strict Off
Option Explicit On

Imports System.Data.SqlClient
Imports System.Linq
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Friend Class frmPricingBatchItemSearch
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView
    Private editExistingBatch As Boolean = False

    Dim mlPriceBatchHeaderID As Integer
    Dim miItemChgTypeID As Short
    Dim miPriceChgTypeID As Short
    Dim mdStartDate As Date
    Dim mbGridCtrlKey As Boolean
    Dim mbSelChange As Boolean
    Dim IsInitializing As Boolean
    Dim _isLoading As Boolean
    Dim _isHeaderInfoChanged As Boolean
    Dim _globalPriceManagementBO As GlobalPriceManagementBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm(ByVal updatedBatchHeader As PriceBatchHeaderBO)
#End Region

    Private Sub frmPricingBatchItemSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmPricingBatchItemSearch_Load Enter")

        IsInitializing = True

        _globalPriceManagementBO = New GlobalPriceManagementBO()

        CenterForm(Me)

        If Not editExistingBatch Then
            '' include ALL in price type dropdown except when GPM flag is 1 and store exceptions are there.
            InitializeComboBoxes((Not _globalPriceManagementBO.IsGlobalPriceManagementEnabled) Or (_globalPriceManagementBO.AreAllStoresGpm))

			If cmbSubTeam.DataSource IsNot Nothing AndAlso cmbSubTeam.DataSource.Any() Then cmbSubTeam.SelectedIndex = 0 'Sub-Team is required

			SetActive(cmbZones, False)
            SetActive(cmbState, False)
            SetActive(ucmbStoreList, False)

            If Not CheckAllStoreSelectionEnabled() Then
                _optSelection_5.Visible = False
            End If
            dtpStartDate.Value = System.DateTime.Today

            txtNumBatch.Text = "1"
        End If

        Dim bIncludeBatchDescriptionExtension As Boolean = ConfigurationServices.AppSettings("IncludeBatchDescriptionExtension")

        Dim iBatchDescriptionLimit As Integer = ConfigurationServices.AppSettings("BatchDescriptionLimit")
        If bIncludeBatchDescriptionExtension = True Then
            iBatchDescriptionLimit = iBatchDescriptionLimit - 6
        End If

        Me.BatchDescLabel.Text = String.Format("Batch Desc (14 characters) :", iBatchDescriptionLimit)
        Me.BatchDescriptionTextBox.MaxLength = iBatchDescriptionLimit

        Dim bIncludeNonRetailItemsDefaulValue As Boolean = ConfigurationServices.AppSettings("IncludeNonRetailItemsDefaultValue")
        Me.chkIncludeNonRetailItems.Checked = bIncludeNonRetailItemsDefaulValue

        GlobalPriceManagementUiUpdates()

        IsInitializing = False

        logger.Debug("frmPricingBatchItemSearch_Load Exit")
    End Sub

    Private Sub GlobalPriceManagementUiUpdates()
        If _globalPriceManagementBO.IsGlobalPriceManagementEnabled Then
            _optType_4.Enabled = False
            _optType_5.Enabled = False

            If _globalPriceManagementBO.AreAllStoresGpm Then
                _optType_3.Checked = False
                _optType_3.Enabled = False
                _optType_1.Checked = True
            Else
                FilterStoreComboBoxToNonGpmStores()
            End If
        End If
    End Sub

    Private Sub InitializeComboBoxes(ByVal includeALL As Boolean)
        logger.Debug("InitializeComboBoxes Enter")

		cmbSubTeam.DataSource = SubTeamDAO.GetSubteams()
		LoadPriceTypeCombo(includeALL)
        frmPricingBatch.PopulateRetailStoreDropDown((Me.ucmbStoreList))
        frmPricingBatch.PopulateRetailStoreZoneDropDown(Me.cmbZones)
        frmPricingBatch.PopulateRetailStoreStateDropDown(Me.cmbState)

        logger.Debug("InitializeComboBoxes Exit")
    End Sub

    ''' <summary>
    ''' loads data into the price change type drop down
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPriceTypeCombo(ByVal includeALL As Boolean)
        logger.Debug("LoadPriceTypeCombo Enter")
        Dim priceChgDAO As New PriceChgTypeDAO
        Dim priceChgList As ArrayList = PriceChgTypeDAO.GetPriceChgTypeList(True, includeALL)

        cmbPriceType.DataSource = priceChgList
        If priceChgList.Count > 0 Then
            cmbPriceType.DisplayMember = "PriceChgTypeDesc"
            cmbPriceType.ValueMember = "PriceChgTypeID"
        End If
        logger.Debug("LoadPriceTypeCombo Exit")
    End Sub

    Private Sub LoadDataTable(ByVal searchItemInfo As PriceBatchSearchBO)
        logger.Debug("LoadDataTable Enter")

        Dim dt As DataTable = PriceBatchSearchDAO.GetDetailSearchData(searchItemInfo)
        Dim rowCounter, totalRows, rowsFound As Integer

        Dim col As New DataColumn
        With col
            .ColumnName = "Selected"
            .DataType = System.Type.GetType("System.Int32")
            .DefaultValue = 0
        End With
        dt.Columns.Add(col)
        totalRows = dt.Rows.Count
        rowCounter = 0
        rowsFound = 0

        mdv = New System.Data.DataView(dt)

        ugrdList.DataSource = mdv
        ugrdList.DisplayLayout.Bands(0).Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        ugrdList.DisplayLayout.Bands(0).Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed

        ugrdList.DisplayLayout.Bands(0).Columns("ItemChgTypeDesc").Header.Caption = "Change Type"
        ugrdList.DisplayLayout.Bands(0).Columns("ItemChgTypeDesc").Header.VisiblePosition = 0
        ugrdList.DisplayLayout.Bands(0).Columns("ItemChgTypeDesc").Width = 62


        ugrdList.DisplayLayout.Bands(0).Columns("PriceChgTypeDesc").Header.Caption = "Price Type"
        ugrdList.DisplayLayout.Bands(0).Columns("PriceChgTypeDesc").Header.VisiblePosition = 1
        ugrdList.DisplayLayout.Bands(0).Columns("PriceChgTypeDesc").Width = 62

        ugrdList.DisplayLayout.Bands(0).Columns("Store_Name").Header.Caption = "Store"
        ugrdList.DisplayLayout.Bands(0).Columns("Store_Name").Header.VisiblePosition = 2
        ugrdList.DisplayLayout.Bands(0).Columns("Store_Name").Width = 123

        ugrdList.DisplayLayout.Bands(0).Columns("Identifier").Header.Caption = "Identifier"
        ugrdList.DisplayLayout.Bands(0).Columns("Identifier").Header.VisiblePosition = 3
        ugrdList.DisplayLayout.Bands(0).Columns("Identifier").Width = 77

        ugrdList.DisplayLayout.Bands(0).Columns("Item_Description").Header.Caption = "Description"
        ugrdList.DisplayLayout.Bands(0).Columns("Item_Description").Header.VisiblePosition = 4
        ugrdList.DisplayLayout.Bands(0).Columns("Item_Description").Width = 230

        ugrdList.DisplayLayout.Bands(0).Columns("Brand_Name").Header.Caption = "Brand"
        ugrdList.DisplayLayout.Bands(0).Columns("Brand_Name").Header.VisiblePosition = 5
        ugrdList.DisplayLayout.Bands(0).Columns("Brand_Name").Width = 118

        ugrdList.DisplayLayout.Bands(0).Columns("StartDate").Header.Caption = "Start Date"
        ugrdList.DisplayLayout.Bands(0).Columns("StartDate").Header.VisiblePosition = 6
        ugrdList.DisplayLayout.Bands(0).Columns("StartDate").Width = 68

        ugrdList.DisplayLayout.Bands(0).Columns("Sale_End_Date").Header.Caption = "End Date"
        ugrdList.DisplayLayout.Bands(0).Columns("Sale_End_Date").Header.VisiblePosition = 7
        ugrdList.DisplayLayout.Bands(0).Columns("Sale_End_Date").Width = 68

        ugrdList.DisplayLayout.Bands(0).Columns("Offer_Id").Hidden = True
        ugrdList.DisplayLayout.Bands(0).Columns("PriceChgTypeId").Hidden = True
        ugrdList.DisplayLayout.Bands(0).Columns("ItemChgTypeId").Hidden = True
        ugrdList.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ugrdList.DisplayLayout.Bands(0).Columns("PriceBatchDetailId").Hidden = True
        ugrdList.DisplayLayout.Bands(0).Columns("Id").Hidden = True

        For rowCounter = 0 To totalRows - 1
            If IsRowBatchable(rowCounter) Then
                rowsFound += 1
            Else
                ugrdList.Rows(rowCounter).Appearance.ForeColorDisabled = Color.Red
                ugrdList.Rows(rowCounter).Appearance.BackColor = Color.LightGray
                ugrdList.Rows(rowCounter).Activation = Activation.Disabled
            End If
        Next

        With ugrdList.DisplayLayout.Bands(0)
            .Columns("Id").SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Descending
        End With

        'This may or may not be required.
        If rowsFound > 0 Then
            If chkSelectAll.Checked Then
                Me.SelectAllRows()
            Else
                'Set the first item to selected.
                Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
                For Each row In ugrdList.Rows
                    If row.Activation <> Activation.Disabled Then
                        ugrdList.Rows(row.Index).Selected = True
                        Exit For
                    End If
                Next
            End If

            Me.StatusBar1.Items.Item(0).Text = String.Format(ResourcesIRMA.GetString("NumberSelected"), ResourcesIRMA.GetString("One"))
            Me.StatusBar1.Items.Item(1).Text = String.Format(ResourcesIRMA.GetString("NumberTotal"), ugrdList.Rows.Count)
        Else
            Me.StatusBar1.Items.Item(1).Text = "0 Total"
            logger.Info(ResourcesIRMA.GetString("NoneFound"))
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If
        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Function IsRowBatchable(rowCounter As Integer) As Boolean
        Return (Not IsDBNull(ugrdList.Rows(rowCounter).Cells("ItemChgTypeDesc").Value) AndAlso ugrdList.Rows(rowCounter).Cells("ItemChgTypeDesc").Value.Equals("DEL")) OrElse Not IsDBNull(ugrdList.Rows(rowCounter).Cells("Id").Value)
    End Function

    Public Sub SetExistingBatchInfo(ByRef PriceBatchHeaderID As Integer)
        logger.Debug("SetExistingBatchInfo Enter")
        Dim lStore_No As Integer
        Dim lSubTeam_No As Integer
        Dim iItemChgTypeID As Short
        Dim iPriceChgTypeID As Short
        Dim dStartDate As Date
        Dim i As Short

        _isLoading = True

        editExistingBatch = True
        mlPriceBatchHeaderID = PriceBatchHeaderID

        ' Initialize the combo boxes so the values can be pre-filled
        InitializeComboBoxes(Not _globalPriceManagementBO.IsGlobalPriceManagementEnabled)

        frmPricingBatch.GetBatchInfo(PriceBatchHeaderID, lStore_No, lSubTeam_No, iItemChgTypeID, iPriceChgTypeID, dStartDate)

        'Tim (2008-03-19): The correct store is set in the combo, but the search defaults to all stores; change to search by store (TFS 5698)
        If lStore_No <> 0 Then
            optSelection(0).Checked = True
        End If

        ' Set the values for all the search criteria and then disable the options
        SetCombo(ucmbStoreList, lStore_No)
        SetActive(ucmbStoreList, False)
        SetActive(cmbZones, False)
        SetActive(cmbState, False)

        For i = 0 To optSelection.UBound
            optSelection(i).Enabled = False
        Next

		If (cmbSubTeam.Items IsNot Nothing) Then
			cmbSubTeam.SelectedItem = cmbSubTeam.Items.Cast(Of SubTeamBO).FirstOrDefault(Function(x) x.SubTeamNo = lSubTeam_No)
		End If

		cmbSubTeam.Enabled = False
		_lblLabel_5.Enabled = False

        For i = 0 To optType.UBound
            optType(i).Enabled = False
        Next

        Select Case iItemChgTypeID
            Case 0
                optType(3).Checked = True
            Case 1
                optType(0).Checked = True
            Case 2
                optType(1).Checked = True
            Case 3
                optType(2).Checked = True
            Case 4
                optType(4).Checked = True
            Case 5
                optType(5).Checked = True
        End Select

        miItemChgTypeID = iItemChgTypeID
        fraType.Enabled = False

        'Select Case iPriceChgTypeID
        '    Case 0, 1
        '        optTagType(0).Checked = True
        '    Case 2
        '        optTagType(1).Checked = True
        '    Case 3
        '        optTagType(2).Checked = True
        'End Select
        'For i = 0 To optTagType.UBound
        '    optTagType(i).Enabled = False
        'Next

        'cmbPriceType.SelectedValue = iPriceChgTypeID

        For Each item As PriceChgTypeBO In cmbPriceType.Items
            If item.PriceChgTypeID = iPriceChgTypeID Then
                cmbPriceType.SelectedItem = item
            End If
        Next
        SetActive(cmbPriceType, False)

        miPriceChgTypeID = iPriceChgTypeID
        fraPriceType.Enabled = False

        dtpStartDate.Value = dStartDate
        mdStartDate = dStartDate
        SetActive(dtpStartDate, False)
        lblDates.Enabled = False

        txtNumBatch.Text = ""
        SetActive(txtNumBatch, False)
        _lblLabel_7.Enabled = False

        _isLoading = False
        logger.Debug("SetExistingBatchInfo Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        RunSearch()
    End Sub

    ''' <summary>
    ''' Creates a price batch for the PriceBatchDetail search items selected by the user by
    ''' creating a new PriceBatchHeader record.  Each detail selected is then updated w/ the
    ''' apropriate PriceBatchHeaderID value.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        logger.Debug("cmdSelect_Click Enter")
        Dim nTotalSelRows As Integer
        Dim i As Integer
        Dim it As Integer
        Dim lNumBatches As Integer
        Dim lNumPer As Integer
        Dim lNumInBatch As Integer
        Dim lStoreList() As Integer
        Dim lPriceBatchHeaderID As Integer
        Dim sAddList As String
        Dim header As PriceBatchHeaderBO
        Dim headerDAO As New PriceBatchHeaderDAO
        Dim factory As DataFactory = Nothing
        Dim transaction As SqlTransaction = Nothing

        ' reset any highlighted errors
        For i = 0 To nTotalSelRows - 1
            ugrdList.Rows(i).CellAppearance.BackColor = Color.White
            ugrdList.Rows(i).CellAppearance.ForeColor = Color.Black
        Next

        '--------------------------------------------
        'Validate the Data Entry fields
        '--------------------------------------------
        If Not txtNumBatch.ReadOnly Then
            If Len(txtNumBatch.Text) = 0 Then
                logger.Info(ResourcesPricing.GetString("EnterNumberBatches"))
                MsgBox(ResourcesPricing.GetString("EnterNumberBatches"), MsgBoxStyle.Critical, Me.Text)
                txtNumBatch.Focus()
                Exit Sub
            Else
                lNumBatches = CInt(txtNumBatch.Text)
                If lNumBatches = 0 Then
                    logger.Info(ResourcesPricing.GetString("NumberBatchesGreater"))
                    MsgBox(ResourcesPricing.GetString("NumberBatchesGreater"), MsgBoxStyle.Critical, Me.Text)
                    txtNumBatch.Focus()
                    Exit Sub
                End If
            End If
        Else
            lNumBatches = 1
        End If

        nTotalSelRows = ugrdList.Rows.Count

        If ugrdList.Selected.Rows.Count <= 0 Then
            logger.Info(ResourcesPricing.GetString("MustSelect"))
            MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        ' Convert the start date into string with "M/dd/yyyy" format
        Dim tmpApplyDate As String
        Dim tempApplyDate As Date
        tempApplyDate = CDate(AutoApplyDateUDTE.Value)
        tmpApplyDate = CDate(AutoApplyDateUDTE.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))

        If CDate(AutoApplyDateUDTE.Value) < CDate(DateTime.Today) Then
            logger.Info(ResourcesPricing.GetString("ApplyDateNotPast"))
            MsgBox(ResourcesItemBulkLoad.GetString("ApplyDateNotPast"), MsgBoxStyle.Critical, Me.Text)
            AutoApplyDateUDTE.Focus()
            Exit Sub
        End If

        If CDate(AutoApplyDateUDTE.Value) > ResourcesPricing.MaxSmalldatetimeDate Then
            logger.Info(ResourcesPricing.GetString("ApplyDateNotTooFarInFuture"))
            MsgBox(ResourcesPricing.GetString("ApplyDateNotTooFarInFuture"), MsgBoxStyle.Critical, Me.Text)
            AutoApplyDateUDTE.Focus()
            Exit Sub
        End If

        ' look at InstanceDataFlags to see if batch description is required
        If InstanceDataDAO.IsFlagActive("Required_BatchDescription") AndAlso (BatchDescriptionTextBox.Text Is Nothing Or BatchDescriptionTextBox.Text = "") Then
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), BatchDescLabel.Text))
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), BatchDescLabel.Text), MsgBoxStyle.Critical, Me.Text)
            BatchDescriptionTextBox.Focus()
            Exit Sub
        End If

        For i = 0 To ugrdList.Selected.Rows.Count - 1
            ugrdList.Selected.Rows(i).Cells("Selected").Value = 1
        Next

        ugrdList.UpdateData()

        If miItemChgTypeID = 4 Then
            Dim OfferTable As DataTable
            Dim OfferBO As PromotionOfferBO = New PromotionOfferBO
            Dim msg As String = String.Empty
            Dim result As DialogResult

            mdv.RowFilter = "Selected = 1"
            OfferTable = mdv.ToTable

            For Each dr As DataRow In OfferTable.Rows
                msg &= OfferBO.IsPromotionLocked(CInt(dr("Offer_Id")), Trim(dr("Item_Description").ToString))
            Next

            If msg.Length > 0 Then
                If (gbLockAdministrator Or gbSuperUser) Then
                    logger.Info(String.Format("One or more of the Promotional Offers you selected is currently being edited by another user. Do you still want to add these offers to a batch?" & vbCrLf & vbCrLf & "{0}", msg))
                    result = MsgBox(String.Format("One or more of the Promotional Offers you selected is currently being edited by another user. Do you still want to add these offers to a batch?" & vbCrLf & vbCrLf & "{0}", msg), MsgBoxStyle.YesNo)
                    If result = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If
                Else
                    logger.Info(String.Format("One or more of the Promotional Offers you selected is currently being edited by another user and cannot be added to a batch." & vbCrLf & vbCrLf & "{0}", msg))
                    MsgBox(String.Format("One or more of the Promotional Offers you selected is currently being edited by another user and cannot be added to a batch." & vbCrLf & vbCrLf & "{0}", msg), MsgBoxStyle.YesNo)
                    Exit Sub
                End If
            End If
        End If

        '20100706 - Dave Stacey - added a handler to check for competitive pricing batch date limit
        Dim bCompetitivePriceBatchDateLimit As Boolean = ConfigurationServices.AppSettings("CompetitivePriceBatchDateLimit")

        If bCompetitivePriceBatchDateLimit = True Then


            Dim iCompetitivePriceBatchCheckDateLimit As Integer = ConfigurationServices.AppSettings("CompetitivePriceBatchDateLimitDays")

            Dim ItemTable As DataTable
            mdv.RowFilter = "Selected = 1"
            ItemTable = mdv.ToTable
            Dim strKeyList As String = String.Empty
            Dim strCompDate As String = String.Empty
            Dim strEndDate As String = String.Empty
            Dim booPriceChgTypeStatus As Boolean
            Dim intPriceChgTypeID As Integer = 0

            strEndDate = "1/1/1900"
            For Each dr As DataRow In ItemTable.Rows
                strKeyList = strKeyList & CStr(dr("Store_No")) & "," & CStr(dr("Identifier")) & "|"

                If dr("Sale_End_Date").GetType IsNot GetType(DBNull) And dr("PriceChgTypeID").GetType IsNot GetType(DBNull) Then
                    If (CDate(dr("Sale_End_Date")) > CDate(strEndDate)) Then
                        strEndDate = CStr(dr("Sale_End_Date"))
                        intPriceChgTypeID = CInt(dr("PriceChgTypeID"))
                    End If
                End If
            Next
            If intPriceChgTypeID > 0 Then
                booPriceChgTypeStatus = PriceBatchSearchDAO.GetCompetitivePriceTypeStatus(intPriceChgTypeID)
            End If
            If booPriceChgTypeStatus = True Then
                If strEndDate <> "1/1/1900" Then
                    strCompDate = PriceBatchSearchDAO.GetCompetitiveDate(strKeyList)
                    If strCompDate = "Missing" Then
                        logger.Info(ResourcesPricing.GetString("CompetitiveBatchMissingItems"))
                        MsgBox(ResourcesItemBulkLoad.GetString("CompetitiveBatchMissingItems"), MsgBoxStyle.Critical, Me.Text)
                        'AutoApplyDateUDTE.Focus()
                        Exit Sub
                    ElseIf CDate(strEndDate) > CDate(strCompDate).AddDays(iCompetitivePriceBatchCheckDateLimit) Then
                        logger.Info(ResourcesPricing.GetString("CompetitiveBatchDateTooFar"))
                        MsgBox(ResourcesItemBulkLoad.GetString("CompetitiveBatchDateTooFar"), MsgBoxStyle.Critical, Me.Text)
                        AutoApplyDateUDTE.Focus()
                        Exit Sub
                    End If
                End If
            End If
        End If
        '--------------------------------------------
        'Apply the updates to the database
        '--------------------------------------------
        'open connection
        factory = New DataFactory(DataFactory.ItemCatalog)

        lStoreList = VB6.CopyArray(frmPricingBatch.GetStoreListAll)

        Dim lPChgList As ArrayList = PriceChgTypeDAO.GetPriceChgTypeList(True, False)
        'Dim ip As Integer
        Dim lItemChgList(6) As Integer
        lItemChgList(0) = 0
        lItemChgList(1) = 1
        lItemChgList(2) = 2
        lItemChgList(3) = 3
        lItemChgList(4) = 4
        lItemChgList(5) = 6
        Dim ipcg As Integer
        '20100124 - Dave Stacey - added a few loops here to handle multiple change and price type batching
        'PCT loop inside Item Change loop inside Store Loop ensures that all pct's will be batched seperately w/minimal impact on the code
        Dim bIncludeBatchDescriptionExtension As Boolean = ConfigurationServices.AppSettings("IncludeBatchDescriptionExtension")
        For i = 0 To UBound(lStoreList)
            For it = 0 To UBound(lItemChgList)
                For ipcg = 0 To lPChgList.Count
                    If lPChgList.Count > ipcg Then
                        mdv.RowFilter = "Store_No = " & lStoreList(i) & " AND ItemChgTypeID = " & lItemChgList(it) & " AND PriceChgTypeID = " & lPChgList(ipcg).PriceChgTypeID & " AND Selected = 1"
                    Else        'add a null to handle change types w/out corresponding price types
                        mdv.RowFilter = "Store_No = " & lStoreList(i) & " AND ItemChgTypeID = " & lItemChgList(it) & " AND Selected = 1"

                    End If
                    mdv.Sort = "ItemChgTypeID, PriceChgTypeID"

                    If mdv.Count > 0 Then
                        Try
                            'start database transaction
                            transaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

                            lNumPer = mdv.Count \ lNumBatches
                            If lNumPer = 0 Then lNumPer = 1

                            Dim iCnt As Integer
                            iCnt = mdv.Count - 1
                            Dim iPCTDesc As String
                            Dim iCTDesc As String
                            Dim iPCT As Integer
                            If Len(mdv.Item(iCnt).Item("PriceChgTypeID").ToString) > 0 Then
                                iPCT = mdv.Item(iCnt).Item("PriceChgTypeID")
                            Else
                                iPCT = 0
                            End If


                            Do While mdv.Count > 0

                                lNumInBatch = 0
                                sAddList = ""
                                'grab Change Type and Price Type off temp table instead of form controls
                                iCTDesc = Trim(mdv.Item(iCnt).Item("ItemChgTypeDesc"))
                                If Len(mdv.Item(iCnt).Item("PriceChgTypeDesc").ToString) > 0 Then
                                    iPCTDesc = Trim(mdv.Item(iCnt).Item("PriceChgTypeDesc"))
                                Else
                                    iPCTDesc = ""
                                End If
                                header = New PriceBatchHeaderBO

                                'extra batch info options might change if user is editing an existing batch or if this is a new batch
                                header.StoreNumber = lStoreList(i)
                                If bIncludeBatchDescriptionExtension = True Then
                                    header.BatchDescription = Trim(ConvertQuotes(BatchDescriptionTextBox.Text & iCTDesc & iPCTDesc))
                                Else
                                    header.BatchDescription = Trim(ConvertQuotes(BatchDescriptionTextBox.Text))
                                End If
                                header.AutoApplyFlag = AutoApplyCheckBox.Checked
                                header.AutoApplyDate = tempApplyDate

                                If mlPriceBatchHeaderID = 0 Then
                                    header.ItemChgTypeID = lItemChgList(it)
                                    If Len(mdv.Item(iCnt).Item("PriceChgTypeID").ToString) > 0 Then
                                        header.PriceChgTypeID = mdv.Item(iCnt).Item("PriceChgTypeID")
                                    End If
                                    header.StartDate = mdStartDate

                                    'query the db to see if there is a default POS batch id should be changed based on the price change type or item change type
                                    header.POSBatchId = headerDAO.GetDefaultPOSBatchID(header)

                                    'create new batch (new PriceBatchHeader record)
                                    lPriceBatchHeaderID = headerDAO.InsertPriceBatchHeader(header, transaction)
                                Else
                                    'if batch header info changed, then update header data
                                    If _isHeaderInfoChanged Then
                                        header.PriceBatchHeaderId = mlPriceBatchHeaderID
                                        PriceBatchDetailDAO.UpdatePriceBatchHeader(header)

                                        'update calling screen so batch header info will be populated
                                        RaiseEvent UpdateCallingForm(header)
                                    End If

                                    lPriceBatchHeaderID = mlPriceBatchHeaderID
                                End If

                                'Put the leftovers in the last batch - let it go over the number per batch
                                If (mdv.Count) < (2 * lNumPer) Then lNumPer = mdv.Count

                                Do While (lNumInBatch < lNumPer) And (mdv.Count > 0)

                                    ' Add to update list
                                    If Len(sAddList) + Len(CStr(mdv.Item(iCnt).Item("PriceBatchDetailID"))) > 8000 Then
                                        header = New PriceBatchHeaderBO
                                        header.PriceBatchDetailIDList = sAddList
                                        header.DetailIDListSeparator = "|"
                                        header.PriceBatchHeaderId = lPriceBatchHeaderID

                                        'update some details now associated w/ this PriceBatchHeader ID
                                        headerDAO.UpdatePriceBatchDetails(header, transaction)

                                        sAddList = CStr(mdv.Item(iCnt).Item("PriceBatchDetailID"))
                                    Else
                                        If Len(sAddList) > 0 Then
                                            sAddList = sAddList & "|" & CStr(mdv.Item(iCnt).Item("PriceBatchDetailID"))
                                        Else
                                            sAddList = CStr(mdv.Item(iCnt).Item("PriceBatchDetailID"))
                                        End If
                                    End If

                                    mdv.Delete(iCnt)
                                    iCnt = iCnt - 1
                                    lNumInBatch = lNumInBatch + 1

                                    If Len(sAddList) > 0 And ((lNumInBatch = lNumPer) And (mdv.Count = 0)) Then
                                        header = New PriceBatchHeaderBO
                                        header.ItemChgTypeID = lItemChgList(it)
                                        header.PriceChgTypeID = iPCT
                                        header.PriceBatchDetailIDList = sAddList
                                        header.DetailIDListSeparator = "|"
                                        header.PriceBatchHeaderId = lPriceBatchHeaderID

                                        'update all details now associated w/ this PriceBatchHeader ID
                                        headerDAO.UpdatePriceBatchDetails(header, transaction)
                                        mlPriceBatchHeaderID = 0
                                    End If
                                Loop

                            Loop

                            transaction.Commit()
                        Catch ex As Exception
                            If transaction IsNot Nothing Then
                                transaction.Rollback()
                            End If
                            Throw ex
                        Finally
                            If transaction IsNot Nothing Then
                                transaction.Dispose()
                            End If
                        End Try
                    End If
                Next
            Next
        Next

        '--------------------------------------------
        ' Refresh the UI screen for the next batch creation
        '--------------------------------------------
        'Refresh the grid from the ADO recordset
        mdv.RowFilter = ""
        Me.StatusBar1.Items.Item(1).Text = String.Format(ResourcesIRMA.GetString("NumberTotal"), mdv.Count)
        Me.StatusBar1.Items.Item(0).Text = String.Format(ResourcesIRMA.GetString("NumberSelected"), ugrdList.Selected.Rows.Count)

        'Reset the expanded batch fields
        Me.AutoApplyCheckBox.Checked = False
        Me.AutoApplyDateUDTE.Value = Nothing
        Me.BatchDescriptionTextBox.Text = Nothing

        logger.Debug("cmdSelect_Click Exit")

    End Sub

    Private Sub frmPricingBatchItemSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        logger.Debug("frmPricingBatchItemSearch_DoubleClick Enter")
        'mdv.RowStateFilter = DataViewRowState.None
        mdv.RowFilter = String.Empty
        logger.Debug("frmPricingBatchItemSearch_DoubleClick Exit")
    End Sub

    Private Sub frmPricingBatchItemSearch_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmPricingBatchItemSearch_FormClosed Enter")
        'If Not (mrsItem Is Nothing) Then
        '    If mrsItem.State = ADODB.ObjectStateEnum.adStateOpen Then mrsItem.Close()
        'End If

        mlPriceBatchHeaderID = 0
        miItemChgTypeID = 0
        miPriceChgTypeID = 0
        mdStartDate = System.DateTime.FromOADate(0)
        logger.Debug("frmPricingBatchItemSearch_FormClosed Exit")
    End Sub

    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        logger.Debug("optType_CheckedChanged Enter")
        If IsInitializing Then Exit Sub
        Dim incAll As Boolean = True

        If eventSender.Checked Then
            Dim Index As Short = optType.GetIndex(eventSender)
            cmbPriceType.Enabled = Not (optType(2).Checked Or optType(4).Checked)
            If optType(5).Checked = True Then
                incAll = False
            End If

            If _globalPriceManagementBO.IsGlobalPriceManagementEnabled AndAlso optType(3).Checked Then
                FilterStoreComboBoxToNonGpmStores()
            Else
                FilterStoreComboBoxToAllStores()
            End If
        End If

        If Not editExistingBatch Then
            Call SetOptions(incAll)
        End If
        logger.Debug("optType_CheckedChanged Exit")
    End Sub

    Private Sub FilterStoreComboBoxToAllStores()
        ucmbStoreList.DisplayLayout.Bands(0).ColumnFilters.ClearAllFilters()
    End Sub

    Private Sub FilterStoreComboBoxToNonGpmStores()
        Dim gpmFilter As FilterCondition = New FilterCondition(ucmbStoreList.DisplayLayout.Bands(0).Columns("IsGpmStore"), FilterComparisionOperator.Equals, False)
        ucmbStoreList.DisplayLayout.Bands(0).ColumnFilters("IsGpmStore").FilterConditions.Add(gpmFilter)
    End Sub

    Private Sub SetOptions(ByVal includeALL As Boolean)
        logger.Debug("SetOptions Enter")
        If optType(4).Checked = True Then
            cmbSubTeam.Enabled = False
            cmbSubTeam.SelectedIndex = -1
        Else
            LoadPriceTypeCombo(includeALL)
            cmbSubTeam.Enabled = True
            If cmbSubTeam.SelectedIndex = -1 Then
                cmbSubTeam.SelectedIndex = 0
            End If
        End If
        logger.Debug("SetOptions Exit")
    End Sub


    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        If IsInitializing Then Exit Sub

        logger.Debug("OptSelection_CheckedChanged Enter")

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)

            SetActive(ucmbStoreList, False)
            SetActive(cmbZones, False)
            SetActive(cmbState, False)

            Select Case Index
                Case 0 'Store
                    SetActive(ucmbStoreList, True)
                Case 1 'Zone
                    SetActive(cmbZones, True)
                Case 2 'State
                    SetActive(cmbState, True)
            End Select

        End If
        logger.Debug("OptSelection_CheckedChanged Exit")
    End Sub

  Private Sub txtBox_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDescription.Enter, txtIdentifier.Enter, txtNumBatch.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtIdentifier_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
        logger.Debug("txtIdentifier_KeyPress Enter")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)

        If Chr(KeyAscii) = "0" And Len(Trim(txtIdentifier.Text)) = 0 Then KeyAscii = 0

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("txtIdentifier_KeyPress Exit")
    End Sub

  Private Sub txtNumBatch_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        logger.Debug("txtNumBatch_KeyPress Enter")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtNumBatch.Tag), txtNumBatch, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("txtNumBatch_KeyPress Exit")
    End Sub

    Public Function RunSearch() As Boolean
        logger.Debug("RunSearch Enter")
        Dim sStores As String
        Dim lZone_ID As Integer
        Dim sState As String
        Dim sItemChgTypeID As String
        Dim sPriceChgTypeID As String
        Dim sSubTeam_No As String
        Dim sStartDate As String
        Dim sIdentifier As String
        Dim sItem_Description As String
        Dim bItems As Boolean
        Dim mbIncScaleItems As Boolean
        Dim mbIncNonRetailItems As Boolean
        sStores = String.Empty
        sState = String.Empty
        sItemChgTypeID = String.Empty
        sPriceChgTypeID = String.Empty
        sSubTeam_No = String.Empty
        sStartDate = String.Empty
        sIdentifier = String.Empty
        sItem_Description = String.Empty
        miItemChgTypeID = 0
        miPriceChgTypeID = 0
        mdStartDate = System.DateTime.FromOADate(0)
        mbIncScaleItems = 0
        mbIncNonRetailItems = 0

        Cursor = Cursors.WaitCursor

        ' Validate location selection
        Select Case True
            Case optSelection(0).Checked
                ' Validate store selection
                If ucmbStoreList.CheckedRows.Count <= 0 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(0).Text), MsgBoxStyle.Critical, Me.Text)
                    Exit Function
                Else
                    sStores = ComboValue(ucmbStoreList)
                End If

            Case optSelection(1).Checked
                ' Validate zone selection
                If cmbZones.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(1).Text), MsgBoxStyle.Critical, Me.Text)
                    Exit Function
                Else
                    lZone_ID = CInt(ComboValue(cmbZones))
                End If

            Case optSelection(2).Checked
                ' Validate state selection
                If cmbState.SelectedIndex = -1 Then
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text))
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), optSelection(2).Text), MsgBoxStyle.Critical, Me.Text)
                    Exit Function
                Else
                    sState = cmbState.Text
                End If
        End Select

        If Len(sStores) = 0 Or sStores = "NULL" Then
            Dim getNonGpmStores As Boolean = GetGetNonGpmStores()
            sStores = frmPricingBatch.GetStoreListString(Nothing, lZone_ID, sState, optSelection(3).Checked, optSelection(4).Checked, optSelection(5).Checked, getNonGpmStores)
        End If

        ' Validate change type selection
        Select Case True
            Case optType(0).Checked 'New
                sItemChgTypeID = "1"
                miItemChgTypeID = 1
            Case optType(1).Checked 'Item
                sItemChgTypeID = "2"
                miItemChgTypeID = 2
            Case optType(2).Checked 'Delete
                sItemChgTypeID = "3"
                miItemChgTypeID = 3
            Case optType(3).Checked 'Price
                sItemChgTypeID = ""
                miItemChgTypeID = 0
            Case optType(4).Checked 'Offer
                sItemChgTypeID = "4"
                miItemChgTypeID = 4
            Case optType(5).Checked 'All
                sItemChgTypeID = "5"
                miItemChgTypeID = 5
        End Select

        '20100124 - Dave Stacey - Remove validation checking that a price type was checked
        If cmbPriceType.SelectedIndex < 0 Then
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), fraPriceType.Text))
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), fraPriceType.Text), MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        miPriceChgTypeID = CType(cmbPriceType.SelectedItem, PriceChgTypeBO).PriceChgTypeID
        sPriceChgTypeID = miPriceChgTypeID.ToString()

		sSubTeam_No = IIf(cmbSubTeam.SelectedItem Is Nothing, 0, cmbSubTeam.SelectedItem.SubTeamNo).ToString()

		sStartDate = CDate(dtpStartDate.Value).ToString(ResourcesIRMA.GetString("DateStringFormat"))
        mdStartDate = dtpStartDate.Value

        If Len(txtIdentifier.Text) > 0 Then
            sIdentifier = txtIdentifier.Text
        End If

        If Len(txtDescription.Text) > 0 Then
            sItem_Description = ConvertQuotes(txtDescription.Text)
        End If
        mbIncScaleItems = chkIncScaleItems.Checked
        mbIncNonRetailItems = chkIncludeNonRetailItems.Checked

        'build business object w/ stored proc param data
        Dim searchItem As New PriceBatchSearchBO
        searchItem.StoreList = sStores
        searchItem.StoreListSeparator = "|"
        If Not sItemChgTypeID.Equals("") Then
            searchItem.ItemChgTypeID = sItemChgTypeID
        End If
        If Not sPriceChgTypeID.Equals("") Then
            searchItem.PriceChgTypeID = sPriceChgTypeID
        End If
        searchItem.SubTeamNo = sSubTeam_No
        searchItem.StartDate = mdStartDate
        searchItem.Identifier = sIdentifier
        searchItem.ItemDescription = sItem_Description
        searchItem.IncScaleItems = mbIncScaleItems
        searchItem.IncNonRetailItems = mbIncNonRetailItems

        Call LoadDataTable(searchItem)
        If ugrdList.Rows.Count > 0 Then bItems = True Else bItems = False

        RunSearch = bItems
        ugrdList.Focus()

        Cursor = Cursors.Arrow

        logger.Debug("RunSearch Exit")

    End Function

    Private Function GetGetNonGpmStores() As Boolean
        Return _globalPriceManagementBO.IsGlobalPriceManagementEnabled AndAlso Not _globalPriceManagementBO.AreAllStoresGpm AndAlso _optType_3.Checked
    End Function

	Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
		logger.Debug("cmbSubTeam_KeyPress Enter")
		Dim KeyAscii As Short = Asc(e.KeyChar)

		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
		logger.Debug("cmbSubTeam_KeyPress Exit")
	End Sub

	Private Sub cmbZones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbZones.KeyPress
        logger.Debug("cmbZones_KeyPress Enter")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbZones.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbZones_KeyPress Exit")
    End Sub

    Private Sub cmbState_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbState.KeyPress
        logger.Debug("cmbState_KeyPress Enter")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbState.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbState_KeyPress Exit")
    End Sub

    Private Sub ugrdList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdList.Click
        logger.Debug("ugrdList_Click Enter")
        Me.StatusBar1.Items.Item(0).Text = String.Format(ResourcesIRMA.GetString("NumberSelected"), ugrdList.Selected.Rows.Count)
        logger.Debug("ugrdList_Click Exit")
    End Sub

    Private Sub AutoApplyDateUDTE_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoApplyDateUDTE.ValueChanged
        logger.Debug("AutoApplyDateUDTE_ValueChanged Enter")
        If AutoApplyDateUDTE.Value = Nothing Then AutoApplyDateUDTE.Value = Now.Date

        If _isLoading Or IsInitializing Then Exit Sub
        _isHeaderInfoChanged = True
        logger.Debug("AutoApplyDateUDTE_ValueChanged Exit")
    End Sub

    Private Sub dtpStartDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpStartDate.ValueChanged
        logger.Debug("dtpStartDate_ValueChanged Enter")
        If dtpStartDate.Value = Nothing Then dtpStartDate.Value = Now.Date
        logger.Debug("dtpStartDate_ValueChanged Exit")
    End Sub

    Private Sub AutoApplyCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoApplyCheckBox.CheckedChanged
        logger.Debug("AutoApplyCheckBox_CheckedChanged Enter")
        AutoApplyDateUDTE.Enabled = (Not AutoApplyCheckBox.Checked)

        If _isLoading Then Exit Sub
        _isHeaderInfoChanged = True
        logger.Debug("AutoApplyCheckBox_CheckedChanged Exit")
    End Sub
    Private Sub BatchDescriptionTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BatchDescriptionTextBox.TextChanged
        logger.Debug("BatchDescriptionTextBox_TextChanged Enter")
        If _isLoading Then Exit Sub
        _isHeaderInfoChanged = True
        logger.Debug("BatchDescriptionTextBox_TextChanged Exit")
    End Sub

    Private Sub cmdBERTReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBERTReport.Click
        logger.Debug("cmdBERTReport_Click Enter")
        Dim sItemIdentifierList As String
        Dim sStoreNumberList As String
        Dim sPriceBatchDetailIdList As String
        Dim sReportURL As String
        Dim x As Integer

        sItemIdentifierList = String.Empty
        sStoreNumberList = String.Empty
        sPriceBatchDetailIdList = String.Empty
        sReportURL = String.Empty

        If ugrdList.Selected.Rows.Count > 0 Then
            Dim count As Integer
            Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each row In Me.ugrdList.Rows
                If row.Selected = True Then
                    count = count + 1
                End If
            Next
            If count < 50 Then 'DN 8/26/2009 per WI 9402 I need to limit the BERT report to select less than 50 rows at a time.  Allowing to many items to be selected in the grid caused the Report's URL to reach it's limit and be truncated
                For x = 0 To ugrdList.Selected.Rows.Count - 1
                    If sItemIdentifierList = "" Then
                        sItemIdentifierList = ugrdList.Selected.Rows(x).Cells("Identifier").Text
                        sStoreNumberList = ugrdList.Selected.Rows(x).Cells("Store_No").Text
                        sPriceBatchDetailIdList = ugrdList.Selected.Rows(x).Cells("PriceBatchDetailId").Text
                    Else
                        sItemIdentifierList = sItemIdentifierList & "|" & ugrdList.Selected.Rows(x).Cells("Identifier").Text
                        sStoreNumberList = sStoreNumberList & "|" & ugrdList.Selected.Rows(x).Cells("Store_No").Text
                        sPriceBatchDetailIdList = sPriceBatchDetailIdList & "|" & ugrdList.Selected.Rows(x).Cells("PriceBatchDetailId").Text
                    End If
                Next
                sReportURL = "SW_BERTReport&rs:Command=Render&rc:Parameters=false&ItemIdentifierList=" & sItemIdentifierList & "&StoreNumberList=" & sStoreNumberList
                Call ReportingServicesReport(sReportURL)
            Else
                MsgBox("Please select less than 50 rows", MsgBoxStyle.Exclamation, "Maximum number of rows to report on has been reached")
            End If
        Else
            MsgBox("At least one row must be selected.", MsgBoxStyle.Critical, Me.Text)
        End If
        logger.Debug("cmdBERTReport_Click Exit")
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged

        If chkSelectAll.Checked Then
            Me.SelectAllRows()
        Else
            ugrdList.Selected.Rows.Clear()
        End If
        Me.StatusBar1.Items.Item(0).Text = String.Format(ResourcesIRMA.GetString("NumberSelected"), ugrdList.Selected.Rows.Count)
    End Sub

    Private Sub ugrdList_ClickCell(sender As Object, e As Infragistics.Win.UltraWinGrid.ClickCellEventArgs) Handles ugrdList.ClickCell
        Me.chkSelectAll.Checked = False
    End Sub

    Private Sub ugrdList_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ugrdList.InitializeRow
        e.Row.Cells("StartDate").Activation = Activation.NoEdit
        e.Row.Cells("Sale_End_Date").Activation = Activation.NoEdit
        e.Row.Height = 17
    End Sub

    Public Sub SelectAllRows()
        Dim enabledRows As UltraGridRow() = ugrdList.Rows.Where(Function(r) r.Activation <> Activation.Disabled).ToArray()
        ugrdList.Selected.Rows.AddRange(enabledRows)
    End Sub
End Class
