Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Linq
Imports Infragistics.Win.UltraWinGrid

Friend Class frmItemSearch
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

    Public plLimitSubTeam_No As Integer
    Public pbOrderFromDistribution As Boolean
    Private m_oSelectedItems As colSelectedItems
    Private m_iIncludeDeltedItems As enumChkBoxValues
    Private m_bIsPreOrderItem As Boolean
    Private m_bIsEXEDistributed As Boolean
    Private m_sItemIdentifier As String = ""
    Private m_sVendorName As String
    Private m_lLimitSubTeam_No As Integer
    Private _resetGlobalVars As Boolean = True
    Private _getLinkCodeID As Integer = 0
    Private mdt As DataTable
    Private _paramList As DBParamList = Nothing
    Private _factory As DataFactory = Nothing

    'Event fired when an Item has been selected.
    Public Event ItemSelected(ByVal itemSearch As ItemSearchBO)

    Private Sub frmItemSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Call InitForm()

        m_oSelectedItems = New colSelectedItems
        m_oSelectedItems.DataSource = ugrdSearchResults

        ' Prepare the jurisdiction dropdown box.
        Dim storeJurisdictionDAO As New StoreJurisdictionDAO
        UltraComboJurisdiction.DataSource = storeJurisdictionDAO.GetJurisdictionList()
        UltraComboJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        UltraComboJurisdiction.ValueMember = "StoreJurisdictionID"

        SetupJurisdictionUltraCombo(UltraComboJurisdiction)

        Call SetupDataTable()

        'TFS 2371 - Faisal Ahmed, 08/20/2011
        If gbQuickSearch Then cmdSearch.PerformClick()

    End Sub

    Public Sub InitForm()

        'Clear the results fields (These are kept for backward compatablility and should be removed when we stop using globals to pass values)
        If _resetGlobalVars Then
            glItemID = 0
            gsItemDescription = ""
            glItemSubTeam = 0
        End If

        Me.HierarchySelector1.SetAddHierarchLevelsActive(False)

        Call LoadDistSubTeam((cmbDistSubTeam))
        Call LoadBrandNew((cmbBrand))

        'TFS 2371 - Faisal Ahmed, 08/20/2011
        If gbQuickSearch = True Then
            Me.txtField(1).Text = glIdentifier
            Me.chkDiscontinued.Checked = True
            Me.chkIncludeDeletedItems.Checked = True
            Me.chkWFMItems.Checked = True
            Me.chkHFM.Checked = True
            Exit Sub
        End If

        Me.txtField(4).Text = GetSetting("IRMA", "ItemSearch", "VendorName")

        If Len(Me.txtField(4).Text) > 0 Then
            If Me.txtField(5).BackColor <> COLOR_INACTIVE Then Me.txtField(5).Text = GetSetting("IRMA", "ItemSearch", "VendorID")
        Else
            SetActive(Me.txtField(5), False)
        End If

        Me.txtField(0).Text = GetSetting("IRMA", "ItemSearch", "ItemDesc")
        Me.txtField(1).Text = GetSetting("IRMA", "ItemSearch", "Identifier")

        Call SetComboNew((cmbBrand), Val(GetSetting("IRMA", "ItemSearch", "BrandID")))
        Call SetCombo((cmbDistSubTeam), Val(GetSetting("IRMA", "ItemSearch", "DistSubTeam_No")))

        If ComboVal(Me.HierarchySelector1.cmbSubTeam) = 0 Then Call SetCombo((Me.HierarchySelector1.cmbSubTeam), Val(GetSetting("IRMA", "ItemSearch", "SubTeam")))
        If Me.HierarchySelector1.cmbCategory.Enabled Then Call SetCombo((Me.HierarchySelector1.cmbCategory), Val(GetSetting("IRMA", "ItemSearch", "Category", "-1")))
        If Me.HierarchySelector1.cmbLevel3.Enabled Then Call SetCombo((Me.HierarchySelector1.cmbLevel3), Val(GetSetting("IRMA", "ItemSearch", "Level3", "-1")))
        If Me.HierarchySelector1.cmbLevel4.Enabled Then Call SetCombo((Me.HierarchySelector1.cmbLevel4), Val(GetSetting("IRMA", "ItemSearch", "Level4", "-1")))

        If Val(GetSetting("IRMA", "ItemSearch", "Discontinued")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.IncludeDiscontinued = Global_Renamed.enumChkBoxValues.UncheckedEnabled
        Else
            Me.IncludeDiscontinued = Global_Renamed.enumChkBoxValues.CheckedEnabled
        End If

        If Val(GetSetting("IRMA", "ItemSearch", "NotAvailable")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.ExcludeNotAvailable = Global_Renamed.enumChkBoxValues.UncheckedEnabled
        Else
            Me.ExcludeNotAvailable = Global_Renamed.enumChkBoxValues.CheckedEnabled
        End If

        If Val(GetSetting("IRMA", "ItemSearch", "WFM")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.SoldAtWFM = Global_Renamed.enumChkBoxValues.UncheckedEnabled
        Else
            Me.SoldAtWFM = Global_Renamed.enumChkBoxValues.CheckedEnabled
        End If

        If Val(GetSetting("IRMA", "ItemSearch", "HFM")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.SoldAtHFM = Global_Renamed.enumChkBoxValues.UncheckedEnabled
        Else
            Me.SoldAtHFM = Global_Renamed.enumChkBoxValues.CheckedEnabled
        End If

        If Val(GetSetting("IRMA", "ItemSearch", "IncludeDeletedItems")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Unchecked
        Else
            Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Checked
        End If

        If Val(GetSetting("IRMA", "ItemSearch", "DefaultIdentifiers")) = System.Windows.Forms.CheckState.Unchecked Then
            Me.chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Unchecked
        Else
            Me.chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Checked
        End If

        'TFS 2371 - Faisal Ahmed, 08/20/2011
        If gbQuickSearch = True Then
            Me.txtField(1).Text = glIdentifier
            Me.chkDiscontinued.Checked = True
            Me.chkIncludeDeletedItems.Checked = True
            Me.chkWFMItems.Checked = True
            Me.chkHFM.Checked = True
        End If

    End Sub

    Public ReadOnly Property IsEXEDistributed() As Boolean
        Get
            IsEXEDistributed = m_bIsEXEDistributed
        End Get
    End Property

    Public ReadOnly Property ItemIdentifier() As String
        Get
            ItemIdentifier = m_sItemIdentifier
        End Get
    End Property

    Public ReadOnly Property IsPreOrderItem() As Boolean
        Get
            IsPreOrderItem = m_bIsPreOrderItem
        End Get
    End Property

    Public ReadOnly Property SelectedItems() As colSelectedItems
        Get
            SelectedItems = m_oSelectedItems
        End Get
    End Property

    Public WriteOnly Property MultiSelect() As Boolean
        Set(ByVal Value As Boolean)
            ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        End Set
    End Property

    Public WriteOnly Property LimitSubTeam_No() As Integer
        Set(ByVal Value As Integer)
            m_lLimitSubTeam_No = Value

            Me.HierarchySelector1.Initialize()

            If Value > 0 Then
                SetCombo(Me.HierarchySelector1.cmbSubTeam, Value)
                Me.HierarchySelector1.cmbSubTeam.Enabled = False
            Else
                Me.HierarchySelector1.cmbSubTeam.SelectedIndex = -1
                Me.HierarchySelector1.cmbSubTeam.Enabled = True
            End If
        End Set
    End Property

    Public WriteOnly Property SubTeam_No() As Integer
        Set(ByVal Value As Integer)
            Call SetCombo((Me.HierarchySelector1.cmbSubTeam), Value)
            SetActive((Me.HierarchySelector1.cmbSubTeam), True)
        End Set
    End Property

    Public Property IncludeDeletedItems() As enumChkBoxValues
        Get
            IncludeDeletedItems = m_iIncludeDeltedItems
        End Get

        Set(ByVal Value As enumChkBoxValues)
            m_iIncludeDeltedItems = Value

            Select Case Value
                Case Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Unchecked
                    Me.chkIncludeDeletedItems.Enabled = False
                Case Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Unchecked
                    Me.chkIncludeDeletedItems.Enabled = True
                Case Global_Renamed.enumChkBoxValues.CheckedDisabled
                    Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Checked
                    Me.chkIncludeDeletedItems.Enabled = False
                Case Global_Renamed.enumChkBoxValues.CheckedEnabled
                    Me.chkIncludeDeletedItems.CheckState = System.Windows.Forms.CheckState.Checked
                    Me.chkIncludeDeletedItems.Enabled = True
            End Select
        End Set
    End Property

    Public WriteOnly Property VendorName() As String
        Set(ByVal Value As String)
            If Trim(Value) <> "" Then
                txtField(4).Text = Trim(Value)

                'don't allow the user to select items from a different vendor
                SetActive(txtField(4), False)

                'allow user to select search by Vendor's Item ID
                SetActive(txtField(5), True)
            End If
        End Set
    End Property

    Public WriteOnly Property VendorDataEnabled() As Boolean
        Set(ByVal Value As Boolean)
            'field 4 is VendorName.  If the vendor name contains data, the VendorName prop has already disabled the fields
            If txtField(4).Text = "" Then
                SetActive(txtField(4), Value)
                SetActive(txtField(5), Value)
            End If
        End Set
    End Property

    Public WriteOnly Property IncludeDiscontinued() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDiscontinued.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDiscontinued.Enabled = True
                Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = True
            End Select
        End Set
    End Property

    Public WriteOnly Property SoldAtHFM() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    chkHFM.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkHFM.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    chkHFM.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkHFM.Enabled = True
                Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
                    chkHFM.CheckState = System.Windows.Forms.CheckState.Checked
                    chkHFM.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
                    chkHFM.CheckState = System.Windows.Forms.CheckState.Checked
                    chkHFM.Enabled = True
            End Select
        End Set
    End Property

    Public WriteOnly Property SoldAtWFM() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    chkWFMItems.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkWFMItems.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    chkWFMItems.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkWFMItems.Enabled = True
                Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
                    chkWFMItems.CheckState = System.Windows.Forms.CheckState.Checked
                    chkWFMItems.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
                    chkWFMItems.CheckState = System.Windows.Forms.CheckState.Checked
                    chkWFMItems.Enabled = True
            End Select
        End Set
    End Property

    Public WriteOnly Property ExcludeNotAvailable() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    chkNotAvailable.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkNotAvailable.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    chkNotAvailable.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkNotAvailable.Enabled = True
                Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
                    chkNotAvailable.CheckState = System.Windows.Forms.CheckState.Checked
                    chkNotAvailable.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
                    chkNotAvailable.CheckState = System.Windows.Forms.CheckState.Checked
                    chkNotAvailable.Enabled = True
            End Select
        End Set
    End Property

    Public WriteOnly Property DefaultIdentifiers() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedDisabled
                    chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDefaultIdentifiers.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.UncheckedEnabled
                    chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDefaultIdentifiers.Enabled = True
                Case Value = Global_Renamed.enumChkBoxValues.CheckedDisabled
                    chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDefaultIdentifiers.Enabled = False
                Case Value = Global_Renamed.enumChkBoxValues.CheckedEnabled
                    chkDefaultIdentifiers.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDefaultIdentifiers.Enabled = True
            End Select
        End Set
    End Property

    ''' <summary>
    ''' By default, the item search screen will update the values for global variables.
    ''' If this flag is set to false, the data is returned to the user with the ItemSelected event and the
    ''' global variables are not updated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResetGlobalVars() As Boolean
        Get
            Return _resetGlobalVars
        End Get
        Set(ByVal value As Boolean)
            _resetGlobalVars = value
        End Set
    End Property

    ''' <summary>
    ''' When searching for Link Code Items, the Item related to the Link Code should not be available
    ''' If this value is populated, query will pass in the id and not include it with the return set.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GetLinkCodeID() As Integer
        Get
            Return _getLinkCodeID
        End Get
        Set(ByVal value As Integer)
            _getLinkCodeID = value
        End Set
    End Property

    Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = Keys.Back Then cmbBrand.SelectedIndex = -1
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = Keys.None Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbDistSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbDistSubTeam.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = Keys.Back Then cmbDistSubTeam.SelectedIndex = -1
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = Keys.None Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        ugrdSearchResults.Selected.Rows.Clear()
        Me.Hide()
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        RunSearch()
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        ReturnSelection()
    End Sub

    Private Sub frmItemSearch_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        If Not txtField(1).Focus Then
            txtField(1).Focus()
        End If
        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToShortTimeString() & " frmItemSearch_Activatied")
    End Sub

    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        If Me.IsInitializing Then Exit Sub

        Dim Index As Short = txtField.GetIndex(eventSender)

        If Index = 4 Then
            If Len(Trim(txtField(4).Text)) > 0 Then
                SetActive(txtField(5), True)
            Else
                txtField(5).Text = ""
                SetActive(txtField(5), False)
            End If
        End If
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)

    If txtField(Index).ReadOnly Then GoTo EventExitSub

    KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

EventExitSub:
    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = Keys.None Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub InitializeDataFactory()

        Try
            If _factory Is Nothing Then
                _factory = New DataFactory(DataFactory.ItemCatalog)
            End If

        Catch ex As Exception
            ProcessException(ex)

        End Try

    End Sub

    Private Sub InitializeParameters()

        Try
            If _paramList Is Nothing Then
                _paramList = New DBParamList()

                'add parameters
                With _paramList
                    .Add(New DBParam("SubTeam_No", DBParamType.Int))
                    .Add(New DBParam("Category_ID", DBParamType.Int))
                    .Add(New DBParam("Vendor", DBParamType.String))
                    .Add(New DBParam("Vendor_ID", DBParamType.String))
                    .Add(New DBParam("Item_Description", DBParamType.String))
                    .Add(New DBParam("Identifier", DBParamType.String))
                    .Add(New DBParam("Discontinue_Item", DBParamType.Bit))
                    .Add(New DBParam("WFM_Item", DBParamType.Bit))
                    .Add(New DBParam("Not_Available", DBParamType.Bit))
                    .Add(New DBParam("HFM_Item", DBParamType.Bit))
                    .Add(New DBParam("IncludeDeletedItems", DBParamType.Bit))
                    .Add(New DBParam("Brand_ID", DBParamType.Int))
                    .Add(New DBParam("DistSubTeam_No", DBParamType.Int))
                    .Add(New DBParam("LinkCodeID", DBParamType.Int))
                    .Add(New DBParam("Jurisdictions", DBParamType.String))
                    .Add(New DBParam("JurisdictionSeparator", DBParamType.String))
                    .Add(New DBParam("ProdHierarchyLevel3_ID", DBParamType.Int))
                    .Add(New DBParam("ProdHierarchyLevel4_ID", DBParamType.Int))
                    .Add(New DBParam("DefaultIdentifiers", DBParamType.Bit))
                    .Add(New DBParam("Vendor_Item_Description", DBParamType.String))
                    .Add(New DBParam("POS_Description", DBParamType.String))
                End With
            Else
                'clear the parameter values
                _paramList.ClearValues()
            End If

        Catch ex As Exception
            ProcessException(ex)

        End Try

    End Sub

    Public Sub RunSearch()

        Dim drSearch As SqlClient.SqlDataReader = Nothing
        Dim lSubTeam_No As Integer
        Dim iCategory_ID As Integer
        Dim iProdHierarchLevel3_ID As Integer
        Dim iProdHierarchLevel4_ID As Integer
        Dim sVendor As String
        Dim sVendorItemID As String
        Dim iBrand_ID As Integer
        Dim sIdentifier As String
        Dim iDistSubTeam_No As Integer
        Dim sItem_Description As String
        Dim sPOSDesc As String
        Dim sVendorItemDesc As String
        Dim jurisdictions As String

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        '-- Do SubTeam search
        lSubTeam_No = ComboVal(HierarchySelector1.cmbSubTeam)
        iCategory_ID = ComboVal(HierarchySelector1.cmbCategory)
        iProdHierarchLevel3_ID = ComboVal(HierarchySelector1.cmbLevel3)
        iProdHierarchLevel4_ID = ComboVal(HierarchySelector1.cmbLevel4)

        '-- Do DistSubTeam search
        iDistSubTeam_No = ComboVal(cmbDistSubTeam)

        '-- Make the where string for Identifier
        sItem_Description = ConvertQuotes(Trim(txtField(0).Text))
        sIdentifier = Trim(txtField(1).Text)
        sVendor = ConvertQuotes(Trim(txtField(4).Text))
        sVendorItemID = Trim(txtField(5).Text)
        'iBrand_ID = ComboVal(cmbBrand)
        iBrand_ID = cmbBrand.SelectedValue
        sPOSDesc = Trim(txtPOSDesc.Text)
        sVendorItemDesc = Trim(txtVendorDesc.Text)
        jurisdictions = ComboValue(UltraComboJurisdiction)

        '-- Check that the user chose at least one jurisdiction.
        If jurisdictions = "NULL" Then
            UltraComboJurisdiction.Rows.Item(0).Cells.Item("Selected").Value = True
            jurisdictions = ComboValue(UltraComboJurisdiction)
        End If

        Try
            Call InitializeParameters()
            Call InitializeDataFactory()

            'set parameter values
            With _paramList
                .Item("SubTeam_No").Value = IIf(lSubTeam_No = 0, System.DBNull.Value, lSubTeam_No)
                .Item("Category_ID").Value = IIf(iCategory_ID = 0, System.DBNull.Value, iCategory_ID)
                .Item("Vendor").Value = sVendor
                .Item("Vendor_ID").Value = sVendorItemID
                .Item("Item_Description").Value = sItem_Description
                .Item("Identifier").Value = sIdentifier
                .Item("Discontinue_Item").Value = System.Math.Abs(chkDiscontinued.CheckState)
                .Item("WFM_Item").Value = System.Math.Abs(chkWFMItems.CheckState)
                .Item("Not_Available").Value = System.Math.Abs(chkNotAvailable.CheckState)
                .Item("HFM_Item").Value = System.Math.Abs(chkHFM.CheckState)
                .Item("IncludeDeletedItems").Value = System.Math.Abs(chkIncludeDeletedItems.CheckState)
                .Item("Brand_ID").Value = IIf(iBrand_ID = 0, System.DBNull.Value, iBrand_ID)
                .Item("DistSubTeam_No").Value = IIf(iDistSubTeam_No = -1, System.DBNull.Value, iDistSubTeam_No)
                .Item("LinkCodeID").Value = _getLinkCodeID
                .Item("Jurisdictions").Value = ComboValue(UltraComboJurisdiction)
                .Item("JurisdictionSeparator").Value = "|"
                .Item("ProdHierarchyLevel3_ID").Value = iProdHierarchLevel3_ID
                .Item("ProdHierarchyLevel4_ID").Value = iProdHierarchLevel4_ID
                .Item("DefaultIdentifiers").Value = System.Math.Abs(chkDefaultIdentifiers.CheckState)
                .Item("Vendor_Item_Description").Value = sVendorItemDesc
                .Item("POS_Description").Value = sPOSDesc
            End With

            drSearch = _factory.GetStoredProcedureDataReader("GetItemSearch", _paramList.CopyToArrayList)

            LoadDataTable(drSearch)

        Catch ex As Exception
            ProcessException(ex)

        Finally
            If drSearch IsNot Nothing Then
                If Not drSearch.IsClosed Then
                    drSearch.Close()
                End If
                drSearch = Nothing
            End If

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("SearchItems")

        With mdt.Columns
            'Visible on grid.
            '--------------------
            .Add(New DataColumn("Description", GetType(String)))
            .Add(New DataColumn("Identifier", GetType(String)))
            .Add(New DataColumn("Pack", GetType(Integer)))
            .Add(New DataColumn("Size", GetType(Decimal)))
            .Add(New DataColumn("UOM", GetType(String)))

            'TFS 2642, Faisal Ahmed, 08/17/2011 - Adding three new columns
            .Add(New DataColumn("Default", GetType(String)))
            .Add(New DataColumn("Deleted", GetType(String)))
            .Add(New DataColumn("Discont", GetType(String)))

            'Hidden.
            '--------------------
            .Add(New DataColumn("Item_Key", GetType(Integer)))
            .Add(New DataColumn("SubTeam_No", GetType(String)))
            .Add(New DataColumn("Pre_Order", GetType(Integer)))
            .Add(New DataColumn("EXEDistributed", GetType(Boolean)))
            .Add(New DataColumn("Brand", GetType(String)))
        End With

    End Sub

    Private Sub LoadDataTable(ByRef SQLDataReader As SqlClient.SqlDataReader)

        Dim oDataView As DataView
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000

        Try
            'Load the data set.
            mdt.Rows.Clear()

            With SQLDataReader
                Do While .Read
                    iLoop = iLoop + 1

                    If iLoop > MaxLoop Then
                        Exit Do
                    End If

                    row = mdt.NewRow
                    row("Brand") = .Item("Brand")
                    row("Item_Key") = .Item("Item_Key")
                    row("Description") = .Item("Item_Description")

                    If Not .Item("Identifier") Is DBNull.Value Then
                        row("Identifier") = .Item("Identifier")
                    Else
                        row("Identifier") = ""
                    End If

                    row("Pack") = .Item("Package_Desc1")
                    row("Size") = String.Format("{0:0.0}", .Item("Package_Desc2"))
                    row("UOM") = .Item("Unit_Abbreviation")

                    'TFS 2642, Faisal Ahmed, 08/17/2011
                    If .Item("Default") = 1 Then
                        row("Default") = "X"
                    Else
                        row("Default") = ""
                    End If

                    If .Item("Deleted") = 1 Then
                        row("Deleted") = "X"
                    Else
                        row("Deleted") = ""
                    End If

                    If .Item("Discont") = 1 Then
                        row("Discont") = "X"
                    Else
                        row("Discont") = ""
                    End If

                    row("SubTeam_No") = .Item("SubTeam_No")
                    row("Pre_Order") = .Item("Pre_Order")
                    row("EXEDistributed") = .Item("EXEDistributed")

                    mdt.Rows.Add(row)
                Loop

                .Close()
            End With

            mdt.AcceptChanges()
            oDataView = New System.Data.DataView(mdt)
            oDataView.Sort = "Description"
            ugrdSearchResults.DataSource = oDataView

            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Default").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Deleted").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Discont").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Default").Header.VisiblePosition = 7
            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Deleted").Header.VisiblePosition = 8
            ugrdSearchResults.DisplayLayout.Bands(0).Columns("Discont").Header.VisiblePosition = 9

            ugrdSearchResults.Focus()

        Catch ex1 As System.InvalidCastException
            ProcessException(ex1)
        Catch ex As Exception
            Throw ex

        End Try

        If iLoop > MaxLoop Then
            ugrdSearchResults.Text = String.Format("Search Results: {0}+ items", mdt.Rows.Count)
            MsgBox(String.Format(ResourcesIRMA.GetString("MoreDataAvailable"), vbCrLf), MsgBoxStyle.Information, Me.Text)
        Else
            ugrdSearchResults.Text = String.Format("Search Results: {0} items", mdt.Rows.Count)
        End If

        If mdt.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdSearchResults.Rows(0).Selected = True
        Else
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If

        If iLoop = 1 Then
            ReturnSelection()
        End If

    End Sub

    Private Sub ReturnSelection()

        Dim itemSearch As ItemSearchBO = Nothing

        '-- Make sure one item was selected
        If ugrdSearchResults.Selected.Rows.Count > 0 Then
            With ugrdSearchResults.Selected.Rows(0)
                ' Populate the ItemSearchBO with the results
                itemSearch = New ItemSearchBO
                itemSearch.ItemKey = .Cells("Item_Key").Value
                itemSearch.ItemDesc = .Cells("Description").Value
                itemSearch.ItemSubTeam = .Cells("SubTeam_No").Value
                itemSearch.ItemIdentifier = .Cells("Identifier").Value

                m_bIsPreOrderItem = .Cells("Pre_Order").Value
                m_bIsEXEDistributed = .Cells("EXEDistributed").Value
                m_sItemIdentifier = .Cells("Identifier").Value
            End With

            ' Reset the global variables if the calling form is using them (default behavior)
            If _resetGlobalVars Then
                glItemID = itemSearch.ItemKey
                gsItemDescription = itemSearch.ItemDesc
                glItemSubTeam = itemSearch.ItemSubTeam
            End If

            RaiseEvent ItemSelected(itemSearch)
            Me.Hide()

            'TFS 2371 - Faisal Ahmed, 08/20/2011
            If gbQuickSearch Then Me.Close()

        Else
            MsgBox(ResourcesItemHosting.GetString("SelectItem"), MsgBoxStyle.Information, Me.Text)
        End If

    End Sub

    Public Sub SetDistSubTeam()

        If (Me.HierarchySelector1.cmbSubTeam.SelectedIndex > -1) Then
            If (IsSubTeamDistributed(VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex)) = True) Then
                ' When the SubTeam_No IS found, leave the DistSubTeam combo populated with Dist SubTeams and leave
                '   enabled for the user to pick one.
                Call PopulateDistSubTeamByRetailSubTeam(cmbDistSubTeam, VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex))
                cmbDistSubTeam.Enabled = True
                cmbDistSubTeam.SelectedIndex = -1
                ' When the DistSubTeam combo is enabled, require a selection from the user when saving.
            Else
                ' When the SubTeam_No is NOT found in the distinct list of DistSubTeam.RetailSubTeam_No(s),
                '   then populate the DistSubTeam combo with the SubTeam text (and disable).
                cmbDistSubTeam.Items.Clear()
                cmbDistSubTeam.Items.Add(Me.HierarchySelector1.cmbSubTeam.Text)
                cmbDistSubTeam.SelectedIndex = 0
                cmbDistSubTeam.Enabled = False
                '   Save as NULL.  You can tell since the DistSubTeam combo will be disabled.
            End If
        End If


    End Sub

    Private Sub frmItemSearch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            _paramList.Clear()
            _paramList = Nothing

            _factory = Nothing

        Catch ex As Exception
            'swallow errors

        End Try

        'Just in case Windows re-uses upon next load
        plLimitSubTeam_No = 0

        If Not txtField(4).ReadOnly Then SaveSetting("IRMA", "ItemSearch", "VendorName", Trim(txtField(4).Text))
        If Not txtField(5).ReadOnly Then SaveSetting("IRMA", "ItemSearch", "VendorID", Trim(txtField(5).Text))
        If chkDiscontinued.Enabled Then SaveSetting("IRMA", "ItemSearch", "Discontinued", Str(chkDiscontinued.CheckState))
        If chkNotAvailable.Enabled Then SaveSetting("IRMA", "ItemSearch", "NotAvailable", Str(chkNotAvailable.CheckState))
        If chkIncludeDeletedItems.Enabled Then SaveSetting("IRMA", "ItemSearch", "IncludeDeletedItems", Str(chkIncludeDeletedItems.CheckState))
        If chkWFMItems.Enabled Then SaveSetting("IRMA", "ItemSearch", "WFM", Str(chkWFMItems.CheckState))
        If chkHFM.Enabled Then SaveSetting("IRMA", "ItemSearch", "HFM", Str(chkHFM.CheckState))
        SaveSetting("IRMA", "ItemSearch", "ItemDesc", Trim(txtField(0).Text))
        SaveSetting("IRMA", "ItemSearch", "Identifier", Trim(txtField(1).Text))

        If cmbBrand.SelectedIndex = -1 Then
            SaveSetting("IRMA", "ItemSearch", "BrandID", "-1")
        Else
            SaveSetting("IRMA", "ItemSearch", "BrandID", CStr(VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex)))
        End If

        If cmbDistSubTeam.SelectedIndex = -1 Then
            SaveSetting("IRMA", "ItemSearch", "DistSubTeam_No", "-1")
        Else
            SaveSetting("IRMA", "ItemSearch", "DistSubTeam_No", CStr(VB6.GetItemData(cmbDistSubTeam, cmbDistSubTeam.SelectedIndex)))
        End If

        If Me.HierarchySelector1.cmbSubTeam.Enabled Then
            If Me.HierarchySelector1.cmbSubTeam.SelectedIndex = -1 Then
                SaveSetting("IRMA", "ItemSearch", "SubTeam", "-1")
            Else
                SaveSetting("IRMA", "ItemSearch", "SubTeam", Str(VB6.GetItemData(Me.HierarchySelector1.cmbSubTeam, Me.HierarchySelector1.cmbSubTeam.SelectedIndex)))
            End If
        End If

        If Me.HierarchySelector1.cmbCategory.SelectedIndex = -1 Then
            SaveSetting("IRMA", "ItemSearch", "Category", "-1")
        Else
            SaveSetting("IRMA", "ItemSearch", "Category", Str(VB6.GetItemData(Me.HierarchySelector1.cmbCategory, Me.HierarchySelector1.cmbCategory.SelectedIndex)))
        End If

        If Me.HierarchySelector1.cmbLevel3.SelectedIndex = -1 Then
            SaveSetting("IRMA", "ItemSearch", "Level3", "-1")
        Else
            SaveSetting("IRMA", "ItemSearch", "Level3", Str(VB6.GetItemData(Me.HierarchySelector1.cmbLevel3, Me.HierarchySelector1.cmbLevel3.SelectedIndex)))
        End If

        If Me.HierarchySelector1.cmbLevel4.SelectedIndex = -1 Then
            SaveSetting("IRMA", "ItemSearch", "Level4", "-1")
        Else
            SaveSetting("IRMA", "ItemSearch", "Level4", Str(VB6.GetItemData(Me.HierarchySelector1.cmbLevel4, Me.HierarchySelector1.cmbLevel4.SelectedIndex)))
        End If

        If chkDefaultIdentifiers.Enabled Then SaveSetting("IRMA", "ItemSearch", "DefaultIdentifiers", Str(chkDefaultIdentifiers.CheckState))

    End Sub

    Private Sub ugrdSearchResults_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdSearchResults.DoubleClickRow
        Call cmdSelect_Click(cmdSelect, New System.EventArgs())
    End Sub

    Private Sub cmdClearCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClearCriteria.Click

        'reset the search criteria
        Try
            cmbDistSubTeam.SelectedIndex = -1
            cmbBrand.SelectedIndex = -1

            txtField(0).Clear()     'Description
            txtField(1).Clear()     'Identifier
            txtField(4).Clear()     'vendor
            txtField(5).Clear()     'Vendor Item ID
            txtVendorDesc.Clear()   'Vendor Item Desc 
            txtPOSDesc.Clear()      'POS Desc


            chkIncludeDeletedItems.CheckState = CheckState.Unchecked
            chkDiscontinued.CheckState = CheckState.Unchecked
            chkNotAvailable.CheckState = CheckState.Unchecked

            With HierarchySelector1
                .cmbSubTeam.SelectedIndex = -1
                'MD 7/31/2009: WI 10613, enabling the subteam combo so that user can select a new value
                .cmbSubTeam.Enabled = True
                .cmbCategory.SelectedIndex = -1
                .cmbLevel3.SelectedIndex = -1
                .cmbLevel4.SelectedIndex = -1
            End With

            chkWFMItems.CheckState = CheckState.Checked
            chkHFM.CheckState = CheckState.Checked
            chkDefaultIdentifiers.CheckState = CheckState.Unchecked

        Catch ex As Exception
            ProcessException(ex)

        End Try

    End Sub

    Private Sub ProcessException(ByRef ex As Exception)

        Dim sMsg As String = "The following error occurred:"

        If ex.InnerException IsNot Nothing Then
            sMsg = String.Format("{0}{1}{1}{2}{1}{1}{3}", sMsg, vbCrLf, ex.Message, ex.InnerException.Message)
        Else
            sMsg = String.Format("{0}{1}{1}{2}", sMsg, vbCrLf, ex.Message)
        End If

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        MsgBox(sMsg, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, ex.TargetSite.Module.Name & " - " & ex.TargetSite.DeclaringType.FullName & "." & ex.TargetSite.Name)

    End Sub

    Private Sub ugrdSearchResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSearchResults.GotFocus
        Me.AcceptButton = cmdSelect
    End Sub

    Private Sub ugrdSearchResults_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdSearchResults.LostFocus
        Me.AcceptButton = cmdSearch
    End Sub

    Private Sub ugrdSearchResults_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ugrdSearchResults.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cmdSearch.PerformClick()
        End If
    End Sub

    Private Sub SetupJurisdictionUltraCombo(ByRef ultraCombo As Infragistics.Win.UltraWinGrid.UltraCombo)

        ' Add an additional unbound column to the UltraCombo.  This will be used for the selection of each item.
        Dim c As UltraGridColumn = ultraCombo.DisplayLayout.Bands(0).Columns.Add()

        c.Key = "Selected"
        c.Header.Caption = String.Empty
        c.DataType = GetType(Boolean)

        ' This allows end users to select / unselect ALL items. 
        c.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always

        ' Move the checkbox column to the first position. 
        c.Header.VisiblePosition = 0

        ' Set the Selected column to be associated with the CheckState property.
        ultraCombo.CheckedListSettings.CheckStateMember = "Selected"
        ultraCombo.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems

        ' Set up the control to use a custom list delimiter. 
        ultraCombo.CheckedListSettings.ListSeparator = "; "

        ' Set ItemCheckArea to Item, so that clicking directly on an item also checks the item.
        ultraCombo.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item

        ' Hide the data rows that we don't want to display.
        ultraCombo.DisplayLayout.Bands(0).Columns("StoreJurisdictionID").Hidden = True

        ' Make the header name a little more user friendly.
        ultraCombo.DisplayLayout.Bands(0).Columns("StoreJurisdictionDesc").Header.Caption = "Jurisdiction"

        ' The default selection will be U.S., and the control will disabled if there is only jurisdiction.
        ultraCombo.Rows.Item(0).Cells.Item("Selected").Value = True

        If ultraCombo.Rows.Count = 1 Then
            ultraCombo.Enabled = False
        End If

    End Sub
End Class