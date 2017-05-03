

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemIdentifier_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemIdentifier_Update'
		DROP  Trigger JDASync_ItemIdentifier_Update
	END
GO


PRINT 'Creating Trigger JDASync_ItemIdentifier_Update'
GO
CREATE Trigger dbo.JDASync_ItemIdentifier_Update 
ON ItemIdentifier
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
			'C',
			GetDate(),
			Inserted.Item_Key,
			Inserted.Identifier,
			Inserted.National_Identifier,
			Item.ItemType_ID		
		FROM
			Inserted
			JOIN Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key
			JOIN Item (NOLOCK)
				ON Item.Item_Key = Inserted.Item_Key
		WHERE
			-- we care only if any of the columns we are tracking changes
			(
			Inserted.Identifier <> Deleted.Identifier
			OR Inserted.National_Identifier <> Deleted.National_Identifier
			)
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemIdentifier_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
