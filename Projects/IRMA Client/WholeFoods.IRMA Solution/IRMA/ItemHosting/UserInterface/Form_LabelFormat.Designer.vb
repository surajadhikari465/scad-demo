<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_LabelFormat
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
        Me.DescriptionTextBox = New System.Windows.Forms.TextBox
        Me.DescriptionLabel = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DescriptionCombo = New System.Windows.Forms.ComboBox
        Me.AddButton = New System.Windows.Forms.Button
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.ApplyButton = New System.Windows.Forms.Button
        Me.CloseButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'DescriptionTextBox
        '
        Me.DescriptionTextBox.AcceptsReturn = True
        Me.DescriptionTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.DescriptionTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.DescriptionTextBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DescriptionTextBox.Location = New System.Drawing.Point(94, 37)
        Me.DescriptionTextBox.MaxLength = 25
        Me.DescriptionTextBox.Name = "DescriptionTextBox"
        Me.DescriptionTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionTextBox.Size = New System.Drawing.Size(319, 20)
        Me.DescriptionTextBox.TabIndex = 1
        Me.DescriptionTextBox.Tag = "String"
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.BackColor = System.Drawing.Color.Transparent
        Me.DescriptionLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.DescriptionLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DescriptionLabel.Location = New System.Drawing.Point(8, 40)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionLabel.Size = New System.Drawing.Size(80, 17)
        Me.DescriptionLabel.TabIndex = 58
        Me.DescriptionLabel.Text = "Description :"
        Me.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(1, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(87, 17)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "Label Format :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DescriptionCombo
        '
        Me.DescriptionCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.DescriptionCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.DescriptionCombo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionCombo.FormattingEnabled = True
        Me.DescriptionCombo.Location = New System.Drawing.Point(94, 10)
        Me.DescriptionCombo.Name = "DescriptionCombo"
        Me.DescriptionCombo.Size = New System.Drawing.Size(319, 22)
        Me.DescriptionCombo.TabIndex = 0
        '
        'AddButton
        '
        Me.AddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AddButton.Location = New System.Drawing.Point(34, 74)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(70, 21)
        Me.AddButton.TabIndex = 2
        Me.AddButton.Text = "Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DeleteButton.Location = New System.Drawing.Point(110, 74)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(70, 21)
        Me.DeleteButton.TabIndex = 3
        Me.DeleteButton.Text = "Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ApplyButton.Location = New System.Drawing.Point(267, 74)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(70, 21)
        Me.ApplyButton.TabIndex = 4
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CloseButton.Location = New System.Drawing.Point(343, 74)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(70, 21)
        Me.CloseButton.TabIndex = 5
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Form_LabelFormat
        '
        Me.AcceptButton = Me.ApplyButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(427, 107)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DescriptionCombo)
        Me.Controls.Add(Me.DescriptionTextBox)
        Me.Controls.Add(Me.DescriptionLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_LabelFormat"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Label Format Management"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents DescriptionTextBox As System.Windows.Forms.TextBox
    Public WithEvents DescriptionLabel As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DescriptionCombo As System.Windows.Forms.ComboBox
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
End Class
