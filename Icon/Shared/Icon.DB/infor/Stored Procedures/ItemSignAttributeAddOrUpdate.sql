CREATE PROCEDURE infor.ItemSignAttributeAddOrUpdate 
	@sourceTable infor.ItemSignAttributeAddOrUpdateType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @certificationAgencyHierarchyId INT = (SELECT hierarchyID from Hierarchy h WHERE h.hierarchyName = 'Certification Agency Management')

	MERGE dbo.ItemSignAttribute AS Target
	USING @sourceTable AS Source
	ON Target.ItemId = Source.ItemId
	WHEN MATCHED THEN
	UPDATE SET
		AnimalWelfareRatingId = Source.AnimalWelfareRatingId,
		Biodynamic = Source.Biodynamic,
		CheeseMilkTypeId = Source.CheeseMilkTypeId,
		CheeseRaw = Source.CheeseRaw,
		EcoScaleRatingId = Source.EcoScaleRatingId,
		GlutenFreeAgencyName = Source.GlutenFreeAgencyName,
		KosherAgencyName = Source.KosherAgencyName,
		Msc = Source.Msc,
		NonGmoAgencyName = Source.NonGmoAgencyName,
		OrganicAgencyName = Source.OrganicAgencyName,
		PremiumBodyCare = Source.PremiumBodyCare,
		SeafoodFreshOrFrozenId = Source.SeafoodFreshOrFrozenId,
		SeafoodCatchTypeId = Source.SeafoodCatchTypeId,
		VeganAgencyName = Source.VeganAgencyName,
		Vegetarian = Source.Vegetarian,
		WholeTrade = Source.WholeTrade,
		GrassFed = Source.GrassFed,
		PastureRaised = Source.PastureRaised,
		FreeRange = Source.FreeRange,
		DryAged = Source.DryAged,
		AirChilled = Source.AirChilled,
		MadeInHouse = Source.MadeInHouse,
		CustomerFriendlyDescription = Source.CustomerFriendlyDescription
	WHEN NOT MATCHED THEN
	INSERT 
		(ItemId,
		AnimalWelfareRatingId,
		Biodynamic,
		CheeseMilkTypeId,
		CheeseRaw,
		EcoScaleRatingId,
		GlutenFreeAgencyName,
		KosherAgencyName,
		Msc,
		NonGmoAgencyName,
		OrganicAgencyName,
		PremiumBodyCare,
		SeafoodFreshOrFrozenId,
		SeafoodCatchTypeId,
		VeganAgencyName,
		Vegetarian,
		WholeTrade,
		GrassFed,
		PastureRaised,
		FreeRange,
		DryAged,
		AirChilled,
		MadeInHouse,
		CustomerFriendlyDescription)
		VALUES
		(Source.ItemId,
		Source.AnimalWelfareRatingId,
		Source.Biodynamic,
		Source.CheeseMilkTypeId,
		Source.CheeseRaw,
		Source.EcoScaleRatingId,
		Source.GlutenFreeAgencyName,
		Source.KosherAgencyName,
		Source.Msc,
		Source.NonGmoAgencyName,
		Source.OrganicAgencyName,
		Source.PremiumBodyCare,
		Source.SeafoodFreshOrFrozenId,
		Source.SeafoodCatchTypeId,
		Source.VeganAgencyName,
		Source.Vegetarian,
		Source.WholeTrade,
		Source.GrassFed,
		Source.PastureRaised,
		Source.FreeRange,
		Source.DryAged,
		Source.AirChilled,
		Source.MadeInHouse,
		Source.CustomerFriendlyDescription);
END
GO
