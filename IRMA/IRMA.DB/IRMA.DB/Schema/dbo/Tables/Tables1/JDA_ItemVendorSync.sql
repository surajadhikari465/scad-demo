CREATE TABLE [dbo].[JDA_ItemVendorSync] (
    [JDA_ItemVendorSync_ID] INT          IDENTITY (1, 1) NOT NULL,
    [ActionCode]            CHAR (1)     NOT NULL,
    [ApplyDate]             DATETIME     NOT NULL,
    [Item_Key]              INT          NOT NULL,
    [Vendor_ID]             INT          NOT NULL,
    [Item_Id]               VARCHAR (20) NULL,
    [SyncState]             TINYINT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JDA_ItemVendorSync] PRIMARY KEY CLUSTERED ([JDA_ItemVendorSync_ID] ASC)
);

