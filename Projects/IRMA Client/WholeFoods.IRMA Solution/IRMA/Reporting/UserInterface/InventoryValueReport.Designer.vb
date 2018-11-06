<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInventoryValueReport

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
    Public WithEvents cmdExit As System.Windows.Forms.Button
  Public WithEvents cmdReport As System.Windows.Forms.Button
  Public WithEvents cboTeam As System.Windows.Forms.ComboBox
  Public WithEvents cboBusUnit As System.Windows.Forms.ComboBox
  Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
  'Public WithEvents crwReport As AxCrystal.AxCrystalReport
  Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
  Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInventoryValueReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cboTeam = New System.Windows.Forms.ComboBox
        Me.cboBusUnit = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpReportType = New System.Windows.Forms.GroupBox
        Me.radioFullDetail = New System.Windows.Forms.RadioButton
        Me.radioSubTeam = New System.Windows.Forms.RadioButton
        Me.radioTeam = New System.Windows.Forms.RadioButton
        Me.radioBU = New System.Windows.Forms.RadioButton
        Me.txtSKUNumber = New System.Windows.Forms.TextBox
        Me.HierarchySelector1 = New HierarchySelector
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpReportType.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(283, 414)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 5
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(235, 414)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 4
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cboTeam
        '
        Me.cboTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTeam.Location = New System.Drawing.Point(94, 58)
        Me.cboTeam.Name = "cboTeam"
        Me.cboTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTeam.Size = New System.Drawing.Size(204, 22)
        Me.cboTeam.Sorted = True
        Me.cboTeam.TabIndex = 1
        '
        'cboBusUnit
        '
        Me.cboBusUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboBusUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboBusUnit.BackColor = System.Drawing.SystemColors.Window
        Me.cboBusUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBusUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBusUnit.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBusUnit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBusUnit.Location = New System.Drawing.Point(94, 30)
        Me.cboBusUnit.Name = "cboBusUnit"
        Me.cboBusUnit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBusUnit.Size = New System.Drawing.Size(204, 22)
        Me.cboBusUnit.Sorted = True
        Me.cboBusUnit.TabIndex = 0
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(16, 424)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 3
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(34, 61)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(56, 19)
        Me._lblLabel_0.TabIndex = 8
        Me._lblLabel_0.Text = "Team:"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(1, 33)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(90, 19)
        Me._lblLabel_1.TabIndex = 6
        Me._lblLabel_1.Text = "Facility/Store:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(11, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(77, 17)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Identifier:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'grpReportType
        '
        Me.grpReportType.Controls.Add(Me.radioFullDetail)
        Me.grpReportType.Controls.Add(Me.radioSubTeam)
        Me.grpReportType.Controls.Add(Me.radioTeam)
        Me.grpReportType.Controls.Add(Me.radioBU)
        Me.grpReportType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpReportType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpReportType.Location = New System.Drawing.Point(12, 12)
        Me.grpReportType.Name = "grpReportType"
        Me.grpReportType.Size = New System.Drawing.Size(312, 132)
        Me.grpReportType.TabIndex = 11
        Me.grpReportType.TabStop = False
        Me.grpReportType.Text = "Report Type"
        '
        'radioFullDetail
        '
        Me.radioFullDetail.AutoSize = True
        Me.radioFullDetail.Location = New System.Drawing.Point(24, 100)
        Me.radioFullDetail.Name = "radioFullDetail"
        Me.radioFullDetail.Size = New System.Drawing.Size(77, 18)
        Me.radioFullDetail.TabIndex = 3
        Me.radioFullDetail.Text = "Full Detail"
        Me.radioFullDetail.UseVisualStyleBackColor = True
        '
        'radioSubTeam
        '
        Me.radioSubTeam.AutoSize = True
        Me.radioSubTeam.Location = New System.Drawing.Point(24, 76)
        Me.radioSubTeam.Name = "radioSubTeam"
        Me.radioSubTeam.Size = New System.Drawing.Size(133, 18)
        Me.radioSubTeam.TabIndex = 2
        Me.radioSubTeam.Text = "SubTeam Summary"
        Me.radioSubTeam.UseVisualStyleBackColor = True
        '
        'radioTeam
        '
        Me.radioTeam.AutoSize = True
        Me.radioTeam.Location = New System.Drawing.Point(24, 52)
        Me.radioTeam.Name = "radioTeam"
        Me.radioTeam.Size = New System.Drawing.Size(112, 18)
        Me.radioTeam.TabIndex = 1
        Me.radioTeam.Text = "Team Summary"
        Me.radioTeam.UseVisualStyleBackColor = True
        '
        'radioBU
        '
        Me.radioBU.AutoSize = True
        Me.radioBU.Checked = True
        Me.radioBU.Location = New System.Drawing.Point(24, 28)
        Me.radioBU.Name = "radioBU"
        Me.radioBU.Size = New System.Drawing.Size(151, 18)
        Me.radioBU.TabIndex = 0
        Me.radioBU.TabStop = True
        Me.radioBU.Text = "Facility/Store Summary"
        Me.radioBU.UseVisualStyleBackColor = True
        '
        'txtSKUNumber
        '
        Me.txtSKUNumber.Location = New System.Drawing.Point(94, 86)
        Me.txtSKUNumber.Name = "txtSKUNumber"
        Me.txtSKUNumber.Size = New System.Drawing.Size(204, 20)
        Me.txtSKUNumber.TabIndex = 4
        '
        'HierarchySelector1
        '
        Me.HierarchySelector1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.HierarchySelector1.ItemIdentifier = Nothing
        Me.HierarchySelector1.Location = New System.Drawing.Point(10, 281)
        Me.HierarchySelector1.Name = "HierarchySelector1"
        Me.HierarchySelector1.SelectedCategoryId = 0
        Me.HierarchySelector1.SelectedLevel3Id = 0
        Me.HierarchySelector1.SelectedLevel4Id = 0
        Me.HierarchySelector1.SelectedSubTeamId = 0
        Me.HierarchySelector1.Size = New System.Drawing.Size(314, 120)
        Me.HierarchySelector1.TabIndex = 12
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboBusUnit)
        Me.GroupBox1.Controls.Add(Me._lblLabel_1)
        Me.GroupBox1.Controls.Add(Me.txtSKUNumber)
        Me.GroupBox1.Controls.Add(Me._lblLabel_0)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cboTeam)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 150)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 127)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Report Parameters"
        '
        'frmInventoryValueReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(335, 467)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.HierarchySelector1)
        Me.Controls.Add(Me.grpReportType)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(249, 252)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInventoryValueReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventory Value Report"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpReportType.ResumeLayout(False)
        Me.grpReportType.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents grpReportType As System.Windows.Forms.GroupBox
  Friend WithEvents radioFullDetail As System.Windows.Forms.RadioButton
  Friend WithEvents radioSubTeam As System.Windows.Forms.RadioButton
  Friend WithEvents radioTeam As System.Windows.Forms.RadioButton
    Friend WithEvents radioBU As System.Windows.Forms.RadioButton
    Friend WithEvents txtSKUNumber As System.Windows.Forms.TextBox
    Friend WithEvents HierarchySelector1 As HierarchySelector
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
#End Region

End Class