CREATE TABLE [dbo].[JDA_StoreItemVendorSync] (
    [JDA_StoreItemVendorSync_ID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [ActionCode]                 CHAR (1) NOT NULL,
    [ApplyDate]                  DATETIME NOT NULL,
    [Store_No]                   INT      NOT NULL,
    [Item_Key]                   INT      NOT NULL,
    [Vendor_ID]                  INT      NOT NULL,
    [PrimaryVendor]              BIT      NOT NULL,
    [SyncState]                  TINYINT  DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JDA_StoreItemVendorSync] PRIMARY KEY CLUSTERED ([JDA_StoreItemVendorSync_ID] ASC)
);

