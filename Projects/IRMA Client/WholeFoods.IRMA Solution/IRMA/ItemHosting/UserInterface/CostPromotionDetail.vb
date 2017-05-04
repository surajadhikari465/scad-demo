Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class CostPromotionDetail

    Private _isAdd As Boolean
    Private _itemBO As ItemBO
    Private _vendorBO As VendorBO
    Private _storeBO As StoreBO
    Private _vendorDealBO As VendorDealBO

    Private IsInitializing As Boolean
    Private mbFilling As Boolean

#Region "properties"

    Public Property IsAdd() As Boolean
        Get
            Return _isAdd
        End Get
        Set(ByVal value As Boolean)
            _isAdd = value
        End Set
    End Property

    Public Property ItemBO() As ItemBO
        Get
            Return _itemBO
        End Get
        Set(ByVal value As ItemBO)
            _itemBO = value
        End Set
    End Property

    Public Property VendorBO() As VendorBO
        Get
            Return _vendorBO
        End Get
        Set(ByVal value As VendorBO)
            _vendorBO = value
        End Set
    End Property

    Public Property StoreBO() As StoreBO
        Get
            Return _storeBO
        End Get
        Set(ByVal value As StoreBO)
            _storeBO = value
        End Set
    End Property

    Public Property VendorDealBO() As VendorDealBO
        Get
            Return _vendorDealBO
        End Get
        Set(ByVal value As VendorDealBO)
            _vendorDealBO = value
        End Set
    End Property

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

    Private Sub CostPromotionDetail_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()

        SetFormTitle()
        LoadData()
        BindData()

        If Not CheckAllStoreSelectionEnabled() Then
            RadioButton_All.Text = "All 365"
        End If
    End Sub

    ''' <summary>
    ''' set form title based on user's action
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetFormTitle()
        Dim title As New StringBuilder

        title.Append(ResourcesItemHosting.GetString("CostPromotionTitle"))
        title.Append(" - ")

        If _isAdd Then
            title.Append("Add")
        Else
            title.Append("Edit")
        End If

        Me.Text = title.ToString
    End Sub

    ''' <summary>
    ''' loads read-only labels with data about current item/vendor
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadData()
        'display item unit data
        Dim packDesc As New StringBuilder
        packDesc.Append(Format(_itemBO.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger")))
        packDesc.Append(" / ")
        packDesc.Append(Format(_itemBO.PackageDesc2, ResourcesIRMA.GetString("NumberFormatBigDecimal")))
        packDesc.Append(" ")
        packDesc.Append(_itemBO.PackageUnitName)

        Me.Label_VendorValue.Text = _vendorBO.VendorName
        Me.Label_ItemValue.Text = _itemBO.ItemDescription
        Me.Label_PkgDescValue.Text = packDesc.ToString

        SetupStoreSelectionGrid()

        If Not _isAdd Then
            'load existing data values into form            
            Me.TextBox_CaseQty.Text = _vendorDealBO.CaseQty.ToString
            Me.TextBox_Amount.Text = _vendorDealBO.CaseAmt.ToString
            Me.dtpStartDate.Value = _vendorDealBO.StartDate
            Me.dtpEndDate.Value = _vendorDealBO.EndDate
            Me.CheckBox_NotStackable.Checked = _vendorDealBO.NotStackable

            'when in EDIT mode, user can only change the following: case qty, case amt, start date, end date
            Me.ComboBox_PromoCodeType.Enabled = False
            Me.ComboBox_DealType.Enabled = False

            '-- Highlight current store
            Call StoreListGridSelectStore(ugrdStoreList, _storeBO.StoreNo)

            'disable store selection box when editing existing cost promos
            Me.GroupBox_StoreSel.Enabled = False
        Else
            'enable store selection grid and populate drop downs
            Me.GroupBox_StoreSel.Enabled = True
        End If
    End Sub

    Private Sub SetupStoreSelectionGrid()
        '-- Fill out the store list
        Dim mdtStores As DataTable = StoreDAO.GetStoreItemVendorList(_itemBO.Item_Key, _vendorBO.VendorID)
        ugrdStoreList.DataSource = mdtStores

        'load zone drop down
        LoadZone(cmbZones)

        'load state drop down
        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

        Call SetCombos()
    End Sub

    ''' <summary>
    ''' loads data into drop downs w/ fixed values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        'get cost promo codes
        Dim codeList As ArrayList = CostPromotionDAO.GetCostPromoCodeTypes
        Me.ComboBox_PromoCodeType.DataSource = codeList
        If codeList.Count > 0 Then
            Me.ComboBox_PromoCodeType.ValueMember = "CostPromoCodeTypeID"
            Me.ComboBox_PromoCodeType.DisplayMember = "CostPromoDesc"
        End If

        'get vendor deal types
        Dim dealList As ArrayList = CostPromotionDAO.GetVendorDealTypes
        Me.ComboBox_DealType.DataSource = dealList
        If dealList.Count > 0 Then
            Me.ComboBox_DealType.ValueMember = "VendorDealTypeID"
            Me.ComboBox_DealType.DisplayMember = "VendorDealTypeDesc"
        End If

        'default selection of drop downs to blank value
        If _isAdd Then
            Me.ComboBox_PromoCodeType.SelectedIndex = -1
            Me.ComboBox_DealType.SelectedIndex = -1
        Else
            'set existing values
            Me.ComboBox_PromoCodeType.SelectedValue = _vendorDealBO.CostPromoBO.CostPromoCodeTypeID
            Me.ComboBox_DealType.SelectedValue = _vendorDealBO.DealTypeBO.VendorDealTypeID
        End If
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim vendorDeal As VendorDealBO
        Dim statusList As ArrayList
        Dim currentStatus As VendorDealStatus
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim storeList As New StringBuilder
        Dim row As UltraGridRow

        If _isAdd Then
            vendorDeal = New VendorDealBO
        Else
            vendorDeal = VendorDealBO
        End If

        'set non-UI values required for save
        vendorDeal.ItemKey = _itemBO.Item_Key
        vendorDeal.VendorID = _vendorBO.VendorID
        vendorDeal.PackageDesc1 = _itemBO.PackageDesc1
        vendorDeal.IsFromVendor = False 'indicates that data is coming from UI or vendor file directly

        'get user entries from form
        If Me.ComboBox_PromoCodeType.SelectedItem IsNot Nothing Then
            Dim costPromoType As New CostPromoCodeTypeBO
            costPromoType.CostPromoCodeTypeID = CType(Me.ComboBox_PromoCodeType.SelectedItem, CostPromoCodeTypeBO).CostPromoCodeTypeID
            costPromoType.CostPromoCode = CType(Me.ComboBox_PromoCodeType.SelectedItem, CostPromoCodeTypeBO).CostPromoCode
            costPromoType.CostPromoDesc = CType(Me.ComboBox_PromoCodeType.SelectedItem, CostPromoCodeTypeBO).CostPromoDesc
            vendorDeal.CostPromoBO = costPromoType
        Else
            vendorDeal.CostPromoBO = Nothing
        End If

        If Me.ComboBox_DealType.SelectedItem IsNot Nothing Then
            Dim vendorDealType As New VendorDealTypeBO
            vendorDealType.VendorDealTypeID = CType(Me.ComboBox_DealType.SelectedItem, VendorDealTypeBO).VendorDealTypeID
            vendorDealType.VendorDealTypeCode = CType(CType(Me.ComboBox_DealType.SelectedItem, VendorDealTypeBO).VendorDealTypeCode, Char)
            vendorDealType.VendorDealTypeDesc = CType(Me.ComboBox_DealType.SelectedItem, VendorDealTypeBO).VendorDealTypeDesc
            vendorDealType.CaseAmtType = Me.Label_AmountType.Text
            vendorDeal.DealTypeBO = vendorDealType
        Else
            vendorDeal.DealTypeBO = Nothing
        End If

        'validate that case qty is a numeric value
        If vendorDeal.ValidateNumericValue(Me.TextBox_CaseQty.Text) Then
            vendorDeal.CaseQty = CType(Me.TextBox_CaseQty.Text, Integer)
        Else
            'add to error msg - must be numeric value
            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_CaseQty.Text.Replace(":", "")))
            message.Append(Environment.NewLine)
        End If

        'validate that case amt is a numeric value
        If vendorDeal.ValidateNumericValue(Me.TextBox_Amount.Text) Then
            vendorDeal.CaseAmt = CType(Me.TextBox_Amount.Text, Decimal)
        Else
            'add to error msg - must be numeric value
            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_Amount.Text.Replace(":", "")))
            message.Append(Environment.NewLine)
        End If

        vendorDeal.StartDate = CType(Me.dtpStartDate.Value, Date)
        vendorDeal.EndDate = CType(Me.dtpEndDate.Value, Date)
        vendorDeal.NotStackable = Me.CheckBox_NotStackable.Checked

        vendorDeal.StoreListSeparator = "|"c

        'get list of selected stores

        If Me.ugrdStoreList.Selected.Rows.Count = 0 Then
        Else
            For Each row In Me.ugrdStoreList.Selected.Rows
                storeList.Append(row.Cells("Store_No").Value.ToString)
                storeList.Append(vendorDeal.StoreListSeparator)
            Next
        End If
        'remove last separator from store list and set value to vendorDeal object
        If storeList.ToString.Length > 0 Then
            vendorDeal.StoreList = storeList.ToString.Substring(0, storeList.ToString.Length - 1)
        End If

        'validate current set of data
        statusList = vendorDeal.ValidateData(vendorDeal)

        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, VendorDealStatus)

            Select Case currentStatus
                Case VendorDealStatus.Error_Required_CostPromoCode
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_PromoCodeType.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_Required_VendorDealType
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_DealType.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_Required_CaseQty
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_greaterThanZero"), Me.Label_CaseQty.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_CaseQty_MustBeWhole
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_decimalRounding"), Me.Label_CaseQty.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_Required_CaseAmt
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_greaterThanZero"), Me.Label_Amount.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_DataRange_CaseAmt
                    message.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_InvalidSmallMoneyValue"), Me.Label_Amount.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_StartDate_PastDate
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_startDateInFuture"), Me.Label_StartDate.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_EndDate_PastDate
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_endDatePastStartDate"), Me.Label_EndDate.Text.Replace(":", ""), Me.Label_StartDate.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_Conflict_NotStackable
                    message.Append(ResourcesItemHosting.GetString("msg_warning_vendorDealStackConflict"))
                    message.Append(Environment.NewLine)
                Case VendorDealStatus.Error_Required_StoreSelection
                    'prompt user to select store only when adding new cost promo
                    If _isAdd Then
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.GroupBox_StoreSel.Text))
                        message.Append(Environment.NewLine)
                    End If
            End Select
        End While

        If message.Length <= 0 Then
            Dim result As DialogResult

            'perform final validation - check if new discount/allowance will result in a negative net cost
            Dim storeCostConflicts As ArrayList = vendorDeal.GetNegativeCostStoreConflicts(vendorDeal)

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
                'save data
                Dim promoDAO As New CostPromotionDAO

                Try
                    If _isAdd Then
                        promoDAO.InsertVendorDeal(vendorDeal)
                    Else
                        promoDAO.UpdateVendorDeal(vendorDeal)
                    End If

                    success = True
                Catch ex As Exception
                    success = False
                    MessageBox.Show(ResourcesCommon.GetString("msg_dbError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                success = True
            End If
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

    Private Sub SetCombos()
        mbFilling = True

        'Zones.
        If Me.RadioButton_Zone.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If Me.RadioButton_State.Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False
    End Sub

#Region "Event Handlers"

    ''' <summary>
    ''' update the label that displays the Amount Type ($ or %)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox_DealType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_DealType.SelectedIndexChanged
        If Me.ComboBox_DealType.SelectedItem IsNot Nothing Then
            Me.Label_AmountType.Text = CType(Me.ComboBox_DealType.SelectedItem, VendorDealTypeBO).CaseAmtType
        End If
    End Sub

    ''' <summary>
    ''' exit form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' save data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ApplyChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ApplyChanges.Click
        'save changes
        If ApplyChanges() Then
            'adding cost promo does not update a parent grid; only editing comes from the parent grid
            If Not _isAdd Then
                'send update to parent grid
                RaiseEvent UpdateCallingForm()
            End If
            Me.Hide()
        End If
    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        If IsInitializing Or mbFilling Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False
    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Me.RadioButton_Zone_CheckedChanged(Me.RadioButton_Zone, New System.EventArgs)
    End Sub

    Private Sub RadioButton_Manual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Manual.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- Manual.
            cmbZones.SelectedIndex = -1
            cmbStates.SelectedIndex = -1

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_All_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_All.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- All Stores or All 365 for RM
            If CheckAllStoreSelectionEnabled() Then
                StoreListGridSelectAll(ugrdStoreList, True)
            Else
                StoreListGridSelectAll365(ugrdStoreList)
            End If

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_AllWFM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_AllWFM.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- All WFM.
            Call StoreListGridSelectAllWFM(ugrdStoreList)

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_Zone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Zone.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- By Zone
            If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_State_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_State.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- By State.
            If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex).ToString)

            mbFilling = False
        End If
    End Sub

#End Region

End Class
