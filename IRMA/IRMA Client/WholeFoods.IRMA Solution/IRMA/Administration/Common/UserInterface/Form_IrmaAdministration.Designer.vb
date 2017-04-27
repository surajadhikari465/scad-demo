<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_IrmaAdministration
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_IrmaAdministration))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ToolStripMenuItem_Stores = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ViewStores = New System.Windows.Forms.ToolStripMenuItem
        Me.CreateStoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_Exit = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_FileWriters = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ViewWriters = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ScheduleJobs = New System.Windows.Forms.ToolStripMenuItem
        Me.ManageScheduledJobsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RunScheduleJobsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.APUploadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_AuditExceptionsReport = New System.Windows.Forms.ToolStripMenuItem
        Me.AverageCostUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BuildStorePOSFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BuildStoreScaleFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloseReceivingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EInvoicingIntegrationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PLUMHostToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.POSPullToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ScalePush = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_POSPush = New System.Windows.Forms.ToolStripMenuItem
        Me.SendOrdersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TlogProcessingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WeeklySalesRollupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_Users = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ViewUsers = New System.Windows.Forms.ToolStripMenuItem
        Me.DataConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ConfigurationData = New System.Windows.Forms.ToolStripMenuItem
        Me.ManageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.BuildToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InstanceDataFlagsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RegionalInstanceDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ManageItemAttributesToolStrinpMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ManagePricingMethodsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ManageMenuAccessToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TaxJurisdictionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ZonesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CurrencyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BatchMgmtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChangeBatchStateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RestoreDeletedItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel_Region = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel_Environment = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel_Version = New System.Windows.Forms.ToolStripStatusLabel
        Me.ManageTitlesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Stores, Me.ToolStripMenuItem_FileWriters, Me.ToolStripMenuItem_ScheduleJobs, Me.ToolStripMenuItem_Users, Me.DataConfigurationToolStripMenuItem, Me.BatchMgmtToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(742, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem_Stores
        '
        Me.ToolStripMenuItem_Stores.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ViewStores, Me.CreateStoreToolStripMenuItem, Me.ToolStripMenuItem_Exit})
        Me.ToolStripMenuItem_Stores.Name = "ToolStripMenuItem_Stores"
        Me.ToolStripMenuItem_Stores.Size = New System.Drawing.Size(50, 20)
        Me.ToolStripMenuItem_Stores.Text = "Stores"
        '
        'ToolStripMenuItem_ViewStores
        '
        Me.ToolStripMenuItem_ViewStores.Name = "ToolStripMenuItem_ViewStores"
        Me.ToolStripMenuItem_ViewStores.Size = New System.Drawing.Size(147, 22)
        Me.ToolStripMenuItem_ViewStores.Text = "View Stores"
        '
        'CreateStoreToolStripMenuItem
        '
        Me.CreateStoreToolStripMenuItem.Name = "CreateStoreToolStripMenuItem"
        Me.CreateStoreToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.CreateStoreToolStripMenuItem.Text = "Create Store"
        '
        'ToolStripMenuItem_Exit
        '
        Me.ToolStripMenuItem_Exit.Name = "ToolStripMenuItem_Exit"
        Me.ToolStripMenuItem_Exit.Size = New System.Drawing.Size(147, 22)
        Me.ToolStripMenuItem_Exit.Text = "Exit"
        '
        'ToolStripMenuItem_FileWriters
        '
        Me.ToolStripMenuItem_FileWriters.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ViewWriters})
        Me.ToolStripMenuItem_FileWriters.Name = "ToolStripMenuItem_FileWriters"
        Me.ToolStripMenuItem_FileWriters.Size = New System.Drawing.Size(73, 20)
        Me.ToolStripMenuItem_FileWriters.Text = "File Writers"
        '
        'ToolStripMenuItem_ViewWriters
        '
        Me.ToolStripMenuItem_ViewWriters.Name = "ToolStripMenuItem_ViewWriters"
        Me.ToolStripMenuItem_ViewWriters.Size = New System.Drawing.Size(145, 22)
        Me.ToolStripMenuItem_ViewWriters.Text = "View Writers"
        '
        'ToolStripMenuItem_ScheduleJobs
        '
        Me.ToolStripMenuItem_ScheduleJobs.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManageScheduledJobsToolStripMenuItem, Me.RunScheduleJobsToolStripMenuItem})
        Me.ToolStripMenuItem_ScheduleJobs.Name = "ToolStripMenuItem_ScheduleJobs"
        Me.ToolStripMenuItem_ScheduleJobs.Size = New System.Drawing.Size(93, 20)
        Me.ToolStripMenuItem_ScheduleJobs.Text = "Scheduled Jobs"
        '
        'ManageScheduledJobsToolStripMenuItem
        '
        Me.ManageScheduledJobsToolStripMenuItem.Name = "ManageScheduledJobsToolStripMenuItem"
        Me.ManageScheduledJobsToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ManageScheduledJobsToolStripMenuItem.Text = "Manage Scheduled Jobs"
        '
        'RunScheduleJobsToolStripMenuItem
        '
        Me.RunScheduleJobsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.APUploadToolStripMenuItem, Me.ToolStripMenuItem_AuditExceptionsReport, Me.AverageCostUpdateToolStripMenuItem, Me.BuildStorePOSFileToolStripMenuItem, Me.BuildStoreScaleFileToolStripMenuItem, Me.CloseReceivingToolStripMenuItem, Me.EInvoicingIntegrationToolStripMenuItem, Me.PLUMHostToolStripMenuItem, Me.POSPullToolStripMenuItem, Me.ToolStripMenuItem_ScalePush, Me.ToolStripMenuItem_POSPush, Me.SendOrdersToolStripMenuItem, Me.TlogProcessingToolStripMenuItem, Me.WeeklySalesRollupToolStripMenuItem})
        Me.RunScheduleJobsToolStripMenuItem.Name = "RunScheduleJobsToolStripMenuItem"
        Me.RunScheduleJobsToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.RunScheduleJobsToolStripMenuItem.Text = "Run Schedule Jobs"
        '
        'APUploadToolStripMenuItem
        '
        Me.APUploadToolStripMenuItem.Name = "APUploadToolStripMenuItem"
        Me.APUploadToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.APUploadToolStripMenuItem.Text = "AP Upload"
        '
        'ToolStripMenuItem_AuditExceptionsReport
        '
        Me.ToolStripMenuItem_AuditExceptionsReport.Name = "ToolStripMenuItem_AuditExceptionsReport"
        Me.ToolStripMenuItem_AuditExceptionsReport.Size = New System.Drawing.Size(201, 22)
        Me.ToolStripMenuItem_AuditExceptionsReport.Text = "Audit Exceptions Report"
        '
        'AverageCostUpdateToolStripMenuItem
        '
        Me.AverageCostUpdateToolStripMenuItem.Name = "AverageCostUpdateToolStripMenuItem"
        Me.AverageCostUpdateToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.AverageCostUpdateToolStripMenuItem.Text = "Average Cost Update"
        '
        'BuildStorePOSFileToolStripMenuItem
        '
        Me.BuildStorePOSFileToolStripMenuItem.Name = "BuildStorePOSFileToolStripMenuItem"
        Me.BuildStorePOSFileToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.BuildStorePOSFileToolStripMenuItem.Text = "Build Store POS File"
        '
        'BuildStoreScaleFileToolStripMenuItem
        '
        Me.BuildStoreScaleFileToolStripMenuItem.Name = "BuildStoreScaleFileToolStripMenuItem"
        Me.BuildStoreScaleFileToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.BuildStoreScaleFileToolStripMenuItem.Text = "Build Store Scale File"
        '
        'CloseReceivingToolStripMenuItem
        '
        Me.CloseReceivingToolStripMenuItem.Name = "CloseReceivingToolStripMenuItem"
        Me.CloseReceivingToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.CloseReceivingToolStripMenuItem.Text = "Close Receiving"
        '
        'EInvoicingIntegrationToolStripMenuItem
        '
        Me.EInvoicingIntegrationToolStripMenuItem.Name = "EInvoicingIntegrationToolStripMenuItem"
        Me.EInvoicingIntegrationToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.EInvoicingIntegrationToolStripMenuItem.Text = "E-Invoicing Integration"
        '
        'PLUMHostToolStripMenuItem
        '
        Me.PLUMHostToolStripMenuItem.Name = "PLUMHostToolStripMenuItem"
        Me.PLUMHostToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.PLUMHostToolStripMenuItem.Text = "PLUM Host"
        '
        'POSPullToolStripMenuItem
        '
        Me.POSPullToolStripMenuItem.Name = "POSPullToolStripMenuItem"
        Me.POSPullToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.POSPullToolStripMenuItem.Text = "POS Pull"
        '
        'ToolStripMenuItem_ScalePush
        '
        Me.ToolStripMenuItem_ScalePush.Name = "ToolStripMenuItem_ScalePush"
        Me.ToolStripMenuItem_ScalePush.Size = New System.Drawing.Size(201, 22)
        Me.ToolStripMenuItem_ScalePush.Text = "Scale Push"
        '
        'ToolStripMenuItem_POSPush
        '
        Me.ToolStripMenuItem_POSPush.Name = "ToolStripMenuItem_POSPush"
        Me.ToolStripMenuItem_POSPush.Size = New System.Drawing.Size(201, 22)
        Me.ToolStripMenuItem_POSPush.Text = "Scale/POS Push"
        '
        'SendOrdersToolStripMenuItem
        '
        Me.SendOrdersToolStripMenuItem.Name = "SendOrdersToolStripMenuItem"
        Me.SendOrdersToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.SendOrdersToolStripMenuItem.Text = "Send Orders"
        '
        'TlogProcessingToolStripMenuItem
        '
        Me.TlogProcessingToolStripMenuItem.Name = "TlogProcessingToolStripMenuItem"
        Me.TlogProcessingToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.TlogProcessingToolStripMenuItem.Text = "Tlog Processing"
        '
        'WeeklySalesRollupToolStripMenuItem
        '
        Me.WeeklySalesRollupToolStripMenuItem.Name = "WeeklySalesRollupToolStripMenuItem"
        Me.WeeklySalesRollupToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.WeeklySalesRollupToolStripMenuItem.Text = "Weekly Sales Rollup"
        '
        'ToolStripMenuItem_Users
        '
        Me.ToolStripMenuItem_Users.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ViewUsers, Me.ManageTitlesToolStripMenuItem})
        Me.ToolStripMenuItem_Users.Name = "ToolStripMenuItem_Users"
        Me.ToolStripMenuItem_Users.Size = New System.Drawing.Size(46, 20)
        Me.ToolStripMenuItem_Users.Text = "Users"
        '
        'ToolStripMenuItem_ViewUsers
        '
        Me.ToolStripMenuItem_ViewUsers.Name = "ToolStripMenuItem_ViewUsers"
        Me.ToolStripMenuItem_ViewUsers.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem_ViewUsers.Text = "View Users"
        '
        'DataConfigurationToolStripMenuItem
        '
        Me.DataConfigurationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ConfigurationData, Me.InstanceDataFlagsToolStripMenuItem, Me.RegionalInstanceDataToolStripMenuItem, Me.ManageItemAttributesToolStrinpMenuItem, Me.ManagePricingMethodsToolStripMenuItem, Me.ManageMenuAccessToolStripMenuItem, Me.TaxJurisdictionsToolStripMenuItem, Me.ZonesToolStripMenuItem, Me.CurrencyToolStripMenuItem})
        Me.DataConfigurationToolStripMenuItem.Name = "DataConfigurationToolStripMenuItem"
        Me.DataConfigurationToolStripMenuItem.Size = New System.Drawing.Size(122, 20)
        Me.DataConfigurationToolStripMenuItem.Text = "System Configuration"
        '
        'ToolStripMenuItem_ConfigurationData
        '
        Me.ToolStripMenuItem_ConfigurationData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManageToolStripMenuItem, Me.ToolStripSeparator2, Me.BuildToolStripMenuItem})
        Me.ToolStripMenuItem_ConfigurationData.Name = "ToolStripMenuItem_ConfigurationData"
        Me.ToolStripMenuItem_ConfigurationData.Size = New System.Drawing.Size(205, 22)
        Me.ToolStripMenuItem_ConfigurationData.Text = "Application Configuration"
        '
        'ManageToolStripMenuItem
        '
        Me.ManageToolStripMenuItem.Image = CType(resources.GetObject("ManageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ManageToolStripMenuItem.Name = "ManageToolStripMenuItem"
        Me.ManageToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.ManageToolStripMenuItem.Text = "Manage Configuration Settings"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(230, 6)
        '
        'BuildToolStripMenuItem
        '
        Me.BuildToolStripMenuItem.Image = CType(resources.GetObject("BuildToolStripMenuItem.Image"), System.Drawing.Image)
        Me.BuildToolStripMenuItem.Name = "BuildToolStripMenuItem"
        Me.BuildToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.BuildToolStripMenuItem.Text = "Build Configurations"
        '
        'InstanceDataFlagsToolStripMenuItem
        '
        Me.InstanceDataFlagsToolStripMenuItem.Name = "InstanceDataFlagsToolStripMenuItem"
        Me.InstanceDataFlagsToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.InstanceDataFlagsToolStripMenuItem.Text = "Instance Data Flags"
        '
        'RegionalInstanceDataToolStripMenuItem
        '
        Me.RegionalInstanceDataToolStripMenuItem.Name = "RegionalInstanceDataToolStripMenuItem"
        Me.RegionalInstanceDataToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.RegionalInstanceDataToolStripMenuItem.Text = "Instance Data"
        '
        'ManageItemAttributesToolStrinpMenuItem
        '
        Me.ManageItemAttributesToolStrinpMenuItem.Name = "ManageItemAttributesToolStrinpMenuItem"
        Me.ManageItemAttributesToolStrinpMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ManageItemAttributesToolStrinpMenuItem.Text = "Item Attributes"
        '
        'ManagePricingMethodsToolStripMenuItem
        '
        Me.ManagePricingMethodsToolStripMenuItem.Name = "ManagePricingMethodsToolStripMenuItem"
        Me.ManagePricingMethodsToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ManagePricingMethodsToolStripMenuItem.Text = "Pricing Methods"
        '
        'ManageMenuAccessToolStripMenuItem
        '
        Me.ManageMenuAccessToolStripMenuItem.Name = "ManageMenuAccessToolStripMenuItem"
        Me.ManageMenuAccessToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ManageMenuAccessToolStripMenuItem.Text = "Menu Access"
        '
        'TaxJurisdictionsToolStripMenuItem
        '
        Me.TaxJurisdictionsToolStripMenuItem.Name = "TaxJurisdictionsToolStripMenuItem"
        Me.TaxJurisdictionsToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.TaxJurisdictionsToolStripMenuItem.Text = "Tax Jurisdictions"
        '
        'ZonesToolStripMenuItem
        '
        Me.ZonesToolStripMenuItem.Name = "ZonesToolStripMenuItem"
        Me.ZonesToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ZonesToolStripMenuItem.Text = "Zones"
        '
        'CurrencyToolStripMenuItem
        '
        Me.CurrencyToolStripMenuItem.Name = "CurrencyToolStripMenuItem"
        Me.CurrencyToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.CurrencyToolStripMenuItem.Text = "Currency"
        '
        'BatchMgmtToolStripMenuItem
        '
        Me.BatchMgmtToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeBatchStateToolStripMenuItem, Me.RestoreDeletedItemToolStripMenuItem})
        Me.BatchMgmtToolStripMenuItem.Name = "BatchMgmtToolStripMenuItem"
        Me.BatchMgmtToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.BatchMgmtToolStripMenuItem.Text = "Data Tools"
        '
        'ChangeBatchStateToolStripMenuItem
        '
        Me.ChangeBatchStateToolStripMenuItem.Name = "ChangeBatchStateToolStripMenuItem"
        Me.ChangeBatchStateToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.ChangeBatchStateToolStripMenuItem.Text = "Change Batch State"
        '
        'RestoreDeletedItemToolStripMenuItem
        '
        Me.RestoreDeletedItemToolStripMenuItem.Name = "RestoreDeletedItemToolStripMenuItem"
        Me.RestoreDeletedItemToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.RestoreDeletedItemToolStripMenuItem.Text = "Restore Deleted Item"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_Region, Me.ToolStripStatusLabel_Environment, Me.ToolStripStatusLabel_Version})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 286)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(742, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel_Region
        '
        Me.ToolStripStatusLabel_Region.Name = "ToolStripStatusLabel_Region"
        Me.ToolStripStatusLabel_Region.Size = New System.Drawing.Size(47, 17)
        Me.ToolStripStatusLabel_Region.Text = "Region: "
        '
        'ToolStripStatusLabel_Environment
        '
        Me.ToolStripStatusLabel_Environment.Name = "ToolStripStatusLabel_Environment"
        Me.ToolStripStatusLabel_Environment.Size = New System.Drawing.Size(74, 17)
        Me.ToolStripStatusLabel_Environment.Text = "Environment: "
        '
        'ToolStripStatusLabel_Version
        '
        Me.ToolStripStatusLabel_Version.Name = "ToolStripStatusLabel_Version"
        Me.ToolStripStatusLabel_Version.Size = New System.Drawing.Size(49, 17)
        Me.ToolStripStatusLabel_Version.Text = "Version: "
        '
        'ManageTitlesToolStripMenuItem
        '
        Me.ManageTitlesToolStripMenuItem.Name = "ManageTitlesToolStripMenuItem"
        Me.ManageTitlesToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ManageTitlesToolStripMenuItem.Text = "Manage Titles"
        '
        'Form_IrmaAdministration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(742, 308)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Form_IrmaAdministration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IRMA Administration"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem_Stores As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ViewStores As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileWriters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ViewWriters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Users As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ViewUsers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ScheduleJobs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_POSPush As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ScalePush As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TlogProcessingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents POSPullToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PLUMHostToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel_Region As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_Environment As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_Version As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents DataConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InstanceDataFlagsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuildStorePOSFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_AuditExceptionsReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RegionalInstanceDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManageItemAttributesToolStrinpMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManagePricingMethodsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WeeklySalesRollupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseReceivingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SendOrdersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents APUploadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManageScheduledJobsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AverageCostUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BatchMgmtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeBatchStateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuildStoreScaleFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ConfigurationData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateStoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EInvoicingIntegrationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuildToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ManageMenuAccessToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TaxJurisdictionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZonesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreDeletedItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunScheduleJobsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrencyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManageTitlesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
