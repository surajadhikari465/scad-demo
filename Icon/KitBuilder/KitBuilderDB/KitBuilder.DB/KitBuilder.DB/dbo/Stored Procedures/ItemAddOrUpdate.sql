CREATE PROCEDURE [dbo].[ItemAddOrUpdate] 
	@itemsTable dbo.ItemAddOrUpdateType READONLY
AS
BEGIN
	MERGE dbo.Items AS Target
	USING @itemsTable AS Source
		ON Target.ItemId = Source.ItemId
	WHEN MATCHED
		THEN
			UPDATE
			SET TARGET.ScanCode = Source.ScanCode
				,TARGET.ProductDesc = Source.ProductDesc
				,TARGET.CustomerFriendlyDesc = Source.CustomerFriendlyDesc
				,TARGET.KitchenDesc = Source.KitchenDesc
				,TARGET.BrandName = Source.BrandName
				,TARGET.ImageUrl = Source.ImageUrl
				,TARGET.InsertDateUtc = Source.InsertDateUtc
				,TARGET.LastUpdatedDateUtc = Source.LastUpdatedDateUtc
	WHEN NOT MATCHED BY TARGET
		THEN
			INSERT (
				ItemId
				,ScanCode
				,ProductDesc
				,CustomerFriendlyDesc
				,KitchenDesc
				,BrandName
				,ImageUrl
				)
			VALUES (
				Source.ItemId
				,Source.ScanCode
				,Source.ProductDesc
				,Source.CustomerFriendlyDesc
				,Source.KitchenDesc
				,Source.BrandName
				,Source.ImageUrl
				);
END

RETURN 0

