--======================================
-- ROLLBACK SCRIPT
-- Bug 8247: Icon has duplicated records in ItemSignAttribute table
--======================================

DELETE FROM app.PostDeploymentScriptHistory WHERE ScriptKey = 'DeleteItemSignAttributeDuplicates';

SET IDENTITY_INSERT dbo.ItemSignAttribute ON

INSERT INTO dbo.ItemSignAttribute
(
	[ItemSignAttributeID]
	,[ItemID]
	,[AnimalWelfareRatingId]
	,[Biodynamic]
	,[CheeseMilkTypeId]
	,[CheeseRaw]
	,[EcoScaleRatingId]
	,[GlutenFreeAgencyName]
	,[HealthyEatingRatingId]
	,[KosherAgencyName]
	,[Msc]
	,[NonGmoAgencyName]
	,[OrganicAgencyName]
	,[PremiumBodyCare]
	,[SeafoodFreshOrFrozenId]
	,[SeafoodCatchTypeId]
	,[VeganAgencyName]
	,[Vegetarian]
	,[WholeTrade]
	,[GrassFed]
	,[PastureRaised]
	,[FreeRange]
	,[DryAged]
	,[AirChilled]
	,[MadeInHouse]
	,[CustomerFriendlyDescription]
	,[AnimalWelfareRating]
	,[MilkType]
	,[DeliverySystems]
	,[DrainedWeightUom]
	,[EcoScaleRating]
	,[FreshOrFrozen]
	,[SeafoodCatchType]
)
SELECT
	[ItemSignAttributeID]
	,[ItemID]
	,[AnimalWelfareRatingId]
	,[Biodynamic]
	,[CheeseMilkTypeId]
	,[CheeseRaw]
	,[EcoScaleRatingId]
	,[GlutenFreeAgencyName]
	,[HealthyEatingRatingId]
	,[KosherAgencyName]
	,[Msc]
	,[NonGmoAgencyName]
	,[OrganicAgencyName]
	,[PremiumBodyCare]
	,[SeafoodFreshOrFrozenId]
	,[SeafoodCatchTypeId]
	,[VeganAgencyName]
	,[Vegetarian]
	,[WholeTrade]
	,[GrassFed]
	,[PastureRaised]
	,[FreeRange]
	,[DryAged]
	,[AirChilled]
	,[MadeInHouse]
	,[CustomerFriendlyDescription]
	,[AnimalWelfareRating]
	,[MilkType]
	,[DeliverySystems]
	,[DrainedWeightUom]
	,[EcoScaleRating]
	,[FreshOrFrozen]
	,[SeafoodCatchType]
FROM dbo.tmp_ItemSign_Duplicates

SET IDENTITY_INSERT dbo.ItemSignAttribute OFF