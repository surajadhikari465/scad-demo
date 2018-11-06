IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetItemSignAttributeByItemKey')
	EXEC('CREATE PROCEDURE [dbo].[GetItemSignAttributeByItemKey] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[GetItemSignAttributeByItemKey]
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