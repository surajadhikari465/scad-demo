
-- this is to remove the trigger that was created before the name change
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Store_AddUpd')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Store_AddUpd'
		DROP  Trigger JDASync_Store_AddUpd
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Store_AddDel')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Store_AddDel'
		DROP  Trigger JDASync_Store_AddDel
	END
GO


PRINT 'Creating Trigger JDASync_Store_AddDel'
GO
CREATE Trigger dbo.JDASync_Store_AddDel
ON Store
FOR INSERT, DELETE
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
	
		DECLARE @BodyText varchar(255)

		SELECT @BodyText = 'Added or Updated IRMA Store: ' + CAST(Inserted.Store_No AS varchar(38))
		FROM Inserted

		SET @BodyText = @BodyText + CHAR(10) + 'Depending on the change, you may need to update the "TBLSTR" table in JDA.'

		EXEC dbo.JDASync_Notify
			@EventKey = 'SYNC_MAP_TABLE_UPDATE',
			@AdditionalBodyText = @BodyText

		
		SELECT @Error_No = @@ERROR

		IF @Error_No <> 0
		BEGIN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('JDASync_Store_AddUpd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
    END
    
    -- reset it
	SET ANSI_NULLS ON

END

GO
