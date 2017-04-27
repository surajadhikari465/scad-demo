<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditRegionalSettings
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
        Me.Label_CorpWriter = New System.Windows.Forms.Label
        Me.Label_ZoneWriter = New System.Windows.Forms.Label
        Me.GroupBox_RegionalSettings = New System.Windows.Forms.GroupBox
        Me.ComboBox_ZoneWriter = New System.Windows.Forms.ComboBox
        Me.ComboBox_CorpWriter = New System.Windows.Forms.ComboBox
        Me.CheckBox_RegionalScale = New System.Windows.Forms.CheckBox
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox_RegionalSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_CorpWriter
        '
        Me.Label_CorpWriter.AutoSize = True
        Me.Label_CorpWriter.Location = New System.Drawing.Point(22, 50)
        Me.Label_CorpWriter.Name = "Label_CorpWriter"
        Me.Label_CorpWriter.Size = New System.Drawing.Size(126, 13)
        Me.Label_CorpWriter.TabIndex = 3
        Me.Label_CorpWriter.Text = "Corporate Scale Writer:"
        '
        'Label_ZoneWriter
        '
        Me.Label_ZoneWriter.AutoSize = True
        Me.Label_ZoneWriter.Location = New System.Drawing.Point(22, 77)
        Me.Label_ZoneWriter.Name = "Label_ZoneWriter"
        Me.Label_ZoneWriter.Size = New System.Drawing.Size(100, 13)
        Me.Label_ZoneWriter.TabIndex = 4
        Me.Label_ZoneWriter.Text = "Zone Scale Writer:"
        '
        'GroupBox_RegionalSettings
        '
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_ZoneWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.Label_CorpWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.ComboBox_ZoneWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.ComboBox_CorpWriter)
        Me.GroupBox_RegionalSettings.Controls.Add(Me.CheckBox_RegionalScale)
        Me.GroupBox_RegionalSettings.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_RegionalSettings.Name = "GroupBox_RegionalSettings"
        Me.GroupBox_RegionalSettings.Size = New System.Drawing.Size(337, 114)
        Me.GroupBox_RegionalSettings.TabIndex = 17
        Me.GroupBox_RegionalSettings.TabStop = False
        Me.GroupBox_RegionalSettings.Text = "Regional Settings"
        '
        'ComboBox_ZoneWriter
        '
        Me.ComboBox_ZoneWriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ZoneWriter.FormattingEnabled = True
        Me.ComboBox_ZoneWriter.Location = New System.Drawing.Point(154, 74)
        Me.ComboBox_ZoneWriter.Name = "ComboBox_ZoneWriter"
        Me.ComboBox_ZoneWriter.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox_ZoneWriter.TabIndex = 2
        '
        'ComboBox_CorpWriter
        '
        Me.ComboBox_CorpWriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_CorpWriter.FormattingEnabled = True
        Me.ComboBox_CorpWriter.Location = New System.Drawing.Point(154, 47)
        Me.ComboBox_CorpWriter.Name = "ComboBox_CorpWriter"
        Me.ComboBox_CorpWriter.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox_CorpWriter.TabIndex = 1
        '
        'CheckBox_RegionalScale
        '
        Me.CheckBox_RegionalScale.AutoSize = True
        Me.CheckBox_RegionalScale.Location = New System.Drawing.Point(25, 24)
        Me.CheckBox_RegionalScale.Name = "CheckBox_RegionalScale"
        Me.CheckBox_RegionalScale.Size = New System.Drawing.Size(193, 17)
        Me.CheckBox_RegionalScale.TabIndex = 0
        Me.CheckBox_RegionalScale.Text = "Use Regional Scale Hosting Files"
        Me.CheckBox_RegionalScale.UseVisualStyleBackColor = True
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(268, 132)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(75, 23)
        Me.Button_Save.TabIndex = 19
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(187, 132)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 18
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_EditRegionalSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 165)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox_RegionalSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_EditRegionalSettings"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Regional Settings"
        Me.GroupBox_RegionalSettings.ResumeLayout(False)
        Me.GroupBox_RegionalSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_CorpWriter As System.Windows.Forms.Label
    Friend WithEvents Label_ZoneWriter As System.Windows.Forms.Label
    Friend WithEvents GroupBox_RegionalSettings As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_ZoneWriter As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_CorpWriter As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox_RegionalScale As System.Windows.Forms.CheckBox
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
End Class
