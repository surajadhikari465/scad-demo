CREATE PROCEDURE [dbo].[UpdateStoreScaleAuth]
        @StoreNo varchar(5)
AS
-- DaveStacey - 20080114 - This procedure updates a store's authorized scale items in the 
--		store item table to facilitate a full item scale file push through the admin client
--		This has not security grants as it should be exclusively used by the DBA

BEGIN
----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION

DECLARE
        @CodeLocation varchar(50)
SELECT @CodeLocation = 'UPDATE dbo.StoreItem....'


UPDATE dbo.StoreItem
SET ScaleAuth = 1 
FROM dbo.storeitem si
JOIN (
SELECT DISTINCT i.item_key, si.store_no
	FROM dbo.item i (NOLOCK) 
		JOIN itemidentifier ii (NOLOCK) ON i.item_key = ii.item_key
		JOIN dbo.itemscale isc (NOLOCK) ON i.item_key = isc.item_key
		JOIN dbo.scale_extratext sct (NOLOCK) ON sct.scale_extratext_id = isc.scale_extratext_id
		JOIN dbo.subteam s (NOLOCK) ON i.subteam_no = s.subteam_no
		JOIN dbo.price p (NOLOCK) ON i.item_key = p.item_key and p.store_no = @StoreNo
		JOIN dbo.itemunit iu (NOLOCK) ON isc.scale_scaleuomunit_id = iu.unit_id 
		JOIN dbo.storeitem si (NOLOCK) ON i.item_key = si.item_key AND p.store_no = si.Store_no
		JOIN dbo.storeitemvendor siv (NOLOCK) ON si.item_key = siv.item_key and siv.store_no = si.store_no 
	WHERE (ii.scale_identifier = 1 or dbo.fn_IsScaleItem(II.Identifier) = 1) and siv.primaryVendor = 1 AND si.Authorized = 1
) AS A
ON A.Item_Key = si.Item_Key AND A.Store_No = si.Store_No
--         ----------------------------------------------
--         -- Commit the transaction
--         ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Successfully updated scale auth for  ''' + @StoreNo + '''' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

END TRY
--===============================================================================================
BEGIN CATCH
        ----------------------------------------------
        -- Rollback the transaction
        ----------------------------------------------
        IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION

        ----------------------------------------------
        -- Display a detailed error message
        ----------------------------------------------
        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
                + CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH
--===============================================================================================
--
END