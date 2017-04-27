<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultipleValueInput
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
        Me.LabelHelp = New System.Windows.Forms.Label()
        Me.TextBoxInput = New System.Windows.Forms.TextBox()
        Me.RefreshItemsBtn = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelHelp
        '
        Me.LabelHelp.AutoSize = True
        Me.LabelHelp.Location = New System.Drawing.Point(8, 433)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(203, 13)
        Me.LabelHelp.TabIndex = 10
        Me.LabelHelp.Text = "Enter values one per line (maximum 1000)"
        '
        'TextBoxInput
        '
        Me.TextBoxInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxInput.Location = New System.Drawing.Point(11, 27)
        Me.TextBoxInput.Multiline = True
        Me.TextBoxInput.Name = "TextBoxInput"
        Me.TextBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxInput.Size = New System.Drawing.Size(360, 395)
        Me.TextBoxInput.TabIndex = 9
        '
        'RefreshItemsBtn
        '
        Me.RefreshItemsBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshItemsBtn.Location = New System.Drawing.Point(300, 428)
        Me.RefreshItemsBtn.Name = "RefreshItemsBtn"
        Me.RefreshItemsBtn.Size = New System.Drawing.Size(71, 23)
        Me.RefreshItemsBtn.TabIndex = 8
        Me.RefreshItemsBtn.Text = "OK"
        Me.RefreshItemsBtn.UseVisualStyleBackColor = True
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClear.Location = New System.Drawing.Point(223, 428)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(71, 23)
        Me.ButtonClear.TabIndex = 11
        Me.ButtonClear.Text = "Clear"
        Me.ButtonClear.UseVisualStyleBackColor = True
        '
        'MultipleValueInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 476)
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.LabelHelp)
        Me.Controls.Add(Me.TextBoxInput)
        Me.Controls.Add(Me.RefreshItemsBtn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MultipleValueInput"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelHelp As Label
    Friend WithEvents TextBoxInput As TextBox
    Friend WithEvents RefreshItemsBtn As Button
    Friend WithEvents ButtonClear As Button
End Class
