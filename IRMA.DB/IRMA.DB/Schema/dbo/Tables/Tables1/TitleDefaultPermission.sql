CREATE TABLE [dbo].[TitleDefaultPermission] (
    [TitleId]                          INT DEFAULT ((0)) NULL,
    [Accountant]                       BIT DEFAULT ((0)) NULL,
    [BatchBuildOnly]                   BIT DEFAULT ((0)) NULL,
    [Buyer]                            BIT DEFAULT ((0)) NULL,
    [Coordinator]                      BIT DEFAULT ((0)) NULL,
    [CostAdministrator]                BIT DEFAULT ((0)) NULL,
    [FacilityCreditProcessor]          BIT DEFAULT ((0)) NULL,
    [DCAdmin]                          BIT DEFAULT ((0)) NULL,
    [EInvoicing]                       BIT DEFAULT ((0)) NULL,
    [InventoryAdministrator]           BIT DEFAULT ((0)) NULL,
    [ItemAdministrator]                BIT DEFAULT ((0)) NULL,
    [LockAdministrator]                BIT DEFAULT ((0)) NULL,
    [POAccountant]                     BIT DEFAULT ((0)) NULL,
    [POApprovalAdministrator]          BIT DEFAULT ((0)) NULL,
    [PriceBatchProcessor]              BIT DEFAULT ((0)) NULL,
    [Distributor]                      BIT DEFAULT ((0)) NULL,
    [SuperUser]                        BIT DEFAULT ((0)) NULL,
    [VendorAdministrator]              BIT DEFAULT ((0)) NULL,
    [VendorCostDiscrepancyAdmin]       BIT DEFAULT ((0)) NULL,
    [Warehouse]                        BIT DEFAULT ((0)) NULL,
    [ApplicationConfigAdmin]           BIT DEFAULT ((0)) NULL,
    [DataAdministrator]                BIT DEFAULT ((0)) NULL,
    [JobAdministrator]                 BIT DEFAULT ((0)) NULL,
    [POSInterfaceAdministrator]        BIT DEFAULT ((0)) NULL,
    [SecurityAdministrator]            BIT DEFAULT ((0)) NULL,
    [StoreAdministrator]               BIT DEFAULT ((0)) NULL,
    [SystemConfigurationAdministrator] BIT DEFAULT ((0)) NULL,
    [UserMaintenance]                  BIT DEFAULT ((0)) NULL,
    [Shrink]                           BIT DEFAULT ((0)) NOT NULL,
    [ShrinkAdmin]                      BIT DEFAULT ((0)) NOT NULL,
    [POEditor]                         BIT DEFAULT ((0)) NOT NULL,
    [DeletePO]                         BIT DEFAULT ((0)) NOT NULL,
    [TaxAdministrator]                 BIT DEFAULT ((0)) NOT NULL,
	[CancelAllSales]				   BIT DEFAULT ((0)) NOT NULL,
    FOREIGN KEY ([TitleId]) REFERENCES [dbo].[Title] ([Title_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TitleDefaultPermission] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TitleDefaultPermission] TO [IRMASchedJobsRole]
    AS [dbo];

