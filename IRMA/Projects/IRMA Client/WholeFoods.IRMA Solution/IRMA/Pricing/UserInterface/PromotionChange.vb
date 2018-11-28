Option Strict Off
Option Explicit On

Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid

Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports System.ComponentModel ' bindinglist

Friend Class frmPromotionChange
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

    Private mdOrigStartDate As Date
    Private mStoreNo As Integer
    Private _itemBO As WholeFoods.IRMA.ItemHosting.BusinessLogic.ItemBO
    Private _editPriceBatchDetailID As Integer
    Private _methodList As BindingList(Of PricingMethodBO)

    Private mdt As DataTable

    Private mbFilling As Boolean
    Private mbPricingMethodLoaded As Boolean
    Private mbPriceTypeLoaded As Boolean

    Private mdtStores As DataTable

    Private Sub frmPromotionChange_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.CenterToParent()

        Call SetupDataTable()
        Call LoadStorePOSPrices()

        mbFilling = True

        SetActive(txtField(iPriceMultiple), False)
        SetActive(txtField(iPriceEarnedDiscount1), False)
        SetActive(txtField(iPriceEarnedDiscount2), False)
        SetActive(txtField(iPriceEarnedDiscount3), False)

        cmdItemVendors.Enabled = False

        Call ShowHideGridPrices()

        If Not mbPricingMethodLoaded Then LoadPricingMethodCombo()
        Call Pricingmethodsetup()

        Call SetupGrid()

        Call LoadStoresPopulateGrid()

        'Select the proper store.
        Call StoreListGridSelectStore(ugrdStoreList, mStoreNo)

        Call UpdateGridPrices()

        'Load the states combo based upon the list of stores.
        Call StoreListGridLoadStatesCombo(StoresUltraDataSource, cmbStates)

        mbFilling = False

        Call SetCombos()

        frmItemPricePendSearch.PopulateRetailStoreZoneDropDown(cmbZones)

        If Not mbPriceTypeLoaded Then LoadPriceTypeCombo()

        mbPricingMethodLoaded = False
        mbPriceTypeLoaded = False

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If
    End Sub

    Private Sub RefreshGrid()

        cmdItemVendors.Enabled = False

        Call LoadStoresPopulateGrid()

        'Select the proper store.
        Call StoreListGridSelectStore(ugrdStoreList, mStoreNo)

        Call UpdateGridPrices()
        'Load the states combo based upon the list of stores.
        Call StoreListGridLoadStatesCombo(StoresUltraDataSource, cmbStates)

        mbFilling = False

        Call SetCombos()

        frmItemPricePendSearch.PopulateRetailStoreZoneDropDown(cmbZones)

        If Not mbPriceTypeLoaded Then LoadPriceTypeCombo()

        mbPricingMethodLoaded = False
        mbPriceTypeLoaded = False
    End Sub

    Private Sub SetupGrid()
        'Grid must be setup at run time since it is bound and uses the datasource as the source for columns.

        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_Name").Width = 100
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Zone_ID").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("State").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("WFM_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Mega_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("CustomerType").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("TaxRate").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Width = 40
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Promo Price").Width = 60
        ugrdStoreList.DisplayLayout.Bands(0).Columns("hasVendor").Hidden = True

    End Sub
    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("StorePOSPrices")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("POSPrice", GetType(Double)))
    End Sub
    Private Function FindStorePOSPrice(ByVal Store_No As Integer) As Double
        Dim POSPrice As Double = 0.0
        Dim i As Integer
        Dim storeNo As Integer

        If mdt.Rows.Count > 0 Then
            For i = 0 To mdt.Rows.Count - 1
                storeNo = CInt(mdt.Rows(i).Item(0))
                If storeNo = Store_No Then
                    POSPrice = CDbl(mdt.Rows(i).Item(1))
                    Exit For
                End If
            Next
        End If

        Return POSPrice
    End Function
    Private Sub LoadStorePOSPrices()

        Dim row As DataRow
        Dim rsStorePrices As DAO.Recordset = Nothing

        Try
            SQLOpenRS(rsStorePrices, "EXEC GetCurrentPrices " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            mdt.Rows.Clear()

            While Not rsStorePrices.EOF
                row = mdt.NewRow
                row("Store_No") = rsStorePrices.Fields("Store_No").Value
                row("POSPrice") = rsStorePrices.Fields("POSPrice").Value

                mdt.Rows.Add(row)

                rsStorePrices.MoveNext()
            End While

            mdt.AcceptChanges()

        Finally
            If rsStorePrices IsNot Nothing Then
                rsStorePrices.Close()
            End If
        End Try

    End Sub

    Private Sub LoadStoresPopulateGrid()

        Dim iRow As Integer = 0
        Dim rsStores As DAO.Recordset = Nothing

        Try
            SQLOpenRS(rsStores, "EXEC GetRetailStoresAndTaxRates " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            StoresUltraDataSource.Rows.Clear()

            'Set the number of rows in the DataSource.
            If Not (rsStores.EOF And rsStores.BOF) Then
                rsStores.MoveLast()
                StoresUltraDataSource.Rows.SetCount(rsStores.RecordCount)
                rsStores.MoveFirst()
            End If

            While Not rsStores.EOF
                PopGridRow(rsStores.Fields, iRow)
                CheckHasVendor(rsStores.Fields, iRow)
                DisableRowIfStoreIsOnGpm(rsStores.Fields("Store_No").Value, iRow)
                iRow = iRow + 1
                rsStores.MoveNext()
            End While
        Finally
            If rsStores IsNot Nothing Then
                rsStores.Close()
            End If
        End Try

    End Sub

    Private Sub PopGridRow(ByVal rsFields As DAO.Fields, ByVal RowNdx As Integer)
        Dim row As UltraDataRow

        'Get the first row.
        row = Me.StoresUltraDataSource.Rows(RowNdx)
        row("Store_No") = rsFields("Store_No").Value
        row("Store_Name") = rsFields("Store_Name").Value
        row("Zone_ID") = rsFields("Zone_ID").Value
        row("State") = rsFields("State").Value
        row("WFM_Store") = rsFields("WFM_Store").Value
        row("Mega_Store") = rsFields("Mega_Store").Value
        row("CustomerType") = rsFields("CustomerType").Value
        row("TaxRate") = rsFields("TaxRate").Value
        row("Price") = ""
        row("Promo Price") = ""
        row("hasVendor") = rsFields("hasVendor").Value
    End Sub

    Private Sub CheckHasVendor(ByVal rsFields As DAO.Fields, ByVal iRow As Integer)

        If rsFields("hasVendor").Value = False Then
            ugrdStoreList.Rows(iRow).Activation = Activation.Disabled
            cmdItemVendors.Enabled = True
        End If
    End Sub

    Private Sub DisableRowIfStoreIsOnGpm(ByVal storeNo As String, ByVal iRow As Integer)
        If (InstanceDataDAO.IsFlagActive("GlobalPriceManagement", storeNo) = True) Then
            ugrdStoreList.Rows(iRow).Activation = Activation.Disabled
        End If
    End Sub

    Private Sub UpdateGridPrices()
        Dim price As String
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow

        'Determine the tax rate for UK or US
        If InstanceDataDAO.IsFlagActive("UseVAT") Then
            'For the UK, the Calced Price displayed in the grid (POS Price - VAT Tax) will be saved to the "Price" field in the DB.
            For Each row In ugrdStoreList.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
                If row.Selected = True Then
                    'Calc Price.
                    If Not IsDBNull(txtPOSPrice.Value) Then
                        If txtPOSPrice.Value > 0 Then
                            price = GetPriceWithoutVAT(txtPOSPrice.Value, Convert.ToDouble(row.Cells("TaxRate").Value))
                            row.Cells("Price").Value = VB6.Format(price, "####0.00")
                        Else
                            row.Cells("Price").Value = ""
                        End If
                    Else
                        row.Cells("Price").Value = ""
                    End If

                    'Calc Promo Price.
                    If Not IsDBNull(txtPOSPromoPrice.Value) Then
                        If txtPOSPromoPrice.Value > 0 Then
                            price = GetPriceWithoutVAT(txtPOSPromoPrice.Value, Convert.ToDouble(row.Cells("TaxRate").Value))
                            row.Cells("Promo Price").Value = VB6.Format(price, "####0.00")
                        Else
                            row.Cells("Promo Price").Value = ""
                        End If
                    Else
                        row.Cells("Promo Price").Value = ""
                    End If
                Else
                    row.Cells("Price").Value = ""
                    row.Cells("Promo Price").Value = ""
                End If

                row.Update()
            Next row
        Else
            'For the US, the POS Price will be saved to the "Price" field in the DB.
            For Each row In ugrdStoreList.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
                If row.Selected = True Then
                    'Price
                    If IsNumeric(txtPOSPrice.Value) AndAlso txtPOSPromoPrice.Value IsNot DBNull.Value Then
                        row.Cells("Price").Value = VB6.Format(1 * txtPOSPrice.Value, "####0.00")
                    Else
                        row.Cells("Price").Value = ""
                    End If
                    'Promo Price
                    If IsNumeric(txtPOSPromoPrice.Value) AndAlso txtPOSPromoPrice.Value IsNot DBNull.Value Then
                        row.Cells("Promo Price").Value = VB6.Format(1 * txtPOSPromoPrice.Value, "####0.00")
                    Else
                        row.Cells("Promo Price").Value = ""
                    End If

                Else
                    row.Cells("Price").Value = ""
                    row.Cells("Promo Price").Value = ""
                End If

                row.Update()
            Next row
        End If

    End Sub

    Public WriteOnly Property StoreNo() As Integer
        Set(ByVal Value As Integer)
            mStoreNo = Value
        End Set
    End Property

    Public WriteOnly Property SaleMultiple() As Byte
        Set(ByVal Value As Byte)
            txtField(iPriceMultiple).Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property POSSalePrice() As Decimal
        Set(ByVal Value As Decimal)
            txtPOSPromoPrice.Value = Value ''''''''''''''''''''
        End Set
    End Property
    Private MyPOSPrice As Decimal
    Public Property POSSalePrice1() As Decimal
        Get
            Return MyPOSPrice
        End Get
        Set(ByVal Value As Decimal)
            MyPOSPrice = Value
        End Set

    End Property

    Public WriteOnly Property StartDate() As Date
        Set(ByVal Value As Date)
            mdOrigStartDate = Value
            dtpStartDate.Value = Value
        End Set
    End Property

    Public WriteOnly Property EndDate() As Date
        Set(ByVal Value As Date)
            dtpEndDate.Value = Value
        End Set
    End Property

    Public WriteOnly Property MSRPMultiple() As Byte
        Set(ByVal Value As Byte)
            txtMSRPMultiple.Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property MSRPPrice() As Decimal
        Set(ByVal Value As Decimal)
            txtMSRPPrice.Value = Value  ', "##0.00")
        End Set
    End Property

    Public WriteOnly Property EarnedDiscount1() As Byte
        Set(ByVal Value As Byte)
            txtField(iPriceEarnedDiscount1).Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property EarnedDiscount2() As Byte
        Set(ByVal Value As Byte)
            txtField(iPriceEarnedDiscount2).Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property EarnedDiscount3() As Byte
        Set(ByVal Value As Byte)
            txtField(iPriceEarnedDiscount3).Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property PricingMethodID() As Integer
        Set(ByVal Value As Integer)
            mbFilling = True
            'must load drop down with values before setting the selected value
            LoadPricingMethodCombo()
            cmbPricingMethod.SelectedValue = Value
            mbFilling = False
        End Set
    End Property

    Public WriteOnly Property PriceType() As Integer
        Set(ByVal Value As Integer)
            mbFilling = True

            'must load drop down with values before setting the selected value
            LoadPriceTypeCombo()

            cmbPriceType.SelectedValue = False
            mbFilling = False
        End Set
    End Property

    Public WriteOnly Property RegularMultiple() As Byte
        Set(ByVal Value As Byte)
            txtMultiple.Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property RegularPOSPrice() As Decimal
        Set(ByVal Value As Decimal)
            txtPOSPrice.Text = VB6.Format(Value, "##0.00")
        End Set
    End Property

    Public WriteOnly Property AllowRegularPriceChange() As Boolean
        Set(ByVal Value As Boolean)
            SetActive((Me.txtMultiple), Value)
            SetActive((Me.txtPOSPrice), Value)
        End Set
    End Property

    Public WriteOnly Property ItemBO() As WholeFoods.IRMA.ItemHosting.BusinessLogic.ItemBO
        Set(ByVal Value As WholeFoods.IRMA.ItemHosting.BusinessLogic.ItemBO)
            _itemBO = Value
        End Set
    End Property

    Public WriteOnly Property EditPriceBatchDetailID() As Integer
        Set(ByVal Value As Integer)
            _editPriceBatchDetailID = Value
        End Set
    End Property

    Private Sub LoadPricingMethodCombo()
        Dim Dao As PricingMethodDAO = New PricingMethodDAO

        mbPricingMethodLoaded = True

        _methodList = Dao.GetPromoScreenPricingMethodList
        With cmbPricingMethod

            .DisplayMember = "Name"
            .ValueMember = "PricingMethodID"
            .DataSource = _methodList

            If cmbPricingMethod.Items.Count > 0 Then cmbPricingMethod.SelectedIndex = GetRegPromoIndex(_methodList)
        End With
    End Sub

    Private Function GetRegPromoIndex(ByVal methodList As BindingList(Of PricingMethodBO)) As Integer
        Dim i As Integer

        For i = 0 To methodList.Count - 1
            If methodList.Item(i).PricingMethodID = 0 Then 'Regular Promo ID
                Return i
            End If
        Next

        Return 0
    End Function

    Private Sub LoadPriceTypeCombo()
        Dim priceChgDAO As New PriceChgTypeDAO
        Dim priceChgList As ArrayList = PriceChgTypeDAO.GetPriceChgTypeList(False, False)

        mbPriceTypeLoaded = True

        cmbPriceType.DataSource = priceChgList
        If priceChgList.Count > 0 Then
            cmbPriceType.DisplayMember = "PriceChgTypeDesc"
            cmbPriceType.ValueMember = "PriceChgTypeID"
        End If
        cmbPriceType.SelectedValue = False
    End Sub

    Private Sub cmbPricingMethod_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbPricingMethod.SelectedIndexChanged
        If IsInitializing Or mbFilling Then Exit Sub

        Call Pricingmethodsetup()

    End Sub

    Private Sub Pricingmethodsetup()

        '  There may be an easier way to determine the currentMethod than an Enum, but this works
        '  Just using the index doesn't since there will likely be gaps in PricingMethod_Id
        Dim currentMethod As PricingMethodBO
        Dim methodEnum As IEnumerator = _methodList.GetEnumerator
        While methodEnum.MoveNext
            currentMethod = CType(methodEnum.Current, PricingMethodBO)
            If currentMethod.PricingMethodID = cmbPricingMethod.SelectedValue Then
                SetActive(_txtField_0, currentMethod.EnableSaleMultiple)
                If currentMethod.EnableSaleMultiple = False Then
                    _txtField_0.Text = "1"
                End If
                SetActive(_txtField_4, currentMethod.EnableEarnedRegMultiple)
                _txtField_4.Text = currentMethod.EarnedRegMultipleDefault.ToString
                SetActive(_txtField_5, currentMethod.EnableEarnedSaleMultiple)
                _txtField_5.Text = currentMethod.EarnedSaleMultipleDefault.ToString
                SetActive(_txtField_6, currentMethod.EnableEarnedLimit)
                _txtField_6.Text = currentMethod.EarnedLimitDefault.ToString
            End If
        End While

        'code for defaulting the Promo price to 0.00 when BOGO method has been selected.
        If cmbPricingMethod.SelectedValue = "2" Then
            txtPOSPromoPrice.Value = "0.00"
        Else
            txtPOSPromoPrice.Value = POSSalePrice1
        End If

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Processes the possible validation error messages and displays the appropriate message
    ''' to the user.  These are data validation messages that are common to all stores, such as missing a 
    ''' required field or impropertly formatted data.
    ''' </summary>
    ''' <param name="currentStatus"></param>
    ''' <remarks></remarks>
    Private Function DisplayValidationError(ByVal currentStatus As PriceChangeStatus) As Boolean
        Dim showErrorMsg As Boolean = False

        Select Case currentStatus
            Case PriceChangeStatus.Error_RegMultipleGreaterZero
                MsgBox(ResourcesItemHosting.GetString("RegularMultipleNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtMultiple.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_RegPriceGreaterEqualZero
                MsgBox(ResourcesItemHosting.GetString("RegularPriceNotNull"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_RegPriceGreaterZero
                MsgBox(ResourcesItemHosting.GetString("RegularPriceNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleMultipleGreaterZero
                MsgBox(ResourcesItemHosting.GetString("SaleMultipleNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtField(iPriceMultiple).Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePriceGreaterEqualZero
                MsgBox(ResourcesItemHosting.GetString("SalePriceNotNull"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPromoPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePriceGreaterZero
                MsgBox(ResourcesItemHosting.GetString("SalePriceNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPromoPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePriceMustEqualZero
                MsgBox(ResourcesItemHosting.GetString("SalePriceMethodNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPromoPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleStartAndEndDatesRequired
                MsgBox(ResourcesIRMA.GetString("StartAndEndDatesRequired"), MsgBoxStyle.Critical, Me.Text)
                If dtpStartDate.Value Is Nothing Then
                    dtpStartDate.Focus()
                Else
                    dtpEndDate.Focus()
                End If
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleStartDateInPast
                MsgBox(ResourcesIRMA.GetString("StartDateNotPast"), MsgBoxStyle.Critical, Me.Text)
                dtpStartDate.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleStartDateGreaterMaxDBDate
                MsgBox(ResourcesPricing.GetString("StartDateMaxValue"), MsgBoxStyle.Critical, Me.Text)
                dtpStartDate.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleEndDateAfterSaleStartDate
                MsgBox(ResourcesItemHosting.GetString("SaleEndDateGreaterThanStart"), MsgBoxStyle.Critical, Me.Text)
                dtpEndDate.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SaleEndDateGreaterMaxDBDate
                MsgBox(ResourcesPricing.GetString("EndDateMaxValue"), MsgBoxStyle.Critical, Me.Text)
                dtpEndDate.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_MSRPMultipleGreaterZero
                MsgBox(ResourcesItemHosting.GetString("MSRPMultipleNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtMSRPMultiple.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_MSRPPriceGreaterZero
                MsgBox(ResourcesItemHosting.GetString("MSRPRequired"), MsgBoxStyle.Critical, Me.Text)
                txtMSRPPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePricingMethodRequired
                MsgBox(ResourcesItemHosting.GetString("PricingMethodRequired"), MsgBoxStyle.Critical, Me.Text)
                cmbPricingMethod.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePriceChgTypeIDRequired
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_6.Text), MsgBoxStyle.Critical, Me.Text)
                cmbPriceType.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_PriceQuantityGreaterZero
                MsgBox(ResourcesItemHosting.GetString("QuantityRegularPriceNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtField(iPriceEarnedDiscount1).Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_SalePriceLimitGreaterZero
                MsgBox(ResourcesItemHosting.GetString("EarnedSaleLimitNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtField(iPriceEarnedDiscount3).Focus()
                showErrorMsg = True
        End Select

        Return showErrorMsg
    End Function

    ''' <summary>
    ''' Populates the warning and error message recordsets.  These are used for item-store specific messages.
    ''' </summary>
    ''' <param name="rsWarning"></param>
    ''' <param name="rsError"></param>
    ''' <param name="promoChangeData"></param>
    ''' <param name="validationStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
     Private Function ProcessWarningAndErrorMessages(ByRef rsWarning As ADODB.Recordset, ByRef rsError As ADODB.Recordset, ByRef promoChangeData As PriceChangeBO, ByVal validationStatus As PriceChangeStatus) As Boolean
        Dim showValError As Boolean = False

        ' Check to see if any error or warning conditions were returned for this particular item-store.
        Select Case validationStatus
            Case PriceChangeStatus.Valid
                'Valid data - do nothing
            Case PriceChangeStatus.Warning_SaleConflictsWithRegPriceChange
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = promoChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesItemHosting.GetString("OverlappingRegularPrices")
            Case PriceChangeStatus.Warning_SaleConflictsWithSalePriceChange
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = promoChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesItemHosting.GetString("OverlappingPromoPrices")
            Case PriceChangeStatus.Warning_SaleCurrentlyOngoing
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = promoChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesItemHosting.GetString("OnSale")
            Case PriceChangeStatus.Error_SaleWithPriceChangeInBatch
                'Error
                rsError.AddNew()
                rsError.Fields("Store_Name").Value = promoChangeData.StoreName
                rsError.Fields("ErrorMessage").Value = ResourcesItemHosting.GetString("IsBatchedSale")
            Case PriceChangeStatus.Error_ExistingPromoWithSameStartDate
                'Error
                rsError.AddNew()
                rsError.Fields("Store_Name").Value = promoChangeData.StoreName
                rsError.Fields("ErrorMessage").Value = ResourcesItemHosting.GetString("OverlappingSamePromoPrices")
            Case Else
                ' Check to see if this is a validation message that is being displayed
                showValError = DisplayValidationError(validationStatus)
                If Not showValError Then
                    ' This was an unknown error message.  
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(validationStatus)
                    MsgBox(String.Format(ResourcesPricing.GetString("UnknownPriceChgError"), vbCrLf, validationStatus, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical, Me.Text)
                    showValError = True
                End If
        End Select
        Return showValError
    End Function

    ''' <summary>
    ''' Apply the promo price changes to the database.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        Dim validationStatus As PriceChangeStatus
        Dim showValError As Boolean
        Dim rsWarning As ADODB.Recordset
        Dim rsError As ADODB.Recordset

        '-- Populate a PriceChangeBO with the data submitted by the user
        Dim promoChangeData As New PriceChangeBO
        promoChangeData.ItemKey = glItemID
        promoChangeData.UserId = giUserID
        promoChangeData.UserIdDate = frmItemPricePendSearch.LockDate
        promoChangeData.PriceChgType = IIf(cmbPriceType.SelectedItem Is Nothing, Nothing, CType(cmbPriceType.SelectedItem, PriceChgTypeBO))
        promoChangeData.StartDate = IIf(dtpStartDate.Value Is Nothing, System.DateTime.MinValue, CDate(dtpStartDate.Value))
        promoChangeData.RegMultiple = IIf(txtMultiple.Text = "", 0, txtMultiple.Text)
        promoChangeData.RegPOSPrice = IIf(txtPOSPrice.Value.Equals(DBNull.Value), 0, txtPOSPrice.Value)
        promoChangeData.MSRPPrice = IIf(txtMSRPPrice.Value.Equals(DBNull.Value), 0, txtMSRPPrice.Value)
        promoChangeData.MSRPMultiple = IIf(txtMSRPMultiple.Text = "", 0, txtMSRPMultiple.Text)
        promoChangeData.PricingMethodId = IIf(cmbPricingMethod.SelectedValue Is Nothing, -1, cmbPricingMethod.SelectedValue)
        promoChangeData.SaleMultiple = IIf(txtField(iPriceMultiple).Text = "", 0, txtField(iPriceMultiple).Text)
        promoChangeData.SalePOSPrice = IIf(txtPOSPromoPrice.Value.Equals(DBNull.Value), 0, txtPOSPromoPrice.Value)
        promoChangeData.SaleEndDate = IIf(dtpEndDate.Value Is Nothing, System.DateTime.MinValue, CDate(dtpEndDate.Value))
        promoChangeData.Sale_EarnedDisc1 = IIf(txtField(iPriceEarnedDiscount1).Text = "", 0, txtField(iPriceEarnedDiscount1).Text)
        promoChangeData.Sale_EarnedDisc2 = IIf(txtField(iPriceEarnedDiscount2).Text = "", 0, txtField(iPriceEarnedDiscount2).Text)
        promoChangeData.Sale_EarnedDisc3 = IIf(txtField(iPriceEarnedDiscount3).Text = "", 0, txtField(iPriceEarnedDiscount3).Text)
        promoChangeData.PriceBatchDetailId = _editPriceBatchDetailID
        promoChangeData.LineDrive = False
        promoChangeData.InsertApplication = "IRMA Client"

        '-- Validate the data that is common to all stores
        showValError = DisplayValidationError(promoChangeData.ValidatePromoPriceChangeDataFormatting())
        If showValError Then
            ' An error message is being displayed to the user.  Stop processing.
            Exit Sub
        End If

        '-- Verify at least one store was selected to receive this promotion
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        '-- Validation against pending and current data
        ' Create warning and error recordsets to drop problems into (rsWarning and rsError).
        rsWarning = New ADODB.Recordset
        rsWarning.Fields.Append("Store_Name", ADODB.DataTypeEnum.adVarChar, 255)
        rsWarning.Fields.Append("WarningMessage", ADODB.DataTypeEnum.adVarChar, 255)
        rsWarning.Fields.Append("ActionMessage", ADODB.DataTypeEnum.adVarChar, 255)
        rsWarning.Open()

        rsError = New ADODB.Recordset
        rsError.Fields.Append("Store_Name", ADODB.DataTypeEnum.adVarChar, 255)
        rsError.Fields.Append("ErrorMessage", ADODB.DataTypeEnum.adVarChar, 255)
        rsError.Open()

        ' Populate the warning and error recordsets for each store selected.
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrdStoreList.Selected.Rows
            ' Update the values in the business object that are specific to the store being processed
            promoChangeData.StoreNo = CInt(row.Cells("Store_No").Value)
            promoChangeData.StoreName = row.Cells("Store_Name").Value
            ' US: the POS Price will be saved to both the Price and POSPrice fields in the database.
            ' UK: the POS Price will be saved in the POSPrice field and the Calculated Price (POS Price – VAT Tax) that is displayed in 
            '    the “Stores” grid will be stored in the Price field in the database.  The grid is updated each time the user changes the
            '    price on the UI.
            promoChangeData.RegPrice = IIf(row.Cells("Price").Value = "", 0, row.Cells("Price").Value)
            promoChangeData.SalePrice = IIf(row.Cells("Promo Price").Value = "", 0, row.Cells("Promo Price").Value)

            ' Validate the price change logic for this store.
            validationStatus = promoChangeData.ValidatePromoPriceChangeData()

            ' Check to see if a store-item specific error or warning was encountered.  
            ' Add the message to a queue that will be displayed to the user once all item-stores are validated.
            showValError = ProcessWarningAndErrorMessages(rsWarning, rsError, promoChangeData, validationStatus)

            ' Check to see if an error message was displayed now because it is common data validation.
            ' (Note: These should be covered by the ValidatePromoPriceChangeDataFormatting sub, but they are repeated in
            '        the DB validation to make sure all applications work consistently.)
            If showValError Then
                ' An error message has been shown to the user.  Stop processing.
                Exit Sub
            End If
        Next row

        ' Display the error messages to the user and stop processing.
        If rsError.RecordCount > 0 Then
            Dim errmsg As String
            errmsg = ResourcesIRMA.GetString("ChangesNotApplied") + Chr(10)
            rsError.MoveFirst()
            Do While Not rsError.EOF
                errmsg = errmsg + "For store: " + CStr(rsError.Fields("Store_Name").Value).Trim + ", "
                errmsg = errmsg + CStr(rsError.Fields("ErrorMessage").Value) + Chr(10)
                rsError.MoveNext()
            Loop
            MessageBox.Show(errmsg, "Change Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            rsError.Close()
            rsWarning.Close()
            Exit Sub
        ElseIf rsWarning.RecordCount > 0 Then
            ' Display the warning messages to the user.
            Dim warnmsg As String
            warnmsg = ResourcesIRMA.GetString("ApplyChanges") + Chr(10)
            rsWarning.MoveFirst()
            Do While Not rsWarning.EOF
                warnmsg = warnmsg + "For store: "
                warnmsg = warnmsg + CStr(rsWarning.Fields("Store_Name").Value).Trim + ", "
                warnmsg = warnmsg + CStr(rsWarning.Fields("WarningMessage").Value) + ", "
                warnmsg = warnmsg + CStr(rsWarning.Fields("ActionMessage").Value) + Chr(10)
                rsWarning.MoveNext()
            Loop
            If MessageBox.Show(warnmsg, "Change Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                ' Stop processing if the user does not want to continue after seeing the warning.
                rsError.Close()
                rsWarning.Close()
                Exit Sub
            End If
        End If
        rsError.Close()
        rsWarning.Close()

        '-- Save the updates for the stores that passed validation and the user said "OK" to the warnings
        For Each row In ugrdStoreList.Selected.Rows
            ' Update the values in the business object that are specific to the store being processed
            promoChangeData.StoreNo = CInt(row.Cells("Store_No").Value)
            promoChangeData.StoreName = row.Cells("Store_Name").Value
            ' US: the POS Price will be saved to both the Price and POSPrice fields in the database.
            ' UK: the POS Price will be saved in the POSPrice field and the Calculated Price (POS Price – VAT Tax) that is displayed in 
            '    the “Stores” grid will be stored in the Price field in the database.  The grid is updated each time the user changes the
            '    price on the UI.
            promoChangeData.RegPrice = IIf(row.Cells("Price").Value = "", 0, row.Cells("Price").Value)
            promoChangeData.SalePrice = IIf(row.Cells("Promo Price").Value = "", 0, row.Cells("Promo Price").Value)

            ' Save the price change to the database.
            Dim saveStatus As Integer = promoChangeData.SavePromoPriceChange()
            If saveStatus <> 0 Then  ' 0 is the VALID code
                ' A validation error was encountered during the save.  Let the user know and exit processing.
                ' Make sure it wasn't just a warning.
                If ValidationDAO.IsErrorCode(saveStatus) Then
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(saveStatus)
                    MsgBox(String.Format(ResourcesPricing.GetString("UnknownPriceChgError"), vbCrLf, saveStatus, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            End If
        Next

        Me.Close()
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)

    If txtField(Index).ReadOnly = False Then
      KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub txtBox_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtMSRPMultiple.Enter, txtMSRPPrice.Enter, txtMultiple.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtMSRPMultiple_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMSRPMultiple.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If txtMSRPMultiple.ReadOnly = False Then
      KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtMSRPMultiple.Tag), txtMSRPMultiple, 0, 0, 0)
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub txtMultiple_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMultiple.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If txtMultiple.ReadOnly = False Then
      KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtMultiple.Tag), txtMultiple, 0, 0, 0)
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub txtPrice_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPOSPrice.Enter
        txtPOSPrice.SelectAll()
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        If IsInitializing Or mbFilling Then Exit Sub

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
            End Select

            Call UpdateGridPrices()

            mbFilling = False

        End If
    End Sub

    Private Sub SetCombos()

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

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        If IsInitializing Or mbFilling Then Exit Sub
        mbFilling = True
        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))
        Call UpdateGridPrices()
        mbFilling = False
    End Sub

    ' for bug 5442: not being able to tab to the "By State" option button (_optSelection_3) from cboZones
    ' or tabbing from cmbStates to ugrdStoreList
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
            ugrdStoreList.Focus()
            CurrentKey = Nothing
        End If
    End Sub
    ' for bug 5442: end

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        If mbFilling Or IsInitializing Then Exit Sub
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())
    End Sub

    Private Sub ugrdStoreList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If mbFilling Or IsInitializing Then Exit Sub
        optSelection.Item(0).Checked = True
    End Sub

    Private Sub txtPOSPromoPrice_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPOSPromoPrice.Enter
        txtPOSPromoPrice.SelectAll()
    End Sub

    Private Sub txtPOSPromoPrice_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPOSPromoPrice.ValueChanged
        Call UpdateGridPrices()
    End Sub

    Private Sub txtPOSPrice_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPOSPrice.ValueChanged
        Call UpdateGridPrices()
    End Sub

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange
        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        optSelection(0).Checked = True
        mbFilling = False

        Dim storeNo As Integer
        storeNo = CInt(ugrdStoreList.Selected.Rows(0).GetCellValue("Store_No"))

        Call UpdateGridPrices()
    End Sub

    Private Sub checkboxPriceDisplay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkboxPriceDisplay.CheckedChanged
        Call ShowHideGridPrices()
    End Sub

    Private Sub ShowHideGridPrices()
        If checkboxPriceDisplay.Checked Then
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Width = 110
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Promo Price").Width = 150
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Hidden = False
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Promo Price").Hidden = False
        Else
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Hidden = True
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Promo Price").Hidden = True
         End If
    End Sub

    Private Sub cmdItemVendors_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemVendors.Click
        Dim fItemVendors As New frmItemVendors(glItemID)
        fItemVendors.Text = ResourcesItemHosting.GetString("VendorTitle") & " " & "[" & gsItemDescription & "]"
        fItemVendors.ShowDialog()
        fItemVendors.Close()
        fItemVendors.Dispose()
        RefreshGrid()
    End Sub

    Private Sub Button_MarginInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.Click
        Dim marginInfoForm As New MarginInfo

        marginInfoForm.ItemBO = _itemBO
        marginInfoForm.ShowDialog()
        marginInfoForm.Dispose()
    End Sub

#Region "Tabbing code"

    ' fix for bug 5355
    Private Sub ugrdStoreList_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ugrdStoreList.KeyPress
        If e.KeyChar = vbTab Then
            checkboxPriceDisplay.Focus()
        End If
    End Sub

#End Region

End Class
