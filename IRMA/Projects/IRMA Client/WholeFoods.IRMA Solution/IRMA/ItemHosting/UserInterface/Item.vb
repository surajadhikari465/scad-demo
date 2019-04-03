Option Strict Off
Option Explicit On

Imports System.Text
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.TaxHosting.DataAccess
Imports WholeFoods.Utility

Friend Class frmItem
    Inherits System.Windows.Forms.Form

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private isInitializing As Boolean
    Private mdtBrand As DataTable
    Private mdtTaxClass As DataTable
    Private mgBydt As DataTable
    Private mOrigTaxClassID As Integer
    Private m_bIsScaleItem As Boolean
    Private msUser_ID_Date As String
    Private mUser_ID As Integer
    Private _scaleData As ScaleBO
    Private _posItemData As POSItemBO
    Private _itemData As ItemBO

    Dim bLoading As Boolean
    Dim pbDataChanged As Boolean
    Dim pbTaxClassChanged As Boolean
    Dim pbPrimary_Vendor As Boolean
    Dim rsItem As DAO.Recordset
    Dim rsItemChange As DAO.Recordset
    Dim mChangeExists As Boolean

    Dim WithEvents scaleFormDetails As Form_ItemScaleDetails
    Dim _scaleBusiness As ScaleDetailsBO
    Dim WithEvents posForm As POSItemInfo
    Dim WithEvents _itemAttributesForm As ItemAttributeUpdateForm
    Dim WithEvents _itemSignAttributesForm As ItemSignAttributeForm
    Dim WithEvents itemOverrideForm As ItemOverride

    Dim mbRollbackBatches As Boolean = False
    Dim sBatchInfo_Rollback As String
    Dim sBatchId_Rollback As String
    Dim sBatchInfo_Update As String
    Dim sBatchId_Update As String
    Dim iconBrandId As Integer?
    Dim msDiff As String

    Public pbUserSubTeam As Boolean
    Public pbDeleted As Boolean
    Public PSaveClick As Boolean = False
    Public plItem_Key As Integer
    Public plSubTeam_No As Integer
    Public plCategory_Id As Integer

    Private Sub frmItem_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        bLoading = True

        '-- Load data into the ComboBoxes
        mdtBrand = GetBrandData()
        cmbBrand.DataSource = mdtBrand
        cmbBrand.DisplayMember = "Brand_Name"
        cmbBrand.ValueMember = "Brand_ID"
        cmbBrand.SelectedIndex = -1



        mgBydt = ItemManagerDAO.GetItemManagers()
        cmbManagedBy.DataSource = mgBydt
        cmbManagedBy.DisplayMember = "Value"
        cmbManagedBy.ValueMember = "Manager_ID"
        cmbManagedBy.SelectedIndex = -1

        LoadOrigin(cmbField(iItemOrigin_ID))

        LoadSustainabilityRankings(cmbSustainRankingDefault)

        mdtTaxClass = GetTaxClassificationData()
        cmbTaxClass.DataSource = mdtTaxClass
        cmbTaxClass.DisplayMember = "TaxClassDesc"
        cmbTaxClass.ValueMember = "TaxClassID"
        cmbTaxClass.SelectedIndex = -1
        mOrigTaxClassID = 0

        cmbLabelType.DataSource = LabelTypeDAO.GetLabelTypeList
        cmbLabelType.DisplayMember = "LabelTypeDesc"
        cmbLabelType.ValueMember = "LabelTypeID"
        cmbLabelType.SelectedIndex = -1

        ReplicateCombo(cmbField(iItemOrigin_ID), cmbField(iItemCountryProc_ID))

        LoadItemType(cmbField(iitemItemType_ID))

        LoadNatClass(cmbField(iItemNatClassID))

        '--Clear text labels.
        lbl_InsertDate.Text = String.Empty
        lbl_UserIDDate.Text = String.Empty
        lbl_UserID.Text = String.Empty
        Call LoadSubTeamByType(enumSubTeamType.All, Me.HierarchySelector1.cmbSubTeam)

        Call LoadDistSubTeam(cmbDistSubTeam)

        HideFields()

        '-- Open data source.
        If glItemID > 0 Then
            RefreshDataSource(glItemID)
        Else
            RefreshDataSource(-1)
        End If

        bLoading = False
    End Sub

    Public ReadOnly Property Identifier() As String
        Get
            Identifier = txtField(1).Text
        End Get
    End Property

    Public ReadOnly Property IsScaleItem() As Boolean
        Get
            IsScaleItem = m_bIsScaleItem
        End Get
    End Property

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        '-- Force the validation event
        If plItem_Key > -1 Then
            If Not lblReadOnly.Visible Then
                If Not SaveData() Then
                    Exit Sub
                End If
            End If
        End If

        glItemID = 0

        If InstanceDataDAO.IsFlagActive("NewItemAutoSku") Then
            '-- Using Auto SKU
            '-- The identifier will be a sku and its value will be the auto generated
            '-- value from the Item's PK. This is set in a stored proc when the user adds the item.
            Dim fItemAdd As New frmItemAdd

            '-- Hardcode ident type to SKU and set the value to nothing
            '-- to tell the stored proc to assign the Item's PK value as the SKU value.
            fItemAdd.psIdentifierType = "S"
            fItemAdd.psIdentifier = Nothing
            fItemAdd.psCheckDigit = Nothing
            fItemAdd.ShowDialog()
            fItemAdd.Close()
            fItemAdd.Dispose()
        Else
            '-- Call the new item identifier form to start the add process
            frmItemIdentifierAdd.pbAddToDatabase = False

            frmItemIdentifierAdd.ShowDialog()

            frmItemIdentifierAdd.Close()
            frmItemIdentifierAdd.Dispose()
        End If

        '-- a new Item was added
        If glItemID <> 0 Then
            '-- Reload brand since user can add one when adding an item
            mdtBrand = GetBrandData()
            cmbBrand.DataSource = mdtBrand
            cmbBrand.DisplayMember = "Brand_Name"
            cmbBrand.ValueMember = "Brand_ID"
            cmbBrand.SelectedIndex = -1

            '-- Put the new data in
            RefreshDataSource(glItemID)

            '-- go to enter the other new data
            txtField(iItemItem_Description).Focus()
        ElseIf plItem_Key > -1 Then
            RefreshDataSource(plItem_Key)
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim dDeleteDate As Date

        frmDeleteDate.DeleteDate = SystemDateTime(True)
        frmDeleteDate.ShowDialog()

        If Not frmDeleteDate.Cancelled Then
            dDeleteDate = frmDeleteDate.DeleteDate
            frmDeleteDate.Close()
            frmDeleteDate.Dispose()

            If dDeleteDate = System.DateTime.FromOADate(0) Then
                MsgBox(ResourcesItemHosting.GetString("DeleteDateRequired"), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            frmDeleteDate.Close()
            frmDeleteDate.Dispose()
            Exit Sub
        End If

        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        ' Task 2178 - Added Delete Date/By
        SQLExecute("EXEC DeleteItemInventory " & plItem_Key & "," & giUserID & ",'" & VB6.Format(dDeleteDate, ResourcesIRMA.GetString("DateStringFormat")) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        RefreshDataSource(plItem_Key)
    End Sub

    Private Sub cmdHistory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdHistory.Click
        glItemID = plItem_Key

        frmItemHistory.Identifier = Me.txtField(iItemIdentifier).Text
        frmItemHistory.Text = ResourcesItemHosting.GetString("ItemOrderHistory") & " " & txtField(iItemItem_Description).Text
        frmItemHistory.ShowDialog()
        frmItemHistory.Close()
        frmItemHistory.Dispose()
    End Sub

    Private Sub cmdIngrZoom_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        On Error GoTo me_err

        Dim fBigText As New frmBigText

        fBigText.txtBigText.MaxLength = txtField(iItemIngredients).MaxLength
        fBigText.Text = Me.Text & " - " & ResourcesItemHosting.GetString("Ingredients")
        fBigText.txtBigText.Text = txtField(iItemIngredients).Text
        fBigText.psValidationTag = txtField(iItemIngredients).Tag
        CenterForm(frmBigText)
        fBigText.ShowDialog()

        If Not fBigText.pbCancel Then
            txtField(iItemIngredients).Text = fBigText.txtBigText.Text
            pbDataChanged = True 'Make sure this gets set
        End If

        fBigText.Close()

me_exit:
        fBigText.Dispose()
        Exit Sub

me_err:
        MsgBox(ResourcesItemHosting.GetString("IngredientError") & " " & Err.Number & " - " & Err.Description)
        GoTo me_exit
    End Sub

    Private Sub cmdInventory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdInventory.Click
        glItemID = plItem_Key

        Dim fItemOnHand As New frmItemOnHand

        fItemOnHand.Text = ResourcesItemHosting.GetString("CurrentUnits") & " " & txtField(iItemItem_Description).Text
        fItemOnHand.ShowDialog()
        fItemOnHand.Close()
        fItemOnHand.Dispose()
    End Sub

    Private Sub cmdItemVendors_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemVendors.Click
        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        glItemID = plItem_Key
        Dim fItemVendors As New frmItemVendors(plItem_Key)
        fItemVendors.Text = ResourcesItemHosting.GetString("VendorTitle") & " " & txtField(iItemItem_Description).Text
        fItemVendors.ShowDialog()
        fItemVendors.Close()
        fItemVendors.Dispose()

        RefreshDataSource(plItem_Key)
    End Sub

    Private Sub cmdIdentifier_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdIdentifier.Click
        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        glItemID = plItem_Key

        Dim fItemIdentifierList As New frmItemIdentifierList

        fItemIdentifierList.Text = ResourcesItemHosting.GetString("IdentifiersTitle") & " " & txtField(iItemItem_Description).Text
        fItemIdentifierList.ShowDialog()
        m_bIsScaleItem = fItemIdentifierList.IsScaleItem
        fItemIdentifierList.Close()
        fItemIdentifierList.Dispose()

        RefreshDataSource(plItem_Key)
    End Sub

    Private Sub cmdBrandAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrandAdd.Click
        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glBrandID = 0
        frmBrandAdd.ShowDialog()
        frmBrandAdd.Close()
        frmBrandAdd.Dispose()

        If glBrandID = -2 Then
            cmbBrand.DataSource = Nothing
            cmbBrand.Items.Clear()
            mdtBrand = GetBrandData()
            cmbBrand.DataSource = mdtBrand

            cmbBrand.DisplayMember = "Brand_Name"
            cmbBrand.ValueMember = "Brand_ID"

            bLoading = True
            For lLoop = 0 To cmbBrand.Items.Count - 1
                cmbBrand.SelectedIndex = lLoop
                If cmbBrand.SelectedValue > lMaxValue Then
                    lMaxValue = cmbBrand.SelectedValue
                    lMax = lLoop
                End If
            Next lLoop
            cmbBrand.SelectedIndex = lMax
            bLoading = False
        End If
    End Sub

    Private Sub cmdPrices_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrices.Click
        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        glStoreID = -1
        glItemID = plItem_Key
        gsItemDescription = txtField(iItemItem_Description).Text
        glItemSubTeam = plSubTeam_No
        frmItemPrice.ShowDialog()
        frmItemPrice.Close()
        frmItemPrice.Dispose()
        GC.Collect()

        RefreshDataSource(glItemID)
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        Dim itemSearchScreen As New frmItemSearch
        itemSearchScreen.ShowDialog()
        itemSearchScreen.Close()

        '-- if its not zero, then something was found
        If glItemID <> 0 Then
            RefreshDataSource(glItemID)
        Else
            RefreshDataSource(plItem_Key)
        End If

        itemSearchScreen.Dispose()
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdShipper_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdShipper.Click
        logger.Info("Shipper details button clicked...")

        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If

        RefreshDataSource(plItem_Key)

        ' Make sure this item is still a Shipper.
        If Not _itemData.IsShipper And chkField(iItemShipper_Item).CheckState = CheckState.Unchecked Then
            logger.Warn(String.Format(ShipperMessages.WARN_SHIPPER_ITEM_NOT_SHIPPER_AFTER_ITEMS_BTN_CLICKED & "  [ShipperKey={0}, Identifier={1}, IsShipper={2}, ShipperChkBoxChecked={3}]", plItem_Key, _itemData.Identifier, _itemData.IsShipper, chkField(iItemShipper_Item).CheckState))
            MessageBox.Show(ShipperMessages.WARN_SHIPPER_ITEM_NOT_SHIPPER_AFTER_ITEMS_BTN_CLICKED, ShipperMessages.CAPTION_SHIPPER_VIEW_ITEMS, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Show Shipper-Items screen.
        Try
            Dim fShipperList As New frmShipperList(_itemData.Shipper)
            fShipperList.ShowDialog()
            fShipperList.Close()
            fShipperList.Dispose()
        Catch ex As Exception
            logger.Error(ex.Message, ex)
            ErrorDialog.HandleError(ex, ErrorDialog.NotificationTypes.DialogAndEmail)
        End Try

        ' Update Shipper's pack info.
        Try
            If SaveData() Then _itemData.Shipper.UpdateInfo(plItem_Key)
        Catch ex As Exception
            logger.Error(ex.Message, ex)
            ErrorDialog.HandleError(ex, ErrorDialog.NotificationTypes.DialogAndEmail)
        End Try

        RefreshDataSource(plItem_Key)
    End Sub

    Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click
        If MsgBox(ResourcesIRMA.GetString("UnlockRecord"), MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
            SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshDataSource(plItem_Key)
        End If
    End Sub

    Private Sub cmdTaxClassZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTaxClassZoom.Click
        If Me.cmbTaxClass.SelectedValue <= 0 Then
            MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_selectTaxClass"), ResourcesTaxHosting.GetString("label_header_taxClass")), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim taxFlagForm As New Form_ViewTaxFlags

            taxFlagForm.TaxClassID = Me.cmbTaxClass.SelectedValue
            taxFlagForm.ItemKey = plItem_Key
            taxFlagForm.ShowDialog(Me)
            taxFlagForm.Close()
            taxFlagForm.Dispose()
        End If
    End Sub

    ''' <summary>
    ''' opens SCALE pop up window populated w/ scale data for current item
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdScale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdScale.Click
        scaleFormDetails = New Form_ItemScaleDetails()

        'set item identifier and default jurisdiction values
        scaleFormDetails.ItemIdentifier = Me.txtField(iItemIdentifier).Text
        scaleFormDetails.Text = scaleFormDetails.Text & " - " & Me.txtField(iItemIdentifier).Text
        scaleFormDetails.ItemKey = glItemID
        scaleFormDetails.StoreJurisdictionId = _itemData.StoreJurisdictionID
        scaleFormDetails.StoreJurisdictionDesc = _itemData.StoreJurisdictionDesc

        scaleFormDetails.ShowDialog(Me)
        scaleFormDetails.Close()
        scaleFormDetails.Dispose()
    End Sub

    ''' <summary>
    ''' opens Attributes pop up window populated w/ attribute data for current item
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdAttributes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAttributes.Click
        _itemAttributesForm = New ItemAttributeUpdateForm()

        'set item identifier and description
        _itemAttributesForm.Identifier = Me.txtField(iItemIdentifier).Text
        _itemAttributesForm.Description = Me.txtField(iItemItem_Description).Text

        'open child form
        _itemAttributesForm.ShowDialog()
        _itemAttributesForm.Close()
        _itemAttributesForm.Dispose()
    End Sub

    ''' <summary>
    ''' Open the POSItemInfo form to allow the user to manage the POS data for the item.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_POSSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_POSSettings.Click
        posForm = New POSItemInfo
        posForm.ItemIdentifier = Me.txtField(iItemIdentifier).Text
        posForm.POSItemBO = _posItemData
        posForm.ShowDialog(Me)
        posForm.Close()
        posForm.Dispose()
    End Sub

    ''' <summary>
    ''' Open the ItemOverride form to allow the user to manage the store jurisdiction override data for the item.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdJurisdiction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdJurisdiction.Click
        ' Save changes before proceeding to ItemOverride.  This keeps the local business objects in sync with any db updates.  The 
        ' SaveData() method has its own confirmation prompt, just in case the user doesn't want to save.
        If pbDataChanged Then
            SaveData()
        End If

        ' The state of pbDataChanged needs to be saved before ItemOverride is displayed.
        Dim itemDataChanged As Boolean = pbDataChanged

        itemOverrideForm = New ItemOverride

        ' Set the default item properties in ItemOverride.  These allow the user to populate the ItemOverride attributes with the 
        ' item and POS information that already exists in Item.
        itemOverrideForm.DefaultItemData = _itemData
        itemOverrideForm.DefaultPOSData = _posItemData
        itemOverrideForm.ItemIdentifier = Me.txtField(iItemIdentifier).Text

        itemOverrideForm.ShowDialog(Me)
        itemOverrideForm.Close()
        itemOverrideForm.Dispose()

        ' After returning from ItemOverride, refresh the Brand ComboBox.  This will keep the Item form current if the user added
        ' a new Brand in ItemOverride.  This will trigger pbDataChanged, which is not a desired side-effect.  Restore the value
        ' of pbDataChanged as it was before ItemOverride was opened.
        Dim brandIndex As Integer = cmbBrand.SelectedIndex

        mdtBrand = GetBrandData()
        cmbBrand.DataSource = mdtBrand
        cmbBrand.DisplayMember = "Brand_Name"
        cmbBrand.ValueMember = "Brand_ID"
        cmbBrand.SelectedIndex = brandIndex

        pbDataChanged = itemDataChanged
    End Sub

    Private Function EditErrorDisplay(ByRef bMustSave As Boolean, ByRef sFieldCaption As String, ByRef ctlControl As System.Windows.Forms.Control) As Boolean
        Dim bSaveData As Boolean
        Dim sMsg As String = String.Empty

        If bMustSave Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), sFieldCaption)
            MsgBox(sMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
            ctlControl.Focus()
            bSaveData = False
        Else
            sMsg = String.Format(ResourcesItemHosting.GetString("ChangesNotMade"), Chr(13), sFieldCaption)
            If MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                ctlControl.Focus()
                bSaveData = False
            Else
                SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                bSaveData = True
            End If
        End If

        EditErrorDisplay = bSaveData
    End Function

    Private Function NotifyOfInvalidCharacters(ByRef sFieldCaption As String, ByRef ctlControl As System.Windows.Forms.Control, ByRef sInvalidCharacters As String) As Boolean
        Dim validationErrorMsg As String = String.Format(ResourcesIRMA.GetString("InvalidCharacters"), sFieldCaption, sInvalidCharacters)
        MsgBox(validationErrorMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
        ctlControl.Focus()
        Return False
    End Function

    Function SaveData() As Boolean
        Dim iLoop As Short
        Dim bMustSave As Boolean
        Dim sSaveSQL As String
        Dim useTaxClassID As Integer = cmbTaxClass.SelectedValue
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim currentStatus As ItemStatus
        Dim savePromptNeeded As Boolean = True
        Dim selectedSubTeamNo As Integer

        SaveData = True

        If glItemID = -1 Or lblReadOnly.Visible Then Exit Function

        Try
            If pbDeleted Then
                SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Exit Function
            End If

            ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
            ' We check here if the rollback batches flag is set, meaning one or more batch-sensitive fields have been updated.
            ' This process was split from its original version so we get the batch list here and actually do the updates, 
            ' if needed, at the end of this (savedata) function.
            If mbRollbackBatches Then
                ' We first check to see if there are any pending batches for the item being edited.
                ' A return value of TRUE means one or more batches were found, so we need to confirm the item and batch(es) modification with the user.
                If ItemDAO.GetBatchesInSentState(plItem_Key, sBatchId_Update, sBatchInfo_Update, sBatchId_Rollback, sBatchInfo_Rollback) Then
                    ' Before actually touching batches, we prompt user to confirm they want to proceed, saving the changes and rolling-back the batches.
                    ' We also set a flag to avoid prompting user a 2nd time to save.
                    savePromptNeeded = False
                    ' A return value of TRUE means the user wants to save changes and rollback batches.
                    ' Otherwise, we refresh the form and exit this save process.
                    If sBatchInfo_Update.Length > 0 Then
                        If Not ItemBO.UserConfirmedSaveChangeAndModifyBatches(txtField(iItemIdentifier).Text, sBatchInfo_Update, BatchModificationType.Update) Then
                            RefreshDataSource(plItem_Key)
                            Return False
                        End If
                    End If
                    If sBatchInfo_Rollback.Length > 0 Then
                        If Not ItemBO.UserConfirmedSaveChangeAndModifyBatches(txtField(iItemIdentifier).Text, sBatchInfo_Rollback, BatchModificationType.Rollback) Then
                            RefreshDataSource(plItem_Key)
                            Return False
                        End If
                    End If
                End If
            End If

            If pbDataChanged Then
                ' Update the ItemBO values for the default jurisdiction
                _itemData.POSDescription = Trim(txtField(iItemPOS_Description).Text)
                _itemData.ItemDescription = Trim(txtField(iItemItem_Description).Text)
                _itemData.SignCaption = Trim(txtField(iItemSign_Description).Text)
                _itemData.PackageDesc1 = txtField(iItemPackage_Desc1).Text
                _itemData.PackageDesc2 = txtField(iItemPackage_Desc2).Text
                If cmbField(iItemPackage_Unit_ID).SelectedIndex = -1 Then
                    _itemData.PackageUnitID = Nothing
                Else
                    _itemData.PackageUnitID = VB6.GetItemData(cmbField(iItemPackage_Unit_ID), cmbField(iItemPackage_Unit_ID).SelectedIndex)
                End If
                If cmbField(iItemDistribution_Unit_ID).SelectedIndex = -1 Then
                    _itemData.DistributionUnitID = Nothing
                Else
                    _itemData.DistributionUnitID = VB6.GetItemData(cmbField(iItemDistribution_Unit_ID), cmbField(iItemDistribution_Unit_ID).SelectedIndex)
                End If
                If cmbField(iItemVendor_Unit_ID).SelectedIndex = -1 Then
                    _itemData.VendorUnitID = Nothing
                Else
                    _itemData.VendorUnitID = VB6.GetItemData(cmbField(iItemVendor_Unit_ID), cmbField(iItemVendor_Unit_ID).SelectedIndex)
                End If
                If cmbField(iItemManufacturing_Unit_ID).SelectedIndex = -1 Then
                    _itemData.ManufacturingUnitID = Nothing
                Else
                    _itemData.ManufacturingUnitID = VB6.GetItemData(cmbField(iItemManufacturing_Unit_ID), cmbField(iItemManufacturing_Unit_ID).SelectedIndex)
                End If
                If cmbField(iItemRetail_Unit_ID).SelectedIndex = -1 Then
                    _itemData.RetailUnitID = Nothing
                Else
                    _itemData.RetailUnitID = VB6.GetItemData(cmbField(iItemRetail_Unit_ID), cmbField(iItemRetail_Unit_ID).SelectedIndex)
                End If

                'validate if average unit weight field is populated when a costed-by-weight item is sold as eash in retail
                _itemData.AverageUnitWeight = IIf(txtField(iItemAverage_Unit_Weight).Text = "", 0.0, txtField(iItemAverage_Unit_Weight).Text)
                _itemData.CostedByWeight = (chkField(iItemCostedByWeight).CheckState = Windows.Forms.CheckState.Checked)
                Dim isWeightedRetailUnit As Boolean = False
                isWeightedRetailUnit = ItemUnitDAO.IsWeightUnit(CInt(_itemData.RetailUnitID))
                If (_itemData.CostedByWeight = True And Not isWeightedRetailUnit And _itemData.AverageUnitWeight <= 0.0) Then
                    SaveData = EditErrorDisplay(bMustSave, lblAvgUnitWeight.Text.Replace(":", ""), txtField(iItemAverage_Unit_Weight))
                    Exit Function
                End If

                ' Validate the data stored in the business object and display and related error messages
                Dim packageDesc1Required As Boolean = txtField(iItemPackage_Desc1).BackColor = COLOR_ACTIVE
                Dim packageDesc2Required As Boolean = txtField(iItemPackage_Desc2).BackColor = COLOR_ACTIVE

                statusList = _itemData.ValidateData(packageDesc1Required, packageDesc2Required)
                statusEnum = statusList.GetEnumerator

                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, ItemStatus)
                    Select Case currentStatus
                        Case ItemStatus.Error_POSDescriptionRequired
                            SaveData = EditErrorDisplay(bMustSave, lblPOSDesc.Text.Replace(":", ""), txtField(iItemPOS_Description))
                            Exit Function
                        Case ItemStatus.Error_POSDescriptionInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(lblPOSDesc.Text.Replace(":", ""), txtField(iItemPOS_Description), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_ItemDescriptionRequired
                            SaveData = EditErrorDisplay(bMustSave, lblDescription.Text.Replace(":", ""), txtField(iItemItem_Description))
                            Exit Function
                        Case ItemStatus.Error_ItemDescriptionInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(lblDescription.Text.Replace(":", ""), txtField(iItemItem_Description), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_SignCaptionRequired
                            SaveData = EditErrorDisplay(bMustSave, lblSignCaption.Text.Replace(":", ""), txtField(iItemSign_Description))
                            Exit Function
                        Case ItemStatus.Error_SignCaptionInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(lblSignCaption.Text.Replace(":", ""), txtField(iItemSign_Description), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_PackageDesc1Required
                            SaveData = EditErrorDisplay(bMustSave, ResourcesItemHosting.GetString("LabelType"), txtField(iItemPackage_Desc1))
                            Exit Function
                        Case ItemStatus.Error_PackageDesc1ValueIsZero
                            If MsgBox(String.Format(ResourcesItemHosting.GetString("PkgDescNotZero"), ResourcesItemHosting.GetString("One")), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                                txtField(iItemPackage_Desc1).Focus()
                                SaveData = False
                            Else
                                SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            End If
                            Exit Function
                        Case ItemStatus.Error_PackageDesc2Required
                            SaveData = EditErrorDisplay(bMustSave, ResourcesItemHosting.GetString("PkgDesc2"), txtField(iItemPackage_Desc2))
                            Exit Function
                        Case ItemStatus.Error_PackageDesc2ValueIsZero
                            If MsgBox(String.Format(ResourcesItemHosting.GetString("PkgDescNotZero"), ResourcesItemHosting.GetString("Two")), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                                txtField(iItemPackage_Desc2).Focus()
                                SaveData = False
                            Else
                                SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            End If
                            Exit Function
                        Case ItemStatus.Error_PackageDescUnitRequired
                            SaveData = EditErrorDisplay(bMustSave, ResourcesItemHosting.GetString("PkgDescUnit"), cmbField(iItemPackage_Unit_ID))
                            Exit Function
                        Case ItemStatus.Error_PackageDescUnitInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(ResourcesItemHosting.GetString("PkgDescUnit"), cmbField(iItemPackage_Unit_ID), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_VendorUnitRequired
                            SaveData = EditErrorDisplay(bMustSave, String.Format(ResourcesItemHosting.GetString("Unit"), lblVendorOrder.Text.Replace(":", "")), cmbField(iItemVendor_Unit_ID))
                            Exit Function
                        Case ItemStatus.Error_VendorUnitInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(String.Format(ResourcesItemHosting.GetString("Unit"), lblVendorOrder.Text.Replace(":", "")), cmbField(iItemVendor_Unit_ID), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_DistributionUnitRequired
                            SaveData = EditErrorDisplay(bMustSave, String.Format(ResourcesItemHosting.GetString("Unit"), lblDistribution.Text.Replace(":", "")), cmbField(iItemDistribution_Unit_ID))
                            Exit Function
                        Case ItemStatus.Error_DistributionUnitInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(String.Format(ResourcesItemHosting.GetString("Unit"), lblDistribution.Text.Replace(":", "")), cmbField(iItemDistribution_Unit_ID), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                        Case ItemStatus.Error_RetailUnitRequired
                            SaveData = EditErrorDisplay(bMustSave, String.Format(ResourcesItemHosting.GetString("Unit"), lblRetail.Text.Replace(":", "")), cmbField(iItemRetail_Unit_ID))
                            Exit Function
                        Case ItemStatus.Error_RetailUnitInvalidCharacters
                            SaveData = NotifyOfInvalidCharacters(String.Format(ResourcesItemHosting.GetString("Unit"), lblRetail.Text.Replace(":", "")), cmbField(iItemRetail_Unit_ID), ItemBO.INVALID_CHARACTERS)
                            Exit Function
                    End Select
                End While

                ' Validate the rest of the data that is not stored in the business object
                '-- Tax Class is required
                If cmbTaxClass.Text Is Nothing Or cmbTaxClass.Text = "" Then
                    SaveData = EditErrorDisplay(bMustSave, lblTaxClass.Text.Replace(":", ""), cmbTaxClass)
                    Exit Function
                End If

                'Look at InstanceDataFlags, figure out if LabelType is required.
                If InstanceDataDAO.IsFlagActive("Required_LabelType") AndAlso (cmbLabelType.Text Is Nothing Or cmbLabelType.Text = "") Then
                    SaveData = EditErrorDisplay(bMustSave, Label_LabelType.Text.Replace(":", ""), cmbLabelType)
                    Exit Function
                End If

                If InstanceDataDAO.IsFlagActive("Required_ManagedBy") AndAlso (cmbManagedBy.SelectedIndex = -1) Then
                    SaveData = EditErrorDisplay(bMustSave, grpManageBy.Text, cmbManagedBy)
                    Exit Function
                End If

                ' Bug #2700:    MW 4.2.5 Test: Managed By field doesn't prompt to save
                ' Updated By:   Denis Ng
                ' Date:         09/14/2011
                ' Comment:      1. New items on the Managed By list are not added through IRMA client. 
                '               2. When user enter an item in the Managed By textbox instead of selecting 
                '               an item from the list, the entry will be verified before it is saved. 
                '               3. If the entry can be located from the existing list, the changes in the form
                '               will be saved. Otherwise, an error message will be displayed.
                If Len(Trim(Me.cmbManagedBy.Text)) > 0 Then
                    Dim newValue As String = UCase(Trim(Me.cmbManagedBy.Text))
                    Dim counter As Integer
                    Dim itemFound As Boolean = False

                    For counter = 0 To mgBydt.Rows.Count - 1
                        If newValue = UCase(Trim(mgBydt.Rows(counter)("Value").ToString)) Then
                            Me.cmbManagedBy.SelectedIndex = counter
                            itemFound = True
                            Exit For
                        End If
                    Next

                    If Not itemFound Then
                        MsgBox("There is a new 'Managed By' item and it cannot be added from the client. Please select from the dropdown list.", MsgBoxStyle.Critical)
                        Exit Function
                    End If
                End If
                ' Bug #2700 End

                'only prompt if the item is a DC item
                If cmbDistSubTeam.SelectedIndex = -1 And cmbDistSubTeam.Enabled And cmbDistSubTeam.Items.Count > 0 And chkField(iItemEXEDist).Checked Then
                    SaveData = EditErrorDisplay(bMustSave, lblDistSubTeam.Text.Replace(":", ""), cmbDistSubTeam)
                    Exit Function
                End If

                If Me.HierarchySelector1.cmbCategory.SelectedIndex = -1 And Not Me.HierarchySelector1.cmbCategory.Enabled Then
                    If pbDataChanged Then
                        SaveData = EditErrorDisplay(bMustSave, Me.HierarchySelector1.lblCategory.Text.Replace(":", ""), (Me.HierarchySelector1.cmbCategory))
                    Else
                        If MsgBox(ResourcesItemHosting.GetString("CategoryRequired"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                            Me.HierarchySelector1.cmbCategory.Focus()
                            SaveData = False
                        Else
                            SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        End If
                        Exit Function
                    End If
                    Exit Function
                End If

                Select Case -1
                    Case Me.HierarchySelector1.cmbSubTeam.SelectedIndex
                        SaveData = EditErrorDisplay(bMustSave, Me.HierarchySelector1.lblSubTeam.Text.Replace(":", ""), Me.HierarchySelector1.cmbSubTeam)
                        Exit Function
                    Case Me.HierarchySelector1.cmbCategory.SelectedIndex
                        SaveData = EditErrorDisplay(bMustSave, Me.HierarchySelector1.lblCategory.Text.Replace(":", ""), Me.HierarchySelector1.cmbCategory)
                        Exit Function
                    Case cmbBrand.SelectedIndex
                        SaveData = EditErrorDisplay(bMustSave, lblBrand.Text.Replace(":", ""), cmbBrand)
                        Exit Function
                End Select

                If InstanceDataDAO.IsFlagActive("FourLevelHierarchy") Then
                    Select Case -1
                        Case Me.HierarchySelector1.cmbLevel3.SelectedIndex
                            SaveData = EditErrorDisplay(bMustSave, Me.HierarchySelector1.lblLevel3.Text.Replace(":", ""), Me.HierarchySelector1.cmbLevel3)
                            Exit Function
                        Case Me.HierarchySelector1.cmbLevel4.SelectedIndex
                            SaveData = EditErrorDisplay(bMustSave, Me.HierarchySelector1.lblLevel4.Text.Replace(":", ""), Me.HierarchySelector1.cmbLevel4)
                            Exit Function
                    End Select
                End If

                If chkField(iItemRetail_Sale).Checked = True And cmbTaxClass.SelectedIndex = -1 Then
                    MsgBox(ResourcesItemHosting.GetString("TaxClassRequired"), MsgBoxStyle.Exclamation, Me.Text)
                    cmbTaxClass.Focus()
                    SaveData = False
                    Exit Function
                End If

                If pbDataChanged And (Not bMustSave) Then
                    If savePromptNeeded And PSaveClick = False Then
                        If MsgBox(ResourcesIRMA.GetString("SaveChanges"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                            SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            Exit Function
                        End If
                    End If
                End If

                '-- Make sure there is data so it don't crash on nulls
                For iLoop = 4 To 15
                    'not looking at fields 7, 15, 16, 17 because they were removed
                    If iLoop <> 10 AndAlso iLoop <> 7 AndAlso iLoop <> 15 AndAlso iLoop <> 16 AndAlso iLoop <> 17 Then
                        If txtField(iLoop).Text = "" Then txtField(iLoop).Text = CStr(0)
                    End If
                Next iLoop
            End If

            'check for any tax overrides on this item;  if any exist then notify user that they will be removed if this change is saved
            If pbTaxClassChanged Then
                Dim taxDAO As New TaxOverrideDAO
                Dim overrideList As ArrayList = taxDAO.GetTaxOverrideList(plItem_Key)

                If overrideList.Count > 0 Then
                    'notify user
                    Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_taxOverridesWillBeDeleted"), Me.lblTaxClass.Text.Replace(":", "")), Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

                    If result = Windows.Forms.DialogResult.OK Then
                        'delete tax overrides
                        taxDAO.DeleteDataForItemKey(plItem_Key)

                        'save changes to tax class
                        useTaxClassID = cmbTaxClass.SelectedValue
                    Else
                        useTaxClassID = mOrigTaxClassID
                    End If
                End If
            End If

            selectedSubTeamNo = ComboValue((Me.HierarchySelector1.cmbSubTeam))

            If selectedSubTeamNo <> plSubTeam_No Then
                If (chkField(iItemRetail_Sale).Checked And SubTeamDAO.IsSubTeamAligned(selectedSubTeamNo)) Then
                    SaveData = False
                    MsgBox("Cannot select aligned subteam. Please select an out-of-scope subteam.", MsgBoxStyle.Exclamation, Me.Text)
                    Me.HierarchySelector1.SelectedSubTeamId = plSubTeam_No
                    Me.HierarchySelector1.SelectedCategoryId = plCategory_Id
                    Exit Function
                End If
            End If

            '-- Everything is ok
            If pbDataChanged Then
                sSaveSQL = "EXEC UpdateItemInfo " & plItem_Key & ", '" &
                                                                Replace(ConvertQuotes(_itemData.POSDescription), ",", "", , , CompareMethod.Text) & "', '" &
                                                                ConvertQuotes(_itemData.ItemDescription) & "', '" &
                                                                ConvertQuotes(_itemData.SignCaption) & "', " &
                                                                txtField(iItemMin_Temperature).Text & ", " &
                                                                txtField(iItemMax_Temperature).Text & ", " &
                                                                txtField(iItemAverage_Unit_Weight).Text & ", " &
                                                                _itemData.PackageDesc1 & ", " &
                                                                _itemData.PackageDesc2 & ", " &
                                                                IIf(_itemData.PackageUnitID Is Nothing, "null", _itemData.PackageUnitID) & ", " &
                                                                IIf(_itemData.RetailUnitID Is Nothing, "null", _itemData.RetailUnitID) & ", " &
                                                                ComboValue((Me.HierarchySelector1.cmbSubTeam)) & ", " &
                                                                IIf(cmbBrand.Text = "", "null", cmbBrand.SelectedValue) & ", " &
                                                                ComboValue((Me.HierarchySelector1.cmbCategory)) & ", " &
                                                                ComboValue(cmbField(iItemOrigin_ID)) & ", " &
                                                                chkField(iItemRetail_Sale).CheckState & ", " &
                                                                chkField(iItemKeep_Frozen).CheckState & ", " &
                                                                chkField(iItemFull_Pallet_Only).CheckState & ", " &
                                                                chkField(iItemShipper_Item).CheckState & ", " &
                                                                chkField(iItemWFM_Item).CheckState & ", " &
                                                                txtField(iItemUnits_Per_Pallet).Text & ", " &
                                                                IIf(_itemData.VendorUnitID Is Nothing, "null", _itemData.VendorUnitID) & ", " &
                                                                IIf(_itemData.DistributionUnitID Is Nothing, "null", _itemData.DistributionUnitID) & ", " &
                                                                txtField(iItemTie).Text & ", " &
                                                                txtField(iItemHigh).Text & ", " &
                                                                txtField(iItemYield).Text & ", " &
                                                                chkField(iNoDistMarkup).CheckState & ", " &
                                                                chkField(iItemOrganic).CheckState & ", " &
                                                                chkField(iItemRefrigerated).CheckState & ", " &
                                                                chkField(iItemNot_Available).CheckState & ", " &
                                                                chkField(iItemPre_Order).CheckState & ", " &
                                                                ComboValue(cmbField(iitemItemType_ID)) & ", " &
                                                                TextValue(txtField(iItemSales_Account).Text) & ", " &
                                                                chkField(iItemHFM).CheckState & ", " &
                                                                TextValue("'" & ConvertQuotes(txtField(iItemNotAvailNote).Text) & "'") & ", " &
                                                                ComboValue(cmbField(iItemCountryProc_ID)) & ", " &
                                                                IIf(_itemData.ManufacturingUnitID Is Nothing, "null", _itemData.ManufacturingUnitID) & ", " &
                                                                chkField(iItemEXEDist).CheckState & ", " &
                                                                IIf(ComboValue(cmbField(iItemNatClassID)) Is Nothing, "99999", ComboValue(cmbField(iItemNatClassID))) & ", " &
                                                                ComboEnabledValue(cmbDistSubTeam) & ", " &
                                                                chkField(iItemCostedByWeight).CheckState & ", " &
                                                                IIf(cmbTaxClass.Text = "", "null", useTaxClassID) & ", " &
                                                                IIf(cmbLabelType.Text = "", "null", cmbLabelType.SelectedValue) & ", " &
                                                                mUser_ID & ", '" &
                                                                msUser_ID_Date & "', " &
                                                                IIf(cmbManagedBy.Text = "", "null", cmbManagedBy.SelectedValue) & ", " &
                                                                chkField(iItemRecallFlag).CheckState & ", " &
                                                                ComboValue(Me.HierarchySelector1.cmbLevel4) & ", " &
                                                                chkField(iItemLockAuthFlag).CheckState & ", " &
                                                                chkField(iItemPurchaseThresholdCouponSubTeam).CheckState & ", " &
                                                                txtField(iItemPurchaseThresholdCouponAmount).Text & ", " &
                                                                IIf(txtField(iItemHandlingChargeOverride).Text = "", "null", txtField(iItemHandlingChargeOverride).Text) & ", " &
                                                                chkField(iItemCatchweightRequired).CheckState & "" & ", " &
                                                                chkField(iItemCOOL).CheckState & ", " &
                                                                chkField(iItemBIO).CheckState & ", " &
                                                                chkField(iItemIngredient).CheckState & ", " &
                                                                IIf(chkSustainRankingRequired.Checked, "1", "0") & "," &
                                                                IIf(cmbSustainRankingDefault.SelectedIndex <= 0, "null", cmbSustainRankingDefault.SelectedIndex.ToString()) & ", " &
                                                                chkUseLastReceivedCost.CheckState & ", " &
                                                                CheckBoxGiftCard.CheckState & "," &
                                                                chkField(iItemNot_Available_365).CheckState

                Try
                    SQLExecute(sSaveSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    MsgBox("Item changes have been saved.", MsgBoxStyle.Information, Me.Text)

                    ' This is suspect because we may be unlocking the item when opening subscreens that may update the item record...?
                    SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    ' Reload the local Item object so that all database changes are relected on the interface.
                    RefreshDataSource(glItemID)
                Catch ex As Exception
                    ErrorDialog.HandleError(ex, ErrorDialog.NotificationTypes.DialogAndEmail)
                End Try

                pbDataChanged = False
                pbTaxClassChanged = False

                If useTaxClassID <> mOrigTaxClassID Then
                    Call CheckTaxClassChange()
                End If

                ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
                ' If we are here and the rollback flag is TRUE and one or more pending batches exist for the item being edited,
                ' the user was prompted and confirmed they want to save their changes and modify the batches.
                If mbRollbackBatches Then ItemBO.UpdateOrRollbackBatches(txtField(iItemIdentifier).Text, sBatchId_Update, sBatchInfo_Update, sBatchId_Rollback, sBatchInfo_Rollback)

            ElseIf Not lblReadOnly.Visible Then
                SQLExecute("EXEC UnlockItem " & plItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If
        Catch ex As Exception
            logger.Error(ex.Message)
            ErrorDialog.HandleError(ex, ErrorDialog.NotificationTypes.DialogAndEmail)
        End Try
    End Function

    Private Sub frmItem_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Info("Closing item screen...")

        ' originally was glItemId > -1, but this caused a vendor Item warning upon close, even if no item was loaded or changed.
        ' to fix, changing to glItemId > 0 
        If glItemID > 0 Then
            e.Cancel = Not SaveData()

            If Not e.Cancel Then
                If chkField(iItemShipper_Item).Enabled And chkField(iItemShipper_Item).CheckState And (txtField(iItemPackage_Desc1).Text = Shipper.SHIPPER_INITIAL_PACK_QTY) Then
                    e.Cancel = True
                    MessageBox.Show(ShipperMessages.ERROR_SHIPPER_CANNOT_SAVE_IF_EMPTY, ShipperMessages.CAPTION_SHIPPER_SAVE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If

                'IF REGIONAL INSTANCE DATA FLAG REQUIRES THAT VENDORS BE REQUIRED TO BATCH ITEMS, INFORM
                'USER IF THIS ITEM DOES NOT HAVE A PRIMARY VENDOR SETUP
                If InstanceDataDAO.IsFlagActive("Required_VendorForBatching") Then
                    'determine if this item has a primary vendor setup for all stores
                    Dim storesWithNoVendor As ArrayList = VendorDAO.GetStoresWithNoVendorForItem(glItemID)

                    If storesWithNoVendor.Count > 0 Then
                        'build error message
                        Dim storeNames As New StringBuilder
                        Dim currentStore As Integer

                        For currentStore = 0 To storesWithNoVendor.Count - 1
                            storeNames.Append(Environment.NewLine)
                            storeNames.Append("     * ")
                            storeNames.Append(storesWithNoVendor(currentStore))
                        Next

                        Dim message As New StringBuilder
                        message.Append(ResourcesItemHosting.GetString("msg_warning_ItemWithNoVendor_part1"))
                        message.Append(Environment.NewLine)
                        message.Append(storeNames.ToString)
                        message.Append(Environment.NewLine)
                        message.Append(Environment.NewLine)
                        message.Append(ResourcesItemHosting.GetString("msg_warning_ItemWithNoVendor_part2"))
                        message.Append(Environment.NewLine)
                        message.Append(Environment.NewLine)
                        message.Append(ResourcesItemHosting.GetString("msg_warning_ItemWithNoVendor_part3"))

                        Dim result As DialogResult = MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If result = Windows.Forms.DialogResult.Yes Then
                            'do not close form
                            e.Cancel = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub CheckTaxClassChange()
        '--------------------------------------------------------------------------------------------
        'Purpose:   For UK only -  If this item is not new and the tax class has changed,
        '                create a price batch detail record and recalculate the POS Price.
        '--------------------------------------------------------------------------------------------
        Dim rsStores As DAO.Recordset = Nothing
        Dim price As Double
        Dim salePrice As Double

        If InstanceDataDAO.IsFlagActive("UseVAT") Then

            'If the tax class has changed, recalc the price and posprice.
            If cmbTaxClass.SelectedValue <> mOrigTaxClassID Then

                Try
                    '------------------------------------------------------------------------------------------------------------
                    'Get a list of all stores for this item with the current POSPrice, POSSale_Price and tax rates.
                    '------------------------------------------------------------------------------------------------------------
                    SQLOpenRS(rsStores, "EXEC GetRetailStoresPOSPriceTax " & plItem_Key, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    While Not rsStores.EOF
                        '------------------------------------------------------------------------------------------------------------------------
                        'Recalc and update the current "Price" and "Sale_Price"  (Not the POS ones) in the current price record.
                        '------------------------------------------------------------------------------------------------------------------------
                        price = GetPriceWithoutVAT(rsStores.Fields("POSPrice").Value, rsStores.Fields("TaxRate").Value)
                        salePrice = GetPriceWithoutVAT(rsStores.Fields("POSSale_Price").Value, rsStores.Fields("TaxRate").Value)

                        If rsStores.Fields("POSPrice").Value > 0 Or rsStores.Fields("POSSale_Price").Value > 0 Then
                            SQLExecute("EXEC UpdatePriceTaxChange " & plItem_Key & "," & rsStores.Fields("Store_No").Value & "," & price & "," & salePrice, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        End If

                        rsStores.MoveNext()
                    End While
                Finally
                    If rsStores IsNot Nothing Then
                        rsStores.Close()
                        rsStores = Nothing
                    End If
                End Try
            End If
        End If
    End Sub

    Private Sub frmItem_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        If Me.isInitializing Then Exit Sub

        Dim Index As Short = txtField.GetIndex(eventSender)

        Select Case Index
            Case 14
                'Yield txtbox.
                If Trim$(txtField(14).Text) = String.Empty Then
                    txtField(14).Text = String.Empty
                Else
                    txtField(14).Text = Format(Convert.ToDecimal(txtField(14).Text), ResourcesIRMA.GetString("NumberFormatSmallInteger"))
                End If
        End Select

        pbDataChanged = True
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    Dim txtBox As TextBox = CType(eventSender, TextBox)
    If Not txtBox.ReadOnly Then txtBox.SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)

    If Index = iItemTie Or Index = iItemHigh Or Index = iItemMin_Temperature Or Index = iItemMax_Temperature Or
           Index = iItemUnits_Per_Pallet Or Index = iItemYield Or Index = iItemAverage_Unit_Weight Or Index = iItemPurchaseThresholdCouponAmount Or
           Index = iItemHandlingChargeOverride Or Index = iItemNotAvailNote Or Index = iItemSign_Description Then
      mbRollbackBatches = mbRollbackBatches Or False
    Else
      mbRollbackBatches = True
    End If

    If Not txtField(Index).ReadOnly Then

      '-- Restrict key presses to that type of field
      KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

      If iItemPOS_Description = Index Then KeyAscii = Asc(UCase(Chr(KeyAscii)))
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub chkField_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkField.CheckStateChanged
        If Me.isInitializing Then Exit Sub

        Dim Index As Short = chkField.GetIndex(eventSender)

        logger.InfoFormat("Item screen checkbox '{0}' clicked...", chkField(Index).Name)

        If bLoading Then Exit Sub

        bLoading = True

        pbDataChanged = True

        If Index = iItemNot_Available Or Index = iItemCatchweightRequired Or Index = iItemCOOL Or
           Index = iItemBIO Or Index = iItemFull_Pallet_Only Or Index = iItemKeep_Frozen Or Index = iNoDistMarkup Or Index = iItemOrganic Or
           Index = iItemRecallFlag Or Index = iItemPre_Order Or Index = iItemRefrigerated Or Index = iItemLockAuthFlag Or Index = iItemRetail_Sale Or
           Index = iItemShipper_Item Or Index = iItemWFM_Item Or Index = iItemHFM Then
            mbRollbackBatches = mbRollbackBatches Or False
        Else
            mbRollbackBatches = True
        End If

        If Index = iItemShipper_Item Then
            If chkField(Index).CheckState = 1 Then
                ' User is marking this item as a Shipper.
                logger.Info("Shipper checkbox checked...")

                ' Perform validation.
                Dim shipperOK As Boolean = False
                Try
                    Shipper.ValidateConvertItemToShipper(plItem_Key)
                    shipperOK = True
                Catch ex As Exception
                    MessageBox.Show(ex.Message, ShipperMessages.CAPTION_SHIPPER_CONVERT_FROM_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                If shipperOK Then
                    ' We're clear to mark this item as a Shipper, but we need to confirm pack info reset with user.
                    If MessageBox.Show(ShipperMessages.CONFIRM_SHIPPER_RESET_PACK_INFO, ShipperMessages.CAPTION_SHIPPER_RESET_PACK_INFO, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        txtField(iItemPackage_Desc1).Text = Shipper.SHIPPER_INITIAL_PACK_QTY
                        txtField(iItemPackage_Desc2).Text = Shipper.SHIPPER_INITIAL_PACK_SIZE
                        ' Get the UOM ID for a "UNIT".
                        glUnitID = ReturnUnitID("Unit")
                        ' Update pack and vendor UOM combos.
                        VB6.SetItemData(cmbField(iItemPackage_Unit_ID), cmbField(iItemPackage_Unit_ID).SelectedIndex, glUnitID)
                        VB6.SetItemData(cmbField(iItemVendor_Unit_ID), cmbField(iItemVendor_Unit_ID).SelectedIndex, giShipper)
                    Else
                        ' User does not want to change pack info, so uncheck Shipper box.
                        shipperOK = False
                    End If
                End If

                ' Finally, make sure the Shipper checkbox on this form reflects the results of the convert-item-to-Shipper results.
                If Not shipperOK Then
                    chkField(Index).CheckState = System.Windows.Forms.CheckState.Unchecked
                End If

                ' Note: There is no Shipper object yet.  The Shipper is created during RefreshDataSource, so when the user
                ' opens a subscreen from this item screen, changes will be saved and the Shipper object created.
            Else
                ' User is removing the Shipper flag from this item.
                logger.Info("Shipper checkbox unchecked...")

                ' Perform validation.
                Try
                    Shipper.ValidateConvertShipperToItem(_itemData.Shipper)
                Catch ex As Exception
                    ' Shipper flag cannot be removed because the Shipper contains item.
                    MessageBox.Show(ex.Message, ShipperMessages.CAPTION_SHIPPER_CONVERT_TO_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    chkField(Index).CheckState = System.Windows.Forms.CheckState.Checked
                End Try
            End If

            '-- Allow them to change package description if not a shipper
            SetActive(cmdShipper, (chkField(iItemShipper_Item).CheckState = 1))
            SetActive(txtField(iItemPackage_Desc1), (chkField(iItemShipper_Item).CheckState = 0))
            SetActive(txtField(iItemPackage_Desc2), (chkField(iItemShipper_Item).CheckState = 0))
            SetActive(cmbField(iItemPackage_Unit_ID), (chkField(iItemShipper_Item).CheckState = 0))
            SetActive(cmbField(iItemVendor_Unit_ID), (chkField(iItemShipper_Item).CheckState = 0))
        End If

        If Index = iItemNot_Available Or iItemNot_Available_365 Then
            If chkField(iItemNot_Available).CheckState = System.Windows.Forms.CheckState.Unchecked And
                chkField(iItemNot_Available_365).CheckState = System.Windows.Forms.CheckState.Unchecked Then
                txtField(iItemNotAvailNote).Text = String.Empty
            End If
            SetActive(txtField(iItemNotAvailNote), (chkField(iItemNot_Available).CheckState = System.Windows.Forms.CheckState.Checked) Or chkField(iItemNot_Available_365).CheckState = System.Windows.Forms.CheckState.Checked)
        End If

        Dim currValue As Integer
        If Index = iItemCostedByWeight Then
            If cmbField(iItemPackage_Unit_ID).SelectedIndex > -1 Then
                currValue = VB6.GetItemData(cmbField(iItemPackage_Unit_ID), cmbField(iItemPackage_Unit_ID).SelectedIndex)
            End If

            LoadPackageDescriptionUnit(cmbField(iItemPackage_Unit_ID), False, True)

            If currValue > 0 Then
                cmbField(iItemPackage_Unit_ID).SelectedIndex = -1
                SetCombo(cmbField(iItemPackage_Unit_ID), currValue)
            End If

            currValue = 0
            If cmbField(iItemRetail_Unit_ID).SelectedIndex > -1 Then
                currValue = VB6.GetItemData(cmbField(iItemRetail_Unit_ID), cmbField(iItemRetail_Unit_ID).SelectedIndex)
            End If

            LoadItemUnitsCost(cmbField(iItemRetail_Unit_ID), False, True)

            If currValue > 0 Then
                SetCombo(cmbField(iItemRetail_Unit_ID), currValue)
            End If

            currValue = 0
            If cmbField(iItemVendor_Unit_ID).SelectedIndex > -1 Then
                currValue = VB6.GetItemData(cmbField(iItemVendor_Unit_ID), cmbField(iItemVendor_Unit_ID).SelectedIndex)
            End If

            LoadItemUnitsVendor(cmbField(iItemVendor_Unit_ID), (chkField(iItemCostedByWeight).CheckState = System.Windows.Forms.CheckState.Checked))

            If currValue > 0 Then
                SetCombo(cmbField(iItemVendor_Unit_ID), currValue)
            End If

            currValue = 0
            If cmbField(iItemDistribution_Unit_ID).SelectedIndex > -1 Then
                currValue = VB6.GetItemData(cmbField(iItemDistribution_Unit_ID), cmbField(iItemDistribution_Unit_ID).SelectedIndex)
            End If

            LoadItemUnitsVendor(cmbField(iItemDistribution_Unit_ID), False, True)

            If currValue > 0 Then
                SetCombo(cmbField(iItemDistribution_Unit_ID), currValue)
            End If

            currValue = 0
            If cmbField(iItemManufacturing_Unit_ID).SelectedIndex > -1 Then
                currValue = VB6.GetItemData(cmbField(iItemManufacturing_Unit_ID), cmbField(iItemManufacturing_Unit_ID).SelectedIndex)
            End If

            LoadItemUnitsVendor(cmbField(iItemManufacturing_Unit_ID), False, True)

            If currValue > 0 Then
                SetCombo(cmbField(iItemManufacturing_Unit_ID), currValue)
            End If
        End If

        If Index = iItemEXEDist And chkField(Index).CheckState = System.Windows.Forms.CheckState.Unchecked Then
            Try
                gRSRecordset = SQLOpenRecordSet("EXEC GetDCOnHand " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If gRSRecordset.Fields("Quantity").Value > 0 Or gRSRecordset.Fields("Weight").Value > 0 Then
                    MsgBox(ResourcesItemHosting.GetString("DCOnHand"), MsgBoxStyle.Exclamation, Me.Text)
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                    chkField(Index).CheckState = System.Windows.Forms.CheckState.Checked
                End If
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try

            Try
                gRSRecordset = SQLOpenRecordSet("EXEC CheckIfItemInOpenOrder " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If gRSRecordset.Fields("OpenOrderCount").Value > 0 Then
                    MsgBox(ResourcesItemHosting.GetString("OpenOrder"), MsgBoxStyle.Exclamation, Me.Text)
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                    chkField(Index).CheckState = System.Windows.Forms.CheckState.Checked
                End If
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                End If
            End Try
        End If

        bLoading = False
    End Sub

    Private Sub cmbDistSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDistSubTeam.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbDistSubTeam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmbDistSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbDistSubTeam.SelectedIndexChanged
        If Me.isInitializing Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged
        If Me.isInitializing Then Exit Sub

        Dim Index As Short = cmbField.GetIndex(eventSender)

        If bLoading Then Exit Sub

        If Index = iItemManagedBy Or Index = iItemDistribution_Unit_ID Or Index = iItemNatClassID Then
            mbRollbackBatches = mbRollbackBatches Or False
        Else
            mbRollbackBatches = True
        End If

        pbDataChanged = True

        If Index = iItemNatClassID Then
            If cmbField(Index).SelectedIndex <> -1 Then ToolTip1.SetToolTip(cmbField(Index), cmbField(Index).Text)
        End If
    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 8 Then
            Select Case Index
                Case iItemOrigin_ID, iItemCountryProc_ID, iItemVendor_Unit_ID, iItemDistribution_Unit_ID, iItemNatClassID, iItemManufacturing_Unit_ID
                    cmbField(Index).SelectedIndex = -1
            End Select
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdAddCat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call AddCategory(Me.HierarchySelector1.cmbCategory, VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex))
    End Sub

    Private Sub cmbBrand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbBrand.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub cmbBrand_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBrand.SelectedIndexChanged
        If Me.isInitializing Or bLoading Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub cmbTaxClass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTaxClass.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then cmbTaxClass.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub cmbTaxClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTaxClass.SelectedIndexChanged
        If Me.isInitializing Or bLoading Then Exit Sub

        pbDataChanged = True
        pbTaxClassChanged = True
    End Sub

    Private Sub cmbLabelType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbLabelType.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then cmbLabelType.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub cmbLabelType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLabelType.SelectedIndexChanged
        If Me.isInitializing Or bLoading Then Exit Sub

        pbDataChanged = True
    End Sub

    Private Sub HideFields()
        grpManageBy.Visible = InstanceDataDAO.IsFlagActive("ShowManagedBy")
    End Sub

    Private Sub SetGpmFieldsAvailability(ByVal pricesAreGloballyManaged As Boolean, ByVal isProduceSubteam As Boolean, ByVal isValidated As Boolean)
        ' only allow users to edit default retail size and default UOM for an item if the item is either not validated, or if the item is validated, and no stores from the 
        ' region is on GPM, and the item is from Produce subteam.
        _txtField_Pack.Enabled = Not pricesAreGloballyManaged
        _txtField_RetailPackSize.Enabled = Not isValidated Or (isValidated And isProduceSubteam And Not pricesAreGloballyManaged)
        _cmbField_RetailPackUOM.Enabled = Not isValidated Or (isValidated And isProduceSubteam And Not pricesAreGloballyManaged)
    End Sub

    Private Sub RefreshDataSource(ByRef lRecord As Integer)
        logger.Info("Refreshing item screen...")

        Dim iLoop As Short
        Dim lUser_ID As Integer
        Dim sUserName As String
        Dim lItemSubTeam_No As Integer
        Dim bIsEXEDistributed As Boolean
        Dim bIsSubTeamEXEDistributed As Boolean
        Dim bDisableBrandAdditions As Boolean = ConfigurationServices.AppSettings("DisableBrandAdditions")
        Dim bIsIngredientItem As Boolean
        Dim isProduceSubteam As Boolean = False

        sUserName = String.Empty

        mbRollbackBatches = False

        Try
            If lRecord = -1 Then
                plItem_Key = -1
            Else
                SQLOpenRS(rsItem, "EXEC GetItemInfo " & lRecord & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsItem.EOF Then
                    plItem_Key = -1

                    ' we've got to close this before we open the next result set!
                    rsItem.Close()
                    rsItem = Nothing
                Else
                    plItem_Key = rsItem.Fields("Item_Key").Value
                    ' the resultset is closed below, after data is loaded from it
                End If
            End If

            bLoading = True

            If plItem_Key = -1 Then
                pbPrimary_Vendor = False
                m_bIsScaleItem = False
                plSubTeam_No = 0
                pbDeleted = False
                pbPrimary_Vendor = False

                lblReadOnly.Text = String.Empty
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = False
            Else
                ' Create the ItemBO and populate it with the values for the default jurisdiction
                Try
                    _itemData = New ItemBO(rsItem)
                Catch ex As Exception
                    ErrorDialog.HandleError("The item screen cannot be refreshed.", ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
                    Exit Sub
                End Try

                _itemData.Item_Key = plItem_Key

                If _itemData IsNot Nothing And _itemData.Shipper IsNot Nothing Then
                    logger.Debug(_itemData.Shipper.ToString)
                End If

                '-- Do Text fields
                txtField(iItemPOS_Description).Text = _itemData.POSDescription
                txtField(iItemItem_Description).Text = _itemData.ItemDescription
                txtField(iItemIdentifier).Text = rsItem.Fields("Identifier").Value
                TextBox_DefaultJurisdiction.Text = _itemData.StoreJurisdictionDesc
                txtField(iItemMin_Temperature).Text = rsItem.Fields("Min_Temperature").Value & ""
                txtField(iItemMax_Temperature).Text = rsItem.Fields("Max_Temperature").Value & ""
                txtField(iItemPackage_Desc1).Text = Format(_itemData.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger"))
                txtField(iItemPackage_Desc2).Text = Format(_itemData.PackageDesc2, ResourcesIRMA.GetString("NumberFormatDecimal3"))
                txtField(iItemAverage_Unit_Weight).Text = rsItem.Fields("Average_Unit_Weight").Value & ""
                txtField(iItemUnits_Per_Pallet).Text = rsItem.Fields("Units_Per_Pallet").Value & ""
                txtField(iItemSign_Description).Text = _itemData.SignCaption
                txtField(iItemTie).Text = rsItem.Fields("Tie").Value & ""
                txtField(iItemHigh).Text = rsItem.Fields("High").Value & ""
                txtField(iItemYield).Text = rsItem.Fields("Yield").Value & ""
                txtField(iItemSales_Account).Text = rsItem.Fields("Sales_Account").Value & ""
                txtField(iItemNotAvailNote).Text = rsItem.Fields("Not_AvailableNote").Value & ""
                txtField(iItemCurrentHandlingCharge).Text = FormatNumber(IIf(IsDBNull(rsItem.Fields("FacilityHandlingCharge").Value), 0, rsItem.Fields("FacilityHandlingCharge").Value)) & ""
                txtField(iItemHandlingChargeOverride).Text = FormatNumber(IIf(IsDBNull(rsItem.Fields("FacilityHandlingChargeOverride").Value), 0, rsItem.Fields("FacilityHandlingChargeOverride").Value)) & ""

                If Not rsItem.Fields("PurchaseThresholdCouponAmount").Value.Equals(DBNull.Value) Then
                    txtField(iItemPurchaseThresholdCouponAmount).Text = FormatNumber(rsItem.Fields("PurchaseThresholdCouponAmount").Value & "", 2)
                Else
                    txtField(iItemPurchaseThresholdCouponAmount).Text = FormatNumber("0", 2)
                End If

                '-- Do Labels
                lbl_InsertDate.Text = ""
                lbl_UserIDDate.Text = ""
                lbl_UserID.Text = ""
                If Not rsItem.Fields("Insert_Date").Value.Equals(DBNull.Value) Then
                    lbl_InsertDate.Text = FormatDateTime(rsItem.Fields("Insert_Date").Value, DateFormat.ShortDate)
                Else
                    lbl_InsertDate.Text = ""
                End If

                '-- Do Combos            
                If Not rsItem.Fields("SubTeam_No").Value.Equals(DBNull.Value) Then
                    plSubTeam_No = rsItem.Fields("SubTeam_No").Value
                Else
                    plSubTeam_No = 0
                    Me.HierarchySelector1.cmbSubTeam.SelectedIndex = -1
                End If
                Me.HierarchySelector1.SelectedSubTeamId = plSubTeam_No

                If Not rsItem.Fields("SubTeam_Name").Value.Equals(DBNull.Value) And rsItem.Fields("SubTeam_Name").Value.ToString().ToLower() = "produce" Then
                    isProduceSubteam = True
                End If

                System.Windows.Forms.Application.DoEvents() 'To make sure the categories get loaded in this user control before setting the category value

                If Not rsItem.Fields("Category_ID").Value.Equals(DBNull.Value) Then
                    Me.HierarchySelector1.SelectedCategoryId = rsItem.Fields("Category_ID").Value
                    plCategory_Id = rsItem.Fields("Category_ID").Value
                Else
                    Me.HierarchySelector1.SelectedCategoryId = 0
                    Me.HierarchySelector1.cmbCategory.SelectedIndex = -1
                End If

                If Not rsItem.Fields("ProdHierarchyLevel3_ID").Value.Equals(DBNull.Value) Then
                    Me.HierarchySelector1.SelectedLevel3Id = rsItem.Fields("ProdHierarchyLevel3_ID").Value
                Else
                    Me.HierarchySelector1.SelectedLevel3Id = 0
                    Me.HierarchySelector1.cmbLevel3.SelectedIndex = -1
                End If

                If Not rsItem.Fields("ProdHierarchyLevel4_ID").Value.Equals(DBNull.Value) Then
                    Me.HierarchySelector1.SelectedLevel4Id = rsItem.Fields("ProdHierarchyLevel4_ID").Value
                Else
                    Me.HierarchySelector1.SelectedLevel4Id = 0
                    Me.HierarchySelector1.cmbLevel4.SelectedIndex = -1
                End If

                cmbBrand.SelectedIndex = mdtBrand.Rows.IndexOf(mdtBrand.Rows.Find(rsItem.Fields("Brand_ID").Value))

                If cmbBrand.SelectedIndex > -1 Then
                    Dim row As DataRow = mdtBrand.Rows.Find(rsItem.Fields("Brand_ID").Value)
                    iconBrandId = If(IsDBNull(row.Item("IconBrandId")), Nothing, row.Item("IconBrandId"))
                Else
                    iconBrandId = Nothing
                End If


                If Not rsItem.Fields("Manager_ID").Value.Equals(DBNull.Value) Then
                    cmbManagedBy.SelectedIndex = mgBydt.Rows.IndexOf(mgBydt.Rows.Find(rsItem.Fields("Manager_ID").Value))
                Else
                    cmbManagedBy.SelectedIndex = -1
                End If

                If Not IsDBNull(rsItem.Fields("TaxClassID").Value) Then
                    cmbTaxClass.SelectedIndex = mdtTaxClass.Rows.IndexOf(mdtTaxClass.Rows.Find(rsItem.Fields("TaxClassID").Value))
                Else
                    cmbTaxClass.SelectedIndex = -1
                End If

                mOrigTaxClassID = cmbTaxClass.SelectedValue

                SetCombo(cmbField(iItemOrigin_ID), rsItem.Fields("Origin_ID").Value)
                SetCombo(cmbField(iItemCountryProc_ID), rsItem.Fields("CountryProc_ID").Value)
                cmbSustainRankingDefault.SelectedIndex = IIf(IsDBNull(rsItem.Fields("SustainabilityRankingID").Value), -1, rsItem.Fields("SustainabilityRankingID").Value)
                SetCombo(cmbField(iitemItemType_ID), rsItem.Fields("ItemType_ID").Value)

                ' set NULL value to 99999 for national class
                Dim natClassSelection As String = "99999"
                If Not IsDBNull(rsItem.Fields("ClassID").Value) Then
                    natClassSelection = rsItem.Fields("ClassID").Value
                End If
                If (natClassSelection = "0") Then 'added for the times natclassid = 0
                    SetCombo(cmbField(iItemNatClassID), "99999")
                Else
                    SetCombo(cmbField(iItemNatClassID), natClassSelection)
                End If

                SetCombo(cmbLabelType, natClassSelection)

                cmbLabelType.SelectedIndex = -1
                If Not IsDBNull(rsItem.Fields("LabelType_ID").Value) Then
                    Dim index As Integer
                    For Each item As LabelTypeBO In cmbLabelType.Items
                        If item.LabelTypeID = rsItem.Fields("LabelType_ID").Value Then
                            cmbLabelType.SelectedIndex = index
                            Exit For
                        End If
                        index = index + 1
                    Next
                End If

                Call SetDistSubTeamCombo(rsItem.Fields("DistSubTeam_No").Value)

                '-- Do Checkboxs
                chkField(iItemRetail_Sale).Checked = System.Math.Abs(CInt(rsItem.Fields("Retail_Sale").Value))
                chkField(iItemKeep_Frozen).Checked = System.Math.Abs(CInt(rsItem.Fields("Keep_Frozen").Value))
                chkField(iItemFull_Pallet_Only).Checked = System.Math.Abs(CInt(rsItem.Fields("Full_Pallet_Only").Value))
                chkField(iItemShipper_Item).CheckState = System.Math.Abs(CInt(rsItem.Fields("Shipper_Item").Value))
                chkField(iItemWFM_Item).CheckState = System.Math.Abs(CInt(rsItem.Fields("WFM_Item").Value))
                chkField(iItemOrganic).CheckState = System.Math.Abs(CInt(rsItem.Fields("Organic").Value))
                chkField(iItemRefrigerated).CheckState = System.Math.Abs(CInt(rsItem.Fields("Refrigerated").Value))
                chkField(iItemNot_Available).CheckState = System.Math.Abs(CInt(rsItem.Fields("Not_Available").Value))
                chkField(iItemPre_Order).CheckState = System.Math.Abs(CInt(rsItem.Fields("Pre_Order").Value))
                chkField(iItemHFM).CheckState = System.Math.Abs(CInt(rsItem.Fields("HFM_Item").Value))
                chkField(iItemEXEDist).CheckState = System.Math.Abs(CInt(rsItem.Fields("EXEDistributed").Value))
                chkField(iNoDistMarkup).CheckState = System.Math.Abs(CInt(rsItem.Fields("NoDistMarkup").Value))
                chkField(iItemCostedByWeight).CheckState = System.Math.Abs(CInt(rsItem.Fields("CostedByWeight").Value))
                chkField(iItemRecallFlag).CheckState = System.Math.Abs(CInt(rsItem.Fields("Recall_Flag").Value))
                chkField(iItemLockAuthFlag).CheckState = System.Math.Abs(CInt(rsItem.Fields("LockAuth").Value))
                chkField(iItemPurchaseThresholdCouponSubTeam).CheckState = System.Math.Abs(CInt(rsItem.Fields("PurchaseThresholdCouponSubTeam").Value))

                '6.28.08 - New selection for catchweight required
                chkField(iItemCatchweightRequired).CheckState = System.Math.Abs(CInt(rsItem.Fields("CatchweightRequired").Value))

                '20090102 - New selection for COOL and BIO req's
                chkField(iItemCOOL).CheckState = System.Math.Abs(CInt(rsItem.Fields("COOL").Value))

                '6.28.08 - New selection for catchweight required
                chkField(iItemBIO).CheckState = System.Math.Abs(CInt(rsItem.Fields("BIO").Value))

                '7.3.10 - New field identifying Ingredients
                chkField(iItemIngredient).CheckState = System.Math.Abs(CInt(rsItem.Fields("Ingredient").Value))

                '8.18.16 - new field for 365 not available
                chkField(iItemNot_Available_365).CheckState = System.Math.Abs(CInt(rsItem.Fields("Not_Available_365").Value))

                ' 7.1.10 - new selection for sustainability
                chkSustainRankingRequired.CheckState = System.Math.Abs(CInt(rsItem.Fields("SustainabilityRankingRequired").Value))

                chkUseLastReceivedCost.CheckState = System.Math.Abs(CInt(_itemData.UseLastReceivedCost))
                CheckBoxGiftCard.CheckState = System.Math.Abs(CInt(_itemData.GiftCard))

                'The package description unit is no longer tied to the costed by weight flag.  All options should be available.
                LoadPackageDescriptionUnit(cmbField(iItemPackage_Unit_ID), False, True)

                SetCombo(cmbField(iItemPackage_Unit_ID), rsItem.Fields("Package_Unit_ID").Value)

                '4.22.08 - Decision reversed for 3.1 to revert back to original functionality
                LoadItemUnitsVendor(cmbField(iItemVendor_Unit_ID), (chkField(iItemCostedByWeight).CheckState = System.Windows.Forms.CheckState.Checked))

                '5/30/08 - Bug 6622 says these UOM's should match the vendor UOM's
                LoadItemUnitsVendor(cmbField(iItemDistribution_Unit_ID), (chkField(iItemCostedByWeight).CheckState = System.Windows.Forms.CheckState.Checked))
                LoadItemUnitsVendor(cmbField(iItemManufacturing_Unit_ID), (chkField(iItemCostedByWeight).CheckState = System.Windows.Forms.CheckState.Checked))

                'The retail units are no longer tied to the costed by weight flag.  All options should be available.
                LoadItemUnitsCost(cmbField(iItemRetail_Unit_ID), False, True)

                SelectItemByItemValue(cmbField(iItemPackage_Unit_ID), CInt(_itemData.PackageUnitID))
                SelectItemByItemValue(cmbField(iItemVendor_Unit_ID), CInt(_itemData.VendorUnitID))
                SelectItemByItemValue(cmbField(iItemDistribution_Unit_ID), CInt(_itemData.DistributionUnitID))
                SelectItemByItemValue(cmbField(iItemManufacturing_Unit_ID), CInt(_itemData.ManufacturingUnitID))
                SelectItemByItemValue(cmbField(iItemRetail_Unit_ID), CInt(_itemData.RetailUnitID))

                pbDeleted = rsItem.Fields("Remove_Item").Value Or rsItem.Fields("Deleted_Item").Value

                pbPrimary_Vendor = (rsItem.Fields("Primary_Vendor_Count").Value = 1)

                bIsEXEDistributed = System.Math.Abs(CInt(rsItem.Fields("IsEXEDistributed").Value))
                bIsSubTeamEXEDistributed = System.Math.Abs(CInt(rsItem.Fields("IsSubTeamEXEDistributed").Value))

                If IsDBNull(rsItem.Fields("User_ID").Value) Then
                    lUser_ID = -1
                Else
                    lUser_ID = rsItem.Fields("User_ID").Value
                    sUserName = rsItem.Fields("UserName").Value
                End If

                lItemSubTeam_No = IIf(IsDBNull(rsItem.Fields("SubTeam_No").Value), -1, rsItem.Fields("SubTeam_No").Value)

                m_bIsScaleItem = (rsItem.Fields("ScaleIdentifierCount").Value > 0)

                If pbDeleted Then
                    Label2.Text = "  Delete Date:"
                Else
                    Label2.Text = "Last Modified:"
                End If

                If Not IsDBNull(rsItem.Fields("User_ID_Date").Value) Then
                    msUser_ID_Date = rsItem.Fields("User_ID_Date").Value
                Else
                    msUser_ID_Date = String.Empty
                End If

                If Not IsDBNull(rsItem.Fields("User_ID").Value) Then
                    mUser_ID = rsItem.Fields("User_ID").Value
                Else
                    mUser_ID = String.Empty
                End If

                'setup SCALE data
                _scaleData = New ScaleBO
                _scaleData.ItemKey = plItem_Key
                _scaleData.ScaleDesc1 = rsItem.Fields("ScaleDesc1").Value & ""
                _scaleData.ScaleDesc2 = rsItem.Fields("ScaleDesc2").Value & ""
                _scaleData.ScaleDesc3 = rsItem.Fields("ScaleDesc3").Value & ""
                _scaleData.ScaleDesc4 = rsItem.Fields("ScaleDesc4").Value & ""
                _scaleData.Ingredients = rsItem.Fields("Ingredients").Value & ""
                _scaleData.ShelfLifeLength = CInt(rsItem.Fields("ShelfLife_Length").Value)

                If rsItem.Fields("ScaleTare").Value IsNot DBNull.Value Then
                    _scaleData.Tare = CInt(rsItem.Fields("ScaleTare").Value)
                End If
                If rsItem.Fields("ScaleUseBy").Value IsNot DBNull.Value Then
                    _scaleData.UseBy = CInt(rsItem.Fields("ScaleUseBy").Value)
                End If
                If rsItem.Fields("ScaleForcedTare").Value IsNot DBNull.Value Then
                    _scaleData.ForcedTare = rsItem.Fields("ScaleForcedTare").Value & ""
                End If

                ' setup POS Item data
                _posItemData = New POSItemBO
                _posItemData.ItemKey = plItem_Key

                If rsItem.Fields("Food_Stamps").Value IsNot DBNull.Value Then
                    _posItemData.FoodStamps = CType(rsItem.Fields("Food_Stamps").Value, Boolean)
                End If
                If rsItem.Fields("Price_Required").Value IsNot DBNull.Value Then
                    _posItemData.PriceRequired = CType(rsItem.Fields("Price_Required").Value, Boolean)
                End If
                If rsItem.Fields("Quantity_Required").Value IsNot DBNull.Value Then
                    _posItemData.QuantityRequired = CType(rsItem.Fields("Quantity_Required").Value, Boolean)
                End If
                If rsItem.Fields("QtyProhibit").Value IsNot DBNull.Value Then
                    _posItemData.QuantityProhibit = CType(rsItem.Fields("QtyProhibit").Value, Boolean)
                End If
                If rsItem.Fields("GroupList").Value IsNot DBNull.Value Then
                    _posItemData.GroupList = CInt(rsItem.Fields("GroupList").Value).ToString()
                End If
                If rsItem.Fields("Case_Discount").Value IsNot DBNull.Value Then
                    _posItemData.CaseDiscount = CType(rsItem.Fields("Case_Discount").Value, Boolean)
                End If
                If rsItem.Fields("Coupon_Multiplier").Value IsNot DBNull.Value Then
                    _posItemData.CouponMultiplier = CType(rsItem.Fields("Coupon_Multiplier").Value, Boolean)
                End If
                If rsItem.Fields("FSA_Eligible").Value IsNot DBNull.Value Then
                    _posItemData.FSAEligible = CType(rsItem.Fields("FSA_Eligible").Value, Boolean)
                End If
                If rsItem.Fields("Misc_Transaction_Sale").Value IsNot DBNull.Value Then
                    _posItemData.MiscTransactionSale = CShort(rsItem.Fields("Misc_Transaction_Sale").Value)
                End If
                If rsItem.Fields("Misc_Transaction_Refund").Value IsNot DBNull.Value Then
                    _posItemData.MiscTransactionRefund = CShort(rsItem.Fields("Misc_Transaction_Refund").Value)
                End If
                If rsItem.Fields("Ice_Tare").Value IsNot DBNull.Value Then
                    _posItemData.IceTare = CInt(rsItem.Fields("Ice_Tare").Value).ToString()
                End If
                If rsItem.Fields("Product_Code").Value IsNot DBNull.Value Then
                    _posItemData.ProductCode = rsItem.Fields("Product_Code").Value.ToString()
                End If
                If rsItem.Fields("Unit_Price_Category").Value IsNot DBNull.Value Then
                    _posItemData.UnitPriceCategory = CInt(rsItem.Fields("Unit_Price_Category").Value).ToString()
                End If

                _posItemData.IsValidated = _itemData.IsValidated
                _posItemData.HasIngredientIdentifier = _itemData.HasIngredientIdentifier

                ' we've got to close this before we open the next result set!
                rsItem.Close()
                rsItem = Nothing

                'Determine if user has access to this item's subteam
                'NOTE: This code has to be here AFTER the above rsItem.Close - cannot have more than one DAO recordset open at a time on same connection
                pbUserSubTeam = ItemAdminSubTeam(plItem_Key)

                '-- Do file locking
                If (lUser_ID = -1) Or (lUser_ID = giUserID) Then
                    lblReadOnly.Visible = False
                    cmdUnlock.Enabled = False
                Else
                    lblReadOnly.Text = String.Format(ResourcesIRMA.GetString("ReadOnly"), sUserName)
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = (gbItemAdministrator And pbUserSubTeam) Or gbLockAdministrator Or (giUserID = lUser_ID) Or gbSuperUser
                End If
            End If

            '-- Set the fields active/inactive
            For iLoop = iItemSales_Account To iItemNotAvailNote
                Select Case iLoop
                    Case iItemIdentifier
                    Case iItemShelfLife_Length 'field removed
                    Case iItemIngredients 'field removed
                    Case iItemScaleDesc1 'field removed
                    Case iItemScaleDesc2 'field removed
                    Case Else : SetActive(txtField(iLoop), (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                End Select
            Next iLoop

            Me.HierarchySelector1.SetAddHierarchLevelsActive((Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
            Me.HierarchySelector1.SetHierarchLevelComboboxesActive((Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))

            Me.HierarchySelector1.SetHierarchLevelComboboxesVisible((Not lblReadOnly.Visible) And (gbSuperUser Or gbBuyer Or gbCoordinator Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))

            SetActive(cmbDistSubTeam, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
            SetActive(cmbBrand, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
            SetActive(cmbTaxClass, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam) Or gbTaxAdministrator) And (Not pbDeleted))
            SetActive(cmbLabelType, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))

            For iLoop = iItemOrigin_ID To iItemManagedBy
                Select Case iLoop
                    Case iItemCost_Unit_ID
                        'Ignore - this was removed and now accessible on VendorCostDetail screen
                    Case iitemFreight_Unit_ID
                        'Leave alone - is always inactive and matches cost - leave on the form for now
                    Case iItemRetail_Unit_ID
                        'SetActive(cmbField(iLoop), (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted) And (chkField(iItemCostedByWeight).CheckState = System.Windows.Forms.CheckState.Unchecked))
                        SetActive(cmbField(iLoop), (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case iItemShelfLife_ID 'field removed
                    Case Else
                        SetActive(cmbField(iLoop), (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                End Select
            Next iLoop

            ' If the Shipper checkbox is checked, the pack-info fields and the pack and vendor UOM combos should be disabled.
            If chkField(iItemShipper_Item).CheckState Then
                SetActive(txtField(iItemPackage_Desc1), False)
                SetActive(txtField(iItemPackage_Desc2), False)
                SetActive(cmbField(iItemPackage_Unit_ID), False)
                SetActive(cmbField(iItemVendor_Unit_ID), False)
            End If

            For iLoop = chkField.LBound To chkField.UBound
                ' The Discontinue flag was Index 5 in the CheckBoxArray, but this checkbox was removed from this form.
                ' Changing the index of other checkboxes proved difficult, so this condition was added.
                If iLoop = 5 Then
                    Continue For
                End If

                Select Case iLoop
                    Case iItemNot_Available
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case iItemWFM_Item
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case iItemEXEDist
                        SetActive(chkField(iLoop), (Not lblReadOnly.Visible) And (Not pbDeleted) And bIsSubTeamEXEDistributed And (gbSuperUser Or gbDCAdmin Or (gbItemAdministrator And pbUserSubTeam)))
                    Case iItemCOOL
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case iItemBIO
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case iItemIngredient
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                    Case Else
                        SetActive(chkField(iLoop),
                            (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
                End Select
            Next iLoop

            ' enable the catchweight required
            If Me.lblCostedWeight.CheckState = CheckState.Checked And Me._chkField_16.CheckState And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted) Then
                Me._chkField_21.Enabled = True
            End If

            '-- Set the fields active/inactive for CheckBoxes that aren't part of a control array.
            SetActive(chkUseLastReceivedCost, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))
            SetActive(CheckBoxGiftCard, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))

            ' give the new POS button the same settings as the 4 POS checkboxes had
            SetActive(Button_POSSettings, (Not lblReadOnly.Visible) And (gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)) And (Not pbDeleted))

            If gbCoordinator Then
                For iLoop = chkField.LBound To chkField.UBound
                    Select Case iLoop
                        Case iItemNot_Available, iItemWFM_Item, iItemHFM : SetActive(chkField(iLoop), (Not lblReadOnly.Visible) And (Not pbDeleted))
                    End Select
                Next iLoop
                SetActive(cmbField(iItemOrigin_ID), (Not lblReadOnly.Visible) And (Not pbDeleted))
            End If

            ' Enable the Sustainability ranking check box depending on the item origin control
            SetActive(Me.chkSustainRankingRequired, cmbField(iItemOrigin_ID).Enabled)

            ' Enable the Sustainability ranking drop down depending on the checkbox
            SetActive(Me.cmbSustainRankingDefault, Me.chkSustainRankingRequired.Enabled And Me.chkSustainRankingRequired.Checked)

            If Not txtField(iItemNotAvailNote).ReadOnly Then
                If (chkField(iItemNot_Available).Enabled Or
                    chkField(iItemNot_Available_365).Enabled) Then
                    SetActive(txtField(iItemNotAvailNote), (chkField(iItemNot_Available).CheckState Or chkField(iItemNot_Available_365).CheckState))
                Else
                    SetActive(txtField(iItemNotAvailNote), False)
                End If
            End If

            ' Enable the Shipper-Items button if the Shipper checkbox is checked.
            SetActive(cmdShipper, ((_itemData.IsShipper) And Not (lblReadOnly.Visible Or pbDeleted)) And ((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser))
            SetActive(cmdItemVendors, (Not (lblReadOnly.Visible Or pbDeleted)) And (gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or Not gbTaxAdministrator))
            SetActive(cmdHistory, (Not (lblReadOnly.Visible Or pbDeleted)) And (gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or Not gbTaxAdministrator))
            SetActive(cmdPrices, Not (lblReadOnly.Visible Or pbDeleted))
            SetActive(cmdInventory, (Not (lblReadOnly.Visible Or pbDeleted)) And (gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or Not gbTaxAdministrator))
            SetActive(cmdSearch, True)
            SetActive(cmdIdentifier, (Not (lblReadOnly.Visible Or pbDeleted)) And (gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or Not gbTaxAdministrator))

            ' Only make visible if this item is a scale item.  The scale item info and 365 non-scale item info screen are mutually exlusive.
            If m_bIsScaleItem Then
                SetActive(cmdScale, Not (lblReadOnly.Visible Or pbDeleted) And (gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or Not gbTaxAdministrator))
                SetActive(ButtonNutrifacts, False)
                SetActive(Button365, False, Not gb365Administrator)
            Else
                SetActive(cmdScale, False)
                SetActive(ButtonNutrifacts, Not (lblReadOnly.Visible Or pbDeleted) And ((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser))

                Dim isButton365Active As Boolean = Not (lblReadOnly.Visible Or pbDeleted) And gb365Administrator And ItemIdentifierBO.IsPosPluIdentifier(Me.txtField(iItemIdentifier).Text)
                SetActive(Button365, isButton365Active, Not gb365Administrator)
            End If

            SetActive(cmdAdd, gbItemAdministrator Or gbSuperUser)
            SetActive(cmdBrandAdd, ((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser) And (Not (pbDeleted Or lblReadOnly.Visible)), bDisableBrandAdditions)

            SetActive(cmdDelete, (Not (pbDeleted Or lblReadOnly.Visible)) And ((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser))
            SetActive(cmdTaxClassZoom, Not (pbDeleted Or lblReadOnly.Visible))

            '----------------------------------------------------------------------------------------------------------
            'Bug 1302 - MY, NE TEST - Item Attributes icon greyed out to Item Administrator; SuperUser can't access
            'SetActive(cmdAttributes, (Not (lblReadOnly.Visible Or pbDeleted)) And Not gbTaxAdministrator)
            '----------------------------------------------------------------------------------------------------------
            SetActive(cmdAttributes, (Not (lblReadOnly.Visible Or pbDeleted)) And (gbPriceBatchProcessor Or gbSuperUser Or (gbItemAdministrator And pbUserSubTeam)))

            SetActive(cmdJurisdiction, (Not (lblReadOnly.Visible Or pbDeleted)) AndAlso InstanceDataDAO.IsFlagActive("UseStoreJurisdictions"))
            Me.HierarchySelector1.SetAddHierarchLevelsActive(((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser) And (Not (pbDeleted Or lblReadOnly.Visible)))

            SetActive(txtField(iItemPurchaseThresholdCouponAmount), ((gbItemAdministrator And pbUserSubTeam) Or gbSuperUser) And (Not (pbDeleted Or lblReadOnly.Visible)))
            SetActive(txtField(iItemCurrentHandlingCharge), False)
            SetActive(txtField(iItemHandlingChargeOverride), gbDCAdmin)

            If Not lRecord = -1 Then
                SQLOpenRS(rsItemChange, "EXEC GetItemChangeInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsItemChange.EOF Then
                    mChangeExists = False
                Else
                    mChangeExists = True
                End If

                If mChangeExists = True Then
                    If Not rsItemChange.Fields("User_ID_Date").Value.Equals(DBNull.Value) Then
                        lbl_UserIDDate.Text = FormatDateTime(rsItemChange.Fields("User_ID_Date").Value, DateFormat.ShortDate)
                    Else
                        lbl_UserIDDate.Text = ""
                    End If

                    If Not rsItemChange.Fields("UserName").Value.Equals(DBNull.Value) Then
                        lbl_UserID.Text = rsItemChange.Fields("UserName").Value
                    Else
                        lbl_UserID.Text = ""
                    End If
                Else
                    lbl_UserID.Text = ""
                    lbl_UserIDDate.Text = ""
                End If

                rsItemChange.Close()
                rsItemChange = Nothing

                If mChangeExists Then
                    GetItemChangeDifferences(lRecord)
                End If
            End If

            'Disable controls if the item has been validated from Icon
            If _itemData.IsValidated() Then
                SetActive(txtField(iItemPOS_Description), False)
                SetActive(cmbTaxClass, False)
                SetActive(txtField(iItemItem_Description), False)
                SetActive(txtField(iItemPackage_Desc1), False)
                SetActive(cmbBrand, False)
                SetActive(cmdBrandAdd, False, bDisableBrandAdditions)
                SetActive(cmdTaxClassZoom, False)
                SetActive(chkField(iItemOrganic), False)
                SetActive(cmbField(iItemNatClassID), False)

                'Lock down the Sign Caption field if we are using the Icon
                '  validated CustomerFriendlyDescription as the Sign_Description
                If InstanceDataDAO.IsFlagActive("EnableIconSignCaptionUpdates") Then
                    SetActive(_txtField_SignCaption, False)
                End If
            End If

            'If the identifier falls into the specified range, it's a non-retail ingredient item
            If _itemData.HasIngredientIdentifier() Then
                SetActive(chkField(iItemRetail_Sale), False)
            End If

            Dim strIdentifier = txtField(iItemIdentifier).Text
            If Len(strIdentifier) = 11 And
                ((strIdentifier >= "46000000000" And strIdentifier <= "46999999999") Or
                (strIdentifier >= "48000000000" And strIdentifier <= "48999999999")) Then
                bIsIngredientItem = True
            End If

            SetFieldsStates(plSubTeam_No, chkField(iItemRetail_Sale).Checked, bIsIngredientItem, _itemData.IsValidated)

            ' if any store in the region is on GPM, disable default jurisdiction Size/UOM controls if the item is validated
            Dim anyStoreInRegionIsOnGpm As Boolean = InstanceDataDAO.FlagIsOnForAnyStore("GlobalPriceManagement")

            SetGpmFieldsAvailability(anyStoreInRegionIsOnGpm, isProduceSubteam, _itemData.IsValidated)

        Catch ex As Exception
            logger.Error(ex.Message)
            ErrorDialog.HandleError(ex, ErrorDialog.NotificationTypes.DialogAndEmail)
        End Try

        pbDataChanged = False
        bLoading = False
    End Sub

    Private Sub SetDistSubTeamCombo(ByRef lValue As Integer)
        'When Item.DistSubTeam_No ISNULL, then the value is zero(0).
        If (lValue > 0) Then
            If (cmbDistSubTeam.Items.Count <= 1) Then
                Call PopulateDistSubTeams(cmbDistSubTeam)
            End If
            Call SetCombo(cmbDistSubTeam, lValue)
        Else
            ' When the DistSubTeam_No is not set, need to apply these rules:
            If (Me.HierarchySelector1.cmbSubTeam.SelectedIndex > -1) Then
                If (IsSubTeamDistributed(VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex)) = True) Then
                    ' When the SubTeam_No IS found, leave the DistSubTeam combo populated with Dist SubTeams and leave
                    '   enabled for the user to pick one.
                    Call PopulateDistSubTeamByRetailSubTeam(cmbDistSubTeam, VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex))
                    SetActive(cmbDistSubTeam, True)
                    cmbDistSubTeam.SelectedIndex = -1
                    ' When the DistSubTeam combo is enabled, require a selection from the user when saving.
                Else
                    ' When the SubTeam_No is NOT found in the distinct list of DistSubTeam.RetailSubTeam_No(s),
                    '   then populate the DistSubTeam combo with the SubTeam text (and disable).
                    cmbDistSubTeam.Items.Clear()
                    cmbDistSubTeam.Items.Add(Me.HierarchySelector1.cmbSubTeam.Text)
                    cmbDistSubTeam.SelectedIndex = 0
                    SetActive(cmbDistSubTeam, False)
                    '   Save as NULL.  You can tell since the DistSubTeam combo will be disabled.
                End If
            End If
        End If
    End Sub

    Private Sub SetDistSubTeamCombo(ByVal lValue As Object)
        cmbDistSubTeam.SelectedIndex = -1

        If Not IsDBNull(lValue) Then
            If (cmbDistSubTeam.Items.Count <= 1) Then
                Call PopulateDistSubTeams(cmbDistSubTeam)
            End If
            Call SetCombo(cmbDistSubTeam, lValue)
        Else
            ' When the DistSubTeam_No is not set, need to apply these rules:
            If (Me.HierarchySelector1.cmbSubTeam.SelectedIndex > -1) Then
                If (IsSubTeamDistributed(VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex)) = True) Then
                    ' When the SubTeam_No IS found, leave the DistSubTeam combo populated with Dist SubTeams and leave
                    '   enabled for the user to pick one.
                    Call PopulateDistSubTeamByRetailSubTeam(cmbDistSubTeam, VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex))
                    SetActive(cmbDistSubTeam, True)
                    cmbDistSubTeam.SelectedIndex = -1
                    ' When the DistSubTeam combo is enabled, require a selection from the user when saving.
                Else
                    ' When the SubTeam_No is NOT found in the distinct list of DistSubTeam.RetailSubTeam_No(s),
                    '   then populate the DistSubTeam combo with the SubTeam text (and disable).
                    cmbDistSubTeam.Items.Clear()
                    cmbDistSubTeam.Items.Add(Me.HierarchySelector1.cmbSubTeam.Text)
                    cmbDistSubTeam.SelectedIndex = 0
                    SetActive(cmbDistSubTeam, False)
                    '   Save as NULL.  You can tell since the DistSubTeam combo will be disabled.
                End If
            End If
        End If
    End Sub

    Private Sub GetItemChangeDifferences(ByVal iItemKey As Integer)
        Dim rsItemDif As DAO.Recordset = Nothing
        Dim sDif As String = ""
        Dim sName As String = ""
        Dim sNewValue As String = ""
        Dim sOldValue As String = ""
        Dim x As Integer

        msDiff = ""

        SQLOpenRS(rsItemDif, "EXEC CheckForItemChangeDifferences " & iItemKey, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If Not rsItemDif.EOF Then
            For x = 0 To rsItemDif.Fields.Count - 1
                If InStr(rsItemDif.Fields(x).Name, "I_") > 0 Then
                    sName = rsItemDif.Fields(x).Name
                    If Trim(rsItemDif.Fields(sName).Value.ToString) <> Trim(rsItemDif("ICH" & Mid(sName, 2)).Value.ToString) Then
                        If msDiff = "" Then
                            sNewValue = rsItemDif(sName).Value.ToString
                            If sNewValue = "" Then sNewValue = "Nothing"

                            sOldValue = rsItemDif.Fields("ICH" & Mid(sName, 2)).Value.ToString
                            If sOldValue = "" Then sOldValue = "Nothing"

                            msDiff = "The " & Mid(sName, 3) & " changed from " & sOldValue & " to " & sNewValue
                        Else
                            sNewValue = rsItemDif(sName).Value.ToString
                            If sNewValue = "" Then sNewValue = "Nothing"

                            sOldValue = rsItemDif.Fields("ICH" & Mid(sName, 2)).Value.ToString
                            If sOldValue = "" Then sOldValue = "Nothing"

                            msDiff = msDiff & vbCrLf & "The " & Mid(sName, 3) & " changed from " & sOldValue & " to " & sNewValue
                        End If
                    End If
                End If
            Next
        Else
            sDif = ""
        End If

        rsItemDif.Close()
        rsItemDif = Nothing
    End Sub

    ''' <summary>
    ''' refreshes the local copy of pos data so it can be passed back to the pos popup form and
    ''' the item overrides form
    ''' </summary>
    ''' <param name="posItemData"></param>
    ''' <remarks></remarks>
    Private Sub POSForm_UpdateCallingForm(ByVal posItemData As POSItemBO) Handles posForm.UpdateCallingForm
        _posItemData = posItemData
    End Sub

    Private Sub AdjustWidthComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles _cmbField_15.DropDown
        Dim senderComboBox As ComboBox = CType(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = CType(IIf((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0), Integer)
        Dim newWidth As Integer
        Dim itemEnum As IEnumerator = CType(sender, ComboBox).Items.GetEnumerator
        Dim currentItem As String

        While itemEnum.MoveNext
            currentItem = CType(itemEnum.Current, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemString
            newWidth = CType(g.MeasureString(currentItem, font).Width, Integer) + vertScrollBarWidth

            If width < newWidth Then
                width = newWidth
            End If
        End While

        senderComboBox.DropDownWidth = width
    End Sub

    Private Sub HierarchySelector1_HierarchySelectionChanged() Handles HierarchySelector1.HierarchySelectionChanged
        If Not (isInitializing Or bLoading) Then
            pbDataChanged = True
        End If
    End Sub

    Private Sub _radioFixedWeight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not (isInitializing Or bLoading) Then
            pbDataChanged = True
        End If
    End Sub

    Private Sub _radioVariableWeight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not (isInitializing Or bLoading) Then
            pbDataChanged = True
        End If
    End Sub

    Private Sub SelectItemByItemValue(ByRef cbo As System.Windows.Forms.ComboBox, ByVal ItemValue As Integer)
        Dim i As Integer

        For i = 0 To cbo.Items.Count - 1
            If VB6.GetItemData(cbo, i) = ItemValue Then
                cbo.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Sub _chkField_16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _chkField_16.CheckedChanged
        If _chkField_16.Checked Then
            Me._chkField_21.Enabled = True
        Else
            Me._chkField_21.Enabled = False
        End If
    End Sub

    Private Sub lbl_UserID_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_UserID.MouseEnter
        ttItemDif.SetToolTip(lbl_UserID, msDiff)
    End Sub

    Private Sub chkSustainRankingRequired_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSustainRankingRequired.CheckStateChanged
        If Me.isInitializing Then Exit Sub

        pbDataChanged = True

        If Not chkSustainRankingRequired.Checked Then cmbSustainRankingDefault.SelectedIndex = -1
        SetActive(cmbSustainRankingDefault, chkSustainRankingRequired.Checked)
    End Sub

    Private Sub cmbSustainRankingDefault_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSustainRankingDefault.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbSustainRankingDefault.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub cmbSustainRankingDefault_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSustainRankingDefault.SelectedIndexChanged
        If Me.isInitializing Or bLoading Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub cmdItemSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemSave.Click
        PSaveClick = True
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                Exit Sub
            End If
        End If
        PSaveClick = False
    End Sub

    Private Sub cmbManagedBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbManagedBy.TextChanged
        pbDataChanged = True
    End Sub

    Private Sub chkUseLastReceivedCost_CheckStateChanged(sender As Object, e As System.EventArgs) Handles chkUseLastReceivedCost.CheckStateChanged
        If Me.isInitializing Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub CheckBoxGiftCard_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxGiftCard.CheckedChanged
        If Not (isInitializing Or bLoading) Then
            pbDataChanged = True
        End If
    End Sub
    Private Sub SetFieldsStates(ByVal SubTeamNo As Integer, ByVal RetailSale As Boolean, ByVal IsIngredientItem As Boolean, ByVal IsValidated As Boolean)
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            If ((RetailSale Or (IsIngredientItem And IsValidated)) And SubTeamDAO.IsSubTeamAligned(SubTeamNo)) Then
                Me.HierarchySelector1.cmbSubTeam.Enabled = False
            Else
                Me.HierarchySelector1.cmbSubTeam.Enabled = True
            End If
        Else
            Me.HierarchySelector1.cmbSubTeam.Enabled = True
        End If
    End Sub

    Private Sub ButtonNutrifacts_Click(sender As Object, e As EventArgs) Handles ButtonNutrifacts.Click
        Using itemNutritionForm As New ItemNutrition(Identifier)
            itemNutritionForm.ShowDialog()
        End Using
    End Sub

    Private Sub lblSignCaption_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblSignCaption.LinkClicked
        _itemSignAttributesForm = New ItemSignAttributeForm()

        'set item identifier and description
        _itemSignAttributesForm.Identifier = Me.txtField(iItemIdentifier).Text
        _itemSignAttributesForm.Description = Me.txtField(iItemItem_Description).Text
        _itemSignAttributesForm.ItemDeleted = pbDeleted

        'open child form
        _itemSignAttributesForm.ShowDialog()
        _itemSignAttributesForm.Close()
        _itemSignAttributesForm.Dispose()
    End Sub

    Private Sub Button365_Click(sender As Object, e As EventArgs) Handles Button365.Click
        Using threeSixtyFiveScaleForm As New Form_ItemScaleDetails365()
            threeSixtyFiveScaleForm.ItemIdentifier = Me.txtField(iItemIdentifier).Text
            threeSixtyFiveScaleForm.Text = threeSixtyFiveScaleForm.Text & " - " & Me.txtField(iItemIdentifier).Text
            threeSixtyFiveScaleForm.ItemKey = glItemID
            threeSixtyFiveScaleForm.StoreJurisdictionId = _itemData.StoreJurisdictionID
            threeSixtyFiveScaleForm.StoreJurisdictionDesc = _itemData.StoreJurisdictionDesc

            threeSixtyFiveScaleForm.ShowDialog(Me)
        End Using
    End Sub
End Class
