<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAvgCostAdjReason
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAvgCostAdjReason))
        Me.lblReasonList = New System.Windows.Forms.Label
        Me.cmbReason = New System.Windows.Forms.ComboBox
        Me.chkActive = New System.Windows.Forms.CheckBox
        Me.grpNewReason = New System.Windows.Forms.GroupBox
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.lblReasonDescription = New System.Windows.Forms.Label
        Me.cmdSave = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.frmErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.grpNewReason.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblReasonList
        '
        Me.lblReasonList.AutoSize = True
        Me.lblReasonList.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReasonList.Location = New System.Drawing.Point(30, 40)
        Me.lblReasonList.Name = "lblReasonList"
        Me.lblReasonList.Size = New System.Drawing.Size(90, 13)
        Me.lblReasonList.TabIndex = 0
        Me.lblReasonList.Text = "Choose Reason:"
        '
        'cmbReason
        '
        Me.cmbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReason.FormattingEnabled = True
        Me.cmbReason.Location = New System.Drawing.Point(126, 37)
        Me.cmbReason.Name = "cmbReason"
        Me.cmbReason.Size = New System.Drawing.Size(255, 21)
        Me.cmbReason.TabIndex = 4
        '
        'chkActive
        '
        Me.chkActive.AutoSize = True
        Me.chkActive.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActive.Location = New System.Drawing.Point(126, 71)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.Size = New System.Drawing.Size(58, 17)
        Me.chkActive.TabIndex = 5
        Me.chkActive.Text = "Active"
        Me.chkActive.UseVisualStyleBackColor = True
        '
        'grpNewReason
        '
        Me.grpNewReason.Controls.Add(Me.cmdAdd)
        Me.grpNewReason.Controls.Add(Me.txtDescription)
        Me.grpNewReason.Controls.Add(Me.lblReasonDescription)
        Me.grpNewReason.Location = New System.Drawing.Point(12, 9)
        Me.grpNewReason.Name = "grpNewReason"
        Me.grpNewReason.Size = New System.Drawing.Size(405, 100)
        Me.grpNewReason.TabIndex = 0
        Me.grpNewReason.TabStop = False
        Me.grpNewReason.Text = "Add New Reason"
        '
        'cmdAdd
        '
        Me.cmdAdd.Enabled = False
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(305, 64)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(76, 29)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'txtDescription
        '
        Me.txtDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDescription.Location = New System.Drawing.Point(126, 36)
        Me.txtDescription.MaxLength = 75
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(255, 22)
        Me.txtDescription.TabIndex = 1
        '
        'lblReasonDescription
        '
        Me.lblReasonDescription.AutoSize = True
        Me.lblReasonDescription.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReasonDescription.Location = New System.Drawing.Point(10, 39)
        Me.lblReasonDescription.Name = "lblReasonDescription"
        Me.lblReasonDescription.Size = New System.Drawing.Size(110, 13)
        Me.lblReasonDescription.TabIndex = 0
        Me.lblReasonDescription.Text = "Reason Description:"
        '
        'cmdSave
        '
        Me.cmdSave.CausesValidation = False
        Me.cmdSave.Enabled = False
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.Location = New System.Drawing.Point(305, 64)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(76, 29)
        Me.cmdSave.TabIndex = 6
        Me.cmdSave.Text = "Save"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbReason)
        Me.GroupBox1.Controls.Add(Me.cmdSave)
        Me.GroupBox1.Controls.Add(Me.lblReasonList)
        Me.GroupBox1.Controls.Add(Me.chkActive)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 115)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(405, 100)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Enable/Disable"
        '
        'frmErrorProvider
        '
        Me.frmErrorProvider.ContainerControl = Me
        '
        'frmAvgCostAdjReason
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(429, 225)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpNewReason)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAvgCostAdjReason"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Average Cost Adjustment Reasons"
        Me.grpNewReason.ResumeLayout(False)
        Me.grpNewReason.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblReasonList As System.Windows.Forms.Label
    Friend WithEvents cmbReason As System.Windows.Forms.ComboBox
    Friend WithEvents chkActive As System.Windows.Forms.CheckBox
    Friend WithEvents grpNewReason As System.Windows.Forms.GroupBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblReasonDescription As System.Windows.Forms.Label
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents frmErrorProvider As System.Windows.Forms.ErrorProvider
End Class
