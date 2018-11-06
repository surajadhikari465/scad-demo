<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KitchenCaseXferRpt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(KitchenCaseXferRpt))
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmbFacility = New System.Windows.Forms.ComboBox
        Me.lblDash = New System.Windows.Forms.Label
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.lblFacility = New System.Windows.Forms.Label
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me._lblLabel_16 = New System.Windows.Forms.Label
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.SuspendLayout()
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(104, 184)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 56
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmbFacility
        '
        Me.cmbFacility.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbFacility.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbFacility.BackColor = System.Drawing.SystemColors.Window
        Me.cmbFacility.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFacility.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFacility.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbFacility.Location = New System.Drawing.Point(121, 70)
        Me.cmbFacility.Name = "cmbFacility"
        Me.cmbFacility.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbFacility.Size = New System.Drawing.Size(177, 22)
        Me.cmbFacility.Sorted = True
        Me.cmbFacility.TabIndex = 52
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(201, 24)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 59
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.cmbSubTeam.Location = New System.Drawing.Point(121, 114)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cmbSubTeam.TabIndex = 54
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(209, 161)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 57
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
        Me.cmdExit.Location = New System.Drawing.Point(257, 161)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 58
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lblFacility
        '
        Me.lblFacility.BackColor = System.Drawing.Color.Transparent
        Me.lblFacility.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFacility.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFacility.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFacility.Location = New System.Drawing.Point(17, 73)
        Me.lblFacility.Name = "lblFacility"
        Me.lblFacility.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFacility.Size = New System.Drawing.Size(97, 17)
        Me.lblFacility.TabIndex = 51
        Me.lblFacility.Text = "Facility :"
        Me.lblFacility.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSubTeam
        '
        Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.Location = New System.Drawing.Point(18, 117)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubTeam.Size = New System.Drawing.Size(97, 17)
        Me.lblSubTeam.TabIndex = 53
        Me.lblSubTeam.Text = "Sub-Team :"
        Me.lblSubTeam.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_16.Location = New System.Drawing.Point(17, 28)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_16.TabIndex = 55
        Me._lblLabel_16.Text = "Date Range :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(218, 25)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 61
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(121, 25)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 60
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'KitchenCaseXferRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(321, 230)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmbFacility)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblFacility)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Name = "KitchenCaseXferRpt"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Kitchen Case Transfer Report"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmbFacility As System.Windows.Forms.ComboBox
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblFacility As System.Windows.Forms.Label
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
End Class
