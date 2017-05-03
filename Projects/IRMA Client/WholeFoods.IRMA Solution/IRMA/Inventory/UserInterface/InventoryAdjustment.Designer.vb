<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInventoryAdjustment
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        isInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        isinitializing = False

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
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents optSubtract As System.Windows.Forms.RadioButton
    Public WithEvents optReset As System.Windows.Forms.RadioButton
    Public WithEvents optAdd As System.Windows.Forms.RadioButton
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdItemSearch As System.Windows.Forms.Button
    Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Public WithEvents txtItemDesc As System.Windows.Forms.TextBox
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInventoryAdjustment))
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TTL Quantity")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TTL Weight")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Band 1")
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 1", 0)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Units")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Pack")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdItemSearch = New System.Windows.Forms.Button
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.optSubtract = New System.Windows.Forms.RadioButton
        Me.optReset = New System.Windows.Forms.RadioButton
        Me.optAdd = New System.Windows.Forms.RadioButton
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.txtItemDesc = New System.Windows.Forms.TextBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_6 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.cmbPack = New System.Windows.Forms.ComboBox
        Me.gridOnHand = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.groupAdjustment = New System.Windows.Forms.GroupBox
        Me.txtQuantity = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.frameUnits = New System.Windows.Forms.GroupBox
        Me.optPounds = New System.Windows.Forms.RadioButton
        Me.optUnits = New System.Windows.Forms.RadioButton
        Me.optCases = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblPackAdjustment = New System.Windows.Forms.Label
        Me.lblAdjustmentUOM = New System.Windows.Forms.Label
        Me.cmbReason = New System.Windows.Forms.ComboBox
        Me.groupResults = New System.Windows.Forms.GroupBox
        Me.txtResultUnits = New System.Windows.Forms.TextBox
        Me.lblResultUnits = New System.Windows.Forms.Label
        Me.Frame1.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridOnHand, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupAdjustment.SuspendLayout()
        CType(Me.txtQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.frameUnits.SuspendLayout()
        Me.groupResults.SuspendLayout()
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
        Me.cmdExit.Location = New System.Drawing.Point(601, 237)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 12
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
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
        Me.cmdAdd.Location = New System.Drawing.Point(553, 237)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 11
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Adjustment")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdItemSearch
        '
        Me.cmdItemSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdItemSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemSearch.Image = CType(resources.GetObject("cmdItemSearch.Image"), System.Drawing.Image)
        Me.cmdItemSearch.Location = New System.Drawing.Point(413, 56)
        Me.cmdItemSearch.Name = "cmdItemSearch"
        Me.cmdItemSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemSearch.Size = New System.Drawing.Size(20, 20)
        Me.cmdItemSearch.TabIndex = 2
        Me.cmdItemSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemSearch, "Search for Vendor")
        Me.cmdItemSearch.UseVisualStyleBackColor = False
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
        Me.cmbSubTeam.Location = New System.Drawing.Point(104, 32)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(329, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 1
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.optSubtract)
        Me.Frame1.Controls.Add(Me.optReset)
        Me.Frame1.Controls.Add(Me.optAdd)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(295, 11)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(121, 73)
        Me.Frame1.TabIndex = 21
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Adjustment Type"
        '
        'optSubtract
        '
        Me.optSubtract.BackColor = System.Drawing.SystemColors.Control
        Me.optSubtract.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSubtract.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSubtract.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSubtract.Location = New System.Drawing.Point(8, 32)
        Me.optSubtract.Name = "optSubtract"
        Me.optSubtract.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSubtract.Size = New System.Drawing.Size(97, 17)
        Me.optSubtract.TabIndex = 9
        Me.optSubtract.Text = "Subtract"
        Me.optSubtract.UseVisualStyleBackColor = False
        '
        'optReset
        '
        Me.optReset.BackColor = System.Drawing.SystemColors.Control
        Me.optReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.optReset.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReset.Location = New System.Drawing.Point(8, 47)
        Me.optReset.Name = "optReset"
        Me.optReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optReset.Size = New System.Drawing.Size(97, 17)
        Me.optReset.TabIndex = 8
        Me.optReset.Text = "Reset"
        Me.optReset.UseVisualStyleBackColor = False
        Me.optReset.Visible = False
        '
        'optAdd
        '
        Me.optAdd.BackColor = System.Drawing.SystemColors.Control
        Me.optAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAdd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAdd.Location = New System.Drawing.Point(8, 16)
        Me.optAdd.Name = "optAdd"
        Me.optAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAdd.Size = New System.Drawing.Size(105, 17)
        Me.optAdd.TabIndex = 7
        Me.optAdd.Text = "Add"
        Me.optAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.optAdd.UseVisualStyleBackColor = False
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(104, 80)
        Me.txtIdentifier.MaxLength = 18
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.ReadOnly = True
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(145, 20)
        Me.txtIdentifier.TabIndex = 16
        Me.txtIdentifier.TabStop = False
        Me.txtIdentifier.Tag = "Integer"
        '
        'txtItemDesc
        '
        Me.txtItemDesc.AcceptsReturn = True
        Me.txtItemDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtItemDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtItemDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemDesc.Location = New System.Drawing.Point(104, 56)
        Me.txtItemDesc.MaxLength = 26
        Me.txtItemDesc.Name = "txtItemDesc"
        Me.txtItemDesc.ReadOnly = True
        Me.txtItemDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtItemDesc.Size = New System.Drawing.Size(308, 20)
        Me.txtItemDesc.TabIndex = 15
        Me.txtItemDesc.TabStop = False
        Me.txtItemDesc.Tag = "String"
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(104, 8)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(329, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 0
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(8, 32)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_5.TabIndex = 27
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.AutoSize = True
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(10, 92)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(54, 14)
        Me._lblLabel_3.TabIndex = 24
        Me._lblLabel_3.Text = "Reason :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(19, 82)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(79, 14)
        Me._lblLabel_1.TabIndex = 17
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(8, 56)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 14
        Me._lblLabel_0.Text = "Item Desc :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Location = New System.Drawing.Point(8, 8)
        Me._lblLabel_6.Name = "_lblLabel_6"
        Me._lblLabel_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_6.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_6.TabIndex = 13
        Me._lblLabel_6.Text = "Business Unit :"
        Me._lblLabel_6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbPack
        '
        Me.cmbPack.Enabled = False
        Me.cmbPack.FormattingEnabled = True
        Me.cmbPack.Location = New System.Drawing.Point(70, 28)
        Me.cmbPack.Name = "cmbPack"
        Me.cmbPack.Size = New System.Drawing.Size(113, 22)
        Me.cmbPack.TabIndex = 29
        '
        'gridOnHand
        '
        Me.gridOnHand.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 74
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 89
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3})
        UltraGridBand1.Header.Caption = "Current On-hand Amounts"
        UltraGridColumn4.Header.VisiblePosition = 0
        UltraGridColumn4.Width = 74
        UltraGridColumn5.Header.VisiblePosition = 1
        UltraGridColumn5.Width = 70
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn4, UltraGridColumn5})
        UltraGridBand2.Header.Caption = "On-hand by Pack Size"
        Me.gridOnHand.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.gridOnHand.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        Me.gridOnHand.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Me.gridOnHand.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.gridOnHand.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.gridOnHand.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.gridOnHand.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridOnHand.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridOnHand.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.gridOnHand.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.gridOnHand.Enabled = False
        Me.gridOnHand.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gridOnHand.Location = New System.Drawing.Point(439, 8)
        Me.gridOnHand.Name = "gridOnHand"
        Me.gridOnHand.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.gridOnHand.Size = New System.Drawing.Size(203, 149)
        Me.gridOnHand.TabIndex = 30
        Me.gridOnHand.Text = "Current On-hand Amounts"
        '
        'groupAdjustment
        '
        Me.groupAdjustment.Controls.Add(Me.txtQuantity)
        Me.groupAdjustment.Controls.Add(Me.frameUnits)
        Me.groupAdjustment.Controls.Add(Me.Label1)
        Me.groupAdjustment.Controls.Add(Me.lblPackAdjustment)
        Me.groupAdjustment.Controls.Add(Me.lblAdjustmentUOM)
        Me.groupAdjustment.Controls.Add(Me.Frame1)
        Me.groupAdjustment.Controls.Add(Me.cmbReason)
        Me.groupAdjustment.Controls.Add(Me.cmbPack)
        Me.groupAdjustment.Controls.Add(Me._lblLabel_3)
        Me.groupAdjustment.Enabled = False
        Me.groupAdjustment.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupAdjustment.Location = New System.Drawing.Point(11, 106)
        Me.groupAdjustment.Name = "groupAdjustment"
        Me.groupAdjustment.Size = New System.Drawing.Size(422, 125)
        Me.groupAdjustment.TabIndex = 31
        Me.groupAdjustment.TabStop = False
        Me.groupAdjustment.Text = "Adjustment Detail"
        '
        'txtQuantity
        '
        Me.txtQuantity.Enabled = False
        Me.txtQuantity.Location = New System.Drawing.Point(70, 56)
        Me.txtQuantity.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtQuantity.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtQuantity.MaskInput = "{double:6.4}"
        Me.txtQuantity.MaxValue = 100000
        Me.txtQuantity.MinValue = 0
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Nullable = True
        Me.txtQuantity.NullText = "0.00"
        Me.txtQuantity.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtQuantity.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtQuantity.Size = New System.Drawing.Size(83, 21)
        Me.txtQuantity.TabIndex = 35
        '
        'frameUnits
        '
        Me.frameUnits.BackColor = System.Drawing.SystemColors.Control
        Me.frameUnits.Controls.Add(Me.optPounds)
        Me.frameUnits.Controls.Add(Me.optUnits)
        Me.frameUnits.Controls.Add(Me.optCases)
        Me.frameUnits.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frameUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameUnits.Location = New System.Drawing.Point(191, 10)
        Me.frameUnits.Name = "frameUnits"
        Me.frameUnits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameUnits.Size = New System.Drawing.Size(97, 73)
        Me.frameUnits.TabIndex = 31
        Me.frameUnits.TabStop = False
        Me.frameUnits.Text = "Quantity Type"
        '
        'optPounds
        '
        Me.optPounds.BackColor = System.Drawing.SystemColors.Control
        Me.optPounds.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPounds.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPounds.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPounds.Location = New System.Drawing.Point(8, 48)
        Me.optPounds.Name = "optPounds"
        Me.optPounds.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPounds.Size = New System.Drawing.Size(73, 17)
        Me.optPounds.TabIndex = 6
        Me.optPounds.TabStop = True
        Me.optPounds.Text = "Pounds"
        Me.optPounds.UseVisualStyleBackColor = False
        '
        'optUnits
        '
        Me.optUnits.BackColor = System.Drawing.SystemColors.Control
        Me.optUnits.Checked = True
        Me.optUnits.Cursor = System.Windows.Forms.Cursors.Default
        Me.optUnits.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUnits.Location = New System.Drawing.Point(8, 16)
        Me.optUnits.Name = "optUnits"
        Me.optUnits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optUnits.Size = New System.Drawing.Size(81, 17)
        Me.optUnits.TabIndex = 4
        Me.optUnits.TabStop = True
        Me.optUnits.Text = "Units"
        Me.optUnits.UseVisualStyleBackColor = False
        '
        'optCases
        '
        Me.optCases.BackColor = System.Drawing.SystemColors.Control
        Me.optCases.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCases.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCases.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCases.Location = New System.Drawing.Point(8, 32)
        Me.optCases.Name = "optCases"
        Me.optCases.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCases.Size = New System.Drawing.Size(73, 17)
        Me.optCases.TabIndex = 5
        Me.optCases.TabStop = True
        Me.optCases.Text = "Cases"
        Me.optCases.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Location = New System.Drawing.Point(10, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 14)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Amount:"
        '
        'lblPackAdjustment
        '
        Me.lblPackAdjustment.AutoSize = True
        Me.lblPackAdjustment.Location = New System.Drawing.Point(2, 32)
        Me.lblPackAdjustment.Name = "lblPackAdjustment"
        Me.lblPackAdjustment.Size = New System.Drawing.Size(62, 14)
        Me.lblPackAdjustment.TabIndex = 4
        Me.lblPackAdjustment.Text = "Pack Size:"
        '
        'lblAdjustmentUOM
        '
        Me.lblAdjustmentUOM.AutoSize = True
        Me.lblAdjustmentUOM.Enabled = False
        Me.lblAdjustmentUOM.Location = New System.Drawing.Point(153, 59)
        Me.lblAdjustmentUOM.Name = "lblAdjustmentUOM"
        Me.lblAdjustmentUOM.Size = New System.Drawing.Size(32, 14)
        Me.lblAdjustmentUOM.TabIndex = 3
        Me.lblAdjustmentUOM.Text = "UOM"
        Me.lblAdjustmentUOM.Visible = False
        '
        'cmbReason
        '
        Me.cmbReason.Enabled = False
        Me.cmbReason.FormattingEnabled = True
        Me.cmbReason.Location = New System.Drawing.Point(70, 89)
        Me.cmbReason.Name = "cmbReason"
        Me.cmbReason.Size = New System.Drawing.Size(218, 22)
        Me.cmbReason.TabIndex = 29
        '
        'groupResults
        '
        Me.groupResults.Controls.Add(Me.txtResultUnits)
        Me.groupResults.Controls.Add(Me.lblResultUnits)
        Me.groupResults.Enabled = False
        Me.groupResults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupResults.Location = New System.Drawing.Point(439, 162)
        Me.groupResults.Name = "groupResults"
        Me.groupResults.Size = New System.Drawing.Size(203, 69)
        Me.groupResults.TabIndex = 34
        Me.groupResults.TabStop = False
        Me.groupResults.Text = "After Adjustment"
        '
        'txtResultUnits
        '
        Me.txtResultUnits.AcceptsReturn = True
        Me.txtResultUnits.BackColor = System.Drawing.SystemColors.Control
        Me.txtResultUnits.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtResultUnits.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResultUnits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtResultUnits.Location = New System.Drawing.Point(17, 30)
        Me.txtResultUnits.MaxLength = 5
        Me.txtResultUnits.Name = "txtResultUnits"
        Me.txtResultUnits.ReadOnly = True
        Me.txtResultUnits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtResultUnits.Size = New System.Drawing.Size(62, 20)
        Me.txtResultUnits.TabIndex = 33
        Me.txtResultUnits.Tag = "Number"
        '
        'lblResultUnits
        '
        Me.lblResultUnits.AutoSize = True
        Me.lblResultUnits.Location = New System.Drawing.Point(81, 33)
        Me.lblResultUnits.Name = "lblResultUnits"
        Me.lblResultUnits.Size = New System.Drawing.Size(53, 14)
        Me.lblResultUnits.TabIndex = 35
        Me.lblResultUnits.Text = "UnitUOM"
        Me.lblResultUnits.Visible = False
        '
        'frmInventoryAdjustment
        '
        Me.AcceptButton = Me.cmdAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(649, 288)
        Me.Controls.Add(Me.groupResults)
        Me.Controls.Add(Me.groupAdjustment)
        Me.Controls.Add(Me.gridOnHand)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdItemSearch)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.txtItemDesc)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(249, 249)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInventoryAdjustment"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventory Adjustment"
        Me.Frame1.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridOnHand, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupAdjustment.ResumeLayout(False)
        Me.groupAdjustment.PerformLayout()
        CType(Me.txtQuantity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.frameUnits.ResumeLayout(False)
        Me.groupResults.ResumeLayout(False)
        Me.groupResults.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbPack As System.Windows.Forms.ComboBox
    Friend WithEvents gridOnHand As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents groupAdjustment As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblPackAdjustment As System.Windows.Forms.Label
    Friend WithEvents groupResults As System.Windows.Forms.GroupBox
    Public WithEvents txtResultUnits As System.Windows.Forms.TextBox
    Friend WithEvents lblResultUnits As System.Windows.Forms.Label
    Public WithEvents frameUnits As System.Windows.Forms.GroupBox
    Public WithEvents optPounds As System.Windows.Forms.RadioButton
    Public WithEvents optUnits As System.Windows.Forms.RadioButton
    Public WithEvents optCases As System.Windows.Forms.RadioButton
    Friend WithEvents cmbReason As System.Windows.Forms.ComboBox
    Friend WithEvents lblAdjustmentUOM As System.Windows.Forms.Label
    Friend WithEvents txtQuantity As Infragistics.Win.UltraWinEditors.UltraNumericEditor
#End Region
End Class