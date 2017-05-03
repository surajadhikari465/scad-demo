
CREATE PROCEDURE [dbo].[EIM_UploadSessionCosts]
	@SessionID integer,
	@UploadRow_ID int,
	@Item_key int,
	@UploadToItemsStore bit

AS
	set nocount on

	-- ******************************************************
	-- Uploads cost data for EIM
	-- David Marine
	-- ******************************************************

	DECLARE
		@Store_No int,
		@UploadValue_ID int,
	
		-- for error reporting
		@LastUploadTypeCodeBeforeError varchar(50),
		@LastUploadRowIdBeforeError int,
		@LastUploadValueIdBeforeError int
			
	-- for any error message
	set @LastUploadTypeCodeBeforeError = 'COST_UPLOAD'
	set @LastUploadRowIdBeforeError = @UploadRow_ID

	IF @UploadToItemsStore = 1
	BEGIN			

		-- the row price data is to be uploaded to the store
		-- stored in the row itself, which is the store the row
		-- data was originally loaded from
		EXEC EIM_UploadSessionCost
			@UploadRow_ID,
			@item_key,
			-1,
			@LastUploadValueIdBeforeError OUTPUT
			
	END	
	ELSE
	BEGIN			
	
		-- all of the row price data is to be uploaded to the stores selected by the users
		-- ************** Loop through UploadSessionUploadTypeStore ***********************

		DECLARE CostUploadStore_cursor CURSOR FOR
			SELECT Store_No
			FROM UploadSessionUploadTypeStore us (NOLOCK)
			inner join UploadSessionUploadType ut (NOLOCK)
			on us.UploadSessionUploadType_ID = ut.UploadSessionUploadType_ID
			WHERE ut.UploadSession_ID = @SessionID and ut.UploadType_Code = 'COST_UPLOAD'

		OPEN CostUploadStore_cursor

		FETCH NEXT FROM CostUploadStore_cursor
		INTO @Store_No

		WHILE @@FETCH_STATUS = 0
		BEGIN

				EXEC EIM_UploadSessionCost
					@UploadRow_ID,
					@item_key,
					@Store_No,
					@LastUploadValueIdBeforeError OUTPUT

			FETCH NEXT FROM CostUploadStore_cursor
				INTO @Store_No
		END
	
		CLOSE CostUploadStore_cursor
		DEALLOCATE CostUploadStore_cursor
	END

