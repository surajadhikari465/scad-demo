Option Strict Off
Option Explicit On

Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic

'For the UK, the Calced Price displayed in the grid (POS Price - VAT Tax) will be saved to the "Price" field in the DB.
'For the US, the POS Price will be saved to the "Price" field in the DB.
'The "POS Price" will always be saved to the "POSPrice" field.

Friend Class frmPriceChange
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private mbFilling As Boolean

    Private mdOrigStartDate As Date
    Private mStoreNo As Integer
    Private _itemBO As ItemBO

    Private Sub frmPriceChange_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'set optional date default to today
        dtStartDate.Value = Today

        cmdItemVendors.Enabled = False

        Call SetupGrid()

        Call ShowHideGridPrices()

        Call LoadStoresPopulateGrid()

        'Select the proper store.
        Call StoreListGridSelectStore(ugrdStoreList, mStoreNo)

        'Load the states combo based upon the list of stores.
        Call StoreListGridLoadStatesCombo(StoresUltraDataSource, cmbStates)

        Call SetCombos()

        frmItemPricePendSearch.PopulateRetailStoreZoneDropDown(cmbZones)

        If Not CheckAllStoreSelectionEnabled() Then
            AllRadioButton.Text = "All 365"
        End If

    End Sub

#Region "Properties"

    Public WriteOnly Property StoreNo() As Integer
        Set(ByVal Value As Integer)
            mStoreNo = Value
        End Set
    End Property

    Public WriteOnly Property Multiple() As Short
        Set(ByVal Value As Short)
            Me.txtMultiple.Text = CStr(Value)
        End Set
    End Property

    Public WriteOnly Property POSPrice() As Decimal
        Set(ByVal Value As Decimal)
            txtPOSPrice.Value = Value  ', "##0.00")
        End Set
    End Property

    Public WriteOnly Property StartDate() As Date
        Set(ByVal Value As Date)
            mdOrigStartDate = Value

            'TSP 2007-03-21: added error handling for invalid dates (TFS #1970)
            '   - not sure why setting the control's value here is still necessary since the control is set to today's date in the form load event
            Try
                dtStartDate.Value = Value

            Catch ex1 As ArgumentException
                'swallow exception
                'likely an invalid value such as:
                '   "The control's value cannot be set outside the range determined by the 'MinValue' and 'MaxValue' properties"

            Catch ex As Exception
                Dim sMsg As String = "An error occurred setting the Start Date!"
                If ex.InnerException IsNot Nothing Then
                    sMsg = String.Format("{0}{1}{1} {2}{1} {3}", sMsg, vbCrLf, ex.Message, ex.InnerException.Message)
                Else
                    sMsg = String.Format("{0}{1}{1} {2}", sMsg, vbCrLf, ex.Message)
                End If
                MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, My.Application.Info.Title & " - frmPriceChange.StartDate()")

                Throw ex
            End Try
        End Set
    End Property

    Public WriteOnly Property AvgCost() As Decimal
        Set(ByVal Value As Decimal)
            lblCost.Text = String.Format(ResourcesPricing.GetString("AvgCostText"), VB6.Format(Value & "", "##0.00"))
        End Set
    End Property

    Public WriteOnly Property ItemBO() As ItemBO
        Set(ByVal Value As ItemBO)
            _itemBO = Value
        End Set
    End Property

#End Region

#Region "Grid Code"

    Private Sub RefreshGrid()

        cmdItemVendors.Enabled = False

        Call LoadStoresPopulateGrid()

        'Select the proper store.
        Call StoreListGridSelectStore(ugrdStoreList, mStoreNo)

        'Load the states combo based upon the list of stores.
        Call StoreListGridLoadStatesCombo(StoresUltraDataSource, cmbStates)

        Call SetCombos()

        frmItemPricePendSearch.PopulateRetailStoreZoneDropDown(cmbZones)
    End Sub

    Private Sub SetupGrid()
        'Grid must be setup at run time since it is bound and uses the datasource as the source for columns.

        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_Name").Width = 130
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Zone_ID").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("State").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("WFM_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Mega_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("CustomerType").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("TaxRate").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Width = 48
        ugrdStoreList.DisplayLayout.Bands(0).Columns("hasVendor").Hidden = True

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
                iRow = iRow + 1
                rsStores.MoveNext()
            End While

            Call UpdateGridPrices()
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
        row("hasVendor") = rsFields("hasVendor").Value
    End Sub

    Private Sub CheckHasVendor(ByVal rsFields As DAO.Fields, ByVal iRow As Integer)

        If rsFields("hasVendor").Value = False Then
            ugrdStoreList.Rows(iRow).Activation = Activation.Disabled
            cmdItemVendors.Enabled = True
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
                Else
                    row.Cells("Price").Value = ""
                End If
                row.Update()

            Next row
        Else
            'For the US, the POS Price will be saved to the "Price" field in the DB.
            For Each row In ugrdStoreList.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
                If row.Selected = True Then
                    If IsNumeric(txtPOSPrice.Value) Then
                        row.Cells("Price").Value = VB6.Format(1 * txtPOSPrice.Value, "####0.00")
                    Else
                        row.Cells("Price").Value = ""
                    End If
                Else
                    row.Cells("Price").Value = ""
                End If
                row.Update()
            Next row
        End If

    End Sub

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange
        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        ManualRadioButton.Checked = True
        mbFilling = False

        Call UpdateGridPrices()
    End Sub

