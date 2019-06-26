--==================================================================
-- Bug 8247: Icon has duplicated records in ItemSignAttribute table.
-- 
-- Script to delete duplicate records from ItemSign Attribute table.
-- Deletes are dumped to dbo.tmp_ItemSign_Duplicates for rollback if necessary.
--==================================================================
DECLARE @scriptKey VARCHAR(128) = 'DeleteItemSignAttributeDuplicates'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tmp_ItemSign_Duplicates')
		DROP TABLE dbo.tmp_ItemSign_Duplicates

	CREATE TABLE dbo.tmp_ItemSign_Duplicates
	(
		[ItemSignAttributeID] [int] NOT NULL,
		[ItemID] [int] NOT NULL,
		[AnimalWelfareRatingId] [int] NULL,
		[Biodynamic] [bit] NOT NULL,
		[CheeseMilkTypeId] [int] NULL,
		[CheeseRaw] [bit] NOT NULL,
		[EcoScaleRatingId] [int] NULL,
		[GlutenFreeAgencyName] [nvarchar](255) NULL,
		[HealthyEatingRatingId] [int] NULL,
		[KosherAgencyName] [nvarchar](255) NULL,
		[Msc] [bit] NOT NULL,
		[NonGmoAgencyName] [nvarchar](255) NULL,
		[OrganicAgencyName] [nvarchar](255) NULL,
		[PremiumBodyCare] [bit] NOT NULL,
		[SeafoodFreshOrFrozenId] [int] NULL,
		[SeafoodCatchTypeId] [int] NULL,
		[VeganAgencyName] [nvarchar](255) NULL,
		[Vegetarian] [bit] NOT NULL,
		[WholeTrade] [bit] NOT NULL,
		[GrassFed] [bit] NOT NULL,
		[PastureRaised] [bit] NOT NULL,
		[FreeRange] [bit] NOT NULL,
		[DryAged] [bit] NOT NULL,
		[AirChilled] [bit] NOT NULL,
		[MadeInHouse] [bit] NOT NULL,
		[CustomerFriendlyDescription] [nvarchar](60) NULL,
		[AnimalWelfareRating] [nvarchar](255) NULL,
		[MilkType] [nvarchar](255) NULL,
		[DeliverySystems] [nvarchar](255) NULL,
		[DrainedWeightUom] [nvarchar](255) NULL,
		[EcoScaleRating] [nvarchar](255) NULL,
		[FreshOrFrozen] [nvarchar](255) NULL,
		[SeafoodCatchType] [nvarchar](255) NULL
	)


	;WITH ItemSignDuplicates AS
	(
		SELECT
			[ItemSignAttributeID],
			[ItemID],
			[AnimalWelfareRatingId],
			[Biodynamic],
			[CheeseMilkTypeId],
			[CheeseRaw],
			[EcoScaleRatingId],
			[GlutenFreeAgencyName],
			[HealthyEatingRatingId],
			[KosherAgencyName],
			[Msc],
			[NonGmoAgencyName],
			[OrganicAgencyName],
			[PremiumBodyCare],
			[SeafoodFreshOrFrozenId],
			[SeafoodCatchTypeId],
			[VeganAgencyName],
			[Vegetarian],
			[WholeTrade],
			[GrassFed],
			[PastureRaised],
			[FreeRange],
			[DryAged],
			[AirChilled],
			[MadeInHouse],
			[CustomerFriendlyDescription],
			[AnimalWelfareRating],
			[MilkType],
			[DeliverySystems],
			[DrainedWeightUom],
			[EcoScaleRating],
			[FreshOrFrozen],
			[SeafoodCatchType],
			ROW_NUMBER() OVER(PARTITION BY ItemID ORDER BY ItemID) as RowNumber
		FROM ItemSignAttribute
	)

	DELETE d
		OUTPUT
			deleted.ItemSignAttributeID,
			deleted.ItemID,
			deleted.AnimalWelfareRatingId,
			deleted.Biodynamic,
			deleted.CheeseMilkTypeId,
			deleted.CheeseRaw,
			deleted.EcoScaleRatingId,
			deleted.GlutenFreeAgencyName,
			deleted.HealthyEatingRatingId,
			deleted.KosherAgencyName,
			deleted.Msc,
			deleted.NonGmoAgencyName,
			deleted.OrganicAgencyName,
			deleted.PremiumBodyCare,
			deleted.SeafoodFreshOrFrozenId,
			deleted.SeafoodCatchTypeId,
			deleted.VeganAgencyName,
			deleted.Vegetarian,
			deleted.WholeTrade,
			deleted.GrassFed,
			deleted.PastureRaised,
			deleted.FreeRange,
			deleted.DryAged,
			deleted.AirChilled,
			deleted.MadeInHouse,
			deleted.CustomerFriendlyDescription,
			deleted.AnimalWelfareRating,
			deleted.MilkType,
			deleted.DeliverySystems,
			deleted.DrainedWeightUom,
			deleted.EcoScaleRating,
			deleted.FreshOrFrozen,
			deleted.SeafoodCatchType
		INTO dbo.tmp_ItemSign_Duplicates
	FROM ItemSignDuplicates d
	WHERE RowNumber > 1

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO