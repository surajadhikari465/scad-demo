Option Strict Off
Option Explicit On

Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Ordering.DataAccess

Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Imports log4net

Friend Class frmOrderItemSearch
	Inherits System.Windows.Forms.Form
	
	Public plLimitSubTeam_No As Integer 'Used to pre-select the search subteam and lock it to that subteam
	Public pbOrderFromDistribution As Boolean
    Private m_oSelectedItems As colSelectedItems
    Private IsInitializing As Boolean

	'-- Public Read-Write  Properties local storage
	Private m_bIsPreOrderItem As Short
    Private m_bIsEXEDistributed As Short
    Private m_iOrderHeader_ID As Integer
	'Public Write Only Properties local storage
	Private m_sVendorName As String
	Private m_bAutoEnter As Boolean
	Private m_lLimitStore_No As Integer
	Private m_lLimitSubTeam_No As Integer 'Used to pre-select the search subteam and lock it to that subteam
	Private m_bIsVendorAFacility As Boolean
    Private m_bIsVendorStoreSame As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private m_blnShowPreOrderMsg As Boolean = True
    Private m_blnShowNonPreOrderMsg As Boolean = True
    Private m_blnIsLoading As Boolean = False


    Dim mdt As DataTable
    Dim mdv As DataView

    Private Sub frmOrderItemSearch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrderItemSearch_FormClosing Entry")
        plLimitSubTeam_No = 0 'Just in case Windows re-uses upon next load
        If m_bAutoEnter Then
            SaveSetting("IRMA", "OrderItemSearch", "ItemDesc", Trim(txtField(0).Text))
            SaveSetting("IRMA", "OrderItemSearch", "Identifier", Trim(txtField(1).Text))

            If cmbBrand.SelectedIndex = -1 Then
                SaveSetting("IRMA", "OrderItemSearch", "BrandID", "-1")
            Else
                SaveSetting("IRMA", "OrderItemSearch", "BrandID", CStr(VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex)))
            End If

        End If
        logger.Debug("frmOrderItemSearch_FormClosing Exit")
    End Sub

    Private Sub frmOrderItemSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderItemSearch_Load Entry")
        Dim sProductType As String

        m_blnIsLoading = True
        m_bIsPreOrderItem = 0
        m_bIsEXEDistributed = -1

        'Clear the results fields (These are kept for backward compatability and should be
        '                           removed when we stop using globals to pass values)
        glItemID = 0
        gsItemDescription = String.Empty
        glItemSubTeam = 0
        sProductType = String.Empty

        AutoEnter = True

        Call LoadBrandNew(cmbBrand)

        LoadStores(cmbStore)
        ' hack due to timing issues
        If (m_lLimitStore_No > 0) Then
            SetCombo(cmbStore, m_lLimitStore_No)
            SetActive(cmbStore, False)
        End If

        Select Case Global_Renamed.geProductType
            Case enumProductType.Product
                sProductType = "Product"
            Case enumProductType.PackagingSupplies
                sProductType = "Packaging"
            Case enumProductType.OtherSupplies
                sProductType = "Supplies"
        End Select

        ' Set the caption to reflect the order type
        Select Case Global_Renamed.geOrderType
            Case enumOrderType.Distribution
                Text = "Distribution " & sProductType & " Order Item Search"
            Case enumOrderType.Purchase
                Text = "Purchase " & sProductType & " Order Item Search"
            Case enumOrderType.Transfer
                Text = "Transfer " & sProductType & " Order Item Search"
            Case enumOrderType.Flowthru
                Text = "Flowthru " & sProductType & " Order Item Search"
        End Select

        If Len(txtField(4).Text) = 0 Then
            SetActive(txtField(5), False)
        Else
            SetActive(txtField(5), True)
        End If

        If m_bAutoEnter Then
            txtField(0).Text = GetSetting("IRMA", "OrderItemSearch", "ItemDesc")
            txtField(1).Text = GetSetting("IRMA", "OrderItemSearch", "Identifier")

            Call SetComboNew(cmbBrand, Val(GetSetting("IRMA", "OrderItemSearch", "BrandID")))

        End If

        m_oSelectedItems = New colSelectedItems
        m_oSelectedItems.DataSource = ugrdSearchResults

        If Global_Renamed.geOrderType = enumOrderType.Flowthru Then
            optPreOrder.Checked = True

            optPreOrder.Enabled = False
            optNonPreOrder.Enabled = False
        Else
            If PreOrderItemsExist(Me.OrderHeader_ID) Then
                optPreOrder.Checked = True
            Else
                optNonPreOrder.Checked = True
            End If


            optNonPreOrder.Enabled = True
            optPreOrder.Enabled = True
        End If

        If Global_Renamed.geOrderType = enumOrderType.Purchase Or Global_Renamed.geOrderType = enumOrderType.Transfer Then
            chkIncludeNotAvailable.Checked = True
        Else
            chkIncludeNotAvailable.Checked = False
        End If

        m_blnIsLoading = False

        logger.Debug("frmOrderItemSearch_Load Exit")

    End Sub

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("SearchItems")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemID", GetType(String)))
        mdt.Columns.Add(New DataColumn("Cost", GetType(String)))
        mdt.Columns.Add(New DataColumn("Brand", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemStatus", GetType(String)))

        mdt.Columns.Add(New DataColumn("NA", GetType(String)))
        mdt.Columns.Add(New DataColumn("PkgDesc", GetType(String)))
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("SubTeam_No", GetType(String)))
        mdt.Columns.Add(New DataColumn("Pre_Order", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("EXEDistributed", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("VendorItemStatusFull", GetType(String)))

        logger.Debug("SetupDataTable Exit")

    End Sub

    Public Property IsEXEDistributed() As Boolean
        Get
            IsEXEDistributed = m_bIsEXEDistributed
        End Get
        Set(ByVal Value As Boolean)
            m_bIsEXEDistributed = System.Math.Abs(CInt(Value))
        End Set
    End Property
	
	Public Property IsPreOrderItem() As Boolean
		Get
			IsPreOrderItem = m_bIsPreOrderItem
		End Get
		Set(ByVal Value As Boolean)
			m_bIsPreOrderItem = System.Math.Abs(CInt(Value))
		End Set
    End Property

    Public Property OrderHeader_ID() As Integer
        Get
            OrderHeader_ID = m_iOrderHeader_ID
        End Get
        Set(ByVal Value As Integer)
            m_iOrderHeader_ID = CInt(Value)
        End Set
    End Property
	
	Public WriteOnly Property StatusLabel() As String
		Set(ByVal Value As String)
			lblStatus.Text = Value
		End Set
	End Property
	
	Public WriteOnly Property IsVendorAFacility() As Boolean
		Set(ByVal Value As Boolean)
			m_bIsVendorAFacility = Value
		End Set
	End Property
	
	'-- Begin Read Only Properties
    '--------------------------------------
	Public ReadOnly Property SelectedItems() As colSelectedItems
		Get
			SelectedItems = m_oSelectedItems
		End Get
	End Property
	
    '-- Public Write Only Properties
    '---------------------------------------
	Public WriteOnly Property MultiSelect() As Boolean
		Set(ByVal Value As Boolean)
            ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        End Set
	End Property

    Public WriteOnly Property AutoEnter() As Boolean
        Set(ByVal Value As Boolean)
            m_bAutoEnter = Value
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

                cmbDistSubTeam.SelectedIndex = -1
                cmbDistSubTeam.Enabled = True
            End If
        End Set
    End Property

    Public WriteOnly Property LimitStore_No() As Integer
        Set(ByVal Value As Integer)

            m_lLimitStore_No = Value

            If Value > 0 Then
                SetCombo(cmbStore, Value)
                SetActive(cmbStore, False)
            Else
                cmbStore.SelectedIndex = -1
                SetActive(cmbStore, True)
            End If

        End Set
    End Property

    Public WriteOnly Property SubTeam_No() As Integer
        Set(ByVal Value As Integer)
            Dim bEnableDistSubTeam As Boolean
            bEnableDistSubTeam = True

            If (Global_Renamed.geOrderType = enumOrderType.Distribution) Then
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

    Public WriteOnly Property VendorName() As String
        Set(ByVal Value As String)
            If Trim(Value) <> "" Then
                txtField(4).Text = Trim(Value)
                SetActive(txtField(4), False) 'don't allow the user to select items from a different vendor
                SetActive(txtField(5), True) 'allow user to select search by Vendor's Item ID
            End If
        End Set
    End Property

    Public WriteOnly Property VendorDataEnabled() As Boolean
        Set(ByVal Value As Boolean)
            If txtField(4).Text = "" Then 'field 4 is VendorName.  If the vendor name contains data, the VendorName prop has already disabled the fields
                SetActive(txtField(4), Value)
                SetActive(txtField(5), Value)
            End If
        End Set
    End Property

    Public WriteOnly Property IncludeDiscontinued() As enumChkBoxValues
        Set(ByVal Value As enumChkBoxValues)
            Select Case True
                Case Value = enumChkBoxValues.UncheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked

                    If frmOrders.IsCredit Then
                        chkDiscontinued.Enabled = True
                    Else
                        chkDiscontinued.Enabled = False
                    End If

                Case Value = enumChkBoxValues.UncheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Unchecked
                    chkDiscontinued.Enabled = True
                Case Value = enumChkBoxValues.CheckedDisabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = False
                Case Value = enumChkBoxValues.CheckedEnabled
                    chkDiscontinued.CheckState = System.Windows.Forms.CheckState.Checked
                    chkDiscontinued.Enabled = True
            End Select

        End Set
    End Property
	
	Private Sub cmbBrand_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrand.KeyPress
        logger.Debug("cmbBrand_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 8 Then cmbBrand.SelectedIndex = -1
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("cmbBrand_KeyPress Exit")
	End Sub
	
	Private Sub cmbDistSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbDistSubTeam.KeyPress
        logger.Debug("cmbDistSubTeam_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 8 Then cmbDistSubTeam.SelectedIndex = -1
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("cmbDistSubTeam_KeyPress Exit")
	End Sub
	
    Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        logger.Debug("cmbStore_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbStore_KeyPress Exit")
    End Sub

	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Unload search form
		Me.Hide()
		
	End Sub
	
    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        Dim DAO As SubTeamDAO = New SubTeamDAO
        Dim dr As DataRow = Nothing
        Dim _dataset As DataSet

        If ugrdSearchResults.Selected.Rows.Count > 0 Then
            'TFS 7548 Faisal Ahmed - Make sure GL accounts are configured for packaging and other supplies subteam
            If Global_Renamed.geOrderType = Global_Renamed.enumOrderType.Transfer Then
                Dim SubTeam As Integer = ugrdSearchResults.Selected.Rows(0).Cells("SubTeam_No").Value

                _dataset = DAO.GetSubTeam(SubTeam)
                dr = _dataset.Tables(0).Rows(0)

                Dim SubTeamName As String = dr("SubTeam_Name").ToString().Trim
                'Dim SubTeamName As String = "[" + cmbSubTeam.SelectedItem.ToString + "]"

                If (Global_Renamed.geProductType = Global_Renamed.enumProductType.PackagingSupplies) Then
                    If Not IsGLPackagingAccountAvailable(SubTeam) Then
                        MsgBox("GL Packaging Account number is required for the selected subteam " + SubTeamName, MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If

                ElseIf (Global_Renamed.geProductType = Global_Renamed.enumProductType.OtherSupplies) Then
                    If Not IsGLSuppliesAccountAvailable(SubTeam) Then
                        MsgBox("GL Supplies Account number is required for the selected subteam " + SubTeamName, MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If
                End If
            End If

            If Global_Renamed.geOrderType = enumOrderType.Distribution Or Global_Renamed.geOrderType = enumOrderType.Flowthru Then
                If ugrdSearchResults.Selected.Rows(0).Cells("NA").Text <> "" Then
                    MsgBox("This item is marked as Not Available and can not be ordered at this time.", MsgBoxStyle.Exclamation, "IRMA Order Item Search")
                Else
                    ReturnSelection()
                End If
            Else
                ReturnSelection()
            End If
        Else
            MsgBox("An item from the list must be selected.", MsgBoxStyle.Exclamation, "IRMA Order Item Search")
        End If
    End Sub
	
	Private Sub frmOrderItemSearch_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmOrderItemSearch_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 13 Then
			RunSearch()
			KeyAscii = 0
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("frmOrderItemSearch_KeyPress Exit")
	End Sub
	
	Public Sub LoadSubTeamCombo(ByRef bIsVendorAFacility As Boolean, ByRef bIsVendorStoreSame As Boolean, Optional ByRef lLimitStore_No As Integer = -1, Optional ByRef lOrderHeader_SubTeamNo As Integer = -1)

        logger.Debug("LoadSubTeamCombo Entry")
        Dim eSubTeamType As enumSubTeamType
		
		m_bIsVendorAFacility = bIsVendorAFacility
		m_bIsVendorStoreSame = bIsVendorStoreSame
		m_lLimitStore_No = lLimitStore_No
		
		' Apply the rules about what subteams to load
        If ((geOrderType = enumOrderType.Transfer) And (geProductType = enumProductType.Product)) Then
            eSubTeamType = enumSubTeamType.All
        ElseIf (geOrderType = enumOrderType.Transfer And geProductType = enumProductType.PackagingSupplies) Then
            eSubTeamType = enumSubTeamType.Packaging
        Else
            eSubTeamType = DetermineSubTeamType((enumSubTeamContext.Item_From), m_bIsVendorAFacility, m_bIsVendorStoreSame)
        End If

        If geOrderType = enumOrderType.Transfer Then
			Call LoadSubTeamByType(eSubTeamType, cmbSubTeam, lLimitStore_No, lOrderHeader_SubTeamNo)
		Else
            Call LoadSubTeamByType(enumSubTeamType.All, cmbSubTeam, lLimitStore_No, lOrderHeader_SubTeamNo)
        End If

        If (cmbSubTeam.SelectedIndex < 0) Then
            If (cmbSubTeam.Items.Count = 1) Then
                cmbSubTeam.SelectedIndex = 0
            End If
        End If

        Call LoadDistSubTeam(cmbDistSubTeam)
        logger.Debug("LoadSubTeamCombo Exit")

    End Sub

    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged

        logger.Debug("txtField_TextChanged Entry")
        If Me.IsInitializing = True Then Exit Sub

        Dim Index As Short = txtField.GetIndex(eventSender)

        If Index = 4 Then
            If Len(Trim(txtField(4).Text)) > 0 Then
                If Global_Renamed.geOrderType <> enumOrderType.Transfer Then SetActive(cmbStore, True)
                SetActive(txtField(5), True)
            Else
                SetActive(cmbStore, False)
                txtField(5).Text = ""
                SetActive(txtField(5), False)
            End If
        End If
        logger.Debug("txtField_TextChanged Exit")

    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    logger.Debug("txtField_KeyPress Entry")
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)
    If txtField(Index).ReadOnly Then GoTo EventExitSub
    KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
    logger.Debug("txtField_KeyPress Exit")
EventExitSub:
    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
    logger.Debug("txtField_KeyPress Exit")
  End Sub
  Private Function IsGLPackagingAccountAvailable(ByVal SubTeamNo As Integer) As Boolean
    Dim DAO As SubTeamDAO = New SubTeamDAO
    Dim dr As DataRow = Nothing
    Dim _dataset As DataSet

    Dim AccountNumber As String = String.Empty
    IsGLPackagingAccountAvailable = False

    _dataset = DAO.GetSubTeam(SubTeamNo)
    dr = _dataset.Tables(0).Rows(0)

    AccountNumber = dr("GLPackagingAcct").ToString().Trim

    _dataset.Dispose()

    If AccountNumber <> String.Empty Then
      IsGLPackagingAccountAvailable = True
    End If
  End Function

  Private Function IsGLSuppliesAccountAvailable(ByVal SubTeamNo As Integer) As Boolean
        Dim DAO As SubTeamDAO = New SubTeamDAO
        Dim dr As DataRow = Nothing
        Dim _dataset As DataSet

        Dim AccountNumber As String = String.Empty
        IsGLSuppliesAccountAvailable = False

        _dataset = DAO.GetSubTeam(SubTeamNo)
        dr = _dataset.Tables(0).Rows(0)

        AccountNumber = dr("GLSuppliesAcct").ToString().Trim

        _dataset.Dispose()

        If AccountNumber <> String.Empty Then
            IsGLSuppliesAccountAvailable = True
        End If
    End Function

    Public Sub RunSearch()
        logger.Debug("RunSearch Entry")
        Dim sSQLText As String
        Dim lSubTeam_No As Integer
        Dim iCategory_ID As Short
        Dim sVendor As String
        Dim sStore_No As String
        Dim sVendor_ID As String
        Dim sBrand_ID As String
        Dim sItem_Description As String
        Dim sIdentifier As String
        Dim iDiscontinue_Item As Short
        Dim iNotAvailable As Short
        Dim lDistSubTeam_No As Integer

        '-- Do SubTeam search
        If cmbSubTeam.SelectedIndex > -1 Then
            lSubTeam_No = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
        Else
            lSubTeam_No = 0
        End If

        If ((cmbDistSubTeam.SelectedIndex > -1) And (cmbDistSubTeam.Items.Count > 1)) Then
            lDistSubTeam_No = VB6.GetItemData(cmbDistSubTeam, cmbDistSubTeam.SelectedIndex)
        Else
            lDistSubTeam_No = 0
        End If

        If cmbCategory.SelectedIndex > -1 Then
            iCategory_ID = VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex)
        Else
            iCategory_ID = 0
        End If

        '-- Make the where string for Identifier
        sItem_Description = ConvertQuotes(Trim(txtField(0).Text))
        sIdentifier = Trim(txtField(1).Text)
        sVendor = ConvertQuotes(Trim(txtField(4).Text))
        sVendor_ID = Trim(txtField(5).Text)

        ' for testing bug 5131
        If cmbBrand.SelectedIndex = -1 Then
            sBrand_ID = "Null"
        Else
            'sBrand_ID = CStr(ComboValue(cmbBrand))
            sBrand_ID = cmbBrand.SelectedValue
        End If

        'Store selection applicable only if vendor name entered
        If Len(sVendor) > 0 Then
            If Me.cmbStore.SelectedIndex = -1 Then
                sStore_No = "NULL"
            Else
                sStore_No = ComboValue(cmbStore)
            End If
        Else
            If ((Global_Renamed.geOrderType = enumOrderType.Transfer) And (m_bIsVendorAFacility = True)) Then
                sStore_No = CStr(m_lLimitStore_No)
            Else
                sStore_No = "NULL"
            End If
        End If

        iDiscontinue_Item = System.Math.Abs(chkDiscontinued.CheckState)

        If chkIncludeNotAvailable.Checked Then
            iNotAvailable = 1
        Else
            iNotAvailable = 0
        End If

        If Global_Renamed.geOrderType <> enumOrderType.Purchase And sVendor <> String.Empty And MultipleVendorExist(sVendor) Then
            MsgBox("'There are multiple vendors that match your input. Please enter a few more letters of the vendor name to narrow your search.")
            _txtField_4.Focus()
            Exit Sub
        End If

        If Global_Renamed.geOrderType = enumOrderType.Transfer Then
            sSQLText = "EXEC GetTransferOrderItemSearch " & glSubTeamNo & ", " & _
                                                    glTransfer_To_SubTeam & ", " & _
                                                    lSubTeam_No & ", " & _
                                                    lDistSubTeam_No & ", " & _
                                                    iCategory_ID & ", '" & _
                                                    sVendor & "', " & _
                                                    sStore_No & ", '" & _
                                                    sItem_Description & "', '" & _
                                                    sIdentifier & "', " & _
                                                    iDiscontinue_Item & ", " & _
                                                    iNotAvailable & ", " & _
                                                    sBrand_ID & ", " & _
                                                    IIf(m_bIsPreOrderItem = 0, 0, 1) & ", " & _
                                                    IIf(m_bIsEXEDistributed > -1, m_bIsEXEDistributed, "NULL") & ", " & _
                                                    CShort(Global_Renamed.geProductType) & ", '" & _
                                                    sVendor_ID & "'"

        Else
            sSQLText = "EXEC GetOrderItemSearch " & CShort(Global_Renamed.geOrderType) & ", " & _
                                                    glSubTeamNo & ", " & _
                                                    glTransfer_To_SubTeam & ", " & _
                                                    lSubTeam_No & ", " & _
                                                    lDistSubTeam_No & ", " & _
                                                    iCategory_ID & ", '" & _
                                                    sVendor & "', " & _
                                                    sStore_No & ", '" & _
                                                    sVendor_ID & "', '" & _
                                                    sItem_Description & "', '" & _
                                                    sIdentifier & "', " & _
                                                    iDiscontinue_Item & ", " & _
                                                    iNotAvailable & ", " & _
                                                    sBrand_ID & ", " & _
                                                    IIf(m_bIsVendorAFacility, 1, 0) & ", " & _
                                                    IIf(m_bIsVendorStoreSame, 1, 0) & ", " & _
                                                    IIf(m_bIsPreOrderItem = 0, 0, 1) & ", " & _
                                                    IIf(m_bIsEXEDistributed > -1, m_bIsEXEDistributed, "NULL") & ", " & _
                                                    CShort(Global_Renamed.geProductType)
        End If

        Call SetupDataTable()
        Call LoadDataTable(sSQLText)
        logger.Debug("RunSearch Exit")

    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String)
        '#
        '#  Remove DAO calls because it was throwing strange "Unspecified" errors. Replaced with ADO calls because its new and shiny.
        '#

        logger.Debug("LoadDataTable Entry")
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim iLoop As Integer = 0
        Dim MaxLoop As Short = 1000

        Dim rsSearch As DataTable = Nothing
        Dim newrow As DataRow = Nothing
        Try
            rsSearch = factory.GetDataTable(sSearchSQL)
            mdt.Rows.Clear()
            For Each row As DataRow In rsSearch.Rows
                newrow = mdt.NewRow
                newrow("Item_Key") = row("Item_Key")
                newrow("Description") = row("Item_Description")
                newrow("Identifier") = row("Identifier")
                newrow("SubTeam_No") = row("SubTeam_No")
                newrow("Pre_Order") = row("Pre_Order")
                newrow("EXEDistributed") = row("EXEDistributed")

                newrow("NA") = IIf(row("Not_Available"), IIf(Not IsDBNull(row("Not_AvailableNote")), "*..." & row("Not_AvailableNote"), "*"), "")

                Debug.Print("")
                If row("Package_Desc1") Is DBNull.Value Then
                    Debug.Print("Package_Desc1: NULL")
                Else
                    Debug.Print("Package_Desc1: " & row("Package_Desc1").ToString)
                End If
                Debug.Print("Package_Desc2: " & row("Package_Desc2"))
                If row("Package_Desc1") IsNot DBNull.Value _
                        AndAlso row("Package_Desc2") IsNot DBNull.Value Then
                    newrow("PkgDesc") = CType(row("Package_Desc1"), Decimal).ToString("#####.##") & _
                            "/" & CType(row("Package_Desc2"), Decimal).ToString("#####.##") & _
                            " " & row("Package_Unit")
               Else
                    newrow("PkgDesc") = " / "
                End If

                newrow("VendorItemID") = row("VendorItemID")
                newrow("VendorItemStatus") = row("VendorItemStatus") & ""
                newrow("VendorItemStatusFull") = row("VendorItemStatusFull") & ""
                newrow("Brand") = row("Brand")
                newrow("Cost") = row("Cost")

                mdt.Rows.Add(newrow)
                iLoop += 1
                If iLoop > MaxLoop Then Exit For
            Next

            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)


            '################################################################################################################################
            ' BUYSIDE Vendor Item Attributes: Please Read! (Robin Eudy: 2/14/2011)
            ' Adding CLIENT SIDE filters for VendorItemStatus. The filters on the uderlying search Stored Proc are unchanged for now.
            ' The data to populate VendorItemStatus from IMHA/VIP does not exist at this point and time. All data in this field will be NULL
            ' To avoid slowing down the search queries with criteria that doesnt exist I have decided to make this filter CLIENT SIDE instead of 
            ' adding filters to  the Stored Proc. THIS SHOULD BE CHANGED once the VendorItemStatus starts to be populated.
            '#################################################################################################################################

            Dim _rowfilter As String = String.Empty
            ' ## Show NULL and Active values for VendorItemStatus by default.
            _rowfilter = "isnull(VendorItemStatus,'null') = 'null' OR VendorItemStatus = '' OR VendorItemStatus = 'A' OR"

            ' ## Show other Statuses if they are checked.
			If chkVendorItemStatus_MfgDiscontinued.Checked Then _rowfilter += " VendorItemStatus = 'M' OR"
            If chkVendorItemStatus_NotAvailable.Checked Then _rowfilter += " VendorItemStatus = 'N' OR"
			If chkVendorItemStatus_VendorDiscontinued.Checked Then _rowfilter += " VendorItemStatus = 'V' OR"
            If chkVendorItemStatus_Seasonal.Checked Then _rowfilter += " VendorItemStatus = 'S' OR"

            ' ## Clean up the filter string we generated. (remove the final OR that isnt needed.)
            If _rowfilter.EndsWith(" OR") Then _rowfilter = _rowfilter.Remove(_rowfilter.Length - 3)
            ' ## Apply filter.
            mdv.RowFilter = _rowfilter

            '################################################################################################################################
            '################################################################################################################################

            ugrdSearchResults.DataSource = mdv

            If iLoop > MaxLoop Then
                MsgBox("More data is available." & vbCrLf & "For more data, please limit search criteria.", MsgBoxStyle.Exclamation, "Notice!")
            End If

            If rsSearch.Rows.Count > 0 And ugrdSearchResults.Rows.Count > 0 Then
                'Set the first item to selected.
                ugrdSearchResults.Rows(0).Selected = True
            Else
                MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            End If

            If Global_Renamed.geOrderType = enumOrderType.Transfer Then
                ugrdSearchResults.DisplayLayout.Bands(0).Columns("VendorItemID").Hidden = True
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Dispose()
            End If
        End Try

        'NOTE: why do this?...hmmm Good Question...did I just answer my own question?
        If iLoop > 0 And Not (m_bAutoEnter) Then
            ugrdSearchResults.Rows(0).Selected = True
            If iLoop = 1 Then ReturnSelection()
        End If
        logger.Debug("LoadDataTable Exit")

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("LoadDataTable Exit")

    End Sub

    Private Sub ReturnSelection()
        logger.Debug("ReturnSelection Entry")

        glItemID = ugrdSearchResults.Selected.Rows(0).Cells("Item_Key").Value
        gsItemDescription = ugrdSearchResults.Selected.Rows(0).Cells("Description").Value
        glItemSubTeam = ugrdSearchResults.Selected.Rows(0).Cells("SubTeam_No").Value
        m_bIsPreOrderItem = ugrdSearchResults.Selected.Rows(0).Cells("Pre_Order").Value
        m_bIsEXEDistributed = ugrdSearchResults.Selected.Rows(0).Cells("EXEDistributed").Value
        Me.Hide()

        logger.Debug("ReturnSelection Exit")
    End Sub

    '********************************
    'SubTeam / Catagory Code
    '********************************
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
        If Asc(e.KeyChar) = 8 Then cmbSubTeam.SelectedIndex = -1
        logger.Debug("cmbSubTeam_KeyPress Exit")

    End Sub

    Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress

        logger.Debug("cmbCategory_KeyPress Entry")
        If Asc(e.KeyChar) = 8 Then Me.cmbCategory.SelectedIndex = -1
        logger.Debug("cmbCategory_KeyPress Exit")

    End Sub

    '*************************************
    'End SubTeam / Catagory Code
    '*************************************

    Private Sub ugrdSearchResults_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdSearchResults.DoubleClickRow

        logger.Debug("ugrdSearchResults_DoubleClickRow Entry")
        Call cmdSelect_Click(cmdSelect, New System.EventArgs())
        logger.Debug("ugrdSearchResults_DoubleClickRow Exit")

    End Sub

    Private Sub optPreOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPreOrder.CheckedChanged
        m_bIsPreOrderItem = optPreOrder.Checked

        If m_blnShowPreOrderMsg And Not m_blnIsLoading Then
            If NonPreOrderItemsExist(Me.OrderHeader_ID) And (Global_Renamed.geOrderType = enumOrderType.Distribution Or Global_Renamed.geOrderType = enumOrderType.Flowthru) Then
                MsgBox("There are Non Pre-Order items on this PO.  Pre-Order and Non Pre-Order items cannot be mixed on a Distribution order.  The Non Pre-Ordered items must be removed before Pre-Ordered items can be added.", MsgBoxStyle.Critical, Me.Text)

                m_blnShowPreOrderMsg = False
                optNonPreOrder.Checked = True
                m_blnShowPreOrderMsg = True
            End If
        End If


    End Sub

    Private Sub optNonPreOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNonPreOrder.CheckedChanged
        m_bIsPreOrderItem = optPreOrder.Checked

        If m_blnShowNonPreOrderMsg And Not m_blnIsLoading Then
            If PreOrderItemsExist(Me.OrderHeader_ID) And (Global_Renamed.geOrderType = enumOrderType.Distribution Or Global_Renamed.geOrderType = enumOrderType.Flowthru) Then
                MsgBox("There are Pre-Order items on this PO.  Pre-Order and Non Pre-Order items cannot be mixed on a Distribution order.  The Pre-Ordered items must be removed before Non Pre-Ordered items can be added.", MsgBoxStyle.Critical, Me.Text)

                m_blnShowNonPreOrderMsg = False
                optPreOrder.Checked = True
                m_blnShowNonPreOrderMsg = True
            End If
        End If
    End Sub
    Private Function MultipleVendorExist(ByVal VendorName As String) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        VendorName = "'" + VendorName + "'"
        MultipleVendorExist = CType(factory.ExecuteScalar("SELECT dbo.fn_IsMultipleMatchedVendors(" & VendorName & ")"), Boolean)
    End Function

    Private Function PreOrderItemsExist(ByVal iOrderHeader_ID As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        PreOrderItemsExist = CType(factory.ExecuteScalar("SELECT dbo.fn_PreOrderItemsExist(" & iOrderHeader_ID & ")"), Boolean)
    End Function

    Private Function NonPreOrderItemsExist(ByVal iOrderHeader_ID As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        NonPreOrderItemsExist = CType(factory.ExecuteScalar("SELECT dbo.fn_NonPreOrderItemsExist(" & iOrderHeader_ID & ")"), Boolean)
    End Function


    Private Sub ugrdOrderList_MouseEnterElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdSearchResults.MouseEnterElement
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

    Private Sub ugrdOrderList_MouseLeaveElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdSearchResults.MouseLeaveElement
        ToolTip1.SetToolTip(ugrdSearchResults, Nothing)
    End Sub

	Private Sub chkSubTeam_Click(sender As Object, e As EventArgs) Handles chkSubTeam.Click
		RefreshSubteamCombo(cmbSubTeam, chkSubTeam.Checked)
	End Sub
End Class