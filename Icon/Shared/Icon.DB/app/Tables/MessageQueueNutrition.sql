
CREATE TABLE [app].[MessageQueueNutrition](
	[MessageQueueNutritionId] [int] IDENTITY(1,1) NOT NULL,
	[MessageQueueId] [int] NOT NULL,
	[Plu] [varchar](50) NULL,
	[RecipeName] [nvarchar](100) NULL,
	[Allergens] [nvarchar](510) NULL,
	[Ingredients] [nvarchar](4000) NULL,
	[ServingsPerPortion] [float] NULL,
	[ServingSizeDesc] [nvarchar](50) NULL,
	[ServingPerContainer] [nvarchar](50) NULL,
	[HshRating] [int] NULL,
	[ServingUnits] [tinyint] NULL,
	[SizeWeight] [int] NULL,
	[Calories] [int] NULL,
	[CaloriesFat] [int] NULL,
	[CaloriesSaturatedFat] [int] NULL,
	[TotalFatWeight] [decimal](10, 1) NULL,
	[TotalFatPercentage] [smallint] NULL,
	[SaturatedFatWeight] [decimal](10, 1) NULL,
	[SaturatedFatPercent] [smallint] NULL,
	[PolyunsaturatedFat] [decimal](10, 1) NULL,
	[MonounsaturatedFat] [decimal](10, 1) NULL,
	[CholesterolWeight] [decimal](10, 1) NULL,
	[CholesterolPercent] [smallint] NULL,
	[SodiumWeight] [decimal](10, 1) NULL,
	[SodiumPercent] [smallint] NULL,
	[PotassiumWeight] [decimal](10, 1) NULL,
	[PotassiumPercent] [smallint] NULL,
	[TotalCarbohydrateWeight] [decimal](10, 1) NULL,
	[TotalCarbohydratePercent] [smallint] NULL,
	[DietaryFiberWeight] [decimal](10, 1) NULL,
	[DietaryFiberPercent] [smallint] NULL,
	[SolubleFiber] [decimal](10, 1) NULL,
	[InsolubleFiber] [decimal](10, 1) NULL,
	[Sugar] [decimal](10, 1) NULL,
	[SugarAlcohol] [decimal](10, 1) NULL,
	[OtherCarbohydrates] [decimal](10, 1) NULL,
	[ProteinWeight] [decimal](10, 1) NULL,
	[ProteinPercent] [smallint] NULL,
	[VitaminA] [smallint] NULL,
	[Betacarotene] [smallint] NULL,
	[VitaminC] [smallint] NULL,
	[Calcium] [smallint] NULL,
	[Iron] [smallint] NULL,
	[VitaminD] [smallint] NULL,
	[VitaminE] [smallint] NULL,
	[Thiamin] [smallint] NULL,
	[Riboflavin] [smallint] NULL,
	[Niacin] [smallint] NULL,
	[VitaminB6] [smallint] NULL,
	[Folate] [smallint] NULL,
	[VitaminB12] [smallint] NULL,
	[Biotin] [smallint] NULL,
	[PantothenicAcid] [smallint] NULL,
	[Phosphorous] [smallint] NULL,
	[Iodine] [smallint] NULL,
	[Magnesium] [smallint] NULL,
	[Zinc] [smallint] NULL,
	[Copper] [smallint] NULL,
	[Transfat] [decimal](10, 1) NULL,
	[CaloriesFromTransfat] [int] NULL,
	[Om6Fatty] [decimal](10, 1) NULL,
	[Om3Fatty] [decimal](10, 1) NULL,
	[Starch] [decimal](10, 1) NULL,
	[Chloride] [smallint] NULL,
	[Chromium] [smallint] NULL,
	[VitaminK] [smallint] NULL,
	[Manganese] [smallint] NULL,
	[Molybdenum] [smallint] NULL,
	[Selenium] [smallint] NULL,
	[TransfatWeight] [decimal](10, 1) NULL,
	[HazardousMaterialFlag] [int] NULL,
	[HazardousMaterialTypeCode] [varchar](4) NULL,
	[InsertDate] [datetime2](7) NOT NULL,
	[AddedSugarsWeight] DECIMAL(10, 1) NULL,
	[AddedSugarsPercent] SMALLINT NULL,
	[CalciumWeight] DECIMAL(10, 1) NULL,
	[IronWeight] DECIMAL(10, 1) NULL,
	[VitaminDWeight] DECIMAL(10, 1) NULL,
	[ProfitCenter] INT NULL,
    [CanadaAllergens] NVARCHAR(510) NULL, 
    [CanadaIngredients] NVARCHAR(4000) NULL, 
    [CanadaSugarPercent] SMALLINT NULL,
    [CanadaServingSizeDesc] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_MessageQueueNutrition] PRIMARY KEY CLUSTERED 
(
	[MessageQueueNutritionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [app].[MessageQueueNutrition] ADD  DEFAULT (sysdatetime()) FOR [InsertDate]
GO

ALTER TABLE [app].[MessageQueueNutrition]  WITH CHECK ADD  CONSTRAINT [FK_app.[MessageQueueNutrition_MessageQueueId] FOREIGN KEY([MessageQueueId])
REFERENCES [app].[MessageQueueProduct] ([MessageQueueId])
ON DELETE CASCADE
GO

ALTER TABLE [app].[MessageQueueNutrition] CHECK CONSTRAINT [FK_app.[MessageQueueNutrition_MessageQueueId]
GO