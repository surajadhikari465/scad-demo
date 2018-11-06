CREATE TABLE [dbo].[InvalidCopyPOItems] (
    [InvalidCopyPOItems_ID] INT            NULL,
    [Item_Key]              INT            NULL,
    [Identifier]            VARCHAR (20)   NULL,
    [Item_Description]      VARCHAR (60)   NULL,
    [Brand_Name]            VARCHAR (50)   NULL,
    [Reason]                VARCHAR (5000) NULL,
    [ReasonType_ID]         INT            NULL,
    [Copy_From_PO]          INT            NULL,
    [Copy_To_PO]            INT            NULL,
    [InsertDate]            DATETIME       NULL
);

