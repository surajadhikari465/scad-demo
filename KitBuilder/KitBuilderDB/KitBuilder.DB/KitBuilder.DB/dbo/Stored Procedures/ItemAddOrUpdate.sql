CREATE PROCEDURE [dbo].[ItemAddOrUpdate] 
	@itemsTable dbo.ItemAddOrUpdateType READONLY
AS
BEGIN

	DECLARE @totalCount INT
	DECLARE @insertCount INT

	SELECT 
		ItemId ,             
		ScanCode,            
		ProductDesc,         
		CustomerFriendlyDesc,
		KitchenDesc,       
		BrandName,           
		ImageUrl,
		FlexibleText
	INTO #allItems
	FROM @itemsTable

	SET @totalCount = @@ROWCOUNT


	CREATE NONCLUSTERED INDEX IX_AddOrUpdateItems_ItemId ON #allItems (ItemId)

	SELECT
		ItemId ,             
		ScanCode,            
		ProductDesc,         
		CustomerFriendlyDesc,
		KitchenDesc,       
		BrandName,           
		ImageUrl,
		FlexibleText  
	INTO #insertItems
	FROM #allItems ai
	WHERE NOT EXISTS (
		SELECT 1 FROM dbo.Items i
		WHERE ai.ItemId = i.ItemId
		)

	 SET @insertCount = @@ROWCOUNT

	 CREATE NONCLUSTERED INDEX IX_InsertItems_ItemId ON #insertItems (ItemId)

	BEGIN TRY	
	BEGIN TRANSACTION

		IF @insertCount <> @totalCount
			UPDATE	i
				SET
					i.ScanCode = ai.ScanCode
					,i.ProductDesc = ai.ProductDesc
					,i.CustomerFriendlyDesc = ai.CustomerFriendlyDesc
					,i.KitchenDesc = ai.KitchenDesc
					,i.BrandName = ai.BrandName
					,i.ImageUrl = ai.ImageUrl
					,i.InsertDateUtc = ai.InsertDateUtc
					,i.LastUpdatedDateUtc = ai.LastUpdatedDateUtc
					,i.FlexibleText = ai.FlexibleText
			FROM dbo.Items i
				INNER JOIN #allItems AI ON i.ItemId = AI.ItemId

		IF @insertCount > 0
			INSERT INTO dbo.Items
			(
				ItemId
				,ScanCode
				,ProductDesc
				,CustomerFriendlyDesc
				,KitchenDesc
				,BrandName
				,ImageUrl
				,FlexibleText
			)
			SELECT 
				II.ItemId
				,II.ScanCode
				,II.ProductDesc
				,II.CustomerFriendlyDesc
				,II.KitchenDesc
				,II.BrandName
				,II.ImageUrl
				,II.FlexibleText
			FROM #insertItems II
		
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION;
	    THROW
	END CATCH
END

