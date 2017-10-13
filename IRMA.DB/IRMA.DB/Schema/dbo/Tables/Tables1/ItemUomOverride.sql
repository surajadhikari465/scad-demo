CREATE TABLE [dbo].[ItemUomOverride] (
    [ItemUomOverride_ID]    INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Key]              INT          NOT NULL,
    [Store_No]              INT          NOT NULL,
    [Scale_ScaleUomUnit_ID] INT          NULL,
    [Scale_FixedWeight]     VARCHAR (25) NULL,
    [Scale_ByCount]         INT          NULL,
    [Retail_Unit_ID]        INT          NULL,
    CONSTRAINT [PK_ItemUomOverride] PRIMARY KEY CLUSTERED ([ItemUomOverride_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemUomOverride_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemUomOverride_RetailItemUnit] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit] FOREIGN KEY ([Scale_ScaleUomUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [AK_Item_Key_Store_No] UNIQUE NONCLUSTERED ([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80)
);


GO
ALTER TABLE [dbo].[ItemUomOverride] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemUomOverride] TO [spice_user]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemUomOverride] TO [TibcoDataWriter]
    AS [dbo];

GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemUomOverride] TO [TibcoDataWriter]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemUomOverride] TO [TibcoDataWriter]
    AS [dbo];
