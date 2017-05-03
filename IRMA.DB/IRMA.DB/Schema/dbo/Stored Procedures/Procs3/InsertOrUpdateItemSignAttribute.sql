
CREATE PROCEDURE [dbo].[InsertOrUpdateItemSignAttribute]
	@ItemKey [int],
	@Locality [nvarchar](50),
	@SignRomanceTextLong [nvarchar](300),
	@SignRomanceTextShort [nvarchar](140),
	@AnimalWelfareRating [nvarchar](10) = NULL,
	@Biodynamic [bit] = NULL,
	@CheeseMilkType [nvarchar](40) = NULL,
	@CheeseRaw [bit] = NULL,
	@EcoScaleRating [nvarchar](30) = NULL,
	@GlutenFree [bit] = NULL,
	@HealthyEatingRating [nvarchar](10) = NULL,
	@Kosher [bit] = NULL,
	@NonGmo [bit] = NULL,
	@Organic [bit] = NULL,
	@PremiumBodyCare [bit] = NULL,
	@ProductionClaims [nvarchar](30) = NULL,
	@FreshOrFrozen [nvarchar](30) = NULL,
	@SeafoodCatchType [nvarchar](15) = NULL,
	@Vegan [bit] = NULL,
	@Vegetarian [bit] = NULL,
	@WholeTrade [bit] = NULL,
	@UomRegulationChicagoBaby [nvarchar](50),
	@UomRegulationTagUom [int],
	@Exclusive [date],
	@ColorAdded [bit]
AS
BEGIN
	
	set nocount on;

    merge into
		ItemSignAttribute with (updlock, rowlock) isa
	using
		(values (@ItemKey)) as input(ItemKey)
	on
		isa.Item_Key = input.ItemKey
	when matched then
		update set 
			Locality = @Locality,
			SignRomanceTextLong = @SignRomanceTextLong,
			SignRomanceTextShort = @SignRomanceTextShort,
			AnimalWelfareRating = ISNULL(@AnimalWelfareRating, isa.AnimalWelfareRating),
			Biodynamic = ISNULL(@Biodynamic,isa.Biodynamic),
			CheeseMilkType = ISNULL(@CheeseMilkType, isa.CheeseMilkType),
			CheeseRaw = ISNULL(@CheeseRaw, isa.CheeseRaw),
			EcoScaleRating = ISNULL(@EcoScaleRating,isa.EcoScaleRating),
			GlutenFree = ISNULL(@GlutenFree, isa.GlutenFree),
			HealthyEatingRating = ISNULL(@HealthyEatingRating, isa.HealthyEatingRating),
			Kosher = ISNULL(@Kosher, isa.Kosher),
			NonGmo = ISNULL(@NonGmo, isa.NonGmo),
			Organic = ISNULL(@Organic, isa.Organic),
			PremiumBodyCare = ISNULL(@PremiumBodyCare, isa.PremiumBodyCare),
			ProductionClaims = ISNULL(@ProductionClaims, isa.ProductionClaims),
			FreshOrFrozen = ISNULL(@FreshOrFrozen, isa.FreshOrFrozen),
			SeafoodCatchType = ISNULL(@SeafoodCatchType,isa.SeafoodCatchType),
			Vegan = ISNULL(@Vegan,isa.Vegan),
			Vegetarian = ISNULL(@Vegetarian, isa.Vegetarian),
			WholeTrade = ISNULL(@WholeTrade, isa.WholeTrade),
			UomRegulationChicagoBaby = @UomRegulationChicagoBaby,
			UomRegulationTagUom = @UomRegulationTagUom,
			Exclusive = @Exclusive,
			ColorAdded = @ColorAdded
	when not matched then
		insert
			(Item_Key, Locality, SignRomanceTextLong, SignRomanceTextShort, AnimalWelfareRating, Biodynamic, CheeseMilkType, CheeseRaw, EcoScaleRating, GlutenFree,
			HealthyEatingRating, Kosher, NonGmo, Organic, PremiumBodyCare, ProductionClaims, FreshOrFrozen, SeafoodCatchType, Vegan, Vegetarian, WholeTrade,
			UomRegulationChicagoBaby, UomRegulationTagUom, Exclusive, ColorAdded)
		values
			(@ItemKey, @Locality, @SignRomanceTextLong, @SignRomanceTextShort, @AnimalWelfareRating, @Biodynamic, @CheeseMilkType, @CheeseRaw, @EcoScaleRating, @GlutenFree,
			@HealthyEatingRating, @Kosher, @NonGmo, @Organic, @PremiumBodyCare, @ProductionClaims, @FreshOrFrozen, @SeafoodCatchType, @Vegan, @Vegetarian, @WholeTrade,
			@UomRegulationChicagoBaby, @UomRegulationTagUom, @Exclusive, @ColorAdded)
	;

	-- Queue event for mammoth to refresh its data.
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @ItemKey, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemSignAttribute] TO [IRMAClientRole]
    AS [dbo];

