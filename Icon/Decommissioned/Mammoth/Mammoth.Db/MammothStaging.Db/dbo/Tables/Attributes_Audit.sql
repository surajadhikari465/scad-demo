CREATE TABLE [dbo].[Attributes_Audit] (
    [AttributeID]      INT            NOT NULL,
    [AttributeGroupID] INT            NULL,
    [AttributeCode]    NVARCHAR (3)   NOT NULL,
    [AttributeDesc]    NVARCHAR (255) NULL,
    [AddedDate]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]     DATETIME       NULL,
    CONSTRAINT [PK_Attributes_Audit] PRIMARY KEY CLUSTERED ([AttributeID] ASC) WITH (FILLFACTOR = 100)
);