CREATE TABLE [dbo].[LastVendor] (
    [Item_Key]       INT           NOT NULL,
    [Store_No]       INT           NOT NULL,
    [Transfer_Order] INT           CONSTRAINT [DF__LastVendo__Trans__3A3865BB] DEFAULT ((0)) NOT NULL,
    [Vendor_ID]      INT           NOT NULL,
    [DateReceived]   SMALLDATETIME NULL,
    [DSD_Order]      BIT           CONSTRAINT [DF__LastVendo__DSD_O__3C20AE2D] DEFAULT ((0)) NOT NULL,
    [BuyingCycle]    INT           NULL,
    [LastVendor]     BIT           CONSTRAINT [DF__LastVendo__LastV__3E08F69F] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LastVendor_ItemKey_StoreNo_TransferOrder] PRIMARY KEY NONCLUSTERED ([Item_Key] ASC, [Store_No] ASC, [Transfer_Order] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__LastVendo__Store__7BB05806] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK_LastVendor_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
CREATE NONCLUSTERED INDEX [idxLastVendorVendor]
    ON [dbo].[LastVendor]([Vendor_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[LastVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LastVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LastVendor] TO [IRMAReportsRole]
    AS [dbo];

