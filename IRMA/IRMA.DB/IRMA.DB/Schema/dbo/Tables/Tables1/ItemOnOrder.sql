CREATE TABLE [dbo].[ItemOnOrder] (
    [Item_Key]       INT             NOT NULL,
    [Store_No]       INT             NOT NULL,
    [On_Order]       DECIMAL (18, 4) CONSTRAINT [DF__ItemOnOrd__On_Or__0E8400AF] DEFAULT ((0)) NULL,
    [Sold_By_Weight] BIT             CONSTRAINT [DF_ItemOnOrder_Sold_By_Weight] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemOnOrder_ItemKey_StoreNo] PRIMARY KEY NONCLUSTERED ([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__ItemOnOrd__Store__4B0D20AB] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_ItemOnOrder_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOnOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOnOrder] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOnOrder] TO [IRMAReportsRole]
    AS [dbo];

