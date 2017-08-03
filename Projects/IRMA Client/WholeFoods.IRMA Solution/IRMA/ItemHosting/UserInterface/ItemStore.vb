Option Strict Off
Option Explicit On

Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.Utility


Friend Class frmItemStore
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

    Private mbNoClick As Boolean
    Private mbFilling As Boolean
    Private NoManualButtonClick As Boolean

    Dim mdt As DataTable
    Dim mdv As DataView
    Private retailUomOverrideEnabled As Boolean = True
    Private scaleUomOverrideEnabled As Boolean = True

    Private _itemStore As ItemStoreBO
    Private storeItemAttributeBO As StoreItemAttributeBO

    Private bCanUpdate As Boolean = (Not frmItem.lblReadOnly.Visible) And _
            (gbSuperUser Or (gbItemAdministrator And frmItem.pbUserSubTeam)) And _
            (Not frmItem.pbDeleted)

    Dim WithEvents itemSearchForm As frmItemSearch

    ''' <summary>
    ''' Load the form and pre-populate the data.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmItemStore_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        LoadInstanceAttributes()
        ' populate the zone and state combo box values
        LoadZone(cmbZones)
        'populate subteam info
        LoadSubTeamInfo()
        ' populate kitchen route combo box
        LoadKitchenRoutes()
        ' populate AgeCode combo box
        LoadAgeCode()
        ' setup and populate the stores grid
        SetupDataTable()
        LoadDataTable()
        ' select the current store
        SelectStores()
        LoadRetailUomOverrideInfo()
        LoadScaleUomOverrideInfo()
        ' populate the ItemStoreBO for the selected store/item combination
        LoadOtherData()
        ' update access to the form controls based on the user's role
        SetPermissionsByRole()
        ' update the visible property of the ECommerce checkbox based on the config key ECommerce
        SetECommerceFlag()
        ' set the zone and state combo box selections
        SetCombos()
        LoadStates()
        ' enable / disable Exception SubTeam combobox based on the value of the intance data flag UKIPS
        SetFieldsStates()

        If Not CheckAllStoreSelectionEnabled() Then
            _optSelection_1.Text = "All 365"
        End If
    End Sub

    Private Sub LoadInstanceAttributes()
        Dim blnShowScanAudit As Boolean = False

        ' check the instance data for whether this screen should display the Exempt checkbox
        ExemptGroupBox.Visible = InstanceDataDAO.IsFlagActive("ExemptShelfTags", glStoreID)
        ' check the instance data to see whether the Freedom System Settings group box should be shown
        FreedomSystemGroupBox.Visible = InstanceDataDAO.IsFlagActive("ShowFreedomSystemSettings", glStoreID)

        'Show scan audit information based on the value of the ShowScanAudit config setting
        Try
            blnShowScanAudit = CBool(ConfigurationServices.AppSettings("ShowScanAudit").ToString)
        Catch ex As Exception
            blnShowScanAudit = False
        End Try

        grpScanAudit.Visible = blnShowScanAudit
    End Sub

    ''' <summary>
    ''' Update access to the form controls based on the user's IRMA role.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetPermissionsByRole()
        SetActive(GroupBox_Authorization, bCanUpdate)
        SetActive(GroupBox_Discontinue, bCanUpdate)
        SetActive(GroupBox_SubTeam, bCanUpdate)
        SetActive(GroupBox_CommonPOS, bCanUpdate)
        SetActive(GroupBox_LinkCode, bCanUpdate)
        SetActive(Button_TaxOverride, bCanUpdate Or gbTaxAdministrator)
        SetActive(Numeric_RoutingPriority, bCanUpdate)
        SetActive(ComboBox_KitchenRouteID, bCanUpdate)

        ' Toggle the Refresh checkbox under certain conditions
        SetAllowRefresh()

        ' now set the region specific restriction fields. these are determined by the type of POS the region is on.
        ' IBM and Retalix regions take two restriction fields, Age and Hours. NCR regions take only one, Age Code.

        ' check to see whether the Age Code text box is enabled
        If InstanceDataDAO.IsFlagActive("UseItemStoreAgeCode", glStoreID) And bCanUpdate Then
            TextBox_AgeCode.Enabled = True
            ComboBox_AgeCode.Enabled = True
            Label_AgeCode.Enabled = True
        Else
            TextBox_AgeCode.Enabled = False
            ComboBox_AgeCode.Enabled = False
            Label_AgeCode.Enabled = False
        End If

        ' check to see whether the Restricted Hours check box is enabled
        If InstanceDataDAO.IsFlagActive("UseItemStoreRestrictedHours", glStoreID) And bCanUpdate Then
            CheckBox_RestrictedHours.Enabled = True
        Else
            CheckBox_RestrictedHours.Enabled = False
        End If

        ' check to see whether the Age Restriction check box is enabled

        If InstanceDataDAO.IsFlagActive("UseItemStoreAgeRestricted", glStoreID) And bCanUpdate Then
            CheckBox_AgeRestrict.Enabled = True
        Else
            CheckBox_AgeRestrict.Enabled = False
        End If

        ' check to see whether to set the Restricted Hours label to "Restricted Hours" or leave as "Restricted"
        If InstanceDataDAO.IsFlagActive("UseItemStoreRestrictedHoursLabel", glStoreID) Then
            CheckBox_RestrictedHours.Text = "Restricted Hours"
        Else
            CheckBox_RestrictedHours.Text = "Restricted"
        End If

    End Sub

    Private Sub SelectStores()
        Dim iLoop As Integer

        '-- Highlight current store
        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
            If ugrdStoreList.Rows(iLoop).Cells("Store_No").Value = glStoreID Then
                ugrdStoreList.Rows(iLoop).Selected = True
                ugrdStoreList.Rows(iLoop).Activate()
            End If
        Next iLoop

    End Sub

    Private Sub LoadSubTeamInfo()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        dt = SubTeamDAO.GetSubteams
        dr = dt.NewRow()
        dr.Item("SubTeam_No") = -1
        dr.Item("SubTeam_Name") = "--No Exception--"
        dt.Rows.Add(dr)

        ComboBox_SubTeam.DataSource = dt
        ComboBox_SubTeam.DisplayMember = "SubTeam_Name"
        ComboBox_SubTeam.ValueMember = "SubTeam_No"
        ComboBox_SubTeam.SelectedValue = -1

    End Sub
    Private Sub LoadRetailUomOverrideInfo()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        dt = ItemDAO.GetRetailUoms
        dr = dt.NewRow()
        dr.Item("Unit_ID") = -1
        dr.Item("Unit_Name") = ""
        dt.Rows.InsertAt(dr, 0)

        cmbRetailUom.DataSource = dt
        cmbRetailUom.DisplayMember = "Unit_Name"
        cmbRetailUom.ValueMember = "Unit_ID"
        cmbRetailUom.SelectedValue = -1

    End Sub
    Private Sub LoadScaleUomOverrideInfo()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        dt = ItemDAO.GetScaleUoms
        dr = dt.NewRow()
        dr.Item("Unit_ID") = -1
        dr.Item("Description") = ""
        dt.Rows.InsertAt(dr, 0)

        cmbScaleUom.DataSource = dt
        cmbScaleUom.DisplayMember = "Description"
        cmbScaleUom.ValueMember = "Unit_ID"
        cmbScaleUom.SelectedValue = -1

    End Sub

    Private Sub HideOptions()
        FreedomSystemGroupBox.Enabled = InstanceDataDAO.IsFlagActive("ShowFreedomSystemSettings")
    End Sub

    Private Sub LoadKitchenRoutes()

        Dim krdt As New DataTable
        krdt = KitchenRouteDAO.GetComboList()
        ComboBox_KitchenRouteID.DataSource = krdt
        ComboBox_KitchenRouteID.DisplayMember = "Value"
        ComboBox_KitchenRouteID.ValueMember = "KitchenRoute_ID"
        ComboBox_KitchenRouteID.SelectedIndex = -1

    End Sub

    Private Sub LoadOtherData()
        ' this is only used for Massachusettes stores
        If ExemptCheckBox.Visible = True Then
            storeItemAttributeBO = New StoreItemAttributeBO
            storeItemAttributeBO.StoreNo = glStoreID
            storeItemAttributeBO.ItemKey = glItemID
            StoreItemAttributeDAO.GetAttribute(storeItemAttributeBO)
            ExemptCheckBox.Checked = storeItemAttributeBO.Exempt
        End If

        ' update the ItemStoreBO for the selected item/store combination
        _itemStore = New ItemStoreBO
        _itemStore.ItemKey = glItemID
        _itemStore.StoreId = glStoreID
        ItemDAO.GetItemStoreData(_itemStore)
        ' pre-fill the UI with the ItemStoreBO values
        CheckBox_AuthorizedItem.Checked = _itemStore.Authorized
        CheckBox_ECommerce.Checked = _itemStore.ECommerce
        CheckBox_Discontinue.Checked = _itemStore.Discontinue
        CheckBox_CompetitiveItem.Checked = _itemStore.CompetitiveItem
        CheckBox_GrillPrint.Checked = _itemStore.GrillPrint
        CheckBox_LineDiscount.Checked = _itemStore.LineDiscount
        CheckBox_RestrictedHours.Checked = _itemStore.RestrictedHours
        CheckBox_SeniorCitizen.Checked = _itemStore.SrCitizenDiscount
        CheckBox_LockedForSale.Checked = _itemStore.StopSale
        CheckBox_VisualVerify.Checked = _itemStore.VisualVerify
        CheckBox_EmployeeDiscount.Checked = _itemStore.Discountable
        CheckBox_RefreshPOSInfo.Checked = _itemStore.RefreshPOSInfo
        'TextBox_AgeCode.Text = _itemStore.AgeCode

        'If Trim(TextBox_AgeCode.Text) <> "" Then
        '    ComboBox_AgeCode.SelectedValue = CInt(TextBox_AgeCode.Text)
        'Else
        '    ComboBox_AgeCode.SelectedValue = 0
        'End If

        If IsDBNull(_itemStore.AgeCode) Or _
           _itemStore.AgeCode = "" Then
            ComboBox_AgeCode.SelectedValue = 0
        Else
            ComboBox_AgeCode.SelectedValue = _itemStore.AgeCode
        End If


        TextBox_POSTare.Text = _itemStore.POS_Tare
        TextBox_LinkValue.Text = _itemStore.POSLinkCode
        TextBox_LinkedItem.Text = _itemStore.LinkedIdentifier
        TextBox_MixMatch.Text = _itemStore.MixMatch
        CheckBox_PrintCondimentOnReceipt.Checked = _itemStore.PrintCondimentOnReceipt

        Numeric_RoutingPriority.Value = _itemStore.RoutingPriority
        CheckBox_ConsolidatePrice.Checked = _itemStore.ConsolidatePrice
        CheckBox_AgeRestrict.Checked = _itemStore.AgeRestrict
        CheckBox_LocalItem.Checked = _itemStore.LocalItem
        TextBox_ItemSurcharge.Text = _itemStore.ItemSurcharge
        CheckBox_ElectronicShelfTag.Checked = _itemStore.ElectronicShelfTag

        If Not _itemStore.KitchenRouteID = -1 Then
            ComboBox_KitchenRouteID.SelectedValue = _itemStore.KitchenRouteID
        End If

        ' Check to see a subteam exception has been setup
        If Not _itemStore.StoreSubTeam = -1 Then
            ComboBox_SubTeam.SelectedValue = _itemStore.StoreSubTeam
        Else
            ' Do not default to the item subteam if a store exception has not been defined.
            ' Instead, just show the default entry.
            ComboBox_SubTeam.SelectedValue = -1
        End If

        'Populate Scan Audit information
        If Not _itemStore.LastScannedUserName_DTS Is Nothing And Not _itemStore.LastScannedDate_DTS Is Nothing Then
            lblLastDTSScan.Text = _itemStore.LastScannedUserName_DTS & " on " & CDate(_itemStore.LastScannedDate_DTS).ToShortDateString & " at " & CDate(_itemStore.LastScannedDate_DTS).ToShortTimeString
        Else
            lblLastDTSScan.Text = ""
        End If

        If Not _itemStore.LastScannedUserName_NonDTS Is Nothing And Not _itemStore.LastScannedDate_NonDTS Is Nothing Then
            lblLastNonDTSScan.Text = _itemStore.LastScannedUserName_NonDTS & " on " & CDate(_itemStore.LastScannedDate_NonDTS).ToShortDateString & " at " & CDate(_itemStore.LastScannedDate_NonDTS).ToShortTimeString
        Else
            lblLastNonDTSScan.Text = ""
        End If

        SetAllowRefresh()

        'Check if 365 store UOM ovverride should be allowed
        If retailUomOverrideEnabled Then
            Me.cmbRetailUom.Enabled = True
        Else
            Me.cmbRetailUom.Enabled = False
        End If

        If scaleUomOverrideEnabled Then
            Me.cmbScaleUom.Enabled = True
        Else
            Me.cmbScaleUom.Enabled = False
        End If

        If IsDBNull(_itemStore.RetailUomId) Or _itemStore.RetailUomId = 0 Then
            cmbRetailUom.SelectedIndex = -1
        Else
            cmbRetailUom.SelectedValue = _itemStore.RetailUomId
        End If

        If IsDBNull(_itemStore.ScaleUomId) Or _itemStore.ScaleUomId = 0 Then
            cmbScaleUom.SelectedIndex = -1
            'Me.TextBox_FixedWeight.Enabled = False
            'Me.ByCountNumericEditor.Enabled = False
        Else
            cmbScaleUom.SelectedValue = _itemStore.ScaleUomId
            'Me.TextBox_FixedWeight.Enabled = True
            'Me.ByCountNumericEditor.Enabled = True
        End If

        If IsDBNull(_itemStore.FixedWeight) Or String.IsNullOrEmpty(_itemStore.FixedWeight) Then
            TextBox_FixedWeight.Text = ""
        Else
            TextBox_FixedWeight.Text = _itemStore.FixedWeight
        End If

        If IsDBNull(_itemStore.ByCount) Or _itemStore.ByCount = 0 Then
            ByCountNumericEditor.Value = 0
        Else
            ByCountNumericEditor.Value = _itemStore.ByCount
        End If

        If _itemStore.ItemStatusCode Is Nothing Then
            cmbItemStatusCode.SelectedIndex = -1
        Else
            cmbItemStatusCode.SelectedItem = _itemStore.ItemStatusCode.ToString()
        End If
    End Sub

    Private Sub SetAllowRefresh()

        If (CheckBox_AuthorizedItem.Checked And Not CheckBox_RefreshPOSInfo.Checked) And bCanUpdate Then
            SetActive(CheckBox_RefreshPOSInfo, True)
        Else
            SetActive(CheckBox_RefreshPOSInfo, False)
        End If

    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("ItemStore")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))

        'Hidden on grid.
        '---------------------
        mdt.Columns.Add(New DataColumn("Store_No", GetType(String)))
        mdt.Columns.Add(New DataColumn("Zone_ID", GetType(String)))
        mdt.Columns.Add(New DataColumn("State", GetType(String)))
        mdt.Columns.Add(New DataColumn("WFM_Store", GetType(String)))
        mdt.Columns.Add(New DataColumn("Mega_Store", GetType(String)))
        mdt.Columns.Add(New DataColumn("CustomerType", GetType(String)))

    End Sub

    Private Sub LoadDataTable()
        ugrdStoreList.DataSource = StoreDAO.GetStoreList
        ' Change the color of the store text to indicate if the store is currently authorized in IRMA
        Dim iLoop As Short
        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
            If StoreDAO.IsItemAuthorized(CInt(ugrdStoreList.Rows(iLoop).Cells("Store_No").Value), glItemID) Then
                ugrdStoreList.Rows(iLoop).Appearance.ForeColor = Color.Black
            Else
                ugrdStoreList.Rows(iLoop).Appearance.ForeColor = Color.Gray
            End If
        Next iLoop

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        Dim errorMsg As New StringBuilder
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim currentStatus As ItemStoreStatus
        Dim authStoreList As New ArrayList
        Dim deAuthStoreList As New ArrayList

        Dim iLoop As Short  ' loop index for stores

        ' validate that at least one store was selected for the update
        If ugrdStoreList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        ' If user is setting the Exempt Shelf tag value, this is the
        ' only thing that can be changed, so just save that.
        If ExemptGroupBox.Visible = True And ExemptCheckBox.Enabled = True Then
            ' confirm the user wants to apply the changes
            If MsgBox(ResourcesItemHosting.GetString("SendExemptChanges"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                Exit Sub
            End If

            ' save the data to the database for each of the selected stores
            For iLoop = 0 To ugrdStoreList.Selected.Rows.Count - 1
                _itemStore.StoreId = ugrdStoreList.Selected.Rows(iLoop).Cells("Store_No").Value

                'This is only for Massachusettes stores
                If InstanceDataDAO.IsFlagActive("ExemptShelfTags", _itemStore.StoreId) Then
                    storeItemAttributeBO.Exempt = ExemptCheckBox.Checked
                    StoreItemAttributeDAO.Save(storeItemAttributeBO)
                End If

            Next iLoop

            Exit Sub
        End If

        'set data into business object container
        _itemStore.ItemKey = glItemID
        _itemStore.Authorized = CheckBox_AuthorizedItem.Checked
        _itemStore.ECommerce = CheckBox_ECommerce.Checked
        _itemStore.Discontinue = CheckBox_Discontinue.Checked
        _itemStore.CompetitiveItem = CheckBox_CompetitiveItem.Checked
        _itemStore.LineDiscount = CheckBox_LineDiscount.Checked
        _itemStore.RestrictedHours = CheckBox_RestrictedHours.Checked
        _itemStore.StopSale = CheckBox_LockedForSale.Checked
        _itemStore.GrillPrint = CheckBox_GrillPrint.Checked
        _itemStore.VisualVerify = CheckBox_VisualVerify.Checked
        _itemStore.SrCitizenDiscount = CheckBox_SeniorCitizen.Checked
        _itemStore.POSLinkCode = TextBox_LinkValue.Text
        ' note: the link code values are populated by the Add and Remove buttons

        _itemStore.PrintCondimentOnReceipt = CheckBox_PrintCondimentOnReceipt.Checked
        _itemStore.KitchenRouteID = ComboBox_KitchenRouteID.SelectedValue

        _itemStore.RoutingPriority = Numeric_RoutingPriority.Value
        _itemStore.ConsolidatePrice = CheckBox_ConsolidatePrice.Checked
        _itemStore.AgeRestrict = CheckBox_AgeRestrict.Checked
        _itemStore.Discountable = CheckBox_EmployeeDiscount.Checked
        _itemStore.RefreshPOSInfo = CheckBox_RefreshPOSInfo.Checked
        _itemStore.LocalItem = CheckBox_LocalItem.Checked
        If String.IsNullOrWhiteSpace(cmbItemStatusCode.SelectedItem) Then
            _itemStore.ItemStatusCode = Nothing
        Else
            _itemStore.ItemStatusCode = Integer.Parse(cmbItemStatusCode.SelectedItem)
        End If

        If Not ComboBox_SubTeam.SelectedItem Is Nothing Then
            ' Compare the selected exception subteam to the item subteam.  If they are the same value,
            ' the exception subteam should be set to NULL.
            If ComboBox_SubTeam.SelectedValue = _itemStore.ItemSubTeam Or ComboBox_SubTeam.SelectedValue = -1 Then
                _itemStore.StoreSubTeam = -1
            Else
                _itemStore.StoreSubTeam = ComboBox_SubTeam.SelectedValue
            End If
        Else
            ' The user is not required to select a subteam.  There is an option for "--No Exception--".
            MsgBox(ResourcesIRMA.GetString("SelectSubTeam"), MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
            '_itemStore.StoreSubTeam = -1
        End If

        If ComboBox_AgeCode.SelectedIndex = -1 Or _
            ComboBox_AgeCode.SelectedValue = 0 Then
            _itemStore.AgeCode = Nothing
        Else
            _itemStore.AgeCode = ComboBox_AgeCode.SelectedIndex
        End If

        'ensure that POS Tare is a valid numeric value
        If ScaleBO.ValidateNumericValue(TextBox_POSTare.Text) Then
            _itemStore.POS_Tare = CInt(TextBox_POSTare.Text).ToString

            'validate that value is >= 0
            If ScaleBO.ValidatePositiveNumber(_itemStore.POS_Tare, True) = False Then
                errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Label_PosTare.Text))
                errorMsg.Append(Environment.NewLine)
            Else
                'warn user if they enter decimal values
                If TextBox_POSTare.Text.IndexOf(".") >= 0 Then
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_decimalRounding"), Label_PosTare.Text))
                    errorMsg.Append(Environment.NewLine)
                End If
            End If
        ElseIf TextBox_POSTare.Text IsNot Nothing AndAlso Not TextBox_POSTare.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Label_PosTare.Text))
            errorMsg.Append(Environment.NewLine)
        Else
            _itemStore.POS_Tare = Nothing
        End If

        'ensure that Mix Match is a valid numeric value
        If ScaleBO.ValidateNumericValue(TextBox_MixMatch.Text) Then
            _itemStore.MixMatch = CInt(TextBox_MixMatch.Text).ToString

            'validate that value is >= 0
            If ScaleBO.ValidatePositiveNumber(_itemStore.MixMatch, True) = False Then
                errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Label_MixMatch.Text))
                errorMsg.Append(Environment.NewLine)
            Else
                'warn user if they enter decimal values
                If TextBox_MixMatch.Text.IndexOf(".") >= 0 Then
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_decimalRounding"), Label_MixMatch.Text))
                    errorMsg.Append(Environment.NewLine)
                End If
            End If
        ElseIf TextBox_MixMatch.Text IsNot Nothing AndAlso Not TextBox_MixMatch.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Label_MixMatch.Text))
            errorMsg.Append(Environment.NewLine)
        Else
            _itemStore.MixMatch = Nothing
        End If

        'ensure that Item Surcharge is a valid integer value
        If IsNumeric(TextBox_ItemSurcharge.Text) Then
            _itemStore.ItemSurcharge = TextBox_ItemSurcharge.Text

            'return an error if the value is not greater than 0
            If _itemStore.ItemSurcharge < 0 Then
                errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_nonNegativeInteger"), Label_ItemSurcharge.Text))
                errorMsg.Append(Environment.NewLine)
            Else
                'warn user if they enter decimal values
                If TextBox_ItemSurcharge.Text.IndexOf(".") >= 0 Then
                    errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_decimalRounding"), Label_ItemSurcharge.Text))
                    errorMsg.Append(Environment.NewLine)
                End If

            End If
        ElseIf TextBox_ItemSurcharge.Text IsNot Nothing AndAlso Not TextBox_ItemSurcharge.Text.Trim.Equals("") Then
            errorMsg.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Label_ItemSurcharge.Text))
            errorMsg.Append(Environment.NewLine)
        Else
            _itemStore.ItemSurcharge = Nothing
        End If

        If cmbRetailUom.SelectedIndex > -1 Then
            _itemStore.RetailUomId = cmbRetailUom.SelectedValue
        End If

        If cmbScaleUom.SelectedIndex > -1 Then
            _itemStore.ScaleUomId = cmbScaleUom.SelectedValue
        End If

        _itemStore.FixedWeight = TextBox_FixedWeight.Text.Trim()
        If (IsDBNull(ByCountNumericEditor.Value) = False) Then
            _itemStore.ByCount = ByCountNumericEditor.Value
        End If

        If cmbScaleUom.SelectedIndex > -1 Then
            If TextBox_FixedWeight.Enabled = True And TextBox_FixedWeight.Text.Length = 0 Then
                'prompt user that this is required
                errorMsg.Append("Fixed Weight value is required when Scale UOM is 'FIXED WEIGHT'.")
            ElseIf ByCountNumericEditor.Enabled = True Then
                If Not ByCountNumericEditor.Value Is Nothing Then
                    If ByCountNumericEditor.Value.ToString().Length = 0 Then
                        'prompt user that this is required
                        errorMsg.Append("By Count value is required when Scale UOM is 'BY COUNT'.")
                    End If
                Else
                    'prompt user that this is required
                    errorMsg.Append("By Count value is required when Scale UOM is 'BY COUNT'.")
                End If
            End If
        End If

        If errorMsg.Length <= 0 Then
            ' confirm the user wants to apply the changes
            If MsgBox(ResourcesIRMA.GetString("SendChanges"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                Exit Sub
            End If

            ' save the data to the database for each of the selected stores
            For iLoop = 0 To ugrdStoreList.Selected.Rows.Count - 1
                _itemStore.StoreId = ugrdStoreList.Selected.Rows(iLoop).Cells("Store_No").Value

                'validate other data (required fields, etc)
                statusList = _itemStore.ValidateData()

                'loop through possible business rule errors before the save
                statusEnum = statusList.GetEnumerator
                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, ItemStoreStatus)

                    Select Case currentStatus
                        Case ItemStoreStatus.Error_VendorRequiredForAuth
                            ' The authorization for this item-store is not valid; add it to an error msg for the user
                            authStoreList.Add(ugrdStoreList.Selected.Rows(iLoop).Cells("Store_Name").Value)
                        Case ItemStoreStatus.Error_PendingPriceChangePreventsDeAuth
                            ' The authorization for this item-store is not valid; add it to an error msg for the user
                            deAuthStoreList.Add(ugrdStoreList.Selected.Rows(iLoop).Cells("Store_Name").Value)
                        Case ItemStoreStatus.Valid
                            ' Save the updates for this item-store
                            ItemDAO.UpdateItemStoreData(_itemStore)
                            ItemDAO.UpdateDiscontinue(_itemStore)
                    End Select
                End While

                If retailUomOverrideEnabled Or scaleUomOverrideEnabled Then
                    ItemDAO.UpdateItemUomOverrideData(_itemStore)
                End If
            Next iLoop

            ' Display an error msg to the user if any of the selected stores were unable to be saved
            ' based on the authorization flags
            If authStoreList.Count > 0 Then
                Dim storeList As New StringBuilder
                For Each store As String In authStoreList
                    If (storeList.Length > 0) Then
                        storeList.Append(", ")
                    End If
                    storeList.Append(store)
                Next
                MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_error_NoVendorForAuthStore"), Environment.NewLine, storeList.ToString), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If deAuthStoreList.Count > 0 Then
                Dim storeList As New StringBuilder
                For Each store As String In deAuthStoreList
                    If (storeList.Length > 0) Then
                        storeList.Append(", ")
                    End If
                    storeList.Append(store)
                Next
                MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_error_PriceChgPreventsDeAuth"), Environment.NewLine, storeList.ToString), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Else
            'display error msg
            MessageBox.Show(errorMsg.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Me.Close()
    End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged

        If IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)
            Dim iLoop As Short

            Call SetCombos()

            If mbNoClick = True Then Exit Sub

            ' Rick begin
            mbFilling = True
            ' Rick end

            'ugrdStoreList.Selected.Rows.Clear()

            Select Case Index
                Case 0
                    '-- Manual.
                    If Not NoManualButtonClick Then
                        ugrdStoreList.Selected.Rows.Clear()
                    End If
                    cmbZones.SelectedIndex = -1
                    cmbStates.SelectedIndex = -1

                Case 1
                    '-- All Stores or All 365 for RM
                    If CheckAllStoreSelectionEnabled() Then
                        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
                            'Select all rows.
                            ugrdStoreList.Selected.Rows.Add(ugrdStoreList.Rows(iLoop))
                        Next iLoop
                    Else
                        ugrdStoreList.Selected.Rows.Clear()
                        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
                            If ugrdStoreList.Rows(iLoop).Cells("Mega_Store").Value = True Then
                                ugrdStoreList.Selected.Rows.Add(ugrdStoreList.Rows(iLoop))
                            End If
                        Next iLoop
                    End If

                Case 2
                    '-- By Zones.

                    ' Rick begin
                    If cmbZones.SelectedIndex > -1 Then
                        Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
                    End If

                    'ugrdStoreList.Selected.Rows.Clear()
                    'If cmbZones.SelectedIndex > -1 Then
                    '    For iLoop = 0 To ugrdStoreList.Rows.Count - 1
                    '        If cmbZones.SelectedIndex > -1 Then
                    '            If CDbl(ugrdStoreList.Rows(iLoop).Cells("Zone_ID").Value) = VB6.GetItemData(cmbZones, cmbZones.SelectedIndex) Then
                    '                ugrdStoreList.Selected.Rows.Add(ugrdStoreList.Rows(iLoop))
                    '            End If
                    '        End If
                    '    Next iLoop
                    'End If
                    ' Rick end

                Case 3
                    '-- By State.
                    ' Rick begin
                    'Call SelectStates()
                    If cmbStates.SelectedIndex > -1 Then
                        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))
                    End If
                    ' Rick end

                Case 4
                    '-- All WFM.
                    ugrdStoreList.Selected.Rows.Clear()
                    For iLoop = 0 To ugrdStoreList.Rows.Count - 1
                        If ugrdStoreList.Rows(iLoop).Cells("WFM_Store").Value = True Then
                            ugrdStoreList.Selected.Rows.Add(ugrdStoreList.Rows(iLoop))
                        End If
                    Next iLoop

            End Select
            ' Rick begin
            mbFilling = False
            ' Rick end
        End If

    End Sub

    Private Sub SelectStates()

        Dim iLoop As Short

        ugrdStoreList.Selected.Rows.Clear()
        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
            If cmbStates.Text = "" Then Exit For
            ' Check to make sure the State was set in the db for this store
            If (ugrdStoreList.Rows(iLoop).Cells("State").Value IsNot DBNull.Value) Then
                ' Select the store if it belongs to the State
                If ugrdStoreList.Rows(iLoop).Cells("State").Value = cmbStates.Text Then
                    ugrdStoreList.Selected.Rows.Add(ugrdStoreList.Rows(iLoop))
                End If
            End If
        Next iLoop

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

        If mbFilling Or IsInitializing Then Exit Sub

        ' Rick begin
        mbFilling = True
        Call SelectStates()

        mbFilling = False
        ' Rick end

    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub
        optSelection(2).Checked = True
        OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

    End Sub

    Private Sub LoadStates()
        Dim iLoop As Short
        For iLoop = 0 To ugrdStoreList.Rows.Count - 1
            ' Check to make sure the State was set in the db for this store
            If (ugrdStoreList.Rows(iLoop).Cells("State").Value IsNot DBNull.Value) Then
                ' Add the State to the list if it's not already there
                If Not StateInList((ugrdStoreList.Rows(iLoop).Cells("State").Value)) And Trim(ugrdStoreList.Rows(iLoop).Cells("State").Text) <> "" Then
                    cmbStates.Items.Add(Trim(ugrdStoreList.Rows(iLoop).Cells("State").Text))
                End If
            End If
        Next iLoop
    End Sub

    Private Sub LoadAgeCode()
        Dim AgeCodeList As New DataTable

        With AgeCodeList.Columns
            .Add("AgeCodeListID", GetType(Integer))
            .Add("AgeCode", GetType(String))
        End With

        AgeCodeList.Rows.Add(0, "")
        AgeCodeList.Rows.Add(1, "18")
        AgeCodeList.Rows.Add(2, "21")

        Me.ComboBox_AgeCode.DataSource = AgeCodeList
        Me.ComboBox_AgeCode.ValueMember = "AgeCodeListID"
        Me.ComboBox_AgeCode.DisplayMember = "AgeCode"

    End Sub

    Private Function StateInList(ByRef strState As String) As Boolean
        Dim i As Short
        StateInList = False
        For i = 0 To cmbStates.Items.Count - 1
            If VB6.GetItemString(cmbStates, i) = strState Then
                StateInList = True
                Exit For
            End If
        Next i
    End Function

    Private Sub ugrdStoreList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdStoreList.AfterSelectChange
        ' Fix for bug 5607 (Rick Kelleher) begin
        RefreshStoreInfo()
        ' Fix for bug 5607 (Rick Kelleher) end

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True
        NoManualButtonClick = True
        optSelection(0).Checked = True
        mbFilling = False
        NoManualButtonClick = False

        'Call UpdateGridPrices()
    End Sub

    Private Sub ugrdStoreList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.Click
        ' Fix for bug 5607 (Rick Kelleher) begin
        RefreshStoreInfo()
        ' Fix for bug 5607 (Rick Kelleher) end
    End Sub

    Private Sub RefreshStoreInfo()
        ' Refresh the store info on the right side of the screen
        Dim row As UltraGridRow
        Dim storeID As Integer

        mbNoClick = True
        ' Fix for bug 5607 (Rick Kelleher) begin
        'Me.optSelection(0).Checked = True
        ' Fix for bug 5607 (Rick Kelleher) end

        retailUomOverrideEnabled = True
        scaleUomOverrideEnabled = True

        ' the selected store has changed - updates the POS data
        If (ugrdStoreList.Selected.Rows.Count > 0) Then
            glStoreID = ugrdStoreList.Selected.Rows(0).Cells("Store_No").Value

            For Each row In ugrdStoreList.Selected.Rows
                storeID = row.Cells("Store_No").Value
                If InstanceDataDAO.IsFlagActive("EnableRetailUOMbyStore", storeID) And retailUomOverrideEnabled Then
                    retailUomOverrideEnabled = True
                Else
                    retailUomOverrideEnabled = False
                End If

                If InstanceDataDAO.IsFlagActive("EnableScaleUOMbyStore", storeID) And scaleUomOverrideEnabled Then
                    scaleUomOverrideEnabled = True
                Else
                    scaleUomOverrideEnabled = False
                End If

                If Not retailUomOverrideEnabled And Not scaleUomOverrideEnabled Then
                    Exit For
                End If
            Next
            LoadInstanceAttributes()
        End If
        LoadOtherData()
        mbNoClick = False
    End Sub

    Private Sub Button_TaxOverride_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TaxOverride.Click
        'allow only 1 store to be selected before overriding tax value
        If ugrdStoreList.Selected.Rows.Count > 1 Then
            MessageBox.Show(ResourcesItemHosting.GetString("SelectOneStoreTaxOverride"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf ugrdStoreList.Selected.Rows.Count = 0 Then
            MessageBox.Show(ResourcesItemHosting.GetString("SelectStoreTaxOverride"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            'open tax override form for selected store/item combo
            Dim taxOverrideForm As New Form_ManageTaxOverride
            taxOverrideForm.StoreNo = ugrdStoreList.Selected.Rows(0).Cells("Store_No").Value
            taxOverrideForm.ItemKey = glItemID
            taxOverrideForm.ShowDialog()
            taxOverrideForm.Close()
            taxOverrideForm.Dispose()
        End If
    End Sub

    ''' <summary>
    ''' Open the Item Search screen to allow the user to search for a specific item to link to this POS item.
    ''' Examples for Link Code:  Linking an item to the associated bottle deposit.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddLink_ItemSearch.Click
        ' Create a new instance of the item search screen.
        itemSearchForm = New frmItemSearch
        itemSearchForm.ResetGlobalVars = False
        itemSearchForm.GetLinkCodeID = glItemID
        AddHandler itemSearchForm.ItemSelected, AddressOf HandleItemSelected

        itemSearchForm.ShowDialog(Me)

        RemoveHandler itemSearchForm.ItemSelected, AddressOf HandleItemSelected
        itemSearchForm.Close()
        itemSearchForm.Dispose()

        ' If the config keys EnablePLUIRMAIConFlow and EnableUPCIRMAToIConFlow are set to 1,
        ' only validated scancodes will be allowed to be linked to items. 
        If TextBox_LinkedItem.Text.Trim.Length > 0 And Not StoreItemAttributeDAO.IsItemValidated(Me.TextBox_LinkedItem.Text) Then
            MsgBox("Selected item is not validated. Please try again.", MsgBoxStyle.Exclamation)
            ' remove the link code value in the ItemStoreBO
            _itemStore.LinkedItemKey = Nothing
            _itemStore.LinkedIdentifier = Nothing
            ' update the UI
            TextBox_LinkedItem.Text = Nothing
        End If
    End Sub

    ''' <summary>
    ''' This is called when an item search is performed and an item is selected.
    ''' </summary>
    ''' <param name="itemSearch"></param>
    ''' <remarks></remarks>
    Private Sub HandleItemSelected(ByVal itemSearch As ItemSearchBO)
        ' the user selected an item for the Link Code

        ' update the ItemStoreBO
        If itemSearch IsNot Nothing Then
            _itemStore.LinkedIdentifier = itemSearch.ItemIdentifier
            _itemStore.LinkedItemKey = itemSearch.ItemKey
        Else
            _itemStore.LinkedIdentifier = Nothing
            _itemStore.LinkedItemKey = Nothing
        End If

        ' update the UI
        TextBox_LinkedItem.Text = _itemStore.LinkedIdentifier
    End Sub

    ''' <summary>
    ''' Clear the Link Code reference.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_DeleteLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DeleteLink.Click
        ' remove the link code value in the ItemStoreBO
        _itemStore.LinkedItemKey = Nothing
        _itemStore.LinkedIdentifier = Nothing
        ' update the UI
        TextBox_LinkedItem.Text = Nothing
    End Sub

    Private Sub Exempt_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Exempt_Button.Click
        ExemptCheckBox.Enabled = Not ExemptCheckBox.Enabled
        GroupBox_CommonPOS.Enabled = Not GroupBox_CommonPOS.Enabled
        GroupBox_SubTeam.Enabled = Not GroupBox_SubTeam.Enabled

        If ExemptCheckBox.Enabled Then
            If ugrdStoreList.Selected.Rows.Count > 1 Then
                ugrdStoreList.Selected.Rows.Clear()
            End If

            ugrdStoreList.DisplayLayout.Override.SelectTypeRow = SelectType.Single
            _optSelection_0.Checked = True
            _optSelection_1.Enabled = False
            _optSelection_2.Enabled = False
            _optSelection_3.Enabled = False
            _optSelection_4.Enabled = False
        Else
            ugrdStoreList.DisplayLayout.Override.SelectTypeRow = SelectType.Extended
            _optSelection_1.Enabled = True
            _optSelection_2.Enabled = True
            _optSelection_3.Enabled = True
            _optSelection_4.Enabled = True
        End If
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
            ugrdStoreList.Focus()
            CurrentKey = Nothing
        End If
    End Sub
    ' for bug 5442: end

    Private Sub SetECommerceFlag()
        If CBool(ConfigurationServices.AppSettings("ECommerce").ToString) Then
            CheckBox_ECommerce.Visible = True
        Else
            CheckBox_ECommerce.Visible = False
        End If
    End Sub
    Private Sub SetFieldsStates()
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            Me.ComboBox_SubTeam.Enabled = False
        Else
            Me.ComboBox_SubTeam.Enabled = True
        End If
    End Sub

#Region "Tabbing code"
    ' Commented out by Rick Kelleher 2/27/08 while fixing bug 5442

    'Private Sub cmbStates_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.Leave
    '    ugrdStoreList.Focus()
    'End Sub

    'Private Sub cmbStates_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStates.LostFocus
    '    ugrdStoreList.Focus()
    'End Sub

    'Private Sub cmbZones_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.Leave
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub cmbZones_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZones.LostFocus
    '    _optSelection_3.Focus()
    'End Sub

    'Private Sub _optSelection_0_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_0.LostFocus
    '    _optSelection_1.Focus()
    'End Sub

    'Private Sub _optSelection_1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.Leave
    '    _optSelection_4.Focus()
    'End Sub

    'Private Sub _optSelection_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_1.LostFocus
    '    _optSelection_4.Focus()
    'End Sub

    'Private Sub _optSelection_4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_4.Leave
    '    _optSelection_2.Focus()
    'End Sub

    'Private Sub _optSelection_4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_4.LostFocus
    '    _optSelection_2.Focus()
    'End Sub

    'Private Sub _optSelection_2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_2.Leave
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub _optSelection_2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_2.LostFocus
    '    cmbZones.Enabled = True
    '    cmbZones.Focus()
    'End Sub

    'Private Sub _optSelection_3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_3.Leave
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub _optSelection_3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _optSelection_3.LostFocus
    '    cmbStates.Enabled = True
    '    cmbStates.Focus()
    'End Sub

    'Private Sub ugrdStoreList_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdStoreList.LostFocus
    '    Exempt_Button.Focus()
    'End Sub

    'Private Sub Exempt_Button_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Exempt_Button.LostFocus
    '    ExemptGroupBox.Focus()
    'End Sub

    'Private Sub ExemptCheckBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExemptCheckBox.LostFocus
    '    Button_TaxOverride.Focus()
    'End Sub

    'Private Sub Button_TaxOverride_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_TaxOverride.LostFocus
    '    CheckBox_AuthorizedItem.Focus()
    'End Sub

    'Private Sub ComboBox_SubTeam_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_SubTeam.LostFocus
    '    CheckBox_CompetitiveItem.Focus()
    'End Sub

    'Private Sub TextBox_MixMatch_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_MixMatch.LostFocus
    '    TextBox_LinkedItem.Focus()
    'End Sub

    'Private Sub Button_DeleteLink_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_DeleteLink.LostFocus
    '    ComboBox_KitchenRouteID.Enabled = True
    '    ComboBox_KitchenRouteID.Focus()
    'End Sub

    'Private Sub ComboBox_KitchenRouteID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_KitchenRouteID.LostFocus
    '    Numeric_RoutingPriority.Enabled = True
    '    Numeric_RoutingPriority.Focus()
    'End Sub

    'Private Sub cmdExit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.LostFocus
    '    _optSelection_0.Focus()
    'End Sub

    'Private Sub _optSelection_3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_3.CheckedChanged

    'End Sub

    'Private Sub _optSelection_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_2.CheckedChanged

    'End Sub

    'Private Sub _optSelection_1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_1.CheckedChanged

    'End Sub

    'Private Sub _optSelection_4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_4.CheckedChanged

    'End Sub
#End Region

    Private Sub cmbScaleUom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbScaleUom.SelectedIndexChanged
        If cmbScaleUom.SelectedIndex > 0 Then
            If cmbScaleUom.GetItemText(cmbScaleUom.SelectedItem).ToUpper().StartsWith("BY COUNT") Then
                ByCountNumericEditor.Enabled = True
                TextBox_FixedWeight.Text = String.Empty
                TextBox_FixedWeight.Enabled = False
            ElseIf cmbScaleUom.GetItemText(cmbScaleUom.SelectedItem).ToUpper().StartsWith("FIXED WEIGHT") Then
                ByCountNumericEditor.Value = Nothing
                ByCountNumericEditor.Enabled = False
                TextBox_FixedWeight.Enabled = True
            Else
                ByCountNumericEditor.Value = Nothing
                ByCountNumericEditor.Enabled = False
                TextBox_FixedWeight.Text = String.Empty
                TextBox_FixedWeight.Enabled = False
            End If
        Else
            TextBox_FixedWeight.Clear()
            ByCountNumericEditor.Value = Nothing
            TextBox_FixedWeight.Enabled = False
            ByCountNumericEditor.Enabled = False
        End If
    End Sub

    Private Sub ByCountNumericEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.Click
        ByCountNumericEditor.SelectAll()
    End Sub

    Private Sub ByCountNumericEditor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ByCountNumericEditor.Enter
        ByCountNumericEditor.SelectAll()
    End Sub
End Class

