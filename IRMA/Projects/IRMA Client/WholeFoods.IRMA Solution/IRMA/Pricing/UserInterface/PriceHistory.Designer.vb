<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPriceHistory
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
	Public WithEvents chkExcludeNew As System.Windows.Forms.CheckBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents _chkInclude_2 As System.Windows.Forms.CheckBox
	Public WithEvents _chkInclude_1 As System.Windows.Forms.CheckBox
	Public WithEvents _chkInclude_0 As System.Windows.Forms.CheckBox
    Public WithEvents lblPriceChanges As System.Windows.Forms.Label
    Public WithEvents lblPromotionChanges As System.Windows.Forms.Label
    Public WithEvents lblItemChanges As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents chkWFMItems As System.Windows.Forms.CheckBox
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents lblStore As System.Windows.Forms.Label
    Public WithEvents chkInclude As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPriceHistory))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.chkExcludeNew = New System.Windows.Forms.CheckBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._chkInclude_2 = New System.Windows.Forms.CheckBox
        Me._chkInclude_1 = New System.Windows.Forms.CheckBox
        Me._chkInclude_0 = New System.Windows.Forms.CheckBox
        Me.lblPriceChanges = New System.Windows.Forms.Label
        Me.lblPromotionChanges = New System.Windows.Forms.Label
        Me.lblItemChanges = New System.Windows.Forms.Label
        Me.chkWFMItems = New System.Windows.Forms.CheckBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.lblDash = New System.Windows.Forms.Label
        Me.lblDates = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
        Me.chkInclude = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.Frame1.SuspendLayout()
        CType(Me.chkInclude, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'chkExcludeNew
        '
        Me.chkExcludeNew.BackColor = System.Drawing.SystemColors.Control
        Me.chkExcludeNew.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkExcludeNew, "chkExcludeNew")
        Me.chkExcludeNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkExcludeNew.Name = "chkExcludeNew"
        Me.chkExcludeNew.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkPrintOnly, "chkPrintOnly")
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._chkInclude_2)
        Me.Frame1.Controls.Add(Me._chkInclude_1)
        Me.Frame1.Controls.Add(Me._chkInclude_0)
        Me.Frame1.Controls.Add(Me.lblPriceChanges)
        Me.Frame1.Controls.Add(Me.lblPromotionChanges)
        Me.Frame1.Controls.Add(Me.lblItemChanges)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        '_chkInclude_2
        '
        Me._chkInclude_2.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkInclude_2, "_chkInclude_2")
        Me._chkInclude_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkInclude_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInclude.SetIndex(Me._chkInclude_2, CType(2, Short))
        Me._chkInclude_2.Name = "_chkInclude_2"
        Me._chkInclude_2.UseVisualStyleBackColor = False
        '
        '_chkInclude_1
        '
        Me._chkInclude_1.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkInclude_1, "_chkInclude_1")
        Me._chkInclude_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkInclude_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInclude.SetIndex(Me._chkInclude_1, CType(1, Short))
        Me._chkInclude_1.Name = "_chkInclude_1"
        Me._chkInclude_1.UseVisualStyleBackColor = False
        '
        '_chkInclude_0
        '
        Me._chkInclude_0.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me._chkInclude_0, "_chkInclude_0")
        Me._chkInclude_0.Checked = True
        Me._chkInclude_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkInclude_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkInclude_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInclude.SetIndex(Me._chkInclude_0, CType(0, Short))
        Me._chkInclude_0.Name = "_chkInclude_0"
        Me._chkInclude_0.UseVisualStyleBackColor = False
        '
        'lblPriceChanges
        '
        Me.lblPriceChanges.BackColor = System.Drawing.Color.Transparent
        Me.lblPriceChanges.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPriceChanges, "lblPriceChanges")
        Me.lblPriceChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPriceChanges.Name = "lblPriceChanges"
        '
        'lblPromotionChanges
        '
        Me.lblPromotionChanges.BackColor = System.Drawing.Color.Transparent
        Me.lblPromotionChanges.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPromotionChanges, "lblPromotionChanges")
        Me.lblPromotionChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPromotionChanges.Name = "lblPromotionChanges"
        '
        'lblItemChanges
        '
        Me.lblItemChanges.BackColor = System.Drawing.Color.Transparent
        Me.lblItemChanges.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblItemChanges, "lblItemChanges")
        Me.lblItemChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItemChanges.Name = "lblItemChanges"
        '
        'chkWFMItems
        '
        Me.chkWFMItems.BackColor = System.Drawing.SystemColors.Control
        Me.chkWFMItems.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkWFMItems, "chkWFMItems")
        Me.chkWFMItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWFMItems.Name = "chkWFMItems"
        Me.chkWFMItems.UseVisualStyleBackColor = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Sorted = True
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStore, "cmbStore")
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.Sorted = True
        '
        'lblSubTeam
        '
        Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSubTeam, "lblSubTeam")
        Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.Name = "lblSubTeam"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDates, "lblDates")
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Name = "lblDates"
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'chkInclude
        '
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(1950, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.MaxDate = New Date(2999, 12, 31, 0, 0, 0, 0)
        Me.dtpStartDate.MinDate = New Date(1950, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Value = Nothing
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(1950, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.MaxDate = New Date(2999, 12, 31, 0, 0, 0, 0)
        Me.dtpEndDate.MinDate = New Date(1950, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Value = Nothing
        '
        'frmPriceHistory
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.chkExcludeNew)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.chkWFMItems)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.lblDates)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me.lblDash)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPriceHistory"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        CType(Me.chkInclude, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
#End Region
End Class