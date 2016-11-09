create table dbo.[ItemSignAttribute](
	[ItemSignAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[AnimalWelfareRatingId] [int] NULL,
	[Biodynamic] [bit] NOT NULL DEFAULT 0,
	[CheeseMilkTypeId] [int] NULL,
	[CheeseRaw] [bit] NOT NULL DEFAULT 0,
	[EcoScaleRatingId] [int] NULL,
	[GlutenFreeAgencyId] [int] NULL,
	[GlutenFreeAgencyName] [nvarchar](255) NULL,
	[HealthyEatingRatingId] [int] NULL,
	[KosherAgencyId] [int] NULL,
	[KosherAgencyName] [nvarchar](255) NULL,
	[Msc] [bit] NOT NULL DEFAULT 0,
	[NonGmoAgencyId] [int] NULL,
	[NonGmoAgencyName] [nvarchar](255) NULL,
	[OrganicAgencyId] [int],
	[OrganicAgencyName] [nvarchar](255) NULL,
	[PremiumBodyCare] [bit] NOT NULL DEFAULT 0,
	[SeafoodFreshOrFrozenId] [int],
	[SeafoodCatchTypeId] [int],
	[VeganAgencyId] [int],
	[VeganAgencyName] [nvarchar](255) NULL,
	[Vegetarian] [bit] NOT NULL DEFAULT 0,
	[WholeTrade] [bit] NOT NULL DEFAULT 0,
	[GrassFed] [bit] NOT NULL DEFAULT 0,
	[PastureRaised] [bit] NOT NULL DEFAULT 0,
	[FreeRange] [bit] NOT NULL DEFAULT 0,
	[DryAged] [bit] NOT NULL DEFAULT 0,
	[AirChilled] [bit] NOT NULL DEFAULT 0,
	[MadeInHouse] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_ItemSignAttribute] PRIMARY KEY CLUSTERED 
(
	[ItemSignAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX IX_ItemSignAttribute_ItemId 
    ON dbo.[ItemSignAttribute] ([ItemID]); 
GO


ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_ItemId] FOREIGN KEY([ItemID])
REFERENCES dbo.Item ([itemID])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_AnimalWelfareRatingId] FOREIGN KEY([AnimalWelfareRatingId])
REFERENCES dbo.AnimalWelfareRating ([AnimalWelfareRatingId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_CheeseMilkTypeId] FOREIGN KEY([CheeseMilkTypeId])
REFERENCES dbo.MilkType ([MilkTypeId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_HealthyEatingRatingId] FOREIGN KEY([HealthyEatingRatingId])
REFERENCES dbo.HealthyEatingRating ([HealthyEatingRatingId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_EcoScaleRatingId] FOREIGN KEY([EcoScaleRatingId])
REFERENCES dbo.EcoScaleRating ([EcoScaleRatingId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_SeafoodFreshOrFrozenId] FOREIGN KEY([SeafoodFreshOrFrozenId])
REFERENCES dbo.SeafoodFreshOrFrozen ([SeafoodFreshOrFrozenId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_SeafoodCatchTypeId] FOREIGN KEY([SeafoodCatchTypeId])
REFERENCES dbo.SeafoodCatchType ([SeafoodCatchTypeId])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_GlutenFreeAgencyId] FOREIGN KEY([GlutenFreeAgencyId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_KosherAgencyId] FOREIGN KEY([KosherAgencyId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_NonGmoAgencyId] FOREIGN KEY([NonGmoAgencyId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_OrganicAgencyId] FOREIGN KEY([OrganicAgencyId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_VeganAgencyId] FOREIGN KEY([VeganAgencyId])
REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID])
GO
