<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderAdd
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
	Public WithEvents cmbProductType As System.Windows.Forms.ComboBox
	Public WithEvents optTransfer As System.Windows.Forms.RadioButton
	Public WithEvents optDistribution As System.Windows.Forms.RadioButton
	Public WithEvents optPurchase As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.Panel
    Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_3 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_2 As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.cmbProductType = New System.Windows.Forms.ComboBox()
        Me.Frame1 = New System.Windows.Forms.Panel()
        Me.optFlowthru = New System.Windows.Forms.RadioButton()
        Me.optTransfer = New System.Windows.Forms.RadioButton()
        Me.optDistribution = New System.Windows.Forms.RadioButton()
        Me.optPurchase = New System.Windows.Forms.RadioButton()
        Me._cmbField_1 = New System.Windows.Forms.ComboBox()
        Me._cmbField_3 = New System.Windows.Forms.ComboBox()
        Me._cmbField_2 = New System.Windows.Forms.ComboBox()
        Me._cmbField_0 = New System.Windows.Forms.ComboBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me._lblLabel_7 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_6 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me._cmbField_4 = New System.Windows.Forms.ComboBox()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._lblLabel_10 = New System.Windows.Forms.Label()
        Me.Frame1.SuspendLayout()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(393, 230)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 10
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Cancel Add")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(345, 230)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 9
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Order")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = ""
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(113, 230)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 35
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'cmbProductType
        '
        Me.cmbProductType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbProductType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbProductType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbProductType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProductType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProductType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbProductType.Location = New System.Drawing.Point(113, 62)
        Me.cmbProductType.Name = "cmbProductType"
        Me.cmbProductType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbProductType.Size = New System.Drawing.Size(193, 22)
        Me.cmbProductType.TabIndex = 3
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.optFlowthru)
        Me.Frame1.Controls.Add(Me.optTransfer)
        Me.Frame1.Controls.Add(Me.optDistribution)
        Me.Frame1.Controls.Add(Me.optPurchase)
        Me.Frame1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(113, 33)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(321, 25)
        Me.Frame1.TabIndex = 19
        '
        'optFlowthru
        '
        Me.optFlowthru.AutoSize = True
        Me.optFlowthru.BackColor = System.Drawing.SystemColors.Control
        Me.optFlowthru.Cursor = System.Windows.Forms.Cursors.Default
        Me.optFlowthru.Enabled = False
        Me.optFlowthru.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optFlowthru.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optFlowthru.Location = New System.Drawing.Point(246, 4)
        Me.optFlowthru.Name = "optFlowthru"
        Me.optFlowthru.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optFlowthru.Size = New System.Drawing.Size(68, 18)
        Me.optFlowthru.TabIndex = 4
        Me.optFlowthru.TabStop = True
        Me.optFlowthru.Text = "Flowthru"
        Me.optFlowthru.UseVisualStyleBackColor = False
        '
        'optTransfer
        '
        Me.optTransfer.AutoSize = True
        Me.optTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.optTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.optTransfer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTransfer.Location = New System.Drawing.Point(172, 4)
        Me.optTransfer.Name = "optTransfer"
        Me.optTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optTransfer.Size = New System.Drawing.Size(67, 18)
        Me.optTransfer.TabIndex = 2
        Me.optTransfer.TabStop = True
        Me.optTransfer.Text = "Transfer"
        Me.optTransfer.UseVisualStyleBackColor = False
        '
        'optDistribution
        '
        Me.optDistribution.AutoSize = True
        Me.optDistribution.BackColor = System.Drawing.SystemColors.Control
        Me.optDistribution.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDistribution.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDistribution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDistribution.Location = New System.Drawing.Point(85, 4)
        Me.optDistribution.Name = "optDistribution"
        Me.optDistribution.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDistribution.Size = New System.Drawing.Size(78, 18)
        Me.optDistribution.TabIndex = 1
        Me.optDistribution.TabStop = True
        Me.optDistribution.Text = "Distribution"
        Me.optDistribution.UseVisualStyleBackColor = False
        '
        'optPurchase
        '
        Me.optPurchase.AutoSize = True
        Me.optPurchase.BackColor = System.Drawing.SystemColors.Control
        Me.optPurchase.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPurchase.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPurchase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPurchase.Location = New System.Drawing.Point(9, 4)
        Me.optPurchase.Name = "optPurchase"
        Me.optPurchase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPurchase.Size = New System.Drawing.Size(71, 18)
        Me.optPurchase.TabIndex = 0
        Me.optPurchase.TabStop = True
        Me.optPurchase.Text = "Purchase"
        Me.optPurchase.UseVisualStyleBackColor = False
        '
        '_cmbField_1
        '
        Me._cmbField_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(113, 90)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(321, 22)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 4
        '
        '_cmbField_3
        '
        Me._cmbField_3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_3.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_3, CType(3, Short))
        Me._cmbField_3.Location = New System.Drawing.Point(113, 174)
        Me._cmbField_3.Name = "_cmbField_3"
        Me._cmbField_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_3.Size = New System.Drawing.Size(193, 22)
        Me._cmbField_3.Sorted = True
        Me._cmbField_3.TabIndex = 7
        '
        '_cmbField_2
        '
        Me._cmbField_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_2.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_2, CType(2, Short))
        Me._cmbField_2.Location = New System.Drawing.Point(113, 146)
        Me._cmbField_2.Name = "_cmbField_2"
        Me._cmbField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_2.Size = New System.Drawing.Size(193, 22)
        Me._cmbField_2.Sorted = True
        Me._cmbField_2.TabIndex = 6
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Location = New System.Drawing.Point(113, 118)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(321, 22)
        Me._cmbField_0.Sorted = True
        Me._cmbField_0.TabIndex = 5
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(113, 10)
        Me._txtField_1.MaxLength = 0
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.ReadOnly = True
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(321, 20)
        Me._txtField_1.TabIndex = 12
        Me._txtField_1.TabStop = False
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(10, 65)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_4.TabIndex = 20
        Me._lblLabel_4.Text = "Product Type :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(18, 39)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_3.TabIndex = 18
        Me._lblLabel_3.Text = "Order Type :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(26, 233)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_8.TabIndex = 17
        Me._lblLabel_8.Text = "Expected :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Location = New System.Drawing.Point(18, 93)
        Me._lblLabel_7.Name = "_lblLabel_7"
        Me._lblLabel_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_7.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_7.TabIndex = 16
        Me._lblLabel_7.Text = "Purchasing :"
        Me._lblLabel_7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(18, 177)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_2.TabIndex = 15
        Me._lblLabel_2.Text = "Transfer To :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(8, 149)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(99, 17)
        Me._lblLabel_0.TabIndex = 13
        Me._lblLabel_0.Text = "Transfer From :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Location = New System.Drawing.Point(18, 121)
        Me._lblLabel_6.Name = "_lblLabel_6"
        Me._lblLabel_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_6.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_6.TabIndex = 14
        Me._lblLabel_6.Text = "Ship To :"
        Me._lblLabel_6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(18, 13)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_1.TabIndex = 11
        Me._lblLabel_1.Text = "Vendor :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        '_cmbField_4
        '
        Me._cmbField_4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_4.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_4, CType(4, Short))
        Me._cmbField_4.Location = New System.Drawing.Point(113, 202)
        Me._cmbField_4.Name = "_cmbField_4"
        Me._cmbField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_4.Size = New System.Drawing.Size(193, 22)
        Me._cmbField_4.Sorted = True
        Me._cmbField_4.TabIndex = 36
        Me._cmbField_4.Visible = False
        '
        'txtField
        '
        '
        '_lblLabel_10
        '
        Me._lblLabel_10.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_10.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_10.Location = New System.Drawing.Point(18, 205)
        Me._lblLabel_10.Name = "_lblLabel_10"
        Me._lblLabel_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_10.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_10.TabIndex = 37
        Me._lblLabel_10.Text = "Transfer To :"
        Me._lblLabel_10.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._lblLabel_10.Visible = False
        '
        'frmOrderAdd
        '
        Me.AcceptButton = Me.cmdAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(456, 279)
        Me.Controls.Add(Me._cmbField_4)
        Me.Controls.Add(Me._lblLabel_10)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.cmbProductType)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me._cmbField_3)
        Me.Controls.Add(Me._cmbField_2)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_8)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(267, 358)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderAdd"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add New Order"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents optFlowthru As System.Windows.Forms.RadioButton
    Public WithEvents _cmbField_4 As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_10 As System.Windows.Forms.Label
#End Region
End Class