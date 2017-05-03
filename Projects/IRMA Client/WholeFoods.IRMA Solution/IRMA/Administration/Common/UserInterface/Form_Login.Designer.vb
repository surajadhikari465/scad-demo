<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Login
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Login))
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.lbl_UserName = New System.Windows.Forms.Label
        Me.lbl_Password = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtUserName
        '
        Me.txtUserName.AcceptsReturn = True
        Me.txtUserName.BackColor = System.Drawing.SystemColors.Window
        Me.txtUserName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUserName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUserName.Location = New System.Drawing.Point(104, 20)
        Me.txtUserName.MaxLength = 20
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUserName.Size = New System.Drawing.Size(180, 22)
        Me.txtUserName.TabIndex = 1
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdOK.Location = New System.Drawing.Point(297, 13)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(94, 28)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCancel.Location = New System.Drawing.Point(297, 47)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(94, 28)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.AcceptsReturn = True
        Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtPassword.Location = New System.Drawing.Point(104, 47)
        Me.txtPassword.MaxLength = 20
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPassword.Size = New System.Drawing.Size(180, 22)
        Me.txtPassword.TabIndex = 0
        '
        'lbl_UserName
        '
        Me.lbl_UserName.BackColor = System.Drawing.Color.Transparent
        Me.lbl_UserName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_UserName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_UserName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_UserName.Location = New System.Drawing.Point(24, 21)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_UserName.Size = New System.Drawing.Size(72, 18)
        Me.lbl_UserName.TabIndex = 10
        Me.lbl_UserName.Text = "User Name:"
        Me.lbl_UserName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lbl_Password
        '
        Me.lbl_Password.BackColor = System.Drawing.Color.Transparent
        Me.lbl_Password.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl_Password.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Password.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_Password.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbl_Password.Location = New System.Drawing.Point(24, 48)
        Me.lbl_Password.Name = "lbl_Password"
        Me.lbl_Password.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl_Password.Size = New System.Drawing.Size(72, 18)
        Me.lbl_Password.TabIndex = 11
        Me.lbl_Password.Text = "Password:"
        Me.lbl_Password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Form_Login
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(406, 87)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.lbl_Password)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_Login"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Log in"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtUserName As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtPassword As System.Windows.Forms.TextBox
    Public WithEvents lbl_UserName As System.Windows.Forms.Label
    Public WithEvents lbl_Password As System.Windows.Forms.Label
End Class
