<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_EditUser
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_EditUser))
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Facilites")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Teams")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("All Facility Teams")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Facility/Team Associations")
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox_UserProperties = New System.Windows.Forms.GroupBox()
        Me.Button_SearchUser = New System.Windows.Forms.Button()
        Me.Label_Location = New System.Windows.Forms.Label()
        Me.TextBox_CoverPg = New System.Windows.Forms.TextBox()
        Me.TextBox_Printer = New System.Windows.Forms.TextBox()
        Me.ComboBox_Title = New System.Windows.Forms.ComboBox()
        Me.Label_CoverPage = New System.Windows.Forms.Label()
        Me.Label_Printer = New System.Windows.Forms.Label()
        Me.ComboBox_HomeLocation = New System.Windows.Forms.ComboBox()
        Me.TextBox_FaxNo = New System.Windows.Forms.TextBox()
        Me.TextBox_FullName = New System.Windows.Forms.TextBox()
        Me.TextBox_UserName = New System.Windows.Forms.TextBox()
        Me.TextBox_PhoneNo = New System.Windows.Forms.TextBox()
        Me.Label_FullName = New System.Windows.Forms.Label()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.Label_UserName = New System.Windows.Forms.Label()
        Me.TextBox_PagerEmail = New System.Windows.Forms.TextBox()
        Me.TextBox_Email = New System.Windows.Forms.TextBox()
        Me.Label_Email = New System.Windows.Forms.Label()
        Me.Label_PagerEmail = New System.Windows.Forms.Label()
        Me.Label_FaxNo = New System.Windows.Forms.Label()
        Me.Label_PhoneNo = New System.Windows.Forms.Label()
        Me.GroupBox_RolesIRMA = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Role_TaxAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_DeletePO = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_POEditor = New System.Windows.Forms.CheckBox()
        Me.Button_ViewRoleConflicts = New System.Windows.Forms.Button()
        Me.CheckBox_Role_Shrink = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_ShrinkAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Einvoicing = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_POApprovalAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_CostAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_DCAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_BatchBuildOnly = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_POAccountant = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Accountant = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Buyer = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_VendorAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_LockAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Warehouse = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_ItemAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Distributor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_FacilityCreditProcessor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_Coordinator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_PriceBatchProcessor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_InventoryAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_SuperUser = New System.Windows.Forms.CheckBox()
        Me.TabControl_SecuritySettings = New System.Windows.Forms.TabControl()
        Me.TabPage_RolesIRMA = New System.Windows.Forms.TabPage()
        Me.GroupBox_AdminRoles = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Role_POSInterfaceAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_UserMaintenance = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_SystemConfigurationAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_JobAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_AppConfigAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_SecurityAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_StoreAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Role_DataAdministrator = New System.Windows.Forms.CheckBox()
        Me.TabPage_RolesSLIM = New System.Windows.Forms.TabPage()
        Me.Group_RolesSLIM = New System.Windows.Forms.GroupBox()
        Me.CheckBox_RoleSLIMECommerce = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMScaleInfo = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMSecureQuery = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMAuthorizations = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMRetailCost = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMStoreSpecials = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMPushToIRMA = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMVendorRequest = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMItemRequest = New System.Windows.Forms.CheckBox()
        Me.CheckBox_RoleSLIMUserAdmin = New System.Windows.Forms.CheckBox()
        Me.TabPage_RolesPromo = New System.Windows.Forms.TabPage()
        Me.GroupBox_RolesPromo = New System.Windows.Forms.GroupBox()
        Me.Radio_RolePromoNone = New System.Windows.Forms.RadioButton()
        Me.Radio_RolePromoAdmin = New System.Windows.Forms.RadioButton()
        Me.Radio_RolePromoBuyer = New System.Windows.Forms.RadioButton()
        Me.TabPage_LocationTeams = New System.Windows.Forms.TabPage()
        Me.Group_LocationTeams = New System.Windows.Forms.GroupBox()
        Me.Button_AddAllUserStoreTeamTitle = New System.Windows.Forms.Button()
        Me.Button_RemoveUserStoreTeamTitle = New System.Windows.Forms.Button()
        Me.Button_AddUserStoreTeamTitle = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TreeView_DataTree = New System.Windows.Forms.TreeView()
        Me.TreeView_Locations = New System.Windows.Forms.TreeView()
        Me.TabPage_SubTeams = New System.Windows.Forms.TabPage()
        Me.Group_SubTeams = New System.Windows.Forms.GroupBox()
        Me.CheckBox_RemoveAllSubTeams = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AddAllSubTeams = New System.Windows.Forms.CheckBox()
        Me.Button_RemoveSubTeam = New System.Windows.Forms.Button()
        Me.TreeView_SubTeams = New System.Windows.Forms.TreeView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CheckBox_IsCoordinator = New System.Windows.Forms.CheckBox()
        Me.Button_AddSubTeam = New System.Windows.Forms.Button()
        Me.ComboBox_SubTeams = New System.Windows.Forms.ComboBox()
        Me.Form_ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Button_DisableAccount = New System.Windows.Forms.Button()
        Me.CheckBox_AcctEnabled = New System.Windows.Forms.CheckBox()
        Me.Label_ChangesHappenImmediately = New System.Windows.Forms.Label()
        Me.ttRoleDescription = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_UserProperties.SuspendLayout()
        Me.GroupBox_RolesIRMA.SuspendLayout()
        Me.TabControl_SecuritySettings.SuspendLayout()
        Me.TabPage_RolesIRMA.SuspendLayout()
        Me.GroupBox_AdminRoles.SuspendLayout()
        Me.TabPage_RolesSLIM.SuspendLayout()
        Me.Group_RolesSLIM.SuspendLayout()
        Me.TabPage_RolesPromo.SuspendLayout()
        Me.GroupBox_RolesPromo.SuspendLayout()
        Me.TabPage_LocationTeams.SuspendLayout()
        Me.Group_LocationTeams.SuspendLayout()
        Me.TabPage_SubTeams.SuspendLayout()
        Me.Group_SubTeams.SuspendLayout()
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Enabled = False
        Me.Button_Save.Image = CType(resources.GetObject("Button_Save.Image"), System.Drawing.Image)
        Me.Button_Save.Location = New System.Drawing.Point(573, 593)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(87, 31)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(482, 593)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(82, 31)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Close"
        Me.Button_Cancel.UseVisualStyleBackColor = True
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
        Me.GroupBox_UserProperties.Location = New System.Drawing.Point(9, 30)
        Me.GroupBox_UserProperties.Name = "GroupBox_UserProperties"
        Me.GroupBox_UserProperties.Size = New System.Drawing.Size(638, 165)
        Me.GroupBox_UserProperties.TabIndex = 6
        Me.GroupBox_UserProperties.TabStop = False
        Me.GroupBox_UserProperties.Text = "User Properties"
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
        Me.Label_Location.Size = New System.Drawing.Size(54, 13)
        Me.Label_Location.TabIndex = 20
        Me.Label_Location.Text = "Location:"
        '
        'TextBox_CoverPg
        '
        Me.TextBox_CoverPg.Location = New System.Drawing.Point(490, 76)
        Me.TextBox_CoverPg.MaxLength = 30
        Me.TextBox_CoverPg.Name = "TextBox_CoverPg"
        Me.TextBox_CoverPg.Size = New System.Drawing.Size(141, 22)
        Me.TextBox_CoverPg.TabIndex = 9
        Me.TextBox_CoverPg.Text = "default"
        Me.TextBox_CoverPg.UseWaitCursor = True
        '
        'TextBox_Printer
        '
        Me.TextBox_Printer.Location = New System.Drawing.Point(490, 50)
        Me.TextBox_Printer.MaxLength = 50
        Me.TextBox_Printer.Name = "TextBox_Printer"
        Me.TextBox_Printer.Size = New System.Drawing.Size(141, 22)
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
        Me.Label_CoverPage.Size = New System.Drawing.Size(67, 13)
        Me.Label_CoverPage.TabIndex = 15
        Me.Label_CoverPage.Text = "Cover Page:"
        '
        'Label_Printer
        '
        Me.Label_Printer.AutoSize = True
        Me.Label_Printer.Location = New System.Drawing.Point(444, 53)
        Me.Label_Printer.Name = "Label_Printer"
        Me.Label_Printer.Size = New System.Drawing.Size(44, 13)
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
        Me.TextBox_FaxNo.Size = New System.Drawing.Size(141, 22)
        Me.TextBox_FaxNo.TabIndex = 11
        '
        'TextBox_FullName
        '
        Me.TextBox_FullName.Location = New System.Drawing.Point(71, 51)
        Me.TextBox_FullName.MaxLength = 50
        Me.TextBox_FullName.Name = "TextBox_FullName"
        Me.TextBox_FullName.Size = New System.Drawing.Size(275, 22)
        Me.TextBox_FullName.TabIndex = 3
        '
        'TextBox_UserName
        '
        Me.TextBox_UserName.Location = New System.Drawing.Point(71, 25)
        Me.TextBox_UserName.MaxLength = 25
        Me.TextBox_UserName.Name = "TextBox_UserName"
        Me.TextBox_UserName.Size = New System.Drawing.Size(209, 22)
        Me.TextBox_UserName.TabIndex = 1
        '
        'TextBox_PhoneNo
        '
        Me.TextBox_PhoneNo.Location = New System.Drawing.Point(490, 24)
        Me.TextBox_PhoneNo.MaxLength = 25
        Me.TextBox_PhoneNo.Name = "TextBox_PhoneNo"
        Me.TextBox_PhoneNo.Size = New System.Drawing.Size(141, 22)
        Me.TextBox_PhoneNo.TabIndex = 7
        '
        'Label_FullName
        '
        Me.Label_FullName.AutoSize = True
        Me.Label_FullName.Location = New System.Drawing.Point(8, 56)
        Me.Label_FullName.Name = "Label_FullName"
        Me.Label_FullName.Size = New System.Drawing.Size(61, 13)
        Me.Label_FullName.TabIndex = 2
        Me.Label_FullName.Text = "Full Name:"
        '
        'Label_Title
        '
        Me.Label_Title.AutoSize = True
        Me.Label_Title.Location = New System.Drawing.Point(38, 106)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(31, 13)
        Me.Label_Title.TabIndex = 13
        Me.Label_Title.Text = "Title:"
        '
        'Label_UserName
        '
        Me.Label_UserName.AutoSize = True
        Me.Label_UserName.Location = New System.Drawing.Point(4, 30)
        Me.Label_UserName.Name = "Label_UserName"
        Me.Label_UserName.Size = New System.Drawing.Size(65, 13)
        Me.Label_UserName.TabIndex = 1
        Me.Label_UserName.Text = "User Name:"
        '
        'TextBox_PagerEmail
        '
        Me.TextBox_PagerEmail.Location = New System.Drawing.Point(490, 102)
        Me.TextBox_PagerEmail.MaxLength = 50
        Me.TextBox_PagerEmail.Name = "TextBox_PagerEmail"
        Me.TextBox_PagerEmail.Size = New System.Drawing.Size(141, 22)
        Me.TextBox_PagerEmail.TabIndex = 10
        '
        'TextBox_Email
        '
        Me.TextBox_Email.Location = New System.Drawing.Point(71, 77)
        Me.TextBox_Email.MaxLength = 50
        Me.TextBox_Email.Name = "TextBox_Email"
        Me.TextBox_Email.Size = New System.Drawing.Size(275, 22)
        Me.TextBox_Email.TabIndex = 4
        '
        'Label_Email
        '
        Me.Label_Email.AutoSize = True
        Me.Label_Email.Location = New System.Drawing.Point(32, 82)
        Me.Label_Email.Name = "Label_Email"
        Me.Label_Email.Size = New System.Drawing.Size(37, 13)
        Me.Label_Email.TabIndex = 5
        Me.Label_Email.Text = "Email:"
        '
        'Label_PagerEmail
        '
        Me.Label_PagerEmail.AutoSize = True
        Me.Label_PagerEmail.Location = New System.Drawing.Point(419, 106)
        Me.Label_PagerEmail.Name = "Label_PagerEmail"
        Me.Label_PagerEmail.Size = New System.Drawing.Size(69, 13)
        Me.Label_PagerEmail.TabIndex = 6
        Me.Label_PagerEmail.Text = "Pager Email:"
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
        Me.Label_PhoneNo.Size = New System.Drawing.Size(43, 13)
        Me.Label_PhoneNo.TabIndex = 7
        Me.Label_PhoneNo.Text = "Phone:"
        '
        'GroupBox_RolesIRMA
        '
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_TaxAdministrator)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_DeletePO)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_POEditor)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.Button_ViewRoleConflicts)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Shrink)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_ShrinkAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Einvoicing)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_POApprovalAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_VendorCostDiscrepancyAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_CostAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_DCAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_BatchBuildOnly)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_POAccountant)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Accountant)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Buyer)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_VendorAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_LockAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Warehouse)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_ItemAdmin)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Distributor)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_FacilityCreditProcessor)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_Coordinator)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_PriceBatchProcessor)
        Me.GroupBox_RolesIRMA.Controls.Add(Me.CheckBox_Role_InventoryAdmin)
        Me.GroupBox_RolesIRMA.Location = New System.Drawing.Point(8, 6)
        Me.GroupBox_RolesIRMA.Name = "GroupBox_RolesIRMA"
        Me.GroupBox_RolesIRMA.Size = New System.Drawing.Size(623, 207)
        Me.GroupBox_RolesIRMA.TabIndex = 9
        Me.GroupBox_RolesIRMA.TabStop = False
        Me.GroupBox_RolesIRMA.Text = "User Roles"
        '
        'CheckBox_Role_TaxAdministrator
        '
        Me.CheckBox_Role_TaxAdministrator.AutoSize = True
        Me.CheckBox_Role_TaxAdministrator.Location = New System.Drawing.Point(406, 88)
        Me.CheckBox_Role_TaxAdministrator.Name = "CheckBox_Role_TaxAdministrator"
        Me.CheckBox_Role_TaxAdministrator.Size = New System.Drawing.Size(115, 17)
        Me.CheckBox_Role_TaxAdministrator.TabIndex = 32
        Me.CheckBox_Role_TaxAdministrator.Text = "Tax Administrator"
        Me.CheckBox_Role_TaxAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_DeletePO
        '
        Me.CheckBox_Role_DeletePO.AutoSize = True
        Me.CheckBox_Role_DeletePO.Location = New System.Drawing.Point(8, 157)
        Me.CheckBox_Role_DeletePO.Name = "CheckBox_Role_DeletePO"
        Me.CheckBox_Role_DeletePO.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox_Role_DeletePO.TabIndex = 31
        Me.CheckBox_Role_DeletePO.Text = "Delete PO"
        Me.CheckBox_Role_DeletePO.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_POEditor
        '
        Me.CheckBox_Role_POEditor.AutoSize = True
        Me.CheckBox_Role_POEditor.Location = New System.Drawing.Point(210, 157)
        Me.CheckBox_Role_POEditor.Name = "CheckBox_Role_POEditor"
        Me.CheckBox_Role_POEditor.Size = New System.Drawing.Size(75, 17)
        Me.CheckBox_Role_POEditor.TabIndex = 30
        Me.CheckBox_Role_POEditor.Text = "PO Editor"
        Me.CheckBox_Role_POEditor.UseVisualStyleBackColor = True
        '
        'Button_ViewRoleConflicts
        '
        Me.Button_ViewRoleConflicts.ForeColor = System.Drawing.Color.Transparent
        Me.Button_ViewRoleConflicts.Image = CType(resources.GetObject("Button_ViewRoleConflicts.Image"), System.Drawing.Image)
        Me.Button_ViewRoleConflicts.Location = New System.Drawing.Point(578, 12)
        Me.Button_ViewRoleConflicts.Name = "Button_ViewRoleConflicts"
        Me.Button_ViewRoleConflicts.Size = New System.Drawing.Size(40, 32)
        Me.Button_ViewRoleConflicts.TabIndex = 29
        Me.Button_ViewRoleConflicts.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Shrink
        '
        Me.CheckBox_Role_Shrink.AutoSize = True
        Me.CheckBox_Role_Shrink.Location = New System.Drawing.Point(406, 42)
        Me.CheckBox_Role_Shrink.Name = "CheckBox_Role_Shrink"
        Me.CheckBox_Role_Shrink.Size = New System.Drawing.Size(59, 17)
        Me.CheckBox_Role_Shrink.TabIndex = 24
        Me.CheckBox_Role_Shrink.Text = "Shrink"
        Me.CheckBox_Role_Shrink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CheckBox_Role_Shrink.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_ShrinkAdmin
        '
        Me.CheckBox_Role_ShrinkAdmin.AutoSize = True
        Me.CheckBox_Role_ShrinkAdmin.Location = New System.Drawing.Point(406, 65)
        Me.CheckBox_Role_ShrinkAdmin.Name = "CheckBox_Role_ShrinkAdmin"
        Me.CheckBox_Role_ShrinkAdmin.Size = New System.Drawing.Size(132, 17)
        Me.CheckBox_Role_ShrinkAdmin.TabIndex = 23
        Me.CheckBox_Role_ShrinkAdmin.Text = "Shrink Administrator"
        Me.CheckBox_Role_ShrinkAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Einvoicing
        '
        Me.CheckBox_Role_Einvoicing.AutoSize = True
        Me.CheckBox_Role_Einvoicing.Location = New System.Drawing.Point(8, 180)
        Me.CheckBox_Role_Einvoicing.Name = "CheckBox_Role_Einvoicing"
        Me.CheckBox_Role_Einvoicing.Size = New System.Drawing.Size(83, 17)
        Me.CheckBox_Role_Einvoicing.TabIndex = 21
        Me.CheckBox_Role_Einvoicing.Text = "E-Invoicing"
        Me.CheckBox_Role_Einvoicing.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_POApprovalAdmin
        '
        Me.CheckBox_Role_POApprovalAdmin.AutoSize = True
        Me.CheckBox_Role_POApprovalAdmin.Location = New System.Drawing.Point(210, 134)
        Me.CheckBox_Role_POApprovalAdmin.Name = "CheckBox_Role_POApprovalAdmin"
        Me.CheckBox_Role_POApprovalAdmin.Size = New System.Drawing.Size(163, 17)
        Me.CheckBox_Role_POApprovalAdmin.TabIndex = 22
        Me.CheckBox_Role_POApprovalAdmin.Text = "PO Approval Administrator"
        Me.CheckBox_Role_POApprovalAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_VendorCostDiscrepancyAdmin
        '
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.AutoSize = True
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.Location = New System.Drawing.Point(406, 134)
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.Name = "CheckBox_Role_VendorCostDiscrepancyAdmin"
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.Size = New System.Drawing.Size(190, 17)
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.TabIndex = 21
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.Text = "Vendor Cost Discrepancy Admin"
        Me.CheckBox_Role_VendorCostDiscrepancyAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_CostAdmin
        '
        Me.CheckBox_Role_CostAdmin.AutoSize = True
        Me.CheckBox_Role_CostAdmin.Location = New System.Drawing.Point(8, 111)
        Me.CheckBox_Role_CostAdmin.Name = "CheckBox_Role_CostAdmin"
        Me.CheckBox_Role_CostAdmin.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox_Role_CostAdmin.TabIndex = 6
        Me.CheckBox_Role_CostAdmin.Text = "Cost Administrator"
        Me.CheckBox_Role_CostAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_DCAdmin
        '
        Me.CheckBox_Role_DCAdmin.AutoSize = True
        Me.CheckBox_Role_DCAdmin.Location = New System.Drawing.Point(8, 134)
        Me.CheckBox_Role_DCAdmin.Name = "CheckBox_Role_DCAdmin"
        Me.CheckBox_Role_DCAdmin.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox_Role_DCAdmin.TabIndex = 8
        Me.CheckBox_Role_DCAdmin.Text = "DC Admin"
        Me.CheckBox_Role_DCAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_BatchBuildOnly
        '
        Me.CheckBox_Role_BatchBuildOnly.AutoSize = True
        Me.CheckBox_Role_BatchBuildOnly.Location = New System.Drawing.Point(8, 42)
        Me.CheckBox_Role_BatchBuildOnly.Name = "CheckBox_Role_BatchBuildOnly"
        Me.CheckBox_Role_BatchBuildOnly.Size = New System.Drawing.Size(112, 17)
        Me.CheckBox_Role_BatchBuildOnly.TabIndex = 2
        Me.CheckBox_Role_BatchBuildOnly.Text = "Batch Build Only"
        Me.CheckBox_Role_BatchBuildOnly.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_POAccountant
        '
        Me.CheckBox_Role_POAccountant.AutoSize = True
        Me.CheckBox_Role_POAccountant.Location = New System.Drawing.Point(210, 111)
        Me.CheckBox_Role_POAccountant.Name = "CheckBox_Role_POAccountant"
        Me.CheckBox_Role_POAccountant.Size = New System.Drawing.Size(103, 17)
        Me.CheckBox_Role_POAccountant.TabIndex = 14
        Me.CheckBox_Role_POAccountant.Text = "PO Accountant"
        Me.CheckBox_Role_POAccountant.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Accountant
        '
        Me.CheckBox_Role_Accountant.AutoSize = True
        Me.CheckBox_Role_Accountant.Location = New System.Drawing.Point(8, 19)
        Me.CheckBox_Role_Accountant.Name = "CheckBox_Role_Accountant"
        Me.CheckBox_Role_Accountant.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_Role_Accountant.TabIndex = 0
        Me.CheckBox_Role_Accountant.Text = "Accountant"
        Me.CheckBox_Role_Accountant.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Buyer
        '
        Me.CheckBox_Role_Buyer.AutoSize = True
        Me.CheckBox_Role_Buyer.Location = New System.Drawing.Point(8, 65)
        Me.CheckBox_Role_Buyer.Name = "CheckBox_Role_Buyer"
        Me.CheckBox_Role_Buyer.Size = New System.Drawing.Size(55, 17)
        Me.CheckBox_Role_Buyer.TabIndex = 4
        Me.CheckBox_Role_Buyer.Text = "Buyer"
        Me.CheckBox_Role_Buyer.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_VendorAdmin
        '
        Me.CheckBox_Role_VendorAdmin.AutoSize = True
        Me.CheckBox_Role_VendorAdmin.Location = New System.Drawing.Point(406, 111)
        Me.CheckBox_Role_VendorAdmin.Name = "CheckBox_Role_VendorAdmin"
        Me.CheckBox_Role_VendorAdmin.Size = New System.Drawing.Size(137, 17)
        Me.CheckBox_Role_VendorAdmin.TabIndex = 19
        Me.CheckBox_Role_VendorAdmin.Text = "Vendor Administrator"
        Me.CheckBox_Role_VendorAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_LockAdmin
        '
        Me.CheckBox_Role_LockAdmin.AutoSize = True
        Me.CheckBox_Role_LockAdmin.Location = New System.Drawing.Point(210, 88)
        Me.CheckBox_Role_LockAdmin.Name = "CheckBox_Role_LockAdmin"
        Me.CheckBox_Role_LockAdmin.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox_Role_LockAdmin.TabIndex = 12
        Me.CheckBox_Role_LockAdmin.Text = "Lock Administrator"
        Me.CheckBox_Role_LockAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Warehouse
        '
        Me.CheckBox_Role_Warehouse.AutoSize = True
        Me.CheckBox_Role_Warehouse.Location = New System.Drawing.Point(407, 157)
        Me.CheckBox_Role_Warehouse.Name = "CheckBox_Role_Warehouse"
        Me.CheckBox_Role_Warehouse.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_Role_Warehouse.TabIndex = 20
        Me.CheckBox_Role_Warehouse.Text = "Warehouse"
        Me.CheckBox_Role_Warehouse.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_ItemAdmin
        '
        Me.CheckBox_Role_ItemAdmin.AutoSize = True
        Me.CheckBox_Role_ItemAdmin.Location = New System.Drawing.Point(210, 65)
        Me.CheckBox_Role_ItemAdmin.Name = "CheckBox_Role_ItemAdmin"
        Me.CheckBox_Role_ItemAdmin.Size = New System.Drawing.Size(121, 17)
        Me.CheckBox_Role_ItemAdmin.TabIndex = 11
        Me.CheckBox_Role_ItemAdmin.Text = "Item Administrator"
        Me.CheckBox_Role_ItemAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Distributor
        '
        Me.CheckBox_Role_Distributor.AutoSize = True
        Me.CheckBox_Role_Distributor.Location = New System.Drawing.Point(407, 19)
        Me.CheckBox_Role_Distributor.Name = "CheckBox_Role_Distributor"
        Me.CheckBox_Role_Distributor.Size = New System.Drawing.Size(68, 17)
        Me.CheckBox_Role_Distributor.TabIndex = 16
        Me.CheckBox_Role_Distributor.Text = "Receiver"
        Me.CheckBox_Role_Distributor.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_FacilityCreditProcessor
        '
        Me.CheckBox_Role_FacilityCreditProcessor.AutoSize = True
        Me.CheckBox_Role_FacilityCreditProcessor.Location = New System.Drawing.Point(210, 19)
        Me.CheckBox_Role_FacilityCreditProcessor.Name = "CheckBox_Role_FacilityCreditProcessor"
        Me.CheckBox_Role_FacilityCreditProcessor.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox_Role_FacilityCreditProcessor.TabIndex = 7
        Me.CheckBox_Role_FacilityCreditProcessor.Text = "Facility Credit Processor"
        Me.CheckBox_Role_FacilityCreditProcessor.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_Coordinator
        '
        Me.CheckBox_Role_Coordinator.AutoSize = True
        Me.CheckBox_Role_Coordinator.Location = New System.Drawing.Point(8, 88)
        Me.CheckBox_Role_Coordinator.Name = "CheckBox_Role_Coordinator"
        Me.CheckBox_Role_Coordinator.Size = New System.Drawing.Size(89, 17)
        Me.CheckBox_Role_Coordinator.TabIndex = 5
        Me.CheckBox_Role_Coordinator.Text = "Coordinator"
        Me.CheckBox_Role_Coordinator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_PriceBatchProcessor
        '
        Me.CheckBox_Role_PriceBatchProcessor.AutoSize = True
        Me.CheckBox_Role_PriceBatchProcessor.Location = New System.Drawing.Point(210, 180)
        Me.CheckBox_Role_PriceBatchProcessor.Name = "CheckBox_Role_PriceBatchProcessor"
        Me.CheckBox_Role_PriceBatchProcessor.Size = New System.Drawing.Size(134, 17)
        Me.CheckBox_Role_PriceBatchProcessor.TabIndex = 15
        Me.CheckBox_Role_PriceBatchProcessor.Text = "Price Batch Processor"
        Me.CheckBox_Role_PriceBatchProcessor.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_InventoryAdmin
        '
        Me.CheckBox_Role_InventoryAdmin.AutoSize = True
        Me.CheckBox_Role_InventoryAdmin.Location = New System.Drawing.Point(210, 42)
        Me.CheckBox_Role_InventoryAdmin.Name = "CheckBox_Role_InventoryAdmin"
        Me.CheckBox_Role_InventoryAdmin.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox_Role_InventoryAdmin.TabIndex = 10
        Me.CheckBox_Role_InventoryAdmin.Text = "Inventory Administrator"
        Me.CheckBox_Role_InventoryAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_SuperUser
        '
        Me.CheckBox_Role_SuperUser.AutoSize = True
        Me.CheckBox_Role_SuperUser.Location = New System.Drawing.Point(406, 20)
        Me.CheckBox_Role_SuperUser.Name = "CheckBox_Role_SuperUser"
        Me.CheckBox_Role_SuperUser.Size = New System.Drawing.Size(82, 17)
        Me.CheckBox_Role_SuperUser.TabIndex = 18
        Me.CheckBox_Role_SuperUser.Text = "Super User"
        Me.CheckBox_Role_SuperUser.UseVisualStyleBackColor = True
        '
        'TabControl_SecuritySettings
        '
        Me.TabControl_SecuritySettings.Controls.Add(Me.TabPage_RolesIRMA)
        Me.TabControl_SecuritySettings.Controls.Add(Me.TabPage_RolesSLIM)
        Me.TabControl_SecuritySettings.Controls.Add(Me.TabPage_RolesPromo)
        Me.TabControl_SecuritySettings.Controls.Add(Me.TabPage_LocationTeams)
        Me.TabControl_SecuritySettings.Controls.Add(Me.TabPage_SubTeams)
        Me.TabControl_SecuritySettings.Location = New System.Drawing.Point(12, 201)
        Me.TabControl_SecuritySettings.Name = "TabControl_SecuritySettings"
        Me.TabControl_SecuritySettings.SelectedIndex = 0
        Me.TabControl_SecuritySettings.Size = New System.Drawing.Size(648, 386)
        Me.TabControl_SecuritySettings.TabIndex = 10
        '
        'TabPage_RolesIRMA
        '
        Me.TabPage_RolesIRMA.Controls.Add(Me.GroupBox_AdminRoles)
        Me.TabPage_RolesIRMA.Controls.Add(Me.GroupBox_RolesIRMA)
        Me.TabPage_RolesIRMA.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_RolesIRMA.Name = "TabPage_RolesIRMA"
        Me.TabPage_RolesIRMA.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_RolesIRMA.Size = New System.Drawing.Size(640, 360)
        Me.TabPage_RolesIRMA.TabIndex = 0
        Me.TabPage_RolesIRMA.Text = "IRMA Roles"
        Me.TabPage_RolesIRMA.UseVisualStyleBackColor = True
        '
        'GroupBox_AdminRoles
        '
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_POSInterfaceAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_UserMaintenance)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_SystemConfigurationAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_JobAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_AppConfigAdmin)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_SecurityAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_StoreAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_DataAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_Role_SuperUser)
        Me.GroupBox_AdminRoles.Location = New System.Drawing.Point(8, 230)
        Me.GroupBox_AdminRoles.Name = "GroupBox_AdminRoles"
        Me.GroupBox_AdminRoles.Size = New System.Drawing.Size(623, 95)
        Me.GroupBox_AdminRoles.TabIndex = 24
        Me.GroupBox_AdminRoles.TabStop = False
        Me.GroupBox_AdminRoles.Text = "Admin Roles"
        '
        'CheckBox_Role_POSInterfaceAdministrator
        '
        Me.CheckBox_Role_POSInterfaceAdministrator.AutoSize = True
        Me.CheckBox_Role_POSInterfaceAdministrator.Location = New System.Drawing.Point(210, 20)
        Me.CheckBox_Role_POSInterfaceAdministrator.Name = "CheckBox_Role_POSInterfaceAdministrator"
        Me.CheckBox_Role_POSInterfaceAdministrator.Size = New System.Drawing.Size(168, 17)
        Me.CheckBox_Role_POSInterfaceAdministrator.TabIndex = 9
        Me.CheckBox_Role_POSInterfaceAdministrator.Text = "POS Interface Administrator"
        Me.CheckBox_Role_POSInterfaceAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_UserMaintenance
        '
        Me.CheckBox_Role_UserMaintenance.AutoSize = True
        Me.CheckBox_Role_UserMaintenance.Location = New System.Drawing.Point(406, 66)
        Me.CheckBox_Role_UserMaintenance.Name = "CheckBox_Role_UserMaintenance"
        Me.CheckBox_Role_UserMaintenance.Size = New System.Drawing.Size(119, 17)
        Me.CheckBox_Role_UserMaintenance.TabIndex = 8
        Me.CheckBox_Role_UserMaintenance.Text = "User Maintenance"
        Me.CheckBox_Role_UserMaintenance.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_SystemConfigurationAdministrator
        '
        Me.CheckBox_Role_SystemConfigurationAdministrator.AutoSize = True
        Me.CheckBox_Role_SystemConfigurationAdministrator.Location = New System.Drawing.Point(406, 43)
        Me.CheckBox_Role_SystemConfigurationAdministrator.Name = "CheckBox_Role_SystemConfigurationAdministrator"
        Me.CheckBox_Role_SystemConfigurationAdministrator.Size = New System.Drawing.Size(210, 17)
        Me.CheckBox_Role_SystemConfigurationAdministrator.TabIndex = 6
        Me.CheckBox_Role_SystemConfigurationAdministrator.Text = "System Configuration Administrator"
        Me.CheckBox_Role_SystemConfigurationAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_JobAdministrator
        '
        Me.CheckBox_Role_JobAdministrator.AutoSize = True
        Me.CheckBox_Role_JobAdministrator.Location = New System.Drawing.Point(8, 66)
        Me.CheckBox_Role_JobAdministrator.Name = "CheckBox_Role_JobAdministrator"
        Me.CheckBox_Role_JobAdministrator.Size = New System.Drawing.Size(117, 17)
        Me.CheckBox_Role_JobAdministrator.TabIndex = 5
        Me.CheckBox_Role_JobAdministrator.Text = "Job Administrator"
        Me.CheckBox_Role_JobAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_AppConfigAdmin
        '
        Me.CheckBox_Role_AppConfigAdmin.AutoSize = True
        Me.CheckBox_Role_AppConfigAdmin.Location = New System.Drawing.Point(8, 20)
        Me.CheckBox_Role_AppConfigAdmin.Name = "CheckBox_Role_AppConfigAdmin"
        Me.CheckBox_Role_AppConfigAdmin.Size = New System.Drawing.Size(197, 17)
        Me.CheckBox_Role_AppConfigAdmin.TabIndex = 4
        Me.CheckBox_Role_AppConfigAdmin.Text = "Application Configuration Admin"
        Me.CheckBox_Role_AppConfigAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_SecurityAdministrator
        '
        Me.CheckBox_Role_SecurityAdministrator.AutoSize = True
        Me.CheckBox_Role_SecurityAdministrator.Location = New System.Drawing.Point(210, 43)
        Me.CheckBox_Role_SecurityAdministrator.Name = "CheckBox_Role_SecurityAdministrator"
        Me.CheckBox_Role_SecurityAdministrator.Size = New System.Drawing.Size(139, 17)
        Me.CheckBox_Role_SecurityAdministrator.TabIndex = 3
        Me.CheckBox_Role_SecurityAdministrator.Text = "Security Administrator"
        Me.CheckBox_Role_SecurityAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_StoreAdministrator
        '
        Me.CheckBox_Role_StoreAdministrator.AutoSize = True
        Me.CheckBox_Role_StoreAdministrator.Location = New System.Drawing.Point(210, 66)
        Me.CheckBox_Role_StoreAdministrator.Name = "CheckBox_Role_StoreAdministrator"
        Me.CheckBox_Role_StoreAdministrator.Size = New System.Drawing.Size(126, 17)
        Me.CheckBox_Role_StoreAdministrator.TabIndex = 2
        Me.CheckBox_Role_StoreAdministrator.Text = "Store Administrator"
        Me.CheckBox_Role_StoreAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_Role_DataAdministrator
        '
        Me.CheckBox_Role_DataAdministrator.AutoSize = True
        Me.CheckBox_Role_DataAdministrator.Location = New System.Drawing.Point(7, 43)
        Me.CheckBox_Role_DataAdministrator.Name = "CheckBox_Role_DataAdministrator"
        Me.CheckBox_Role_DataAdministrator.Size = New System.Drawing.Size(123, 17)
        Me.CheckBox_Role_DataAdministrator.TabIndex = 1
        Me.CheckBox_Role_DataAdministrator.Text = "Data Administrator"
        Me.ttRoleDescription.SetToolTip(Me.CheckBox_Role_DataAdministrator, "Data Administrator can access the following Admin screens:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "     - Batch Rollback" & _
        "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "     - Restore Deleted Item" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "     - Scale/POS Push")
        Me.CheckBox_Role_DataAdministrator.UseVisualStyleBackColor = True
        '
        'TabPage_RolesSLIM
        '
        Me.TabPage_RolesSLIM.Controls.Add(Me.Group_RolesSLIM)
        Me.TabPage_RolesSLIM.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_RolesSLIM.Name = "TabPage_RolesSLIM"
        Me.TabPage_RolesSLIM.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_RolesSLIM.Size = New System.Drawing.Size(640, 360)
        Me.TabPage_RolesSLIM.TabIndex = 1
        Me.TabPage_RolesSLIM.Text = "SLIM Access"
        Me.TabPage_RolesSLIM.UseVisualStyleBackColor = True
        '
        'Group_RolesSLIM
        '
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMECommerce)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMScaleInfo)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMSecureQuery)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMAuthorizations)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMRetailCost)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMStoreSpecials)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMPushToIRMA)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMVendorRequest)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMItemRequest)
        Me.Group_RolesSLIM.Controls.Add(Me.CheckBox_RoleSLIMUserAdmin)
        Me.Group_RolesSLIM.Location = New System.Drawing.Point(8, 6)
        Me.Group_RolesSLIM.Name = "Group_RolesSLIM"
        Me.Group_RolesSLIM.Size = New System.Drawing.Size(617, 324)
        Me.Group_RolesSLIM.TabIndex = 4
        Me.Group_RolesSLIM.TabStop = False
        '
        'CheckBox_RoleSLIMECommerce
        '
        Me.CheckBox_RoleSLIMECommerce.AutoSize = True
        Me.CheckBox_RoleSLIMECommerce.Location = New System.Drawing.Point(16, 90)
        Me.CheckBox_RoleSLIMECommerce.Name = "CheckBox_RoleSLIMECommerce"
        Me.CheckBox_RoleSLIMECommerce.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_RoleSLIMECommerce.TabIndex = 9
        Me.CheckBox_RoleSLIMECommerce.Text = "ECommerce"
        Me.CheckBox_RoleSLIMECommerce.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMScaleInfo
        '
        Me.CheckBox_RoleSLIMScaleInfo.AutoSize = True
        Me.CheckBox_RoleSLIMScaleInfo.Location = New System.Drawing.Point(161, 42)
        Me.CheckBox_RoleSLIMScaleInfo.Name = "CheckBox_RoleSLIMScaleInfo"
        Me.CheckBox_RoleSLIMScaleInfo.Size = New System.Drawing.Size(76, 17)
        Me.CheckBox_RoleSLIMScaleInfo.TabIndex = 4
        Me.CheckBox_RoleSLIMScaleInfo.Text = "Scale Info"
        Me.CheckBox_RoleSLIMScaleInfo.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMSecureQuery
        '
        Me.CheckBox_RoleSLIMSecureQuery.AutoSize = True
        Me.CheckBox_RoleSLIMSecureQuery.Location = New System.Drawing.Point(292, 42)
        Me.CheckBox_RoleSLIMSecureQuery.Name = "CheckBox_RoleSLIMSecureQuery"
        Me.CheckBox_RoleSLIMSecureQuery.Size = New System.Drawing.Size(121, 17)
        Me.CheckBox_RoleSLIMSecureQuery.TabIndex = 7
        Me.CheckBox_RoleSLIMSecureQuery.Text = "Secure Web Query"
        Me.CheckBox_RoleSLIMSecureQuery.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMAuthorizations
        '
        Me.CheckBox_RoleSLIMAuthorizations.AutoSize = True
        Me.CheckBox_RoleSLIMAuthorizations.Location = New System.Drawing.Point(161, 19)
        Me.CheckBox_RoleSLIMAuthorizations.Name = "CheckBox_RoleSLIMAuthorizations"
        Me.CheckBox_RoleSLIMAuthorizations.Size = New System.Drawing.Size(102, 17)
        Me.CheckBox_RoleSLIMAuthorizations.TabIndex = 3
        Me.CheckBox_RoleSLIMAuthorizations.Text = "Authorizations"
        Me.CheckBox_RoleSLIMAuthorizations.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMRetailCost
        '
        Me.CheckBox_RoleSLIMRetailCost.AutoSize = True
        Me.CheckBox_RoleSLIMRetailCost.Location = New System.Drawing.Point(161, 65)
        Me.CheckBox_RoleSLIMRetailCost.Name = "CheckBox_RoleSLIMRetailCost"
        Me.CheckBox_RoleSLIMRetailCost.Size = New System.Drawing.Size(81, 17)
        Me.CheckBox_RoleSLIMRetailCost.TabIndex = 5
        Me.CheckBox_RoleSLIMRetailCost.Text = "Retail Cost"
        Me.CheckBox_RoleSLIMRetailCost.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMStoreSpecials
        '
        Me.CheckBox_RoleSLIMStoreSpecials.AutoSize = True
        Me.CheckBox_RoleSLIMStoreSpecials.Location = New System.Drawing.Point(15, 65)
        Me.CheckBox_RoleSLIMStoreSpecials.Name = "CheckBox_RoleSLIMStoreSpecials"
        Me.CheckBox_RoleSLIMStoreSpecials.Size = New System.Drawing.Size(97, 17)
        Me.CheckBox_RoleSLIMStoreSpecials.TabIndex = 2
        Me.CheckBox_RoleSLIMStoreSpecials.Text = "Store Specials"
        Me.CheckBox_RoleSLIMStoreSpecials.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMPushToIRMA
        '
        Me.CheckBox_RoleSLIMPushToIRMA.AutoSize = True
        Me.CheckBox_RoleSLIMPushToIRMA.Location = New System.Drawing.Point(292, 19)
        Me.CheckBox_RoleSLIMPushToIRMA.Name = "CheckBox_RoleSLIMPushToIRMA"
        Me.CheckBox_RoleSLIMPushToIRMA.Size = New System.Drawing.Size(81, 17)
        Me.CheckBox_RoleSLIMPushToIRMA.TabIndex = 6
        Me.CheckBox_RoleSLIMPushToIRMA.Text = "IRMA Push"
        Me.CheckBox_RoleSLIMPushToIRMA.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMVendorRequest
        '
        Me.CheckBox_RoleSLIMVendorRequest.AutoSize = True
        Me.CheckBox_RoleSLIMVendorRequest.Location = New System.Drawing.Point(15, 42)
        Me.CheckBox_RoleSLIMVendorRequest.Name = "CheckBox_RoleSLIMVendorRequest"
        Me.CheckBox_RoleSLIMVendorRequest.Size = New System.Drawing.Size(109, 17)
        Me.CheckBox_RoleSLIMVendorRequest.TabIndex = 1
        Me.CheckBox_RoleSLIMVendorRequest.Text = "Vendor Request"
        Me.CheckBox_RoleSLIMVendorRequest.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMItemRequest
        '
        Me.CheckBox_RoleSLIMItemRequest.AutoSize = True
        Me.CheckBox_RoleSLIMItemRequest.Location = New System.Drawing.Point(15, 19)
        Me.CheckBox_RoleSLIMItemRequest.Name = "CheckBox_RoleSLIMItemRequest"
        Me.CheckBox_RoleSLIMItemRequest.Size = New System.Drawing.Size(93, 17)
        Me.CheckBox_RoleSLIMItemRequest.TabIndex = 0
        Me.CheckBox_RoleSLIMItemRequest.Text = "Item Request"
        Me.CheckBox_RoleSLIMItemRequest.UseVisualStyleBackColor = True
        '
        'CheckBox_RoleSLIMUserAdmin
        '
        Me.CheckBox_RoleSLIMUserAdmin.AutoSize = True
        Me.CheckBox_RoleSLIMUserAdmin.Location = New System.Drawing.Point(292, 65)
        Me.CheckBox_RoleSLIMUserAdmin.Name = "CheckBox_RoleSLIMUserAdmin"
        Me.CheckBox_RoleSLIMUserAdmin.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_RoleSLIMUserAdmin.TabIndex = 8
        Me.CheckBox_RoleSLIMUserAdmin.Text = "User Admin"
        Me.CheckBox_RoleSLIMUserAdmin.UseVisualStyleBackColor = True
        '
        'TabPage_RolesPromo
        '
        Me.TabPage_RolesPromo.Controls.Add(Me.GroupBox_RolesPromo)
        Me.TabPage_RolesPromo.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_RolesPromo.Name = "TabPage_RolesPromo"
        Me.TabPage_RolesPromo.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_RolesPromo.Size = New System.Drawing.Size(640, 360)
        Me.TabPage_RolesPromo.TabIndex = 2
        Me.TabPage_RolesPromo.Text = "Promo Planner Roles"
        Me.TabPage_RolesPromo.UseVisualStyleBackColor = True
        '
        'GroupBox_RolesPromo
        '
        Me.GroupBox_RolesPromo.Controls.Add(Me.Radio_RolePromoNone)
        Me.GroupBox_RolesPromo.Controls.Add(Me.Radio_RolePromoAdmin)
        Me.GroupBox_RolesPromo.Controls.Add(Me.Radio_RolePromoBuyer)
        Me.GroupBox_RolesPromo.Location = New System.Drawing.Point(8, 6)
        Me.GroupBox_RolesPromo.Name = "GroupBox_RolesPromo"
        Me.GroupBox_RolesPromo.Size = New System.Drawing.Size(617, 324)
        Me.GroupBox_RolesPromo.TabIndex = 3
        Me.GroupBox_RolesPromo.TabStop = False
        '
        'Radio_RolePromoNone
        '
        Me.Radio_RolePromoNone.AutoSize = True
        Me.Radio_RolePromoNone.Location = New System.Drawing.Point(17, 67)
        Me.Radio_RolePromoNone.Name = "Radio_RolePromoNone"
        Me.Radio_RolePromoNone.Size = New System.Drawing.Size(53, 17)
        Me.Radio_RolePromoNone.TabIndex = 2
        Me.Radio_RolePromoNone.TabStop = True
        Me.Radio_RolePromoNone.Text = "None"
        Me.Radio_RolePromoNone.UseVisualStyleBackColor = True
        '
        'Radio_RolePromoAdmin
        '
        Me.Radio_RolePromoAdmin.AutoSize = True
        Me.Radio_RolePromoAdmin.Location = New System.Drawing.Point(17, 44)
        Me.Radio_RolePromoAdmin.Name = "Radio_RolePromoAdmin"
        Me.Radio_RolePromoAdmin.Size = New System.Drawing.Size(128, 17)
        Me.Radio_RolePromoAdmin.TabIndex = 1
        Me.Radio_RolePromoAdmin.TabStop = True
        Me.Radio_RolePromoAdmin.Text = "Order Administrator"
        Me.Radio_RolePromoAdmin.UseVisualStyleBackColor = True
        '
        'Radio_RolePromoBuyer
        '
        Me.Radio_RolePromoBuyer.AutoSize = True
        Me.Radio_RolePromoBuyer.Location = New System.Drawing.Point(17, 21)
        Me.Radio_RolePromoBuyer.Name = "Radio_RolePromoBuyer"
        Me.Radio_RolePromoBuyer.Size = New System.Drawing.Size(84, 17)
        Me.Radio_RolePromoBuyer.TabIndex = 0
        Me.Radio_RolePromoBuyer.TabStop = True
        Me.Radio_RolePromoBuyer.Text = "Store Buyer"
        Me.Radio_RolePromoBuyer.UseVisualStyleBackColor = True
        '
        'TabPage_LocationTeams
        '
        Me.TabPage_LocationTeams.Controls.Add(Me.Group_LocationTeams)
        Me.TabPage_LocationTeams.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_LocationTeams.Name = "TabPage_LocationTeams"
        Me.TabPage_LocationTeams.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_LocationTeams.Size = New System.Drawing.Size(640, 360)
        Me.TabPage_LocationTeams.TabIndex = 3
        Me.TabPage_LocationTeams.Text = "Location Teams"
        Me.TabPage_LocationTeams.UseVisualStyleBackColor = True
        '
        'Group_LocationTeams
        '
        Me.Group_LocationTeams.Controls.Add(Me.Button_AddAllUserStoreTeamTitle)
        Me.Group_LocationTeams.Controls.Add(Me.Button_RemoveUserStoreTeamTitle)
        Me.Group_LocationTeams.Controls.Add(Me.Button_AddUserStoreTeamTitle)
        Me.Group_LocationTeams.Controls.Add(Me.Label2)
        Me.Group_LocationTeams.Controls.Add(Me.Label1)
        Me.Group_LocationTeams.Controls.Add(Me.TreeView_DataTree)
        Me.Group_LocationTeams.Controls.Add(Me.TreeView_Locations)
        Me.Group_LocationTeams.Location = New System.Drawing.Point(9, 6)
        Me.Group_LocationTeams.Name = "Group_LocationTeams"
        Me.Group_LocationTeams.Size = New System.Drawing.Size(616, 324)
        Me.Group_LocationTeams.TabIndex = 8
        Me.Group_LocationTeams.TabStop = False
        '
        'Button_AddAllUserStoreTeamTitle
        '
        Me.Button_AddAllUserStoreTeamTitle.Enabled = False
        Me.Button_AddAllUserStoreTeamTitle.Image = CType(resources.GetObject("Button_AddAllUserStoreTeamTitle.Image"), System.Drawing.Image)
        Me.Button_AddAllUserStoreTeamTitle.Location = New System.Drawing.Point(251, 96)
        Me.Button_AddAllUserStoreTeamTitle.Name = "Button_AddAllUserStoreTeamTitle"
        Me.Button_AddAllUserStoreTeamTitle.Size = New System.Drawing.Size(40, 32)
        Me.Button_AddAllUserStoreTeamTitle.TabIndex = 27
        Me.Button_AddAllUserStoreTeamTitle.UseVisualStyleBackColor = True
        '
        'Button_RemoveUserStoreTeamTitle
        '
        Me.Button_RemoveUserStoreTeamTitle.Enabled = False
        Me.Button_RemoveUserStoreTeamTitle.Image = CType(resources.GetObject("Button_RemoveUserStoreTeamTitle.Image"), System.Drawing.Image)
        Me.Button_RemoveUserStoreTeamTitle.Location = New System.Drawing.Point(539, 58)
        Me.Button_RemoveUserStoreTeamTitle.Name = "Button_RemoveUserStoreTeamTitle"
        Me.Button_RemoveUserStoreTeamTitle.Size = New System.Drawing.Size(40, 32)
        Me.Button_RemoveUserStoreTeamTitle.TabIndex = 26
        Me.Button_RemoveUserStoreTeamTitle.UseVisualStyleBackColor = True
        '
        'Button_AddUserStoreTeamTitle
        '
        Me.Button_AddUserStoreTeamTitle.Enabled = False
        Me.Button_AddUserStoreTeamTitle.Image = CType(resources.GetObject("Button_AddUserStoreTeamTitle.Image"), System.Drawing.Image)
        Me.Button_AddUserStoreTeamTitle.Location = New System.Drawing.Point(251, 58)
        Me.Button_AddUserStoreTeamTitle.Name = "Button_AddUserStoreTeamTitle"
        Me.Button_AddUserStoreTeamTitle.Size = New System.Drawing.Size(40, 32)
        Me.Button_AddUserStoreTeamTitle.TabIndex = 24
        Me.Button_AddUserStoreTeamTitle.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(294, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(131, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "User's Facilities && Teams"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Available Facilities && Teams"
        '
        'TreeView_DataTree
        '
        Me.TreeView_DataTree.HideSelection = False
        Me.TreeView_DataTree.Location = New System.Drawing.Point(6, 35)
        Me.TreeView_DataTree.Name = "TreeView_DataTree"
        TreeNode1.Name = "_node0"
        TreeNode1.Text = "Facilites"
        TreeNode2.Name = "_node1"
        TreeNode2.Text = "Teams"
        Me.TreeView_DataTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Me.TreeView_DataTree.Size = New System.Drawing.Size(239, 283)
        Me.TreeView_DataTree.TabIndex = 21
        '
        'TreeView_Locations
        '
        Me.TreeView_Locations.Location = New System.Drawing.Point(297, 35)
        Me.TreeView_Locations.Name = "TreeView_Locations"
        TreeNode3.Name = "_node3"
        TreeNode3.Text = "All Facility Teams"
        TreeNode4.Name = "_node4"
        TreeNode4.Text = "Facility/Team Associations"
        Me.TreeView_Locations.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3, TreeNode4})
        Me.TreeView_Locations.Size = New System.Drawing.Size(236, 283)
        Me.TreeView_Locations.TabIndex = 20
        '
        'TabPage_SubTeams
        '
        Me.TabPage_SubTeams.Controls.Add(Me.Group_SubTeams)
        Me.TabPage_SubTeams.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_SubTeams.Name = "TabPage_SubTeams"
        Me.TabPage_SubTeams.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_SubTeams.Size = New System.Drawing.Size(640, 360)
        Me.TabPage_SubTeams.TabIndex = 4
        Me.TabPage_SubTeams.Text = "Sub Teams"
        Me.TabPage_SubTeams.UseVisualStyleBackColor = True
        '
        'Group_SubTeams
        '
        Me.Group_SubTeams.Controls.Add(Me.CheckBox_RemoveAllSubTeams)
        Me.Group_SubTeams.Controls.Add(Me.CheckBox_AddAllSubTeams)
        Me.Group_SubTeams.Controls.Add(Me.Button_RemoveSubTeam)
        Me.Group_SubTeams.Controls.Add(Me.TreeView_SubTeams)
        Me.Group_SubTeams.Controls.Add(Me.Label4)
        Me.Group_SubTeams.Controls.Add(Me.CheckBox_IsCoordinator)
        Me.Group_SubTeams.Controls.Add(Me.Button_AddSubTeam)
        Me.Group_SubTeams.Controls.Add(Me.ComboBox_SubTeams)
        Me.Group_SubTeams.Location = New System.Drawing.Point(9, 6)
        Me.Group_SubTeams.Name = "Group_SubTeams"
        Me.Group_SubTeams.Size = New System.Drawing.Size(616, 324)
        Me.Group_SubTeams.TabIndex = 0
        Me.Group_SubTeams.TabStop = False
        '
        'CheckBox_RemoveAllSubTeams
        '
        Me.CheckBox_RemoveAllSubTeams.AutoSize = True
        Me.CheckBox_RemoveAllSubTeams.Location = New System.Drawing.Point(197, 110)
        Me.CheckBox_RemoveAllSubTeams.Name = "CheckBox_RemoveAllSubTeams"
        Me.CheckBox_RemoveAllSubTeams.Size = New System.Drawing.Size(82, 17)
        Me.CheckBox_RemoveAllSubTeams.TabIndex = 22
        Me.CheckBox_RemoveAllSubTeams.Text = "Remove All"
        Me.CheckBox_RemoveAllSubTeams.UseVisualStyleBackColor = True
        '
        'CheckBox_AddAllSubTeams
        '
        Me.CheckBox_AddAllSubTeams.AutoSize = True
        Me.CheckBox_AddAllSubTeams.Location = New System.Drawing.Point(197, 73)
        Me.CheckBox_AddAllSubTeams.Name = "CheckBox_AddAllSubTeams"
        Me.CheckBox_AddAllSubTeams.Size = New System.Drawing.Size(63, 17)
        Me.CheckBox_AddAllSubTeams.TabIndex = 21
        Me.CheckBox_AddAllSubTeams.Text = "Add All"
        Me.CheckBox_AddAllSubTeams.UseVisualStyleBackColor = True
        '
        'Button_RemoveSubTeam
        '
        Me.Button_RemoveSubTeam.Enabled = False
        Me.Button_RemoveSubTeam.Image = CType(resources.GetObject("Button_RemoveSubTeam.Image"), System.Drawing.Image)
        Me.Button_RemoveSubTeam.Location = New System.Drawing.Point(144, 102)
        Me.Button_RemoveSubTeam.Name = "Button_RemoveSubTeam"
        Me.Button_RemoveSubTeam.Size = New System.Drawing.Size(47, 31)
        Me.Button_RemoveSubTeam.TabIndex = 20
        Me.Button_RemoveSubTeam.UseVisualStyleBackColor = True
        '
        'TreeView_SubTeams
        '
        Me.TreeView_SubTeams.CheckBoxes = True
        Me.TreeView_SubTeams.Location = New System.Drawing.Point(285, 15)
        Me.TreeView_SubTeams.Name = "TreeView_SubTeams"
        Me.TreeView_SubTeams.ShowPlusMinus = False
        Me.TreeView_SubTeams.ShowRootLines = False
        Me.TreeView_SubTeams.Size = New System.Drawing.Size(314, 303)
        Me.TreeView_SubTeams.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Sub Team:"
        '
        'CheckBox_IsCoordinator
        '
        Me.CheckBox_IsCoordinator.AutoSize = True
        Me.CheckBox_IsCoordinator.Location = New System.Drawing.Point(9, 65)
        Me.CheckBox_IsCoordinator.Name = "CheckBox_IsCoordinator"
        Me.CheckBox_IsCoordinator.Size = New System.Drawing.Size(100, 17)
        Me.CheckBox_IsCoordinator.TabIndex = 17
        Me.CheckBox_IsCoordinator.Text = "Is Coordinator"
        Me.CheckBox_IsCoordinator.UseVisualStyleBackColor = True
        '
        'Button_AddSubTeam
        '
        Me.Button_AddSubTeam.Image = CType(resources.GetObject("Button_AddSubTeam.Image"), System.Drawing.Image)
        Me.Button_AddSubTeam.Location = New System.Drawing.Point(144, 65)
        Me.Button_AddSubTeam.Name = "Button_AddSubTeam"
        Me.Button_AddSubTeam.Size = New System.Drawing.Size(47, 31)
        Me.Button_AddSubTeam.TabIndex = 14
        Me.Button_AddSubTeam.UseVisualStyleBackColor = True
        '
        'ComboBox_SubTeams
        '
        Me.ComboBox_SubTeams.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_SubTeams.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_SubTeams.FormattingEnabled = True
        Me.ComboBox_SubTeams.Location = New System.Drawing.Point(8, 31)
        Me.ComboBox_SubTeams.Name = "ComboBox_SubTeams"
        Me.ComboBox_SubTeams.Size = New System.Drawing.Size(256, 21)
        Me.ComboBox_SubTeams.TabIndex = 15
        '
        'Form_ErrorProvider
        '
        Me.Form_ErrorProvider.ContainerControl = Me
        '
        'Button_DisableAccount
        '
        Me.Button_DisableAccount.Image = CType(resources.GetObject("Button_DisableAccount.Image"), System.Drawing.Image)
        Me.Button_DisableAccount.Location = New System.Drawing.Point(532, 7)
        Me.Button_DisableAccount.Name = "Button_DisableAccount"
        Me.Button_DisableAccount.Size = New System.Drawing.Size(115, 27)
        Me.Button_DisableAccount.TabIndex = 11
        Me.Button_DisableAccount.Text = "Disable Account"
        Me.Button_DisableAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_DisableAccount.UseVisualStyleBackColor = True
        '
        'CheckBox_AcctEnabled
        '
        Me.CheckBox_AcctEnabled.AutoSize = True
        Me.CheckBox_AcctEnabled.Enabled = False
        Me.CheckBox_AcctEnabled.Location = New System.Drawing.Point(12, 7)
        Me.CheckBox_AcctEnabled.Name = "CheckBox_AcctEnabled"
        Me.CheckBox_AcctEnabled.Size = New System.Drawing.Size(113, 17)
        Me.CheckBox_AcctEnabled.TabIndex = 0
        Me.CheckBox_AcctEnabled.TabStop = False
        Me.CheckBox_AcctEnabled.Text = "Account Enabled"
        Me.CheckBox_AcctEnabled.UseVisualStyleBackColor = True
        '
        'Label_ChangesHappenImmediately
        '
        Me.Label_ChangesHappenImmediately.AutoSize = True
        Me.Label_ChangesHappenImmediately.Location = New System.Drawing.Point(24, 595)
        Me.Label_ChangesHappenImmediately.Name = "Label_ChangesHappenImmediately"
        Me.Label_ChangesHappenImmediately.Size = New System.Drawing.Size(229, 13)
        Me.Label_ChangesHappenImmediately.TabIndex = 12
        Me.Label_ChangesHappenImmediately.Text = "Changes on this tab are saved immediately."
        Me.Label_ChangesHappenImmediately.Visible = False
        '
        'ttRoleDescription
        '
        Me.ttRoleDescription.AutoPopDelay = 5000
        Me.ttRoleDescription.InitialDelay = 500
        Me.ttRoleDescription.IsBalloon = True
        Me.ttRoleDescription.ReshowDelay = 0
        '
        'Form_EditUser
        '
        Me.AcceptButton = Me.Button_SearchUser
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(673, 632)
        Me.Controls.Add(Me.Label_ChangesHappenImmediately)
        Me.Controls.Add(Me.Button_DisableAccount)
        Me.Controls.Add(Me.TabControl_SecuritySettings)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.GroupBox_UserProperties)
        Me.Controls.Add(Me.CheckBox_AcctEnabled)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_EditUser"
        Me.ShowInTaskbar = False
        Me.Text = "Edit User Record"
        Me.GroupBox_UserProperties.ResumeLayout(False)
        Me.GroupBox_UserProperties.PerformLayout()
        Me.GroupBox_RolesIRMA.ResumeLayout(False)
        Me.GroupBox_RolesIRMA.PerformLayout()
        Me.TabControl_SecuritySettings.ResumeLayout(False)
        Me.TabPage_RolesIRMA.ResumeLayout(False)
        Me.GroupBox_AdminRoles.ResumeLayout(False)
        Me.GroupBox_AdminRoles.PerformLayout()
        Me.TabPage_RolesSLIM.ResumeLayout(False)
        Me.Group_RolesSLIM.ResumeLayout(False)
        Me.Group_RolesSLIM.PerformLayout()
        Me.TabPage_RolesPromo.ResumeLayout(False)
        Me.GroupBox_RolesPromo.ResumeLayout(False)
        Me.GroupBox_RolesPromo.PerformLayout()
        Me.TabPage_LocationTeams.ResumeLayout(False)
        Me.Group_LocationTeams.ResumeLayout(False)
        Me.Group_LocationTeams.PerformLayout()
        Me.TabPage_SubTeams.ResumeLayout(False)
        Me.Group_SubTeams.ResumeLayout(False)
        Me.Group_SubTeams.PerformLayout()
        CType(Me.Form_ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_UserProperties As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_RolesIRMA As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_FullName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UserName As System.Windows.Forms.TextBox
    Friend WithEvents Label_FullName As System.Windows.Forms.Label
    Friend WithEvents Label_UserName As System.Windows.Forms.Label
    Friend WithEvents Label_FaxNo As System.Windows.Forms.Label
    Friend WithEvents Label_PhoneNo As System.Windows.Forms.Label
    Friend WithEvents Label_PagerEmail As System.Windows.Forms.Label
    Friend WithEvents Label_Email As System.Windows.Forms.Label
    Friend WithEvents TextBox_PhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_PagerEmail As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Email As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_FaxNo As System.Windows.Forms.TextBox
    Friend WithEvents Label_CoverPage As System.Windows.Forms.Label
    Friend WithEvents Label_Printer As System.Windows.Forms.Label
    Friend WithEvents Label_Title As System.Windows.Forms.Label
    Friend WithEvents TextBox_CoverPg As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Printer As System.Windows.Forms.TextBox
    Friend WithEvents TabControl_SecuritySettings As System.Windows.Forms.TabControl
    Friend WithEvents TabPage_RolesIRMA As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_RolesSLIM As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_RolesPromo As System.Windows.Forms.TabPage
    Friend WithEvents Label_Location As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Title As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_HomeLocation As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage_LocationTeams As System.Windows.Forms.TabPage
    Friend WithEvents Group_RolesSLIM As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_RolesPromo As System.Windows.Forms.GroupBox
    Friend WithEvents Group_LocationTeams As System.Windows.Forms.GroupBox
    Friend WithEvents Form_ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents Button_SearchUser As System.Windows.Forms.Button
    Friend WithEvents CheckBox_RoleSLIMScaleInfo As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMSecureQuery As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMAuthorizations As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMRetailCost As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMStoreSpecials As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMPushToIRMA As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMVendorRequest As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMItemRequest As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMUserAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents Radio_RolePromoAdmin As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_RolePromoBuyer As System.Windows.Forms.RadioButton
    Friend WithEvents TabPage_SubTeams As System.Windows.Forms.TabPage
    Friend WithEvents Group_SubTeams As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_IsCoordinator As System.Windows.Forms.CheckBox
    Friend WithEvents Button_AddSubTeam As System.Windows.Forms.Button
    Friend WithEvents ComboBox_SubTeams As System.Windows.Forms.ComboBox
    Friend WithEvents TreeView_Locations As System.Windows.Forms.TreeView
    Friend WithEvents TreeView_SubTeams As System.Windows.Forms.TreeView
    Friend WithEvents TreeView_DataTree As System.Windows.Forms.TreeView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button_AddAllUserStoreTeamTitle As System.Windows.Forms.Button
    Friend WithEvents Button_RemoveUserStoreTeamTitle As System.Windows.Forms.Button
    Friend WithEvents Button_AddUserStoreTeamTitle As System.Windows.Forms.Button
    Friend WithEvents Button_DisableAccount As System.Windows.Forms.Button
    Friend WithEvents CheckBox_AcctEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents Button_RemoveSubTeam As System.Windows.Forms.Button
    Friend WithEvents Label_ChangesHappenImmediately As System.Windows.Forms.Label
    Friend WithEvents CheckBox_AddAllSubTeams As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RemoveAllSubTeams As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Einvoicing As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_POApprovalAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_VendorCostDiscrepancyAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_CostAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_DCAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_BatchBuildOnly As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_POAccountant As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Accountant As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Buyer As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_VendorAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_LockAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Warehouse As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_ItemAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Distributor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_FacilityCreditProcessor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Coordinator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_PriceBatchProcessor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_InventoryAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_SuperUser As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_AdminRoles As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_Role_SystemConfigurationAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_JobAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_AppConfigAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_SecurityAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_StoreAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_DataAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_UserMaintenance As System.Windows.Forms.CheckBox
    Friend WithEvents ttRoleDescription As System.Windows.Forms.ToolTip
    Friend WithEvents CheckBox_Role_POSInterfaceAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_Shrink As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_ShrinkAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents Button_ViewRoleConflicts As System.Windows.Forms.Button
    Friend WithEvents Radio_RolePromoNone As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox_Role_POEditor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_DeletePO As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Role_TaxAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_RoleSLIMECommerce As System.Windows.Forms.CheckBox

End Class
