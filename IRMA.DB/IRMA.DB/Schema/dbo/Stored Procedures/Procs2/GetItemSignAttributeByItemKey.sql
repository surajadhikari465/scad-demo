
CREATE PROCEDURE [dbo].[GetItemSignAttributeByItemKey]
	@Item_Key int
	
AS
	SELECT
	   [ItemSignAttributeID]
      ,[Item_Key]
      ,[Locality]
      ,[SignRomanceTextLong]
      ,[SignRomanceTextShort]
      ,[AnimalWelfareRating]
      ,[Biodynamic]
      ,[CheeseMilkType]
      ,[CheeseRaw]
      ,[EcoScaleRating]
      ,[GlutenFree]
      ,[HealthyEatingRating]
      ,[Kosher]
      ,[NonGmo]
      ,[PremiumBodyCare]
      ,[FreshOrFrozen]
      ,[SeafoodCatchType]
      ,[Vegan]
      ,[Vegetarian]
      ,[WholeTrade]
      ,[UomRegulationChicagoBaby]
      ,[UomRegulationTagUom]
      ,[Msc]
      ,[GrassFed]
      ,[PastureRaised]
      ,[FreeRange]
      ,[DryAged]
      ,[AirChilled]
      ,[MadeInHouse]
      ,[Exclusive]
      ,[ColorAdded]
	FROM ItemSignAttribute (NOLOCK) 
	WHERE Item_Key = @Item_Key

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.GetItemSignAttributeByItemKey.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSignAttributeByItemKey] TO [IRMAClientRole]
    AS [dbo];

