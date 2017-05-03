<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Promotion_AddRequirement
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
        Me.ListBox_AvailableGroups = New System.Windows.Forms.ListBox
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Button_Add = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox_Quantity = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ListBox_AvailableGroups
        '
        Me.ListBox_AvailableGroups.FormattingEnabled = True
        Me.ListBox_AvailableGroups.Location = New System.Drawing.Point(12, 29)
        Me.ListBox_AvailableGroups.Name = "ListBox_AvailableGroups"
        Me.ListBox_AvailableGroups.Size = New System.Drawing.Size(275, 108)
        Me.ListBox_AvailableGroups.TabIndex = 0
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(125, 190)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(210, 190)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 2
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(219, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "How many Items are required from this group:"
        '
        'TextBox_Quantity
        '
        Me.TextBox_Quantity.Location = New System.Drawing.Point(233, 140)
        Me.TextBox_Quantity.Name = "TextBox_Quantity"
        Me.TextBox_Quantity.Size = New System.Drawing.Size(54, 20)
        Me.TextBox_Quantity.TabIndex = 4
        Me.TextBox_Quantity.Text = "1"
        Me.TextBox_Quantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(204, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Available Groups to add to this Promotion:"
        '
        'Form_Promotion_AddRequirement
        '
        Me.AcceptButton = Me.Button_Add
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(295, 225)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox_Quantity)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button_Add)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.ListBox_AvailableGroups)
        Me.Name = "Form_Promotion_AddRequirement"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Requirement"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBox_AvailableGroups As System.Windows.Forms.ListBox
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_Quantity As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
