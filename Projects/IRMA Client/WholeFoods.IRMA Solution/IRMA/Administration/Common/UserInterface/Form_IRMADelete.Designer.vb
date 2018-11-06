<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_IRMADelete
    Inherits Form_IRMABase

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
        Me.Panel_StandardButtons = New System.Windows.Forms.Panel
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Label_Warning = New System.Windows.Forms.Label
        Me.Panel_Instructions = New System.Windows.Forms.Panel
        Me.GroupBox_DeleteData = New System.Windows.Forms.GroupBox
        Me.Panel_StandardButtons.SuspendLayout()
        Me.Panel_Instructions.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_StandardButtons
        '
        Me.Panel_StandardButtons.Controls.Add(Me.Button_Delete)
        Me.Panel_StandardButtons.Controls.Add(Me.Button_Cancel)
        Me.Panel_StandardButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_StandardButtons.Location = New System.Drawing.Point(0, 216)
        Me.Panel_StandardButtons.Name = "Panel_StandardButtons"
        Me.Panel_StandardButtons.Size = New System.Drawing.Size(576, 50)
        Me.Panel_StandardButtons.TabIndex = 4
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.Location = New System.Drawing.Point(489, 15)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 2
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Location = New System.Drawing.Point(389, 15)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Label_Warning
        '
        Me.Label_Warning.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Warning.AutoEllipsis = True
        Me.Label_Warning.AutoSize = True
        Me.Label_Warning.CausesValidation = False
        Me.Label_Warning.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Warning.ForeColor = System.Drawing.Color.Red
        Me.Label_Warning.Location = New System.Drawing.Point(93, 9)
        Me.Label_Warning.Name = "Label_Warning"
        Me.Label_Warning.Size = New System.Drawing.Size(407, 17)
        Me.Label_Warning.TabIndex = 0
        Me.Label_Warning.Text = "Warning!  You are about to delete the following record."
        Me.Label_Warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel_Instructions
        '
        Me.Panel_Instructions.Controls.Add(Me.Label_Warning)
        Me.Panel_Instructions.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_Instructions.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Instructions.Name = "Panel_Instructions"
        Me.Panel_Instructions.Size = New System.Drawing.Size(576, 42)
        Me.Panel_Instructions.TabIndex = 3
        '
        'GroupBox_DeleteData
        '
        Me.GroupBox_DeleteData.Location = New System.Drawing.Point(12, 48)
        Me.GroupBox_DeleteData.Name = "GroupBox_DeleteData"
        Me.GroupBox_DeleteData.Size = New System.Drawing.Size(550, 146)
        Me.GroupBox_DeleteData.TabIndex = 6
        Me.GroupBox_DeleteData.TabStop = False
        '
        'Form_IRMADelete2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(576, 266)
        Me.Controls.Add(Me.GroupBox_DeleteData)
        Me.Controls.Add(Me.Panel_StandardButtons)
        Me.Controls.Add(Me.Panel_Instructions)
        Me.Name = "Form_IRMADelete2"
        Me.Panel_StandardButtons.ResumeLayout(False)
        Me.Panel_Instructions.ResumeLayout(False)
        Me.Panel_Instructions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Protected WithEvents Panel_StandardButtons As System.Windows.Forms.Panel
    Protected WithEvents Button_Delete As System.Windows.Forms.Button
    Protected WithEvents Button_Cancel As System.Windows.Forms.Button
    Protected WithEvents Label_Warning As System.Windows.Forms.Label
    Protected WithEvents Panel_Instructions As System.Windows.Forms.Panel
    Protected WithEvents GroupBox_DeleteData As System.Windows.Forms.GroupBox

End Class
