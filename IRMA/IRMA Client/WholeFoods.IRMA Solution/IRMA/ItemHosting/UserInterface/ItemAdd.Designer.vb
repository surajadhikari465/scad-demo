<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemAdd
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
        InitializeComponent()
        Me.IsInitializing = False
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
	Public WithEvents _cmbField_15 As System.Windows.Forms.ComboBox
	Public WithEvents chkRetailSale As System.Windows.Forms.CheckBox
    Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents _cmbField_6 As System.Windows.Forms.ComboBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents _cmbField_4 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_5 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_2 As System.Windows.Forms.TextBox
    Public WithEvents lblNationalClass As System.Windows.Forms.Label
    Public WithEvents lblRetailSale As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents lblPOSDesc As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me._cmbField_15 = New System.Windows.Forms.ComboBox()
        Me.chkRetailSale = New System.Windows.Forms.CheckBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me._cmbField_6 = New System.Windows.Forms.ComboBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._cmbField_4 = New System.Windows.Forms.ComboBox()
        Me._cmbField_5 = New System.Windows.Forms.ComboBox()
        Me._txtField_3 = New System.Windows.Forms.TextBox()
        Me._txtField_2 = New System.Windows.Forms.TextBox()
        Me.lblNationalClass = New System.Windows.Forms.Label()
        Me.lblRetailSale = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.lblPOSDesc = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.fraUnits = New System.Windows.Forms.GroupBox()
        Me._cmbField_9 = New System.Windows.Forms.ComboBox()
        Me._cmbField_8 = New System.Windows.Forms.ComboBox()
        Me.lblDistribution = New System.Windows.Forms.Label()
        Me.lblVendorOrder = New System.Windows.Forms.Label()
        Me.lblRetail = New System.Windows.Forms.Label()
        Me.chkCostedByWeight = New System.Windows.Forms.CheckBox()
        Me.lblCostedWeight = New System.Windows.Forms.Label()
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.lblTaxClass = New System.Windows.Forms.Label()
        Me.cmbTaxClass = New System.Windows.Forms.ComboBox()
        Me.Label_LabelType = New System.Windows.Forms.Label()
        Me.cmbLabelType = New System.Windows.Forms.ComboBox()
        Me.Button_ViewTaxFlags = New System.Windows.Forms.Button()
        Me.Label_Brand = New System.Windows.Forms.Label()
        Me.ComboBox_Brand = New System.Windows.Forms.ComboBox()
        Me.AddBrandButton = New System.Windows.Forms.Button()
        Me.Label_NumScaleDigits = New System.Windows.Forms.Label()
        Me.GroupBox_NumScaleDigits = New System.Windows.Forms.GroupBox()
        Me.RadioButton_NumScaleDigits_5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton_NumScaleDigits_4 = New System.Windows.Forms.RadioButton()
        Me.Label_SendToScale = New System.Windows.Forms.Label()
        Me.HierarchySelector1 = New HierarchySelector()
        Me.GroupBox_RetailPackage = New System.Windows.Forms.GroupBox()
        Me.Label_RetailPackSize = New System.Windows.Forms.Label()
        Me.Label_Pack = New System.Windows.Forms.Label()
        Me.Label_RetailPackUOM = New System.Windows.Forms.Label()
        Me.lblSlash1 = New System.Windows.Forms.Label()
        Me.grpManageBy = New System.Windows.Forms.GroupBox()
        Me.cmbManagedBy = New System.Windows.Forms.ComboBox()
        Me.Label_DefaultJurisdiction = New System.Windows.Forms.Label()
        Me.ComboBoxJurisdiction = New System.Windows.Forms.ComboBox()
        Me.GroupBox_SendToScale = New System.Windows.Forms.GroupBox()
        Me.RadioButton_SendToScale_No = New System.Windows.Forms.RadioButton()
        Me.RadioButton_SendToScale_Yes = New System.Windows.Forms.RadioButton()
        Me.chkFoodStamps = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkOrganic = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.fraUnits.SuspendLayout()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_NumScaleDigits.SuspendLayout()
        Me.GroupBox_RetailPackage.SuspendLayout()
        Me.grpManageBy.SuspendLayout()
        Me.GroupBox_SendToScale.SuspendLayout()
        Me.SuspendLayout()
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
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
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
        'chkRetailSale
        '
        Me.chkRetailSale.BackColor = System.Drawing.SystemColors.Control
        Me.chkRetailSale.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkRetailSale, "chkRetailSale")
        Me.chkRetailSale.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRetailSale.Name = "chkRetailSale"
        Me.chkRetailSale.UseVisualStyleBackColor = False
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
        Me._txtField_4.Tag = "CurrencyExt"
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
        Me._txtField_5.Tag = "CurrencyExt"
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
        Me._txtField_1.Tag = "Number"
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
        Me._cmbField_4.TabStop = False
        Me._cmbField_4.Tag = "Should be removed but didn't want to mess up the control index."
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
        'lblNationalClass
        '
        Me.lblNationalClass.BackColor = System.Drawing.Color.Transparent
        Me.lblNationalClass.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblNationalClass, "lblNationalClass")
        Me.lblNationalClass.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNationalClass.Name = "lblNationalClass"
        '
        'lblRetailSale
        '
        Me.lblRetailSale.BackColor = System.Drawing.Color.Transparent
        Me.lblRetailSale.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRetailSale, "lblRetailSale")
        Me.lblRetailSale.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRetailSale.Name = "lblRetailSale"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'lblPOSDesc
        '
        Me.lblPOSDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblPOSDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPOSDesc, "lblPOSDesc")
        Me.lblPOSDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPOSDesc.Name = "lblPOSDesc"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDescription, "lblDescription")
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Name = "lblDescription"
        '
        'fraUnits
        '
        Me.fraUnits.BackColor = System.Drawing.SystemColors.Control
        Me.fraUnits.Controls.Add(Me._cmbField_9)
        Me.fraUnits.Controls.Add(Me._cmbField_8)
        Me.fraUnits.Controls.Add(Me.lblDistribution)
        Me.fraUnits.Controls.Add(Me.lblVendorOrder)
        Me.fraUnits.Controls.Add(Me.lblRetail)
        Me.fraUnits.Controls.Add(Me._cmbField_5)
        resources.ApplyResources(Me.fraUnits, "fraUnits")
        Me.fraUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUnits.Name = "fraUnits"
        Me.fraUnits.TabStop = False
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
        'lblDistribution
        '
        Me.lblDistribution.BackColor = System.Drawing.Color.Transparent
        Me.lblDistribution.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDistribution, "lblDistribution")
        Me.lblDistribution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDistribution.Name = "lblDistribution"
        '
        'lblVendorOrder
        '
        Me.lblVendorOrder.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorOrder.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorOrder, "lblVendorOrder")
        Me.lblVendorOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorOrder.Name = "lblVendorOrder"
        '
        'lblRetail
        '
        Me.lblRetail.BackColor = System.Drawing.Color.Transparent
        Me.lblRetail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblRetail, "lblRetail")
        Me.lblRetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRetail.Name = "lblRetail"
        '
        'chkCostedByWeight
        '
        Me.chkCostedByWeight.BackColor = System.Drawing.SystemColors.Control
        Me.chkCostedByWeight.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkCostedByWeight, "chkCostedByWeight")
        Me.chkCostedByWeight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCostedByWeight.Name = "chkCostedByWeight"
        Me.chkCostedByWeight.UseVisualStyleBackColor = False
        '
        'lblCostedWeight
        '
        Me.lblCostedWeight.BackColor = System.Drawing.Color.Transparent
        Me.lblCostedWeight.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCostedWeight, "lblCostedWeight")
        Me.lblCostedWeight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCostedWeight.Name = "lblCostedWeight"
        '
        'cmbField
        '
        '
        'txtField
        '
        '
        'lblTaxClass
        '
        resources.ApplyResources(Me.lblTaxClass, "lblTaxClass")
        Me.lblTaxClass.Name = "lblTaxClass"
        '
        'cmbTaxClass
        '
        Me.cmbTaxClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTaxClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTaxClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbTaxClass, "cmbTaxClass")
        Me.cmbTaxClass.FormattingEnabled = True
        Me.cmbTaxClass.Name = "cmbTaxClass"
        '
        'Label_LabelType
        '
        resources.ApplyResources(Me.Label_LabelType, "Label_LabelType")
        Me.Label_LabelType.Name = "Label_LabelType"
        '
        'cmbLabelType
        '
        Me.cmbLabelType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbLabelType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbLabelType, "cmbLabelType")
        Me.cmbLabelType.FormattingEnabled = True
        Me.cmbLabelType.Name = "cmbLabelType"
        '
        'Button_ViewTaxFlags
        '
        resources.ApplyResources(Me.Button_ViewTaxFlags, "Button_ViewTaxFlags")
        Me.Button_ViewTaxFlags.Name = "Button_ViewTaxFlags"
        Me.Button_ViewTaxFlags.TabStop = False
        Me.Button_ViewTaxFlags.UseVisualStyleBackColor = True
        '
        'Label_Brand
        '
        resources.ApplyResources(Me.Label_Brand, "Label_Brand")
        Me.Label_Brand.Name = "Label_Brand"
        '
        'ComboBox_Brand
        '
        Me.ComboBox_Brand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Brand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Brand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_Brand, "ComboBox_Brand")
        Me.ComboBox_Brand.FormattingEnabled = True
        Me.ComboBox_Brand.Name = "ComboBox_Brand"
        '
        'AddBrandButton
        '
        resources.ApplyResources(Me.AddBrandButton, "AddBrandButton")
        Me.AddBrandButton.Name = "AddBrandButton"
        Me.AddBrandButton.TabStop = False
        Me.AddBrandButton.UseVisualStyleBackColor = True
        '
        'Label_NumScaleDigits
        '
        resources.ApplyResources(Me.Label_NumScaleDigits, "Label_NumScaleDigits")
        Me.Label_NumScaleDigits.Name = "Label_NumScaleDigits"
        '
        'GroupBox_NumScaleDigits
        '
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_5)
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_4)
        resources.ApplyResources(Me.GroupBox_NumScaleDigits, "GroupBox_NumScaleDigits")
        Me.GroupBox_NumScaleDigits.Name = "GroupBox_NumScaleDigits"
        Me.GroupBox_NumScaleDigits.TabStop = False
        '
        'RadioButton_NumScaleDigits_5
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_5, "RadioButton_NumScaleDigits_5")
        Me.RadioButton_NumScaleDigits_5.Name = "RadioButton_NumScaleDigits_5"
        Me.RadioButton_NumScaleDigits_5.UseVisualStyleBackColor = True
        '
        'RadioButton_NumScaleDigits_4
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_4, "RadioButton_NumScaleDigits_4")
        Me.RadioButton_NumScaleDigits_4.Checked = True
        Me.RadioButton_NumScaleDigits_4.Name = "RadioButton_NumScaleDigits_4"
        Me.RadioButton_NumScaleDigits_4.TabStop = True
        Me.RadioButton_NumScaleDigits_4.UseVisualStyleBackColor = True
        '
        'Label_SendToScale
        '
        resources.ApplyResources(Me.Label_SendToScale, "Label_SendToScale")
        Me.Label_SendToScale.Name = "Label_SendToScale"
        '
        'HierarchySelector1
        '
        resources.ApplyResources(Me.HierarchySelector1, "HierarchySelector1")
        Me.HierarchySelector1.ItemIdentifier = Nothing
        Me.HierarchySelector1.Name = "HierarchySelector1"
        Me.HierarchySelector1.SelectedCategoryId = 0
        Me.HierarchySelector1.SelectedLevel3Id = 0
        Me.HierarchySelector1.SelectedLevel4Id = 0
        Me.HierarchySelector1.SelectedSubTeamId = 0
        '
        'GroupBox_RetailPackage
        '
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackSize)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_Pack)
        Me.GroupBox_RetailPackage.Controls.Add(Me.Label_RetailPackUOM)
        Me.GroupBox_RetailPackage.Controls.Add(Me.lblSlash1)
        Me.GroupBox_RetailPackage.Controls.Add(Me._cmbField_6)
        Me.GroupBox_RetailPackage.Controls.Add(Me._txtField_5)
        Me.GroupBox_RetailPackage.Controls.Add(Me._txtField_4)
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
        'lblSlash1
        '
        Me.lblSlash1.BackColor = System.Drawing.Color.Transparent
        Me.lblSlash1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSlash1, "lblSlash1")
        Me.lblSlash1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash1.Name = "lblSlash1"
        '
        'grpManageBy
        '
        Me.grpManageBy.Controls.Add(Me.cmbManagedBy)
        resources.ApplyResources(Me.grpManageBy, "grpManageBy")
        Me.grpManageBy.ForeColor = System.Drawing.Color.Black
        Me.grpManageBy.Name = "grpManageBy"
        Me.grpManageBy.TabStop = False
        '
        'cmbManagedBy
        '
        resources.ApplyResources(Me.cmbManagedBy, "cmbManagedBy")
        Me.cmbManagedBy.FormattingEnabled = True
        Me.cmbManagedBy.Name = "cmbManagedBy"
        '
        'Label_DefaultJurisdiction
        '
        resources.ApplyResources(Me.Label_DefaultJurisdiction, "Label_DefaultJurisdiction")
        Me.Label_DefaultJurisdiction.Name = "Label_DefaultJurisdiction"
        '
        'ComboBoxJurisdiction
        '
        Me.ComboBoxJurisdiction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBoxJurisdiction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxJurisdiction.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBoxJurisdiction, "ComboBoxJurisdiction")
        Me.ComboBoxJurisdiction.Name = "ComboBoxJurisdiction"
        '
        'GroupBox_SendToScale
        '
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_No)
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_Yes)
        resources.ApplyResources(Me.GroupBox_SendToScale, "GroupBox_SendToScale")
        Me.GroupBox_SendToScale.Name = "GroupBox_SendToScale"
        Me.GroupBox_SendToScale.TabStop = False
        '
        'RadioButton_SendToScale_No
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_No, "RadioButton_SendToScale_No")
        Me.RadioButton_SendToScale_No.Checked = True
        Me.RadioButton_SendToScale_No.Name = "RadioButton_SendToScale_No"
        Me.RadioButton_SendToScale_No.TabStop = True
        Me.RadioButton_SendToScale_No.UseVisualStyleBackColor = True
        '
        'RadioButton_SendToScale_Yes
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_Yes, "RadioButton_SendToScale_Yes")
        Me.RadioButton_SendToScale_Yes.Name = "RadioButton_SendToScale_Yes"
        Me.RadioButton_SendToScale_Yes.UseVisualStyleBackColor = True
        '
        'chkFoodStamps
        '
        Me.chkFoodStamps.BackColor = System.Drawing.SystemColors.Control
        Me.chkFoodStamps.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkFoodStamps, "chkFoodStamps")
        Me.chkFoodStamps.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFoodStamps.Name = "chkFoodStamps"
        Me.chkFoodStamps.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'chkOrganic
        '
        Me.chkOrganic.BackColor = System.Drawing.SystemColors.Control
        Me.chkOrganic.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkOrganic, "chkOrganic")
        Me.chkOrganic.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOrganic.Name = "chkOrganic"
        Me.chkOrganic.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'frmItemAdd
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.chkOrganic)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.chkFoodStamps)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox_SendToScale)
        Me.Controls.Add(Me.ComboBoxJurisdiction)
        Me.Controls.Add(Me.Label_DefaultJurisdiction)
        Me.Controls.Add(Me.grpManageBy)
        Me.Controls.Add(Me.GroupBox_RetailPackage)
        Me.Controls.Add(Me.HierarchySelector1)
        Me.Controls.Add(Me.Label_SendToScale)
        Me.Controls.Add(Me.GroupBox_NumScaleDigits)
        Me.Controls.Add(Me.Label_NumScaleDigits)
        Me.Controls.Add(Me.AddBrandButton)
        Me.Controls.Add(Me.Label_Brand)
        Me.Controls.Add(Me.ComboBox_Brand)
        Me.Controls.Add(Me.Button_ViewTaxFlags)
        Me.Controls.Add(Me.Label_LabelType)
        Me.Controls.Add(Me.cmbLabelType)
        Me.Controls.Add(Me.lblTaxClass)
        Me.Controls.Add(Me.cmbTaxClass)
        Me.Controls.Add(Me.chkCostedByWeight)
        Me.Controls.Add(Me.lblCostedWeight)
        Me.Controls.Add(Me.fraUnits)
        Me.Controls.Add(Me.chkRetailSale)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me._txtField_3)
        Me.Controls.Add(Me._txtField_2)
        Me.Controls.Add(Me.lblNationalClass)
        Me.Controls.Add(Me.lblRetailSale)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.lblPOSDesc)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me._cmbField_4)
        Me.Controls.Add(Me._cmbField_15)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemAdd"
        Me.ShowInTaskbar = False
        Me.fraUnits.ResumeLayout(False)
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_NumScaleDigits.ResumeLayout(False)
        Me.GroupBox_NumScaleDigits.PerformLayout()
        Me.GroupBox_RetailPackage.ResumeLayout(False)
        Me.GroupBox_RetailPackage.PerformLayout()
        Me.grpManageBy.ResumeLayout(False)
        Me.GroupBox_SendToScale.ResumeLayout(False)
        Me.GroupBox_SendToScale.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents fraUnits As System.Windows.Forms.GroupBox
    Public WithEvents _cmbField_9 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_8 As System.Windows.Forms.ComboBox
    Public WithEvents lblDistribution As System.Windows.Forms.Label
    Public WithEvents lblVendorOrder As System.Windows.Forms.Label
    Public WithEvents lblRetail As System.Windows.Forms.Label
    Public WithEvents chkCostedByWeight As System.Windows.Forms.CheckBox
    Public WithEvents lblCostedWeight As System.Windows.Forms.Label
    Friend WithEvents lblTaxClass As System.Windows.Forms.Label
    Friend WithEvents cmbTaxClass As System.Windows.Forms.ComboBox
    Friend WithEvents Label_LabelType As System.Windows.Forms.Label
    Friend WithEvents cmbLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents Button_ViewTaxFlags As System.Windows.Forms.Button
    Friend WithEvents Label_Brand As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Brand As System.Windows.Forms.ComboBox
    Friend WithEvents AddBrandButton As System.Windows.Forms.Button
    Friend WithEvents Label_NumScaleDigits As System.Windows.Forms.Label
    Friend WithEvents GroupBox_NumScaleDigits As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_NumScaleDigits_5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_NumScaleDigits_4 As System.Windows.Forms.RadioButton
    Friend WithEvents Label_SendToScale As System.Windows.Forms.Label
    Friend WithEvents HierarchySelector1 As HierarchySelector
    Friend WithEvents GroupBox_RetailPackage As System.Windows.Forms.GroupBox
    Public WithEvents Label_RetailPackSize As System.Windows.Forms.Label
    Public WithEvents Label_Pack As System.Windows.Forms.Label
    Public WithEvents Label_RetailPackUOM As System.Windows.Forms.Label
    Public WithEvents lblSlash1 As System.Windows.Forms.Label
    Friend WithEvents grpManageBy As System.Windows.Forms.GroupBox
    Friend WithEvents cmbManagedBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label_DefaultJurisdiction As System.Windows.Forms.Label
    Friend WithEvents ComboBoxJurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox_SendToScale As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_SendToScale_No As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_SendToScale_Yes As System.Windows.Forms.RadioButton
    Public WithEvents chkFoodStamps As System.Windows.Forms.CheckBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents chkOrganic As System.Windows.Forms.CheckBox
    Public WithEvents Label2 As System.Windows.Forms.Label
#End Region
End Class