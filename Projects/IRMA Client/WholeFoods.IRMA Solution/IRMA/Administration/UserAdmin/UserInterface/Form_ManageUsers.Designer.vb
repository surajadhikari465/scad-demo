<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_ManageUsers
    Inherits Form_IRMABase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageUsers))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Button_Delete = New System.Windows.Forms.Button()
        Me.Button_Edit = New System.Windows.Forms.Button()
        Me.Button_Add = New System.Windows.Forms.Button()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.DataGridView_ManageUsers = New System.Windows.Forms.DataGridView()
        Me.UserDAOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Group_FilterOptions = New System.Windows.Forms.GroupBox()
        Me.Label_FilterStatus = New System.Windows.Forms.Label()
        Me.Radio_FilterStatusEnabled = New System.Windows.Forms.RadioButton()
        Me.Radio_FilterStatusDisabled = New System.Windows.Forms.RadioButton()
        Me.Radio_FiterStatusAll = New System.Windows.Forms.RadioButton()
        Me.ComboBox_FilterTitle = New System.Windows.Forms.ComboBox()
        Me.Label_FilterTitle = New System.Windows.Forms.Label()
        Me.Button_ApplyFilter = New System.Windows.Forms.Button()
        Me.Button_ClearFilter = New System.Windows.Forms.Button()
        Me.TextBox_FilterFullName = New System.Windows.Forms.TextBox()
        Me.Label_FilterFullName = New System.Windows.Forms.Label()
        Me.TextBox_FilterUserName = New System.Windows.Forms.TextBox()
        Me.Label_FilterWFMUser = New System.Windows.Forms.Label()
        Me.colUserName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFullName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTitle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAccountEnabled = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colUserID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView_ManageUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UserDAOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Group_FilterOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.Enabled = False
        Me.Button_Delete.Image = CType(resources.GetObject("Button_Delete.Image"), System.Drawing.Image)
        Me.Button_Delete.Location = New System.Drawing.Point(586, 95)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(97, 30)
        Me.Button_Delete.TabIndex = 11
        Me.Button_Delete.Text = "Disable"
        Me.Button_Delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Delete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Edit.Enabled = False
        Me.Button_Edit.Image = CType(resources.GetObject("Button_Edit.Image"), System.Drawing.Image)
        Me.Button_Edit.Location = New System.Drawing.Point(586, 55)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(97, 30)
        Me.Button_Edit.TabIndex = 9
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Edit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Add.Image = CType(resources.GetObject("Button_Add.Image"), System.Drawing.Image)
        Me.Button_Add.Location = New System.Drawing.Point(586, 16)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(97, 30)
        Me.Button_Add.TabIndex = 8
        Me.Button_Add.Text = "Add"
        Me.Button_Add.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(598, 551)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(97, 28)
        Me.Button_Close.TabIndex = 12
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Controls.Add(Me.DataGridView_ManageUsers)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 98)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(689, 449)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        '
        'DataGridView_ManageUsers
        '
        Me.DataGridView_ManageUsers.AllowUserToAddRows = False
        Me.DataGridView_ManageUsers.AllowUserToDeleteRows = False
        Me.DataGridView_ManageUsers.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_ManageUsers.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_ManageUsers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_ManageUsers.AutoGenerateColumns = False
        Me.DataGridView_ManageUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView_ManageUsers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_ManageUsers.CausesValidation = False
        Me.DataGridView_ManageUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_ManageUsers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colUserName, Me.colFullName, Me.colTitle, Me.colAccountEnabled, Me.colUserID})
        Me.DataGridView_ManageUsers.DataSource = Me.UserDAOBindingSource
        Me.DataGridView_ManageUsers.Location = New System.Drawing.Point(6, 16)
        Me.DataGridView_ManageUsers.MultiSelect = False
        Me.DataGridView_ManageUsers.Name = "DataGridView_ManageUsers"
        Me.DataGridView_ManageUsers.ReadOnly = True
        Me.DataGridView_ManageUsers.RowHeadersVisible = False
        Me.DataGridView_ManageUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView_ManageUsers.Size = New System.Drawing.Size(574, 427)
        Me.DataGridView_ManageUsers.TabIndex = 10
        '
        'UserDAOBindingSource
        '
        Me.UserDAOBindingSource.DataSource = GetType(WholeFoods.IRMA.Administration.Common.DataAccess.UserDAO)
        '
        'Group_FilterOptions
        '
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterStatus)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FilterStatusEnabled)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FilterStatusDisabled)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FiterStatusAll)
        Me.Group_FilterOptions.Controls.Add(Me.ComboBox_FilterTitle)
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterTitle)
        Me.Group_FilterOptions.Controls.Add(Me.Button_ApplyFilter)
        Me.Group_FilterOptions.Controls.Add(Me.Button_ClearFilter)
        Me.Group_FilterOptions.Controls.Add(Me.TextBox_FilterFullName)
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterFullName)
        Me.Group_FilterOptions.Controls.Add(Me.TextBox_FilterUserName)
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterWFMUser)
        Me.Group_FilterOptions.Location = New System.Drawing.Point(12, 12)
        Me.Group_FilterOptions.Name = "Group_FilterOptions"
        Me.Group_FilterOptions.Size = New System.Drawing.Size(687, 88)
        Me.Group_FilterOptions.TabIndex = 0
        Me.Group_FilterOptions.TabStop = False
        Me.Group_FilterOptions.Text = "User Filter"
        '
        'Label_FilterStatus
        '
        Me.Label_FilterStatus.AutoSize = True
        Me.Label_FilterStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterStatus.Location = New System.Drawing.Point(324, 56)
        Me.Label_FilterStatus.Name = "Label_FilterStatus"
        Me.Label_FilterStatus.Size = New System.Drawing.Size(42, 13)
        Me.Label_FilterStatus.TabIndex = 13
        Me.Label_FilterStatus.Text = "Status:"
        '
        'Radio_FilterStatusEnabled
        '
        Me.Radio_FilterStatusEnabled.AutoSize = True
        Me.Radio_FilterStatusEnabled.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FilterStatusEnabled.Location = New System.Drawing.Point(430, 54)
        Me.Radio_FilterStatusEnabled.Name = "Radio_FilterStatusEnabled"
        Me.Radio_FilterStatusEnabled.Size = New System.Drawing.Size(67, 17)
        Me.Radio_FilterStatusEnabled.TabIndex = 4
        Me.Radio_FilterStatusEnabled.Text = "Enabled"
        Me.Radio_FilterStatusEnabled.UseVisualStyleBackColor = True
        '
        'Radio_FilterStatusDisabled
        '
        Me.Radio_FilterStatusDisabled.AutoSize = True
        Me.Radio_FilterStatusDisabled.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FilterStatusDisabled.Location = New System.Drawing.Point(503, 54)
        Me.Radio_FilterStatusDisabled.Name = "Radio_FilterStatusDisabled"
        Me.Radio_FilterStatusDisabled.Size = New System.Drawing.Size(70, 17)
        Me.Radio_FilterStatusDisabled.TabIndex = 5
        Me.Radio_FilterStatusDisabled.Text = "Disabled"
        Me.Radio_FilterStatusDisabled.UseVisualStyleBackColor = True
        '
        'Radio_FiterStatusAll
        '
        Me.Radio_FiterStatusAll.AutoSize = True
        Me.Radio_FiterStatusAll.Checked = True
        Me.Radio_FiterStatusAll.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FiterStatusAll.Location = New System.Drawing.Point(372, 54)
        Me.Radio_FiterStatusAll.Name = "Radio_FiterStatusAll"
        Me.Radio_FiterStatusAll.Size = New System.Drawing.Size(38, 17)
        Me.Radio_FiterStatusAll.TabIndex = 3
        Me.Radio_FiterStatusAll.TabStop = True
        Me.Radio_FiterStatusAll.Text = "All"
        Me.Radio_FiterStatusAll.UseVisualStyleBackColor = True
        '
        'ComboBox_FilterTitle
        '
        Me.ComboBox_FilterTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_FilterTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_FilterTitle.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox_FilterTitle.FormattingEnabled = True
        Me.ComboBox_FilterTitle.Location = New System.Drawing.Point(372, 25)
        Me.ComboBox_FilterTitle.Name = "ComboBox_FilterTitle"
        Me.ComboBox_FilterTitle.Size = New System.Drawing.Size(201, 21)
        Me.ComboBox_FilterTitle.TabIndex = 2
        Me.ComboBox_FilterTitle.Text = "All Titles"
        '
        'Label_FilterTitle
        '
        Me.Label_FilterTitle.AutoSize = True
        Me.Label_FilterTitle.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterTitle.Location = New System.Drawing.Point(335, 28)
        Me.Label_FilterTitle.Name = "Label_FilterTitle"
        Me.Label_FilterTitle.Size = New System.Drawing.Size(31, 13)
        Me.Label_FilterTitle.TabIndex = 8
        Me.Label_FilterTitle.Text = "Title:"
        '
        'Button_ApplyFilter
        '
        Me.Button_ApplyFilter.Image = CType(resources.GetObject("Button_ApplyFilter.Image"), System.Drawing.Image)
        Me.Button_ApplyFilter.Location = New System.Drawing.Point(582, 17)
        Me.Button_ApplyFilter.Name = "Button_ApplyFilter"
        Me.Button_ApplyFilter.Size = New System.Drawing.Size(97, 30)
        Me.Button_ApplyFilter.TabIndex = 6
        Me.Button_ApplyFilter.Text = "Apply Filter"
        Me.Button_ApplyFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_ApplyFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_ApplyFilter.UseVisualStyleBackColor = True
        '
        'Button_ClearFilter
        '
        Me.Button_ClearFilter.Image = CType(resources.GetObject("Button_ClearFilter.Image"), System.Drawing.Image)
        Me.Button_ClearFilter.Location = New System.Drawing.Point(582, 50)
        Me.Button_ClearFilter.Name = "Button_ClearFilter"
        Me.Button_ClearFilter.Size = New System.Drawing.Size(97, 30)
        Me.Button_ClearFilter.TabIndex = 7
        Me.Button_ClearFilter.Text = "Clear Filter"
        Me.Button_ClearFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_ClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_ClearFilter.UseVisualStyleBackColor = True
        '
        'TextBox_FilterFullName
        '
        Me.TextBox_FilterFullName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_FilterFullName.Location = New System.Drawing.Point(82, 53)
        Me.TextBox_FilterFullName.Name = "TextBox_FilterFullName"
        Me.TextBox_FilterFullName.Size = New System.Drawing.Size(220, 22)
        Me.TextBox_FilterFullName.TabIndex = 1
        '
        'Label_FilterFullName
        '
        Me.Label_FilterFullName.AutoSize = True
        Me.Label_FilterFullName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterFullName.Location = New System.Drawing.Point(13, 56)
        Me.Label_FilterFullName.Name = "Label_FilterFullName"
        Me.Label_FilterFullName.Size = New System.Drawing.Size(61, 13)
        Me.Label_FilterFullName.TabIndex = 2
        Me.Label_FilterFullName.Text = "Full Name:"
        '
        'TextBox_FilterUserName
        '
        Me.TextBox_FilterUserName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_FilterUserName.Location = New System.Drawing.Point(82, 25)
        Me.TextBox_FilterUserName.Name = "TextBox_FilterUserName"
        Me.TextBox_FilterUserName.Size = New System.Drawing.Size(220, 22)
        Me.TextBox_FilterUserName.TabIndex = 0
        '
        'Label_FilterWFMUser
        '
        Me.Label_FilterWFMUser.AutoSize = True
        Me.Label_FilterWFMUser.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterWFMUser.Location = New System.Drawing.Point(25, 28)
        Me.Label_FilterWFMUser.Name = "Label_FilterWFMUser"
        Me.Label_FilterWFMUser.Size = New System.Drawing.Size(47, 13)
        Me.Label_FilterWFMUser.TabIndex = 0
        Me.Label_FilterWFMUser.Text = "User ID:"
        '
        'colUserName
        '
        Me.colUserName.DataPropertyName = "UserName"
        Me.colUserName.FillWeight = 60.0!
        Me.colUserName.HeaderText = "WFM Login ID"
        Me.colUserName.MaxInputLength = 25
        Me.colUserName.Name = "colUserName"
        Me.colUserName.ReadOnly = True
        '
        'colFullName
        '
        Me.colFullName.DataPropertyName = "FullName"
        Me.colFullName.FillWeight = 120.0!
        Me.colFullName.HeaderText = "Name"
        Me.colFullName.MaxInputLength = 50
        Me.colFullName.Name = "colFullName"
        Me.colFullName.ReadOnly = True
        '
        'colTitle
        '
        Me.colTitle.DataPropertyName = "Title"
        Me.colTitle.FillWeight = 120.0!
        Me.colTitle.HeaderText = "Title"
        Me.colTitle.MaxInputLength = 50
        Me.colTitle.Name = "colTitle"
        Me.colTitle.ReadOnly = True
        '
        'colAccountEnabled
        '
        Me.colAccountEnabled.DataPropertyName = "AccountEnabled"
        Me.colAccountEnabled.FillWeight = 80.0!
        Me.colAccountEnabled.HeaderText = "Account Enabled"
        Me.colAccountEnabled.Name = "colAccountEnabled"
        Me.colAccountEnabled.ReadOnly = True
        '
        'colUserID
        '
        Me.colUserID.DataPropertyName = "User_ID"
        Me.colUserID.FillWeight = 60.0!
        Me.colUserID.HeaderText = "_hidden_User_ID"
        Me.colUserID.MaxInputLength = 10
        Me.colUserID.Name = "colUserID"
        Me.colUserID.ReadOnly = True
        Me.colUserID.Visible = False
        '
        'Form_ManageUsers
        '
        Me.AcceptButton = Me.Button_ApplyFilter
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 590)
        Me.Controls.Add(Me.Group_FilterOptions)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(722, 617)
        Me.Name = "Form_ManageUsers"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Users"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGridView_ManageUsers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UserDAOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Group_FilterOptions.ResumeLayout(False)
        Me.Group_FilterOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents DataGridView_ManageUsers As System.Windows.Forms.DataGridView
    Friend WithEvents Group_FilterOptions As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_FilterTitle As System.Windows.Forms.ComboBox
    Friend WithEvents Label_FilterTitle As System.Windows.Forms.Label
    Friend WithEvents Button_ApplyFilter As System.Windows.Forms.Button
    Friend WithEvents Button_ClearFilter As System.Windows.Forms.Button
    Friend WithEvents TextBox_FilterFullName As System.Windows.Forms.TextBox
    Friend WithEvents Label_FilterFullName As System.Windows.Forms.Label
    Friend WithEvents TextBox_FilterUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label_FilterWFMUser As System.Windows.Forms.Label
    Friend WithEvents Label_FilterStatus As System.Windows.Forms.Label
    Friend WithEvents Radio_FilterStatusEnabled As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_FilterStatusDisabled As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_FiterStatusAll As System.Windows.Forms.RadioButton
    Friend WithEvents UserDAOBindingSource As BindingSource
    Friend WithEvents colUserName As DataGridViewTextBoxColumn
    Friend WithEvents colFullName As DataGridViewTextBoxColumn
    Friend WithEvents colTitle As DataGridViewTextBoxColumn
    Friend WithEvents colAccountEnabled As DataGridViewCheckBoxColumn
    Friend WithEvents colUserID As DataGridViewTextBoxColumn
End Class
