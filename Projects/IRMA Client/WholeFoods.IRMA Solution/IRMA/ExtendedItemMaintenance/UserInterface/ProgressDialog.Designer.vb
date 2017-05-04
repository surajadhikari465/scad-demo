<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgressDialog
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
        Me.ProgressBarControl = New System.Windows.Forms.ProgressBar
        Me.LabelProgressMessage = New System.Windows.Forms.Label
        Me.LabelSubmessage = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.LabelTextProgress = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ProgressBarControl
        '
        Me.ProgressBarControl.Location = New System.Drawing.Point(7, 47)
        Me.ProgressBarControl.Name = "ProgressBarControl"
        Me.ProgressBarControl.Size = New System.Drawing.Size(456, 19)
        Me.ProgressBarControl.TabIndex = 0
        Me.ProgressBarControl.UseWaitCursor = True
        '
        'LabelProgressMessage
        '
        Me.LabelProgressMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelProgressMessage.ForeColor = System.Drawing.Color.Navy
        Me.LabelProgressMessage.Location = New System.Drawing.Point(12, 8)
        Me.LabelProgressMessage.Name = "LabelProgressMessage"
        Me.LabelProgressMessage.Size = New System.Drawing.Size(347, 17)
        Me.LabelProgressMessage.TabIndex = 1
        Me.LabelProgressMessage.Text = "Progress Message"
        Me.LabelProgressMessage.UseWaitCursor = True
        '
        'LabelSubmessage
        '
        Me.LabelSubmessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSubmessage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LabelSubmessage.Location = New System.Drawing.Point(31, 27)
        Me.LabelSubmessage.Name = "LabelSubmessage"
        Me.LabelSubmessage.Size = New System.Drawing.Size(315, 17)
        Me.LabelSubmessage.TabIndex = 2
        Me.LabelSubmessage.Text = "Please Stand By..."
        Me.LabelSubmessage.UseWaitCursor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Panel1.Controls.Add(Me.ProgressBarControl)
        Me.Panel1.Controls.Add(Me.LabelTextProgress)
        Me.Panel1.Controls.Add(Me.LabelSubmessage)
        Me.Panel1.Controls.Add(Me.LabelProgressMessage)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(474, 80)
        Me.Panel1.TabIndex = 3
        Me.Panel1.UseWaitCursor = True
        '
        'LabelTextProgress
        '
        Me.LabelTextProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTextProgress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LabelTextProgress.Location = New System.Drawing.Point(342, 25)
        Me.LabelTextProgress.Name = "LabelTextProgress"
        Me.LabelTextProgress.Size = New System.Drawing.Size(121, 17)
        Me.LabelTextProgress.TabIndex = 3
        Me.LabelTextProgress.Text = "1 of 100"
        Me.LabelTextProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LabelTextProgress.UseWaitCursor = True
        '
        'ProgressDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(480, 86)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProgressDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Progress"
        Me.UseWaitCursor = True
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelSubmessage As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ProgressBarControl As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelProgressMessage As System.Windows.Forms.Label
    Friend WithEvents LabelTextProgress As System.Windows.Forms.Label
End Class
