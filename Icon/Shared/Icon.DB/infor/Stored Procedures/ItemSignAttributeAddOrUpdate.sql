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
		AnimalWelfareRating = Source.AnimalWelfareRating,
		Biodynamic = Source.Biodynamic,
		MilkType = Source.MilkType,
		CheeseRaw = Source.CheeseRaw,
		EcoScaleRating = Source.EcoScaleRating,
		GlutenFreeAgencyName = Source.GlutenFreeAgencyName,
		KosherAgencyName = Source.KosherAgencyName,
		Msc = Source.Msc,
		NonGmoAgencyName = Source.NonGmoAgencyName,
		OrganicAgencyName = Source.OrganicAgencyName,
		PremiumBodyCare = Source.PremiumBodyCare,
		FreshOrFrozen = Source.FreshOrFrozen,
		SeafoodCatchType = Source.SeafoodCatchType,
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
		AnimalWelfareRating,
		Biodynamic,
		MilkType,
		CheeseRaw,
		EcoScaleRating,
		GlutenFreeAgencyName,
		KosherAgencyName,
		Msc,
		NonGmoAgencyName,
		OrganicAgencyName,
		PremiumBodyCare,
		FreshOrFrozen,
		SeafoodCatchType,
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
		Source.AnimalWelfareRating,
		Source.Biodynamic,
		Source.MilkType,
		Source.CheeseRaw,
		Source.EcoScaleRating,
		Source.GlutenFreeAgencyName,
		Source.KosherAgencyName,
		Source.Msc,
		Source.NonGmoAgencyName,
		Source.OrganicAgencyName,
		Source.PremiumBodyCare,
		Source.FreshOrFrozen,
		Source.SeafoodCatchType,
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
