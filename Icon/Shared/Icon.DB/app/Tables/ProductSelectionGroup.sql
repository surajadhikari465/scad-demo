CREATE TABLE [app].[ProductSelectionGroup](
	[ProductSelectionGroupId] [int] IDENTITY(1,1) NOT NULL,
	[ProductSelectionGroupName] [nvarchar](131) NOT NULL,
	[ProductSelectionGroupTypeId] [int] NOT NULL,
	[TraitId] [int] NULL,
	[TraitValue] [nvarchar](255) NULL,
	[MerchandiseHierarchyClassId] [int] NULL,
	[AttributeId] [int] NULL,
	[AttributeValue] [nvarchar](255) NULL,
 CONSTRAINT [PK__ProductSelectionGroup] PRIMARY KEY CLUSTERED 
(
	[ProductSelectionGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [Unique_Name_AttributeId_AttributeValue] UNIQUE NONCLUSTERED 
(
	[ProductSelectionGroupName] ASC,
	[AttributeId] ASC,
	[AttributeValue] ASC,
	[MerchandiseHierarchyClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[ProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK__ProductSelectionGroup_Attributes] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[Attributes] ([AttributeId])
GO

ALTER TABLE [app].[ProductSelectionGroup] CHECK CONSTRAINT [FK__ProductSelectionGroup_Attributes]
GO

ALTER TABLE [app].[ProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK_ProductSelectionGroup_ProductSelectionGroupType] FOREIGN KEY([ProductSelectionGroupTypeId])
REFERENCES [app].[ProductSelectionGroupType] ([ProductSelectionGroupTypeId])
GO

ALTER TABLE [app].[ProductSelectionGroup] CHECK CONSTRAINT [FK_ProductSelectionGroup_ProductSelectionGroupType]
GO

ALTER TABLE [app].[ProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK__ProductSelectionGroup_MerchandiseHierarchyClass] FOREIGN KEY([MerchandiseHierarchyClassId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO

ALTER TABLE [app].[ProductSelectionGroup] CHECK CONSTRAINT [FK__ProductSelectionGroup_MerchandiseHierarchyClass]
GO

ALTER TABLE [app].[ProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK__ProductSelectionGroup_Trait] FOREIGN KEY([TraitId])
REFERENCES [dbo].[Trait] ([TraitID])
GO

ALTER TABLE [app].[ProductSelectionGroup] CHECK CONSTRAINT [FK__ProductSelectionGroup_Trait]
GO

ALTER TABLE [app].[ProductSelectionGroup]
ADD CONSTRAINT [CHK_Trait_Merchandise_Combination] CHECK
(
	(CASE
		WHEN (AttributeId IS NOT NULL OR AttributeValue IS NOT NULL) AND MerchandiseHierarchyClassId IS NOT NULL THEN 0
		WHEN (AttributeId IS NOT NULL AND AttributeValue IS NULL) AND MerchandiseHierarchyClassId IS NULL THEN 0
		WHEN (AttributeId IS NULL AND AttributeValue IS NOT NULL) AND MerchandiseHierarchyClassId IS NULL THEN 0
		ELSE 1
	END) = 1
)
GO