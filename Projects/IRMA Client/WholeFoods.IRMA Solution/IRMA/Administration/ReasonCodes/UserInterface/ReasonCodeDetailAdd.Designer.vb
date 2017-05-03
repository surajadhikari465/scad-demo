<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReasonCodeDetailAdd
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReasonCodeDetailAdd))
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtExtDescription = New System.Windows.Forms.TextBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblReasonCodeType = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtCode
        '
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.Location = New System.Drawing.Point(97, 30)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(61, 20)
        Me.txtCode.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(267, 30)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(215, 20)
        Me.txtDescription.TabIndex = 1
        '
        'txtExtDescription
        '
        Me.txtExtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtDescription.Location = New System.Drawing.Point(267, 77)
        Me.txtExtDescription.Multiline = True
        Me.txtExtDescription.Name = "txtExtDescription"
        Me.txtExtDescription.Size = New System.Drawing.Size(215, 111)
        Me.txtExtDescription.TabIndex = 2
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
        Me.cmdExit.Location = New System.Drawing.Point(387, 204)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(48, 41)
        Me.cmdExit.TabIndex = 28
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(333, 204)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(48, 41)
        Me.cmdSubmit.TabIndex = 29
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(45, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Code: "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(182, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Description: "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(125, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 13)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "Extended Description: "
        '
        'lblReasonCodeType
        '
        Me.lblReasonCodeType.AutoSize = True
        Me.lblReasonCodeType.Location = New System.Drawing.Point(175, 29)
        Me.lblReasonCodeType.Name = "lblReasonCodeType"
        Me.lblReasonCodeType.Size = New System.Drawing.Size(0, 13)
        Me.lblReasonCodeType.TabIndex = 34
        '
        'ReasonCodeDetailAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 257)
        Me.Controls.Add(Me.lblReasonCodeType)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtExtDescription)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtCode)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ReasonCodeDetailAdd"
        Me.Text = "Add a Reason Code Detail"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtExtDescription As System.Windows.Forms.TextBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblReasonCodeType As System.Windows.Forms.Label
End Class
