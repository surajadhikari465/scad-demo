<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmReceivingLog
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
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents txtDateTime As System.Windows.Forms.TextBox
	Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReceivingLog))
        Dim DateButton1 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Dim DateButton2 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.txtDateTime = New System.Windows.Forms.TextBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.cmbFromDate = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        Me.cmbToDate = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbFromDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbToDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Enabled = False
        Me.cmdClose.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.Location = New System.Drawing.Point(184, 168)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(41, 41)
        Me.cmdClose.TabIndex = 6
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdClose, "Close receiving")
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(280, 168)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 8
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
        Me.cmdReport.Location = New System.Drawing.Point(232, 168)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 7
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Display Report")
        Me.cmdReport.UseVisualStyleBackColor = False
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
        Me.cmbSubTeam.Location = New System.Drawing.Point(96, 112)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(225, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 4
        '
        'txtDateTime
        '
        Me.txtDateTime.AcceptsReturn = True
        Me.txtDateTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateTime.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTime.Location = New System.Drawing.Point(96, 80)
        Me.txtDateTime.MaxLength = 0
        Me.txtDateTime.Name = "txtDateTime"
        Me.txtDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTime.Size = New System.Drawing.Size(177, 20)
        Me.txtDateTime.TabIndex = 3
        Me.txtDateTime.TabStop = False
        Me.txtDateTime.Tag = "Date"
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
        Me.cmbStore.Location = New System.Drawing.Point(96, 16)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(225, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 0
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(197, 48)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(17, 17)
        Me._lblLabel_4.TabIndex = 13
        Me._lblLabel_4.Text = "-"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(16, 112)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_3.TabIndex = 12
        Me._lblLabel_3.Text = "Sub-Team :"
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
        Me._lblLabel_2.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_2.TabIndex = 11
        Me._lblLabel_2.Text = "Closed :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(0, 48)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 10
        Me._lblLabel_0.Text = "Date :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(0, 16)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_1.TabIndex = 9
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbFromDate
        '
        Me.cmbFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.cmbFromDate.DateButtons.Add(DateButton1)
        Me.cmbFromDate.Location = New System.Drawing.Point(96, 45)
        Me.cmbFromDate.Name = "cmbFromDate"
        Me.cmbFromDate.NonAutoSizeHeight = 21
        Me.cmbFromDate.Size = New System.Drawing.Size(95, 21)
        Me.cmbFromDate.TabIndex = 14
        '
        'cmbToDate
        '
        Me.cmbToDate.BackColor = System.Drawing.SystemColors.Window
        Me.cmbToDate.DateButtons.Add(DateButton2)
        Me.cmbToDate.Enabled = False
        Me.cmbToDate.Location = New System.Drawing.Point(220, 45)
        Me.cmbToDate.Name = "cmbToDate"
        Me.cmbToDate.NonAutoSizeHeight = 21
        Me.cmbToDate.Size = New System.Drawing.Size(89, 21)
        Me.cmbToDate.TabIndex = 15
        '
        'frmReceivingLog
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(329, 219)
        Me.Controls.Add(Me.cmbToDate)
        Me.Controls.Add(Me.cmbFromDate)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.txtDateTime)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReceivingLog"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Daily Purchase Order Receiving Log"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbFromDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbToDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbFromDate As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents cmbToDate As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
#End Region 
End Class