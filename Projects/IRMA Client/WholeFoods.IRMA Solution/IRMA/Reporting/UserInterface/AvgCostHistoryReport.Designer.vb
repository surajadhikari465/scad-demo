<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAvgCostHistoryReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAvgCostHistoryReport))
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.lblEndSate = New System.Windows.Forms.Label
        Me.lblIdentifier = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblBusinessUnit = New System.Windows.Forms.Label
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.lblTolerance = New System.Windows.Forms.Label
        Me.chkLimitOutput = New System.Windows.Forms.CheckBox
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.txtTolerance = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.Location = New System.Drawing.Point(27, 97)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(61, 13)
        Me.lblStartDate.TabIndex = 0
        Me.lblStartDate.Text = "Start Date:"
        '
        'lblEndSate
        '
        Me.lblEndSate.AutoSize = True
        Me.lblEndSate.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndSate.Location = New System.Drawing.Point(31, 124)
        Me.lblEndSate.Name = "lblEndSate"
        Me.lblEndSate.Size = New System.Drawing.Size(57, 13)
        Me.lblEndSate.TabIndex = 1
        Me.lblEndSate.Text = "End Date:"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.AutoSize = True
        Me.lblIdentifier.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIdentifier.Location = New System.Drawing.Point(31, 71)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.Size = New System.Drawing.Size(57, 13)
        Me.lblIdentifier.TabIndex = 2
        Me.lblIdentifier.Text = "Identifier:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(245, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "(optional)"
        '
        'lblBusinessUnit
        '
        Me.lblBusinessUnit.AutoSize = True
        Me.lblBusinessUnit.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBusinessUnit.Location = New System.Drawing.Point(8, 15)
        Me.lblBusinessUnit.Name = "lblBusinessUnit"
        Me.lblBusinessUnit.Size = New System.Drawing.Size(80, 13)
        Me.lblBusinessUnit.TabIndex = 4
        Me.lblBusinessUnit.Text = "Business Unit:"
        '
        'lblSubTeam
        '
        Me.lblSubTeam.AutoSize = True
        Me.lblSubTeam.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubTeam.Location = New System.Drawing.Point(27, 43)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.Size = New System.Drawing.Size(61, 13)
        Me.lblSubTeam.TabIndex = 5
        Me.lblSubTeam.Text = "Sub Team:"
        '
        'lblTolerance
        '
        Me.lblTolerance.AutoSize = True
        Me.lblTolerance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTolerance.Location = New System.Drawing.Point(29, 152)
        Me.lblTolerance.Name = "lblTolerance"
        Me.lblTolerance.Size = New System.Drawing.Size(60, 13)
        Me.lblTolerance.TabIndex = 6
        Me.lblTolerance.Text = "Tolerance:"
        '
        'chkLimitOutput
        '
        Me.chkLimitOutput.AutoSize = True
        Me.chkLimitOutput.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLimitOutput.Location = New System.Drawing.Point(94, 177)
        Me.chkLimitOutput.Name = "chkLimitOutput"
        Me.chkLimitOutput.Size = New System.Drawing.Size(212, 17)
        Me.chkLimitOutput.TabIndex = 7
        Me.chkLimitOutput.Text = "Limit output to Tolerance violations"
        Me.chkLimitOutput.UseVisualStyleBackColor = True
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(94, 94)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(149, 22)
        Me.dtpStartDate.TabIndex = 8
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(94, 121)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(149, 22)
        Me.dtpEndDate.TabIndex = 9
        '
        'txtTolerance
        '
        Me.txtTolerance.Location = New System.Drawing.Point(94, 149)
        Me.txtTolerance.Name = "txtTolerance"
        Me.txtTolerance.Size = New System.Drawing.Size(85, 22)
        Me.txtTolerance.TabIndex = 10
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(180, 153)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "%"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.Location = New System.Drawing.Point(94, 68)
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.Size = New System.Drawing.Size(149, 22)
        Me.txtIdentifier.TabIndex = 12
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Location = New System.Drawing.Point(94, 12)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(208, 21)
        Me.cmbStore.TabIndex = 63
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Location = New System.Drawing.Point(94, 40)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(208, 21)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 64
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(208, 203)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 66
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
        Me.cmdExit.Location = New System.Drawing.Point(261, 203)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 67
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'frmAvgCostHistoryReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(318, 256)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtTolerance)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.chkLimitOutput)
        Me.Controls.Add(Me.lblTolerance)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.lblBusinessUnit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.lblEndSate)
        Me.Controls.Add(Me.lblStartDate)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAvgCostHistoryReport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Average Cost History Variance Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStartDate As System.Windows.Forms.Label
    Friend WithEvents lblEndSate As System.Windows.Forms.Label
    Friend WithEvents lblIdentifier As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblBusinessUnit As System.Windows.Forms.Label
    Friend WithEvents lblSubTeam As System.Windows.Forms.Label
    Friend WithEvents lblTolerance As System.Windows.Forms.Label
    Friend WithEvents chkLimitOutput As System.Windows.Forms.CheckBox
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtTolerance As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
End Class
