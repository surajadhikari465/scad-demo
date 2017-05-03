<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IngredientsCreateForm
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
        Me.grpNewRecord = New System.Windows.Forms.GroupBox()
        Me.LabelTypeCbx = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DescriptionTxt = New System.Windows.Forms.TextBox()
        Me.IngredientsTxt = New System.Windows.Forms.TextBox()
        Me.ExtraTextLabel = New System.Windows.Forms.Label()
        Me.DescriptionLabel = New System.Windows.Forms.Label()
        Me.AddRecordBtn = New System.Windows.Forms.Button()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.grpNewRecord.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpNewRecord
        '
        Me.grpNewRecord.CausesValidation = False
        Me.grpNewRecord.Controls.Add(Me.LabelTypeCbx)
        Me.grpNewRecord.Controls.Add(Me.Label1)
        Me.grpNewRecord.Controls.Add(Me.DescriptionTxt)
        Me.grpNewRecord.Controls.Add(Me.IngredientsTxt)
        Me.grpNewRecord.Controls.Add(Me.ExtraTextLabel)
        Me.grpNewRecord.Controls.Add(Me.DescriptionLabel)
        Me.grpNewRecord.Location = New System.Drawing.Point(12, 10)
        Me.grpNewRecord.Name = "grpNewRecord"
        Me.grpNewRecord.Size = New System.Drawing.Size(444, 321)
        Me.grpNewRecord.TabIndex = 11
        Me.grpNewRecord.TabStop = False
        '
        'LabelTypeCbx
        '
        Me.LabelTypeCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LabelTypeCbx.FormattingEnabled = True
        Me.LabelTypeCbx.Location = New System.Drawing.Point(91, 50)
        Me.LabelTypeCbx.Name = "LabelTypeCbx"
        Me.LabelTypeCbx.Size = New System.Drawing.Size(325, 21)
        Me.LabelTypeCbx.TabIndex = 136
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(24, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(80, 17)
        Me.Label1.TabIndex = 135
        Me.Label1.Text = "Label Type :"
        '
        'DescriptionTxt
        '
        Me.DescriptionTxt.BackColor = System.Drawing.SystemColors.Window
        Me.DescriptionTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.DescriptionTxt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DescriptionTxt.Location = New System.Drawing.Point(91, 21)
        Me.DescriptionTxt.MaxLength = 50
        Me.DescriptionTxt.Name = "DescriptionTxt"
        Me.DescriptionTxt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionTxt.Size = New System.Drawing.Size(325, 20)
        Me.DescriptionTxt.TabIndex = 1
        Me.DescriptionTxt.Tag = "String"
        '
        'IngredientsTxt
        '
        Me.IngredientsTxt.Location = New System.Drawing.Point(27, 95)
        Me.IngredientsTxt.MaxLength = 4200
        Me.IngredientsTxt.Multiline = True
        Me.IngredientsTxt.Name = "IngredientsTxt"
        Me.IngredientsTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.IngredientsTxt.Size = New System.Drawing.Size(389, 220)
        Me.IngredientsTxt.TabIndex = 2
        '
        'ExtraTextLabel
        '
        Me.ExtraTextLabel.BackColor = System.Drawing.Color.Transparent
        Me.ExtraTextLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ExtraTextLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtraTextLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExtraTextLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ExtraTextLabel.Location = New System.Drawing.Point(24, 70)
        Me.ExtraTextLabel.Name = "ExtraTextLabel"
        Me.ExtraTextLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExtraTextLabel.Size = New System.Drawing.Size(73, 22)
        Me.ExtraTextLabel.TabIndex = 132
        Me.ExtraTextLabel.Text = "Ingredients :"
        Me.ExtraTextLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.BackColor = System.Drawing.Color.Transparent
        Me.DescriptionLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.DescriptionLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DescriptionLabel.Location = New System.Drawing.Point(24, 24)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionLabel.Size = New System.Drawing.Size(80, 17)
        Me.DescriptionLabel.TabIndex = 133
        Me.DescriptionLabel.Text = "Description :"
        '
        'AddRecordBtn
        '
        Me.AddRecordBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AddRecordBtn.Location = New System.Drawing.Point(345, 337)
        Me.AddRecordBtn.Name = "AddRecordBtn"
        Me.AddRecordBtn.Size = New System.Drawing.Size(112, 30)
        Me.AddRecordBtn.TabIndex = 3
        Me.AddRecordBtn.Text = "Add This Record"
        Me.AddRecordBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.CausesValidation = False
        Me.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.CancelBtn.Location = New System.Drawing.Point(268, 337)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(70, 30)
        Me.CancelBtn.TabIndex = 4
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'IngredientsCreateForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 379)
        Me.Controls.Add(Me.grpNewRecord)
        Me.Controls.Add(Me.AddRecordBtn)
        Me.Controls.Add(Me.CancelBtn)
        Me.Name = "IngredientsCreateForm"
        Me.Text = "Create Ingredients"
        Me.grpNewRecord.ResumeLayout(False)
        Me.grpNewRecord.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpNewRecord As System.Windows.Forms.GroupBox
    Public WithEvents DescriptionTxt As System.Windows.Forms.TextBox
    Friend WithEvents IngredientsTxt As System.Windows.Forms.TextBox
    Public WithEvents ExtraTextLabel As System.Windows.Forms.Label
    Public WithEvents DescriptionLabel As System.Windows.Forms.Label
    Friend WithEvents AddRecordBtn As System.Windows.Forms.Button
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents LabelTypeCbx As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
End Class
