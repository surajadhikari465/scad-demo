
CREATE PROCEDURE [dbo].[EIM_UploadSessionPrices]
	@SessionID integer,
	@UploadRow_ID int,
	@Item_key int,
	@UploadToItemsStore bit

AS
	set nocount on

	-- ******************************************************
	-- Uploads price data for EIM
	-- David Marine
	-- ******************************************************

	DECLARE
		@Store_No int,
		@UploadValue_ID int,
	
		-- for error reporting
		@LastUploadTypeCodeBeforeError varchar(50),
		@LastUploadRowIdBeforeError int,
		@LastUploadValueIdBeforeError int

	-- check to see if we are suppose to do a price upload for the session
	IF EXISTS(
		SELECT *
		FROM UploadSessionUploadType ut (NOLOCK)
		WHERE ut.UploadSession_ID = @SessionID and ut.UploadType_Code = 'PRICE_UPLOAD'
		)
	BEGIN
				
		-- for any error message
		set @LastUploadTypeCodeBeforeError = 'PRICE_UPLOAD'
		set @LastUploadRowIdBeforeError = @UploadRow_ID

		IF @UploadToItemsStore = 1
		BEGIN

			-- the row price data is to be uploaded to the store
			-- stored in the row itself, which is the store the row
			-- data was originally loaded from
			EXEC EIM_UploadSessionPrice
				@UploadRow_ID,
				@item_key,
				-1,
				@LastUploadValueIdBeforeError OUTPUT

		END	
		ELSE
		BEGIN			
			-- all of the row price data is to be uploaded to the stores selected by the users
			-- loop through UploadSessionUploadTypeStore rows

			DECLARE PriceUploadStore_cursor CURSOR FOR
				SELECT Store_No
				FROM UploadSessionUploadTypeStore us (NOLOCK)
				inner join UploadSessionUploadType ut (NOLOCK)
				on us.UploadSessionUploadType_ID = ut.UploadSessionUploadType_ID
				WHERE ut.UploadSession_ID = @SessionID and ut.UploadType_Code = 'PRICE_UPLOAD'

			OPEN PriceUploadStore_cursor

			FETCH NEXT FROM PriceUploadStore_cursor
				INTO @Store_No

			WHILE @@FETCH_STATUS = 0
			BEGIN

					EXEC EIM_UploadSessionPrice
						@UploadRow_ID,
						@item_key,
						@Store_No,
						@LastUploadValueIdBeforeError OUTPUT

				FETCH NEXT FROM PriceUploadStore_cursor
					INTO @Store_No
			END
			
			CLOSE PriceUploadStore_cursor		
			DEALLOCATE PriceUploadStore_cursor
		END			
	END
	
