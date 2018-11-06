<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DeleteUser
    Inherits Form_IRMADelete

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
        Me.Label_FullNameVal = New System.Windows.Forms.Label
        Me.Label_UserNameVal = New System.Windows.Forms.Label
        Me.Label_FullName = New System.Windows.Forms.Label
        Me.Label_UserName = New System.Windows.Forms.Label
        Me.Panel_StandardButtons.SuspendLayout()
        Me.Panel_Instructions.SuspendLayout()
        Me.GroupBox_DeleteData.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_StandardButtons
        '
        Me.Panel_StandardButtons.Location = New System.Drawing.Point(0, 118)
        '
        'Button_Delete
        '
        '
        'Label_Warning
        '
        Me.Label_Warning.Location = New System.Drawing.Point(12, 9)
        Me.Label_Warning.Size = New System.Drawing.Size(386, 17)
        Me.Label_Warning.Text = "Warning!  You are about to disable a User account. "
        '
        'GroupBox_DeleteData
        '
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FullNameVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_UserNameVal)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_FullName)
        Me.GroupBox_DeleteData.Controls.Add(Me.Label_UserName)
        Me.GroupBox_DeleteData.Size = New System.Drawing.Size(550, 70)
        '
        'Label_FullNameVal
        '
        Me.Label_FullNameVal.AutoSize = True
        Me.Label_FullNameVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FullNameVal.Location = New System.Drawing.Point(179, 47)
        Me.Label_FullNameVal.Name = "Label_FullNameVal"
        Me.Label_FullNameVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_FullNameVal.TabIndex = 17
        Me.Label_FullNameVal.Text = "Label1"
        '
        'Label_UserNameVal
        '
        Me.Label_UserNameVal.AutoSize = True
        Me.Label_UserNameVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_UserNameVal.Location = New System.Drawing.Point(179, 16)
        Me.Label_UserNameVal.Name = "Label_UserNameVal"
        Me.Label_UserNameVal.Size = New System.Drawing.Size(45, 13)
        Me.Label_UserNameVal.TabIndex = 16
        Me.Label_UserNameVal.Text = "Label1"
        '
        'Label_FullName
        '
        Me.Label_FullName.AutoSize = True
        Me.Label_FullName.Location = New System.Drawing.Point(44, 47)
        Me.Label_FullName.Name = "Label_FullName"
        Me.Label_FullName.Size = New System.Drawing.Size(57, 13)
        Me.Label_FullName.TabIndex = 15
        Me.Label_FullName.Text = "Full Name:"
        '
        'Label_UserName
        '
        Me.Label_UserName.AutoSize = True
        Me.Label_UserName.Location = New System.Drawing.Point(44, 16)
        Me.Label_UserName.Name = "Label_UserName"
        Me.Label_UserName.Size = New System.Drawing.Size(63, 13)
        Me.Label_UserName.TabIndex = 13
        Me.Label_UserName.Text = "User Name:"
        '
        'Form_DeleteUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(576, 168)
        Me.Name = "Form_DeleteUser"
        Me.ShowInTaskbar = False
        Me.Text = "Disable User"
        Me.Panel_StandardButtons.ResumeLayout(False)
        Me.Panel_Instructions.ResumeLayout(False)
        Me.Panel_Instructions.PerformLayout()
        Me.GroupBox_DeleteData.ResumeLayout(False)
        Me.GroupBox_DeleteData.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_FullNameVal As System.Windows.Forms.Label
    Friend WithEvents Label_UserNameVal As System.Windows.Forms.Label
    Friend WithEvents Label_FullName As System.Windows.Forms.Label
    Friend WithEvents Label_UserName As System.Windows.Forms.Label

End Class
