<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditDataArchiving
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_EditDataArchiving))
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Form_ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Button_DisableAccount = New System.Windows.Forms.Button
        Me.CheckBox_AcctEnabled = New System.Windows.Forms.CheckBox
        Me.Button_SearchUser = New System.Windows.Forms.Button
        Me.Label_Location = New System.Windows.Forms.Label
        Me.TextBox_CoverPg = New System.Windows.Forms.TextBox
        Me.TextBox_Printer = New System.Windows.Forms.TextBox
        Me.ComboBox_Title = New System.Windows.Forms.ComboBox
        Me.Label_CoverPage = New System.Windows.Forms.Label
        Me.Label_Printer = New System.Windows.Forms.Label
        Me.ComboBox_HomeLocation = New System.Windows.Forms.ComboBox
        Me.TextBox_FaxNo = New System.Windows.Forms.TextBox
        Me.TextBox_FullName = New System.Windows.Forms.TextBox
        Me.TextBox_UserName = New System.Windows.Forms.TextBox
        Me.TextBox_PhoneNo = New System.Windows.Forms.TextBox
        Me.Label_FullName = New System.Windows.Forms.Label
        Me.Label_Title = New System.Windows.Forms.Label
        Me.Label_UserName = New System.Windows.Forms.Label
        Me.TextBox_PagerEmail = New System.Windows.Forms.TextBox
        Me.TextBox_Email = New System.Windows.Forms.TextBox
        Me.Label_Email = New System.Windows.Forms.Label
        Me.Label_PagerEmail = New System.Windows.Forms.Label
        Me.GroupBox_UserProperties = New System.Windows.Forms.GroupBox
        Me.Label_FaxNo = New System.Windows.Forms.Label
        Me.Label_PhoneNo = New System.Windows.Forms.Label
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.ttRoleDescription = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_UserProperties.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Enabled = False
        Me.Button_Save.Image = CType(resources.GetObject("Button_Save.Image"), System.Drawing.Image)
        Me.Button_Save.Location = New System.Drawing.Point(572, 492)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(87, 31)
        Me.Button_Save.TabIndex = 16
        Me.Button_Save.Text = "Save"
        Me.Button_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Form_ErrorProvider
        '
        Me.Form_ErrorProvider.ContainerControl = Me
        '
        'Button_DisableAccount
        '
        Me.Button_DisableAccount.Image = CType(resources.GetObject("Button_DisableAccount.Image"), System.Drawing.Image)
        Me.Button_DisableAccount.Location = New System.Drawing.Point(551, 19)
        Me.Button_DisableAccount.Name = "Button_DisableAccount"
        Me.Button_DisableAccount.Size = New System.Drawing.Size(115, 27)
        Me.Button_DisableAccount.TabIndex = 18
        Me.Button_DisableAccount.Text = "Disable Account"
        Me.Button_DisableAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_DisableAccount.UseVisualStyleBackColor = True
        '
        'CheckBox_AcctEnabled
        '
        Me.CheckBox_AcctEnabled.AutoSize = True
        Me.CheckBox_AcctEnabled.Enabled = False
        Me.CheckBox_AcctEnabled.Location = New System.Drawing.Point(28, 25)
        Me.CheckBox_AcctEnabled.Name = "CheckBox_AcctEnabled"
        Me.CheckBox_AcctEnabled.Size = New System.Drawing.Size(108, 17)
        Me.CheckBox_AcctEnabled.TabIndex = 13
        Me.CheckBox_AcctEnabled.TabStop = False
        Me.CheckBox_AcctEnabled.Text = "Account Enabled"
        Me.CheckBox_AcctEnabled.UseVisualStyleBackColor = True
        '
        'Button_SearchUser
        '
        Me.Button_SearchUser.Image = CType(resources.GetObject("Button_SearchUser.Image"), System.Drawing.Image)
        Me.Button_SearchUser.Location = New System.Drawing.Point(286, 18)
        Me.Button_SearchUser.Name = "Button_SearchUser"
        Me.Button_SearchUser.Size = New System.Drawing.Size(85, 30)
        Me.Button_SearchUser.TabIndex = 2
        Me.Button_SearchUser.Text = "Search AD"
        Me.Button_SearchUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_SearchUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_SearchUser.UseVisualStyleBackColor = True
        '
        'Label_Location
        '
        Me.Label_Location.AutoSize = True
        Me.Label_Location.Location = New System.Drawing.Point(15, 133)
        Me.Label_Location.Name = "Label_Location"
        Me.Label_Location.Size = New System.Drawing.Size(51, 13)
        Me.Label_Location.TabIndex = 20
        Me.Label_Location.Text = "Location:"
        '
        'TextBox_CoverPg
        '
        Me.TextBox_CoverPg.Location = New System.Drawing.Point(490, 76)
        Me.TextBox_CoverPg.MaxLength = 30
        Me.TextBox_CoverPg.Name = "TextBox_CoverPg"
        Me.TextBox_CoverPg.Size = New System.Drawing.Size(141, 20)
        Me.TextBox_CoverPg.TabIndex = 9
        Me.TextBox_CoverPg.Text = "default"
        Me.TextBox_CoverPg.UseWaitCursor = True
        '
        'TextBox_Printer
        '
        Me.TextBox_Printer.Location = New System.Drawing.Point(490, 50)
        Me.TextBox_Printer.MaxLength = 50
        Me.TextBox_Printer.Name = "TextBox_Printer"
        Me.TextBox_Printer.Size = New System.Drawing.Size(141, 20)
        Me.TextBox_Printer.TabIndex = 8
        '
        'ComboBox_Title
        '
        Me.ComboBox_Title.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_Title.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Title.FormattingEnabled = True
        Me.ComboBox_Title.Location = New System.Drawing.Point(71, 103)
        Me.ComboBox_Title.Name = "ComboBox_Title"
        Me.ComboBox_Title.Size = New System.Drawing.Size(209, 21)
        Me.ComboBox_Title.TabIndex = 5
        '
        'Label_CoverPage
        '
        Me.Label_CoverPage.AutoSize = True
        Me.Label_CoverPage.Location = New System.Drawing.Point(421, 79)
        Me.Label_CoverPage.Name = "Label_CoverPage"
        Me.Label_CoverPage.Size = New System.Drawing.Size(66, 13)
        Me.Label_CoverPage.TabIndex = 15
        Me.Label_CoverPage.Text = "Cover Page:"
        '
        'Label_Printer
        '
        Me.Label_Printer.AutoSize = True
        Me.Label_Printer.Location = New System.Drawing.Point(444, 53)
        Me.Label_Printer.Name = "Label_Printer"
        Me.Label_Printer.Size = New System.Drawing.Size(40, 13)
        Me.Label_Printer.TabIndex = 14
        Me.Label_Printer.Text = "Printer:"
        '
        'ComboBox_HomeLocation
        '
        Me.ComboBox_HomeLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_HomeLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_HomeLocation.FormattingEnabled = True
        Me.ComboBox_HomeLocation.Location = New System.Drawing.Point(71, 129)
        Me.ComboBox_HomeLocation.Name = "ComboBox_HomeLocation"
        Me.ComboBox_HomeLocation.Size = New System.Drawing.Size(209, 21)
        Me.ComboBox_HomeLocation.TabIndex = 6
        '
        'TextBox_FaxNo
        '
        Me.TextBox_FaxNo.Location = New System.Drawing.Point(490, 128)
        Me.TextBox_FaxNo.MaxLength = 15
        Me.TextBox_FaxNo.Name = "TextBox_FaxNo"
        Me.TextBox_FaxNo.Size = New System.Drawing.Size(141, 20)
        Me.TextBox_FaxNo.TabIndex = 11
        '
        'TextBox_FullName
        '
        Me.TextBox_FullName.Location = New System.Drawing.Point(71, 51)
        Me.TextBox_FullName.MaxLength = 50
        Me.TextBox_FullName.Name = "TextBox_FullName"
        Me.TextBox_FullName.Size = New System.Drawing.Size(275, 20)
        Me.TextBox_FullName.TabIndex = 3
        '
        'TextBox_UserName
        '
        Me.TextBox_UserName.Location = New System.Drawing.Point(71, 25)
        Me.TextBox_UserName.MaxLength = 25
        Me.TextBox_UserName.Name = "TextBox_UserName"
        Me.TextBox_UserName.Size = New System.Drawing.Size(209, 20)
        Me.TextBox_UserName.TabIndex = 1
        '
        'TextBox_PhoneNo
        '
        Me.TextBox_PhoneNo.Location = New System.Drawing.Point(490, 24)
        Me.TextBox_PhoneNo.MaxLength = 25
        Me.TextBox_PhoneNo.Name = "TextBox_PhoneNo"
        Me.TextBox_PhoneNo.Size = New System.Drawing.Size(141, 20)
        Me.TextBox_PhoneNo.TabIndex = 7
        '
        'Label_FullName
        '
        Me.Label_FullName.AutoSize = True
        Me.Label_FullName.Location = New System.Drawing.Point(8, 56)
        Me.Label_FullName.Name = "Label_FullName"
        Me.Label_FullName.Size = New System.Drawing.Size(57, 13)
        Me.Label_FullName.TabIndex = 2
        Me.Label_FullName.Text = "Full Name:"
        '
        'Label_Title
        '
        Me.Label_Title.AutoSize = True
        Me.Label_Title.Location = New System.Drawing.Point(38, 106)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(30, 13)
        Me.Label_Title.TabIndex = 13
        Me.Label_Title.Text = "Title:"
        '
        'Label_UserName
        '
        Me.Label_UserName.AutoSize = True
        Me.Label_UserName.Location = New System.Drawing.Point(4, 30)
        Me.Label_UserName.Name = "Label_UserName"
        Me.Label_UserName.Size = New System.Drawing.Size(63, 13)
        Me.Label_UserName.TabIndex = 1
        Me.Label_UserName.Text = "User Name:"
        '
        'TextBox_PagerEmail
        '
        Me.TextBox_PagerEmail.Location = New System.Drawing.Point(490, 102)
        Me.TextBox_PagerEmail.MaxLength = 50
        Me.TextBox_PagerEmail.Name = "TextBox_PagerEmail"
        Me.TextBox_PagerEmail.Size = New System.Drawing.Size(141, 20)
        Me.TextBox_PagerEmail.TabIndex = 10
        '
        'TextBox_Email
        '
        Me.TextBox_Email.Location = New System.Drawing.Point(71, 77)
        Me.TextBox_Email.MaxLength = 50
        Me.TextBox_Email.Name = "TextBox_Email"
        Me.TextBox_Email.Size = New System.Drawing.Size(275, 20)
        Me.TextBox_Email.TabIndex = 4
        '
        'Label_Email
        '
        Me.Label_Email.AutoSize = True
        Me.Label_Email.Location = New System.Drawing.Point(32, 82)
        Me.Label_Email.Name = "Label_Email"
        Me.Label_Email.Size = New System.Drawing.Size(35, 13)
        Me.Label_Email.TabIndex = 5
        Me.Label_Email.Text = "Email:"
        '
        'Label_PagerEmail
        '
        Me.Label_PagerEmail.AutoSize = True
        Me.Label_PagerEmail.Location = New System.Drawing.Point(419, 106)
        Me.Label_PagerEmail.Name = "Label_PagerEmail"
        Me.Label_PagerEmail.Size = New System.Drawing.Size(66, 13)
        Me.Label_PagerEmail.TabIndex = 6
        Me.Label_PagerEmail.Text = "Pager Email:"
        '
        'GroupBox_UserProperties
        '
        Me.GroupBox_UserProperties.Controls.Add(Me.Button_SearchUser)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_Location)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_CoverPg)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_Printer)
        Me.GroupBox_UserProperties.Controls.Add(Me.ComboBox_Title)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_CoverPage)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_Printer)
        Me.GroupBox_UserProperties.Controls.Add(Me.ComboBox_HomeLocation)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_FaxNo)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_FullName)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_UserName)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_PhoneNo)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_FullName)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_Title)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_UserName)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_PagerEmail)
        Me.GroupBox_UserProperties.Controls.Add(Me.TextBox_Email)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_Email)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_PagerEmail)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_FaxNo)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_PhoneNo)
        Me.GroupBox_UserProperties.Location = New System.Drawing.Point(28, 48)
        Me.GroupBox_UserProperties.Name = "GroupBox_UserProperties"
        Me.GroupBox_UserProperties.Size = New System.Drawing.Size(638, 425)
        Me.GroupBox_UserProperties.TabIndex = 14
        Me.GroupBox_UserProperties.TabStop = False
        Me.GroupBox_UserProperties.Text = "User Properties"
        '
        'Label_FaxNo
        '
        Me.Label_FaxNo.AutoSize = True
        Me.Label_FaxNo.Location = New System.Drawing.Point(461, 133)
        Me.Label_FaxNo.Name = "Label_FaxNo"
        Me.Label_FaxNo.Size = New System.Drawing.Size(27, 13)
        Me.Label_FaxNo.TabIndex = 8
        Me.Label_FaxNo.Text = "Fax:"
        '
        'Label_PhoneNo
        '
        Me.Label_PhoneNo.AutoSize = True
        Me.Label_PhoneNo.Location = New System.Drawing.Point(445, 29)
        Me.Label_PhoneNo.Name = "Label_PhoneNo"
        Me.Label_PhoneNo.Size = New System.Drawing.Size(41, 13)
        Me.Label_PhoneNo.TabIndex = 7
        Me.Label_PhoneNo.Text = "Phone:"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(484, 492)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(82, 31)
        Me.Button_Cancel.TabIndex = 15
        Me.Button_Cancel.Text = "Close"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'ttRoleDescription
        '
        Me.ttRoleDescription.AutoPopDelay = 5000
        Me.ttRoleDescription.InitialDelay = 500
        Me.ttRoleDescription.IsBalloon = True
        Me.ttRoleDescription.ReshowDelay = 0
        '
        'Form_EditDataArchiving
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(691, 667)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_DisableAccount)
        Me.Controls.Add(Me.CheckBox_AcctEnabled)
        Me.Controls.Add(Me.GroupBox_UserProperties)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Name = "Form_EditDataArchiving"
        Me.Text = "Form_EditDataArchiving"
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_UserProperties.ResumeLayout(False)
        Me.GroupBox_UserProperties.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents ttRoleDescription As System.Windows.Forms.ToolTip
    Friend WithEvents Form_ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents Button_DisableAccount As System.Windows.Forms.Button
    Friend WithEvents CheckBox_AcctEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_UserProperties As System.Windows.Forms.GroupBox
    Friend WithEvents Button_SearchUser As System.Windows.Forms.Button
    Friend WithEvents Label_Location As System.Windows.Forms.Label
    Friend WithEvents TextBox_CoverPg As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Printer As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox_Title As System.Windows.Forms.ComboBox
    Friend WithEvents Label_CoverPage As System.Windows.Forms.Label
    Friend WithEvents Label_Printer As System.Windows.Forms.Label
    Friend WithEvents ComboBox_HomeLocation As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox_FaxNo As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_FullName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UserName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_PhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents Label_FullName As System.Windows.Forms.Label
    Friend WithEvents Label_Title As System.Windows.Forms.Label
    Friend WithEvents Label_UserName As System.Windows.Forms.Label
    Friend WithEvents TextBox_PagerEmail As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Email As System.Windows.Forms.TextBox
    Friend WithEvents Label_Email As System.Windows.Forms.Label
    Friend WithEvents Label_PagerEmail As System.Windows.Forms.Label
    Friend WithEvents Label_FaxNo As System.Windows.Forms.Label
    Friend WithEvents Label_PhoneNo As System.Windows.Forms.Label
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
End Class
