<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItem
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        isInitializing = True
        InitializeComponent()
        isInitializing = False
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents _cmbField_4 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_5 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_8 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_9 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_14 As System.Windows.Forms.ComboBox
    Public WithEvents lblRetail As System.Windows.Forms.Label
    Public WithEvents lblVendorOrder As System.Windows.Forms.Label
    Public WithEvents lblDistribution As System.Windows.Forms.Label
    Public WithEvents lblManufacturing As System.Windows.Forms.Label
    Public WithEvents fraUnits As System.Windows.Forms.GroupBox
    Public WithEvents cmbDistSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_15 As System.Windows.Forms.ComboBox
    Public WithEvents _chkField_16 As System.Windows.Forms.CheckBox
    Public WithEvents _cmbField_13 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_18 As System.Windows.Forms.TextBox
    Public WithEvents _chkField_15 As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_12 As System.Windows.Forms.ComboBox
    Public WithEvents _chkField_10 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_8 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_11 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_9 As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_13 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_12 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_14 As System.Windows.Forms.TextBox
    Public WithEvents _chkField_4 As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_11 As System.Windows.Forms.TextBox
    Public WithEvents cmdBrandAdd As System.Windows.Forms.Button
    Public WithEvents _txtField_10 As System.Windows.Forms.TextBox
    Public WithEvents cmdIdentifier As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdInventory As System.Windows.Forms.Button
    Public WithEvents _chkField_14 As System.Windows.Forms.CheckBox
    Public WithEvents cmdPrices As System.Windows.Forms.Button
    Public WithEvents cmdHistory As System.Windows.Forms.Button
    Public WithEvents cmdUnlock As System.Windows.Forms.Button
    Public WithEvents _chkField_12 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_7 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_6 As System.Windows.Forms.CheckBox
    Public WithEvents chkShipper As System.Windows.Forms.CheckBox
    Public WithEvents cmdShipper As System.Windows.Forms.Button
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents cmdItemVendors As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents _txtField_9 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_8 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_6 As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_6 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_3 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_2 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Public WithEvents lblNotAvailableNote As System.Windows.Forms.Label
    Public WithEvents lblDistSubTeam As System.Windows.Forms.Label
    Public WithEvents lblNationalClass As System.Windows.Forms.Label
    Public WithEvents lblCountry As System.Windows.Forms.Label
    Public WithEvents lblSalesAccount As System.Windows.Forms.Label
    Public WithEvents lblItemType As System.Windows.Forms.Label
    Public WithEvents lblSlash2 As System.Windows.Forms.Label
    Public WithEvents lblTieHigh As System.Windows.Forms.Label
    Public WithEvents lblYield As System.Windows.Forms.Label
    Public WithEvents lblAvgUnitWeight As System.Windows.Forms.Label
    Public WithEvents lblReadOnly As System.Windows.Forms.Label
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents lblTmpRange As System.Windows.Forms.Label
    Public WithEvents lblUnitsPerPallet As System.Windows.Forms.Label
    Public WithEvents lblSlash1 As System.Windows.Forms.Label
    Public WithEvents lblBrand As System.Windows.Forms.Label
    Public WithEvents lblOrigin As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblPOSDesc As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents chkField As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents rdoField As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents lblCostedWeight As System.Windows.Forms.CheckBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItem))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdIdentifier = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdInventory = New System.Windows.Forms.Button()
        Me.cmdPrices = New System.Windows.Forms.Button()
        Me.cmdHistory = New System.Windows.Forms.Button()
        Me.cmdUnlock = New System.Windows.Forms.Button()
        Me.cmdShipper = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdItemVendors = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdTaxClassZoom = New System.Windows.Forms.Button()
        Me.cmdScale = New System.Windows.Forms.Button()
        Me.cmdAttributes = New System.Windows.Forms.Button()
        Me.cmdJurisdiction = New System.Windows.Forms.Button()
        Me.cmbSustainRankingDefault = New System.Windows.Forms.ComboBox()
        Me.cmdItemSave = New System.Windows.Forms.Button()
        Me.ButtonNutrifacts = New System.Windows.Forms.Button()
        Me.Button365 = New System.Windows.Forms.Button()
        Me.fraUnits = New System.Windows.Forms.GroupBox()
        Me._cmbField_4 = New System.Windows.Forms.ComboBox()
        Me._cmbField_5 = New System.Windows.Forms.ComboBox()
        Me._cmbField_8 = New System.Windows.Forms.ComboBox()
        Me._cmbField_9 = New System.Windows.Forms.ComboBox()
        Me._cmbField_14 = New System.Windows.Forms.ComboBox()
        Me.lblRetail = New System.Windows.Forms.Label()
        Me.lblVendorOrder = New System.Windows.Forms.Label()
        Me.lblDistribution = New System.Windows.Forms.Label()
        Me.lblManufacturing = New System.Windows.Forms.Label()
        Me.cmbDistSubTeam = New System.Windows.Forms.ComboBox()
        Me._cmbField_15 = New System.Windows.Forms.ComboBox()
        Me._chkField_16 = New System.Windows.Forms.CheckBox()
        Me._cmbField_13 = New System.Windows.Forms.ComboBox()
        Me._txtField_18 = New System.Windows.Forms.TextBox()
        Me._chkField_15 = New System.Windows.Forms.CheckBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._cmbField_12 = New System.Windows.Forms.ComboBox()
        Me._chkField_10 = New System.Windows.Forms.CheckBox()
        Me._chkField_8 = New System.Windows.Forms.CheckBox()
        Me._chkField_11 = New System.Windows.Forms.CheckBox()
        Me._chkField_9 = New System.Windows.Forms.CheckBox()
        Me._txtField_13 = New System.Windows.Forms.TextBox()
        Me._txtField_12 = New System.Windows.Forms.TextBox()
        Me._txtField_14 = New System.Windows.Forms.TextBox()
        Me._chkField_4 = New System.Windows.Forms.CheckBox()
        Me._txtField_11 = New System.Windows.Forms.TextBox()
        Me.cmdBrandAdd = New System.Windows.Forms.Button()
        Me._txtField_10 = New System.Windows.Forms.TextBox()
        Me._chkField_14 = New System.Windows.Forms.CheckBox()
        Me._chkField_12 = New System.Windows.Forms.CheckBox()
        Me._chkField_7 = New System.Windows.Forms.CheckBox()
        Me._chkField_6 = New System.Windows.Forms.CheckBox()
        Me.chkShipper = New System.Windows.Forms.CheckBox()
        Me.lblCostedWeight = New System.Windows.Forms.CheckBox()
        Me._txtField_9 = New System.Windows.Forms.TextBox()
        Me._txtField_8 = New System.Windows.Forms.TextBox()
        Me._txtField_6 = New System.Windows.Forms.TextBox()
        Me._cmbField_6 = New System.Windows.Forms.ComboBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._cmbField_3 = New System.Windows.Forms.ComboBox()
        Me._txtField_2 = New System.Windows.Forms.TextBox()
        Me._txtField_3 = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me.lblNotAvailableNote = New System.Windows.Forms.Label()
        Me.lblDistSubTeam = New System.Windows.Forms.Label()
        Me.lblNationalClass = New System.Windows.Forms.Label()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblSalesAccount = New System.Windows.Forms.Label()
        Me.lblItemType = New System.Windows.Forms.Label()
        Me.lblSlash2 = New System.Windows.Forms.Label()
        Me.lblTieHigh = New System.Windows.Forms.Label()
        Me.lblYield = New System.Windows.Forms.Label()
        Me.lblAvgUnitWeight = New System.Windows.Forms.Label()
        Me.lblReadOnly = New System.Windows.Forms.Label()
        Me.lblDash = New System.Windows.Forms.Label()
        Me.lblTmpRange = New System.Windows.Forms.Label()
        Me.lblUnitsPerPallet = New System.Windows.Forms.Label()
        Me.lblSlash1 = New System.Windows.Forms.Label()
        Me.lblBrand = New System.Windows.Forms.Label()
        Me.lblOrigin = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblPOSDesc = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.cmbBrand = New System.Windows.Forms.ComboBox()
        Me.cmbTaxClass = New System.Windows.Forms.ComboBox()
        Me.lblTaxClass = New System.Windows.Forms.Label()
        Me.cmbLabelType = New System.Windows.Forms.ComboBox()
        Me.Label_LabelType = New System.Windows.Forms.Label()
        Me.chkField = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me._chkField_18 = New System.Windows.Forms.CheckBox()
        Me._chkField_19 = New System.Windows.Forms.CheckBox()
        Me._chkField_20 = New System.Windows.Forms.CheckBox()
        Me._chkField_21 = New System.Windows.Forms.CheckBox()
        Me._chkField_22 = New System.Windows.Forms.CheckBox()
        Me._chkField_23 = New System.Windows.Forms.CheckBox()
        Me._chkField_24 = New System.Windows.Forms.CheckBox()
        Me._chkField_25 = New System.Windows.Forms.CheckBox()
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.cmbManagedBy = New System.Windows.Forms.ComboBox()
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._txtField_19 = New System.Windows.Forms.TextBox()
        Me._txtField_20 = New System.Windows.Forms.TextBox()
        Me._txtField_21 = New System.Windows.Forms.TextBox()
        Me.rdoField = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.Button_POSSettings = New System.Windows.Forms.Button()
        Me.HierarchySelector1 = New HierarchySelector()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_InsertDate = New System.Windows.Forms.Label()
        Me.lbl_UserIDDate = New System.Windows.Forms.Label()
        Me.lbl_UserID = New System.Windows.Forms.Label()
        Me.GroupBox_RetailPackage = New System.Windows.Forms.GroupBox()
        Me.Label_RetailPackSize = New System.Windows.Forms.Label()
        Me.Label_Pack = New System.Windows.Forms.Label()
        Me.Label_RetailPackUOM = New System.Windows.Forms.Label()
        Me.grpManageBy = New System.Windows.Forms.GroupBox()
        Me.lblPurchTransCouponAmt = New System.Windows.Forms.Label()
        Me.Label_DefaultJurisdiction = New System.Windows.Forms.Label()
        Me.TextBox_DefaultJurisdiction = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ttItemDif = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkSustainRankingRequired = New System.Windows.Forms.CheckBox()
        Me.chkUseLastReceivedCost = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGiftCard = New System.Windows.Forms.CheckBox()
        Me.lblSignCaption = New System.Windows.Forms.LinkLabel()
        Me.fraUnits.SuspendLayout()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rdoField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_RetailPackage.SuspendLayout()
        Me.grpManageBy.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdIdentifier
        '
        resources.ApplyResources(Me.cmdIdentifier, "cmdIdentifier")
        Me.cmdIdentifier.BackColor = System.Drawing.SystemColors.Control
        Me.cmdIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdIdentifier.Name = "cmdIdentifier"
        Me.ToolTip1.SetToolTip(Me.cmdIdentifier, resources.GetString("cmdIdentifier.ToolTip"))
        Me.cmdIdentifier.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdInventory
        '
        resources.ApplyResources(Me.cmdInventory, "cmdInventory")
        Me.cmdInventory.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInventory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInventory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInventory.Name = "cmdInventory"
        Me.ToolTip1.SetToolTip(Me.cmdInventory, resources.GetString("cmdInventory.ToolTip"))
        Me.cmdInventory.UseVisualStyleBackColor = False
        '
        'cmdPrices
        '
        resources.ApplyResources(Me.cmdPrices, "cmdPrices")
        Me.cmdPrices.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrices.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrices.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrices.Name = "cmdPrices"
        Me.ToolTip1.SetToolTip(Me.cmdPrices, resources.GetString("cmdPrices.ToolTip"))
        Me.cmdPrices.UseVisualStyleBackColor = False
        '
        'cmdHistory
        '
        resources.ApplyResources(Me.cmdHistory, "cmdHistory")
        Me.cmdHistory.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHistory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHistory.Name = "cmdHistory"
        Me.ToolTip1.SetToolTip(Me.cmdHistory, resources.GetString("cmdHistory.ToolTip"))
        Me.cmdHistory.UseVisualStyleBackColor = False
        '
        'cmdUnlock
        '
        resources.ApplyResources(Me.cmdUnlock, "cmdUnlock")
        Me.cmdUnlock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnlock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnlock.Name = "cmdUnlock"
        Me.ToolTip1.SetToolTip(Me.cmdUnlock, resources.GetString("cmdUnlock.ToolTip"))
        Me.cmdUnlock.UseVisualStyleBackColor = False
        '
        'cmdShipper
        '
        resources.ApplyResources(Me.cmdShipper, "cmdShipper")
        Me.cmdShipper.BackColor = System.Drawing.SystemColors.Control
        Me.cmdShipper.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdShipper.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdShipper.Name = "cmdShipper"
        Me.ToolTip1.SetToolTip(Me.cmdShipper, resources.GetString("cmdShipper.ToolTip"))
        Me.cmdShipper.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdItemVendors
        '
        resources.ApplyResources(Me.cmdItemVendors, "cmdItemVendors")
        Me.cmdItemVendors.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemVendors.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemVendors.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemVendors.Name = "cmdItemVendors"
        Me.ToolTip1.SetToolTip(Me.cmdItemVendors, resources.GetString("cmdItemVendors.ToolTip"))
        Me.cmdItemVendors.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdTaxClassZoom
        '
        Me.cmdTaxClassZoom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTaxClassZoom.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdTaxClassZoom, "cmdTaxClassZoom")
        Me.cmdTaxClassZoom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTaxClassZoom.Name = "cmdTaxClassZoom"
        Me.ToolTip1.SetToolTip(Me.cmdTaxClassZoom, resources.GetString("cmdTaxClassZoom.ToolTip"))
        Me.cmdTaxClassZoom.UseVisualStyleBackColor = False
        '
        'cmdScale
        '
        resources.ApplyResources(Me.cmdScale, "cmdScale")
        Me.cmdScale.BackColor = System.Drawing.SystemColors.Control
        Me.cmdScale.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdScale.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdScale.Name = "cmdScale"
        Me.ToolTip1.SetToolTip(Me.cmdScale, resources.GetString("cmdScale.ToolTip"))
        Me.cmdScale.UseVisualStyleBackColor = False
        '
        'cmdAttributes
        '
        resources.ApplyResources(Me.cmdAttributes, "cmdAttributes")
        Me.cmdAttributes.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAttributes.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAttributes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAttributes.Name = "cmdAttributes"
        Me.ToolTip1.SetToolTip(Me.cmdAttributes, resources.GetString("cmdAttributes.ToolTip"))
        Me.cmdAttributes.UseVisualStyleBackColor = False
        '
        'cmdJurisdiction
        '
        resources.ApplyResources(Me.cmdJurisdiction, "cmdJurisdiction")
        Me.cmdJurisdiction.BackColor = System.Drawing.SystemColors.Control
        Me.cmdJurisdiction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdJurisdiction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdJurisdiction.Name = "cmdJurisdiction"
        Me.ToolTip1.SetToolTip(Me.cmdJurisdiction, resources.GetString("cmdJurisdiction.ToolTip"))
        Me.cmdJurisdiction.UseVisualStyleBackColor = False
        '
        'cmbSustainRankingDefault
        '
        Me.cmbSustainRankingDefault.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbSustainRankingDefault.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSustainRankingDefault.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSustainRankingDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSustainRankingDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSustainRankingDefault, "cmbSustainRankingDefault")
        Me.cmbSustainRankingDefault.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSustainRankingDefault.Name = "cmbSustainRankingDefault"
        Me.ToolTip1.SetToolTip(Me.cmbSustainRankingDefault, resources.GetString("cmbSustainRankingDefault.ToolTip"))
        '
        'cmdItemSave
        '
        resources.ApplyResources(Me.cmdItemSave, "cmdItemSave")
        Me.cmdItemSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemSave.Name = "cmdItemSave"
        Me.ToolTip1.SetToolTip(Me.cmdItemSave, resources.GetString("cmdItemSave.ToolTip"))
        Me.cmdItemSave.UseVisualStyleBackColor = False
        '
        'ButtonNutrifacts
        '
        resources.ApplyResources(Me.ButtonNutrifacts, "ButtonNutrifacts")
        Me.ButtonNutrifacts.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonNutrifacts.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonNutrifacts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonNutrifacts.Name = "ButtonNutrifacts"
        Me.ToolTip1.SetToolTip(Me.ButtonNutrifacts, resources.GetString("ButtonNutrifacts.ToolTip"))
        Me.ButtonNutrifacts.UseVisualStyleBackColor = False
        '
        'Button365
        '
        resources.ApplyResources(Me.Button365, "Button365")
        Me.Button365.BackColor = System.Drawing.SystemColors.Control
        Me.Button365.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button365.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button365.Name = "Button365"
        Me.ToolTip1.SetToolTip(Me.Button365, resources.GetString("Button365.ToolTip"))
        Me.Button365.UseVisualStyleBackColor = False
        '
        'fraUnits
        '
        Me.fraUnits.BackColor = System.Drawing.SystemColors.Control
        Me.fraUnits.Controls.Add(Me._cmbField_4)
        Me.fraUnits.Controls.Add(Me._cmbField_5)
        Me.fraUnits.Controls.Add(Me._cmbField_8)
        Me.fraUnits.Controls.Add(Me._cmbField_9)
        Me.fraUnits.Controls.Add(Me._cmbField_14)
        Me.fraUnits.Controls.Add(Me.lblRetail)
        Me.fraUnits.Controls.Add(Me.lblVendorOrder)
        Me.fraUnits.Controls.Add(Me.lblDistribution)
        Me.fraUnits.Controls.Add(Me.lblManufacturing)
        resources.ApplyResources(Me.fraUnits, "fraUnits")
        Me.fraUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUnits.Name = "fraUnits"
        Me.fraUnits.TabStop = False
        '
        '_cmbField_4
        '
        Me._cmbField_4.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_4, "_cmbField_4")
        Me._cmbField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_4, CType(4, Short))
        Me._cmbField_4.Name = "_cmbField_4"
        Me._cmbField_4.Sorted = True
        Me._cmbField_4.Tag = "Remove this when ripping out the control array junk."
        '
        '_cmbField_5
        '
        Me._cmbField_5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_5.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_5, "_cmbField_5")
        Me._cmbField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_5, CType(5, Short))
        Me._cmbField_5.Name = "_cmbField_5"
        Me._cmbField_5.Sorted = True
        '
        '_cmbField_8
        '
        Me._cmbField_8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_8.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_8, "_cmbField_8")
        Me._cmbField_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_8, CType(8, Short))
        Me._cmbField_8.Name = "_cmbField_8"
        Me._cmbField_8.Sorted = True
        '
        '_cmbField_9
        '
        Me._cmbField_9.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_9.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_9, "_cmbField_9")
        Me._cmbField_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_9, CType(9, Short))
        Me._cmbField_9.Name = "_cmbField_9"
        Me._cmbField_9.Sorted = True
        '
        '_cmbField_14
        '
        Me._cmbField_14.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_14.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_14.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_14, "_cmbField_14")
        Me._cmbField_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_14, CType(14, Short))
        Me._cmbField_14.Name = "_cmbField_14"
        Me._cmbField_14.Sorted = True
        '
        'lblRetail
        '
        Me.lblRetail.BackColor = System.Drawing.Color.Transparent
        Me.lblRetail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRetail, "lblRetail")
        Me.lblRetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRetail.Name = "lblRetail"
        '
        'lblVendorOrder
        '
        Me.lblVendorOrder.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorOrder.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorOrder, "lblVendorOrder")
        Me.lblVendorOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorOrder.Name = "lblVendorOrder"
        '
        'lblDistribution
        '
        Me.lblDistribution.BackColor = System.Drawing.Color.Transparent
        Me.lblDistribution.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDistribution, "lblDistribution")
        Me.lblDistribution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDistribution.Name = "lblDistribution"
        '
        'lblManufacturing
        '
        Me.lblManufacturing.BackColor = System.Drawing.Color.Transparent
        Me.lblManufacturing.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblManufacturing, "lblManufacturing")
        Me.lblManufacturing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblManufacturing.Name = "lblManufacturing"
        '
        'cmbDistSubTeam
        '
        Me.cmbDistSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbDistSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDistSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDistSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDistSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbDistSubTeam, "cmbDistSubTeam")
        Me.cmbDistSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDistSubTeam.Name = "cmbDistSubTeam"
        Me.cmbDistSubTeam.Sorted = True
        '
        '_cmbField_15
        '
        Me._cmbField_15.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_15.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_15.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_15.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_15, "_cmbField_15")
        Me._cmbField_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_15, CType(15, Short))
        Me._cmbField_15.Name = "_cmbField_15"
        Me._cmbField_15.Sorted = True
        '
        '_chkField_16
        '
        Me._chkField_16.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_16, "_chkField_16")
        Me._chkField_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_16, CType(16, Short))
        Me._chkField_16.Name = "_chkField_16"
        Me._chkField_16.UseVisualStyleBackColor = False
        '
        '_cmbField_13
        '
        Me._cmbField_13.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_13.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_13.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_13, "_cmbField_13")
        Me._cmbField_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_13, CType(13, Short))
        Me._cmbField_13.Name = "_cmbField_13"
        Me._cmbField_13.Sorted = True
        '
        '_txtField_18
        '
        Me._txtField_18.AcceptsReturn = True
        Me._txtField_18.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_18.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_18, "_txtField_18")
        Me._txtField_18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_18, CType(18, Short))
        Me._txtField_18.Name = "_txtField_18"
        Me._txtField_18.Tag = "String"
        '
        '_chkField_15
        '
        Me._chkField_15.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_15, "_chkField_15")
        Me._chkField_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_15, CType(15, Short))
        Me._chkField_15.Name = "_chkField_15"
        Me._chkField_15.UseVisualStyleBackColor = False
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_0, "_txtField_0")
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.Tag = "Number"
        '
        '_cmbField_12
        '
        Me._cmbField_12.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_12.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_12.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_12, "_cmbField_12")
        Me._cmbField_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_12, CType(12, Short))
        Me._cmbField_12.Name = "_cmbField_12"
        '
        '_chkField_10
        '
        Me._chkField_10.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_10, "_chkField_10")
        Me._chkField_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_10, CType(10, Short))
        Me._chkField_10.Name = "_chkField_10"
        Me._chkField_10.UseVisualStyleBackColor = False
        '
        '_chkField_8
        '
        Me._chkField_8.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_8, "_chkField_8")
        Me._chkField_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_8, CType(8, Short))
        Me._chkField_8.Name = "_chkField_8"
        Me._chkField_8.UseVisualStyleBackColor = False
        '
        '_chkField_11
        '
        Me._chkField_11.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_11, "_chkField_11")
        Me._chkField_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_11, CType(11, Short))
        Me._chkField_11.Name = "_chkField_11"
        Me._chkField_11.UseVisualStyleBackColor = False
        '
        '_chkField_9
        '
        Me._chkField_9.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_9, "_chkField_9")
        Me._chkField_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_9, CType(9, Short))
        Me._chkField_9.Name = "_chkField_9"
        Me._chkField_9.UseVisualStyleBackColor = False
        '
        '_txtField_13
        '
        Me._txtField_13.AcceptsReturn = True
        Me._txtField_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_13.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_13, "_txtField_13")
        Me._txtField_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_13, CType(13, Short))
        Me._txtField_13.Name = "_txtField_13"
        Me._txtField_13.Tag = "Number"
        '
        '_txtField_12
        '
        Me._txtField_12.AcceptsReturn = True
        Me._txtField_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_12.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_12, "_txtField_12")
        Me._txtField_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_12, CType(12, Short))
        Me._txtField_12.Name = "_txtField_12"
        Me._txtField_12.Tag = "Number"
        '
        '_txtField_14
        '
        Me._txtField_14.AcceptsReturn = True
        Me._txtField_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_14.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_14, "_txtField_14")
        Me._txtField_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_14, CType(14, Short))
        Me._txtField_14.Name = "_txtField_14"
        Me._txtField_14.Tag = "Number"
        '
        '_chkField_4
        '
        Me._chkField_4.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_4, "_chkField_4")
        Me._chkField_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_4, CType(4, Short))
        Me._chkField_4.Name = "_chkField_4"
        Me._chkField_4.UseVisualStyleBackColor = False
        '
        '_txtField_11
        '
        Me._txtField_11.AcceptsReturn = True
        Me._txtField_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_11.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_11, "_txtField_11")
        Me._txtField_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_11, CType(11, Short))
        Me._txtField_11.Name = "_txtField_11"
        Me._txtField_11.Tag = "ExtCurrency"
        '
        'cmdBrandAdd
        '
        Me.cmdBrandAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrandAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdBrandAdd, "cmdBrandAdd")
        Me.cmdBrandAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrandAdd.Name = "cmdBrandAdd"
        Me.cmdBrandAdd.UseVisualStyleBackColor = False
        '
        '_txtField_10
        '
        Me._txtField_10.AcceptsReturn = True
        Me._txtField_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_10.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_10, "_txtField_10")
        Me._txtField_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_10, CType(10, Short))
        Me._txtField_10.Name = "_txtField_10"
        Me._txtField_10.Tag = "String"
        '
        '_chkField_14
        '
        Me._chkField_14.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_14, "_chkField_14")
        Me._chkField_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_14, CType(14, Short))
        Me._chkField_14.Name = "_chkField_14"
        Me._chkField_14.UseVisualStyleBackColor = False
        '
        '_chkField_12
        '
        Me._chkField_12.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_12, "_chkField_12")
        Me._chkField_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_12, CType(12, Short))
        Me._chkField_12.Name = "_chkField_12"
        Me._chkField_12.UseVisualStyleBackColor = False
        '
        '_chkField_7
        '
        Me._chkField_7.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_7, "_chkField_7")
        Me._chkField_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_7, CType(7, Short))
        Me._chkField_7.Name = "_chkField_7"
        Me._chkField_7.UseVisualStyleBackColor = False
        '
        '_chkField_6
        '
        Me._chkField_6.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_6, "_chkField_6")
        Me._chkField_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_6, CType(6, Short))
        Me._chkField_6.Name = "_chkField_6"
        Me._chkField_6.UseVisualStyleBackColor = False
        '
        'chkShipper
        '
        Me.chkShipper.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkShipper, "chkShipper")
        Me.chkShipper.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShipper.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me.chkShipper, CType(13, Short))
        Me.chkShipper.Name = "chkShipper"
        Me.chkShipper.UseVisualStyleBackColor = False
        '
        'lblCostedWeight
        '
        Me.lblCostedWeight.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.lblCostedWeight, "lblCostedWeight")
        Me.lblCostedWeight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCostedWeight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me.lblCostedWeight, CType(17, Short))
        Me.lblCostedWeight.Name = "lblCostedWeight"
        Me.lblCostedWeight.UseVisualStyleBackColor = False
        '
        '_txtField_9
        '
        Me._txtField_9.AcceptsReturn = True
        Me._txtField_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_9.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_9, "_txtField_9")
        Me._txtField_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_9, CType(9, Short))
        Me._txtField_9.Name = "_txtField_9"
        Me._txtField_9.Tag = "Number"
        '
        '_txtField_8
        '
        Me._txtField_8.AcceptsReturn = True
        Me._txtField_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_8.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_8, "_txtField_8")
        Me._txtField_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_8, CType(8, Short))
        Me._txtField_8.Name = "_txtField_8"
        Me._txtField_8.Tag = "Number"
        '
        '_txtField_6
        '
        Me._txtField_6.AcceptsReturn = True
        Me._txtField_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_6.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_6, "_txtField_6")
        Me._txtField_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_6, CType(6, Short))
        Me._txtField_6.Name = "_txtField_6"
        Me._txtField_6.Tag = "Number"
        '
        '_cmbField_6
        '
        Me._cmbField_6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_6.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_6, "_cmbField_6")
        Me._cmbField_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_6, CType(6, Short))
        Me._cmbField_6.Name = "_cmbField_6"
        Me._cmbField_6.Sorted = True
        '
        '_txtField_5
        '
        Me._txtField_5.AcceptsReturn = True
        Me._txtField_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_5.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_5, "_txtField_5")
        Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
        Me._txtField_5.Name = "_txtField_5"
        Me._txtField_5.Tag = "ExtCurrency"
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_4, "_txtField_4")
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.Tag = "ExtCurrency"
        '
        '_cmbField_3
        '
        Me._cmbField_3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_3.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me._cmbField_3, "_cmbField_3")
        Me._cmbField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_3, CType(3, Short))
        Me._cmbField_3.Name = "_cmbField_3"
        Me._cmbField_3.Sorted = True
        '
        '_txtField_2
        '
        Me._txtField_2.AcceptsReturn = True
        Me._txtField_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_2.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_2, "_txtField_2")
        Me._txtField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_2, CType(2, Short))
        Me._txtField_2.Name = "_txtField_2"
        Me._txtField_2.Tag = "String"
        '
        '_txtField_3
        '
        Me._txtField_3.AcceptsReturn = True
        Me._txtField_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_3.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_3, "_txtField_3")
        Me._txtField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_3, CType(3, Short))
        Me._txtField_3.Name = "_txtField_3"
        Me._txtField_3.Tag = "POSString"
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Control
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_1, "_txtField_1")
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.ReadOnly = True
        Me._txtField_1.TabStop = False
        Me._txtField_1.Tag = "Integer"
        '
        'lblNotAvailableNote
        '
        Me.lblNotAvailableNote.BackColor = System.Drawing.Color.Transparent
        Me.lblNotAvailableNote.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNotAvailableNote, "lblNotAvailableNote")
        Me.lblNotAvailableNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNotAvailableNote.Name = "lblNotAvailableNote"
        '
        'lblDistSubTeam
        '
        Me.lblDistSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblDistSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDistSubTeam, "lblDistSubTeam")
        Me.lblDistSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDistSubTeam.Name = "lblDistSubTeam"
        '
        'lblNationalClass
        '
        Me.lblNationalClass.BackColor = System.Drawing.Color.Transparent
        Me.lblNationalClass.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNationalClass, "lblNationalClass")
        Me.lblNationalClass.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNationalClass.Name = "lblNationalClass"
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.Transparent
        Me.lblCountry.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCountry, "lblCountry")
        Me.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountry.Name = "lblCountry"
        '
        'lblSalesAccount
        '
        Me.lblSalesAccount.BackColor = System.Drawing.Color.Transparent
        Me.lblSalesAccount.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSalesAccount, "lblSalesAccount")
        Me.lblSalesAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSalesAccount.Name = "lblSalesAccount"
        '
        'lblItemType
        '
        Me.lblItemType.BackColor = System.Drawing.Color.Transparent
        Me.lblItemType.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblItemType, "lblItemType")
        Me.lblItemType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItemType.Name = "lblItemType"
        '
        'lblSlash2
        '
        Me.lblSlash2.BackColor = System.Drawing.Color.Transparent
        Me.lblSlash2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSlash2, "lblSlash2")
        Me.lblSlash2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash2.Name = "lblSlash2"
        '
        'lblTieHigh
        '
        Me.lblTieHigh.BackColor = System.Drawing.Color.Transparent
        Me.lblTieHigh.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTieHigh, "lblTieHigh")
        Me.lblTieHigh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTieHigh.Name = "lblTieHigh"
        '
        'lblYield
        '
        Me.lblYield.BackColor = System.Drawing.Color.Transparent
        Me.lblYield.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblYield, "lblYield")
        Me.lblYield.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYield.Name = "lblYield"
        '
        'lblAvgUnitWeight
        '
        Me.lblAvgUnitWeight.BackColor = System.Drawing.Color.Transparent
        Me.lblAvgUnitWeight.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAvgUnitWeight, "lblAvgUnitWeight")
        Me.lblAvgUnitWeight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAvgUnitWeight.Name = "lblAvgUnitWeight"
        '
        'lblReadOnly
        '
        Me.lblReadOnly.BackColor = System.Drawing.Color.Transparent
        Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblReadOnly, "lblReadOnly")
        Me.lblReadOnly.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblReadOnly.Name = "lblReadOnly"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'lblTmpRange
        '
        Me.lblTmpRange.BackColor = System.Drawing.Color.Transparent
        Me.lblTmpRange.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTmpRange, "lblTmpRange")
        Me.lblTmpRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTmpRange.Name = "lblTmpRange"
        '
        'lblUnitsPerPallet
        '
        Me.lblUnitsPerPallet.BackColor = System.Drawing.Color.Transparent
        Me.lblUnitsPerPallet.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblUnitsPerPallet, "lblUnitsPerPallet")
        Me.lblUnitsPerPallet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitsPerPallet.Name = "lblUnitsPerPallet"
        '
        'lblSlash1
        '
        Me.lblSlash1.BackColor = System.Drawing.Color.Transparent
        Me.lblSlash1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSlash1, "lblSlash1")
        Me.lblSlash1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash1.Name = "lblSlash1"
        '
        'lblBrand
        '
        Me.lblBrand.BackColor = System.Drawing.Color.Transparent
        Me.lblBrand.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblBrand, "lblBrand")
        Me.lblBrand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrand.Name = "lblBrand"
        '
        'lblOrigin
        '
        Me.lblOrigin.BackColor = System.Drawing.Color.Transparent
        Me.lblOrigin.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblOrigin, "lblOrigin")
        Me.lblOrigin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOrigin.Name = "lblOrigin"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDescription, "lblDescription")
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Name = "lblDescription"
        '
        'lblPOSDesc
        '
        Me.lblPOSDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblPOSDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPOSDesc, "lblPOSDesc")
        Me.lblPOSDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPOSDesc.Name = "lblPOSDesc"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'cmbBrand
        '
        Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBrand.FormattingEnabled = True
        resources.ApplyResources(Me.cmbBrand, "cmbBrand")
        Me.cmbBrand.Name = "cmbBrand"
        '
        'cmbTaxClass
        '
        Me.cmbTaxClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTaxClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTaxClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTaxClass.FormattingEnabled = True
        resources.ApplyResources(Me.cmbTaxClass, "cmbTaxClass")
        Me.cmbTaxClass.Name = "cmbTaxClass"
        '
        'lblTaxClass
        '
        resources.ApplyResources(Me.lblTaxClass, "lblTaxClass")
        Me.lblTaxClass.Name = "lblTaxClass"
        '
        'cmbLabelType
        '
        Me.cmbLabelType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbLabelType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLabelType.FormattingEnabled = True
        resources.ApplyResources(Me.cmbLabelType, "cmbLabelType")
        Me.cmbLabelType.Name = "cmbLabelType"
        '
        'Label_LabelType
        '
        resources.ApplyResources(Me.Label_LabelType, "Label_LabelType")
        Me.Label_LabelType.Name = "Label_LabelType"
        '
        'chkField
        '
        '
        '_chkField_18
        '
        resources.ApplyResources(Me._chkField_18, "_chkField_18")
        Me.chkField.SetIndex(Me._chkField_18, CType(18, Short))
        Me._chkField_18.Name = "_chkField_18"
        Me._chkField_18.UseVisualStyleBackColor = True
        '
        '_chkField_19
        '
        Me._chkField_19.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_19, "_chkField_19")
        Me._chkField_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_19, CType(19, Short))
        Me._chkField_19.Name = "_chkField_19"
        Me._chkField_19.UseVisualStyleBackColor = False
        '
        '_chkField_20
        '
        Me._chkField_20.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkField_20, "_chkField_20")
        Me._chkField_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_20, CType(20, Short))
        Me._chkField_20.Name = "_chkField_20"
        Me._chkField_20.UseVisualStyleBackColor = False
        '
        '_chkField_21
        '
        resources.ApplyResources(Me._chkField_21, "_chkField_21")
        Me._chkField_21.BackColor = System.Drawing.SystemColors.Control
        Me._chkField_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_21, CType(21, Short))
        Me._chkField_21.Name = "_chkField_21"
        Me._chkField_21.UseVisualStyleBackColor = False
        '
        '_chkField_22
        '
        resources.ApplyResources(Me._chkField_22, "_chkField_22")
        Me._chkField_22.BackColor = System.Drawing.SystemColors.Control
        Me._chkField_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_22, CType(22, Short))
        Me._chkField_22.Name = "_chkField_22"
        Me._chkField_22.UseVisualStyleBackColor = False
        '
        '_chkField_23
        '
        resources.ApplyResources(Me._chkField_23, "_chkField_23")
        Me._chkField_23.BackColor = System.Drawing.SystemColors.Control
        Me._chkField_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_23, CType(23, Short))
        Me._chkField_23.Name = "_chkField_23"
        Me._chkField_23.UseVisualStyleBackColor = False
        '
        '_chkField_24
        '
        resources.ApplyResources(Me._chkField_24, "_chkField_24")
        Me._chkField_24.BackColor = System.Drawing.SystemColors.Control
        Me._chkField_24.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_24, CType(24, Short))
        Me._chkField_24.Name = "_chkField_24"
        Me._chkField_24.UseVisualStyleBackColor = False
        '
        '_chkField_25
        '
        resources.ApplyResources(Me._chkField_25, "_chkField_25")
        Me.chkField.SetIndex(Me._chkField_25, CType(25, Short))
        Me._chkField_25.Name = "_chkField_25"
        Me._chkField_25.UseVisualStyleBackColor = True
        '
        'cmbField
        '
        '
        'cmbManagedBy
        '
        Me.cmbManagedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbManagedBy, "cmbManagedBy")
        Me.cmbManagedBy.FormattingEnabled = True
        Me.cmbField.SetIndex(Me.cmbManagedBy, CType(16, Short))
        Me.cmbManagedBy.Name = "cmbManagedBy"
        '
        'txtField
        '
        '
        '_txtField_19
        '
        Me._txtField_19.AcceptsReturn = True
        Me._txtField_19.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_19.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_19, "_txtField_19")
        Me._txtField_19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_19, CType(19, Short))
        Me._txtField_19.Name = "_txtField_19"
        Me._txtField_19.Tag = "ExtCurrency"
        '
        '_txtField_20
        '
        Me._txtField_20.AcceptsReturn = True
        Me._txtField_20.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_20.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_20, "_txtField_20")
        Me._txtField_20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_20, CType(20, Short))
        Me._txtField_20.Name = "_txtField_20"
        Me._txtField_20.Tag = "ExtCurrency"
        '
        '_txtField_21
        '
        Me._txtField_21.AcceptsReturn = True
        Me._txtField_21.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_21.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_21, "_txtField_21")
        Me._txtField_21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_21, CType(21, Short))
        Me._txtField_21.Name = "_txtField_21"
        Me._txtField_21.Tag = "ExtCurrency"
        '
        'Button_POSSettings
        '
        resources.ApplyResources(Me.Button_POSSettings, "Button_POSSettings")
        Me.Button_POSSettings.Name = "Button_POSSettings"
        Me.Button_POSSettings.UseVisualStyleBackColor = True
        '
        'HierarchySelector1
        '
        resources.ApplyResources(Me.HierarchySelector1, "HierarchySelector1")
        Me.HierarchySelector1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.HierarchySelector1.ItemIdentifier = Nothing
        Me.HierarchySelector1.Name = "HierarchySelector1"
        Me.HierarchySelector1.SelectedCategoryId = 0
        Me.HierarchySelector1.SelectedLevel3Id = 0
        Me.HierarchySelector1.SelectedLevel4Id = 0
        Me.HierarchySelector1.SelectedSubTeamId = 0
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.TextBox1, "TextBox1")
        Me.TextBox1.Name = "TextBox1"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'lbl_InsertDate
        '
        resources.ApplyResources(Me.lbl_InsertDate, "lbl_InsertDate")
        Me.lbl_InsertDate.Name = "lbl_InsertDate"
        '
        'lbl_UserIDDate
        '
        resources.ApplyResources(Me.lbl_UserIDDate, "lbl_UserIDDate")
        Me.lbl_UserIDDate.Name = "lbl_UserIDDate"
        '
        'lbl_UserID
        '
        resources.ApplyResources(Me.lbl_UserID, "lbl_UserID")
        Me.lbl_UserID.Name = "lbl_UserID"
        '
        'GroupBox_RetailPackage
        '
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackSize)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_Pack)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackUOM)
        Me.GroupBox_RetailPackage.Controls.Add(Me._cmbField_6)
        Me.GroupBox_RetailPackage.Controls.Add(Me.lblSlash1)
        Me.GroupBox_RetailPackage.Controls.Add(Me._txtField_4)
        Me.GroupBox_RetailPackage.Controls.Add(Me._txtField_5)
        resources.ApplyResources(Me.GroupBox_RetailPackage, "GroupBox_RetailPackage")
        Me.GroupBox_RetailPackage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_RetailPackage.Name = "GroupBox_RetailPackage"
        Me.GroupBox_RetailPackage.TabStop = False
        '
        'Label_RetailPackSize
        '
        Me.Label_RetailPackSize.BackColor = System.Drawing.Color.Transparent
        Me.Label_RetailPackSize.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_RetailPackSize, "Label_RetailPackSize")
        Me.Label_RetailPackSize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_RetailPackSize.Name = "Label_RetailPackSize"
        '
        'Label_Pack
        '
        Me.Label_Pack.BackColor = System.Drawing.Color.Transparent
        Me.Label_Pack.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Pack, "Label_Pack")
        Me.Label_Pack.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Pack.Name = "Label_Pack"
        '
        'Label_RetailPackUOM
        '
        Me.Label_RetailPackUOM.BackColor = System.Drawing.Color.Transparent
        Me.Label_RetailPackUOM.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_RetailPackUOM, "Label_RetailPackUOM")
        Me.Label_RetailPackUOM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_RetailPackUOM.Name = "Label_RetailPackUOM"
        '
        'grpManageBy
        '
        Me.grpManageBy.Controls.Add(Me.cmbManagedBy)
        resources.ApplyResources(Me.grpManageBy, "grpManageBy")
        Me.grpManageBy.ForeColor = System.Drawing.Color.Black
        Me.grpManageBy.Name = "grpManageBy"
        Me.grpManageBy.TabStop = False
        '
        'lblPurchTransCouponAmt
        '
        Me.lblPurchTransCouponAmt.BackColor = System.Drawing.Color.Transparent
        Me.lblPurchTransCouponAmt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPurchTransCouponAmt, "lblPurchTransCouponAmt")
        Me.lblPurchTransCouponAmt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchTransCouponAmt.Name = "lblPurchTransCouponAmt"
        '
        'Label_DefaultJurisdiction
        '
        resources.ApplyResources(Me.Label_DefaultJurisdiction, "Label_DefaultJurisdiction")
        Me.Label_DefaultJurisdiction.Name = "Label_DefaultJurisdiction"
        '
        'TextBox_DefaultJurisdiction
        '
        Me.TextBox_DefaultJurisdiction.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.TextBox_DefaultJurisdiction, "TextBox_DefaultJurisdiction")
        Me.TextBox_DefaultJurisdiction.Name = "TextBox_DefaultJurisdiction"
        Me.TextBox_DefaultJurisdiction.ReadOnly = True
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Name = "Label5"
        '
        'ttItemDif
        '
        Me.ttItemDif.AutoPopDelay = 5000
        Me.ttItemDif.InitialDelay = 500
        Me.ttItemDif.ReshowDelay = 100
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkSustainRankingRequired)
        Me.GroupBox2.Controls.Add(Me.cmbSustainRankingDefault)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'chkSustainRankingRequired
        '
        Me.chkSustainRankingRequired.BackColor = System.Drawing.SystemColors.Control
        Me.chkSustainRankingRequired.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkSustainRankingRequired, "chkSustainRankingRequired")
        Me.chkSustainRankingRequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSustainRankingRequired.Name = "chkSustainRankingRequired"
        Me.chkSustainRankingRequired.UseVisualStyleBackColor = False
        '
        'chkUseLastReceivedCost
        '
        Me.chkUseLastReceivedCost.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkUseLastReceivedCost, "chkUseLastReceivedCost")
        Me.chkUseLastReceivedCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseLastReceivedCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseLastReceivedCost.Name = "chkUseLastReceivedCost"
        Me.chkUseLastReceivedCost.UseVisualStyleBackColor = False
        '
        'CheckBoxGiftCard
        '
        Me.CheckBoxGiftCard.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.CheckBoxGiftCard, "CheckBoxGiftCard")
        Me.CheckBoxGiftCard.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBoxGiftCard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBoxGiftCard.Name = "CheckBoxGiftCard"
        Me.CheckBoxGiftCard.UseVisualStyleBackColor = False
        '
        'lblSignCaption
        '
        resources.ApplyResources(Me.lblSignCaption, "lblSignCaption")
        Me.lblSignCaption.Name = "lblSignCaption"
        Me.lblSignCaption.TabStop = True
        '
        'frmItem
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me._chkField_25)
        Me.Controls.Add(Me.Button365)
        Me.Controls.Add(Me.lblSignCaption)
        Me.Controls.Add(Me.ButtonNutrifacts)
        Me.Controls.Add(Me.CheckBoxGiftCard)
        Me.Controls.Add(Me.chkUseLastReceivedCost)
        Me.Controls.Add(Me.cmdItemSave)
        Me.Controls.Add(Me._chkField_24)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me._chkField_23)
        Me.Controls.Add(Me._chkField_22)
        Me.Controls.Add(Me._chkField_21)
        Me.Controls.Add(Me._txtField_21)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me._txtField_20)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmdJurisdiction)
        Me.Controls.Add(Me.TextBox_DefaultJurisdiction)
        Me.Controls.Add(Me.Label_DefaultJurisdiction)
        Me.Controls.Add(Me._txtField_19)
        Me.Controls.Add(Me._chkField_20)
        Me.Controls.Add(Me.lblPurchTransCouponAmt)
        Me.Controls.Add(Me._chkField_19)
        Me.Controls.Add(Me._chkField_18)
        Me.Controls.Add(Me.GroupBox_RetailPackage)
        Me.Controls.Add(Me.cmdAttributes)
        Me.Controls.Add(Me.lbl_UserID)
        Me.Controls.Add(Me.lbl_UserIDDate)
        Me.Controls.Add(Me.lbl_InsertDate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.HierarchySelector1)
        Me.Controls.Add(Me.Button_POSSettings)
        Me.Controls.Add(Me.cmdScale)
        Me.Controls.Add(Me.cmdTaxClassZoom)
        Me.Controls.Add(Me.Label_LabelType)
        Me.Controls.Add(Me.cmbLabelType)
        Me.Controls.Add(Me.lblTaxClass)
        Me.Controls.Add(Me.cmbTaxClass)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.fraUnits)
        Me.Controls.Add(Me.cmbDistSubTeam)
        Me.Controls.Add(Me._cmbField_15)
        Me.Controls.Add(Me._chkField_16)
        Me.Controls.Add(Me._cmbField_13)
        Me.Controls.Add(Me._txtField_18)
        Me.Controls.Add(Me._chkField_15)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._cmbField_12)
        Me.Controls.Add(Me._chkField_10)
        Me.Controls.Add(Me._chkField_8)
        Me.Controls.Add(Me._chkField_11)
        Me.Controls.Add(Me._chkField_9)
        Me.Controls.Add(Me._txtField_13)
        Me.Controls.Add(Me._txtField_12)
        Me.Controls.Add(Me._txtField_14)
        Me.Controls.Add(Me._chkField_4)
        Me.Controls.Add(Me._txtField_11)
        Me.Controls.Add(Me.cmdBrandAdd)
        Me.Controls.Add(Me._txtField_10)
        Me.Controls.Add(Me.cmdIdentifier)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdInventory)
        Me.Controls.Add(Me._chkField_14)
        Me.Controls.Add(Me.cmdPrices)
        Me.Controls.Add(Me.cmdHistory)
        Me.Controls.Add(Me.cmdUnlock)
        Me.Controls.Add(Me._chkField_12)
        Me.Controls.Add(Me._chkField_7)
        Me.Controls.Add(Me._chkField_6)
        Me.Controls.Add(Me.chkShipper)
        Me.Controls.Add(Me.lblCostedWeight)
        Me.Controls.Add(Me.cmdShipper)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmdItemVendors)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me._txtField_9)
        Me.Controls.Add(Me._txtField_8)
        Me.Controls.Add(Me._txtField_6)
        Me.Controls.Add(Me._cmbField_3)
        Me.Controls.Add(Me._txtField_2)
        Me.Controls.Add(Me._txtField_3)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.lblNotAvailableNote)
        Me.Controls.Add(Me.lblDistSubTeam)
        Me.Controls.Add(Me.lblNationalClass)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblSalesAccount)
        Me.Controls.Add(Me.lblItemType)
        Me.Controls.Add(Me.lblSlash2)
        Me.Controls.Add(Me.lblTieHigh)
        Me.Controls.Add(Me.lblYield)
        Me.Controls.Add(Me.lblAvgUnitWeight)
        Me.Controls.Add(Me.lblReadOnly)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.lblTmpRange)
        Me.Controls.Add(Me.lblUnitsPerPallet)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.lblOrigin)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblPOSDesc)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.grpManageBy)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItem"
        Me.ShowInTaskbar = False
        Me.fraUnits.ResumeLayout(False)
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rdoField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_RetailPackage.ResumeLayout(False)
        Me.GroupBox_RetailPackage.PerformLayout()
        Me.grpManageBy.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbBrand As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTaxClass As System.Windows.Forms.ComboBox
    Friend WithEvents lblTaxClass As System.Windows.Forms.Label
    Friend WithEvents cmbLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents Label_LabelType As System.Windows.Forms.Label
    Public WithEvents cmdTaxClassZoom As System.Windows.Forms.Button
    Public WithEvents cmdScale As System.Windows.Forms.Button
    Friend WithEvents Button_POSSettings As System.Windows.Forms.Button
    Friend WithEvents HierarchySelector1 As HierarchySelector
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_InsertDate As System.Windows.Forms.Label
    Friend WithEvents lbl_UserIDDate As System.Windows.Forms.Label
    Friend WithEvents lbl_UserID As System.Windows.Forms.Label
    Public WithEvents cmdAttributes As System.Windows.Forms.Button
    Friend WithEvents GroupBox_RetailPackage As System.Windows.Forms.GroupBox
    Public WithEvents Label_RetailPackUOM As System.Windows.Forms.Label
    Public WithEvents Label_Pack As System.Windows.Forms.Label
    Public WithEvents Label_RetailPackSize As System.Windows.Forms.Label
    Friend WithEvents grpManageBy As System.Windows.Forms.GroupBox
    Friend WithEvents cmbManagedBy As System.Windows.Forms.ComboBox
    Friend WithEvents _chkField_18 As System.Windows.Forms.CheckBox
    Friend WithEvents _chkField_19 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_20 As System.Windows.Forms.CheckBox
    Public WithEvents lblPurchTransCouponAmt As System.Windows.Forms.Label
    Public WithEvents _txtField_19 As System.Windows.Forms.TextBox
    Friend WithEvents Label_DefaultJurisdiction As System.Windows.Forms.Label
    Friend WithEvents TextBox_DefaultJurisdiction As System.Windows.Forms.TextBox
    Public WithEvents cmdJurisdiction As System.Windows.Forms.Button
    Public WithEvents _txtField_20 As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents _txtField_21 As System.Windows.Forms.TextBox
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents _chkField_21 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_22 As System.Windows.Forms.CheckBox
    Public WithEvents _chkField_23 As System.Windows.Forms.CheckBox
    Public WithEvents ttItemDif As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents chkSustainRankingRequired As System.Windows.Forms.CheckBox
    Public WithEvents cmbSustainRankingDefault As System.Windows.Forms.ComboBox
    Public WithEvents _chkField_24 As System.Windows.Forms.CheckBox
    Public WithEvents cmdItemSave As System.Windows.Forms.Button
    Public WithEvents chkUseLastReceivedCost As System.Windows.Forms.CheckBox
    Public WithEvents CheckBoxGiftCard As System.Windows.Forms.CheckBox
    Public WithEvents ButtonNutrifacts As System.Windows.Forms.Button
    Public WithEvents lblSignCaption As System.Windows.Forms.LinkLabel
    Public WithEvents Button365 As Button
    Friend WithEvents _chkField_25 As CheckBox

#End Region
End Class
