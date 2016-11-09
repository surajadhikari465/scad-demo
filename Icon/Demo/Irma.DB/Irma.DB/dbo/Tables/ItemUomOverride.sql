CREATE TABLE [dbo].[ItemUomOverride] (
    [ItemUomOverride_ID]    INT          IDENTITY (1, 1) NOT NULL,
    [Item_Key]              INT          NOT NULL,
    [Store_No]              INT          NOT NULL,
    [Scale_ScaleUomUnit_ID] INT          NULL,
    [Scale_FixedWeight]     VARCHAR (25) NULL,
    [Scale_ByCount]         INT          NULL,
    [Retail_Unit_ID]        INT          NULL,
    CONSTRAINT [PK_ItemUomOverride] PRIMARY KEY CLUSTERED ([ItemUomOverride_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemUomOverride_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemUomOverride_RetailItemUnit] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit] FOREIGN KEY ([Scale_ScaleUomUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMAAdminRole]
    AS [dbo];

