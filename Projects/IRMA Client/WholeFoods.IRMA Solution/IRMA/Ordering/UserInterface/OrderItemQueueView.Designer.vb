<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderItemQueueView
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
	Public WithEvents cmdVendorSearch As System.Windows.Forms.Button
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtVendor As System.Windows.Forms.TextBox
	Public WithEvents cmdVendorClear As System.Windows.Forms.Button
	Public WithEvents cmbUser As System.Windows.Forms.ComboBox
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents fraOptional As System.Windows.Forms.GroupBox
	Public WithEvents cmbTransferToSubteam As System.Windows.Forms.ComboBox
	Public WithEvents cmbPurchasing As System.Windows.Forms.ComboBox
	Public WithEvents optCredit As System.Windows.Forms.RadioButton
	Public WithEvents optTransfer As System.Windows.Forms.RadioButton
	Public WithEvents optOrder As System.Windows.Forms.RadioButton
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents fraRequired As System.Windows.Forms.GroupBox
	Public WithEvents cmdReports As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdItemEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents lblRowCount As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderItemQueueView))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Order_Item_Queue_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_key")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Primary_Vendor")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("User")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Insert_Date")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit_ID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Discontinued", 0)
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
        Me.cmdVendorSearch = New System.Windows.Forms.Button()
        Me.cmdVendorClear = New System.Windows.Forms.Button()
        Me.cmdReports = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdItemEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.fraOptional = New System.Windows.Forms.GroupBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.txtVendor = New System.Windows.Forms.TextBox()
        Me.cmbUser = New System.Windows.Forms.ComboBox()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.fraRequired = New System.Windows.Forms.GroupBox()
        Me.cmbTransferToSubteam = New System.Windows.Forms.ComboBox()
        Me.cmbPurchasing = New System.Windows.Forms.ComboBox()
        Me.optCredit = New System.Windows.Forms.RadioButton()
        Me.optTransfer = New System.Windows.Forms.RadioButton()
        Me.optOrder = New System.Windows.Forms.RadioButton()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me.lblRowCount = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.ugrdOrderList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.fraOptional.SuspendLayout()
        Me.fraRequired.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdOrderList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdVendorSearch
        '
        Me.cmdVendorSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdVendorSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdVendorSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdVendorSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdVendorSearch.Image = CType(resources.GetObject("cmdVendorSearch.Image"), System.Drawing.Image)
        Me.cmdVendorSearch.Location = New System.Drawing.Point(717, 24)
        Me.cmdVendorSearch.Name = "cmdVendorSearch"
        Me.cmdVendorSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdVendorSearch.Size = New System.Drawing.Size(21, 21)
        Me.cmdVendorSearch.TabIndex = 25
        Me.cmdVendorSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdVendorSearch, "Vendor Search")
        Me.cmdVendorSearch.UseVisualStyleBackColor = False
        '
        'cmdVendorClear
        '
        Me.cmdVendorClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdVendorClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdVendorClear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdVendorClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdVendorClear.Image = CType(resources.GetObject("cmdVendorClear.Image"), System.Drawing.Image)
        Me.cmdVendorClear.Location = New System.Drawing.Point(371, 16)
        Me.cmdVendorClear.Name = "cmdVendorClear"
        Me.cmdVendorClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdVendorClear.Size = New System.Drawing.Size(21, 21)
        Me.cmdVendorClear.TabIndex = 17
        Me.cmdVendorClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdVendorClear, "Clear Search Criteria")
        Me.cmdVendorClear.UseVisualStyleBackColor = False
        '
        'cmdReports
        '
        Me.cmdReports.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReports.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReports.Image = CType(resources.GetObject("cmdReports.Image"), System.Drawing.Image)
        Me.cmdReports.Location = New System.Drawing.Point(120, 488)
        Me.cmdReports.Name = "cmdReports"
        Me.cmdReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReports.Size = New System.Drawing.Size(41, 41)
        Me.cmdReports.TabIndex = 6
        Me.cmdReports.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReports, "Order Reports")
        Me.cmdReports.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(736, 488)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 4
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdItemEdit
        '
        Me.cmdItemEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemEdit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdItemEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemEdit.Image = CType(resources.GetObject("cmdItemEdit.Image"), System.Drawing.Image)
        Me.cmdItemEdit.Location = New System.Drawing.Point(64, 488)
        Me.cmdItemEdit.Name = "cmdItemEdit"
        Me.cmdItemEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemEdit.Size = New System.Drawing.Size(41, 41)
        Me.cmdItemEdit.TabIndex = 3
        Me.cmdItemEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemEdit, "View Item")
        Me.cmdItemEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(8, 488)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(41, 41)
        Me.cmdDelete.TabIndex = 2
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Delete Item from Queue")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(736, 128)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(41, 41)
        Me.cmdSearch.TabIndex = 0
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search for items in queue")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'fraOptional
        '
        Me.fraOptional.BackColor = System.Drawing.SystemColors.Control
        Me.fraOptional.Controls.Add(Me.txtDescription)
        Me.fraOptional.Controls.Add(Me.txtIdentifier)
        Me.fraOptional.Controls.Add(Me.txtVendor)
        Me.fraOptional.Controls.Add(Me.cmdVendorClear)
        Me.fraOptional.Controls.Add(Me.cmbUser)
        Me.fraOptional.Controls.Add(Me._lblLabel_2)
        Me.fraOptional.Controls.Add(Me._lblLabel_1)
        Me.fraOptional.Controls.Add(Me._lblLabel_8)
        Me.fraOptional.Controls.Add(Me._lblLabel_0)
        Me.fraOptional.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOptional.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOptional.Location = New System.Drawing.Point(368, 8)
        Me.fraOptional.Name = "fraOptional"
        Me.fraOptional.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOptional.Size = New System.Drawing.Size(409, 116)
        Me.fraOptional.TabIndex = 15
        Me.fraOptional.TabStop = False
        Me.fraOptional.Text = "Optional"
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(88, 40)
        Me.txtDescription.MaxLength = 60
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(305, 20)
        Me.txtDescription.TabIndex = 20
        Me.txtDescription.Tag = "String"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(88, 64)
        Me.txtIdentifier.MaxLength = 13
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(305, 20)
        Me.txtIdentifier.TabIndex = 19
        Me.txtIdentifier.Tag = "String"
        '
        'txtVendor
        '
        Me.txtVendor.AcceptsReturn = True
        Me.txtVendor.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendor.Location = New System.Drawing.Point(88, 16)
        Me.txtVendor.MaxLength = 50
        Me.txtVendor.Name = "txtVendor"
        Me.txtVendor.ReadOnly = True
        Me.txtVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVendor.Size = New System.Drawing.Size(261, 20)
        Me.txtVendor.TabIndex = 18
        Me.txtVendor.Tag = "String"
        '
        'cmbUser
        '
        Me.cmbUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUser.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUser.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUser.Location = New System.Drawing.Point(88, 88)
        Me.cmbUser.Name = "cmbUser"
        Me.cmbUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbUser.Size = New System.Drawing.Size(305, 22)
        Me.cmbUser.Sorted = True
        Me.cmbUser.TabIndex = 16
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(8, 40)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 24
        Me._lblLabel_2.Text = "Description :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(8, 64)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_1.TabIndex = 23
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(8, 16)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_8.TabIndex = 22
        Me._lblLabel_8.Text = "Vendor :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(7, 90)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(75, 17)
        Me._lblLabel_0.TabIndex = 21
        Me._lblLabel_0.Text = "Created By :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraRequired
        '
        Me.fraRequired.BackColor = System.Drawing.SystemColors.Control
        Me.fraRequired.Controls.Add(Me.cmbTransferToSubteam)
        Me.fraRequired.Controls.Add(Me.cmbPurchasing)
        Me.fraRequired.Controls.Add(Me.optCredit)
        Me.fraRequired.Controls.Add(Me.optTransfer)
        Me.fraRequired.Controls.Add(Me.optOrder)
        Me.fraRequired.Controls.Add(Me._lblLabel_4)
        Me.fraRequired.Controls.Add(Me._lblLabel_3)
        Me.fraRequired.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRequired.Location = New System.Drawing.Point(8, 8)
        Me.fraRequired.Name = "fraRequired"
        Me.fraRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRequired.Size = New System.Drawing.Size(357, 116)
        Me.fraRequired.TabIndex = 7
        Me.fraRequired.TabStop = False
        Me.fraRequired.Text = "Required"
        '
        'cmbTransferToSubteam
        '
        Me.cmbTransferToSubteam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTransferToSubteam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTransferToSubteam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTransferToSubteam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTransferToSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransferToSubteam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTransferToSubteam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTransferToSubteam.Location = New System.Drawing.Point(85, 85)
        Me.cmbTransferToSubteam.Name = "cmbTransferToSubteam"
        Me.cmbTransferToSubteam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTransferToSubteam.Size = New System.Drawing.Size(260, 22)
        Me.cmbTransferToSubteam.Sorted = True
        Me.cmbTransferToSubteam.TabIndex = 12
        '
        'cmbPurchasing
        '
        Me.cmbPurchasing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbPurchasing.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPurchasing.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPurchasing.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPurchasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPurchasing.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPurchasing.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPurchasing.Location = New System.Drawing.Point(85, 53)
        Me.cmbPurchasing.Name = "cmbPurchasing"
        Me.cmbPurchasing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPurchasing.Size = New System.Drawing.Size(260, 22)
        Me.cmbPurchasing.Sorted = True
        Me.cmbPurchasing.TabIndex = 11
        '
        'optCredit
        '
        Me.optCredit.BackColor = System.Drawing.SystemColors.Control
        Me.optCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCredit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCredit.Location = New System.Drawing.Point(272, 24)
        Me.optCredit.Name = "optCredit"
        Me.optCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCredit.Size = New System.Drawing.Size(57, 23)
        Me.optCredit.TabIndex = 10
        Me.optCredit.TabStop = True
        Me.optCredit.Text = "Credit"
        Me.optCredit.UseVisualStyleBackColor = False
        '
        'optTransfer
        '
        Me.optTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.optTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.optTransfer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTransfer.Location = New System.Drawing.Point(176, 24)
        Me.optTransfer.Name = "optTransfer"
        Me.optTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optTransfer.Size = New System.Drawing.Size(72, 23)
        Me.optTransfer.TabIndex = 9
        Me.optTransfer.TabStop = True
        Me.optTransfer.Text = "Transfer"
        Me.optTransfer.UseVisualStyleBackColor = False
        '
        'optOrder
        '
        Me.optOrder.BackColor = System.Drawing.SystemColors.Control
        Me.optOrder.Checked = True
        Me.optOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.Location = New System.Drawing.Point(88, 24)
        Me.optOrder.Name = "optOrder"
        Me.optOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOrder.Size = New System.Drawing.Size(57, 23)
        Me.optOrder.TabIndex = 8
        Me.optOrder.TabStop = True
        Me.optOrder.Text = "Order"
        Me.optOrder.UseVisualStyleBackColor = False
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(0, 85)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_4.TabIndex = 14
        Me._lblLabel_4.Text = "Transfer To :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(0, 53)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_3.TabIndex = 13
        Me._lblLabel_3.Text = "Purchasing :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblRowCount
        '
        Me.lblRowCount.BackColor = System.Drawing.Color.Transparent
        Me.lblRowCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRowCount.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRowCount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRowCount.Location = New System.Drawing.Point(8, 152)
        Me.lblRowCount.Name = "lblRowCount"
        Me.lblRowCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRowCount.Size = New System.Drawing.Size(241, 17)
        Me.lblRowCount.TabIndex = 5
        Me.lblRowCount.Text = "Number of Items : 0"
        '
        'ugrdOrderList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdOrderList.DisplayLayout.Appearance = Appearance1
        Me.ugrdOrderList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.Width = 14
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 3
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(0, 17)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 107
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 2
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(205, 17)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 204
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = "Qty"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(58, 0)
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 50
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(72, 0)
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.Width = 70
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.Caption = "Primary Vendor"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.Width = 120
        UltraGridColumn8.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn8.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn8.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(56, 0)
        UltraGridColumn8.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn8.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn8.Width = 60
        UltraGridColumn9.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn9.Header.Caption = "Date"
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn9.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn10.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn10.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.DataType = GetType(Boolean)
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11})
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrderList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdOrderList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdOrderList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdOrderList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrderList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdOrderList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdOrderList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrderList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdOrderList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdOrderList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdOrderList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.BackColor = System.Drawing.SystemColors.Highlight
        Appearance7.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugrdOrderList.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.ugrdOrderList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdOrderList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdOrderList.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdOrderList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdOrderList.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdOrderList.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdOrderList.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdOrderList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdOrderList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdOrderList.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdOrderList.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdOrderList.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ExtendFirstColumn
        Me.ugrdOrderList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrderList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdOrderList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdOrderList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdOrderList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdOrderList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdOrderList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdOrderList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdOrderList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdOrderList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdOrderList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdOrderList.Location = New System.Drawing.Point(10, 175)
        Me.ugrdOrderList.Name = "ugrdOrderList"
        Me.ugrdOrderList.Size = New System.Drawing.Size(766, 308)
        Me.ugrdOrderList.TabIndex = 26
        Me.ugrdOrderList.Text = "Search Results"
        '
        'frmOrderItemQueueView
        '
        Me.AcceptButton = Me.cmdSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(785, 534)
        Me.Controls.Add(Me.ugrdOrderList)
        Me.Controls.Add(Me.cmdVendorSearch)
        Me.Controls.Add(Me.fraOptional)
        Me.Controls.Add(Me.fraRequired)
        Me.Controls.Add(Me.cmdReports)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdItemEdit)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.lblRowCount)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(18, 37)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderItemQueueView"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Order Item Queue"
        Me.fraOptional.ResumeLayout(False)
        Me.fraOptional.PerformLayout()
        Me.fraRequired.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdOrderList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdOrderList As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class