CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesKit]
	@kitAttributes ItemKitAttributesType READONLY
AS
BEGIN 
	
	DECLARE @todayutc DATETIME = SYSUTCDATETIME();
	DECLARE @totalRecordCount int;
	DECLARE @insertRecordCount int;

	SELECT * INTO #itemKitAttributes FROM @kitAttributes;
	SET @totalRecordCount = @@ROWCOUNT;


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