#End Region

#Region "Button Code"

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
                MsgBox(ResourcesPricing.GetString("MultipleGreater"), MsgBoxStyle.Critical, Me.Text)
                txtMultiple.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_RegPriceGreaterEqualZero
                MsgBox(ResourcesPricing.GetString("PriceNotNull"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_RegPriceGreaterZero
                MsgBox(ResourcesPricing.GetString("PriceNotZero"), MsgBoxStyle.Critical, Me.Text)
                txtPOSPrice.Focus()
                showErrorMsg = True
            Case PriceChangeStatus.Error_RegStartDateInPast
                MsgBox(ResourcesIRMA.GetString("StartDateNotPast"), MsgBoxStyle.Critical, Me.Text)
                dtStartDate.Focus()
                showErrorMsg = True
        End Select

        Return showErrorMsg
    End Function

    ''' <summary>
    ''' Populates the warning and error message recordsets.  These are used for item-store specific messages.
    ''' </summary>
    ''' <param name="rsWarning"></param>
    ''' <param name="rsError"></param>
    ''' <param name="priceChangeData"></param>
    ''' <param name="validationStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessWarningAndErrorMessages(ByRef rsWarning As ADODB.Recordset, ByRef rsError As ADODB.Recordset, ByRef priceChangeData As PriceChangeBO, ByVal validationStatus As PriceChangeStatus) As Boolean
        Dim showValError As Boolean = False

        ' Check to see if any error or warning conditions were returned for this particular item-store.
        Select Case validationStatus
            Case PriceChangeStatus.Valid
                'Valid data - do nothing
            Case PriceChangeStatus.Warning_RegConflictsWithRegPriceChange
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = priceChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesPricing.GetString("OverlappingRegularPrice")
            Case PriceChangeStatus.Warning_RegConflictsWithSalePriceChange
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = priceChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesPricing.GetString("OverlappingPromoPrice")
            Case PriceChangeStatus.Warning_RegWithSaleCurrentlyOngoing
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = priceChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesPricing.GetString("CurrentSale")
            Case PriceChangeStatus.Warning_RegWithPriceChangeInBatch
                'Warn
                rsWarning.AddNew()
                rsWarning.Fields("Store_Name").Value = priceChangeData.StoreName
                rsWarning.Fields("WarningMessage").Value = ResourcesPricing.GetString("IsBatchedReg")
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
    ''' Apply the regular price changes to the database.
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
        Dim priceChangeData As New PriceChangeBO
        priceChangeData.ItemKey = glItemID
        priceChangeData.UserId = giUserID
        priceChangeData.UserIdDate = frmItemPricePendSearch.LockDate
        priceChangeData.StartDate = IIf(dtStartDate.Value Is Nothing Or dtStartDate.Value Is DBNull.Value, System.DateTime.MinValue, CDate(dtStartDate.Value))
        priceChangeData.RegMultiple = IIf(txtMultiple.Text = "", 0, txtMultiple.Text)
        priceChangeData.RegPOSPrice = IIf(txtPOSPrice.Value Is DBNull.Value, 0, txtPOSPrice.Value)
        priceChangeData.OldStartDate = CDate(mdOrigStartDate)
        priceChangeData.InsertApplication = "IRMA Client"

        ' Set the PriceChgTypeID value for the regular price change.
        priceChangeData.PriceChgType = PriceChgTypeDAO.GetRegularPriceChgTypeData()

        '-- Validate the data that is common to all stores
        showValError = DisplayValidationError(priceChangeData.ValidateRegularPriceChangeDataFormatting())
        If showValError Then
            ' An error message is being displayed to the user.  Stop processing.
            Exit Sub
        End If

        '-- Verify at least one store was selected to receive this promotion
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
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
            priceChangeData.StoreNo = CInt(row.Cells("Store_No").Value)
            priceChangeData.StoreName = row.Cells("Store_Name").Value
            ' US: the POS Price will be saved to both the Price and POSPrice fields in the database.
            ' UK: the POS Price will be saved in the POSPrice field and the Calculated Price (POS Price – VAT Tax) that is displayed in 
            '    the “Stores” grid will be stored in the Price field in the database.  The grid is updated each time the user changes the
            '    price on the UI.
            priceChangeData.RegPrice = IIf(row.Cells("Price").Value = "", 0, row.Cells("Price").Value)

            ' Validate the price change logic for this store.
            validationStatus = priceChangeData.ValidateRegularPriceChangeData()

            ' Check to see if a store-item specific error or warning was encountered.  
            ' Add the message to a queue that will be displayed to the user once all item-stores are validated.
            showValError = ProcessWarningAndErrorMessages(rsWarning, rsError, priceChangeData, validationStatus)

            ' Check to see if an error message was displayed now because it is common data validation.
            ' (Note: These should be covered by the ValidateRegularPriceChangeDataFormatting sub, but they are repeated in
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
            priceChangeData.StoreNo = CInt(row.Cells("Store_No").Value)
            priceChangeData.StoreName = row.Cells("Store_Name").Value
            ' US: the POS Price will be saved to both the Price and POSPrice fields in the database.
            ' UK: the POS Price will be saved in the POSPrice field and the Calculated Price (POS Price – VAT Tax) that is displayed in 
            '    the “Stores” grid will be stored in the Price field in the database.  The grid is updated each time the user changes the
            '    price on the UI.
            priceChangeData.RegPrice = IIf(row.Cells("Price").Value = "", 0, row.Cells("Price").Value)

            ' Save the price change to the database.
            Dim saveStatus As Integer = priceChangeData.SaveRegularPriceChange()
            If saveStatus <> 0 Then ' 0 is the VALID code
                ' A validation error was encountered during the save.  Let the user know and exit processing.
                ' Make sure it wasn't just a warning.
                If ValidationDAO.IsErrorCode(saveStatus) Then
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(validationStatus)
                    MsgBox(String.Format(ResourcesPricing.GetString("UnknownPriceChgError"), vbCrLf, validationStatus, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            End If
        Next

        Me.Close()
    End Sub

#End Region

#Region "Combo Code"

    Private Sub SetCombos()

        mbFilling = True

        'Zones.
        If ZoneRadioButton.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If StateRadioButton.Checked = True Then
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

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        Call UpdateGridPrices()

        mbFilling = False

    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        ZoneRadioButton.Checked = True

        Call SelectZone()

    End Sub

    ' for bug 5442: not being able to tab to the "By State" option button (StateRadioButton) from cboZones
    ' or tabbing from cmbStates to ugrdStoreList
    ' Rick Kelleher 3/4/08
    ' start
    Private CurrentKey As Integer

    Private Sub cmbZones_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles cmbZones.PreviewKeyDown
        CurrentKey = e.KeyValue
    End Sub

    Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
        If CurrentKey = 9 Then
            StateRadioButton.Focus()
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

    'Private Sub cmbStates_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.LostFocus
    '    ugrdStoreList.Focus()
    'End Sub

    ' for bug 5442: end

#End Region

#Region "Text Box Code"

    Private Sub txtPrice_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPOSPrice.Enter

        txtPOSPrice.SelectAll()

    End Sub

    Private Sub txtPOSPrice_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPOSPrice.LostFocus
        dtStartDate.Focus()
    End Sub

    Private Sub txtPrice_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPOSPrice.ValueChanged

        Call UpdateGridPrices()

    End Sub

    Private Sub txtMultiple_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        HighlightText(txtMultiple)

    End Sub

    Private Sub txtMultiple_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtMultiple.Tag), txtMultiple, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

#End Region

#Region "Option Button Code"

    Private Sub ManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        cmbZones.SelectedIndex = -1
        cmbStates.SelectedIndex = -1
        Call UpdateGridPrices()
        mbFilling = False

    End Sub

    Private Sub ZoneRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoneRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()
        Call SelectZone()

    End Sub

    Private Sub SelectZone()

        mbFilling = True

        ugrdStoreList.Selected.Rows.Clear()
        If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
        Call UpdateGridPrices()
        mbFilling = False

    End Sub


    Private Sub StateRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
        Call UpdateGridPrices()
        mbFilling = False

    End Sub

    Private Sub AllRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        If CheckAllStoreSelectionEnabled() Then
            Call StoreListGridSelectAll(ugrdStoreList, True)
        Else
            Call StoreListGridSelectAll365(ugrdStoreList)
        End If
        Call UpdateGridPrices()
        mbFilling = False

    End Sub

    Private Sub AllWFMRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        Call StoreListGridSelectAllWFM(ugrdStoreList)
        Call UpdateGridPrices()
        mbFilling = False

    End Sub
#End Region

#Region "Check Box Code"

    Private Sub checkboxPriceDisplay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkboxPriceDisplay.CheckedChanged

        Call ShowHideGridPrices()

    End Sub

    Private Sub ShowHideGridPrices()

        If checkboxPriceDisplay.Checked Then
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Width = 100
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Hidden = False

        Else
            ugrdStoreList.DisplayLayout.Bands(0).Columns("Price").Hidden = True

        End If

    End Sub

#End Region

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

    Private Function IsBatched(ByRef iStore_No As Integer) As Boolean
        Dim bResult As Boolean
        Dim rsResult As DAO.Recordset = Nothing

        Try
            SQLOpenRS(rsResult, "EXEC GetIsBatched " & glItemID & ", '" & iStore_No & "', '|', NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            bResult = rsResult.Fields("IsBatched").Value

        Finally
            If rsResult IsNot Nothing Then
                rsResult.Close()
            End If
        End Try
        IsBatched = bResult
    End Function

#Region "Tabbing code"

    ' fix for bug 5355
    Private Sub ugrdStoreList_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ugrdStoreList.KeyPress
        If e.KeyChar = vbTab Then
            checkboxPriceDisplay.Focus()
        End If
    End Sub

    ' Commented out by Rick Kelleher 2/27/08 while fixing bug 5441

    'Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
    '    StateRadioButton.Focus()
    'End Sub

    'Private Sub cmbZones_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.LostFocus
    '    StateRadioButton.Focus()
    'End Sub

    'Private Sub ManualRadioButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ManualRadioButton.Leave
    '    AllRadioButton.Focus()
    'End Sub

    'Private Sub ManualRadioButton_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ManualRadioButton.LostFocus
    '    AllRadioButton.Focus()
    'End Sub

    'Private Sub AllRadioButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllRadioButton.Leave
    '    AllWFMRadioButton.Focus()
    'End Sub

    'Private Sub AllRadioButton_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllRadioButton.LostFocus
    '    AllWFMRadioButton.Focus()
    'End Sub

    'Private Sub AllWFMRadioButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.Leave
    '    ZoneRadioButton.Focus()
    'End Sub

    'Private Sub AllWFMRadioButton_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.LostFocus
    '    ZoneRadioButton.Focus()
    'End Sub

    'Private Sub ZoneRadioButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoneRadioButton.Leave
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub ZoneRadioButton_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoneRadioButton.LostFocus
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub StateRadioButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles StateRadioButton.Leave
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub StateRadioButton_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles StateRadioButton.LostFocus
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub cmdExit_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.Leave
    '    txtMultiple.Focus()
    'End Sub

    'Private Sub cmdExit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.LostFocus
    '    txtMultiple.Focus()
    'End Sub

    'Private Sub txtMultiple_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMultiple.Leave
    '    txtPOSPrice.Focus()
    'End Sub

    'Private Sub txtMultiple_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMultiple.LostFocus
    '    txtPOSPrice.Focus()
    'End Sub

    'Private Sub dtStartDate_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtStartDate.Leave
    '    ManualRadioButton.Focus()
    'End Sub

    'Private Sub dtStartDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtStartDate.LostFocus
    '    ManualRadioButton.Focus()
    'End Sub

    'Private Sub Button_MarginInfo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.Leave
    '    cmdItemVendors.Focus()
    'End Sub

    'Private Sub Button_MarginInfo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.LostFocus
    '    cmdItemVendors.Focus()
    'End Sub

    'Private Sub cmdItemVendors_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdItemVendors.Leave
    '    cmdSelect.Focus()
    'End Sub

    'Private Sub cmdItemVendors_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdItemVendors.LostFocus
    '    cmdSelect.Focus()
    'End Sub

    'Private Sub cmdSelect_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.Leave
    '    cmdExit.Focus()
    'End Sub

    'Private Sub cmdSelect_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.LostFocus
    '    cmdExit.Focus()
    'End Sub

    'Private Sub ugrdStoreList_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.Leave
    '    checkboxPriceDisplay.Focus()
    'End Sub

    'Private Sub ugrdStoreList_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.LostFocus
    '    checkboxPriceDisplay.Focus()
    'End Sub

    'Private Sub checkboxPriceDisplay_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxPriceDisplay.Leave
    '    Button_MarginInfo.Focus()
    'End Sub

    'Private Sub checkboxPriceDisplay_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxPriceDisplay.LostFocus
    '    Button_MarginInfo.Focus()
    'End Sub
#End Region

End Class
