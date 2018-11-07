<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApproveInvoice
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ApproveInvoice))
        Me.Button_Approve = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_ApprovalOptions = New System.Windows.Forms.GroupBox()
        Me.RichTextBox_PayInvoice = New System.Windows.Forms.RichTextBox()
        Me.RichTextBox_PayPO = New System.Windows.Forms.RichTextBox()
        Me.RadioButton_PayInvoice = New System.Windows.Forms.RadioButton()
        Me.RadioButton_PayPO = New System.Windows.Forms.RadioButton()
        Me.GroupBox_ApprovalOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Approve
        '
        Me.Button_Approve.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Approve.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Approve.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Approve.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Approve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Approve.Image = CType(resources.GetObject("Button_Approve.Image"), System.Drawing.Image)
        Me.Button_Approve.Location = New System.Drawing.Point(310, 193)
        Me.Button_Approve.Name = "Button_Approve"
        Me.Button_Approve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Approve.Size = New System.Drawing.Size(41, 41)
        Me.Button_Approve.TabIndex = 18
        Me.Button_Approve.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Approve, "Approve Upload")
        Me.Button_Approve.UseVisualStyleBackColor = False
        '
        'GroupBox_ApprovalOptions
        '
        Me.GroupBox_ApprovalOptions.Controls.Add(Me.RichTextBox_PayInvoice)
        Me.GroupBox_ApprovalOptions.Controls.Add(Me.RichTextBox_PayPO)
        Me.GroupBox_ApprovalOptions.Controls.Add(Me.RadioButton_PayInvoice)
        Me.GroupBox_ApprovalOptions.Controls.Add(Me.RadioButton_PayPO)
        Me.GroupBox_ApprovalOptions.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_ApprovalOptions.Name = "GroupBox_ApprovalOptions"
        Me.GroupBox_ApprovalOptions.Size = New System.Drawing.Size(339, 172)
        Me.GroupBox_ApprovalOptions.TabIndex = 19
        Me.GroupBox_ApprovalOptions.TabStop = False
        Me.GroupBox_ApprovalOptions.Text = "Approval Options"
        '
        'RichTextBox_PayInvoice
        '
        Me.RichTextBox_PayInvoice.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_PayInvoice.Location = New System.Drawing.Point(23, 114)
        Me.RichTextBox_PayInvoice.Name = "RichTextBox_PayInvoice"
        Me.RichTextBox_PayInvoice.ReadOnly = True
        Me.RichTextBox_PayInvoice.Size = New System.Drawing.Size(283, 49)
        Me.RichTextBox_PayInvoice.TabIndex = 4
        Me.RichTextBox_PayInvoice.Text = "The vendor invoice cost will not be updated to match the Purchase Order values.  " & _
            "The vendor will be paid according to the invoice amounts."
        '
        'RichTextBox_PayPO
        '
        Me.RichTextBox_PayPO.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_PayPO.Location = New System.Drawing.Point(23, 42)
        Me.RichTextBox_PayPO.Name = "RichTextBox_PayPO"
        Me.RichTextBox_PayPO.ReadOnly = True
        Me.RichTextBox_PayPO.Size = New System.Drawing.Size(283, 32)
        Me.RichTextBox_PayPO.TabIndex = 3
        Me.RichTextBox_PayPO.Text = "The vendor will be paid the lower or equivalent cost when comparing PO and invoic" & _
            "e costs."
        '
        'RadioButton_PayInvoice
        '
        Me.RadioButton_PayInvoice.AutoSize = True
        Me.RadioButton_PayInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_PayInvoice.Location = New System.Drawing.Point(6, 91)
        Me.RadioButton_PayInvoice.Name = "RadioButton_PayInvoice"
        Me.RadioButton_PayInvoice.Size = New System.Drawing.Size(109, 17)
        Me.RadioButton_PayInvoice.TabIndex = 1
        Me.RadioButton_PayInvoice.TabStop = True
        Me.RadioButton_PayInvoice.Text = "Pay by Invoice"
        Me.RadioButton_PayInvoice.UseVisualStyleBackColor = True
        '
        'RadioButton_PayPO
        '
        Me.RadioButton_PayPO.AutoSize = True
        Me.RadioButton_PayPO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_PayPO.Location = New System.Drawing.Point(6, 19)
        Me.RadioButton_PayPO.Name = "RadioButton_PayPO"
        Me.RadioButton_PayPO.Size = New System.Drawing.Size(136, 17)
        Me.RadioButton_PayPO.TabIndex = 0
        Me.RadioButton_PayPO.TabStop = True
        Me.RadioButton_PayPO.Text = "Pay by Agreed Cost"
        Me.RadioButton_PayPO.UseVisualStyleBackColor = True
        '
        'ApproveInvoice
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(363, 246)
        Me.Controls.Add(Me.GroupBox_ApprovalOptions)
        Me.Controls.Add(Me.Button_Approve)
        Me.Name = "ApproveInvoice"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Approve Invoice For Upload"
        Me.GroupBox_ApprovalOptions.ResumeLayout(False)
        Me.GroupBox_ApprovalOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Button_Approve As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox_ApprovalOptions As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_PayInvoice As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_PayPO As System.Windows.Forms.RadioButton
    Friend WithEvents RichTextBox_PayPO As System.Windows.Forms.RichTextBox
    Friend WithEvents RichTextBox_PayInvoice As System.Windows.Forms.RichTextBox
End Class
