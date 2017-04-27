<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ItemOverride
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        IsInitializing = True
        InitializeComponent()
        IsInitializing = False
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
    Public WithEvents ComboBox_Retail As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox_VendorOrder As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox_Distribution As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox_Manufacturing As System.Windows.Forms.ComboBox
    Public WithEvents Label_Retail As System.Windows.Forms.Label
    Public WithEvents Label_VendorOrder As System.Windows.Forms.Label
    Public WithEvents Label_Distribution As System.Windows.Forms.Label
    Public WithEvents Label_Manufacturing As System.Windows.Forms.Label
    Public WithEvents GroupBox_Units As System.Windows.Forms.GroupBox
    Public WithEvents TextBox_SignCaption As System.Windows.Forms.TextBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents ComboBox_UnitOfMeasure As System.Windows.Forms.ComboBox
    Public WithEvents TextBox_Size As System.Windows.Forms.TextBox
    Public WithEvents TextBox_Pack As System.Windows.Forms.TextBox
    Public WithEvents TextBox_Description As System.Windows.Forms.TextBox
    Public WithEvents TextBox_POSDesc As System.Windows.Forms.TextBox
    Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Public WithEvents Label_SignCaption As System.Windows.Forms.Label
    Public WithEvents lblSlash1 As System.Windows.Forms.Label
    Public WithEvents Label_Description As System.Windows.Forms.Label
    Public WithEvents Label_POSDesc As System.Windows.Forms.Label
    Public WithEvents Label_Identifier As System.Windows.Forms.Label
    Public WithEvents chkField As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ItemOverride))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.Button_RefreshItemInfo = New System.Windows.Forms.Button()
        Me.Button_RefreshPOSData = New System.Windows.Forms.Button()
        Me.ButtonSaveItemInformation = New System.Windows.Forms.Button()
        Me.ButtonSavePOSSettings = New System.Windows.Forms.Button()
        Me.GroupBox_Units = New System.Windows.Forms.GroupBox()
        Me._cmbField_4 = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Retail = New System.Windows.Forms.ComboBox()
        Me.ComboBox_VendorOrder = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Distribution = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Manufacturing = New System.Windows.Forms.ComboBox()
        Me.Label_Retail = New System.Windows.Forms.Label()
        Me.Label_VendorOrder = New System.Windows.Forms.Label()
        Me.Label_Distribution = New System.Windows.Forms.Label()
        Me.Label_Manufacturing = New System.Windows.Forms.Label()
        Me.TextBox_SignCaption = New System.Windows.Forms.TextBox()
        Me.ComboBox_UnitOfMeasure = New System.Windows.Forms.ComboBox()
        Me.TextBox_Size = New System.Windows.Forms.TextBox()
        Me.TextBox_Pack = New System.Windows.Forms.TextBox()
        Me.TextBox_Description = New System.Windows.Forms.TextBox()
        Me.TextBox_POSDesc = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me.Label_SignCaption = New System.Windows.Forms.Label()
        Me.lblSlash1 = New System.Windows.Forms.Label()
        Me.Label_Description = New System.Windows.Forms.Label()
        Me.Label_POSDesc = New System.Windows.Forms.Label()
        Me.Label_Identifier = New System.Windows.Forms.Label()
        Me.chkField = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.GroupBox_RetailPackage = New System.Windows.Forms.GroupBox()
        Me.Label_RetailPackSize = New System.Windows.Forms.Label()
        Me.Label_Pack = New System.Windows.Forms.Label()
        Me.Label_RetailPackUOM = New System.Windows.Forms.Label()
        Me.Label_AlternateJurisdiction = New System.Windows.Forms.Label()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPage_ItemInfo = New System.Windows.Forms.TabPage()
        Me.GroupBoxAttributes = New System.Windows.Forms.GroupBox()
        Me.LabelSustainabilityRanking = New System.Windows.Forms.Label()
        Me.CheckBoxSustainabilityRanking = New System.Windows.Forms.CheckBox()
        Me.ComboBoxSustainabilityRanking = New System.Windows.Forms.ComboBox()
        Me.ButtonBrandAdd = New System.Windows.Forms.Button()
        Me.ComboBoxBrand = New System.Windows.Forms.ComboBox()
        Me.LabelBrand = New System.Windows.Forms.Label()
        Me.CheckBoxCostedByWeight = New System.Windows.Forms.CheckBox()
        Me.ComboBoxOrigin = New System.Windows.Forms.ComboBox()
        Me.LabelOrigin = New System.Windows.Forms.Label()
        Me.ComboBoxLabelType = New System.Windows.Forms.ComboBox()
        Me.CheckBoxLockAuth = New System.Windows.Forms.CheckBox()
        Me.ComboBoxCountryOfProc = New System.Windows.Forms.ComboBox()
        Me.LabelLabelType = New System.Windows.Forms.Label()
        Me.CheckBoxIngredient = New System.Windows.Forms.CheckBox()
        Me.LabelCountryOfProc = New System.Windows.Forms.Label()
        Me.CheckBoxRecall = New System.Windows.Forms.CheckBox()
        Me.CheckBoxNotAvailable = New System.Windows.Forms.CheckBox()
        Me.LabelNotAvailableNote = New System.Windows.Forms.Label()
        Me.TextBoxNotAvailableNote = New System.Windows.Forms.TextBox()
        Me.TabPage_POSInfo = New System.Windows.Forms.TabPage()
        Me.GroupBox_Common = New System.Windows.Forms.GroupBox()
        Me.LabelUnitPriceCategory = New System.Windows.Forms.Label()
        Me.LabelProductCode = New System.Windows.Forms.Label()
        Me.TextBoxProductCode = New System.Windows.Forms.TextBox()
        Me.TextBoxUnitPriceCategory = New System.Windows.Forms.TextBox()
        Me.CheckBoxFSAEligible = New System.Windows.Forms.CheckBox()
        Me.TextBox_MiscTransSale = New System.Windows.Forms.TextBox()
        Me.TextBox_MiscTransRefund = New System.Windows.Forms.TextBox()
        Me.Label_IceTare = New System.Windows.Forms.Label()
        Me.TextBox_IceTare = New System.Windows.Forms.TextBox()
        Me.Label_MiscTransRefund = New System.Windows.Forms.Label()
        Me.Label_MiscTransSale = New System.Windows.Forms.Label()
        Me.CheckBox_CouponMultiplier = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CaseDiscount = New System.Windows.Forms.CheckBox()
        Me.Label_GroupList = New System.Windows.Forms.Label()
        Me.TextBox_GroupList = New System.Windows.Forms.TextBox()
        Me.CheckBox_PriceRequired = New System.Windows.Forms.CheckBox()
        Me.CheckBox_QuantityProhibit = New System.Windows.Forms.CheckBox()
        Me.CheckBox_QuantityRequired = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FoodStamps = New System.Windows.Forms.CheckBox()
        Me.ComboBox_AltJurisdiction = New System.Windows.Forms.ComboBox()
        Me.GroupBox_Units.SuspendLayout()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_RetailPackage.SuspendLayout()
        Me.TabControl.SuspendLayout()
        Me.TabPage_ItemInfo.SuspendLayout()
        Me.GroupBoxAttributes.SuspendLayout()
        Me.TabPage_POSInfo.SuspendLayout()
        Me.GroupBox_Common.SuspendLayout()
        Me.SuspendLayout()
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
        'Button_RefreshItemInfo
        '
        resources.ApplyResources(Me.Button_RefreshItemInfo, "Button_RefreshItemInfo")
        Me.Button_RefreshItemInfo.BackColor = System.Drawing.SystemColors.Control
        Me.Button_RefreshItemInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_RefreshItemInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_RefreshItemInfo.Name = "Button_RefreshItemInfo"
        Me.ToolTip1.SetToolTip(Me.Button_RefreshItemInfo, resources.GetString("Button_RefreshItemInfo.ToolTip"))
        Me.Button_RefreshItemInfo.UseVisualStyleBackColor = False
        '
        'Button_RefreshPOSData
        '
        resources.ApplyResources(Me.Button_RefreshPOSData, "Button_RefreshPOSData")
        Me.Button_RefreshPOSData.BackColor = System.Drawing.SystemColors.Control
        Me.Button_RefreshPOSData.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_RefreshPOSData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_RefreshPOSData.Name = "Button_RefreshPOSData"
        Me.ToolTip1.SetToolTip(Me.Button_RefreshPOSData, resources.GetString("Button_RefreshPOSData.ToolTip"))
        Me.Button_RefreshPOSData.UseVisualStyleBackColor = False
        '
        'ButtonSaveItemInformation
        '
        resources.ApplyResources(Me.ButtonSaveItemInformation, "ButtonSaveItemInformation")
        Me.ButtonSaveItemInformation.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonSaveItemInformation.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSaveItemInformation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSaveItemInformation.Name = "ButtonSaveItemInformation"
        Me.ToolTip1.SetToolTip(Me.ButtonSaveItemInformation, resources.GetString("ButtonSaveItemInformation.ToolTip"))
        Me.ButtonSaveItemInformation.UseVisualStyleBackColor = False
        '
        'ButtonSavePOSSettings
        '
        resources.ApplyResources(Me.ButtonSavePOSSettings, "ButtonSavePOSSettings")
        Me.ButtonSavePOSSettings.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonSavePOSSettings.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSavePOSSettings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSavePOSSettings.Name = "ButtonSavePOSSettings"
        Me.ToolTip1.SetToolTip(Me.ButtonSavePOSSettings, resources.GetString("ButtonSavePOSSettings.ToolTip"))
        Me.ButtonSavePOSSettings.UseVisualStyleBackColor = False
        '
        'GroupBox_Units
        '
        Me.GroupBox_Units.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox_Units.Controls.Add(Me._cmbField_4)
        Me.GroupBox_Units.Controls.Add(Me.ComboBox_Retail)
        Me.GroupBox_Units.Controls.Add(Me.ComboBox_VendorOrder)
        Me.GroupBox_Units.Controls.Add(Me.ComboBox_Distribution)
        Me.GroupBox_Units.Controls.Add(Me.ComboBox_Manufacturing)
        Me.GroupBox_Units.Controls.Add(Me.Label_Retail)
        Me.GroupBox_Units.Controls.Add(Me.Label_VendorOrder)
        Me.GroupBox_Units.Controls.Add(Me.Label_Distribution)
        Me.GroupBox_Units.Controls.Add(Me.Label_Manufacturing)
        resources.ApplyResources(Me.GroupBox_Units, "GroupBox_Units")
        Me.GroupBox_Units.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_Units.Name = "GroupBox_Units"
        Me.GroupBox_Units.TabStop = False
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
        'ComboBox_Retail
        '
        Me.ComboBox_Retail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Retail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Retail.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_Retail.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_Retail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_Retail, "ComboBox_Retail")
        Me.ComboBox_Retail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me.ComboBox_Retail, CType(5, Short))
        Me.ComboBox_Retail.Items.AddRange(New Object() {resources.GetString("ComboBox_Retail.Items")})
        Me.ComboBox_Retail.Name = "ComboBox_Retail"
        Me.ComboBox_Retail.Sorted = True
        '
        'ComboBox_VendorOrder
        '
        Me.ComboBox_VendorOrder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_VendorOrder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_VendorOrder.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_VendorOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_VendorOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_VendorOrder, "ComboBox_VendorOrder")
        Me.ComboBox_VendorOrder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me.ComboBox_VendorOrder, CType(8, Short))
        Me.ComboBox_VendorOrder.Items.AddRange(New Object() {resources.GetString("ComboBox_VendorOrder.Items")})
        Me.ComboBox_VendorOrder.Name = "ComboBox_VendorOrder"
        Me.ComboBox_VendorOrder.Sorted = True
        '
        'ComboBox_Distribution
        '
        Me.ComboBox_Distribution.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Distribution.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Distribution.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_Distribution.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_Distribution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_Distribution, "ComboBox_Distribution")
        Me.ComboBox_Distribution.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me.ComboBox_Distribution, CType(9, Short))
        Me.ComboBox_Distribution.Items.AddRange(New Object() {resources.GetString("ComboBox_Distribution.Items")})
        Me.ComboBox_Distribution.Name = "ComboBox_Distribution"
        Me.ComboBox_Distribution.Sorted = True
        '
        'ComboBox_Manufacturing
        '
        Me.ComboBox_Manufacturing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Manufacturing.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Manufacturing.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_Manufacturing.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_Manufacturing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_Manufacturing, "ComboBox_Manufacturing")
        Me.ComboBox_Manufacturing.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me.ComboBox_Manufacturing, CType(14, Short))
        Me.ComboBox_Manufacturing.Items.AddRange(New Object() {resources.GetString("ComboBox_Manufacturing.Items")})
        Me.ComboBox_Manufacturing.Name = "ComboBox_Manufacturing"
        Me.ComboBox_Manufacturing.Sorted = True
        '
        'Label_Retail
        '
        Me.Label_Retail.BackColor = System.Drawing.Color.Transparent
        Me.Label_Retail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Retail, "Label_Retail")
        Me.Label_Retail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Retail.Name = "Label_Retail"
        '
        'Label_VendorOrder
        '
        Me.Label_VendorOrder.BackColor = System.Drawing.Color.Transparent
        Me.Label_VendorOrder.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_VendorOrder, "Label_VendorOrder")
        Me.Label_VendorOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_VendorOrder.Name = "Label_VendorOrder"
        '
        'Label_Distribution
        '
        Me.Label_Distribution.BackColor = System.Drawing.Color.Transparent
        Me.Label_Distribution.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Distribution, "Label_Distribution")
        Me.Label_Distribution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Distribution.Name = "Label_Distribution"
        '
        'Label_Manufacturing
        '
        Me.Label_Manufacturing.BackColor = System.Drawing.Color.Transparent
        Me.Label_Manufacturing.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Manufacturing, "Label_Manufacturing")
        Me.Label_Manufacturing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Manufacturing.Name = "Label_Manufacturing"
        '
        'TextBox_SignCaption
        '
        Me.TextBox_SignCaption.AcceptsReturn = True
        Me.TextBox_SignCaption.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_SignCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_SignCaption, "TextBox_SignCaption")
        Me.TextBox_SignCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBox_SignCaption, CType(10, Short))
        Me.TextBox_SignCaption.Name = "TextBox_SignCaption"
        Me.TextBox_SignCaption.Tag = "String"
        '
        'ComboBox_UnitOfMeasure
        '
        Me.ComboBox_UnitOfMeasure.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_UnitOfMeasure.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_UnitOfMeasure.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_UnitOfMeasure.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_UnitOfMeasure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_UnitOfMeasure, "ComboBox_UnitOfMeasure")
        Me.ComboBox_UnitOfMeasure.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me.ComboBox_UnitOfMeasure, CType(6, Short))
        Me.ComboBox_UnitOfMeasure.Items.AddRange(New Object() {resources.GetString("ComboBox_UnitOfMeasure.Items")})
        Me.ComboBox_UnitOfMeasure.Name = "ComboBox_UnitOfMeasure"
        Me.ComboBox_UnitOfMeasure.Sorted = True
        '
        'TextBox_Size
        '
        Me.TextBox_Size.AcceptsReturn = True
        Me.TextBox_Size.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_Size.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_Size, "TextBox_Size")
        Me.TextBox_Size.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBox_Size, CType(5, Short))
        Me.TextBox_Size.Name = "TextBox_Size"
        Me.TextBox_Size.Tag = "ExtCurrency"
        '
        'TextBox_Pack
        '
        Me.TextBox_Pack.AcceptsReturn = True
        Me.TextBox_Pack.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_Pack.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_Pack, "TextBox_Pack")
        Me.TextBox_Pack.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBox_Pack, CType(4, Short))
        Me.TextBox_Pack.Name = "TextBox_Pack"
        Me.TextBox_Pack.Tag = "ExtCurrency"
        '
        'TextBox_Description
        '
        Me.TextBox_Description.AcceptsReturn = True
        Me.TextBox_Description.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_Description.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_Description, "TextBox_Description")
        Me.TextBox_Description.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBox_Description, CType(2, Short))
        Me.TextBox_Description.Name = "TextBox_Description"
        Me.TextBox_Description.Tag = "String"
        '
        'TextBox_POSDesc
        '
        Me.TextBox_POSDesc.AcceptsReturn = True
        Me.TextBox_POSDesc.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_POSDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_POSDesc, "TextBox_POSDesc")
        Me.TextBox_POSDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBox_POSDesc, CType(3, Short))
        Me.TextBox_POSDesc.Name = "TextBox_POSDesc"
        Me.TextBox_POSDesc.Tag = "POSString"
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_1, "_txtField_1")
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.ReadOnly = True
        Me._txtField_1.TabStop = False
        Me._txtField_1.Tag = "Integer"
        '
        'Label_SignCaption
        '
        Me.Label_SignCaption.BackColor = System.Drawing.Color.Transparent
        Me.Label_SignCaption.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_SignCaption, "Label_SignCaption")
        Me.Label_SignCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_SignCaption.Name = "Label_SignCaption"
        '
        'lblSlash1
        '
        Me.lblSlash1.BackColor = System.Drawing.Color.Transparent
        Me.lblSlash1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSlash1, "lblSlash1")
        Me.lblSlash1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash1.Name = "lblSlash1"
        '
        'Label_Description
        '
        Me.Label_Description.BackColor = System.Drawing.Color.Transparent
        Me.Label_Description.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Description, "Label_Description")
        Me.Label_Description.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Description.Name = "Label_Description"
        '
        'Label_POSDesc
        '
        Me.Label_POSDesc.BackColor = System.Drawing.Color.Transparent
        Me.Label_POSDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_POSDesc, "Label_POSDesc")
        Me.Label_POSDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_POSDesc.Name = "Label_POSDesc"
        '
        'Label_Identifier
        '
        Me.Label_Identifier.BackColor = System.Drawing.Color.Transparent
        Me.Label_Identifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_Identifier, "Label_Identifier")
        Me.Label_Identifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Identifier.Name = "Label_Identifier"
        '
        'GroupBox_RetailPackage
        '
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackSize)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_Pack)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackUOM)
        Me.GroupBox_RetailPackage.Controls.Add(Me.ComboBox_UnitOfMeasure)
        Me.GroupBox_RetailPackage.Controls.Add(Me.lblSlash1)
        Me.GroupBox_RetailPackage.Controls.Add(Me.TextBox_Pack)
        Me.GroupBox_RetailPackage.Controls.Add(Me.TextBox_Size)
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
        'Label_AlternateJurisdiction
        '
        resources.ApplyResources(Me.Label_AlternateJurisdiction, "Label_AlternateJurisdiction")
        Me.Label_AlternateJurisdiction.Name = "Label_AlternateJurisdiction"
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPage_ItemInfo)
        Me.TabControl.Controls.Add(Me.TabPage_POSInfo)
        resources.ApplyResources(Me.TabControl, "TabControl")
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        '
        'TabPage_ItemInfo
        '
        Me.TabPage_ItemInfo.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_ItemInfo.Controls.Add(Me.GroupBoxAttributes)
        Me.TabPage_ItemInfo.Controls.Add(Me.ButtonSaveItemInformation)
        Me.TabPage_ItemInfo.Controls.Add(Me.CheckBoxNotAvailable)
        Me.TabPage_ItemInfo.Controls.Add(Me.LabelNotAvailableNote)
        Me.TabPage_ItemInfo.Controls.Add(Me.TextBoxNotAvailableNote)
        Me.TabPage_ItemInfo.Controls.Add(Me.Button_RefreshItemInfo)
        Me.TabPage_ItemInfo.Controls.Add(Me.TextBox_POSDesc)
        Me.TabPage_ItemInfo.Controls.Add(Me.Label_POSDesc)
        Me.TabPage_ItemInfo.Controls.Add(Me.Label_Description)
        Me.TabPage_ItemInfo.Controls.Add(Me.Label_SignCaption)
        Me.TabPage_ItemInfo.Controls.Add(Me.TextBox_Description)
        Me.TabPage_ItemInfo.Controls.Add(Me.TextBox_SignCaption)
        Me.TabPage_ItemInfo.Controls.Add(Me.GroupBox_RetailPackage)
        Me.TabPage_ItemInfo.Controls.Add(Me.GroupBox_Units)
        resources.ApplyResources(Me.TabPage_ItemInfo, "TabPage_ItemInfo")
        Me.TabPage_ItemInfo.Name = "TabPage_ItemInfo"
        Me.TabPage_ItemInfo.UseVisualStyleBackColor = True
        '
        'GroupBoxAttributes
        '
        Me.GroupBoxAttributes.Controls.Add(Me.LabelSustainabilityRanking)
        Me.GroupBoxAttributes.Controls.Add(Me.CheckBoxSustainabilityRanking)
        Me.GroupBoxAttributes.Controls.Add(Me.ComboBoxSustainabilityRanking)
        Me.GroupBoxAttributes.Controls.Add(Me.ButtonBrandAdd)
        Me.GroupBoxAttributes.Controls.Add(Me.ComboBoxBrand)
        Me.GroupBoxAttributes.Controls.Add(Me.LabelBrand)
        Me.GroupBoxAttributes.Controls.Add(Me.CheckBoxCostedByWeight)
        Me.GroupBoxAttributes.Controls.Add(Me.ComboBoxOrigin)
        Me.GroupBoxAttributes.Controls.Add(Me.LabelOrigin)
        Me.GroupBoxAttributes.Controls.Add(Me.ComboBoxLabelType)
        Me.GroupBoxAttributes.Controls.Add(Me.CheckBoxLockAuth)
        Me.GroupBoxAttributes.Controls.Add(Me.ComboBoxCountryOfProc)
        Me.GroupBoxAttributes.Controls.Add(Me.LabelLabelType)
        Me.GroupBoxAttributes.Controls.Add(Me.CheckBoxIngredient)
        Me.GroupBoxAttributes.Controls.Add(Me.LabelCountryOfProc)
        Me.GroupBoxAttributes.Controls.Add(Me.CheckBoxRecall)
        resources.ApplyResources(Me.GroupBoxAttributes, "GroupBoxAttributes")
        Me.GroupBoxAttributes.Name = "GroupBoxAttributes"
        Me.GroupBoxAttributes.TabStop = False
        '
        'LabelSustainabilityRanking
        '
        resources.ApplyResources(Me.LabelSustainabilityRanking, "LabelSustainabilityRanking")
        Me.LabelSustainabilityRanking.Name = "LabelSustainabilityRanking"
        '
        'CheckBoxSustainabilityRanking
        '
        resources.ApplyResources(Me.CheckBoxSustainabilityRanking, "CheckBoxSustainabilityRanking")
        Me.CheckBoxSustainabilityRanking.Name = "CheckBoxSustainabilityRanking"
        Me.CheckBoxSustainabilityRanking.UseVisualStyleBackColor = True
        '
        'ComboBoxSustainabilityRanking
        '
        Me.ComboBoxSustainabilityRanking.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBoxSustainabilityRanking, "ComboBoxSustainabilityRanking")
        Me.ComboBoxSustainabilityRanking.Name = "ComboBoxSustainabilityRanking"
        '
        'ButtonBrandAdd
        '
        Me.ButtonBrandAdd.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonBrandAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ButtonBrandAdd, "ButtonBrandAdd")
        Me.ButtonBrandAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonBrandAdd.Name = "ButtonBrandAdd"
        Me.ButtonBrandAdd.UseVisualStyleBackColor = False
        '
        'ComboBoxBrand
        '
        resources.ApplyResources(Me.ComboBoxBrand, "ComboBoxBrand")
        Me.ComboBoxBrand.FormattingEnabled = True
        Me.ComboBoxBrand.Name = "ComboBoxBrand"
        '
        'LabelBrand
        '
        resources.ApplyResources(Me.LabelBrand, "LabelBrand")
        Me.LabelBrand.Name = "LabelBrand"
        '
        'CheckBoxCostedByWeight
        '
        resources.ApplyResources(Me.CheckBoxCostedByWeight, "CheckBoxCostedByWeight")
        Me.CheckBoxCostedByWeight.Name = "CheckBoxCostedByWeight"
        Me.CheckBoxCostedByWeight.UseVisualStyleBackColor = True
        '
        'ComboBoxOrigin
        '
        resources.ApplyResources(Me.ComboBoxOrigin, "ComboBoxOrigin")
        Me.ComboBoxOrigin.FormattingEnabled = True
        Me.ComboBoxOrigin.Name = "ComboBoxOrigin"
        '
        'LabelOrigin
        '
        resources.ApplyResources(Me.LabelOrigin, "LabelOrigin")
        Me.LabelOrigin.Name = "LabelOrigin"
        '
        'ComboBoxLabelType
        '
        resources.ApplyResources(Me.ComboBoxLabelType, "ComboBoxLabelType")
        Me.ComboBoxLabelType.FormattingEnabled = True
        Me.ComboBoxLabelType.Name = "ComboBoxLabelType"
        '
        'CheckBoxLockAuth
        '
        resources.ApplyResources(Me.CheckBoxLockAuth, "CheckBoxLockAuth")
        Me.CheckBoxLockAuth.Name = "CheckBoxLockAuth"
        Me.CheckBoxLockAuth.UseVisualStyleBackColor = True
        '
        'ComboBoxCountryOfProc
        '
        resources.ApplyResources(Me.ComboBoxCountryOfProc, "ComboBoxCountryOfProc")
        Me.ComboBoxCountryOfProc.FormattingEnabled = True
        Me.ComboBoxCountryOfProc.Name = "ComboBoxCountryOfProc"
        '
        'LabelLabelType
        '
        resources.ApplyResources(Me.LabelLabelType, "LabelLabelType")
        Me.LabelLabelType.Name = "LabelLabelType"
        '
        'CheckBoxIngredient
        '
        resources.ApplyResources(Me.CheckBoxIngredient, "CheckBoxIngredient")
        Me.CheckBoxIngredient.Name = "CheckBoxIngredient"
        Me.CheckBoxIngredient.UseVisualStyleBackColor = True
        '
        'LabelCountryOfProc
        '
        resources.ApplyResources(Me.LabelCountryOfProc, "LabelCountryOfProc")
        Me.LabelCountryOfProc.Name = "LabelCountryOfProc"
        '
        'CheckBoxRecall
        '
        resources.ApplyResources(Me.CheckBoxRecall, "CheckBoxRecall")
        Me.CheckBoxRecall.Name = "CheckBoxRecall"
        Me.CheckBoxRecall.UseVisualStyleBackColor = True
        '
        'CheckBoxNotAvailable
        '
        resources.ApplyResources(Me.CheckBoxNotAvailable, "CheckBoxNotAvailable")
        Me.CheckBoxNotAvailable.Name = "CheckBoxNotAvailable"
        Me.CheckBoxNotAvailable.UseVisualStyleBackColor = True
        '
        'LabelNotAvailableNote
        '
        resources.ApplyResources(Me.LabelNotAvailableNote, "LabelNotAvailableNote")
        Me.LabelNotAvailableNote.Name = "LabelNotAvailableNote"
        '
        'TextBoxNotAvailableNote
        '
        resources.ApplyResources(Me.TextBoxNotAvailableNote, "TextBoxNotAvailableNote")
        Me.TextBoxNotAvailableNote.Name = "TextBoxNotAvailableNote"
        '
        'TabPage_POSInfo
        '
        Me.TabPage_POSInfo.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_POSInfo.Controls.Add(Me.ButtonSavePOSSettings)
        Me.TabPage_POSInfo.Controls.Add(Me.Button_RefreshPOSData)
        Me.TabPage_POSInfo.Controls.Add(Me.GroupBox_Common)
        resources.ApplyResources(Me.TabPage_POSInfo, "TabPage_POSInfo")
        Me.TabPage_POSInfo.Name = "TabPage_POSInfo"
        Me.TabPage_POSInfo.UseVisualStyleBackColor = True
        '
        'GroupBox_Common
        '
        Me.GroupBox_Common.Controls.Add(Me.LabelUnitPriceCategory)
        Me.GroupBox_Common.Controls.Add(Me.LabelProductCode)
        Me.GroupBox_Common.Controls.Add(Me.TextBoxProductCode)
        Me.GroupBox_Common.Controls.Add(Me.TextBoxUnitPriceCategory)
        Me.GroupBox_Common.Controls.Add(Me.CheckBoxFSAEligible)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_MiscTransSale)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_MiscTransRefund)
        Me.GroupBox_Common.Controls.Add(Me.Label_IceTare)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_IceTare)
        Me.GroupBox_Common.Controls.Add(Me.Label_MiscTransRefund)
        Me.GroupBox_Common.Controls.Add(Me.Label_MiscTransSale)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_CouponMultiplier)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_CaseDiscount)
        Me.GroupBox_Common.Controls.Add(Me.Label_GroupList)
        Me.GroupBox_Common.Controls.Add(Me.TextBox_GroupList)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_PriceRequired)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_QuantityProhibit)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_QuantityRequired)
        Me.GroupBox_Common.Controls.Add(Me.CheckBox_FoodStamps)
        resources.ApplyResources(Me.GroupBox_Common, "GroupBox_Common")
        Me.GroupBox_Common.Name = "GroupBox_Common"
        Me.GroupBox_Common.TabStop = False
        '
        'LabelUnitPriceCategory
        '
        resources.ApplyResources(Me.LabelUnitPriceCategory, "LabelUnitPriceCategory")
        Me.LabelUnitPriceCategory.Name = "LabelUnitPriceCategory"
        '
        'LabelProductCode
        '
        resources.ApplyResources(Me.LabelProductCode, "LabelProductCode")
        Me.LabelProductCode.Name = "LabelProductCode"
        '
        'TextBoxProductCode
        '
        resources.ApplyResources(Me.TextBoxProductCode, "TextBoxProductCode")
        Me.TextBoxProductCode.Name = "TextBoxProductCode"
        '
        'TextBoxUnitPriceCategory
        '
        resources.ApplyResources(Me.TextBoxUnitPriceCategory, "TextBoxUnitPriceCategory")
        Me.TextBoxUnitPriceCategory.Name = "TextBoxUnitPriceCategory"
        '
        'CheckBoxFSAEligible
        '
        Me.CheckBoxFSAEligible.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxFSAEligible.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBoxFSAEligible, "CheckBoxFSAEligible")
        Me.CheckBoxFSAEligible.Name = "CheckBoxFSAEligible"
        Me.CheckBoxFSAEligible.UseVisualStyleBackColor = False
        '
        'TextBox_MiscTransSale
        '
        resources.ApplyResources(Me.TextBox_MiscTransSale, "TextBox_MiscTransSale")
        Me.TextBox_MiscTransSale.Name = "TextBox_MiscTransSale"
        '
        'TextBox_MiscTransRefund
        '
        resources.ApplyResources(Me.TextBox_MiscTransRefund, "TextBox_MiscTransRefund")
        Me.TextBox_MiscTransRefund.Name = "TextBox_MiscTransRefund"
        '
        'Label_IceTare
        '
        resources.ApplyResources(Me.Label_IceTare, "Label_IceTare")
        Me.Label_IceTare.Name = "Label_IceTare"
        '
        'TextBox_IceTare
        '
        resources.ApplyResources(Me.TextBox_IceTare, "TextBox_IceTare")
        Me.TextBox_IceTare.Name = "TextBox_IceTare"
        '
        'Label_MiscTransRefund
        '
        resources.ApplyResources(Me.Label_MiscTransRefund, "Label_MiscTransRefund")
        Me.Label_MiscTransRefund.Name = "Label_MiscTransRefund"
        '
        'Label_MiscTransSale
        '
        resources.ApplyResources(Me.Label_MiscTransSale, "Label_MiscTransSale")
        Me.Label_MiscTransSale.Name = "Label_MiscTransSale"
        '
        'CheckBox_CouponMultiplier
        '
        Me.CheckBox_CouponMultiplier.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox_CouponMultiplier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_CouponMultiplier, "CheckBox_CouponMultiplier")
        Me.CheckBox_CouponMultiplier.Name = "CheckBox_CouponMultiplier"
        Me.CheckBox_CouponMultiplier.UseVisualStyleBackColor = False
        '
        'CheckBox_CaseDiscount
        '
        Me.CheckBox_CaseDiscount.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox_CaseDiscount.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_CaseDiscount, "CheckBox_CaseDiscount")
        Me.CheckBox_CaseDiscount.Name = "CheckBox_CaseDiscount"
        Me.CheckBox_CaseDiscount.UseVisualStyleBackColor = False
        '
        'Label_GroupList
        '
        resources.ApplyResources(Me.Label_GroupList, "Label_GroupList")
        Me.Label_GroupList.Name = "Label_GroupList"
        '
        'TextBox_GroupList
        '
        resources.ApplyResources(Me.TextBox_GroupList, "TextBox_GroupList")
        Me.TextBox_GroupList.Name = "TextBox_GroupList"
        '
        'CheckBox_PriceRequired
        '
        Me.CheckBox_PriceRequired.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox_PriceRequired.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_PriceRequired, "CheckBox_PriceRequired")
        Me.CheckBox_PriceRequired.Name = "CheckBox_PriceRequired"
        Me.CheckBox_PriceRequired.UseVisualStyleBackColor = False
        '
        'CheckBox_QuantityProhibit
        '
        resources.ApplyResources(Me.CheckBox_QuantityProhibit, "CheckBox_QuantityProhibit")
        Me.CheckBox_QuantityProhibit.Name = "CheckBox_QuantityProhibit"
        Me.CheckBox_QuantityProhibit.UseVisualStyleBackColor = True
        '
        'CheckBox_QuantityRequired
        '
        Me.CheckBox_QuantityRequired.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox_QuantityRequired.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_QuantityRequired, "CheckBox_QuantityRequired")
        Me.CheckBox_QuantityRequired.Name = "CheckBox_QuantityRequired"
        Me.CheckBox_QuantityRequired.UseVisualStyleBackColor = False
        '
        'CheckBox_FoodStamps
        '
        Me.CheckBox_FoodStamps.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox_FoodStamps.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_FoodStamps, "CheckBox_FoodStamps")
        Me.CheckBox_FoodStamps.Name = "CheckBox_FoodStamps"
        Me.CheckBox_FoodStamps.UseVisualStyleBackColor = False
        '
        'ComboBox_AltJurisdiction
        '
        Me.ComboBox_AltJurisdiction.FormattingEnabled = True
        Me.ComboBox_AltJurisdiction.Items.AddRange(New Object() {resources.GetString("ComboBox_AltJurisdiction.Items"), resources.GetString("ComboBox_AltJurisdiction.Items1")})
        resources.ApplyResources(Me.ComboBox_AltJurisdiction, "ComboBox_AltJurisdiction")
        Me.ComboBox_AltJurisdiction.Name = "ComboBox_AltJurisdiction"
        '
        'ItemOverride
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ComboBox_AltJurisdiction)
        Me.Controls.Add(Me.TabControl)
        Me.Controls.Add(Me.Label_AlternateJurisdiction)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.Label_Identifier)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ItemOverride"
        Me.ShowInTaskbar = False
        Me.GroupBox_Units.ResumeLayout(False)
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_RetailPackage.ResumeLayout(False)
        Me.GroupBox_RetailPackage.PerformLayout()
        Me.TabControl.ResumeLayout(False)
        Me.TabPage_ItemInfo.ResumeLayout(False)
        Me.TabPage_ItemInfo.PerformLayout()
        Me.GroupBoxAttributes.ResumeLayout(False)
        Me.GroupBoxAttributes.PerformLayout()
        Me.TabPage_POSInfo.ResumeLayout(False)
        Me.GroupBox_Common.ResumeLayout(False)
        Me.GroupBox_Common.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox_RetailPackage As System.Windows.Forms.GroupBox
    Public WithEvents Label_RetailPackUOM As System.Windows.Forms.Label
    Public WithEvents Label_Pack As System.Windows.Forms.Label
    Public WithEvents Label_RetailPackSize As System.Windows.Forms.Label
    Friend WithEvents Label_AlternateJurisdiction As System.Windows.Forms.Label
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPage_ItemInfo As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_POSInfo As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox_Common As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_MiscTransSale As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_MiscTransRefund As System.Windows.Forms.TextBox
    Friend WithEvents Label_IceTare As System.Windows.Forms.Label
    Friend WithEvents TextBox_IceTare As System.Windows.Forms.TextBox
    Friend WithEvents Label_MiscTransRefund As System.Windows.Forms.Label
    Friend WithEvents Label_MiscTransSale As System.Windows.Forms.Label
    Public WithEvents CheckBox_CouponMultiplier As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_CaseDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents Label_GroupList As System.Windows.Forms.Label
    Friend WithEvents TextBox_GroupList As System.Windows.Forms.TextBox
    Public WithEvents CheckBox_PriceRequired As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_QuantityProhibit As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_QuantityRequired As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_FoodStamps As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_AltJurisdiction As System.Windows.Forms.ComboBox
    Public WithEvents Button_RefreshItemInfo As System.Windows.Forms.Button
    Public WithEvents Button_RefreshPOSData As System.Windows.Forms.Button
    Friend WithEvents CheckBoxNotAvailable As System.Windows.Forms.CheckBox
    Friend WithEvents LabelNotAvailableNote As System.Windows.Forms.Label
    Friend WithEvents TextBoxNotAvailableNote As System.Windows.Forms.TextBox
    Friend WithEvents ComboBoxBrand As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxOrigin As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxCountryOfProc As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxLockAuth As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRecall As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxIngredient As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxCostedByWeight As System.Windows.Forms.CheckBox
    Friend WithEvents LabelLabelType As System.Windows.Forms.Label
    Friend WithEvents LabelCountryOfProc As System.Windows.Forms.Label
    Friend WithEvents LabelOrigin As System.Windows.Forms.Label
    Friend WithEvents LabelBrand As System.Windows.Forms.Label
    Public WithEvents ButtonSaveItemInformation As System.Windows.Forms.Button
    Friend WithEvents GroupBoxAttributes As System.Windows.Forms.GroupBox
    Friend WithEvents LabelSustainabilityRanking As System.Windows.Forms.Label
    Friend WithEvents CheckBoxSustainabilityRanking As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxSustainabilityRanking As System.Windows.Forms.ComboBox
    Public WithEvents ButtonBrandAdd As System.Windows.Forms.Button
    Public WithEvents CheckBoxFSAEligible As System.Windows.Forms.CheckBox
    Friend WithEvents LabelUnitPriceCategory As System.Windows.Forms.Label
    Friend WithEvents LabelProductCode As System.Windows.Forms.Label
    Friend WithEvents TextBoxProductCode As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxUnitPriceCategory As System.Windows.Forms.TextBox
    Public WithEvents ButtonSavePOSSettings As System.Windows.Forms.Button

#End Region
End Class