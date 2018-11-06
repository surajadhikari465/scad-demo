<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddInvoiceCharges
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddInvoiceCharges))
        Me.rdoNonAllocated = New System.Windows.Forms.RadioButton
        Me.rdoAllocated = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label_ChargeOrAllowance = New System.Windows.Forms.Label
        Me.TextBox_value = New System.Windows.Forms.TextBox
        Me.ComboBox_SubTeamGLAcct = New System.Windows.Forms.ComboBox
        Me.ComboBox_AllocatedCharges = New System.Windows.Forms.ComboBox
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Button_Ok = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'rdoNonAllocated
        '
        Me.rdoNonAllocated.AutoSize = True
        Me.rdoNonAllocated.Location = New System.Drawing.Point(6, 42)
        Me.rdoNonAllocated.Name = "rdoNonAllocated"
        Me.rdoNonAllocated.Size = New System.Drawing.Size(129, 17)
        Me.rdoNonAllocated.TabIndex = 0
        Me.rdoNonAllocated.Text = "&Non-Allocated Charge"
        Me.rdoNonAllocated.UseVisualStyleBackColor = True
        '
        'rdoAllocated
        '
        Me.rdoAllocated.AutoSize = True
        Me.rdoAllocated.Checked = True
        Me.rdoAllocated.Location = New System.Drawing.Point(6, 19)
        Me.rdoAllocated.Name = "rdoAllocated"
        Me.rdoAllocated.Size = New System.Drawing.Size(106, 17)
        Me.rdoAllocated.TabIndex = 1
        Me.rdoAllocated.TabStop = True
        Me.rdoAllocated.Text = "&Allocated Charge"
        Me.rdoAllocated.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdoNonAllocated)
        Me.GroupBox1.Controls.Add(Me.rdoAllocated)
        Me.GroupBox1.Location = New System.Drawing.Point(378, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(147, 80)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "&Charge Type"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label_ChargeOrAllowance)
        Me.GroupBox2.Controls.Add(Me.TextBox_value)
        Me.GroupBox2.Controls.Add(Me.ComboBox_SubTeamGLAcct)
        Me.GroupBox2.Controls.Add(Me.ComboBox_AllocatedCharges)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(357, 80)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'Label_ChargeOrAllowance
        '
        Me.Label_ChargeOrAllowance.AutoSize = True
        Me.Label_ChargeOrAllowance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ChargeOrAllowance.Location = New System.Drawing.Point(203, 32)
        Me.Label_ChargeOrAllowance.Name = "Label_ChargeOrAllowance"
        Me.Label_ChargeOrAllowance.Size = New System.Drawing.Size(14, 13)
        Me.Label_ChargeOrAllowance.TabIndex = 2
        Me.Label_ChargeOrAllowance.Text = "+"
        '
        'TextBox_value
        '
        Me.TextBox_value.Location = New System.Drawing.Point(217, 29)
        Me.TextBox_value.Name = "TextBox_value"
        Me.TextBox_value.Size = New System.Drawing.Size(122, 20)
        Me.TextBox_value.TabIndex = 1
        '
        'ComboBox_SubTeamGLAcct
        '
        Me.ComboBox_SubTeamGLAcct.FormattingEnabled = True
        Me.ComboBox_SubTeamGLAcct.Location = New System.Drawing.Point(20, 29)
        Me.ComboBox_SubTeamGLAcct.Name = "ComboBox_SubTeamGLAcct"
        Me.ComboBox_SubTeamGLAcct.Size = New System.Drawing.Size(179, 21)
        Me.ComboBox_SubTeamGLAcct.TabIndex = 0
        '
        'ComboBox_AllocatedCharges
        '
        Me.ComboBox_AllocatedCharges.FormattingEnabled = True
        Me.ComboBox_AllocatedCharges.Location = New System.Drawing.Point(20, 29)
        Me.ComboBox_AllocatedCharges.Name = "ComboBox_AllocatedCharges"
        Me.ComboBox_AllocatedCharges.Size = New System.Drawing.Size(179, 21)
        Me.ComboBox_AllocatedCharges.TabIndex = 0
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Image = CType(resources.GetObject("Button_Cancel.Image"), System.Drawing.Image)
        Me.Button_Cancel.Location = New System.Drawing.Point(453, 99)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(70, 24)
        Me.Button_Cancel.TabIndex = 4
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_Ok
        '
        Me.Button_Ok.Image = CType(resources.GetObject("Button_Ok.Image"), System.Drawing.Image)
        Me.Button_Ok.Location = New System.Drawing.Point(378, 99)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(69, 24)
        Me.Button_Ok.TabIndex = 5
        Me.Button_Ok.Text = "&Ok"
        Me.Button_Ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'AddInvoiceCharges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 127)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "AddInvoiceCharges"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Charges to an Invoice"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rdoNonAllocated As System.Windows.Forms.RadioButton
    Friend WithEvents rdoAllocated As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_value As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox_SubTeamGLAcct As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents ComboBox_AllocatedCharges As System.Windows.Forms.ComboBox
    Friend WithEvents Label_ChargeOrAllowance As System.Windows.Forms.Label
End Class
