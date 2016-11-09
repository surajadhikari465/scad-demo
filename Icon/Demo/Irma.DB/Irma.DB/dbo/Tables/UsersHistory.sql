CREATE TABLE [dbo].[UsersHistory] (
    [User_ID]                    INT          NOT NULL,
    [UserName]                   VARCHAR (25) NOT NULL,
    [FullName]                   VARCHAR (50) NULL,
    [Printer]                    VARCHAR (50) NULL,
    [CoverPage]                  VARCHAR (30) NULL,
    [EMail]                      VARCHAR (50) NULL,
    [Pager_Email]                VARCHAR (50) NULL,
    [Fax_Number]                 VARCHAR (15) NULL,
    [AccountEnabled]             BIT          NOT NULL,
    [SuperUser]                  BIT          NOT NULL,
    [PO_Accountant]              BIT          NOT NULL,
    [Accountant]                 BIT          NOT NULL,
    [Distributor]                BIT          NOT NULL,
    [FacilityCreditProcessor]    BIT          NOT NULL,
    [Buyer]                      BIT          NOT NULL,
    [Coordinator]                BIT          NOT NULL,
    [Item_Administrator]         BIT          NOT NULL,
    [Vendor_Administrator]       BIT          NOT NULL,
    [Lock_Administrator]         BIT          NOT NULL,
    [Telxon_Store_Limit]         INT          NULL,
    [Phone_Number]               VARCHAR (25) NULL,
    [Title]                      VARCHAR (60) NULL,
    [RecvLog_Store_Limit]        INT          NULL,
    [Warehouse]                  BIT          NOT NULL,
    [Effective_Date]             DATETIME     CONSTRAINT [DF__UsersHist__Effec__22ECB487] DEFAULT (getdate()) NOT NULL,
    [HOST_NAME]                  VARCHAR (20) CONSTRAINT [DF__UsersHist__HOST___23E0D8C0] DEFAULT (host_name()) NOT NULL,
    [PriceBatchProcessor]        BIT          CONSTRAINT [DF__UsersHist__Price__1EB20F4F] DEFAULT ((0)) NOT NULL,
    [Inventory_Administrator]    BIT          NULL,
    [BatchBuildOnly]             BIT          DEFAULT ((0)) NOT NULL,
    [DCAdmin]                    BIT          DEFAULT ((0)) NULL,
    [PromoAccessLevel]           SMALLINT     NULL,
    [CostAdmin]                  BIT          DEFAULT ((0)) NOT NULL,
    [VendorCostDiscrepancyAdmin] BIT          DEFAULT ((0)) NOT NULL,
    [POApprovalAdmin]            BIT          DEFAULT ((0)) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UsersHistoryUserID]
    ON [dbo].[UsersHistory]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersHistory] TO [IRMAReportsRole]
    AS [dbo];

