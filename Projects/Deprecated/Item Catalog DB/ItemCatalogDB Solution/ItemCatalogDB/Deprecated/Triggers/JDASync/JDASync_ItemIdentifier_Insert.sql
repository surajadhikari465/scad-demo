

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemIdentifier_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemIdentifier_Insert'
		DROP  Trigger JDASync_ItemIdentifier_Insert
	END
GO


PRINT 'Creating Trigger JDASync_ItemIdentifier_Insert'
GO
CREATE Trigger dbo.JDASync_ItemIdentifier_Insert 
ON ItemIdentifier
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
			'A',
			GetDate(),
			Inserted.Item_Key,
			Inserted.Identifier,
			Inserted.National_Identifier,
			Item.ItemType_ID	
		FROM Inserted
			JOIN Item (NOLOCK)
				ON Item.Item_Key = Inserted.Item_Key
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemIdentifier_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
