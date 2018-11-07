<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ControlGroup3WayMatchLog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ControlGroup3WayMatchLog))
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.lblControlGroup = New System.Windows.Forms.Label
        Me.cmbUserID = New System.Windows.Forms.ComboBox
        Me.cmbStatus = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtControlGroupID = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(59, 53)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(54, 21)
        Me._lblLabel_1.TabIndex = 75
        Me._lblLabel_1.Text = "User Id:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(65, 153)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(79, 17)
        Me.chkPrintOnly.TabIndex = 72
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
        Me.cmdReport.Location = New System.Drawing.Point(158, 141)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 73
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
        Me.cmdExit.Location = New System.Drawing.Point(218, 141)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 74
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lblControlGroup
        '
        Me.lblControlGroup.BackColor = System.Drawing.Color.Transparent
        Me.lblControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblControlGroup.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblControlGroup.Location = New System.Drawing.Point(4, 9)
        Me.lblControlGroup.Name = "lblControlGroup"
        Me.lblControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblControlGroup.Size = New System.Drawing.Size(109, 17)
        Me.lblControlGroup.TabIndex = 68
        Me.lblControlGroup.Text = "Control group id :"
        Me.lblControlGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbUserID
        '
        Me.cmbUserID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUserID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUserID.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUserID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUserID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUserID.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUserID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUserID.Location = New System.Drawing.Point(119, 50)
        Me.cmbUserID.Name = "cmbUserID"
        Me.cmbUserID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbUserID.Size = New System.Drawing.Size(149, 22)
        Me.cmbUserID.TabIndex = 77
        '
        'cmbStatus
        '
        Me.cmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStatus.Location = New System.Drawing.Point(119, 97)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStatus.Size = New System.Drawing.Size(149, 22)
        Me.cmbStatus.TabIndex = 79
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(62, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(51, 21)
        Me.Label1.TabIndex = 78
        Me.Label1.Text = "Status:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtControlGroupID
        '
        Me.txtControlGroupID.Location = New System.Drawing.Point(119, 9)
        Me.txtControlGroupID.Name = "txtControlGroupID"
        Me.txtControlGroupID.Size = New System.Drawing.Size(149, 20)
        Me.txtControlGroupID.TabIndex = 99
        '
        'ControlGroup3WayMatchLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(289, 197)
        Me.Controls.Add(Me.txtControlGroupID)
        Me.Controls.Add(Me.cmbStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbUserID)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblControlGroup)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ControlGroup3WayMatchLog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Control Group Log Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblControlGroup As System.Windows.Forms.Label
    Public WithEvents cmbUserID As System.Windows.Forms.ComboBox
    Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtControlGroupID As System.Windows.Forms.TextBox
End Class
