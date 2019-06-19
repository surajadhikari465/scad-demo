CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesKit]
	@kitAttributes ItemKitAttributesType READONLY
AS
BEGIN 
	
	DECLARE @todayutc DATETIME = SYSUTCDATETIME();
	DECLARE @totalRecordCount int;
	DECLARE @insertRecordCount int;
	DECLARE @deleteRecordCount int;

	SELECT 
		[ItemID],			
		[KitchenItem],		
		[HospitalityItem],	
		[ImageUrl],			
		[KitchenDescription]
	INTO #itemKitAttributes 
	FROM @kitAttributes
	WHERE	KitchenItem is not null or
			HospitalityItem is not null or
			KitchenDescription is not null or
			ImageUrl is not null

	SET @totalRecordCount = @@ROWCOUNT;

SELECT 
		[ItemID],			
		[KitchenItem],		
		[HospitalityItem],	
		[ImageUrl],			
		[KitchenDescription]
	INTO #deleteItemKitAttributes 
	FROM @kitAttributes
	WHERE	KitchenItem is not null and
			HospitalityItem is not null and
			KitchenDescription is not null and
			ImageUrl is not null
	
	SET @deleteRecordCount = @@ROWCOUNT;



	SELECT 
		ka.ItemID, 
		ka.HospitalityItem, 
		ka.KitchenItem, 
		ka.KitchenDescription, 
		ka.ImageUrl
	INTO #insertKitAttributes
	FROM #itemKitAttributes ka
	WHERE NOT EXISTS (SELECT 1 FROM dbo.ItemAttributes_Kit ika WHERE ika.ItemID = ka.ItemID );

	SET @insertRecordCount = @@ROWCOUNT

	BEGIN TRY
	BEGIN TRAN

		IF @totalRecordCount <> @insertRecordCount
			UPDATE ika
			SET
				ika.HospitalityItem= a.HospitalityItem,
				ika.KitchenItem = a.KitchenItem,
				ika.Desc_Kitchen = a.KitchenDescription,
				ika.ImageUrl = a.ImageUrl, 
				ika.ModifiedUTCDate = @todayutc
			FROM dbo.ItemAttributes_Kit		ika
			INNER JOIN #itemKitAttributes a on ika.ItemID = a.ItemID

		if @deleteRecordCount > 0
		delete k
		from dbo.ItemAttributes_Kit k
		inner join  #deleteItemKitAttributes d 
			on k.ItemId = d.ItemID


		IF @insertRecordCount > 0 
			INSERT INTO dbo.ItemAttributes_Kit
			(
				ItemId,
				HospitalityItem, 
				KitchenItem, 
				Desc_Kitchen, 
				ImageUrl, 
				InsertUTCDate
			)
			SELECT 
				ItemID, 
				HospitalityItem, 
				KitchenItem, 
				KitchenDescription, 
				ImageUrl, 
				@todayutc
			FROM #insertKitAttributes

		COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
	    THROW
    END CATCH

END 
GO

GRANT EXECUTE ON [dbo].[AddOrUpdateItemAttributesKit] TO [MammothRole]
GO