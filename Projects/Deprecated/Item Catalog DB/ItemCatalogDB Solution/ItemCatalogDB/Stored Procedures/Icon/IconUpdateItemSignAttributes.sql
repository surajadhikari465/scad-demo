CREATE PROCEDURE [dbo].[IconUpdateItemSignAttributes]
	@Items dbo.IconUpdateItemType READONLY
AS
BEGIN

	--Update/Insert ItemSignAttributes
	MERGE 
		INTO ItemSignAttribute isa
		USING 
			(SELECT ii.Item_Key,
					i.*
			 FROM
			 @Items i
			 JOIN ItemIdentifier ii on i.ScanCode = ii.Identifier
			 WHERE ii.Default_Identifier = 1 and ii.Remove_Identifier = 0 and ii.Deleted_Identifier = 0
				and i.HasItemSignAttributes = 1) i
		ON (isa.Item_Key = i.Item_Key)
	WHEN MATCHED THEN
		UPDATE SET isa.AnimalWelfareRating = i.AnimalWelfareRating,
				   isa.Biodynamic = i.Biodynamic,
				   isa.CheeseMilkType = i.CheeseMilkType,
				   isa.CheeseRaw = i.CheeseRaw,
				   isa.EcoScaleRating = i.EcoScaleRating,
				   isa.GlutenFree = i.GlutenFree,
				   isa.Kosher = i.Kosher, 
				   isa.NonGmo = i.NonGmo, 
				   isa.Organic = i.Organic, 
				   isa.PremiumBodyCare = i.PremiumBodyCare, 
				   isa.FreshOrFrozen = i.FreshOrFrozen, 
				   isa.SeafoodCatchType = i.SeafoodCatchType, 
				   isa.Vegan = i.Vegan, 
				   isa.Vegetarian = i.Vegetarian, 
				   isa.WholeTrade = i.WholeTrade, 
				   isa.Msc = i.Msc, 
				   isa.GrassFed = i.GrassFed, 
				   isa.PastureRaised = i.PastureRaised, 
				   isa.FreeRange = i.FreeRange, 
				   isa.DryAged = i.DryAged, 
				   isa.AirChilled = i.AirChilled, 
				   isa.MadeInHouse = i.MadeInHouse
	WHEN NOT MATCHED THEN
		INSERT (Item_Key, 
				AnimalWelfareRating, 
				Biodynamic, 
				CheeseMilkType, 
				CheeseRaw, 
				EcoScaleRating, 
				GlutenFree, 
				Kosher, 
				NonGmo, 
				Organic, 
				PremiumBodyCare, 
				FreshOrFrozen, 
				SeafoodCatchType, 
				Vegan, 
				Vegetarian, 
				WholeTrade, 
				Msc, 
				GrassFed, 
				PastureRaised, 
				FreeRange, 
				DryAged, 
				AirChilled, 
				MadeInHouse)
		VALUES (i.Item_Key,
				i.AnimalWelfareRating, 
				i.Biodynamic, 
				i.CheeseMilkType, 
				i.CheeseRaw, 
				i.EcoScaleRating, 
				i.GlutenFree, 
				i.Kosher, 
				i.NonGmo, 
				i.Organic, 
				i.PremiumBodyCare, 
				i.FreshOrFrozen, 
				i.SeafoodCatchType, 
				i.Vegan, 
				i.Vegetarian, 
				i.WholeTrade, 
				i.Msc, 
				i.GrassFed, 
				i.PastureRaised, 
				i.FreeRange, 
				i.DryAged, 
				i.AirChilled, 
				i.MadeInHouse);

	--Update Organic on Item table
	UPDATE Item
	SET Organic = CASE
						WHEN il.Organic = 1 THEN 1
						ELSE 0
				  END
	FROM Item i
	JOIN ItemIdentifier ii on i.Item_Key = ii.Item_Key
	JOIN @Items il on il.ScanCode = ii.Identifier
	WHERE i.Remove_Item = 0 and i.Deleted_Item = 0
		and ii.Default_Identifier = 1 and ii.Remove_Identifier = 0 and ii.Deleted_Identifier = 0
		and il.HasItemSignAttributes = 1
END