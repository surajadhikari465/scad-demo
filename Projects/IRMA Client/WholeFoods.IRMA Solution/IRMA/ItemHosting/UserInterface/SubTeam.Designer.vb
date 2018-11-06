<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SubTeam
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
        Me.Button_Close = New System.Windows.Forms.Button
        Me.Button_AddSubTeam = New System.Windows.Forms.Button
        Me.Button_EditSubTeam = New System.Windows.Forms.Button
        Me.ComboBox_SubTeams = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Sub Team:"
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.AutoSize = True
        Me.Button_Close.Location = New System.Drawing.Point(293, 164)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(61, 28)
        Me.Button_Close.TabIndex = 8
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Button_AddSubTeam
        '
        Me.Button_AddSubTeam.AutoSize = True
        Me.Button_AddSubTeam.Location = New System.Drawing.Point(12, 12)
        Me.Button_AddSubTeam.Name = "Button_AddSubTeam"
        Me.Button_AddSubTeam.Size = New System.Drawing.Size(89, 28)
        Me.Button_AddSubTeam.TabIndex = 7
        Me.Button_AddSubTeam.Text = "New SubTeam"
        Me.Button_AddSubTeam.UseVisualStyleBackColor = True
        '
        'Button_EditSubTeam
        '
        Me.Button_EditSubTeam.AutoSize = True
        Me.Button_EditSubTeam.Location = New System.Drawing.Point(253, 58)
        Me.Button_EditSubTeam.Name = "Button_EditSubTeam"
        Me.Button_EditSubTeam.Size = New System.Drawing.Size(89, 28)
        Me.Button_EditSubTeam.TabIndex = 6
        Me.Button_EditSubTeam.Text = "Edit Sub Team"
        Me.Button_EditSubTeam.UseVisualStyleBackColor = True
        '
        'ComboBox_SubTeams
        '
        Me.ComboBox_SubTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SubTeams.FormattingEnabled = True
        Me.ComboBox_SubTeams.Location = New System.Drawing.Point(87, 31)
        Me.ComboBox_SubTeams.Name = "ComboBox_SubTeams"
        Me.ComboBox_SubTeams.Size = New System.Drawing.Size(255, 21)
        Me.ComboBox_SubTeams.TabIndex = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ComboBox_SubTeams)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Button_EditSubTeam)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 60)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(349, 98)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Edit Existing"
        '
        'SubTeam
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(373, 201)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Button_AddSubTeam)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SubTeam"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Sub Teams"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Button_AddSubTeam As System.Windows.Forms.Button
    Friend WithEvents Button_EditSubTeam As System.Windows.Forms.Button
    Friend WithEvents ComboBox_SubTeams As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
