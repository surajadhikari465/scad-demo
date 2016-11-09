CREATE TABLE [dbo].[Attributes] (
    [AttributeID]      INT            IDENTITY (1, 1) NOT NULL,
    [AttributeGroupID] INT            NULL,
    [AttributeCode]    NVARCHAR (3)   NOT NULL,
    [AttributeDesc]    NVARCHAR (255) NULL,
    [AddedDate]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]     DATETIME       NULL,
    CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC) WITH (FILLFACTOR = 100)
);



