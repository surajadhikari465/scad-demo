CREATE TABLE [dbo].[AttributeGroups] (
    [AttributeGroupID]   INT            NOT NULL,
    [AttributeGroupCode] NVARCHAR (3)   NOT NULL,
    [AttributeGroupDesc] NVARCHAR (255) NULL,
    [AddedDate]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    CONSTRAINT [PK_AttributeGroups] PRIMARY KEY CLUSTERED ([AttributeGroupID] ASC) WITH (FILLFACTOR = 100)
);

