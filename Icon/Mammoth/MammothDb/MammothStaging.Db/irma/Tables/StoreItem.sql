CREATE TABLE [irma].[StoreItem] (
	[Region]				   NCHAR(2) NOT NULL,
    [StoreItemAuthorizationID] INT NOT NULL,
    [Store_No]                 INT NOT NULL,
    [Item_Key]                 INT NOT NULL,
    [Authorized]               BIT DEFAULT ((0)) NOT NULL,
    [POSDeAuth]                BIT DEFAULT ((0)) NOT NULL,
    [ScaleAuth]                BIT DEFAULT ((0)) NOT NULL,
    [ScaleDeAuth]              BIT DEFAULT ((0)) NOT NULL,
    [Refresh]                  BIT CONSTRAINT [DF_StoreItem_Refresh] DEFAULT ((0)) NOT NULL,
    [ECommerce]                BIT DEFAULT ((0)) NULL,
    CONSTRAINT [PK_StoreItem_StoreItemAuthorizationID] PRIMARY KEY CLUSTERED ([Region] ASC, [StoreItemAuthorizationID] ASC) WITH (FILLFACTOR = 100)
);

