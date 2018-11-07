<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_SendOrders
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
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.Label1 = New System.Windows.Forms.Label
        Me.SendOrdersButton = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckFaxStatusButton = New System.Windows.Forms.Button
        Me.LogTextBox1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 283)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(599, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel1.Text = "Ready"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(193, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Click here to send all queued orders"
        '
        'SendOrdersButton
        '
        Me.SendOrdersButton.Location = New System.Drawing.Point(28, 50)
        Me.SendOrdersButton.Name = "SendOrdersButton"
        Me.SendOrdersButton.Size = New System.Drawing.Size(173, 23)
        Me.SendOrdersButton.TabIndex = 2
        Me.SendOrdersButton.Text = "Send Orders"
        Me.SendOrdersButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(155, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Click here to check fax status"
        '
        'CheckFaxStatusButton
        '
        Me.CheckFaxStatusButton.Location = New System.Drawing.Point(28, 105)
        Me.CheckFaxStatusButton.Name = "CheckFaxStatusButton"
        Me.CheckFaxStatusButton.Size = New System.Drawing.Size(170, 23)
        Me.CheckFaxStatusButton.TabIndex = 4
        Me.CheckFaxStatusButton.Text = "Check Fax Status"
        Me.CheckFaxStatusButton.UseVisualStyleBackColor = True
        '
        'LogTextBox1
        '
        Me.LogTextBox1.Location = New System.Drawing.Point(28, 162)
        Me.LogTextBox1.Multiline = True
        Me.LogTextBox1.Name = "LogTextBox1"
        Me.LogTextBox1.Size = New System.Drawing.Size(555, 109)
        Me.LogTextBox1.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Send Order Log"
        '
        'Form_SendOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 305)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LogTextBox1)
        Me.Controls.Add(Me.CheckFaxStatusButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.SendOrdersButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "Form_SendOrders"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Send Orders"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SendOrdersButton As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CheckFaxStatusButton As System.Windows.Forms.Button
    Friend WithEvents LogTextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
