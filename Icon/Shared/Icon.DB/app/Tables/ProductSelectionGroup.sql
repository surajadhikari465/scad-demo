CREATE TABLE [app].[ProductSelectionGroup](
	[ProductSelectionGroupId] [int] IDENTITY(1,1) NOT NULL,
	[ProductSelectionGroupName] [nvarchar](131) NOT NULL,
	[ProductSelectionGroupTypeId] [int] NOT NULL,
	[TraitId] [int] NULL,
	[TraitValue] [nvarchar](255) NULL,
	[MerchandiseHierarchyClassId] [int] NULL
 CONSTRAINT [PK__ProductSelectionGroup] PRIMARY KEY CLUSTERED 
(
	[ProductSelectionGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [Unique_Name_TraitId_TraitValue] UNIQUE NONCLUSTERED 
(
	[ProductSelectionGroupName] ASC,
	[TraitId] ASC,
	[TraitValue] ASC,
	[MerchandiseHierarchyClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [app].[ProductSelectionGroup]  WITH CHECK ADD  CONSTRAINT [FK__ProductSelectionGroup_Trait] FOREIGN KEY([TraitId])
REFERENCES [dbo].[Trait] ([traitID])
GO

ALTER TABLE [app].[ProductSelectionGroup] CHECK CONSTRAINT [FK__ProductSelectionGroup_Trait]
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

ALTER TABLE [app].[ProductSelectionGroup]
ADD CONSTRAINT [CHK_Trait_Merchandise_Combination] CHECK
(
	(CASE
		WHEN (TraitId IS NOT NULL OR TraitValue IS NOT NULL) AND MerchandiseHierarchyClassId IS NOT NULL THEN 0
		WHEN (TraitId IS NOT NULL AND TraitValue IS NULL) AND MerchandiseHierarchyClassId IS NULL THEN 0
		WHEN (TraitId IS NULL AND TraitValue IS NOT NULL) AND MerchandiseHierarchyClassId IS NULL THEN 0
		ELSE 1
	END) = 1
)
GO