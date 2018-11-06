<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ErrorDialog
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ErrorDialog))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.RichTextBoxFullError = New System.Windows.Forms.RichTextBox
        Me.TextBoxShortMessage = New System.Windows.Forms.TextBox
        Me.LabelEmailHasBeenSent = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lnkDetailToggle = New System.Windows.Forms.LinkLabel
        Me.ButtonCopyToClipboard = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_Button.Location = New System.Drawing.Point(515, 528)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 36)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'RichTextBoxFullError
        '
        Me.RichTextBoxFullError.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxFullError.Location = New System.Drawing.Point(12, 157)
        Me.RichTextBoxFullError.Name = "RichTextBoxFullError"
        Me.RichTextBoxFullError.ReadOnly = True
        Me.RichTextBoxFullError.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBoxFullError.Size = New System.Drawing.Size(570, 365)
        Me.RichTextBoxFullError.TabIndex = 1
        Me.RichTextBoxFullError.Text = ""
        Me.RichTextBoxFullError.Visible = False
        '
        'TextBoxShortMessage
        '
        Me.TextBoxShortMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxShortMessage.BackColor = System.Drawing.SystemColors.Control
        Me.TextBoxShortMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxShortMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxShortMessage.ForeColor = System.Drawing.Color.Red
        Me.TextBoxShortMessage.Location = New System.Drawing.Point(57, 21)
        Me.TextBoxShortMessage.MaximumSize = New System.Drawing.Size(525, 125)
        Me.TextBoxShortMessage.MinimumSize = New System.Drawing.Size(525, 104)
        Me.TextBoxShortMessage.Multiline = True
        Me.TextBoxShortMessage.Name = "TextBoxShortMessage"
        Me.TextBoxShortMessage.ReadOnly = True
        Me.TextBoxShortMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxShortMessage.Size = New System.Drawing.Size(525, 104)
        Me.TextBoxShortMessage.TabIndex = 2
        Me.TextBoxShortMessage.TabStop = False
        '
        'LabelEmailHasBeenSent
        '
        Me.LabelEmailHasBeenSent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelEmailHasBeenSent.AutoEllipsis = True
        Me.LabelEmailHasBeenSent.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmailHasBeenSent.Location = New System.Drawing.Point(57, 528)
        Me.LabelEmailHasBeenSent.Name = "LabelEmailHasBeenSent"
        Me.LabelEmailHasBeenSent.Size = New System.Drawing.Size(449, 36)
        Me.LabelEmailHasBeenSent.TabIndex = 4
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox1.ErrorImage = CType(resources.GetObject("PictureBox1.ErrorImage"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(39, 36)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'lnkDetailToggle
        '
        Me.lnkDetailToggle.AutoSize = True
        Me.lnkDetailToggle.Location = New System.Drawing.Point(12, 140)
        Me.lnkDetailToggle.Name = "lnkDetailToggle"
        Me.lnkDetailToggle.Size = New System.Drawing.Size(127, 13)
        Me.lnkDetailToggle.TabIndex = 7
        Me.lnkDetailToggle.TabStop = True
        Me.lnkDetailToggle.Text = "Show Exception Details"
        '
        'ButtonCopyToClipboard
        '
        Me.ButtonCopyToClipboard.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonCopyToClipboard.Image = CType(resources.GetObject("ButtonCopyToClipboard.Image"), System.Drawing.Image)
        Me.ButtonCopyToClipboard.Location = New System.Drawing.Point(12, 528)
        Me.ButtonCopyToClipboard.Name = "ButtonCopyToClipboard"
        Me.ButtonCopyToClipboard.Size = New System.Drawing.Size(39, 36)
        Me.ButtonCopyToClipboard.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.ButtonCopyToClipboard, "Copy Details to Clipboard")
        Me.ButtonCopyToClipboard.UseVisualStyleBackColor = True
        '
        'ErrorDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(594, 568)
        Me.Controls.Add(Me.TextBoxShortMessage)
        Me.Controls.Add(Me.ButtonCopyToClipboard)
        Me.Controls.Add(Me.lnkDetailToggle)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.LabelEmailHasBeenSent)
        Me.Controls.Add(Me.RichTextBoxFullError)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 600)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 230)
        Me.Name = "ErrorDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "An error has courred."
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents RichTextBoxFullError As System.Windows.Forms.RichTextBox
    Friend WithEvents TextBoxShortMessage As System.Windows.Forms.TextBox
    Friend WithEvents LabelEmailHasBeenSent As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lnkDetailToggle As System.Windows.Forms.LinkLabel
    Friend WithEvents ButtonCopyToClipboard As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
