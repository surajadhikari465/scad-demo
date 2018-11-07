
CREATE PROCEDURE [dbo].[UpdateStoreItemVendorDiscontinue]
    @Discontinue bit,
	@Item_Key int, 
    @Store_No int = NULL,
	@Vendor_ID int = NULL

AS

-- **************************************************************************************************
-- Procedure: UpdateStoreItemVendorDiscontinue
--    Author: Benjamin Sims
--      Date: 12/20/2012
--
-- Description: This stored procedure is called by ItemStore.vb and other places in the IRMA code
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/20/2012	BS		8755	Created Document.  Updates the Discontinue flag in StoreItemVendor.
--								Logic is put in place for the future vendor-level discontinue
--								functionality.  But for now, users will only be able to Discontinue
--								an item at a store level.
-- **************************************************************************************************

BEGIN
	SET NOCOUNT ON
				
	BEGIN TRY
		DECLARE @DiscontinueUpdated bit
			
		IF @Vendor_ID IS NULL
		BEGIN
			SELECT @DiscontinueUpdated =  CASE WHEN DiscontinueItem IS NOT NULL AND @Discontinue IS NOT NULL AND DiscontinueItem <> @Discontinue THEN 1
												WHEN (DiscontinueItem IS NULL AND @Discontinue IS NOT NULL) OR (DiscontinueItem IS NOT NULL AND @Discontinue IS NULL) THEN 1
												ELSE 0
											END
			FROM StoreItemVendor
			WHERE
				StoreItemVendor.Item_Key = @Item_Key
				AND StoreItemVendor.Store_No = @Store_No

			-- Update StoreItemVendor for Store-Item combination
			UPDATE
				StoreItemVendor
			SET
				DiscontinueItem = @Discontinue
			WHERE
				StoreItemVendor.Item_Key = @Item_Key
				AND StoreItemVendor.Store_No = @Store_No
		END

		IF @Store_No IS NULL
		BEGIN
			SELECT @DiscontinueUpdated =  CASE WHEN DiscontinueItem IS NOT NULL AND @Discontinue IS NOT NULL AND DiscontinueItem <> @Discontinue THEN 1
												WHEN (DiscontinueItem IS NULL AND @Discontinue IS NOT NULL) OR (DiscontinueItem IS NOT NULL AND @Discontinue IS NULL) THEN 1
												ELSE 0
											END
			FROM StoreItemVendor
			WHERE
				StoreItemVendor.Item_Key = @Item_Key
				AND StoreItemVendor.Vendor_ID = @Vendor_ID

			-- Update StoreItemVendor for Item-Vendor combination
			UPDATE
				StoreItemVendor
			SET
				DiscontinueItem = @Discontinue
			WHERE
				StoreItemVendor.Item_Key = @Item_Key
				AND StoreItemVendor.Vendor_ID = @Vendor_ID
		END

		IF	@DiscontinueUpdated  = 1 
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, @Store_No, 'ItemLocaleAddOrUpdate', NULL, NULL
	END
					
	END TRY

	BEGIN CATCH	
		DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
		RAISERROR ('UpdateStoreItemVendorDiscontinue failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
	END CATCH
		
END

SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemVendorDiscontinue] TO [IRMAClientRole]
    AS [dbo];

