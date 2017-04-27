Option Strict Off
Option Explicit On

Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.TaxHosting.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports System.Linq
Imports WholeFoods.Utility

Friend Class ItemOverride
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private _ItemIdentifier As String

    ' Stores the item data for the default jurisdiction
    Private _defaultItemData As ItemBO
    Private _defaultPOSData As POSItemBO

    ' Stores the item data for the currently selected override jurisdiction
    Private _overrideItemData As ItemBO
    Private _overridePOSData As POSItemBO

    ' Flags to track changes to the data
    Private _itemChanges As Boolean
    Private _posChanges As Boolean

    Private brandTable As DataTable

#Region "Form Load"

    Private Sub frmItem_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        IsInitializing = True

        Dim storeJurisdiction As New StoreJurisdictionDAO

        ' [Tom Lux, TFS 10678, Aug 4 2009] Getting "Cannot bind to the new display member" error, so moved .DataSource below DisplayMember and ValueMember.
        ComboBox_AltJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        ComboBox_AltJurisdiction.ValueMember = "StoreJurisdictionID"
        ComboBox_AltJurisdiction.DataSource = storeJurisdiction.GetJurisdictionList(CInt(_defaultItemData.StoreJurisdictionID))

        LoadPackageDescriptionUnit(ComboBox_UnitOfMeasure, False)
        LoadItemUnitsVendor(ComboBox_VendorOrder, False)
        LoadItemUnitsVendor(ComboBox_Distribution, False)
        LoadItemUnitsVendor(ComboBox_Manufacturing, False)
        LoadItemUnitsCost(ComboBox_Retail, False)

        IsInitializing = False

        _txtField_1.Text = _ItemIdentifier

        ' Populate the Brand ComboBox.
        brandTable = GetBrandData()
        ComboBoxBrand.DataSource = brandTable
        ComboBoxBrand.DisplayMember = "Brand_Name"
        ComboBoxBrand.ValueMember = "Brand_ID"
        ComboBoxBrand.SelectedIndex = -1

        ' Populate the Label Type ComboBox.
        ComboBoxLabelType.DataSource = LabelTypeDAO.GetLabelTypeList
        ComboBoxLabelType.DisplayMember = "LabelTypeDesc"
        ComboBoxLabelType.ValueMember = "LabelTypeID"
        ComboBoxLabelType.SelectedIndex = -1

        'Show or hide the Add Brand Button
        Dim bDisableBrandAdditions As Boolean = ConfigurationServices.AppSettings("DisableBrandAdditions")
        SetActive(ButtonBrandAdd, True, bDisableBrandAdditions)

        ' Populate the Origin ComboBox.
        LoadOrigin(ComboBoxOrigin)

        ' Populate the Sustainability Ranking ComboBox.
        LoadSustainabilityRankings(ComboBoxSustainabilityRanking)

        ' Copy the Country of Proc ComboBox from the Origin ComboBox.
        ReplicateCombo(ComboBoxOrigin, ComboBoxCountryOfProc)

        ' Pre-fill the jurisdiction data, if it exists.
        PopulateJurisdictionData(ComboBox_AltJurisdiction.SelectedValue)

        ' Hide any options that are not being used by the region.
        HideOptions()

        ' Set ReadOnly permissions on the form.
        SetPermissions()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub SetPermissions()
        Dim IsEditable As Boolean = True

        ' [Tom Lux, TFS 10678, Aug 4 2009] This form should be editable for superusers and not buyers,
        ' but the buyer flags are set to TRUE for SuperUsers, so we need to make sure the user is not a superuser.

        ' TFS 12450 Robert S. - TFS 10678 introduced this bug -  The fix for 10678 ended up preventing users in 
        ' roles OTHER THAN Buyer and SuperUser from being able to edit jursidiciton information.
        ' This panel has the *exact* same permissions as the parent so the correct fix should have been 
        ' to enforce the same permission requirements here.

        If gbSuperUser Or (gbItemAdministrator And ItemAdminSubTeam(_overrideItemData.Item_Key)) Then
            IsEditable = True
        Else
            IsEditable = False
        End If

        ComboBox_AltJurisdiction.Enabled = IsEditable
        TextBox_POSDesc.Enabled = IsEditable
        TextBox_Description.Enabled = IsEditable
        TextBox_SignCaption.Enabled = IsEditable
        TextBox_Pack.Enabled = IsEditable
        TextBox_Size.Enabled = IsEditable
        ComboBox_UnitOfMeasure.Enabled = IsEditable
        ComboBox_VendorOrder.Enabled = IsEditable
        ComboBox_Distribution.Enabled = IsEditable
        ComboBox_Manufacturing.Enabled = IsEditable
        ComboBox_Retail.Enabled = IsEditable
        CheckBox_FoodStamps.Enabled = IsEditable
        CheckBox_QuantityProhibit.Enabled = IsEditable
        CheckBox_QuantityRequired.Enabled = IsEditable
        CheckBox_PriceRequired.Enabled = IsEditable
        CheckBox_CaseDiscount.Enabled = IsEditable
        CheckBox_CouponMultiplier.Enabled = IsEditable
        TextBox_GroupList.Enabled = IsEditable
        TextBox_IceTare.Enabled = IsEditable
        TextBox_MiscTransRefund.Enabled = IsEditable
        TextBox_MiscTransSale.Enabled = IsEditable

        ' New 4.8 controls.
        ' -----------------
        ' Item data.
        CheckBoxNotAvailable.Enabled = IsEditable
        CheckBoxSustainabilityRanking.Enabled = IsEditable
        CheckBoxCostedByWeight.Enabled = IsEditable
        CheckBoxIngredient.Enabled = IsEditable
        CheckBoxRecall.Enabled = IsEditable
        CheckBoxLockAuth.Enabled = IsEditable
        ComboBoxBrand.Enabled = IsEditable
        ComboBoxOrigin.Enabled = IsEditable
        ComboBoxCountryOfProc.Enabled = IsEditable
        ComboBoxLabelType.Enabled = IsEditable
        SetActive(Me.ComboBoxSustainabilityRanking, Me.CheckBoxSustainabilityRanking.Enabled And Me.CheckBoxSustainabilityRanking.Checked)
        TextBoxNotAvailableNote.Enabled = IsEditable

        ' POS data.
        CheckBoxFSAEligible.Enabled = IsEditable
        TextBoxProductCode.Enabled = IsEditable
        TextBoxUnitPriceCategory.Enabled = IsEditable

        'Disable controls if the item has been validated from Icon
        If _overrideItemData.IsValidated() Then
            SetActive(TextBox_POSDesc, False)
            SetActive(TextBox_Description, False)
            SetActive(TextBox_Pack, False)
            SetActive(ComboBoxBrand, False)
            SetActive(CheckBox_FoodStamps, False)
        End If

    End Sub

    ''' <summary>
    ''' Hides unused fields as determined by Instance Data flags.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HideOptions()
        ' NOTE: This should be kept in sync with any updates made to the logic on the POSItemInfo.vb form.
        Me.CheckBox_CaseDiscount.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSCaseDiscount")
        Me.CheckBox_CouponMultiplier.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSCouponMultipler")

        Me.TextBox_IceTare.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSIceTare")
        Me.Label_IceTare.Visible = Me.TextBox_IceTare.Visible

        Me.TextBox_MiscTransRefund.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransRefund")
        Me.Label_MiscTransRefund.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransRefund")

        Me.TextBox_MiscTransSale.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransSale")
        Me.Label_MiscTransSale.Visible = InstanceDataDAO.IsFlagActive("ShowItemPOSMiscTransSale")
    End Sub

