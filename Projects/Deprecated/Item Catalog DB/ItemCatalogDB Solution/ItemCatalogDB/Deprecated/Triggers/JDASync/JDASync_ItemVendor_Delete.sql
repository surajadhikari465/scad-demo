

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemVendor_Delete')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemVendor_Delete'
		DROP  Trigger JDASync_ItemVendor_Delete
	END
GO


PRINT 'Creating Trigger JDASync_ItemVendor_Delete'
GO
CREATE Trigger dbo.JDASync_ItemVendor_Delete 
ON ItemVendor
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
		Insert INTO JDA_ItemVendorSync
		(
			ActionCode,
			ApplyDate,
			Item_Key,
			Vendor_ID,
			Item_Id
		)
		SELECT
			'D',
			GetDate(),
			Deleted.Item_Key,
			Deleted.Vendor_ID,
			Deleted.Item_Id	
		FROM Deleted
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemVendor_Delete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
