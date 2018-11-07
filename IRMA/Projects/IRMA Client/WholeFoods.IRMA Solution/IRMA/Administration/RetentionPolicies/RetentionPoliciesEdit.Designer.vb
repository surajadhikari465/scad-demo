<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmRetentionPolicies
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        IsInitializing = False

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
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents lblLabel As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRetentionPolicies))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboColumn = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NumericUpDownDaysToKeep = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label_IsSecureTransfer = New System.Windows.Forms.Label()
        Me.CheckBox_IncludedInDailyPurge = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CheckBox_DailyPurgeCompleted = New System.Windows.Forms.CheckBox()
        Me.txtSchema = New System.Windows.Forms.TextBox()
        Me.txtTable = New System.Windows.Forms.TextBox()
        Me.cboJobName = New System.Windows.Forms.ComboBox()
        Me.NumericUpDownTimeToStart = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDownTimeToEnd = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtLastPurgedDateTime = New System.Windows.Forms.TextBox()
        Me.lblPurgedDateTime = New System.Windows.Forms.Label()
        CType(Me.NumericUpDownDaysToKeep, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownTimeToStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownTimeToEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdApply, "cmdApply")
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Name = "cmdApply"
        Me.ToolTip1.SetToolTip(Me.cmdApply, resources.GetString("cmdApply.ToolTip"))
        Me.cmdApply.UseVisualStyleBackColor = False
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
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Name = "Label5"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Name = "Label4"
        '
        'lblLabel
        '
        Me.lblLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblLabel, "lblLabel")
        Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.Name = "lblLabel"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        'cboColumn
        '
        Me.cboColumn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboColumn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboColumn.BackColor = System.Drawing.SystemColors.Window
        Me.cboColumn.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboColumn, "cboColumn")
        Me.cboColumn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboColumn.Name = "cboColumn"
        Me.cboColumn.Sorted = True
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'NumericUpDownDaysToKeep
        '
        resources.ApplyResources(Me.NumericUpDownDaysToKeep, "NumericUpDownDaysToKeep")
        Me.NumericUpDownDaysToKeep.Maximum = New Decimal(New Integer() {3650, 0, 0, 0})
        Me.NumericUpDownDaysToKeep.Name = "NumericUpDownDaysToKeep"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Name = "Label6"
        '
        'Label_IsSecureTransfer
        '
        resources.ApplyResources(Me.Label_IsSecureTransfer, "Label_IsSecureTransfer")
        Me.Label_IsSecureTransfer.Name = "Label_IsSecureTransfer"
        '
        'CheckBox_IncludedInDailyPurge
        '
        resources.ApplyResources(Me.CheckBox_IncludedInDailyPurge, "CheckBox_IncludedInDailyPurge")
        Me.CheckBox_IncludedInDailyPurge.Name = "CheckBox_IncludedInDailyPurge"
        Me.CheckBox_IncludedInDailyPurge.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'CheckBox_DailyPurgeCompleted
        '
        resources.ApplyResources(Me.CheckBox_DailyPurgeCompleted, "CheckBox_DailyPurgeCompleted")
        Me.CheckBox_DailyPurgeCompleted.Name = "CheckBox_DailyPurgeCompleted"
        Me.CheckBox_DailyPurgeCompleted.UseVisualStyleBackColor = True
        '
        'txtSchema
        '
        Me.txtSchema.AcceptsReturn = True
        Me.txtSchema.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchema.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtSchema, "txtSchema")
        Me.txtSchema.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchema.Name = "txtSchema"
        Me.txtSchema.Tag = "String"
        '
        'txtTable
        '
        Me.txtTable.AcceptsReturn = True
        Me.txtTable.BackColor = System.Drawing.SystemColors.Window
        Me.txtTable.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtTable, "txtTable")
        Me.txtTable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTable.Name = "txtTable"
        Me.txtTable.Tag = "String"
        '
        'cboJobName
        '
        Me.cboJobName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboJobName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboJobName.BackColor = System.Drawing.SystemColors.Window
        Me.cboJobName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboJobName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboJobName, "cboJobName")
        Me.cboJobName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboJobName.Name = "cboJobName"
        Me.cboJobName.Sorted = True
        '
        'NumericUpDownTimeToStart
        '
        resources.ApplyResources(Me.NumericUpDownTimeToStart, "NumericUpDownTimeToStart")
        Me.NumericUpDownTimeToStart.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.NumericUpDownTimeToStart.Name = "NumericUpDownTimeToStart"
        '
        'NumericUpDownTimeToEnd
        '
        resources.ApplyResources(Me.NumericUpDownTimeToEnd, "NumericUpDownTimeToEnd")
        Me.NumericUpDownTimeToEnd.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.NumericUpDownTimeToEnd.Name = "NumericUpDownTimeToEnd"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Name = "Label8"
        '
        'txtLastPurgedDateTime
        '
        Me.txtLastPurgedDateTime.AcceptsReturn = True
        Me.txtLastPurgedDateTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtLastPurgedDateTime.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtLastPurgedDateTime, "txtLastPurgedDateTime")
        Me.txtLastPurgedDateTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLastPurgedDateTime.Name = "txtLastPurgedDateTime"
        Me.txtLastPurgedDateTime.Tag = "String"
        '
        'lblPurgedDateTime
        '
        Me.lblPurgedDateTime.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurgedDateTime.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPurgedDateTime, "lblPurgedDateTime")
        Me.lblPurgedDateTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurgedDateTime.Name = "lblPurgedDateTime"
        '
        'frmRetentionPolicies
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.txtLastPurgedDateTime)
        Me.Controls.Add(Me.lblPurgedDateTime)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.NumericUpDownTimeToEnd)
        Me.Controls.Add(Me.NumericUpDownTimeToStart)
        Me.Controls.Add(Me.cboJobName)
        Me.Controls.Add(Me.txtTable)
        Me.Controls.Add(Me.txtSchema)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.CheckBox_DailyPurgeCompleted)
        Me.Controls.Add(Me.Label_IsSecureTransfer)
        Me.Controls.Add(Me.CheckBox_IncludedInDailyPurge)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.NumericUpDownDaysToKeep)
        Me.Controls.Add(Me.cboColumn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRetentionPolicies"
        Me.ShowInTaskbar = False
        CType(Me.NumericUpDownDaysToKeep, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownTimeToStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownTimeToEnd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents cboColumn As ComboBox
    Public WithEvents Label2 As Label
    Friend WithEvents NumericUpDownDaysToKeep As NumericUpDown
    Public WithEvents Label6 As Label
    Friend WithEvents Label_IsSecureTransfer As Label
    Friend WithEvents CheckBox_IncludedInDailyPurge As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents CheckBox_DailyPurgeCompleted As CheckBox
    Public WithEvents txtSchema As TextBox
    Public WithEvents txtTable As TextBox
    Public WithEvents cboJobName As ComboBox
    Friend WithEvents NumericUpDownTimeToStart As NumericUpDown
    Friend WithEvents NumericUpDownTimeToEnd As NumericUpDown
    Public WithEvents Label8 As Label
    Public WithEvents txtLastPurgedDateTime As TextBox
    Public WithEvents lblPurgedDateTime As Label
#End Region
End Class