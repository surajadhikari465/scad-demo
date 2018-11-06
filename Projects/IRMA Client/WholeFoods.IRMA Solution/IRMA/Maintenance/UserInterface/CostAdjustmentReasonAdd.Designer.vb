Namespace WholeFoods.IRMA.Administration.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
        Partial Class CostAdjustmentReasonAdd
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CostAdjustmentReasonAdd))
            Me.cmdAdd = New System.Windows.Forms.Button
            Me.cmdExit = New System.Windows.Forms.Button
            Me.txtReason = New System.Windows.Forms.TextBox
            Me.lblLabel = New System.Windows.Forms.Label
            Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
            CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmdAdd
            '
            Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
            Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
            Me.cmdAdd.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
            Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
            Me.cmdAdd.Location = New System.Drawing.Point(228, 34)
            Me.cmdAdd.Name = "cmdAdd"
            Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
            Me.cmdAdd.TabIndex = 6
            Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            Me.cmdAdd.UseVisualStyleBackColor = False
            '
            'cmdExit
            '
            Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
            Me.cmdExit.CausesValidation = False
            Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
            Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
            Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
            Me.cmdExit.Location = New System.Drawing.Point(276, 34)
            Me.cmdExit.Name = "cmdExit"
            Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.cmdExit.Size = New System.Drawing.Size(41, 41)
            Me.cmdExit.TabIndex = 7
            Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            Me.cmdExit.UseVisualStyleBackColor = False
            '
            'txtReason
            '
            Me.txtReason.AcceptsReturn = True
            Me.txtReason.BackColor = System.Drawing.SystemColors.Window
            Me.txtReason.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtReason.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtReason.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtReason.Location = New System.Drawing.Point(73, 11)
            Me.txtReason.MaxLength = 50
            Me.txtReason.Name = "txtReason"
            Me.txtReason.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtReason.Size = New System.Drawing.Size(244, 20)
            Me.txtReason.TabIndex = 5
            '
            'lblLabel
            '
            Me.lblLabel.BackColor = System.Drawing.Color.Transparent
            Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
            Me.lblLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
            Me.lblLabel.Location = New System.Drawing.Point(12, 14)
            Me.lblLabel.Name = "lblLabel"
            Me.lblLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.lblLabel.Size = New System.Drawing.Size(55, 17)
            Me.lblLabel.TabIndex = 4
            Me.lblLabel.Text = "Reason :"
            Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'ErrorProvider1
            '
            Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
            Me.ErrorProvider1.ContainerControl = Me
            '
            'CostAdjustmentReasonAdd
            '
            Me.AcceptButton = Me.cmdAdd
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdExit
            Me.ClientSize = New System.Drawing.Size(337, 87)
            Me.Controls.Add(Me.cmdAdd)
            Me.Controls.Add(Me.cmdExit)
            Me.Controls.Add(Me.txtReason)
            Me.Controls.Add(Me.lblLabel)
            Me.Name = "CostAdjustmentReasonAdd"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "New Cost Adjustment Reason"
            CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Public WithEvents cmdAdd As System.Windows.Forms.Button
        Public WithEvents cmdExit As System.Windows.Forms.Button
        Public WithEvents txtReason As System.Windows.Forms.TextBox
        Public WithEvents lblLabel As System.Windows.Forms.Label
        Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    End Class
End Namespace