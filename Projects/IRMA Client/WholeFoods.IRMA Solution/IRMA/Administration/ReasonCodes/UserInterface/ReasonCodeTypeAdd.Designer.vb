<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReasonCodeTypeAdd
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReasonCodeTypeAdd))
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.txtReasonCodeTypeDesc = New System.Windows.Forms.TextBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.txtReasonCodeTypeAbbr = New System.Windows.Forms.TextBox()
        Me.lblAbbr = New System.Windows.Forms.Label()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(174, 103)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 11
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, "Add Type")
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'txtReasonCodeTypeDesc
        '
        Me.txtReasonCodeTypeDesc.Location = New System.Drawing.Point(115, 60)
        Me.txtReasonCodeTypeDesc.Name = "txtReasonCodeTypeDesc"
        Me.txtReasonCodeTypeDesc.Size = New System.Drawing.Size(221, 20)
        Me.txtReasonCodeTypeDesc.TabIndex = 12
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(222, 103)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 28
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtReasonCodeTypeAbbr
        '
        Me.txtReasonCodeTypeAbbr.Location = New System.Drawing.Point(115, 34)
        Me.txtReasonCodeTypeAbbr.Name = "txtReasonCodeTypeAbbr"
        Me.txtReasonCodeTypeAbbr.Size = New System.Drawing.Size(42, 20)
        Me.txtReasonCodeTypeAbbr.TabIndex = 29
        '
        'lblAbbr
        '
        Me.lblAbbr.AutoSize = True
        Me.lblAbbr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAbbr.Location = New System.Drawing.Point(25, 37)
        Me.lblAbbr.Name = "lblAbbr"
        Me.lblAbbr.Size = New System.Drawing.Size(82, 13)
        Me.lblAbbr.TabIndex = 30
        Me.lblAbbr.Text = "Abbreviation:"
        '
        'lblDesc
        '
        Me.lblDesc.AutoSize = True
        Me.lblDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.Location = New System.Drawing.Point(25, 63)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(75, 13)
        Me.lblDesc.TabIndex = 31
        Me.lblDesc.Text = "Description:"
        '
        'ReasonCodeTypeAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(366, 170)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblAbbr)
        Me.Controls.Add(Me.txtReasonCodeTypeAbbr)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtReasonCodeTypeDesc)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Name = "ReasonCodeTypeAdd"
        Me.Text = "Add a Reason Code Type"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Friend WithEvents txtReasonCodeTypeDesc As System.Windows.Forms.TextBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents txtReasonCodeTypeAbbr As System.Windows.Forms.TextBox
    Friend WithEvents lblAbbr As System.Windows.Forms.Label
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
