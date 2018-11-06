<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_RegionalInstanceData
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
        Me.GroupBox_NumScaleDigits = New System.Windows.Forms.GroupBox
        Me.RadioButton_VariableByItem = New System.Windows.Forms.RadioButton
        Me.RadioButton_Always5 = New System.Windows.Forms.RadioButton
        Me.RadioButton_Always4 = New System.Windows.Forms.RadioButton
        Me.Label_RegionName = New System.Windows.Forms.Label
        Me.TextBox_RegionName = New System.Windows.Forms.TextBox
        Me.TextBox_RegionAbbr = New System.Windows.Forms.TextBox
        Me.TextBox_UGCulture = New System.Windows.Forms.TextBox
        Me.TextBox_UGDateMask = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox_RegionInfo = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox_NumScaleDigits.SuspendLayout()
        Me.GroupBox_RegionInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_NumScaleDigits
        '
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_VariableByItem)
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_Always5)
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_Always4)
        Me.GroupBox_NumScaleDigits.Location = New System.Drawing.Point(44, 239)
        Me.GroupBox_NumScaleDigits.Name = "GroupBox_NumScaleDigits"
        Me.GroupBox_NumScaleDigits.Size = New System.Drawing.Size(254, 111)
        Me.GroupBox_NumScaleDigits.TabIndex = 3
        Me.GroupBox_NumScaleDigits.TabStop = False
        Me.GroupBox_NumScaleDigits.Text = "# Digits to send to Scales"
        '
        'RadioButton_VariableByItem
        '
        Me.RadioButton_VariableByItem.AutoSize = True
        Me.RadioButton_VariableByItem.Checked = True
        Me.RadioButton_VariableByItem.Location = New System.Drawing.Point(38, 72)
        Me.RadioButton_VariableByItem.Name = "RadioButton_VariableByItem"
        Me.RadioButton_VariableByItem.Size = New System.Drawing.Size(107, 17)
        Me.RadioButton_VariableByItem.TabIndex = 6
        Me.RadioButton_VariableByItem.TabStop = True
        Me.RadioButton_VariableByItem.Text = "Variable By Item"
        Me.RadioButton_VariableByItem.UseVisualStyleBackColor = True
        '
        'RadioButton_Always5
        '
        Me.RadioButton_Always5.AutoSize = True
        Me.RadioButton_Always5.Location = New System.Drawing.Point(38, 49)
        Me.RadioButton_Always5.Name = "RadioButton_Always5"
        Me.RadioButton_Always5.Size = New System.Drawing.Size(69, 17)
        Me.RadioButton_Always5.TabIndex = 5
        Me.RadioButton_Always5.Text = "Always 5"
        Me.RadioButton_Always5.UseVisualStyleBackColor = True
        '
        'RadioButton_Always4
        '
        Me.RadioButton_Always4.AutoSize = True
        Me.RadioButton_Always4.Location = New System.Drawing.Point(38, 26)
        Me.RadioButton_Always4.Name = "RadioButton_Always4"
        Me.RadioButton_Always4.Size = New System.Drawing.Size(69, 17)
        Me.RadioButton_Always4.TabIndex = 4
        Me.RadioButton_Always4.Text = "Always 4"
        Me.RadioButton_Always4.UseVisualStyleBackColor = True
        '
        'Label_RegionName
        '
        Me.Label_RegionName.AutoSize = True
        Me.Label_RegionName.Location = New System.Drawing.Point(10, 27)
        Me.Label_RegionName.Name = "Label_RegionName"
        Me.Label_RegionName.Size = New System.Drawing.Size(76, 13)
        Me.Label_RegionName.TabIndex = 1
        Me.Label_RegionName.Text = "Region Name"
        '
        'TextBox_RegionName
        '
        Me.TextBox_RegionName.Location = New System.Drawing.Point(101, 24)
        Me.TextBox_RegionName.MaxLength = 20
        Me.TextBox_RegionName.Name = "TextBox_RegionName"
        Me.TextBox_RegionName.ReadOnly = True
        Me.TextBox_RegionName.Size = New System.Drawing.Size(127, 22)
        Me.TextBox_RegionName.TabIndex = 1
        Me.TextBox_RegionName.TabStop = False
        '
        'TextBox_RegionAbbr
        '
        Me.TextBox_RegionAbbr.Location = New System.Drawing.Point(101, 59)
        Me.TextBox_RegionAbbr.MaxLength = 2
        Me.TextBox_RegionAbbr.Name = "TextBox_RegionAbbr"
        Me.TextBox_RegionAbbr.ReadOnly = True
        Me.TextBox_RegionAbbr.Size = New System.Drawing.Size(46, 22)
        Me.TextBox_RegionAbbr.TabIndex = 2
        Me.TextBox_RegionAbbr.TabStop = False
        '
        'TextBox_UGCulture
        '
        Me.TextBox_UGCulture.Location = New System.Drawing.Point(101, 97)
        Me.TextBox_UGCulture.MaxLength = 5
        Me.TextBox_UGCulture.Name = "TextBox_UGCulture"
        Me.TextBox_UGCulture.ReadOnly = True
        Me.TextBox_UGCulture.Size = New System.Drawing.Size(127, 22)
        Me.TextBox_UGCulture.TabIndex = 3
        Me.TextBox_UGCulture.TabStop = False
        '
        'TextBox_UGDateMask
        '
        Me.TextBox_UGDateMask.Location = New System.Drawing.Point(101, 125)
        Me.TextBox_UGDateMask.MaxLength = 10
        Me.TextBox_UGDateMask.Name = "TextBox_UGDateMask"
        Me.TextBox_UGDateMask.ReadOnly = True
        Me.TextBox_UGDateMask.Size = New System.Drawing.Size(127, 22)
        Me.TextBox_UGDateMask.TabIndex = 4
        Me.TextBox_UGDateMask.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Region Abbr"
        '
        'GroupBox_RegionInfo
        '
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label5)
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label4)
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label2)
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label3)
        Me.GroupBox_RegionInfo.Controls.Add(Me.TextBox2)
        Me.GroupBox_RegionInfo.Controls.Add(Me.TextBox_RegionAbbr)
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label_RegionName)
        Me.GroupBox_RegionInfo.Controls.Add(Me.Label1)
        Me.GroupBox_RegionInfo.Controls.Add(Me.TextBox_RegionName)
        Me.GroupBox_RegionInfo.Controls.Add(Me.TextBox_UGCulture)
        Me.GroupBox_RegionInfo.Controls.Add(Me.TextBox_UGDateMask)
        Me.GroupBox_RegionInfo.Location = New System.Drawing.Point(44, 32)
        Me.GroupBox_RegionInfo.Name = "GroupBox_RegionInfo"
        Me.GroupBox_RegionInfo.Size = New System.Drawing.Size(254, 158)
        Me.GroupBox_RegionInfo.TabIndex = 0
        Me.GroupBox_RegionInfo.TabStop = False
        Me.GroupBox_RegionInfo.Text = "Region Info"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 23)
        Me.Label2.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 23)
        Me.Label3.TabIndex = 2
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(0, 0)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 22)
        Me.TextBox2.TabIndex = 3
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(223, 367)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 7
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "UG Culture"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "UG Date Mask"
        '
        'Form_RegionalInstanceData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 402)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox_RegionInfo)
        Me.Controls.Add(Me.GroupBox_NumScaleDigits)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_RegionalInstanceData"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Regional Instance Data"
        Me.GroupBox_NumScaleDigits.ResumeLayout(False)
        Me.GroupBox_NumScaleDigits.PerformLayout()
        Me.GroupBox_RegionInfo.ResumeLayout(False)
        Me.GroupBox_RegionInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_NumScaleDigits As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_VariableByItem As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Always5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Always4 As System.Windows.Forms.RadioButton
    Friend WithEvents Label_RegionName As System.Windows.Forms.Label
    Friend WithEvents TextBox_RegionName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_RegionAbbr As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UGCulture As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UGDateMask As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox_RegionInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
