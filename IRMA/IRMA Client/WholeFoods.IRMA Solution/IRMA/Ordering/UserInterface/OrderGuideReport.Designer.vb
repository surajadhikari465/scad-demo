<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderGuideReport
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents chkPreOrder As System.Windows.Forms.CheckBox
	Public WithEvents chkWFMItems As System.Windows.Forms.CheckBox
	Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cboWarehouse As System.Windows.Forms.ComboBox
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderGuideReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.chkPreOrder = New System.Windows.Forms.CheckBox
        Me.chkWFMItems = New System.Windows.Forms.CheckBox
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me.cboWarehouse = New System.Windows.Forms.ComboBox
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(232, 112)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 7
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(184, 112)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 6
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'chkPreOrder
        '
        Me.chkPreOrder.BackColor = System.Drawing.SystemColors.Control
        Me.chkPreOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPreOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPreOrder.Location = New System.Drawing.Point(104, 104)
        Me.chkPreOrder.Name = "chkPreOrder"
        Me.chkPreOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPreOrder.Size = New System.Drawing.Size(25, 17)
        Me.chkPreOrder.TabIndex = 4
        Me.chkPreOrder.UseVisualStyleBackColor = False
        '
        'chkWFMItems
        '
        Me.chkWFMItems.BackColor = System.Drawing.SystemColors.Control
        Me.chkWFMItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWFMItems.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWFMItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWFMItems.Location = New System.Drawing.Point(104, 80)
        Me.chkWFMItems.Name = "chkWFMItems"
        Me.chkWFMItems.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWFMItems.Size = New System.Drawing.Size(25, 17)
        Me.chkWFMItems.TabIndex = 3
        Me.chkWFMItems.UseVisualStyleBackColor = False
        '
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Location = New System.Drawing.Point(104, 56)
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cboSubTeam.Sorted = True
        Me.cboSubTeam.TabIndex = 2
        '
        'cboWarehouse
        '
        Me.cboWarehouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboWarehouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboWarehouse.BackColor = System.Drawing.SystemColors.Window
        Me.cboWarehouse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWarehouse.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWarehouse.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboWarehouse.Location = New System.Drawing.Point(104, 32)
        Me.cboWarehouse.Name = "cboWarehouse"
        Me.cboWarehouse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWarehouse.Size = New System.Drawing.Size(177, 22)
        Me.cboWarehouse.Sorted = True
        Me.cboWarehouse.TabIndex = 1
        '
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Location = New System.Drawing.Point(104, 8)
        Me.cboStore.Name = "cboStore"
        Me.cboStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStore.Size = New System.Drawing.Size(177, 22)
        Me.cboStore.Sorted = True
        Me.cboStore.TabIndex = 0
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(56, 136)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 5
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(0, 104)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_3.TabIndex = 12
        Me._lblLabel_3.Text = "PreOrder :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(0, 80)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_2.TabIndex = 11
        Me._lblLabel_2.Text = "WFM Items :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(0, 32)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_0.TabIndex = 10
        Me._lblLabel_0.Text = "Warehouse :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(0, 56)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_5.TabIndex = 9
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(0, 8)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_1.TabIndex = 8
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmOrderGuideReport
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(300, 169)
        Me.Controls.Add(Me.chkPreOrder)
        Me.Controls.Add(Me.chkWFMItems)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cboWarehouse)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(226, 580)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderGuideReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Order Guide Report"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class