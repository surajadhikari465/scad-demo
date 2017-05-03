<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemPriceReport
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
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents chkWFMItems As System.Windows.Forms.CheckBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbCategory As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Public WithEvents lblCategory As System.Windows.Forms.Label
    Public WithEvents lblStore As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemPriceReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.chkWFMItems = New System.Windows.Forms.CheckBox
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.lblCategory = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
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
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkPrintOnly, "chkPrintOnly")
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
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
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkDiscontinued, "chkDiscontinued")
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.UseVisualStyleBackColor = False
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
        'cmbCategory
        '
        Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbCategory, "cmbCategory")
        Me.cmbCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Sorted = True
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
        'lblCategory
        '
        Me.lblCategory.BackColor = System.Drawing.Color.Transparent
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCategory, "lblCategory")
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Name = "lblCategory"
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'frmItemPriceReport
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.chkWFMItems)
        Me.Controls.Add(Me.chkDiscontinued)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblStore)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemPriceReport"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class