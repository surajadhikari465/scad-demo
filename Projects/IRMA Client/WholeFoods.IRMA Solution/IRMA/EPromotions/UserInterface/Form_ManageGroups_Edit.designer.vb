<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageGroups_Edit
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox_GroupName = New System.Windows.Forms.TextBox
        Me.RadioButton_OR = New System.Windows.Forms.RadioButton
        Me.RadioButton_AND = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Group Name:"
        '
        'TextBox_GroupName
        '
        Me.TextBox_GroupName.Location = New System.Drawing.Point(82, 12)
        Me.TextBox_GroupName.MaxLength = 50
        Me.TextBox_GroupName.Name = "TextBox_GroupName"
        Me.TextBox_GroupName.Size = New System.Drawing.Size(182, 20)
        Me.TextBox_GroupName.TabIndex = 1
        '
        'RadioButton_OR
        '
        Me.RadioButton_OR.AutoSize = True
        Me.RadioButton_OR.Location = New System.Drawing.Point(132, 43)
        Me.RadioButton_OR.Name = "RadioButton_OR"
        Me.RadioButton_OR.Size = New System.Drawing.Size(36, 17)
        Me.RadioButton_OR.TabIndex = 2
        Me.RadioButton_OR.Text = "Or"
        Me.RadioButton_OR.UseVisualStyleBackColor = True
        '
        'RadioButton_AND
        '
        Me.RadioButton_AND.AutoSize = True
        Me.RadioButton_AND.Checked = True
        Me.RadioButton_AND.Location = New System.Drawing.Point(82, 43)
        Me.RadioButton_AND.Name = "RadioButton_AND"
        Me.RadioButton_AND.Size = New System.Drawing.Size(44, 17)
        Me.RadioButton_AND.TabIndex = 3
        Me.RadioButton_AND.TabStop = True
        Me.RadioButton_AND.Text = "And"
        Me.RadioButton_AND.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Group Logic:"
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(189, 77)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 5
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(108, 77)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 6
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_ManageGroups_Edit
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(276, 113)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RadioButton_AND)
        Me.Controls.Add(Me.RadioButton_OR)
        Me.Controls.Add(Me.TextBox_GroupName)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManageGroups_Edit"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form_ManageGroups_Edit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_GroupName As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton_OR As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_AND As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
End Class
