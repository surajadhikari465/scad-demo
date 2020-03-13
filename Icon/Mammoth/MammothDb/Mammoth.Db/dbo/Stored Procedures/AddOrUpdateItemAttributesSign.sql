CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesSign]
	@signAttributes [dbo].ItemSignAttributesType READONLY
AS
BEGIN
	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @today DATETIME = GETDATE();
	DECLARE @totalRecordCount int;
	DECLARE @insertRecordCount int;

	-- =========================================
	-- Insert / Update based on ItemID
	-- =========================================
	SELECT * INTO #signAttributes FROM @signAttributes;
	SET @totalRecordCount = @@ROWCOUNT;

	CREATE NONCLUSTERED INDEX [IX_SignAttributes_TempTable_ItemId] ON #signAttributes (ItemID);

	SELECT
		sa.ItemID,
		sa.Agency_GlutenFree,
		sa.Agency_Kosher,
		sa.Agency_NonGMO,
		sa.Agency_Organic,
		sa.Agency_Vegan,
		sa.CheeseMilkType,
		sa.IsAirChilled,
		sa.IsBiodynamic,
		sa.IsCheeseRaw,
		sa.IsDryAged,
		sa.IsFreeRange,
		sa.IsGrassFed,
		sa.IsMadeInHouse,
		sa.IsMsc,
		sa.IsPastureRaised,
		sa.IsPremiumBodyCare,
		sa.IsVegetarian,
		sa.IsWholeTrade,
		sa.Rating_AnimalWelfare,
		sa.Rating_EcoScale,
		sa.Rating_HealthyEating,
		sa.Seafood_CatchType,
		sa.Seafood_FreshOrFrozen
	INTO #insertSignAttributes
	FROM #signAttributes sa
	WHERE NOT EXISTS (SELECT 1 FROM dbo.ItemAttributes_Sign isa WHERE isa.ItemID = sa.ItemID);

	SET @insertRecordCount = @@ROWCOUNT

	BEGIN TRY
	BEGIN TRAN
		IF @totalRecordCount <> @insertRecordCount
			UPDATE isa
			SET
				isa.Agency_GlutenFree = a.Agency_GlutenFree,
				isa.Agency_Kosher = a.Agency_Kosher,
				isa.Agency_NonGMO = a.Agency_NonGMO,
				isa.Agency_Organic = a.Agency_Organic,
				isa.Agency_Vegan = a.Agency_Vegan,
				isa.CheeseMilkType = a.CheeseMilkType,
				isa.IsAirChilled = a.IsAirChilled,
				isa.IsBiodynamic = a.IsBiodynamic,
				isa.IsCheeseRaw = a.IsCheeseRaw,
				isa.IsDryAged = a.IsDryAged,
				isa.IsFreeRange = a.IsFreeRange,
				isa.IsGrassFed = a.IsGrassFed,
				isa.IsMadeInHouse = a.IsMadeInHouse,
				isa.IsMsc = a.IsMsc,
				isa.IsPastureRaised = a.IsPastureRaised,
				isa.IsPremiumBodyCare = a.IsPremiumBodyCare,
				isa.IsVegetarian = a.IsVegetarian,
				isa.IsWholeTrade = a.IsWholeTrade,
				isa.Rating_AnimalWelfare = a.Rating_AnimalWelfare,
				isa.Rating_EcoScale = a.Rating_EcoScale,
				isa.Rating_HealthyEating = a.Rating_HealthyEating,
				isa.Seafood_CatchType = a.Seafood_CatchType,
				isa.Seafood_FreshOrFrozen = a.Seafood_FreshOrFrozen,
				isa.ModifiedDate = @today
			FROM dbo.ItemAttributes_Sign		isa
			INNER JOIN #signAttributes			a on isa.ItemID = a.ItemID

		IF @insertRecordCount > 0
			INSERT INTO dbo.ItemAttributes_Sign
			(
				ItemID,
				Agency_GlutenFree,
				Agency_Kosher,
				Agency_NonGMO,
				Agency_Organic,
				Agency_Vegan,
				CheeseMilkType,
				IsAirChilled,
				IsBiodynamic,
				IsCheeseRaw,
				IsDryAged,
				IsFreeRange,
				IsGrassFed,
				IsMadeInHouse,
				IsMsc,
				IsPastureRaised,
				IsPremiumBodyCare,
				IsVegetarian,
				IsWholeTrade,
				Rating_AnimalWelfare,
				Rating_EcoScale,
				Rating_HealthyEating,
				Seafood_CatchType,
				Seafood_FreshOrFrozen,
				AddedDate
			)
			SELECT
				ItemID,
				Agency_GlutenFree,
				Agency_Kosher,
				Agency_NonGMO,
				Agency_Organic,
				Agency_Vegan,
				CheeseMilkType,
				IsAirChilled,
				IsBiodynamic,
				IsCheeseRaw,
				IsDryAged,
				IsFreeRange,
				IsGrassFed,
				IsMadeInHouse,
				IsMsc,
				IsPastureRaised,
				IsPremiumBodyCare,
				IsVegetarian,
				IsWholeTrade,
				Rating_AnimalWelfare,
				Rating_EcoScale,
				Rating_HealthyEating,
				Seafood_CatchType,
				Seafood_FreshOrFrozen,
				@today
			FROM #insertSignAttributes

		COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
	    THROW
    END CATCH
END
GO

GRANT EXECUTE ON [dbo].[AddOrUpdateItemAttributesSign] TO [MammothRole]
GO