<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FailedRequestsDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FailedRequestsDialog))
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.RichTextBoxFullError = New System.Windows.Forms.RichTextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TextBoxShortMessage = New System.Windows.Forms.TextBox()
        Me.TextBoxError = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_Button.Location = New System.Drawing.Point(505, 418)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'RichTextBoxFullError
        '
        Me.RichTextBoxFullError.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxFullError.Location = New System.Drawing.Point(12, 150)
        Me.RichTextBoxFullError.Name = "RichTextBoxFullError"
        Me.RichTextBoxFullError.ReadOnly = True
        Me.RichTextBoxFullError.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBoxFullError.Size = New System.Drawing.Size(560, 262)
        Me.RichTextBoxFullError.TabIndex = 1
        Me.RichTextBoxFullError.Text = ""
        '
        'TextBoxShortMessage
        '
        Me.TextBoxShortMessage.BackColor = System.Drawing.SystemColors.Control
        Me.TextBoxShortMessage.Location = New System.Drawing.Point(12, 12)
        Me.TextBoxShortMessage.Multiline = True
        Me.TextBoxShortMessage.Name = "TextBoxShortMessage"
        Me.TextBoxShortMessage.Size = New System.Drawing.Size(560, 40)
        Me.TextBoxShortMessage.TabIndex = 2
        '
        'TextBoxError
        '
        Me.TextBoxError.BackColor = System.Drawing.SystemColors.Control
        Me.TextBoxError.Location = New System.Drawing.Point(12, 58)
        Me.TextBoxError.Multiline = True
        Me.TextBoxError.Name = "TextBoxError"
        Me.TextBoxError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxError.Size = New System.Drawing.Size(560, 86)
        Me.TextBoxError.TabIndex = 3
        '
        'FailedRequestsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(584, 451)
        Me.Controls.Add(Me.TextBoxError)
        Me.Controls.Add(Me.TextBoxShortMessage)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.RichTextBoxFullError)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 600)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 230)
        Me.Name = "FailedRequestsDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print Batch Request"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents RichTextBoxFullError As System.Windows.Forms.RichTextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TextBoxShortMessage As TextBox
    Friend WithEvents TextBoxError As TextBox
End Class
