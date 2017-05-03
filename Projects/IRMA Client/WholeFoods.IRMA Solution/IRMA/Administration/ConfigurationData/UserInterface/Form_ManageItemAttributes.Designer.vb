<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageItemAttributes
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
        Me.Label_Available_Attribute_Fields = New System.Windows.Forms.Label
        Me.Combo_Available_Attribute_Fields = New System.Windows.Forms.ComboBox
        Me.Label_Screen_Text = New System.Windows.Forms.Label
        Me.Text_Screen_Text = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Number_Max_Width = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.Label_Default_Value = New System.Windows.Forms.Label
        Me.Text_Default_Value = New System.Windows.Forms.TextBox
        Me.Button_Create_Attribute = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Combo_Field_Types = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Text_Field_Values = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        CType(Me.Number_Max_Width, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_Available_Attribute_Fields
        '
        Me.Label_Available_Attribute_Fields.AutoSize = True
        Me.Label_Available_Attribute_Fields.Location = New System.Drawing.Point(37, 31)
        Me.Label_Available_Attribute_Fields.Name = "Label_Available_Attribute_Fields"
        Me.Label_Available_Attribute_Fields.Size = New System.Drawing.Size(135, 13)
        Me.Label_Available_Attribute_Fields.TabIndex = 0
        Me.Label_Available_Attribute_Fields.Text = "Available Attribute Fields"
        '
        'Combo_Available_Attribute_Fields
        '
        Me.Combo_Available_Attribute_Fields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_Available_Attribute_Fields.FormattingEnabled = True
        Me.Combo_Available_Attribute_Fields.Location = New System.Drawing.Point(248, 31)
        Me.Combo_Available_Attribute_Fields.Name = "Combo_Available_Attribute_Fields"
        Me.Combo_Available_Attribute_Fields.Size = New System.Drawing.Size(121, 21)
        Me.Combo_Available_Attribute_Fields.TabIndex = 1
        '
        'Label_Screen_Text
        '
        Me.Label_Screen_Text.AutoSize = True
        Me.Label_Screen_Text.Location = New System.Drawing.Point(37, 86)
        Me.Label_Screen_Text.Name = "Label_Screen_Text"
        Me.Label_Screen_Text.Size = New System.Drawing.Size(152, 13)
        Me.Label_Screen_Text.TabIndex = 2
        Me.Label_Screen_Text.Text = "Screen Text / Attribute Name"
        '
        'Text_Screen_Text
        '
        Me.Text_Screen_Text.Location = New System.Drawing.Point(248, 79)
        Me.Text_Screen_Text.Name = "Text_Screen_Text"
        Me.Text_Screen_Text.Size = New System.Drawing.Size(121, 22)
        Me.Text_Screen_Text.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(37, 174)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Max Width"
        '
        'Number_Max_Width
        '
        Me.Number_Max_Width.Location = New System.Drawing.Point(248, 166)
        Me.Number_Max_Width.Name = "Number_Max_Width"
        Me.Number_Max_Width.Size = New System.Drawing.Size(43, 24)
        Me.Number_Max_Width.TabIndex = 6
        '
        'Label_Default_Value
        '
        Me.Label_Default_Value.AutoSize = True
        Me.Label_Default_Value.Location = New System.Drawing.Point(37, 219)
        Me.Label_Default_Value.Name = "Label_Default_Value"
        Me.Label_Default_Value.Size = New System.Drawing.Size(77, 13)
        Me.Label_Default_Value.TabIndex = 7
        Me.Label_Default_Value.Text = "Default Value"
        '
        'Text_Default_Value
        '
        Me.Text_Default_Value.Location = New System.Drawing.Point(248, 212)
        Me.Text_Default_Value.Name = "Text_Default_Value"
        Me.Text_Default_Value.Size = New System.Drawing.Size(100, 22)
        Me.Text_Default_Value.TabIndex = 8
        '
        'Button_Create_Attribute
        '
        Me.Button_Create_Attribute.Location = New System.Drawing.Point(248, 304)
        Me.Button_Create_Attribute.Name = "Button_Create_Attribute"
        Me.Button_Create_Attribute.Size = New System.Drawing.Size(121, 23)
        Me.Button_Create_Attribute.TabIndex = 11
        Me.Button_Create_Attribute.Text = "Create Attribute"
        Me.Button_Create_Attribute.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(248, 346)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(121, 23)
        Me.Button_Cancel.TabIndex = 12
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Combo_Field_Types
        '
        Me.Combo_Field_Types.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_Field_Types.FormattingEnabled = True
        Me.Combo_Field_Types.Location = New System.Drawing.Point(248, 121)
        Me.Combo_Field_Types.Name = "Combo_Field_Types"
        Me.Combo_Field_Types.Size = New System.Drawing.Size(121, 21)
        Me.Combo_Field_Types.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(37, 129)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Field Type"
        '
        'Text_Field_Values
        '
        Me.Text_Field_Values.Location = New System.Drawing.Point(248, 258)
        Me.Text_Field_Values.Name = "Text_Field_Values"
        Me.Text_Field_Values.Size = New System.Drawing.Size(100, 22)
        Me.Text_Field_Values.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(40, 264)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Field Values"
        '
        'Form_ManageItemAttributes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 392)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Text_Field_Values)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Combo_Field_Types)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Create_Attribute)
        Me.Controls.Add(Me.Text_Default_Value)
        Me.Controls.Add(Me.Label_Default_Value)
        Me.Controls.Add(Me.Number_Max_Width)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Text_Screen_Text)
        Me.Controls.Add(Me.Label_Screen_Text)
        Me.Controls.Add(Me.Combo_Available_Attribute_Fields)
        Me.Controls.Add(Me.Label_Available_Attribute_Fields)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManageItemAttributes"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create Item Attributes"
        CType(Me.Number_Max_Width, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_Available_Attribute_Fields As System.Windows.Forms.Label
    Friend WithEvents Combo_Available_Attribute_Fields As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Screen_Text As System.Windows.Forms.Label
    Friend WithEvents Text_Screen_Text As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Number_Max_Width As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents Label_Default_Value As System.Windows.Forms.Label
    Friend WithEvents Text_Default_Value As System.Windows.Forms.TextBox
    Friend WithEvents Button_Create_Attribute As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Combo_Field_Types As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Text_Field_Values As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
