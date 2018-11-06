<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryWeeklyHistoryReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryWeeklyHistoryReport))
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.lblFacility = New System.Windows.Forms.Label
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.grpReportType = New System.Windows.Forms.GroupBox
        Me.radioPost = New System.Windows.Forms.RadioButton
        Me.radioPre = New System.Windows.Forms.RadioButton
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmbFacility = New System.Windows.Forms.ComboBox
        Me.grpReportType.SuspendLayout()
        Me.SuspendLayout()
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
        Me.cmbSubTeam.Location = New System.Drawing.Point(135, 133)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(178, 22)
        Me.cmbSubTeam.TabIndex = 58
        '
        'lblFacility
        '
        Me.lblFacility.BackColor = System.Drawing.Color.Transparent
        Me.lblFacility.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFacility.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFacility.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFacility.Location = New System.Drawing.Point(21, 110)
        Me.lblFacility.Name = "lblFacility"
        Me.lblFacility.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFacility.Size = New System.Drawing.Size(97, 17)
        Me.lblFacility.TabIndex = 55
        Me.lblFacility.Text = "Facility :"
        Me.lblFacility.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSubTeam
        '
        Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.Location = New System.Drawing.Point(21, 133)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubTeam.Size = New System.Drawing.Size(97, 17)
        Me.lblSubTeam.TabIndex = 57
        Me.lblSubTeam.Text = "Sub-Team :"
        Me.lblSubTeam.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'grpReportType
        '
        Me.grpReportType.Controls.Add(Me.radioPost)
        Me.grpReportType.Controls.Add(Me.radioPre)
        Me.grpReportType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpReportType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpReportType.Location = New System.Drawing.Point(78, 12)
        Me.grpReportType.Name = "grpReportType"
        Me.grpReportType.Size = New System.Drawing.Size(182, 87)
        Me.grpReportType.TabIndex = 59
        Me.grpReportType.TabStop = False
        Me.grpReportType.Text = "Report Type"
        '
        'radioPost
        '
        Me.radioPost.AutoSize = True
        Me.radioPost.Location = New System.Drawing.Point(24, 52)
        Me.radioPost.Name = "radioPost"
        Me.radioPost.Size = New System.Drawing.Size(108, 18)
        Me.radioPost.TabIndex = 1
        Me.radioPost.Text = "Post-Allocation"
        Me.radioPost.UseVisualStyleBackColor = True
        '
        'radioPre
        '
        Me.radioPre.AutoSize = True
        Me.radioPre.Checked = True
        Me.radioPre.Location = New System.Drawing.Point(24, 28)
        Me.radioPre.Name = "radioPre"
        Me.radioPre.Size = New System.Drawing.Size(100, 18)
        Me.radioPre.TabIndex = 0
        Me.radioPre.TabStop = True
        Me.radioPre.Text = "Pre-allocation"
        Me.radioPre.UseVisualStyleBackColor = True
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(32, 199)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 60
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(135, 176)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 61
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
        Me.cmdExit.Location = New System.Drawing.Point(183, 176)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 62
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmbFacility
        '
        Me.cmbFacility.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbFacility.FormattingEnabled = True
        Me.cmbFacility.Location = New System.Drawing.Point(135, 106)
        Me.cmbFacility.Name = "cmbFacility"
        Me.cmbFacility.Size = New System.Drawing.Size(178, 22)
        Me.cmbFacility.TabIndex = 65
        '
        'InventoryWeeklyHistoryReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(322, 239)
        Me.Controls.Add(Me.cmbFacility)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.grpReportType)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.lblFacility)
        Me.Controls.Add(Me.lblSubTeam)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InventoryWeeklyHistoryReport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventory Weekly History Report"
        Me.grpReportType.ResumeLayout(False)
        Me.grpReportType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents lblFacility As System.Windows.Forms.Label
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Friend WithEvents grpReportType As System.Windows.Forms.GroupBox
    Friend WithEvents radioPost As System.Windows.Forms.RadioButton
    Friend WithEvents radioPre As System.Windows.Forms.RadioButton
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents cmbFacility As System.Windows.Forms.ComboBox
End Class
