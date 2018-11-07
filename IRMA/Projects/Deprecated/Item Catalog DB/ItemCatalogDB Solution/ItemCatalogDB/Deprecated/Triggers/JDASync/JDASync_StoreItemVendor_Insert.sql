

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_StoreItemVendor_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_StoreItemVendor_Insert'
		DROP  Trigger JDASync_StoreItemVendor_Insert
	END
GO


PRINT 'Creating Trigger JDASync_StoreItemVendor_Insert'
GO
CREATE Trigger dbo.JDASync_StoreItemVendor_Insert 
ON StoreItemVendor
FOR INSERT
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
			'A',
			GetDate(),
			Inserted.Store_No,
			Inserted.Item_Key,
			Inserted.Vendor_ID,
			Inserted.PrimaryVendor	
		FROM Inserted
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_StoreItemVendor_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
