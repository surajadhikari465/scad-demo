<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditStaticStoreFTPInfo
    Inherits Form_IRMABase

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
        Me.TextBox_Port = New System.Windows.Forms.TextBox()
        Me.Label_Port = New System.Windows.Forms.Label()
        Me.TextBox_FTPPassword = New System.Windows.Forms.TextBox()
        Me.TextBox_FTPUser = New System.Windows.Forms.TextBox()
        Me.Label_FTPPassword = New System.Windows.Forms.Label()
        Me.Label_FTPUser = New System.Windows.Forms.Label()
        Me.TextBox_IPAddress = New System.Windows.Forms.TextBox()
        Me.Label_IPAddress = New System.Windows.Forms.Label()
        Me.CheckBox_IsSecureTransfer = New System.Windows.Forms.CheckBox()
        Me.Label_IsSecureTransfer = New System.Windows.Forms.Label()
        Me.TextBox_ChangeDirectory = New System.Windows.Forms.TextBox()
        Me.Label_ChangeDirectory = New System.Windows.Forms.Label()
        Me.GroupBox_FtpInfo = New System.Windows.Forms.GroupBox()
        Me.Label_FileWriterTypeValue = New System.Windows.Forms.Label()
        Me.ComboBox_FileWriterType = New System.Windows.Forms.ComboBox()
        Me.Label_FileWriterType = New System.Windows.Forms.Label()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox_FtpInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox_Port
        '
        Me.TextBox_Port.Location = New System.Drawing.Point(153, 161)
        Me.TextBox_Port.MaxLength = 4
        Me.TextBox_Port.Name = "TextBox_Port"
        Me.TextBox_Port.Size = New System.Drawing.Size(100, 22)
        Me.TextBox_Port.TabIndex = 5
        '
        'Label_Port
        '
        Me.Label_Port.AutoSize = True
        Me.Label_Port.Location = New System.Drawing.Point(17, 164)
        Me.Label_Port.Name = "Label_Port"
        Me.Label_Port.Size = New System.Drawing.Size(28, 13)
        Me.Label_Port.TabIndex = 14
        Me.Label_Port.Text = "Port"
        '
        'TextBox_FTPPassword
        '
        Me.TextBox_FTPPassword.Location = New System.Drawing.Point(153, 109)
        Me.TextBox_FTPPassword.MaxLength = 25
        Me.TextBox_FTPPassword.Name = "TextBox_FTPPassword"
        Me.TextBox_FTPPassword.Size = New System.Drawing.Size(100, 22)
        Me.TextBox_FTPPassword.TabIndex = 3
        '
        'TextBox_FTPUser
        '
        Me.TextBox_FTPUser.Location = New System.Drawing.Point(153, 82)
        Me.TextBox_FTPUser.MaxLength = 25
        Me.TextBox_FTPUser.Name = "TextBox_FTPUser"
        Me.TextBox_FTPUser.Size = New System.Drawing.Size(100, 22)
        Me.TextBox_FTPUser.TabIndex = 2
        '
        'Label_FTPPassword
        '
        Me.Label_FTPPassword.AutoSize = True
        Me.Label_FTPPassword.Location = New System.Drawing.Point(17, 112)
        Me.Label_FTPPassword.Name = "Label_FTPPassword"
        Me.Label_FTPPassword.Size = New System.Drawing.Size(76, 13)
        Me.Label_FTPPassword.TabIndex = 11
        Me.Label_FTPPassword.Text = "FTP Password"
        '
        'Label_FTPUser
        '
        Me.Label_FTPUser.AutoSize = True
        Me.Label_FTPUser.Location = New System.Drawing.Point(17, 85)
        Me.Label_FTPUser.Name = "Label_FTPUser"
        Me.Label_FTPUser.Size = New System.Drawing.Size(50, 13)
        Me.Label_FTPUser.TabIndex = 10
        Me.Label_FTPUser.Text = "FTP User"
        '
        'TextBox_IPAddress
        '
        Me.TextBox_IPAddress.Location = New System.Drawing.Point(153, 56)
        Me.TextBox_IPAddress.MaxLength = 15
        Me.TextBox_IPAddress.Name = "TextBox_IPAddress"
        Me.TextBox_IPAddress.Size = New System.Drawing.Size(100, 22)
        Me.TextBox_IPAddress.TabIndex = 1
        '
        'Label_IPAddress
        '
        Me.Label_IPAddress.AutoSize = True
        Me.Label_IPAddress.Location = New System.Drawing.Point(17, 59)
        Me.Label_IPAddress.Name = "Label_IPAddress"
        Me.Label_IPAddress.Size = New System.Drawing.Size(60, 13)
        Me.Label_IPAddress.TabIndex = 16
        Me.Label_IPAddress.Text = "IP Address"
        '
        'CheckBox_IsSecureTransfer
        '
        Me.CheckBox_IsSecureTransfer.AutoSize = True
        Me.CheckBox_IsSecureTransfer.Location = New System.Drawing.Point(153, 187)
        Me.CheckBox_IsSecureTransfer.Name = "CheckBox_IsSecureTransfer"
        Me.CheckBox_IsSecureTransfer.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_IsSecureTransfer.TabIndex = 6
        Me.CheckBox_IsSecureTransfer.UseVisualStyleBackColor = True
        '
        'Label_IsSecureTransfer
        '
        Me.Label_IsSecureTransfer.AutoSize = True
        Me.Label_IsSecureTransfer.Location = New System.Drawing.Point(17, 188)
        Me.Label_IsSecureTransfer.Name = "Label_IsSecureTransfer"
        Me.Label_IsSecureTransfer.Size = New System.Drawing.Size(101, 13)
        Me.Label_IsSecureTransfer.TabIndex = 19
        Me.Label_IsSecureTransfer.Text = "Is Secure Transfer?"
        '
        'TextBox_ChangeDirectory
        '
        Me.TextBox_ChangeDirectory.Location = New System.Drawing.Point(153, 135)
        Me.TextBox_ChangeDirectory.MaxLength = 100
        Me.TextBox_ChangeDirectory.Name = "TextBox_ChangeDirectory"
        Me.TextBox_ChangeDirectory.Size = New System.Drawing.Size(121, 22)
        Me.TextBox_ChangeDirectory.TabIndex = 4
        '
        'Label_ChangeDirectory
        '
        Me.Label_ChangeDirectory.AutoSize = True
        Me.Label_ChangeDirectory.Location = New System.Drawing.Point(17, 138)
        Me.Label_ChangeDirectory.Name = "Label_ChangeDirectory"
        Me.Label_ChangeDirectory.Size = New System.Drawing.Size(96, 13)
        Me.Label_ChangeDirectory.TabIndex = 20
        Me.Label_ChangeDirectory.Text = "Change Directory"
        '
        'GroupBox_FtpInfo
        '
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_FileWriterTypeValue)
        Me.GroupBox_FtpInfo.Controls.Add(Me.ComboBox_FileWriterType)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_FileWriterType)
        Me.GroupBox_FtpInfo.Controls.Add(Me.TextBox_FTPUser)
        Me.GroupBox_FtpInfo.Controls.Add(Me.TextBox_ChangeDirectory)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_FTPUser)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_ChangeDirectory)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_FTPPassword)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_IsSecureTransfer)
        Me.GroupBox_FtpInfo.Controls.Add(Me.TextBox_FTPPassword)
        Me.GroupBox_FtpInfo.Controls.Add(Me.CheckBox_IsSecureTransfer)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_Port)
        Me.GroupBox_FtpInfo.Controls.Add(Me.TextBox_IPAddress)
        Me.GroupBox_FtpInfo.Controls.Add(Me.TextBox_Port)
        Me.GroupBox_FtpInfo.Controls.Add(Me.Label_IPAddress)
        Me.GroupBox_FtpInfo.Location = New System.Drawing.Point(22, 12)
        Me.GroupBox_FtpInfo.Name = "GroupBox_FtpInfo"
        Me.GroupBox_FtpInfo.Size = New System.Drawing.Size(290, 227)
        Me.GroupBox_FtpInfo.TabIndex = 0
        Me.GroupBox_FtpInfo.TabStop = False
        Me.GroupBox_FtpInfo.Text = "FTP Info"
        '
        'Label_FileWriterTypeValue
        '
        Me.Label_FileWriterTypeValue.AutoSize = True
        Me.Label_FileWriterTypeValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FileWriterTypeValue.Location = New System.Drawing.Point(153, 29)
        Me.Label_FileWriterTypeValue.Name = "Label_FileWriterTypeValue"
        Me.Label_FileWriterTypeValue.Size = New System.Drawing.Size(45, 13)
        Me.Label_FileWriterTypeValue.TabIndex = 23
        Me.Label_FileWriterTypeValue.Text = "Label1"
        '
        'ComboBox_FileWriterType
        '
        Me.ComboBox_FileWriterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FileWriterType.FormattingEnabled = True
        Me.ComboBox_FileWriterType.Location = New System.Drawing.Point(153, 27)
        Me.ComboBox_FileWriterType.Name = "ComboBox_FileWriterType"
        Me.ComboBox_FileWriterType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_FileWriterType.TabIndex = 22
        '
        'Label_FileWriterType
        '
        Me.Label_FileWriterType.AutoSize = True
        Me.Label_FileWriterType.Location = New System.Drawing.Point(17, 30)
        Me.Label_FileWriterType.Name = "Label_FileWriterType"
        Me.Label_FileWriterType.Size = New System.Drawing.Size(86, 13)
        Me.Label_FileWriterType.TabIndex = 21
        Me.Label_FileWriterType.Text = "File Writer Type"
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(237, 245)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(75, 23)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(156, 245)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_EditStaticStoreFTPInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 283)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox_FtpInfo)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_EditStaticStoreFTPInfo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Edit Static Store FTP Info"
        Me.GroupBox_FtpInfo.ResumeLayout(False)
        Me.GroupBox_FtpInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TextBox_Port As System.Windows.Forms.TextBox
    Friend WithEvents Label_Port As System.Windows.Forms.Label
    Friend WithEvents TextBox_FTPPassword As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_FTPUser As System.Windows.Forms.TextBox
    Friend WithEvents Label_FTPPassword As System.Windows.Forms.Label
    Friend WithEvents Label_FTPUser As System.Windows.Forms.Label
    Friend WithEvents TextBox_IPAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label_IPAddress As System.Windows.Forms.Label
    Friend WithEvents CheckBox_IsSecureTransfer As System.Windows.Forms.CheckBox
    Friend WithEvents Label_IsSecureTransfer As System.Windows.Forms.Label
    Friend WithEvents TextBox_ChangeDirectory As System.Windows.Forms.TextBox
    Friend WithEvents Label_ChangeDirectory As System.Windows.Forms.Label
    Friend WithEvents GroupBox_FtpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Label_FileWriterType As System.Windows.Forms.Label
    Friend WithEvents ComboBox_FileWriterType As System.Windows.Forms.ComboBox
    Friend WithEvents Label_FileWriterTypeValue As System.Windows.Forms.Label
End Class
