<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShortReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShortReport))
        Dim DateButton1 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Dim DateButton2 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cboWarehouse = New System.Windows.Forms.ComboBox
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmbToDate = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        Me.cmbFromDate = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        CType(Me.cmbToDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbFromDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Location = New System.Drawing.Point(103, 40)
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cboSubTeam.TabIndex = 9
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
        Me.cmdExit.Location = New System.Drawing.Point(241, 103)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 12
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
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
        Me.cmdReport.Location = New System.Drawing.Point(193, 104)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 11
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdReport.UseVisualStyleBackColor = False
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
        Me.cboWarehouse.Location = New System.Drawing.Point(103, 12)
        Me.cboWarehouse.Name = "cboWarehouse"
        Me.cboWarehouse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWarehouse.Size = New System.Drawing.Size(177, 22)
        Me.cboWarehouse.Sorted = True
        Me.cboWarehouse.TabIndex = 8
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_5.Location = New System.Drawing.Point(2, 45)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(91, 17)
        Me._lblLabel_5.TabIndex = 14
        Me._lblLabel_5.Text = "SubTeam:"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(2, 15)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(94, 17)
        Me._lblLabel_1.TabIndex = 13
        Me._lblLabel_1.Text = "Warehouse:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(70, 126)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 10
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmbToDate
        '
        Me.cmbToDate.BackColor = System.Drawing.SystemColors.Window
        Me.cmbToDate.DateButtons.Add(DateButton1)
        Me.cmbToDate.Location = New System.Drawing.Point(193, 68)
        Me.cmbToDate.Name = "cmbToDate"
        Me.cmbToDate.NonAutoSizeHeight = 21
        Me.cmbToDate.Size = New System.Drawing.Size(89, 21)
        Me.cmbToDate.TabIndex = 19
        '
        'cmbFromDate
        '
        Me.cmbFromDate.BackColor = System.Drawing.SystemColors.Window
        Me.cmbFromDate.DateButtons.Add(DateButton2)
        Me.cmbFromDate.Location = New System.Drawing.Point(69, 68)
        Me.cmbFromDate.Name = "cmbFromDate"
        Me.cmbFromDate.NonAutoSizeHeight = 21
        Me.cmbFromDate.Size = New System.Drawing.Size(95, 21)
        Me.cmbFromDate.TabIndex = 18
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_4.Location = New System.Drawing.Point(170, 71)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(17, 17)
        Me._lblLabel_4.TabIndex = 17
        Me._lblLabel_4.Text = "-"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_0.Location = New System.Drawing.Point(-27, 71)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 16
        Me._lblLabel_0.Text = "Date :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ShortReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(309, 157)
        Me.Controls.Add(Me.cmbToDate)
        Me.Controls.Add(Me.cmbFromDate)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cboWarehouse)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShortReport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "ShortReport"
        CType(Me.cmbToDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbFromDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cboWarehouse As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Friend WithEvents cmbToDate As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents cmbFromDate As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
End Class
