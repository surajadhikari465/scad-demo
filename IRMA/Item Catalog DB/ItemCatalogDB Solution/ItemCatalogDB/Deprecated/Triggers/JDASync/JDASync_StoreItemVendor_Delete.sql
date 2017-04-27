

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_StoreItemVendor_Delete')
	BEGIN
		PRINT 'Dropping Trigger JDASync_StoreItemVendor_Delete'
		DROP  Trigger JDASync_StoreItemVendor_Delete
	END
GO


PRINT 'Creating Trigger JDASync_StoreItemVendor_Delete'
GO
CREATE Trigger dbo.JDASync_StoreItemVendor_Delete 
ON StoreItemVendor
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
		INSERT INTO JDA_StoreItemVendorSync
		(
			ActionCode,
			ApplyDate,
			Store_No,
			Item_Key,
			Vendor_ID,
			PrimaryVendor
		)
		SELECT
			'D',
			GetDate(),
			Deleted.Store_No,
			Deleted.Item_Key,
			Deleted.Vendor_ID,
			Deleted.PrimaryVendor	
		FROM Deleted
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_StoreItemVendor_Delete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
