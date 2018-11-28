create table dbo.[ItemSignAttribute](
	[ItemSignAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[AnimalWelfareRatingId] [int] NULL,
	[Biodynamic] [bit] NOT NULL DEFAULT 0,
	[CheeseMilkTypeId] [int] NULL,
	[CheeseRaw] [bit] NOT NULL DEFAULT 0,
	[EcoScaleRatingId] [int] NULL,
	[GlutenFreeAgencyName] [nvarchar](255) NULL,
	[HealthyEatingRatingId] [int] NULL,
	[KosherAgencyName] [nvarchar](255) NULL,
	[Msc] [bit] NOT NULL DEFAULT 0,
	[NonGmoAgencyName] [nvarchar](255) NULL,
	[OrganicAgencyName] [nvarchar](255) NULL,
	[PremiumBodyCare] [bit] NOT NULL DEFAULT 0,
	[SeafoodFreshOrFrozenId] [int],
	[SeafoodCatchTypeId] [int],
	[VeganAgencyName] [nvarchar](255) NULL,
	[Vegetarian] [bit] NOT NULL DEFAULT 0,
	[WholeTrade] [bit] NOT NULL DEFAULT 0,
	[GrassFed] [bit] NOT NULL DEFAULT 0,
	[PastureRaised] [bit] NOT NULL DEFAULT 0,
	[FreeRange] [bit] NOT NULL DEFAULT 0,
	[DryAged] [bit] NOT NULL DEFAULT 0,
	[AirChilled] [bit] NOT NULL DEFAULT 0,
	[MadeInHouse] [bit] NOT NULL DEFAULT 0,
	[CustomerFriendlyDescription] NVARCHAR (60) NULL,
	AnimalWelfareRating NVARCHAR(255) NULL,
	MilkType NVARCHAR(255) NULL,
	DeliverySystems NVARCHAR(255) NULL,
	DrainedWeightUom NVARCHAR(255) NULL,
	EcoScaleRating NVARCHAR(255) NULL,
	FreshOrFrozen NVARCHAR(255) NULL,
	SeafoodCatchType NVARCHAR(255) NULL
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

ALTER TABLE dbo.[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_HealthyEatingRatingId] FOREIGN KEY([HealthyEatingRatingId])
REFERENCES dbo.HealthyEatingRating ([HealthyEatingRatingId])
GO