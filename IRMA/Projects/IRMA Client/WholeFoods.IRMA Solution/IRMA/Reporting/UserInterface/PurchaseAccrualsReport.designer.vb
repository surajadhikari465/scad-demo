<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PurchaseAccrualsReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PurchaseAccrualsReport))
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.dtpAsOfDate = New System.Windows.Forms.DateTimePicker
        Me.SuspendLayout()
        '
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Location = New System.Drawing.Point(92, 22)
        Me.cboStore.Name = "cboStore"
        Me.cboStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStore.Size = New System.Drawing.Size(177, 22)
        Me.cboStore.Sorted = True
        Me.cboStore.TabIndex = 6
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.AutoSize = True
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(48, 25)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(40, 14)
        Me._lblLabel_1.TabIndex = 5
        Me._lblLabel_1.Text = "Store:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.cboSubTeam.Location = New System.Drawing.Point(92, 57)
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cboSubTeam.TabIndex = 10
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(222, 130)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 12
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(174, 130)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 11
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.AutoSize = True
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_5.Location = New System.Drawing.Point(22, 60)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(66, 14)
        Me._lblLabel_5.TabIndex = 13
        Me._lblLabel_5.Text = "Sub-Team:"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(21, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(67, 14)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "As Of Date:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(62, 153)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 17
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'dtpAsOfDate
        '
        Me.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpAsOfDate.Location = New System.Drawing.Point(92, 92)
        Me.dtpAsOfDate.Name = "dtpAsOfDate"
        Me.dtpAsOfDate.Size = New System.Drawing.Size(177, 20)
        Me.dtpAsOfDate.TabIndex = 19
        '
        'PurchaseAccrualsReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(299, 190)
        Me.Controls.Add(Me.dtpAsOfDate)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me._lblLabel_1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PurchaseAccrualsReport"
        Me.ShowInTaskbar = False
        Me.Text = "Purchase Accruals Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cboStore As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Friend WithEvents dtpAsOfDate As System.Windows.Forms.DateTimePicker
End Class
