CREATE TABLE [dbo].[ShelfTagAttribute] (
    [ShelfTagAttributeID] INT           IDENTITY (1, 1) NOT NULL,
    [ShelfTagID]          INT           NOT NULL,
    [LabelTypeID]         INT           NOT NULL,
    [ShelfTag_Type]       VARCHAR (5)   NOT NULL,
    [ShelfTag_Ext]        VARCHAR (10)  NOT NULL,
    [ShelfTagAttrDesc]    VARCHAR (200) NULL,
    [CreateDate]          DATETIME      CONSTRAINT [DF_ShelfTagAttribute_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]          DATETIME      CONSTRAINT [DF_ShelfTagAttribute_ModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ShelfTagAttribute] PRIMARY KEY CLUSTERED ([ShelfTagAttributeID] ASC),
    CONSTRAINT [FK_ShelfTagAttribute_LabelType] FOREIGN KEY ([LabelTypeID]) REFERENCES [dbo].[LabelType] ([LabelType_ID])
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagAttribute] TO [IRMAReportsRole]
    AS [dbo];


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ShelfTagAttribute]
    ON [dbo].[ShelfTagAttribute]([ShelfTagID] ASC, [LabelTypeID] ASC, [ShelfTag_Type] ASC, [ShelfTagAttrDesc] ASC, [ShelfTag_Ext] ASC);

