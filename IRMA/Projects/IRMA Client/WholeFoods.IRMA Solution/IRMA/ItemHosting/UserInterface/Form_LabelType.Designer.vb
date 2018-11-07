<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_LabelType
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
        Me.AddButton = New System.Windows.Forms.Button
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.DescriptionTextbox = New System.Windows.Forms.TextBox
        Me.DescriptionLabel = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.DescriptionCombo = New System.Windows.Forms.ComboBox
        Me.LinesPerLabelNumericEditor = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.CharsPerLineNumericEditor = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.ApplyButton = New System.Windows.Forms.Button
        Me.CloseButton = New System.Windows.Forms.Button
        CType(Me.LinesPerLabelNumericEditor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CharsPerLineNumericEditor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AddButton
        '
        Me.AddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AddButton.Location = New System.Drawing.Point(10, 134)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(70, 21)
        Me.AddButton.TabIndex = 4
        Me.AddButton.Text = "Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DeleteButton.Location = New System.Drawing.Point(86, 134)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(70, 21)
        Me.DeleteButton.TabIndex = 5
        Me.DeleteButton.Text = "Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'DescriptionTextbox
        '
        Me.DescriptionTextbox.AcceptsReturn = True
        Me.DescriptionTextbox.BackColor = System.Drawing.SystemColors.Window
        Me.DescriptionTextbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.DescriptionTextbox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionTextbox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DescriptionTextbox.Location = New System.Drawing.Point(137, 42)
        Me.DescriptionTextbox.MaxLength = 25
        Me.DescriptionTextbox.Name = "DescriptionTextbox"
        Me.DescriptionTextbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionTextbox.Size = New System.Drawing.Size(319, 20)
        Me.DescriptionTextbox.TabIndex = 1
        Me.DescriptionTextbox.Tag = "String"
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.BackColor = System.Drawing.Color.Transparent
        Me.DescriptionLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.DescriptionLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DescriptionLabel.Location = New System.Drawing.Point(51, 42)
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
        Me.Label2.Location = New System.Drawing.Point(30, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(101, 18)
        Me.Label2.TabIndex = 67
        Me.Label2.Text = "Lines Per Label :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(-1, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(132, 18)
        Me.Label3.TabIndex = 68
        Me.Label3.Text = "Characters Per Line :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(51, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(80, 17)
        Me.Label1.TabIndex = 72
        Me.Label1.Text = "Label Type :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DescriptionCombo
        '
        Me.DescriptionCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.DescriptionCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.DescriptionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DescriptionCombo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionCombo.FormattingEnabled = True
        Me.DescriptionCombo.Location = New System.Drawing.Point(137, 12)
        Me.DescriptionCombo.Name = "DescriptionCombo"
        Me.DescriptionCombo.Size = New System.Drawing.Size(319, 22)
        Me.DescriptionCombo.TabIndex = 0
        '
        'LinesPerLabelNumericEditor
        '
        Me.LinesPerLabelNumericEditor.AlwaysInEditMode = True
        Me.LinesPerLabelNumericEditor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinesPerLabelNumericEditor.Location = New System.Drawing.Point(137, 68)
        Me.LinesPerLabelNumericEditor.MaxValue = 999
        Me.LinesPerLabelNumericEditor.MinValue = 1
        Me.LinesPerLabelNumericEditor.Name = "LinesPerLabelNumericEditor"
        Me.LinesPerLabelNumericEditor.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.LinesPerLabelNumericEditor.Size = New System.Drawing.Size(100, 21)
        Me.LinesPerLabelNumericEditor.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always
        Me.LinesPerLabelNumericEditor.TabIndex = 2
        Me.LinesPerLabelNumericEditor.Value = 1
        '
        'CharsPerLineNumericEditor
        '
        Me.CharsPerLineNumericEditor.AlwaysInEditMode = True
        Me.CharsPerLineNumericEditor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CharsPerLineNumericEditor.Location = New System.Drawing.Point(137, 100)
        Me.CharsPerLineNumericEditor.MaxValue = 100
        Me.CharsPerLineNumericEditor.MinValue = 1
        Me.CharsPerLineNumericEditor.Name = "CharsPerLineNumericEditor"
        Me.CharsPerLineNumericEditor.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.CharsPerLineNumericEditor.Size = New System.Drawing.Size(100, 21)
        Me.CharsPerLineNumericEditor.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always
        Me.CharsPerLineNumericEditor.TabIndex = 3
        Me.CharsPerLineNumericEditor.Value = 1
        '
        'ApplyButton
        '
        Me.ApplyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ApplyButton.Location = New System.Drawing.Point(310, 134)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(70, 21)
        Me.ApplyButton.TabIndex = 6
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CloseButton.Location = New System.Drawing.Point(386, 134)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(70, 21)
        Me.CloseButton.TabIndex = 7
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Form_LabelType
        '
        Me.AcceptButton = Me.ApplyButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(467, 162)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(Me.CharsPerLineNumericEditor)
        Me.Controls.Add(Me.LinesPerLabelNumericEditor)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DescriptionCombo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.DescriptionTextbox)
        Me.Controls.Add(Me.DescriptionLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_LabelType"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scale Label Type Management"
        CType(Me.LinesPerLabelNumericEditor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CharsPerLineNumericEditor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Public WithEvents DescriptionTextbox As System.Windows.Forms.TextBox
    Public WithEvents DescriptionLabel As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DescriptionCombo As System.Windows.Forms.ComboBox
    Friend WithEvents LinesPerLabelNumericEditor As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents CharsPerLineNumericEditor As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
End Class
