

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemVendor_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemVendor_Update'
		DROP  Trigger JDASync_ItemVendor_Update
	END
GO


PRINT 'Creating Trigger JDASync_ItemVendor_Update'
GO
CREATE Trigger dbo.JDASync_ItemVendor_Update 
ON ItemVendor
FOR UPDATE
AS
BEGIN
	-- this is critical to the functioning of the audit
	-- it allows us to compare null to null
	SET ANSI_NULLS OFF

    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
		INSERT INTO JDA_ItemVendorSync
		(
			ActionCode,
			ApplyDate,
			Item_Key,
			Vendor_ID,
			Item_Id
		)
		SELECT
			'C',
			GetDate(),
			Inserted.Item_Key,
			Inserted.Vendor_ID,
			Inserted.Item_Id	
		FROM
			Inserted
			JOIN Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key
				AND Deleted.Vendor_ID = Inserted.Vendor_ID
		WHERE
			-- we care only if any of the columns we are tracking changes
			Inserted.Item_Key <> Deleted.Item_Key
			OR Inserted.Vendor_ID <> Deleted.Vendor_ID
			OR Inserted.Item_Id <> ISNULL(Deleted.Item_Id,-1)
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemVendor_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
