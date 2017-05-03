<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderItemSearch
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        Me.IsInitializing = True
		'This call is required by the Windows Form Designer.
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
	Public WithEvents cmbDistSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderItemSearch))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Pre_Order")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EXEDistributed")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemId")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PkgDesc")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NA")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatus", 0, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatusFull", 1)
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
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmbDistSubTeam = New System.Windows.Forms.ComboBox()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me.cmbBrand = New System.Windows.Forms.ComboBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_7 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.lblSubTeam = New System.Windows.Forms.Label()
        Me.ugrdSearchResults = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.fraDisplay = New System.Windows.Forms.GroupBox()
        Me.optNonPreOrder = New System.Windows.Forms.RadioButton()
        Me.optPreOrder = New System.Windows.Forms.RadioButton()
        Me.chkIncludeNotAvailable = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkVendorItemStatus_Seasonal = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_VendorDiscontinued = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_MfgDiscontinued = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_NotAvailable = New System.Windows.Forms.CheckBox()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraDisplay.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.cmdSelect.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Image = CType(resources.GetObject("cmdSelect.Image"), System.Drawing.Image)
        Me.cmdSelect.Location = New System.Drawing.Point(894, 512)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(41, 41)
        Me.cmdSelect.TabIndex = 13
        Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelect, "Select Item")
        Me.cmdSelect.UseVisualStyleBackColor = False
        Me.cmdSelect.UseWaitCursor = True
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(942, 512)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 14
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        Me.cmdExit.UseWaitCursor = True
        '
        'cmbDistSubTeam
        '
        Me.cmbDistSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbDistSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDistSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDistSubTeam.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.cmbDistSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDistSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDistSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDistSubTeam.Location = New System.Drawing.Point(112, 198)
        Me.cmbDistSubTeam.Name = "cmbDistSubTeam"
        Me.cmbDistSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbDistSubTeam.Size = New System.Drawing.Size(169, 22)
        Me.cmbDistSubTeam.Sorted = True
        Me.cmbDistSubTeam.TabIndex = 9
        Me.cmbDistSubTeam.UseWaitCursor = True
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(112, 29)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(225, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 2
        Me.cmbStore.UseWaitCursor = True
        '
        'cmbBrand
        '
        Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBrand.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBrand.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBrand.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBrand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBrand.Location = New System.Drawing.Point(112, 173)
        Me.cmbBrand.Name = "cmbBrand"
        Me.cmbBrand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBrand.Size = New System.Drawing.Size(169, 22)
        Me.cmbBrand.Sorted = True
        Me.cmbBrand.TabIndex = 8
        Me.cmbBrand.UseWaitCursor = True
        '
        '_txtField_5
        '
        Me._txtField_5.AcceptsReturn = True
        Me._txtField_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_5.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._txtField_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
        Me._txtField_5.Location = New System.Drawing.Point(112, 100)
        Me._txtField_5.MaxLength = 20
        Me._txtField_5.Name = "_txtField_5"
        Me._txtField_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_5.Size = New System.Drawing.Size(169, 20)
        Me._txtField_5.TabIndex = 5
        Me._txtField_5.Tag = "Integer"
        Me._txtField_5.UseWaitCursor = True
        '
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkDiscontinued.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.chkDiscontinued.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Location = New System.Drawing.Point(822, 193)
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDiscontinued.Size = New System.Drawing.Size(145, 17)
        Me.chkDiscontinued.TabIndex = 9
        Me.chkDiscontinued.Text = "Include Discontinued"
        Me.chkDiscontinued.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDiscontinued.UseVisualStyleBackColor = False
        Me.chkDiscontinued.UseWaitCursor = True
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._txtField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Location = New System.Drawing.Point(112, 6)
        Me._txtField_4.MaxLength = 50
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_4.Size = New System.Drawing.Size(337, 20)
        Me._txtField_4.TabIndex = 1
        Me._txtField_4.Tag = "String"
        Me._txtField_4.UseWaitCursor = True
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(112, 54)
        Me._txtField_0.MaxLength = 60
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(337, 20)
        Me._txtField_0.TabIndex = 3
        Me._txtField_0.Tag = "String"
        Me._txtField_0.UseWaitCursor = True
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(112, 77)
        Me._txtField_1.MaxLength = 13
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(169, 20)
        Me._txtField_1.TabIndex = 0
        Me._txtField_1.Tag = "String"
        Me._txtField_1.UseWaitCursor = True
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(19, 193)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(86, 35)
        Me._lblLabel_5.TabIndex = 19
        Me._lblLabel_5.Text = "Distribution  Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_5.UseWaitCursor = True
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(32, 512)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(409, 33)
        Me.lblStatus.TabIndex = 18
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblStatus.UseWaitCursor = True
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(16, 32)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_4.TabIndex = 17
        Me._lblLabel_4.Text = "Store :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_4.UseWaitCursor = True
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(8, 176)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_3.TabIndex = 16
        Me._lblLabel_3.Text = "Brand :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_3.UseWaitCursor = True
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(40, 10)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_0.TabIndex = 15
        Me._lblLabel_0.Text = "Vendor :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_0.UseWaitCursor = True
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Location = New System.Drawing.Point(8, 104)
        Me._lblLabel_7.Name = "_lblLabel_7"
        Me._lblLabel_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_7.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_7.TabIndex = 13
        Me._lblLabel_7.Text = "Vendor Item ID :"
        Me._lblLabel_7.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_7.UseWaitCursor = True
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(40, 54)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_2.TabIndex = 14
        Me._lblLabel_2.Text = "Desc :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_2.UseWaitCursor = True
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(40, 80)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_1.TabIndex = 11
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_1.UseWaitCursor = True
        '
        'txtField
        '
        '
        'cmbCategory
        '
        Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(112, 148)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(169, 22)
        Me.cmbCategory.Sorted = True
        Me.cmbCategory.TabIndex = 7
        Me.cmbCategory.UseWaitCursor = True
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.cmbSubTeam.FormattingEnabled = True
        Me.cmbSubTeam.Location = New System.Drawing.Point(112, 123)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Size = New System.Drawing.Size(169, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 6
        Me.cmbSubTeam.UseWaitCursor = True
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCategory.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCategory.Location = New System.Drawing.Point(61, 151)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(44, 14)
        Me.lblCategory.TabIndex = 53
        Me.lblCategory.Text = "Class :"
        Me.lblCategory.UseWaitCursor = True
        '
        'lblSubTeam
        '
        Me.lblSubTeam.AutoSize = True
        Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeam.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSubTeam.Location = New System.Drawing.Point(36, 126)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.Size = New System.Drawing.Size(68, 14)
        Me.lblSubTeam.TabIndex = 52
        Me.lblSubTeam.Text = "Sub-Team :"
        Me.lblSubTeam.UseWaitCursor = True
        '
        'ugrdSearchResults
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdSearchResults.DisplayLayout.Appearance = Appearance1
        Me.ugrdSearchResults.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 14
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(377, 17)
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 229
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 1
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(0, 17)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 9
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 14
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.VisiblePosition = 10
        UltraGridColumn5.Hidden = True
        UltraGridColumn5.Width = 14
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.VisiblePosition = 11
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.Width = 14
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.Caption = "Vendor Item Id"
        UltraGridColumn7.Header.VisiblePosition = 8
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn8.Header.VisiblePosition = 3
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn9.Header.Caption = "Vendor Pack"
        UltraGridColumn9.Header.VisiblePosition = 7
        UltraGridColumn10.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn10.Header.VisiblePosition = 5
        UltraGridColumn11.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn11.Header.VisiblePosition = 6
        Appearance16.TextHAlignAsString = "Center"
        UltraGridColumn12.CellAppearance = Appearance16
        UltraGridColumn12.Header.Caption = "Vendor Status"
        UltraGridColumn12.Header.VisiblePosition = 4
        UltraGridColumn12.Width = 97
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13})
        Me.ugrdSearchResults.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdSearchResults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdSearchResults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdSearchResults.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdSearchResults.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.BackColor = System.Drawing.SystemColors.HighlightText
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdSearchResults.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdSearchResults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdSearchResults.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdSearchResults.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdSearchResults.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdSearchResults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdSearchResults.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSearchResults.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdSearchResults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdSearchResults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdSearchResults.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdSearchResults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdSearchResults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdSearchResults.Location = New System.Drawing.Point(16, 231)
        Me.ugrdSearchResults.Name = "ugrdSearchResults"
        Me.ugrdSearchResults.Size = New System.Drawing.Size(967, 275)
        Me.ugrdSearchResults.TabIndex = 12
        Me.ugrdSearchResults.Text = "Search Results"
        Me.ugrdSearchResults.UseWaitCursor = True
        '
        'fraDisplay
        '
        Me.fraDisplay.BackColor = System.Drawing.SystemColors.Control
        Me.fraDisplay.Controls.Add(Me.optNonPreOrder)
        Me.fraDisplay.Controls.Add(Me.optPreOrder)
        Me.fraDisplay.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDisplay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDisplay.Location = New System.Drawing.Point(822, 109)
        Me.fraDisplay.Name = "fraDisplay"
        Me.fraDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDisplay.Size = New System.Drawing.Size(121, 62)
        Me.fraDisplay.TabIndex = 54
        Me.fraDisplay.TabStop = False
        Me.fraDisplay.UseWaitCursor = True
        '
        'optNonPreOrder
        '
        Me.optNonPreOrder.BackColor = System.Drawing.SystemColors.Control
        Me.optNonPreOrder.Checked = True
        Me.optNonPreOrder.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.optNonPreOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNonPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNonPreOrder.Location = New System.Drawing.Point(6, 39)
        Me.optNonPreOrder.Name = "optNonPreOrder"
        Me.optNonPreOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNonPreOrder.Size = New System.Drawing.Size(105, 17)
        Me.optNonPreOrder.TabIndex = 11
        Me.optNonPreOrder.TabStop = True
        Me.optNonPreOrder.Text = "Non Pre-Order"
        Me.optNonPreOrder.UseVisualStyleBackColor = False
        Me.optNonPreOrder.UseWaitCursor = True
        '
        'optPreOrder
        '
        Me.optPreOrder.BackColor = System.Drawing.SystemColors.Control
        Me.optPreOrder.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.optPreOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.Location = New System.Drawing.Point(6, 17)
        Me.optPreOrder.Name = "optPreOrder"
        Me.optPreOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPreOrder.Size = New System.Drawing.Size(105, 17)
        Me.optPreOrder.TabIndex = 10
        Me.optPreOrder.TabStop = True
        Me.optPreOrder.Text = "Pre-Order"
        Me.optPreOrder.UseVisualStyleBackColor = False
        Me.optPreOrder.UseWaitCursor = True
        '
        'chkIncludeNotAvailable
        '
        Me.chkIncludeNotAvailable.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeNotAvailable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIncludeNotAvailable.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.chkIncludeNotAvailable.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeNotAvailable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeNotAvailable.Location = New System.Drawing.Point(822, 211)
        Me.chkIncludeNotAvailable.Name = "chkIncludeNotAvailable"
        Me.chkIncludeNotAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeNotAvailable.Size = New System.Drawing.Size(145, 17)
        Me.chkIncludeNotAvailable.TabIndex = 55
        Me.chkIncludeNotAvailable.Text = "Include Not Available"
        Me.chkIncludeNotAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIncludeNotAvailable.UseVisualStyleBackColor = False
        Me.chkIncludeNotAvailable.UseWaitCursor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_Seasonal)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_VendorDiscontinued)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_MfgDiscontinued)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_NotAvailable)
        Me.GroupBox1.Location = New System.Drawing.Point(665, 109)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(142, 116)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vendor Status"
        Me.GroupBox1.UseWaitCursor = True
        '
        'chkVendorItemStatus_Seasonal
        '
        Me.chkVendorItemStatus_Seasonal.AutoSize = True
        Me.chkVendorItemStatus_Seasonal.Location = New System.Drawing.Point(6, 91)
        Me.chkVendorItemStatus_Seasonal.Name = "chkVendorItemStatus_Seasonal"
        Me.chkVendorItemStatus_Seasonal.Size = New System.Drawing.Size(108, 18)
        Me.chkVendorItemStatus_Seasonal.TabIndex = 3
        Me.chkVendorItemStatus_Seasonal.Text = "Include Seasonal"
        Me.chkVendorItemStatus_Seasonal.UseVisualStyleBackColor = True
        Me.chkVendorItemStatus_Seasonal.UseWaitCursor = True
        '
        'chkVendorItemStatus_VendorDiscontinued
        '
        Me.chkVendorItemStatus_VendorDiscontinued.AutoSize = True
        Me.chkVendorItemStatus_VendorDiscontinued.Location = New System.Drawing.Point(6, 67)
        Me.chkVendorItemStatus_VendorDiscontinued.Name = "chkVendorItemStatus_VendorDiscontinued"
        Me.chkVendorItemStatus_VendorDiscontinued.Size = New System.Drawing.Size(128, 18)
        Me.chkVendorItemStatus_VendorDiscontinued.TabIndex = 2
        Me.chkVendorItemStatus_VendorDiscontinued.Text = "Include Vendor Disco"
        Me.chkVendorItemStatus_VendorDiscontinued.UseVisualStyleBackColor = True
        Me.chkVendorItemStatus_VendorDiscontinued.UseWaitCursor = True
        '
        'chkVendorItemStatus_MfgDiscontinued
        '
        Me.chkVendorItemStatus_MfgDiscontinued.AutoSize = True
        Me.chkVendorItemStatus_MfgDiscontinued.Location = New System.Drawing.Point(6, 43)
        Me.chkVendorItemStatus_MfgDiscontinued.Name = "chkVendorItemStatus_MfgDiscontinued"
        Me.chkVendorItemStatus_MfgDiscontinued.Size = New System.Drawing.Size(111, 18)
        Me.chkVendorItemStatus_MfgDiscontinued.TabIndex = 1
        Me.chkVendorItemStatus_MfgDiscontinued.Text = "Include Mfg Disco"
        Me.chkVendorItemStatus_MfgDiscontinued.UseVisualStyleBackColor = True
        Me.chkVendorItemStatus_MfgDiscontinued.UseWaitCursor = True
        '
        'chkVendorItemStatus_NotAvailable
        '
        Me.chkVendorItemStatus_NotAvailable.AutoSize = True
        Me.chkVendorItemStatus_NotAvailable.Location = New System.Drawing.Point(6, 19)
        Me.chkVendorItemStatus_NotAvailable.Name = "chkVendorItemStatus_NotAvailable"
        Me.chkVendorItemStatus_NotAvailable.Size = New System.Drawing.Size(125, 18)
        Me.chkVendorItemStatus_NotAvailable.TabIndex = 0
        Me.chkVendorItemStatus_NotAvailable.Text = "Include Not Available"
        Me.chkVendorItemStatus_NotAvailable.UseVisualStyleBackColor = True
        Me.chkVendorItemStatus_NotAvailable.UseWaitCursor = True
        '
        'frmOrderItemSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(995, 563)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkIncludeNotAvailable)
        Me.Controls.Add(Me.fraDisplay)
        Me.Controls.Add(Me.ugrdSearchResults)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.cmbDistSubTeam)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me._txtField_5)
        Me.Controls.Add(Me.chkDiscontinued)
        Me.Controls.Add(Me._txtField_4)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(308, 184)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderItemSearch"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Order Item Search"
        Me.UseWaitCursor = True
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraDisplay.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
	Friend WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Friend WithEvents lblCategory As System.Windows.Forms.Label
	Friend WithEvents lblSubTeam As System.Windows.Forms.Label
	Friend WithEvents ugrdSearchResults As Infragistics.Win.UltraWinGrid.UltraGrid
	Public WithEvents fraDisplay As System.Windows.Forms.GroupBox
	Public WithEvents optNonPreOrder As System.Windows.Forms.RadioButton
	Public WithEvents optPreOrder As System.Windows.Forms.RadioButton
	Public WithEvents chkIncludeNotAvailable As System.Windows.Forms.CheckBox
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents chkVendorItemStatus_Seasonal As System.Windows.Forms.CheckBox
	Friend WithEvents chkVendorItemStatus_VendorDiscontinued As System.Windows.Forms.CheckBox
	Friend WithEvents chkVendorItemStatus_MfgDiscontinued As System.Windows.Forms.CheckBox
    Friend WithEvents chkVendorItemStatus_NotAvailable As System.Windows.Forms.CheckBox
#End Region 
End Class