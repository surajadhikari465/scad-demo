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
				,TARGET.LargeImageUrl = Source.LargeImageUrl
				,TARGET.SmallImageUrl = Source.SmallImageUrl
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
				,LargeImageUrl
				,SmallImageUrl
				)
			VALUES (
				Source.ItemId
				,Source.ScanCode
				,Source.ProductDesc
				,Source.CustomerFriendlyDesc
				,Source.KitchenDesc
				,Source.BrandName
				,Source.LargeImageUrl
				,Source.SmallImageUrl
				);
END

RETURN 0

