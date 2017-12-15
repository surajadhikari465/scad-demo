<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTitles
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageTitles))
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox_IRMARoles = New System.Windows.Forms.GroupBox()
        Me.CheckBox_TaxAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_DeletePO = New System.Windows.Forms.CheckBox()
        Me.CheckBox_POEditor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ShrinkAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Shrink = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Einvoicing = New System.Windows.Forms.CheckBox()
        Me.CheckBox_POApprovalAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_VendorCostDiscrepancyAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CostAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_DCAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_BatchBuildOnly = New System.Windows.Forms.CheckBox()
        Me.CheckBox_POAccountant = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Accountant = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Buyer = New System.Windows.Forms.CheckBox()
        Me.CheckBox_VendorAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_LockAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Warehouse = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ItemAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Receiver = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FacilityCreditProcessor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Coordinator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_PriceBatchProcessor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_InventoryAdmin = New System.Windows.Forms.CheckBox()
        Me.GroupBox_AdminRoles = New System.Windows.Forms.GroupBox()
        Me.CheckBox_POSInterfaceAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_UserMaintenance = New System.Windows.Forms.CheckBox()
        Me.CheckBox_JobAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AppConfigAdmin = New System.Windows.Forms.CheckBox()
        Me.CheckBox_StoreAdministrator = New System.Windows.Forms.CheckBox()
        Me.CheckBox_DataAdministrator = New System.Windows.Forms.CheckBox()
        Me.ComboBox_Titles = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button_DeleteTitle = New System.Windows.Forms.Button()
        Me.Button_AddTitle = New System.Windows.Forms.Button()
        Me.Button_Edit = New System.Windows.Forms.Button()
        Me.ttRoleDescription = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_ViewRoleConflicts = New System.Windows.Forms.Button()
        Me.CheckBox_CancelAllSales = New System.Windows.Forms.CheckBox()
        Me.GroupBox_IRMARoles.SuspendLayout()
        Me.GroupBox_AdminRoles.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Image = CType(resources.GetObject("Button_Save.Image"), System.Drawing.Image)
        Me.Button_Save.Location = New System.Drawing.Point(536, 416)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(87, 31)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(449, 416)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(82, 31)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Close"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'GroupBox_IRMARoles
        '
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_CancelAllSales)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_TaxAdministrator)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_DeletePO)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_POEditor)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_ShrinkAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Shrink)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Einvoicing)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_POApprovalAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_VendorCostDiscrepancyAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_CostAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_DCAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_BatchBuildOnly)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_POAccountant)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Accountant)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Buyer)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_VendorAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_LockAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Warehouse)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_ItemAdmin)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Receiver)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_FacilityCreditProcessor)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_Coordinator)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_PriceBatchProcessor)
        Me.GroupBox_IRMARoles.Controls.Add(Me.CheckBox_InventoryAdmin)
        Me.GroupBox_IRMARoles.Location = New System.Drawing.Point(12, 46)
        Me.GroupBox_IRMARoles.Name = "GroupBox_IRMARoles"
        Me.GroupBox_IRMARoles.Size = New System.Drawing.Size(611, 204)
        Me.GroupBox_IRMARoles.TabIndex = 5
        Me.GroupBox_IRMARoles.TabStop = False
        Me.GroupBox_IRMARoles.Text = "User Roles"
        '
        'CheckBox_TaxAdministrator
        '
        Me.CheckBox_TaxAdministrator.AutoSize = True
        Me.CheckBox_TaxAdministrator.Location = New System.Drawing.Point(416, 88)
        Me.CheckBox_TaxAdministrator.Name = "CheckBox_TaxAdministrator"
        Me.CheckBox_TaxAdministrator.Size = New System.Drawing.Size(114, 17)
        Me.CheckBox_TaxAdministrator.TabIndex = 22
        Me.CheckBox_TaxAdministrator.Text = "Tax Administrator"
        Me.CheckBox_TaxAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_DeletePO
        '
        Me.CheckBox_DeletePO.AutoSize = True
        Me.CheckBox_DeletePO.Location = New System.Drawing.Point(8, 157)
        Me.CheckBox_DeletePO.Name = "CheckBox_DeletePO"
        Me.CheckBox_DeletePO.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox_DeletePO.TabIndex = 21
        Me.CheckBox_DeletePO.Text = "Delete PO"
        Me.CheckBox_DeletePO.UseVisualStyleBackColor = True
        '
        'CheckBox_POEditor
        '
        Me.CheckBox_POEditor.AutoSize = True
        Me.CheckBox_POEditor.Location = New System.Drawing.Point(210, 157)
        Me.CheckBox_POEditor.Name = "CheckBox_POEditor"
        Me.CheckBox_POEditor.Size = New System.Drawing.Size(75, 17)
        Me.CheckBox_POEditor.TabIndex = 20
        Me.CheckBox_POEditor.Text = "PO Editor"
        Me.CheckBox_POEditor.UseVisualStyleBackColor = True
        '
        'CheckBox_ShrinkAdmin
        '
        Me.CheckBox_ShrinkAdmin.AutoSize = True
        Me.CheckBox_ShrinkAdmin.Location = New System.Drawing.Point(416, 65)
        Me.CheckBox_ShrinkAdmin.Name = "CheckBox_ShrinkAdmin"
        Me.CheckBox_ShrinkAdmin.Size = New System.Drawing.Size(132, 17)
        Me.CheckBox_ShrinkAdmin.TabIndex = 16
        Me.CheckBox_ShrinkAdmin.Text = "Shrink Administrator"
        Me.CheckBox_ShrinkAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Shrink
        '
        Me.CheckBox_Shrink.AutoSize = True
        Me.CheckBox_Shrink.Location = New System.Drawing.Point(416, 42)
        Me.CheckBox_Shrink.Name = "CheckBox_Shrink"
        Me.CheckBox_Shrink.Size = New System.Drawing.Size(59, 17)
        Me.CheckBox_Shrink.TabIndex = 15
        Me.CheckBox_Shrink.Text = "Shrink"
        Me.CheckBox_Shrink.UseVisualStyleBackColor = True
        '
        'CheckBox_Einvoicing
        '
        Me.CheckBox_Einvoicing.AutoSize = True
        Me.CheckBox_Einvoicing.Location = New System.Drawing.Point(8, 180)
        Me.CheckBox_Einvoicing.Name = "CheckBox_Einvoicing"
        Me.CheckBox_Einvoicing.Size = New System.Drawing.Size(83, 17)
        Me.CheckBox_Einvoicing.TabIndex = 7
        Me.CheckBox_Einvoicing.Text = "E-Invoicing"
        Me.CheckBox_Einvoicing.UseVisualStyleBackColor = True
        '
        'CheckBox_POApprovalAdmin
        '
        Me.CheckBox_POApprovalAdmin.AutoSize = True
        Me.CheckBox_POApprovalAdmin.Location = New System.Drawing.Point(210, 134)
        Me.CheckBox_POApprovalAdmin.Name = "CheckBox_POApprovalAdmin"
        Me.CheckBox_POApprovalAdmin.Size = New System.Drawing.Size(163, 17)
        Me.CheckBox_POApprovalAdmin.TabIndex = 12
        Me.CheckBox_POApprovalAdmin.Text = "PO Approval Administrator"
        Me.CheckBox_POApprovalAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_VendorCostDiscrepancyAdmin
        '
        Me.CheckBox_VendorCostDiscrepancyAdmin.AutoSize = True
        Me.CheckBox_VendorCostDiscrepancyAdmin.Location = New System.Drawing.Point(416, 134)
        Me.CheckBox_VendorCostDiscrepancyAdmin.Name = "CheckBox_VendorCostDiscrepancyAdmin"
        Me.CheckBox_VendorCostDiscrepancyAdmin.Size = New System.Drawing.Size(189, 17)
        Me.CheckBox_VendorCostDiscrepancyAdmin.TabIndex = 18
        Me.CheckBox_VendorCostDiscrepancyAdmin.Text = "Vendor Cost Discrepancy Admin"
        Me.CheckBox_VendorCostDiscrepancyAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_CostAdmin
        '
        Me.CheckBox_CostAdmin.AutoSize = True
        Me.CheckBox_CostAdmin.Location = New System.Drawing.Point(8, 111)
        Me.CheckBox_CostAdmin.Name = "CheckBox_CostAdmin"
        Me.CheckBox_CostAdmin.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox_CostAdmin.TabIndex = 4
        Me.CheckBox_CostAdmin.Text = "Cost Administrator"
        Me.CheckBox_CostAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_DCAdmin
        '
        Me.CheckBox_DCAdmin.AutoSize = True
        Me.CheckBox_DCAdmin.Location = New System.Drawing.Point(8, 134)
        Me.CheckBox_DCAdmin.Name = "CheckBox_DCAdmin"
        Me.CheckBox_DCAdmin.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox_DCAdmin.TabIndex = 6
        Me.CheckBox_DCAdmin.Text = "DC Admin"
        Me.CheckBox_DCAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_BatchBuildOnly
        '
        Me.CheckBox_BatchBuildOnly.AutoSize = True
        Me.CheckBox_BatchBuildOnly.Location = New System.Drawing.Point(8, 42)
        Me.CheckBox_BatchBuildOnly.Name = "CheckBox_BatchBuildOnly"
        Me.CheckBox_BatchBuildOnly.Size = New System.Drawing.Size(112, 17)
        Me.CheckBox_BatchBuildOnly.TabIndex = 1
        Me.CheckBox_BatchBuildOnly.Text = "Batch Build Only"
        Me.CheckBox_BatchBuildOnly.UseVisualStyleBackColor = True
        '
        'CheckBox_POAccountant
        '
        Me.CheckBox_POAccountant.AutoSize = True
        Me.CheckBox_POAccountant.Location = New System.Drawing.Point(210, 111)
        Me.CheckBox_POAccountant.Name = "CheckBox_POAccountant"
        Me.CheckBox_POAccountant.Size = New System.Drawing.Size(103, 17)
        Me.CheckBox_POAccountant.TabIndex = 11
        Me.CheckBox_POAccountant.Text = "PO Accountant"
        Me.CheckBox_POAccountant.UseVisualStyleBackColor = True
        '
        'CheckBox_Accountant
        '
        Me.CheckBox_Accountant.AutoSize = True
        Me.CheckBox_Accountant.Location = New System.Drawing.Point(8, 19)
        Me.CheckBox_Accountant.Name = "CheckBox_Accountant"
        Me.CheckBox_Accountant.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_Accountant.TabIndex = 0
        Me.CheckBox_Accountant.Text = "Accountant"
        Me.CheckBox_Accountant.UseVisualStyleBackColor = True
        '
        'CheckBox_Buyer
        '
        Me.CheckBox_Buyer.AutoSize = True
        Me.CheckBox_Buyer.Location = New System.Drawing.Point(8, 65)
        Me.CheckBox_Buyer.Name = "CheckBox_Buyer"
        Me.CheckBox_Buyer.Size = New System.Drawing.Size(55, 17)
        Me.CheckBox_Buyer.TabIndex = 2
        Me.CheckBox_Buyer.Text = "Buyer"
        Me.CheckBox_Buyer.UseVisualStyleBackColor = True
        '
        'CheckBox_VendorAdmin
        '
        Me.CheckBox_VendorAdmin.AutoSize = True
        Me.CheckBox_VendorAdmin.Location = New System.Drawing.Point(416, 111)
        Me.CheckBox_VendorAdmin.Name = "CheckBox_VendorAdmin"
        Me.CheckBox_VendorAdmin.Size = New System.Drawing.Size(136, 17)
        Me.CheckBox_VendorAdmin.TabIndex = 17
        Me.CheckBox_VendorAdmin.Text = "Vendor Administrator"
        Me.CheckBox_VendorAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_LockAdmin
        '
        Me.CheckBox_LockAdmin.AutoSize = True
        Me.CheckBox_LockAdmin.Location = New System.Drawing.Point(210, 88)
        Me.CheckBox_LockAdmin.Name = "CheckBox_LockAdmin"
        Me.CheckBox_LockAdmin.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox_LockAdmin.TabIndex = 10
        Me.CheckBox_LockAdmin.Text = "Lock Administrator"
        Me.CheckBox_LockAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Warehouse
        '
        Me.CheckBox_Warehouse.AutoSize = True
        Me.CheckBox_Warehouse.Location = New System.Drawing.Point(416, 157)
        Me.CheckBox_Warehouse.Name = "CheckBox_Warehouse"
        Me.CheckBox_Warehouse.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_Warehouse.TabIndex = 19
        Me.CheckBox_Warehouse.Text = "Warehouse"
        Me.CheckBox_Warehouse.UseVisualStyleBackColor = True
        '
        'CheckBox_ItemAdmin
        '
        Me.CheckBox_ItemAdmin.AutoSize = True
        Me.CheckBox_ItemAdmin.Location = New System.Drawing.Point(211, 65)
        Me.CheckBox_ItemAdmin.Name = "CheckBox_ItemAdmin"
        Me.CheckBox_ItemAdmin.Size = New System.Drawing.Size(121, 17)
        Me.CheckBox_ItemAdmin.TabIndex = 9
        Me.CheckBox_ItemAdmin.Text = "Item Administrator"
        Me.CheckBox_ItemAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_Receiver
        '
        Me.CheckBox_Receiver.AutoSize = True
        Me.CheckBox_Receiver.Location = New System.Drawing.Point(416, 19)
        Me.CheckBox_Receiver.Name = "CheckBox_Receiver"
        Me.CheckBox_Receiver.Size = New System.Drawing.Size(68, 17)
        Me.CheckBox_Receiver.TabIndex = 14
        Me.CheckBox_Receiver.Text = "Receiver"
        Me.CheckBox_Receiver.UseVisualStyleBackColor = True
        '
        'CheckBox_FacilityCreditProcessor
        '
        Me.CheckBox_FacilityCreditProcessor.AutoSize = True
        Me.CheckBox_FacilityCreditProcessor.Location = New System.Drawing.Point(210, 19)
        Me.CheckBox_FacilityCreditProcessor.Name = "CheckBox_FacilityCreditProcessor"
        Me.CheckBox_FacilityCreditProcessor.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox_FacilityCreditProcessor.TabIndex = 5
        Me.CheckBox_FacilityCreditProcessor.Text = "Facility Credit Processor"
        Me.CheckBox_FacilityCreditProcessor.UseVisualStyleBackColor = True
        '
        'CheckBox_Coordinator
        '
        Me.CheckBox_Coordinator.AutoSize = True
        Me.CheckBox_Coordinator.Location = New System.Drawing.Point(8, 88)
        Me.CheckBox_Coordinator.Name = "CheckBox_Coordinator"
        Me.CheckBox_Coordinator.Size = New System.Drawing.Size(89, 17)
        Me.CheckBox_Coordinator.TabIndex = 3
        Me.CheckBox_Coordinator.Text = "Coordinator"
        Me.CheckBox_Coordinator.UseVisualStyleBackColor = True
        '
        'CheckBox_PriceBatchProcessor
        '
        Me.CheckBox_PriceBatchProcessor.AutoSize = True
        Me.CheckBox_PriceBatchProcessor.Location = New System.Drawing.Point(210, 180)
        Me.CheckBox_PriceBatchProcessor.Name = "CheckBox_PriceBatchProcessor"
        Me.CheckBox_PriceBatchProcessor.Size = New System.Drawing.Size(134, 17)
        Me.CheckBox_PriceBatchProcessor.TabIndex = 13
        Me.CheckBox_PriceBatchProcessor.Text = "Price Batch Processor"
        Me.CheckBox_PriceBatchProcessor.UseVisualStyleBackColor = True
        '
        'CheckBox_InventoryAdmin
        '
        Me.CheckBox_InventoryAdmin.AutoSize = True
        Me.CheckBox_InventoryAdmin.Location = New System.Drawing.Point(210, 42)
        Me.CheckBox_InventoryAdmin.Name = "CheckBox_InventoryAdmin"
        Me.CheckBox_InventoryAdmin.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox_InventoryAdmin.TabIndex = 8
        Me.CheckBox_InventoryAdmin.Text = "Inventory Administrator"
        Me.CheckBox_InventoryAdmin.UseVisualStyleBackColor = True
        '
        'GroupBox_AdminRoles
        '
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_POSInterfaceAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_UserMaintenance)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_JobAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_AppConfigAdmin)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_StoreAdministrator)
        Me.GroupBox_AdminRoles.Controls.Add(Me.CheckBox_DataAdministrator)
        Me.GroupBox_AdminRoles.Location = New System.Drawing.Point(12, 273)
        Me.GroupBox_AdminRoles.Name = "GroupBox_AdminRoles"
        Me.GroupBox_AdminRoles.Size = New System.Drawing.Size(611, 113)
        Me.GroupBox_AdminRoles.TabIndex = 6
        Me.GroupBox_AdminRoles.TabStop = False
        Me.GroupBox_AdminRoles.Text = "Admin Roles"
        '
        'CheckBox_POSInterfaceAdministrator
        '
        Me.CheckBox_POSInterfaceAdministrator.AutoSize = True
        Me.CheckBox_POSInterfaceAdministrator.Location = New System.Drawing.Point(210, 25)
        Me.CheckBox_POSInterfaceAdministrator.Name = "CheckBox_POSInterfaceAdministrator"
        Me.CheckBox_POSInterfaceAdministrator.Size = New System.Drawing.Size(168, 17)
        Me.CheckBox_POSInterfaceAdministrator.TabIndex = 3
        Me.CheckBox_POSInterfaceAdministrator.Text = "POS Interface Administrator"
        Me.CheckBox_POSInterfaceAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_UserMaintenance
        '
        Me.CheckBox_UserMaintenance.AutoSize = True
        Me.CheckBox_UserMaintenance.Location = New System.Drawing.Point(211, 71)
        Me.CheckBox_UserMaintenance.Name = "CheckBox_UserMaintenance"
        Me.CheckBox_UserMaintenance.Size = New System.Drawing.Size(119, 17)
        Me.CheckBox_UserMaintenance.TabIndex = 5
        Me.CheckBox_UserMaintenance.Text = "User Maintenance"
        Me.CheckBox_UserMaintenance.UseVisualStyleBackColor = True
        '
        'CheckBox_JobAdministrator
        '
        Me.CheckBox_JobAdministrator.AutoSize = True
        Me.CheckBox_JobAdministrator.Location = New System.Drawing.Point(8, 71)
        Me.CheckBox_JobAdministrator.Name = "CheckBox_JobAdministrator"
        Me.CheckBox_JobAdministrator.Size = New System.Drawing.Size(117, 17)
        Me.CheckBox_JobAdministrator.TabIndex = 2
        Me.CheckBox_JobAdministrator.Text = "Job Administrator"
        Me.CheckBox_JobAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_AppConfigAdmin
        '
        Me.CheckBox_AppConfigAdmin.AutoSize = True
        Me.CheckBox_AppConfigAdmin.Location = New System.Drawing.Point(8, 25)
        Me.CheckBox_AppConfigAdmin.Name = "CheckBox_AppConfigAdmin"
        Me.CheckBox_AppConfigAdmin.Size = New System.Drawing.Size(197, 17)
        Me.CheckBox_AppConfigAdmin.TabIndex = 0
        Me.CheckBox_AppConfigAdmin.Text = "Application Configuration Admin"
        Me.CheckBox_AppConfigAdmin.UseVisualStyleBackColor = True
        '
        'CheckBox_StoreAdministrator
        '
        Me.CheckBox_StoreAdministrator.AutoSize = True
        Me.CheckBox_StoreAdministrator.Location = New System.Drawing.Point(210, 48)
        Me.CheckBox_StoreAdministrator.Name = "CheckBox_StoreAdministrator"
        Me.CheckBox_StoreAdministrator.Size = New System.Drawing.Size(126, 17)
        Me.CheckBox_StoreAdministrator.TabIndex = 4
        Me.CheckBox_StoreAdministrator.Text = "Store Administrator"
        Me.CheckBox_StoreAdministrator.UseVisualStyleBackColor = True
        '
        'CheckBox_DataAdministrator
        '
        Me.CheckBox_DataAdministrator.AutoSize = True
        Me.CheckBox_DataAdministrator.Location = New System.Drawing.Point(7, 48)
        Me.CheckBox_DataAdministrator.Name = "CheckBox_DataAdministrator"
        Me.CheckBox_DataAdministrator.Size = New System.Drawing.Size(123, 17)
        Me.CheckBox_DataAdministrator.TabIndex = 1
        Me.CheckBox_DataAdministrator.Text = "Data Administrator"
        Me.CheckBox_DataAdministrator.UseVisualStyleBackColor = True
        '
        'ComboBox_Titles
        '
        Me.ComboBox_Titles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Titles.FormattingEnabled = True
        Me.ComboBox_Titles.Location = New System.Drawing.Point(67, 12)
        Me.ComboBox_Titles.Name = "ComboBox_Titles"
        Me.ComboBox_Titles.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox_Titles.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Title"
        '
        'Button_DeleteTitle
        '
        Me.Button_DeleteTitle.ForeColor = System.Drawing.Color.Transparent
        Me.Button_DeleteTitle.Image = CType(resources.GetObject("Button_DeleteTitle.Image"), System.Drawing.Image)
        Me.Button_DeleteTitle.Location = New System.Drawing.Point(360, 5)
        Me.Button_DeleteTitle.Name = "Button_DeleteTitle"
        Me.Button_DeleteTitle.Size = New System.Drawing.Size(33, 31)
        Me.Button_DeleteTitle.TabIndex = 3
        Me.Button_DeleteTitle.UseVisualStyleBackColor = True
        '
        'Button_AddTitle
        '
        Me.Button_AddTitle.Image = CType(resources.GetObject("Button_AddTitle.Image"), System.Drawing.Image)
        Me.Button_AddTitle.Location = New System.Drawing.Point(282, 5)
        Me.Button_AddTitle.Name = "Button_AddTitle"
        Me.Button_AddTitle.Size = New System.Drawing.Size(33, 31)
        Me.Button_AddTitle.TabIndex = 1
        Me.Button_AddTitle.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.ForeColor = System.Drawing.Color.Transparent
        Me.Button_Edit.Image = CType(resources.GetObject("Button_Edit.Image"), System.Drawing.Image)
        Me.Button_Edit.Location = New System.Drawing.Point(321, 5)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(33, 31)
        Me.Button_Edit.TabIndex = 2
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'ttRoleDescription
        '
        Me.ttRoleDescription.AutoPopDelay = 50000
        Me.ttRoleDescription.InitialDelay = 500
        Me.ttRoleDescription.IsBalloon = True
        Me.ttRoleDescription.ReshowDelay = 1
        '
        'Button_ViewRoleConflicts
        '
        Me.Button_ViewRoleConflicts.ForeColor = System.Drawing.Color.Transparent
        Me.Button_ViewRoleConflicts.Image = CType(resources.GetObject("Button_ViewRoleConflicts.Image"), System.Drawing.Image)
        Me.Button_ViewRoleConflicts.Location = New System.Drawing.Point(399, 5)
        Me.Button_ViewRoleConflicts.Name = "Button_ViewRoleConflicts"
        Me.Button_ViewRoleConflicts.Size = New System.Drawing.Size(33, 31)
        Me.Button_ViewRoleConflicts.TabIndex = 4
        Me.Button_ViewRoleConflicts.UseVisualStyleBackColor = True
        '
        'CheckBox_CancelAllSales
        '
        Me.CheckBox_CancelAllSales.AutoSize = True
        Me.CheckBox_CancelAllSales.Location = New System.Drawing.Point(416, 180)
        Me.CheckBox_CancelAllSales.Name = "CheckBox_CancelAllSales"
        Me.CheckBox_CancelAllSales.Size = New System.Drawing.Size(105, 17)
        Me.CheckBox_CancelAllSales.TabIndex = 23
        Me.CheckBox_CancelAllSales.Text = "Cancel All Sales"
        Me.CheckBox_CancelAllSales.UseVisualStyleBackColor = True
        '
        'Form_ManageTitles
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(631, 459)
        Me.Controls.Add(Me.Button_ViewRoleConflicts)
        Me.Controls.Add(Me.Button_Edit)
        Me.Controls.Add(Me.Button_AddTitle)
        Me.Controls.Add(Me.Button_DeleteTitle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_Titles)
        Me.Controls.Add(Me.GroupBox_AdminRoles)
        Me.Controls.Add(Me.GroupBox_IRMARoles)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_ManageTitles"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Titles"
        Me.GroupBox_IRMARoles.ResumeLayout(False)
        Me.GroupBox_IRMARoles.PerformLayout()
        Me.GroupBox_AdminRoles.ResumeLayout(False)
        Me.GroupBox_AdminRoles.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_IRMARoles As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_Einvoicing As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POApprovalAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_VendorCostDiscrepancyAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_CostAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_DCAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_BatchBuildOnly As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_POAccountant As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Accountant As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Buyer As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_VendorAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_LockAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Warehouse As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ItemAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Receiver As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_FacilityCreditProcessor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Coordinator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_PriceBatchProcessor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_InventoryAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_AdminRoles As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_JobAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_AppConfigAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_StoreAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_DataAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_Titles As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button_DeleteTitle As System.Windows.Forms.Button
    Friend WithEvents Button_AddTitle As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents CheckBox_UserMaintenance As System.Windows.Forms.CheckBox
    Friend WithEvents ttRoleDescription As System.Windows.Forms.ToolTip
    Friend WithEvents CheckBox_POSInterfaceAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_ShrinkAdmin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Shrink As System.Windows.Forms.CheckBox
    Friend WithEvents Button_ViewRoleConflicts As System.Windows.Forms.Button
    Friend WithEvents CheckBox_POEditor As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_DeletePO As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_TaxAdministrator As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_CancelAllSales As CheckBox
End Class
