

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemIdentifier_Delete')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemIdentifier_Delete'
		DROP  Trigger JDASync_ItemIdentifier_Delete
	END
GO


PRINT 'Creating Trigger JDASync_ItemIdentifier_Delete'
GO
CREATE Trigger dbo.JDASync_ItemIdentifier_Delete 
ON ItemIdentifier
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
		INSERT INTO JDA_ItemIdentifierSync
		(
			ActionCode,
			ApplyDate,
			Item_Key,
			Identifier,
			National_Identifier,
			ItemType_ID
		)
		SELECT
			'D',
			GetDate(),
			Deleted.Item_Key,
			Deleted.Identifier,
			Deleted.National_Identifier,
			Item.ItemType_ID	
		FROM Deleted
			JOIN Item (NOLOCK)
				ON Item.Item_Key = Deleted.Item_Key
		WHERE
			-- default identifiers should never be actually
			-- deleted, but if they are, we are not supposed
			-- to sync the change
			Deleted.Default_Identifier = 0
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemIdentifier_Delete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
