<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class COOLShipping
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(COOLShipping))
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.lblControlGroup = New System.Windows.Forms.Label
        Me.lblDash = New System.Windows.Forms.Label
        Me._lblLabel_16 = New System.Windows.Forms.Label
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkDateRange = New System.Windows.Forms.CheckBox
        Me.txtSubTeam_No = New System.Windows.Forms.TextBox
        Me.txtStore_No = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkCOOL = New System.Windows.Forms.CheckBox
        Me.chkBIO = New System.Windows.Forms.CheckBox
        Me.txtDC = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(192, 234)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 83
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(245, 234)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 84
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lblControlGroup
        '
        Me.lblControlGroup.BackColor = System.Drawing.Color.Transparent
        Me.lblControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblControlGroup.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblControlGroup.Location = New System.Drawing.Point(-4, 198)
        Me.lblControlGroup.Name = "lblControlGroup"
        Me.lblControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblControlGroup.Size = New System.Drawing.Size(125, 17)
        Me.lblControlGroup.TabIndex = 80
        Me.lblControlGroup.Text = "SubTeam No :"
        Me.lblControlGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(204, 35)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(10, 17)
        Me.lblDash.TabIndex = 94
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_16.Location = New System.Drawing.Point(6, 24)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(105, 38)
        Me._lblLabel_16.TabIndex = 93
        Me._lblLabel_16.Text = "Date Range :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Enabled = False
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(218, 32)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(85, 20)
        Me.dtpEndDate.TabIndex = 96
        Me.dtpEndDate.Value = New Date(2008, 1, 11, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Enabled = False
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(117, 32)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 20)
        Me.dtpStartDate.TabIndex = 95
        Me.dtpStartDate.Value = New Date(2008, 1, 11, 0, 0, 0, 0)
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkDateRange)
        Me.GroupBox1.Controls.Add(Me.dtpEndDate)
        Me.GroupBox1.Controls.Add(Me.lblDash)
        Me.GroupBox1.Controls.Add(Me.dtpStartDate)
        Me.GroupBox1.Controls.Add(Me._lblLabel_16)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(303, 69)
        Me.GroupBox1.TabIndex = 97
        Me.GroupBox1.TabStop = False
        '
        'chkDateRange
        '
        Me.chkDateRange.AutoSize = True
        Me.chkDateRange.Location = New System.Drawing.Point(6, 7)
        Me.chkDateRange.Name = "chkDateRange"
        Me.chkDateRange.Size = New System.Drawing.Size(15, 14)
        Me.chkDateRange.TabIndex = 97
        Me.chkDateRange.UseVisualStyleBackColor = True
        '
        'txtSubTeam_No
        '
        Me.txtSubTeam_No.Location = New System.Drawing.Point(127, 198)
        Me.txtSubTeam_No.Name = "txtSubTeam_No"
        Me.txtSubTeam_No.Size = New System.Drawing.Size(59, 20)
        Me.txtSubTeam_No.TabIndex = 98
        '
        'txtStore_No
        '
        Me.txtStore_No.Location = New System.Drawing.Point(127, 87)
        Me.txtStore_No.MaxLength = 32
        Me.txtStore_No.Name = "txtStore_No"
        Me.txtStore_No.Size = New System.Drawing.Size(59, 20)
        Me.txtStore_No.TabIndex = 101
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(-4, 90)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(125, 17)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Store No :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkCOOL
        '
        Me.chkCOOL.AutoSize = True
        Me.chkCOOL.Location = New System.Drawing.Point(116, 113)
        Me.chkCOOL.Name = "chkCOOL"
        Me.chkCOOL.Size = New System.Drawing.Size(55, 17)
        Me.chkCOOL.TabIndex = 103
        Me.chkCOOL.Text = "COOL"
        Me.chkCOOL.UseVisualStyleBackColor = True
        '
        'chkBIO
        '
        Me.chkBIO.AutoSize = True
        Me.chkBIO.Location = New System.Drawing.Point(116, 147)
        Me.chkBIO.Name = "chkBIO"
        Me.chkBIO.Size = New System.Drawing.Size(44, 17)
        Me.chkBIO.TabIndex = 104
        Me.chkBIO.Text = "BIO"
        Me.chkBIO.UseVisualStyleBackColor = True
        '
        'txtDC
        '
        Me.txtDC.Location = New System.Drawing.Point(127, 172)
        Me.txtDC.Name = "txtDC"
        Me.txtDC.Size = New System.Drawing.Size(59, 20)
        Me.txtDC.TabIndex = 106
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(-4, 172)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(125, 17)
        Me.Label3.TabIndex = 105
        Me.Label3.Text = "DC :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'COOLShipping
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 283)
        Me.Controls.Add(Me.txtDC)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkBIO)
        Me.Controls.Add(Me.chkCOOL)
        Me.Controls.Add(Me.txtStore_No)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSubTeam_No)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblControlGroup)
        Me.Name = "COOLShipping"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "COOL Shipping"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblControlGroup As System.Windows.Forms.Label
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkDateRange As System.Windows.Forms.CheckBox
    Friend WithEvents txtSubTeam_No As System.Windows.Forms.TextBox
    Friend WithEvents txtStore_No As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCOOL As System.Windows.Forms.CheckBox
    Friend WithEvents chkBIO As System.Windows.Forms.CheckBox
    Friend WithEvents txtDC As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
End Class