#End Region

#Region "Property accessors"
    Public Property DefaultItemData() As ItemBO
        Get
            Return _defaultItemData
        End Get
        Set(ByVal value As ItemBO)
            _defaultItemData = value
        End Set
    End Property

    Public Property DefaultPOSData() As POSItemBO
        Get
            Return _defaultPOSData
        End Get
        Set(ByVal value As POSItemBO)
            _defaultPOSData = value
        End Set
    End Property

    Public Property ItemIdentifier() As String
        Get
            Return _ItemIdentifier
        End Get
        Set(ByVal value As String)
            _ItemIdentifier = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Lookup the override values in the database for the selected jurisdiction and
    ''' display them to the user.
    ''' </summary>
    ''' <param name="overrideJurisdictionId"></param>
    ''' <remarks></remarks>
    Private Sub PopulateJurisdictionData(ByVal overrideJurisdictionId As Integer)

        ' Initialize the override objects for the Item data and the POSItem data
        _overrideItemData = New ItemBO
        _overridePOSData = New POSItemBO

        ' Populate the item key and jurisdiction id for these objects
        _overrideItemData.Item_Key = _defaultItemData.Item_Key
        _overrideItemData.StoreJurisdictionID = ComboBox_AltJurisdiction.SelectedValue
        _overridePOSData.ItemKey = _defaultItemData.Item_Key
        _overridePOSData.StoreJurisdictionID = ComboBox_AltJurisdiction.SelectedValue

        ' Read all of the override data for the item and finish populating the override BOs
        Dim results As SqlDataReader = StoreJurisdictionDAO.GetStoreOverrideData(_defaultItemData.Item_Key, ComboBox_AltJurisdiction.SelectedValue)

        If results.HasRows() Then
            While results.Read
                If (Not results.IsDBNull(results.GetOrdinal("Item_Description"))) Then
                    _overrideItemData.ItemDescription = results.GetString(results.GetOrdinal("Item_Description"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Sign_Description"))) Then
                    _overrideItemData.SignCaption = results.GetString(results.GetOrdinal("Sign_Description"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Package_Desc1"))) Then
                    _overrideItemData.PackageDesc1 = results.GetDecimal(results.GetOrdinal("Package_Desc1")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Package_Desc2"))) Then
                    _overrideItemData.PackageDesc2 = results.GetDecimal(results.GetOrdinal("Package_Desc2")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Package_Unit_ID"))) Then
                    _overrideItemData.PackageUnitID = results.GetInt32(results.GetOrdinal("Package_Unit_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Retail_Unit_ID"))) Then
                    _overrideItemData.RetailUnitID = results.GetInt32(results.GetOrdinal("Retail_Unit_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Vendor_Unit_ID"))) Then
                    _overrideItemData.VendorUnitID = results.GetInt32(results.GetOrdinal("Vendor_Unit_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Distribution_Unit_ID"))) Then
                    _overrideItemData.DistributionUnitID = results.GetInt32(results.GetOrdinal("Distribution_Unit_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("POS_Description"))) Then
                    _overrideItemData.POSDescription = results.GetString(results.GetOrdinal("POS_Description"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Manufacturing_Unit_ID"))) Then
                    _overrideItemData.ManufacturingUnitID = results.GetInt32(results.GetOrdinal("Manufacturing_Unit_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Brand_ID"))) Then
                    _overrideItemData.BrandID = results.GetInt32(results.GetOrdinal("Brand_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Origin_ID"))) Then
                    _overrideItemData.OriginID = results.GetInt32(results.GetOrdinal("Origin_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("CountryProc_ID"))) Then
                    _overrideItemData.CountryOfProcID = results.GetInt32(results.GetOrdinal("CountryProc_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("SustainabilityRankingRequired"))) Then
                    _overrideItemData.SustainabilityRankingRequired = results.GetBoolean(results.GetOrdinal("SustainabilityRankingRequired")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("SustainabilityRankingID"))) Then
                    _overrideItemData.SustainabilityRankingID = results.GetInt32(results.GetOrdinal("SustainabilityRankingID")).ToString()
                Else
                    _overrideItemData.SustainabilityRankingID = -1
                End If
                If (Not results.IsDBNull(results.GetOrdinal("LabelType_ID"))) Then
                    _overrideItemData.LabelTypeID = results.GetInt32(results.GetOrdinal("LabelType_ID")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("CostedByWeight"))) Then
                    _overrideItemData.CostedByWeight = results.GetBoolean(results.GetOrdinal("CostedByWeight")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Average_Unit_Weight"))) Then
                    _overrideItemData.AverageUnitWeight = results.GetDecimal(results.GetOrdinal("Average_Unit_Weight")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Ingredient"))) Then
                    _overrideItemData.Ingredient = results.GetBoolean(results.GetOrdinal("Ingredient")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Recall_Flag"))) Then
                    _overrideItemData.Recall = results.GetBoolean(results.GetOrdinal("Recall_Flag")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("LockAuth"))) Then
                    _overrideItemData.LockAuth = results.GetBoolean(results.GetOrdinal("LockAuth")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Not_Available"))) Then
                    _overrideItemData.NotAvailable = results.GetBoolean(results.GetOrdinal("Not_Available")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Not_AvailableNote"))) Then
                    _overrideItemData.NotAvailableNote = results.GetString(results.GetOrdinal("Not_AvailableNote")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("QtyProhibit"))) Then
                    _overridePOSData.QuantityProhibit = results.GetBoolean(results.GetOrdinal("QtyProhibit"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Food_Stamps"))) Then
                    _overridePOSData.FoodStamps = results.GetBoolean(results.GetOrdinal("Food_Stamps"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Price_Required"))) Then
                    _overridePOSData.PriceRequired = results.GetBoolean(results.GetOrdinal("Price_Required"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Quantity_Required"))) Then
                    _overridePOSData.QuantityRequired = results.GetBoolean(results.GetOrdinal("Quantity_Required"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("GroupList"))) Then
                    _overridePOSData.GroupList = results.GetInt32(results.GetOrdinal("GroupList")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Case_Discount"))) Then
                    _overridePOSData.CaseDiscount = results.GetBoolean(results.GetOrdinal("Case_Discount"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Coupon_Multiplier"))) Then
                    _overridePOSData.CouponMultiplier = results.GetBoolean(results.GetOrdinal("Coupon_Multiplier"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Misc_Transaction_Sale"))) Then
                    _overridePOSData.MiscTransactionSale = results.GetInt16(results.GetOrdinal("Misc_Transaction_Sale")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Misc_Transaction_Refund"))) Then
                    _overridePOSData.MiscTransactionRefund = results.GetInt16(results.GetOrdinal("Misc_Transaction_Refund")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Ice_Tare"))) Then
                    _overridePOSData.IceTare = results.GetInt32(results.GetOrdinal("Ice_Tare")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("FSA_Eligible"))) Then
                    _overridePOSData.FSAEligible = results.GetBoolean(results.GetOrdinal("FSA_Eligible"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Product_Code"))) Then
                    _overridePOSData.ProductCode = results.GetString(results.GetOrdinal("Product_Code")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Unit_Price_Category"))) Then
                    _overridePOSData.UnitPriceCategory = results.GetInt32(results.GetOrdinal("Unit_Price_Category")).ToString()
                End If
                If (Not results.IsDBNull(results.GetOrdinal("IsValidated"))) Then
                    _overrideItemData.IsValidated = results.GetBoolean(results.GetOrdinal("IsValidated")).ToString()
                End If
            End While
        Else
            _overrideItemData.ItemDescription = _defaultItemData.ItemDescription
            _overrideItemData.POSDescription = _defaultItemData.POSDescription
            _overrideItemData.PackageDesc1 = _defaultItemData.PackageDesc1
            _overrideItemData.BrandID = _defaultItemData.BrandID
            _overridePOSData.FoodStamps = _defaultPOSData.FoodStamps
            _overrideItemData.IsValidated = _defaultItemData.IsValidated
        End If

        ' Populate the Item Information tab with override data for the selected jurisdiction
        PopulateItemInformation(_overrideItemData)

        ' Populate the POS Settings tab with override data for the selected jurisdiction
        PopulatePOSInformation(_overridePOSData)

        ' Default the data change flags for the newly selected jurisdiction
        _itemChanges = False
        _posChanges = False
    End Sub

    ''' <summary>
    ''' Refresh the UI data with the values from the ItemBO.
    ''' </summary>
    ''' <param name="itemData"></param>
    ''' <remarks></remarks>
    Private Sub PopulateItemInformation(ByRef itemData As ItemBO)

        ' Text fields.
        TextBox_POSDesc.Text = itemData.POSDescription
        TextBox_Description.Text = itemData.ItemDescription
        TextBox_SignCaption.Text = itemData.SignCaption
        TextBox_Pack.Text = Format(itemData.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger"))
        TextBox_Size.Text = Format(itemData.PackageDesc2, ResourcesIRMA.GetString("NumberFormatDecimal3"))
        If itemData.NotAvailable Then
            TextBoxNotAvailableNote.Text = itemData.NotAvailableNote
        Else
            TextBoxNotAvailableNote.Text = String.Empty
            TextBoxNotAvailableNote.Enabled = False
        End If

        ' CheckBoxes.
        CheckBoxNotAvailable.Checked = itemData.NotAvailable
        CheckBoxSustainabilityRanking.Checked = itemData.SustainabilityRankingRequired
        CheckBoxCostedByWeight.Checked = itemData.CostedByWeight
        CheckBoxIngredient.Checked = itemData.Ingredient
        CheckBoxRecall.Checked = itemData.Recall
        CheckBoxLockAuth.Checked = itemData.LockAuth

        ' ComboBoxes.
        If Not itemData.PackageUnitID Is Nothing Then
            SelectItemByItemValue(ComboBox_UnitOfMeasure, CInt(itemData.PackageUnitID))
        End If
        If Not itemData.VendorUnitID Is Nothing Then
            SelectItemByItemValue(ComboBox_VendorOrder, CInt(itemData.VendorUnitID))
        End If
        If Not itemData.DistributionUnitID Is Nothing Then
            SelectItemByItemValue(ComboBox_Distribution, CInt(itemData.DistributionUnitID))
        End If
        If Not itemData.ManufacturingUnitID Is Nothing Then
            SelectItemByItemValue(ComboBox_Manufacturing, CInt(itemData.ManufacturingUnitID))
        End If
        If Not itemData.RetailUnitID Is Nothing Then
            SelectItemByItemValue(ComboBox_Retail, CInt(itemData.RetailUnitID))
        End If
        If Not itemData.BrandID Is Nothing Then
            ComboBoxBrand.SelectedIndex = brandTable.Rows.IndexOf(brandTable.Rows.Find(itemData.BrandID))
        End If
        If Not itemData.OriginID Is Nothing Then
            SetCombo(ComboBoxOrigin, itemData.OriginID)
        End If
        If Not itemData.CountryOfProcID Is Nothing Then
            SetCombo(ComboBoxCountryOfProc, itemData.CountryOfProcID)
        End If
        If Not itemData.LabelTypeID Is Nothing Then
            Dim index As Integer
            For Each item As LabelTypeBO In ComboBoxLabelType.Items
                If item.LabelTypeID = itemData.LabelTypeID Then
                    ComboBoxLabelType.SelectedIndex = index
                    Exit For
                End If
                index = index + 1
            Next
        End If
        If itemData.SustainabilityRankingRequired Then
            ComboBoxSustainabilityRanking.SelectedIndex = If(itemData.SustainabilityRankingID = -1, -1, itemData.SustainabilityRankingID)
        Else
            ComboBoxSustainabilityRanking.SelectedIndex = -1
            ComboBoxSustainabilityRanking.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Refresh the UI data with the values from the POSItemBO.
    ''' </summary>
    ''' <param name="posData"></param>
    ''' <remarks></remarks>
    Private Sub PopulatePOSInformation(ByRef posData As POSItemBO)

        ' CheckBoxes.
        CheckBox_FoodStamps.Checked = posData.FoodStamps
        CheckBox_QuantityProhibit.Checked = posData.QuantityProhibit
        CheckBox_QuantityRequired.Checked = posData.QuantityRequired
        CheckBox_PriceRequired.Checked = posData.PriceRequired
        CheckBox_CaseDiscount.Checked = posData.CaseDiscount
        CheckBox_CouponMultiplier.Checked = posData.CouponMultiplier
        CheckBoxFSAEligible.Checked = posData.FSAEligible

        ' TextBoxes.
        TextBox_GroupList.Text = posData.GroupList
        TextBox_IceTare.Text = posData.IceTare
        TextBox_MiscTransSale.Text = posData.MiscTransactionSale
        TextBox_MiscTransRefund.Text = posData.MiscTransactionRefund
        TextBoxProductCode.Text = posData.ProductCode
        TextBoxUnitPriceCategory.Text = posData.UnitPriceCategory

    End Sub

#Region "Changes to Item Information Data"
    ' Checking for TextChanged event on some of the ComboBoxes in case the user clears the box using the Backspace key.

    Private Sub TextBox_POSDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_POSDesc.TextChanged
        _itemChanges = True
    End Sub

    Private Sub TextBox_Description_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Description.TextChanged
        _itemChanges = True
    End Sub

    Private Sub TextBox_SignCaption_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_SignCaption.TextChanged
        _itemChanges = True
    End Sub

    Private Sub TextBox_Pack_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Pack.TextChanged
        _itemChanges = True
    End Sub

    Private Sub TextBox_Size_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Size.TextChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBox_UnitOfMeasure_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_UnitOfMeasure.SelectedIndexChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBox_VendorOrder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_VendorOrder.SelectedIndexChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBox_Distribution_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Distribution.SelectedIndexChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBox_Manufacturing_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Manufacturing.SelectedIndexChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBox_Retail_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Retail.SelectedIndexChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBoxBrand_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxBrand.SelectedIndexChanged, ComboBoxBrand.TextChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBoxOrigin_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxOrigin.SelectedIndexChanged, ComboBoxOrigin.TextChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBoxCountryOfProc_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxCountryOfProc.SelectedIndexChanged, ComboBoxCountryOfProc.TextChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBoxLabelType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxLabelType.SelectedIndexChanged, ComboBoxLabelType.TextChanged
        _itemChanges = True
    End Sub

    Private Sub ComboBoxSustainabilityRanking_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxSustainabilityRanking.SelectedIndexChanged, ComboBoxSustainabilityRanking.TextChanged
        _itemChanges = True
    End Sub

    Private Sub TextBoxNotAvailableNote_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBoxNotAvailableNote.TextChanged
        _itemChanges = True
    End Sub

    Private Sub CheckBoxCostedByWeight_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxCostedByWeight.CheckedChanged
        _itemChanges = True

        Dim unitOfMeasureIndex As Integer = 0
        Dim vendorOrderIndex As Integer = 0
        Dim distributionIndex As Integer = 0
        Dim manufacturingIndex As Integer = 0
        Dim retailIndex As Integer = 0

        If ComboBox_UnitOfMeasure.SelectedIndex > -1 Then
            unitOfMeasureIndex = VB6.GetItemData(ComboBox_UnitOfMeasure, ComboBox_UnitOfMeasure.SelectedIndex)
        End If

        If ComboBox_VendorOrder.SelectedIndex > -1 Then
            vendorOrderIndex = VB6.GetItemData(ComboBox_VendorOrder, ComboBox_VendorOrder.SelectedIndex)
        End If

        If ComboBox_Distribution.SelectedIndex > -1 Then
            distributionIndex = VB6.GetItemData(ComboBox_Distribution, ComboBox_Distribution.SelectedIndex)
        End If

        If ComboBox_Manufacturing.SelectedIndex > -1 Then
            manufacturingIndex = VB6.GetItemData(ComboBox_Manufacturing, ComboBox_Manufacturing.SelectedIndex)
        End If

        If ComboBox_Retail.SelectedIndex > -1 Then
            retailIndex = VB6.GetItemData(ComboBox_Retail, ComboBox_Retail.SelectedIndex)
        End If

        LoadPackageDescriptionUnit(ComboBox_UnitOfMeasure, CheckBoxCostedByWeight.Checked)
        LoadItemUnitsVendor(ComboBox_VendorOrder, CheckBoxCostedByWeight.Checked)
        LoadItemUnitsVendor(ComboBox_Distribution, CheckBoxCostedByWeight.Checked)
        LoadItemUnitsVendor(ComboBox_Manufacturing, CheckBoxCostedByWeight.Checked)
        LoadItemUnitsCost(ComboBox_Retail, CheckBoxCostedByWeight.Checked)

        If unitOfMeasureIndex > 0 Then
            SetCombo(ComboBox_UnitOfMeasure, unitOfMeasureIndex)
        End If

        If vendorOrderIndex > 0 Then
            SetCombo(ComboBox_VendorOrder, vendorOrderIndex)
        End If

        If distributionIndex > 0 Then
            SetCombo(ComboBox_Distribution, distributionIndex)
        End If

        If manufacturingIndex > 0 Then
            SetCombo(ComboBox_Manufacturing, manufacturingIndex)
        End If

        If retailIndex > 0 Then
            SetCombo(ComboBox_Retail, retailIndex)
        End If
    End Sub

    Private Sub CheckBoxIngredient_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxIngredient.CheckedChanged
        _itemChanges = True
    End Sub

    Private Sub CheckBoxRecall_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxRecall.CheckedChanged
        _itemChanges = True
    End Sub

    Private Sub CheckBoxLockAuth_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxLockAuth.CheckedChanged
        _itemChanges = True
    End Sub

#End Region

#Region "Changes to POS Settings Data"
    Private Sub CheckBox_EmployeeDisc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        _posChanges = True
    End Sub

    Private Sub CheckBox_FoodStamps_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_FoodStamps.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub CheckBox_QuantityProhibit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_QuantityProhibit.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub CheckBox_QuantityRequired_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_QuantityRequired.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub CheckBox_PriceRequired_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_PriceRequired.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub CheckBox_CaseDiscount_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CaseDiscount.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub CheckBox_CouponMultiplier_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CouponMultiplier.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub TextBox_GroupList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_GroupList.TextChanged
        _posChanges = True
    End Sub

    Private Sub TextBox_IceTare_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_IceTare.TextChanged
        _posChanges = True
    End Sub

    Private Sub TextBox_MiscTransRefund_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_MiscTransRefund.TextChanged
        _posChanges = True
    End Sub

    Private Sub TextBox_MiscTransSale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_MiscTransSale.TextChanged
        _posChanges = True
    End Sub

    Private Sub CheckBoxFSAEligible_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxFSAEligible.CheckedChanged
        _posChanges = True
    End Sub

    Private Sub TextBoxProductCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBoxProductCode.TextChanged
        _posChanges = True
    End Sub

    Private Sub TextBoxUnitPriceCategory_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBoxUnitPriceCategory.TextChanged
        _posChanges = True
    End Sub

#End Region

    ''' <summary>
    ''' Save the updates to the database for the override data.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SaveChanges() As Boolean

        Dim saveSuccess As Boolean = False
        Dim valErrors As Boolean = False
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim currentItemStatus As ItemStatus
        Dim currentPOSStatus As POSItemStatus
        Dim errorMsg As New StringBuilder

        ' Check that a Sustainability Ranking is present if Sustainability Ranking Required is checked.
        If ComboBoxSustainabilityRanking.SelectedValue = Nothing And CheckBoxSustainabilityRanking.Checked Then
            If MsgBox("A Sustainability Ranking is required, but none has been chosen.  This may prevent orders that contain this item from being closed.  Proceed anyway?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                Exit Function
            End If
        End If

        ' Populate the override business objects with the current form data, but don't change the
        ' JurisdictionID here.  It is set when the jurisdiction ComboBox is first selected.  
        ' If the user is changing her jurisdiction selection, we want to save the values from the previous selection first.

        ' TextBoxes.
        _overrideItemData.POSDescription = Trim(TextBox_POSDesc.Text)
        _overrideItemData.ItemDescription = Trim(TextBox_Description.Text)
        _overrideItemData.SignCaption = Trim(TextBox_SignCaption.Text)
        _overrideItemData.PackageDesc1 = TextBox_Pack.Text
        _overrideItemData.PackageDesc2 = TextBox_Size.Text
        _overrideItemData.NotAvailableNote = Trim(TextBoxNotAvailableNote.Text)

        ' ComboBoxes.
        If ComboBox_UnitOfMeasure.SelectedIndex <> -1 Then
            _overrideItemData.PackageUnitID = VB6.GetItemData(ComboBox_UnitOfMeasure, ComboBox_UnitOfMeasure.SelectedIndex)
        Else
            _overrideItemData.PackageUnitID = Nothing
        End If
        If ComboBox_VendorOrder.SelectedIndex <> -1 Then
            _overrideItemData.VendorUnitID = VB6.GetItemData(ComboBox_VendorOrder, ComboBox_VendorOrder.SelectedIndex)
        Else
            _overrideItemData.VendorUnitID = Nothing
        End If
        If ComboBox_Distribution.SelectedIndex <> -1 Then
            _overrideItemData.DistributionUnitID = VB6.GetItemData(ComboBox_Distribution, ComboBox_Distribution.SelectedIndex)
        Else
            _overrideItemData.DistributionUnitID = Nothing
        End If
        If ComboBox_Manufacturing.SelectedIndex <> -1 Then
            _overrideItemData.ManufacturingUnitID = VB6.GetItemData(ComboBox_Manufacturing, ComboBox_Manufacturing.SelectedIndex)
        Else
            _overrideItemData.ManufacturingUnitID = Nothing
        End If
        If ComboBox_Retail.SelectedIndex <> -1 Then
            _overrideItemData.RetailUnitID = VB6.GetItemData(ComboBox_Retail, ComboBox_Retail.SelectedIndex)
        Else
            _overrideItemData.RetailUnitID = Nothing
        End If
        If ComboBoxBrand.SelectedIndex <> -1 Then
            _overrideItemData.BrandID = ComboBoxBrand.SelectedValue
        Else
            _overrideItemData.BrandID = Nothing
        End If
        If ComboBoxOrigin.SelectedIndex <> -1 Then
            _overrideItemData.OriginID = ComboValue(ComboBoxOrigin)
        Else
            _overrideItemData.OriginID = Nothing
        End If
        If ComboBoxCountryOfProc.SelectedIndex <> -1 Then
            _overrideItemData.CountryOfProcID = ComboValue(ComboBoxCountryOfProc)
        Else
            _overrideItemData.CountryOfProcID = Nothing
        End If
        If ComboBoxLabelType.SelectedIndex <> -1 Then
            _overrideItemData.LabelTypeID = ComboBoxLabelType.SelectedValue
        Else
            _overrideItemData.LabelTypeID = Nothing
        End If

        If ComboBoxSustainabilityRanking.SelectedIndex > 0 Then
            _overrideItemData.SustainabilityRankingID = ComboBoxSustainabilityRanking.SelectedIndex
        Else
            _overrideItemData.SustainabilityRankingID = Nothing
        End If

        ' CheckBoxes.
        _overrideItemData.NotAvailable = CheckBoxNotAvailable.Checked
        _overrideItemData.SustainabilityRankingRequired = CheckBoxSustainabilityRanking.Checked
        _overrideItemData.CostedByWeight = CheckBoxCostedByWeight.Checked
        _overrideItemData.Ingredient = CheckBoxIngredient.Checked
        _overrideItemData.Recall = CheckBoxRecall.Checked
        _overrideItemData.LockAuth = CheckBoxLockAuth.Checked

        ' Populate the POS Settings tab with override data for the selected jurisdiction.
        _overridePOSData.FoodStamps = CheckBox_FoodStamps.Checked
        _overridePOSData.QuantityProhibit = CheckBox_QuantityProhibit.Checked
        _overridePOSData.QuantityRequired = CheckBox_QuantityRequired.Checked
        _overridePOSData.PriceRequired = CheckBox_PriceRequired.Checked
        _overridePOSData.CaseDiscount = CheckBox_CaseDiscount.Checked
        _overridePOSData.CouponMultiplier = CheckBox_CouponMultiplier.Checked
        _overridePOSData.GroupList = TextBox_GroupList.Text
        _overridePOSData.IceTare = TextBox_IceTare.Text
        _overridePOSData.MiscTransactionSale = TextBox_MiscTransSale.Text
        _overridePOSData.MiscTransactionRefund = TextBox_MiscTransRefund.Text
        _overridePOSData.FSAEligible = CheckBoxFSAEligible.Checked
        _overridePOSData.UnitPriceCategory = TextBoxUnitPriceCategory.Text
        _overridePOSData.ProductCode = TextBoxProductCode.Text

        ' Validate the data on the Item Information tab
        statusList = _overrideItemData.ValidateData()

        ' Loop through possible validation erorrs and build message string containing all errors.
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentItemStatus = CType(statusEnum.Current, ItemStatus)

            Select Case currentItemStatus
                Case ItemStatus.Error_POSDescriptionRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_POSDesc.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_ItemDescriptionRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Description.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_SignCaptionRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_SignCaption.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_PackageDesc1Required
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Pack.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_PackageDesc1ValueIsZero
                    errorMsg.Append(String.Format(ResourcesItemHosting.GetString("PkgDescNotZero"), ResourcesItemHosting.GetString("One")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_PackageDesc2Required
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_RetailPackSize.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_PackageDesc2ValueIsZero
                    errorMsg.Append(String.Format(ResourcesItemHosting.GetString("PkgDescNotZero"), ResourcesItemHosting.GetString("Two")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_PackageDescUnitRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_RetailPackUOM.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_VendorUnitRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_VendorOrder.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_DistributionUnitRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Distribution.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
                Case ItemStatus.Error_RetailUnitRequired
                    errorMsg.Append(String.Format(ResourcesIRMA.GetString("Required"), Me.Label_Retail.Text.Replace(":", "")))
                    errorMsg.Append(Environment.NewLine)
            End Select
        End While

        ' Validate the data on the POS Settings tab.
        statusList = _overridePOSData.ValidateData()

        ' Loop through possible validation erorrs and build message string containing all errors.
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentPOSStatus = CType(statusEnum.Current, POSItemStatus)

            Select Case currentPOSStatus
                Case POSItemStatus.Error_GroupListNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Me.Label_GroupList.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_QtyRequiredAndProhibitBothTrue
                    errorMsg.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_QtyReqAndProhibit"), Me.CheckBox_QuantityProhibit.Text, Me.CheckBox_QuantityRequired.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_IceTareNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Me.Label_IceTare.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_MiscTransSaleNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransSale.Text))
                    errorMsg.Append(Environment.NewLine)
                Case POSItemStatus.Error_MiscTransRefundNotIntegerFormat
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MiscTransRefund.Text))
                    errorMsg.Append(Environment.NewLine)
            End Select
        End While

        If errorMsg.Length <= 0 Then
            ' Save the changes to the database.
            StoreJurisdictionDAO.SaveStoreOverrideData(_overrideItemData.Item_Key, _overrideItemData.StoreJurisdictionID, _overrideItemData, _overridePOSData)
            saveSuccess = True

            ' Reset the change flags since all changes have been saved.
            _itemChanges = False
            _posChanges = False
        Else
            MessageBox.Show(errorMsg.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Return saveSuccess

    End Function

    ''' <summary>
    ''' The user changed the selected jurisdiction.  Save the current changes, if there are any,
    ''' and then reload the data for the new jurisdiction.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_AltJurisdiction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_AltJurisdiction.SelectedIndexChanged
        If Not IsInitializing Then

            ' If the user is selecting another jurisdiction, prompt them to save any changes to the current
            ' jurisdiction first.
            If _itemChanges Or _posChanges Then
                Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Windows.Forms.DialogResult.Yes Then
                    If SaveChanges() Then
                        ' Pre-fill the UI values for the newly selected jurisdiction.
                        PopulateJurisdictionData(ComboBox_AltJurisdiction.SelectedValue)
                    Else
                        ' Remain on the previous selection and allow the user to correct the
                        ' values for saving.  Set the change flags to false before resetting the 
                        ' selection so the user is not prompted to save again.
                        Dim previousItemChangesFlag As Boolean = _itemChanges
                        Dim previousPOSChangesFlag As Boolean = _posChanges
                        _itemChanges = False
                        _posChanges = False
                        ComboBox_AltJurisdiction.SelectedValue = _overrideItemData.StoreJurisdictionID
                        _itemChanges = previousItemChangesFlag
                        _posChanges = previousPOSChangesFlag
                    End If
                Else
                    ' Pre-fill the UI values for the newly selected jurisdiction, without a save.
                    PopulateJurisdictionData(ComboBox_AltJurisdiction.SelectedValue)
                End If
            Else
                ' There are no changes to save.  Reload the UI values for the new selection.
                PopulateJurisdictionData(ComboBox_AltJurisdiction.SelectedValue)
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user is exiting the form.  Prompt them to save changes, if there are any.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click

        ' If there have been changes, prompt the user to save before exiting.
        If _itemChanges Or _posChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                If SaveChanges() Then
                    Me.Close()
                End If
            Else
                ' Just close without saving - change the edit flags so the user is not prompted for save
                ' again on the form closing event.
                _itemChanges = False
                _posChanges = False
                Me.Close()
            End If
        Else
            Me.Close()
        End If

    End Sub

    ''' <summary>
    ''' The user is closing the form.  Prompt them to save changes first, if there have been any.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemOverride_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' If there have been changes, prompt the user to save before exiting.
        If _itemChanges Or _posChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                If SaveChanges() = False Then
                    ' Do not close the form if the save was not successful.
                    e.Cancel = True
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' This button allows the user to populate the item override values with the values from the default
    ''' item jurisdiction.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_RefreshItemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RefreshItemInfo.Click

        ' Populate the Item Information tab with default item data
        Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_confirm_refreshOverrideData"), Me.TabPage_ItemInfo.Text), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            PopulateItemInformation(_defaultItemData)

        End If
    End Sub

    ''' <summary>
    ''' This button allows the user to populate the POS override values with the values from the default
    ''' item jurisdiction.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_RefreshPOSData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RefreshPOSData.Click

        ' Populate the POS Settings tab with default item data
        Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_confirm_refreshOverrideData"), Me.TabPage_POSInfo.Text), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            PopulatePOSInformation(_defaultPOSData)

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

    Private Sub ButtonBrandAdd_Click(sender As System.Object, e As System.EventArgs) Handles ButtonBrandAdd.Click

        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer
        Dim brandTable As New DataTable
        Dim bLoading As Boolean

        glBrandID = 0
        frmBrandAdd.ShowDialog()
        frmBrandAdd.Close()
        frmBrandAdd.Dispose()

        If glBrandID = -2 Then
            ComboBoxBrand.DataSource = Nothing
            ComboBoxBrand.Items.Clear()
            brandTable = GetBrandData()
            ComboBoxBrand.DataSource = brandTable

            ComboBoxBrand.DisplayMember = "Brand_Name"
            ComboBoxBrand.ValueMember = "Brand_ID"

            bLoading = True
            For lLoop = 0 To ComboBoxBrand.Items.Count - 1
                ComboBoxBrand.SelectedIndex = lLoop
                If ComboBoxBrand.SelectedValue > lMaxValue Then
                    lMaxValue = ComboBoxBrand.SelectedValue
                    lMax = lLoop
                End If
            Next lLoop
            ComboBoxBrand.SelectedIndex = lMax
            bLoading = False
        End If

    End Sub

    Private Sub CheckBoxNotAvailable_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxNotAvailable.CheckedChanged
        TextBoxNotAvailableNote.Enabled = CheckBoxNotAvailable.Checked
        If Not CheckBoxNotAvailable.Checked Then
            TextBoxNotAvailableNote.Text = String.Empty
        End If
        _itemChanges = True
    End Sub

    Private Sub ButtonSaveSettings_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSavePOSSettings.Click, ButtonSaveItemInformation.Click
        If SaveChanges() Then
            MsgBox("Changes have been saved.", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub CheckBoxSustainabilityRanking_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxSustainabilityRanking.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        _itemChanges = True

        If Not CheckBoxSustainabilityRanking.Checked Then ComboBoxSustainabilityRanking.SelectedIndex = -1
        SetActive(ComboBoxSustainabilityRanking, CheckBoxSustainabilityRanking.Enabled And CheckBoxSustainabilityRanking.Checked)

    End Sub

End Class