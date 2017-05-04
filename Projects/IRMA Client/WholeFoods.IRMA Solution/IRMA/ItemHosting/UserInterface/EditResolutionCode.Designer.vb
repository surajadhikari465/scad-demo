<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditResolutionCode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditResolutionCode))
        Me.lblCodeDesc = New System.Windows.Forms.Label()
        Me.txtCodeDesc = New System.Windows.Forms.TextBox()
        Me.lblDefault = New System.Windows.Forms.Label()
        Me.chkDefault = New System.Windows.Forms.CheckBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.lblActive = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblCodeDesc
        '
        Me.lblCodeDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblCodeDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCodeDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCodeDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCodeDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCodeDesc.Location = New System.Drawing.Point(30, 25)
        Me.lblCodeDesc.Name = "lblCodeDesc"
        Me.lblCodeDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCodeDesc.Size = New System.Drawing.Size(123, 17)
        Me.lblCodeDesc.TabIndex = 4
        Me.lblCodeDesc.Text = "Code Description:"
        Me.lblCodeDesc.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtCodeDesc
        '
        Me.txtCodeDesc.AcceptsReturn = True
        Me.txtCodeDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtCodeDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCodeDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtCodeDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCodeDesc.Location = New System.Drawing.Point(159, 25)
        Me.txtCodeDesc.MaxLength = 50
        Me.txtCodeDesc.Name = "txtCodeDesc"
        Me.txtCodeDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCodeDesc.Size = New System.Drawing.Size(370, 20)
        Me.txtCodeDesc.TabIndex = 5
        Me.txtCodeDesc.Tag = "String"
        '
        'lblDefault
        '
        Me.lblDefault.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefault.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefault.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblDefault.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefault.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDefault.Location = New System.Drawing.Point(30, 58)
        Me.lblDefault.Name = "lblDefault"
        Me.lblDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefault.Size = New System.Drawing.Size(123, 17)
        Me.lblDefault.TabIndex = 8
        Me.lblDefault.Text = "Default:"
        Me.lblDefault.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'chkDefault
        '
        Me.chkDefault.AutoSize = True
        Me.chkDefault.Location = New System.Drawing.Point(159, 61)
        Me.chkDefault.Name = "chkDefault"
        Me.chkDefault.Size = New System.Drawing.Size(15, 14)
        Me.chkDefault.TabIndex = 28
        Me.chkDefault.UseVisualStyleBackColor = True
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdExit.Location = New System.Drawing.Point(488, 69)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 29
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
        Me.cmdSubmit.Location = New System.Drawing.Point(430, 69)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 30
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'chkActive
        '
        Me.chkActive.AutoSize = True
        Me.chkActive.Location = New System.Drawing.Point(158, 86)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.Size = New System.Drawing.Size(15, 14)
        Me.chkActive.TabIndex = 32
        Me.chkActive.UseVisualStyleBackColor = True
        '
        'lblActive
        '
        Me.lblActive.BackColor = System.Drawing.SystemColors.Control
        Me.lblActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActive.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActive.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblActive.Location = New System.Drawing.Point(31, 83)
        Me.lblActive.Name = "lblActive"
        Me.lblActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActive.Size = New System.Drawing.Size(123, 17)
        Me.lblActive.TabIndex = 31
        Me.lblActive.Text = "Active: "
        Me.lblActive.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'EditResolutionCode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(557, 133)
        Me.Controls.Add(Me.chkActive)
        Me.Controls.Add(Me.lblActive)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.chkDefault)
        Me.Controls.Add(Me.lblDefault)
        Me.Controls.Add(Me.txtCodeDesc)
        Me.Controls.Add(Me.lblCodeDesc)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditResolutionCode"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblCodeDesc As System.Windows.Forms.Label
    Public WithEvents txtCodeDesc As System.Windows.Forms.TextBox
    Public WithEvents lblDefault As System.Windows.Forms.Label
    Friend WithEvents chkDefault As System.Windows.Forms.CheckBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Friend WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents lblActive As System.Windows.Forms.Label
End Class
