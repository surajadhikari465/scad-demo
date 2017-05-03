<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTGMCreate
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
	Public WithEvents cmdPrevious As System.Windows.Forms.Button
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents cmdCreate As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents _txtDate_1 As System.Windows.Forms.TextBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents _txtDate_0 As System.Windows.Forms.TextBox
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
	Public WithEvents _Frame_0 As System.Windows.Forms.GroupBox
	Public WithEvents txtSearch As System.Windows.Forms.TextBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents optCategory As System.Windows.Forms.RadioButton
	Public WithEvents optBrand As System.Windows.Forms.RadioButton
	Public WithEvents optAll As System.Windows.Forms.RadioButton
	Public WithEvents OptVendor As System.Windows.Forms.RadioButton
	Public WithEvents lblPullType As System.Windows.Forms.Label
	Public WithEvents _Frame_1 As System.Windows.Forms.GroupBox
	Public WithEvents chkHIAH As System.Windows.Forms.CheckBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _Frame_2 As System.Windows.Forms.GroupBox
	Public WithEvents lblInstructions As System.Windows.Forms.Label
	Public WithEvents Frame As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTGMCreate))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdPrevious = New System.Windows.Forms.Button()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdCreate = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me._Frame_0 = New System.Windows.Forms.GroupBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_16 = New System.Windows.Forms.Label()
        Me._txtDate_1 = New System.Windows.Forms.TextBox()
        Me._txtDate_0 = New System.Windows.Forms.TextBox()
        Me._Frame_1 = New System.Windows.Forms.GroupBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.optCategory = New System.Windows.Forms.RadioButton()
        Me.optBrand = New System.Windows.Forms.RadioButton()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.OptVendor = New System.Windows.Forms.RadioButton()
        Me.lblPullType = New System.Windows.Forms.Label()
        Me._Frame_2 = New System.Windows.Forms.GroupBox()
        Me.chkHIAH = New System.Windows.Forms.CheckBox()
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.Frame = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._Frame_0.SuspendLayout()
        Me._Frame_1.SuspendLayout()
        Me._Frame_2.SuspendLayout()
        CType(Me.Frame, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdPrevious
        '
        Me.cmdPrevious.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrevious.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrevious.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrevious.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrevious.Image = CType(resources.GetObject("cmdPrevious.Image"), System.Drawing.Image)
        Me.cmdPrevious.Location = New System.Drawing.Point(240, 160)
        Me.cmdPrevious.Name = "cmdPrevious"
        Me.cmdPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrevious.Size = New System.Drawing.Size(41, 41)
        Me.cmdPrevious.TabIndex = 10
        Me.cmdPrevious.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdPrevious, "Previous Selection")
        Me.cmdPrevious.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNext.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNext.Image = CType(resources.GetObject("cmdNext.Image"), System.Drawing.Image)
        Me.cmdNext.Location = New System.Drawing.Point(288, 160)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNext.Size = New System.Drawing.Size(41, 41)
        Me.cmdNext.TabIndex = 9
        Me.cmdNext.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdNext, "Next Selection")
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdCreate
        '
        Me.cmdCreate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCreate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreate.Image = CType(resources.GetObject("cmdCreate.Image"), System.Drawing.Image)
        Me.cmdCreate.Location = New System.Drawing.Point(288, 160)
        Me.cmdCreate.Name = "cmdCreate"
        Me.cmdCreate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreate.Size = New System.Drawing.Size(41, 41)
        Me.cmdCreate.TabIndex = 7
        Me.cmdCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCreate, "Create TGM View")
        Me.cmdCreate.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(336, 160)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 6
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(360, 88)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(25, 20)
        Me.cmdSearch.TabIndex = 17
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        Me.cmdSearch.Visible = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = ""
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(136, 62)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = ""
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(136, 40)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 39
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        '_Frame_0
        '
        Me._Frame_0.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_0.Controls.Add(Me.cmbSubTeam)
        Me._Frame_0.Controls.Add(Me._lblLabel_0)
        Me._Frame_0.Controls.Add(Me._lblLabel_5)
        Me._Frame_0.Controls.Add(Me._lblLabel_16)
        Me._Frame_0.Controls.Add(Me.dtpEndDate)
        Me._Frame_0.Controls.Add(Me.dtpStartDate)
        Me._Frame_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Frame_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame.SetIndex(Me._Frame_0, CType(0, Short))
        Me._Frame_0.Location = New System.Drawing.Point(-8, 24)
        Me._Frame_0.Name = "_Frame_0"
        Me._Frame_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_0.Size = New System.Drawing.Size(401, 121)
        Me._Frame_0.TabIndex = 8
        Me._Frame_0.TabStop = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(136, 16)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(233, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 1
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(0, 64)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(129, 17)
        Me._lblLabel_0.TabIndex = 4
        Me._lblLabel_0.Text = "Sales End Date :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(8, 16)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(121, 17)
        Me._lblLabel_5.TabIndex = 0
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Location = New System.Drawing.Point(0, 40)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(129, 17)
        Me._lblLabel_16.TabIndex = 2
        Me._lblLabel_16.Text = "Sales Start Date :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_txtDate_1
        '
        Me._txtDate_1.AcceptsReturn = True
        Me._txtDate_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtDate_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtDate_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtDate_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.SetIndex(Me._txtDate_1, CType(1, Short))
        Me._txtDate_1.Location = New System.Drawing.Point(86, 177)
        Me._txtDate_1.MaxLength = 10
        Me._txtDate_1.Name = "_txtDate_1"
        Me._txtDate_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtDate_1.Size = New System.Drawing.Size(81, 20)
        Me._txtDate_1.TabIndex = 5
        '
        '_txtDate_0
        '
        Me._txtDate_0.AcceptsReturn = True
        Me._txtDate_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtDate_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtDate_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtDate_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.SetIndex(Me._txtDate_0, CType(0, Short))
        Me._txtDate_0.Location = New System.Drawing.Point(86, 153)
        Me._txtDate_0.MaxLength = 10
        Me._txtDate_0.Name = "_txtDate_0"
        Me._txtDate_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtDate_0.Size = New System.Drawing.Size(81, 20)
        Me._txtDate_0.TabIndex = 3
        '
        '_Frame_1
        '
        Me._Frame_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_1.Controls.Add(Me.txtSearch)
        Me._Frame_1.Controls.Add(Me.cmdSearch)
        Me._Frame_1.Controls.Add(Me.optCategory)
        Me._Frame_1.Controls.Add(Me.optBrand)
        Me._Frame_1.Controls.Add(Me.optAll)
        Me._Frame_1.Controls.Add(Me.OptVendor)
        Me._Frame_1.Controls.Add(Me.lblPullType)
        Me._Frame_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Frame_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame.SetIndex(Me._Frame_1, CType(1, Short))
        Me._Frame_1.Location = New System.Drawing.Point(-8, 24)
        Me._Frame_1.Name = "_Frame_1"
        Me._Frame_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_1.Size = New System.Drawing.Size(401, 121)
        Me._Frame_1.TabIndex = 11
        Me._Frame_1.TabStop = False
        Me._Frame_1.Visible = False
        '
        'txtSearch
        '
        Me.txtSearch.AcceptsReturn = True
        Me.txtSearch.BackColor = System.Drawing.SystemColors.Window
        Me.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearch.Location = New System.Drawing.Point(168, 88)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearch.Size = New System.Drawing.Size(193, 20)
        Me.txtSearch.TabIndex = 16
        Me.txtSearch.Tag = "String"
        Me.txtSearch.Visible = False
        '
        'optCategory
        '
        Me.optCategory.BackColor = System.Drawing.SystemColors.Control
        Me.optCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCategory.Location = New System.Drawing.Point(32, 56)
        Me.optCategory.Name = "optCategory"
        Me.optCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCategory.Size = New System.Drawing.Size(113, 17)
        Me.optCategory.TabIndex = 14
        Me.optCategory.TabStop = True
        Me.optCategory.Text = "Category"
        Me.optCategory.UseVisualStyleBackColor = False
        '
        'optBrand
        '
        Me.optBrand.BackColor = System.Drawing.SystemColors.Control
        Me.optBrand.Cursor = System.Windows.Forms.Cursors.Default
        Me.optBrand.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optBrand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBrand.Location = New System.Drawing.Point(32, 40)
        Me.optBrand.Name = "optBrand"
        Me.optBrand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optBrand.Size = New System.Drawing.Size(113, 17)
        Me.optBrand.TabIndex = 13
        Me.optBrand.TabStop = True
        Me.optBrand.Text = "Brand"
        Me.optBrand.UseVisualStyleBackColor = False
        '
        'optAll
        '
        Me.optAll.BackColor = System.Drawing.SystemColors.Control
        Me.optAll.Checked = True
        Me.optAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAll.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAll.Location = New System.Drawing.Point(32, 24)
        Me.optAll.Name = "optAll"
        Me.optAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAll.Size = New System.Drawing.Size(113, 17)
        Me.optAll.TabIndex = 12
        Me.optAll.TabStop = True
        Me.optAll.Text = "All Items"
        Me.optAll.UseVisualStyleBackColor = False
        '
        'OptVendor
        '
        Me.OptVendor.BackColor = System.Drawing.SystemColors.Control
        Me.OptVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptVendor.Location = New System.Drawing.Point(32, 72)
        Me.OptVendor.Name = "OptVendor"
        Me.OptVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptVendor.Size = New System.Drawing.Size(113, 17)
        Me.OptVendor.TabIndex = 15
        Me.OptVendor.TabStop = True
        Me.OptVendor.Text = "Vendor"
        Me.OptVendor.UseVisualStyleBackColor = False
        '
        'lblPullType
        '
        Me.lblPullType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPullType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPullType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPullType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPullType.Location = New System.Drawing.Point(168, 72)
        Me.lblPullType.Name = "lblPullType"
        Me.lblPullType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPullType.Size = New System.Drawing.Size(137, 17)
        Me.lblPullType.TabIndex = 23
        '
        '_Frame_2
        '
        Me._Frame_2.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_2.Controls.Add(Me.chkHIAH)
        Me._Frame_2.Controls.Add(Me.chkDiscontinued)
        Me._Frame_2.Controls.Add(Me._lblLabel_4)
        Me._Frame_2.Controls.Add(Me._lblLabel_2)
        Me._Frame_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Frame_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame.SetIndex(Me._Frame_2, CType(2, Short))
        Me._Frame_2.Location = New System.Drawing.Point(-8, 24)
        Me._Frame_2.Name = "_Frame_2"
        Me._Frame_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_2.Size = New System.Drawing.Size(401, 121)
        Me._Frame_2.TabIndex = 18
        Me._Frame_2.TabStop = False
        Me._Frame_2.Visible = False
        '
        'chkHIAH
        '
        Me.chkHIAH.BackColor = System.Drawing.SystemColors.Control
        Me.chkHIAH.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHIAH.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHIAH.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHIAH.Location = New System.Drawing.Point(168, 40)
        Me.chkHIAH.Name = "chkHIAH"
        Me.chkHIAH.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHIAH.Size = New System.Drawing.Size(17, 17)
        Me.chkHIAH.TabIndex = 22
        Me.chkHIAH.UseVisualStyleBackColor = False
        '
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDiscontinued.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Location = New System.Drawing.Point(168, 16)
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDiscontinued.Size = New System.Drawing.Size(17, 17)
        Me.chkDiscontinued.TabIndex = 20
        Me.chkDiscontinued.Text = "Check1"
        Me.chkDiscontinued.UseVisualStyleBackColor = False
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(64, 40)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_4.TabIndex = 21
        Me._lblLabel_4.Text = "Sold At WFM:"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(16, 16)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(145, 17)
        Me._lblLabel_2.TabIndex = 19
        Me._lblLabel_2.Text = "Include Discontinued :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstructions
        '
        Me.lblInstructions.BackColor = System.Drawing.SystemColors.Control
        Me.lblInstructions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstructions.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInstructions.Location = New System.Drawing.Point(8, 8)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstructions.Size = New System.Drawing.Size(369, 17)
        Me.lblInstructions.TabIndex = 24
        '
        'frmTGMCreate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(387, 209)
        Me.Controls.Add(Me._txtDate_1)
        Me.Controls.Add(Me.cmdPrevious)
        Me.Controls.Add(Me._txtDate_0)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cmdCreate)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._Frame_0)
        Me.Controls.Add(Me._Frame_1)
        Me.Controls.Add(Me._Frame_2)
        Me.Controls.Add(Me.lblInstructions)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(95, 579)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTGMCreate"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TGM Creation Wizard"
        Me._Frame_0.ResumeLayout(False)
        Me._Frame_1.ResumeLayout(False)
        Me._Frame_1.PerformLayout()
        Me._Frame_2.ResumeLayout(False)
        CType(Me.Frame, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
#End Region 
End Class