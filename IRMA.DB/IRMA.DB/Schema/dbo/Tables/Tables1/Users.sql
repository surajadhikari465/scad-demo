CREATE TABLE [dbo].[Users] (
    [User_ID]                          INT          IDENTITY (1, 1) NOT NULL,
    [UserName]                         VARCHAR (25) NOT NULL,
    [FullName]                         VARCHAR (50) NULL,
    [Printer]                          VARCHAR (50) NULL,
    [CoverPage]                        VARCHAR (30) CONSTRAINT [DF_CoverPage] DEFAULT ('default') NULL,
    [EMail]                            VARCHAR (50) NULL,
    [Pager_Email]                      VARCHAR (50) NULL,
    [Fax_Number]                       VARCHAR (15) NULL,
    [AccountEnabled]                   BIT          CONSTRAINT [DF__Users__AccountEn__7B3C2211] DEFAULT ((1)) NOT NULL,
    [SuperUser]                        BIT          CONSTRAINT [DF__Users__SuperUser__7C30464A] DEFAULT ((0)) NOT NULL,
    [PO_Accountant]                    BIT          CONSTRAINT [DF__Users__Buyer__7E188EBC] DEFAULT ((0)) NOT NULL,
    [Accountant]                       BIT          CONSTRAINT [DF__Users__Accountan__7F0CB2F5] DEFAULT ((0)) NOT NULL,
    [Distributor]                      BIT          CONSTRAINT [DF__Users__Distribut__0000D72E] DEFAULT ((0)) NOT NULL,
    [FacilityCreditProcessor]          BIT          CONSTRAINT [DF__Users__Approve_T__00F4FB67] DEFAULT ((0)) NOT NULL,
    [Buyer]                            BIT          CONSTRAINT [DF__Users__Reserved___01E91FA0] DEFAULT ((0)) NOT NULL,
    [Coordinator]                      BIT          CONSTRAINT [DF__Users__Reserved___03D16812] DEFAULT ((0)) NOT NULL,
    [Item_Administrator]               BIT          CONSTRAINT [DF_Users_Item_Administrator] DEFAULT ((0)) NOT NULL,
    [Vendor_Administrator]             BIT          CONSTRAINT [DF_Users_Vendor_Administrator] DEFAULT ((0)) NOT NULL,
    [Lock_Administrator]               BIT          CONSTRAINT [DF_Users_Lock_Administrator] DEFAULT ((0)) NOT NULL,
    [Telxon_Store_Limit]               INT          NULL,
    [Phone_Number]                     VARCHAR (25) NULL,
    [Title]                            INT          NULL,
    [RecvLog_Store_Limit]              INT          NULL,
    [Warehouse]                        BIT          CONSTRAINT [DF__Users__Warehouse__6AE38E7C] DEFAULT ((0)) NOT NULL,
    [PriceBatchProcessor]              BIT          CONSTRAINT [DF_Users_PriceBatchProcessor] DEFAULT ((0)) NOT NULL,
    [Inventory_Administrator]          BIT          CONSTRAINT [DF_Users_Inventory_Administrator] DEFAULT ((0)) NOT NULL,
    [BatchBuildOnly]                   BIT          DEFAULT ((0)) NOT NULL,
    [DCAdmin]                          BIT          DEFAULT ((0)) NULL,
    [PromoAccessLevel]                 SMALLINT     NULL,
    [CostAdmin]                        BIT          CONSTRAINT [DF_Users_CostAdmin] DEFAULT ((0)) NOT NULL,
    [POApprovalAdmin]                  BIT          CONSTRAINT [DF_Users_POApprovalAdmin] DEFAULT ((0)) NOT NULL,
    [EInvoicing_Administrator]         BIT          DEFAULT ((0)) NOT NULL,
    [VendorCostDiscrepancyAdmin]       BIT          DEFAULT ((0)) NOT NULL,
    [ApplicationConfigAdmin]           BIT          DEFAULT ((0)) NOT NULL,
    [DataAdministrator]                BIT          DEFAULT ((0)) NOT NULL,
    [JobAdministrator]                 BIT          DEFAULT ((0)) NOT NULL,
    [POSInterfaceAdministrator]        BIT          DEFAULT ((0)) NOT NULL,
    [SecurityAdministrator]            BIT          DEFAULT ((0)) NOT NULL,
    [StoreAdministrator]               BIT          DEFAULT ((0)) NOT NULL,
    [SystemConfigurationAdministrator] BIT          DEFAULT ((0)) NOT NULL,
    [UserMaintenance]                  BIT          DEFAULT ((0)) NOT NULL,
    [Shrink]                           BIT          DEFAULT ((0)) NOT NULL,
    [ShrinkAdmin]                      BIT          DEFAULT ((0)) NOT NULL,
    [POEditor]                         BIT          DEFAULT ((0)) NOT NULL,
    [DeletePO]                         BIT          DEFAULT ((0)) NOT NULL,
    [TaxAdministrator]                 BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users_UserID] PRIMARY KEY CLUSTERED ([User_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Users_AccessLevel] FOREIGN KEY ([PromoAccessLevel]) REFERENCES [dbo].[UserAccess] ([UserAccessLevel_ID]),
    CONSTRAINT [FK_Users_Store1] FOREIGN KEY ([Telxon_Store_Limit]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Users_TelxonStoreLimit] FOREIGN KEY ([Telxon_Store_Limit]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Users_Title] FOREIGN KEY ([Title]) REFERENCES [dbo].[Title] ([Title_ID])
);


GO
ALTER TABLE [dbo].[Users] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUserName]
    ON [dbo].[Users]([UserName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [UsersAddUpdateDelete]
ON [dbo].[Users]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO UsersHistory ([User_ID], UserName, FullName, Printer, CoverPage, EMail, Pager_Email, Fax_Number,  AccountEnabled, SuperUser, 
							  PO_Accountant, Accountant, Distributor, FacilityCreditProcessor, Buyer, Coordinator, Item_Administrator, 
							  Vendor_Administrator, Lock_Administrator, Telxon_Store_Limit, Phone_Number, Title, RecvLog_Store_Limit, 
							  Warehouse,  PriceBatchProcessor, Inventory_Administrator,	BatchBuildOnly, DCAdmin, PromoAccessLevel, 
							  CostAdmin, VendorCostDiscrepancyAdmin, POApprovalAdmin)
    SELECT [User_ID], UserName, FullName, Printer, CoverPage, EMail, Pager_Email, Fax_Number,  AccountEnabled, SuperUser, 
		   PO_Accountant, Accountant, Distributor, FacilityCreditProcessor, Buyer, Coordinator, Item_Administrator, 
		   Vendor_Administrator, Lock_Administrator, Telxon_Store_Limit, Phone_Number, Title, RecvLog_Store_Limit,  
		   Warehouse, PriceBatchProcessor, Inventory_Administrator, BatchBuildOnly, DCAdmin, PromoAccessLevel, 
		   CostAdmin, VendorCostDiscrepancyAdmin, POApprovalAdmin
    FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        DELETE UsersHistory
        FROM UsersHistory H
        INNER JOIN
            Deleted ON Deleted.[User_ID] = H.[User_ID]
        LEFT JOIN
            Inserted ON Deleted.[User_ID] = Inserted.[User_ID]
        WHERE Inserted.[User_ID] IS NULL

        SELECT @Error_No = @@ERROR
    END
    
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UsersAddUpdateDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[Users] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Users] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Users] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Users] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Users] TO [iCONReportingRole]
    AS [dbo];

