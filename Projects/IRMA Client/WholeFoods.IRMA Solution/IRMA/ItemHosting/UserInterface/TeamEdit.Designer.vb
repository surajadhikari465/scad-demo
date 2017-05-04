<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TeamEdit
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox_TeamNumber = New System.Windows.Forms.MaskedTextBox
        Me.TextBox_TeamName = New System.Windows.Forms.TextBox
        Me.TextBox_TeamAbbrev = New System.Windows.Forms.TextBox
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(54, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Team Number:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(65, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Team Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(26, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(117, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Team Abbreviation:"
        '
        'TextBox_TeamNumber
        '
        Me.TextBox_TeamNumber.Location = New System.Drawing.Point(149, 19)
        Me.TextBox_TeamNumber.Mask = "000"
        Me.TextBox_TeamNumber.Name = "TextBox_TeamNumber"
        Me.TextBox_TeamNumber.Size = New System.Drawing.Size(34, 20)
        Me.TextBox_TeamNumber.TabIndex = 3
        '
        'TextBox_TeamName
        '
        Me.TextBox_TeamName.Location = New System.Drawing.Point(149, 41)
        Me.TextBox_TeamName.MaxLength = 100
        Me.TextBox_TeamName.Name = "TextBox_TeamName"
        Me.TextBox_TeamName.Size = New System.Drawing.Size(189, 20)
        Me.TextBox_TeamName.TabIndex = 4
        '
        'TextBox_TeamAbbrev
        '
        Me.TextBox_TeamAbbrev.Location = New System.Drawing.Point(149, 67)
        Me.TextBox_TeamAbbrev.MaxLength = 10
        Me.TextBox_TeamAbbrev.Name = "TextBox_TeamAbbrev"
        Me.TextBox_TeamAbbrev.Size = New System.Drawing.Size(189, 20)
        Me.TextBox_TeamAbbrev.TabIndex = 5
        '
        'Button_Save
        '
        Me.Button_Save.AutoSize = True
        Me.Button_Save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_Save.Location = New System.Drawing.Point(313, 110)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(42, 23)
        Me.Button_Save.TabIndex = 6
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.AutoSize = True
        Me.Button_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_Cancel.CausesValidation = False
        Me.Button_Cancel.Location = New System.Drawing.Point(361, 110)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(50, 23)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'TeamEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(423, 145)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.TextBox_TeamAbbrev)
        Me.Controls.Add(Me.TextBox_TeamName)
        Me.Controls.Add(Me.TextBox_TeamNumber)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "TeamEdit"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "TeamEdit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox_TeamNumber As System.Windows.Forms.MaskedTextBox
    Friend WithEvents TextBox_TeamName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_TeamAbbrev As System.Windows.Forms.TextBox
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
End Class
