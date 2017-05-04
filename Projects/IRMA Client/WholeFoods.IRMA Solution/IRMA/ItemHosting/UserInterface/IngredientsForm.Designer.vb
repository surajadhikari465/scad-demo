<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IngredientsForm
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
        Me.grpDetails = New System.Windows.Forms.GroupBox()
        Me.DescriptionTxt = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.IngredientsTxt = New System.Windows.Forms.TextBox()
        Me.SaveChangesBtn = New System.Windows.Forms.Button()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.LabelTypeCbx = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.LabelTypeCbx)
        Me.grpDetails.Controls.Add(Me.Label1)
        Me.grpDetails.Controls.Add(Me.DescriptionTxt)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.IngredientsTxt)
        Me.grpDetails.Location = New System.Drawing.Point(12, 12)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(441, 227)
        Me.grpDetails.TabIndex = 1
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Ingredients Record Details"
        '
        'DescriptionTxt
        '
        Me.DescriptionTxt.Location = New System.Drawing.Point(129, 25)
        Me.DescriptionTxt.Name = "DescriptionTxt"
        Me.DescriptionTxt.Size = New System.Drawing.Size(282, 20)
        Me.DescriptionTxt.TabIndex = 0
        Me.DescriptionTxt.WordWrap = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Ingredients Description:"
        '
        'IngredientsTxt
        '
        Me.IngredientsTxt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IngredientsTxt.Location = New System.Drawing.Point(12, 78)
        Me.IngredientsTxt.MaxLength = 4200
        Me.IngredientsTxt.Multiline = True
        Me.IngredientsTxt.Name = "IngredientsTxt"
        Me.IngredientsTxt.Size = New System.Drawing.Size(399, 143)
        Me.IngredientsTxt.TabIndex = 0
        '
        'SaveChangesBtn
        '
        Me.SaveChangesBtn.Location = New System.Drawing.Point(328, 245)
        Me.SaveChangesBtn.Name = "SaveChangesBtn"
        Me.SaveChangesBtn.Size = New System.Drawing.Size(125, 30)
        Me.SaveChangesBtn.TabIndex = 0
        Me.SaveChangesBtn.Text = "Save Changes"
        Me.SaveChangesBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.Location = New System.Drawing.Point(197, 245)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(125, 30)
        Me.CancelBtn.TabIndex = 0
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'LabelTypeCbx
        '
        Me.LabelTypeCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LabelTypeCbx.FormattingEnabled = True
        Me.LabelTypeCbx.Location = New System.Drawing.Point(129, 51)
        Me.LabelTypeCbx.Name = "LabelTypeCbx"
        Me.LabelTypeCbx.Size = New System.Drawing.Size(282, 21)
        Me.LabelTypeCbx.TabIndex = 142
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(47, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(80, 17)
        Me.Label1.TabIndex = 141
        Me.Label1.Text = "Label Type:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'IngredientsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 282)
        Me.Controls.Add(Me.CancelBtn)
        Me.Controls.Add(Me.SaveChangesBtn)
        Me.Controls.Add(Me.grpDetails)
        Me.Name = "IngredientsForm"
        Me.Text = "Ingredients"
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents DescriptionTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents IngredientsTxt As System.Windows.Forms.TextBox
    Friend WithEvents SaveChangesBtn As System.Windows.Forms.Button
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents LabelTypeCbx As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
End Class
