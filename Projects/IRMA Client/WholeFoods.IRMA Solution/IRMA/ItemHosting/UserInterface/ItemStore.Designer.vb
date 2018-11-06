<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemStore
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
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
    Public WithEvents _Frame1_1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents CheckBox_RestrictedHours As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_LineDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_LockedForSale As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox_CommonPOS As System.Windows.Forms.GroupBox
    Public WithEvents Frame1 As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
    Public WithEvents chkField As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents optField As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemStore))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_4 = New System.Windows.Forms.RadioButton()
        Me._optSelection_3 = New System.Windows.Forms.RadioButton()
        Me._optSelection_2 = New System.Windows.Forms.RadioButton()
        Me._optSelection_1 = New System.Windows.Forms.RadioButton()
        Me._optSelection_0 = New System.Windows.Forms.RadioButton()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.CheckBox_AuthorizedItem = New System.Windows.Forms.CheckBox()
        Me.GroupBox_CommonPOS = New System.Windows.Forms.GroupBox()
        Me.Label_ItemStatusCode = New System.Windows.Forms.Label()
        Me.ComboBox_AgeCode = New System.Windows.Forms.ComboBox()
        Me.cmbItemStatusCode = New System.Windows.Forms.ComboBox()
        Me.CheckBox_ElectronicShelfTag = New System.Windows.Forms.CheckBox()
        Me.TextBox_ItemSurcharge = New System.Windows.Forms.TextBox()
        Me.Label_ItemSurcharge = New System.Windows.Forms.Label()
        Me.CheckBox_LocalItem = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RefreshPOSInfo = New System.Windows.Forms.CheckBox()
        Me.CheckBox_EmployeeDiscount = New System.Windows.Forms.CheckBox()
        Me.TextBox_MixMatch = New System.Windows.Forms.TextBox()
        Me.Label_MixMatch = New System.Windows.Forms.Label()
        Me.CheckBox_AgeRestrict = New System.Windows.Forms.CheckBox()
        Me.FreedomSystemGroupBox = New System.Windows.Forms.GroupBox()
        Me.ComboBox_KitchenRouteID = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CheckBox_ConsolidatePrice = New System.Windows.Forms.CheckBox()
        Me.CheckBox_PrintCondimentOnReceipt = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Numeric_RoutingPriority = New System.Windows.Forms.NumericUpDown()
        Me.TextBox_LinkValue = New System.Windows.Forms.TextBox()
        Me.Label_LinkValue = New System.Windows.Forms.Label()
        Me.GroupBox_LinkCode = New System.Windows.Forms.GroupBox()
        Me.Button_DeleteLink = New System.Windows.Forms.Button()
        Me.TextBox_LinkedItem = New System.Windows.Forms.TextBox()
        Me.Button_AddLink_ItemSearch = New System.Windows.Forms.Button()
        Me.TextBox_POSTare = New System.Windows.Forms.TextBox()
        Me.CheckBox_CompetitiveItem = New System.Windows.Forms.CheckBox()
        Me.TextBox_AgeCode = New System.Windows.Forms.TextBox()
        Me.CheckBox_RestrictedHours = New System.Windows.Forms.CheckBox()
        Me.CheckBox_LineDiscount = New System.Windows.Forms.CheckBox()
        Me.Label_AgeCode = New System.Windows.Forms.Label()
        Me.CheckBox_LockedForSale = New System.Windows.Forms.CheckBox()
        Me.CheckBox_GrillPrint = New System.Windows.Forms.CheckBox()
        Me.Label_PosTare = New System.Windows.Forms.Label()
        Me.CheckBox_SeniorCitizen = New System.Windows.Forms.CheckBox()
        Me.CheckBox_VisualVerify = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Discontinue = New System.Windows.Forms.CheckBox()
        Me.CheckBox_OrderedByInfor = New System.Windows.Forms.CheckBox()
        Me.cmbRetailUom = New System.Windows.Forms.ComboBox()
        Me.lbl365RetailUom = New System.Windows.Forms.Label()
        Me._Frame1_1 = New System.Windows.Forms.GroupBox()
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.cmbStates = New System.Windows.Forms.ComboBox()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me.Button_TaxOverride = New System.Windows.Forms.Button()
        Me.Frame1 = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.chkField = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.optField = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ComboBox_SubTeam = New System.Windows.Forms.ComboBox()
        Me.GroupBox_SubTeam = New System.Windows.Forms.GroupBox()
        Me.ExemptGroupBox = New System.Windows.Forms.GroupBox()
        Me.ExemptCheckBox = New System.Windows.Forms.CheckBox()
        Me.Exempt_Button = New System.Windows.Forms.Button()
        Me.GroupBox_Authorization = New System.Windows.Forms.GroupBox()
        Me.grpScanAudit = New System.Windows.Forms.GroupBox()
        Me.lblLastNonDTSScan = New System.Windows.Forms.Label()
        Me.lblLastDTSScan = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox_Discontinue = New System.Windows.Forms.GroupBox()
        Me.cmbScaleUom = New System.Windows.Forms.ComboBox()
        Me.lblScaleUom = New System.Windows.Forms.Label()
        Me.lblFixedWeight = New System.Windows.Forms.Label()
        Me.TextBox_FixedWeight = New System.Windows.Forms.TextBox()
        Me.lblByCount = New System.Windows.Forms.Label()
        Me.GroupBox_StoreRetailUOM = New System.Windows.Forms.GroupBox()
        Me.ByCountNumericEditor = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.GroupBox_CommonPOS.SuspendLayout()
        Me.FreedomSystemGroupBox.SuspendLayout()
        CType(Me.Numeric_RoutingPriority, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_LinkCode.SuspendLayout()
        Me._Frame1_1.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_SubTeam.SuspendLayout()
        Me.ExemptGroupBox.SuspendLayout()
        Me.GroupBox_Authorization.SuspendLayout()
        Me.grpScanAudit.SuspendLayout()
        Me.GroupBox_Discontinue.SuspendLayout()
        Me.GroupBox_StoreRetailUOM.SuspendLayout()
        CType(Me.ByCountNumericEditor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_optSelection_4
        '
        Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_4, "_optSelection_4")
        Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
        Me._optSelection_4.Name = "_optSelection_4"
        Me._optSelection_4.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_4, resources.GetString("_optSelection_4.ToolTip"))
        Me._optSelection_4.UseVisualStyleBackColor = False
        '
        '_optSelection_3
        '
        Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_3, "_optSelection_3")
        Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
        Me._optSelection_3.Name = "_optSelection_3"
        Me._optSelection_3.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_3, resources.GetString("_optSelection_3.ToolTip"))
        Me._optSelection_3.UseVisualStyleBackColor = False
        '
        '_optSelection_2
        '
        Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_2, "_optSelection_2")
        Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
        Me._optSelection_2.Name = "_optSelection_2"
        Me._optSelection_2.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_2, resources.GetString("_optSelection_2.ToolTip"))
        Me._optSelection_2.UseVisualStyleBackColor = False
        '
        '_optSelection_1
        '
        Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_1, "_optSelection_1")
        Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
        Me._optSelection_1.Name = "_optSelection_1"
        Me._optSelection_1.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_1, resources.GetString("_optSelection_1.ToolTip"))
        Me._optSelection_1.UseVisualStyleBackColor = False
        '
        '_optSelection_0
        '
        Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_0.Checked = True
        Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_0, "_optSelection_0")
        Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
        Me._optSelection_0.Name = "_optSelection_0"
        Me._optSelection_0.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_0, resources.GetString("_optSelection_0.ToolTip"))
        Me._optSelection_0.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
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
        'CheckBox_AuthorizedItem
        '
        resources.ApplyResources(Me.CheckBox_AuthorizedItem, "CheckBox_AuthorizedItem")
        Me.CheckBox_AuthorizedItem.Name = "CheckBox_AuthorizedItem"
        Me.ToolTip1.SetToolTip(Me.CheckBox_AuthorizedItem, resources.GetString("CheckBox_AuthorizedItem.ToolTip"))
        Me.CheckBox_AuthorizedItem.UseVisualStyleBackColor = True
        '
        'GroupBox_CommonPOS
        '
        Me.GroupBox_CommonPOS.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_ItemStatusCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.ComboBox_AgeCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.cmbItemStatusCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_ElectronicShelfTag)
        Me.GroupBox_CommonPOS.Controls.Add(Me.TextBox_ItemSurcharge)
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_ItemSurcharge)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_LocalItem)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_RefreshPOSInfo)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_EmployeeDiscount)
        Me.GroupBox_CommonPOS.Controls.Add(Me.TextBox_MixMatch)
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_MixMatch)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_AgeRestrict)
        Me.GroupBox_CommonPOS.Controls.Add(Me.FreedomSystemGroupBox)
        Me.GroupBox_CommonPOS.Controls.Add(Me.TextBox_LinkValue)
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_LinkValue)
        Me.GroupBox_CommonPOS.Controls.Add(Me.GroupBox_LinkCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.TextBox_POSTare)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_CompetitiveItem)
        Me.GroupBox_CommonPOS.Controls.Add(Me.TextBox_AgeCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_RestrictedHours)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_LineDiscount)
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_AgeCode)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_LockedForSale)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_GrillPrint)
        Me.GroupBox_CommonPOS.Controls.Add(Me.Label_PosTare)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_SeniorCitizen)
        Me.GroupBox_CommonPOS.Controls.Add(Me.CheckBox_VisualVerify)
        resources.ApplyResources(Me.GroupBox_CommonPOS, "GroupBox_CommonPOS")
        Me.GroupBox_CommonPOS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_CommonPOS.Name = "GroupBox_CommonPOS"
        Me.GroupBox_CommonPOS.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox_CommonPOS, resources.GetString("GroupBox_CommonPOS.ToolTip"))
        '
        'Label_ItemStatusCode
        '
        resources.ApplyResources(Me.Label_ItemStatusCode, "Label_ItemStatusCode")
        Me.Label_ItemStatusCode.ForeColor = System.Drawing.Color.Black
        Me.Label_ItemStatusCode.Name = "Label_ItemStatusCode"
        '
        'ComboBox_AgeCode
        '
        Me.ComboBox_AgeCode.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_AgeCode, "ComboBox_AgeCode")
        Me.ComboBox_AgeCode.Name = "ComboBox_AgeCode"
        '
        'cmbItemStatusCode
        '
        Me.cmbItemStatusCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbItemStatusCode.FormattingEnabled = True
        Me.cmbItemStatusCode.Items.AddRange(New Object() {resources.GetString("cmbItemStatusCode.Items"), resources.GetString("cmbItemStatusCode.Items1"), resources.GetString("cmbItemStatusCode.Items2"), resources.GetString("cmbItemStatusCode.Items3"), resources.GetString("cmbItemStatusCode.Items4"), resources.GetString("cmbItemStatusCode.Items5"), resources.GetString("cmbItemStatusCode.Items6"), resources.GetString("cmbItemStatusCode.Items7"), resources.GetString("cmbItemStatusCode.Items8"), resources.GetString("cmbItemStatusCode.Items9"), resources.GetString("cmbItemStatusCode.Items10"), resources.GetString("cmbItemStatusCode.Items11"), resources.GetString("cmbItemStatusCode.Items12"), resources.GetString("cmbItemStatusCode.Items13"), resources.GetString("cmbItemStatusCode.Items14"), resources.GetString("cmbItemStatusCode.Items15"), resources.GetString("cmbItemStatusCode.Items16"), resources.GetString("cmbItemStatusCode.Items17"), resources.GetString("cmbItemStatusCode.Items18"), resources.GetString("cmbItemStatusCode.Items19"), resources.GetString("cmbItemStatusCode.Items20"), resources.GetString("cmbItemStatusCode.Items21"), resources.GetString("cmbItemStatusCode.Items22"), resources.GetString("cmbItemStatusCode.Items23"), resources.GetString("cmbItemStatusCode.Items24"), resources.GetString("cmbItemStatusCode.Items25"), resources.GetString("cmbItemStatusCode.Items26"), resources.GetString("cmbItemStatusCode.Items27"), resources.GetString("cmbItemStatusCode.Items28"), resources.GetString("cmbItemStatusCode.Items29"), resources.GetString("cmbItemStatusCode.Items30"), resources.GetString("cmbItemStatusCode.Items31"), resources.GetString("cmbItemStatusCode.Items32"), resources.GetString("cmbItemStatusCode.Items33"), resources.GetString("cmbItemStatusCode.Items34"), resources.GetString("cmbItemStatusCode.Items35"), resources.GetString("cmbItemStatusCode.Items36"), resources.GetString("cmbItemStatusCode.Items37"), resources.GetString("cmbItemStatusCode.Items38"), resources.GetString("cmbItemStatusCode.Items39"), resources.GetString("cmbItemStatusCode.Items40"), resources.GetString("cmbItemStatusCode.Items41"), resources.GetString("cmbItemStatusCode.Items42"), resources.GetString("cmbItemStatusCode.Items43"), resources.GetString("cmbItemStatusCode.Items44"), resources.GetString("cmbItemStatusCode.Items45"), resources.GetString("cmbItemStatusCode.Items46"), resources.GetString("cmbItemStatusCode.Items47"), resources.GetString("cmbItemStatusCode.Items48"), resources.GetString("cmbItemStatusCode.Items49"), resources.GetString("cmbItemStatusCode.Items50")})
        resources.ApplyResources(Me.cmbItemStatusCode, "cmbItemStatusCode")
        Me.cmbItemStatusCode.Name = "cmbItemStatusCode"
        '
        'CheckBox_ElectronicShelfTag
        '
        Me.CheckBox_ElectronicShelfTag.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_ElectronicShelfTag.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_ElectronicShelfTag, "CheckBox_ElectronicShelfTag")
        Me.CheckBox_ElectronicShelfTag.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_ElectronicShelfTag.Name = "CheckBox_ElectronicShelfTag"
        Me.CheckBox_ElectronicShelfTag.UseVisualStyleBackColor = False
        '
        'TextBox_ItemSurcharge
        '
        resources.ApplyResources(Me.TextBox_ItemSurcharge, "TextBox_ItemSurcharge")
        Me.TextBox_ItemSurcharge.Name = "TextBox_ItemSurcharge"
        '
        'Label_ItemSurcharge
        '
        resources.ApplyResources(Me.Label_ItemSurcharge, "Label_ItemSurcharge")
        Me.Label_ItemSurcharge.Name = "Label_ItemSurcharge"
        '
        'CheckBox_LocalItem
        '
        resources.ApplyResources(Me.CheckBox_LocalItem, "CheckBox_LocalItem")
        Me.CheckBox_LocalItem.Name = "CheckBox_LocalItem"
        Me.CheckBox_LocalItem.UseVisualStyleBackColor = True
        '
        'CheckBox_RefreshPOSInfo
        '
        Me.CheckBox_RefreshPOSInfo.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_RefreshPOSInfo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_RefreshPOSInfo, "CheckBox_RefreshPOSInfo")
        Me.CheckBox_RefreshPOSInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_RefreshPOSInfo.Name = "CheckBox_RefreshPOSInfo"
        Me.ToolTip1.SetToolTip(Me.CheckBox_RefreshPOSInfo, resources.GetString("CheckBox_RefreshPOSInfo.ToolTip"))
        Me.CheckBox_RefreshPOSInfo.UseVisualStyleBackColor = False
        '
        'CheckBox_EmployeeDiscount
        '
        resources.ApplyResources(Me.CheckBox_EmployeeDiscount, "CheckBox_EmployeeDiscount")
        Me.CheckBox_EmployeeDiscount.Name = "CheckBox_EmployeeDiscount"
        Me.CheckBox_EmployeeDiscount.UseVisualStyleBackColor = True
        '
        'TextBox_MixMatch
        '
        resources.ApplyResources(Me.TextBox_MixMatch, "TextBox_MixMatch")
        Me.TextBox_MixMatch.Name = "TextBox_MixMatch"
        '
        'Label_MixMatch
        '
        resources.ApplyResources(Me.Label_MixMatch, "Label_MixMatch")
        Me.Label_MixMatch.Name = "Label_MixMatch"
        '
        'CheckBox_AgeRestrict
        '
        Me.CheckBox_AgeRestrict.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_AgeRestrict.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_AgeRestrict, "CheckBox_AgeRestrict")
        Me.CheckBox_AgeRestrict.Name = "CheckBox_AgeRestrict"
        Me.CheckBox_AgeRestrict.UseVisualStyleBackColor = False
        '
        'FreedomSystemGroupBox
        '
        Me.FreedomSystemGroupBox.Controls.Add(Me.ComboBox_KitchenRouteID)
        Me.FreedomSystemGroupBox.Controls.Add(Me.Label2)
        Me.FreedomSystemGroupBox.Controls.Add(Me.CheckBox_ConsolidatePrice)
        Me.FreedomSystemGroupBox.Controls.Add(Me.CheckBox_PrintCondimentOnReceipt)
        Me.FreedomSystemGroupBox.Controls.Add(Me.Label1)
        Me.FreedomSystemGroupBox.Controls.Add(Me.Numeric_RoutingPriority)
        Me.FreedomSystemGroupBox.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.FreedomSystemGroupBox, "FreedomSystemGroupBox")
        Me.FreedomSystemGroupBox.Name = "FreedomSystemGroupBox"
        Me.FreedomSystemGroupBox.TabStop = False
        '
        'ComboBox_KitchenRouteID
        '
        Me.ComboBox_KitchenRouteID.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_KitchenRouteID, "ComboBox_KitchenRouteID")
        Me.ComboBox_KitchenRouteID.Name = "ComboBox_KitchenRouteID"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'CheckBox_ConsolidatePrice
        '
        resources.ApplyResources(Me.CheckBox_ConsolidatePrice, "CheckBox_ConsolidatePrice")
        Me.CheckBox_ConsolidatePrice.Name = "CheckBox_ConsolidatePrice"
        Me.CheckBox_ConsolidatePrice.UseVisualStyleBackColor = True
        '
        'CheckBox_PrintCondimentOnReceipt
        '
        resources.ApplyResources(Me.CheckBox_PrintCondimentOnReceipt, "CheckBox_PrintCondimentOnReceipt")
        Me.CheckBox_PrintCondimentOnReceipt.Name = "CheckBox_PrintCondimentOnReceipt"
        Me.CheckBox_PrintCondimentOnReceipt.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Numeric_RoutingPriority
        '
        resources.ApplyResources(Me.Numeric_RoutingPriority, "Numeric_RoutingPriority")
        Me.Numeric_RoutingPriority.Maximum = New Decimal(New Integer() {22, 0, 0, 0})
        Me.Numeric_RoutingPriority.Name = "Numeric_RoutingPriority"
        '
        'TextBox_LinkValue
        '
        resources.ApplyResources(Me.TextBox_LinkValue, "TextBox_LinkValue")
        Me.TextBox_LinkValue.Name = "TextBox_LinkValue"
        '
        'Label_LinkValue
        '
        resources.ApplyResources(Me.Label_LinkValue, "Label_LinkValue")
        Me.Label_LinkValue.Name = "Label_LinkValue"
        '
        'GroupBox_LinkCode
        '
        Me.GroupBox_LinkCode.Controls.Add(Me.Button_DeleteLink)
        Me.GroupBox_LinkCode.Controls.Add(Me.TextBox_LinkedItem)
        Me.GroupBox_LinkCode.Controls.Add(Me.Button_AddLink_ItemSearch)
        Me.GroupBox_LinkCode.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.GroupBox_LinkCode, "GroupBox_LinkCode")
        Me.GroupBox_LinkCode.Name = "GroupBox_LinkCode"
        Me.GroupBox_LinkCode.TabStop = False
        '
        'Button_DeleteLink
        '
        resources.ApplyResources(Me.Button_DeleteLink, "Button_DeleteLink")
        Me.Button_DeleteLink.Name = "Button_DeleteLink"
        Me.Button_DeleteLink.UseVisualStyleBackColor = True
        '
        'TextBox_LinkedItem
        '
        resources.ApplyResources(Me.TextBox_LinkedItem, "TextBox_LinkedItem")
        Me.TextBox_LinkedItem.Name = "TextBox_LinkedItem"
        Me.TextBox_LinkedItem.TabStop = False
        '
        'Button_AddLink_ItemSearch
        '
        resources.ApplyResources(Me.Button_AddLink_ItemSearch, "Button_AddLink_ItemSearch")
        Me.Button_AddLink_ItemSearch.Name = "Button_AddLink_ItemSearch"
        Me.Button_AddLink_ItemSearch.UseVisualStyleBackColor = True
        '
        'TextBox_POSTare
        '
        resources.ApplyResources(Me.TextBox_POSTare, "TextBox_POSTare")
        Me.TextBox_POSTare.Name = "TextBox_POSTare"
        '
        'CheckBox_CompetitiveItem
        '
        resources.ApplyResources(Me.CheckBox_CompetitiveItem, "CheckBox_CompetitiveItem")
        Me.CheckBox_CompetitiveItem.Name = "CheckBox_CompetitiveItem"
        Me.CheckBox_CompetitiveItem.UseVisualStyleBackColor = True
        '
        'TextBox_AgeCode
        '
        resources.ApplyResources(Me.TextBox_AgeCode, "TextBox_AgeCode")
        Me.TextBox_AgeCode.Name = "TextBox_AgeCode"
        '
        'CheckBox_RestrictedHours
        '
        Me.CheckBox_RestrictedHours.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_RestrictedHours.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_RestrictedHours, "CheckBox_RestrictedHours")
        Me.CheckBox_RestrictedHours.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me.CheckBox_RestrictedHours, CType(0, Short))
        Me.CheckBox_RestrictedHours.Name = "CheckBox_RestrictedHours"
        Me.CheckBox_RestrictedHours.UseVisualStyleBackColor = False
        '
        'CheckBox_LineDiscount
        '
        Me.CheckBox_LineDiscount.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_LineDiscount.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_LineDiscount, "CheckBox_LineDiscount")
        Me.CheckBox_LineDiscount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me.CheckBox_LineDiscount, CType(1, Short))
        Me.CheckBox_LineDiscount.Name = "CheckBox_LineDiscount"
        Me.CheckBox_LineDiscount.UseVisualStyleBackColor = False
        '
        'Label_AgeCode
        '
        resources.ApplyResources(Me.Label_AgeCode, "Label_AgeCode")
        Me.Label_AgeCode.Name = "Label_AgeCode"
        '
        'CheckBox_LockedForSale
        '
        resources.ApplyResources(Me.CheckBox_LockedForSale, "CheckBox_LockedForSale")
        Me.CheckBox_LockedForSale.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_LockedForSale.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_LockedForSale.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me.CheckBox_LockedForSale, CType(2, Short))
        Me.CheckBox_LockedForSale.Name = "CheckBox_LockedForSale"
        Me.CheckBox_LockedForSale.UseVisualStyleBackColor = True
        '
        'CheckBox_GrillPrint
        '
        Me.CheckBox_GrillPrint.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_GrillPrint.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_GrillPrint, "CheckBox_GrillPrint")
        Me.CheckBox_GrillPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_GrillPrint.Name = "CheckBox_GrillPrint"
        Me.CheckBox_GrillPrint.UseVisualStyleBackColor = False
        '
        'Label_PosTare
        '
        resources.ApplyResources(Me.Label_PosTare, "Label_PosTare")
        Me.Label_PosTare.Name = "Label_PosTare"
        '
        'CheckBox_SeniorCitizen
        '
        Me.CheckBox_SeniorCitizen.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_SeniorCitizen.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.CheckBox_SeniorCitizen, "CheckBox_SeniorCitizen")
        Me.CheckBox_SeniorCitizen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_SeniorCitizen.Name = "CheckBox_SeniorCitizen"
        Me.CheckBox_SeniorCitizen.UseVisualStyleBackColor = False
        '
        'CheckBox_VisualVerify
        '
        resources.ApplyResources(Me.CheckBox_VisualVerify, "CheckBox_VisualVerify")
        Me.CheckBox_VisualVerify.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_VisualVerify.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_VisualVerify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_VisualVerify.Name = "CheckBox_VisualVerify"
        Me.CheckBox_VisualVerify.UseVisualStyleBackColor = True
        '
        'CheckBox_Discontinue
        '
        resources.ApplyResources(Me.CheckBox_Discontinue, "CheckBox_Discontinue")
        Me.CheckBox_Discontinue.Name = "CheckBox_Discontinue"
        Me.ToolTip1.SetToolTip(Me.CheckBox_Discontinue, resources.GetString("CheckBox_Discontinue.ToolTip"))
        Me.CheckBox_Discontinue.UseVisualStyleBackColor = True
        '
        'CheckBox_OrderedByInfor
        '
        resources.ApplyResources(Me.CheckBox_OrderedByInfor, "CheckBox_OrderedByInfor")
        Me.CheckBox_OrderedByInfor.Name = "CheckBox_OrderedByInfor"
        Me.ToolTip1.SetToolTip(Me.CheckBox_OrderedByInfor, resources.GetString("CheckBox_OrderedByInfor.ToolTip"))
        Me.CheckBox_OrderedByInfor.UseVisualStyleBackColor = True
        '
        'cmbRetailUom
        '
        Me.cmbRetailUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRetailUom.FormattingEnabled = True
        resources.ApplyResources(Me.cmbRetailUom, "cmbRetailUom")
        Me.cmbRetailUom.Name = "cmbRetailUom"
        '
        'lbl365RetailUom
        '
        resources.ApplyResources(Me.lbl365RetailUom, "lbl365RetailUom")
        Me.lbl365RetailUom.Name = "lbl365RetailUom"
        '
        '_Frame1_1
        '
        Me._Frame1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_1.Controls.Add(Me.ugrdStoreList)
        Me._Frame1_1.Controls.Add(Me.cmbStates)
        Me._Frame1_1.Controls.Add(Me._optSelection_4)
        Me._Frame1_1.Controls.Add(Me._optSelection_3)
        Me._Frame1_1.Controls.Add(Me.cmbZones)
        Me._Frame1_1.Controls.Add(Me._optSelection_2)
        Me._Frame1_1.Controls.Add(Me._optSelection_1)
        Me._Frame1_1.Controls.Add(Me._optSelection_0)
        resources.ApplyResources(Me._Frame1_1, "_Frame1_1")
        Me._Frame1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.SetIndex(Me._Frame1_1, CType(1, Short))
        Me._Frame1_1.Name = "_Frame1_1"
        Me._Frame1_1.TabStop = False
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridBand1.ColHeadersVisible = False
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(148, 0)
        UltraGridColumn2.Width = 20
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdStoreList, "ugrdStoreList")
        Me.ugrdStoreList.Name = "ugrdStoreList"
        Me.ugrdStoreList.TabStop = False
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStates, "cmbStates")
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.Sorted = True
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZones, "cmbZones")
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.Sorted = True
        '
        'Button_TaxOverride
        '
        resources.ApplyResources(Me.Button_TaxOverride, "Button_TaxOverride")
        Me.Button_TaxOverride.Name = "Button_TaxOverride"
        Me.Button_TaxOverride.UseVisualStyleBackColor = True
        '
        'optSelection
        '
        '
        'ComboBox_SubTeam
        '
        Me.ComboBox_SubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SubTeam.FormattingEnabled = True
        resources.ApplyResources(Me.ComboBox_SubTeam, "ComboBox_SubTeam")
        Me.ComboBox_SubTeam.Name = "ComboBox_SubTeam"
        '
        'GroupBox_SubTeam
        '
        Me.GroupBox_SubTeam.Controls.Add(Me.ComboBox_SubTeam)
        resources.ApplyResources(Me.GroupBox_SubTeam, "GroupBox_SubTeam")
        Me.GroupBox_SubTeam.ForeColor = System.Drawing.Color.Black
        Me.GroupBox_SubTeam.Name = "GroupBox_SubTeam"
        Me.GroupBox_SubTeam.TabStop = False
        '
        'ExemptGroupBox
        '
        Me.ExemptGroupBox.Controls.Add(Me.ExemptCheckBox)
        Me.ExemptGroupBox.Controls.Add(Me.Exempt_Button)
        resources.ApplyResources(Me.ExemptGroupBox, "ExemptGroupBox")
        Me.ExemptGroupBox.Name = "ExemptGroupBox"
        Me.ExemptGroupBox.TabStop = False
        '
        'ExemptCheckBox
        '
        Me.ExemptCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.ExemptCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ExemptCheckBox, "ExemptCheckBox")
        Me.ExemptCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExemptCheckBox.Name = "ExemptCheckBox"
        Me.ExemptCheckBox.UseVisualStyleBackColor = False
        '
        'Exempt_Button
        '
        resources.ApplyResources(Me.Exempt_Button, "Exempt_Button")
        Me.Exempt_Button.Name = "Exempt_Button"
        Me.Exempt_Button.UseVisualStyleBackColor = True
        '
        'GroupBox_Authorization
        '
        Me.GroupBox_Authorization.Controls.Add(Me.CheckBox_OrderedByInfor)
        Me.GroupBox_Authorization.Controls.Add(Me.CheckBox_AuthorizedItem)
        resources.ApplyResources(Me.GroupBox_Authorization, "GroupBox_Authorization")
        Me.GroupBox_Authorization.ForeColor = System.Drawing.Color.Black
        Me.GroupBox_Authorization.Name = "GroupBox_Authorization"
        Me.GroupBox_Authorization.TabStop = False
        '
        'grpScanAudit
        '
        Me.grpScanAudit.Controls.Add(Me.lblLastNonDTSScan)
        Me.grpScanAudit.Controls.Add(Me.lblLastDTSScan)
        Me.grpScanAudit.Controls.Add(Me.Label4)
        Me.grpScanAudit.Controls.Add(Me.Label3)
        resources.ApplyResources(Me.grpScanAudit, "grpScanAudit")
        Me.grpScanAudit.Name = "grpScanAudit"
        Me.grpScanAudit.TabStop = False
        '
        'lblLastNonDTSScan
        '
        Me.lblLastNonDTSScan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblLastNonDTSScan, "lblLastNonDTSScan")
        Me.lblLastNonDTSScan.Name = "lblLastNonDTSScan"
        '
        'lblLastDTSScan
        '
        Me.lblLastDTSScan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblLastDTSScan, "lblLastDTSScan")
        Me.lblLastDTSScan.Name = "lblLastDTSScan"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'GroupBox_Discontinue
        '
        Me.GroupBox_Discontinue.Controls.Add(Me.CheckBox_Discontinue)
        resources.ApplyResources(Me.GroupBox_Discontinue, "GroupBox_Discontinue")
        Me.GroupBox_Discontinue.ForeColor = System.Drawing.Color.Black
        Me.GroupBox_Discontinue.Name = "GroupBox_Discontinue"
        Me.GroupBox_Discontinue.TabStop = False
        '
        'cmbScaleUom
        '
        Me.cmbScaleUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbScaleUom.FormattingEnabled = True
        resources.ApplyResources(Me.cmbScaleUom, "cmbScaleUom")
        Me.cmbScaleUom.Name = "cmbScaleUom"
        '
        'lblScaleUom
        '
        resources.ApplyResources(Me.lblScaleUom, "lblScaleUom")
        Me.lblScaleUom.Name = "lblScaleUom"
        '
        'lblFixedWeight
        '
        resources.ApplyResources(Me.lblFixedWeight, "lblFixedWeight")
        Me.lblFixedWeight.Name = "lblFixedWeight"
        '
        'TextBox_FixedWeight
        '
        resources.ApplyResources(Me.TextBox_FixedWeight, "TextBox_FixedWeight")
        Me.TextBox_FixedWeight.Name = "TextBox_FixedWeight"
        '
        'lblByCount
        '
        resources.ApplyResources(Me.lblByCount, "lblByCount")
        Me.lblByCount.Name = "lblByCount"
        '
        'GroupBox_StoreRetailUOM
        '
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.cmbRetailUom)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.ByCountNumericEditor)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.lbl365RetailUom)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.lblByCount)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.lblScaleUom)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.cmbScaleUom)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.TextBox_FixedWeight)
        Me.GroupBox_StoreRetailUOM.Controls.Add(Me.lblFixedWeight)
        resources.ApplyResources(Me.GroupBox_StoreRetailUOM, "GroupBox_StoreRetailUOM")
        Me.GroupBox_StoreRetailUOM.Name = "GroupBox_StoreRetailUOM"
        Me.GroupBox_StoreRetailUOM.TabStop = False
        '
        'ByCountNumericEditor
        '
        Me.ByCountNumericEditor.AlwaysInEditMode = True
        resources.ApplyResources(Me.ByCountNumericEditor, "ByCountNumericEditor")
        Me.ByCountNumericEditor.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.ByCountNumericEditor.MaskInput = "nnnnnnnnn"
        Me.ByCountNumericEditor.MaxValue = 36
        Me.ByCountNumericEditor.MinValue = 1
        Me.ByCountNumericEditor.Name = "ByCountNumericEditor"
        Me.ByCountNumericEditor.Nullable = True
        Me.ByCountNumericEditor.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.ByCountNumericEditor.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always
        Me.ByCountNumericEditor.Value = Nothing
        '
        'frmItemStore
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.GroupBox_StoreRetailUOM)
        Me.Controls.Add(Me.GroupBox_Discontinue)
        Me.Controls.Add(Me.grpScanAudit)
        Me.Controls.Add(Me.GroupBox_Authorization)
        Me.Controls.Add(Me.ExemptGroupBox)
        Me.Controls.Add(Me.GroupBox_SubTeam)
        Me.Controls.Add(Me.Button_TaxOverride)
        Me.Controls.Add(Me._Frame1_1)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.GroupBox_CommonPOS)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemStore"
        Me.ShowInTaskbar = False
        Me.GroupBox_CommonPOS.ResumeLayout(False)
        Me.GroupBox_CommonPOS.PerformLayout()
        Me.FreedomSystemGroupBox.ResumeLayout(False)
        Me.FreedomSystemGroupBox.PerformLayout()
        CType(Me.Numeric_RoutingPriority, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_LinkCode.ResumeLayout(False)
        Me.GroupBox_LinkCode.PerformLayout()
        Me._Frame1_1.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_SubTeam.ResumeLayout(False)
        Me.ExemptGroupBox.ResumeLayout(False)
        Me.GroupBox_Authorization.ResumeLayout(False)
        Me.GroupBox_Authorization.PerformLayout()
        Me.grpScanAudit.ResumeLayout(False)
        Me.grpScanAudit.PerformLayout()
        Me.GroupBox_Discontinue.ResumeLayout(False)
        Me.GroupBox_Discontinue.PerformLayout()
        Me.GroupBox_StoreRetailUOM.ResumeLayout(False)
        Me.GroupBox_StoreRetailUOM.PerformLayout()
        CType(Me.ByCountNumericEditor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_TaxOverride As System.Windows.Forms.Button
    Friend WithEvents CheckBox_CompetitiveItem As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_SeniorCitizen As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_GrillPrint As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_VisualVerify As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_POSTare As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_AgeCode As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_LinkedItem As System.Windows.Forms.TextBox
    Friend WithEvents Label_AgeCode As System.Windows.Forms.Label
    Friend WithEvents Label_PosTare As System.Windows.Forms.Label
    Friend WithEvents Button_AddLink_ItemSearch As System.Windows.Forms.Button
    Friend WithEvents Button_DeleteLink As System.Windows.Forms.Button
    Friend WithEvents GroupBox_LinkCode As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_SubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox_SubTeam As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_LinkValue As System.Windows.Forms.TextBox
    Friend WithEvents Label_LinkValue As System.Windows.Forms.Label
    Friend WithEvents ExemptGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Exempt_Button As System.Windows.Forms.Button
    Public WithEvents ExemptCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FreedomSystemGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_ConsolidatePrice As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_PrintCondimentOnReceipt As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Numeric_RoutingPriority As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_KitchenRouteID As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox_AgeRestrict As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_Authorization As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_AuthorizedItem As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_MixMatch As System.Windows.Forms.TextBox
    Friend WithEvents Label_MixMatch As System.Windows.Forms.Label
    Friend WithEvents CheckBox_EmployeeDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents grpScanAudit As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblLastDTSScan As System.Windows.Forms.Label
    Friend WithEvents lblLastNonDTSScan As System.Windows.Forms.Label
    Public WithEvents CheckBox_RefreshPOSInfo As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_LocalItem As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_ItemSurcharge As System.Windows.Forms.TextBox
    Friend WithEvents Label_ItemSurcharge As System.Windows.Forms.Label
    Public WithEvents CheckBox_ElectronicShelfTag As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_Discontinue As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_Discontinue As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_OrderedByInfor As CheckBox
    Friend WithEvents ComboBox_AgeCode As System.Windows.Forms.ComboBox
    Friend WithEvents lbl365RetailUom As System.Windows.Forms.Label
    Friend WithEvents cmbScaleUom As System.Windows.Forms.ComboBox
    Friend WithEvents lblScaleUom As System.Windows.Forms.Label
    Friend WithEvents lblFixedWeight As System.Windows.Forms.Label
    Friend WithEvents TextBox_FixedWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblByCount As System.Windows.Forms.Label
    Friend WithEvents GroupBox_StoreRetailUOM As System.Windows.Forms.GroupBox
    Friend WithEvents ByCountNumericEditor As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents cmbRetailUom As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemStatusCode As ComboBox
    Friend WithEvents Label_ItemStatusCode As Label

#End Region
End Class
