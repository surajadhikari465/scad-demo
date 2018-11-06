<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmGLUploads
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
    Public WithEvents cmdExport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents _optUpload_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optUpload_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraUpload As System.Windows.Forms.GroupBox
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optUpload As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGLUploads))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.fraUpload = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me._optUpload_1 = New System.Windows.Forms.RadioButton
        Me._optUpload_0 = New System.Windows.Forms.RadioButton
        Me.lblDates = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optUpload = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me._optDateRange_1 = New System.Windows.Forms.RadioButton
        Me._optDateRange_0 = New System.Windows.Forms.RadioButton
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.cdbFileSave = New System.Windows.Forms.SaveFileDialog
        Me.lblDash = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.grpExport = New System.Windows.Forms.GroupBox
        Me.cmdSelectDirectory = New System.Windows.Forms.Button
        Me.textOutputLocation = New System.Windows.Forms.TextBox
        Me.cbdDirectoryChoose = New System.Windows.Forms.FolderBrowserDialog
        Me.formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.dgTransactions = New System.Windows.Forms.DataGridView
        Me.grpTransactions = New System.Windows.Forms.GroupBox
        Me.optDateRange = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.fraUpload.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optUpload, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.grpExport.SuspendLayout()
        CType(Me.formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTransactions.SuspendLayout()
        CType(Me.optDateRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExport
        '
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Image = CType(resources.GetObject("cmdExport.Image"), System.Drawing.Image)
        Me.cmdExport.Location = New System.Drawing.Point(607, 21)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(40, 40)
        Me.cmdExport.TabIndex = 6
        Me.cmdExport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExport, "Export and Commit")
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(671, 229)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(40, 40)
        Me.cmdExit.TabIndex = 7
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(191, 71)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(94, 71)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Enabled = False
        Me.cmdPrint.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Image = Global.My.Resources.Resources.PrintIcon
        Me.cmdPrint.Location = New System.Drawing.Point(671, 183)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(40, 40)
        Me.cmdPrint.TabIndex = 45
        Me.cmdPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdPrint, "Print (Export) Only")
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(671, 137)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(40, 40)
        Me.cmdSearch.TabIndex = 48
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
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
        Me.cmbStore.Location = New System.Drawing.Point(85, 68)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(239, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 5
        '
        'fraUpload
        '
        Me.fraUpload.BackColor = System.Drawing.SystemColors.Control
        Me.fraUpload.Controls.Add(Me.Label1)
        Me.fraUpload.Controls.Add(Me._optUpload_1)
        Me.fraUpload.Controls.Add(Me.cmbStore)
        Me.fraUpload.Controls.Add(Me._optUpload_0)
        Me.fraUpload.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUpload.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUpload.Location = New System.Drawing.Point(12, 12)
        Me.fraUpload.Name = "fraUpload"
        Me.fraUpload.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUpload.Size = New System.Drawing.Size(347, 110)
        Me.fraUpload.TabIndex = 0
        Me.fraUpload.TabStop = False
        Me.fraUpload.Text = "Export"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 45
        Me.Label1.Text = "Source BU:"
        '
        '_optUpload_1
        '
        Me._optUpload_1.BackColor = System.Drawing.SystemColors.Control
        Me._optUpload_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optUpload_1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optUpload_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUpload.SetIndex(Me._optUpload_1, CType(1, Short))
        Me._optUpload_1.Location = New System.Drawing.Point(19, 45)
        Me._optUpload_1.Name = "_optUpload_1"
        Me._optUpload_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optUpload_1.Size = New System.Drawing.Size(92, 17)
        Me._optUpload_1.TabIndex = 2
        Me._optUpload_1.TabStop = True
        Me._optUpload_1.Text = "Transfers"
        Me._optUpload_1.UseVisualStyleBackColor = False
        '
        '_optUpload_0
        '
        Me._optUpload_0.BackColor = System.Drawing.SystemColors.Control
        Me._optUpload_0.Checked = True
        Me._optUpload_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optUpload_0.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optUpload_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUpload.SetIndex(Me._optUpload_0, CType(0, Short))
        Me._optUpload_0.Location = New System.Drawing.Point(19, 21)
        Me._optUpload_0.Name = "_optUpload_0"
        Me._optUpload_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optUpload_0.Size = New System.Drawing.Size(101, 17)
        Me._optUpload_0.TabIndex = 1
        Me._optUpload_0.TabStop = True
        Me._optUpload_0.Text = "Distribution"
        Me._optUpload_0.UseVisualStyleBackColor = False
        '
        'lblDates
        '
        Me.lblDates.AutoSize = True
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Location = New System.Drawing.Point(15, 76)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(73, 13)
        Me.lblDates.TabIndex = 10
        Me.lblDates.Text = "Date Range :"
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'optUpload
        '
        '
        '_optDateRange_1
        '
        Me._optDateRange_1.AutoSize = True
        Me.optDateRange.SetIndex(Me._optDateRange_1, CType(1, Short))
        Me._optDateRange_1.Location = New System.Drawing.Point(18, 45)
        Me._optDateRange_1.Name = "_optDateRange_1"
        Me._optDateRange_1.Size = New System.Drawing.Size(65, 17)
        Me._optDateRange_1.TabIndex = 1
        Me._optDateRange_1.Text = "Custom"
        Me._optDateRange_1.UseVisualStyleBackColor = True
        '
        '_optDateRange_0
        '
        Me._optDateRange_0.AutoSize = True
        Me._optDateRange_0.Checked = True
        Me.optDateRange.SetIndex(Me._optDateRange_0, CType(0, Short))
        Me._optDateRange_0.Location = New System.Drawing.Point(18, 22)
        Me._optDateRange_0.Name = "_optDateRange_0"
        Me._optDateRange_0.Size = New System.Drawing.Size(118, 17)
        Me._optDateRange_0.TabIndex = 0
        Me._optDateRange_0.TabStop = True
        Me._optDateRange_0.Text = "Previous Fiscal Week"
        Me._optDateRange_0.UseVisualStyleBackColor = True
        '
        'cdbFileSave
        '
        Me.cdbFileSave.DefaultExt = "CSV"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(174, 70)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 16)
        Me.lblDash.TabIndex = 39
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me._optDateRange_1)
        Me.GroupBox1.Controls.Add(Me._optDateRange_0)
        Me.GroupBox1.Controls.Add(Me.dtpStartDate)
        Me.GroupBox1.Controls.Add(Me.dtpEndDate)
        Me.GroupBox1.Controls.Add(Me.lblDash)
        Me.GroupBox1.Controls.Add(Me.lblDates)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(365, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(346, 110)
        Me.GroupBox1.TabIndex = 43
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Date Range"
        '
        'grpExport
        '
        Me.grpExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpExport.Controls.Add(Me.cmdSelectDirectory)
        Me.grpExport.Controls.Add(Me.textOutputLocation)
        Me.grpExport.Controls.Add(Me.cmdExport)
        Me.grpExport.Enabled = False
        Me.grpExport.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpExport.Location = New System.Drawing.Point(12, 504)
        Me.grpExport.Name = "grpExport"
        Me.grpExport.Size = New System.Drawing.Size(653, 73)
        Me.grpExport.TabIndex = 44
        Me.grpExport.TabStop = False
        Me.grpExport.Text = "Export Location"
        '
        'cmdSelectDirectory
        '
        Me.cmdSelectDirectory.Location = New System.Drawing.Point(495, 28)
        Me.cmdSelectDirectory.Name = "cmdSelectDirectory"
        Me.cmdSelectDirectory.Size = New System.Drawing.Size(75, 23)
        Me.cmdSelectDirectory.TabIndex = 2
        Me.cmdSelectDirectory.Text = "Browse"
        Me.cmdSelectDirectory.UseVisualStyleBackColor = True
        '
        'textOutputLocation
        '
        Me.textOutputLocation.Location = New System.Drawing.Point(15, 30)
        Me.textOutputLocation.Name = "textOutputLocation"
        Me.textOutputLocation.Size = New System.Drawing.Size(474, 22)
        Me.textOutputLocation.TabIndex = 1
        '
        'formErrorProvider
        '
        Me.formErrorProvider.ContainerControl = Me
        '
        'dgTransactions
        '
        Me.dgTransactions.AllowUserToAddRows = False
        Me.dgTransactions.AllowUserToDeleteRows = False
        Me.dgTransactions.AllowUserToOrderColumns = True
        Me.dgTransactions.AllowUserToResizeRows = False
        Me.dgTransactions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgTransactions.Location = New System.Drawing.Point(22, 18)
        Me.dgTransactions.Name = "dgTransactions"
        Me.dgTransactions.ReadOnly = True
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgTransactions.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgTransactions.ShowCellToolTips = False
        Me.dgTransactions.ShowEditingIcon = False
        Me.dgTransactions.Size = New System.Drawing.Size(605, 346)
        Me.dgTransactions.TabIndex = 46
        '
        'grpTransactions
        '
        Me.grpTransactions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTransactions.Controls.Add(Me.dgTransactions)
        Me.grpTransactions.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpTransactions.Location = New System.Drawing.Point(9, 128)
        Me.grpTransactions.Name = "grpTransactions"
        Me.grpTransactions.Size = New System.Drawing.Size(656, 370)
        Me.grpTransactions.TabIndex = 47
        Me.grpTransactions.TabStop = False
        Me.grpTransactions.Text = "Pending Transactions"
        '
        'optDateRange
        '
        '
        'frmGLUploads
        '
        Me.AcceptButton = Me.cmdExport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(724, 587)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.grpTransactions)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.fraUpload)
        Me.Controls.Add(Me.grpExport)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGLUploads"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "GL Transaction Export"
        Me.fraUpload.ResumeLayout(False)
        Me.fraUpload.PerformLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optUpload, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpExport.ResumeLayout(False)
        Me.grpExport.PerformLayout()
        CType(Me.formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgTransactions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTransactions.ResumeLayout(False)
        CType(Me.optDateRange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cdbFileSave As System.Windows.Forms.SaveFileDialog
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents _optDateRange_1 As System.Windows.Forms.RadioButton
    Friend WithEvents _optDateRange_0 As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpExport As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSelectDirectory As System.Windows.Forms.Button
    Friend WithEvents textOutputLocation As System.Windows.Forms.TextBox
    Public WithEvents cmdPrint As System.Windows.Forms.Button
    Friend WithEvents cbdDirectoryChoose As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents grpTransactions As System.Windows.Forms.GroupBox
    Friend WithEvents dgTransactions As System.Windows.Forms.DataGridView
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents optDateRange As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
#End Region
End Class