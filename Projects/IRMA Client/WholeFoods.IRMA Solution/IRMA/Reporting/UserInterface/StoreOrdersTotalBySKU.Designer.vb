<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmStoreOrdersTotBySKUReport

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
  Public WithEvents cboWarehouse As System.Windows.Forms.ComboBox
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStoreOrdersTotBySKUReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cboTeam = New System.Windows.Forms.ComboBox
        Me.cboWarehouse = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me._labelDateFrom = New System.Windows.Forms.Label
        Me._labelDateTo = New System.Windows.Forms.Label
        Me._dateFrom = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me._dateTo = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me._formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.HierarchySelector1 = New HierarchySelector
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._dateFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._dateTo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.cmdExit.Location = New System.Drawing.Point(368, 256)
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
        Me.cmdReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(320, 256)
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
        Me.cboTeam.Location = New System.Drawing.Point(104, 48)
        Me.cboTeam.Name = "cboTeam"
        Me.cboTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTeam.Size = New System.Drawing.Size(305, 22)
        Me.cboTeam.Sorted = True
        Me.cboTeam.TabIndex = 1
        '
        'cboWarehouse
        '
        Me.cboWarehouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboWarehouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboWarehouse.BackColor = System.Drawing.SystemColors.Window
        Me.cboWarehouse.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWarehouse.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWarehouse.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboWarehouse.Location = New System.Drawing.Point(104, 20)
        Me.cboWarehouse.Name = "cboWarehouse"
        Me.cboWarehouse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWarehouse.Size = New System.Drawing.Size(305, 22)
        Me.cboWarehouse.Sorted = True
        Me.cboWarehouse.TabIndex = 0
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(312, 231)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.Yes
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
        Me._lblLabel_0.Location = New System.Drawing.Point(6, 51)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(91, 17)
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
        Me._lblLabel_1.Location = New System.Drawing.Point(3, 23)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(94, 17)
        Me._lblLabel_1.TabIndex = 6
        Me._lblLabel_1.Text = "Warehouse:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_labelDateFrom
        '
        Me._labelDateFrom.BackColor = System.Drawing.Color.Transparent
        Me._labelDateFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me._labelDateFrom.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelDateFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me._labelDateFrom.Location = New System.Drawing.Point(-3, 225)
        Me._labelDateFrom.Name = "_labelDateFrom"
        Me._labelDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._labelDateFrom.Size = New System.Drawing.Size(91, 17)
        Me._labelDateFrom.TabIndex = 11
        Me._labelDateFrom.Text = "Date From:"
        Me._labelDateFrom.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_labelDateTo
        '
        Me._labelDateTo.BackColor = System.Drawing.Color.Transparent
        Me._labelDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me._labelDateTo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me._labelDateTo.Location = New System.Drawing.Point(-3, 251)
        Me._labelDateTo.Name = "_labelDateTo"
        Me._labelDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._labelDateTo.Size = New System.Drawing.Size(91, 17)
        Me._labelDateTo.TabIndex = 12
        Me._labelDateTo.Text = "Date To:"
        Me._labelDateTo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_dateFrom
        '
        Me._dateFrom.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me._dateFrom.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._dateFrom.Location = New System.Drawing.Point(94, 221)
        Me._dateFrom.MaskInput = ""
        Me._dateFrom.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me._dateFrom.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me._dateFrom.Name = "_dateFrom"
        Me._dateFrom.Size = New System.Drawing.Size(119, 21)
        Me._dateFrom.TabIndex = 111
        Me._dateFrom.Value = Nothing
        '
        '_dateTo
        '
        Me._dateTo.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me._dateTo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._dateTo.Location = New System.Drawing.Point(94, 247)
        Me._dateTo.MaskInput = ""
        Me._dateTo.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me._dateTo.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me._dateTo.Name = "_dateTo"
        Me._dateTo.Size = New System.Drawing.Size(119, 21)
        Me._dateTo.TabIndex = 112
        Me._dateTo.Value = Nothing
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        'HierarchySelector1
        '
        Me.HierarchySelector1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.HierarchySelector1.ItemIdentifier = Nothing
        Me.HierarchySelector1.Location = New System.Drawing.Point(12, 88)
        Me.HierarchySelector1.Name = "HierarchySelector1"
        Me.HierarchySelector1.SelectedCategoryId = 0
        Me.HierarchySelector1.SelectedLevel3Id = 0
        Me.HierarchySelector1.SelectedLevel4Id = 0
        Me.HierarchySelector1.SelectedSubTeamId = 0
        Me.HierarchySelector1.Size = New System.Drawing.Size(312, 120)
        Me.HierarchySelector1.TabIndex = 113
        '
        'frmStoreOrdersTotBySKUReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(425, 308)
        Me.Controls.Add(Me.HierarchySelector1)
        Me.Controls.Add(Me._dateTo)
        Me.Controls.Add(Me._dateFrom)
        Me.Controls.Add(Me._labelDateTo)
        Me.Controls.Add(Me._labelDateFrom)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cboTeam)
        Me.Controls.Add(Me.cboWarehouse)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(249, 252)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStoreOrdersTotBySKUReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Store Orders Total by SKU report"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._dateFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._dateTo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents _labelDateFrom As System.Windows.Forms.Label
    Public WithEvents _labelDateTo As System.Windows.Forms.Label
    Friend WithEvents _dateFrom As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents _dateTo As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents HierarchySelector1 As HierarchySelector
#End Region

End Class