<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CostPromotionDetail
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CostPromotionDetail))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label_PkgDescValue = New System.Windows.Forms.Label
        Me.Label_ItemValue = New System.Windows.Forms.Label
        Me.Label_VendorValue = New System.Windows.Forms.Label
        Me.Label_PkgDesc = New System.Windows.Forms.Label
        Me.Label_Item = New System.Windows.Forms.Label
        Me.Label_Vendor = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label_NotStackable = New System.Windows.Forms.Label
        Me.CheckBox_NotStackable = New System.Windows.Forms.CheckBox
        Me.Label_AmountType = New System.Windows.Forms.Label
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.TextBox_Amount = New System.Windows.Forms.TextBox
        Me.TextBox_CaseQty = New System.Windows.Forms.TextBox
        Me.ComboBox_DealType = New System.Windows.Forms.ComboBox
        Me.Label_EndDate = New System.Windows.Forms.Label
        Me.Label_StartDate = New System.Windows.Forms.Label
        Me.Label_Amount = New System.Windows.Forms.Label
        Me.Label_CaseQty = New System.Windows.Forms.Label
        Me.Label_DealType = New System.Windows.Forms.Label
        Me.Label_PromoCodeType = New System.Windows.Forms.Label
        Me.ComboBox_PromoCodeType = New System.Windows.Forms.ComboBox
        Me.Button_Exit = New System.Windows.Forms.Button
        Me.Button_ApplyChanges = New System.Windows.Forms.Button
        Me.GroupBox_StoreSel = New System.Windows.Forms.GroupBox
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.RadioButton_Manual = New System.Windows.Forms.RadioButton
        Me.RadioButton_All = New System.Windows.Forms.RadioButton
        Me.RadioButton_Zone = New System.Windows.Forms.RadioButton
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.RadioButton_State = New System.Windows.Forms.RadioButton
        Me.RadioButton_AllWFM = New System.Windows.Forms.RadioButton
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_StoreSel.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label_PkgDescValue)
        Me.GroupBox1.Controls.Add(Me.Label_ItemValue)
        Me.GroupBox1.Controls.Add(Me.Label_VendorValue)
        Me.GroupBox1.Controls.Add(Me.Label_PkgDesc)
        Me.GroupBox1.Controls.Add(Me.Label_Item)
        Me.GroupBox1.Controls.Add(Me.Label_Vendor)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(325, 96)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'Label_PkgDescValue
        '
        Me.Label_PkgDescValue.AutoSize = True
        Me.Label_PkgDescValue.Location = New System.Drawing.Point(116, 68)
        Me.Label_PkgDescValue.Name = "Label_PkgDescValue"
        Me.Label_PkgDescValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_PkgDescValue.TabIndex = 7
        Me.Label_PkgDescValue.Text = "Label1"
        '
        'Label_ItemValue
        '
        Me.Label_ItemValue.AutoSize = True
        Me.Label_ItemValue.Location = New System.Drawing.Point(116, 42)
        Me.Label_ItemValue.Name = "Label_ItemValue"
        Me.Label_ItemValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_ItemValue.TabIndex = 6
        Me.Label_ItemValue.Text = "Label1"
        '
        'Label_VendorValue
        '
        Me.Label_VendorValue.AutoSize = True
        Me.Label_VendorValue.Location = New System.Drawing.Point(116, 16)
        Me.Label_VendorValue.Name = "Label_VendorValue"
        Me.Label_VendorValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_VendorValue.TabIndex = 5
        Me.Label_VendorValue.Text = "Label1"
        '
        'Label_PkgDesc
        '
        Me.Label_PkgDesc.BackColor = System.Drawing.Color.Transparent
        Me.Label_PkgDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PkgDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_PkgDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PkgDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_PkgDesc.Location = New System.Drawing.Point(9, 68)
        Me.Label_PkgDesc.Name = "Label_PkgDesc"
        Me.Label_PkgDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PkgDesc.Size = New System.Drawing.Size(101, 17)
        Me.Label_PkgDesc.TabIndex = 3
        Me.Label_PkgDesc.Text = "Retail Package :"
        Me.Label_PkgDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Item
        '
        Me.Label_Item.BackColor = System.Drawing.Color.Transparent
        Me.Label_Item.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Item.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_Item.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Item.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Item.Location = New System.Drawing.Point(34, 42)
        Me.Label_Item.Name = "Label_Item"
        Me.Label_Item.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Item.Size = New System.Drawing.Size(76, 17)
        Me.Label_Item.TabIndex = 2
        Me.Label_Item.Text = "Item :"
        Me.Label_Item.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Vendor
        '
        Me.Label_Vendor.BackColor = System.Drawing.Color.Transparent
        Me.Label_Vendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Vendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_Vendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Vendor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Vendor.Location = New System.Drawing.Point(34, 16)
        Me.Label_Vendor.Name = "Label_Vendor"
        Me.Label_Vendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Vendor.Size = New System.Drawing.Size(76, 17)
        Me.Label_Vendor.TabIndex = 1
        Me.Label_Vendor.Text = "Vendor :"
        Me.Label_Vendor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label_NotStackable)
        Me.GroupBox2.Controls.Add(Me.CheckBox_NotStackable)
        Me.GroupBox2.Controls.Add(Me.Label_AmountType)
        Me.GroupBox2.Controls.Add(Me.dtpEndDate)
        Me.GroupBox2.Controls.Add(Me.dtpStartDate)
        Me.GroupBox2.Controls.Add(Me.TextBox_Amount)
        Me.GroupBox2.Controls.Add(Me.TextBox_CaseQty)
        Me.GroupBox2.Controls.Add(Me.ComboBox_DealType)
        Me.GroupBox2.Controls.Add(Me.Label_EndDate)
        Me.GroupBox2.Controls.Add(Me.Label_StartDate)
        Me.GroupBox2.Controls.Add(Me.Label_Amount)
        Me.GroupBox2.Controls.Add(Me.Label_CaseQty)
        Me.GroupBox2.Controls.Add(Me.Label_DealType)
        Me.GroupBox2.Controls.Add(Me.Label_PromoCodeType)
        Me.GroupBox2.Controls.Add(Me.ComboBox_PromoCodeType)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 114)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(325, 211)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        '
        'Label_NotStackable
        '
        Me.Label_NotStackable.BackColor = System.Drawing.Color.Transparent
        Me.Label_NotStackable.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_NotStackable.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_NotStackable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_NotStackable.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_NotStackable.Location = New System.Drawing.Point(6, 180)
        Me.Label_NotStackable.Name = "Label_NotStackable"
        Me.Label_NotStackable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_NotStackable.Size = New System.Drawing.Size(104, 17)
        Me.Label_NotStackable.TabIndex = 117
        Me.Label_NotStackable.Text = "NOT Stackable :"
        Me.Label_NotStackable.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CheckBox_NotStackable
        '
        Me.CheckBox_NotStackable.AutoSize = True
        Me.CheckBox_NotStackable.Location = New System.Drawing.Point(119, 180)
        Me.CheckBox_NotStackable.Name = "CheckBox_NotStackable"
        Me.CheckBox_NotStackable.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_NotStackable.TabIndex = 116
        Me.CheckBox_NotStackable.UseVisualStyleBackColor = True
        '
        'Label_AmountType
        '
        Me.Label_AmountType.AutoSize = True
        Me.Label_AmountType.Location = New System.Drawing.Point(183, 104)
        Me.Label_AmountType.Name = "Label_AmountType"
        Me.Label_AmountType.Size = New System.Drawing.Size(0, 13)
        Me.Label_AmountType.TabIndex = 57
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Location = New System.Drawing.Point(119, 153)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpEndDate.TabIndex = 56
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(119, 126)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpStartDate.TabIndex = 55
        '
        'TextBox_Amount
        '
        Me.TextBox_Amount.Location = New System.Drawing.Point(119, 100)
        Me.TextBox_Amount.MaxLength = 11
        Me.TextBox_Amount.Name = "TextBox_Amount"
        Me.TextBox_Amount.Size = New System.Drawing.Size(58, 20)
        Me.TextBox_Amount.TabIndex = 17
        '
        'TextBox_CaseQty
        '
        Me.TextBox_CaseQty.Location = New System.Drawing.Point(119, 74)
        Me.TextBox_CaseQty.Name = "TextBox_CaseQty"
        Me.TextBox_CaseQty.Size = New System.Drawing.Size(58, 20)
        Me.TextBox_CaseQty.TabIndex = 16
        Me.TextBox_CaseQty.Text = "1"
        '
        'ComboBox_DealType
        '
        Me.ComboBox_DealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_DealType.FormattingEnabled = True
        Me.ComboBox_DealType.Location = New System.Drawing.Point(119, 47)
        Me.ComboBox_DealType.Name = "ComboBox_DealType"
        Me.ComboBox_DealType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_DealType.TabIndex = 15
        '
        'Label_EndDate
        '
        Me.Label_EndDate.BackColor = System.Drawing.Color.Transparent
        Me.Label_EndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_EndDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_EndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_EndDate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_EndDate.Location = New System.Drawing.Point(25, 157)
        Me.Label_EndDate.Name = "Label_EndDate"
        Me.Label_EndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_EndDate.Size = New System.Drawing.Size(85, 17)
        Me.Label_EndDate.TabIndex = 14
        Me.Label_EndDate.Text = "End Date :"
        Me.Label_EndDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_StartDate
        '
        Me.Label_StartDate.BackColor = System.Drawing.Color.Transparent
        Me.Label_StartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_StartDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_StartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_StartDate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_StartDate.Location = New System.Drawing.Point(25, 130)
        Me.Label_StartDate.Name = "Label_StartDate"
        Me.Label_StartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_StartDate.Size = New System.Drawing.Size(85, 17)
        Me.Label_StartDate.TabIndex = 13
        Me.Label_StartDate.Text = "Start Date :"
        Me.Label_StartDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Amount
        '
        Me.Label_Amount.BackColor = System.Drawing.Color.Transparent
        Me.Label_Amount.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Amount.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_Amount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Amount.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Amount.Location = New System.Drawing.Point(25, 103)
        Me.Label_Amount.Name = "Label_Amount"
        Me.Label_Amount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Amount.Size = New System.Drawing.Size(85, 17)
        Me.Label_Amount.TabIndex = 12
        Me.Label_Amount.Text = "Amount :"
        Me.Label_Amount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_CaseQty
        '
        Me.Label_CaseQty.BackColor = System.Drawing.Color.Transparent
        Me.Label_CaseQty.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_CaseQty.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_CaseQty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_CaseQty.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_CaseQty.Location = New System.Drawing.Point(25, 77)
        Me.Label_CaseQty.Name = "Label_CaseQty"
        Me.Label_CaseQty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_CaseQty.Size = New System.Drawing.Size(85, 17)
        Me.Label_CaseQty.TabIndex = 11
        Me.Label_CaseQty.Text = "Case Qty :"
        Me.Label_CaseQty.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_DealType
        '
        Me.Label_DealType.BackColor = System.Drawing.Color.Transparent
        Me.Label_DealType.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_DealType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_DealType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_DealType.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_DealType.Location = New System.Drawing.Point(25, 50)
        Me.Label_DealType.Name = "Label_DealType"
        Me.Label_DealType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_DealType.Size = New System.Drawing.Size(85, 17)
        Me.Label_DealType.TabIndex = 10
        Me.Label_DealType.Text = "Deal Type :"
        Me.Label_DealType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_PromoCodeType
        '
        Me.Label_PromoCodeType.BackColor = System.Drawing.Color.Transparent
        Me.Label_PromoCodeType.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PromoCodeType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_PromoCodeType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PromoCodeType.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_PromoCodeType.Location = New System.Drawing.Point(25, 23)
        Me.Label_PromoCodeType.Name = "Label_PromoCodeType"
        Me.Label_PromoCodeType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PromoCodeType.Size = New System.Drawing.Size(85, 17)
        Me.Label_PromoCodeType.TabIndex = 9
        Me.Label_PromoCodeType.Text = "Promo Type :"
        Me.Label_PromoCodeType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox_PromoCodeType
        '
        Me.ComboBox_PromoCodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_PromoCodeType.FormattingEnabled = True
        Me.ComboBox_PromoCodeType.Location = New System.Drawing.Point(119, 20)
        Me.ComboBox_PromoCodeType.Name = "ComboBox_PromoCodeType"
        Me.ComboBox_PromoCodeType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_PromoCodeType.TabIndex = 0
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Exit.Location = New System.Drawing.Point(605, 333)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 97
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Exit, "Exit")
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Button_ApplyChanges
        '
        Me.Button_ApplyChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ApplyChanges.BackColor = System.Drawing.SystemColors.Control
        Me.Button_ApplyChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_ApplyChanges.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_ApplyChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_ApplyChanges.Image = CType(resources.GetObject("Button_ApplyChanges.Image"), System.Drawing.Image)
        Me.Button_ApplyChanges.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_ApplyChanges.Location = New System.Drawing.Point(558, 333)
        Me.Button_ApplyChanges.Name = "Button_ApplyChanges"
        Me.Button_ApplyChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_ApplyChanges.Size = New System.Drawing.Size(41, 41)
        Me.Button_ApplyChanges.TabIndex = 114
        Me.Button_ApplyChanges.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_ApplyChanges, "Apply Changes")
        Me.Button_ApplyChanges.UseVisualStyleBackColor = False
        '
        'GroupBox_StoreSel
        '
        Me.GroupBox_StoreSel.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_StoreSel.Controls.Add(Me.ugrdStoreList)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_Manual)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_All)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_Zone)
        Me.GroupBox_StoreSel.Controls.Add(Me.cmbZones)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_State)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_AllWFM)
        Me.GroupBox_StoreSel.Controls.Add(Me.cmbStates)
        Me.GroupBox_StoreSel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_StoreSel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_StoreSel.Location = New System.Drawing.Point(343, 12)
        Me.GroupBox_StoreSel.Name = "GroupBox_StoreSel"
        Me.GroupBox_StoreSel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_StoreSel.Size = New System.Drawing.Size(303, 313)
        Me.GroupBox_StoreSel.TabIndex = 115
        Me.GroupBox_StoreSel.TabStop = False
        Me.GroupBox_StoreSel.Text = "Store Selection"
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance2.TextHAlign = Infragistics.Win.HAlign.Center
        UltraGridColumn2.Header.Appearance = Appearance2
        UltraGridColumn2.Header.Caption = "Stores"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance3.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance3
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance4.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance4
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance6.BackColor2 = System.Drawing.SystemColors.Control
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Appearance7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance7
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdStoreList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdStoreList.Location = New System.Drawing.Point(32, 100)
        Me.ugrdStoreList.Name = "ugrdStoreList"
        Me.ugrdStoreList.Size = New System.Drawing.Size(241, 196)
        Me.ugrdStoreList.TabIndex = 9
        '
        'RadioButton_Manual
        '
        Me.RadioButton_Manual.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_Manual.Checked = True
        Me.RadioButton_Manual.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_Manual.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_Manual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_Manual.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_Manual.Location = New System.Drawing.Point(37, 21)
        Me.RadioButton_Manual.Name = "RadioButton_Manual"
        Me.RadioButton_Manual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_Manual.Size = New System.Drawing.Size(66, 17)
        Me.RadioButton_Manual.TabIndex = 0
        Me.RadioButton_Manual.TabStop = True
        Me.RadioButton_Manual.Text = "Manual"
        Me.RadioButton_Manual.UseVisualStyleBackColor = False
        '
        'RadioButton_All
        '
        Me.RadioButton_All.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_All.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_All.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_All.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_All.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_All.Location = New System.Drawing.Point(115, 21)
        Me.RadioButton_All.Name = "RadioButton_All"
        Me.RadioButton_All.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_All.Size = New System.Drawing.Size(78, 17)
        Me.RadioButton_All.TabIndex = 1
        Me.RadioButton_All.TabStop = True
        Me.RadioButton_All.Text = "All Stores"
        Me.RadioButton_All.UseVisualStyleBackColor = False
        '
        'RadioButton_Zone
        '
        Me.RadioButton_Zone.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_Zone.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_Zone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_Zone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_Zone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_Zone.Location = New System.Drawing.Point(37, 46)
        Me.RadioButton_Zone.Name = "RadioButton_Zone"
        Me.RadioButton_Zone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_Zone.Size = New System.Drawing.Size(71, 17)
        Me.RadioButton_Zone.TabIndex = 3
        Me.RadioButton_Zone.TabStop = True
        Me.RadioButton_Zone.Text = "By Zone"
        Me.RadioButton_Zone.UseVisualStyleBackColor = False
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZones.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Location = New System.Drawing.Point(112, 42)
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZones.Size = New System.Drawing.Size(161, 22)
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabIndex = 4
        '
        'RadioButton_State
        '
        Me.RadioButton_State.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_State.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_State.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_State.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_State.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_State.Location = New System.Drawing.Point(37, 71)
        Me.RadioButton_State.Name = "RadioButton_State"
        Me.RadioButton_State.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_State.Size = New System.Drawing.Size(70, 17)
        Me.RadioButton_State.TabIndex = 5
        Me.RadioButton_State.TabStop = True
        Me.RadioButton_State.Text = "By State"
        Me.RadioButton_State.UseVisualStyleBackColor = False
        '
        'RadioButton_AllWFM
        '
        Me.RadioButton_AllWFM.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_AllWFM.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_AllWFM.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_AllWFM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_AllWFM.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_AllWFM.Location = New System.Drawing.Point(201, 21)
        Me.RadioButton_AllWFM.Name = "RadioButton_AllWFM"
        Me.RadioButton_AllWFM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_AllWFM.Size = New System.Drawing.Size(81, 17)
        Me.RadioButton_AllWFM.TabIndex = 2
        Me.RadioButton_AllWFM.TabStop = True
        Me.RadioButton_AllWFM.Text = "All WFM"
        Me.RadioButton_AllWFM.UseVisualStyleBackColor = False
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Location = New System.Drawing.Point(112, 69)
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStates.Size = New System.Drawing.Size(161, 22)
        Me.cmbStates.Sorted = True
        Me.cmbStates.TabIndex = 6
        '
        'CostPromotionDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 386)
        Me.Controls.Add(Me.GroupBox_StoreSel)
        Me.Controls.Add(Me.Button_ApplyChanges)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "CostPromotionDetail"
        Me.Text = "Cost Promotion"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_StoreSel.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label_PkgDescValue As System.Windows.Forms.Label
    Friend WithEvents Label_ItemValue As System.Windows.Forms.Label
    Friend WithEvents Label_VendorValue As System.Windows.Forms.Label
    Public WithEvents Label_PkgDesc As System.Windows.Forms.Label
    Public WithEvents Label_Item As System.Windows.Forms.Label
    Public WithEvents Label_Vendor As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents Label_PromoCodeType As System.Windows.Forms.Label
    Friend WithEvents ComboBox_PromoCodeType As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_DealType As System.Windows.Forms.ComboBox
    Public WithEvents Label_EndDate As System.Windows.Forms.Label
    Public WithEvents Label_StartDate As System.Windows.Forms.Label
    Public WithEvents Label_Amount As System.Windows.Forms.Label
    Public WithEvents Label_CaseQty As System.Windows.Forms.Label
    Public WithEvents Label_DealType As System.Windows.Forms.Label
    Friend WithEvents TextBox_CaseQty As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Amount As System.Windows.Forms.TextBox
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents Label_AmountType As System.Windows.Forms.Label
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Button_ApplyChanges As System.Windows.Forms.Button
    Public WithEvents GroupBox_StoreSel As System.Windows.Forms.GroupBox
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents RadioButton_Manual As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_All As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_Zone As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents RadioButton_State As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_AllWFM As System.Windows.Forms.RadioButton
    Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents Label_NotStackable As System.Windows.Forms.Label
    Friend WithEvents CheckBox_NotStackable As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
