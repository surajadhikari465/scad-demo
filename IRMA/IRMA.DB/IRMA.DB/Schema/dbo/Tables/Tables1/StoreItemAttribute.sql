CREATE TABLE [dbo].[StoreItemAttribute] (
    [StoreItemAttribute_ID] INT      IDENTITY (1, 1) NOT NULL,
    [Store_No]              INT      NOT NULL,
    [Item_Key]              INT      NOT NULL,
    [Exempt]                BIT      NOT NULL,
    [CreateDate]            DATETIME CONSTRAINT [DF_StoreItemAttribute_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]            DATETIME CONSTRAINT [DF_StoreItemAttribute_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [UserID]                INT      NULL,
    CONSTRAINT [PK_StoreItemAttribute] PRIMARY KEY CLUSTERED ([StoreItemAttribute_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemAttribute]
    ON [dbo].[StoreItemAttribute]([Store_No] ASC, [Item_Key] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemAttribute] TO [IRMAReportsRole]
    AS [dbo];

