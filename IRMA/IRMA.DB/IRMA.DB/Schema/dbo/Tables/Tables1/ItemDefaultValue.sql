CREATE TABLE [dbo].[ItemDefaultValue] (
    [ItemDefaultValue_ID]     INT           IDENTITY (1, 1) NOT NULL,
    [ItemDefaultAttribute_ID] INT           NOT NULL,
    [ProdHierarchyLevel4_ID]  INT           NULL,
    [Category_ID]             INT           NULL,
    [Value]                   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ItemDefaultValue] PRIMARY KEY CLUSTERED ([ItemDefaultValue_ID] ASC),
    CONSTRAINT [FK_ItemDefaultValue_ItemCategory] FOREIGN KEY ([Category_ID]) REFERENCES [dbo].[ItemCategory] ([Category_ID]),
    CONSTRAINT [FK_ItemDefaultValue_ItemDefaultAttribute] FOREIGN KEY ([ItemDefaultAttribute_ID]) REFERENCES [dbo].[ItemDefaultAttribute] ([ItemDefaultAttribute_ID]),
    CONSTRAINT [FK_ItemDefaultValue_ProdHierarchyLevel4] FOREIGN KEY ([ProdHierarchyLevel4_ID]) REFERENCES [dbo].[ProdHierarchyLevel4] ([ProdHierarchyLevel4_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemDefaultValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemDefaultValue] TO [IRMAReportsRole]
    AS [dbo];

