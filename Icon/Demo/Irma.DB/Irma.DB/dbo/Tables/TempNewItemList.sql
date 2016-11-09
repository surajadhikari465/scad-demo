CREATE TABLE [dbo].[TempNewItemList] (
    [item_key]     INT          IDENTITY (1, 1) NOT NULL,
    [Identifier]   VARCHAR (13) NULL,
    [Pushed]       BIT          CONSTRAINT [DF_TempNewItemList_Pushed] DEFAULT ((0)) NOT NULL,
    [ItemFlagged]  BIT          CONSTRAINT [DF_TempNewItemList_ItemFlagged] DEFAULT ((0)) NOT NULL,
    [PriceFlagged] BIT          CONSTRAINT [DF_TempNewItemList_PriceFlagged] DEFAULT ((0)) NOT NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TempNewItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempNewItemList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempNewItemList] TO [IRMAReportsRole]
    AS [dbo];

